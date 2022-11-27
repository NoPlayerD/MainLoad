Module En

    Private Property text As String

    Public Sub msg(m As Integer)

        Dim olumlu As Boolean
        Dim msg(8) As String

        msg(1) = "The category has been successfully deleted."
        msg(2) = "Category created successfully."
        msg(3) = "Item successfully cleared."
        msg(4) = "Adding item successful."
        msg(6) = "The sub-category folder has been successfully cleared."
        msg(7) = "The sub-category folder has been successfully created."
        msg(8) = "Error, please try again."


        If m = 8 Then
            olumlu = False
        Else
            olumlu = True
        End If


        If olumlu = False Then
            MsgBox(msg(m), MsgBoxStyle.Critical, "Error")
        ElseIf olumlu = True Then
            MsgBox(msg(m), MsgBoxStyle.Information, "Successful")
        End If

    End Sub
    Public Sub names(n As String)

        Dim name(7) As String

        name(1) = "Open category location"
        name(2) = "Category"
        name(3) = "Sub-Folder"
        name(4) = "Guide"
        name(5) = "Open Data Location"
        name(6) = "About"
        name(7) = "Application Settings"


        text = name(n)

    End Sub

    Public ReadOnly Property read() As String
        Get
            Return Text
        End Get
    End Property

    Public WriteOnly Property write() As String
        Set(text As String)
        End Set
    End Property

End Module
