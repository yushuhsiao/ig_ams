using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using ams.EInvoice;

namespace PayNow
{
    public class InvoiceController : ApiController
    {
        static Invoice_PayNow Invoice = new Invoice_PayNow();
        PayNowEInvoice EInvoice = new PayNowEInvoice();

        // 參考 Models.Invoice
        Invoice_PayNow[] InvoiceRequest = new Invoice_PayNow[]
        {
            //依照樣式取得 字串  如何接收  如何給我這些參數
            //new InvoiceRequest { mem_cid="70828783", Year="2017", period="1", Description="test", Amount=100, Quantity=1, UnitPrice=1, SerialNumber="AB12345678", BuyerId="", BuyerName="TEST", BuyerAdd="taichung", BuyerPhoneNo="0912345678", BuyerEmail="test@mail.cc", TotalAmount=100, OrderInfo="test", Send=0, CarrierType="", CarrierId1="", CarrierId2="", NPOBAN="" }
            new Invoice_PayNow { mem_cid="", Year="", period="", Description="", Amount=0, Quantity=0, UnitPrice=0, SerialNumber="", BuyerId="", BuyerName="", BuyerAdd="", BuyerPhoneNo="", BuyerEmail="", TotalAmount=0, OrderInfo="", Send = false, CarrierType="", CarrierId1="", CarrierId2="", NPOBAN="" }
        };

        public string ReturnStr;

        public string MonthStr = string.Format("{0:00}", DateTime.Now.Month);

        //密碼
        public string mem_pw = "70828783";

        public object ObjInvoiceBody;

        public object ObjInvoice;

        public IEnumerable<Invoice_PayNow> Get(string mem_cid, string Year, string period, string Description, int Amount, int Quantity, int UnitPrice, string SerialNumber, string Remark, string BuyerId, string BuyerName, string BuyerAdd, string BuyerPhoneNo, string BuyerEmail, int TotalAmount, string OrderInfo, bool Send, string CarrierType, string CarrierId1, string CarrierId2, string NPOBAN)
        {
            // 將資料帶入
            Invoice.mem_cid = mem_cid;
            Invoice.Description = Description;
            Invoice.Amount = Amount;
            Invoice.Quantity = Quantity;
            Invoice.UnitPrice = UnitPrice;
            Invoice.SerialNumber = SerialNumber;
            Invoice.BuyerId = BuyerId;
            Invoice.BuyerName = BuyerName;
            Invoice.BuyerAdd = BuyerAdd;
            Invoice.BuyerPhoneNo = BuyerPhoneNo;
            Invoice.BuyerEmail = BuyerEmail;
            Invoice.TotalAmount = TotalAmount;
            Invoice.SerialNumber = SerialNumber;
            Invoice.Remark = Remark;
            Invoice.OrderInfo = OrderInfo;
            Invoice.Send = Send;
            Invoice.CarrierType = CarrierType;
            Invoice.CarrierId1 = CarrierId1;
            Invoice.CarrierId2 = CarrierId2;
            Invoice.NPOBAN = NPOBAN;


            //開立發票
            Invoice.Year = string.Format("{0:0000}", DateTime.Now.Year);

            MonthSet();
            InvoiceRequest[0].mem_cid = Invoice.mem_cid;
            InvoiceRequest[0].Year = Invoice.Year;
            InvoiceRequest[0].period = Invoice.period;

            string InvoiceNo = EInvoice.GetInvoiceNumber(Invoice.mem_cid, Invoice.Year, Invoice.period);
            InvoiceRequest[0].InvoiceNo = InvoiceNo;
            Invoice.InvoiceNo = InvoiceNo;

            //發票明細上傳
            InvoiceRequest[0].Description = Invoice.Description;
            InvoiceRequest[0].Amount = Invoice.Amount;
            InvoiceRequest[0].Quantity = Invoice.Quantity;
            InvoiceRequest[0].UnitPrice = Invoice.UnitPrice;
            InvoiceRequest[0].SerialNumber = Invoice.SerialNumber;
            InvoiceRequest[0].Remark = Invoice.Remark;

            ObjInvoiceBody = EInvoice.UploadInvoice_Body(Invoice.InvoiceNo, Invoice.Description, Convert.ToString(Invoice.Amount), Convert.ToString(Invoice.Quantity), Convert.ToString(Invoice.UnitPrice), Invoice.mem_cid, Invoice.SerialNumber, Invoice.Remark);

            //發票資訊上傳
            mem_pw = TripleDESEncoding(mem_pw);
            InvoiceRequest[0].BuyerId = Invoice.BuyerId;
            InvoiceRequest[0].BuyerName = Invoice.BuyerName;
            InvoiceRequest[0].BuyerAdd = Invoice.BuyerAdd;
            InvoiceRequest[0].BuyerPhoneNo = Invoice.BuyerPhoneNo;
            InvoiceRequest[0].BuyerEmail = Invoice.BuyerEmail;
            InvoiceRequest[0].TotalAmount = Invoice.TotalAmount;
            InvoiceRequest[0].Send = Invoice.Send;
            InvoiceRequest[0].CarrierType = Invoice.CarrierType;
            InvoiceRequest[0].CarrierId1 = Invoice.CarrierId1;
            InvoiceRequest[0].CarrierId2 = Invoice.CarrierId2;
            InvoiceRequest[0].NPOBAN = Invoice.NPOBAN;

            ObjInvoice = EInvoice.UploadInvoice(Invoice.mem_cid, mem_pw, Invoice.InvoiceNo, Invoice.BuyerId, Invoice.BuyerName, Invoice.BuyerAdd, Invoice.BuyerPhoneNo, Invoice.BuyerEmail, Convert.ToString(Invoice.TotalAmount), Invoice.SerialNumber, Send, Invoice.CarrierType, Invoice.CarrierId1, Invoice.CarrierId2, Invoice.NPOBAN);

            // 回傳整個Json 格式要的東西
            return InvoiceRequest;
        }

        //發票期數判別
        public void MonthSet()
        {
            switch (MonthStr)
            {
                case "01":
                case "02": Invoice.period = "0"; break;
                case "03":
                case "04": Invoice.period = "1"; break;
                case "05":
                case "06": Invoice.period = "2"; break;
                case "07":
                case "08": Invoice.period = "3"; break;
                case "09":
                case "10": Invoice.period = "4"; break;
                case "11":
                case "12": Invoice.period = "5"; break;
            }
        }

        //加密 mem_pw
        private string TripleDESEncoding(string p_strInfomation)
        {
            Byte[] data = Encoding.UTF8.GetBytes(p_strInfomation);
            TripleDES tdes = TripleDES.Create();

            tdes.IV = Encoding.UTF8.GetBytes("12345678");
            tdes.Key = Encoding.UTF8.GetBytes("1234567890" + mem_pw + "123456"); //{1234567890}+{商家密碼}+{123456}
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.Zeros;
            ICryptoTransform ict = tdes.CreateEncryptor();
            Byte[] enc = ict.TransformFinalBlock(data, 0, data.Length);
            ReturnStr = Convert.ToBase64String(enc);
            ReturnStr = ReturnStr.Replace(" ", "+");
            return ReturnStr;
        }
    }
}
