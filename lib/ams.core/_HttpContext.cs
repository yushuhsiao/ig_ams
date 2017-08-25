//using Owin;
//using Microsoft.Owin;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
using IActionFilter = System.Web.Mvc.IActionFilter;
using IHttpActionFilter = System.Web.Http.Filters.IActionFilter;

namespace System.Web
{
    [_DebuggerStepThrough]
    public partial class _HttpContext : HttpContextWrapper
    {
        readonly HttpContext _context;

        public static void Init(HttpApplication app)
        {
            // http notification order :
            app.BeginRequest += _BeginRequest;
            // BeginRequest 
            // AuthenticateRequest 
            // PostAuthenticateRequest 
            // AuthorizeRequest 
            // PostAuthorizeRequest 
            // ResolveRequestCache 
            // PostResolveRequestCache 
            // MapRequestHandler 
            // PostMapRequestHandler 
            // AcquireRequestState 
            // PostAcquireRequestState 
            // PreRequestHandlerExecute 
            // PostRequestHandlerExecute 
            // ReleaseRequestState 
            // PostReleaseRequestState 
            // UpdateRequestCache 
            // PostUpdateRequestCache 
            // LogRequest 
            // PostLogRequest 
            app.EndRequest += _EndRequest;
            // EndRequest 
            // PreSendRequestContent 
            // PreSendRequestHeaders 
            // RequestCompleted 
            // Error
        }
        public static void Init(GlobalFilterCollection filters) => filters.Add(new _ActionFilter());
        public static void Init(HttpConfiguration config) => config.Filters.Add(new _HttpActionFilter());

        //public static void Init(IAppBuilder app, Action<IOwinContext> cb)
        //{
        //    if (cb == null) return;
        //    app.Use((context, task) =>
        //    {
        //        _BeginRequest(app, EventArgs.Empty);
        //        cb(context);
        //        _EndRequest(app, EventArgs.Empty);
        //        return TaskHelpers.Completed();
        //    });
        //}

