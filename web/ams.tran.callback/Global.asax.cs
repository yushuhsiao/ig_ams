using Microsoft.Owin;
using Owin;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: OwinStartup(typeof(ams.OwinStart))]

namespace ams
{
    public class OwinStart
    {
        public void Configuration(IAppBuilder app)
        {
            TextLogWriter.Enabled = true;
            app.Use((context, task) =>
            {
                string body;
                using (StreamReader sr = new StreamReader(context.Request.Body))
                    body = sr.ReadToEnd();
                context.Request.Body.Position = 0;
                log.message("Http", $@"Url : {context.Request.Uri}
RemoteIpAddress : {context.Request.RemoteIpAddress}
{body}");
                return TaskHelpers.Completed();
            });
        }
        bool log_form_post()
        {
            _HttpContext context = _HttpContext.Current;
            if (string.Compare(context.Request.HttpMethod, "post", true) != 0) return false;
            string url = context.Request.AppRelativeCurrentExecutionFilePath;
            log.message("Http", $@"Url : {context.Request.Url}
RemoteIpAddress : {context.RequestIP}
{context.ReadFormBody()}");
            return true;
        }
    }

    public class Global : HttpApplication
    {
        public override void Init()
        {
            base.Init();
            _HttpContext.Init(this);
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalFilterCollection filters = GlobalFilters.Filters;
            filters.Add(new AccessLimitFilter());
            filters.Add(new HandleErrorAttribute());
            _HttpContext.Init(filters);
            GlobalConfiguration.Configure(RegisterWebApi);
            Global.RegisterRoutes(RouteTable.Routes);
        }

        static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            //).DataTokens.Add("Area", "Default");
        }

        static void RegisterWebApi(HttpConfiguration config)
        {
            _HttpContext.Init(config);
            config.Filters.Add(new AccessLimitFilter());
            config.MapHttpAttributeRoutes();
            //config.Filters.Add(_ExceptionFilterAttribute.Instance);
            //_ActionFilterAttribute.Init(config);

            //_MediaTypeFormatters.Init(config);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

        }

        public static HttpStatusCode PostHttpRequest(string url, string request_text) { string response_text; return PostHttpRequest(url, request_text, out response_text); }
        public static HttpStatusCode PostHttpRequest(string url, string request_text, out string response_text)
        {
            Guid id = Guid.NewGuid();
            DateTime t1 = DateTime.Now;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter sw = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
                sw.Write(request_text);
            log.message("Notify", $@"ID : {id}
Url : {url}
{request_text}");
            HttpWebResponse response = null;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
            using (response)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    response_text = sr.ReadToEnd();
                TimeSpan t2 = DateTime.Now - t1;
                log.message("Notify", $@"ID : {id}, Status : {response.StatusCode}, Time : {(int)t2.TotalMilliseconds}ms
{response_text}");
                return response.StatusCode;
            }
        }
    }
}