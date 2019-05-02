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

namespace InnateGlory
{
    internal class RedisTicketStore<TUser> : ITicketStore where TUser : class, IUser
    {
        private struct _TicketData
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
                    data.bin = JsonTicketSerializer.TicketSerializer.Serialize(data.Ticket);
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
                    data.Ticket = JsonTicketSerializer.TicketSerializer.Deserialize(data.bin);
                    data.json = JsonHelper.SerializeObject(data.Ticket);
                    return true;
                }
                catch { }
                return _null.noop(false, out data);
            }
        }

        private readonly Dictionary<string, _TicketData> _tickets = new Dictionary<string, _TicketData>();
        private readonly UserManager<TUser> _userManager;
        private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieOptionsMonitor;
        private readonly IConfiguration<RedisTicketStore<TUser>> _config;
        //private readonly ISqlConfig _config2;
        private readonly ILogger _logger;

        public RedisTicketStore(IServiceProvider services, UserManager<TUser> userManager, IOptionsMonitor<CookieAuthenticationOptions> cookieOptionsMonitor)
        {
            this._userManager = userManager;
            this._cookieOptionsMonitor = cookieOptionsMonitor;
            //this._cookieOptions = cookieOptions;
            this._config = services.GetService<IConfiguration<RedisTicketStore<TUser>>>();
            //this._config2 = services.GetService<ISqlConfig<RedisTicketStore<TUser>>>();//.GetService(this);
            this._logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("RedisTicketStore");
            //this._userManager.UserDataExpireTime = CookieExpireTimeSpan;
        }

        //private string SchemeName => _userManager.SchemeName;
        public string SchemeName { get; set; }

        private CookieAuthenticationOptions _cookieOptions => _cookieOptionsMonitor.Get(this.SchemeName);

        //private TimeSpan CookieExpireTimeSpan() => _cookieOptions.ExpireTimeSpan;

        #region redis

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

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.UserManager.Redis_Key2)]
        //private string RedisConfiguration() => _config.GetValue<string>();

        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.UserManager.Redis_Key2)]
        private string RedisConfiguration() => _config.GetValue<string>();

        private void OnRedisError(Exception ex)
        {
            _logger.Log(LogLevel.Error, 0, null, ex); //_logger.LogError(ex, null);
            lock (this)
                using (_redis?.Multiplexer)
                    _redis = null;
        }

        #endregion

        Task ITicketStore.RemoveAsync(string key) { this._Remove(key); return Task.CompletedTask; }

        Task ITicketStore.RenewAsync(string key, AuthenticationTicket ticket) { this._Store(ticket, key); return Task.CompletedTask; }

        Task<AuthenticationTicket> ITicketStore.RetrieveAsync(string key) => Task.FromResult(this._Retrieve(key));

        Task<string> ITicketStore.StoreAsync(AuthenticationTicket ticket) => Task.FromResult(this._Store(ticket, null));



        private RedisKey make_keyA(string key, AuthenticationTicket ticket) => $"{ticket?.AuthenticationScheme ?? this.SchemeName}:A_{key}";

        private RedisKey make_keyB(string key, AuthenticationTicket ticket) => $"{ticket?.AuthenticationScheme ?? this.SchemeName}:B_{key}";



        private void _Remove(string key)
        {
            try
            {
                this._tickets.TryRemove(key, syncLock: true);
                var redis = this.GetRedis();
                lock (redis)
                {
                    redis.KeyDelete(make_keyA(key, null));
                    redis.KeyDelete(make_keyB(key, null));
                }
            }
            catch (Exception ex)
            {
                this.OnRedisError(ex);
            }
        }

        private AuthenticationTicket _Retrieve(string key)
        {
            _TicketData ticket2 = this._tickets.GetValue(key, syncLock: true);
            RedisValue base64;
            try
            {
                var redis = this.GetRedis();
                lock (redis)
                    base64 = redis.StringGet(make_keyA(key, null));
            }
            catch (Exception ex)
            {
                this.OnRedisError(ex);
                base64 = default(RedisValue);
            }

            try
            {
                if (base64.HasValue)
                {
                    if (base64 == ticket2.base64)
                    {
                        _userManager.GetUserStoreItem(ticket2.Ticket.Principal, create: true).Timer.Reset();
                        return ticket2.Ticket;
                    }

                    if (_TicketData.Deserialize(base64, out _TicketData ticket1))
                    {
                        if (_userManager.GetUserStoreItem(ticket1.Ticket.Principal, out var user, create: true))
                        {
                            user.Timer.Reset();
                            this._tickets.SetValue(key, ticket1);
                            return ticket1.Ticket;
                        }
                        else _logger.Log(LogLevel.Information, 0, "Invalid ticket data, UserId not found.");
                    }
                    else _logger.Log(LogLevel.Information, 0, "Deserialize ticket failed.");
                }
                else _logger.Log(LogLevel.Information, 0, $@"Ticket ""{key}"" not found."); // timeout or deleted (kicked)
            }
            catch { }
            _Remove(key);
            return null;
        }

        private string _Store(AuthenticationTicket ticket, string original_key)
        {
            //if (ticket.AuthenticationScheme == this.SchemeName)
            {
                if (ticket.Principal.GetUserId(out UserId userId))
                {
                    string key = original_key ?? Guid.NewGuid().ToString("N");
                    ticket.Properties.Parameters[_Consts.UserManager.Ticket_SessionId] = key;
                    if (_TicketData.Serialize(ticket, out var data))
                    {
                        try
                        {
                            RedisKey keyA = make_keyA(key, ticket);
                            RedisKey keyB = make_keyB(key, ticket);
                            var redis = this.GetRedis();
                            lock (redis)
                            {
                                redis.StringSet(key: keyA, value: data.base64, expiry: _cookieOptions.ExpireTimeSpan);
                                redis.StringSet(key: keyB, value: data.json, expiry: _cookieOptions.ExpireTimeSpan);
                            }
                            this._tickets.SetValue(key, data, syncLock: true);
                            _userManager.GetUserStoreItem(userId, create: true)?.Timer.Reset();
                            return key;
                        }
                        catch (Exception ex)
                        {
                            this.OnRedisError(ex);
                        }
                    }
                    else
                        _logger.Log(LogLevel.Information, 0, "ticket serialize");
                }
                else
                    _logger.Log(LogLevel.Information, 0, "Invalid ticket data, UserId not found.");
            }
            return original_key;
        }
    }
}