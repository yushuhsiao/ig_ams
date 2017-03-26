using ams;
using ams.Controllers;
using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using arguments = ams.Controllers.PaymentInfoApiController.arguments;
using ams.tran2;

using ams.EInvoice;

namespace PayNow
{
    /// <summary>
    /// 電子發票系統
    /// </summary>
    public class Invoice_PayNow 
    {
        /// <summary>
        /// 商家統編 商家帳號
        /// 測試帳號 70828783
        /// 正式帳號 24810043
        /// </summary>
        [JsonProperty]
        public string mem_cid { get; set; }
        /// <summary>
        /// 商家密碼  
        /// 測試密碼 70828783
        /// 正式密碼 27838280
        /// </summary>
        
        [JsonProperty]
        public string mem_pw { get; set; }

        [JsonProperty]        

        public string InvoiceNo { get; set; }

        public object ObjInvoiceBody { get; set; }
        
        public object ObjInvoice { get; set; }
        
        public string Year { get; set; }

        public string period { get; set; }

        public string Description { get; set; }

        public int Amount { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }

        public string SerialNumber { get; set; }

        public string Remark { get; set; }

        public string BuyerId { get; set; }

        public string BuyerName { get; set; }

        public string BuyerAdd { get; set; }

        public string BuyerPhoneNo { get; set; }

        public string BuyerEmail { get; set; }

        public int TotalAmount { get; set; }

        public string OrderInfo { get; set; }

        public bool Send { get; set; }

        public int SendCheck { get; set; }

        public string CarrierType { get; set; }

        public string CarrierId1 { get; set; }

        public string CarrierId2 { get; set; }

        public string NPOBAN { get; set; }

    }    
}
