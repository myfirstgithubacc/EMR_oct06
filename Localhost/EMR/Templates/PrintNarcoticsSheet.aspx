<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintNarcoticsSheet.aspx.cs" Inherits="EMR_Templates_PrintNarcoticsSheet" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="99%" Height="500">
                </rsweb:ReportViewer>
    </div>
    </form>

</body>
</html>
