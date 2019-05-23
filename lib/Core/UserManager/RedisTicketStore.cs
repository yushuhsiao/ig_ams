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
        //private readonly UserManager _userManager;
        private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieOptionsMonitor;
        private readonly IConfiguration<RedisTicketStore> _config;
        private readonly ILogger _logger;

        public RedisTicketStore(IServiceProvider services)
        {
            //this._userManager = userManager;
            this._cookieOptionsMonitor = services.GetService<IOptionsMonitor<CookieAuthenticationOptions>>();
            this._config = services.GetService<IConfiguration<RedisTicketStore>>();
            this._logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<RedisTicketStore>();
            this._redis = new RedisDatabase();
        }

        public string SchemeName { get; set; }

        private CookieAuthenticationOptions _cookieOptions => _cookieOptionsMonitor.Get(this.SchemeName);



        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.UserManager.Redis_Key2)]
        private string RedisConfiguration() => _config.GetValue<string>();

        private RedisDatabase _redis;

        private RedisKey make_keyA(string key, AuthenticationTicket ticket) => $"{ticket?.AuthenticationScheme ?? this.SchemeName}:A_{key}";

        private RedisKey make_keyB(string key, AuthenticationTicket ticket) => $"{ticket?.AuthenticationScheme ?? this.SchemeName}:B_{key}";



        async Task ITicketStore.RemoveAsync(string key) => await this._RemoveAsync(key);

        async Task ITicketStore.RenewAsync(string key, AuthenticationTicket ticket) => await this._StoreAsync(key, ticket);

        async Task<AuthenticationTicket> ITicketStore.RetrieveAsync(string key) => await this._RetrieveAsync(key);

        async Task<string> ITicketStore.StoreAsync(AuthenticationTicket ticket) => await this._StoreAsync(null, ticket);

        const int redis_retry = 3;


        private async Task _RemoveAsync(string key)
        {
            this._tickets.TryRemove(key, syncLock: true);
            var keyA = make_keyA(key, null);
            var keyB = make_keyB(key, null);
            for (int i = 0; i < redis_retry; i++)
            {
                try
                {
                    var redis = await _redis.GetDatabaseAsync(null, RedisConfiguration);
                    await redis.KeyDeleteAsync(keyA);
                    await redis.KeyDeleteAsync(keyB);
                    _redis.SetIdle();
                    break;
                }
                catch (Exception ex)
                {
                    _redis.OnError(_logger, ex, ex.Message);
                }
            }
        }

        private async Task<AuthenticationTicket> _RetrieveAsync(string key)
        {
            var keyA = make_keyA(key, null);
            RedisValue base64 = default(RedisValue);

            for (int i = 0; i < redis_retry; i++)
            {
                try
                {
                    var redis = await _redis.GetDatabaseAsync(null, RedisConfiguration);
                    base64 = await redis.StringGetAsync(keyA);
                    _redis.SetIdle();
                    break;
                }
                catch (Exception ex)
                {
                    _redis.OnError(_logger, ex, ex.Message);
                }
            }

            try
            {
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
                        else _logger.LogInformation("Invalid ticket data, UserId not found.");
                    }
                    else _logger.LogInformation("Deserialize ticket failed.");
                    await _RemoveAsync(key);
                }
                else _logger.LogInformation($@"Ticket ""{key}"" not found."); // timeout or deleted (kicked)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                        RedisKey keyA = make_keyA(key, ticket);
                        RedisKey keyB = make_keyB(key, ticket);
                        for (int i = 0; i < redis_retry; i++)
                        {
                            try
                            {
                                var db = await _redis.GetDatabaseAsync(null, RedisConfiguration);
                                await db.StringSetAsync(key: keyA, value: data.base64, expiry: _cookieOptions.ExpireTimeSpan);
                                await db.StringSetAsync(key: keyB, value: data.json, expiry: _cookieOptions.ExpireTimeSpan);
                                _redis.SetIdle();
                                break;
                            }
                            catch (Exception ex)
                            {
                                _redis.OnError(_logger, ex, ex.Message);
                            }
                        }
                        this._tickets.SetValue(key, data, syncLock: true);
                        //_userManager.GetUserStoreItem(userId, create: true)?.Timer.Reset();
                        return key;
                    }
                    else _logger.LogInformation("ticket serialize");
                }
                else _logger.LogInformation("Invalid ticket data, UserId not found.");
            }
            return original_key;
        }
    }
}