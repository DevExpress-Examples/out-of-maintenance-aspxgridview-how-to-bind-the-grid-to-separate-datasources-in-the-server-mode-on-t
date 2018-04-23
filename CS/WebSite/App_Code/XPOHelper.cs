using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.DB.Cte;
using System.Xml;

    public class XPOHelper
    {
        UnitOfWork uow;
        IDataLayer dal;
        String strCTEName = "";
        System.Web.UI.Page page;
        public XPServerCollectionSource GetXCS(string sServerName, string sUserName, string sPassword, string sDBName, string sSql, string sColumnXML,
            System.Web.UI.Page oPage, bool p_bHasKeyField)
        {
            XPServerCollectionSource l_xcs = null;
            page = oPage;
            page.Unload += new EventHandler(Page_Unload);
            strCTEName = "CTE_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();
            //Use your own GetConnectionString method based on the passed parameters
            string connectionString = MSSqlConnectionProviderWithCte.GetConnectionString(sServerName, sDBName);
            XPDictionary dict = new ReflectionDictionary();
            XPClassInfo classInfo = dict.CreateClass(dict.GetClassInfo(typeof(BasePersistentClass)), strCTEName);

            XmlDocument l_xmlDoc = new XmlDocument();
            l_xmlDoc.LoadXml(sColumnXML);
            XmlNodeList l_xnl = l_xmlDoc.SelectNodes("//FLD");
            for (int i = 0; i < l_xnl.Count; i++)
            {
                Type l_type = null;
                switch (l_xnl[i]["DATTYP"].InnerText.ToLower())
                {
                    case "int64":
                        l_type = typeof(Int64);
                        break;
                    case "byte[]":
                        l_type = typeof(Byte[]);
                        break;
                    case "boolean":
                        l_type = typeof(Boolean);
                        break;
                    case "string":
                        l_type = typeof(String);
                        break;
                    case "char[]":
                        l_type = typeof(Char[]);
                        break;
                    case "datetime":
                        l_type = typeof(DateTime);
                        break;
                    case "datetimeoffset":
                        l_type = typeof(DateTimeOffset);
                        break;
                    case "decimal":
                        l_type = typeof(Decimal);
                        break;
                    case "double":
                        l_type = typeof(Double);
                        break;
                    case "int32":
                        l_type = typeof(Int32);
                        break;
                    case "single":
                        l_type = typeof(Single);
                        break;
                    case "int16":
                        l_type = typeof(Int16);
                        break;
                    case "object":
                        l_type = typeof(Object);
                        break;
                    case "timeSpan":
                        l_type = typeof(TimeSpan);
                        break;
                    case "byte":
                        l_type = typeof(Byte);
                        break;
                    case "guid":
                        l_type = typeof(Guid);
                        break;
                    case "xml":
                        l_type = typeof(XmlDocument);
                        break;
                }
                
                if (i == 0)
                    classInfo.CreateMember(l_xnl[i]["COL"].InnerText, l_type, new KeyAttribute(true));
                else
                    classInfo.CreateMember(l_xnl[i]["COL"].InnerText, l_type);
            }

            dal = new SimpleDataLayer(dict, XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists));
            uow = new UnitOfWork(dal);
            string l_strQuery = "";
            l_strQuery = " AS (" + sSql + ")";
            uow.RegisterCte(strCTEName, @l_strQuery);
            try
            {

                l_xcs = new XPServerCollectionSource(uow, classInfo);
                return l_xcs;
            }
            finally
            {

            }
            return l_xcs;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            uow.UnregisterCte(strCTEName);
            uow.Dispose();
            dal.Dispose();
            page.Unload -= new EventHandler(Page_Unload);
        }

        [NonPersistent]
        public class BasePersistentClass : XPLiteObject
        {
            public BasePersistentClass(Session session) : base(session) { }
            public BasePersistentClass(Session session, XPClassInfo classInfo) : base(session, classInfo) { }
        }
    }
