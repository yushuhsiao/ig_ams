using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace InnateGlory
{
    internal class RedisTicketStore : ITicketStore
    {
        private class _TicketData
        {
            public AuthenticationTicket Ticket { get; set; }
            public RedisValue json { get; set; }
            public byte[] bin { get; set; }
            public RedisValue base64 { get; set; }

            public static bool Serialize(AuthenticationTicket ticket, out _TicketData data)
            {
                try
                {
                    data = new _TicketData();
                    data.Ticket = ticket;
                    data.bin = JsonTicketSerializer.Default.Serialize(data.Ticket);
                    data.base64 = Convert.ToBase64String(data.bin);
                    data.json = JsonHelper.SerializeObject(data.Ticket, formatting: Formatting.Indented);
                    return true;
                }
                catch { }
                return _null.noop(false, out data);
            }

            public static bool Deserialize(RedisValue base64, out _TicketData data)
            {
                try
                {
                    data = new _TicketData();
                    data.base64 = base64;
                    data.bin = Convert.FromBase64String(base64);
                    data.Ticket = JsonTicketSerializer.Default.Deserialize(data.bin);
                    data.json = JsonHelper.SerializeObject(data.Ticket);
                    return true;
                }
                catch { }
                return _null.noop(false, out data);
            }
        }

        private readonly Dictionary<string, _TicketData> _tickets = new Dictionary<string, _TicketData>();
        private readonly UserManager _userManager;
        private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieOptionsMonitor;
        private readonly IConfiguration<RedisTicketStore> _config;
        //private readonly ISqlConfig _config2;
        private readonly ILogger _logger;

        public RedisTicketStore(IServiceProvider services, UserManager userManager, IOptionsMonitor<CookieAuthenticationOptions> cookieOptionsMonitor)
        {
            this._userManager = userManager;
            this._cookieOptionsMonitor = cookieOptionsMonitor;
            //this._cookieOptions = cookieOptions;
            this._config = services.GetService<IConfiguration<RedisTicketStore>>();
            //this._config2 = services.GetService<ISqlConfig<RedisTicketStore<TUser>>>();//.GetService(this);
            this._logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("RedisTicketStore");
            //this._userManager.UserDataExpireTime = CookieExpireTimeSpan;
        }

        //private string SchemeName => _userManager.SchemeName;
        public string SchemeName { get; set; }

        private CookieAuthenticationOptions _cookieOptions => _cookieOptionsMonitor.Get(this.SchemeName);

        //private TimeSpan CookieExpireTimeSpan() => _cookieOptions.ExpireTimeSpan;

        #region redis

        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.UserManager.Redis_Key2)]
        private string RedisConfiguration() => _config.GetValue<string>();

        private bool _redis_busy = false;
        private IDatabase _redis;

        private IDatabase GetRedis()
        {
            lock (this)
            {
                if (_redis == null)
                    _redis = ConnectionMultiplexer.Connect(RedisConfiguration())?.GetDatabase();
                //_redis = new RedisDatabaseWithLock(ConnectionMultiplexer.Connect(RedisConfiguration())?.GetDatabase());
                return _redis;
            }
        }

        private async Task<IDatabase> GetRedisAsync()
        {
            IDatabase _redis;
            for (; ; await Task.Delay(1))
            {
                lock (this)
                {
                    if (_redis_busy == false)
                    {
                        _redis = this._redis;
                        _redis_busy = true;
                        break;
                    }
                }
            }
            if (_redis == null)
            {
                var config = RedisConfiguration();
                _redis = (await ConnectionMultiplexer.ConnectAsync(config)).GetDatabase();
                lock (this) this._redis = _redis;
            }
            return await Task.FromResult(_redis);
        }

        private void ReleaseRedis()
        {
            lock (this) _redis_busy = false;
        }

        private void OnRedisError(Exception ex)
        {
            _logger.Log(LogLevel.Error, 0, null, ex); //_logger.LogError(ex, null);
            lock (this)
            {
                using (_redis?.Multiplexer)
                    _redis = null;
                ReleaseRedis();
            }
        }

        #endregion

        private RedisKey make_keyA(string key, AuthenticationTicket ticket) => $"{ticket?.AuthenticationScheme ?? this.SchemeName}:A_{key}";

        private RedisKey make_keyB(string key, AuthenticationTicket ticket) => $"{ticket?.AuthenticationScheme ?? this.SchemeName}:B_{key}";



        async Task ITicketStore.RemoveAsync(string key) => await this._RemoveAsync(key);

        async Task ITicketStore.RenewAsync(string key, AuthenticationTicket ticket) => await this._StoreAsync(key, ticket);

        async Task<AuthenticationTicket> ITicketStore.RetrieveAsync(string key) => await this._RetrieveAsync(key);

        async Task<string> ITicketStore.StoreAsync(AuthenticationTicket ticket) => await this._StoreAsync(null, ticket);



        private async Task _RemoveAsync(string key)
        {
            try
            {
                this._tickets.TryRemove(key, syncLock: true);
                var keyA = make_keyA(key, null);
                var keyB = make_keyB(key, null);
                var redis = await this.GetRedisAsync();
                await redis.KeyDeleteAsync(keyA);
                await redis.KeyDeleteAsync(keyB);
                ReleaseRedis();
            }
            catch (Exception ex)
            {
                this.OnRedisError(ex);
            }
        }

        private async Task<AuthenticationTicket> _RetrieveAsync(string key)
        {
            try
            {
                var keyA = make_keyA(key, null);
                var redis = await this.GetRedisAsync();
                RedisValue base64 = await redis.StringGetAsync(keyA);
                ReleaseRedis();

                if (base64.HasValue)
                {
                    _TicketData ticket2 = this._tickets.GetValue(key, syncLock: true);
                    if (ticket2 != null && base64 == ticket2.base64)
                        return ticket2.Ticket;

                    if (_TicketData.Deserialize(base64, out _TicketData ticket1))
                    {
                        if (ticket1.Ticket.Principal.GetUserId(out UserId userId))
                        {
                            this._tickets.SetValue(key, ticket1);
                            return ticket1.Ticket;
                        }
                        else _logger.Log(LogLevel.Information, 0, "Invalid ticket data, UserId not found.");
                    }
                    else _logger.Log(LogLevel.Information, 0, "Deserialize ticket failed.");
                    await _RemoveAsync(key);
                }
                else _logger.Log(LogLevel.Information, 0, $@"Ticket ""{key}"" not found."); // timeout or deleted (kicked)
            }
            catch (Exception ex)
            {
                this.OnRedisError(ex);
            }
            return null;
        }

        private async Task<string> _StoreAsync(string original_key, AuthenticationTicket ticket)
        {
            //if (ticket.AuthenticationScheme == this.SchemeName)
            {
                if (ticket.Principal.GetUserId(out UserId userId))
                {
                    string key = original_key ?? Guid.NewGuid().ToString("N");
                    ticket.Principal.SetSessionId(key);
                    //ticket.Properties.Parameters[_Consts.UserManager.Ticket_SessionId] = key;
                    if (_TicketData.Serialize(ticket, out var data))
                    {
                        try
                        {
                            RedisKey keyA = make_keyA(key, ticket);
                            RedisKey keyB = make_keyB(key, ticket);
                            var redis = await this.GetRedisAsync();
                            await redis.StringSetAsync(key: keyA, value: data.base64, expiry: _cookieOptions.ExpireTimeSpan);
                            await redis.StringSetAsync(key: keyB, value: data.json, expiry: _cookieOptions.ExpireTimeSpan);
                            ReleaseRedis();
                            this._tickets.SetValue(key, data, syncLock: true);
                            //_userManager.GetUserStoreItem(userId, create: true)?.Timer.Reset();
                            return key;
                        }
                        catch (Exception ex)
                        {
                            this.OnRedisError(ex);
                        }
                    }
                    else _logger.Log(LogLevel.Information, 0, "ticket serialize");
                }
                else _logger.Log(LogLevel.Information, 0, "Invalid ticket data, UserId not found.");
            }
            return original_key;
        }
    }
}