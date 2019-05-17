using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InnateGlory
{
    public class UserManager
    {
        //private readonly DataService _services;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IOptions<AuthenticationOptions> _authenticationOptions;
        //private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieOptionsMonitor;
        //private readonly ILogger _logger;
        private readonly IConfiguration<UserManager> _config;

        public UserManager(DataService services)
        {
            ClaimsPrincipal.ClaimsPrincipalSelector = ClaimsPrincipalSelector;
            //this._services = services;
            this._httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            //this._authenticationOptions = services.GetRequiredService<IOptions<AuthenticationOptions>>();
            //this._cookieOptionsMonitor = services.GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>();
            //this._logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("UserManager");
            this._config = services.GetService<IConfiguration<UserManager>>(); //_services.GetService<ISqlConfig<UserManager<TUser>>>();//.GetService(this);
            //this.Guest = services.CreateInstance<UserIdentity>(this, UserId.Guest);
            //this.Guest.Id = UserId.Root;
            //Tick.OnTick += this.Cleanup;
        }

        //public string SchemeName => _authenticationOptions.Value.DefaultScheme;

        //public UserIdentity Guest { get; }

        //private UserIdentity Root { get; }

        //public UserIdentity GetCurrentUser(HttpContext context)
        //{
        //    context = context ?? _httpContextAccessor.HttpContext;
        //    //if (this.GetUserStoreItem(context?.User, out var user))
        //    //    return user;
        //    return this.Guest;
        //}

        //public UserIdentity CurrentUser => this.GetCurrentUser(null);

        /// <see cref="ClaimsPrincipal.Current"/>
        private ClaimsPrincipal ClaimsPrincipalSelector() => _httpContextAccessor.HttpContext?.User;

        private static string GetScheme(HttpContext context, string scheme) => scheme ?? context.RequestServices.GetService<IOptions<AuthenticationOptions>>().Value.DefaultScheme;

        public async Task<string> SignInAsync(HttpContext context, UserId userId, string scheme = null)
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.SetUserId(userId);
            //var userStoreItem = this.AddUserStoreItem(user, principal);

            AuthenticationProperties properties = new AuthenticationProperties();

            context = context ?? _httpContextAccessor.HttpContext;
            await context.SignInAsync(GetScheme(context, scheme), principal, properties);

            //properties.Parameters.TryGetValue(_Consts.UserManager.Ticket_SessionId, out object sessionId);
            principal.GetSessionId(out string sessionId);
            return await Task.FromResult(sessionId/* as string*/);
        }

        public async Task SignOutAsync(HttpContext context, UserId userId, string scheme = null)
        {
            context = context ?? _httpContextAccessor.HttpContext;
            await context.SignOutAsync(GetScheme(context, scheme), null);
        }

        //public Task<string> SignInAsync(UserIdentity user, UserId userId, HttpContext context = null, string scheme = null)
        //{
        //    if (user == null) return Task.FromResult<string>(null);

        //    ClaimsPrincipal principal = new ClaimsPrincipal();
        //    var userStoreItem = this.AddUserStoreItem(user, principal);

        //    AuthenticationProperties properties = new AuthenticationProperties();

        //    context = context ?? _httpContextAccessor.HttpContext;
        //    context.SignInAsync(
        //        scheme: scheme ?? this.SchemeName,
        //        principal: principal,
        //        properties: properties);

        //    properties.Parameters.TryGetValue(_Consts.UserManager.Ticket_SessionId, out object sessionId);
        //    return Task.FromResult(sessionId as string);
        //}

        //public Task SignOutAsync(HttpContext context = null)
        //{
        //    context = context ?? _httpContextAccessor.HttpContext;
        //    return context.SignOutAsync(
        //        scheme: this.SchemeName,
        //        properties: null);
        //}
        //#region UserStore

        //private List<UserIdentity> _users = new List<UserIdentity>();
        //private TimeCounter _cleanup_timer = new TimeCounter();

        //internal Func<TimeSpan> UserDataExpireTime { get; set; }

        /// <summary>
        /// Add <see cref="TUser"/> and set <see cref="Claim"/>
        /// </summary>
        //internal UserIdentity AddUserStoreItem(UserIdentity user, ClaimsPrincipal principal = null)
        //{
        //    if (user == null)
        //        return null;
        //    lock (_users)
        //    {
        //        if (!this.GetUserStoreItem(user.Id, out var result))
        //            _users.Add(result = user);
        //        //principal._SetUserId(user.Id);
        //        principal.SetUserId(user.Id);
        //        return result;
        //    }
        //}

        //internal UserIdentity GetUserStoreItem(ClaimsPrincipal principal, bool create = false)
        //{
        //    GetUserStoreItem(principal, out var userIdentity, create);
        //    return userIdentity;
        //}
        //internal UserIdentity GetUserStoreItem(ClaimsIdentity identity, bool create = false)
        //{
        //    GetUserStoreItem(identity, out var userIdentity, create);
        //    return userIdentity;
        //}
        //internal UserIdentity GetUserStoreItem(UserId userId, bool create = false)
        //{
        //    GetUserStoreItem(userId, out UserIdentity userIdentity, create);
        //    return userIdentity;
        //}

        //internal bool GetUserStoreItem(ClaimsPrincipal principal, out UserIdentity userIdentity, bool create = false)
        //{
        //    if (principal._GetUserId(out UserId userId))
        //        return GetUserStoreItem(userId, out userIdentity, create);
        //    return _null.noop(false, out userIdentity);
        //}
        //internal bool GetUserStoreItem(ClaimsIdentity identity, out UserIdentity userIdentity, bool create = false)
        //{
        //    if (identity._GetUserId(out UserId userId))
        //        return GetUserStoreItem(userId, out userIdentity, create);
        //    return _null.noop(false, out userIdentity);
        //}
        //internal bool GetUserStoreItem(UserId userId, out UserIdentity result, bool create = false)
        //{
        //    lock (_users)
        //    {
        //        result = null;
        //        double idleTimeout = 0;
        //        if (_cleanup_timer.IsTimeout(100, true))
        //        {
        //            TimeSpan t = _cookieOptionsMonitor.Get(this.SchemeName).ExpireTimeSpan;
        //            idleTimeout = t.TotalMilliseconds.Max(60000) * 2;
        //        }

        //        for (int i = _users.Count - 1; i >= 0; i--)
        //        {
        //            var user = _users[i];
        //            if (user.Id == userId)
        //            {
        //                result = user;
        //                user.Timer.Reset();
        //                if (idleTimeout == 0) break;
        //            }
        //            if (idleTimeout > 0 && user.Timer.IsTimeout(idleTimeout))
        //            {
        //                _logger.Log(LogLevel.Information, 0, $"Release UserData : {user}");
        //                _users.RemoveAt(i);
        //            }
        //        }

        //        if (result == null && create)
        //        {
        //            UserIdentity user = _services.CreateInstance<UserIdentity>(this, userId);
        //            _users.Add(result = user);
        //        }
        //        return result != null;
        //    }
        //}

        //internal class UserStoreItem
        //{
        //    public TimeCounter Timer { get; } = new TimeCounter();

        //    public UserIdentity User { get; set; }

        //    public UserStoreItem(UserIdentity user) { this.User = user; }
        //}

        //#endregion

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
    }
}