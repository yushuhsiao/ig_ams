using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace InnateGlory.Authentication
{
    internal class ApiAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        private class _cacheItem
        {
            public UserId Id;
            public UserName corp;
            public UserName name;
            public DateTime time;
        }
        private List<_cacheItem> _cache = new List<_cacheItem>();

        public bool GetUserId(IServiceProvider _services, UserName corp, UserName name, out UserId id)
        {
            if (corp.IsValid && name.IsValid)
            {
                lock (_cache)
                {
                    for (int i = _cache.Count - 1; i >= 0; i--)
                    {
                        if (_cache[i].corp == corp && _cache[i].name == name)
                        {
                            id = _cache[i].Id;
                            _cache[i].time = DateTime.Now;
                            return true;
                        }
                        TimeSpan t = DateTime.Now - _cache[i].time;
                        if (t.TotalMinutes > 30)
                            _cache.RemoveAt(i);
                    }
                }

                var dataService = _services.GetService<DataService>();
                if (dataService.Users.GetUser(corp, name, out var user))
                {
                    lock (_cache)
                    {
                        _cache.Add(new _cacheItem() { Id = id = user.Id, corp = corp, name = name });
                        while (_cache.Count > 1000)
                            _cache.RemoveAt(0);
                        return true;
                    }
                }
            }
            return _null.noop(false, out id);

        }
    }
    internal class ApiAuthenticationHandler<TUser> : AuthenticationHandler<ApiAuthenticationSchemeOptions> where TUser : class, IUser
    {
        private UserManager<TUser> _userManager;
        private IServiceProvider _services;
        public ApiAuthenticationHandler(IServiceProvider services, UserManager<TUser> userManager, IOptionsMonitor<ApiAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _services = services;
            _userManager = userManager;
        }

        private Task<AuthenticateResult> _Success(UserId userId)
        {
            Logger.LogInformation($"ApiAuth : UserId={userId}");
            _userManager.GetUserStoreItem(userId, true);
            ClaimsPrincipal claims = new ClaimsPrincipal();
            claims.SetUserId(userId);
            var ticket = new AuthenticationTicket(claims, this.Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private Task<AuthenticateResult> _Fail(string message) => Task.FromResult(AuthenticateResult.Fail(message));

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var h1 = Context.Request.Headers[_Consts.UserManager.AUTH_USER].ToString();
            var h2 = Context.Request.Headers[_Consts.UserManager.AUTH_INTERNAL].ToString();
            bool is_internal;
            if (h2.ToBoolean(out is_internal) && is_internal)
            {
                if (false == _userManager.InternalApiServer)
                    return _Fail("Authentication.InternalApiServer = false");
            }

            if (h1.ToInt64(out long _userId))
            {
                return _Success(_userId);
            }
            else if (!string.IsNullOrEmpty(h1))
            {
                string auth_corp, auth_user;
                int n = h1.LastIndexOf('@');
                if (n >= 0)
                {
                    auth_corp = h1.Substring(n + 1);
                    auth_user = h1.Substring(0, n);
                }
                else
                {
                    auth_corp = null;
                    auth_user = h1;
                }
                if (Options.GetUserId(_services, auth_corp, auth_user, out UserId userId))
                    return _Success(userId);
            }

            return _Fail("Authentication failed");
        }
    }
}
