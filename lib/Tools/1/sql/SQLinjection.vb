Public Class SQLinjection
    Public Shared Function escape(ByVal input As String) As String
        Dim ReplaceString() As String = {"'", """", ")", "(", ";", "-", "|"}
        For i As Integer = 0 To ReplaceString.Length - 1
            input = Replace(input, ReplaceString(i), "''")
        Next
        Return input
    End Function

    Public Shared Function validate_string(ByVal input As String) As Boolean
        Dim known_bad() As String = {"select", "insert", "update", "delete", "drop", "--", "'"}
        Dim i As Integer
        Dim v As Boolean = True
        For i = LBound(known_bad) To UBound(known_bad)
            If (InStr(1, input, known_bad(i), CompareMethod.Text) <> 0) Then
                v = False
                Exit Function
            End If
        Next
        Return v
    End Function

    Public Shared Function validatepassword(ByVal input As String) As Boolean
        Dim good_password_chars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim v As Boolean = True
        Dim i As Integer
        Dim c As String
        For i = 1 To Len(input)
            c = Mid(input, i, 1)
            If (InStr(good_password_chars, c) = 0) Then
                v = False
                Exit Function
            End If
        Next
        Return v
    End Function

    Public Shared Function Validate(ByVal input As String) As String
        If validate_string(input) Then
            Return escape(input)
        Else
            Return ""
        End If

    End Function
End Class
