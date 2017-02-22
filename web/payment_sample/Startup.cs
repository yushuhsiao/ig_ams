using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using ams;
using System.Web;
using System.Net.Http.Formatting;

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
            //AUTH_SITE = "ig02",
            //AUTH_USER = "test",
            //API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQAz3R5jA4jVU2XZgEtDfxWcEgQkdOTVy1eCTpfE6dPGRZzaQy908l4ZiMB2mPXbdsv7XGMihlGG/x6/yWaeQ0FbeAcetRfHMKIJMTjhaZ01plj8+JFBKg38W6PyqsL/9xWT7zCLXWdvrKhnWZv2ikJkUpcAOUiaxHaKEiUfjWT3ng==",
            AUTH_SITE = "ig07",
            AUTH_USER = "_api_user",
            API_KEY = "BwIAAACkAABSU0EyAAQAAAEAAQArS1TqSr1Te3J5iaSDzERfjyhFfpNrTYkNAmyyQkK7k0spsJ9CWuOKlJM4j9kFWZrqJK9rOsY0GQVOitGgIa5uVeZAGacsL3G8T7jXHN2Xv5tbkUCULwErJImJC7GcYXSSt9KxjLW9Elpe4lOazrnJfJ0X+OoX52tegbjGhN89qVGSsOSYMMdRevo3Ci0oU1wocCA/eeVELNLMRoulX4Zc6WA775nELSq07JPAJt6kAIVwzkF9ZG/toQySk2dNrse7Sq5g71vexlcbjCl1dhU70uQDF+gCM6q9u9lcC/x+OiPWRWVsbbAq09dZwB4e97LHoUnqDqNTgexyWvziv/nYcaOrPbp3uKyIo8HmRIvk8ltB7GDiaSHOJyxbxGOOumIg75A4xE/uJGFomuIwTipvxZgSk+ND2r0oShG18i79uI9YhTs6VU/ySY3KFKxyE2qimAIea1GmmEfXMsZCAGXMEU/mXoqhSiyota3gVo/vVniwcRjhANFgeV74Tq1fHTHAHWZjAtXV2BtQJrF6q0toUa0QbkInGUOyrCYsA1GeNqu2orGAa9TAe/AOTgqn68YaU+nHdPPmgP00136UswWR0XZb0xoTrA3/SVfE+t4+jiCADqJZST9Q5sKlJqXiQC133LrZQheJZt6oxFiGotniMAfCay8rKtxNA/c/hBntgoVUL6A+/ZrAjwXB6H/+tzalH2ghc+FzgdF4SiGt8GL5T7jDemjp5NuJ2XHTV3JDVUUFfMosVV1BC+xTPCCS5QE=",
            BASE_URL = "http://127.0.0.1:7001",
        };

        public static ForwardGameResult submit(string PaymentType, string ResultUrl)
        {
            return submit(
                HttpContext.Current.Request.Form["name"],
                HttpContext.Current.Request.Form["mn"].ToInt32() ?? 0,
                PaymentType,
                "http://10.10.10.250:7001/payment_sample/Notify",
                "http://ams.betis73168.com:7001/payment_sample/" + ResultUrl,
                HttpContext.Current.Request.Form["ResultType"]);
        }

        public static ForwardGameResult submit(string name, int mn, string PaymentType, string NotifyUrl, string ResultUrl, string ResultType)
        {
            ErrorMessage msg = null; return api.SubmitPayment(name, mn, PaymentType: PaymentType, NotifyUrl: NotifyUrl, ResultUrl: ResultUrl, ResultType: ResultType, onError: (_msg) => msg = _msg);
        }
    }

    public class NotifyApiController : ApiController
    {
        [Route("~/Notify")]
        public void Notify(FormDataCollection form)
        {
        }
    }
}
