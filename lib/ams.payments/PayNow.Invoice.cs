using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ams.EInvoice;
using System.Security.Cryptography;

namespace PayNow
{
    public class Invoice : Invoice_PayNow
    {
        PayNowEInvoice EInvoice = new PayNowEInvoice();

        public string MonthStr = string.Format("{0:00}", DateTime.Now.Month);
        public string Year = string.Format("{0:0000}", DateTime.Now.Year);

        private string ReturnStr;
        private string Url;

        public void MonthSet()
        {
            switch (MonthStr)
            {
                case "01":
                case "02": period = "0"; break;
                case "03":
                case "04": period = "1"; break;
                case "05":
                case "06": period = "2"; break;
                case "07":
                case "08": period = "3"; break;
                case "09":
                case "10": period = "4"; break;
                case "11":
                case "12": period = "5"; break;
            }
        }

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

        public string getInvoice()
        {
            MonthSet();
            InvoiceNo = EInvoice.GetInvoiceNumber(mem_cid, Year, period);
            /*
            if (InvoiceNo.Substring(0, 2) == "F_")
            {
                throw new Exception(InvoiceNo.Substring(2, InvoiceNo.Length - 2));
            }
            */
            return InvoiceNo;
        }

        public object uploadInvoiceBody()
        {

            ObjInvoiceBody = EInvoice.UploadInvoice_Body(InvoiceNo, Description, Convert.ToString(Amount), Convert.ToString(Quantity), Convert.ToString(UnitPrice), mem_cid, SerialNumber, Remark);
            /*
            if (ObjInvoiceBody.ReturnStatus == false)
            {
                throw new Exception(ObjInvoiceBody.ErrorMsg);
            }
            */
            return ObjInvoiceBody;
        }
       
        public object uploadInvoice()
        {
            ////test
            //MonthSet();
            //InvoiceNo = EInvoice.GetInvoiceNumber(mem_cid, Year, period);
            //ObjInvoiceBody = EInvoice.UploadInvoice_Body(InvoiceNo, Description, Convert.ToString(Amount), Convert.ToString(Quantity), Convert.ToString(UnitPrice), mem_cid, SerialNumber, Remark);
            //ObjInvoiceBody = EInvoice.UploadInvoice_Body(InvoiceNo, "儲值", "100", "1", "100", mem_cid, SerialNumber, Remark);

            mem_pw = TripleDESEncoding(mem_pw);
            Send = (SendCheck == 0) ? false : true;
            ObjInvoice = EInvoice.UploadInvoice(mem_cid, mem_pw, InvoiceNo, BuyerId, BuyerName, BuyerAdd, BuyerPhoneNo, BuyerEmail, Convert.ToString(TotalAmount), SerialNumber, Send, CarrierType, CarrierId1, CarrierId2, NPOBAN);

            return ObjInvoice;
        }

        #region cancelInvoice_I 發票作廢
        public string cancelInvoice_I()
        {
            ReturnStr = EInvoice.CancelInvoice_I(mem_cid, InvoiceNo);

            return ReturnStr;
        }
        #endregion

        #region check_Invoice 發票查詢
        /// <summary>
        /// 以發票號碼查詢
        /// </summary>
        /// <returns></returns>
        public string check_Invoice()
        {
            ReturnStr = EInvoice.Check_invoice(mem_cid, InvoiceNo);
            return ReturnStr;
        }
        
        public string get_InvoiceURL_I()
        {
            Url = EInvoice.Get_InvoiceURL_I(mem_cid, InvoiceNo);
            return Url;
        }


        /// <summary>
        /// 以商家自訂編號查詢
        /// </summary>
        /// <returns></returns>
        public string check_InvoiceOrder()
        {
            ReturnStr = EInvoice.Check_invoiceOrder(mem_cid, SerialNumber);
            return ReturnStr;
        }
              
        public string get_InvoiceURL_O()
        {
            Url = EInvoice.Get_InvoiceURL_O(mem_cid, SerialNumber);
            return Url;
        }

        #endregion
    }
}
