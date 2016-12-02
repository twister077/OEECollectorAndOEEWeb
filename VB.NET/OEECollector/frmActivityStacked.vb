Imports System.Data
Imports System.Data.SqlServerCe
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmActivityStacked

   Private Sub frmActivityStacked_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      Me.Width = frmOEEMain.Width
      Me.Height = frmOEEMain.pnlDetails.Height
      chaActivityChart.Width = Me.Width - chaActivityChart.Location.X - 10
      chaActivityChart.Height = Me.Height - chaActivityChart.Location.Y - 10
      grpSelection.Height = Me.Height - grpSelection.Location.Y - 10

      chaActivityChart.ChartAreas(0).AxisY.LabelStyle.IsEndLabelVisible = False
      chaActivityChart.ChartAreas(0).AxisY.LabelStyle.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.LabelStyle.IsEndLabelVisible = False
      chaActivityChart.ChartAreas(0).AxisX.LabelStyle.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MajorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MajorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MinorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MinorGrid.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MajorTickMark.Enabled = False
      chaActivityChart.ChartAreas(0).AxisY.MajorTickMark.Enabled = False
      chaActivityChart.ChartAreas(0).AxisX.MinorTickMark.Enabled = False
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


   Private Sub tmrRefresk_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick

      Call mfblnRefresh(gintSelectedMach)

   End Sub


   Public Function mfblnRefresh(ByVal intSelMach As Integer) As Boolean

      Dim intX As Integer
      Dim intY As Integer
      Dim intZ As Integer
      Dim dblMaxY As Double
      Dim dblInterval As Double
      Dim strSqlQuery As String
      Dim dtaTable2 As New DataTable
      Dim rstActivityGroup As ADODB.Recordset
      Dim datStartDateTime As DateTime
      Dim datEndDateTime As DateTime
      Dim strSqlWhereDateAdditive As String

      strSqlWhereDateAdditive = ""
      If chkCurrentShift.Checked = True Then
         datStartDateTime = garrTeamShift.datStartShift
         'strSqlWhereDateAdditive = "WHERE      (tblOee_Reg.fldOeeStartDateTime > CONVERT(DATETIME, '" & gfstrDatToStr(datStartDateTime) & "', 101)) "
         strSqlWhereDateAdditive = "WHERE      tblOee_Reg.fldOeeStartDateTime > '" & gfstrDatToStr(datStartDateTime) & "' "
      End If
      If chkLast24Hours.Checked = True Then
         datStartDateTime = garrTeamShift.datStartShift.AddDays(-1)
         strSqlWhereDateAdditive = "WHERE      tblOee_Reg.fldOeeStartDateTime > '" & gfstrDatToStr(datStartDateTime) & "' "
      End If
      If chkFromToDate.Checked = True Then
         datStartDateTime = New DateTime(dtpFromDate.Value.Year, dtpFromDate.Value.Month, dtpFromDate.Value.Day, dtpFromTime.Value.Hour, _
                                      dtpFromTime.Value.Minute, 0)
         datEndDateTime = New DateTime(dtpToDate.Value.Year, dtpToDate.Value.Month, dtpToDate.Value.Day, dtpToTime.Value.Hour, _
                                      dtpToTime.Value.Minute, 0)
         strSqlWhereDateAdditive = "WHERE      tblOee_Reg.fldOeeStartDateTime > '" & gfstrDatToStr(datStartDateTime) & "' " & _
                                   "AND        tblOee_Reg.fldOeeEndDateTime < '" & gfstrDatToStr(datEndDateTime) & "' "
      End If

      If chkInclUnscheduled.Checked = False Then
         strSqlWhereDateAdditive = strSqlWhereDateAdditive & _
                                   "AND        (tblOee_ActivityGroup.fldOeeActivityGroupCalcForOee <> 0) "
      End If

      strSqlQuery = "SELECT SUM (tblOee_Reg.fldOeeActivityDuration) AS fldSumTime, " & _
                    "COUNT      (tblOee_Reg.fldOeeActivityDescription) AS fldCountActivity, " & _
                    "            tblOee_Reg.fldOeeActivityDescription, " & _
                    "            tblOee_ActivityGroup.fldOEEActivityGroupColorNr " & _
                    "FROM        tblOee_Reg " & _
                    "INNER JOIN  tblOee_Activity " & _
                    "ON          tblOee_Reg.fldOeeActivityID = tblOee_Activity.fldOeeActivityNr " & _
                    "INNER JOIN  tblOee_ActivityGroup " & _
                    "ON          tblOee_Reg.fldOeeActivityGroupID = tblOee_ActivityGroup.fldOeeActivityGroupNr " & _
                    strSqlWhereDateAdditive & _
                    "AND        (tblOee_Reg.fldOeeActivityDuration > 0) " & _
                    "AND        (tblOee_Reg.fldOeeMachineStatusID IN (3, 4, 5)) " & _
                    "GROUP BY    tblOee_Reg.fldOeeActivityDescription, " & _
                    "            tblOee_ActivityGroup.fldOEEActivityGroupColorNr, " & _
                    "            tblOee_Reg.fldOeeMachineStatusID, " & _
                    "            tblOee_Reg.fldOeeMachineID, " & _
                    "            tblOee_Reg.fldOeeCountryID, " & _
                    "            tblOee_Reg.fldOeePlantID, " & _
                    "            tblOee_Reg.fldOeeSubPlantID, " & _
                    "            tblOee_Reg.fldOeeDepartmentID " & _
                    "HAVING     (tblOee_Reg.fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & ") " & _
                    "AND        (tblOee_Reg.fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & ") " & _
                    "AND        (tblOee_Reg.fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & ") " & _
                    "AND        (tblOee_Reg.fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ") " & _
                    "AND        (tblOee_Reg.fldOeeMachineID = " & garrMachine(intSelMach).intMachineNr & ") " & _
                    "ORDER BY    fldSumTime DESC"




      Try
         rstActivityGroup = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         'build datatable to bind to the activity chart

         dtaTable2.Columns.Add("fldActivity", GetType(String))
         dtaTable2.Columns.Add("fldActivityTotalValue", GetType(Integer))

         'add data to activity datatable 
         dblMaxY = 0
         Dim dblSumTime As Double

         If Not rstActivityGroup.EOF Then
            rstActivityGroup.MoveFirst()
            For intX = 0 To (rstActivityGroup.RecordCount - 1)
               dblSumTime = rstActivityGroup.Fields("fldSumTime").Value
               dtaTable2.Rows.Add(rstActivityGroup.Fields("fldCountActivity").Value & "x " & rstActivityGroup.Fields("fldOeeActivityDescription").Value, dblSumTime)
               If dblMaxY = 0 Then
                  dblMaxY = rstActivityGroup.Fields("fldSumTime").Value
               End If
               rstActivityGroup.MoveNext()
               If intX = 13 Then
                  Exit For
               End If
            Next
         End If

         'compensate no registered time
         If dblMaxY = 0 Then dblMaxY = 1

         dblMaxY = dblMaxY + (dblMaxY / 10)

         'adjust interval graph
         dblInterval = dblMaxY / 10
         For intY = 1 To dblMaxY
            If dblMaxY / intY <= 10 Then
               If Len(CStr(intY)) > 1 Then
                  For intX = 1 To 9
                     If Mid(CStr(intX + intY), Len(CStr(intX + intY))) = "0" Then
                        dblInterval = intX + intY
                        Exit For
                     End If
                  Next
               End If
               Exit For
            End If
         Next

         With chaActivityChart.ChartAreas(0)
            .AxisY.Maximum = dblMaxY
            .AxisY.Minimum = 0
            .AxisY.Interval = CInt(dblInterval)
            .AxisY.LabelStyle.Format = "N2"
            .AxisX.Title = ""
            .AxisY.Title = "Scale max. " & CInt(dblMaxY / 60) & " minutes"
            .AxisY.TitleFont = New Font("Calibri", 12)
         End With

         With chaActivityChart.Series(0)
            .Points.DataBind(dtaTable2.DefaultView, "fldActivity", "fldActivityTotalValue", Nothing)
            .ChartType = DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 4
            .IsVisibleInLegend = False
         End With

         rstActivityGroup.MoveFirst()
         For intX = 0 To (rstActivityGroup.RecordCount - 1)
            chaActivityChart.Series(0).Points.ElementAt(intX).Color = Color.FromArgb(rstActivityGroup.Fields("fldOEEActivityGroupColorNr").Value)
            chaActivityChart.Series(0).Points(intX).Color = Color.FromArgb(rstActivityGroup.Fields("fldOEEActivityGroupColorNr").Value)
            If intX = 13 Then
               Exit For
            End If
            rstActivityGroup.MoveNext()
         Next

         chaActivityChart.Legends.Clear()
         chaActivityChart.Legends.Add(New Legend("Default"))
         chaActivityChart.Legends("Default").Docking = Docking.Right
         chaActivityChart.Legends("Default").Alignment = StringAlignment.Near
         chaActivityChart.Legends("Default").Font = New System.Drawing.Font("Calibri", 11, System.Drawing.FontStyle.Regular)
         chaActivityChart.Legends("Default").BorderColor = Color.White

         intZ = 0
         For Each dp As DataPoint In chaActivityChart.Series(0).Points
            chaActivityChart.Legends("Default").CustomItems.Add(dp.Color, dp.AxisLabel)
            chaActivityChart.Legends("Default").CustomItems(intZ).BorderColor = Color.White
            If intZ = 13 Then
               Exit For
            End If
            intZ = intZ + 1
         Next

      Catch ex As Exception
         Dim strError As String
         strError = ex.Message
      End Try

   End Function


   Private Sub chkCurrentShift_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

      If chkCurrentShift.Checked Then
         chkLast24Hours.Checked = False
         chkFromToDate.Checked = False
      Else
         If chkLast24Hours.Checked = False And chkFromToDate.Checked = False Then
            chkCurrentShift.Checked = True
         End If
      End If

   End Sub


   Private Sub chkLast24Hours_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

      If chkLast24Hours.Checked Then
         chkCurrentShift.Checked = False
         chkFromToDate.Checked = False
      Else
         If chkCurrentShift.Checked = False And chkFromToDate.Checked = False Then
            chkLast24Hours.Checked = True
         End If
      End If

   End Sub


   Private Sub chkFromToDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

      If chkFromToDate.Checked Then
         chkCurrentShift.Checked = False
         chkLast24Hours.Checked = False
      Else
         If chkLast24Hours.Checked = False And chkCurrentShift.Checked = False Then
            chkFromToDate.Checked = True
         End If
      End If

   End Sub

   Private Sub chkInclShortbreak_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

   End Sub
End Class