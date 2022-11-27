'Licensed under the Apache License, Version 2.0 (the "License");
'you may not use this file except in compliance with the License.
'You may obtain a copy of the License at

'   http://www.apache.org/licenses/LICENSE-2.0

'Unless required by applicable law or agreed to in writing, software
'distributed under the License is distributed on an "AS IS" BASIS,
'WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'See the License for the specific language governing permissions and
'limitations under the License.

'--------------------

Imports System.IO
Imports Main_Load.FirstLoad
Imports Main_Load.Tr
Imports Main_Load.En
Imports System.Data

Public Class Form1

    Dim MainPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\MainLoad"
    Dim change As Boolean = False
    Dim sview As String
    Dim lang As String


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        'başlangıç
        If My.Settings.Lang = "" Then
            lang = "En"
            My.Settings.Lang = lang
        Else
            lang = My.Settings.Lang
        End If

        tnames()

        If ComboBox1.SelectedIndex < 0 Then
            If lang = "Tr" Then ComboBox1.SelectedIndex = 1 Else ComboBox1.SelectedIndex = 0
        End If

        FirstLoad.FirstLoad()
        Timer1.Start()

        Me.Height = 445

    End Sub

    Private Sub ExtendBtn_Click(sender As Object, e As EventArgs) Handles ExtendBtn.Click

        'Yükseklik değiştirme butonu
        If Me.Height = 445 Then
            Me.Height = Me.Height + 143
            ExtendBtn.Text = "▲"
        Else
            Me.Height = Me.Height - 143
            ExtendBtn.Text = "▼"
        End If

    End Sub

    Private Sub KatSilBtn_Click(sender As Object, e As EventArgs) Handles KatSilBtn.Click

        'Kategori silme butonu
        Try
            Dim skat As String = ListBox1.SelectedItem.ToString

            Try
                FileSystem.Kill(MainPath + "\Categories\" + skat + "/*.*")
                Directory.Delete(MainPath + "\Categories\" + skat)
            Catch ex As Exception
                Directory.Delete(MainPath + "\Categories\" + skat)
            End Try
            msg(1)
        Catch ex As Exception
            msg(8)
        End Try

    End Sub

    Private Sub ItemSilBtn_Click(sender As Object, e As EventArgs) Handles ItemSilBtn.Click

        'Item silme butonu
        Dim succes As Boolean = False
        Try
                    My.Computer.FileSystem.DeleteFile(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + ListBox2.SelectedItem.ToString)
            msg(3)
            Exit Sub
                Catch ex As Exception
                    succes = False
                End Try

                Try
                    My.Computer.FileSystem.DeleteFile(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + sview)
            msg(3)
            Exit Sub
                Catch ex As Exception
                    succes = False
                End Try



                If succes = False Then
            msg(8)
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        '50 ms/1

        'item ve kategori verilerini çekme
        KategoriCekme()
        ItemReload_Lbox()
        ItemReload_Lview()
        FolderReload()

        'değişen bir şey var ise reload atma
        If change = True Then
            change = False
            ChangeReload()
        End If

        'seçili kategori yok ise
        If ListBox1.SelectedIndex = -1 Then
            ListBox2.Items.Clear()
            ListView1.Items.Clear()
        End If


        'seçili listview itemi var ise onu çağırabilmek için sview değerini o olarak atar
        If ListView1.Focused = True Then
            If Not sview = ListView1.FocusedItem.Text Then sview = ListView1.FocusedItem.Text
        End If

    End Sub

    Public Sub KategoriCekme()

        'Gerek var ise kategorileri yeniler/gösterir
        Dim Kisimleri = My.Computer.FileSystem.GetDirectories(MainPath + "\Categories\")
        Dim Ksayisi = My.Computer.FileSystem.GetDirectories(MainPath + "\Categories\").Count

        If Not ListBox1.Items.Count = Ksayisi Then

            ListBox1.Items.Clear()

            For Each isim As String In Kisimleri
                Dim result As String = Path.GetFileName(isim)
                ListBox1.Items.Add(result)
            Next

        End If

    End Sub

    Private Sub KatEkleBtn_Click(sender As Object, e As EventArgs) Handles KatEkleBtn.Click

        'Kategori oluşturma butonu
        Dim CategorieName_Space As Boolean = False
        Dim CategorieName As String = InputBox("Yeni kategori ismi giriniz:")
        If CategorieName = "" Then CategorieName_Space = True


        If Not CategorieName_Space = True Then
            Try
                Directory.CreateDirectory(MainPath + "\Categories\" + CategorieName)
                msg(2)
            Catch ex As Exception
                msg(8)
            End Try
        Else
            msg(8)
        End If

    End Sub

    Private Sub ItemEkleBtn_Click(sender As Object, e As EventArgs) Handles ItemEkleBtn.Click

        'Item ekleme butonu
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then

            If Not ListBox1.SelectedIndex = -1 Then
                Try
                    For Each file1 In OpenFileDialog1.FileNames
                        Dim file2 As String = file1.Split("\").Last
                        My.Computer.FileSystem.MoveFile(file1, MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + file2)
                    Next

                    msg(4)
                Catch ex As Exception
                    msg(8)
                End Try

            Else
                msg(8)
            End If
        End If

    End Sub

    Private Sub ItemReload_Lbox()

        'Item list reload - ListBox
        If Not ListBox1.SelectedIndex = -1 Then

            Dim CPath As String = MainPath + "\Categories\" + ListBox1.SelectedItem.ToString
            Dim FPath As String = MainPath + "\Categories\"
            Dim fsayisi As Integer = My.Computer.FileSystem.GetFiles(CPath).Count
            Dim ksayisi As Integer = My.Computer.FileSystem.GetDirectories(CPath).Count
            Dim toplam As Integer = ksayisi + fsayisi
            Dim isayisi As Integer = ListBox2.Items.Count


            If Not isayisi = fsayisi Then

                ListBox2.Items.Clear()

                For Each maddeler In My.Computer.FileSystem.GetFiles(CPath)
                    Dim sonuc As String = maddeler.Split("\").Last
                    ListBox2.Items.Add(sonuc)
                Next
            End If
        End If

    End Sub

    Private Sub FolderReload()

        'kategori altı klasör reload
        If Not ListBox1.SelectedIndex = -1 Then

            Dim CPath As String = MainPath + "\Categories\" + ListBox1.SelectedItem.ToString
            Dim dsayisi As Integer = My.Computer.FileSystem.GetDirectories(CPath + "\").Count
            Dim odsayisi As Integer = ListBox3.Items.Count

            If Not odsayisi = dsayisi Then
                ListBox3.Items.Clear()

                For Each kats In My.Computer.FileSystem.GetDirectories(CPath)
                    Dim sonuc As String = kats.Split("\").Last
                    ListBox3.Items.Add(sonuc)
                Next
            End If
        End If

    End Sub

    Private Function ChangeReload()

        'değişen kategoriye göre reload
        If ListBox1.SelectedIndex >= 0 Then
            Try
                Dim CPath As String = MainPath + "\Categories\" + ListBox1.SelectedItem.ToString
                Dim di As New IO.DirectoryInfo(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\")
                Dim fsayisi As Integer = My.Computer.FileSystem.GetFiles(CPath).Count
                Dim isayisi As Integer = ListView1.Items.Count
                Dim dsayisi As Integer = My.Computer.FileSystem.GetDirectories(CPath + "\").Count
                Dim odsayisi As Integer = ListBox3.Items.Count

                '------------------------------------

                ListBox2.Items.Clear()

                For Each maddeler In My.Computer.FileSystem.GetFiles(CPath)
                    Dim sonuc As String = maddeler.Split("\").Last
                    ListBox2.Items.Add(sonuc)
                Next


                '------------------------------------

                ImageList1.Images.Clear()
                ListView1.Items.Clear()
                ListView1.BeginUpdate()


                For Each fi As IO.FileInfo In di.GetFiles("*")

                    Dim icons As Icon = SystemIcons.WinLogo
                    Dim li As New ListViewItem(fi.Name, 1)

                    If Not (ImageList1.Images.ContainsKey(fi.FullName)) Then
                        icons = System.Drawing.Icon.ExtractAssociatedIcon(fi.FullName)
                        ImageList1.Images.Add(fi.FullName, icons)
                    End If

                    icons = Icon.ExtractAssociatedIcon(fi.FullName)
                    ImageList1.Images.Add(icons)
                    ListView1.Items.Add(fi.Name, fi.FullName)

                    ListView1.EndUpdate()
                Next


                '------------------------------------

                ListBox3.Items.Clear()

                For Each kats In My.Computer.FileSystem.GetDirectories(CPath)
                    Dim sonuc As String = kats.Split("\").Last
                    ListBox3.Items.Add(sonuc)
                Next

            Catch ex As Exception
            End Try
        End If
    End Function

    Private Sub ItemReload_Lview()

        'Item list reload - ListView
        If Not ListBox1.SelectedIndex = -1 Then

            Dim CPath As String = MainPath + "\Categories\" + ListBox1.SelectedItem.ToString
            Dim di As New IO.DirectoryInfo(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\")
            Dim fsayisi As Integer = My.Computer.FileSystem.GetFiles(CPath).Count
            Dim isayisi As Integer = ListView1.Items.Count


            If Not fsayisi = isayisi Then

                ImageList1.Images.Clear()
                ListView1.Items.Clear()
                ListView1.BeginUpdate()


                For Each fi As IO.FileInfo In di.GetFiles("*")

                    Dim icons As Icon = SystemIcons.WinLogo
                    Dim li As New ListViewItem(fi.Name, 1)

                    If Not (ImageList1.Images.ContainsKey(fi.FullName)) Then
                        icons = System.Drawing.Icon.ExtractAssociatedIcon(fi.FullName)
                        ImageList1.Images.Add(fi.FullName, icons)
                    End If

                    icons = Icon.ExtractAssociatedIcon(fi.FullName)
                    ImageList1.Images.Add(icons)
                    ListView1.Items.Add(fi.Name, fi.FullName)

                    ListView1.EndUpdate()
                Next
            End If
        End If

    End Sub

    Private Sub ListBox2_DoubleClick(sender As Object, e As EventArgs) Handles ListBox2.DoubleClick

        'item başlatma - listbox
        If Not ListBox1.SelectedIndex < 0 And Not ListBox2.SelectedIndex < 0 Then
            ItemStart(ListBox2.SelectedItem.ToString, 0)
        End If

    End Sub

    Private Sub ListBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox2.KeyDown

        'enter basılırsa - listbox
        If e.KeyCode = Keys.Enter And Not ListBox1.SelectedIndex < 0 And Not ListBox2.SelectedIndex < 0 Then
            ItemStart(ListBox2.SelectedItem.ToString, 0)
        End If

    End Sub

    Private Sub ListBox2_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox2.DragEnter

        'Listbox2 drag-drop giriş, effect ekleme
        If e.Data.GetDataPresent(DataFormats.FileDrop, False) = True Then
            e.Effect = DragDropEffects.All
        End If

    End Sub

    Private Sub ListBox2_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox2.DragDrop

        'ListBox2 drag-drop bırakma
        Dim DroppedItems As String() = e.Data.GetData(DataFormats.FileDrop)
        Dim success As Boolean = False

        For Each f In DroppedItems
            Dim myfile As String = f
            Try
                System.IO.File.Move(myfile, MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + FName(myfile))
                success = True
            Catch ex As Exception
                msg(8)
                success = False
                Exit Sub
            End Try
        Next

        If success = True Then
            msg(4)
            success = False
        End If

    End Sub
    
	Public Function FName(path As String)
        Return System.IO.Path.GetFileName(path)
    End Function

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter

        'Listbox1 drag-drop giriş, effect ekleme
        If e.Data.GetDataPresent(DataFormats.FileDrop, False) = True Then
            e.Effect = DragDropEffects.All
        End If

    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop

        'ListBox1 Drag-Drop bırakma
        Dim DroppedItems As String() = e.Data.GetData(DataFormats.FileDrop)
        Dim success As Boolean = False

        For Each f In DroppedItems
            Dim myfile As String = f
            Dim control As Boolean

            Try
                FName(myfile).ToString.Remove(FName(myfile).ToString.LastIndexOf("."))
                control = False
            Catch ex As Exception
                control = True
            End Try

            If control = True Then
                Try
                    System.IO.Directory.Move(myfile, MainPath + "\Categories\" + FName(myfile))
                    success = True
                Catch ex As Exception
                    success = False
                End Try
            End If
        Next

        If success = True Then
            msg(4)
            success = False
        Else
            msg(8)
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'Veri klasörü açma butonu
        Process.Start(MainPath)

    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick

        'item başlatma - listview
        If Not ListBox1.SelectedIndex < 0 And Not ListView1.FocusedItem.Index < 0 Then
            ItemStart(ListView1.FocusedItem.Text, 0)
        End If

    End Sub

    Private Sub ListView1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListView1.KeyDown

        'enter basılırsa - listview
        If e.KeyCode = Keys.Enter Then
            ItemStart(ListView1.FocusedItem.Text, 0)
        End If
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter

        'Listview drag-drop giriş, effect ekleme
        If e.Data.GetDataPresent(DataFormats.FileDrop, False) = True Then
            e.Effect = DragDropEffects.All
        End If

    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop

        'Listview drag-drop bırakma
        Dim DroppedItems As String() = e.Data.GetData(DataFormats.FileDrop)
        Dim success As Boolean = False

        For Each f In DroppedItems
            Dim myfile As String = f
            Try
                System.IO.File.Move(myfile, MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + FName(myfile))
                success = True
            Catch ex As Exception
                msg(8)
                success = False
                Exit Sub
            End Try
        Next

        If success = True Then
            msg(4)
            success = False
        End If

    End Sub

    Private Sub Kilavuzbtn_Click(sender As Object, e As EventArgs) Handles Kilavuzbtn.Click

        'kılavuz butonu
        Dim Msg(10) As String
        Msg(1) = "- Bu; günlük hayatta kullanabileceğiniz, işlerinzi kolaylaştırmayı hedefleyen bir uygulamadır.."
        Msg(2) = "● Kategorileri, kategori ekleme butonundan tekil olarak veya sürükleyerek tekil ve çoğul olarak ekleyebilirsiniz."
        Msg(3) = "● İtemleri, item ekleme butonundan tekil ve çoğul olarak veya sürükleyerek tekil ve çoğul olarak ekleyebilirsiniz."
        Msg(4) = "● Kategori altı klasörleri soldan sağa doğru 2. ve aşağıdaki kutucuktan görebilir ve açabilirsiniz. Eklemek ve silmek isterseniz alt klasör menüsünü kullanabilirsiniz."
        Msg(5) = "● Kategorileri, kategori silme; itemleri de item silme butonundan silebilirsiniz."
        Msg(6) = "● İtemleri ve kategori altı klasörleri çift tıklayarak veya enter tuşuna basarak açabilirsiniz."
        Msg(7) = "● Veri konumunu aç butonunu kullanarak verilerin tutulduğu klasöre gidebilir ve düzenleme yapabilirsiniz."
        Msg(8) = "● Kategori konumunu aç butonunu kullanarak seçtiğiniz kategorinin konumuna hızlıca ulaşabilirsiniz."
        Msg(9) = "●UYARI: Eğer kısayol veya bir başka dosya uzantısına sahip bir dosyanız çalışmıyor ise dosyanızın ismindeki boşlukları veya başka geçersiz olabilecek karakterleri silmeyi deneyin, eğer olumlu sonuç alamazsanız ve dosyanız bir kısayol ise kısayol ismini hedef dosya ile aynı yapınız, eğer hala olumlu sonuç alamıyorsanız dosyanızı gözden geçirip tekrar deneyiniz.."
        Msg(10) = "- Programımızı kullandığınız için teşekkür eder, iyi kullanımlar dileriz.."

        Dim line1 = vbNewLine
        Dim line2 = vbNewLine + vbNewLine

        Dim Msgs = Msg(1) + line2 + Msg(2) + line1 + Msg(3) + line1 + Msg(4) + line1 + Msg(5) + line1 + Msg(6) + line1 + Msg(7) + line1 + Msg(8) + line2 + Msg(9) + (line2 + line2) + Msg(10)
        MsgBox(Msgs, , "Kılavuz")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'kategori konumunu açma butonu
        Try
            Process.Start(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\")
        Catch ex As Exception
            msg(8)
        End Try

    End Sub

    Private Sub ListBox3_DoubleClick(sender As Object, e As EventArgs) Handles ListBox3.DoubleClick

        'item başlatma - listbox3
        If Not ListBox1.SelectedIndex < 0 And Not ListBox3.SelectedIndex < 0 Then
            ItemStart(ListBox3.SelectedItem.ToString, 1)
        End If


    End Sub

    Private Sub ListBox3_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox3.KeyDown

        'enter basılırsa - listbox3
        If e.KeyCode = Keys.Enter Then
            ItemStart(ListBox3.SelectedItem.ToString, 1)
        End If

    End Sub

    Private Function ItemStart(item As String, smod As Integer)

        'item başlatma
        Try
            Dim lpath As String = MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\"

            If smod = 0 Then
                Process.Start(lpath + item)
            ElseIf smod = 1 Then

                Dim path As String = lpath
                Dim Proc As String = "Explorer.exe"
                Dim Args As String =
       ControlChars.Quote &
       IO.Path.Combine(path, item) &
       ControlChars.Quote
                Process.Start(Proc, Args)

            End If
        Catch ex As Exception

            Try
                Dim myitem As String = MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + item

                Dim psi = New ProcessStartInfo With {.FileName = "C:\Windows\SysNative\cmd.exe", .Arguments = "/C start """" " + myitem, .UseShellExecute = True, .CreateNoWindow = True, .WindowStyle = ProcessWindowStyle.Hidden}
                Process.Start(psi)
            Catch exx As Exception
            End Try

        End Try

        Return item
    End Function


    Private Sub ListBox3_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox3.DragEnter

        'Listbox4 drag-drop giriş, effect ekleme
        If e.Data.GetDataPresent(DataFormats.FileDrop, False) = True Then
            e.Effect = DragDropEffects.All
        End If

    End Sub

    Private Sub ListBox3_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox3.DragDrop

        'ListBox3 Drag-Drop bırakma
        Dim DroppedItems As String() = e.Data.GetData(DataFormats.FileDrop)
        Dim success As Boolean = False

        For Each f In DroppedItems
            Dim myfile As String = f
            Dim control As Boolean

            Try
                FName(myfile).ToString.Remove(FName(myfile).ToString.LastIndexOf("."))
                control = False
            Catch ex As Exception
                control = True
            End Try

            If control = True Then
                Try
                    System.IO.Directory.Move(myfile, MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + FName(myfile))
                    success = True
                Catch ex As Exception
                    success = False
                End Try
            End If
        Next

        If success = True Then
            msg(4)
            success = False
        Else
            msg(8)
        End If

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

        'kategori değişince change değerini değiştirme
        change = True

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'kategori altı klasör silme
        Dim succes As Boolean

        Try
            Dim path() As String = System.IO.Directory.GetFiles(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + ListBox3.SelectedItem.ToString + "\")
            For Each file In path
                System.IO.File.Delete(file)
            Next

            Dim di As New IO.DirectoryInfo(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + ListBox3.SelectedItem.ToString)
            di.Delete(True)
            msg(6)
            Exit Sub
        Catch ex As Exception
            succes = False
        End Try

        If succes = False Then
            msg(8)
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        'kategori altı klasör oluşturma
        Dim name As String = InputBox("Lütfen bir klasör adı giriniz: ")
        Try
            Directory.CreateDirectory(MainPath + "\Categories\" + ListBox1.SelectedItem.ToString + "\" + name)
            msg(7)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        'hakkında menüsü
        Dim dt(5) As String
        Dim ab As String

        If lang = "Tr" Then
            dt(1) = "► Uygulama ismi:  " + Application.ProductName
            dt(2) = "► Uygulama versiyonu:  v" + Application.ProductVersion.ToString
            dt(3) = "► Uygulama dili:  " + "Türkçe"
            dt(4) = "► Uygulama lisansı:  " + "APACHE LICENSE, VERSION 2.0"
            dt(5) = "► Yapan:  NoPlayer.D"
            ab = "Hakkında"
        Else
            dt(1) = "► Application Name:  " + Application.ProductName
            dt(2) = "► Application Version:  v" + Application.ProductVersion.ToString
            dt(3) = "► Applicatoin Language:  " + "English"
            dt(4) = "► Application License:  " + "APACHE LICENSE, VERSION 2.0"
            dt(5) = "► Cretor:  NoPlayer.D"
            ab = "About"
        End If


        Dim show As String = dt(1) + vbNewLine + dt(2) + vbNewLine + dt(3) + vbNewLine + dt(4) + vbNewLine + dt(5) + vbNewLine
        MsgBox(show, MsgBoxStyle.Information, ab)

    End Sub

    Private Function msg(code As Integer)

        'msgbox'ları göstermemizi sağlar
        If lang = "Tr" Then
            Tr.msg(code)
        ElseIf lang = "En" Then
            En.msg(code)
        End If

        Return code
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        'dili değiştirir
        If ComboBox1.SelectedIndex = 0 Then
            lang = "En"
            My.Settings.Lang = lang
        Else
            lang = "Tr"
            My.Settings.Lang = lang
        End If
        tnames()

    End Sub
    Public Function tnames()

        If lang = "Tr" Then
            For i As Integer = 1 To 7
                Tr.names(i)
                sitem(i)
            Next
        Else
            For i As Integer = 1 To 7
                En.names(i)
                sitem(i)
            Next
        End If

    End Function

    Public Function sitem(item As Integer)

        If lang = "Tr" Then

            If item = 1 Then
                Button3.Text = Tr.read
            ElseIf item = 2 Then
                GroupBox3.Text = Tr.read
            ElseIf item = 3 Then
                GroupBox6.Text = Tr.read
            ElseIf item = 4 Then
                Kilavuzbtn.Text = Tr.read
            ElseIf item = 5 Then
                Button1.Text = Tr.read
            ElseIf item = 6 Then
                Button5.Text = Tr.read
            ElseIf item = 7 Then
                GroupBox5.Text = Tr.read
            End If

        Else

            If item = 1 Then
                Button3.Text = En.read
            ElseIf item = 2 Then
                GroupBox3.Text = En.read
            ElseIf item = 3 Then
                GroupBox6.Text = En.read
            ElseIf item = 4 Then
                Kilavuzbtn.Text = En.read
            ElseIf item = 5 Then
                Button1.Text = En.read
            ElseIf item = 6 Then
                Button5.Text = En.read
            ElseIf item = 7 Then
                GroupBox5.Text = En.read
            End If

        End If


        Return item
    End Function

End Class
