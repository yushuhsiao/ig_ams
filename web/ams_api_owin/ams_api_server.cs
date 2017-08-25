using ams;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

namespace LogService
{
    class ApiServer
    {
        static void Main(string[] args)
        {
            ConsoleLogWriter.Enabled = true;
            WebApp.Start(ApiServer.Url, new ApiServer().Configuration);
            Console.ReadKey();
        }

        [AppSetting, DefaultValue("http://*:1111/")]
        static string Url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        public void Configuration(IAppBuilder app)
        {
            _User.Manager.Init(app);
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            _MediaTypeFormatters.Init(config);
            app.UseWebApi(config);
        }

        [DebuggerStepThrough]
        Task ProcessRequest(IOwinContext context) => Task.Factory.StartNew(ProcessRequest, context);
        void ProcessRequest(object state)
        {
            IOwinContext context = (IOwinContext)state;
            log.message("owin", "{0}", context.Request.Path);
        }
    }
}
