using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InnateGlory
{
    public static partial class AuthenticationExtensions
    {
        public static IServiceCollection AddCookieAuth(this IServiceCollection services, string scheme = _Consts.UserManager.ApplicationScheme)
        {
            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = scheme;
                options.DefaultAuthenticateScheme = scheme;// CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = scheme;// CookieAuthenticationDefaults.AuthenticationScheme;
                //options.AddScheme<Test2>("Test2", "Test2");
            });
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, _ConfigureCookie>());
            builder.AddCookie(scheme);
            return services;
        }

        public static IServiceCollection AddApiAuth(this IServiceCollection services, string scheme = _Consts.UserManager.ApplicationScheme)
        {
            //services.Replace(ServiceDescriptor.Scoped<IAuthenticationService, _AuthenticationService>());
            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = scheme;
                options.DefaultAuthenticateScheme = scheme;
                options.DefaultChallengeScheme = scheme;
            });
            services.AddSingleton(s => new RedisTicketStore(s, scheme));
            services.Configure<AuthenticationOptions>(o =>
            {
                o.AddScheme(scheme, _scheme =>
                {
                    _scheme.HandlerType = typeof(_ApiAuthHandler);
                    _scheme.DisplayName = _scheme.Name;
                });
            });
            services.Configure<_ApiAuthOptions>(scheme, o =>
            {
            });
            services.TryAddSingleton<_ApiAuthHandler>();
            return services;
        }

        private static string GetScheme(this HttpContext context, string scheme) => scheme ?? context.RequestServices.GetService<IOptions<AuthenticationOptions>>().Value.DefaultScheme;

        public static async Task SignInByTokenAsync(this HttpContext context, string token, string scheme = null)
        {
            var _scheme = context.GetScheme(scheme);
            var opts = context.RequestServices
                .GetService<IOptionsMonitor<CookieAuthenticationOptions>>()
                .Get(_scheme);
            var ticket = await opts.SessionStore.RetrieveAsync(token);
            if (ticket != null)
            {
                await context.SignInAsync(_scheme, ticket.Principal, ticket.Properties);
                context.User = ticket.Principal;
            }
            await Task.CompletedTask;
        }

        public static async Task<string> SignInAsync(this HttpContext context, UserId userId, string scheme = _Consts.UserManager.ApplicationScheme)
        {
            //string sessionId = Guid.NewGuid().ToString("N");
            ClaimsPrincipal user = new ClaimsPrincipal();
            user.SetUserId(userId);
            //user.SetSessionId(sessionId);
            AuthenticationTicket ticket = new AuthenticationTicket(user, scheme);
            ITicketStore ticketStore = context.RequestServices.GetService<RedisTicketStore>();
            string sessionId = await ticketStore.StoreAsync(ticket);
            return sessionId;

            //_ApiAuthHandler handler = context.RequestServices.GetService<_ApiAuthHandler>();
            //using (var redis = await context.RequestServices.GetRedisConnectionAsync(handler.RedisConfiguration))
            //    await redis.SetObject($"{_Consts.UserManager.AccessToken}:{sessionId}", user, expiry: TimeSpan.FromMinutes(30));
            //return await Task.FromResult(sessionId);
        }

        public static async Task SignOutAsync(this HttpContext context, UserId userId, string scheme = null)
        {
            await context
                .SignOutAsync(context.GetScheme(scheme), null);

            //await context.RequestServices.GetService<_ApiAuthHandler>()
            //    .SignOutAsync(null);
        }

        private class _ConfigureCookie : IPostConfigureOptions<CookieAuthenticationOptions>
        {
            private IServiceProvider _service;
            private IHttpContextAccessor _httpContextAccessor;
            private IConfiguration _config;

            [AppSetting(SectionName = _Consts.UserManager.ConfigSection)]
            public string CookieName => _config.GetValue<string>();

            [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue("00:30:00")]
            public TimeSpan Expire => _config.GetValue<TimeSpan>();

            [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(true)]
            public bool SlidingExpiration => _config.GetValue<bool>();

            public _ConfigureCookie(IServiceProvider service)
            {
                _service = service;
                _httpContextAccessor = service.GetService<IHttpContextAccessor>();
                _config = service.GetService<IConfiguration<_ConfigureCookie>>();
                ClaimsPrincipal.ClaimsPrincipalSelector = ClaimsPrincipalSelector;
            }

            private ClaimsPrincipal ClaimsPrincipalSelector() => _httpContextAccessor?.HttpContext?.User;

            void IPostConfigureOptions<CookieAuthenticationOptions>.PostConfigure(string name, CookieAuthenticationOptions options)
            {
                options.SessionStore = new RedisTicketStore(_service, name);
                //?? serviceProvider.GetService<UserManager<TUser>>().TicketStore;

                //var authenticationOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
                //options.DataProtectionProvider = options.DataProtectionProvider ?? serviceProvider.GetRequiredService<IDataProtectionProvider>();
                //var dataProtector = options.DataProtectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", authenticationOptions.DefaultScheme, "v2");
                //options.TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(TicketSerializer2.Default, dataProtector);

                if (this.CookieName != null)
                {
                    options.Cookie.Name = this.CookieName;
                }
                options.ExpireTimeSpan = this.Expire;
                options.SlidingExpiration = this.SlidingExpiration;
            }
        }



        public static void SetUserId(this ClaimsPrincipal principal, UserId userId)
        {
            principal.SetValue("_UserId", userId.ToString());
        }
        public static bool GetUserId(this ClaimsPrincipal principal, out UserId userId)
        {
            if (principal.GetValue("_UserId", out var value))
                return UserId.TryParse(value, out userId);
            return _null.noop(false, out userId);
        }
        public static UserId GetUserId(this ClaimsPrincipal principal)
        {
            if (principal.GetUserId(out UserId userId))
                return userId;
            return UserId.Guest;
        }

        public static bool GetSessionId(this ClaimsPrincipal principal, out string sessionId)
            => principal.GetValue("_SessionId", out sessionId);
        public static void SetSessionId(this ClaimsPrincipal principal, string sessionId)
            => principal.SetValue("_SessionId", sessionId);



        private static bool GetValue(this ClaimsPrincipal principal, string type, out string value)
        {
            if (principal.GetIdentity(out var identity, create: false))
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type == type)
                    {
                        value = claim.Value;
                        return true;
                    }
                }
            }
            return _null.noop(false, out value);
        }
        private static void SetValue(this ClaimsPrincipal principal, string type, string value)
        {
            if (principal.GetIdentity(out var identity, create: true))
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type == type)
                    {
                        if (claim.Value == value)
                            return;
                        identity.TryRemoveClaim(claim);
                        break;
                    }
                }
                identity.AddClaim(new Claim(type, value));
            }
        }

        private static bool GetIdentity(this ClaimsPrincipal principal, out ClaimsIdentity result, bool create = false)
            => GetIdentity(principal, _Consts.UserManager.AuthenticationType, out result, create);
        private static bool GetIdentity(this ClaimsPrincipal principal, string authenticationType, out ClaimsIdentity result, bool create = false)
        {
            if (principal != null)
            {
                foreach (var identity in principal.Identities)
                {
                    if (identity.AuthenticationType == authenticationType)
                    {
                        result = identity;
                        return true;
                    }
                }
                if (create)
                {
                    principal.AddIdentity(result = new ClaimsIdentity(authenticationType));
                    return true;
                }
            }
            return _null.noop(false, out result);
        }
    }
}
//public static IServiceCollection AddCookieAuth(this IServiceCollection services, string scheme = _Consts.UserManager.ApplicationScheme)
//{
//    /// <see cref="AuthenticationScheme"/>
//    /// <see cref="IAuthenticationHandler"/>
//    /// <see cref="IAuthenticationRequestHandler"/>
//    /// <see cref="IAuthenticationSignInHandler"/>
//    /// <see cref="IAuthenticationSignOutHandler"/>

