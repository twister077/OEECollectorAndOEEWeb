Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Data
Imports System.Net
Imports System.IO


Public Class frmOEEMain

   <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")> _
   Private Shared Function CreateRoundRectRgn(ByVal iLeft As Integer, ByVal iTop As Integer, ByVal iRight As Integer, ByVal iBottom As Integer, _
                                              ByVal iWidth As Integer, ByVal iHeight As Integer) As IntPtr
   End Function

   Public mlngLastCounter As Long
   Public mintTimerInterval As Integer
   Public mintLineSpeedSample As Integer
   Public mintRefeshInterval As Integer
   Public mintGarbageCleaner As Integer
   Public mblnNoQuit As Boolean

   'Todo:
   'Bug ActivityStacked no shift last 24 hours
   'No shiftinformation log only ones per 24 hours


   Private Sub frmOEEMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      'gfblnResetIPArduino("192.168.2.123", 101)
      'gfintIPArduino("192.168.2.123", 100)

      Dim intX As Integer
      Dim tt As New ToolTip()
      Dim argCommandLine As System.Collections.ObjectModel.ReadOnlyCollection(Of String)

      If Not My.Computer.FileSystem.DirectoryExists(My.Application.Info.DirectoryPath & "\Cache") Then
         My.Computer.FileSystem.CreateDirectory(My.Application.Info.DirectoryPath & "\Cache")
      End If

      'get local database via OEEWeb API
      If gfblnGetDB(0) = False Then
         End
      End If

      'get commandline arguments
      argCommandLine = My.Application.CommandLineArgs
      If argCommandLine.Count = 0 Then
         Me.Width = My.Computer.Screen.WorkingArea.Width
         Me.Height = My.Computer.Screen.WorkingArea.Height
      Else
         For intX = 0 To argCommandLine.Count - 1
            If argCommandLine(intX) = "/fullscreen" Then
               Me.Width = My.Computer.Screen.Bounds.Width
               Me.Height = My.Computer.Screen.Bounds.Height
               Exit For
            Else
               Me.Width = My.Computer.Screen.WorkingArea.Width
               Me.Height = My.Computer.Screen.WorkingArea.Height
            End If
         Next
      End If

      For intX = 0 To argCommandLine.Count - 1
         If argCommandLine(intX) = "/noquit" Then
            mblnNoQuit = True
         End If
      Next

      'make form size dynamic
      Me.Location = New Point(0, 0)
      lblStatus.Width = Me.Width
      lblStatus.Location = New Point(0, Me.Height - 86)
      grpButtons.Location = New Point(Me.Width - 425, lblStatus.Location.Y + 1)
      chkSimulate1.Location = New Point(chkSimulate1.Location.X, lblStatus.Location.Y + 21)
      chkSimulate2.Location = New Point(chkSimulate2.Location.X, chkSimulate1.Location.Y + 21)
      lblInfo.Location = New Point(lblInfo.Location.X, lblStatus.Location.Y - 15)
      pnlMachinesInfo.Width = Me.Width
      pnlDetails.Width = Me.Width
      pnlDetails.Height = lblInfo.Location.Y - pnlDetails.Location.Y

      'set correct tooltips for tray icons
      tt.SetToolTip(picUndefinedActi, "Undefined Activity's")
      tt.SetToolTip(picSplit, "Split Activity")
      tt.SetToolTip(picBars, "Activity Chart")
      tt.SetToolTip(picExit, "Home/Exit")
      tt.SetToolTip(picLogs, "Logs")
      tt.SetToolTip(picOEEChart, "OEE Chart")

      lblInfo.Text = My.Application.Info.AssemblyName & _
                     " (version " & My.Application.Info.Version.ToString & ")"

      gfblnWriteLog("Manually started OEECollector", 4)

      'get all machine related info
      Call gsGetMachineInfo()
      Call gfblnGetStatusDescription()
      Call gfblnGetMachActivities()
      Call gfblnGetOrderInformation()
      Call gfblnGetArticleInformation()
      Call gfblnGetTeamShift()

      For intX = 0 To gintMachineCount - 1
         garrMachine(intX).intCurrStatus = 7
         Call gfblnAddReg(intX, 0, 0, _
                         "Predefined", "Start OEECollector", False, 0, 0)
         Thread.Sleep(1000)
         If garrMachine(intX).intModuleTypeNr = 1 Then
            'Disabled counter reset
            gfblnResetIPModuleCounter(intX, garrMachine(intX).strCommIPAddress, garrMachine(intX).strResetCounter)
         ElseIf garrMachine(intX).intModuleTypeNr = 2 Then
            'Disabled counter reset            
            gfblnResetComModuleCounter(intX, garrMachine(intX).strCommComport, garrMachine(intX).strCommBitsPerSecond, garrMachine(intX).strCommDataBits,
            garrMachine(intX).strCommParity, garrMachine(intX).strSensorAddress)
         ElseIf garrMachine(intX).intModuleTypeNr = 5 Then
            If intX = 0 Then
               chkSimulate1.Visible = True
            ElseIf intX = 1 Then
               chkSimulate2.Visible = True
            End If
         End If
      Next intX

      tmrPerformJob.Enabled = True
      tmrRefresh.Enabled = True

      For intX = 0 To gintMachineCount - 1
         garrOEE(intX).intPerformance = 100
         garrMachine(intX).datCurrProdStartTime = Date.Now
         Select Case intX
            Case 0
               If Not Me.BackgroundWorker1.IsBusy Then
                  Me.BackgroundWorker1.RunWorkerAsync()
               End If
            Case 1
               If Not Me.BackgroundWorker2.IsBusy Then
                  Me.BackgroundWorker2.RunWorkerAsync()
               End If
            Case 2
               If Not Me.BackgroundWorker3.IsBusy Then
                  Me.BackgroundWorker3.RunWorkerAsync()
               End If
            Case 3
               If Not Me.BackgroundWorker4.IsBusy Then
                  Me.BackgroundWorker4.RunWorkerAsync()
               End If
            Case 4
               If Not Me.BackgroundWorker5.IsBusy Then
                  Me.BackgroundWorker5.RunWorkerAsync()
               End If
         End Select
      Next

      gsChangePanel(pnlMachinesInfo, frmOEEMachSelect)

   End Sub


   Private Sub tmrPerformJob_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrPerformJob.Tick

      'tmrPerformJob.Enabled = False

      For intX = 0 To gintMachineCount - 1
         Select Case intX
            Case 0
               If Not Me.BackgroundWorker1.IsBusy Then
                  Me.BackgroundWorker1.RunWorkerAsync()
               End If
            Case 1
               If Not Me.BackgroundWorker2.IsBusy Then
                  Me.BackgroundWorker2.RunWorkerAsync()
               End If
            Case 2
               If Not Me.BackgroundWorker3.IsBusy Then
                  Me.BackgroundWorker3.RunWorkerAsync()
               End If
            Case 3
               If Not Me.BackgroundWorker4.IsBusy Then
                  Me.BackgroundWorker4.RunWorkerAsync()
               End If
            Case 4
               If Not Me.BackgroundWorker5.IsBusy Then
                  Me.BackgroundWorker5.RunWorkerAsync()
               End If
         End Select
      Next

      tmrPerformJob.Enabled = True

   End Sub


   Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

      'Call gfblnGetTeamShift()
      If mintGarbageCleaner = 600 Then
         mfblnGarbageCleaner("JeDen OEECollector")
         mintGarbageCleaner = 0
      Else
         mintGarbageCleaner = mintGarbageCleaner + 1
      End If
      gfblnGetMachineStatus(0)

   End Sub


   Private Sub BackgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork

      If BackgroundWorker2.CancellationPending = True Then
         e.Cancel = True
      Else
         gfblnGetMachineStatus(1)
      End If

   End Sub


   Private Sub BackgroundWorker3_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork

      gfblnGetMachineStatus(2)

   End Sub


   Private Sub BackgroundWorker4_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker4.DoWork

      gfblnGetMachineStatus(3)

   End Sub


   Private Sub BackgroundWorker5_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker5.DoWork

      gfblnGetMachineStatus(4)

   End Sub


   Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

   End Sub


   Private Sub BackgroundWorker2_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker2.RunWorkerCompleted

   End Sub


   Private Sub BackgroundWorker3_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker3.RunWorkerCompleted

   End Sub


   Private Sub BackgroundWorker4_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker4.RunWorkerCompleted

   End Sub


   Private Sub BackgroundWorker5_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker5.RunWorkerCompleted

   End Sub


   Public Function mfblnUpdateMachDetailsForm(ByVal intSelMach As Integer) As Boolean

      Dim intCurrActiGroupNr As Integer
      Dim intCurrActivityNr As Integer

      frmMachineDetails.Width = Screen.PrimaryScreen.Bounds.Width

      If mblnNoQuit Then
         picExit.Visible = True
      End If
      picBars.Visible = True
      picLogs.Visible = True
      picOEEChart.Visible = True
      picSplit.Visible = True
      picUndefinedActi.Visible = True

      frmMachineDetails.lblMachinename.Text = garrMachine(intSelMach).intMachineCode & " - " & _
                               garrMachine(intSelMach).strMachine
      frmMachineDetails.lblTimenr.Text = DateTime.Now.ToString("HH:mm")
      frmMachineDetails.lblShiftname.Text = garrTeamShift.strShift
      frmMachineDetails.lblTeamname.Text = garrTeamShift.strTeam
      If garrMachine(intSelMach).intSensorType = 1 Then
         If mintTimerInterval >= 2 Then
            frmMachineDetails.lblLineSpeednr.Text = mintLineSpeedSample / 3 * 60
            Dim inttest As Integer = (mintLineSpeedSample * 60) / 3
            mintTimerInterval = 0
            mintLineSpeedSample = 0
         Else
            mintLineSpeedSample = mintLineSpeedSample + garrMachine(intSelMach).lngCounter - garrMachine(intSelMach).lngLastCounter
            mintTimerInterval = mintTimerInterval + 1
         End If
         frmMachineDetails.lblLineSpeednr.Text = garrMachine(intSelMach).lngAvgSpeed
      End If
      frmMachineDetails.lblCounterValue.Text = garrMachine(intSelMach).lngCounter
      frmMachineDetails.lblMachineUnit.Text = garrMachine(intSelMach).strMachUnit

      Select Case garrMachine(intSelMach).intCurrStatus
         Case 7
            'Initialize
            Call mfblnUpdateColor(Color.LightSkyBlue)
         Case 1
            'Undefined Production
            frmMachineDetails.lblNormSpeednr.Text = garrMachine(intSelMach).intNormSpeed
            intCurrActivityNr = garrMachine(intSelMach).intCurrActivityNr
            If intCurrActivityNr = 0 Then
               intCurrActivityNr = 1
            End If
            If garrMachine(intSelMach).intUndefinedProdNr = intCurrActivityNr Then
               frmMachineDetails.lblActivitygroupname.Text = "Production"
               frmMachineDetails.lblActivityname.Text = "Undefined Production"
               If mintRefeshInterval > 10 Then
                  If frmMachineDetails.BackColor = Color.MediumPurple Then
                     Call mfblnUpdateColor(Color.LightYellow)
                  Else
                     Call mfblnUpdateColor(Color.MediumPurple)
                  End If
                  mintRefeshInterval = 0
               Else
                  mintRefeshInterval = mintRefeshInterval + 1
               End If
            Else
               Call mfblnUpdateColor(Color.Green)
            End If
         Case 2
            'Defined Production
            Call mfblnUpdateColor(Color.LightGreen)
            frmMachineDetails.lblActivitygroupname.Text = "Production"



            'Only apply new order description and normspeed when last record is modified
            If gintUndefinedTableKeyID = gflngGetLastTableKeyId(gintSelectedMach) Then
               If garrMachine(intSelMach).strCurrArticleNr = "" Then
                  frmMachineDetails.lblActivityname.Text = garrMachine(intSelMach).strCurrOrderNr & " - " & garrMachine(intSelMach).strCurrOrderDescription
                  frmMachineDetails.lblNormSpeednr.Text = garrMachine(intSelMach).intCurrOrderNormSpeed

               End If
               If garrMachine(intSelMach).strCurrOrderNr = "" Then
                  frmMachineDetails.lblActivityname.Text = garrMachine(intSelMach).strCurrArticleNr & " - " & garrMachine(intSelMach).strCurrArticleDescription
                  frmMachineDetails.lblNormSpeednr.Text = garrMachine(intSelMach).intCurrArticleNormSpeed
               End If
            ElseIf gintUndefinedTableKeyID = 0 Then
               If garrMachine(intSelMach).strCurrArticleNr = "" Then
                  frmMachineDetails.lblActivityname.Text = garrMachine(intSelMach).strCurrOrderNr & " - " & garrMachine(intSelMach).strCurrOrderDescription
                  frmMachineDetails.lblNormSpeednr.Text = garrMachine(intSelMach).intCurrOrderNormSpeed
               End If
               If garrMachine(intSelMach).strCurrOrderNr = "" Then
                  frmMachineDetails.lblActivityname.Text = garrMachine(intSelMach).strCurrArticleNr & " - " & garrMachine(intSelMach).strCurrArticleDescription
                  frmMachineDetails.lblNormSpeednr.Text = garrMachine(intSelMach).intCurrArticleNormSpeed
               End If

            End If
         Case 3
            'Shortbreak
            frmMachineDetails.lblActivitygroupname.Text = "Idle"
            frmMachineDetails.lblActivityname.Text = "Short break detected"
            Call mfblnUpdateColor(Color.LightYellow)

         Case 4
            'Undefined standstill
            intCurrActiGroupNr = garrMachine(intSelMach).intCurrActiGroupNr
            intCurrActivityNr = garrMachine(intSelMach).intCurrActivityNr
            If garrMachine(intSelMach).intUndefinedStopNr = intCurrActivityNr Then
               frmMachineDetails.lblActivitygroupname.Text = "Idle"
               frmMachineDetails.lblActivityname.Text = "Select Stop activity"
               If mintRefeshInterval > 10 Then
                  If frmMachineDetails.BackColor = Color.MediumPurple Then
                     Call mfblnUpdateColor(Color.LightYellow)
                  Else
                     Call mfblnUpdateColor(Color.MediumPurple)
                  End If
                  mintRefeshInterval = 0
               Else
                  mintRefeshInterval = mintRefeshInterval + 1
               End If
               If gfblnControlExists("frmActivityInput", pnlDetails) = False Then
                  gsChangePanel(pnlDetails, frmActivityInput)
                  frmActivityInput.mfblnShowActivty(gintSelectedMach, 1, True)
               End If
            Else
            End If
         Case 5
            'Defined standstill
            If garrMachine(intSelMach).blnErrMessDisplayed Then
               frmMachineDetails.lblActivitygroupname.Text = "Failure"
               frmMachineDetails.lblActivityname.Text = garrMachine(intSelMach).strIOFailure
               frmMachineDetails.BackColor = Color.FromArgb("-2396013")
               Me.picSplit.Visible = False
            Else
               Try
                  intCurrActiGroupNr = garrMachine(intSelMach).intCurrActiGroupNr
                  intCurrActivityNr = garrMachine(intSelMach).intCurrActivityNr
                  frmMachineDetails.lblActivitygroupname.Text = garrMachine(intSelMach).ActiGroup(intCurrActiGroupNr).strActiGroup(intCurrActivityNr)
                  frmMachineDetails.lblActivityname.Text = garrMachine(intSelMach).ActiGroup(intCurrActiGroupNr).strActi(intCurrActivityNr)
                  frmMachineDetails.BackColor = Color.FromArgb(garrMachine(intSelMach).ActiGroup(intCurrActiGroupNr).intColorNr(intCurrActivityNr))
               Catch ex As Exception

               End Try
            End If

      End Select

      frmMachineDetails.ChartOEE.Series(0).Points(0).YValues(0) = garrOEE(intSelMach).intAvailability
      frmMachineDetails.ChartOEE.Series(0).Points(1).YValues(0) = garrOEE(intSelMach).intPerformance
      frmMachineDetails.ChartOEE.Series(0).Points(2).YValues(0) = garrOEE(intSelMach).intQuality
      frmMachineDetails.ChartOEE.Series(0).Points(3).YValues(0) = garrOEE(intSelMach).intOEE
      frmMachineDetails.ChartOEE.Series(0).Points.ResumeUpdates()

      If gfblnControlExists("frmActivityStacked", pnlDetails) Then
         frmActivityStacked.mfblnRefresh(gintSelectedMach)
      End If

   End Function


   Public Function mfblnUpdateMachSelectForm(ByVal intSelMach As Integer) As Boolean

      If mblnNoQuit Then
         picExit.Visible = False
      End If
      picBars.Visible = False
      picLogs.Visible = False
      picOEEChart.Visible = False
      picSplit.Visible = False
      picUndefinedActi.Visible = False

      Dim intCurrActiGroupNr As Integer
      Dim intCurrActivityNr As Integer

      If intSelMach = 1 Then
         intSelMach = 1
      End If


      glblMachTop(intSelMach).Text = garrMachine(intSelMach).intMachineCode & " - " & garrMachine(intSelMach).strMachine

      Select Case garrMachine(intSelMach).intCurrStatus
         Case 7
            'Initialize
            gclrActivityColor(intSelMach) = Color.LightSkyBlue
         Case 1
            'Undefined Production
            intCurrActivityNr = garrMachine(intSelMach).intCurrActivityNr
            If intCurrActivityNr = 0 Then
               intCurrActivityNr = 1
            End If
            If garrMachine(intSelMach).intUndefinedProdNr = intCurrActivityNr Then
               glblMachMsg(intSelMach).Text = "Undefined production"
               If mintRefeshInterval > 10 Then
                  If gclrActivityColor(intSelMach) = Color.MediumPurple Then
                     gclrActivityColor(intSelMach) = Color.LightYellow
                  Else
                     gclrActivityColor(intSelMach) = Color.MediumPurple
                  End If
                  mintRefeshInterval = 0
               Else
                  mintRefeshInterval = mintRefeshInterval + 1
               End If
            Else
            End If
         Case 2
            'Defined Production
            gclrActivityColor(intSelMach) = Color.LightGreen
            If garrMachine(intSelMach).strCurrArticleNr = "" Then
               glblMachMsg(intSelMach).Text = garrMachine(intSelMach).strCurrOrderNr & " - " & garrMachine(intSelMach).strCurrOrderDescription
            End If
            If garrMachine(intSelMach).strCurrOrderNr = "" Then
               glblMachMsg(intSelMach).Text = garrMachine(intSelMach).strCurrArticleNr & " - " & garrMachine(intSelMach).strCurrArticleDescription
            End If

         Case 3
            'Shortbreak
            gclrActivityColor(intSelMach) = Color.LightYellow
            glblMachMsg(intSelMach).Text = "Short break"

         Case 4
            'Undefined Activity
            'update activity info on MachSelect form
            intCurrActiGroupNr = garrMachine(intSelMach).intCurrActiGroupNr
            intCurrActivityNr = garrMachine(intSelMach).intCurrActivityNr
            If garrMachine(intSelMach).intUndefinedStopNr = intCurrActivityNr Then
               glblMachMsg(intSelMach).Text = "Select Stop Activity"
               If mintRefeshInterval > 10 Then
                  If gclrActivityColor(intSelMach) = Color.MediumPurple Then
                     gclrActivityColor(intSelMach) = Color.LightYellow
                  Else
                     gclrActivityColor(intSelMach) = Color.MediumPurple
                  End If
                  mintRefeshInterval = 0
               Else
                  mintRefeshInterval = mintRefeshInterval + 1
               End If
            Else
            End If
         Case 5
            'Defined Activity
            If garrMachine(intSelMach).blnErrMessDisplayed Then
               glblMachMsg(intSelMach).Text = garrMachine(intSelMach).strIOFailure
               gclrActivityColor(intSelMach) = Color.FromArgb("-2396013")
            Else
               'Try
               intCurrActiGroupNr = garrMachine(intSelMach).intCurrActiGroupNr
               intCurrActivityNr = garrMachine(intSelMach).intCurrActivityNr
               glblMachMsg(intSelMach).Text = garrMachine(intSelMach).ActiGroup(intCurrActiGroupNr).strActi(intCurrActivityNr)
               gclrActivityColor(intSelMach) = Color.FromArgb(garrMachine(intSelMach).ActiGroup(intCurrActiGroupNr).intColorNr(intCurrActivityNr))
               'Catch ex As Exception

               'End Try
            End If

      End Select

      glblMachMain(intSelMach).BackColor = gclrActivityColor(intSelMach)
      gchaMachChartOEE(intSelMach).BackColor = gclrActivityColor(intSelMach)
      glblMachMsg(intSelMach).BackColor = gclrActivityColor(intSelMach)
      glblMachTop(intSelMach).BackColor = gclrActivityColor(intSelMach)
      gpicMachOEENorm(intSelMach).BackColor = gclrActivityColor(intSelMach)
      gpicMachNotify(intSelMach).BackColor = gclrActivityColor(intSelMach)

      gfblnCalcOEE(intSelMach)
      gchaMachChartOEE(intSelMach).Series("Series1").Points(0).YValues(0) = garrOEE(intSelMach).intAvailability
      gchaMachChartOEE(intSelMach).Series("Series1").Points(1).YValues(0) = garrOEE(intSelMach).intPerformance
      gchaMachChartOEE(intSelMach).Series("Series1").Points(2).YValues(0) = garrOEE(intSelMach).intQuality
      gchaMachChartOEE(intSelMach).Series("Series1").Points(3).YValues(0) = garrOEE(intSelMach).intOEE
      gchaMachChartOEE(intSelMach).Series("Series1").Points.ResumeUpdates()

   End Function


   Public Function mfblnUpdateColor(ByVal clrLabelColor As System.Drawing.Color) As Boolean

      Dim clrActColor As System.Drawing.Color
      Dim clrActTextColor As System.Drawing.Color

      clrActTextColor = Color.Black
      clrActColor = clrLabelColor

      frmMachineDetails.BackColor = clrActColor

   End Function


   Private Sub picExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picExit.Click

      Call msbExit()

   End Sub


   Private Sub lblExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

      Call msbExit()

   End Sub


   Public Sub msbExit()


      'at close check for open activity records and prompt for defining undefined activities

      If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
         If mblnNoQuit Then
            picExit.Visible = False
         Else
            picExit.ImageLocation = gstrImagesLocation & "Action-exit-icon.png"
         End If
         gsChangePanel(pnlMachinesInfo, frmOEEMachSelect)
         gsChangePanel(pnlDetails, frmEmpty)
         picBars.Enabled = False
      Else
         garrMessageSystem(gintSelectedMach).strTitle = "Exit Jeden OEECollector"
         garrMessageSystem(gintSelectedMach).strMessage = "Are you sure you want to quit?" & vbCrLf & "No machine data will be registered!"
         garrMessageSystem(gintSelectedMach).intAction = 99
         frmMessageBox.ShowDialog()
         frmMessageBox.lblTitle.Text = garrMessageSystem(gintSelectedMach).strTitle
         frmMessageBox.lblMessage.Text = garrMessageSystem(gintSelectedMach).strMessage
         frmMessageBox.Refresh()
      End If

   End Sub


   Private Sub picOEEChart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOEEChart.Click

      If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
         If Not gfblnControlExists("frmOeeProgress", pnlDetails) Then
            gsChangePanel(pnlDetails, frmOeeProgress)
         End If
         frmOeeProgress.mfblnRefresh(gintSelectedMach)
      End If

   End Sub


   Private Sub picBars_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picBars.Click

      If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
         If Not gfblnControlExists("frmActivityStacked", pnlDetails) Then
            gsChangePanel(pnlDetails, frmActivityStacked)
         End If
         frmActivityStacked.mfblnRefresh(gintSelectedMach)
      End If

   End Sub


   Private Sub picUndefinedActi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picUndefinedActi.Click

      If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
         gsChangePanel(pnlDetails, frmUndefinedActivities)
         frmUndefinedActivities.gfblnShowUndefined(gintSelectedMach, 1)
      End If

   End Sub


   Private Sub picLogs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picLogs.Click

      If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
         gsChangePanel(pnlDetails, frmActivityShiftLog)
         frmActivityShiftLog.mfblnShowActivities(gintSelectedMach)
      End If

   End Sub


   Private Sub picSplit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picSplit.Click

      If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
         garrMessageSystem(gintSelectedMach).strTitle = "Split current activity"
         garrMessageSystem(gintSelectedMach).strMessage = "Are you sure you want to split the current activity?"
         garrMessageSystem(gintSelectedMach).intAction = 1
         frmMessageBox.ShowDialog()
         frmMessageBox.lblTitle.Text = garrMessageSystem(gintSelectedMach).strTitle
         frmMessageBox.lblMessage.Text = garrMessageSystem(gintSelectedMach).strMessage
         frmMessageBox.Refresh()
      End If

   End Sub


   Private Sub chkSimulate1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSimulate1.CheckedChanged

      If chkSimulate1.CheckState = 1 Then
         gblnEmulateProduction(0) = True
      Else
         gblnEmulateProduction(0) = False
      End If

   End Sub


   Private Sub chkSimulate2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSimulate2.CheckedChanged

      If chkSimulate2.CheckState = 1 Then
         gblnEmulateProduction(1) = True
      Else
         gblnEmulateProduction(1) = False
      End If

   End Sub


   Private Sub tmrRefresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick

      Dim intX As Integer

      Call gfblnScheduledAlerts()

      For intX = 0 To gintMachineCount - 1
         If gfblnControlExists("frmMachineDetails", pnlMachinesInfo) Then
            If gintSelectedMach = intX Then
               mfblnUpdateMachDetailsForm(intX)
            End If
         ElseIf gfblnControlExists("frmOEEMachSelect", pnlMachinesInfo) Then
            mfblnUpdateMachSelectForm(intX)
         End If
      Next

   End Sub

   Private Sub tmrSync_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSync.Tick

      Dim intX As Integer

      For intX = 0 To gintMachineCount - 1
         gfblnSyncData(intX)
      Next
      gfblnGetTeamShift()

   End Sub

End Class