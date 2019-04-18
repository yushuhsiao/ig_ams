<%@ page import="java.util.*" contentType="text/html; charset=UTF-8"%>
<%@ page import="com.eitop.platform.tools.encrypt.*" %>
<%
boolean flag=false;
StringBuffer sb=new StringBuffer();
String bank_code=request.getParameter("bank_code").trim().toUpperCase();
if(!(bank_code==null||bank_code.equals("")))
{
	sb.append("bank_code="+bank_code);
	flag=true;
}
String client_ip=request.getParameter("client_ip");
if(!(client_ip==null||client_ip.equals("")))
{
	if(flag)
    {
     sb.append("&client_ip="+client_ip);
    }
     else
	{
		sb.append("client_ip="+client_ip);
	}
	flag=true;
}
String extend_param=request.getParameter("extend_param");
if(!(extend_param==null||extend_param.equals("")))
{
	if(flag)
	{
		sb.append("&extend_param="+extend_param);
	}
	else
	{
		sb.append("extend_param="+extend_param);
	}
	flag=true;
}
String extra_return_param=request.getParameter("extra_return_param");
if(!(extra_return_param==null||extra_return_param.equals("")))
{
	if(flag)
	{
		sb.append("&extra_return_param="+extra_return_param);
	}
	else
	{
		sb.append("extra_return_param="+extra_return_param);
	}
	flag=true;
}
String input_charset=request.getParameter("input_charset");
if(!(input_charset==null||input_charset.equals("")))
{
	if(flag)
	{
		sb.append("&input_charset="+input_charset);
	}
	else
	{
		sb.append("input_charset="+input_charset);
	}
	flag=true;
}
String interface_version=request.getParameter("interface_version");
if(!(interface_version==null||interface_version.equals("")))
{
	sb.append("&interface_version="+interface_version);
}

String merchant_code=request.getParameter("merchant_code");

if(!(merchant_code==null||merchant_code.equals("")))
{
	sb.append("&merchant_code="+merchant_code);
}

String notify_url=request.getParameter("notify_url");

if(!(notify_url==null||notify_url.equals("")))
{
	sb.append("&notify_url="+notify_url);
}
String order_amount=request.getParameter("order_amount");
if(!(order_amount==null||order_amount.equals("")))
{
	sb.append("&order_amount="+order_amount);
}

String order_no=request.getParameter("order_no");
if(!(order_no==null||order_no.equals("")))
{
	sb.append("&order_no="+order_no);
}
String order_time=request.getParameter("order_time");
if(!(order_time==null||order_time.equals("")))
{
	sb.append("&order_time="+order_time);
}
String product_code=request.getParameter("product_code");
if(!(product_code==null||product_code.equals("")))
{
  sb.append("&product_code="+product_code);
}
String product_desc=request.getParameter("product_desc");
if(!(product_desc==null||product_desc.equals("")))
{
  sb.append("&product_desc="+product_desc);
}
String product_name=request.getParameter("product_name");
if(!(product_name==null||product_name.equals("")))
{
	sb.append("&product_name="+product_name);
}
String product_num=request.getParameter("product_num");
if(!(product_num==null||product_num.equals("")))
{
  sb.append("&product_num="+product_num);
}
String return_url=request.getParameter("return_url");
if(!(return_url==null||return_url.equals("")))
{
  sb.append("&return_url="+return_url);
}
String service_type=request.getParameter("service_type");
if(!(service_type==null||service_type.equals("")))
{
	sb.append("&service_type="+service_type);
}
String show_url=request.getParameter("show_url");
if(!(show_url==null||show_url.equals("")))
{
  sb.append("&show_url="+show_url);
}
String sign_type=request.getParameter("sign_type");

String key="123456789a123456789_";
sb.append("&key={"+key+"}");

String signmsg=sb.toString();
String sign=MD5Digest.encrypt(signmsg);
%>
<html>
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>模拟商户提交</title>
  </head>
  <body onload="document.form1.submit()">
    <form action="https://payment.dinpay.com/B2BReceiveMerchantAction.do" name="form1" method="POST">

            <input type="hidden" name="service_type" size="40" value="<%=service_type %>" />

            <input type="hidden" name="merchant_code" size="40" value="<%=merchant_code %>" />

            <input type="hidden" name="input_charset" size="40" value="<%=input_charset %>" />

            <input type="hidden" name="notify_url" size="40" value="<%=notify_url %>" />

            <input type="hidden" name="return_url" size="40" value="<%=return_url %>" />

            <input type="hidden" name="client_ip" size="40" value="<%=client_ip %>" />

            <input type="hidden" name="interface_version" size="40" value="<%=interface_version %>" />

            <input type="hidden" name="sign_type" size="40" value="<%=sign_type %>" />

            <input type="hidden" name="order_no" size="40" value="<%=order_no %>" />

            <input type="hidden" name="order_time" size="40" value="<%=order_time %>" />

            <input type="hidden" name="order_amount" size="40" value="<%=order_amount %>" />

            <input type="hidden" name="product_name" value="<%=product_name %>" size="40" />

            <input type="hidden" name="show_url" size="40" value="<%=show_url %>" />

            <input type="hidden" name="product_code" size="40" value="<%=product_code %>" />

            <input type="hidden" name="product_num" size="40" value="<%=product_num%>" />

            <input type="hidden" name="product_desc" size="40" value="<%=product_desc %>" />

            <input type="hidden" name="bank_code" size="40" value="<%=bank_code %>" />

            <input type="hidden" name="extra_return_param" size="40"value="<%=extra_return_param %>" />

            <input type="hidden" name="extend_param" size="40" value="<%=extend_param%>" />

            <input type="hidden" name="sign" size="" value="<%=sign%>" />
    </form>

  </body>
</html>
