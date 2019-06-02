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
        private static void AddApiAuth(this IServiceCollection services)
        {
            services.Configure<AuthenticationOptions>(o =>
            {
                o.AddScheme(_Consts.UserManager.ApiAuthScheme, _scheme =>
                {
                    _scheme.HandlerType = typeof(_ApiAuthHandler);
                    _scheme.DisplayName = null;

                });
            });
            services.Configure<_ApiAuthOptions>(_Consts.UserManager.ApiAuthScheme, o =>
            {
            });
            services.AddSingleton<_ApiAuthHandler>();
            services.AddSingleton<_ApiAuthSignInHandler>();
            services.AddSingleton<_ApiAuthSignOutHandler>();
        }

        private class _ApiAuthOptions : AuthenticationSchemeOptions
        {
            private IConfiguration _config;

            [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.UserManager.ApiAuth)]
            public string RedisConfiguration => _config.GetValue<string>();

            public void Init(IServiceProvider service)
            {
                this._config = service.GetService<IConfiguration<_ApiAuthOptions>>();
            }
        }

        private class _ApiAuthHandler : IAuthenticationHandler
        {
            private IServiceProvider _service;
            private IOptionsMonitor<_ApiAuthOptions> _options;

            public _ApiAuthHandler(IServiceProvider service, IOptionsMonitor<_ApiAuthOptions> options)
            {
                this._service = service;
                this._options = options;
            }

            async Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync()
            {
                var httpContext = _service.GetService<IHttpContextAccessor>().HttpContext;
                if (httpContext != null &&
                    httpContext.Request.Headers.TryGetValue(_Consts.UserManager.ApiAuthScheme, out var _token))
                {
                    var token = (_token.ToString() ?? "").Replace("Bearer ", "");
                    using (var redis = await _service.GetRedisConnectionAsync(_options.CurrentValue.RedisConfiguration))
                    {
                        var user = await redis.GetObject<ClaimsPrincipal>($"ApiAuth:{token}");
                        if (user != null &&
                            user.GetUserId(out var userId) &&
                            user.GetSessionId(out var sessionId))
                        {
                            var ticket = new AuthenticationTicket(user, _Consts.UserManager.ApiAuthScheme);
                            return await Task.FromResult(AuthenticateResult.Success(ticket));
                        }
                    }
                }
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            #region

            Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties) => Task.CompletedTask;

            Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties) => Task.CompletedTask;

            Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) => Task.CompletedTask;

            #endregion
        }
        private class _ApiAuthSignInHandler : IAuthenticationSignInHandler
        {
            private IServiceProvider _service;
            private IOptionsMonitor<_ApiAuthOptions> _options;

            public _ApiAuthSignInHandler(IServiceProvider service, IOptionsMonitor<_ApiAuthOptions> options)
            {
                this._service = service;
                this._options = options;
            }

            public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
            {
                if (user.GetUserId(out UserId userId))
                {
                    string key = Guid.NewGuid().ToString("N");
                    user.SetSessionId(key);
                    using (var redis = await _service.GetRedisConnectionAsync(_options.CurrentValue.RedisConfiguration))
                    {
                        await redis.SetObject($"ApiAuth:{key}", user, expiry: TimeSpan.FromMinutes(30));
                    }
                }
                await Task.CompletedTask;
            }
         
            #region

            Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync() => Task.FromResult(AuthenticateResult.NoResult());

            Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties) => Task.CompletedTask;

            Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties) => Task.CompletedTask;

            Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) => Task.CompletedTask;

            Task IAuthenticationSignOutHandler.SignOutAsync(AuthenticationProperties properties) => Task.CompletedTask;

            #endregion
        }
        private class _ApiAuthSignOutHandler : IAuthenticationSignOutHandler
        {
            private IServiceProvider _service;
            private IOptionsMonitor<_ApiAuthOptions> _options;

            public _ApiAuthSignOutHandler(IServiceProvider service, IOptionsMonitor<_ApiAuthOptions> options)
            {
                this._service = service;
                this._options = options;
            }

            public async Task SignOutAsync(AuthenticationProperties properties)
            {
                var httpContext = _service.GetService<IHttpContextAccessor>().HttpContext;
                ClaimsPrincipal user = httpContext.User;
                if (user != null &&
                    user.GetUserId(out var userId) &&
                    user.GetSessionId(out var sessionId))
                {
                    using (var redis = await _service.GetRedisConnectionAsync(_options.CurrentValue.RedisConfiguration))
                    {
                        await redis.KeyDeleteAsync($"ApiAuth:{sessionId}");
                    }
                }
                await Task.CompletedTask;
            }

            #region

            Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync() => Task.FromResult(AuthenticateResult.NoResult());

            Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties) => Task.CompletedTask;

            Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties) => Task.CompletedTask;

            Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) => Task.CompletedTask;

            #endregion

        }
    }
}
