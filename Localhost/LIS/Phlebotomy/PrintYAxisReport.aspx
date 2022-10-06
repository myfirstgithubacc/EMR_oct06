<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintYAxisReport.aspx.cs" Inherits="LIS_Phlebotomy_PrintYAxisReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <rsweb:reportviewer id="ReportViewer1" runat="server" height="550px" showprintbutton="true"
            width="100%">
        </rsweb:reportviewer>
    </div>
    </form>
</body>
</html>