//    //services.Replace(ServiceDescriptor.Scoped<IAuthenticationService, _AuthenticationService>());
//    //services.Replace(ServiceDescriptor.Singleton<IClaimsTransformation, _ClaimsTransformation>()); // Can be replaced with scoped ones that use DbContext
//    //services.Replace(ServiceDescriptor.Scoped<IAuthenticationHandlerProvider, _AuthenticationHandlerProvider>());
//    //services.Replace(ServiceDescriptor.Singleton<IAuthenticationSchemeProvider, _AuthenticationSchemeProvider>());
//    /// <see cref="AuthenticationCoreServiceCollectionExtensions.AddAuthenticationCore(IServiceCollection)"/>
//    //services.TryAddScoped<IAuthenticationService, AuthenticationService>();
//    //services.TryAddSingleton<IClaimsTransformation, NoopClaimsTransformation>(); // Can be replaced with scoped ones that use DbContext
//    //services.TryAddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
//    //services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();

//    var builder = services.AddAuthentication(options =>
//    {
//        options.DefaultScheme = scheme;
//        options.DefaultAuthenticateScheme = scheme;// CookieAuthenticationDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = scheme;// CookieAuthenticationDefaults.AuthenticationScheme;
//        //options.AddScheme<Test2>("Test2", "Test2");
//    });


//    //builder
//    //    .AddScheme<ApiAuthenticationSchemeOptions, ApiAuthenticationHandler>(_Consts.UserManager.ApiAuthScheme, _Consts.UserManager.ApiAuthScheme, options => { })
//    //    .AddCookie(_Consts.UserManager.AccessTokenScheme, options => { })
//    //;

//    //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<Test1Options>, Test1PostConfig>());
//    //builder.AddScheme<Test1Options, Test1Handler>("Test1", "Test1", testConfigure);

//    //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.AccessTokenScheme, ConfigureCookie);
//    //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.AccessTokenScheme, Authentication.AccessTokenAuthenticationHandler.Configure);
//    //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.UserTokenScheme, (name, s, options) =>
//    //{
//    //    ConfigureCookie<TUser>(name, s, options);
//    //    UserTokenAuthenticationHandler.Configure(name, s, options);
//    //});
//    //builder.AddCookie(scheme, options => { });

//    services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, _ConfigureCookie>());
//    builder.AddCookie(scheme);

//    //builder.AddScheme<CookieAuthenticationOptions, CookieAuthenticationHandler>(scheme, null, opts =>
//    //{
//    //});

//    //builder.Services.AddApiAuth();

//    //builder.AddScheme<_ApiAuthOptions, _ApiAuthHandler>(_Consts.UserManager.ApiAuthScheme, o => { });

//    return services;
//}