using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace InnateGlory
{
    partial class AuthenticationExtensions
    {
        private class _ApiAuthOptions : AuthenticationSchemeOptions
        {
        }

        private class _ApiAuthHandler : IAuthenticationSignInHandler
        {
            private IServiceProvider _service;
            //private IOptionsMonitor<_ApiAuthOptions> _options;
            //private IConfiguration _config;

            //[AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.UserManager.ApiAuth)]
            //public string RedisConfiguration => _config.GetValue<string>();

            public _ApiAuthHandler(IServiceProvider service/*, IOptionsMonitor<_ApiAuthOptions> options*/)
            {
                this._service = service;
                //this._options = options;
                //this._options.CurrentValue.Init(service);
                //this._config = service.GetService<IConfiguration<_ApiAuthHandler>>();
            }

            public async Task<AuthenticateResult> AuthenticateAsync()
            {
                var httpContext = _service.GetService<IHttpContextAccessor>().HttpContext;
                if (httpContext != null &&
                    httpContext.Request.Headers.TryGetValue(_Consts.UserManager.Authorization, out var _token))
                {
                    var token = (_token.ToString() ?? "").Replace("Bearer ", "");
                    ITicketStore ticketStore = _service.GetService<RedisTicketStore>();
                    var ticket = await ticketStore.RetrieveAsync(token);
                    if (ticket != null)
                        return await Task.FromResult(AuthenticateResult.Success(ticket));
                    //using (var redis = await _service.GetRedisConnectionAsync(RedisConfiguration))
                    //{
                    //    var user = await redis.GetObject<ClaimsPrincipal>($"{_Consts.UserManager.AccessToken}:{token}");
                    //    if (user != null &&
                    //        user.GetUserId(out var userId) &&
                    //        user.GetSessionId(out var sessionId))
                    //    {
                    //        var ticket = new AuthenticationTicket(user, _Consts.UserManager.ApiAuthScheme);
                    //        return await Task.FromResult(AuthenticateResult.Success(ticket));
                    //    }
                    //}
                }
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties)
            {
                return Task.CompletedTask;
            }

            Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties)
            {
                return Task.CompletedTask;
            }

            Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context)
            {
                return Task.CompletedTask;
            }

            public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
            {
                //if (user.GetUserId(out UserId userId))
                //{
                //    if (user.GetSessionId(out var sessionId))
                //    {
                //    }
                //    else
                //    {
                //        sessionId = Guid.NewGuid().ToString("N");
                //        user.SetSessionId(sessionId);
                //    }
                //    using (var redis = await _service.GetRedisConnectionAsync(RedisConfiguration))
                //    {
                //        await redis.SetObject($"ApiAuth:{sessionId}", user, expiry: TimeSpan.FromMinutes(30));
                //    }
                //}
                await Task.CompletedTask;
            }

            public async Task SignOutAsync(AuthenticationProperties properties)
            {
                //var httpContext = _service.GetService<IHttpContextAccessor>().HttpContext;
                //ClaimsPrincipal user = httpContext.User;
                //if (user != null &&
                //    user.GetUserId(out var userId) &&
                //    user.GetSessionId(out var sessionId))
                //{
                //    using (var redis = await _service.GetRedisConnectionAsync(RedisConfiguration))
                //    {
                //        await redis.KeyDeleteAsync($"ApiAuth:{sessionId}");
                //    }
                //}
                await Task.CompletedTask;
            }

        }


        //private class _ApiAuthHandler : IAuthenticationHandler
        //{
        //    private IServiceProvider _service;
        //    private IOptionsMonitor<_ApiAuthOptions> _options;

        //    public _ApiAuthHandler(IServiceProvider service, IOptionsMonitor<_ApiAuthOptions> options)
        //    {
        //        this._service = service;
        //        this._options = options;
        //        this._options.CurrentValue.Init(service);
        //    }

        //    public async Task<AuthenticationTicket> GetTicket(string token)
        //    {
        //        using (var redis = await _service.GetRedisConnectionAsync(_options.CurrentValue.RedisConfiguration))
        //        {
        //            var user = await redis.GetObject<ClaimsPrincipal>($"ApiAuth:{token}");
        //            if (user != null &&
        //                user.GetUserId(out var userId) &&
        //                user.GetSessionId(out var sessionId))
        //            {
        //                var ticket = new AuthenticationTicket(user, _Consts.UserManager.ApiAuthScheme);
        //                return ticket;
        //            }
        //        }
        //        return await Task.FromResult<AuthenticationTicket>(null);
        //    }

        //    async Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync()
        //    {
        //        var httpContext = _service.GetService<IHttpContextAccessor>().HttpContext;
        //        if (httpContext != null &&
        //            httpContext.Request.Headers.TryGetValue(_Consts.UserManager.ApiAuthScheme, out var _token))
        //        {
        //            var token = (_token.ToString() ?? "").Replace("Bearer ", "");
        //            var ticket = await this.GetTicket(token);
        //            if (ticket != null)
        //                return await Task.FromResult(AuthenticateResult.Success(ticket));
        //        }
        //        return await Task.FromResult(AuthenticateResult.NoResult());
        //    }

        //    #region

        //    Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) => Task.CompletedTask;

        //    #endregion
        //}
        //private class _ApiAuthSignInHandler : IAuthenticationSignInHandler
        //{
        //    private IServiceProvider _service;
        //    private IOptionsMonitor<_ApiAuthOptions> _options;

        //    public _ApiAuthSignInHandler(IServiceProvider service, IOptionsMonitor<_ApiAuthOptions> options)
        //    {
        //        this._service = service;
        //        this._options = options;
        //        this._options.CurrentValue.Init(service);
        //    }

        //    public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        //    {
        //        if (user.GetUserId(out UserId userId))
        //        {
        //            string key = Guid.NewGuid().ToString("N");
        //            user.SetSessionId(key);
        //            using (var redis = await _service.GetRedisConnectionAsync(_options.CurrentValue.RedisConfiguration))
        //            {
        //                await redis.SetObject($"ApiAuth:{key}", user, expiry: TimeSpan.FromMinutes(30));
        //            }
        //        }
        //        await Task.CompletedTask;
        //    }
         
        //    #region

        //    Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync() => Task.FromResult(AuthenticateResult.NoResult());

        //    Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) => Task.CompletedTask;

        //    Task IAuthenticationSignOutHandler.SignOutAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    #endregion
        //}
        //private class _ApiAuthSignOutHandler : IAuthenticationSignOutHandler
        //{
        //    private IServiceProvider _service;
        //    private IOptionsMonitor<_ApiAuthOptions> _options;

        //    public _ApiAuthSignOutHandler(IServiceProvider service, IOptionsMonitor<_ApiAuthOptions> options)
        //    {
        //        this._service = service;
        //        this._options = options;
        //        this._options.CurrentValue.Init(service);
        //    }

        //    public async Task SignOutAsync(AuthenticationProperties properties)
        //    {
        //        var httpContext = _service.GetService<IHttpContextAccessor>().HttpContext;
        //        ClaimsPrincipal user = httpContext.User;
        //        if (user != null &&
        //            user.GetUserId(out var userId) &&
        //            user.GetSessionId(out var sessionId))
        //        {
        //            using (var redis = await _service.GetRedisConnectionAsync(_options.CurrentValue.RedisConfiguration))
        //            {
        //                await redis.KeyDeleteAsync($"ApiAuth:{sessionId}");
        //            }
        //        }
        //        await Task.CompletedTask;
        //    }

        //    #region

        //    Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync() => Task.FromResult(AuthenticateResult.NoResult());

        //    Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties) => Task.CompletedTask;

        //    Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) => Task.CompletedTask;

        //    #endregion

        //}
    }
}
