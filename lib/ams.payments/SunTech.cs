using ams;
using ams.Controllers;
using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using arguments = ams.Controllers.PaymentInfoApiController.arguments;
using ams.tran2;

namespace SunTech
{
    /// <summary>
    /// 紅陽科技線上金流系統
    /// </summary>
    /// <see cref="https://test.esafe.com.tw/"/>
    /// <see cref="https://www.esafe.com.tw/"/>
    public abstract class PaymentInfo_SunTech : PaymentInfo
    {
        /// <summary>
        /// 交易密碼 (紅陽科技->商家專區->修改密碼->修改其他密碼)
        /// </summary>
        [JsonProperty]
        public string TransactionPassword;
        /// <summary>
        /// 授權密碼 (紅陽科技->商家專區->修改密碼->修改其他密碼)
        /// </summary>
        [JsonProperty]
        public string AuthorizePassword;
        [JsonProperty]
        public string SubmitUrl;

        internal override void Add(PaymentInfoApiController sender, arguments args) => _Set(sender, args, false);
        internal override void Set(PaymentInfoApiController sender, arguments args) => _Set(sender, args, true);
        internal override void Add(PaymentInfoApiController sender, arguments args, SqlBuilder sql) => _Set(sender, args, sql, "u");
        internal override void Set(PaymentInfoApiController sender, arguments args, SqlBuilder sql) => _Set(sender, args, sql, "nu");

        /// <summary>
        /// Password 與 Url 的設定
        /// </summary>
        void _Set(PaymentInfoApiController sender, arguments args, bool allow_null)
        {
            sender.ModelState.Validate(args._extdata2, nameof(TransactionPassword), allow_null: allow_null);
            sender.ModelState.Validate(args._extdata2, nameof(AuthorizePassword), allow_null: allow_null);
            sender.ModelState.Validate(args._extdata2, nameof(SubmitUrl), allow_null: allow_null);
        }
        /// <summary>
        /// Password 與 Url 寫入 PaymentAccount 的 extdata 欄位
        /// </summary>
        void _Set(PaymentInfoApiController sender, arguments args, SqlBuilder sql, string flag)
        {
            sql[flag, "extdata"] = (args._extdata2 as JObject)?.ToString(Formatting.Indented);
        }
    }

    #region public abstract class SunTechRequest
    public abstract class SunTechRequest
    {
        /// <summary>
        /// *商店代號
        /// </summary>
        public string web { get; set; }

        /// <summary>
        /// *交易金額
        /// </summary>
        /// <remarks>不可有小數點和千位符號。(新台幣)</remarks>
        /// <length>8</length>
        public int MN { get; set; }

        /// <summary>
        /// *交易內容
        /// </summary>
        /// <remarks>不可有特殊字元。包含：*'<>[]”</remarks>
        /// <length>400</length>
        public string OrderInfo { get; set; }

        /// <summary>
        /// 商家訂單編號
        /// </summary>
        /// <remarks>商家訂單編號：紅陽交易系統僅阻擋同 Td 在同瀏覽器上的未完成交易。</remarks>
        /// <length>20</length>
        public string Td { get; set; }

        /// <summary>
        /// 消費者姓名
        /// </summary>
        /// <remarks>不可有特殊字元。包含：*'<>[]”</remarks>
        /// <length>30</length>
        public string sna { get; set; }

        /// <summary>
        /// 消費者 Email 
        /// </summary>
        /// <remarks>空白 or 符合 Email 格式內容。交易成功時會發送成功訊息給消費者。</remarks>
        /// <length>100</length>
        public string email { get; set; }

        /// <summary>
        /// 備註1
        /// </summary>
        /// <remarks>不可有特殊字元。包含：*'<>[]”</remarks>
        /// <length>400</length>
        public string note1 { get; set; }

        /// <summary>
        /// 備註2
        /// </summary>
        /// <remarks>不可有特殊字元。包含：*'<>[]”</remarks>
        /// <length>400</length>
        public string note2 { get; set; }

        /// <summary>
        /// *交易檢查碼
        /// </summary>
        /// <remarks>交易檢查碼組合順序，並使用「SHA-1 雜湊函數」取得組合字串的雜湊值(轉大寫)。</remarks>
        public string ChkValue //{ get; set; }
        {
            get { return $"{web}{Password}{MN}".SHA1Hex(Encoding.UTF8).ToUpper(); }
        }
        [JsonIgnore]
        public string Password;
    }
    #endregion

