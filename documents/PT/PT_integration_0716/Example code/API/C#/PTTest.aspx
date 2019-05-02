<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTTest.aspx.cs" Inherits="PTTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Playtek API Test</title>
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" id="btnSend" value="Send" />
        <textarea id="show">

        </textarea>
    </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnSend").click(function () {
                /*$.post("https://kioskpublicapi.redhorse88.com/getPlayerInfo/PPLAYMARTIN", {}, function (data) {
                    $("#show").html(data);
                });*/
                $.ajax({
                    type: 'GET',
                    url: 'RequestSending.aspx',
                    crossDomain: true,
                    beforeSend: function (msg) { $('#show').html("Processing"); },
                    success: function (data) { $('#show').html(data); },
                    error: function(e){ $('#show').html(e.message); },
                    datatype: 'json',
                });
            });
        });

    </script>
</body>
</html>
