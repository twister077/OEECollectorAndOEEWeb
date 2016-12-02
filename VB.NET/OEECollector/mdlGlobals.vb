Imports System
Imports System.Diagnostics
Imports System.Net.Sockets
Imports System.Data
Imports System.Data.SQLite
Imports System.Data.SqlServerCe
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Threading
Imports System.IO

Module mdlGlobals

    Public gstrSqlLoginuser As String = "Password"
    Public gstrSqlLoginPassword As String = "Password"
    Public gstrSqlitePassword As String = "Password"
    Public gstrOEEWebserver As String = "https://web2pywebserver:8000/OEEWeb"
    Public gstrWorkstation As String
    Public gstrSqlFileLocation As String = My.Application.Info.DirectoryPath & "\OEESQLSettings.sdf"
    Public gstrSqlLiteLocation As String = My.Application.Info.DirectoryPath & "\dblocal.db"
    Public gstrImagesLocation As String = My.Application.Info.DirectoryPath & "\Images\"
    Public glblMachMain(4) As Label
    Public gchaMachChartOEE(4) As System.Windows.Forms.DataVisualization.Charting.Chart
    Public glblMachTop(4) As Label
    Public glblMachMsg(4) As Label
    Public gpicMachOEENorm(4) As PictureBox
    Public gpicMachNotify(4) As PictureBox
    Public gclrActivityColor(4) As System.Drawing.Color
    Public garrMessageSystem(4) As MessageSystem

    Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal process As IntPtr, ByVal minimumWorkingSetSize As Integer, ByVal maximumWorkingSetSize As Integer) As Integer


    Public Structure MessageSystem

      Dim strTitle As String
      Dim strMessage As String
      Dim intAction As Integer
      Dim blnPromptedForUndef As Boolean

   End Structure


   Public Function gfblnEncrypt(ByVal strFile As String) As Boolean

      Dim connCon As SQLiteConnection

      connCon = New SQLiteConnection("Data Source=" & gstrSqlLiteLocation)
      connCon.Open()
      connCon.ChangePassword(gstrSqlitePassword)
      connCon.Close()

   End Function


   Public Function gdtaSqlCeTable(ByVal strSqlQuery As String) As DataTable

      'orig name = gdtaSqliteTable

      Dim dtaTable As DataTable
      Dim connCon As SQLiteConnection
      Dim cmd As SQLiteCommand
      Dim reader As SQLiteDataReader
      Dim blnError As Boolean
      Dim intX As Integer
      Dim strError As String = ""

      dtaTable = New DataTable()

      For intX = 1 To 10
         Try
            connCon = New SQLiteConnection("Password=" & gstrSqlitePassword & ";Data Source=" & gstrSqlLiteLocation)
            'connCon = New SQLiteConnection("Data Source=" & gstrSqlLiteLocation)
            connCon.Open()
            cmd = New SQLiteCommand(connCon)
            cmd.CommandText = strSqlQuery
            reader = cmd.ExecuteReader()
            dtaTable.Load(reader)
            blnError = False
            Exit For
            'reader.Close()
            'connCon.Close()
         Catch ex As Exception
            strError = ex.Message
            blnError = True
         End Try
      Next

      If blnError Then
         gfblnWriteLog("Error trying to connect the local SQLite database" & vbCrLf & strError, 1)
      End If

      Return dtaTable

   End Function


   Public Function gintSqlCeExecuteNonQuery(ByVal strSqlQuery As String) As Integer

      'origname = gintSqliteExecuteNonQuery

      Dim connCon As SQLiteConnection
      Dim cmd As SQLiteCommand
      Dim intRowsUpdated As Integer
      Dim blnError As Boolean
      Dim intX As Integer
      Dim strError As String = ""

      For intX = 1 To 10
         Try
            connCon = New SQLiteConnection("Password=" & gstrSqlitePassword & ";Data Source=" & gstrSqlLiteLocation)
            'connCon = New SQLiteConnection("Data Source=" & gstrSqlLiteLocation)
            connCon.Open()
            cmd = New SQLiteCommand(connCon)
            cmd.CommandText = strSqlQuery
            intRowsUpdated = cmd.ExecuteNonQuery()
            connCon.Close()
            blnError = False
            Exit For
         Catch ex As Exception
            strError = ex.Message
            blnError = True
         End Try
      Next

      If blnError Then
         gfblnWriteLog("Error trying to connect the local SQLite database" & vbCrLf & strError, 1)
      End If


      Return intRowsUpdated

   End Function


   Public Function gdtaSqlCeTable1(ByVal strSqlQuery As String) As DataTable

      'origname = gdtaSqlCeTable

      Dim connCon As New SqlCeConnection
      Dim cmd As New SqlCeDataAdapter
      Dim dtaTable As New DataTable()

      Try
         dtaTable.Clear()
         connCon = New SqlCeConnection("Data Source=" & gstrSqlFileLocation)
         cmd = New SqlCeDataAdapter(strSqlQuery, connCon)
         cmd.Fill(dtaTable)
         cmd = Nothing
         'connCon.Close()
      Catch ex As Exception
         gfblnWriteLog("Error trying to connect to SQLCe database", 1)
      End Try

      Return dtaTable

   End Function


   Public Function gintSqlCeExecuteNonQuery1(ByVal strSqlQuery As String) As Integer

      'origname = gintSqlCeExecuteNonQuery
      Dim connCon As New SqlCeConnection
      Dim cmd As SqlCeCommand
      Dim intRowsUpdated As Integer

      Try
         connCon = New SqlCeConnection("Data Source=" & gstrSqlFileLocation)
         cmd = New SqlCeCommand(strSqlQuery, connCon)

         If connCon.State = ConnectionState.Closed Then connCon.Open()
         intRowsUpdated = cmd.ExecuteNonQuery()
         cmd = Nothing
         'connCon.Close()
      Catch ex As Exception
         gfblnWriteLog("Error trying to connect to SQLCe database", 1)
      End Try

      Return intRowsUpdated

   End Function


    Public Sub gsGetRecordSet(ByRef strSqlQuery As String, ByRef rstRecordset As ADODB.Recordset)

        Dim connCon As ADODB.Connection

        Try
            connCon = CreateObject("ADODB.Connection")

            Call connCon.Open("Provider = sqloledb;Data Source=SQLServerName;Initial Catalog=OEE;" & "User ID = " & gstrSqlLoginuser & ";" & "Password = " & gstrSqlLoginPassword & ";")
            rstRecordset = New ADODB.Recordset
            Call rstRecordset.Open(strSqlQuery, connCon, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
            connCon = Nothing
        Catch
            gfblnWriteLog("Error trying to connect to SQL Server 'sql-sinbad-kdn'.", 1)
        End Try

    End Sub


   Public Function gfconvertToADODB(ByRef tblDataTable As DataTable) As ADODB.Recordset

      Dim strError As String

      'gfconvertToADODB will be the recordet
      gfconvertToADODB = Nothing

      Try
         Dim rstResult As New ADODB.Recordset
         Dim resultFields As ADODB.Fields = rstResult.Fields
         Dim col As DataColumn
         Dim intI As Integer

         rstResult.CursorLocation = ADODB.CursorLocationEnum.adUseClient

         For Each col In tblDataTable.Columns
            resultFields.Append(col.ColumnName, mfTranslateType(col.DataType), col.MaxLength, _
                                IIf(col.AllowDBNull, _
                                ADODB.FieldAttributeEnum.adFldIsNullable, vbNull))
         Next

         rstResult.Open()

         For Each row As DataRow In tblDataTable.Rows

            rstResult.AddNew(System.Reflection.Missing.Value, System.Reflection.Missing.Value)

            For intI = 0 To tblDataTable.Columns.Count - 1
               resultFields(intI).Value = row(intI)
            Next
         Next

         Return (rstResult)
         rstResult.Close()

      Catch ex As Exception
         strError = Err.Description
      End Try

   End Function


   Private Function mfTranslateType(ByRef type As Type) As ADODB.DataTypeEnum

      mfTranslateType = Nothing

      Try
         Select Case type.UnderlyingSystemType.ToString
            Case "System.Boolean"
               Return ADODB.DataTypeEnum.adBoolean
            Case "System.Byte"
               Return ADODB.DataTypeEnum.adUnsignedTinyInt
            Case "System.Char"
               Return ADODB.DataTypeEnum.adChar
            Case "System.DateTime"
               Return ADODB.DataTypeEnum.adDate
            Case "System.Decimal"
               Return ADODB.DataTypeEnum.adCurrency
            Case "System.Double"
               Return ADODB.DataTypeEnum.adDouble
            Case "System.Decimal"
               Return ADODB.DataTypeEnum.adDouble
            Case "System.Int16"
               Return ADODB.DataTypeEnum.adSmallInt
            Case "System.Int32"
               Return ADODB.DataTypeEnum.adInteger
            Case "System.Int64"
               Return ADODB.DataTypeEnum.adBigInt
            Case "System.SByte"
               Return ADODB.DataTypeEnum.adTinyInt
            Case "System.Single"
               Return ADODB.DataTypeEnum.adSingle
            Case "System.UInt16"
               Return ADODB.DataTypeEnum.adUnsignedSmallInt
            Case "System.UInt32"
               Return ADODB.DataTypeEnum.adUnsignedInt
            Case "System.UInt64"
               Return ADODB.DataTypeEnum.adUnsignedBigInt
            Case "System.String"
               'case default
               Return ADODB.DataTypeEnum.adVarWChar
         End Select
      Catch ex As Exception
         MsgBox(ex.Message)
      End Try

   End Function


   Public Function gfToDate(ByVal datDate As DateTime) As DateTime

      Try
         gfToDate = Format(datDate, "yyyy-MM-dd HH:mm:ss")
      Catch ex As Exception
         gfToDate = Date.Now
      End Try

   End Function


   Public Sub gsChangePanel(ByVal pnlPanel As Panel, ByVal strFormName As Form)

      strFormName.TopLevel = False
      strFormName.Show()

      pnlPanel.Controls.Clear()
      pnlPanel.Controls.Add(strFormName)

   End Sub


   Function gfblnControlExists(ByVal strControllName As String, ByVal cntParent As Control) As Boolean

      gfblnControlExists = False
      For Each cntElemement As Control In cntParent.Controls
         If cntElemement.Name = strControllName Then
            Return True
            Exit For
         End If
      Next

   End Function


   Public Function gfstrDatToStr(ByVal datDate As Date) As String

      Dim strMonth As String
      Dim strDay As String
      Dim strYear As String
      Dim strHour As String
      Dim strMinutes As String
      Dim strSeconds As String

      If datDate.Month.ToString.Length < 2 Then
         strMonth = "0" & datDate.Month.ToString
      Else
         strMonth = datDate.Month.ToString
      End If

      If datDate.Day.ToString.Length < 2 Then
         strDay = "0" & datDate.Day.ToString
      Else
         strDay = datDate.Day.ToString
      End If

      strYear = datDate.Year

      If datDate.Hour.ToString.Length < 2 Then
         strHour = "0" & datDate.Hour.ToString
      Else
         strHour = datDate.Hour.ToString
      End If

      If datDate.Minute.ToString.Length < 2 Then
         strMinutes = "0" & datDate.Minute.ToString
      Else
         strMinutes = datDate.Minute.ToString
      End If

      If datDate.Second.ToString.Length < 2 Then
         strSeconds = "0" & datDate.Second.ToString
      Else
         strSeconds = datDate.Second.ToString
      End If

      'gfstrDatToStr = strMonth & "/" & strDay & "/" & Now.Year & " " & strHour & ":" & strMinutes & ":" & strSeconds
      gfstrDatToStr = Now.Year & "-" & strMonth & "-" & strDay & " " & strHour & ":" & strMinutes & ":" & strSeconds

   End Function


   Public Function mfblnGarbageCleaner(ByVal strProcess As String) As Boolean

      mfblnGarbageCleaner = False
      Try
         GC.Collect()
         GC.WaitForPendingFinalizers()
         If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1)
            Dim Procs As Process() = Process.GetProcessesByName(strProcess)
            Dim Proc As Process
            For Each Proc In Procs
               SetProcessWorkingSetSize(Proc.Handle, -1, -1)
               mfblnGarbageCleaner = True
            Next Proc
         End If
      Catch ex As Exception
         mfblnGarbageCleaner = False
      End Try

   End Function


   Public Function gfblnScheduledAlerts() As Boolean

      Dim intTimeSpan As TimeSpan
      Dim intMinutes As Integer

      intTimeSpan = garrTeamShift.datEndShift.Subtract(Date.Now)
      intMinutes = (intTimeSpan.Minutes + (intTimeSpan.Hours * 60))

      If intMinutes <= 20 Then
         If gblnGetAllUndefined() Then
            If garrMessageSystem(0).blnPromptedForUndef = False Then
               garrMessageSystem(0).blnPromptedForUndef = True
               Try
                  garrMessageSystem(0).strTitle = "Undefined activities"
                  garrMessageSystem(0).strMessage = "Shift is almost over!" & vbCrLf & _
                                                    "Please define the undefined activities."
                  garrMessageSystem(0).intAction = 77
                  gintSelectedMach = 0
                  frmMessageBox.ShowDialog()
                  frmMessageBox.lblTitle.Text = garrMessageSystem(0).strTitle
                  frmMessageBox.lblMessage.Text = garrMessageSystem(0).strMessage
                  frmMessageBox.Refresh()
               Catch ex As Exception
               End Try
            End If
         End If
      Else
         garrMessageSystem(0).blnPromptedForUndef = False
      End If

   End Function


   Public Function gfblnWriteLog(ByVal strMessage As String, ByVal intEntryType As Integer) As Boolean

      Dim strSource As String
      Dim strLog As String
      Dim strMachine As String
      Dim evtLog As New EventLog

      strSource = "OEECollector"
      strLog = "Application"
      strMachine = "."

      evtLog = New EventLog(strLog, strMachine, strSource)

      Select Case intEntryType
         Case 1
            evtLog.WriteEntry(strMessage, EventLogEntryType.Error, 234, CType(3, Short))
         Case 2
            evtLog.WriteEntry(strMessage, EventLogEntryType.Warning, 234, CType(3, Short))
         Case 4
            evtLog.WriteEntry(strMessage, EventLogEntryType.Information, 234, CType(3, Short))
      End Select

   End Function


   Public Function gfblnCheckOpenTcpPort(ByVal strHost As String, ByVal intPort As Integer) As Boolean

      Dim iasResult As IAsyncResult
      Dim tcpClient As New TcpClient
      Dim tmrConnect As System.Threading.WaitHandle
      Dim blnTrowException As Boolean

      blnTrowException = False
      iasResult = tcpClient.BeginConnect(strHost, intPort, Nothing, Nothing)
      tmrConnect = iasResult.AsyncWaitHandle

      Try
         If iasResult.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(777)) = False Then
            tcpClient.Close()
            Throw New TimeoutException()
         End If
         tcpClient.EndConnect(iasResult)
         blnTrowException = False
      Catch ex As Exception
         blnTrowException = True
      Finally
         tmrConnect.Close()
      End Try

      If blnTrowException Then
         Throw New TimeoutException()
      End If

   End Function


   Public Function gfblnGetDB(ByVal intSelMach As Integer) As Boolean

      Dim client As New WebClient()
      Dim intX As Integer
      Dim strError As String

      strError = ""

      For intX = 1 To 10
         Try
            ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf customCertValidation)
            client.DownloadFile(gstrOEEWebserver & "/default/dblocal.db?wsname=" & My.Computer.Name, _
                                gstrSqlLiteLocation)
            gfblnEncrypt(gstrSqlLiteLocation)
            Return True
            Exit Function
         Catch ex As Exception
            strError = ex.Message
            gfblnGetDB = False
         End Try
         Thread.Sleep(1000)
      Next

      If intX = 10 Then
         gfblnWriteLog("Error trying to connect to remote WebServer (gfblnGetDB)" & vbCrLf & strError, 1)
      End If

   End Function


   Private Function customCertValidation(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chain As X509Chain, ByVal errors As SslPolicyErrors) As Boolean

      Return True

   End Function


   Public Function gfblnSyncData(ByVal intSelMach As Integer) As Boolean

      Dim strSqlQuery As String
      Dim dtaTable As New DataTable()
      Dim datStartDateTime As DateTime
      Dim intRecordCount As Integer
      Dim intFirstReg As Integer
      Dim intLastReg As Integer
      Dim intX As Integer

      datStartDateTime = garrTeamShift.datStartShift
      If datStartDateTime = "0:00:00" Then
         datStartDateTime = Date.Now
         datStartDateTime = datStartDateTime.AddHours(-12)
      End If

      strSqlQuery = "SELECT   * " & _
                    "FROM     tblOee_Reg " & _
                    "WHERE    fldOeeStartDateTime > '" & gfstrDatToStr(datStartDateTime) & "' " & _
                    "AND     (fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & ") " & _
                    "AND     (fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & ") " & _
                    "AND     (fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & ") " & _
                    "AND     (fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ") " & _
                    "AND     (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "');"
      dtaTable = gdtaSqlCeTable(strSqlQuery)

      intRecordCount = dtaTable.Rows.Count
      If intRecordCount > 0 Then
         'get from/to info of RegNr to select the rows that need to be deleted
         intFirstReg = dtaTable.Rows(0).Item(1)
         intLastReg = dtaTable.Rows(intRecordCount - 1).Item(1)
         'delete selected rows
         gfblnDeleteFromWeb(intSelMach, intFirstReg, intLastReg, 1)
         'convert datatable to csv-file
         gfblnDataTableToCSV(dtaTable, My.Application.Info.DirectoryPath & "\Cache\reg.csv")
         'upload  csv-file for insertion
         gfblnCSVUpload(intSelMach, My.Application.Info.DirectoryPath & "\Cache\reg.csv", 1)
         'My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\Cache\reg.csv", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
      End If

      dtaTable.Clear()
      intRecordCount = 0

      strSqlQuery = "SELECT   * " & _
                    "FROM     tblOee_Progress " & _
                    "WHERE   (fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & ") " & _
                    "AND     (fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & ") " & _
                    "AND     (fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & ") " & _
                    "AND     (fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ") " & _
                    "AND     (fldOeeMachineID = '" & garrMachine(intSelMach).intMachineNr & "') " & _
                    "AND     fldOeeSyncDate IS NULL;"
      dtaTable = gdtaSqlCeTable(strSqlQuery)

      intRecordCount = dtaTable.Rows.Count
      If intRecordCount > 0 Then
         gfblnDataTableToCSV(dtaTable, My.Application.Info.DirectoryPath & "\Cache\progress.csv")
         gfblnCSVUpload(intSelMach, My.Application.Info.DirectoryPath & "\Cache\progress.csv", 2)
         'My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\Cache\progress.csv", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)

         For intX = 0 To intRecordCount - 1
            strSqlQuery = "UPDATE tblOee_Progress " & _
                             "SET    fldOeeSyncDate = '" & gfstrDatToStr(Date.Now) & "' " & _
                             "WHERE (fldOeeRegID = '" & dtaTable.Rows(intX).Item(1) & "');"
            gintSqlCeExecuteNonQuery(strSqlQuery)
         Next
      End If

      gfblnUpdate(intSelMach)

   End Function


   Public Function gfblnDataTableToCSV(ByVal dtaTable As DataTable, ByVal strFileName As String) As Boolean

      Dim strInitialSep As String = ""
      Dim strSep As String = ","
      Dim writer As System.IO.StreamWriter
      Dim builder As New System.Text.StringBuilder
      Dim strFieldValue As String
      Dim intCount As Integer
      Dim datDate As DateTime

      writer = New System.IO.StreamWriter(strFileName)

      intCount = dtaTable.Rows.Count
      If intCount > 0 Then
         For Each col As DataColumn In dtaTable.Columns
            builder.Append(strInitialSep).Append(col.ColumnName.ToString)
            strInitialSep = strSep
         Next
         writer.WriteLine(builder.ToString())

         For Each row As DataRow In dtaTable.Rows
            builder = New System.Text.StringBuilder
            strInitialSep = ""

            For Each col As DataColumn In dtaTable.Columns
               strFieldValue = IIf(IsDBNull(row(col.ColumnName)), "", row(col.ColumnName))
               If Date.TryParse(strFieldValue, datDate) Then
                  builder.Append(strInitialSep).Append(gfstrDatToStr(strFieldValue))
               Else
                  builder.Append(strInitialSep).Append(strFieldValue)
               End If
               strInitialSep = strSep
            Next
            writer.WriteLine(builder.ToString())
         Next
         If Not writer Is Nothing Then writer.Close()

      End If

   End Function


   Public Function gfblnCSVUpload(ByVal intSelMach As Integer, ByVal strFilename As String, ByVal intKindNr As Integer) As Boolean

      Dim client2 As New WebClient()
      Dim strError As String = ""
      Dim blnError As Boolean


      Try
         ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf customCertValidation)
         client2.UploadFile(gstrOEEWebserver & "/default/api/post" &
                         "?kind=" & intKindNr &
                         "&machineid=" & garrMachine(intSelMach).intMachineNr &
                         "&countrynr=" & garrMachine(intSelMach).intCountryNr &
                         "&plantnr=" & garrMachine(intSelMach).intPlantNr &
                         "&subplantnr=" & garrMachine(intSelMach).intSubPlantNr &
                         "&departmentnr=" & garrMachine(intSelMach).intDepartmentNr,
                         strFilename)
         blnError = False
         IO.File.Delete(strFilename)
      Catch ex As Exception
         strError = ex.Message
         blnError = True
      End Try

      If blnError Then
         gfblnWriteLog("Error trying to connect to remote WebServer (gfblnCSVUpload)" & vbCrLf & strError, 1)
      End If

   End Function


   Public Function gfblnCSVDownload(ByVal intSelMach As Integer) As Boolean

      Dim client As New WebClient()
      Dim strError As String
      Dim dtaDataTable As DataTable
      Dim strText As String
      Dim strCSV As String

      Try
         ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf customCertValidation)
         strCSV = client.DownloadString(gstrOEEWebserver & _
                             "/default/api.csv/get" & _
                             "?machineid=" & garrMachine(intSelMach).intMachineNr & _
                             "&countrynr=" & garrMachine(intSelMach).intCountryNr & _
                             "&plantnr=" & garrMachine(intSelMach).intPlantNr & _
                             "&subplantnr=" & garrMachine(intSelMach).intSubPlantNr & _
                             "&departmentnr=" & garrMachine(intSelMach).intDepartmentNr)
         dtaDataTable = gfdtaCsvStringToDatatable(strCSV, ",")
         strText = dtaDataTable.Rows(0).Item(3)
      Catch ex As Exception
         strError = ex.Message
      End Try

   End Function


   Public Function gfdtaCsvStringToDatatable(ByVal strCSVData As String, ByVal strSeparator As String) As DataTable

      Dim dtaTable As New DataTable
      Dim dtrDataRow As DataRow
      Dim blnHeader As Boolean
      Dim strColumn As String
      Dim intX As Integer
      Dim intY As Integer

      blnHeader = True

      Dim strRows() = strCSVData.Split({Environment.NewLine}, StringSplitOptions.None)

      For intX = 0 To strRows(0).Split(strSeparator).Length - 1
         strColumn = strRows(0).Split(strSeparator)(intX)
         dtaTable.Columns.Add(If(blnHeader, strColumn, "column" & intX + 1))
      Next

      For intX = If(blnHeader, 1, 0) To strRows.Length - 1
         dtrDataRow = dtaTable.NewRow
         For intY = 0 To strRows(intX).Split(strSeparator).Length - 1
            If intY <= dtaTable.Columns.Count - 1 Then
               dtrDataRow(intY) = strRows(intX).Split(strSeparator)(intY)
            Else
               Throw New Exception("The number of columns on row " & intX + If(blnHeader, 0, 1) & _
                                   " is greater than the amount of columns in the " & If(blnHeader, "header.", "first row."))
            End If
         Next
         dtaTable.Rows.Add(dtrDataRow)
      Next
      Return dtaTable

   End Function


   Public Function gfblnDeleteFromWeb(ByVal intSelMach As Integer, ByVal intRegFrom As Integer, ByVal intRegTo As Integer, ByVal intKindNr As Integer) As Boolean

      Dim client2 As New WebClient()
      Dim strCSV As String

      'defining gaat goed, maar bij een nieuwe import wordt het niet de ene laatse verwijderd, dus dubbele entries.


      Try
         ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf customCertValidation)
         strCSV = client2.DownloadString(gstrOEEWebserver & _
                             "/default/api.csv/delete" & _
                             "?kind=" & intKindNr & _
                             "&machineid=" & garrMachine(intSelMach).intMachineNr & _
                             "&countrynr=" & garrMachine(intSelMach).intCountryNr & _
                             "&plantnr=" & garrMachine(intSelMach).intPlantNr & _
                             "&subplantnr=" & garrMachine(intSelMach).intSubPlantNr & _
                             "&departmentnr=" & garrMachine(intSelMach).intDepartmentNr & _
                             "&regfrom=" & intRegFrom & _
                             "&regto=" & intRegTo)
         Return True
      Catch ex As Exception
         gfblnWriteLog("Error trying to connect to remote WebServer (gfblnDeleteFromWeb)" & vbCrLf & ex.Message, 1)
         Return False
      End Try

   End Function


   Public Function gfintGetRandom(ByVal intMin As Integer, ByVal intMax As Integer) As Integer

      Dim Generator As System.Random = New System.Random()
      Return Generator.Next(intMin, intMax)

   End Function


   Public Function gfblnUpdate(ByVal intSelMach As Integer) As Boolean

      'needs to be called directly after sync
      'otherwise difference is detected via Reg and Progress

      Dim intCountCountry As Integer
      Dim intCountPlant As Integer
      Dim intCountSubPlant As Integer
      Dim intCountDepartment As Integer
      Dim intCountActivityGroup As Integer
      Dim intCountActivity As Integer
      Dim intCountModuleSensorStyle As Integer
      Dim intCountModuleType As Integer
      Dim intCountModule As Integer
      Dim intCountMachineIOFailure As Integer
      Dim intCountMachineShortbreak As Integer
      Dim intCountMachineStatus As Integer
      Dim intCountMachineUndefinedProduction As Integer
      Dim intCountMachineUndefinedStandstill As Integer
      Dim intCountMachineUnscheduled As Integer
      Dim intCountMachineUnit As Integer
      Dim intCountMachine As Integer
      Dim intCountMachineActivity As Integer
      Dim intCountDailySchedule As Integer
      Dim intCountArticle As Integer
      Dim intCountOrder As Integer
      Dim intCountShiftTime As Integer
      Dim intCountTeam As Integer
      Dim intCountReg As Integer
      Dim intCountProgress As Integer

      Dim strSqlQuery As String
      Dim dtaTable As DataTable
      Dim strUpdateUrl As String
      Dim client As New WebClient()
      Dim intUpdate As Integer

      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Country " & _
                    "WHERE  fldOeeCountryNr = " & garrMachine(intSelMach).intCountryNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountCountry = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Plant " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantNr = " & garrMachine(intSelMach).intPlantNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountPlant = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_SubPlant " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantNr = " & garrMachine(intSelMach).intSubPlantNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountSubPlant = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Department " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentNr = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountDepartment = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_ActivityGroup " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountActivityGroup = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Activity " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountActivity = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_ModuleSensorStyle " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountModuleSensorStyle = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_ModuleType " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountModuleType = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Module " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountModule = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineIOFailure " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineIOFailure = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineShortbreak " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineShortbreak = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineStatus " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineStatus = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM tblOee_MachineUndefinedProduction " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineUndefinedProduction = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineUndefinedStandstill " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineUndefinedStandstill = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineUnscheduled " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineUnscheduled = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineUnit " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineUnit = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Machine " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & " " & _
                    "AND    fldOeeMachineNr = " & garrMachine(intSelMach).intMachineNr & ";"

      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachine = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_MachineActivity " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & " " & _
                    "AND    fldOeeMachineID = " & garrMachine(intSelMach).intMachineNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountMachineActivity = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_DailySchedule " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountDailySchedule = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Article " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountArticle = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Order " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountOrder = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_ShiftTime " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountShiftTime = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Team " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountTeam = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Reg " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & " " & _
                    "AND    fldOeeMachineID = " & garrMachine(intSelMach).intMachineNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountReg = dtaTable.Rows.Count

      dtaTable.Clear()
      strSqlQuery = "SELECT * " & _
                    "FROM   tblOee_Progress " & _
                    "WHERE  fldOeeCountryID = " & garrMachine(intSelMach).intCountryNr & " " & _
                    "AND    fldOeePlantID = " & garrMachine(intSelMach).intPlantNr & " " & _
                    "AND    fldOeeSubPlantID = " & garrMachine(intSelMach).intSubPlantNr & " " & _
                    "AND    fldOeeDepartmentID = " & garrMachine(intSelMach).intDepartmentNr & " " & _
                    "AND    fldOeeMachineID = " & garrMachine(intSelMach).intMachineNr & ";"
      dtaTable = gdtaSqlCeTable(strSqlQuery)
      intCountProgress = dtaTable.Rows.Count

      strUpdateUrl = "?countrynr=" & garrMachine(intSelMach).intCountryNr & _
                     "&plantnr=" & garrMachine(intSelMach).intPlantNr & _
                     "&subplantnr=" & garrMachine(intSelMach).intSubPlantNr & _
                     "&departmentnr=" & garrMachine(intSelMach).intDepartmentNr & _
                     "&machineid=" & garrMachine(intSelMach).intMachineNr & _
                     "&countcountry=" & intCountCountry & _
                     "&countplant=" & intCountPlant & _
                     "&countsubplant=" & intCountSubPlant & _
                     "&countdepartment=" & intCountDepartment & _
                     "&countactivitygroup=" & intCountActivityGroup & _
                     "&countactivity=" & intCountActivity & _
                     "&countmodulesensorstyle=" & intCountModuleSensorStyle & _
                     "&countmoduletype=" & intCountModuleType & _
                     "&countmodule=" & intCountModule & _
                     "&countmachineiofailure=" & intCountMachineIOFailure & _
                     "&countmachineshortbreak=" & intCountMachineShortbreak & _
                     "&countmachinestatus=" & intCountMachineStatus & _
                     "&countmachineundefinedproduction=" & intCountMachineUndefinedProduction & _
                     "&countmachineundefinedstandstill=" & intCountMachineUndefinedStandstill & _
                     "&countmachineunscheduled=" & intCountMachineUnscheduled & _
                     "&countmachineunit=" & intCountMachineUnit & _
                     "&countmachine=" & intCountMachine & _
                     "&countmachineactivity=" & intCountMachineActivity & _
                     "&countdailyschedule=" & intCountDailySchedule & _
                     "&countarticle=" & intCountArticle & _
                     "&countorder=" & intCountOrder & _
                     "&countshifttime=" & intCountShiftTime & _
                     "&countteam=" & intCountTeam & _
                     "&countreg=" & intCountReg & _
                     "&countprogress=" & intCountProgress

      Dim strError As String


      Try
         ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf customCertValidation)
         intUpdate = client.DownloadString(gstrOEEWebserver & "/default/update" & _
                                           strUpdateUrl)
         If intUpdate = 1 Then
            'MsgBox("An update is pending." & vbCrLf & _
            '       "Jeden OEE Collector will restart now!!!", MsgBoxStyle.Information, "Update pending")
            gfblnUpdateReg(intSelMach, garrMachine(intSelMach).intCurrStatus)
            gfblnAddReg(intSelMach, 0, 0, "Update", "Update pending", False, 0, 4)
            Application.Restart()
         End If
      Catch ex As Exception
         strError = ex.Message
      End Try

   End Function

End Module
