Imports System.Data.SqlServerCe
Imports System.Data

Public Class frmUndefinedActivities

   Private Sub frmUndefinedStandstill_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      Me.Width = frmOEEMain.Width
      Me.Height = frmOEEMain.pnlDetails.Height
      lstUndefStand2.Width = Me.Width - lstUndefStand2.Location.X - 10
      lstUndefStand2.Height = Me.Height - lstUndefStand2.Location.Y - 10
      grpUndefined.Height = Me.Height - grpUndefined.Location.Y - 10

      chkUndefProd.Checked = True

   End Sub


   Public Function gfblnShowUndefined(ByVal intSelMach As Integer, ByVal intUndefinedKind As Integer) As Boolean

      Dim strSqlQuery As String
      Dim strSqlQueryAdd As String = ""
      Dim rstUndefStand As ADODB.Recordset
      Dim intX As Integer

      'intUndefinedKind 1 = undefined prod, 2 = undefined standstill
      'two kinds of passthroughs: define undefined standstill and production
      If intUndefinedKind = 1 Then
         strSqlQueryAdd = "AND        (fldOEEMachineStatusID = 1)"
         chkUndefProd.Checked = True
      ElseIf intUndefinedKind = 2 Then
         chkUndefStill.Checked = True
         strSqlQueryAdd = "AND        (fldOEEMachineStatusID = 4)"
      End If

      strSqlQuery = "SELECT      fldOeeRegTableKeyID, " & _
                    "            fldOeeStartDateTime, " & _
                    "            fldOeeEndDateTime, " & _
                    "            fldOeeShiftStartDateTime, " & _
                    "            fldOeeShiftEndDateTime, " & _
                    "            fldOeeActivityDuration, " & _
                    "            fldOeeActivityID, " & _
                    "            fldOeeActivityDescription, " & _
                    "            fldOeeActivityGroupID, " & _
                    "            fldOeeActivityGroupDescription " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE      (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND        (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        (fldOeeCountryID = '" & garrMachine(gintSelectedMach).intCountryNr & "') " & _
                    "AND        (fldOeePlantID = '" & garrMachine(gintSelectedMach).intPlantNr & "') " & _
                    "AND        (fldOeeSubPlantID = '" & garrMachine(gintSelectedMach).intSubPlantNr & "') " & _
                    "AND        (fldOeeDepartmentID = '" & garrMachine(gintSelectedMach).intDepartmentNr & "') " & _
                    "AND        (fldOeeMachineID = '" & garrMachine(gintSelectedMach).intMachineNr & "') " & _
                    strSqlQueryAdd

      rstUndefStand = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      Dim lstItem As ListViewItem
      Dim strRecords(4) As String

      lstUndefStand2.Clear()
      lstUndefStand2.Columns.Add("RecordIndex", 0, HorizontalAlignment.Left)
      lstUndefStand2.Columns.Add("Start date", 188, HorizontalAlignment.Left)
      lstUndefStand2.Columns.Add("End date", 188, HorizontalAlignment.Left)
      lstUndefStand2.Columns.Add("Activitygroup", 150, HorizontalAlignment.Left)
      lstUndefStand2.Columns.Add("Activity", 340, HorizontalAlignment.Left)

      If Not rstUndefStand.EOF Then
         rstUndefStand.MoveFirst()
         For intX = 0 To rstUndefStand.RecordCount - 1
            strRecords(0) = rstUndefStand.Fields("fldOeeRegTableKeyID").Value
            strRecords(1) = rstUndefStand.Fields("fldOeeStartDateTime").Value
            strRecords(2) = IIf(IsDBNull(rstUndefStand.Fields("fldOeeEndDateTime").Value), Date.Now, rstUndefStand.Fields("fldOeeEndDateTime").Value)
            strRecords(3) = rstUndefStand.Fields("fldOeeActivityGroupDescription").Value
            strRecords(4) = rstUndefStand.Fields("fldOeeActivityDescription").Value
            lstItem = New ListViewItem(strRecords)
            lstUndefStand2.Items.Add(lstItem)
            rstUndefStand.MoveNext()
         Next
      End If

   End Function


   Private Sub lstUndefStand2_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstUndefStand2.MouseDoubleClick

      Dim lstItem As ListViewItem = lstUndefStand2.HitTest(e.Location).Item

      gintUndefinedTableKeyID = lstItem.SubItems(0).Text

      If lstItem IsNot Nothing Then
         If chkUndefStill.Checked Then
            gsChangePanel(frmOEEMain.pnlDetails, frmActivityInput)
            frmActivityInput.mfblnShowActivty(gintSelectedMach, 1, True)
         End If
         If chkUndefProd.Checked Then
            gsChangePanel(frmOEEMain.pnlDetails, frmProductionInput)
         End If
      End If

   End Sub


   Private Sub chkUndefStill_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUndefStill.CheckedChanged

      If chkUndefStill.Checked = True Then
         gfblnShowUndefined(gintSelectedMach, 2)
      End If

   End Sub


   Private Sub chkUndefProd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUndefProd.CheckedChanged

      If chkUndefProd.Checked = True Then
         gfblnShowUndefined(gintSelectedMach, 1)
      End If

   End Sub

End Class