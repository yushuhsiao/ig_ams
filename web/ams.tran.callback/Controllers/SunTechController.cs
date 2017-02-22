using ams.Controllers;
using ams.Data;
using Newtonsoft.Json;
using SunTech;
using System;
using System.Text;
using System.Web;

namespace ams
{
    using System.Collections.Specialized;
    using System.Net;
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

        NameValueCollection build_form(NameValueCollection form, tran2.MemberPaymentApiController.Data data, bool success)
        {
            NameValueCollection result = new NameValueCollection();
            foreach (var key in form.AllKeys)
            {
                if (string.Compare(key, "web", true) == 0) { continue; }
                if (string.Compare(key, "ChkValue", true) == 0) { continue; }
                string value = form[key].Trim(true);
                if (value == null) { continue; }
                result.Add(key, value);
            }
            if (data == null)
            {
                result["Success"] = false.ToString().ToLower();
            }
            else
            {
                result["TranID"] = data.TranID.ToString();
                result["SerialNumber"] = data.SerialNumber;
                result["UserName"] = data.UserName;
                result["RequestTime"] = string.Concat(data.RequestTime.ToUniversalTime().ToString("s"), "Z");
                result["Amount1"] = data.Amount1.ToString();
                result["Success"] = success.ToString().ToLower();
            }
            return result;
        }

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
                            data.FormData = build_form(Request.Form, data, success);
                            if (!string.IsNullOrEmpty(data.NotifyUrl))
                                //return View(data);
                                Global.PostHttpRequest(data.NotifyUrl, data.FormData.ToQueryString());
                            return new HttpStatusCodeResult(HttpStatusCode.OK);
                        }
                    }
                    else if (msg.SendType == SendType.網頁傳送)
                    {
                        ams.tran2.MemberPaymentApiController.Data data;
                        if (new ams.tran2.MemberPaymentApiController().try_proc_in(out data, pp, null, msg.Td, success, null))
                        {
                            data.FormData = build_form(Request.Form, data, success);
                            var resultType = data.ResultType ?? tran2.ResultType.FormPost;
                            if (resultType == tran2.ResultType.FormPost)
                                return View("Result", data);
                            else if (resultType == tran2.ResultType.Redirect)
                                return Redirect($"{data.ResultUrl}?{data.FormData.ToQueryString()}");
                        }
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            //return View("Result");
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