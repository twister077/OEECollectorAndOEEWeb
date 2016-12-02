Imports System.Data.SqlServerCe
Imports System.Data


Public Class frmActivityShiftLog

   Public mlngSelectedTableKeyID As Long
   Public mstrOeeUserLogInformation As String
   Public mstrOeeUserShiftLogInformation As String

   Private Sub frmActivityShiftLog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      Me.Width = frmOEEMain.Width
      Me.Height = frmOEEMain.pnlDetails.Height
      lblLogView.Width = Me.Width - lblLogView.Location.X - 10
      txtLogEdit.Width = Me.Width - txtLogEdit.Location.X - 10
      txtLogEdit.Height = Me.Height - txtLogEdit.Location.Y - 10
      lstActivity.Width = Me.Width - lstActivity.Location.X - 10
      lstActivity.Height = Me.Height - lstActivity.Location.Y - 10
      grpUndefined.Height = Me.Height - grpUndefined.Location.Y - 10

      mfblnShowActivities(gintSelectedMach)

   End Sub


   Public Function mfblnShowActivities(ByVal intSelMach As Integer) As Boolean

      Dim strSqlQuery As String
      Dim strSqlQueryAdd As String
      Dim rstActivities As ADODB.Recordset
      Dim intX As Integer

      lstActivity.Visible = True
      txtLogEdit.Visible = False
      lblLogView.Visible = False
      chkActivityLog.Checked = True

      If chkInclShortbreaks.Checked Then
         strSqlQueryAdd = ""
      Else
         strSqlQueryAdd = "AND        (fldOEEMachineStatusID <> 3) "
         'strSqlQueryAdd = ""
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
                    "            fldOeeActivityGroupDescription, " & _
                    "            fldOeeArticleNr, " & _
                    "            fldOeeArticleDescription, " & _
                    "            fldOeeOrderNr, " & _
                    "            fldOeeOrderDescription " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE      (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND        (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        (fldOeeCountryID = '" & garrMachine(gintSelectedMach).intCountryNr & "') " & _
                    "AND        (fldOeePlantID = '" & garrMachine(gintSelectedMach).intPlantNr & "') " & _
                    "AND        (fldOeeSubPlantID = '" & garrMachine(gintSelectedMach).intSubPlantNr & "') " & _
                    "AND        (fldOeeDepartmentID = '" & garrMachine(gintSelectedMach).intDepartmentNr & "') " & _
                    "AND        (fldOeeMachineID = '" & garrMachine(gintSelectedMach).intMachineNr & "') " & _
                    strSqlQueryAdd & _
                    "ORDER BY    fldOeeStartDateTime DESC;"

      rstActivities = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      Dim lstItem As ListViewItem
      Dim strRecords(4) As String

      lstActivity.Clear()
      lstActivity.Columns.Add("RecordIndex", 0, HorizontalAlignment.Left)
      lstActivity.Columns.Add("Start date", 188, HorizontalAlignment.Left)
      lstActivity.Columns.Add("End date", 188, HorizontalAlignment.Left)
      lstActivity.Columns.Add("Activitygroup", 150, HorizontalAlignment.Left)
      lstActivity.Columns.Add("Activity", 340, HorizontalAlignment.Left)

      If Not rstActivities.EOF Then
         rstActivities.MoveFirst()
         For intX = 0 To rstActivities.RecordCount - 1
            strRecords(0) = rstActivities.Fields("fldOeeRegTableKeyID").Value
            strRecords(1) = rstActivities.Fields("fldOeeStartDateTime").Value
            strRecords(2) = IIf(IsDBNull(rstActivities.Fields("fldOeeEndDateTime").Value), Date.Now, rstActivities.Fields("fldOeeEndDateTime").Value)
            strRecords(3) = rstActivities.Fields("fldOeeActivityGroupDescription").Value
            strRecords(4) = rstActivities.Fields("fldOeeActivityDescription").Value
            lstItem = New ListViewItem(strRecords)
            lstActivity.Items.Add(lstItem)
            rstActivities.MoveNext()
         Next
      End If

   End Function


   Private Sub lstActivity_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstActivity.MouseDoubleClick

      Dim lstItem As ListViewItem = lstActivity.HitTest(e.Location).Item

      If lstItem IsNot Nothing Then
         If chkActivityLog.Checked Then
            mlngSelectedTableKeyID = lstItem.SubItems(0).Text
            picBack.Visible = True
            cmdSubmitLog.Visible = True
            lstActivity.Visible = False
            chkInclShortbreaks.Visible = False
            lblLogView.Visible = True
            txtLogEdit.Visible = True
            lblActiLogs.Text = "Activities - Write activity log:"
            txtLogEdit.Text = ""
            lblLogView.Text = gfstrSelectedLogs(gintSelectedMach, mlngSelectedTableKeyID, False)
         End If
      End If

   End Sub


   Public Function gfstrSelectedLogs(ByVal intSelMach As Integer, ByVal lngSelTabelID As Long, ByVal blnGetShiftLog As Boolean) As String

      Dim strSqlQuery As String
      Dim rstCurrentLogs As ADODB.Recordset
      Dim intX As Integer
      Dim strShiftLogBuilder As String
      Dim strRecordLog As String
      'Shiftlog collect all shiftlog info NOT the last one

      gfstrSelectedLogs = ""
      If blnGetShiftLog Then
         strSqlQuery = "SELECT      fldOeeRegTableKeyID, " & _
                       "            fldOeeUserShiftLogInformation " & _
                       "FROM        tblOee_Reg " & _
                       "WHERE      (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                       "AND        (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                       "AND        (fldOeeCountryID = '" & garrMachine(gintSelectedMach).intCountryNr & "') " & _
                       "AND        (fldOeePlantID = '" & garrMachine(gintSelectedMach).intPlantNr & "') " & _
                       "AND        (fldOeeSubPlantID = '" & garrMachine(gintSelectedMach).intSubPlantNr & "') " & _
                       "AND        (fldOeeDepartmentID = '" & garrMachine(gintSelectedMach).intDepartmentNr & "') " & _
                       "AND        (fldOeeMachineID = '" & garrMachine(gintSelectedMach).intMachineNr & "') " & _
                       "ORDER BY    fldOeeRegTableKeyID DESC;"

         rstCurrentLogs = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         strShiftLogBuilder = ""
         rstCurrentLogs.MoveFirst()
         If Not rstCurrentLogs.EOF Then
            For intX = 0 To rstCurrentLogs.RecordCount - 1
               strRecordLog = IIf(IsDBNull(rstCurrentLogs.Fields("fldOeeUserShiftLogInformation").Value), "", _
                                                    rstCurrentLogs.Fields("fldOeeUserShiftLogInformation").Value)
               If mlngSelectedTableKeyID = 0 Then
                  mlngSelectedTableKeyID = rstCurrentLogs.Fields("fldOeeRegTableKeyID").Value
               End If
               If Len(strRecordLog) > 0 Then
                  strShiftLogBuilder = strShiftLogBuilder & vbCrLf & strRecordLog
               End If
               rstCurrentLogs.MoveNext()
            Next
            gfstrSelectedLogs = strShiftLogBuilder
         End If
      Else
         strSqlQuery = "SELECT       fldOeeRegTableKeyID, " & _
                       "             fldOeeUserLogInformation, " & _
                       "             fldOeeUserShiftLogInformation " & _
                       "FROM         tblOee_Reg " & _
                       "WHERE       (fldOeeRegTableKeyID = '" & mlngSelectedTableKeyID & "');"

         rstCurrentLogs = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         If Not rstCurrentLogs.EOF Then
            gfstrSelectedLogs = IIf(IsDBNull(rstCurrentLogs.Fields("fldOeeUserLogInformation").Value), 1, _
                                              rstCurrentLogs.Fields("fldOeeUserLogInformation").Value)

         End If
      End If

   End Function


   Private Sub cmdSubmitLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmitLog.Click

      Dim strSqlQuery As String = ""
      Dim strTotalShiftLog As String
      Dim rstRegLastKey As ADODB.Recordset
      Dim strRecordLog As String

      'first get te last tablekey
      'then check if there is an active log in that record
      'yes then update
      'no then update without first reader active log

      If Len(txtLogEdit.Text) > 0 Then
         If chkActivityLog.Checked Then
            If Len(lblLogView.Text) > 0 Then
               strTotalShiftLog = lblLogView.Text & vbCrLf & Date.Now & " - " & txtLogEdit.Text
            Else
               strTotalShiftLog = Date.Now & " - " & txtLogEdit.Text
            End If
            strSqlQuery = "UPDATE tblOee_Reg " & _
                          "SET    fldOeeUserLogInformation = '" & strTotalShiftLog & "' " & _
                          "WHERE (fldOeeRegTableKeyID = '" & mlngSelectedTableKeyID & "');"
         End If
         If chkShiftlog.Checked Then
            strSqlQuery = "SELECT      fldOeeRegTableKeyID, " & _
                          "            fldOeeUserShiftLogInformation " & _
                          "FROM        tblOee_Reg " & _
                          "WHERE      (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                          "AND        (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                          "AND        (fldOeeCountryID = '" & garrMachine(gintSelectedMach).intCountryNr & "') " & _
                          "AND        (fldOeePlantID = '" & garrMachine(gintSelectedMach).intPlantNr & "') " & _
                          "AND        (fldOeeSubPlantID = '" & garrMachine(gintSelectedMach).intSubPlantNr & "') " & _
                          "AND        (fldOeeDepartmentID = '" & garrMachine(gintSelectedMach).intDepartmentNr & "') " & _
                          "AND        (fldOeeMachineID = '" & garrMachine(gintSelectedMach).intMachineNr & "') " & _
                          "ORDER BY    fldOeeShiftStartDateTime DESC;"

            rstRegLastKey = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

            If Not rstRegLastKey.EOF Then
               mlngSelectedTableKeyID = rstRegLastKey.Fields("fldOeeRegTableKeyID").Value
               strRecordLog = rstRegLastKey.Fields("fldOeeUserShiftLogInformation").Value
            Else
               strRecordLog = ""
            End If

            If Len(strRecordLog) > 0 Then
               strTotalShiftLog = Date.Now & " - " & txtLogEdit.Text & vbCrLf & strRecordLog
            Else
               strTotalShiftLog = Date.Now & " - " & txtLogEdit.Text
            End If

            strSqlQuery = "UPDATE tblOee_Reg " & _
                          "SET    fldOeeUserShiftLogInformation = '" & strTotalShiftLog & "' " & _
                          "WHERE (fldOeeRegTableKeyID = '" & mlngSelectedTableKeyID & "');"
         End If

         gintSqlCeExecuteNonQuery(strSqlQuery)

         txtLogEdit.Text = ""

         If chkShiftlog.Checked Then
            lblLogView.Text = gfstrSelectedLogs(gintSelectedMach, mlngSelectedTableKeyID, True)
         Else
            lblLogView.Text = gfstrSelectedLogs(gintSelectedMach, mlngSelectedTableKeyID, False)
         End If
      End If
      lblLogView.Refresh()

   End Sub


   Private Sub chkActivityLog_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkActivityLog.CheckedChanged

      If chkActivityLog.Checked Then
         lblLogView.Visible = True
         chkInclShortbreaks.Visible = True
         cmdSubmitLog.Visible = False
         picBack.Visible = False
         mfblnShowAllActivities(gintSelectedMach)
      End If

   End Sub


   Private Function mfblnShowAllActivities(ByVal intSelMach As Integer) As Boolean

      chkShiftlog.Checked = False
      chkInclShortbreaks.Enabled = True
      lblActiLogs.Text = "Activities:"
      mfblnShowActivities(intSelMach)

   End Function


   Private Sub chkShiftlog_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShiftlog.CheckedChanged

      If chkShiftlog.Checked Then
         picBack.Visible = False
         chkActivityLog.Checked = False
         chkInclShortbreaks.Visible = False
         lblLogView.Visible = True
         lstActivity.Visible = False
         txtLogEdit.Visible = True
         lblActiLogs.Text = "Shift - Write shift log:"
         txtLogEdit.Text = ""
         lblLogView.Text = gfstrSelectedLogs(gintSelectedMach, mlngSelectedTableKeyID, True)
         cmdSubmitLog.Visible = True
      End If

   End Sub


   Private Sub chkInclShortbreaks_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInclShortbreaks.CheckedChanged

      mfblnShowActivities(gintSelectedMach)

   End Sub


   Private Sub picBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picBack.Click

      If chkActivityLog.Checked Then
         picBack.Visible = False
         cmdSubmitLog.Visible = False
         lblLogView.Visible = True
         chkInclShortbreaks.Visible = True
         mfblnShowAllActivities(gintSelectedMach)
      End If

   End Sub

End Class