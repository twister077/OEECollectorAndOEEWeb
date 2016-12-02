Public Class frmActivityInput

   Dim mlblSet(13) As Label
   Public mintNextCount As Integer
   Public mintActivity As Integer
   Public mblnInitActivity As Boolean


   Private Sub frmActivityInput_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      Me.Width = frmOEEMain.Width
      Me.Height = frmOEEMain.pnlDetails.Height

      'put activitylabels in an array
      mlblSet(0) = lblActivity1
      mlblSet(1) = lblActivity2
      mlblSet(2) = lblActivity3
      mlblSet(3) = lblActivity4
      mlblSet(4) = lblActivity5
      mlblSet(5) = lblActivity6
      mlblSet(6) = lblActivity7
      mlblSet(7) = lblActivity8
      mlblSet(8) = lblActivity9
      mlblSet(9) = lblActivity10
      mlblSet(10) = lblActivity11
      mlblSet(11) = lblActivity12
      mlblSet(12) = lblActivity13
      mlblSet(13) = lblActivity14

      mfblnShowActivty(gintSelectedMach, 1, True)

   End Sub


   Public Function mfblnShowActivty(ByVal intSelMach As Integer, ByVal intActivity As Integer, ByVal blnInitiate As Boolean) As Boolean

      Dim intX As Integer
      Dim intCount As Integer

      mlblSet(0) = lblActivity1
      mlblSet(1) = lblActivity2
      mlblSet(2) = lblActivity3
      mlblSet(3) = lblActivity4
      mlblSet(4) = lblActivity5
      mlblSet(5) = lblActivity6
      mlblSet(6) = lblActivity7
      mlblSet(7) = lblActivity8
      mlblSet(8) = lblActivity9
      mlblSet(9) = lblActivity10
      mlblSet(10) = lblActivity11
      mlblSet(11) = lblActivity12
      mlblSet(12) = lblActivity13
      mlblSet(13) = lblActivity14

      'which group is selected (minus 1)
      mintActivity = intActivity

      'if true then run initial loss selection
      If blnInitiate Then
         For intX = 0 To 13
            If intX < garrMachine(intSelMach).intActiGroupCount Then
               mlblSet(intX).Visible = True
               mlblSet(intX).Text = Trim(garrMachine(intSelMach).ActiGroup(intX).strActiGroup(0))
               mlblSet(intX).TextAlign = ContentAlignment.MiddleCenter
               mlblSet(intX).BackColor = Color.FromArgb(garrMachine(intSelMach).ActiGroup(intX).intColorNr(0))
            Else
               mlblSet(intX).Visible = False
            End If
         Next

         mblnInitActivity = True
         Exit Function
      End If

      intCount = garrMachine(intSelMach).ActiGroup(intActivity - 1).intActiCount
      For intX = 0 To 13
         'redefine first index of label
         If intX = 0 Then
            gintStartIndexSelected = intX + mintNextCount
         End If
         If Not mintNextCount + intX >= intCount Then
            mlblSet(intX).Visible = True
            mlblSet(intX).BackColor = Color.FromArgb(garrMachine(intSelMach).ActiGroup(intActivity - 1).intColorNr(0))
            mlblSet(intX).Text = garrMachine(intSelMach).ActiGroup(intActivity - 1).strActi(intX + mintNextCount)
         Else
            mlblSet(intX).Visible = False
            mlblSet(intX).Text = ""
         End If
         If intX = 13 Then
            If Not mintNextCount + intX >= intCount Then
               mintNextCount = mintNextCount + 13
               mlblSet(intX).Text = "Next _______"
               mlblSet(intX).Visible = True
               mlblSet(intX).BackColor = Color.FromArgb(garrMachine(intSelMach).ActiGroup(intActivity - 1).intColorNr(0))
            Else
               mlblSet(intX).Text = "Previous _______"
               mlblSet(intX).Visible = True
               mlblSet(intX).BackColor = Color.FromArgb(garrMachine(intSelMach).ActiGroup(intActivity - 1).intColorNr(0))
            End If
         End If
      Next

   End Function


   Private Sub lblActivity1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity1.Click

      Call mfblnSelectActivity(0)

   End Sub


   Private Sub lblActivity2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity2.Click

      Call mfblnSelectActivity(1)

   End Sub


   Private Sub lblActivity3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity3.Click

      Call mfblnSelectActivity(2)

   End Sub


   Private Sub lblActivity4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity4.Click

      Call mfblnSelectActivity(3)

   End Sub


   Private Sub lblActivity5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity5.Click

      Call mfblnSelectActivity(4)

   End Sub


   Private Sub lblActivity6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity6.Click

      Call mfblnSelectActivity(5)

   End Sub


   Private Sub lblActivity7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity7.Click

      Call mfblnSelectActivity(6)

   End Sub


   Private Sub lblActivity8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity8.Click

      Call mfblnSelectActivity(7)

   End Sub


   Private Sub lblActivity9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity9.Click

      Call mfblnSelectActivity(8)

   End Sub


   Private Sub lblActivity10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity10.Click

      Call mfblnSelectActivity(9)

   End Sub


   Private Sub lblActivity11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity11.Click

      Call mfblnSelectActivity(10)

   End Sub


   Private Sub lblActivity12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity12.Click

      Call mfblnSelectActivity(11)

   End Sub


   Private Sub lblActivity13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity13.Click

      Call mfblnSelectActivity(12)

   End Sub


   Public Function mfblnSelectActivity(ByVal intSelectedLbl As Integer) As Boolean

      Dim intActiGroupNr As Integer
      Dim intActivityNr As Integer
      Dim intCurrActiGroupNr As Integer
      Dim intCurrActivityNr As Integer

      If mblnInitActivity = True Then
         mblnInitActivity = False
         mintActivity = intSelectedLbl + 1
         Call mfblnShowActivty(gintSelectedMach, mintActivity, False)
      Else

         'check if current activity is defined or previous undefined is now defined
         If gintUndefinedTableKeyID = 0 Then
            garrMachine(gintSelectedMach).intCurrActiGroupNr = mintActivity - 1
            garrMachine(gintSelectedMach).intCurrActivityNr = gintStartIndexSelected + intSelectedLbl
            intCurrActiGroupNr = mintActivity - 1
            intCurrActivityNr = gintStartIndexSelected + intSelectedLbl
            garrMachine(gintSelectedMach).intCurrStatus = 5

            intActiGroupNr = mintActivity - 1
            intActivityNr = gintStartIndexSelected + intSelectedLbl
         Else
            intActiGroupNr = mintActivity - 1
            intActivityNr = gintStartIndexSelected + intSelectedLbl
         End If

         'write activity info to regtable
         Call gfblnDefineActivity(gintSelectedMach, _
                                    5, _
                                    garrMachine(gintSelectedMach).ActiGroup(intActiGroupNr).intActiGroupNr(intActivityNr), _
                                    garrMachine(gintSelectedMach).ActiGroup(intActiGroupNr).intActiNr(intActivityNr), _
                                    garrMachine(gintSelectedMach).ActiGroup(intActiGroupNr).strActiGroup(intActivityNr),
                                    garrMachine(gintSelectedMach).ActiGroup(intActiGroupNr).strActi(intActivityNr), _
                                    gintUndefinedTableKeyID, _
                                    garrMachine(gintSelectedMach).ActiGroup(intActiGroupNr).intActiGroupCalcForOEE(intActivityNr))
         'garrMachine(gintSelectedMach).intCurrActiGroupNr = intActiGroupNr
         'garrMachine(gintSelectedMach).intCurrActivityNr = intActivityNr

         'reset nextcount for new appearance
         mintNextCount = 0
         Me.Hide()

         Call gfblnGetUndefined(gintSelectedMach)
         Select Case garrMachine(gintSelectedMach).intPendingStand
            Case 0
               'no pending undefined prod
               Select Case garrMachine(gintSelectedMach).intPendingProd
                  Case 0
                     'no pending undefined prod and standstills
                     'get current status and based on that forward to correct view
                     Select Case garrMachine(gintSelectedMach).intCurrStatus
                        Case 1
                           gsChangePanel(frmOEEMain.pnlDetails, frmOeeProgress)
                        Case 2
                           gsChangePanel(frmOEEMain.pnlDetails, frmOeeProgress)
                        Case 3
                           gsChangePanel(frmOEEMain.pnlDetails, frmActivityStacked)
                        Case 4
                           gsChangePanel(frmOEEMain.pnlDetails, frmActivityStacked)
                        Case 5
                           gsChangePanel(frmOEEMain.pnlDetails, frmActivityStacked)
                     End Select
                  Case Is >= 1
                     'no pending undefined prod but still pending undefined standstills
                     gsChangePanel(frmOEEMain.pnlDetails, frmUndefinedActivities)
                     frmUndefinedActivities.gfblnShowUndefined(gintSelectedMach, 1)
               End Select
            Case Is >= 1
               'stll pending undefined prod
               gsChangePanel(frmOEEMain.pnlDetails, frmUndefinedActivities)
               frmUndefinedActivities.gfblnShowUndefined(gintSelectedMach, 2)
         End Select
      End If

   End Function


   Private Sub lblActivity14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActivity14.Click

      Call msbNextActivities()

   End Sub


   Public Sub msbNextActivities()

      If lblActivity14.Text = "Next _______" Then
         Call mfblnShowActivty(gintSelectedMach, mintActivity, False)
      ElseIf lblActivity14.Text = "Previous _______" Then
         mintNextCount = 0
         Call mfblnShowActivty(gintSelectedMach, mintActivity, True)
      Else

      End If

   End Sub

End Class