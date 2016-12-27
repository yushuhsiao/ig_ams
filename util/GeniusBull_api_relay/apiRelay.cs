using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web;
using System.Web.Routing;
using System.Net;
using System.IO;

[assembly: OwinStartup(typeof(IG.OwinStartup))]
[assembly: PreApplicationStartMethod(typeof(IG.Relay), "Start")]

namespace IG
{
    public class Relay : RouteBase, IRouteHandler, IHttpHandler
    {
        public static void Start()
        {
            TextLogWriter.Enabled = true;
            RouteTable.Routes.Add(new Relay());
        }

        RouteData default_RouteData;

        public Relay()
        {
            default_RouteData = new RouteData(this, this);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext) => default_RouteData;

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) => this;

        bool IHttpHandler.IsReusable { get { return true; } }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            log.message(null, $"{context.Request.UserHostAddress}\t{context.Request.Path}");
            byte[] tmp = null;
            string url = $"http://10.10.10.32:9080{context.Request.Path}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = context.Request.HttpMethod;
            request.ContentType = context.Request.ContentType;
            if (context.Request.InputStream.Length > 0)
            {
                tmp = new byte[8192];
                using (Stream s1 = request.GetRequestStream())
                {
                    for (;;)
                    {
                        int n = context.Request.InputStream.Read(tmp, 0, tmp.Length);
                        if (n <= 0) break;
                        s1.Write(tmp, 0, tmp.Length);
                    }
                }
            }

            HttpWebResponse response = null;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
            using (response)
            {
                context.Response.StatusCode = (int)response.StatusCode;
                context.Response.ContentType = response.ContentType;
                using (Stream s2 = response.GetResponseStream())
                using (context.Response.OutputStream)
                {
                    tmp = tmp ?? new byte[8192];
                    for (;;)
                    {
                        int n = s2.Read(tmp, 0, tmp.Length);
                        if (n <= 0) break;
                        context.Response.OutputStream.Write(tmp, 0, n);
                    }
                }
            }
        }
    }
    public class OwinStartup
    {

        public void Configuration(IAppBuilder app)
        {
            app.Map("/*", xxx);
            app.Use(xxx);
        }

        void xxx(IAppBuilder app)
        {
        }

        Task xxx(IOwinContext context, Func<Task> task)
        {
            return task();
        }
    }
}
