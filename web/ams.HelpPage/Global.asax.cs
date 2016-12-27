using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace ams.HelpPage
{
    public class Global : _HttpApplication
    {
        public Global()
        {
            typeof(ams.Data.IG01PlatformInfo).ToString();
            typeof(SunTech.PaymentInfo_SunTech).ToString();
        }

        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(RegisterWebApi);
            RegisterMvc();
            Areas.HelpPage.HelpPageConfig.Register(GlobalConfiguration.Configuration);
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}