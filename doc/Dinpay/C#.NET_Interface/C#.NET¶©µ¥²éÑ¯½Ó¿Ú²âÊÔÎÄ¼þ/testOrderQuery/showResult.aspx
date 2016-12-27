<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="showResult.aspx.cs" Inherits="testOrderQuery.showResult" ResponseEncoding="utf-8" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
        智付订单查询结果：
            <input type="hidden" id="msgs" runat="server" /><%=msgs.Value%>
            <table width="100%"  border="1">
                <tr>
                    <td>订单号</td>
                    <td>订单金额</td>
                    <td>订单时间</td>
                    <td>支付状态</td>
               </tr>
               <tr>
                    <td><input  type="hidden" id="orderid" name="orderid" runat="server" /><%=orderid.Value%></td>
                    <td><input type="hidden"  id="amount" name="amount" runat="server" /><%=amount.Value%></td>
                    <td><input  type="hidden" id="date" name="date" runat="server" /><%=date.Value%></td>
                    <td><input  type="hidden" id="status" name="status" runat="server" /><%=status.Value%></td>
               </tr>
             </table>
</body>
</html>
