Imports System.Net
Imports System.Data
Imports System.Data.SqlServerCe
Imports Modbus.Device
Imports System.Net.Sockets
Imports System.IO.Ports

Module mdlOEECollector

   Public gblnReStarted(4) As Boolean
   Public garrMachine(4) As MachineInfo
   Public garrOEE(4) As OEE
   Public garrTeamShift As TeamShift
   Public gintMachineCount As Integer
   Public gintSelectedMach As Integer
   Public gintStartIndexSelected As Integer
   Public gintUndefinedTableKeyID As Integer
   Public gintCurrPosProdBoundary(4) As Integer
   Public gintCurrPosShortBreakBoundary(4) As Integer
   Public gintCurrPosStopCodeBoundary(4) As Integer
   Public gblnEmulateProduction(4) As Boolean
   Public gintOEESkip(4) As Integer
   Public gintUpdatedPerformanceTrigger(4) As Integer
   Public gintStatusBeforeUpdate(4) As Integer

   Public Structure MachineInfo

      'Location information
      Dim intCountryNr As Integer
      Dim strCountry As String
      Dim intPlantNr As Integer
      Dim strPlant As String
      Dim intSubPlantNr As Integer
      Dim strSubPlant As String
      Dim intDepartmentNr As Integer
      Dim strDepartment As String

      'Machine information
      Dim intMachineNr As Integer
      Dim intMachineCode As Integer
      Dim strMachine As String

      'Counter information
      Dim lngCounter As Long
      Dim lngLastCounter As Long
      Dim lngAvgSpeed As Long
      Dim lngAvgCalcSpeed As Long
      Dim intAvgSpeedCounter As Integer
      Dim datLastCounter As Date
      Dim datNewCounter As Date

      'Module information
      Dim intModuleTypeNr As Integer
      Dim strModuleType As String
      Dim strCommIPAddress As String
      Dim intCommIPPort As Integer 'empty
      Dim strSensorAddress As String
      Dim strResetCounter As String
      Dim intSensorType As Integer
      Dim strCommComport As String
      Dim strCommBitsPerSecond As String
      Dim strCommDataBits As String
      Dim strCommParity As String
      Dim strCommStopBits As String
      Dim strCommFlowControl As String

      'Counter/State information
      Dim intMachUnitNr As Integer
      Dim strMachUnit As String
      Dim lngShortBreakTimer As Long
      Dim lngStopCodeTimer As Long
      Dim lngProductionTimer As Long
      Dim intNormSpeed As Integer
      Dim intAverageSpeed As Integer 'average speed of current production (article/order)
      Dim decDevider As Decimal
      Dim blnErrMessDisplayed As Boolean

      'Activity information
      Dim ActiGroup As ActivityGroup()
      Dim intActiGroupCount As Integer
      Dim intCurrActiGroupNr As Integer
      Dim intCurrActivityNr As Integer
      Dim intCurrOEE1 As Integer
      Dim intCurrOEE2 As Integer
      Dim intCurrOEE3 As Integer
      Dim intTargetOEE1 As Integer
      Dim intTargetOEE2 As Integer
      Dim intPendingProd As Integer
      Dim intPendingStand As Integer

      'Status information
      Dim intCurrStatus As Integer   'MachineState = 1 for Undefined Production, 2 for Defined Production 3 for Shortbreak, 4 for Undefined Standstill, 5 for Defined Standstill
      Dim intStatusPrev As Integer
      Dim intStatusNr() As String
      Dim strStatus() As String

      'Standard activity information
      Dim intShortbreakNr As Integer
      Dim strShortbreak As String
      Dim intUndefinedStopNr As Integer
      Dim strUndefinedStop As String
      Dim intUndefinedProdNr As Integer
      Dim strUndefinedProd As String
      Dim intUnscheduledNr As Integer
      Dim strUnscheduled As String
      Dim intUndefActAndShortBrGrpNr As Integer
      Dim strUndefActAndShortBrGrp As String
      Dim intIOFailureNr As Integer
      Dim strIOFailure As String

      'Production information
      Dim OrderInfo As OrderInformation
      Dim ArticleInfo As ArticleInformation
      Dim datCurrProdStartTime As DateTime
      Dim strCurrArticleNr As String 'sqlfieldnr
      Dim strCurrArticleDescription As String 'sqlfieldnr
      Dim intCurrArticleNormSpeed As Integer
      Dim strCurrOrderNr As String 'sqlfieldnr
      Dim strCurrOrderDescription As String 'sqlfieldnr
      Dim intCurrOrderNormSpeed As Integer

   End Structure


   Public Structure ActivityGroup

      Dim intActiNr() As Integer
      Dim strActi() As String
      Dim intActiGroupNr() As Integer
      Dim strActiGroup() As String
      Dim intActiCount As Integer
      Dim intColorNr() As Integer
      Dim intActiGroupCalcForOEE() As Integer

   End Structure


   Public Structure OrderInformation

      Dim strOrderNr() As String
      Dim strOrderDescription() As String
      Dim strOrderInformation() As String
      Dim intOrderNormSpeed() As Integer
      Dim intOrderCount As Integer

   End Structure


   Public Structure ArticleInformation

      Dim strArticleNr() As String
      Dim strArticleDescription() As String
      Dim strArticleInformation() As String
      Dim intArticleNormSpeed() As Integer
      Dim intArticleCount As Integer

   End Structure


   Public Structure TeamShift

      Dim datStartShift As DateTime
      Dim datEndShift As DateTime
      Dim strTeam As String
      Dim intTeamNr As Integer
      Dim intTeamColor As Integer
      Dim intShiftNr As Integer
      Dim strShift As String

   End Structure


   Public Structure OEE

      Dim intAvailability As Integer
      Dim intPerformance As Integer
      Dim intQuality As Integer
      Dim intOEE As Integer

   End Structure


   Public Sub gsGetMachineInfo()

      Dim intX As Integer
      Dim strSqlQuery As String
      Dim rstMachine As ADODB.Recordset

      'testcode
      If My.Computer.Name = "TWISTERTABLET" Then
         gstrWorkstation = "PW166-A12NL"
      Else
         gstrWorkstation = My.Computer.Name
      End If

      strSqlQuery = "SELECT  tblOee_Country.fldOeeCountryDescription, " & _
                    "                tblOee_Plant.fldOeePlantDescription, " & _
                    "                tblOee_SubPlant.fldOeeSubPlantDescription, " & _
                    "                tblOee_Department.fldOeeDepartmentDescription, " & _
                    "                tblOee_Machine.fldOeeMachineTableKeyID, " & _
                    "                tblOee_Machine.fldOeePlantID, " & _
                    "                tblOee_Machine.fldOeeSubPlantID, " & _
                    "                tblOee_Machine.fldOeeCountryID, " & _
                    "                tblOee_Machine.fldOeeDepartmentID, " & _
                    "                tblOee_Machine.fldOeeMachineSortOrder, " & _
                    "                tblOee_Machine.fldOeeMachineNr, " & _
                    "                tblOee_Machine.fldOeeMachineCode, " & _
                    "                tblOee_Machine.fldOeeMachineDescription, " & _
                    "                tblOee_Machine.fldOeeMachineWorkstationDescription, " & _
                    "                tblOee_Machine.fldOeeMachineProductionShortbreakTimer, " & _
                    "                tblOee_Machine.fldOeeMachineStopCodeTimer, " & _
                    "                tblOee_Machine.fldOeeMachineProductionBoundaryTimer, " & _
                    "                tblOee_Machine.fldOeeMachineSpeed, " & _
                    "                tblOee_Machine.fldOeeMachineDevider, " & _
                    "                tblOee_Machine.fldOeeMachineUnitID, " & _
                    "                tblOee_Machine.fldOeeMachineShortbreakID, " & _
                    "                tblOee_Machine.fldOeeMachineUndefinedStandstillID, " & _
                    "                tblOee_Machine.fldOeeMachineUndefinedProdID, " & _
                    "                tblOee_Machine.fldOeeMachineUnscheduledID, " & _
                    "                tblOee_Machine.fldOeeMachineIOFailureID, " & _
                    "                tblOee_Machine.fldOeeMachineTarget1OEE, " & _
                    "                tblOee_Machine.fldOeeMachineTarget2OEE, " & _
                    "                tblOee_Module.fldOeeModuleNr, " & _
                    "                tblOee_Module.fldOeeModuleIpAddress, " & _
                    "                tblOee_Module.fldOeeModuleIpAddressPort, " & _
                    "                tblOee_Module.fldOeeModuleSensorAddress, " & _
                    "                tblOee_Module.fldOeeModuleSensorResetAddress, " & _
                    "                tblOee_Module.fldOeeModuleSensorStyleID, " & _
                    "                tblOee_Module.fldOeeModuleComPort, " & _
                    "                tblOee_Module.fldOeeModuleBitsPerSecond, " & _
                    "                tblOee_Module.fldOeeModuleDataBits, " & _
                    "                tblOee_Module.fldOeeModuleParity, " & _
                    "                tblOee_Module.fldOeeModuleStopBits, " & _
                    "                tblOee_Module.fldOeeModuleFlowControl, " & _
                    "                tblOee_Module.fldOeeModuleDescription, " & _
                    "                tblOee_Module.fldOeeModuleInformation, " & _
                    "                tblOee_Module.fldOeeModuleHistory, " & _
                    "                tblOee_ModuleType.fldOeeModuleTypeNr, " & _
                    "                tblOee_ModuleType.fldOeeModuleTypeDescription, " & _
                    "                tblOee_ModuleType.fldOeeModuleTypeConnection, " & _
                    "                tblOee_MachineUndefinedProduction.fldOeeMachineUndefinedProductionDescription, " & _
                    "                tblOee_MachineUndefinedStandstill.fldOeeMachineUndefinedStandstillDescription, " & _
                    "                tblOee_MachineShortbreak.fldOeeMachineShortBreakDescription, " & _
                    "                tblOee_MachineUnscheduled.fldOeeMachineUnscheduledDescription, " & _
                    "                tblOee_MachineIOFailure.fldOeeMachineIOFailureDescription, " & _
                    "                tblOee_MachineUnit.fldOeeMachineUnitDescription " & _
                    "FROM            tblOee_Machine " & _
                    "INNER JOIN      tblOee_Country " & _
                    "ON              tblOee_Country.fldOeeCountryNr = tblOee_Machine.fldOeeCountryID " & _
                    "INNER JOIN      tblOee_Plant " & _
                    "ON              tblOee_Plant.fldOeePlantNr = tblOee_Machine.fldOeePlantID " & _
                    "INNER JOIN      tblOee_SubPlant " & _
                    "ON              tblOee_SubPlant.fldOeeSubPlantNr = tblOee_Machine.fldOeeSubPlantID " & _
                    "INNER JOIN      tblOee_Department " & _
                    "ON              tblOee_Machine.fldOeeDepartmentID = tblOee_Department.fldOeeDepartmentNr " & _
                    "INNER JOIN      tblOee_Module " & _
                    "ON              tblOee_Module.fldOeeModuleNr = tblOee_Machine.fldOeeModuleID " & _
                    "INNER JOIN      tblOee_ModuleType " & _
                    "ON              tblOee_Module.fldOeeModuleTypeID = tblOee_ModuleType.fldOeeModuleTypeNr " & _
                    "INNER JOIN      tblOee_MachineUndefinedProduction " & _
                    "ON              tblOee_Machine.fldOeeMachineUndefinedProdID = tblOee_MachineUndefinedProduction.fldOeeMachineUndefinedProductionNr " & _
                    "INNER JOIN      tblOee_MachineUndefinedStandstill " & _
                    "ON              tblOee_Machine.fldOeeMachineUndefinedStandstillID = tblOee_MachineUndefinedStandstill.fldOeeMachineUndefinedStandstillNr " & _
                    "INNER JOIN      tblOee_MachineShortbreak " & _
                    "ON              tblOee_Machine.fldOeeMachineShortbreakID = tblOee_MachineShortbreak.fldOeeMachineShortBreakNr " & _
                    "INNER JOIN      tblOee_MachineUnscheduled " & _
                    "ON              tblOee_Machine.fldOeeMachineUnscheduledID = tblOee_MachineUnscheduled.fldOeeMachineUnscheduledNr " & _
                    "INNER JOIN      tblOee_MachineIOFailure " & _
                    "ON              tblOee_Machine.fldOeeMachineIOFailureID = tblOee_MachineIOFailure.fldOeeMachineIOFailureNr " & _
                    "INNER JOIN      tblOee_MachineUnit " & _
                    "ON              tblOee_Machine.fldOeeMachineUnitID = tblOee_MachineUnit.fldOeeMachineUnitNr " & _
                    "WHERE          (tblOee_Machine.fldOeeMachineWorkstationDescription = '" & gstrWorkstation & "') " & _
                    "ORDER BY        tblOee_Machine.fldOeeMachineSortOrder;"

      rstMachine = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      gintMachineCount = rstMachine.RecordCount
      If Not rstMachine.EOF Then
         rstMachine.MoveFirst()
         For intX = 0 To rstMachine.RecordCount - 1
            garrMachine(intX).intCountryNr = rstMachine.Fields("fldOeeCountryID").Value
            garrMachine(intX).strCountry = rstMachine.Fields("fldOeeCountryDescription").Value
            garrMachine(intX).intPlantNr = rstMachine.Fields("fldOeePlantID").Value
            garrMachine(intX).strPlant = rstMachine.Fields("fldOeePlantDescription").Value
            garrMachine(intX).intSubPlantNr = rstMachine.Fields("fldOeeSubPlantID").Value
            garrMachine(intX).strSubPlant = rstMachine.Fields("fldOeeSubPlantDescription").Value
            garrMachine(intX).intDepartmentNr = rstMachine.Fields("fldOeeDepartmentID").Value
            garrMachine(intX).strDepartment = rstMachine.Fields("fldOeeDepartmentDescription").Value

            garrMachine(intX).intMachineNr = rstMachine.Fields("fldOeeMachineNr").Value
            garrMachine(intX).intMachineCode = rstMachine.Fields("fldOeeMachineCode").Value
            garrMachine(intX).strMachine = rstMachine.Fields("fldOeeMachineDescription").Value

            'garrMachine(intX).lngLastCounter = gfintIPModuleCounter(garrMachine(intX).strCommIPAddress, garrMachine(intX).strSensorAddress)
            'testcode
            'garrMachine(intX).lngLastCounter = 0
            garrMachine(intX).datLastCounter = gfToDate(Date.Now)

            garrMachine(intX).intModuleTypeNr = rstMachine.Fields("fldOeeModuleTypeNr").Value
            garrMachine(intX).strModuleType = rstMachine.Fields("fldOeeModuleTypeDescription").Value
            garrMachine(intX).intSensorType = rstMachine.Fields("fldOeeModuleSensorStyleID").Value
            garrMachine(intX).strCommIPAddress = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleIpAddress").Value), "", rstMachine.Fields("fldOeeModuleIpAddress").Value)
            garrMachine(intX).intCommIPPort = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleIpAddressPort").Value), 0, rstMachine.Fields("fldOeeModuleIpAddressPort").Value)
            garrMachine(intX).strSensorAddress = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleSensorAddress").Value), "", rstMachine.Fields("fldOeeModuleSensorAddress").Value)
            garrMachine(intX).strResetCounter = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleSensorResetAddress").Value), "", rstMachine.Fields("fldOeeModuleSensorResetAddress").Value)
            garrMachine(intX).strCommComport = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleComPort").Value), "", rstMachine.Fields("fldOeeModuleComPort").Value)
            garrMachine(intX).strCommBitsPerSecond = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleBitsPerSecond").Value), "", rstMachine.Fields("fldOeeModuleBitsPerSecond").Value)
            garrMachine(intX).strCommDataBits = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleDataBits").Value), "", rstMachine.Fields("fldOeeModuleDataBits").Value)
            garrMachine(intX).strCommParity = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleParity").Value), "", rstMachine.Fields("fldOeeModuleParity").Value)
            garrMachine(intX).strCommStopBits = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleStopBits").Value), "", rstMachine.Fields("fldOeeModuleStopBits").Value)
            garrMachine(intX).strCommFlowControl = IIf(IsDBNull(rstMachine.Fields("fldOeeModuleFlowControl").Value), "", rstMachine.Fields("fldOeeModuleFlowControl").Value)

            garrMachine(intX).lngShortBreakTimer = rstMachine.Fields("fldOeeMachineProductionShortbreakTimer").Value
            garrMachine(intX).lngStopCodeTimer = rstMachine.Fields("fldOeeMachineStopCodeTimer").Value
            garrMachine(intX).lngProductionTimer = rstMachine.Fields("fldOeeMachineProductionBoundaryTimer").Value
            garrMachine(intX).intNormSpeed = rstMachine.Fields("fldOeeMachineSpeed").Value
            garrMachine(intX).intMachUnitNr = rstMachine.Fields("fldOeeMachineUnitID").Value
            garrMachine(intX).strMachUnit = rstMachine.Fields("fldOeeMachineUnitDescription").Value
            garrMachine(intX).decDevider = rstMachine.Fields("fldOeeMachineDevider").Value
            garrMachine(intX).intTargetOEE1 = IIf(IsDBNull(rstMachine.Fields("fldOeeMachineTarget1OEE").Value), 50, rstMachine.Fields("fldOeeMachineTarget1OEE").Value)
            garrMachine(intX).intTargetOEE1 = IIf(IsDBNull(rstMachine.Fields("fldOeeMachineTarget2OEE").Value), 75, rstMachine.Fields("fldOeeMachineTarget2OEE").Value)

            garrMachine(intX).intShortbreakNr = rstMachine.Fields("fldOeeMachineShortbreakID").Value
            garrMachine(intX).strShortbreak = rstMachine.Fields("fldOeeMachineShortBreakDescription").Value
            garrMachine(intX).intUndefinedStopNr = rstMachine.Fields("fldOeeMachineUndefinedStandstillID").Value
            garrMachine(intX).strUndefinedStop = rstMachine.Fields("fldOeeMachineUndefinedStandstillDescription").Value
            garrMachine(intX).intUndefinedProdNr = rstMachine.Fields("fldOeeMachineUndefinedProdID").Value
            garrMachine(intX).strUndefinedProd = rstMachine.Fields("fldOeeMachineUndefinedProductionDescription").Value
            garrMachine(intX).intUnscheduledNr = rstMachine.Fields("fldOeeMachineUnscheduledID").Value
            garrMachine(intX).strUnscheduled = rstMachine.Fields("fldOeeMachineUnscheduledDescription").Value
            garrMachine(intX).intIOFailureNr = rstMachine.Fields("fldOeeMachineIOFailureID").Value
            garrMachine(intX).strIOFailure = rstMachine.Fields("fldOeeMachineIOFailureDescription").Value

            rstMachine.MoveNext()
         Next
      Else
         garrMessageSystem(gintSelectedMach).strTitle = "No Machines"
         garrMessageSystem(gintSelectedMach).strMessage = "No Machines allocated to this workstation!" & vbCrLf & "Please check OEE Admin for more information."
         garrMessageSystem(gintSelectedMach).intAction = 0
         frmMessageBox.ShowDialog()
         frmMessageBox.lblTitle.Text = garrMessageSystem(gintSelectedMach).strTitle
         frmMessageBox.lblMessage.Text = garrMessageSystem(gintSelectedMach).strMessage
         frmMessageBox.Refresh()
         Environment.Exit(0)
      End If

   End Sub


   Public Function gfblnGetMachActivities() As Boolean

      Dim intX As Integer
      Dim intY As Integer
      Dim intZ As Integer
      Dim strSqlQuery As String
      Dim rstOEEActiGroup As ADODB.Recordset
      Dim rstOEEActivities As ADODB.Recordset

      For intX = 0 To gintMachineCount - 1
         'all activitygroup per machine
         strSqlQuery = "SELECT DISTINCT tblOee_ActivityGroup.fldOeeActivityGroupNr, " & _
                       "                tblOee_ActivityGroup.fldOeeActivityGroupDescription, " & _
                       "                tblOee_Machine.fldOeeCountryID, " & _
                       "                tblOee_Machine.fldOeePlantID, " & _
                       "                tblOee_Machine.fldOeeSubPlantID, " & _
                       "                tblOee_Machine.fldOeeDepartmentID " & _
                       "FROM            tblOee_Activity " & _
                       "INNER JOIN      tblOee_MachineActivity " & _
                       "ON              tblOee_Activity.fldOeeActivityNr = tblOee_MachineActivity.fldOeeMachineActivityID " & _
                       "INNER JOIN      tblOee_Machine " & _
                       "ON              tblOee_MachineActivity.fldOeeMachineID = tblOee_Machine.fldOeeMachineNr " & _
                       "INNER JOIN      tblOee_ActivityGroup " & _
                       "ON              tblOee_Activity.fldOeeActivityGroupID = tblOee_ActivityGroup.fldOeeActivityGroupNr " & _
                       "WHERE          (tblOee_MachineActivity.fldOeeMachineID = " & garrMachine(intX).intMachineNr & ") " & _
                       "AND            (tblOee_Machine.fldOeeCountryID = " & garrMachine(intX).intCountryNr & ") " & _
                       "AND            (tblOee_Machine.fldOeePlantID = " & garrMachine(intX).intPlantNr & ") " & _
                       "AND            (tblOee_Machine.fldOeeSubPlantID = " & garrMachine(intX).intSubPlantNr & ") " & _
                       "AND            (tblOee_Machine.fldOeeDepartmentID = " & garrMachine(intX).intDepartmentNr & ");"

         rstOEEActiGroup = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))
         garrMachine(intX).intActiGroupCount = rstOEEActiGroup.RecordCount

         If rstOEEActiGroup.EOF Then
            Exit Function
         End If

         ReDim garrMachine(intX).ActiGroup(rstOEEActiGroup.RecordCount - 1)

         rstOEEActiGroup.MoveFirst()
         For intY = 0 To rstOEEActiGroup.RecordCount - 1
            'get activity count per machine and per category
            strSqlQuery = "SELECT        tblOee_Activity.fldOeeActivityNr, " & _
                          "              tblOee_Activity.fldOeeActivityDescription, " & _
                          "              tblOee_ActivityGroup.fldOeeActivityGroupNr, " & _
                          "              tblOee_ActivityGroup.fldOeeActivityGroupDescription, " & _
                          "              tblOee_ActivityGroup.fldOeeActivityGroupColorNr, " & _
                          "              tblOee_activityGroup.fldOeeActivityGroupCalcForOee, " & _
                          "              tblOee_Machine.fldOeeCountryID, " & _
                          "              tblOee_Machine.fldOeePlantID, " & _
                          "              tblOee_Machine.fldOeeSubPlantID, " & _
                          "              tblOee_Machine.fldOeeDepartmentID " & _
                          "FROM          tblOee_Activity " & _
                          "INNER JOIN    tblOee_MachineActivity " & _
                          "ON            tblOee_Activity.fldOeeActivityNr = tblOee_MachineActivity.fldOeeMachineActivityID " & _
                          "INNER JOIN    tblOee_Machine " & _
                          "ON            tblOee_MachineActivity.fldOeeMachineID = tblOee_Machine.fldOeeMachineNr " & _
                          "INNER JOIN    tblOee_ActivityGroup " & _
                          "ON            tblOee_Activity.fldOeeActivityGroupID = tblOee_ActivityGroup.fldOeeActivityGroupNr " & _
                          "WHERE        (tblOee_MachineActivity.fldOeeMachineID = " & garrMachine(intX).intMachineNr & ") " & _
                          "AND          (tblOee_ActivityGroup.fldOeeActivityGroupNr = " & rstOEEActiGroup.Fields("fldOeeActivityGroupNr").Value & ") " & _
                          "AND          (tblOee_Machine.fldOeeCountryID = " & garrMachine(intX).intCountryNr & ") " & _
                          "AND          (tblOee_Machine.fldOeePlantID = " & garrMachine(intX).intPlantNr & ") " & _
                          "AND          (tblOee_Machine.fldOeeSubPlantID = " & garrMachine(intX).intSubPlantNr & ") " & _
                          "AND          (tblOee_Machine.fldOeeDepartmentID = " & garrMachine(intX).intDepartmentNr & ") " & _
                          "ORDER BY      tblOee_Activity.fldOeeActivityNr;"

            rstOEEActivities = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

            garrMachine(intX).ActiGroup(intY).intActiCount = rstOEEActivities.RecordCount

            ReDim garrMachine(intX).ActiGroup(intY).intActiGroupNr(rstOEEActivities.RecordCount - 1)
            ReDim garrMachine(intX).ActiGroup(intY).strActiGroup(rstOEEActivities.RecordCount - 1)
            ReDim garrMachine(intX).ActiGroup(intY).intActiGroupCalcForOEE(rstOEEActivities.RecordCount - 1)
            ReDim garrMachine(intX).ActiGroup(intY).intActiNr(rstOEEActivities.RecordCount - 1)
            ReDim garrMachine(intX).ActiGroup(intY).strActi(rstOEEActivities.RecordCount - 1)
            ReDim garrMachine(intX).ActiGroup(intY).intColorNr(rstOEEActivities.RecordCount - 1)

            'fill array per activitygroup
            rstOEEActivities.MoveFirst()
            For intZ = 0 To rstOEEActivities.RecordCount - 1
               garrMachine(intX).ActiGroup(intY).intActiGroupNr(intZ) = rstOEEActivities.Fields("fldOeeActivityGroupNr").Value
               garrMachine(intX).ActiGroup(intY).strActiGroup(intZ) = rstOEEActivities.Fields("fldOeeActivityGroupDescription").Value
               garrMachine(intX).ActiGroup(intY).intActiGroupCalcForOEE(intZ) = rstOEEActivities.Fields("fldOeeActivityGroupCalcForOee").Value
               garrMachine(intX).ActiGroup(intY).intActiNr(intZ) = rstOEEActivities.Fields("fldOeeActivityNr").Value
               garrMachine(intX).ActiGroup(intY).strActi(intZ) = rstOEEActivities.Fields("fldOeeActivityDescription").Value
               garrMachine(intX).ActiGroup(intY).intColorNr(intZ) = rstOEEActivities.Fields("fldOeeActivityGroupColorNr").Value
               rstOEEActivities.MoveNext()
            Next
            rstOEEActiGroup.MoveNext()
         Next
      Next

   End Function


   Public Function gfblnGetOrderInformation() As Boolean

      Dim intX As Integer
      Dim intY As Integer
      Dim strSqlQuery As String
      Dim rstOrderInfo As ADODB.Recordset

      For intX = 0 To gintMachineCount - 1
         strSqlQuery = "SELECT      tblOee_Order.fldOeeOrderTableKeyID, " & _
                       "            tblOee_Order.fldOeeCountryID, " & _
                       "            tblOee_Order.fldOeePlantID, " & _
                       "            tblOee_Order.fldOeeSubPlantID, " & _
                       "            tblOee_Order.fldOeeDepartmentID, " & _
                       "            tblOee_Order.fldOeeOrderNr, " & _
                       "            tblOee_Order.fldOeeOrderDescription, " & _
                       "            tblOee_Order.fldOeeOrderInformation, " & _
                       "            tblOee_Order.fldOeeOrderHistory, " & _
                       "            tblOee_Order.fldOeeDateModified, " & _
                       "            tblOee_Order.fldOeeArticleID, " & _
                       "            tblOee_Article.fldOeeArticleNormSpeed " & _
                       "FROM        tblOee_Order " & _
                       "INNER JOIN  tblOee_Article " & _
                       "ON          tblOee_Order.fldOeeArticleID = tblOee_Article.fldOeeArticleNr " & _
                       "WHERE      (tblOee_Order.fldOeeCountryID = " & garrMachine(intX).intCountryNr & ") " & _
                       "AND        (tblOee_Order.fldOeePlantID = " & garrMachine(intX).intPlantNr & ") " & _
                       "AND        (tblOee_Order.fldOeeSubPlantID = " & garrMachine(intX).intSubPlantNr & ") " & _
                       "AND        (tblOee_Order.fldOeeDepartmentID = " & garrMachine(intX).intDepartmentNr & ") " & _
                       "AND        (tblOee_Order.fldOeeOrderHistory = 'F') " & _
                       "ORDER BY    tblOee_Order.fldOeeOrderNr"

         rstOrderInfo = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         garrMachine(intX).OrderInfo.intOrderCount = rstOrderInfo.RecordCount

         ReDim garrMachine(intX).OrderInfo.strOrderNr(rstOrderInfo.RecordCount - 1)
         ReDim garrMachine(intX).OrderInfo.strOrderDescription(rstOrderInfo.RecordCount - 1)
         ReDim garrMachine(intX).OrderInfo.strOrderInformation(rstOrderInfo.RecordCount - 1)
         ReDim garrMachine(intX).OrderInfo.intOrderNormSpeed(rstOrderInfo.RecordCount - 1)

         rstOrderInfo.MoveFirst()
         If Not rstOrderInfo.EOF Then
            For intY = 0 To rstOrderInfo.RecordCount - 1
               garrMachine(intX).OrderInfo.strOrderNr(intY) = rstOrderInfo.Fields("fldOeeOrderNr").Value
               garrMachine(intX).OrderInfo.strOrderDescription(intY) = rstOrderInfo.Fields("fldOeeOrderDescription").Value
               garrMachine(intX).OrderInfo.strOrderInformation(intY) = rstOrderInfo.Fields("fldOeeOrderInformation").Value
               garrMachine(intX).OrderInfo.intOrderNormSpeed(intY) = rstOrderInfo.Fields("fldOeeArticleNormSpeed").Value
               rstOrderInfo.MoveNext()
            Next
         Else
         End If
      Next

   End Function


   Public Function gfblnGetArticleInformation() As Boolean

      Dim intX As Integer
      Dim intY As Integer
      Dim strSqlQuery As String
      Dim rstArticleInfo As ADODB.Recordset

      strSqlQuery = "SELECT      fldOeeArticleTableKeyID, " & _
                       "            fldOeeCountryID, " & _
                       "            fldOeePlantID, " & _
                       "            fldOeeSubPlantID, " & _
                       "            fldOeeDepartmentID, " & _
                       "            fldOeeArticleNr, " & _
                       "            fldOeeArticleDescription, " & _
                       "            fldOeeArticleInformation, " & _
                       "            fldOeeArticleNormSpeed, " & _
                       "            fldOeeArticleHistory, " & _
                       "            fldOeeDateModified " & _
                       "FROM        tblOee_Article " & _
                       "WHERE      (fldOeeCountryID = " & garrMachine(intX).intCountryNr & ") " & _
                       "AND        (fldOeePlantID = " & garrMachine(intX).intPlantNr & ") " & _
                       "AND        (fldOeeSubPlantID = " & garrMachine(intX).intSubPlantNr & ") " & _
                       "AND        (fldOeeDepartmentID = " & garrMachine(intX).intDepartmentNr & ") " & _
                       "AND        (fldOeeArticleHistory = 'F') " & _
                       "ORDER BY    fldOeeArticleNr"

      rstArticleInfo = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      garrMachine(intX).ArticleInfo.intArticleCount = rstArticleInfo.RecordCount

      ReDim garrMachine(intX).ArticleInfo.strArticleNr(rstArticleInfo.RecordCount - 1)
      ReDim garrMachine(intX).ArticleInfo.strArticleDescription(rstArticleInfo.RecordCount - 1)
      ReDim garrMachine(intX).ArticleInfo.strArticleInformation(rstArticleInfo.RecordCount - 1)
      ReDim garrMachine(intX).ArticleInfo.intArticleNormSpeed(rstArticleInfo.RecordCount - 1)

      rstArticleInfo.MoveFirst()
      If Not rstArticleInfo.EOF Then
         For intY = 0 To rstArticleInfo.RecordCount - 1
            garrMachine(intX).ArticleInfo.strArticleNr(intY) = rstArticleInfo.Fields("fldOeeArticleNr").Value
            garrMachine(intX).ArticleInfo.strArticleDescription(intY) = rstArticleInfo.Fields("fldOeeArticleDescription").Value
            garrMachine(intX).ArticleInfo.strArticleInformation(intY) = rstArticleInfo.Fields("fldOeeArticleInformation").Value
            garrMachine(intX).ArticleInfo.intArticleNormSpeed(intY) = rstArticleInfo.Fields("fldOeeArticleNormSpeed").Value
            rstArticleInfo.MoveNext()
         Next
      Else
      End If

   End Function


   Public Function gfblnGetStatusDescription() As Boolean

      Dim intX As Integer
      Dim intY As Integer
      Dim strSqlQuery As String
      Dim rstStatusDescription As ADODB.Recordset

      For intX = 0 To gintMachineCount - 1
         strSqlQuery = "SELECT    fldOeeMachineStatusNr, " & _
                       "          fldOeeMachineStatusDescription, " & _
                       "          fldOeeCountryID, " & _
                       "          fldOeePlantID, " & _
                       "          fldOeeSubPlantID, " & _
                       "          fldOeeDepartmentID " & _
                       "FROM      tblOee_MachineStatus " & _
                       "WHERE    (fldOeeCountryID = '" & garrMachine(intX).intCountryNr & "') " & _
                       "AND      (fldOeePlantID = '" & garrMachine(intX).intPlantNr & "') " & _
                       "AND      (fldOeeSubPlantID = '" & garrMachine(intX).intSubPlantNr & "') " & _
                       "AND      (fldOeeDepartmentID = '" & garrMachine(intX).intDepartmentNr & "') " & _
                       "ORDER BY  fldOeeMachineStatusNr"

         rstStatusDescription = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         ReDim garrMachine(intX).strStatus(rstStatusDescription.RecordCount)

         rstStatusDescription.MoveFirst()
         For intY = 1 To rstStatusDescription.RecordCount
            garrMachine(intX).strStatus(intX) = rstStatusDescription.Fields("fldOeeMachineStatusDescription").Value
            rstStatusDescription.MoveNext()
         Next
      Next

   End Function


   Public Function gfblnGetTeamShift() As Boolean

      Dim intCount As Integer
      Dim intX As Integer
      Dim intY As Integer
      Dim strSqlQuery As String
      Dim datShiftStartTime As DateTime
      Dim datShiftEndTime As DateTime
      Dim datDailyStartTime As DateTime
      Dim datDailyEndTime As DateTime
      Dim rstDailySchedule As ADODB.Recordset

      strSqlQuery = "SELECT      tblOee_DailySchedule.fldOeeDailyScheduleTableKeyID, " & _
                       "            tblOee_ShiftTime.fldOeeCountryID, " & _
                       "            tblOee_ShiftTime.fldOeePlantID, " & _
                       "            tblOee_ShiftTime.fldOeeSubPlantID, " & _
                       "            tblOee_ShiftTime.fldOeeDepartmentID, " & _
                       "            tblOee_ShiftTime.fldOeeShiftTimeNr, " & _
                       "            tblOee_ShiftTime.fldOeeShiftTimeDescription, " & _
                       "            tblOee_ShiftTime.fldOeeShiftTimeStart, " & _
                       "            tblOee_ShiftTime.fldOeeShiftTimeEnd, " & _
                       "            tblOee_Team.fldOeeTeamNr, " & _
                       "            tblOee_Team.fldOeeTeamDescription, " & _
                       "            tblOee_Team.fldOeeTeamColorNr, " & _
                       "            tblOee_DailySchedule.fldOeeDailyScheduleStartDate, " & _
                       "            tblOee_DailySchedule.fldOeeDailyScheduleEndDate " & _
                       "FROM        tblOee_DailySchedule " & _
                       "INNER JOIN  tblOee_ShiftTime " & _
                       "ON          tblOee_DailySchedule.fldOeeShiftTimeID = tblOee_ShiftTime.fldOeeShiftTimeNr " & _
                       "INNER JOIN  tblOee_Team " & _
                       "ON          tblOee_DailySchedule.fldOeeTeamID = tblOee_Team.fldOeeTeamNr " & _
                       "AND         tblOee_ShiftTime.fldOeeCountryID = tblOee_Team.fldOeeCountryID " & _
                       "AND         tblOee_ShiftTime.fldOeePlantID = tblOee_Team.fldOeePlantID " & _
                       "AND         tblOee_ShiftTime.fldOeeSubPlantID = tblOee_Team.fldOeeSubPlantID " & _
                       "WHERE      (tblOee_ShiftTime.fldOeeCountryID = '" & garrMachine(0).intCountryNr & "') " & _
                       "AND        (tblOee_ShiftTime.fldOeePlantID = '" & garrMachine(0).intPlantNr & "') " & _
                       "AND        (tblOee_ShiftTime.fldOeeSubPlantID = '" & garrMachine(0).intSubPlantNr & "') " & _
                       "AND        (tblOee_ShiftTime.fldOeeDepartmentID = '" & garrMachine(0).intDepartmentNr & "') " & _
                       "ORDER BY    tblOee_DailySchedule.fldOeeDailyScheduleStartDate;"

      rstDailySchedule = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      intCount = rstDailySchedule.RecordCount

      Dim intResult1 As Integer
      Dim intResult2 As Integer

      rstDailySchedule.MoveFirst()
      For intX = 0 To rstDailySchedule.RecordCount - 1
         datDailyStartTime = gfToDate(rstDailySchedule.Fields("fldOeeDailyScheduleStartDate").Value)
         datDailyEndTime = gfToDate(rstDailySchedule.Fields("fldOeeDailyScheduleEndDate").Value)
         datShiftStartTime = gfToDate(rstDailySchedule.Fields("fldOeeShiftTimeStart").Value)
         datShiftEndTime = gfToDate(rstDailySchedule.Fields("fldOeeShiftTimeEnd").Value)

         datDailyStartTime = New DateTime(datDailyStartTime.Year, datDailyStartTime.Month, datDailyStartTime.Day, datShiftStartTime.Hour, _
                                           datShiftStartTime.Minute, 0)
         datDailyEndTime = New DateTime(datDailyEndTime.Year, datDailyEndTime.Month, datDailyEndTime.Day, datShiftEndTime.Hour, _
                                           datShiftEndTime.Minute, 0)

         intResult1 = DateTime.Compare(gfToDate(Date.Now), datDailyStartTime)
         Select Case intResult1
            Case Is > 0
               intResult2 = DateTime.Compare(gfToDate(Date.Now), datDailyEndTime)
               Select Case intResult2
                  Case Is < 0
                     Dim strTeamInfoCurrent As String
                     Dim strTeamInfoNew As String
                     strTeamInfoCurrent = gfstrDatToStr(garrTeamShift.datStartShift) & "', '" & _
                                          gfstrDatToStr(garrTeamShift.datEndShift) & "', '" & _
                                          garrTeamShift.intShiftNr & "', '" & _
                                          garrTeamShift.strShift & "', '" & _
                                          garrTeamShift.intTeamNr & "', '" & _
                                          garrTeamShift.strTeam
                     strTeamInfoNew = gfstrDatToStr(datDailyStartTime) & "', '" & _
                                      gfstrDatToStr(datDailyEndTime) & "', '" & _
                                      rstDailySchedule.Fields("fldOeeShiftTimeNr").Value & "', '" & _
                                      rstDailySchedule.Fields("fldOeeShiftTimeDescription").Value & "', '" & _
                                      rstDailySchedule.Fields("fldOeeTeamNr").Value & "', '" & _
                                      rstDailySchedule.Fields("fldOeeTeamDescription").Value
                     If garrTeamShift.datStartShift <> "#12:00:00 AM#" Then
                        If strTeamInfoCurrent <> strTeamInfoNew Then
                           For intY = 0 To gintMachineCount - 1
                              gfblnShiftChange(intY, strTeamInfoNew)
                           Next
                        End If
                     End If
                     garrTeamShift.datStartShift = datDailyStartTime
                     garrTeamShift.datEndShift = datDailyEndTime
                     garrTeamShift.intShiftNr = rstDailySchedule.Fields("fldOeeShiftTimeNr").Value
                     garrTeamShift.strShift = rstDailySchedule.Fields("fldOeeShiftTimeDescription").Value
                     garrTeamShift.intTeamNr = rstDailySchedule.Fields("fldOeeTeamNr").Value
                     garrTeamShift.strTeam = rstDailySchedule.Fields("fldOeeTeamDescription").Value
                     garrTeamShift.intTeamColor = rstDailySchedule.Fields("fldOeeTeamColorNr").Value
                     Exit For
               End Select
         End Select

         rstDailySchedule.MoveNext()
      Next

      If garrTeamShift.datStartShift = "#12:00:00 AM#" Then
         gfblnWriteLog("No shift information available. Please check the current shift information.", 2)
      End If

   End Function


   Public Function gfblnGetMachineStatus(ByVal intSelMach As Integer) As Boolean

      If garrMachine(intSelMach).intSensorType = 1 Then
         'sensortype counter

         If gblnEmulateProduction(intSelMach) = True Then
            'testcode (see production)
            garrMachine(intSelMach).lngCounter = garrMachine(intSelMach).lngLastCounter + 1
         Else
            'sensortype counter
            If garrMachine(intSelMach).intModuleTypeNr = 1 Then
               garrMachine(intSelMach).lngCounter = gfintIPModuleCounter(intSelMach, garrMachine(intSelMach).strCommIPAddress, garrMachine(intSelMach).strSensorAddress)
            ElseIf garrMachine(intSelMach).intModuleTypeNr = 2 Then
               garrMachine(intSelMach).lngCounter = gfintComModuleCounter(intSelMach, garrMachine(intSelMach).strCommComport, garrMachine(intSelMach).strCommBitsPerSecond, _
                                                                          garrMachine(intSelMach).strCommDataBits, garrMachine(intSelMach).strCommParity, garrMachine(intSelMach).strSensorAddress)
            End If
            'testcode (see production)
            'garrMachine(intSelMach).lngCounter = 0
         End If

         If garrMachine(intSelMach).lngCounter > garrMachine(intSelMach).lngLastCounter Then
            If garrMachine(intSelMach).intAvgSpeedCounter < 3 Then
               garrMachine(intSelMach).lngAvgCalcSpeed = garrMachine(intSelMach).lngAvgCalcSpeed + (garrMachine(intSelMach).lngCounter - garrMachine(intSelMach).lngLastCounter)
               garrMachine(intSelMach).intAvgSpeedCounter = garrMachine(intSelMach).intAvgSpeedCounter + 1
            Else
               'after 3 seconds of monitoring inputpulses multiply by 20 to get speed per minute
               garrMachine(intSelMach).lngAvgSpeed = garrMachine(intSelMach).lngAvgCalcSpeed * 20
               garrMachine(intSelMach).intAvgSpeedCounter = 0
               garrMachine(intSelMach).lngAvgCalcSpeed = 0
            End If

            'testcode (see production)
            If gblnEmulateProduction(intSelMach) = True Then
               gintCurrPosProdBoundary(intSelMach) = garrMachine(intSelMach).lngProductionTimer + 1
            End If

            gfblnMachineStatusAction(intSelMach, 1, True)
            Else
               gfblnMachineStatusAction(intSelMach, 1, False)
         End If
            garrMachine(intSelMach).lngLastCounter = garrMachine(intSelMach).lngCounter
      Else
         'sensortype status
         If garrMachine(intSelMach).intModuleTypeNr = 1 Then
            If gfblnIPModuleStatus(intSelMach, garrMachine(intSelMach).strCommIPAddress, garrMachine(intSelMach).strSensorAddress) = False Then
               gfblnMachineStatusAction(intSelMach, 2, True)
            Else
               gfblnMachineStatusAction(intSelMach, 2, False)
            End If
         ElseIf garrMachine(intSelMach).intModuleTypeNr = 2 Then
            If gfblnComModuleStatus(intSelMach, garrMachine(intSelMach).strCommComport, garrMachine(intSelMach).strCommBitsPerSecond, _
                                    garrMachine(intSelMach).strCommDataBits, garrMachine(intSelMach).strCommParity, garrMachine(intSelMach).strSensorAddress) Then
               gfblnMachineStatusAction(intSelMach, 2, True)
            Else
               gfblnMachineStatusAction(intSelMach, 2, False)
            End If

         End If
      End If

   End Function


   Public Function gfblnMachineStatusAction(ByVal intSelMach As Integer, ByVal intSensorType As Integer, ByVal blnProduction As Boolean) As Boolean

      Dim intCalcOEE As Boolean

      gfblnCalcOEE(intSelMach)
      garrMachine(intSelMach).intAverageSpeed = gfintGetRecPerformance(intSelMach, 0, 0)

      'continue status after update
      If gfblnContinueAfterUpdating(intSelMach) Then
         Select Case gintStatusBeforeUpdate(intSelMach)
            Case 1
               gintCurrPosProdBoundary(intSelMach) = garrMachine(intSelMach).lngProductionTimer
               garrMachine(intSelMach).intStatusPrev = 1
               garrMachine(intSelMach).intCurrStatus = 1
            Case 2
               gintCurrPosProdBoundary(intSelMach) = garrMachine(intSelMach).lngProductionTimer
               garrMachine(intSelMach).intStatusPrev = 2
               garrMachine(intSelMach).intCurrStatus = 2
            Case 3
               garrMachine(intSelMach).intStatusPrev = 3
               garrMachine(intSelMach).intCurrStatus = 3
            Case 4
               gintCurrPosShortBreakBoundary(intSelMach) = garrMachine(intSelMach).lngShortBreakTimer
               gintCurrPosStopCodeBoundary(intSelMach) = garrMachine(intSelMach).lngStopCodeTimer
               garrMachine(intSelMach).intStatusPrev = 4
               garrMachine(intSelMach).intCurrStatus = 4
            Case 5
               gintCurrPosShortBreakBoundary(intSelMach) = garrMachine(intSelMach).lngShortBreakTimer
               gintCurrPosStopCodeBoundary(intSelMach) = garrMachine(intSelMach).lngStopCodeTimer
               garrMachine(intSelMach).intStatusPrev = 5
               garrMachine(intSelMach).intCurrStatus = 5
         End Select
      End If

      If blnProduction Then
         If gintCurrPosProdBoundary(intSelMach) > garrMachine(intSelMach).lngProductionTimer Then
            'compensate status change from undefined > defined
            If Not garrMachine(intSelMach).intCurrStatus = 2 Then
               If garrMachine(intSelMach).intCurrStatus = 3 Then
                  garrMachine(intSelMach).intStatusPrev = 3
               End If
               garrMachine(intSelMach).intCurrStatus = 1
               gintCurrPosShortBreakBoundary(intSelMach) = 0
               gintCurrPosStopCodeBoundary(intSelMach) = 0
            End If
            If garrMachine(intSelMach).intStatusPrev = 3 Then
               'check if second last record was instatus 1 or 2 and if so then copy record info second last to new
               garrMachine(intSelMach).lngCounter = 0
               gfblnContinueProduction(intSelMach)
               garrMachine(intSelMach).intStatusPrev = garrMachine(intSelMach).intCurrStatus
               garrMachine(intSelMach).intCurrActivityNr = garrMachine(intSelMach).intUndefinedProdNr
               garrMachine(intSelMach).datCurrProdStartTime = Date.Now
            Else
               'write sql record undefined production
               If gfblnUpdateReg(intSelMach, garrMachine(intSelMach).intCurrStatus) = False Then
                  garrMachine(intSelMach).datCurrProdStartTime = Date.Now
                  'add new undefined production record
                  Call gfblnAddReg(intSelMach, 0, 0, "Production", garrMachine(intSelMach).strUndefinedProd, False, 0, 1)
                  garrMachine(intSelMach).intCurrActivityNr = garrMachine(intSelMach).intUndefinedProdNr
               End If
               garrMachine(intSelMach).intStatusPrev = garrMachine(intSelMach).intCurrStatus
            End If
         Else
            gintCurrPosProdBoundary(intSelMach) = gintCurrPosProdBoundary(intSelMach) + 1 'timer must be 1 second
         End If
      Else
         'Standstill seen
         'keep polling until i/o connection is back online
         If garrMachine(intSelMach).blnErrMessDisplayed Then
            gfblnUpdateReg(intSelMach, garrMachine(intSelMach).intCurrStatus)
            Exit Function
         End If

         If gintCurrPosShortBreakBoundary(intSelMach) > garrMachine(intSelMach).lngShortBreakTimer Then
            If gintCurrPosStopCodeBoundary(intSelMach) > garrMachine(intSelMach).lngStopCodeTimer Then
               'Stop Code
               If intSensorType = 1 Then
                  If garrMachine(intSelMach).intModuleTypeNr = 1 Then
                     'Disabled counter reset
                     gfblnResetIPModuleCounter(intSelMach, garrMachine(intSelMach).strCommIPAddress, garrMachine(intSelMach).strResetCounter)
                  ElseIf garrMachine(intSelMach).intModuleTypeNr = 4 Then
                     'Disabled counter reset
                     gfblnResetComModuleCounter(intSelMach, garrMachine(intSelMach).strCommComport, garrMachine(intSelMach).strCommBitsPerSecond,
                     garrMachine(intSelMach).strCommDataBits, garrMachine(intSelMach).strCommParity, garrMachine(intSelMach).strSensorAddress)
                  End If
                  garrMachine(intSelMach).lngCounter = 0
                  garrMachine(intSelMach).intAverageSpeed = 0
               End If
               'compensate for initial status change from shortbreak to undefined standstill
               If gfintLastStatus(intSelMach, 0) = 3 Then
                  garrMachine(intSelMach).intCurrStatus = 4
               End If
               'write sql record undefined activity to defined on machine etc based undefined activitynr
               If gfblnUpdateReg(intSelMach, garrMachine(intSelMach).intCurrStatus) = False Then
                  'add new undefined standstill record
                  If garrMachine(intSelMach).blnErrMessDisplayed Then
                     intCalcOEE = 4
                  Else
                     intCalcOEE = 1
                  End If
                  Call gfblnAddReg(intSelMach, garrMachine(intSelMach).intUndefActAndShortBrGrpNr, _
                                   garrMachine(intSelMach).intUndefinedStopNr, _
                                   "Standstill", _
                                   garrMachine(intSelMach).strUndefinedStop, False, 0, intCalcOEE)
                  garrMachine(intSelMach).intCurrActiGroupNr = garrMachine(intSelMach).intUndefActAndShortBrGrpNr
                  garrMachine(intSelMach).intCurrActivityNr = garrMachine(intSelMach).intUndefinedStopNr
               End If
            Else
               'Short Break
               garrMachine(intSelMach).intStatusPrev = garrMachine(intSelMach).intCurrStatus
               garrMachine(intSelMach).intCurrStatus = 3
               'write enddate production sql record and write new line undefined activity
               gintCurrPosProdBoundary(intSelMach) = 0
               If gfblnUpdateReg(intSelMach, garrMachine(intSelMach).intCurrStatus) = False Then
                  Call gfblnAddReg(intSelMach, garrMachine(intSelMach).intUndefActAndShortBrGrpNr, _
                                   garrMachine(intSelMach).intShortbreakNr, _
                                   "Short break", _
                                   garrMachine(intSelMach).strShortbreak, False, 0, 1)
                  garrMachine(intSelMach).intCurrActiGroupNr = garrMachine(intSelMach).intUndefActAndShortBrGrpNr
                  garrMachine(intSelMach).intCurrActivityNr = garrMachine(intSelMach).intUndefinedStopNr
               End If
               gintCurrPosStopCodeBoundary(intSelMach) = gintCurrPosStopCodeBoundary(intSelMach) + 1 'timer must be 1 second
            End If
         Else
            'still production if intstatus = 1
            gintCurrPosShortBreakBoundary(intSelMach) = gintCurrPosShortBreakBoundary(intSelMach) + 1 'timer must be 1 second
         End If
      End If

   End Function


   Public Function gfblnCalcOEE(ByVal intSelMach As Integer) As Boolean

      Dim lngTotalRegTime As Long
      Dim lngTotalProdTime As Long
      Dim strSqlQuery As String
      Dim rstOEE As ADODB.Recordset
      Dim rstOEE3 As ADODB.Recordset
      Dim dtaTable As DataTable
      Dim intPerformance As Integer
      Dim intCounter As Integer
      Dim intNormSpeed As Integer
      Dim intDuration As Decimal
      Dim intRecPerformance As Decimal
      Dim intX As Integer
      Dim intTotalDuration As Integer

      strSqlQuery = "SELECT SUM (tblOee_Reg.fldOeeActivityDuration) AS fldTotalTime " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE      (tblOee_Reg.fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND        (tblOee_Reg.fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        (tblOee_Reg.fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND        (tblOee_Reg.fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "GROUP BY    fldOeeActivityGroupCalcForOee " & _
                    "HAVING     (fldOeeActivityGroupCalcForOee = 1)"

      dtaTable = gdtaSqlCeTable(strSqlQuery)

      If dtaTable.Rows.Count > 0 Then
         rstOEE = gfconvertToADODB(dtaTable)
         If Not rstOEE.EOF Then
            rstOEE.MoveFirst()
            lngTotalRegTime = IIf(IsDBNull(rstOEE.Fields("fldTotalTime").Value), 1, rstOEE.Fields("fldTotalTime").Value)
         End If
      End If
      If lngTotalRegTime = 0 Then
         lngTotalRegTime = 1
      End If

      strSqlQuery = "SELECT SUM (tblOee_Reg.fldOeeActivityDuration) AS fldTotalTime " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE      (tblOee_Reg.fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND        (tblOee_Reg.fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        (tblOee_Reg.fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND        (tblOee_Reg.fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeMachineStatusID = '1' " & _
                    "OR          tblOee_Reg.fldOeeMachineStatusID = '2')"

      dtaTable.Clear()
      dtaTable = gdtaSqlCeTable(strSqlQuery)

      If dtaTable.Rows.Count > 0 Then
         lngTotalProdTime = IIf(IsDBNull(dtaTable.Rows(0).Item(0)), 1, (dtaTable.Rows(0).Item(0)))
      End If
      If lngTotalProdTime = 0 Then
         lngTotalProdTime = 1
      End If

      garrOEE(intSelMach).intAvailability = (lngTotalProdTime / lngTotalRegTime) * 100

      'calc performance:
      '(speed / normspeed) * timefactor of total production time 
      strSqlQuery = "SELECT      fldOeeNormSpeed, " & _
                    "            fldOeeActivityDuration, " & _
                    "            fldOeeCounter " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE      (tblOee_Reg.fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND        (tblOee_Reg.fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        (tblOee_Reg.fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND        (tblOee_Reg.fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "AND        (tblOee_Reg.fldOeeMachineStatusID = '1' " & _
                    "OR          tblOee_Reg.fldOeeMachineStatusID = '2')"

      rstOEE3 = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstOEE3.EOF Then
         rstOEE3.MoveFirst()
         For intX = 0 To rstOEE3.RecordCount - 1
            intTotalDuration = intTotalDuration + rstOEE3.Fields("fldOeeActivityDuration").Value
            rstOEE3.MoveNext()
         Next
         rstOEE3.MoveFirst()
      End If

      If Not intTotalDuration = 0 Then
         intRecPerformance = 0
         If Not rstOEE3.EOF Then
            'set OEE Performance
            For intX = 0 To rstOEE3.RecordCount - 1
               intCounter = IIf(IsDBNull(rstOEE3.Fields("fldOeeCounter").Value), 100, _
                                                        rstOEE3.Fields("fldOeeCounter").Value)
               intNormSpeed = IIf(IsDBNull(rstOEE3.Fields("fldOeeNormSpeed").Value), 100, _
                                                        rstOEE3.Fields("fldOeeNormSpeed").Value)
               intDuration = IIf(IsDBNull(rstOEE3.Fields("fldOeeActivityDuration").Value), 100, _
                                                        rstOEE3.Fields("fldOeeActivityDuration").Value)

               If intDuration = 0 Then
                  intRecPerformance = intRecPerformance + intDuration
               Else
                  If intCounter = 0 Then
                     intRecPerformance = intRecPerformance + intDuration
                  Else
                     intRecPerformance = intRecPerformance + (intCounter / (intDuration / 60) / intNormSpeed * 100 * intDuration)
                  End If
               End If

               rstOEE3.MoveNext()
            Next

            If intTotalDuration = 0 Then
               intPerformance = 100
            Else
               intPerformance = intRecPerformance / intTotalDuration
            End If
         Else
            intPerformance = 100
         End If
      Else
         intPerformance = 100
      End If


      If intPerformance = 0 Then
         If (garrOEE(intSelMach).intPerformance - intPerformance) < 10 Then
            garrOEE(intSelMach).intPerformance = intPerformance
         End If
      Else
         garrOEE(intSelMach).intPerformance = intPerformance
      End If

      If garrOEE(intSelMach).intPerformance > 100 Then
         garrOEE(intSelMach).intPerformance = 100
      End If

      garrOEE(intSelMach).intQuality = 100
      garrOEE(intSelMach).intOEE = ((garrOEE(intSelMach).intAvailability / 100) + _
                                    (garrOEE(intSelMach).intPerformance / 100) + _
                                    (garrOEE(intSelMach).intQuality / 100)) / 3 * 100

      Try
         If garrOEE(intSelMach).intOEE >= garrMachine(intSelMach).intTargetOEE2 Then
            If garrOEE(intSelMach).intOEE >= garrMachine(intSelMach).intTargetOEE1 Then
               gpicMachOEENorm(intSelMach).ImageLocation = gstrImagesLocation & "greendot.png"
            Else
               gpicMachOEENorm(intSelMach).ImageLocation = gstrImagesLocation & "yellowdot.png"
            End If
         Else
            gpicMachOEENorm(intSelMach).ImageLocation = gstrImagesLocation & "reddot.png"
         End If
      Catch ex As Exception
      End Try

   End Function


   Public Function gfintGetRecPerformance(ByVal intSelMach As Integer, ByVal lngTableKeyID As Long, ByVal intNormSpeed As Integer) As Integer

      Dim intCurrNormSpeed As Integer
      Dim intTimeSpan As TimeSpan
      Dim intSeconds As Integer
      Dim intAverageSpeed As Integer
      Dim strSqlQuery As String
      Dim rstRegTable As ADODB.Recordset
      Dim intCounter As Integer
      Dim datStartDate As DateTime
      Dim datEndDate As DateTime

      If lngTableKeyID = 0 Then
         If garrMachine(intSelMach).intCurrArticleNormSpeed = 0 Then
            intCurrNormSpeed = garrMachine(intSelMach).intCurrOrderNormSpeed
         End If
         If garrMachine(intSelMach).intCurrOrderNormSpeed = 0 Then
            intCurrNormSpeed = garrMachine(intSelMach).intCurrArticleNormSpeed
         End If
         If intCurrNormSpeed = 0 Then
            intCurrNormSpeed = garrMachine(intSelMach).intNormSpeed
            garrMachine(gintSelectedMach).intCurrArticleNormSpeed = garrMachine(intSelMach).intNormSpeed
         End If

         intCounter = garrMachine(intSelMach).lngCounter

         'when record needs to be written then write also record performance and shift performance
         Try
            intTimeSpan = Date.Now.Subtract(garrMachine(intSelMach).datCurrProdStartTime)
            intSeconds = intTimeSpan.Seconds + (intTimeSpan.Minutes * 60)
            intAverageSpeed = intCounter / intSeconds * 60
            If intAverageSpeed > 100 Then
               intAverageSpeed = 100
            End If
         Catch ex As Exception
            intAverageSpeed = 0
         End Try
         Return intAverageSpeed
      Else
         'get counter info
         strSqlQuery = "SELECT   fldOeeRegTableKeyID, " & _
                       "         fldOeeCounter, " & _
                       "         fldOeeStartDateTime, " & _
                       "         fldOeeEndDateTime " & _
                       "FROM     tblOee_Reg " & _
                       "WHERE (fldOeeRegTableKeyID = '" & lngTableKeyID & "');"

         rstRegTable = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         intCounter = IIf(IsDBNull(rstRegTable.Fields("fldOeeCounter").Value), 0, rstRegTable.Fields("fldOeeCounter").Value)
         datStartDate = rstRegTable.Fields("fldOeeStartDateTime").Value
         datEndDate = IIf(IsDBNull(rstRegTable.Fields("fldOeeEndDateTime").Value), Date.Now, rstRegTable.Fields("fldOeeEndDateTime").Value)

         Try
            intTimeSpan = datEndDate.Subtract(datStartDate)
            intSeconds = intTimeSpan.Seconds + (intTimeSpan.Minutes * 60)
            intAverageSpeed = intCounter / intSeconds * 60
            If intAverageSpeed > 100 Then
               intAverageSpeed = 100
            End If
         Catch ex As Exception
            intAverageSpeed = 0
         End Try
         Return intAverageSpeed

      End If

   End Function


   Public Function gfintLastStatus(ByVal intSelMach As Integer, ByVal lngRegTableKeyID As Long) As Integer

      Dim strSqlQuery As String
      Dim rstLastStatus As ADODB.Recordset

      strSqlQuery = "SELECT   fldOeeRegTableKeyID, " & _
                    "         fldOeeStartDateTime, " & _
                    "         fldOeeMachineStatusID " & _
                    "FROM     tblOee_Reg " & _
                    "WHERE    fldOeeMachineCode = '" & garrMachine(intSelMach).intMachineCode & "' " & _
                    "ORDER BY fldOeeStartDateTime DESC"

      rstLastStatus = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstLastStatus.EOF Then
         rstLastStatus.MoveFirst()
         Select Case rstLastStatus.Fields("fldOeeMachineStatusID").Value
            Case 1
               'Undefined Production
               gfintLastStatus = 1
            Case 2
               gfintLastStatus = 2
            Case 3
               gfintLastStatus = 3
            Case 4
               'Undefined Standstill
               gfintLastStatus = 4
            Case 5
               gfintLastStatus = 5
         End Select

         If lngRegTableKeyID <> 0 Then
            If rstLastStatus.Fields("fldOeeRegTableKeyID").Value <> lngRegTableKeyID Then
               'not the last record, so no final graphical change
               gfintLastStatus = 0
            End If
         End If
      End If

   End Function


   Public Function gflngGetLastTableKeyId(ByVal intSelMach As Integer) As Long

      Dim strSqlQuery As String
      Dim rstLastTableKey As ADODB.Recordset

      strSqlQuery = "SELECT   fldOeeRegTableKeyID " & _
                    "FROM     tblOee_Reg " & _
                    "WHERE    fldOeeMachineCode = '" & garrMachine(intSelMach).intMachineCode & "' " & _
                    "ORDER BY fldOeeStartDateTime DESC"

      rstLastTableKey = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstLastTableKey.EOF Then
         rstLastTableKey.MoveFirst()
         Return rstLastTableKey.Fields("fldOeeRegTableKeyID").Value
      End If

   End Function


   Public Function gflngGetLastRecordId(ByVal intSelMach As Integer) As Long

      Dim strSqlQuery As String
      Dim rstRegNr As ADODB.Recordset

      strSqlQuery = "SELECT   fldOeeRegNr " & _
                    "FROM     tblOee_Reg " & _
                    "WHERE    fldOeeMachineCode = '" & garrMachine(intSelMach).intMachineCode & "' " & _
                    "ORDER BY fldOeeStartDateTime DESC"

      rstRegNr = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstRegNr.EOF Then
         rstRegNr.MoveFirst()
         Return rstRegNr.Fields("fldOeeRegNr").Value
      End If

   End Function


   Public Function gfblnAddReg(ByVal intSelMach As Integer, ByVal intSelActiGroupNr As Integer, _
                               ByVal intSelActiNr As Integer, ByVal strSelActiGroup As String, ByVal strSelActi As String, _
                               ByVal blnProduction As Boolean, ByVal intProduct As Integer, ByVal intCalcForOEE As Integer) As Boolean

      Dim strSqlQuery As String
      Dim dtaTable As DataTable
      Dim rstReg As ADODB.Recordset
      Dim lngRegNr As Long
      Dim strAddReg(25) As String
      Dim datEndDate As Date
      Dim intTimeSpan As TimeSpan
      Dim intShiftMinutes As Integer
      Dim intActivityMinutes As Integer

      intTimeSpan = garrTeamShift.datEndShift.Subtract(garrTeamShift.datStartShift)
      intShiftMinutes = intTimeSpan.Minutes + (intTimeSpan.Hours * 60)

      datEndDate = Date.Now
      intActivityMinutes = 0

      strSqlQuery = "SELECT      MAX(fldOeeRegNr) AS RegNr, " & _
                    "            MAX(fldOeeRegTableKeyID) AS RegTableKeyID, " & _
                    "            fldOeeCountryID, " & _
                    "            fldOeePlantID, " & _
                    "            fldOeeSubPlantID, " & _
                    "            fldOeeDepartmentID, " & _
                    "            fldOeeMachineID " & _
                    "FROM        tblOee_Reg " & _
                    "GROUP BY    fldOeeCountryID, " & _
                    "            fldOeePlantID, " & _
                    "            fldOeeSubPlantID, " & _
                    "            fldOeeDepartmentID, " & _
                    "            fldOeeMachineID " & _
                    "HAVING     (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND        (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND        (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND        (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND        (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "');"

      dtaTable = gdtaSqlCeTable(strSqlQuery)

      If dtaTable.Rows.Count > 0 Then
         rstReg = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))
         If Not rstReg.EOF Then
            rstReg.MoveFirst()
            lngRegNr = rstReg.Fields("RegNr").Value + 1
         Else
            lngRegNr = 1
         End If
      Else
         lngRegNr = 1
      End If


      If garrOEE(intSelMach).intPerformance = 0 Then
         Dim rstReg1 As ADODB.Recordset

         strSqlQuery = "SELECT   fldOeeRegTableKeyID, " & _
                       "         fldOeeRegNr, " & _
                       "         fldOeeCurrentPerformance " & _
                       "FROM     tblOee_Reg " & _
                       "WHERE    fldOeeMachineCode = '" & garrMachine(intSelMach).intMachineCode & "' " & _
                       "AND      fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "' " & _
                       "AND      fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "' " & _
                       "AND      fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "' " & _
                       "AND      fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "' " & _
                       "AND      fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "' " & _
                       "ORDER BY fldOeeRegTableKeyID DESC;"

         rstReg1 = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         If Not rstReg1.EOF Then
            rstReg1.MoveFirst()
            garrOEE(intSelMach).intPerformance = rstReg1.Fields("fldOeeCurrentPerformance").Value
         End If


         Dim strTest As String = "ok"
      End If

      strSqlQuery = "INSERT INTO tblOee_Reg " & _
                     "           (fldOeeRegNr, " & _
                     "            fldOeeStartDateTime, " & _
                     "            fldOeeEndDateTime, " & _
                     "            fldOeeActivityDuration, " & _
                     "            fldOeeShiftStartDateTime, " & _
                     "            fldOeeShiftEndDateTime, " & _
                     "            fldOeeShiftDuration, " & _
                     "            fldOeeShiftTimeID, " & _
                     "            fldOeeShiftTimeDescription, " & _
                     "            fldOeeTeamID, " & _
                     "            fldOeeTeamDescription, " & _
                     "            fldOeeTeamColorID, " & _
                     "            fldOeeTeamColorDescription, " & _
                     "            fldOeeCountryID, " & _
                     "            fldOeeCountryDescription, " & _
                     "            fldOeePlantID, " & _
                     "            fldOeePlantDescription, " & _
                     "            fldOeeSubPlantID, " & _
                     "            fldOeeSubPlantDescription, " & _
                     "            fldOeeDepartmentID, " & _
                     "            fldOeeDepartmentDescription, " & _
                     "            fldOeeMachineID, " & _
                     "            fldOeeMachineCode, " & _
                     "            fldOeeMachineDescription, " & _
                     "            fldOeeMachineStatusID, " & _
                     "            fldOeeMachineStatusDescription, " & _
                     "            fldOeeActivityID, " & _
                     "            fldOeeActivityDescription, " & _
                     "            fldOeeActivityGroupID, " & _
                     "            fldOeeActivityGroupDescription, " & _
                     "            fldOeeCounter, " & _
                     "            fldOeeCounterUnitID, " & _
                     "            fldOeeCounterUnitDescription, " & _
                     "            fldOeeNormSpeed, " & _
                     "            fldOeeUserLogInformation, " & _
                     "            fldOeeUserShiftLogInformation, " & _
                     "            fldOeeCurrentOEE, " & _
                     "            fldOeeCurrentAvailability, " & _
                     "            fldOeeCurrentPerformance, " & _
                     "            fldOeeCurrentQuality, " & _
                     "            fldOeeActivityGroupCalcForOee) " & _
                     "VALUES      ('" & lngRegNr & "', '" & _
                                       gfstrDatToStr(Date.Now) & "', " & _
                                       "NULL" & ", '" & _
                                       intActivityMinutes & "', '" & _
                                       gfstrDatToStr(garrTeamShift.datStartShift) & "', '" & _
                                       gfstrDatToStr(garrTeamShift.datEndShift) & "', '" & _
                                       intShiftMinutes & "', '" & _
                                       garrTeamShift.intShiftNr & "', '" & _
                                       garrTeamShift.strShift & "', '" & _
                                       garrTeamShift.intTeamNr & "', '" & _
                                       garrTeamShift.strTeam & "', '" & _
                                       garrTeamShift.intTeamColor & "', '" & _
                                       garrTeamShift.intTeamColor & "', '" & _
                                       garrMachine(intSelMach).intCountryNr & "', '" & _
                                       garrMachine(intSelMach).strCountry & "', '" & _
                                       garrMachine(intSelMach).intPlantNr & "', '" & _
                                       garrMachine(intSelMach).strPlant & "', '" & _
                                       garrMachine(intSelMach).intSubPlantNr & "', '" & _
                                       garrMachine(intSelMach).strSubPlant & "', '" & _
                                       garrMachine(intSelMach).intDepartmentNr & "', '" & _
                                       garrMachine(intSelMach).strDepartment & "', '" & _
                                       garrMachine(intSelMach).intMachineNr & "', '" & _
                                       garrMachine(intSelMach).intMachineCode & "', '" & _
                                       garrMachine(intSelMach).strMachine & "', '" & _
                                       garrMachine(intSelMach).intCurrStatus & "', '" & _
                                       garrMachine(intSelMach).strStatus(garrMachine(intSelMach).intCurrStatus) & "', '" & _
                                       intSelActiNr & "', '" & _
                                       strSelActi & "', '" & _
                                       intSelActiGroupNr & "', '" & _
                                       strSelActiGroup & "', '" & _
                                       garrMachine(intSelMach).lngCounter & "', '" & _
                                       garrMachine(intSelMach).intMachUnitNr & "', '" & _
                                       garrMachine(intSelMach).strMachUnit & "', '" & _
                                       garrMachine(intSelMach).intNormSpeed & "', '" & _
                                       "" & "', '" & _
                                       "" & "', '" & _
                                       garrOEE(intSelMach).intOEE & "', '" & _
                                       garrOEE(intSelMach).intAvailability & "', '" & _
                                       garrOEE(intSelMach).intPerformance & "', '" & _
                                       garrOEE(intSelMach).intQuality & "', '" & _
                                       intCalcForOEE & "');"

      gintSqlCeExecuteNonQuery(strSqlQuery)

      If garrMachine(intSelMach).intCurrStatus = 1 Or garrMachine(intSelMach).intCurrStatus = 2 Then
         gfblnUpdatePerformance(intSelMach, lngRegNr, 1, garrMachine(intSelMach).intNormSpeed)
      End If

   End Function


   Public Function gfblnUpdateReg(ByVal intSelMach As Integer, ByVal intStatus As Integer) As Boolean

      Dim intRecStatus As Integer
      Dim datEndDate As DateTime
      Dim intTimeSpan As TimeSpan
      Dim intActivityDuration As Integer
      Dim strSqlQuery As String
      Dim rstRegOpen As ADODB.Recordset
      Dim lngRegNr As Long
      Dim intCurrNormSpeed As Integer

      If garrMachine(intSelMach).intCurrArticleNormSpeed = 0 Then
         intCurrNormSpeed = garrMachine(intSelMach).intCurrOrderNormSpeed
      End If
      If garrMachine(intSelMach).intCurrOrderNormSpeed = 0 Then
         intCurrNormSpeed = garrMachine(intSelMach).intCurrArticleNormSpeed
      End If
      If intCurrNormSpeed = 0 Then
         intCurrNormSpeed = garrMachine(intSelMach).intNormSpeed
      End If


      strSqlQuery = "SELECT       fldOeeRegTableKeyID, " & _
                    "             fldOeeRegNr, " & _
                    "             fldOeeStartDateTime, " & _
                    "             fldOeeEndDateTime, " & _
                    "             fldOeeActivityDuration, " & _
                    "             fldOeeShiftStartDateTime, " & _
                    "             fldOeeShiftEndDateTime, " & _
                    "             fldOeeMachineStatusID " & _
                    "FROM         tblOee_Reg " & _
                    "WHERE       (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "ORDER BY     fldOeeStartDateTime DESC;"

      rstRegOpen = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstRegOpen.EOF Then
         rstRegOpen.MoveLast()
         rstRegOpen.MoveFirst()

         lngRegNr = rstRegOpen.Fields("fldOeeRegNr").Value
         intRecStatus = rstRegOpen.Fields("fldOeeMachineStatusID").Value

         'check if status is changed
         If intRecStatus = intStatus Then
            'The same record update + adjust timeminutes, counter, oee
            datEndDate = gfToDate(Date.Now)
            intTimeSpan = datEndDate.Subtract(rstRegOpen.Fields("fldOeeStartDateTime").Value)
            intActivityDuration = intTimeSpan.Seconds + (intTimeSpan.Minutes * 60)

            strSqlQuery = "UPDATE tblOee_Reg " & _
                          "SET    fldOeeActivityDuration = '" & intActivityDuration & "', " & _
                          "       fldOeeEndDateTime = '" & gfstrDatToStr(Date.Now) & "', " & _
                          "       fldOeeCounter = '" & garrMachine(intSelMach).lngCounter & "', " & _
                          "       fldOeeAverageSpeed = '" & garrMachine(intSelMach).intAverageSpeed & "', " & _
                          "       fldOeeCurrentAvailability = '" & garrOEE(intSelMach).intAvailability & "', " & _
                          "       fldOeeCurrentPerformance = '" & garrOEE(intSelMach).intPerformance & "', " & _
                          "       fldOeeCurrentQuality = '" & garrOEE(intSelMach).intQuality & "', " & _
                          "       fldOeeCurrentOEE = '" & garrOEE(intSelMach).intOEE & "' " & _
                          "WHERE (fldOeeRegTableKeyID = '" & rstRegOpen.Fields("fldOeeRegTableKeyID").Value & "');"

            gintSqlCeExecuteNonQuery(strSqlQuery)

            'update performance table every 5 minutes
            Dim intMinutes As Integer

            intMinutes = Date.Now.Minute
            If Len(intMinutes.ToString) = 2 Then
               intMinutes = Mid(intMinutes, 2)
            End If

            'update progress every minute
            If gintUpdatedPerformanceTrigger(intSelMach) = 0 Then
               gfblnUpdatePerformance(intSelMach, lngRegNr, intActivityDuration, intCurrNormSpeed)
               gintUpdatedPerformanceTrigger(intSelMach) = 1
            ElseIf gintUpdatedPerformanceTrigger(intSelMach) >= 59 Then
               gfblnUpdatePerformance(intSelMach, lngRegNr, intActivityDuration, intCurrNormSpeed)
               gintUpdatedPerformanceTrigger(intSelMach) = 1
            Else
               gintUpdatedPerformanceTrigger(intSelMach) = gintUpdatedPerformanceTrigger(intSelMach) + 1
            End If

            Return True
         ElseIf intRecStatus = 1 And intStatus = 2 Then
            Return True
         Else
            gintUpdatedPerformanceTrigger(intSelMach) = 0
            'close last record
            datEndDate = gfToDate(Date.Now)
            intTimeSpan = datEndDate.Subtract(rstRegOpen.Fields("fldOeeStartDateTime").Value)
            intActivityDuration = intTimeSpan.Seconds + (intTimeSpan.Minutes * 60)

            strSqlQuery = "UPDATE tblOee_Reg " & _
                          "SET    fldOeeActivityDuration = '" & intActivityDuration & "', " & _
                          "       fldOeeEndDateTime = '" & gfstrDatToStr(Date.Now) & "', " & _
                          "       fldOeeCounter = '" & garrMachine(intSelMach).lngCounter & "', " & _
                          "       fldOeeCurrentAvailability = '" & garrOEE(intSelMach).intAvailability & "', " & _
                          "       fldOeeCurrentPerformance = '" & garrOEE(intSelMach).intPerformance & "', " & _
                          "       fldOeeCurrentQuality = '" & garrOEE(intSelMach).intQuality & "', " & _
                          "       fldOeeCurrentOEE = '" & garrOEE(intSelMach).intOEE & "' " & _
                          "WHERE (fldOeeRegTableKeyID = '" & rstRegOpen.Fields("fldOeeRegTableKeyID").Value & "');"

            gintSqlCeExecuteNonQuery(strSqlQuery)

            'add last performance record to tblOee_Progress
            If rstRegOpen.Fields("fldOeeMachineStatusID").Value = 1 Or rstRegOpen.Fields("fldOeeMachineStatusID").Value = 2 Then
               gfblnUpdatePerformance(intSelMach, lngRegNr, intActivityDuration, intCurrNormSpeed)
            End If


            Return False
         End If
      Else
         Return False
      End If

   End Function


   Public Function gfblnUpdatePerformance(ByVal intSelMach As Integer, ByVal lngRegNr As Long, ByVal intActivityMinutes As Integer, ByVal intCurrNormSpeed As Integer) As Boolean

      Dim strSqlQuery As String
      Dim datEndDate As DateTime
      Dim intTimeSpan As TimeSpan
      Dim rstRegOpen As ADODB.Recordset

      'when no regnr is given then get last regnr
      If lngRegNr = 0 Then
         lngRegNr = gflngGetLastRecordId(intSelMach)
      End If

      If intActivityMinutes = 0 Then
         strSqlQuery = "SELECT       fldOeeRegTableKeyID, " & _
                       "             fldOeeRegNr, " & _
                       "             fldOeeStartDateTime, " & _
                       "             fldOeeEndDateTime, " & _
                       "             fldOeeActivityDuration, " & _
                       "             fldOeeShiftStartDateTime, " & _
                       "             fldOeeShiftEndDateTime, " & _
                       "             fldOeeMachineStatusID " & _
                       "FROM         tblOee_Reg " & _
                       "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                       "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                       "AND         (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                       "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                       "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                       "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                       "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                       "ORDER BY     fldOeeStartDateTime DESC;"

         rstRegOpen = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         If Not rstRegOpen.EOF Then
            datEndDate = gfToDate(Date.Now)
            intTimeSpan = datEndDate.Subtract(rstRegOpen.Fields("fldOeeStartDateTime").Value)
            intActivityMinutes = intTimeSpan.Seconds + (intTimeSpan.Minutes * 60)
         End If

      End If

      strSqlQuery = "INSERT INTO tblOee_Progress " & _
                    "           (fldOeeRegID, " & _
                    "            fldOeeMachineID, " & _
                    "            fldOeeStartDateTime, " & _
                    "            fldOeeActivityDuration, " & _
                    "            fldOeeCounter, " & _
                    "            fldOeeNormSpeed, " & _
                    "            fldOeeCountryID, " & _
                    "            fldOeePlantID, " & _
                    "            fldOeeSubPlantID, " & _
                    "            fldOeeDepartmentID, " & _
                    "            fldOeeCurrentPerformance, " & _
                    "            fldOeeCurrentAvailability, " & _
                    "            fldOeeCurrentQuality, " & _
                    "            fldOeeCurrentOee, " & _
                    "            fldOeeRegHistory, " & _
                    "            fldOeeDateModified) " & _
                    "VALUES      (" & lngRegNr & ", " & _
                    "             " & garrMachine(intSelMach).intMachineNr & ", " & _
                    "             '" & gfstrDatToStr(Date.Now) & "', " & _
                    "             " & intActivityMinutes & ", " & _
                    "             " & garrMachine(intSelMach).lngCounter & ", " & _
                    "             " & intCurrNormSpeed & ", " & _
                    "             " & garrMachine(intSelMach).intCountryNr & ", " & _
                    "             " & garrMachine(intSelMach).intPlantNr & ", " & _
                    "             " & garrMachine(intSelMach).intSubPlantNr & ", " & _
                    "             " & garrMachine(intSelMach).intDepartmentNr & ", " & _
                    "             " & garrOEE(intSelMach).intPerformance & ", " & _
                    "             " & garrOEE(intSelMach).intAvailability & ", " & _
                    "             " & garrOEE(intSelMach).intQuality & ", " & _
                    "             " & garrOEE(intSelMach).intOEE & ", " & _
                    "             0, " & _
                    "             '" & gfstrDatToStr(Date.Now) & "');"

      gintSqlCeExecuteNonQuery(strSqlQuery)

   End Function


   Public Function gblnGetAllUndefined() As Boolean

      Dim strSqlQuery As String
      Dim rstReg As ADODB.Recordset
      Dim intX As Integer
      Dim intTimeSpan As TimeSpan
      Dim intMinutes As Integer

      'all machine
      strSqlQuery = "SELECT       fldOeeRegTableKeyID, " & _
                    "             fldOeeMachineStatusID, " & _
                    "             fldOeeStartDateTime " & _
                    "FROM         tblOee_Reg " & _
                    "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        ((fldOeeMachineStatusID = 1) " & _
                    "OR          (fldOeeMachineStatusID = 4)) " & _
                    "AND         (fldOeeCountryID = '" & garrMachine(0).intCountryNr & "') " & _
                    "AND         (fldOeePlantID = '" & garrMachine(0).intPlantNr & "') " & _
                    "AND         (fldOeeSubPlantID = '" & garrMachine(0).intSubPlantNr & "') " & _
                    "AND         (fldOeeDepartmentID = '" & garrMachine(0).intDepartmentNr & "') " & _
                    "ORDER BY     fldOeeStartDateTime;"

      rstReg = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstReg.EOF Then
         rstReg.MoveFirst()
         For intX = 0 To rstReg.RecordCount - 1
            intTimeSpan = garrTeamShift.datEndShift.Subtract(rstReg.Fields("fldOeeStartDateTime").Value)
            intMinutes = (intTimeSpan.Minutes + (intTimeSpan.Hours * 60))
            If intMinutes > 20 Then
               Return True
            End If
         Next
      Else
         gblnGetAllUndefined = False
      End If

   End Function


   Public Function gfblnGetUndefined(ByVal intSelMach As Integer) As Boolean

      Dim strSqlQuery As String
      Dim rstReg As ADODB.Recordset
      Dim intX As Integer

      strSqlQuery = "SELECT       fldOeeRegTableKeyID, " & _
                    "             fldOeeMachineStatusID, " & _
                    "             fldOeeStartDateTime " & _
                    "FROM         tblOee_Reg " & _
                    "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND        ((fldOeeMachineStatusID = 1) " & _
                    "OR          (fldOeeMachineStatusID = 4)) " & _
                    "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "AND         (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "ORDER BY     fldOeeMachineStatusID;"

      rstReg = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      garrMachine(intSelMach).intPendingProd = 0
      garrMachine(intSelMach).intPendingStand = 0

      If Not rstReg.EOF Then
         rstReg.MoveFirst()
         For intX = 0 To rstReg.RecordCount - 1
            If rstReg.Fields("fldOeeMachineStatusID").Value = 1 Then
               garrMachine(intSelMach).intPendingProd = garrMachine(intSelMach).intPendingProd + 1
            ElseIf rstReg.Fields("fldOeeMachineStatusID").Value = 4 Then
               garrMachine(intSelMach).intPendingStand = garrMachine(intSelMach).intPendingStand + 1
            End If
         Next
      End If


   End Function


   Public Function gfblnContinueProduction(ByVal intSelMach As Integer) As Boolean

      Dim strSqlQuery As String
      Dim strSqlQueryAdd As String
      Dim rstRegCopy As ADODB.Recordset
      Dim strSQLRecord(44) As String
      Dim intX As Integer

      strSqlQueryAdd = ""

      strSqlQuery = "SELECT      fldOeeRegTableKeyID, " & _
                    "            fldOeeMachineID, " & _
                    "            fldOeeMachineCode, " & _
                    "            fldOeeMachineDescription, " & _
                    "            fldOeeMachineStatusID, " & _
                    "            fldOeeMachineStatusDescription, " & _
                    "            fldOeeActivityGroupID, " & _
                    "            fldOeeActivityGroupDescription, " & _
                    "            fldOeeActivityID, " & _
                    "            fldOeeActivityDescription, " & _
                    "            fldOeeArticleNr, " & _
                    "            fldOeeArticleDescription, " & _
                    "            fldOeeOrderNr, " & _
                    "            fldOeeOrderDescription, " & _
                    "            fldOeeStartDateTime, " & _
                    "            fldOeeEndDateTime, " & _
                    "            fldOeeActivityDuration, " & _
                    "            fldOeeShiftStartDateTime, " & _
                    "            fldOeeShiftEndDateTime, " & _
                    "            fldOeeShiftDuration, " & _
                    "            fldOeePlantID, " & _
                    "            fldOeePlantDescription, " & _
                    "            fldOeeSubPlantID, " & _
                    "            fldOeeSubPlantDescription, " & _
                    "            fldOeeCountryID, " & _
                    "            fldOeeCountryDescription, " & _
                    "            fldOeeDepartmentID, " & _
                    "            fldOeeDepartmentDescription, " & _
                    "            fldOeeTeamID, " & _
                    "            fldOeeTeamDescription, " & _
                    "            fldOeeTeamColorID, " & _
                    "            fldOeeTeamColorDescription, " & _
                    "            fldOeeShiftTimeID, " & _
                    "            fldOeeShiftTimeDescription, " & _
                    "            fldOeeAverageSpeed, " & _
                    "            fldOeeNormSpeed, " & _
                    "            fldOeeCounter, " & _
                    "            fldOeeCounterUnitID, " & _
                    "            fldOeeCounterUnitDescription, " & _
                    "            fldOeeCurrentOee, " & _
                    "            fldOeeCurrentAvailability, " & _
                    "            fldOeeCurrentPerformance, " & _
                    "            fldOeeCurrentQuality, " & _
                    "            fldOeeActivityGroupCalcForOee " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND         (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "ORDER BY     fldOeeStartDateTime DESC;"

      rstRegCopy = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstRegCopy.EOF Then
         rstRegCopy.MoveLast()
         rstRegCopy.MoveFirst()
         If rstRegCopy.Fields("fldOeeMachineStatusID").Value = 3 Then
            rstRegCopy.MoveNext()
            If rstRegCopy.EOF Then
               Exit Function
            End If
            If rstRegCopy.Fields("fldOeeMachineStatusID").Value = 1 Or rstRegCopy.Fields("fldOeeMachineStatusID").Value = 2 Then
               'copy record to new record
               For intX = 0 To 42
                  strSQLRecord(intX) = IIf(IsDBNull(rstRegCopy.Fields(intX + 1).Value), "", rstRegCopy.Fields(intX + 1).Value)
               Next
               strSqlQuery = "INSERT INTO tblOee_Reg " & _
                             "           (fldOeeMachineID, " & _
                             "            fldOeeMachineCode, " & _
                             "            fldOeeMachineDescription, " & _
                             "            fldOeeMachineStatusID, " & _
                             "            fldOeeMachineStatusDescription, " & _
                             "            fldOeeActivityGroupID, " & _
                             "            fldOeeActivityGroupDescription, " & _
                             "            fldOeeActivityID, " & _
                             "            fldOeeActivityDescription, " & _
                             "            fldOeeArticleNr, " & _
                             "            fldOeeArticleDescription, " & _
                             "            fldOeeOrderNr, " & _
                             "            fldOeeOrderDescription, " & _
                             "            fldOeeStartDateTime, " & _
                             "            fldOeeEndDateTime, " & _
                             "            fldOeeActivityDuration, " & _
                             "            fldOeeShiftStartDateTime, " & _
                             "            fldOeeShiftEndDateTime, " & _
                             "            fldOeeShiftDuration, " & _
                             "            fldOeePlantID, " & _
                             "            fldOeePlantDescription, " & _
                             "            fldOeeSubPlantID, " & _
                             "            fldOeeSubPlantDescription, " & _
                             "            fldOeeCountryID, " & _
                             "            fldOeeCountryDescription, " & _
                             "            fldOeeDepartmentID, " & _
                             "            fldOeeDepartmentDescription, " & _
                             "            fldOeeTeamID, " & _
                             "            fldOeeTeamDescription, " & _
                             "            fldOeeTeamColorID, " & _
                             "            fldOeeTeamColorDescription, " & _
                             "            fldOeeShiftTimeID, " & _
                             "            fldOeeShiftTimeDescription, " & _
                             "            fldOeeAverageSpeed, " & _
                             "            fldOeeNormSpeed, " & _
                             "            fldOeeCounter, " & _
                             "            fldOeeCounterUnitID, " & _
                             "            fldOeeCounterUnitDescription, " & _
                             "            fldOeeCurrentOee, " & _
                             "            fldOeeCurrentAvailability, " & _
                             "            fldOeeCurrentPerformance, " & _
                             "            fldOeeCurrentQuality, " & _
                             "            fldOeeActivityGroupCalcForOee) " & _
                             "VALUES      ('"

               For intX = 0 To 42
                  If intX = 13 Then
                     'startdate
                     strSqlQueryAdd = strSqlQueryAdd & gfstrDatToStr(Date.Now) & "', '"
                  ElseIf intX = 14 Then
                     'enddate
                     strSqlQueryAdd = strSqlQueryAdd & gfstrDatToStr(Date.Now) & "', '"
                  ElseIf intX = 16 Then
                     'adjust datetime notation shiftstartdate
                     strSqlQueryAdd = strSqlQueryAdd & gfstrDatToStr(strSQLRecord(intX)) & "', '"
                  ElseIf intX = 17 Then
                     'adjust datetime notation shiftenddate
                     strSqlQueryAdd = strSqlQueryAdd & gfstrDatToStr(strSQLRecord(intX)) & "', '"
                  ElseIf intX = 42 Then
                     'last record
                     strSqlQueryAdd = strSqlQueryAdd & strSQLRecord(intX) & "')"
                  Else
                     strSqlQueryAdd = strSqlQueryAdd & strSQLRecord(intX) & "', '"
                  End If
               Next

               gintSqlCeExecuteNonQuery(strSqlQuery)

               garrMachine(intSelMach).intCurrStatus = strSQLRecord(3)
               garrMachine(intSelMach).intStatusPrev = strSQLRecord(3)
            End If
         End If
      End If

   End Function


   Public Function gfblnShiftChange(ByVal intSelMach As Integer, ByVal strNewShiftInfo As String) As Boolean

      Dim strSqlQuery As String
      Dim strSqlQueryAdd As String
      Dim rstRegCopy As ADODB.Recordset

      strSqlQueryAdd = ""

      strSqlQuery = "SELECT      fldOeeRegTableKeyID, " & _
                    "            fldOeeRegNr, " & _
                    "            fldOeeMachineID, " & _
                    "            fldOeeMachineCode, " & _
                    "            fldOeeMachineDescription, " & _
                    "            fldOeeMachineStatusID, " & _
                    "            fldOeeMachineStatusDescription, " & _
                    "            fldOeeActivityGroupID, " & _
                    "            fldOeeActivityGroupDescription, " & _
                    "            fldOeeActivityID, " & _
                    "            fldOeeActivityDescription, " & _
                    "            fldOeeArticleNr, " & _
                    "            fldOeeArticleDescription, " & _
                    "            fldOeeOrderNr, " & _
                    "            fldOeeOrderDescription, " & _
                    "            fldOeeStartDateTime, " & _
                    "            fldOeeEndDateTime, " & _
                    "            fldOeeActivityDuration, " & _
                    "            fldOeeShiftStartDateTime, " & _
                    "            fldOeeShiftEndDateTime, " & _
                    "            fldOeeShiftTimeID, " & _
                    "            fldOeeShiftTimeDescription, " & _
                    "            fldOeeTeamID, " & _
                    "            fldOeeTeamDescription, " & _
                    "            fldOeeTeamColorID, " & _
                    "            fldOeeTeamColorDescription, " & _
                    "            fldOeeShiftDuration, " & _
                    "            fldOeePlantID, " & _
                    "            fldOeePlantDescription, " & _
                    "            fldOeeSubPlantID, " & _
                    "            fldOeeSubPlantDescription, " & _
                    "            fldOeeCountryID, " & _
                    "            fldOeeCountryDescription, " & _
                    "            fldOeeDepartmentID, " & _
                    "            fldOeeDepartmentDescription, " & _
                    "            fldOeeAverageSpeed, " & _
                    "            fldOeeNormSpeed, " & _
                    "            fldOeeCounter, " & _
                    "            fldOeeCounterUnitID, " & _
                    "            fldOeeCounterUnitDescription, " & _
                    "            fldOeeCurrentOee, " & _
                    "            fldOeeCurrentAvailability, " & _
                    "            fldOeeCurrentPerformance, " & _
                    "            fldOeeCurrentQuality, " & _
                    "            fldOeeActivityGroupCalcForOee " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND         (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "ORDER BY     fldOeeStartDateTime DESC;"

      rstRegCopy = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstRegCopy.EOF Then
         rstRegCopy.MoveLast()
         rstRegCopy.MoveFirst()
         'copy record to new record
         strSqlQuery = "INSERT INTO tblOee_Reg " & _
                       "           (fldOeeRegNr, " & _
                       "            fldOeeMachineID, " & _
                       "            fldOeeMachineCode, " & _
                       "            fldOeeMachineDescription, " & _
                       "            fldOeeMachineStatusID, " & _
                       "            fldOeeMachineStatusDescription, " & _
                       "            fldOeeActivityGroupID, " & _
                       "            fldOeeActivityGroupDescription, " & _
                       "            fldOeeActivityID, " & _
                       "            fldOeeActivityDescription, " & _
                       "            fldOeeArticleNr, " & _
                       "            fldOeeArticleDescription, " & _
                       "            fldOeeOrderNr, " & _
                       "            fldOeeOrderDescription, " & _
                       "            fldOeeStartDateTime, " & _
                       "            fldOeeEndDateTime, " & _
                       "            fldOeeActivityDuration, " & _
                       "            fldOeeShiftStartDateTime, " & _
                       "            fldOeeShiftEndDateTime, " & _
                       "            fldOeeShiftTimeID, " & _
                       "            fldOeeShiftTimeDescription, " & _
                       "            fldOeeTeamID, " & _
                       "            fldOeeTeamDescription, " & _
                       "            fldOeeTeamColorID, " & _
                       "            fldOeeTeamColorDescription, " & _
                       "            fldOeeShiftDuration, " & _
                       "            fldOeePlantID, " & _
                       "            fldOeePlantDescription, " & _
                       "            fldOeeSubPlantID, " & _
                       "            fldOeeSubPlantDescription, " & _
                       "            fldOeeCountryID, " & _
                       "            fldOeeCountryDescription, " & _
                       "            fldOeeDepartmentID, " & _
                       "            fldOeeDepartmentDescription, " & _
                       "            fldOeeAverageSpeed, " & _
                       "            fldOeeNormSpeed, " & _
                       "            fldOeeCounter, " & _
                       "            fldOeeCounterUnitID, " & _
                       "            fldOeeCounterUnitDescription, " & _
                       "            fldOeeCurrentOee, " & _
                       "            fldOeeCurrentAvailability, " & _
                       "            fldOeeCurrentPerformance, " & _
                       "            fldOeeCurrentQuality, " & _
                       "            fldOeeActivityGroupCalcForOee) " & _
                       "VALUES      ('" & (CLng(rstRegCopy.Fields("fldOeeRegNr").Value) + 1) & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeMachineID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeMachineCode").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeMachineDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeMachineStatusID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeMachineStatusDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeActivityGroupID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeActivityGroupDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeActivityID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeActivityDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeArticleNr").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeArticleDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeOrderNr").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeOrderDescription").Value & "', " & _
                       "             '" & gfstrDatToStr(Date.Now) & "', " & _
                       "             '" & gfstrDatToStr(Date.Now) & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeActivityDuration").Value & "', " & _
                       "             '" & strNewShiftInfo & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeTeamColorID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeTeamColorDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeShiftDuration").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeePlantID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeePlantDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeSubPlantID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeSubPlantDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCountryID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCountryDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeDepartmentID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeDepartmentDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeAverageSpeed").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeNormSpeed").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCounter").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCounterUnitID").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCounterUnitDescription").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCurrentOee").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCurrentAvailability").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCurrentPerformance").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeCurrentQuality").Value & "', " & _
                       "             '" & rstRegCopy.Fields("fldOeeActivityGroupCalcForOee").Value & "');"

         gintSqlCeExecuteNonQuery(strSqlQuery)

      End If

   End Function


   Public Function gfblnContinueAfterUpdating(ByVal intSelMach As Integer) As Boolean

      Dim strSqlQuery As String
      Dim strSqlQueryAdd As String
      Dim rstRegCopy As ADODB.Recordset

      strSqlQueryAdd = ""

      strSqlQuery = "SELECT      fldOeeRegTableKeyID, " & _
                    "            fldOeeRegNr, " & _
                    "            fldOeeMachineID, " & _
                    "            fldOeeMachineCode, " & _
                    "            fldOeeMachineDescription, " & _
                    "            fldOeeMachineStatusID, " & _
                    "            fldOeeMachineStatusDescription, " & _
                    "            fldOeeActivityGroupID, " & _
                    "            fldOeeActivityGroupDescription, " & _
                    "            fldOeeActivityID, " & _
                    "            fldOeeActivityDescription, " & _
                    "            fldOeeArticleNr, " & _
                    "            fldOeeArticleDescription, " & _
                    "            fldOeeOrderNr, " & _
                    "            fldOeeOrderDescription, " & _
                    "            fldOeeStartDateTime, " & _
                    "            fldOeeEndDateTime, " & _
                    "            fldOeeActivityDuration, " & _
                    "            fldOeeShiftStartDateTime, " & _
                    "            fldOeeShiftEndDateTime, " & _
                    "            fldOeeShiftTimeID, " & _
                    "            fldOeeShiftTimeDescription, " & _
                    "            fldOeeTeamID, " & _
                    "            fldOeeTeamDescription, " & _
                    "            fldOeeTeamColorID, " & _
                    "            fldOeeTeamColorDescription, " & _
                    "            fldOeeShiftDuration, " & _
                    "            fldOeePlantID, " & _
                    "            fldOeePlantDescription, " & _
                    "            fldOeeSubPlantID, " & _
                    "            fldOeeSubPlantDescription, " & _
                    "            fldOeeCountryID, " & _
                    "            fldOeeCountryDescription, " & _
                    "            fldOeeDepartmentID, " & _
                    "            fldOeeDepartmentDescription, " & _
                    "            fldOeeAverageSpeed, " & _
                    "            fldOeeNormSpeed, " & _
                    "            fldOeeCounter, " & _
                    "            fldOeeCounterUnitID, " & _
                    "            fldOeeCounterUnitDescription, " & _
                    "            fldOeeCurrentOee, " & _
                    "            fldOeeCurrentAvailability, " & _
                    "            fldOeeCurrentPerformance, " & _
                    "            fldOeeCurrentQuality, " & _
                    "            fldOeeActivityGroupCalcForOee " & _
                    "FROM        tblOee_Reg " & _
                    "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                    "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                    "AND         (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                    "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                    "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                    "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                    "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "ORDER BY     fldOeeStartDateTime DESC;"

      rstRegCopy = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

      If Not rstRegCopy.EOF Then
         rstRegCopy.MoveLast()
         rstRegCopy.MoveFirst()
         rstRegCopy.MoveNext()
         If rstRegCopy.Fields("fldOeeActivityGroupDescription").Value = "Update" Then
            rstRegCopy.MoveNext()
            'copy record to new record
            gintStatusBeforeUpdate = rstRegCopy.Fields("fldOeeMachineStatusID").Value
            strSqlQuery = "INSERT INTO tblOee_Reg " & _
                          "           (fldOeeRegNr, " & _
                          "            fldOeeMachineID, " & _
                          "            fldOeeMachineCode, " & _
                          "            fldOeeMachineDescription, " & _
                          "            fldOeeMachineStatusID, " & _
                          "            fldOeeMachineStatusDescription, " & _
                          "            fldOeeActivityGroupID, " & _
                          "            fldOeeActivityGroupDescription, " & _
                          "            fldOeeActivityID, " & _
                          "            fldOeeActivityDescription, " & _
                          "            fldOeeArticleNr, " & _
                          "            fldOeeArticleDescription, " & _
                          "            fldOeeOrderNr, " & _
                          "            fldOeeOrderDescription, " & _
                          "            fldOeeStartDateTime, " & _
                          "            fldOeeEndDateTime, " & _
                          "            fldOeeActivityDuration, " & _
                          "            fldOeeShiftStartDateTime, " & _
                          "            fldOeeShiftEndDateTime, " & _
                          "            fldOeeShiftTimeID, " & _
                          "            fldOeeShiftTimeDescription, " & _
                          "            fldOeeTeamID, " & _
                          "            fldOeeTeamDescription, " & _
                          "            fldOeeTeamColorID, " & _
                          "            fldOeeTeamColorDescription, " & _
                          "            fldOeeShiftDuration, " & _
                          "            fldOeePlantID, " & _
                          "            fldOeePlantDescription, " & _
                          "            fldOeeSubPlantID, " & _
                          "            fldOeeSubPlantDescription, " & _
                          "            fldOeeCountryID, " & _
                          "            fldOeeCountryDescription, " & _
                          "            fldOeeDepartmentID, " & _
                          "            fldOeeDepartmentDescription, " & _
                          "            fldOeeAverageSpeed, " & _
                          "            fldOeeNormSpeed, " & _
                          "            fldOeeCounter, " & _
                          "            fldOeeCounterUnitID, " & _
                          "            fldOeeCounterUnitDescription, " & _
                          "            fldOeeCurrentOee, " & _
                          "            fldOeeCurrentAvailability, " & _
                          "            fldOeeCurrentPerformance, " & _
                          "            fldOeeCurrentQuality, " & _
                          "            fldOeeActivityGroupCalcForOee) " & _
                          "VALUES      ('" & (CLng(rstRegCopy.Fields("fldOeeRegNr").Value) + 1) & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeMachineID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeMachineCode").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeMachineDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeMachineStatusID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeMachineStatusDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeActivityGroupID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeActivityGroupDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeActivityID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeActivityDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeArticleNr").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeArticleDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeOrderNr").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeOrderDescription").Value & "', " & _
                          "             '" & gfstrDatToStr(Date.Now) & "', " & _
                          "             '" & gfstrDatToStr(Date.Now) & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeActivityDuration").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeShiftStartDateTime").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeShiftEndDateTime").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeShiftTimeID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeShiftTimeDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeTeamID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeTeamDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeTeamColorID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeTeamColorDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeShiftDuration").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeePlantID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeePlantDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeSubPlantID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeSubPlantDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCountryID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCountryDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeDepartmentID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeDepartmentDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeAverageSpeed").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeNormSpeed").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCounter").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCounterUnitID").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCounterUnitDescription").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCurrentOee").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCurrentAvailability").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCurrentPerformance").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeCurrentQuality").Value & "', " & _
                          "             '" & rstRegCopy.Fields("fldOeeActivityGroupCalcForOee").Value & "');"

            gintSqlCeExecuteNonQuery(strSqlQuery)
            Return True
         Else
            Return False
         End If
      End If

   End Function


   Public Function gfblnDefineActivity(ByVal intSelMach As Integer, ByVal intMachineStatus As Integer, ByVal intActiGroupNr As Integer, ByVal intActiNr As Integer, _
                                         ByVal strActiGroup As String, ByVal strActi As String, ByVal intTableKeyID As Long, _
                                         ByVal intCalcForOEE As Integer) As Boolean

      Dim strSqlQuery As String
      Dim rstEditReg As ADODB.Recordset

      If intTableKeyID = 0 Then
         strSqlQuery = "SELECT       fldOeeRegTableKeyID " & _
                       "FROM         tblOee_Reg " & _
                       "WHERE       (fldOeeShiftStartDateTime = '" & gfstrDatToStr(garrTeamShift.datStartShift) & "') " & _
                       "AND         (fldOeeShiftEndDateTime = '" & gfstrDatToStr(garrTeamShift.datEndShift) & "') " & _
                       "AND         (fldOeeCountryID = '" & garrMachine(intSelMach).intCountryNr & "') " & _
                       "AND         (fldOeePlantID = '" & garrMachine(intSelMach).intPlantNr & "') " & _
                       "AND         (fldOeeSubPlantID = '" & garrMachine(intSelMach).intSubPlantNr & "') " & _
                       "AND         (fldOeeDepartmentID = '" & garrMachine(intSelMach).intDepartmentNr & "') " & _
                       "AND         (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                       "ORDER BY     fldOeeStartDateTime;"

         rstEditReg = gfconvertToADODB(gdtaSqlCeTable(strSqlQuery))

         If Not rstEditReg.EOF Then
            rstEditReg.MoveFirst()
            rstEditReg.MoveLast()

            strSqlQuery = "UPDATE tblOee_Reg " & _
                          "SET    fldOeeActivityGroupID = '" & intActiGroupNr & "', " & _
                          "       fldOeeActivityID = '" & intActiNr & "', " & _
                          "       fldOeeActivityGroupDescription = '" & strActiGroup & "', " & _
                          "       fldOeeActivityDescription = '" & strActi & "', " & _
                          "       fldOeeActivityGroupCalcForOee = '" & intCalcForOEE & "', " & _
                          "       fldOeeMachineStatusID = " & intMachineStatus & ", " & _
                          "       fldOeeMachineStatusDescription = '" & garrMachine(intSelMach).strStatus(intMachineStatus) & "' " & _
                          "WHERE (fldOeeRegTableKeyID = '" & rstEditReg.Fields("fldOeeRegTableKeyID").Value & "');"

            gintSqlCeExecuteNonQuery(strSqlQuery)
            Return True
         End If

      Else
         strSqlQuery = "UPDATE tblOee_Reg " & _
                       "SET    fldOeeActivityGroupID = '" & intActiGroupNr & "', " & _
                       "       fldOeeActivityID = '" & intActiNr & "', " & _
                       "       fldOeeActivityGroupDescription = '" & strActiGroup & "', " & _
                       "       fldOeeActivityDescription = '" & strActi & "', " & _
                       "       fldOeeActivityGroupCalcForOee = '" & intCalcForOEE & "', " & _
                       "       fldOeeMachineStatusID = " & intMachineStatus & ", " & _
                       "       fldOeeMachineStatusDescription = '" & garrMachine(intSelMach).strStatus(garrMachine(intSelMach).intCurrStatus) & "' " & _
                       "WHERE (fldOeeRegTableKeyID = '" & intTableKeyID & "');"

         gintSqlCeExecuteNonQuery(strSqlQuery)
         gintUndefinedTableKeyID = 0
         Return True
      End If

   End Function


   Public Function gfblnDefineProduction(ByVal intSelMach As Integer, ByVal intMachineStatus As Integer, ByVal intArticleOrOrder As Integer, ByVal strArtiOrderNr As String, _
                                         ByVal strArtiOrderDescription As String, ByVal intAverageSpeed As Integer, ByVal intTableKeyID As Long) As Boolean

      Dim strSqlQuery As String

      If intArticleOrOrder = 1 Then
         strSqlQuery = "UPDATE tblOee_Reg " & _
                       "SET    fldOeeArticleNr = '" & strArtiOrderNr & "', " & _
                       "       fldOeeArticleDescription = '" & strArtiOrderDescription & "', " & _
                       "       fldOeeActivityDescription = '" & strArtiOrderNr & " - " & strArtiOrderDescription & "', " & _
                       "       fldOeeActivityGroupDescription = 'Production', " & _
                       "       fldOeeAverageSpeed = '" & intAverageSpeed & "', " & _
                       "       fldOeeMachineStatusID = " & intMachineStatus & ", " & _
                       "       fldOeeMachineStatusDescription = '" & garrMachine(intSelMach).strStatus(garrMachine(intSelMach).intCurrStatus) & "' " & _
                       "WHERE (fldOeeRegTableKeyID = '" & intTableKeyID & "');"
      Else
         strSqlQuery = "UPDATE tblOee_Reg " & _
                       "SET    fldOeeOrderNr = '" & strArtiOrderNr & "', " & _
                       "       fldOeeOrderDescription = '" & strArtiOrderDescription & "', " & _
                       "       fldOeeActivityDescription = '" & strArtiOrderNr & " - " & strArtiOrderDescription & "', " & _
                       "       fldOeeActivityGroupDescription = 'Production', " & _
                       "       fldOeeAverageSpeed = '" & intAverageSpeed & "', " & _
                       "       fldOeeMachineStatusID = " & intMachineStatus & ", " & _
                       "       fldOeeMachineStatusDescription = '" & garrMachine(intSelMach).strStatus(garrMachine(intSelMach).intCurrStatus) & "' " & _
                       "WHERE (fldOeeRegTableKeyID = '" & intTableKeyID & "');"
      End If

      gintSqlCeExecuteNonQuery(strSqlQuery)
      gintUndefinedTableKeyID = 0
      Return True

   End Function


   Public Function gfintIPModuleCounter(ByVal intSelMach As Integer, ByVal strIP As String, ByVal intAddress As Integer) As Integer

      Dim masterTcpClient As TcpClient
      Dim master As ModbusIpMaster
      Dim slaveID As Byte
      Dim startAddress As UShort
      Dim numInputs As UShort
      Dim inputs() As UShort

      Try
         gfblnCheckOpenTcpPort(strIP, 502)
         masterTcpClient = New TcpClient(strIP, 502)
         master = ModbusIpMaster.CreateIp(masterTcpClient)
         slaveID = 1
         startAddress = 0
         numInputs = UShort.Parse(24)

         inputs = master.ReadInputRegisters(slaveID, startAddress, numInputs)
         gfintIPModuleCounter = inputs(intAddress)

         masterTcpClient.Close()
         garrMachine(intSelMach).blnErrMessDisplayed = False
         mfblnChangeNotifyIcon(intSelMach, 1)
      Catch ex As Exception
         gfintIPModuleCounter = 0
         If garrMachine(intSelMach).blnErrMessDisplayed = False Then
            gfblnWriteLog("Error trying to get module counter information from '" & strIP & "' with port '" & intAddress & "'." & vbCrLf & ex.Message, 1)
            'gfblnModuleErrorHandler(intSelMach)
         End If
         mfblnChangeNotifyIcon(intSelMach, 2)
      End Try

   End Function


   Public Function gfintIPArduino(ByVal strIP As String, ByVal intAddress As Integer) As Integer

      Dim masterTcpClient As TcpClient
      Dim master As ModbusIpMaster
      Dim slaveID As Byte
      Dim startAddress As UShort

      Try
         'gfblnCheckOpenTcpPort(strIP, 502)
         masterTcpClient = New TcpClient(strIP, 502)
         master = ModbusIpMaster.CreateIp(masterTcpClient)
         slaveID = 1
         startAddress = 0

         gfintIPArduino = master.ReadInputRegisters(1, intAddress, 1).GetValue(0)

         masterTcpClient.Close()
      Catch ex As Exception
         gfintIPArduino = 0
      End Try

   End Function


   Public Function gfblnResetIPArduino(ByVal strIP As String, ByVal intAddress As Integer)

      Dim masterTcpClient As TcpClient
      Dim master As ModbusIpMaster

      Try
         'gfblnCheckOpenTcpPort(strIP, 502)
         masterTcpClient = New TcpClient(strIP, 502)
         master = ModbusIpMaster.CreateIp(masterTcpClient)

         master.WriteSingleCoil(1, intAddress, 1)
         masterTcpClient.Close()
         gfblnResetIPArduino = True
      Catch ex As Exception
         gfblnResetIPArduino = False
      End Try

   End Function



   Public Function gfblnResetIPModuleCounter(ByVal intSelMach As Integer, ByVal strIP As String, ByVal intAddress As Integer)

      Dim masterTcpClient As TcpClient
      Dim master As ModbusIpMaster

      Try
         gfblnCheckOpenTcpPort(strIP, 502)
         masterTcpClient = New TcpClient(strIP, 502)
         master = ModbusIpMaster.CreateIp(masterTcpClient)

         master.WriteSingleCoil(1, intAddress, 1)
         masterTcpClient.Close()
         gfblnResetIPModuleCounter = True
         garrMachine(intSelMach).blnErrMessDisplayed = False
         mfblnChangeNotifyIcon(intSelMach, 1)
      Catch ex As Exception
         gfblnResetIPModuleCounter = False
         If garrMachine(intSelMach).blnErrMessDisplayed = False Then
            gfblnWriteLog("Error trying to reset module counter from '" & strIP & "' with port '" & intAddress & "'." & vbCrLf & ex.Message, 1)
            'gfblnModuleErrorHandler(intSelMach)
         End If
         mfblnChangeNotifyIcon(intSelMach, 2)
      End Try

   End Function


   Public Function gfblnIPModuleStatus(ByVal intSelMach As Integer, ByVal strIP As String, ByVal intAddress As Integer) As Boolean

      Dim masterTcpClient As TcpClient
      Dim master As ModbusIpMaster
      Dim slaveID As Byte
      Dim startAddress As UShort
      Dim numInputs As UShort
      Dim inputs() As Boolean

      Try
         gfblnCheckOpenTcpPort(strIP, intAddress)
         masterTcpClient = New TcpClient(strIP, 502)
         master = ModbusIpMaster.CreateIp(masterTcpClient)
         slaveID = 1
         startAddress = 0
         numInputs = UShort.Parse(4)

         'Dim status() As Boolean = master.ReadInputs(slaveID, startAddress, numInputs)

         inputs = master.ReadInputs(slaveID, startAddress, numInputs)
         gfblnIPModuleStatus = inputs(intAddress)

         masterTcpClient.Close()
         garrMachine(intSelMach).blnErrMessDisplayed = False
         mfblnChangeNotifyIcon(intSelMach, 1)
      Catch ex As Exception
         gfblnIPModuleStatus = 0
         If garrMachine(intSelMach).blnErrMessDisplayed = False Then
            gfblnWriteLog("Error trying to get module status information from '" & strIP & "' with port '" & intAddress & "'." & vbCrLf & ex.Message, 1)
            'gfblnModuleErrorHandler(intSelMach)
         End If
         mfblnChangeNotifyIcon(intSelMach, 2)
      End Try

   End Function


   Public Function gfintComModuleCounter(ByVal intSelMach As Integer, ByVal strPortNr As String, ByVal strBaudrate As String, ByVal intDataBits As Integer, _
                                         ByVal intParity As Integer, ByVal intAddress As Integer) As Integer

      Dim serialPort As SerialPort

      serialPort = New SerialPort()

      With serialPort
         .PortName = "COM" & strPortNr
         .BaudRate = strBaudrate
         .DataBits = 8
         .ParityReplace = &H3B
         .Parity = IO.Ports.Parity.None
         .StopBits = IO.Ports.StopBits.One
         .Handshake = IO.Ports.Handshake.None
         .RtsEnable = False
         .ReceivedBytesThreshold = 1
         .NewLine = vbCr
         .ReadTimeout = 10000
         .WriteTimeout = 10000
      End With

      Try
         If Not serialPort.IsOpen Then
            serialPort.Open()
         Else
            serialPort.Close()
         End If
         serialPort.WriteLine("#0" & intAddress & "0")
         gfintComModuleCounter = Mid(serialPort.ReadLine(), 4)
         garrMachine(intSelMach).blnErrMessDisplayed = False
         mfblnChangeNotifyIcon(intSelMach, 1)
      Catch ex As Exception
         Dim strError As String
         strError = ex.Message
         gfintComModuleCounter = 0
         If garrMachine(intSelMach).blnErrMessDisplayed = False Then
            gfblnWriteLog("Error trying to get module counter information from 'COM" & strPortNr & "' with port '" & intAddress & "'." & vbCrLf & ex.Message, 1)
            'gfblnModuleErrorHandler(intSelMach)
         End If
         mfblnChangeNotifyIcon(intSelMach, 2)
      Finally
         serialPort.Close()
      End Try

   End Function


   Public Function gfblnResetComModuleCounter(ByVal intSelMach As Integer, ByVal strPortNr As String, ByVal strBaudrate As String, ByVal intDataBits As Integer, _
                                              ByVal intParity As Integer, ByVal intAddress As Integer) As Boolean

      Dim serialPort As SerialPort

      serialPort = New SerialPort()

      With serialPort
         .PortName = "COM" & strPortNr
         .BaudRate = strBaudrate
         '.DataBits = intDataBits
         .DataBits = 8
         .ParityReplace = &H3B
         .Parity = IO.Ports.Parity.None
         .StopBits = IO.Ports.StopBits.One
         .Handshake = IO.Ports.Handshake.None
         .RtsEnable = False
         .ReceivedBytesThreshold = 1
         .NewLine = vbCr
         .ReadTimeout = 10000
         .WriteTimeout = 10000
      End With

      Try
         If Not serialPort.IsOpen Then
            serialPort.Open()
         Else
            serialPort.Close()
         End If
         serialPort.WriteLine("$0" & intAddress & "C0")
         Dim strTest As String
         strTest = Mid(serialPort.ReadLine(), 4)
         gfblnResetComModuleCounter = True
         garrMachine(intSelMach).blnErrMessDisplayed = False
         mfblnChangeNotifyIcon(intSelMach, 1)
      Catch ex As Exception
         gfblnResetComModuleCounter = False
         If garrMachine(intSelMach).blnErrMessDisplayed = False Then
            gfblnWriteLog("Error trying to rest module counter from 'COM" & strPortNr & "' with port '" & intAddress & "'." & vbCrLf & ex.Message, 1)
            'gfblnModuleErrorHandler(intSelMach)
         End If
         mfblnChangeNotifyIcon(intSelMach, 2)
      Finally
         serialPort.Close()
      End Try

   End Function


   Public Function gfblnComModuleStatus(ByVal intSelMach As Integer, ByVal strPortNr As String, ByVal strBaudrate As String, ByVal intDataBits As Integer, ByVal intParity As Integer, _
                                        ByVal intAddress As Integer) As Boolean

      Dim serialPort As SerialPort
      Dim strSerialLine As String

      serialPort = New SerialPort()

      With serialPort
         .PortName = "COM" & strPortNr
         .BaudRate = strBaudrate
         '.DataBits = intDataBits
         .DataBits = 8
         .ParityReplace = &H3B
         .Parity = IO.Ports.Parity.None
         .StopBits = IO.Ports.StopBits.One
         .Handshake = IO.Ports.Handshake.None
         .RtsEnable = False
         .ReceivedBytesThreshold = 1
         .NewLine = vbCr
         .ReadTimeout = 10000
         .WriteTimeout = 10000
      End With

      'set default value to false
      gfblnComModuleStatus = False
      Try
         If Not serialPort.IsOpen Then
            serialPort.Open()
         Else
            serialPort.Close()
         End If
         serialPort.WriteLine("@0" & intAddress)
         strSerialLine = serialPort.ReadLine()
         If strSerialLine = ">000E" Then
            gfblnComModuleStatus = True
         ElseIf strSerialLine = ">000F" Then
            gfblnComModuleStatus = False
         End If
         garrMachine(intSelMach).blnErrMessDisplayed = False
         mfblnChangeNotifyIcon(intSelMach, 1)
      Catch ex As Exception
         gfblnComModuleStatus = False
         If garrMachine(intSelMach).blnErrMessDisplayed = False Then
            gfblnWriteLog("Error with I/O module 'COM" & strPortNr & "' with port '" & intAddress & "'." & vbCrLf & ex.Message, 1)
            'gfblnModuleErrorHandler(intSelMach)
         End If
         mfblnChangeNotifyIcon(intSelMach, 2)
      Finally
         serialPort.Close()
      End Try

   End Function


   Public Function gfblnModuleErrorHandler(ByVal intSelMach As Integer) As Boolean

      garrMachine(intSelMach).blnErrMessDisplayed = True
      garrMessageSystem(intSelMach).strTitle = "I/O Module error"
      garrMessageSystem(intSelMach).strMessage = "Error with the I/O module of" & vbCrLf & _
                                                 "'" & garrMachine(intSelMach).strMachine & "' - "
      Select Case garrMachine(intSelMach).intModuleTypeNr
         Case 1
            garrMessageSystem(intSelMach).strMessage = garrMessageSystem(intSelMach).strMessage & "'" & _
                                                       garrMachine(intSelMach).strCommIPAddress & "'."
         Case 2
            garrMessageSystem(intSelMach).strMessage = garrMessageSystem(intSelMach).strMessage & "'COM" & _
                                                       garrMachine(intSelMach).strCommComport & "'."
         Case 3
            garrMessageSystem(intSelMach).strMessage = garrMessageSystem(intSelMach).strMessage & "'USB" & _
                                                       garrMachine(intSelMach).strCommComport & "'."
         Case 4
            garrMessageSystem(intSelMach).strMessage = garrMessageSystem(intSelMach).strMessage & "'PARALLEL" & _
                                                       garrMachine(intSelMach).strCommComport & "'."
      End Select

      gfblnUpdateReg(intSelMach, garrMachine(intSelMach).intCurrStatus)
      garrMachine(intSelMach).intCurrStatus = 5
      Call gfblnAddReg(intSelMach, 2, _
                       garrMachine(intSelMach).intIOFailureNr, _
                       "Predefined", _
                       garrMachine(intSelMach).strIOFailure, False, 0, 1)
      garrMachine(intSelMach).intCurrActiGroupNr = garrMachine(intSelMach).intUndefActAndShortBrGrpNr
      garrMachine(intSelMach).intCurrActivityNr = garrMachine(intSelMach).intIOFailureNr

      garrMessageSystem(intSelMach).intAction = 66
      gintSelectedMach = intSelMach
      frmMessageBox.ShowDialog()
      frmMessageBox.mblnShowMessage(intSelMach)
      frmMessageBox.lblTitle.Text = garrMessageSystem(intSelMach).strTitle
      frmMessageBox.lblMessage.Text = garrMessageSystem(intSelMach).strMessage
      frmMessageBox.Refresh()

   End Function


   Public Function mfblnChangeNotifyIcon(ByVal intSelMach As Integer, ByVal intState As Integer) As Boolean

      Try
         Select Case intState
            Case 1
               gpicMachNotify(intSelMach).ImageLocation = gstrImagesLocation & "notify_ok.png"
            Case 2
               gpicMachNotify(intSelMach).ImageLocation = gstrImagesLocation & "notify_error.png"
         End Select
      Catch ex As Exception

      End Try

   End Function

End Module