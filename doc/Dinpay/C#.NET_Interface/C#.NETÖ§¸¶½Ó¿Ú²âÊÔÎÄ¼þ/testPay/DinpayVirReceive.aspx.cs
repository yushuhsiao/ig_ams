using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DinPayC
{
    public partial class DinpayVirReceive : System.Web.UI.Page
    {
        public string Key = "123456789a123456789_";  //<--支付密钥--> 注:此处密钥必须与商家后台里的密钥一致

        protected void Page_Load(object sender, EventArgs e)
        {
            string ResponseText ="";
            string m_id ="";
            string m_orderid ="";
            string m_oamount ="";
            string m_ocurrency ="";
            string m_url ="";
            string m_language ="";
            string s_name ="";
            string s_addr ="";
            string s_postcode ="";
            string s_tel ="";
            string s_eml ="";
            string r_name ="";
            string r_addr ="";
            string r_postcode ="";
            string r_eml ="";
            string r_tel ="";
            string m_ocomment ="";
            string m_status ="";
            string modate ="";
            string newmd5info ="";
            try
            {
                m_id = Request.Form["m_id"].ToString();
                m_orderid = Request.Form["m_orderid"].ToString();
                m_oamount = Request.Form["m_oamount"].ToString();
                m_ocurrency = Request.Form["m_ocurrency"].ToString();
                m_url = Request.Form["m_url"].ToString();
                m_language = Request.Form["m_language"].ToString();
                s_name = Request.Form["s_name"].ToString();
                s_addr = Request.Form["s_addr"].ToString();
                s_postcode = Request.Form["s_postcode"].ToString();
                s_tel = Request.Form["s_tel"].ToString();
                s_eml = Request.Form["s_eml"].ToString();
                r_name = Request.Form["r_name"].ToString();
                r_addr = Request.Form["r_addr"].ToString();
                r_postcode = Request.Form["r_postcode"].ToString();
                r_eml = Request.Form["r_eml"].ToString();
                r_tel = Request.Form["r_tel"].ToString();
                m_ocomment = Request.Form["m_ocomment"].ToString();
                m_status = Request.Form["m_status"].ToString();
                modate = Request.Form["modate"].ToString();
                newmd5info = Request.Form["newmd5info"].ToString();
    
                if(newmd5info=="")
                {
                    ResponseText = "认证签名为空值";
                }
                else
                {
                    string newOrderMessage = m_id+m_orderid+m_oamount+Key+m_status;
                    string newMD5text=FormsAuthentication.HashPasswordForStoringInConfigFile(newOrderMessage,"md5").Trim().ToUpper();
                    string Upnewmd5info = newmd5info.ToUpper();
                    if (newMD5text != Upnewmd5info)
                    {
                        ResponseText = "认证失败!!!";
                    }
                    else
                    {
                        if (m_status == "2")
                        {
                            ResponseText += "认证成功，可以更新数据库!!!<br>";
                            ResponseText += "m_id =" + m_id + "<br>";
                            ResponseText += "m_orderid =" + m_orderid + "<br>";
                            ResponseText += "m_oamount =" + m_oamount + "<br>";
                            ResponseText += "m_ocurrency =" + m_ocurrency + "<br>";
                            ResponseText += "m_language =" + m_language + "<br>";
                            ResponseText += "s_name =" + s_name + "<br>";
                            ResponseText += "s_addr =" + s_addr + "<br>";
                            ResponseText += "s_postcode =" + s_postcode + "<br>";
                            ResponseText += "s_tel =" + s_tel + "<br>";
                            ResponseText += "s_eml =" + s_eml + "<br>";
                            ResponseText += "r_name =" + r_name + "<br>";
                            ResponseText += "r_addr =" + r_addr + "<br>";
                            ResponseText += "r_postcode =" + r_postcode + "<br>";
                            ResponseText += "r_eml =" + r_eml + "<br>";
                            ResponseText += "r_tel =" + r_tel + "<br>";
                            ResponseText += "m_ocomment =" + m_ocomment + "<br>";
                            ResponseText += "m_status =" + m_status + "<br>";
                            ResponseText += "modate =" + modate + "<br>";
                            ResponseText += "newmd5info=" + newmd5info + "<br>";
                        }
                        else
                        {
                            ResponseText = "支付失败!!!";
                        }
                    }

                }
            }
            finally
            {
            }
        
        }
    }
}