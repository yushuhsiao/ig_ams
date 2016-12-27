<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="getXmlData.aspx.cs" Inherits="testOrderQuery.getXmlData" ResponseEncoding="utf-8"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单查询</title>
    <script type="text/javascript">
        <!--异步获取服务器数据-->
        var xmlHttp;
    	function createXMLHttpRequest(){
    		if(window.ActiveXObject){
    			xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	    	} else {
	    		xmlHttp = new XMLHttpRequest();
	    	}
    	}
    	function sendServer() {
    		var queryMessage = document.getElementById("QueryMessage").value;
    		var digest = document.getElementById("Digest").value;
    		var m_id = document.getElementById("M_ID").value;

    		createXMLHttpRequest();
    		xmlHttp.open("POST",'https://payment.dinpay.com/VirMQueryOrder.do?QueryMessage='+queryMessage+'&Digest='+digest+'&M_ID='+m_id,true);
    		xmlHttp.onreadystatechange = callback;
    		xmlHttp.send();
    	}

    	function callback(){
    		if(xmlHttp.readyState == 4){
    			if(xmlHttp.status == 200){
    				var data = xmlHttp.responseText;
    				document.getElementById("OrderData").value  = data.substring(data.indexOf("<OrderData>")+11,data.indexOf("</OrderData>"));
                                document.getElementById("Sign").value  = data.substring(data.indexOf("<Sign>")+6,data.indexOf("</Sign>"));
                                document.getElementById("Code").value  = data.substring(data.indexOf("<Code>")+6,data.indexOf("</Code>"));
                                document.getElementById("dinpayForm").submit();
    			} else {
    				alert("服务器连接失败"+xmlHttp.status);
    			}
    		}
    	}
    </script>
</head>
<body onload="sendServer();">
    <form id="dinpayForm" name="dinpayForm" action="showResult.aspx" runat="server">
        <input id="M_ID" name="M_ID" type="hidden" runat="server" />
        <input id="Key" name="Key" type="hidden" runat="server" />
        <input id="MOrderID" name="MOrderID" type="hidden" runat="server" />
        <input id="MODate" name="MODate" type="hidden" runat="server" />
        <input id="Digest" name="Digest" type="hidden" runat="server" />
        <input id="QueryMessage" name="QueryMessage" type="hidden" runat="server" />
        <input id="OrderData" name="OrderData"  value="" type="text" runat="server"/>
	    <input id="Sign" name="Sign"  value="" type="text" runat="server"/>
	    <input id="Code" name="Code"  value="" type="text" runat="server"/>
    </form>
</body>
</html>
