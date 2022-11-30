Module Language

    Private Property text As String

    Public Sub Msg_EN(m As Integer)
        Dim msg(8) As String

        msg(1) = "The category has been successfully deleted."
        msg(2) = "Category created successfully."
        msg(3) = "Item successfully cleared."
        msg(4) = "Adding item successful."
        msg(6) = "The sub-category folder has been successfully cleared."
        msg(7) = "The sub-category folder has been successfully created."
        msg(8) = "Error, please try again."

        If m = 8 Then
            MsgBox(msg(m), MsgBoxStyle.Critical, "Error")
        Else
            MsgBox(msg(m), MsgBoxStyle.Information, "Successful")
        End If
    End Sub
    Public Sub Names_EN(n As String)
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
    Public Sub Msg_TR(m As Integer)
        Dim msg(8) As String

        msg(1) = "Kategori başarıyla silindi."
        msg(2) = "Kategori başarıyla oluşturuldu."
        msg(3) = "Item başarıyla temizlendi."
        msg(4) = "Item ekleme işlemi başarılı."
        msg(6) = "Kategori altı klasör başarıyla temizlendi."
        msg(7) = "Kategori altı klasör başarıyla oluşturuldu."
        msg(8) = "Hata, lütfen tekrar deneyin."

        If m = 8 Then
            MsgBox(msg(m), MsgBoxStyle.Critical, "Hata")
        Else
            MsgBox(msg(m), MsgBoxStyle.Information, "Başarılı")
        End If
    End Sub

    Public Sub Names_TR(n As String)
        Dim name(7) As String

        name(1) = "Kategori konumunu aç"
        name(2) = "Kategori"
        name(3) = "Alt-Klasör"
        name(4) = "Kılavuz"
        name(5) = "Veri Konumunu Aç"
        name(6) = "Hakkında"
        name(7) = "Uygulama Ayarları"

        text = name(n)
    End Sub

    Public ReadOnly Property read() As String
        Get
            Return text
        End Get
    End Property
End Module
