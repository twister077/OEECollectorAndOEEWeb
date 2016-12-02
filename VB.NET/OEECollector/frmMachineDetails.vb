Public Class frmMachineDetails

   Private Sub frmMachineDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      Me.Width = frmOEEMain.Width
      ChartOEE.Location = New Point(Me.Width - 308, ChartOEE.Location.Y)
      grpActivityInfo.Width = ChartOEE.Location.X - 16
      grpModuleActi.Width = ChartOEE.Location.X - 6 - grpModuleActi.Location.X

      ChartOEE.ChartAreas(0).AxisY.Maximum = 100
      ChartOEE.ChartAreas(0).AxisX.LabelStyle.IsEndLabelVisible = False
      ChartOEE.ChartAreas(0).AxisX.LabelStyle.Enabled = False
      ChartOEE.ChartAreas(0).AxisY.Enabled = DataVisualization.Charting.AxisEnabled.False
      ChartOEE.ChartAreas(0).AxisX.LineWidth = 0
      ChartOEE.ChartAreas(0).AxisY.LineWidth = 0
      ChartOEE.ChartAreas(0).AxisX.LabelStyle.Enabled = False
      ChartOEE.ChartAreas(0).AxisY.LabelStyle.Enabled = False
      ChartOEE.ChartAreas(0).AxisX.MajorGrid.Enabled = False
      ChartOEE.ChartAreas(0).AxisY.MajorGrid.Enabled = False
      ChartOEE.ChartAreas(0).AxisX.MinorGrid.Enabled = False
      ChartOEE.ChartAreas(0).AxisY.MinorGrid.Enabled = False
      ChartOEE.ChartAreas(0).AxisX.MajorTickMark.Enabled = False
      ChartOEE.ChartAreas(0).AxisY.MajorTickMark.Enabled = False
      ChartOEE.ChartAreas(0).AxisX.MinorTickMark.Enabled = False
      ChartOEE.ChartAreas(0).AxisY.MinorTickMark.Enabled = False
      ChartOEE.ChartAreas(0).BackImageTransparentColor = Color.Black

      lblTimenr.Text = Date.Now.ToShortTimeString

   End Sub

End Class