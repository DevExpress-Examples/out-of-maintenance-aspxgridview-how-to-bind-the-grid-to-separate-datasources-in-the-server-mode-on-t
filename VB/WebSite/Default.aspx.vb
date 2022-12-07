Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.OleDb
Imports System.Data

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		If Session("currentSource") IsNot Nothing Then
			Select Case Session("currentSource").ToString()
				Case "Datasource1"
					BindToDS1()
				Case "Datasource2"
					BindToDS2()
			End Select
		End If
	End Sub
	Private Function GetFieldInfoFromQuery(ByVal p_strQuery As String, ByVal p_strConnectionString As String) As String
		Dim l_adapterSchema As New OleDbDataAdapter(p_strQuery, p_strConnectionString)
		Dim g_dtSchema As New DataTable()
		l_adapterSchema.FillSchema(g_dtSchema, SchemaType.Source)
		Dim l_strColumnSchema As String = "<FLDINF>"
		For Each l_dcolum As DataColumn In g_dtSchema.Columns
			l_strColumnSchema = String.Concat(l_strColumnSchema, "<FLD>")
			l_strColumnSchema = String.Concat(l_strColumnSchema, "<COL>", l_dcolum.ColumnName, "</COL>")
			l_strColumnSchema = String.Concat(l_strColumnSchema, "<DATTYP>", l_dcolum.DataType.ToString().ToLower().Replace("system.", ""), "</DATTYP>")
			l_strColumnSchema = String.Concat(l_strColumnSchema, "</FLD>")
		Next l_dcolum
		l_strColumnSchema &= "</FLDINF>"
		Return l_strColumnSchema
	End Function

	Private Sub BindToDS1()
		BindGrid("SELECT * FROM [TABLE]", "DATABASE_NAME")
	End Sub
	Private Sub BindToDS2()
		BindGrid("SELECT * FROM [TABLE]", "DATABASE2_NAME")
	End Sub
	Protected Sub ASPxGridView1_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
		If e.Parameters = "1" Then
			Session("currentSource") = "Datasource1"
			BindToDS1()
		End If
		If e.Parameters = "2" Then
			Session("currentSource") = "Datasource2"
			BindToDS2()
		End If
	End Sub
	Private Sub BindGrid(ByVal sqlQuery As String, ByVal DBName As String)
		Dim connectionString As String = String.Format("Provider=SQLOLEDB;Server=servername;Initial Catalog = {0};Integrated security = SSPI;", DBName)
		'configure your connection string

		Dim helper As New XPOHelper()
		Dim columnSchema As String = GetFieldInfoFromQuery(sqlQuery, connectionString)
		Dim collection As DevExpress.Xpo.XPServerCollectionSource = helper.GetXCS("servername", "username", "password", DBName, sqlQuery, columnSchema, Me.Page, True)
		ASPxGridView1.Columns.Clear()
		ASPxGridView1.AutoGenerateColumns = True
		ASPxGridView1.DataSource = collection
		ASPxGridView1.DataBind()
	End Sub
End Class