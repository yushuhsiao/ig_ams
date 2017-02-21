using ams.Controllers;
using ams.Data;
using Newtonsoft.Json;
using SunTech;
using System.Web;

namespace ams
{
    using System.Web.Mvc;
    public class SunTechController : Controller
    {
        //private ActionResult __Result<TPaymentInfo, TResponse>(TResponse msg)
        //    where TPaymentInfo : PaymentInfo_SunTech
        //    where TResponse : SunTechResponse
        //{
        //    _HttpContext context = _HttpContext.Current;
        //    foreach (CorpInfo c in CorpInfo.Cache.Value)
        //    {
        //        foreach (PaymentInfo p in PaymentInfo.Cache[c.ID].Value)
        //        {
        //            TPaymentInfo pp = p as TPaymentInfo;
        //            if (pp == null) continue;
        //            bool success = msg.errcode == "00";
        //            if (msg.SendType == SendType.背景傳送)
        //            {
        //                PaymentTranData data;
        //                if (new PaymentTranApiController().try_proc_in(out data, pp, null, msg.Td, success, () => new TranCert()
        //                {
        //                    TranID = null,
        //                    SerialNumber = msg.Td,
        //                    data1 = context.GetFormData().ToString(Formatting.Indented),
        //                    data2 = context.ReadFormBody(),
        //                }))
        //                {
        //                    if (string.IsNullOrEmpty(data.NotifyUrl))
        //                        return View();
        //                    Global.PostHttpRequest(data.NotifyUrl, "");
        //                }
        //            }
        //            else if (msg.SendType == SendType.網頁傳送)
        //            {
        //                PaymentTranData data;
        //                if (new PaymentTranApiController().try_proc_in(out data, pp, null, msg.Td, success, null))
        //                    return View("Result", data);
        //            }
        //        }
        //    }
        //    return View("Result");
        //}
        private ActionResult _Result<TPaymentInfo, TResponse>(TResponse msg)
            where TPaymentInfo : PaymentInfo_SunTech
            where TResponse : SunTechResponse
        {
            _HttpContext context = _HttpContext.Current;
            foreach (CorpInfo c in CorpInfo.Cache.Value)
            {
                foreach (PaymentInfo p in PaymentInfo.Cache[c.ID].Value)
                {
                    TPaymentInfo pp = p as TPaymentInfo;
                    if (pp == null) continue;
                    bool success = msg.errcode == "00";
                    if (msg.SendType == SendType.背景傳送)
                    {
                        ams.tran2.MemberPaymentApiController.Data data;
                        if (new ams.tran2.MemberPaymentApiController().try_proc_in(out data, pp, null, msg.Td, success, () => new TranCert()
                        {
                            TranID = null,
                            SerialNumber = msg.Td,
                            data1 = context.GetFormData().ToString(Formatting.Indented),
                            data2 = context.ReadFormBody(),
                        }))
                        {
                            if (string.IsNullOrEmpty(data.NotifyUrl))
                                return View();
                            Global.PostHttpRequest(data.NotifyUrl, "TranID=" + data.TranID + "&SerialNumber=" + data.SerialNumber + "&UserName=" + data.UserName + "&RequestTime=" + data.RequestTime + "&Amount1=" + data.Amount1 + "&Success=" + data.Finished );
                        }
                    }
                    else if (msg.SendType == SendType.網頁傳送)
                    {
                        ams.tran2.MemberPaymentApiController.Data data;
                        if (new ams.tran2.MemberPaymentApiController().try_proc_in(out data, pp, null, msg.Td, success, null))
                        {
                            if (data.ResultType == tran2.ResultType.FormPost)
                                return View("Result", data);
                            else if (data.ResultType == tran2.ResultType.Redirect)
                            {
                                string redirect = Server.UrlEncode("?TranID=" + data.TranID + "&SerialNumber=" + data.SerialNumber + "&UserName=" + data.UserName + "&RequestTime=" + data.RequestTime + "&Amount1=" + data.Amount1 + "&Success=" + data.Finished );  // TODO : convert data to query string
                                Response.Redirect($"{data.ResultUrl}{redirect}");
                            }
                        }
                    }
                }
            }
            return View("Result");
        }

        /// <summary>
        /// BuySafe 交易成功
        /// </summary>
        [Route("~/SunTech/BuySafe/Success")]
        /// <summary>
        /// BuySafe 交易失敗
        /// </summary>
        [Route("~/SunTech/BuySafe/Failed")]
        [HttpPost]
        public ActionResult BuySafe_Result(SunTech_BuySafe.Response msg) => _Result<SunTech_BuySafe, SunTech_BuySafe.Response>(msg);

        /// <summary>
        /// WebATM 交易成功
        /// </summary>
        [Route("~/SunTech/WebATM/Success")]
        /// <summary>
        /// WebATM 交易失敗
        /// </summary>
        [Route("~/SunTech/WebATM/Failed")]
        [HttpPost]
        public ActionResult WebATM_Result(SunTech_WebATM.Response msg) => _Result<SunTech_WebATM, SunTech_WebATM.Response>(msg);
    }
}
namespace ams
{
    using System.Web.Http;
    public class SunTachApiController : ApiController
    {
        /// <summary>
        /// BuySafe 交易回傳確認
        /// </summary>
        [HttpPost, Route("~/SunTech/BuySafe/Confirm")]
        public IHttpActionResult BuySafe_Confirm() { return Ok("0000"); }



        /// <summary>
        /// WebATM 交易回傳確認
        /// </summary>
        [HttpPost, Route("~/SunTech/WebATM/Confirm")]
        public IHttpActionResult WebATM_Confirm() { return Ok("0000"); }



        /// <summary>
        /// PayCode 繳款成功
        /// </summary>
        [HttpPost, Route("~/SunTech/PayCode/Success")]
        public IHttpActionResult PayCode_Success() { return Ok(); }
        /// <summary>
        /// PayCode 交易完成
        /// </summary>
        [HttpPost, Route("~/SunTech/PayCode/Finish")]
        public IHttpActionResult PayCode_Finish() { return Ok(); }
        /// <summary>
        /// PayCode 交易回傳確認
        /// </summary>
        [HttpPost, Route("~/SunTech/PayCode/Confirm")]
        public IHttpActionResult PayCode_Confirm() { return Ok("0000"); }



        /// <summary>
        /// 24Payment 繳款成功
        /// </summary>
        [HttpPost, Route("~/SunTech/24Payment/Success")]
        public IHttpActionResult _24Payment_Success() { return Ok(); }
        /// <summary>
        /// 24Payment 交易完成
        /// </summary>
        [HttpPost, Route("~/SunTech/24Payment/Finish")]
        public IHttpActionResult _24Payment_Finish() { return Ok(); }
        /// <summary>
        /// 24Payment 交易回傳確認
        /// </summary>
        [HttpPost, Route("~/SunTech/24Payment/Confirm")]
        public IHttpActionResult _24Payment_Confirm() { return Ok("0000"); }
    }
}