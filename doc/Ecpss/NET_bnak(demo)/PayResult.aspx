<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="UTF-8" %>
<%@ Import Namespace="System.Security.Cryptography" %>

<%
    String MD5key = "LHTG]{SK";									//MD5加密码(测试用)
    String BillNo = System.Web.HttpContext.Current.Request.Params["BillNo"].ToString();		//订单号
    String Amount =	System.Web.HttpContext.Current.Request.Params["Amount"].ToString();	//交易金额
    String Succeed = System.Web.HttpContext.Current.Request.Params["Succeed"].ToString();	//支付状态:该值说明见于word说明文档[商户根据该值来修改数据库中相应订单的状态]
    String Result = System.Web.HttpContext.Current.Request.Params["Result"].ToString();		//支付结果 (是支付状态的文字说明)
    String SignMD5info = System.Web.HttpContext.Current.Request.Params["SignMD5info"].ToString();	//验证返回码(调试时使用)


    String md5src = BillNo+"&"+Amount+"&"+Succeed+"&"+MD5key;				//对数据进行加密验证

    String md5sign;
    md5sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5src, "MD5");
%>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title>ASP.net_receive</title>
</head>
<body>
<!-- 请加上你们网站的框架。就是你们网站的头部top，左部left等。还有字体等你们都要做调整。 -->

 <%
 if (SignMD5info==md5sign){
 %>
 <!-- MD5验证成功 -->
	<table width="728" border="0" cellspacing="0" cellpadding="0" align="center">
  <tr>
    <td  align="right" valign="top" width="200">Your order number is：</td>
    <td  align="left" valign="top"><%= BillNo%></td>
  </tr>
    <tr>
    <td  align="right" valign="top">Amount：</td>
    <td  align="left" valign="top"><%= Amount%></td>
  </tr>
    <tr>
    <td  align="right" valign="top">Payment result：</td>
	<%if (Succeed=="88" ) 
	{
	%><!-- 可修改订单状态为正在付款中 -->
	<!-- 提交支付信息成功，返回绿色的提示信息 -->
	<td  align="left" valign="top" style="color:green;"><%= Result%></td>
	<%
	}
	else
	{
	%><!-- 提交支付信息失败，返回红色的提示信息 -->
    <td  align="left" valign="top" style="color:red;"><%= Result%>&nbsp;&nbsp;&nbsp;&nbsp;<%= Succeed%></td>
	<%
	}%>
  </tr>
  
</table>
<%
}
else 
{
%>
 <!-- MD5验证失败 -->
<table width="728" border="0" cellspacing="0" cellpadding="0" align="center">
 <tr>
    <td  align="center" valign="top" style="color:red;">Validation failed!</td>
	</tr>
	</table>
<%	
}
 %>
 </body>
</html>