Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing

Public Class frmOEEMachSelect

   Private Sub frmOEEMachineSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      Dim intX As Integer
      Dim tt As New ToolTip()

      'put all graphical items in an array
      glblMachMain(0) = lblMachine1
      glblMachMain(1) = lblMachine2
      glblMachMain(2) = lblMachine3
      glblMachMain(3) = lblMachine4
      glblMachMain(4) = lblMachine5
      gchaMachChartOEE(0) = ChartOEE1
      gchaMachChartOEE(1) = ChartOEE2
      gchaMachChartOEE(2) = ChartOEE3
      gchaMachChartOEE(3) = ChartOEE4
      gchaMachChartOEE(4) = ChartOEE5
      glblMachTop(0) = lblMachTop1
      glblMachTop(1) = lblMachTop2
      glblMachTop(2) = lblMachTop3
      glblMachTop(3) = lblMachTop4
      glblMachTop(4) = lblMachTop5
      glblMachMsg(0) = lblMachMsg1
      glblMachMsg(1) = lblMachMsg2
      glblMachMsg(2) = lblMachMsg3
      glblMachMsg(3) = lblMachMsg4
      glblMachMsg(4) = lblMachMsg5
      gpicMachOEENorm(0) = picOEENorm1
      gpicMachOEENorm(1) = picOEENorm2
      gpicMachOEENorm(2) = picOEENorm3
      gpicMachOEENorm(3) = picOEENorm4
      gpicMachOEENorm(4) = picOEENorm5
      gpicMachNotify(0) = picNotify1
      gpicMachNotify(1) = picNotify2
      gpicMachNotify(2) = picNotify3
      gpicMachNotify(3) = picNotify4
      gpicMachNotify(4) = picNotify5

      'loop through label arrays and load default
      For intX = 0 To gintMachineCount - 1
         glblMachMain(intX).Visible = True
         gchaMachChartOEE(intX).Visible = True
         glblMachTop(intX).Visible = True
         glblMachMsg(intX).Visible = True
         gpicMachOEENorm(intX).Visible = True
         gpicMachNotify(intX).Visible = True
         glblMachMsg(intX).Text = "Initializing ......."
         gchaMachChartOEE(intX).ChartAreas("ChartArea1").AxisY.Maximum = 100
         gchaMachChartOEE(intX).ChartAreas("ChartArea1").AxisX.LabelStyle.IsEndLabelVisible = False
         gchaMachChartOEE(intX).ChartAreas("ChartArea1").AxisX.LabelStyle.Enabled = False
         gchaMachChartOEE(intX).ChartAreas("ChartArea1").AxisY.Enabled = DataVisualization.Charting.AxisEnabled.False
         gchaMachChartOEE(intX).ChartAreas(0).AxisX.LineWidth = 0
         gchaMachChartOEE(intX).ChartAreas(0).AxisY.LineWidth = 0
         gchaMachChartOEE(intX).ChartAreas(0).AxisX.LabelStyle.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisY.LabelStyle.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisX.MajorGrid.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisY.MajorGrid.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisX.MinorGrid.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisY.MinorGrid.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisX.MajorTickMark.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisY.MajorTickMark.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisX.MinorTickMark.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).AxisY.MinorTickMark.Enabled = False
         gchaMachChartOEE(intX).ChartAreas(0).BackImageTransparentColor = Color.Black
         tt.SetToolTip(gpicMachOEENorm(intX), "OEE Norm")
         tt.SetToolTip(gchaMachChartOEE(intX), "OEE Bars")
         tt.SetToolTip(gpicMachNotify(intX), "I/O Module status")
         gpicMachOEENorm(intX).ImageLocation = gstrImagesLocation & "greendot.png"
         Me.Refresh()
      Next
      Me.Refresh()

   End Sub


   Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick

      'tmrUpdate.Enabled = False
      'Call gsbMachNewCounter()
      'tmrUpdate.Enabled = True

   End Sub


   Public Sub msbMachClick1()

      frmOEEMain.picExit.ImageLocation = gstrImagesLocation & "home-icon.png"
      frmOEEMain.picBars.Enabled = True
      gintSelectedMach = 0
      mfblnMachForwardTo(gintSelectedMach)

   End Sub


   Public Sub msbMachClick2()

      frmOEEMain.picExit.ImageLocation = gstrImagesLocation & "home-icon.png"
      frmOEEMain.picBars.Enabled = True
      gintSelectedMach = 1
      mfblnMachForwardTo(gintSelectedMach)

   End Sub


   Public Sub msbMachClick3()

      frmOEEMain.picExit.ImageLocation = gstrImagesLocation & "home-icon.png"
      frmOEEMain.picBars.Enabled = True
      gintSelectedMach = 2
      mfblnMachForwardTo(gintSelectedMach)

   End Sub


   Public Sub msbMachClick4()

      frmOEEMain.picExit.ImageLocation = gstrImagesLocation & "home-icon.png"
      frmOEEMain.picBars.Enabled = True
      gintSelectedMach = 3
      mfblnMachForwardTo(gintSelectedMach)

   End Sub


   Public Sub msbMachClick5()

      frmOEEMain.picExit.ImageLocation = gstrImagesLocation & "home-icon.png"
      frmOEEMain.picBars.Enabled = True
      gintSelectedMach = 4
      mfblnMachForwardTo(gintSelectedMach)

   End Sub


   Private Function mfblnMachForwardTo(ByVal intSelMach) As Boolean

      'old
      gsChangePanel(frmOEEMain.pnlMachinesInfo, frmMachineDetails)
      Select Case garrMachine(intSelMach).intCurrStatus
         Case 0
            If gfblnControlExists("frmActivityStacked", frmOEEMain.pnlDetails) = False Then
               gsChangePanel(frmOEEMain.pnlDetails, frmActivityStacked)
            End If
         Case 1
            gsChangePanel(frmOEEMain.pnlDetails, frmUndefinedActivities)
            frmUndefinedActivities.gfblnShowUndefined(intSelMach, 1)
         Case 2
            If gfblnControlExists("frmOeeProgress", frmOEEMain.pnlDetails) = False Then
               gsChangePanel(frmOEEMain.pnlDetails, frmOeeProgress)
            End If
         Case 3
            If gfblnControlExists("frmActivityStacked", frmOEEMain.pnlDetails) = False Then
               gsChangePanel(frmOEEMain.pnlDetails, frmActivityStacked)
            End If
         Case 4
            frmActivityInput.mfblnShowActivty(intSelMach, 1, True)
         Case 5
            If gfblnControlExists("frmActivityStacked", frmOEEMain.pnlDetails) = False Then
               gsChangePanel(frmOEEMain.pnlDetails, frmActivityStacked)
            End If
      End Select

   End Function

   Private Sub lblMachine1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachine1.Click

      msbMachClick1()

   End Sub


   Private Sub lblMachTop1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachTop1.Click

      msbMachClick1()

   End Sub


   Private Sub ChartOEE1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOEE1.Click

      msbMachClick1()

   End Sub


   Private Sub lblMachMsg1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachMsg1.Click

      msbMachClick1()

   End Sub


   Private Sub picOEENorm1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOEENorm1.Click

      msbMachClick1()

   End Sub


   Private Sub lblMachTop2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachTop2.Click

      msbMachClick2()

   End Sub


   Private Sub lblMachMsg2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachMsg2.Click

      msbMachClick2()

   End Sub


   Private Sub picOEENorm2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOEENorm2.Click

      msbMachClick2()

   End Sub


   Private Sub lblMachine2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachine2.Click

      msbMachClick2()

   End Sub


   Private Sub ChartOEE2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOEE2.Click

      msbMachClick2()

   End Sub


   Private Sub lblMachTop3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachTop3.Click

      msbMachClick3()

   End Sub


   Private Sub ChartOEE3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOEE3.Click

      msbMachClick3()

   End Sub


   Private Sub lblMachMsg3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachMsg3.Click

      msbMachClick3()

   End Sub


   Private Sub picOEENorm3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOEENorm3.Click

      msbMachClick3()

   End Sub


   Private Sub lblMachine3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachine3.Click

      msbMachClick3()

   End Sub


   Private Sub lblMachTop4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachTop4.Click

      msbMachClick4()

   End Sub


   Private Sub ChartOEE4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOEE4.Click

      msbMachClick4()

   End Sub


   Private Sub lblMachMsg4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachMsg4.Click

      msbMachClick4()

   End Sub


   Private Sub picOEENorm4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOEENorm4.Click

      msbMachClick4()

   End Sub


   Private Sub lblMachine4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachine4.Click

      msbMachClick4()

   End Sub


   Private Sub lblMachTop5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachTop5.Click

      msbMachClick5()

   End Sub


   Private Sub ChartOEE5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChartOEE5.Click

      msbMachClick5()

   End Sub


   Private Sub lblMachMsg5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachMsg5.Click

      msbMachClick5()

   End Sub


   Private Sub picOEENorm5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOEENorm5.Click

      msbMachClick5()

   End Sub


   Private Sub lblMachine5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMachine5.Click

      msbMachClick5()

   End Sub


   Private Sub picNotify1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picNotify1.Click

      msbMachClick1()

   End Sub


   Private Sub picNotify2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picNotify2.Click

      msbMachClick2()

   End Sub


   Private Sub picNotify3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picNotify3.Click

      msbMachClick3()

   End Sub


   Private Sub picNotify4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picNotify4.Click

      msbMachClick4()

   End Sub


   Private Sub picNotify5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picNotify5.Click

      msbMachClick5()

   End Sub


End Class