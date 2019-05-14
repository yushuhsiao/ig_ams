using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace InnateGlory
{
    public class amsUser : IUser
    {
        private DataService _dataService;
        public amsUser(DataService dataService)
        {
            _dataService = dataService;
        }
        public amsUser(DataService dataService, Entity.UserData userData)
        {
            _dataService = dataService;
            _userData = userData;
        }

        public UserId Id
        {
            get;
            set;
        }

        private Entity.UserData _userData;
        public Entity.UserData UserData => _userData = _userData ?? _dataService.Users.GetUser(this.Id);

        void t()
        {
            //ClaimsPrincipal.Current.GetUserId
        }
    }

    public class UserManager
    {
        private readonly IServiceProvider _services;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<AuthenticationOptions> _authenticationOptions;
        private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieOptionsMonitor;
        private readonly ILogger _logger;
        private readonly IConfiguration<UserManager> _config;

        public UserManager(IServiceProvider services)
        {
            ClaimsPrincipal.ClaimsPrincipalSelector = ClaimsPrincipalSelector;
            this._services = services;
            this._httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            this._authenticationOptions = services.GetRequiredService<IOptions<AuthenticationOptions>>();
            this._cookieOptionsMonitor = services.GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>();
            this._logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("UserManager");
            this._config = services.GetService<IConfiguration<UserManager>>(); //_services.GetService<ISqlConfig<UserManager<TUser>>>();//.GetService(this);
            this.Guest = services.CreateInstance<amsUser>();
            this.Guest.Id = UserId.Guest;
            //this.Guest.Id = UserId.Root;
            //Tick.OnTick += this.Cleanup;
        }

        public string SchemeName => _authenticationOptions.Value.DefaultScheme;

        public amsUser Guest { get; }

        private amsUser Root { get; }

        public amsUser GetCurrentUser(HttpContext context)
        {
            context = context ?? _httpContextAccessor.HttpContext;
            if (this.GetUserStoreItem(context?.User, out var user))
                return user.User;
            return this.Guest;
        }

        public amsUser CurrentUser => this.GetCurrentUser(null);

        /// <see cref="ClaimsPrincipal.Current"/>
        private ClaimsPrincipal ClaimsPrincipalSelector() => _httpContextAccessor.HttpContext?.User;

        public Task<string> SignInAsync(amsUser user, UserId userId, HttpContext context = null, string scheme = null)
        {
            if (user == null) return Task.FromResult<string>(null);

            ClaimsPrincipal principal = new ClaimsPrincipal();
            var userStoreItem = this.AddUserStoreItem(user, principal);

            AuthenticationProperties properties = new AuthenticationProperties();

            context = context ?? _httpContextAccessor.HttpContext;
            context.SignInAsync(
                scheme: scheme ?? this.SchemeName,
                principal: principal,
                properties: properties);

            properties.Parameters.TryGetValue(_Consts.UserManager.Ticket_SessionId, out object sessionId);
            return Task.FromResult(sessionId as string);
        }

        public Task SignOutAsync(HttpContext context = null)
        {
            context = context ?? _httpContextAccessor.HttpContext;
            return context.SignOutAsync(
                scheme: this.SchemeName,
                properties: null);
        }

        #region UserStore

        private List<UserStoreItem> _users = new List<UserStoreItem>();
        private TimeCounter _cleanup_timer = new TimeCounter();

        //internal Func<TimeSpan> UserDataExpireTime { get; set; }

        /// <summary>
        /// Add <see cref="TUser"/> and set <see cref="Claim"/>
        /// </summary>
        private UserStoreItem AddUserStoreItem(amsUser user, ClaimsPrincipal principal = null)
        {
            if (user == null)
                return null;
            lock (_users)
            {
                if (!this.GetUserStoreItem(user.Id, out var result))
                    _users.Add(result = new UserStoreItem(user));
                principal.SetUserId(user.Id);
                return result;
            }
        }

        internal UserStoreItem GetUserStoreItem(ClaimsPrincipal principal, bool create = false)
        {
            GetUserStoreItem(principal, out UserStoreItem userData, create);
            return userData;
        }
        internal UserStoreItem GetUserStoreItem(ClaimsIdentity identity, bool create = false)
        {
            GetUserStoreItem(identity, out UserStoreItem userData, create);
            return userData;
        }
        internal UserStoreItem GetUserStoreItem(UserId userId, bool create = false)
        {
            GetUserStoreItem(userId, out UserStoreItem userData, create);
            return userData;
        }

        internal bool GetUserStoreItem(ClaimsPrincipal principal, out UserStoreItem userData, bool create = false)
        {
            if (principal.GetUserId(out UserId userId))
                return GetUserStoreItem(userId, out userData, create);
            return _null.noop(false, out userData);
        }
        internal bool GetUserStoreItem(ClaimsIdentity identity, out UserStoreItem userData, bool create = false)
        {
            if (identity.GetUserId(out UserId userId))
                return GetUserStoreItem(userId, out userData, create);
            return _null.noop(false, out userData);
        }
        internal bool GetUserStoreItem(UserId userId, out UserStoreItem result, bool create = false)
        {
            lock (_users)
            {
                result = null;
                double idleTimeout = 0;
                if (_cleanup_timer.IsTimeout(100, true))
                {
                    TimeSpan t = _cookieOptionsMonitor.Get(this.SchemeName).ExpireTimeSpan;
                    idleTimeout = t.TotalMilliseconds.Max(60000) * 2;
                }

                for (int i = _users.Count - 1; i >= 0; i--)
                {
                    var user = _users[i];
                    if (user.User.Id == userId)
                    {
                        result = user;
                        user.Timer.Reset();
                        if (idleTimeout == 0) break;
                    }
                    if (idleTimeout > 0 && user.Timer.IsTimeout(idleTimeout))
                    {
                        _logger.Log(LogLevel.Information, 0, $"Release UserData : {user}");
                        _users.RemoveAt(i);
                    }
                }

                if (result == null && create)
                {
                    amsUser user = _services.CreateInstance<amsUser>();
                    user.Id = userId;
                    _users.Add(result = new UserStoreItem(user));
                }
                return result != null;
            }
        }

        internal class UserStoreItem
        {
            public TimeCounter Timer { get; } = new TimeCounter();

            public amsUser User { get; set; }

            public UserStoreItem(amsUser user) { this.User = user; }
        }

        #endregion

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection, Key = _Consts.UserManager.InternalApiServer), DefaultValue(false)]
        public bool InternalApiServer => _config.GetValue<bool>();

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(true)]
        public bool AllowAgentLogin => _config.GetValue<bool>();

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(true)]
        public bool AllowAdminLogin => _config.GetValue<bool>();

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(false)]
        public bool AllowMemberLogin => _config.GetValue<bool>();
    }

    partial class amsExtensions
    {
        public static IUser GetCurrentUser(this IServiceProvider services) => services.GetService<UserManager>().CurrentUser;
        public static IUser GetCurrentUser(this HttpContext context) => context.RequestServices.GetService<UserManager>().GetCurrentUser(context);
    }
}