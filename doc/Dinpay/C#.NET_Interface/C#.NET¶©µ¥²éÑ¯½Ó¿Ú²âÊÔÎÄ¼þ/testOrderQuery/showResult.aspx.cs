using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace testOrderQuery
{
    public partial class showResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string m_orderid = "";
            string m_oamount = "";
            string m_odate = "";
            string payStatus = "";
            string msg = "";

            try
            {
                string OrderInfo = Request.Form["OrderData"].ToString();
                string signMsg = Request.Form["Sign"].ToString();
                string Code = Request.Form["Code"].ToString();
                string key = Request.Form["Key"].ToString();

                if (Code == "08")
                {
                    string newMD5text = FormsAuthentication.HashPasswordForStoringInConfigFile(OrderInfo + key, "md5").Trim().ToUpper();

                    if (newMD5text == signMsg)
                    {
                        //分割字符串
                        char c1 = (char)1;//这种特殊字符键盘是打不出来的
                        OrderInfo = OrderInfo.Replace("|", c1.ToString());

                        string[] arr = OrderInfo.Split(c1);

                        m_orderid = arr[0];
                        m_oamount = arr[1];
                        m_odate = arr[2];
                        payStatus = arr[3];

                        if (payStatus == "2")
                        {
                            payStatus = "支付成功";
                        }
                        else
                        {
                            if (payStatus == "3")
                            {
                                payStatus = "支付失败";
                            }
                            else
                            {
                                payStatus = "未支付";
                            }
                        }

                    }
                    else
                    {
                        msg = "认证失败!!!";
                    }
                }
                else
                {
                    msg = "查询失败，错误代码Code=" + Code;
                }
            }
            finally
            {
            }

            orderid.Value = m_orderid;
            amount.Value = m_oamount;
            date.Value = m_odate;
            status.Value = payStatus;
            msgs.Value = msg;
        }
    }
}