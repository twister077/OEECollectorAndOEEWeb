<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmActivityStacked
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
      Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
      Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
      Me.chaActivityChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
      Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
      Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
      Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
      Me.lblToDate = New System.Windows.Forms.Label()
      Me.grpSelection = New System.Windows.Forms.GroupBox()
      Me.chkInclUnscheduled = New System.Windows.Forms.CheckBox()
      Me.chkFromToDate = New System.Windows.Forms.RadioButton()
      Me.chkLast24Hours = New System.Windows.Forms.RadioButton()
      Me.chkCurrentShift = New System.Windows.Forms.RadioButton()
      Me.dtpToTime = New System.Windows.Forms.DateTimePicker()
      Me.dtpFromTime = New System.Windows.Forms.DateTimePicker()
      Me.lblStopActivities = New System.Windows.Forms.Label()
      CType(Me.chaActivityChart, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpSelection.SuspendLayout()
      Me.SuspendLayout()
      '
      'chaActivityChart
      '
      ChartArea3.Name = "ChartArea1"
      Me.chaActivityChart.ChartAreas.Add(ChartArea3)
      Me.chaActivityChart.Location = New System.Drawing.Point(236, 33)
      Me.chaActivityChart.Name = "chaActivityChart"
      Series3.ChartArea = "ChartArea1"
      Series3.Name = "Series1"
      Me.chaActivityChart.Series.Add(Series3)
      Me.chaActivityChart.Size = New System.Drawing.Size(848, 387)
      Me.chaActivityChart.TabIndex = 0
      Me.chaActivityChart.Text = "Chart1"
      '
      'tmrRefresh
      '
      Me.tmrRefresh.Interval = 500
      '
      'dtpFromDate
      '
      Me.dtpFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.dtpFromDate.Location = New System.Drawing.Point(27, 98)
      Me.dtpFromDate.MaxDate = New Date(2777, 1, 31, 0, 0, 0, 0)
      Me.dtpFromDate.Name = "dtpFromDate"
      Me.dtpFromDate.Size = New System.Drawing.Size(180, 24)
      Me.dtpFromDate.TabIndex = 1
      '
      'dtpToDate
      '
      Me.dtpToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.dtpToDate.Location = New System.Drawing.Point(27, 179)
      Me.dtpToDate.Name = "dtpToDate"
      Me.dtpToDate.Size = New System.Drawing.Size(180, 24)
      Me.dtpToDate.TabIndex = 2
      '
      'lblToDate
      '
      Me.lblToDate.AutoSize = True
      Me.lblToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.lblToDate.Location = New System.Drawing.Point(24, 154)
      Me.lblToDate.Name = "lblToDate"
      Me.lblToDate.Size = New System.Drawing.Size(30, 18)
      Me.lblToDate.TabIndex = 4
      Me.lblToDate.Text = "To:"
      '
      'grpSelection
      '
      Me.grpSelection.Controls.Add(Me.chkInclUnscheduled)
      Me.grpSelection.Controls.Add(Me.dtpFromDate)
      Me.grpSelection.Controls.Add(Me.chkFromToDate)
      Me.grpSelection.Controls.Add(Me.chkLast24Hours)
      Me.grpSelection.Controls.Add(Me.chkCurrentShift)
      Me.grpSelection.Controls.Add(Me.dtpToTime)
      Me.grpSelection.Controls.Add(Me.dtpFromTime)
      Me.grpSelection.Controls.Add(Me.dtpToDate)
      Me.grpSelection.Controls.Add(Me.lblToDate)
      Me.grpSelection.Location = New System.Drawing.Point(13, 25)
      Me.grpSelection.Name = "grpSelection"
      Me.grpSelection.Size = New System.Drawing.Size(217, 395)
      Me.grpSelection.TabIndex = 10
      Me.grpSelection.TabStop = False
      '
      'chkInclUnscheduled
      '
      Me.chkInclUnscheduled.AutoSize = True
      Me.chkInclUnscheduled.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkInclUnscheduled.Location = New System.Drawing.Point(9, 254)
      Me.chkInclUnscheduled.Name = "chkInclUnscheduled"
      Me.chkInclUnscheduled.Size = New System.Drawing.Size(160, 22)
      Me.chkInclUnscheduled.TabIndex = 16
      Me.chkInclUnscheduled.Text = "Include unscheduled"
      Me.chkInclUnscheduled.UseVisualStyleBackColor = True
      '
      'chkFromToDate
      '
      Me.chkFromToDate.AutoSize = True
      Me.chkFromToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkFromToDate.Location = New System.Drawing.Point(9, 71)
      Me.chkFromToDate.Name = "chkFromToDate"
      Me.chkFromToDate.Size = New System.Drawing.Size(66, 22)
      Me.chkFromToDate.TabIndex = 15
      Me.chkFromToDate.Text = "From:"
      Me.chkFromToDate.UseVisualStyleBackColor = True
      '
      'chkLast24Hours
      '
      Me.chkLast24Hours.AutoSize = True
      Me.chkLast24Hours.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkLast24Hours.Location = New System.Drawing.Point(9, 44)
      Me.chkLast24Hours.Name = "chkLast24Hours"
      Me.chkLast24Hours.Size = New System.Drawing.Size(116, 22)
      Me.chkLast24Hours.TabIndex = 14
      Me.chkLast24Hours.Text = "Last 24 hours"
      Me.chkLast24Hours.UseVisualStyleBackColor = True
      '
      'chkCurrentShift
      '
      Me.chkCurrentShift.AutoSize = True
      Me.chkCurrentShift.Checked = True
      Me.chkCurrentShift.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.chkCurrentShift.Location = New System.Drawing.Point(9, 17)
      Me.chkCurrentShift.Name = "chkCurrentShift"
      Me.chkCurrentShift.Size = New System.Drawing.Size(108, 22)
      Me.chkCurrentShift.TabIndex = 13
      Me.chkCurrentShift.TabStop = True
      Me.chkCurrentShift.Text = "Current Shift"
      Me.chkCurrentShift.UseVisualStyleBackColor = True
      '
      'dtpToTime
      '
      Me.dtpToTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.dtpToTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
      Me.dtpToTime.Location = New System.Drawing.Point(27, 206)
      Me.dtpToTime.MaxDate = New Date(2777, 1, 31, 0, 0, 0, 0)
      Me.dtpToTime.Name = "dtpToTime"
      Me.dtpToTime.Size = New System.Drawing.Size(80, 24)
      Me.dtpToTime.TabIndex = 12
      '
      'dtpFromTime
      '
      Me.dtpFromTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
      Me.dtpFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
      Me.dtpFromTime.Location = New System.Drawing.Point(27, 125)
      Me.dtpFromTime.MaxDate = New Date(2777, 1, 31, 0, 0, 0, 0)
      Me.dtpFromTime.Name = "dtpFromTime"
      Me.dtpFromTime.Size = New System.Drawing.Size(80, 24)
      Me.dtpFromTime.TabIndex = 11
      '
      'lblStopActivities
      '
      Me.lblStopActivities.AutoSize = True
      Me.lblStopActivities.BackColor = System.Drawing.Color.Transparent
      Me.lblStopActivities.Font = New System.Drawing.Font("Calibri", 16.0!)
      Me.lblStopActivities.Location = New System.Drawing.Point(16, 3)
      Me.lblStopActivities.Name = "lblStopActivities"
      Me.lblStopActivities.Size = New System.Drawing.Size(136, 27)
      Me.lblStopActivities.TabIndex = 76
      Me.lblStopActivities.Text = "Idle activities:"
      '
      'frmActivityStacked
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackColor = System.Drawing.Color.WhiteSmoke
      Me.ClientSize = New System.Drawing.Size(1096, 432)
      Me.ControlBox = False
      Me.Controls.Add(Me.lblStopActivities)
      Me.Controls.Add(Me.chaActivityChart)
      Me.Controls.Add(Me.grpSelection)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
      Me.Name = "frmActivityStacked"
      Me.Text = "frmActivityStacked"
      CType(Me.chaActivityChart, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpSelection.ResumeLayout(False)
      Me.grpSelection.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents chaActivityChart As System.Windows.Forms.DataVisualization.Charting.Chart
   Friend WithEvents tmrRefresh As System.Windows.Forms.Timer
   Friend WithEvents dtpFromDate As System.Windows.Forms.DateTimePicker
   Friend WithEvents dtpToDate As System.Windows.Forms.DateTimePicker
   Friend WithEvents lblToDate As System.Windows.Forms.Label
   Friend WithEvents grpSelection As System.Windows.Forms.GroupBox
   Friend WithEvents dtpToTime As System.Windows.Forms.DateTimePicker
   Friend WithEvents dtpFromTime As System.Windows.Forms.DateTimePicker
   Friend WithEvents chkCurrentShift As System.Windows.Forms.RadioButton
   Friend WithEvents chkLast24Hours As System.Windows.Forms.RadioButton
   Friend WithEvents chkFromToDate As System.Windows.Forms.RadioButton
   Friend WithEvents lblStopActivities As System.Windows.Forms.Label
   Friend WithEvents chkInclUnscheduled As System.Windows.Forms.CheckBox
End Class
