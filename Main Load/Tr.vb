Imports Main_Load.Form1
Module Tr

    Private Property text As String

    Public Sub msg(m As Integer)

        Dim olumlu As Boolean
        Dim msg(8) As String

        msg(1) = "Kategori başarıyla silindi."
        msg(2) = "Kategori başarıyla oluşturuldu."
        msg(3) = "Item başarıyla temizlendi."
        msg(4) = "Item ekleme işlemi başarılı."
        msg(6) = "Kategori altı klasör başarıyla temizlendi."
        msg(7) = "Kategori altı klasör başarıyla oluşturuldu."
        msg(8) = "Hata, lütfen tekrar deneyin."


        If m = 8 Then
            olumlu = False
        Else
            olumlu = True
        End If


        If olumlu = False Then
            MsgBox(msg(m), MsgBoxStyle.Critical, "Hata")
        ElseIf olumlu = True Then
            MsgBox(msg(m), MsgBoxStyle.Information, "Başarılı")
        End If

    End Sub

    Public Sub names(n As String)

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

    Public WriteOnly Property write() As String
        Set(text As String)
        End Set
    End Property

End Module
