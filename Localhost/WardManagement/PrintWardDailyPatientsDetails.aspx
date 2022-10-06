<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintWardDailyPatientsDetails.aspx.cs" Inherits="WardManagement_PrintWardDailyPatientsDetails" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>
        <div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="550px" Width="100%">
            </rsweb:ReportViewer>
        </div>
    </div>
    </form>
</body>
</html>
