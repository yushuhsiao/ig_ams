using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace InnateGlory
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationExtensions(this IServiceCollection services, string scheme = _Consts.UserManager.ApplicationScheme)
        {
            /// <see cref="AuthenticationScheme"/>
            /// <see cref="IAuthenticationHandler"/>
            /// <see cref="IAuthenticationRequestHandler"/>
            /// <see cref="IAuthenticationSignInHandler"/>
            /// <see cref="IAuthenticationSignOutHandler"/>

            services.Replace(ServiceDescriptor.Scoped<IAuthenticationService, _AuthenticationService>());
            //services.Replace(ServiceDescriptor.Singleton<IClaimsTransformation, _ClaimsTransformation>()); // Can be replaced with scoped ones that use DbContext
            //services.Replace(ServiceDescriptor.Scoped<IAuthenticationHandlerProvider, _AuthenticationHandlerProvider>());
            //services.Replace(ServiceDescriptor.Singleton<IAuthenticationSchemeProvider, _AuthenticationSchemeProvider>());
            /// <see cref="AuthenticationCoreServiceCollectionExtensions.AddAuthenticationCore(IServiceCollection)"/>
            //services.TryAddScoped<IAuthenticationService, AuthenticationService>();
            //services.TryAddSingleton<IClaimsTransformation, NoopClaimsTransformation>(); // Can be replaced with scoped ones that use DbContext
            //services.TryAddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
            //services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();

            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = scheme;
                options.DefaultAuthenticateScheme = scheme;// CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = scheme;// CookieAuthenticationDefaults.AuthenticationScheme;
                //options.AddScheme<Test2>("Test2", "Test2");
            });

            //builder
            //    .AddScheme<ApiAuthenticationSchemeOptions, ApiAuthenticationHandler>(_Consts.UserManager.ApiAuthScheme, _Consts.UserManager.ApiAuthScheme, options => { })
            //    .AddCookie(_Consts.UserManager.AccessTokenScheme, options => { })
            //;

            //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<Test1Options>, Test1PostConfig>());
            //builder.AddScheme<Test1Options, Test1Handler>("Test1", "Test1", testConfigure);

            //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.AccessTokenScheme, ConfigureCookie);
            //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.AccessTokenScheme, Authentication.AccessTokenAuthenticationHandler.Configure);
            //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.UserTokenScheme, (name, s, options) =>
            //{
            //    ConfigureCookie<TUser>(name, s, options);
            //    UserTokenAuthenticationHandler.Configure(name, s, options);
            //});
            //builder.AddCookie(scheme, options => { });



            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, _ConfigureCookie>());
            builder.AddCookie(scheme);

            return services;
        }

        private static string GetScheme(HttpContext context, string scheme) => scheme ?? context.RequestServices.GetService<IOptions<AuthenticationOptions>>().Value.DefaultScheme;

        public static async Task<string> SignInAsync(this HttpContext context, UserId userId, string scheme = null)
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.SetUserId(userId);
            //var userStoreItem = this.AddUserStoreItem(user, principal);

            AuthenticationProperties properties = new AuthenticationProperties();

            //context = context ?? _httpContextAccessor.HttpContext;
            await context.SignInAsync(GetScheme(context, scheme), principal, properties);

            //properties.Parameters.TryGetValue(_Consts.UserManager.Ticket_SessionId, out object sessionId);
            principal.GetSessionId(out string sessionId);
            return await Task.FromResult(sessionId/* as string*/);
        }

        public static async Task SignOutAsync(this HttpContext context, UserId userId, string scheme = null)
        {
            //context = context ?? _httpContextAccessor.HttpContext;
            await context.SignOutAsync(GetScheme(context, scheme), null);
        }

        private class _ConfigureCookie : IPostConfigureOptions<CookieAuthenticationOptions>
        {
            private IServiceProvider _services;
            private IHttpContextAccessor _httpContextAccessor;

            #region bind from appsettings.json

            public string CookieName { get; set; }
            public TimeSpan? Expire { get; set; }
            public bool? SlidingExpiration { get; set; }

            #endregion

            public _ConfigureCookie(IServiceProvider services)
            {
                _services = services;
                ClaimsPrincipal.ClaimsPrincipalSelector = ClaimsPrincipalSelector;
            }

            private ClaimsPrincipal ClaimsPrincipalSelector() => _httpContextAccessor.HttpContext?.User;

            void IPostConfigureOptions<CookieAuthenticationOptions>.PostConfigure(string name, CookieAuthenticationOptions options)
            {
                _services.GetRequiredService<IConfiguration>().Bind(_Consts.UserManager.ConfigSection, this);
                var sessionStore = _services.CreateInstance<RedisTicketStore>();
                sessionStore.SchemeName = name;
                options.SessionStore = sessionStore;
                //?? serviceProvider.GetService<UserManager<TUser>>().TicketStore;

                //var authenticationOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
                //options.DataProtectionProvider = options.DataProtectionProvider ?? serviceProvider.GetRequiredService<IDataProtectionProvider>();
                //var dataProtector = options.DataProtectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", authenticationOptions.DefaultScheme, "v2");
                //options.TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(TicketSerializer2.Default, dataProtector);

                if (this.CookieName != null)
                    options.Cookie.Name = this.CookieName;
                options.ExpireTimeSpan = this.Expire ?? TimeSpan.FromMinutes(10);
                options.SlidingExpiration = this.SlidingExpiration ?? true;
            }
        }


        class Test2 : IAuthenticationHandler
        {
            Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync()
            {
                throw new NotImplementedException();
            }

            Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties properties)
            {
                throw new NotImplementedException();
            }

            Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties properties)
            {
                throw new NotImplementedException();
            }

            Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context)
            {
                throw new NotImplementedException();
            }
        }


        static void testConfigure(Test1Options options)
        {
        }

        class Test1PostConfig : IPostConfigureOptions<Test1Options>
        {
            void IPostConfigureOptions<Test1Options>.PostConfigure(string name, Test1Options options)
            {
                throw new NotImplementedException();
            }
        }
        class Test1Options : AuthenticationSchemeOptions
        {
        }
        class Test1Handler : AuthenticationHandler<Test1Options>
        {
            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                throw new NotImplementedException();
            }

            public Test1Handler(IOptionsMonitor<Test1Options> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
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
