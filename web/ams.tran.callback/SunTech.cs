using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SunTech
{
    public class SunTechApiController : ApiController
    {
        //const string Url1 = "https://www.esafe.com.tw/Service/Etopm.aspx";
        //const string Url2 = "https://test.esafe.com.tw/Service/Etopm.aspx";
        //public const string Url = Url2;
        //public static string 交易密碼 = "88888888";

        //public static string ChkValue(string web, int MN)
        //{
        //    return $"{web}{SunTechApiController.交易密碼}{MN}".SHA1Hex(Encoding.ASCII);
        //}



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
        public IHttpActionResult PayCode_Confirm() { return Ok(); }



        /// <summary>
        /// WebATM 交易成功
        /// </summary>
        [HttpPost, Route("~/SunTech/WebATM/Success")]
        public IHttpActionResult WebATM_Success() { return Ok(); }
        /// <summary>
        /// WebATM 交易失敗
        /// </summary>
        [HttpPost, Route("~/SunTech/WebATM/Failed")]
        public IHttpActionResult WebATM_Failed() { return Ok(); }
        /// <summary>
        /// WebATM 交易回傳確認
        /// </summary>
        [HttpPost, Route("~/SunTech/WebATM/Confirm")]
        public IHttpActionResult WebATM_Confirm() { return Ok(); }



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
        public IHttpActionResult _24Payment_Confirm() { return Ok(); }



        /// <summary>
        /// BuySafe 交易成功
        /// </summary>
        [HttpPost, Route("~/SunTech/BuySafe/Success")]
        public IHttpActionResult BuySafe_Success() { return Ok(); }
        /// <summary>
        /// BuySafe 交易失敗
        /// </summary>
        [HttpPost, Route("~/SunTech/BuySafe/Failed")]
        public IHttpActionResult BuySafe_Failed() { return Ok(); }
        /// <summary>
        /// BuySafe 交易回傳確認
        /// </summary>
        [HttpPost, Route("~/SunTech/BuySafe/Confirm")]
        public IHttpActionResult BuySafe_Confirm() { return Ok(); }
    }
}