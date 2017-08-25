using StackExchange.Redis;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using System.Web.Routing;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
using System.Web.Http.Controllers;

[assembly: PreApplicationStartMethod(typeof(ams._HttpApplication), "Main")]

namespace ams
{
    [_DebuggerStepThrough]
    public class _HttpApplication : System.Web.HttpApplication
    {
        public static void Main()
        {
            TextLogWriter.Enabled = true;

            ConnectionMultiplexer redis1 = ConnectionMultiplexer.Connect(DB.Redis.Message1, new RedisLogWriter());
            DB.RedisChannels.ams.Subscribe(redis1, (ch, value) =>
                log.message("redis", "Channel:{0}, Value:{1}", ch, value));
            DB.RedisChannels.TableVer.Subscribe(redis1, (ch, value) =>
                log.message("redis", "Channel:{0}, Value:{1}", ch, value));
        }

        static HttpConfiguration configuration;
        public static HttpConfiguration Configuration
        {
            get { return configuration ?? GlobalConfiguration.Configuration; }
            private set { configuration = value; }
        }

        public static _HttpApplication Current
        {
            get { return HttpContext.Current?.ApplicationInstance as _HttpApplication; }
        }

        public override void Init()
        {
            base.Init();
            this.RemoveHeaders();
            _HttpContext.Init(this);

            //log.message(null, "init start");

            //SqlConfig.Cache.GetInstance().GetValue();

            //router.Initialize();
            //MessageManager.Initialize();

            //System.Web.Razor.RazorCodeLanguage.Languages.Add("html", new System.Web.Razor.CSharpRazorCodeLanguage());
            //System.Web.WebPages.WebPageHttpHandler.RegisterExtension("html");

            //using (SqlCmd sqlcmd = SqlCmd.Open(DB.DB01W))
            //{
            //	//Permission1.Cache.Init(sqlcmd);
            //	//User.Init(sqlcmd);
            //}
            //_User.Manager.Init(this);
        }
        //public virtual void OwinInit(IAppBuilder app) { }

        protected void RegisterMvc()
        {
            GlobalFilterCollection filters = GlobalFilters.Filters;
            _HttpContext.Init(filters);
            AccessControlFilter.Init(filters);
            filters.Add(new HandleErrorAttribute());

            RouteCollection routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            //).DataTokens.Add("Area", "Default");
        }

        protected void RegisterWebApi(HttpConfiguration config)
        {
            _HttpApplication.Configuration = config;
            _HttpContext.Init(config);
            AccessControlFilter.Init(config);
            //config.Filters.Add(_ExceptionFilterAttribute.Instance);
            _ActionFilterAttribute.Init(config);
            _ActionValueBinder.Init(config);
            //System.Web.Http.ExceptionHandling.

            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.amsInitMediaTypeFormatters();
        }

        void RemoveHeaders()
        {
            System.Web.Mvc.MvcHandler.DisableMvcResponseHeader = true;
            System.Web.WebPages.WebPageHttpHandler.DisableWebPagesResponseHeader = true;
            this.PreSendRequestHeaders += _PreSendRequestHeaders;
        }

        private void _PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context == null) return;
            context.Response.Headers.Remove("X-AspNet-Version");
            context.Response.Headers.Remove("X-SourceFiles");
            context.Response.Headers.Remove("Server");
        }
    }

    //public class _ModelBinder : DefaultModelBinder
    //{
    //    public static void Init()
    //    {
    //        ModelBinders.Binders.Add(typeof(_empty), new _ModelBinder());
    //    }
    //    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        //if (bindingContext.ModelType == typeof(HomePageModels))
    //        //{
    //        //    HttpRequestBase request = controllerContext.HttpContext.Request;

    //        //    string title = request.Form.Get("Title");
    //        //    string day = request.Form.Get("Day");
    //        //    string month = request.Form.Get("Month");
    //        //    string year = request.Form.Get("Year");

    //        //    return new HomePageModels
    //        //    {
    //        //        Title = title,
    //        //        Date = day + "/" + month + "/" + year
    //        //    };

    //        //    //// call the default model binder this new binding context
    //        //    //return base.BindModel(controllerContext, newBindingContext);
    //        //}
    //        return base.BindModel(controllerContext, bindingContext);
    //    }

    //}



    //public class OwinStartup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        //_User.Manager.Init(app);
    //        _HttpApplication.Current?.OwinInit(app);
    //    }

    //    //    [AppSetting("owin:UseWebAPI")]
    //    //    public static bool UseWebAPI
    //    //    {
    //    //        get { return AutomaticAppStartup && app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
    //    //    }
    //    //    [AppSetting("owin:AutomaticAppStartup")]
    //    //    public static bool AutomaticAppStartup
    //    //    {
    //    //        get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
    //    //    }
    //}
}