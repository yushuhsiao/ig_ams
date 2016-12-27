using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DinPayC
{
    public partial class DinpayAspVir : System.Web.UI.Page
    {
        public string Key = "123456789a123456789_";  //<--支付密钥--> 注:此处密钥必须与商家后台里的密钥一致

        protected void Page_Load(object sender, EventArgs e)
        {
            DinpayAspVirForm.Action = "https://payment.dinpay.com/VirReceiveMerchantAction.do";
            try
            {
                M_ID.Value = Request.Form["M_ID"].ToString();                  //<----商家号-------->
                MODate.Value = Request.Form["MODate"].ToString();              //<---不可以为空的--->
                MOrderID.Value = Request.Form["MOrderID"].ToString();          //<----定单号-------->
                MOAmount.Value = Request.Form["MOAmount"].ToString();          //<----定单金额------> 
                MOCurrency.Value = Request.Form["MOCurrency"].ToString();      //<----币种-默认为1---->
                M_URL.Value = Request.Form["M_URL"].ToString();                //<--返回地址-此项默认为空不起作用-->
                M_Language.Value = Request.Form["M_Language"].ToString();
                S_Name.Value = Request.Form["S_Name"].ToString();
                S_Address.Value = Request.Form["S_Address"].ToString();
                S_PostCode.Value = Request.Form["S_PostCode"].ToString();
                S_Telephone.Value = Request.Form["S_Telephone"].ToString();
                S_Email.Value = Request.Form["S_Email"].ToString();
                R_Name.Value = Request.Form["R_Name"].ToString();
                R_Address.Value = Request.Form["R_Address"].ToString();
                R_PostCode.Value = Request.Form["R_PostCode"].ToString();
                R_Telephone.Value = Request.Form["R_Telephone"].ToString();
                R_Email.Value = Request.Form["R_Email"].ToString();
                MOComment.Value = Request.Form["MOComment"].ToString();
                State.Value = Request.Form["State"].ToString();

                string OrderMessage = M_ID.Value + MOrderID.Value + MOAmount.Value + MOCurrency.Value + M_URL.Value + M_Language.Value + S_PostCode.Value + S_Telephone.Value + S_Email.Value + R_PostCode.Value + R_Telephone.Value + R_Email.Value + MODate.Value + Key;
                //Response.Write("串起来的订单信息为：" + OrderMessage + "<br>");
                string digest = FormsAuthentication.HashPasswordForStoringInConfigFile(OrderMessage, "md5").Trim().ToUpper();
                //Response.Write("加密认证为：" + digest);
                digestinfo.Value = digest;

            }
            finally
            {
            }
        }
    }
}