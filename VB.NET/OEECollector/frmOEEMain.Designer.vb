<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOEEMain
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
      Me.components = New System.ComponentModel.Container()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOEEMain))
      Me.lblStatus = New System.Windows.Forms.Label()
      Me.tmrPerformJob = New System.Windows.Forms.Timer(Me.components)
      Me.picLogs = New System.Windows.Forms.PictureBox()
      Me.picExit = New System.Windows.Forms.PictureBox()
      Me.pnlDetails = New System.Windows.Forms.Panel()
      Me.pnlMachinesInfo = New System.Windows.Forms.Panel()
      Me.lblInfo = New System.Windows.Forms.Label()
      Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
      Me.picBars = New System.Windows.Forms.PictureBox()
      Me.grpButtons = New System.Windows.Forms.GroupBox()
      Me.picOEEChart = New System.Windows.Forms.PictureBox()
      Me.picSplit = New System.Windows.Forms.PictureBox()
      Me.picUndefinedActi = New System.Windows.Forms.PictureBox()
      Me.BackgroundWorker2 = New System.ComponentModel.BackgroundWorker()
      Me.BackgroundWorker3 = New System.ComponentModel.BackgroundWorker()
      Me.BackgroundWorker4 = New System.ComponentModel.BackgroundWorker()
      Me.BackgroundWorker5 = New System.ComponentModel.BackgroundWorker()
      Me.chkSimulate1 = New System.Windows.Forms.CheckBox()
      Me.SQLSettingsDataSetBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.chkSimulate2 = New System.Windows.Forms.CheckBox()
      Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
      Me.tmrSync = New System.Windows.Forms.Timer(Me.components)
      CType(Me.picLogs, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.picExit, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.picBars, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpButtons.SuspendLayout()
      CType(Me.picOEEChart, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.picSplit, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.picUndefinedActi, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.SQLSettingsDataSetBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'lblStatus
      '
      Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.lblStatus.Location = New System.Drawing.Point(-1, 645)
      Me.lblStatus.Name = "lblStatus"
      Me.lblStatus.Size = New System.Drawing.Size(1096, 88)
      Me.lblStatus.TabIndex = 3
      Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'tmrPerformJob
      '
      Me.tmrPerformJob.Enabled = True
      Me.tmrPerformJob.Interval = 1000
      '
      'picLogs
      '
      Me.picLogs.Image = CType(resources.GetObject("picLogs.Image"), System.Drawing.Image)
      Me.picLogs.Location = New System.Drawing.Point(273, 10)
      Me.picLogs.Name = "picLogs"
      Me.picLogs.Size = New System.Drawing.Size(60, 60)
      Me.picLogs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picLogs.TabIndex = 16
      Me.picLogs.TabStop = False
      '
      'picExit
      '
      Me.picExit.Image = CType(resources.GetObject("picExit.Image"), System.Drawing.Image)
      Me.picExit.Location = New System.Drawing.Point(349, 15)
      Me.picExit.Name = "picExit"
      Me.picExit.Size = New System.Drawing.Size(54, 55)
      Me.picExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picExit.TabIndex = 17
      Me.picExit.TabStop = False
      '
      'pnlDetails
      '
      Me.pnlDetails.BackColor = System.Drawing.Color.WhiteSmoke
      Me.pnlDetails.Location = New System.Drawing.Point(0, 196)
      Me.pnlDetails.Name = "pnlDetails"
      Me.pnlDetails.Size = New System.Drawing.Size(1096, 432)
      Me.pnlDetails.TabIndex = 53
      '
      'pnlMachinesInfo
      '
      Me.pnlMachinesInfo.BackColor = System.Drawing.Color.WhiteSmoke
      Me.pnlMachinesInfo.Location = New System.Drawing.Point(0, 0)
      Me.pnlMachinesInfo.Name = "pnlMachinesInfo"
      Me.pnlMachinesInfo.Size = New System.Drawing.Size(1094, 199)
      Me.pnlMachinesInfo.TabIndex = 56
      '
      'lblInfo
      '
      Me.lblInfo.AutoSize = True
      Me.lblInfo.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblInfo.Location = New System.Drawing.Point(-1, 630)
      Me.lblInfo.Name = "lblInfo"
      Me.lblInfo.Size = New System.Drawing.Size(35, 14)
      Me.lblInfo.TabIndex = 57
      Me.lblInfo.Text = "Info: "
      '
      'BackgroundWorker1
      '
      '
      'picBars
      '
      Me.picBars.BackColor = System.Drawing.Color.White
      Me.picBars.Enabled = False
      Me.picBars.Image = CType(resources.GetObject("picBars.Image"), System.Drawing.Image)
      Me.picBars.Location = New System.Drawing.Point(145, 12)
      Me.picBars.Name = "picBars"
      Me.picBars.Size = New System.Drawing.Size(60, 60)
      Me.picBars.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picBars.TabIndex = 1
      Me.picBars.TabStop = False
      '
      'grpButtons
      '
      Me.grpButtons.Controls.Add(Me.picOEEChart)
      Me.grpButtons.Controls.Add(Me.picSplit)
      Me.grpButtons.Controls.Add(Me.picUndefinedActi)
      Me.grpButtons.Controls.Add(Me.picBars)
      Me.grpButtons.Controls.Add(Me.picExit)
      Me.grpButtons.Controls.Add(Me.picLogs)
      Me.grpButtons.Location = New System.Drawing.Point(671, 646)
      Me.grpButtons.Name = "grpButtons"
      Me.grpButtons.Size = New System.Drawing.Size(411, 78)
      Me.grpButtons.TabIndex = 60
      Me.grpButtons.TabStop = False
      '
      'picOEEChart
      '
      Me.picOEEChart.BackColor = System.Drawing.Color.White
      Me.picOEEChart.Image = CType(resources.GetObject("picOEEChart.Image"), System.Drawing.Image)
      Me.picOEEChart.InitialImage = CType(resources.GetObject("picOEEChart.InitialImage"), System.Drawing.Image)
      Me.picOEEChart.Location = New System.Drawing.Point(210, 10)
      Me.picOEEChart.Name = "picOEEChart"
      Me.picOEEChart.Size = New System.Drawing.Size(48, 60)
      Me.picOEEChart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picOEEChart.TabIndex = 22
      Me.picOEEChart.TabStop = False
      '
      'picSplit
      '
      Me.picSplit.Image = CType(resources.GetObject("picSplit.Image"), System.Drawing.Image)
      Me.picSplit.Location = New System.Drawing.Point(11, 7)
      Me.picSplit.Name = "picSplit"
      Me.picSplit.Size = New System.Drawing.Size(60, 60)
      Me.picSplit.TabIndex = 62
      Me.picSplit.TabStop = False
      '
      'picUndefinedActi
      '
      Me.picUndefinedActi.BackColor = System.Drawing.Color.White
      Me.picUndefinedActi.Image = CType(resources.GetObject("picUndefinedActi.Image"), System.Drawing.Image)
      Me.picUndefinedActi.InitialImage = CType(resources.GetObject("picUndefinedActi.InitialImage"), System.Drawing.Image)
      Me.picUndefinedActi.Location = New System.Drawing.Point(77, 12)
      Me.picUndefinedActi.Name = "picUndefinedActi"
      Me.picUndefinedActi.Size = New System.Drawing.Size(60, 60)
      Me.picUndefinedActi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.picUndefinedActi.TabIndex = 18
      Me.picUndefinedActi.TabStop = False
      '
      'BackgroundWorker2
      '
      '
      'BackgroundWorker3
      '
      '
      'BackgroundWorker4
      '
      '
      'BackgroundWorker5
      '
      '
      'chkSimulate1
      '
      Me.chkSimulate1.AutoSize = True
      Me.chkSimulate1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkSimulate1.Location = New System.Drawing.Point(13, 666)
      Me.chkSimulate1.Name = "chkSimulate1"
      Me.chkSimulate1.Size = New System.Drawing.Size(84, 19)
      Me.chkSimulate1.TabIndex = 61
      Me.chkSimulate1.Text = "Simulate 1"
      Me.chkSimulate1.UseVisualStyleBackColor = True
      Me.chkSimulate1.Visible = False
      '
      'chkSimulate2
      '
      Me.chkSimulate2.AutoSize = True
      Me.chkSimulate2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkSimulate2.Location = New System.Drawing.Point(12, 691)
      Me.chkSimulate2.Name = "chkSimulate2"
      Me.chkSimulate2.Size = New System.Drawing.Size(84, 19)
      Me.chkSimulate2.TabIndex = 63
      Me.chkSimulate2.Text = "Simulate 2"
      Me.chkSimulate2.UseVisualStyleBackColor = True
      Me.chkSimulate2.Visible = False
      '
      'tmrRefresh
      '
      Me.tmrRefresh.Enabled = True
      '
      'tmrSync
      '
      Me.tmrSync.Enabled = True
      Me.tmrSync.Interval = 60000
      '
      'frmOEEMain
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackColor = System.Drawing.Color.White
      Me.ClientSize = New System.Drawing.Size(1094, 731)
      Me.Controls.Add(Me.chkSimulate2)
      Me.Controls.Add(Me.chkSimulate1)
      Me.Controls.Add(Me.grpButtons)
      Me.Controls.Add(Me.lblInfo)
      Me.Controls.Add(Me.pnlMachinesInfo)
      Me.Controls.Add(Me.pnlDetails)
      Me.Controls.Add(Me.lblStatus)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "frmOEEMain"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "OEECollector - Inputscreen"
      CType(Me.picLogs, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.picExit, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.picBars, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpButtons.ResumeLayout(False)
      CType(Me.picOEEChart, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.picSplit, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.picUndefinedActi, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.SQLSettingsDataSetBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents lblStatus As System.Windows.Forms.Label
   Friend WithEvents tmrPerformJob As System.Windows.Forms.Timer
   Friend WithEvents picLogs As System.Windows.Forms.PictureBox
   Friend WithEvents picExit As System.Windows.Forms.PictureBox
   Friend WithEvents SQLSettingsDataSetBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents pnlDetails As System.Windows.Forms.Panel
   Friend WithEvents pnlMachinesInfo As System.Windows.Forms.Panel
   Friend WithEvents lblInfo As System.Windows.Forms.Label
   Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
   Friend WithEvents picBars As System.Windows.Forms.PictureBox
   Friend WithEvents grpButtons As System.Windows.Forms.GroupBox
   Friend WithEvents picUndefinedActi As System.Windows.Forms.PictureBox
   Friend WithEvents picOEEChart As System.Windows.Forms.PictureBox
   Friend WithEvents BackgroundWorker2 As System.ComponentModel.BackgroundWorker
   Friend WithEvents BackgroundWorker3 As System.ComponentModel.BackgroundWorker
   Friend WithEvents BackgroundWorker4 As System.ComponentModel.BackgroundWorker
   Friend WithEvents BackgroundWorker5 As System.ComponentModel.BackgroundWorker
   Friend WithEvents chkSimulate1 As System.Windows.Forms.CheckBox
   Friend WithEvents picSplit As System.Windows.Forms.PictureBox
   Friend WithEvents chkSimulate2 As System.Windows.Forms.CheckBox
   Friend WithEvents tmrRefresh As System.Windows.Forms.Timer
   Friend WithEvents tmrSync As System.Windows.Forms.Timer
End Class
