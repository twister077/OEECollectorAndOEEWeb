Imports System.Data.SqlServerCe
Imports System.Data


Public Class frmProductionInput

   Private Sub frmProductionInput_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

      Me.Width = frmOEEMain.Width
      Me.Height = frmOEEMain.pnlDetails.Height
      lstArticle.Width = Me.Width - lstArticle.Location.X - 10
      lstArticle.Height = Me.Height - lstArticle.Location.Y - 10
      grpUndefined.Height = Me.Height - grpUndefined.Location.Y - 10

      mfblnListArticle(gintSelectedMach, 2)

   End Sub


   Private Sub chkArticleNunbers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkArticleNunbers.CheckedChanged

      If chkArticleNunbers.Checked Then
         mfblnListArticle(gintSelectedMach, 1)
      End If

   End Sub


   Private Sub chkOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOrders.CheckedChanged

      If chkOrders.Checked Then
         mfblnListArticle(gintSelectedMach, 2)
      End If

   End Sub


   Public Function mfblnListArticle(ByVal intSelMach As Integer, ByVal intKindList As Integer) As Boolean

      Dim lstItem As ListViewItem
      Dim strColumnNames(3) As String
      Dim strRecords(3) As String
      Dim intX As Integer

      If intKindList = 1 Then
         strColumnNames(0) = "Article number"
         strColumnNames(1) = "Article description"
         strColumnNames(2) = "Article information"
         strColumnNames(3) = "Article normspeed"
      ElseIf intKindList = 2 Then
         strColumnNames(0) = "Order number"
         strColumnNames(1) = "Order description"
         strColumnNames(2) = "Order information"
         strColumnNames(3) = "Order normspeed"
      End If
      lstArticle.Columns.Clear()
      lstArticle.Columns.Add(strColumnNames(0))
      lstArticle.Columns.Add(strColumnNames(1))
      lstArticle.Columns.Add(strColumnNames(2))
      lstArticle.Columns.Add(strColumnNames(3))
      lstArticle.Columns(0).Width = 150
      lstArticle.Columns(1).Width = 200
      lstArticle.Columns(2).Width = 490
      lstArticle.Columns(3).Width = 100

      lstArticle.Items.Clear()

      If intKindList = 1 Then
         For intX = 0 To garrMachine(0).ArticleInfo.intArticleCount - 1
            strRecords(0) = garrMachine(0).ArticleInfo.strArticleNr(intX)
            strRecords(1) = garrMachine(0).ArticleInfo.strArticleDescription(intX)
            strRecords(2) = garrMachine(0).ArticleInfo.strArticleInformation(intX)
            strRecords(3) = garrMachine(0).ArticleInfo.intArticleNormSpeed(intX)
            lstItem = New ListViewItem(strRecords)
            lstArticle.Items.Add(lstItem)
         Next
      ElseIf intKindList = 2 Then
         For intX = 0 To garrMachine(0).OrderInfo.intOrderCount - 1
            strRecords(0) = garrMachine(0).OrderInfo.strOrderNr(intX)
            strRecords(1) = garrMachine(0).OrderInfo.strOrderDescription(intX)
            strRecords(2) = garrMachine(0).OrderInfo.strOrderInformation(intX)
            strRecords(3) = garrMachine(0).OrderInfo.intOrderNormSpeed(intX)
            lstItem = New ListViewItem(strRecords)
            lstArticle.Items.Add(lstItem)
         Next
      End If

   End Function


   Private Sub lstArticle_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstArticle.MouseDoubleClick

      Dim lstItem As ListViewItem = lstArticle.HitTest(e.Location).Item
      Dim intListKind As Integer
      Dim intAverageSpeed As Integer


      'if current defined tablekeyid is the same as last tablekeyid then update current info
      If gintUndefinedTableKeyID = gflngGetLastTableKeyId(gintSelectedMach) Then
         If chkArticleNunbers.Checked Then
            intListKind = 1
            garrMachine(gintSelectedMach).strCurrArticleNr = lstItem.SubItems(0).Text
            garrMachine(gintSelectedMach).strCurrArticleDescription = lstItem.SubItems(1).Text
            garrMachine(gintSelectedMach).intCurrArticleNormSpeed = lstItem.SubItems(3).Text
            garrMachine(gintSelectedMach).strCurrOrderNr = ""
            garrMachine(gintSelectedMach).strCurrOrderDescription = ""
            garrMachine(gintSelectedMach).intCurrOrderNormSpeed = 0
         Else
            intListKind = 2
            garrMachine(gintSelectedMach).strCurrOrderNr = lstItem.SubItems(0).Text
            garrMachine(gintSelectedMach).strCurrOrderDescription = lstItem.SubItems(1).Text
            garrMachine(gintSelectedMach).intCurrOrderNormSpeed = lstItem.SubItems(3).Text
            garrMachine(gintSelectedMach).strCurrArticleNr = ""
            garrMachine(gintSelectedMach).strCurrArticleDescription = ""
            garrMachine(gintSelectedMach).intCurrArticleNormSpeed = 0
         End If
      End If

      'check for last state
      If gfintLastStatus(gintSelectedMach, gintUndefinedTableKeyID) <> 0 Then
         garrMachine(gintSelectedMach).intCurrStatus = 2
      End If

      'recalculate average performance
      intAverageSpeed = gfintGetRecPerformance(gintSelectedMach, gintUndefinedTableKeyID, garrMachine(gintSelectedMach).intCurrOrderNormSpeed)

      'write activity info to regtable
      Call gfblnDefineProduction(gintSelectedMach, _
                                 2, _
                                 intListKind, _
                                 lstItem.SubItems(0).Text, _
                                 lstItem.SubItems(1).Text, _
                                 intAverageSpeed, _
                                 gintUndefinedTableKeyID)

      'handle where to forward the view next
      Call gfblnGetUndefined(gintSelectedMach)
      Select Case garrMachine(gintSelectedMach).intPendingProd
         Case 0
            'no pending undefined prod
            Select Case garrMachine(gintSelectedMach).intPendingStand
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
                  frmUndefinedActivities.gfblnShowUndefined(gintSelectedMach, 2)
            End Select
         Case Is >= 1
            'stll pending undefined prod
            gsChangePanel(frmOEEMain.pnlDetails, frmUndefinedActivities)
            frmUndefinedActivities.gfblnShowUndefined(gintSelectedMach, 1)
      End Select

   End Sub

End Class