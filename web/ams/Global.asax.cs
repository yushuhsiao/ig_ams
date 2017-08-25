using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.V8;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
//[assembly: Microsoft.Owin.OwinStartup(typeof(ams.OwinStartup))]

namespace ams
{
    public class Global : _HttpApplication
    {
        public Global()
        {
            typeof(ams.Data.IG01PlatformInfo).ToString();
            typeof(ams.Data.AppealPlatformInfo).ToString();
            typeof(SunTech.PaymentInfo_SunTech).ToString();
        }

        public override void Init()
        {
            base.Init();
            //ams.SqlConfig.Cache.GetValue();
            _User.Manager.Init(this);
        }

        void Application_Start(object sender, EventArgs e)
        {
            log.message(null, "Application_Start 0");
            GlobalConfiguration.Configure(RegisterWebApi);
            RegisterMvc();
            //Global.RegisterGlobalFilters(GlobalFilters.Filters);
            //Areas.HelpPage.HelpPageConfig.Register(GlobalConfiguration.Configuration);
            //Global.RegisterRoutes(RouteTable.Routes);
            Configure(JsEngineSwitcher.Instance);
            Bundles.RegisterBundles(BundleTable.Bundles);
            log.message(null, "Application_Start 1");
        }
        protected virtual void OnApplication_Start() { }

        public static void Configure(JsEngineSwitcher engineSwitcher)
        {
            engineSwitcher.EngineFactories
                .AddMsie(new MsieSettings
                {
                    UseEcmaScript5Polyfill = true,
                    UseJson2Library = true
                })
                .AddV8();
            engineSwitcher.DefaultEngineName = MsieJsEngine.EngineName;
        }

        //public override void OwinInit(IAppBuilder app)
        //{
        //    if (OwinStartup.UseWebAPI)
        //    {
        //        HttpConfiguration config = new HttpConfiguration();
        //        Areas.HelpPage.HelpPageConfig.Register(config);
        //        RegisterWebApi(config);
        //        app.UseWebApi(config);
        //    }
        //    //app.Use((owinContext, next) =>
        //    //{
        //    //    _HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        //    //    return next();
        //    //});

        //    //app.UseStageMarker(PipelineStage.MapHandler);

        //    //app.UseKentorOwinCookieSaver();

        //    //app.MapSignalR(MessageHub.url, new HubConfiguration());
        //}

        //static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    AccessControlFilter.Init(filters);
        //    filters.Add(new HandleErrorAttribute());
        //}

        //static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapMvcAttributeRoutes();

        //    //routes.MapRoute(
        //    //    name: "Default",
        //    //    url: "{controller}/{action}/{id}",
        //    //    defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
        //    //).DataTokens.Add("Area", "Default");
        //}
    }
}