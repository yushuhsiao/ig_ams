using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSsharp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          try{
            bool flag = false;
            string sb = "";

            string bank_code1 = Request.Form["bank_code"].ToString().Trim();
            if (bank_code1 != ""){
                sb = sb + "bank_code=" + bank_code1;
	            flag = true;
            }

            string client_ip1 = Request.Form["client_ip"].ToString().Trim();
            if (client_ip1 != ""){
                if(flag){
                    sb = sb + "&client_ip=" + client_ip1;
                }else{
                    sb = sb + "client_ip=" + client_ip1;
                }
	            flag = true;
            }

            string extend_param1 = Request.Form["extend_param"].ToString().Trim();
            if (extend_param1 != ""){
                if(flag){
                    sb = sb + "&extend_param=" + extend_param1;
                }else{
                    sb = sb + "extend_param=" + extend_param1;
                }
	            flag = true;
            }

            string extra_return_param1 = Request.Form["extra_return_param"].ToString().Trim();
            if (extra_return_param1 != ""){
                if(flag){
                    sb = sb + "&extra_return_param=" + extra_return_param1;
                }else{
                    sb = sb + "extra_return_param=" + extra_return_param1;
                }
	            flag = true;
            }
	        

            string input_charset1 = Request.Form["input_charset"].ToString().Trim();
            if (input_charset1 != ""){
                if(flag){
                    sb = sb + "&input_charset=" + input_charset1;
                }else{
                    sb = sb + "input_charset=" + input_charset1;
                }
	            flag = true;
            }

            string interface_version1 = Request.Form["interface_version"].ToString().Trim();
            if (interface_version1 != ""){
                sb = sb + "&interface_version=" + interface_version1;
	            flag = true;
            }


            string merchant_code1 = Request.Form["merchant_code"].ToString().Trim();
            if (merchant_code1 != ""){
                sb = sb + "&merchant_code=" + merchant_code1;
	            flag = true;
            }

            string notify_url1 = Request.Form["notify_url"].ToString().Trim();
            if (notify_url1 != ""){
                sb = sb + "&notify_url=" + notify_url1;
	            flag = true;
            }

            string order_amount1 = Request.Form["order_amount"].ToString().Trim();
            if (order_amount1 != ""){
                sb = sb + "&order_amount=" + order_amount1;
	            flag = true;
            }

            string order_no1 = Request.Form["order_no"].ToString().Trim();
            if (order_no1 != ""){
                sb = sb + "&order_no=" + order_no1;
	            flag = true;
            }

            string order_time1 = Request.Form["order_time"].ToString().Trim();
            if (order_time1 != ""){
                sb = sb + "&order_time=" + order_time1;
	            flag = true;
            }

            string product_code1 = Request.Form["product_code"].ToString().Trim();
            if (product_code1 != ""){
                sb = sb + "&product_code=" + product_code1;
	            flag = true;
            }

            string product_desc1 = Request.Form["product_desc"].ToString().Trim();
            if (product_desc1 != ""){
                sb = sb + "&product_desc=" + product_desc1;
	            flag = true;
            }


            string product_name1 = Request.Form["product_name"].ToString().Trim();
            if (product_name1 != ""){
                sb = sb + "&product_name=" + product_name1;
	            flag = true;
            }

            string product_num1 = Request.Form["product_num"].ToString().Trim();
            if (product_num1 != ""){
                sb = sb + "&product_num=" + product_num1;
	            flag = true;
            }

            string return_url1 = Request.Form["return_url"].ToString().Trim();
            if (return_url1 != ""){
                sb = sb + "&return_url=" + return_url1;
	            flag = true;
            }

            string service_type1 = Request.Form["service_type"].ToString().Trim();
            if (service_type1 != ""){
                sb = sb + "&service_type=" + service_type1;
	            flag = true;
            }

            string show_url1 = Request.Form["show_url"].ToString().Trim();
            if (show_url1 != ""){
                sb = sb + "&show_url=" + show_url1;
	            flag = true;
            }

            string sign_type1 = Request.Form["sign_type"].ToString().Trim();

            string key = "123456789a123456789_";
            sb = sb + "&key={" + key + "}";

            string sign1 = FormsAuthentication.HashPasswordForStoringInConfigFile(sb, "md5").Trim().ToUpper();

            
            service_type.Value = service_type1;
            merchant_code.Value = merchant_code1;
            input_charset.Value = input_charset1;
            notify_url.Value = notify_url1;
            return_url.Value = return_url1;
            client_ip.Value = client_ip1;
            interface_version.Value = interface_version1;
            sign_type.Value = sign_type1;
            order_no.Value = order_no1;
            order_time.Value = order_time1;
            order_amount.Value = order_amount1;
            product_name.Value = product_name1;
            show_url.Value = show_url1;
            product_code.Value = product_code1;
            product_num.Value = product_num1;
            product_desc.Value = product_desc1;
            bank_code.Value = bank_code1;
            extra_return_param.Value = extra_return_param1;
            extend_param.Value = extend_param1;
            sign.Value = sign1;


           }
            finally
            {

            }

        }
    }
}