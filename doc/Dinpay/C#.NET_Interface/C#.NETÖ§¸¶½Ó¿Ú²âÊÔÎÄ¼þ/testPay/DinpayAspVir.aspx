<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinpayAspVir.aspx.cs" Inherits="DinPayC.DinpayAspVir" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="DinpayAspVirForm" name="DinpayAspVirForm" runat="server">
        <input id="M_ID" name="M_ID" type="hidden" runat="server" />
        <input id="MOrderID" name="MOrderID" type="hidden" runat="server" />
        <input id="MOAmount" name="MOAmount" type="hidden" runat="server" />
        <input id="MOCurrency" name="MOCurrency" type="hidden" runat="server" />
        <input id="M_URL" name="M_URL" type="hidden" runat="server" />
        <input id="M_Language" name="M_Language" type="hidden" runat="server" />
        <input id="S_Name" name="S_Name" type="hidden" runat="server" />
        <input id="S_Address" name="S_Address" type="hidden" runat="server" />
        <input id="S_PostCode" name="S_PostCode" type="hidden" runat="server" />
        <input id="S_Telephone" name="S_Telephone" type="hidden" runat="server" />
        <input id="S_Email" name="S_Email" type="hidden" runat="server" />
        <input id="R_Name" name="R_Name" type="hidden" runat="server" />
        <input id="R_Address" name="R_Address" type="hidden" runat="server" />
        <input id="R_PostCode" name="R_PostCode" type="hidden" runat="server" />
        <input id="R_Telephone" name="R_Telephone" type="hidden" runat="server" />
        <input id="R_Email" name="R_Email" type="hidden" runat="server" />
        <input id="G_Name" name="G_Name" type="hidden" runat="server" />
        <input id="G_Number" name="G_Number" type="hidden" runat="server" />
        <input id="G_Count" name="G_Count" type="hidden" runat="server" />
        <input id="G_Info" name="G_Info" type="hidden" runat="server" />
        <input id="G_Url" name="G_Url" type="hidden" runat="server" />
        <input id="MOComment" name="MOComment" type="hidden" runat="server" />
        <input id="MODate" name="MODate" type="hidden" runat="server" />
        <input id="State" name="State" type="hidden" runat="server" />
        <input id="digestinfo" name="digestinfo" type="hidden" runat="server" />
	<input id="P_Bank" name="P_Bank" type="hidden" runat="server" />
        <script type="text/javascript">
            document.getElementById("DinpayAspVirForm").submit();
        </script>

    </form>
</body>
</html>
