<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUndefinedActivities
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
      Me.lstUndefStand2 = New System.Windows.Forms.ListView()
      Me.lblUndefined = New System.Windows.Forms.Label()
      Me.chkUndefStill = New System.Windows.Forms.RadioButton()
      Me.chkUndefProd = New System.Windows.Forms.RadioButton()
      Me.grpUndefined = New System.Windows.Forms.GroupBox()
      Me.SuspendLayout()
      '
      'lstUndefStand2
      '
      Me.lstUndefStand2.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lstUndefStand2.FullRowSelect = True
      Me.lstUndefStand2.Location = New System.Drawing.Point(236, 33)
      Me.lstUndefStand2.Name = "lstUndefStand2"
      Me.lstUndefStand2.Size = New System.Drawing.Size(848, 387)
      Me.lstUndefStand2.TabIndex = 1
      Me.lstUndefStand2.UseCompatibleStateImageBehavior = False
      Me.lstUndefStand2.View = System.Windows.Forms.View.Details
      '
      'lblUndefined
      '
      Me.lblUndefined.AutoSize = True
      Me.lblUndefined.BackColor = System.Drawing.Color.Transparent
      Me.lblUndefined.Font = New System.Drawing.Font("Calibri", 16.0!)
      Me.lblUndefined.Location = New System.Drawing.Point(16, 3)
      Me.lblUndefined.Name = "lblUndefined"
      Me.lblUndefined.Size = New System.Drawing.Size(198, 27)
      Me.lblUndefined.TabIndex = 70
      Me.lblUndefined.Text = "Undefined activities:"
      '
      'chkUndefStill
      '
      Me.chkUndefStill.AutoSize = True
      Me.chkUndefStill.Checked = True
      Me.chkUndefStill.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkUndefStill.Location = New System.Drawing.Point(22, 42)
      Me.chkUndefStill.Name = "chkUndefStill"
      Me.chkUndefStill.Size = New System.Drawing.Size(155, 22)
      Me.chkUndefStill.TabIndex = 71
      Me.chkUndefStill.TabStop = True
      Me.chkUndefStill.Text = "Undefined Standstill"
      Me.chkUndefStill.UseVisualStyleBackColor = True
      '
      'chkUndefProd
      '
      Me.chkUndefProd.AutoSize = True
      Me.chkUndefProd.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkUndefProd.Location = New System.Drawing.Point(22, 69)
      Me.chkUndefProd.Name = "chkUndefProd"
      Me.chkUndefProd.Size = New System.Drawing.Size(168, 22)
      Me.chkUndefProd.TabIndex = 72
      Me.chkUndefProd.Text = "Undefined Production"
      Me.chkUndefProd.UseVisualStyleBackColor = True
      '
      'grpUndefined
      '
      Me.grpUndefined.Location = New System.Drawing.Point(13, 25)
      Me.grpUndefined.Name = "grpUndefined"
      Me.grpUndefined.Size = New System.Drawing.Size(217, 395)
      Me.grpUndefined.TabIndex = 73
      Me.grpUndefined.TabStop = False
      '
      'frmUndefinedActivities
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackColor = System.Drawing.Color.WhiteSmoke
      Me.ClientSize = New System.Drawing.Size(1096, 432)
      Me.Controls.Add(Me.lblUndefined)
      Me.Controls.Add(Me.chkUndefProd)
      Me.Controls.Add(Me.chkUndefStill)
      Me.Controls.Add(Me.lstUndefStand2)
      Me.Controls.Add(Me.grpUndefined)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Name = "frmUndefinedActivities"
      Me.Text = "frmUndefinedStandstill"
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents lstUndefStand2 As System.Windows.Forms.ListView
   Friend WithEvents lblUndefined As System.Windows.Forms.Label
   Friend WithEvents chkUndefStill As System.Windows.Forms.RadioButton
   Friend WithEvents chkUndefProd As System.Windows.Forms.RadioButton
   Friend WithEvents grpUndefined As System.Windows.Forms.GroupBox
End Class
