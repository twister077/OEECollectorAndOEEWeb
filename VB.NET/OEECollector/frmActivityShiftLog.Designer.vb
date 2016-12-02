<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmActivityShiftLog
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmActivityShiftLog))
      Me.chkShiftlog = New System.Windows.Forms.RadioButton()
      Me.chkActivityLog = New System.Windows.Forms.RadioButton()
      Me.lblActiLogs = New System.Windows.Forms.Label()
      Me.lstActivity = New System.Windows.Forms.ListView()
      Me.grpUndefined = New System.Windows.Forms.GroupBox()
      Me.chkInclShortbreaks = New System.Windows.Forms.CheckBox()
      Me.cmdSubmitLog = New System.Windows.Forms.Button()
      Me.txtLogEdit = New System.Windows.Forms.TextBox()
      Me.lblLogView = New System.Windows.Forms.Label()
      Me.picBack = New System.Windows.Forms.PictureBox()
      Me.grpUndefined.SuspendLayout()
      CType(Me.picBack, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'chkShiftlog
      '
      Me.chkShiftlog.AutoSize = True
      Me.chkShiftlog.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkShiftlog.Location = New System.Drawing.Point(22, 69)
      Me.chkShiftlog.Name = "chkShiftlog"
      Me.chkShiftlog.Size = New System.Drawing.Size(79, 22)
      Me.chkShiftlog.TabIndex = 77
      Me.chkShiftlog.Text = "Shift log"
      Me.chkShiftlog.UseVisualStyleBackColor = True
      '
      'chkActivityLog
      '
      Me.chkActivityLog.AutoSize = True
      Me.chkActivityLog.Checked = True
      Me.chkActivityLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkActivityLog.Location = New System.Drawing.Point(22, 42)
      Me.chkActivityLog.Name = "chkActivityLog"
      Me.chkActivityLog.Size = New System.Drawing.Size(95, 22)
      Me.chkActivityLog.TabIndex = 76
      Me.chkActivityLog.TabStop = True
      Me.chkActivityLog.Text = "Activity log"
      Me.chkActivityLog.UseVisualStyleBackColor = True
      '
      'lblActiLogs
      '
      Me.lblActiLogs.AutoSize = True
      Me.lblActiLogs.BackColor = System.Drawing.Color.Transparent
      Me.lblActiLogs.Font = New System.Drawing.Font("Calibri", 16.0!)
      Me.lblActiLogs.Location = New System.Drawing.Point(16, 3)
      Me.lblActiLogs.Name = "lblActiLogs"
      Me.lblActiLogs.Size = New System.Drawing.Size(99, 27)
      Me.lblActiLogs.TabIndex = 75
      Me.lblActiLogs.Text = "Activities:"
      '
      'lstActivity
      '
      Me.lstActivity.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.lstActivity.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lstActivity.FullRowSelect = True
      Me.lstActivity.Location = New System.Drawing.Point(236, 33)
      Me.lstActivity.Name = "lstActivity"
      Me.lstActivity.Size = New System.Drawing.Size(848, 387)
      Me.lstActivity.TabIndex = 74
      Me.lstActivity.UseCompatibleStateImageBehavior = False
      Me.lstActivity.View = System.Windows.Forms.View.Details
      '
      'grpUndefined
      '
      Me.grpUndefined.Controls.Add(Me.picBack)
      Me.grpUndefined.Controls.Add(Me.chkInclShortbreaks)
      Me.grpUndefined.Controls.Add(Me.cmdSubmitLog)
      Me.grpUndefined.Location = New System.Drawing.Point(13, 25)
      Me.grpUndefined.Name = "grpUndefined"
      Me.grpUndefined.Size = New System.Drawing.Size(217, 395)
      Me.grpUndefined.TabIndex = 78
      Me.grpUndefined.TabStop = False
      '
      'chkInclShortbreaks
      '
      Me.chkInclShortbreaks.AutoSize = True
      Me.chkInclShortbreaks.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkInclShortbreaks.Location = New System.Drawing.Point(9, 98)
      Me.chkInclShortbreaks.Name = "chkInclShortbreaks"
      Me.chkInclShortbreaks.Size = New System.Drawing.Size(160, 22)
      Me.chkInclShortbreaks.TabIndex = 0
      Me.chkInclShortbreaks.Text = "Include short breaks"
      Me.chkInclShortbreaks.UseVisualStyleBackColor = True
      '
      'cmdSubmitLog
      '
      Me.cmdSubmitLog.BackColor = System.Drawing.Color.LightCyan
      Me.cmdSubmitLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.cmdSubmitLog.Location = New System.Drawing.Point(6, 358)
      Me.cmdSubmitLog.Name = "cmdSubmitLog"
      Me.cmdSubmitLog.Size = New System.Drawing.Size(205, 31)
      Me.cmdSubmitLog.TabIndex = 80
      Me.cmdSubmitLog.Text = "Submit"
      Me.cmdSubmitLog.UseVisualStyleBackColor = False
      Me.cmdSubmitLog.Visible = False
      '
      'txtLogEdit
      '
      Me.txtLogEdit.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.txtLogEdit.Location = New System.Drawing.Point(236, 284)
      Me.txtLogEdit.Multiline = True
      Me.txtLogEdit.Name = "txtLogEdit"
      Me.txtLogEdit.Size = New System.Drawing.Size(848, 136)
      Me.txtLogEdit.TabIndex = 79
      '
      'lblLogView
      '
      Me.lblLogView.BackColor = System.Drawing.Color.White
      Me.lblLogView.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblLogView.Location = New System.Drawing.Point(240, 33)
      Me.lblLogView.Name = "lblLogView"
      Me.lblLogView.Size = New System.Drawing.Size(844, 242)
      Me.lblLogView.TabIndex = 82
      '
      'picBack
      '
      Me.picBack.Image = CType(resources.GetObject("picBack.Image"), System.Drawing.Image)
      Me.picBack.Location = New System.Drawing.Point(73, 278)
      Me.picBack.Name = "picBack"
      Me.picBack.Size = New System.Drawing.Size(70, 70)
      Me.picBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picBack.TabIndex = 81
      Me.picBack.TabStop = False
      Me.picBack.Visible = False
      '
      'frmActivityShiftLog
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackColor = System.Drawing.Color.WhiteSmoke
      Me.ClientSize = New System.Drawing.Size(1096, 432)
      Me.Controls.Add(Me.lblLogView)
      Me.Controls.Add(Me.txtLogEdit)
      Me.Controls.Add(Me.chkShiftlog)
      Me.Controls.Add(Me.chkActivityLog)
      Me.Controls.Add(Me.lblActiLogs)
      Me.Controls.Add(Me.lstActivity)
      Me.Controls.Add(Me.grpUndefined)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Name = "frmActivityShiftLog"
      Me.Text = "frmActivityShiftLog"
      Me.grpUndefined.ResumeLayout(False)
      Me.grpUndefined.PerformLayout()
      CType(Me.picBack, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents chkShiftlog As System.Windows.Forms.RadioButton
   Friend WithEvents chkActivityLog As System.Windows.Forms.RadioButton
   Friend WithEvents lblActiLogs As System.Windows.Forms.Label
   Friend WithEvents lstActivity As System.Windows.Forms.ListView
   Friend WithEvents grpUndefined As System.Windows.Forms.GroupBox
   Friend WithEvents txtLogEdit As System.Windows.Forms.TextBox
   Friend WithEvents cmdSubmitLog As System.Windows.Forms.Button
   Friend WithEvents lblLogView As System.Windows.Forms.Label
   Friend WithEvents chkInclShortbreaks As System.Windows.Forms.CheckBox
   Friend WithEvents picBack As System.Windows.Forms.PictureBox
End Class
