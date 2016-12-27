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

        void _Set(PaymentInfoApiController sender, arguments args, bool allow_null)
        {
            sender.ModelState.Validate(args._extdata2, nameof(TransactionPassword), allow_null: allow_null);
            sender.ModelState.Validate(args._extdata2, nameof(AuthorizePassword), allow_null: allow_null);
            sender.ModelState.Validate(args._extdata2, nameof(SubmitUrl), allow_null: allow_null);
        }
        void _Set(PaymentInfoApiController sender, arguments args, SqlBuilder sql, string flag)
        {
            sql[flag, "extdata"] = (args._extdata2 as JObject)?.ToString(Formatting.Indented);
        }
    }
    [PaymentInfo(PaymentType = PaymentType.SunTech_BuySafe)]
    public class SunTech_BuySafe : PaymentInfo_SunTech
    {
        //public override void tranApi_CreateData(PaymentTranArguments args, PaymentTranData data) { data.Amount1 = (int)data.Amount1; }
        public override void tranApi_CreateData(ams.tran2.MemberPaymentApiController controller, SqlBuilder sql) { sql["", nameof(controller.Amount1)] = (int)controller.Amount1; }
        //public override ForwardGameArguments tranApi_CreateForm(PaymentTranArguments args, PaymentTranData data)
        //{
        //    return new ForwardGameArguments()
        //    {
        //        ForwardType = ForwardType.FormPost,
        //        Url = this.SubmitUrl,
        //        Body = new Request()
        //        {
        //            web = this.MerhantId,
        //            MN = (int)data.Amount1,
        //            Password = this.TransactionPassword,
        //            OrderInfo = args.OrderInfo ?? "OrderInfo",
        //            Td = data.SerialNumber,
        //            //sna = data.UserName,
        //            //ChkValue = ChkValue((int)data.Amount1),
        //        }
        //    };
        //}
        public override ForwardGameArguments tranApi_CreateForm(ams.tran2.MemberPaymentApiController controller, ams.tran2.MemberPaymentApiController.Data data)
        {
            return new ForwardGameArguments()
            {
                ForwardType = ForwardType.FormPost,
                Url = this.SubmitUrl,
                Body = new Request()
                {
                    web = this.MerhantId,
                    MN = (int)data.Amount1,
                    Password = this.TransactionPassword,
                    OrderInfo = controller.OrderInfo ?? "OrderInfo",
                    Td = data.SerialNumber,
                    //sna = data.UserName,
                    //ChkValue = ChkValue((int)data.Amount1),
                }
            };
        }

        #region public class BuySafeRequest : SunTechRequest
        /// <summary>
        /// BuySafe 信用卡付款
        /// 1.2
        /// </summary>
        public class Request : SunTechRequest
        {
            /// <summary>
            /// 交易類別
            /// </summary>
            /// <remarks>空白 or 0 信用卡交易、1 銀聯卡交易。</remarks>
            public int? Card_Type { get; set; }

            /// <summary>
            /// 語言類別
            /// </summary>
            /// <remarks>空白 or EN(英文)、JIS(日文)。</remarks>
            public string Country_Type { get; set; }

            /// <summary>
            /// 分期期數
            /// </summary>
            /// <remarks>空白 or 3、6、12、24 (Card_Type 必須為 0) (非聯信收單不可帶值)</remarks>
            public int? Term { get; set; }
        }
        #endregion

        #region public class BuySafeResponse : SunTechResponse
        /// <summary>
        /// BuySafe 信用卡付款
        /// 1.4
        /// </summary>
        public class Response : SunTechResponse
        {
            /// <summary>
            /// 商家網站名稱
            /// </summary>
            public string webname { get; set; }

            /// <summary>
            /// 消費者姓名
            /// </summary>
            /// <remarks>因個資法會轉換為隱藏。例：王○明</remarks>
            public string Name { get; set; }

            /// <summary>
            /// 交易授權碼
            /// </summary>
            /// <remarks>非信用卡交易則空白。</remarks>
            public string ApproveCode { get; set; }

            /// <summary>
            /// 授權卡號後 4 碼 
            /// </summary>
            /// <remarks>非信用卡交易則空白。</remarks>
            public int? Card_NO { get; set; }

            /// <summary>
            /// 交易類別
            /// </summary>
            /// <remarks>0 信用卡交易、1 銀聯卡交易。</remarks>
            public int? Card_Type { get; set; }
        }
        #endregion
    }
    [PaymentInfo(PaymentType = PaymentType.SunTech_WebATM)]
    public class SunTech_WebATM : PaymentInfo_SunTech
    {
        //public override void tranApi_CreateData(PaymentTranArguments args, PaymentTranData data) { data.Amount1 = (int)data.Amount1; }
        public override void tranApi_CreateData(ams.tran2.MemberPaymentApiController controller, SqlBuilder sql) { sql["", nameof(controller.Amount1)] = (int)controller.Amount1; }
        //public override ForwardGameArguments tranApi_CreateForm(PaymentTranArguments args, PaymentTranData data)
        //{
        //    return new ForwardGameArguments()
        //    {
        //        ForwardType = ForwardType.FormPost,
        //        Url = this.SubmitUrl,
        //        Body = new Request()
        //        {
        //            web = this.MerhantId,
        //            MN = (int)data.Amount1,
        //            Password = this.TransactionPassword,
        //            OrderInfo = args.OrderInfo ?? "OrderInfo",
        //            Td = data.SerialNumber,
        //            //sna = data.UserName,
        //            //ChkValue = ChkValue((int)data.Amount1),
        //        }
        //    };
        //}
        public override ForwardGameArguments tranApi_CreateForm(ams.tran2.MemberPaymentApiController controller, ams.tran2.MemberPaymentApiController.Data data)
        {
            return new ForwardGameArguments()
            {
                ForwardType = ForwardType.FormPost,
                Url = this.SubmitUrl,
                Body = new Request()
                {
                    web = this.MerhantId,
                    MN = (int)data.Amount1,
                    Password = this.TransactionPassword,
                    OrderInfo = controller.OrderInfo ?? "OrderInfo",
                    Td = data.SerialNumber,
                    //sna = data.UserName,
                    //ChkValue = ChkValue((int)data.Amount1),
                }
            };
        }

        #region public class WebATMRequest : SunTechRequest
        /// <summary>
        /// Web_ATM 即時付
        /// 2.2
        /// </summary>
        public class Request : SunTechRequest
        {
            /// <summary>
            /// 消費者電話
            /// </summary>
            /// <remarks>空白 or 純數字。</remarks>
            /// <length>20</length>
            public string sdt { get; set; }
        }
        #endregion

        #region public class WebATMResponse : SunTechResponse
        /// <summary>
        /// Web_ATM 即時付
        /// 2.4
        /// </summary>
        public class Response : SunTechResponse
        {
            /// <summary>
            /// 商家網站名稱
            /// </summary>
            public string webname { get; set; }

            /// <summary>
            /// 消費者姓名
            /// </summary>
            /// <remarks>因個資法會轉換為隱藏。例：王○明</remarks>
            public string Name { get; set; }
        }
        #endregion
    }
    [PaymentInfo(PaymentType = PaymentType.SunTech_PayCode)]
    public class SunTech_PayCode : PaymentInfo_SunTech
    {
        //public override void tranApi_CreateData(PaymentTranArguments args, PaymentTranData data)
        //{
        //    throw new NotImplementedException();
        //}
        public override void tranApi_CreateData(ams.tran2.MemberPaymentApiController controller, SqlBuilder sql)
        {
            throw new NotImplementedException();
        }
        //public override ForwardGameArguments tranApi_CreateForm(PaymentTranArguments args, PaymentTranData data)
        //{
        //    throw new NotImplementedException();
        //}
        public override ForwardGameArguments tranApi_CreateForm(ams.tran2.MemberPaymentApiController controller, ams.tran2.MemberPaymentApiController.Data data)
        {
            throw new NotImplementedException();
        }

        #region public class PayCodeRequest : SunTechRequest
        /// <summary>
        /// PayCode 超商代碼繳費
        /// 4.2
        /// </summary>
        public class Request : SunTechRequest
        {
            /// <summary>
            /// 消費者電話
            /// </summary>
            /// <remarks>空白 or 純數字。</remarks>
            /// <length>20</length>
            public string sdt { get; set; }

            /// <summary>
            /// *繳費期限
            /// </summary>
            /// <remarks>格式：YYYYMMDD，繳款期限最長 180 天。</remarks>
            /// <length>8</length>
            public DateTime DueDate { get; set; }

            /// <summary>
            /// 用戶編號
            /// </summary>
            /// <remarks>不可有特殊字元。包含：*'<>[]”
            /// 本欄位可供商家自行定義。例：會員編號…</remarks>
            /// <length>15</length>
            public string UserNo { get; set; }

            /// <summary>
            /// 列帳日期
            /// </summary>
            /// <remarks>商家自行定義帳單產生日。格式：YYYYMMDD。</remarks>
            /// <length>8</length>
            public DateTime? BillDate { get; set; }
        }
        #endregion

        #region public class PayCodeResponse1 : SunTechResponse
        /// <summary>
        /// PayCode 超商代碼繳費-即時回傳 
        /// 4.4.1
        /// </summary>
        public class Response1 : SunTechResponse
        {
            /// <summary>
            /// 用戶編號
            /// </summary>
            public string UserNo { get; set; }

            /// <summary>
            /// 繳款代碼
            /// </summary>
            public string paycode { get; set; }

            /// <summary>
            /// 可繳款超商
            /// </summary>
            /// <remarks>4：全家超商、5：統一超商、6：OK 超商、7：萊爾富超商。使用逗點符號分隔，例：4,6,7 →表示統一超商不可繳費。</remarks>
            public string PayType { get; set; }
        }
        #endregion

        #region public class PayCodeResponse2 : SunTechResponse
        /// <summary>
        /// PayCode 超商代碼繳費-離線回傳
        /// 4.4.2
        /// </summary>
        public class Response2 : SunTechResponse
        {
            /// <summary>
            /// 用戶編號
            /// </summary>
            public string UserNo { get; set; }

            /// <summary>
            /// 繳款日期
            /// </summary>
            /// <remarks>格式：YYYYMMDD。</remarks>
            public DateTime PayDate { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <remarks>4：全家超商繳款、5：統一超商繳款、6：OK 超商繳款、7：萊爾富超商繳款。</remarks>
            public int PayType { get; set; }
        }
        #endregion
    }
    [PaymentInfo(PaymentType = PaymentType.SunTech_24Payment)]
    public class SunTech_24Payment : PaymentInfo_SunTech
    {
        //public override void tranApi_CreateData(PaymentTranArguments args, PaymentTranData data)
        //{
        //    throw new NotImplementedException();
        //}
        public override void tranApi_CreateData(ams.tran2.MemberPaymentApiController controller, SqlBuilder sql)
        {
            throw new NotImplementedException();
        }
        //public override ForwardGameArguments tranApi_CreateForm(PaymentTranArguments args, PaymentTranData data)
        //{
        //    throw new NotImplementedException();
        //}
        public override ForwardGameArguments tranApi_CreateForm(ams.tran2.MemberPaymentApiController controller, ams.tran2.MemberPaymentApiController.Data data)
        {
            throw new NotImplementedException();
        }

        #region public class _24PayRequest : SunTechRequest
        /// <summary>
        /// 超商代收 24Pay
        /// 3.2
        /// </summary>
        public class Request : SunTechRequest
        {
            /// <summary>
            /// 消費者電話
            /// </summary>
            /// <remarks>空白 or 純數字。</remarks>
            /// <length>20</length>
            public string sdt { get; set; }

            /// <summary>
            /// *繳費期限
            /// </summary>
            /// <remarks>繳款期限最長 180 天。(YYYYMMDD)</remarks>
            /// <length>8</length>
            public DateTime DueDate { get; set; }

            /// <summary>
            /// 用戶編號
            /// </summary>
            /// <remarks>不可有特殊字元。包含：*'<>[]”
            /// 本欄位可供商家自行定義。例：會員編號…</remarks>
            /// <length>15</length>
            public string UserNo { get; set; }

            /// <summary>
            /// 列帳日期
            /// </summary>
            /// <remarks>商家自行定義帳單產生日。(YYYYMMDD)</remarks>
            /// <length>8</length>
            public DateTime? BillDate { get; set; }

            /// <summary>
            /// *產品名稱
            /// </summary>
            /// <remarks>每項產品名稱最多 100 個字元，不可有特殊字元。包含：*'<>[]”
            /// 如有負值商家.建議整合為單一產品傳送。</remarks>
            /// <length>100</length>
            public string ProductName1 { get; set; }
            public string ProductName2 { get; set; }
            public string ProductName3 { get; set; }
            public string ProductName4 { get; set; }
            public string ProductName5 { get; set; }
            public string ProductName6 { get; set; }
            public string ProductName7 { get; set; }
            public string ProductName8 { get; set; }
            public string ProductName9 { get; set; }
            public string ProductName10 { get; set; }

            /// <summary>
            /// *產品單價
            /// </summary>
            /// <remarks>每項單價>0 元；≦9999999。
            /// 如有負值商家.建議整合為單一產品傳送。</remarks>
            /// <length>8</length>
            public int ProductPrice1 { get; set; }
            public int ProductPrice2 { get; set; }
            public int ProductPrice3 { get; set; }
            public int ProductPrice4 { get; set; }
            public int ProductPrice5 { get; set; }
            public int ProductPrice6 { get; set; }
            public int ProductPrice7 { get; set; }
            public int ProductPrice8 { get; set; }
            public int ProductPrice9 { get; set; }
            public int ProductPrice10 { get; set; }

            /// <summary>
            /// *產品數量
            /// </summary>
            /// <length>5</length>
            /// <remarks>每項數量>0；≦99999。</remarks>
            public int ProductQuantity1 { get; set; }
            public int ProductQuantity2 { get; set; }
            public int ProductQuantity3 { get; set; }
            public int ProductQuantity4 { get; set; }
            public int ProductQuantity5 { get; set; }
            public int ProductQuantity6 { get; set; }
            public int ProductQuantity7 { get; set; }
            public int ProductQuantity8 { get; set; }
            public int ProductQuantity9 { get; set; }
            public int ProductQuantity10 { get; set; }
        }
        #endregion

        #region public class _24PayResponse1 : SunTechResponse
        /// <summary>
        /// 超商代收 24Pay - 即時回傳 
        /// 3.4.1
        /// </summary>
        public class Response1 : SunTechResponse
        {
            /// <summary>
            /// 用戶編號
            /// </summary>
            public string UserNo { get; set; }

            /// <summary>
            /// 超商第一段條碼
            /// </summary>
            public string BarcodeA { get; set; }

            /// <summary>
            /// 超商第二段條碼
            /// </summary>
            public string BarcodeB { get; set; }

            /// <summary>
            /// 超商第三段條碼
            /// </summary>
            public string BarcodeC { get; set; }

            /// <summary>
            /// 郵局一段條碼
            /// </summary>
            public string PostBarcodeA { get; set; }

            /// <summary>
            /// 郵局二段條碼
            /// </summary>
            public string PostBarcodeB { get; set; }

            /// <summary>
            /// 郵局三段條碼
            /// </summary>
            public string PostBarcodeC { get; set; }

            /// <summary>
            /// 虛擬帳號
            /// </summary>
            /// <remarks>金額大於 3 則銀行臨櫃匯款。</remarks>
            public string EntityATM { get; set; }
        }
        #endregion

        #region public class _24PayResponse2 : SunTechResponse
        /// <summary>
        /// 超商代收 24Pay - 離線回傳 
        /// 3.4.2
        /// </summary>
        public class Response2 : SunTechResponse
        {
            /// <summary>
            /// 消費者姓名
            /// </summary>
            /// <remarks>因個資法會轉換為隱藏。例：王○明</remarks>
            public string Name { get; set; }

            /// <summary>
            /// 用戶編號
            /// </summary>
            public string UserNo { get; set; }

            /// <summary>
            /// 繳款日期
            /// </summary>
            /// <remarks>格式：YYYYMMDD。</remarks>
            public DateTime PayDate { get; set; }

            /// <summary>
            /// 繳款方式
            /// </summary>
            /// <remarks>1= 超商條碼繳款、2= 郵局條碼繳款、3= 虛擬帳號繳款。</remarks>
            public string PayType { get; set; }
        }
        #endregion
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
            get { return $"{web}{Password}{MN}".SHA1Hex(Encoding.ASCII); }
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
            get { return $"{web}{Password}{MN}{buysafeno}{errcode}".SHA1Hex(Encoding.ASCII); }
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
    //    public static string 交易密碼 = "88888888";

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