using ams;
using ams.Data;
using ams.Models;
using System.Data;

namespace SunTech
{
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
        /// <summary>
        /// 交易基本資料
        /// </summary>
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
}
