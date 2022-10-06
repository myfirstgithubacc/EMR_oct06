<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppointmentSlip.aspx.cs"
    Inherits="EMRReports_AppointmentSlip" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <asp:HiddenField ID="hdnReportTitle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnHtmlBody" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnDocumentNo" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnDocumentDate" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnRegistrationId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnEncounterId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnSponsorId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnShowBorder" runat="server" Value="True"></asp:HiddenField>
    <rsweb:reportviewer id="ReportViewer1" runat="server" height="550px" width="100%">
        </rsweb:reportviewer>
    </form>
</body>
</html>
