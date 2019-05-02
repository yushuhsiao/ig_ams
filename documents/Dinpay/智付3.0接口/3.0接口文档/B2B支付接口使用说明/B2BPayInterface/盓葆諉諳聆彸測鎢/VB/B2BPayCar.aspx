<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="B2BPayCar.aspx.vb" Inherits="vb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form action="https://payment.dinpay.com/B2BReceiveMerchantAction.do" id="form1" name="form1" method="POST" runat="server">

            <input type="hidden" name="service_type" id="service_type" size="40" runat="server" />

            <input type="hidden" name="merchant_code" id="merchant_code" size="40" runat="server" />

            <input type="hidden" name="input_charset" id="input_charset" size="40" runat="server" />

            <input type="hidden" name="notify_url" id="notify_url" size="40" runat="server" />

            <input type="hidden" name="return_url" id="return_url" size="40" runat="server" />

            <input type="hidden" name="client_ip" id="client_ip" size="40" runat="server" />

            <input type="hidden" name="interface_version" id="interface_version" size="40" runat="server" />

            <input type="hidden" name="sign_type" id="sign_type" size="40" runat="server" />

            <input type="hidden" name="order_no" id="order_no" size="40" runat="server" />

            <input type="hidden" name="order_time" id="order_time" size="40" runat="server" />

            <input type="hidden" name="order_amount" id="order_amount" size="40" runat="server" />

            <input type="hidden" name="product_name" id="product_name" runat="server" size="40" />

            <input type="hidden" name="show_url" id="show_url" size="40" runat="server" />

            <input type="hidden" name="product_code" id="product_code" size="40" runat="server" />

            <input type="hidden" name="product_num" id="product_num" size="40" runat="server" />

            <input type="hidden" name="product_desc" id="product_desc" size="40" runat="server" />

            <input type="hidden" name="bank_code" id="bank_code" size="40" runat="server" />

            <input type="hidden" name="extra_return_param" id="extra_return_param" size="40" runat="server" />

            <input type="hidden" name="extend_param" id="extend_param" size="40" runat="server" />

            <input type="hidden" name="sign" size="" id="sign" runat="server" />

            <script type="text/javascript">
                document.getElementById("form1").submit();
            </script>
    </form>
</body>
</html>
