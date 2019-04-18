<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="UTF-8" %>
<%@ Import Namespace="System.Security.Cryptography" %>

<%
    String MD5key;        //md5key
    MD5key = "12345678"; //

    String MerNo;        //商户号
    MerNo = "10005";	 //()

    String BillNo;        //[]商户网店订单号
    BillNo = "000001111123";


    String Amount;        //交易金额
    Amount = "0.01";

    String OrderDesc;	//[ѡ]
       OrderDesc="";


    String ReturnURL;    //[]返回地址
    ReturnURL = "http://localhost/PayResult.aspx";

	String AdviceURL; 
	AdviceURL="";  // '[必填]支付完成后，后台接收支付结果，可用来更新数据库值

    String md5src;        //加密
    md5src=MerNo+"&"+BillNo+"&"+Amount+"&"+ReturnURL+"&"+MD5key


    String  SignInfo;        //[]MD5
    SignInfo = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5src, "MD5");

    String Remark;        //备注
    Remark = "";
	
	String products;
    products="products info"; //------------------物品信息



	String defaultBankNumber;	//[选填]银行代码
	defaultBankNumber="";

    String orderTime;    //[必填]交易时间yyyyMMddHHmmss
	orderTime="";  

%>

<html>
<head>
    <title>send.net</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
</head>
<body>
<form METHOD="post" action="https://pay.ecpss.cn/sslpayment" name="pay">
<table >
    <tr>
  <td><input name="OrderDesc" type="hidden" value="<%=OrderDesc%>"></td></tr>
    <tr>
  <td><input name="Remark" type="hidden" value="<%=Remark%>"></td></tr>
    <tr>
  <td><input name="AdviceURL" type="hidden" value="<%=AdviceURL%>"></td></tr>
    <tr>
  <td><input name="ReturnURL" type="hidden" value="<%=ReturnURL%>"></td></tr>
  
    <tr>
  <td><input name="BillNo" type="hidden" class="input" value="<%=BillNo%>"></td></tr>
    <tr>
  <td><input name="MerNo" type="hidden" class="input" value="<%=MerNo%>"></td></tr>
    <tr>
  <td><input name="Amount" type="hidden" class="input" value="<%=Amount%>"></td></tr>
    <tr>
  <td><input name="SignInfo" type="hidden" class="input" value="<%=SignInfo%>"></td></tr>
  <tr>
  <td><input name="defaultBankNumber" type="hidden" class="input" value="<%=defaultBankNumber%>"></td></tr>
  <tr>
  <td><input name="orderTime" type="hidden" class="input" value="<%=orderTime%>"></td></tr>


        <tr>  
		<td><input type="hidden" name="products" value="<%=products%>"></td></tr>
    
   <input name="B1" type="submit" class="input" value="Payment">
   </table>
             
</form>
</body>
</html>