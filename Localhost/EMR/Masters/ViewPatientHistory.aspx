<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPatientHistory.aspx.cs"
    Inherits="EMR_Masters_ViewPatientHistory" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table cellpadding="0" cellspacing="0" class="clsheader" width="100%">
            <tr>
                <td>
                    View History
                </td>
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print" OnClick="btnPrint_Click" />&nbsp;
                            <asp:Button ID="btnClose" runat="server" SkinID="Button" Text="Close" OnClientClick="window.close();" Visible="false"/>&nbsp;
                            &nbsp;
                                                 <asp:Button ID="btnClosePopup" Text="Back" ToolTip="Back" SkinID="Button" runat="server"
                                                 OnClick="btnClosePopup_OnClick" Visible="false" BackColor="#FFCBA4" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table border="0" width="98%" cellpadding="3" cellspacing="0" style="margin-left: 8px;
            margin-right: 8px">
            <tr>
                <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="2">
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="From" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" SkinID="label" Text="To" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="Template" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlTemplatePatient" runat="server" EmptyMessage="[ Select ]"
                                    Width="250px" MarkFirstMatch="true" />
                                <%-- Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplatePatient_SelectedIndexChanged" --%>
                            </td>
                            <td>
                                <asp:Button ID="btnRefreshData" runat="server" Text="Refresh" SkinID="button" OnClick="btnRefreshData_OnClick" />
                            </td>
                        </tr>
         </table>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <telerik:RadEditor ID="editorBody" runat="server" Height="500px" EditModes="Preview"
                                Width="100%" ToolsFile="~/Include/XML/Visiting.xml" BorderWidth="0" BorderStyle="Solid"
                                ToolbarMode="Default">
                                <CssFiles>
                                <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                            </CssFiles>
                            <SpellCheckSettings AllowAddCustom="true" />
                            <ImageManager ViewPaths="~/medical_illustration" />
                            </telerik:RadEditor>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Skin="Metro" Behaviors="Maximize">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Maximize">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hdnSignedId" runat="server" />
            <asp:HiddenField ID="hdnUnSignedId" runat="server" />
            <asp:HiddenField ID="hdnImagePath" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
