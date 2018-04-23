<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v12.1, Version=12.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v12.1, Version=12.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" OnCustomCallback="ASPxGridView1_CustomCallback" EnableViewState="false"
            ClientInstanceName="grid">
        </dx:ASPxGridView>
        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Bind to the first database"
            AutoPostBack="false">
            <ClientSideEvents Click="function (s,e) { grid.PerformCallback('1'); }" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Bind to the second database"
            AutoPostBack="false">
            <ClientSideEvents Click="function (s,e) { grid.PerformCallback('2'); }" />
        </dx:ASPxButton>
    </div>
    </form>
</body>
</html>
