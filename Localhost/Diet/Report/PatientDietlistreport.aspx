<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientDietlistreport.aspx.cs"
    Inherits="Diet_Report_PatientDietlistreport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Diet Sheet</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnClose" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />
                </td>
            </tr>
            <tr>
                <td>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="500px" Width="99%">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
