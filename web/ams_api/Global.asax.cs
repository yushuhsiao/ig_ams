using System;
using System.Web.Http;
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
            GlobalConfiguration.Configure(RegisterWebApi);
        }

        //public override void OwinInit(IAppBuilder app)
        //{
        //    HttpConfiguration config = new HttpConfiguration();
        //    RegisterWebApi(config);
        //    app.UseWebApi(config);
        //}
    }
}