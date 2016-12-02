<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessageBox
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessageBox))
      Me.picOk = New System.Windows.Forms.PictureBox()
      Me.picNo = New System.Windows.Forms.PictureBox()
      Me.lblTitle = New System.Windows.Forms.Label()
      Me.lblMessage = New System.Windows.Forms.Label()
      Me.PictureBox1 = New System.Windows.Forms.PictureBox()
      Me.grpMessage = New System.Windows.Forms.GroupBox()
      CType(Me.picOk, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.picNo, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpMessage.SuspendLayout()
      Me.SuspendLayout()
      '
      'picOk
      '
      Me.picOk.BackColor = System.Drawing.Color.White
      Me.picOk.Image = CType(resources.GetObject("picOk.Image"), System.Drawing.Image)
      Me.picOk.Location = New System.Drawing.Point(17, 14)
      Me.picOk.Name = "picOk"
      Me.picOk.Size = New System.Drawing.Size(70, 70)
      Me.picOk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picOk.TabIndex = 0
      Me.picOk.TabStop = False
      '
      'picNo
      '
      Me.picNo.Image = CType(resources.GetObject("picNo.Image"), System.Drawing.Image)
      Me.picNo.Location = New System.Drawing.Point(105, 19)
      Me.picNo.Name = "picNo"
      Me.picNo.Size = New System.Drawing.Size(60, 60)
      Me.picNo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picNo.TabIndex = 1
      Me.picNo.TabStop = False
      '
      'lblTitle
      '
      Me.lblTitle.BackColor = System.Drawing.Color.White
      Me.lblTitle.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblTitle.Location = New System.Drawing.Point(12, 12)
      Me.lblTitle.Name = "lblTitle"
      Me.lblTitle.Size = New System.Drawing.Size(422, 26)
      Me.lblTitle.TabIndex = 2
      Me.lblTitle.Text = "......."
      Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      '
      'lblMessage
      '
      Me.lblMessage.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblMessage.Location = New System.Drawing.Point(30, 68)
      Me.lblMessage.Name = "lblMessage"
      Me.lblMessage.Size = New System.Drawing.Size(396, 93)
      Me.lblMessage.TabIndex = 3
      Me.lblMessage.Text = "......."
      Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'PictureBox1
      '
      Me.PictureBox1.BackColor = System.Drawing.Color.White
      Me.PictureBox1.Location = New System.Drawing.Point(6, 6)
      Me.PictureBox1.Name = "PictureBox1"
      Me.PictureBox1.Size = New System.Drawing.Size(444, 38)
      Me.PictureBox1.TabIndex = 4
      Me.PictureBox1.TabStop = False
      '
      'grpMessage
      '
      Me.grpMessage.BackColor = System.Drawing.Color.White
      Me.grpMessage.Controls.Add(Me.picOk)
      Me.grpMessage.Controls.Add(Me.picNo)
      Me.grpMessage.Location = New System.Drawing.Point(262, 164)
      Me.grpMessage.Name = "grpMessage"
      Me.grpMessage.Size = New System.Drawing.Size(183, 92)
      Me.grpMessage.TabIndex = 5
      Me.grpMessage.TabStop = False
      '
      'frmMessageBox
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackColor = System.Drawing.Color.Snow
      Me.ClientSize = New System.Drawing.Size(457, 268)
      Me.ControlBox = False
      Me.Controls.Add(Me.grpMessage)
      Me.Controls.Add(Me.lblTitle)
      Me.Controls.Add(Me.PictureBox1)
      Me.Controls.Add(Me.lblMessage)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
      Me.Name = "frmMessageBox"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      CType(Me.picOk, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.picNo, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpMessage.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub
   Friend WithEvents picOk As System.Windows.Forms.PictureBox
   Friend WithEvents picNo As System.Windows.Forms.PictureBox
   Friend WithEvents lblTitle As System.Windows.Forms.Label
   Friend WithEvents lblMessage As System.Windows.Forms.Label
   Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
   Friend WithEvents grpMessage As System.Windows.Forms.GroupBox
End Class
