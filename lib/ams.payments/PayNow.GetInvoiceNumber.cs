using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ams.EInvoice;

namespace PayNow
{
    /// [Invoice] 
    /// <summary>
    /// 實作方式
    /// </summary>
    public class GetInvoiceNumber : Invoice_PayNow
    {
        public string MonthStr = string.Format("{0:00}", DateTime.Now.Month);
        public string Year = string.Format("{0:0000}", DateTime.Now.Year);
        public string period, InvoiceNo;

        public void Month()
        {
            switch (MonthStr)
            {
                case "1" :
                case "2" : period = "0"; break;
                case "3" : 
                case "4" : period = "1"; break;
                case "5" : 
                case "6" : period = "2"; break;
                case "7" : 
                case "8" : period = "3"; break;
                case "9" : 
                case "10": period = "4"; break;
                case "11": 
                case "12": period = "5"; break;
            }            
        }
        public void set()
        {
            InvoiceNo = PayNowEInvoice.GetInvoiceNumber(mem_cid, Year, period);

            if (InvoiceNo.Substring(0, 2) == "F_")
            {
                throw new Exception(InvoiceNo.Substring(2, InvoiceNo.Length - 2));
            }
            
        }
        
        
    }

    #region public class Request : GetInvoiceNumber
    ///// <summary>
    ///// 取得發票號碼
    ///// </summary>
    //public class Request : GetInvoiceNumber
    //{        
        
    //}
    #endregion

    #region public class Response : GetInvoiceNumberResponse
    ///// <summary>
    ///// 回傳發票號碼
    ///// </summary>
    //public class Response : GetInvoiceNumberResponse
    //{

    //}
    #endregion
}
