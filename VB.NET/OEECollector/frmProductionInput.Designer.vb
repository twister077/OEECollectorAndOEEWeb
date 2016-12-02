<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProductionInput
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
      Me.lblUndefined = New System.Windows.Forms.Label()
      Me.grpUndefined = New System.Windows.Forms.GroupBox()
      Me.chkArticleNunbers = New System.Windows.Forms.RadioButton()
      Me.chkOrders = New System.Windows.Forms.RadioButton()
      Me.lstArticle = New System.Windows.Forms.ListView()
      Me.grpUndefined.SuspendLayout()
      Me.SuspendLayout()
      '
      'lblUndefined
      '
      Me.lblUndefined.AutoSize = True
      Me.lblUndefined.BackColor = System.Drawing.Color.Transparent
      Me.lblUndefined.Font = New System.Drawing.Font("Calibri", 16.0!)
      Me.lblUndefined.Location = New System.Drawing.Point(16, 3)
      Me.lblUndefined.Name = "lblUndefined"
      Me.lblUndefined.Size = New System.Drawing.Size(184, 27)
      Me.lblUndefined.TabIndex = 71
      Me.lblUndefined.Text = "Define production:"
      '
      'grpUndefined
      '
      Me.grpUndefined.Controls.Add(Me.chkArticleNunbers)
      Me.grpUndefined.Controls.Add(Me.chkOrders)
      Me.grpUndefined.Location = New System.Drawing.Point(13, 25)
      Me.grpUndefined.Name = "grpUndefined"
      Me.grpUndefined.Size = New System.Drawing.Size(217, 395)
      Me.grpUndefined.TabIndex = 74
      Me.grpUndefined.TabStop = False
      '
      'chkArticleNunbers
      '
      Me.chkArticleNunbers.AutoSize = True
      Me.chkArticleNunbers.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkArticleNunbers.Location = New System.Drawing.Point(9, 44)
      Me.chkArticleNunbers.Name = "chkArticleNunbers"
      Me.chkArticleNunbers.Size = New System.Drawing.Size(128, 22)
      Me.chkArticleNunbers.TabIndex = 74
      Me.chkArticleNunbers.Text = "Article numbers"
      Me.chkArticleNunbers.UseVisualStyleBackColor = True
      '
      'chkOrders
      '
      Me.chkOrders.AutoSize = True
      Me.chkOrders.Checked = True
      Me.chkOrders.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkOrders.Location = New System.Drawing.Point(9, 17)
      Me.chkOrders.Name = "chkOrders"
      Me.chkOrders.Size = New System.Drawing.Size(72, 22)
      Me.chkOrders.TabIndex = 73
      Me.chkOrders.TabStop = True
      Me.chkOrders.Text = "Orders"
      Me.chkOrders.UseVisualStyleBackColor = True
      '
      'lstArticle
      '
      Me.lstArticle.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lstArticle.FullRowSelect = True
      Me.lstArticle.Location = New System.Drawing.Point(236, 33)
      Me.lstArticle.Name = "lstArticle"
      Me.lstArticle.Size = New System.Drawing.Size(848, 387)
      Me.lstArticle.TabIndex = 75
      Me.lstArticle.UseCompatibleStateImageBehavior = False
      Me.lstArticle.View = System.Windows.Forms.View.Details
      '
      'frmProductionInput
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(1096, 432)
      Me.Controls.Add(Me.lstArticle)
      Me.Controls.Add(Me.lblUndefined)
      Me.Controls.Add(Me.grpUndefined)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Name = "frmProductionInput"
      Me.Text = "frmProductionInput"
      Me.grpUndefined.ResumeLayout(False)
      Me.grpUndefined.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents lblUndefined As System.Windows.Forms.Label
   Friend WithEvents grpUndefined As System.Windows.Forms.GroupBox
   Friend WithEvents chkOrders As System.Windows.Forms.RadioButton
   Friend WithEvents lstArticle As System.Windows.Forms.ListView
   Friend WithEvents chkArticleNunbers As System.Windows.Forms.RadioButton
End Class
