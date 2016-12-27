using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace testOrderQuery
{
    public partial class getXmlData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                M_ID.Value = Request.Form["M_ID"].ToString();                  //<----商家号-------->
                MODate.Value = Request.Form["MODate"].ToString();              //<---不可以为空的--->
                MOrderID.Value = Request.Form["MOrderID"].ToString();          //<----定单号-------->
                Key.Value = Request.Form["Key"].ToString();

                string OrderInfo = M_ID.Value + "|" + MOrderID.Value + "|" + MODate.Value;
                QueryMessage.Value = OrderInfo;
                string digestInfo = FormsAuthentication.HashPasswordForStoringInConfigFile(OrderInfo + Key.Value, "md5").Trim().ToUpper();
                Digest.Value = digestInfo;

            }
            finally
            {
            }
        }
    }
}