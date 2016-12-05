Public NotInheritable Class frmSplash

   Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      If My.Application.Info.Title <> "" Then
         ApplicationTitle.Text = My.Application.Info.Title
      Else
         ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
      End If
      
      
      Version.Text = "Version: " & My.Application.Info.Version.ToString
      Copyright.Text = My.Application.Info.Copyright

   End Sub

End Class
