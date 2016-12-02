Public Class frmMessageBox

   Private Sub frmMessageBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      lblTitle.Text = garrMessageSystem(gintSelectedMach).strTitle
      lblMessage.Text = garrMessageSystem(gintSelectedMach).strMessage

   End Sub


   Private Sub frmMessageBox_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown

      If e.KeyCode = Keys.Enter Then
         Call mfblnShowMessageBox()
         e.Handled = False
      End If

   End Sub


   Private Sub picNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picNo.Click

      Me.Hide()

   End Sub


   Private Sub picOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOk.Click

      Call mfblnShowMessageBox()

   End Sub


   Public Function mfblnShowMessageBox() As Boolean

      Dim intX As Integer

      Select Case garrMessageSystem(gintSelectedMach).intAction
         Case 0
            Environment.Exit(0)
         Case 1
            'call split function
            Select Case garrMachine(gintSelectedMach).intCurrStatus
               Case 2
                  gfblnUpdatePerformance(gintSelectedMach, 0, 0, garrMachine(gintSelectedMach).intNormSpeed)
                  If garrMachine(gintSelectedMach).intSensorType = 1 Then
                     garrMachine(gintSelectedMach).intAvgSpeedCounter = 0
                     garrMachine(gintSelectedMach).lngAvgCalcSpeed = 0
                     garrMachine(gintSelectedMach).lngCounter = 0
                     garrMachine(gintSelectedMach).lngLastCounter = 0
                     garrMachine(gintSelectedMach).intAverageSpeed = 0
                     garrMachine(gintSelectedMach).intCurrArticleNormSpeed = 0
                     garrMachine(gintSelectedMach).intCurrOrderNormSpeed = 0
                  End If
                  garrMachine(gintSelectedMach).intCurrStatus = 1
                  garrMachine(gintSelectedMach).intStatusPrev = garrMachine(gintSelectedMach).intCurrStatus
                  Call gfblnAddReg(gintSelectedMach, 0, 0, "Undefined", "Undefined Production", False, 0, 1)
                  garrMachine(gintSelectedMach).intCurrActivityNr = garrMachine(0).intUndefinedProdNr
                  gsChangePanel(frmOEEMain.pnlDetails, frmUndefinedActivities)
                  frmUndefinedActivities.gfblnShowUndefined(gintSelectedMach, 1)
               Case 5
                  garrMachine(gintSelectedMach).intCurrStatus = 4
                  Call gfblnAddReg(gintSelectedMach, garrMachine(gintSelectedMach).intUndefActAndShortBrGrpNr, _
                                   garrMachine(gintSelectedMach).intUndefinedStopNr, _
                                   "Undefined", _
                                   garrMachine(gintSelectedMach).strUndefinedStop, False, 0, 1)
                  garrMachine(gintSelectedMach).intCurrActiGroupNr = garrMachine(gintSelectedMach).intUndefActAndShortBrGrpNr
                  garrMachine(gintSelectedMach).intCurrActivityNr = garrMachine(gintSelectedMach).intUndefinedStopNr
            End Select
            Me.Hide()
         Case 66
            Me.Hide()
         Case 77
            Me.Hide()
         Case (99)
            For intX = 0 To gintMachineCount - 1
               garrMachine(intX).intAverageSpeed = 0
               gfblnUpdateReg(intX, garrMachine(intX).intCurrStatus)

               'counter reset
               garrMachine(intX).lngCounter = 0
               '

               garrMachine(intX).intCurrStatus = 6
               Call gfblnAddReg(intX, 0, _
                                0, _
                                "Predefined", _
                                "Manual exit OEECollector", _
                                False, 0, 1)
               gfblnSyncData(intX)
            Next
            gfblnWriteLog("Manually exited OEECollector", 4)
            Environment.Exit(0)
      End Select

   End Function


   Public Function mblnShowMessage(ByVal intSelMach As Integer) As Boolean

      lblTitle.Text = garrMessageSystem(intSelMach).strTitle
      lblMessage.Text = garrMessageSystem(intSelMach).strMessage

   End Function

End Class