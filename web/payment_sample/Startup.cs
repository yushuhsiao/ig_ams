using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using ams;
using System.Web;
using System.Net.Http.Formatting;

//using PayNow; //要拿掉，不可能叫所有程式都引用

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

        /// <summary>
        /// API 選擇 
        /// ig02測試 ig07正式
        /// </summary>
        static api_client api = new api_client()
        {
            AUTH_SITE = "ig02",
            AUTH_USER = "test",
            API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQAz3R5jA4jVU2XZgEtDfxWcEgQkdOTVy1eCTpfE6dPGRZzaQy908l4ZiMB2mPXbdsv7XGMihlGG/x6/yWaeQ0FbeAcetRfHMKIJMTjhaZ01plj8+JFBKg38W6PyqsL/9xWT7zCLXWdvrKhnWZv2ikJkUpcAOUiaxHaKEiUfjWT3ng==",
            //AUTH_SITE = "ig07",
            //AUTH_USER = "_api_user",
            //API_KEY = "BwIAAACkAABSU0EyAAQAAAEAAQArS1TqSr1Te3J5iaSDzERfjyhFfpNrTYkNAmyyQkK7k0spsJ9CWuOKlJM4j9kFWZrqJK9rOsY0GQVOitGgIa5uVeZAGacsL3G8T7jXHN2Xv5tbkUCULwErJImJC7GcYXSSt9KxjLW9Elpe4lOazrnJfJ0X+OoX52tegbjGhN89qVGSsOSYMMdRevo3Ci0oU1wocCA/eeVELNLMRoulX4Zc6WA775nELSq07JPAJt6kAIVwzkF9ZG/toQySk2dNrse7Sq5g71vexlcbjCl1dhU70uQDF+gCM6q9u9lcC/x+OiPWRWVsbbAq09dZwB4e97LHoUnqDqNTgexyWvziv/nYcaOrPbp3uKyIo8HmRIvk8ltB7GDiaSHOJyxbxGOOumIg75A4xE/uJGFomuIwTipvxZgSk+ND2r0oShG18i79uI9YhTs6VU/ySY3KFKxyE2qimAIea1GmmEfXMsZCAGXMEU/mXoqhSiyota3gVo/vVniwcRjhANFgeV74Tq1fHTHAHWZjAtXV2BtQJrF6q0toUa0QbkInGUOyrCYsA1GeNqu2orGAa9TAe/AOTgqn68YaU+nHdPPmgP00136UswWR0XZb0xoTrA3/SVfE+t4+jiCADqJZST9Q5sKlJqXiQC133LrZQheJZt6oxFiGotniMAfCay8rKtxNA/c/hBntgoVUL6A+/ZrAjwXB6H/+tzalH2ghc+FzgdF4SiGt8GL5T7jDemjp5NuJ2XHTV3JDVUUFfMosVV1BC+xTPCCS5QE=",
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
            ErrorMessage msg = null;
            return api.SubmitPayment(name, mn, PaymentType: PaymentType, NotifyUrl: NotifyUrl, ResultUrl: ResultUrl, ResultType: ResultType, onError: (_msg) => msg = _msg);
        }

        //電子發票

        public static InvoiceResult Invoice(string Invoice, string RequestUrl)
        {
            return Invoice1(
                HttpContext.Current.Request.Form["mem_cid"],
                HttpContext.Current.Request.Form["Description"],
                HttpContext.Current.Request.Form["Amount"].ToInt32() ?? 0,
                HttpContext.Current.Request.Form["Quantity"].ToInt32() ?? 0,
                HttpContext.Current.Request.Form["UnitPrice"].ToInt32() ?? 0,
                HttpContext.Current.Request.Form["SerialNumber"],
                HttpContext.Current.Request.Form["Remark"],
                HttpContext.Current.Request.Form["BuyerId"],
                HttpContext.Current.Request.Form["BuyerName"],
                HttpContext.Current.Request.Form["BuyerAdd"],
                HttpContext.Current.Request.Form["BuyerPhoneNo"],
                HttpContext.Current.Request.Form["BuyerEmail"],
                HttpContext.Current.Request.Form["TotalAmount"].ToInt32() ?? 0,
                HttpContext.Current.Request.Form["OrderInfo"],
                (bool)HttpContext.Current.Session["Send"],
                HttpContext.Current.Request.Form["CarrierType"],
                HttpContext.Current.Request.Form["CarrierId1"],
                HttpContext.Current.Request.Form["CarrierId2"],
                HttpContext.Current.Request.Form["NPOBAN"]);
        }

        public static InvoiceResult Invoice1(string mem_cid, string Description, int Amount, int Quantity, int UnitPrice, string SerialNumber, string Remark, string BuyerId, string BuyerName, string BuyerAdd, string BuyerPhoneNo, string BuyerEmail, int TotalAmount, string OrderInfo, bool Send, string CarrierType, string CarrierId1, string CarrierId2, string NPOBAN)
        {
            ErrorMessage msg = null; return api.GetInvoice(mem_cid, Description, Amount, Quantity, UnitPrice, SerialNumber, Remark, BuyerId, BuyerName, BuyerAdd, BuyerPhoneNo, BuyerEmail, TotalAmount, OrderInfo, Send, CarrierType, CarrierId1, CarrierId2, NPOBAN, onError: (_msg) => msg = _msg);
        }

        #region 電子發票 // 不應該引用 ams.payments

        //    static Invoice EInvoice = new Invoice();

        //    #region getInvoice 取得發票號碼

        //    public static string mem_cid()
        //    {
        //        return HttpContext.Current.Request.Form["mem_cid"];
        //    }

        //    public static string getInvoiceNumber()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        return EInvoice.getInvoice();
        //    }
        //    #endregion

        //    #region uploadInvoiceBody 發票明細資料上傳

        //    //public static Invoice invoiceBody(int Amount, int Quantity, int UnitPrice, string SerialNumber)
        //    //{
        //    //    return invoiceBody(
        //    //        HttpContext.Current.Request.Form["Amount"].ToInt32() ?? 0,
        //    //        HttpContext.Current.Request.Form["Quantity"].ToInt32() ?? 0,
        //    //        HttpContext.Current.Request.Form["UnitPrice"].ToInt32() ?? 0,
        //    //        HttpContext.Current.Request.Form["SerialNumber"]);
        //    //}

        //    public static int Amount()
        //    {
        //        return HttpContext.Current.Request.Form["Amount"].ToInt32() ?? 0;
        //    }
        //    public static int Quantity()
        //    {
        //        return HttpContext.Current.Request.Form["Quantity"].ToInt32() ?? 0;
        //    }
        //    public static int UnitPrice()
        //    {
        //        return HttpContext.Current.Request.Form["UnitPrice"].ToInt32() ?? 0;
        //    }
        //    public static string SerialNumber()
        //    {
        //        return HttpContext.Current.Request.Form["SerialNumber"];
        //    }
        //    public static string Description()
        //    {
        //        return HttpContext.Current.Request.Form["Description"];
        //    }
        //    public static string Remark()
        //    {
        //        return HttpContext.Current.Request.Form["Remark"];
        //    }

        //    public static string InvoiceNo()
        //    {
        //        return HttpContext.Current.Request.Form["InvoiceNo"];
        //    }

        //    public static object ObjInvoiceBody()
        //    {
        //        EInvoice.InvoiceNo = InvoiceNo();
        //        EInvoice.Description = Description();
        //        EInvoice.Amount = Amount();
        //        EInvoice.Quantity = Quantity();
        //        EInvoice.UnitPrice = UnitPrice();
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.SerialNumber = SerialNumber();
        //        EInvoice.Remark = Remark();
        //        return EInvoice.uploadInvoiceBody();
        //    }

        //    #endregion

        //    #region UploadInvoice  發票資訊上傳

        //    public static string mem_pw()
        //    {
        //        return HttpContext.Current.Request.Form["mem_pw"];
        //    }
        //    public static string BuyerId()
        //    {
        //        return HttpContext.Current.Request.Form["BuyerId"];
        //    }
        //    public static string BuyerName()
        //    {
        //        return HttpContext.Current.Request.Form["BuyerName"];
        //    }
        //    public static string BuyerPhoneNo()
        //    {
        //        return HttpContext.Current.Request.Form["BuyerPhoneNo"];
        //    }
        //    public static string BuyerAdd()
        //    {
        //        return HttpContext.Current.Request.Form["BuyerAdd"];
        //    }
        //    public static string BuyerEmail()
        //    {
        //        return HttpContext.Current.Request.Form["BuyerEmail"];
        //    }
        //    public static int TotalAmount()
        //    {
        //        return HttpContext.Current.Request.Form["TotalAmount"].ToInt32() ?? 0;
        //    }
        //    public static string OrderInfo()
        //    {
        //        return HttpContext.Current.Request.Form["OrderInfo"];
        //    }
        //    public static int send()
        //    {
        //        return HttpContext.Current.Request.Form["send"].ToInt32() ?? 0;
        //    }
        //    public static string CarrierType()
        //    {
        //        return HttpContext.Current.Request.Form["CarrierType"];
        //    }
        //    public static string CarrierId1()
        //    {
        //        return HttpContext.Current.Request.Form["CarrierId1"];
        //    }
        //    public static string CarrierId2()
        //    {
        //        return HttpContext.Current.Request.Form["CarrierId2"];
        //    }
        //    public static string NPOBAN()
        //    {
        //        return HttpContext.Current.Request.Form["NPOBAN"];
        //    }

        //    public static object ObjInvoice()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.mem_pw = mem_pw();
        //        EInvoice.InvoiceNo = InvoiceNo();
        //        EInvoice.BuyerId = BuyerId();
        //        EInvoice.BuyerName = BuyerName();
        //        EInvoice.BuyerPhoneNo = BuyerPhoneNo();
        //        EInvoice.BuyerAdd = BuyerAdd();
        //        EInvoice.BuyerEmail = BuyerEmail();
        //        EInvoice.TotalAmount = TotalAmount();
        //        EInvoice.SerialNumber = SerialNumber();
        //        EInvoice.OrderInfo = OrderInfo();
        //        EInvoice.SendCheck = send();
        //        EInvoice.CarrierType = CarrierType();
        //        EInvoice.CarrierId1 = CarrierId1();
        //        EInvoice.CarrierId2 = CarrierId2();
        //        EInvoice.NPOBAN = NPOBAN();
        //        return EInvoice.uploadInvoice();
        //    }
        //    #endregion

        //    #region cancelInvoice 發票作廢
        //    public static string Cancel()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.InvoiceNo = InvoiceNo();
        //        return EInvoice.cancelInvoice_I();
        //    }
        //    #endregion

        //    #region Check_invoice 以統編跟發票號碼
        //    public static string Check_invoice()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.InvoiceNo = InvoiceNo();
        //        return EInvoice.check_Invoice();
        //    }
        //    public static string Get_InvoiceURL()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.InvoiceNo = InvoiceNo();
        //        return EInvoice.get_InvoiceURL_I();
        //    }

        //    #endregion

        //    #region Check_invoice 以統編跟自訂編號
        //    public static string Check_invoiceOrder()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.SerialNumber = SerialNumber();
        //        return EInvoice.check_InvoiceOrder();
        //    }
        //    public static string Get_InvoiceURLOrder()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.SerialNumber = SerialNumber();
        //        return EInvoice.get_InvoiceURL_O();
        //    }

        //    #endregion



        //    public static string getAPI()
        //    {
        //        EInvoice.mem_cid = mem_cid();
        //        EInvoice.Description = Description();
        //        EInvoice.Amount = Amount();
        //        EInvoice.Quantity = Quantity();
        //        EInvoice.UnitPrice = UnitPrice();
        //        EInvoice.SerialNumber = SerialNumber();
        //        EInvoice.Remark = Remark();
        //        EInvoice.BuyerId = BuyerId();
        //        EInvoice.BuyerName = BuyerName();
        //        EInvoice.BuyerPhoneNo = BuyerPhoneNo();
        //        EInvoice.BuyerAdd = BuyerAdd();
        //        EInvoice.BuyerEmail = BuyerEmail();
        //        EInvoice.TotalAmount = TotalAmount();
        //        EInvoice.OrderInfo = OrderInfo();
        //        EInvoice.SendCheck = send();
        //        EInvoice.CarrierType = CarrierType();
        //        EInvoice.CarrierId1 = CarrierId1();
        //        EInvoice.CarrierId2 = CarrierId2();
        //        EInvoice.NPOBAN = NPOBAN();

        //        string apiurl = "http://localhost:58477/api/Invoice?mem_cid=" + EInvoice.mem_cid + "&Year=&period=&Description=" + EInvoice.Description + "&Amount=" + EInvoice.Amount + "&Quantity=" + EInvoice.Quantity + "&UnitPrice=" + EInvoice.UnitPrice + "&SerialNumber=" + EInvoice.SerialNumber + "&Remark=" + EInvoice.Remark + "&BuyerId=" + EInvoice.BuyerId + "&BuyerName=" + EInvoice.BuyerName + "&BuyerAdd=" + EInvoice.BuyerAdd + "&BuyerPhoneNo=" + EInvoice.BuyerPhoneNo + "&BuyerEmail=" + EInvoice.BuyerEmail + "&TotalAmount=" + EInvoice.TotalAmount + "&OrderInfo=" + EInvoice.OrderInfo + "&Send=" + EInvoice.Send + "&CarrierType=" + EInvoice.CarrierType + "&CarrierId1=" + EInvoice.CarrierId1 + "&CarrierId2=" + EInvoice.CarrierId2 + "&NPOBAN=" + EInvoice.NPOBAN;
        //        return apiurl;
        //    }
        //
        #endregion
    }
    public class NotifyApiController : ApiController
    {
        [Route("~/Notify")]
        public void Notify(FormDataCollection form)
        {
        }
    }
}