    #region public abstract class SunTechResponse
    public abstract class SunTechResponse
    {
        /// <summary>
        /// 紅陽訂單編號
        /// </summary>
        public string buysafeno { get; set; }

        /// <summary>
        /// 商店代號
        /// </summary>
        public string web { get; set; }

        /// <summary>
        /// 商家訂單編號
        /// </summary>
        public string Td { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int MN { get; set; }

        /// <summary>
        /// 備註1
        /// </summary>
        public string note1 { get; set; }

        /// <summary>
        /// 備註2
        /// </summary>
        public string note2 { get; set; }

        /// <summary>
        /// 回覆代碼
        /// </summary>
        /// <remarks>00(數字)表交易成功。其餘交易失敗！請搭配交易檢查碼進行交易驗證。</remarks>
        public string errcode { get; set; }

        /// <summary>
        /// 回覆代碼解釋
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 交易檢查碼
        /// </summary>
        /// <remarks>交易檢查碼組合順序，並使用「SHA-1 雜湊函數」取得組合字串的雜湊值(轉大寫)。</remarks>
        public string ChkValue { get; set; }

        /// <summary>
        /// 傳送方式
        /// </summary>
        /// <remarks>1：背景傳送；2：網頁傳送。商家需可接受 1-2 次的回傳(非重複交易)。</remarks>
        public SendType? SendType { get; set; }

        public string ChkValue_Local
        {
            get { return $"{web}{Password}{buysafeno}{MN}{errcode}".SHA1Hex(Encoding.UTF8).ToUpper(); }
        }

        [JsonIgnore]
        public string Password;

        public bool ChkValue_Verified
        {
            get { return ChkValue == ChkValue_Local; }
        }
    }
    #endregion

    public enum SendType : int
    {
        背景傳送 = 1,
        網頁傳送 = 2,
    }

    static class helpers
    {
    }
    //public class SunTechApiController : ApiController
    //{
    //    const string Url1 = "https://www.esafe.com.tw/Service/Etopm.aspx";
    //    const string Url2 = "https://test.esafe.com.tw/Service/Etopm.aspx";
    //    public const string Url = Url2;
    //    public static string 交易密碼 = "ig168ig168";

    //    public static string ChkValue(string web, int MN)
    //    {
    //        return $"{web}{SunTechApiController.交易密碼}{MN}".SHA1Hex(Encoding.ASCII);
    //    }



        //    /// <summary>
        //    /// PayCode 繳款成功
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/PayCode/Success")]
        //    public IHttpActionResult PayCode_Success() { return Ok(); }
        //    /// <summary>
        //    /// PayCode 交易完成
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/PayCode/Finish")]
        //    public IHttpActionResult PayCode_Finish() { return Ok(); }
        //    /// <summary>
        //    /// PayCode 交易回傳確認
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/PayCode/Confirm")]
        //    public IHttpActionResult PayCode_Confirm() { return Ok(); }



        //    /// <summary>
        //    /// WebATM 交易成功
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/WebATM/Success")]
        //    public IHttpActionResult WebATM_Success() { return Ok(); }
        //    /// <summary>
        //    /// WebATM 交易失敗
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/WebATM/Failed")]
        //    public IHttpActionResult WebATM_Failed() { return Ok(); }
        //    /// <summary>
        //    /// WebATM 交易回傳確認
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/WebATM/Confirm")]
        //    public IHttpActionResult WebATM_Confirm() { return Ok(); }



        //    /// <summary>
        //    /// 24Payment 繳款成功
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/24Payment/Success")]
        //    public IHttpActionResult _24Payment_Success() { return Ok(); }
        //    /// <summary>
        //    /// 24Payment 交易完成
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/24Payment/Finish")]
        //    public IHttpActionResult _24Payment_Finish() { return Ok(); }
        //    /// <summary>
        //    /// 24Payment 交易回傳確認
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/24Payment/Confirm")]
        //    public IHttpActionResult _24Payment_Confirm() { return Ok(); }



        //    /// <summary>
        //    /// BuySafe 交易成功
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/BuySafe/Success")]
        //    public IHttpActionResult BuySafe_Success() { return Ok(); }
        //    /// <summary>
        //    /// BuySafe 交易失敗
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/BuySafe/Failed")]
        //    public IHttpActionResult BuySafe_Failed() { return Ok(); }
        //    /// <summary>
        //    /// BuySafe 交易回傳確認
        //    /// </summary>
        //    [HttpPost, Route("~/SunTech/BuySafe/Confirm")]
        //    public IHttpActionResult BuySafe_Confirm() { return Ok(); }
        //}
    }