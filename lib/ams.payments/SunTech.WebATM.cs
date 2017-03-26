using ams;
using ams.Data;
using ams.Models;
using System.Data;

namespace SunTech
{
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
}
