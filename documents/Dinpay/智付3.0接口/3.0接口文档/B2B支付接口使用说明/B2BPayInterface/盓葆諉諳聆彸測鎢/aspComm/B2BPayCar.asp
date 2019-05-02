<%
flag = false
sb = ""

bank_code = trim(request("bank_code"))
if bank_code <> "" then
	sb = sb&"bank_code="&bank_code
	flag=true
end if

client_ip = trim(request("client_ip"))
if client_ip <> "" then
        if flag=true then
            sb = sb&"&client_ip="&client_ip
        else
            sb = sb&"client_ip="&client_ip
        end if
	flag=true
end if

extend_param = trim(request("extend_param"))
if extend_param <> "" then
        if flag=true then
            sb = sb&"&extend_param="&extend_param
        else
            sb = sb&"extend_param="&extend_param
        end if
	flag=true
end if

extra_return_param = trim(request("extra_return_param"))
if extra_return_param <> "" then
        if flag=true then
            sb = sb&"&extra_return_param="&extra_return_param
        else
            sb = sb&"extra_return_param="&extra_return_param
        end if
	flag=true
end if

input_charset = trim(request("input_charset"))
if input_charset <> "" then
        if flag=true then
            sb = sb&"&input_charset="&input_charset
        else
            sb = sb&"input_charset="&input_charset
        end if
	flag=true
end if

interface_version = trim(request("interface_version"))
if interface_version <> "" then
	sb = sb&"&interface_version="&interface_version
	flag=true
end if

merchant_code = trim(request("merchant_code"))
if merchant_code <> "" then
	sb = sb&"&merchant_code="&merchant_code
	flag=true
end if

notify_url = trim(request("notify_url"))
if notify_url <> "" then
	sb = sb&"&notify_url="&notify_url
	flag=true
end if

order_amount = trim(request("order_amount"))
if order_amount <> "" then
	sb = sb&"&order_amount="&order_amount
	flag=true
end if

order_no = trim(request("order_no"))
if order_no <> "" then
	sb = sb&"&order_no="&order_no
	flag=true
end if

order_time = trim(request("order_time"))
if order_time <> "" then
	sb = sb&"&order_time="&order_time
	flag=true
end if

product_code = trim(request("product_code"))
if product_code <> "" then
	sb = sb&"&product_code="&product_code
	flag=true
end if

product_desc = trim(request("product_desc"))
if product_desc <> "" then
	sb = sb&"&product_desc="&product_desc
	flag=true
end if

product_name = trim(request("product_name"))
if product_name <> "" then
	sb = sb&"&product_name="&product_name
	flag=true
end if

product_num = trim(request("product_num"))
if product_num <> "" then
	sb = sb&"&product_num="&product_num
	flag=true
end if

return_url = trim(request("return_url"))
if return_url <> "" then
	sb = sb&"&return_url="&return_url
	flag=true
end if

service_type = trim(request("service_type"))
if service_type <> "" then
	sb = sb&"&service_type="&service_type
	flag=true
end if

show_url = trim(request("show_url"))
if show_url <> "" then
	sb = sb&"&show_url="&show_url
	flag=true
end if

sign_type = trim(request("sign_type"))

key = "123456789a123456789_"
sb = sb&"&key={"&key&"}"

'创建NPS安全组件
Set NPSMAC = Server.CreateObject("NPSDigest.NPSSecurity")

'签证
NPSMAC.OrderInfo = sb
sign = NPSMAC.GetDigest
'Response.Write "sb=" & sb&"<br>"
'Response.Write "sign=" & sign&"<br>"

%>
<html>
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>模拟商户提交</title>
  </head>
  <body onLoad="document.form1.submit();">
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
