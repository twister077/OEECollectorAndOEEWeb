<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMachineDetails
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
      Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
      Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
      Dim LegendItem1 As System.Windows.Forms.DataVisualization.Charting.LegendItem = New System.Windows.Forms.DataVisualization.Charting.LegendItem()
      Dim LegendItem2 As System.Windows.Forms.DataVisualization.Charting.LegendItem = New System.Windows.Forms.DataVisualization.Charting.LegendItem()
      Dim LegendItem3 As System.Windows.Forms.DataVisualization.Charting.LegendItem = New System.Windows.Forms.DataVisualization.Charting.LegendItem()
      Dim LegendItem4 As System.Windows.Forms.DataVisualization.Charting.LegendItem = New System.Windows.Forms.DataVisualization.Charting.LegendItem()
      Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
      Dim DataPoint1 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 0.0R)
      Dim DataPoint2 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 0.0R)
      Dim DataPoint3 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 0.0R)
      Dim DataPoint4 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 0.0R)
      Me.lblTeamname = New System.Windows.Forms.Label()
      Me.lblTeam = New System.Windows.Forms.Label()
      Me.lblTimenr = New System.Windows.Forms.Label()
      Me.lblTime = New System.Windows.Forms.Label()
      Me.lblNormSpeed = New System.Windows.Forms.Label()
      Me.lblNormSpeednr = New System.Windows.Forms.Label()
      Me.lblLineSpeed = New System.Windows.Forms.Label()
      Me.lblLineSpeednr = New System.Windows.Forms.Label()
      Me.lblShiftname = New System.Windows.Forms.Label()
      Me.lblShift = New System.Windows.Forms.Label()
      Me.lblMachine = New System.Windows.Forms.Label()
      Me.ChartOEE = New System.Windows.Forms.DataVisualization.Charting.Chart()
      Me.lblActivitygroupname = New System.Windows.Forms.Label()
      Me.lblCounter = New System.Windows.Forms.Label()
      Me.lblCounterValue = New System.Windows.Forms.Label()
      Me.lblMachineUnit = New System.Windows.Forms.Label()
      Me.grpMachInfo = New System.Windows.Forms.GroupBox()
      Me.lblMachinename = New System.Windows.Forms.Label()
      Me.grpModuleActi = New System.Windows.Forms.GroupBox()
      Me.grpActivityInfo = New System.Windows.Forms.GroupBox()
      Me.lblActivity = New System.Windows.Forms.Label()
      Me.lblActivitygroup = New System.Windows.Forms.Label()
      Me.lblActivityname = New System.Windows.Forms.Label()
      CType(Me.ChartOEE, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpMachInfo.SuspendLayout()
      Me.grpModuleActi.SuspendLayout()
      Me.grpActivityInfo.SuspendLayout()
      Me.SuspendLayout()
      '
      'lblTeamname
      '
      Me.lblTeamname.BackColor = System.Drawing.Color.Transparent
      Me.lblTeamname.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblTeamname.Location = New System.Drawing.Point(151, 36)
      Me.lblTeamname.Name = "lblTeamname"
      Me.lblTeamname.Size = New System.Drawing.Size(200, 24)
      Me.lblTeamname.TabIndex = 79
      '
      'lblTeam
      '
      Me.lblTeam.AutoSize = True
      Me.lblTeam.BackColor = System.Drawing.Color.Transparent
      Me.lblTeam.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblTeam.Location = New System.Drawing.Point(6, 36)
      Me.lblTeam.Name = "lblTeam"
      Me.lblTeam.Size = New System.Drawing.Size(63, 26)
      Me.lblTeam.TabIndex = 78
      Me.lblTeam.Text = "Team:"
      '
      'lblTimenr
      '
      Me.lblTimenr.AutoSize = True
      Me.lblTimenr.BackColor = System.Drawing.Color.Transparent
      Me.lblTimenr.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblTimenr.Location = New System.Drawing.Point(151, 60)
      Me.lblTimenr.Name = "lblTimenr"
      Me.lblTimenr.Size = New System.Drawing.Size(40, 26)
      Me.lblTimenr.TabIndex = 77
      Me.lblTimenr.Text = "7:7"
      '
      'lblTime
      '
      Me.lblTime.AutoSize = True
      Me.lblTime.BackColor = System.Drawing.Color.Transparent
      Me.lblTime.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblTime.Location = New System.Drawing.Point(6, 60)
      Me.lblTime.Name = "lblTime"
      Me.lblTime.Size = New System.Drawing.Size(60, 26)
      Me.lblTime.TabIndex = 76
      Me.lblTime.Text = "Time:"
      '
      'lblNormSpeed
      '
      Me.lblNormSpeed.AutoSize = True
      Me.lblNormSpeed.BackColor = System.Drawing.Color.Transparent
      Me.lblNormSpeed.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblNormSpeed.Location = New System.Drawing.Point(6, 37)
      Me.lblNormSpeed.Name = "lblNormSpeed"
      Me.lblNormSpeed.Size = New System.Drawing.Size(122, 26)
      Me.lblNormSpeed.TabIndex = 71
      Me.lblNormSpeed.Text = "Norm speed:"
      '
      'lblNormSpeednr
      '
      Me.lblNormSpeednr.BackColor = System.Drawing.Color.Transparent
      Me.lblNormSpeednr.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold)
      Me.lblNormSpeednr.Location = New System.Drawing.Point(127, 37)
      Me.lblNormSpeednr.Name = "lblNormSpeednr"
      Me.lblNormSpeednr.Size = New System.Drawing.Size(54, 24)
      Me.lblNormSpeednr.TabIndex = 70
      Me.lblNormSpeednr.Text = "0"
      Me.lblNormSpeednr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'lblLineSpeed
      '
      Me.lblLineSpeed.AutoSize = True
      Me.lblLineSpeed.BackColor = System.Drawing.Color.Transparent
      Me.lblLineSpeed.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblLineSpeed.Location = New System.Drawing.Point(6, 12)
      Me.lblLineSpeed.Name = "lblLineSpeed"
      Me.lblLineSpeed.Size = New System.Drawing.Size(108, 26)
      Me.lblLineSpeed.TabIndex = 69
      Me.lblLineSpeed.Text = "Line speed:"
      '
      'lblLineSpeednr
      '
      Me.lblLineSpeednr.BackColor = System.Drawing.Color.Transparent
      Me.lblLineSpeednr.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold)
      Me.lblLineSpeednr.Location = New System.Drawing.Point(127, 12)
      Me.lblLineSpeednr.Name = "lblLineSpeednr"
      Me.lblLineSpeednr.Size = New System.Drawing.Size(54, 24)
      Me.lblLineSpeednr.TabIndex = 68
      Me.lblLineSpeednr.Text = "0"
      Me.lblLineSpeednr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'lblShiftname
      '
      Me.lblShiftname.BackColor = System.Drawing.Color.Transparent
      Me.lblShiftname.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblShiftname.Location = New System.Drawing.Point(151, 12)
      Me.lblShiftname.Name = "lblShiftname"
      Me.lblShiftname.Size = New System.Drawing.Size(200, 24)
      Me.lblShiftname.TabIndex = 67
      '
      'lblShift
      '
      Me.lblShift.AutoSize = True
      Me.lblShift.BackColor = System.Drawing.Color.Transparent
      Me.lblShift.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblShift.Location = New System.Drawing.Point(6, 12)
      Me.lblShift.Name = "lblShift"
      Me.lblShift.Size = New System.Drawing.Size(57, 26)
      Me.lblShift.TabIndex = 66
      Me.lblShift.Text = "Shift:"
      '
      'lblMachine
      '
      Me.lblMachine.AutoSize = True
      Me.lblMachine.BackColor = System.Drawing.Color.Transparent
      Me.lblMachine.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblMachine.Location = New System.Drawing.Point(14, 14)
      Me.lblMachine.Name = "lblMachine"
      Me.lblMachine.Size = New System.Drawing.Size(107, 29)
      Me.lblMachine.TabIndex = 62
      Me.lblMachine.Text = "Machine:"
      '
      'ChartOEE
      '
      ChartArea1.CursorY.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
      ChartArea1.Name = "ChartArea1"
      Me.ChartOEE.ChartAreas.Add(ChartArea1)
      LegendItem1.BorderColor = System.Drawing.Color.Empty
      LegendItem1.Color = System.Drawing.Color.Yellow
      LegendItem1.Name = "Availability"
      LegendItem2.BorderColor = System.Drawing.Color.Empty
      LegendItem2.Color = System.Drawing.Color.Red
      LegendItem2.Name = "Performance"
      LegendItem3.BorderColor = System.Drawing.Color.Empty
      LegendItem3.Color = System.Drawing.Color.Blue
      LegendItem3.Name = "Quality"
      LegendItem4.BorderColor = System.Drawing.Color.Empty
      LegendItem4.Color = System.Drawing.Color.Lime
      LegendItem4.Name = "OEE"
      Legend1.CustomItems.Add(LegendItem1)
      Legend1.CustomItems.Add(LegendItem2)
      Legend1.CustomItems.Add(LegendItem3)
      Legend1.CustomItems.Add(LegendItem4)
      Legend1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Legend1.IsTextAutoFit = False
      Legend1.Name = "Legend1"
      Me.ChartOEE.Legends.Add(Legend1)
      Me.ChartOEE.Location = New System.Drawing.Point(786, 12)
      Me.ChartOEE.Name = "ChartOEE"
      Series1.ChartArea = "ChartArea1"
      Series1.IsVisibleInLegend = False
      Series1.Legend = "Legend1"
      Series1.Name = "Series1"
      DataPoint1.Color = System.Drawing.Color.Yellow
      DataPoint1.IsVisibleInLegend = True
      DataPoint1.LegendText = "Performance"
      DataPoint2.Color = System.Drawing.Color.Red
      DataPoint3.Color = System.Drawing.Color.Blue
      DataPoint4.Color = System.Drawing.Color.Lime
      Series1.Points.Add(DataPoint1)
      Series1.Points.Add(DataPoint2)
      Series1.Points.Add(DataPoint3)
      Series1.Points.Add(DataPoint4)
      Series1.SmartLabelStyle.Enabled = False
      Series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[String]
      Me.ChartOEE.Series.Add(Series1)
      Me.ChartOEE.Size = New System.Drawing.Size(296, 175)
      Me.ChartOEE.TabIndex = 60
      Me.ChartOEE.Text = "OEE"
      '
      'lblActivitygroupname
      '
      Me.lblActivitygroupname.BackColor = System.Drawing.Color.Transparent
      Me.lblActivitygroupname.Font = New System.Drawing.Font("Calibri", 16.0!, System.Drawing.FontStyle.Bold)
      Me.lblActivitygroupname.Location = New System.Drawing.Point(151, 14)
      Me.lblActivitygroupname.Name = "lblActivitygroupname"
      Me.lblActivitygroupname.Size = New System.Drawing.Size(201, 24)
      Me.lblActivitygroupname.TabIndex = 57
      '
      'lblCounter
      '
      Me.lblCounter.AutoSize = True
      Me.lblCounter.BackColor = System.Drawing.Color.Transparent
      Me.lblCounter.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblCounter.Location = New System.Drawing.Point(6, 61)
      Me.lblCounter.Name = "lblCounter"
      Me.lblCounter.Size = New System.Drawing.Size(86, 26)
      Me.lblCounter.TabIndex = 80
      Me.lblCounter.Text = "Counter:"
      '
      'lblCounterValue
      '
      Me.lblCounterValue.BackColor = System.Drawing.Color.Transparent
      Me.lblCounterValue.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold)
      Me.lblCounterValue.Location = New System.Drawing.Point(98, 61)
      Me.lblCounterValue.Name = "lblCounterValue"
      Me.lblCounterValue.Size = New System.Drawing.Size(83, 24)
      Me.lblCounterValue.TabIndex = 81
      Me.lblCounterValue.Text = "0"
      Me.lblCounterValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'lblMachineUnit
      '
      Me.lblMachineUnit.BackColor = System.Drawing.Color.Transparent
      Me.lblMachineUnit.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblMachineUnit.Location = New System.Drawing.Point(178, 61)
      Me.lblMachineUnit.Name = "lblMachineUnit"
      Me.lblMachineUnit.Size = New System.Drawing.Size(85, 24)
      Me.lblMachineUnit.TabIndex = 82
      '
      'grpMachInfo
      '
      Me.grpMachInfo.BackColor = System.Drawing.Color.Transparent
      Me.grpMachInfo.Controls.Add(Me.lblShiftname)
      Me.grpMachInfo.Controls.Add(Me.lblTimenr)
      Me.grpMachInfo.Controls.Add(Me.lblTeamname)
      Me.grpMachInfo.Controls.Add(Me.lblShift)
      Me.grpMachInfo.Controls.Add(Me.lblTime)
      Me.grpMachInfo.Controls.Add(Me.lblTeam)
      Me.grpMachInfo.Location = New System.Drawing.Point(10, 48)
      Me.grpMachInfo.Name = "grpMachInfo"
      Me.grpMachInfo.Size = New System.Drawing.Size(357, 93)
      Me.grpMachInfo.TabIndex = 83
      Me.grpMachInfo.TabStop = False
      '
      'lblMachinename
      '
      Me.lblMachinename.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold)
      Me.lblMachinename.Location = New System.Drawing.Point(160, 14)
      Me.lblMachinename.Name = "lblMachinename"
      Me.lblMachinename.Size = New System.Drawing.Size(259, 29)
      Me.lblMachinename.TabIndex = 85
      '
      'grpModuleActi
      '
      Me.grpModuleActi.BackColor = System.Drawing.Color.Transparent
      Me.grpModuleActi.Controls.Add(Me.lblLineSpeed)
      Me.grpModuleActi.Controls.Add(Me.lblLineSpeednr)
      Me.grpModuleActi.Controls.Add(Me.lblNormSpeednr)
      Me.grpModuleActi.Controls.Add(Me.lblMachineUnit)
      Me.grpModuleActi.Controls.Add(Me.lblNormSpeed)
      Me.grpModuleActi.Controls.Add(Me.lblCounterValue)
      Me.grpModuleActi.Controls.Add(Me.lblCounter)
      Me.grpModuleActi.Location = New System.Drawing.Point(373, 48)
      Me.grpModuleActi.Name = "grpModuleActi"
      Me.grpModuleActi.Size = New System.Drawing.Size(407, 93)
      Me.grpModuleActi.TabIndex = 84
      Me.grpModuleActi.TabStop = False
      '
      'grpActivityInfo
      '
      Me.grpActivityInfo.BackColor = System.Drawing.Color.Transparent
      Me.grpActivityInfo.Controls.Add(Me.lblActivity)
      Me.grpActivityInfo.Controls.Add(Me.lblActivitygroup)
      Me.grpActivityInfo.Controls.Add(Me.lblActivitygroupname)
      Me.grpActivityInfo.Controls.Add(Me.lblActivityname)
      Me.grpActivityInfo.Location = New System.Drawing.Point(10, 140)
      Me.grpActivityInfo.Name = "grpActivityInfo"
      Me.grpActivityInfo.Size = New System.Drawing.Size(770, 47)
      Me.grpActivityInfo.TabIndex = 86
      Me.grpActivityInfo.TabStop = False
      '
      'lblActivity
      '
      Me.lblActivity.AutoSize = True
      Me.lblActivity.BackColor = System.Drawing.Color.Transparent
      Me.lblActivity.Font = New System.Drawing.Font("Calibri", 15.75!)
      Me.lblActivity.Location = New System.Drawing.Point(369, 14)
      Me.lblActivity.Name = "lblActivity"
      Me.lblActivity.Size = New System.Drawing.Size(82, 26)
      Me.lblActivity.TabIndex = 88
      Me.lblActivity.Text = "Activity:"
      '
      'lblActivitygroup
      '
      Me.lblActivitygroup.AutoSize = True
      Me.lblActivitygroup.BackColor = System.Drawing.Color.Transparent
      Me.lblActivitygroup.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblActivitygroup.Location = New System.Drawing.Point(6, 14)
      Me.lblActivitygroup.Name = "lblActivitygroup"
      Me.lblActivitygroup.Size = New System.Drawing.Size(137, 26)
      Me.lblActivitygroup.TabIndex = 88
      Me.lblActivitygroup.Text = "Activity group:"
      '
      'lblActivityname
      '
      Me.lblActivityname.BackColor = System.Drawing.Color.Transparent
      Me.lblActivityname.Font = New System.Drawing.Font("Calibri", 16.0!, System.Drawing.FontStyle.Bold)
      Me.lblActivityname.Location = New System.Drawing.Point(485, 14)
      Me.lblActivityname.Name = "lblActivityname"
      Me.lblActivityname.Size = New System.Drawing.Size(279, 24)
      Me.lblActivityname.TabIndex = 65
      '
      'frmMachineDetails
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackColor = System.Drawing.Color.WhiteSmoke
      Me.ClientSize = New System.Drawing.Size(1094, 199)
      Me.Controls.Add(Me.lblMachinename)
      Me.Controls.Add(Me.grpActivityInfo)
      Me.Controls.Add(Me.grpModuleActi)
      Me.Controls.Add(Me.lblMachine)
      Me.Controls.Add(Me.grpMachInfo)
      Me.Controls.Add(Me.ChartOEE)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Name = "frmMachineDetails"
      Me.Text = "frmMachineDetails"
      CType(Me.ChartOEE, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpMachInfo.ResumeLayout(False)
      Me.grpMachInfo.PerformLayout()
      Me.grpModuleActi.ResumeLayout(False)
      Me.grpModuleActi.PerformLayout()
      Me.grpActivityInfo.ResumeLayout(False)
      Me.grpActivityInfo.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents lblTeamname As System.Windows.Forms.Label
   Friend WithEvents lblTeam As System.Windows.Forms.Label
   Friend WithEvents lblTimenr As System.Windows.Forms.Label
   Friend WithEvents lblTime As System.Windows.Forms.Label
   Friend WithEvents lblNormSpeed As System.Windows.Forms.Label
   Friend WithEvents lblNormSpeednr As System.Windows.Forms.Label
   Friend WithEvents lblLineSpeed As System.Windows.Forms.Label
   Friend WithEvents lblLineSpeednr As System.Windows.Forms.Label
   Friend WithEvents lblShiftname As System.Windows.Forms.Label
   Friend WithEvents lblShift As System.Windows.Forms.Label
   Friend WithEvents lblMachine As System.Windows.Forms.Label
   Friend WithEvents ChartOEE As System.Windows.Forms.DataVisualization.Charting.Chart
   Friend WithEvents lblActivitygroupname As System.Windows.Forms.Label
   Friend WithEvents lblCounter As System.Windows.Forms.Label
   Friend WithEvents lblCounterValue As System.Windows.Forms.Label
   Friend WithEvents lblMachineUnit As System.Windows.Forms.Label
   Friend WithEvents grpMachInfo As System.Windows.Forms.GroupBox
   Friend WithEvents grpModuleActi As System.Windows.Forms.GroupBox
   Friend WithEvents lblMachinename As System.Windows.Forms.Label
   Friend WithEvents grpActivityInfo As System.Windows.Forms.GroupBox
   Friend WithEvents lblActivitygroup As System.Windows.Forms.Label
   Friend WithEvents lblActivityname As System.Windows.Forms.Label
   Friend WithEvents lblActivity As System.Windows.Forms.Label
End Class
