Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim flag As Boolean = False
            Dim sb As String = ""

            Dim bank_code1 As String = Trim(Request.Form("bank_code").ToString())
            If bank_code1 <> "" Then
                sb = sb + "bank_code=" + bank_code1
                flag = True
            End If

            Dim client_ip1 As String = Trim(Request.Form("client_ip").ToString())
            If client_ip1 <> "" Then
                If flag = True Then
                    sb = sb + "&client_ip=" + client_ip1
                Else
                    sb = sb + "client_ip=" + client_ip1
                End If
                flag = True
            End If


            Dim extend_param1 As String = Trim(Request.Form("extend_param").ToString())
            If extend_param1 <> "" Then
                If flag = True Then
                    sb = sb + "&extend_param=" + extend_param1
                Else
                    sb = sb + "extend_param=" + extend_param1
                End If
                flag = True
            End If

            Dim extra_return_param1 As String = Trim(Request.Form("extra_return_param").ToString())
            If extra_return_param1 <> "" Then
                If flag = True Then
                    sb = sb + "&extra_return_param=" + extra_return_param1
                Else
                    sb = sb + "extra_return_param=" + extra_return_param1
                End If
                flag = True
            End If

            Dim input_charset1 As String = Trim(Request.Form("input_charset").ToString())
            If input_charset1 <> "" Then
                If flag = True Then
                    sb = sb + "&input_charset=" + input_charset1
                Else
                    sb = sb + "input_charset=" + input_charset1
                End If
                flag = True
            End If

            Dim interface_version1 As String = Trim(Request.Form("interface_version").ToString())
            If interface_version1 <> "" Then
                sb = sb + "&interface_version=" + interface_version1
                flag = True
            End If

            Dim merchant_code1 As String = Trim(Request.Form("merchant_code").ToString())
            If merchant_code1 <> "" Then
                sb = sb + "&merchant_code=" + merchant_code1
                flag = True
            End If

            Dim notify_url1 As String = Trim(Request.Form("notify_url").ToString())
            If notify_url1 <> "" Then
                sb = sb + "&notify_url=" + notify_url1
                flag = True
            End If

            Dim order_amount1 As String = Trim(Request.Form("order_amount").ToString())
            If order_amount1 <> "" Then
                sb = sb + "&order_amount=" + order_amount1
                flag = True
            End If

            Dim order_no1 As String = Trim(Request.Form("order_no").ToString())
            If order_no1 <> "" Then
                sb = sb + "&order_no=" + order_no1
                flag = True
            End If

            Dim order_time1 As String = Trim(Request.Form("order_time").ToString())
            If order_time1 <> "" Then
                sb = sb + "&order_time=" + order_time1
                flag = True
            End If

            Dim product_code1 As String = Trim(Request.Form("product_code").ToString())
            If product_code1 <> "" Then
                sb = sb + "&product_code=" + product_code1
                flag = True
            End If

            Dim product_desc1 As String = Trim(Request.Form("product_desc").ToString())
            If product_desc1 <> "" Then
                sb = sb + "&product_desc=" + product_desc1
                flag = True
            End If

            Dim product_name1 As String = Trim(Request.Form("product_name").ToString())
            If product_name1 <> "" Then
                sb = sb + "&product_name=" + product_name1
                flag = True
            End If

            Dim product_num1 As String = Trim(Request.Form("product_num").ToString())
            If product_num1 <> "" Then
                sb = sb + "&product_num=" + product_num1
                flag = True
            End If

            Dim return_url1 As String = Trim(Request.Form("return_url").ToString())
            If return_url1 <> "" Then
                sb = sb + "&return_url=" + return_url1
                flag = True
            End If

            Dim service_type1 As String = Trim(Request.Form("service_type").ToString())
            If service_type1 <> "" Then
                sb = sb + "&service_type=" + service_type1
                flag = True
            End If

            Dim show_url1 As String = Trim(Request.Form("show_url").ToString())
            If show_url1 <> "" Then
                sb = sb + "&show_url=" + show_url1
                flag = True
            End If


            Dim sign_type1 As String = Trim(Request.Form("sign_type").ToString())

            Dim key As String = "123456789a123456789_"
            sb = sb + "&key={" + key + "}"

            Dim sign1 As String = UCase(Trim(FormsAuthentication.HashPasswordForStoringInConfigFile(sb, "md5")))

            service_type.Value = service_type1
            merchant_code.Value = merchant_code1
            input_charset.Value = input_charset1
            notify_url.Value = notify_url1
            return_url.Value = return_url1
            client_ip.Value = client_ip1
            interface_version.Value = interface_version1
            sign_type.Value = sign_type1
            order_no.Value = order_no1
            order_time.Value = order_time1
            order_amount.Value = order_amount1
            product_name.Value = product_name1
            show_url.Value = show_url1
            product_code.Value = product_code1
            product_num.Value = product_num1
            product_desc.Value = product_desc1
            bank_code.Value = bank_code1
            extra_return_param.Value = extra_return_param1
            extend_param.Value = extend_param1
            sign.Value = sign1

        Catch ex As Exception

        End Try

    End Sub

End Class