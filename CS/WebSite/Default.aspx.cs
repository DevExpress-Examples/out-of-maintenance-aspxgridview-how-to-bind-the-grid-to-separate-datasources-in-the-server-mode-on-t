using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["currentSource"] != null) {
            switch (Session["currentSource"].ToString()) {
                case "Datasource1":
                    BindToDS1();                    
                    break;
                case "Datasource2":
                    BindToDS2();
                    break;
            }
        }
    }
    private string GetFieldInfoFromQuery(string p_strQuery, string p_strConnectionString) {
        OleDbDataAdapter l_adapterSchema = new OleDbDataAdapter(p_strQuery, p_strConnectionString);
        DataTable g_dtSchema = new DataTable();
        l_adapterSchema.FillSchema(g_dtSchema, SchemaType.Source);
        string l_strColumnSchema = "<FLDINF>";
        foreach (DataColumn l_dcolum in g_dtSchema.Columns) {
            l_strColumnSchema = string.Concat(l_strColumnSchema, "<FLD>");
            l_strColumnSchema = string.Concat(l_strColumnSchema, "<COL>", l_dcolum.ColumnName, "</COL>");
            l_strColumnSchema = string.Concat(l_strColumnSchema, "<DATTYP>", l_dcolum.DataType.ToString().ToLower().Replace("system.", ""), "</DATTYP>");
            l_strColumnSchema = string.Concat(l_strColumnSchema, "</FLD>");
        }
        l_strColumnSchema += "</FLDINF>";
        return l_strColumnSchema;
    }
    
    private void BindToDS1() {
        BindGrid("SELECT * FROM [TABLE]", "DATABASE_NAME");
    }
    private void BindToDS2() {
        BindGrid("SELECT * FROM [TABLE]", "DATABASE2_NAME");
    }
    protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e) {
        if (e.Parameters == "1") {
            Session["currentSource"] = "Datasource1";
            BindToDS1();
        }
        if (e.Parameters == "2") {
            Session["currentSource"] = "Datasource2";
            BindToDS2();
        }
    }
    private void BindGrid(string sqlQuery, string DBName) {
        string connectionString = String.Format("Provider=SQLOLEDB;Server=servername;Initial Catalog = {0};Integrated security = SSPI;", DBName);
        //configure your connection string

        XPOHelper helper = new XPOHelper();        
        string columnSchema = GetFieldInfoFromQuery(sqlQuery, connectionString);
        DevExpress.Xpo.XPServerCollectionSource collection = helper.GetXCS("servername", "username", "password", DBName, sqlQuery, columnSchema, this.Page, true);
        ASPxGridView1.Columns.Clear();
        ASPxGridView1.AutoGenerateColumns = true;
        ASPxGridView1.DataSource = collection;
        ASPxGridView1.DataBind();
    }
}