        private static void _BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context == null) return;
            new _HttpContext(context);
        }

        private static void _EndRequest(object sender, EventArgs e)
        {
            using (_HttpContext.Current?._data)
                return;
        }

        public System.Web.Mvc.Controller Controller { get; private set; }
        public System.Web.Mvc.ActionDescriptor ActionDescriptor { get; private set; }
        [DebuggerStepThrough]
        class _ActionFilter : IActionFilter
        {
            void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
            {
                _HttpContext.Current.Controller = filterContext.Controller as System.Web.Mvc.Controller;
                _HttpContext.Current.ActionDescriptor = filterContext.ActionDescriptor;
            }

            void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
            {
            }
        }

        public System.Web.Http.Controllers.HttpActionDescriptor HttpActionDescriptor { get; private set; }
        public System.Web.Http.ApiController ApiController { get; private set; }
        [DebuggerStepThrough]
        class _HttpActionFilter : IHttpActionFilter
        {
            bool IFilter.AllowMultiple
            {
                [DebuggerStepThrough]
                get { return this.GetType().GetCustomAttribute<AttributeUsageAttribute>(true).AllowMultiple; }
            }
            Task<HttpResponseMessage> IHttpActionFilter.ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
            {
                _HttpContext context = _HttpContext.Current;
                context.HttpActionDescriptor = actionContext.ActionDescriptor;
                context.ApiController = actionContext.ControllerContext.Controller as System.Web.Http.ApiController;

                //if ((apiController != null) &&
                //    (actionContext.ActionDescriptor.GetCustomAttributes<ams.ControllerArgumentsAttribute>().Count > 0))
                //{
                //    try { ams.json.PopulateObject(context.Arguments, apiController); }
                //    catch { }
                //}
                return continuation();
            }
        }

        public string Arguments
        {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            internal set;
        }

        class __data : /*List<KeyValuePair<string, object>>, */IDisposable
        {
            Dictionary<string, object> _dict;
            List<IDisposable> _cleanups;

            public bool GetValue<T>(out T value, string key)
            {
                lock (this)
                {
                    if (_dict != null)
                    {
                        object tmp;
                        if (_dict.TryGetValue(key, out tmp) && (tmp is T))
                        {
                            value = (T)tmp;
                            return true;
                        }
                    }
                }
                //for (int i = 0, n = this.Count; i < n; i++)
                //{
                //    KeyValuePair<string, object> n1 = this[i];
                //    if (n1.Key != key) continue;
                //    if (n1.Value is T)
                //    {
                //        value = (T)n1.Value;
                //        return true;
                //    }
                //}
                value = default(T);
                return false;
            }

            public T AddValue<T>(string key, T value, bool auto_dispose)
            {
                lock (this)
                {
                    this._dict = this._dict ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    this._dict[key] = value;
                    if (auto_dispose && (value is IDisposable))
                    {
                        this._cleanups=this._cleanups?? new List<IDisposable>();
                        this._cleanups.Add((IDisposable)value);
                    }
                }
                //this.Add(new KeyValuePair<string, object>(key, value));
                //if (auto_dispose)
                //{
                //    if (value is IDisposable)
                //    {
                //        if (_cleanups == null)
                //            _cleanups = new List<IDisposable>();
                //        _cleanups.Add((IDisposable)value);
                //    }
                //}
                return value;
            }

            void IDisposable.Dispose()
            {
                lock (this)
                {
                    if (_dict != null)
                        _dict.Clear();
                    if (_cleanups != null)
                        while (_cleanups.Count > 0)
                            using (_cleanups[0])
                                _cleanups.RemoveAt(0);
                }
            }
        } readonly __data _data = new __data();

        public T GetData<T>(string key, Func<string, T> getInstance, bool auto_dispose = true) where T : class
        {
            if (string.IsNullOrEmpty(key))
                return null;
            lock (this)
            {
                T obj;
                if (_data.GetValue(out obj, key))
                    return obj;

                obj = (getInstance ?? _null.noop<string, T>)(key);
                if (obj == null) return null;
                return _data.AddValue(key, obj, auto_dispose);
            }
        }
        public void GetData<T>(string key, out T location, Func<string, T> getInstance, bool auto_dispose = true) where T : class
        {
            location = GetData<T>(key, getInstance, auto_dispose);
        }

        #region Redis

        //public ConnectionMultiplexer GetRedis(string configuration)
        //{
        //    return GetData(configuration, (key) => ConnectionMultiplexer.Connect(key, new RedisLogWriter()), true);
        //}

        //public static IDisposable GetRedis(out ConnectionMultiplexer result, ConnectionMultiplexer existing, string configuration)
        //{
        //    if ((existing != null) && (existing.Configuration == configuration))
        //    { result = existing; return null; }
        //    _HttpContext context = _HttpContext.Current;
        //    if (context == null)
        //        return result = ConnectionMultiplexer.Connect(configuration, new RedisLogWriter());
        //    else
        //        result = context.GetRedis(configuration);
        //    return null;
        //}

        #endregion

        #region SqlCmd

        //List<SqlCmd> sqlcmd;

        public static IDisposable GetSqlCmd(out SqlCmd result, SqlCmd existing, string connectionString, params string[] connectionStrings)
        {
            if ((existing != null) && (
                (connectionString == existing.ConnectionString) ||
                (connectionStrings.Contains(existing.ConnectionString))))
            { result = existing; return null; }
            _HttpContext context = _HttpContext.Current;
            if (context == null)
                return result = new SqlCmd(connectionString);
            result = context.GetData(connectionString, (key) => new SqlCmd(key), true);
            return null;
        }

        public static SqlCmd GetSqlCmd(string connectionString)
        {
            _HttpContext context = _HttpContext.Current;
            if (context == null)
                return new SqlCmd(connectionString);
            return context.GetData(connectionString, (key) => new SqlCmd(key), true);
        }

        //public static IDisposable GetSqlCmd(out SqlCmd result, SqlCmd existing, string connectionString, params string[] connectionStrings)
        //{
        //    if ((existing != null) && (
        //        (connectionString == existing.ctorConnectionString) ||
        //        (connectionStrings.Contains(existing.ctorConnectionString))))
        //    { result = existing; return null; }
        //    _HttpContext context = _HttpContext.Current;
        //    if (context == null)
        //        return result = new SqlCmd(null, connectionString);
        //    else
        //        result = context.GetSqlCmd(connectionString);
        //    return null;
        //}

        #endregion

        [DebuggerStepThrough]
        public _HttpContext(HttpContext httpContext)
              : base(httpContext)
        {
            this._context = httpContext;
            this._context.Items[typeof(_HttpContext)] = this;
        }

        public static _HttpContext Current
        {
            [DebuggerStepThrough]
            get { return _HttpContext.GetContext(HttpContext.Current); }
            [DebuggerStepThrough]
            set { HttpContext.Current = value._context; }
        }

        public static IOwinContext CurrentOwinContext
        {
            get
            {
                try { return CallContext.LogicalGetData(typeof(IOwinContext).FullName) as IOwinContext as IOwinContext; }
                catch { return null; }
            }
        }
        public static async Task OwinUse(IOwinContext context, Func<Task> next)
        {
            CallContext.LogicalSetData(typeof(IOwinContext).FullName, context);
            await next();
            CallContext.FreeNamedDataSlot(typeof(IOwinContext).FullName);
        }


        //public User _User
        //{
        //    get { return (this.User as User) ?? ams.User.Default; }
        //}

        [DebuggerStepThrough]
        public static _HttpContext GetContext(HttpContext context)
        {
            if (context == null) return null;
            return context.Items[typeof(_HttpContext)] as _HttpContext;
        }

        public string RequestIP
        {
            [DebuggerStepThrough]
            get
            {
                try
                {
                    if (base.Request.IsLocal)
                        return "127.0.0.1";
                    else
                        return base.Request.UserHostAddress;
                }
                catch
                {
                    return "";
                }
            }
        }

        public string SiteRootUrl
        {
            [DebuggerStepThrough]
            get { return this.Request.Url.GetLeftPart(UriPartial.Authority) + this.Request.ApplicationPath; }
        }

        string _FormBody;
        public string ReadFormBody()
        {
            if (_FormBody == null)
            {
                using (StreamReader sr = new StreamReader(this.Request.InputStream))
                    _FormBody = sr.ReadToEnd();
                this.Request.InputStream.Position = 0;
            }
            return _FormBody;
        }

        //[DebuggerStepThrough]
        //public void SetResponseStatusCode(HttpStatusCode statusCode, bool end = false)
        //{
        //    base.Response.StatusCode = (int)statusCode;
        //    try { if (end) base.Response.End(); }
        //    catch { }
        //}
    }

    //partial class _HttpContext
    //{
    //    public override HttpSessionStateBase Session
    //    {
    //        [DebuggerStepThrough]
    //        get { return this._Session ?? base.Session; }
    //    }

    //    _HttpSessionState _session;
    //    public _HttpSessionState _Session
    //    {
    //        get
    //        {
    //            if (this._session == null)
    //            {
    //                HttpSessionState session = this._context.Session;
    //                if (session != null)
    //                    this._session = new _HttpSessionState(this, session);
    //            }
    //            return this._session;
    //        }
    //    }
    //}
    //public class _HttpSessionState : HttpSessionStateWrapper
    //{
    //    public UserID UserID
    //    {
    //        get;
    //        private set;
    //    }

    //    internal _HttpSessionState(_HttpContext context, HttpSessionState httpSessionState) : base(httpSessionState)
    //    {
    //        string sessionID = base.SessionID;
    //        IDatabase redis = _HttpContext.Current.GetRedis(DB.Redis.UserSession).GetDatabase();
    //        int? userID = redis.StringGet(sessionID).ToInt32();
    //        this.UserID = userID ?? UserManager.GuestUserID;
    //        redis.StringSet(sessionID, userID, SessionStateSection.Timeout);
    //    }

    //    public void SetUserID(UserID? userID)
    //    {
    //        string sessionID = base.SessionID;
    //        IDatabase redis = _HttpContext.Current.GetRedis(DB.Redis.UserSession).GetDatabase();
    //        this.UserID = userID ?? UserManager.GuestUserID;
    //        redis.StringSet(sessionID, this.UserID.ToString(), SessionStateSection.Timeout);
    //    }

    //    static SessionStateSection SessionStateSection
    //    {
    //        get { return (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState"); }
    //    }
    //}
}