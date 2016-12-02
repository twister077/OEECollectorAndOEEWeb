Imports System.Data
Imports System.Data.SqlServerCe
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.IO


Public Class frmOeeProgress


   Private Sub frmOeeProgress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en")
      System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture

      Me.Width = frmOEEMain.Width
      Me.Height = frmOEEMain.pnlDetails.Height
      chaActivityChart.Width = Me.Width - chaActivityChart.Location.X - 10
      chaActivityChart.Height = Me.Height - chaActivityChart.Location.Y - 10
      lblTimeScale.Location = New Point(lblTimeScale.Location.X, Me.Height - 25)
      grpSelection.Height = Me.Height - grpSelection.Location.Y - 10

      'chaActivityChart.ChartAreas(0).AxisY.LabelStyle.IsEndLabelVisible = False
      'chaActivityChart.ChartAreas(0).AxisY.LabelStyle.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MajorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MinorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MajorTickMark.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MinorTickMark.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MajorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MinorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MajorTickMark.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MinorTickMark.Enabled = False

      Try
         dtpFromDate.Value = garrTeamShift.datStartShift.AddDays(-2)
      Catch ex As Exception

      End Try
      dtpFromTime.Format = DateTimePickerFormat.Custom
      dtpFromTime.CustomFormat = "HH:mm tt"
      dtpFromTime.ShowUpDown = True
      dtpToTime.Format = DateTimePickerFormat.Custom
      dtpToTime.CustomFormat = "HH:mm tt"
      dtpToTime.ShowUpDown = True

      Call mfblnRefresh(gintSelectedMach)

   End Sub


   Public Function mfblnRefresh(ByVal intSelMach As Integer) As Boolean

      Dim dtaTable2 As New DataTable()
      Dim strSqlQuery As String
      Dim rstOEEProgress As ADODB.Recordset
      Dim strSqlWhereDateAdditive As String
      Dim datStartDateTime As DateTime
      Dim datEndDateTime As DateTime
      Dim intX As Integer
      Dim dblDurationPlus As Integer
      Dim intY As Integer
      Dim dblMaxY As Double
      Dim intInterval As Integer

      strSqlWhereDateAdditive = ""
      If chkCurrentShift.Checked = True Then
         datStartDateTime = garrTeamShift.datStartShift
         strSqlWhereDateAdditive = "WHERE     fldOeeStartDateTime > '" & gfstrDatToStr(datStartDateTime) & "' "
      End If
      If chkLast24Hours.Checked = True Then
         datStartDateTime = garrTeamShift.datStartShift.AddDays(-1)
         strSqlWhereDateAdditive = "WHERE      (fldOeeStartDateTime > '" & gfstrDatToStr(datStartDateTime) & "') "
      End If
      If chkFromToDate.Checked = True Then
         datStartDateTime = New DateTime(dtpFromDate.Value.Year, dtpFromDate.Value.Month, dtpFromDate.Value.Day, dtpFromTime.Value.Hour, _
                                      dtpFromTime.Value.Minute, 0)
         datEndDateTime = New DateTime(dtpToDate.Value.Year, dtpToDate.Value.Month, dtpToDate.Value.Day, dtpToTime.Value.Hour, _
                                      dtpToTime.Value.Minute, 0)
         strSqlWhereDateAdditive = "WHERE      (fldOeeStartDateTime >'" & gfstrDatToStr(datStartDateTime) & "') " & _
                                   "AND        (fldOeeStartDateTime < '" & gfstrDatToStr(datEndDateTime) & "') "
      End If

      strSqlQuery = "SELECT      fldOeeProgressTableKeyID, " & _
                    "            fldOeeCurrentOee, " & _
                    "            fldOeeCurrentAvailability, " & _
                    "            fldOeeCurrentPerformance, " & _
                    "            fldOeeCurrentQuality, " & _
                    "            fldOeeActivityDuration " & _
                    "FROM        tblOee_Progress " & _
                    strSqlWhereDateAdditive & _
                    "AND        (fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & ") " & _
                    "AND        (fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & ") " & _
                    "AND        (fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & ") " & _
                    "AND        (fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ") " & _
                    "AND        (tblOee_Progress.fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "ORDER BY    fldOeeStartDateTime"

      rstOEEProgress = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))
      If rstOEEProgress Is Nothing Then
         Exit Function
      End If
      'build datatable to bind to the oee chart
      dtaTable2.Columns.Add("fldActivityDurationValue", GetType(Integer))
      dtaTable2.Columns.Add("fldAvailabilityValue", GetType(Integer))
      dtaTable2.Columns.Add("fldPerformanceValue", GetType(Integer))
      dtaTable2.Columns.Add("fldQualityValue", GetType(Integer))
      dtaTable2.Columns.Add("fldOEEValue", GetType(Integer))

      'add data to oee datatable 
      If Not rstOEEProgress.EOF Then
         rstOEEProgress.MoveFirst()
         For intX = 0 To rstOEEProgress.RecordCount - 1
            dblDurationPlus = dblDurationPlus + (rstOEEProgress.Fields("fldOeeActivityDuration").Value / 60)
            dtaTable2.Rows.Add(dblDurationPlus, rstOEEProgress.Fields("fldOeeCurrentAvailability").Value, _
                               rstOEEProgress.Fields("fldOeeCurrentPerformance").Value, rstOEEProgress.Fields("fldOeeCurrentQuality").Value, _
                               rstOEEProgress.Fields("fldOeeCurrentOee").Value)
            rstOEEProgress.MoveNext()
         Next
      Else
         Exit Function
      End If

      'adjust interval oee chart
      dblMaxY = dblDurationPlus
      intInterval = dblMaxY / 10
      For intY = 1 To dblMaxY
         If dblMaxY / intY <= 10 Then
            If Len(CStr(intY)) > 1 Then
               For intX = 1 To 10
                  If Mid(CStr(intX + intY), Len(CStr(intX + intY))) = "0" Then
                     intInterval = intX + intY
                     Exit For
                  End If
               Next
            End If
            Exit For
         End If
      Next

      'set interval oee chart
      With chaActivityChart.ChartAreas(0)
         .AxisY.Title = "Percentage"
         .AxisY.Minimum = 0
         .AxisX.Minimum = 0
         .AxisY.Maximum = 100
         .AxisY.Interval = 10
         .AxisX.Interval = intInterval
      End With

      'bind column from oee datatable to oee chart
      With chaActivityChart.Series(0)
         .Points.DataBind(dtaTable2.DefaultView, "fldActivityDurationValue", "fldAvailabilityValue", Nothing)
         .ChartType = DataVisualization.Charting.SeriesChartType.Line
         .BorderWidth = 2
         .IsVisibleInLegend = False
      End With
      With chaActivityChart.Series(1)
         .Points.DataBind(dtaTable2.DefaultView, "fldActivityDurationValue", "fldPerformanceValue", Nothing)
         .ChartType = DataVisualization.Charting.SeriesChartType.Line
         .BorderWidth = 2
         .IsVisibleInLegend = False
      End With
      With chaActivityChart.Series(2)
         .Points.DataBind(dtaTable2.DefaultView, "fldActivityDurationValue", "fldQualityValue", Nothing)
         .ChartType = DataVisualization.Charting.SeriesChartType.Line
         .BorderWidth = 2
         .IsVisibleInLegend = False
      End With
      With chaActivityChart.Series(3)
         .Points.DataBind(dtaTable2.DefaultView, "fldActivityDurationValue", "fldOEEValue", Nothing)
         .ChartType = DataVisualization.Charting.SeriesChartType.Line
         .BorderWidth = 2
         .IsVisibleInLegend = False
      End With

      'set color
      rstOEEProgress.MoveFirst()
      For intX = 0 To (rstOEEProgress.RecordCount - 1)
         chaActivityChart.Series(0).Points.ElementAt(intX).Color = Color.Yellow
         chaActivityChart.Series(0).Points(intX).Color = Color.Yellow
         chaActivityChart.Series(1).Points.ElementAt(intX).Color = Color.Red
         chaActivityChart.Series(1).Points(intX).Color = Color.Red
         chaActivityChart.Series(2).Points.ElementAt(intX).Color = Color.Blue
         chaActivityChart.Series(2).Points(intX).Color = Color.Blue
         chaActivityChart.Series(3).Points.ElementAt(intX).Color = Color.Lime
         chaActivityChart.Series(3).Points(intX).Color = Color.Lime
         rstOEEProgress.MoveNext()
      Next

      chaActivityChart.Legends.Clear()
      chaActivityChart.Legends.Add(New Legend("Default"))
      chaActivityChart.Legends("Default").Docking = Docking.Right
      chaActivityChart.Legends("Default").Alignment = StringAlignment.Near
      chaActivityChart.Legends("Default").Font = New System.Drawing.Font("Calibri", 11, System.Drawing.FontStyle.Regular)
      chaActivityChart.Legends("Default").BorderColor = Color.White
      chaActivityChart.Legends("Default").CustomItems.Add(Color.Yellow, "Availability")
      chaActivityChart.Legends("Default").CustomItems.Add(Color.Red, "Performance")
      chaActivityChart.Legends("Default").CustomItems.Add(Color.Blue, "Quality")
      chaActivityChart.Legends("Default").CustomItems.Add(Color.Lime, "OEE Score")
      For intX = 0 To 3
         chaActivityChart.Legends("Default").CustomItems(intX).BorderColor = Color.White
      Next

   End Function


   Private Sub chkCurrentShift_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCurrentShift.CheckedChanged

      If chkCurrentShift.Checked Then
         chkLast24Hours.Checked = False
         chkFromToDate.Checked = False
      Else
         If chkLast24Hours.Checked = False And chkFromToDate.Checked = False Then
            chkCurrentShift.Checked = True
         End If
      End If

   End Sub


   Private Sub chkLast24Hours_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLast24Hours.CheckedChanged

      If chkLast24Hours.Checked Then
         chkCurrentShift.Checked = False
         chkFromToDate.Checked = False
      Else
         If chkCurrentShift.Checked = False And chkFromToDate.Checked = False Then
            chkLast24Hours.Checked = True
         End If
      End If

   End Sub


   Private Sub chkFromToDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFromToDate.CheckedChanged

      If chkFromToDate.Checked Then
         chkCurrentShift.Checked = False
         chkLast24Hours.Checked = False
      Else
         If chkLast24Hours.Checked = False And chkCurrentShift.Checked = False Then
            chkFromToDate.Checked = True
         End If
      End If

   End Sub


   Private Sub tmrRefresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick

      mfblnRefresh(gintSelectedMach)

   End Sub

End Class