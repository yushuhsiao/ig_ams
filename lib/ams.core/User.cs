using ams.Data;
using Microsoft.Owin;
//using Owin;
//using Microsoft.Owin;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace ams
{
    public partial class _User : IPrincipal, IIdentity
    {
        public struct SessionData
        {
            public UserID CorpID;
            public UserType UserType;
            public UserID ID;
            public UserName UserName;

            public static readonly SessionData Null = new SessionData()
            {
                CorpID = UserID.guest,
                UserType = UserType.Guest,
                ID = UserID.guest,
                UserName = "guest",
            };
            public static readonly SessionData Service = new SessionData()
            {
                CorpID = UserID.root,
                UserType = UserType.Agent,
                ID = UserID.root,
                UserName = UserName.root,
            };
        }

        class UserStore : List<_User>
        {
            public SessionData? FromRedis(string sessionID)
            {
                foreach (IDatabase redis in DB.Redis.GetDataBase(this, DB.Redis.UserSession))
                {
                    try
                    {
                        string json_string = redis.StringGet(sessionID);
                        if (!string.IsNullOrEmpty(json_string))
                            return json.DeserializeObject<SessionData>(json_string);
                    }
                    catch { }
                }
                return null;
            }

            private void set_user_expire(_User user)
            {
                TimeSpan t1 = _User.Manager.SessionStateSection.Timeout;
                long t2 = t1.Ticks;
                t2 /= 2;
                user.expireAt = DateTime.Now.AddTicks(t2);
            }


            public void UpdateExpire(_User user)
            {
                set_user_expire(user);
                foreach (var redis in DB.Redis.GetDataBase(this, DB.Redis.UserSession))
                    redis.KeyExpire(user.sessionID, _User.Manager.SessionStateSection.Timeout);
            }

            public void SetUser(_User user)
            {
                set_user_expire(user);
                foreach (var redis in DB.Redis.GetDataBase(this, DB.Redis.UserSession))
                {
                    redis.StringSet(
                        key: user.sessionID,
                        value: json.SerializeObject(user._data),
                        expiry: _User.Manager.SessionStateSection.Timeout);
                }
            }

            public _User GetUser(string sessionID, out SessionData? data)
            {
                _User user = null;
                data = this.FromRedis(sessionID);
                for (int i = this.Count - 1; i >= 0; i--)
                {
                    _User _user = this[i];
                    if (_user.UserType.In(UserType.Agent, UserType.Admin, UserType.Member))
                    {
                        if (DateTime.Now < _user.expireAt)
                        {
                            if (_user.sessionID != sessionID)
                                continue;
                            if (data.HasValue && (data.Value.ID == _user.ID))
                            {
                                this.UpdateExpire(user = _user);
                                continue;
                            }
                        }
                    }
                    using (_user as IDisposable)
                        this.RemoveAt(i);
                }
                return user;
            }

            public void RemoveUser(string sessionID)
            {
                foreach (var redis in DB.Redis.GetDataBase(this, DB.Redis.UserSession))
                    redis.KeyDelete(sessionID);
                for (int i = this.Count - 1; i >= 0; i--)
                {
                    _User _user = this[i];
                    if (_user.sessionID == sessionID)
                        using (_user as IDisposable)
                            this.RemoveAt(i);
                }
            }
        }

        public static class Manager
        {
            static readonly UserStore _users = new UserStore();

            #region //
            //public static void Init(IAppBuilder app)
            //{
            //    //app.CreatePerOwinContext(() => new Manager());
            //    //
            //    //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //    //{
            //    //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    //    CookieName = _User.Manager.SessionStateSection.CookieName,
            //    //    ExpireTimeSpan = _User.Manager.SessionStateSection.Timeout
            //    //});

            //    app.Use(GetUser);

            //    //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ApplicationCookie);

            //    // 讓應用程式使用 Cookie 儲存已登入使用者的資訊
            //    // 並使用 Cookie 暫時儲存使用者利用協力廠商登入提供者登入的相關資訊；
            //    //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //    //{
            //    //    Provider = new CookieAuthenticationProvider()
            //    //    {
            //    //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<Manager, amsUser, int>(
            //    //            validateInterval: TimeSpan.FromMinutes(30),
            //    //            regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType),
            //    //            getUserIdCallback: (id) => (Int32.Parse(id.GetUserId())))
            //    //    }
            //    //});
            //    //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //    //// 設定 OAuth 基礎流程的應用程式
            //    //PublicClientId = "self";
            //    //// 讓應用程式使用 Bearer 權杖驗證使用者

            //    //app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            //    //{
            //    //    //TokenEndpointPath = new PathString("/Token"),
            //    //    Provider = new _OAuthProvider(),
            //    //    //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            //    //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    //    // 在生產模式中設定 AllowInsecureHttp = false
            //    //    AllowInsecureHttp = true
            //    //});
            //}
            #endregion

            //public static void Init(IAppBuilder app) => app.Use(GetUser);

            public static void Init(HttpApplication app)
            {
                //app.PostAcquireRequestState += _PostAcquireRequestState;
                app.PostMapRequestHandler += _PostMapRequestHandler;
                app.AcquireRequestState += _AcquireRequestState;
            }

            //private static void _PostAcquireRequestState(object sender, EventArgs e)
            //{
            //}

            private static void _PostMapRequestHandler(object sender, EventArgs e)
            {
                HttpContext context = HttpContext.Current;
                if (context == null) return;
                if (context.Handler is System.Web.Http.WebHost.HttpControllerHandler)
                    context.SetSessionStateBehavior(SessionStateBehavior.Required);
                //else if (string.Compare(context.Request.AppRelativeCurrentExecutionFilePath, Controllers.UserSignInApiController.login_url, true) == 0)
                //    context.SetSessionStateBehavior(SessionStateBehavior.Required);
            }

            private static void _AcquireRequestState(object sender, EventArgs e)
            {
                _HttpContext context = _HttpContext.Current;
                context.User = _ApiUser.ApiAuth() ?? SessionAuth(context) ?? _User.Null;
            }

            //static string GetSessionID(_HttpContext context)
            //{
            //    string sessionID = null;
            //    if (context == null) return sessionID;
            //    if (context.Session != null)
            //    {
            //        sessionID = context.Session.SessionID;
            //        if (sessionID != null) return sessionID;
            //    }
            //    try
            //    {
            //        IOwinContext owin = context.GetOwinContext();
            //        if (owin == null) return sessionID;
            //        return owin.Request.Cookies[_User.Manager.SessionStateSection.CookieName];
            //    }
            //    catch { }
            //    return sessionID;
            //}

            //private static Task GetUser(IOwinContext owin, Func<Task> next)
            //{
            //    _HttpContext context = _HttpContext.Current;
            //    _User user = null;
            //    if (context != null)
            //        user = context.User as _User;
            //    if ((user == null) || (user == _User.Null))
            //    {
            //        string sessionID;
            //        try { sessionID = owin.Request.Cookies[_User.Manager.SessionStateSection.CookieName]; }
            //        catch { return next.Invoke(); }
            //        user = SessionAuth(context, sessionID);
            //        if (user != null)
            //        {
            //            context.User = user;
            //            return next.Invoke();
            //        }
            //        _ApiUser.ApiAuth(owin);
            //    }
            //    return next.Invoke();
            //}

            private static _User SessionAuth(_HttpContext context)
            {
                string sessionID = context?.Session?.SessionID;
                if (sessionID == null)
                    return null;
                lock (_users)
                {
                    SessionData? data;
                    _User user = _users.GetUser(sessionID, out data);
                    if (user == null)
                    {
                        if (context.Session != null)
                            context.Session["."] = 0;
                        user = new _User(sessionID, data ?? SessionData.Null);
                        _users.Add(user);
                        _users.SetUser(user);
                    }
                    return user;
                }
            }

            //private static _User SessionAuth(_HttpContext context, IOwinContext owin)
            //{
            //    string sessionID = GetSessionID(context, owin);
            //    if (sessionID == null)
            //        return null;
            //    lock (_users)
            //    {
            //        _SessionData data;
            //        _User user = _users.GetUser(sessionID, out data);
            //        if (user == null)
            //        {
            //            if (context.Session != null)
            //                context.Session["."] = 0;
            //            user = new _User(sessionID, data ?? _SessionData.Null);
            //            _users.Add(user);
            //            _users.SetUser(user);
            //        }
            //        return user;
            //    }
            //}

            //private static void GetCurrentUser(_HttpContext context, IOwinContext owin)
            //{
            //    if (context == null) return;
            //    _User user = context.User as _User;
            //    if ((user != null) && (user != _User.Null)) return;
            //    context.User =
            //        owin.ApiAuth(context) ??
            //        SessionAuth(context, owin) ??
            //        _User.Null;
            //}

            public static void SetCurrentUser(UserData userdata)
            {
                _HttpContext context = _HttpContext.Current;
                //_User user = _User.Current;
                //if ((userdata != null) && (user.ID == userdata.ID)) return;
                string sessionID = context?.Session?.SessionID;
                lock (_users)
                {
                    _users.RemoveUser(sessionID);
                    if (userdata == null)
                        return;
                    _User user = new _User(sessionID, new SessionData()
                    {
                        CorpID = userdata.CorpID,
                        UserType = userdata.UserType,
                        ID = userdata.ID,
                        UserName = userdata.UserName,
                    });
                    _users.Add(user);
                    _users.SetUser(user);
                }
            }

            public static SessionStateSection SessionStateSection
            {
                [DebuggerStepThrough]
                get { return ((SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState")); }
            }
        }


        //static readonly RedisValue key_ID = "ID";
        //static readonly RedisValue key_Name = "Name";
        //static readonly RedisValue key_UserType = "UserType";
        //static readonly RedisValue key_CorpID = "CorpID";

        public static _User Current
        {
            get
            {
                return _HttpContext.Current?.User as _User
                    ?? _HttpContext.CurrentOwinContext?.Request?.User as _User
                    ?? _User.Null;
                //if (user == null)
                //{
                //    try { user = (CallContext.LogicalGetData(typeof(IOwinContext).FullName) as IOwinContext)?.Request?.User as _User; }
                //    catch { }
                //}
                //return user ?? _User.Null;
            }
        }

        static readonly _User Null = new _User(null, SessionData.Null);
        public static readonly _User Service = new _User(null, SessionData.Service);

        protected _User(string sessionID, SessionData data)
        {
            this.sessionID = sessionID;
            this._data = data;
        }



        private readonly string sessionID;

        private readonly SessionData _data;

        private DateTime expireAt;

        public UserID ID
        {
            get { return _data.ID; }
        }

        public UserType UserType
        {
            get { return _data.UserType; }
        }

        public string Name
        {
            get { return _data.UserName; }
        }

        public UserID CorpID
        {
            get { return _data.CorpID; }
        }

        CorpInfo _CorpInfo;
        public CorpInfo GetCorpInfo()
        {
            lock (this) return this._CorpInfo = this._CorpInfo ?? CorpInfo.GetCorpInfo(this.CorpID);
        }

        UserData _UserData;
        public UserData GetUserData()
        {
            lock (this) return this._UserData = this._UserData ?? this.GetCorpInfo().GetUserData(this.UserType, this.ID);
        }

        #region IPrincipal

        IIdentity IPrincipal.Identity
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        [DebuggerStepThrough]
        bool IPrincipal.IsInRole(string role)
        {
            return true;
        }

        #endregion

        #region IIdentity

        string IIdentity.AuthenticationType
        {
            [DebuggerStepThrough]
            get { return "Forms"; }
        }

        string IIdentity.Name
        {
            [DebuggerStepThrough]
            get { return this.Name; }
        }

        public bool IsAuthenticated
        {
            [DebuggerStepThrough]
            get { return !this.ID.IsGuest; }
        }

        #endregion
    }

    // root : ID = CorpID = 1, ParentID=0
    // corp : ID = CorpID = n, ParentID=1
}