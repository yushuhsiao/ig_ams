﻿using InnateGlory.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace InnateGlory
{
    public static class UserManagerExtensions
    {
        [DebuggerStepThrough] private static UserManager<TUser> _UserManagerFactory<TUser>(IServiceProvider services) where TUser : class, IUser => services.GetService<UserManager<TUser>>();
        [DebuggerStepThrough] private static TUser /***********/_CurrentUserFactory<TUser>(IServiceProvider services) where TUser : class, IUser => services.GetService<UserManager<TUser>>()?.CurrentUser;

        public static IServiceCollection AddUserManager<TUser>(this IServiceCollection services, string scheme = _Consts.UserManager.ApplicationScheme) where TUser : class, IUser
        {
            /// <see cref="AuthenticationScheme"/>
            /// <see cref="IAuthenticationHandler"/>
            /// <see cref="IAuthenticationRequestHandler"/>
            /// <see cref="IAuthenticationSignInHandler"/>
            /// <see cref="IAuthenticationSignOutHandler"/>

            services.AddHttpContextAccessor();
            services.AddSingleton<UserManager<TUser>>();
            services.AddSingleton<IUserManager>(_UserManagerFactory<TUser>);
            services.AddScoped(_CurrentUserFactory<TUser>);
            services.AddScoped<IUser>(_CurrentUserFactory<TUser>);

            /// <see cref="AuthenticationCoreServiceCollectionExtensions.AddAuthenticationCore(IServiceCollection)"/>
            services.Replace(ServiceDescriptor.Scoped<IAuthenticationService, _AuthenticationService>());
            //services.Replace(ServiceDescriptor.Scoped<IAuthenticationService, _AuthenticationService>());
            //services.Replace(ServiceDescriptor.Singleton<IClaimsTransformation, _ClaimsTransformation>()); // Can be replaced with scoped ones that use DbContext
            //services.Replace(ServiceDescriptor.Scoped<IAuthenticationHandlerProvider, _AuthenticationHandlerProvider>());
            //services.Replace(ServiceDescriptor.Singleton<IAuthenticationSchemeProvider, _AuthenticationSchemeProvider>());

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = scheme;
            })
            .AddScheme<ApiAuthenticationSchemeOptions, ApiAuthenticationHandler<TUser>>(_Consts.UserManager.ApiAuthScheme, _Consts.UserManager.ApiAuthScheme, options => { })
            .AddCookie(_Consts.UserManager.AccessTokenScheme, options => { })
            .AddCookie(scheme, options => { })
            ;

            services.Configure<CookieAuthenticationOptions>(scheme, ConfigureCookie<TUser>);
            services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.AccessTokenScheme, ConfigureCookie<TUser>);
            services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.AccessTokenScheme, Authentication.AccessTokenAuthenticationHandler.Configure);
            //services.Configure<CookieAuthenticationOptions>(_Consts.UserManager.UserTokenScheme, (name, s, options) =>
            //{
            //    ConfigureCookie<TUser>(name, s, options);
            //    UserTokenAuthenticationHandler.Configure(name, s, options);
            //});

            return services;
        }

        // bind from appsettings.json
        private class AuthenticationSection
        {
            public string CookieName { get; set; }
            public TimeSpan? Expire { get; set; }
            public bool? SlidingExpiration { get; set; }
        }

        private static void ConfigureCookie<TUser>(string name, IServiceProvider services, CookieAuthenticationOptions options) where TUser : class, IUser
        {
            var config = services.GetRequiredService<IConfiguration>().Bind<AuthenticationSection>(_Consts.UserManager.ConfigSection);
            var sessionStore = services.CreateInstance<RedisTicketStore<TUser>>();
            sessionStore.SchemeName = name;
            options.SessionStore = sessionStore;
            //?? serviceProvider.GetService<UserManager<TUser>>().TicketStore;

            //var authenticationOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
            //options.DataProtectionProvider = options.DataProtectionProvider ?? serviceProvider.GetRequiredService<IDataProtectionProvider>();
            //var dataProtector = options.DataProtectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", authenticationOptions.DefaultScheme, "v2");
            //options.TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(TicketSerializer2.Default, dataProtector);

            if (config.CookieName != null)
                options.Cookie.Name = config.CookieName;
            options.ExpireTimeSpan = config.Expire ?? TimeSpan.FromMinutes(10);
            options.SlidingExpiration = config.SlidingExpiration ?? true;
        }

        private const string UserIdClaim = "UserId";

        public static void SetUserId(this ClaimsPrincipal principal, UserId userId)
        {
            if (principal == null) return;
            string _id = userId.Id.ToString();
            ClaimsIdentity identity = null;
            foreach (var _identity in principal.Identities)
            {
                foreach (var claim in _identity.Claims)
                {
                    if (claim.Type == UserIdClaim)
                    {
                        if (claim.Value == _id)
                            return;
                        identity = _identity;
                        identity.TryRemoveClaim(claim);
                        break;
                    }
                }
                if (identity != null)
                    break;
            }
            if (identity == null)
                principal.AddIdentity(identity = new ClaimsIdentity());
            identity.AddClaim(new Claim(UserIdClaim, _id));
        }
        public static void SetUserId(this ClaimsIdentity identity, UserId userId)
        {
            if (identity == null) return;
            string _id = userId.ToString();
            foreach (var claim in identity.Claims)
            {
                if (claim.Type == UserIdClaim)
                {
                    if (claim.Value == _id)
                        return;
                    identity.TryRemoveClaim(claim);
                    break;
                }
            }
            identity.AddClaim(new Claim(UserIdClaim, _id));
        }

        public static bool GetUserId(this ClaimsPrincipal principal, out UserId userId)
        {
            if (principal != null)
                foreach (var identity in principal.Identities)
                    if (identity.GetUserId(out userId))
                        return true;
            return _null.noop(false, out userId);
        }
        public static bool GetUserId(this ClaimsIdentity identity, out UserId userId)
        {
            if (identity != null)
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type == UserIdClaim && claim.Value.ToInt64(out long _userId))
                    {
                        userId = _userId;
                        return true;
                    }
                }
            }
            return _null.noop(false, out userId);
        }
    }



}
