<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TemplateData.aspx.cs" Inherits="EMR_Dashboard_PatientParts_TemplateData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" width="100%" cellpadding="0" cellspacing="1">
            <tr>
                <td align="center" style="color: green; font-size: 12px; font-weight: bold;">
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel4" runat="server" Style="border-width: 1px; border-color: LightBlue;
            border-style: solid;" Width="99%" Height="550px" ScrollBars="Auto">
            <table border="0" width="99%" cellpadding="0" cellspacing="1" style="margin-left: 2px">
                <tr>
                    <td id="tdTemplate" runat="server">
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
