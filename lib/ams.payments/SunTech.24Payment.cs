using ams;
using ams.Data;
using ams.Models;
using System;
using System.Data;

namespace SunTech
{
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
}
