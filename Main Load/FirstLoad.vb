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

Module FirstLoad

    Dim MainPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\MainLoad"

    Public Sub FirstLoad()

        If Not Directory.Exists(MainPath) Then
            Directory.CreateDirectory(MainPath)
            MsgBox("Uyarı, veri klasörü oluşturuldu.")
        End If
        'Ana klasör MainPath


        If Not Directory.Exists(MainPath + "\Settings") Then
            Directory.CreateDirectory(MainPath + "\Settings")
        End If
        'Ayarlar klasörü MainPath


        If Not Directory.Exists(MainPath + "\Categories") Then
            Directory.CreateDirectory(MainPath + "\Categories")
        End If
        'Kategori klasörü MainPath

        Licence(MainPath + "\Settings\LICENCE.txt")


    End Sub

    Private Function Licence(path As String)

        Try
            If File.Exists(path) Then
                File.Delete(path)
                My.Computer.FileSystem.WriteAllText(path, My.Resources.LICENCE, True)
            Else
                My.Computer.FileSystem.WriteAllText(path, My.Resources.LICENCE, True)
            End If
        Catch ex As Exception
        End Try
        Return path
    End Function

End Module
