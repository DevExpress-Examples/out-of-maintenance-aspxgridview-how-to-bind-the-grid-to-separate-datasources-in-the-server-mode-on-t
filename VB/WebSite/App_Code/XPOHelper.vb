Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata
Imports DevExpress.Xpo.DB.Cte
Imports System.Xml

	Public Class XPOHelper
		Private uow As UnitOfWork
		Private dal As IDataLayer
		Private strCTEName As String = ""
		Private page As System.Web.UI.Page
		Public Function GetXCS(ByVal sServerName As String, ByVal sUserName As String, ByVal sPassword As String, ByVal sDBName As String, ByVal sSql As String, ByVal sColumnXML As String, ByVal oPage As System.Web.UI.Page, ByVal p_bHasKeyField As Boolean) As XPServerCollectionSource
			Dim l_xcs As XPServerCollectionSource = Nothing
			page = oPage
			AddHandler page.Unload, AddressOf Page_Unload
			strCTEName = "CTE_" & DateTime.Now.Hour.ToString() & DateTime.Now.Minute.ToString() & DateTime.Now.Millisecond.ToString()
			'Use your own GetConnectionString method based on the passed parameters
			Dim connectionString As String = MSSqlConnectionProviderWithCte.GetConnectionString(sServerName, sDBName)
			Dim dict As XPDictionary = New ReflectionDictionary()
			Dim classInfo As XPClassInfo = dict.CreateClass(dict.GetClassInfo(GetType(BasePersistentClass)), strCTEName)

			Dim l_xmlDoc As New XmlDocument()
			l_xmlDoc.LoadXml(sColumnXML)
			Dim l_xnl As XmlNodeList = l_xmlDoc.SelectNodes("//FLD")
			For i As Integer = 0 To l_xnl.Count - 1
				Dim l_type As Type = Nothing
				Select Case l_xnl(i)("DATTYP").InnerText.ToLower()
					Case "int64"
						l_type = GetType(Int64)
					Case "byte[]"
						l_type = GetType(Byte())
					Case "boolean"
						l_type = GetType(Boolean)
					Case "string"
						l_type = GetType(String)
					Case "char[]"
						l_type = GetType(Char())
					Case "datetime"
						l_type = GetType(DateTime)
					Case "datetimeoffset"
						l_type = GetType(DateTimeOffset)
					Case "decimal"
						l_type = GetType(Decimal)
					Case "double"
						l_type = GetType(Double)
					Case "int32"
						l_type = GetType(Int32)
					Case "single"
						l_type = GetType(Single)
					Case "int16"
						l_type = GetType(Int16)
					Case "object"
						l_type = GetType(Object)
					Case "timeSpan"
						l_type = GetType(TimeSpan)
					Case "byte"
						l_type = GetType(Byte)
					Case "guid"
						l_type = GetType(Guid)
					Case "xml"
						l_type = GetType(XmlDocument)
				End Select

				If i = 0 Then
					classInfo.CreateMember(l_xnl(i)("COL").InnerText, l_type, New KeyAttribute(True))
				Else
					classInfo.CreateMember(l_xnl(i)("COL").InnerText, l_type)
				End If
			Next i

			dal = New SimpleDataLayer(dict, XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists))
			uow = New UnitOfWork(dal)
			Dim l_strQuery As String = ""
			l_strQuery = " AS (" & sSql & ")"
			uow.RegisterCte(strCTEName, l_strQuery)
			Try

				l_xcs = New XPServerCollectionSource(uow, classInfo)
				Return l_xcs
			Finally

			End Try
			Return l_xcs
		End Function

		Protected Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs)
			uow.UnregisterCte(strCTEName)
			uow.Dispose()
			dal.Dispose()
			RemoveHandler page.Unload, AddressOf Page_Unload
		End Sub

		<NonPersistent> _
		Public Class BasePersistentClass
			Inherits XPLiteObject
			Public Sub New(ByVal session As Session)
				MyBase.New(session)
			End Sub
			Public Sub New(ByVal session As Session, ByVal classInfo As XPClassInfo)
				MyBase.New(session, classInfo)
			End Sub
		End Class
	End Class
