using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using ams;
using System.Web;

[assembly: OwinStartup(typeof(payment_sample.Startup))]

namespace payment_sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請參閱  http://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }

        static api_client api = new api_client()
        {
            AUTH_SITE = "ig02",
            AUTH_USER = "test",
            API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQAz3R5jA4jVU2XZgEtDfxWcEgQkdOTVy1eCTpfE6dPGRZzaQy908l4ZiMB2mPXbdsv7XGMihlGG/x6/yWaeQ0FbeAcetRfHMKIJMTjhaZ01plj8+JFBKg38W6PyqsL/9xWT7zCLXWdvrKhnWZv2ikJkUpcAOUiaxHaKEiUfjWT3ng==",
            //AUTH_SITE = "ig05",
            //AUTH_USER = "_acc",
            //API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQC7EXGp7klm5zCsaJEfHcltz2ZQHHEWNeKVhgtBscW0FITNBZHu1ceVeEnOQJZdxOiTu7c7jmSZD9Ts7TazHeKCiJCrZPkvZ/iqNOzkIOXrRnMW+aIu860P8LBtvcofXS95Qbi4Cn39A7/Ph7cwc64hRYL5ZNp2YcsOsAn7F8mtzw==",
            BASE_URL = "http://127.0.0.1:7001",
        };

        public static ForwardGameResult submit(string PaymentType, string ResultUrl)
        {
            return submit(
                HttpContext.Current.Request.Form["name"],
                HttpContext.Current.Request.Form["mn"].ToInt32() ?? 0,
                PaymentType,
                null, //"http://10.10.10.86:7001/payment_sample/Notify",
                "http://ams.betis73168.com:7001/payment_sample/" + ResultUrl);
        }

        public static ForwardGameResult submit(string name, int mn, string PaymentType, string NotifyUrl, string ResultUrl)
        {
            ErrorMessage msg = null;
            return api.SubmitPayment(name, mn, PaymentType: PaymentType, NotifyUrl: NotifyUrl, ResultUrl: ResultUrl, onError: (_msg) => msg = _msg);
        }
    }

    public class NotifyApiController : ApiController
    {
        [Route("~/Notify")]
        public void Notify()
        {
        }
    }
}
