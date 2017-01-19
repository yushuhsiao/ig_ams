using ams;
using ams.Data;
using ams.Models;
using System;
using System.Data;

namespace SunTech
{
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
}
