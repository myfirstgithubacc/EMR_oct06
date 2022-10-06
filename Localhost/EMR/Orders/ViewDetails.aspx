<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewDetails.aspx.cs" Inherits="ViewDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Details</title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upMainPannel" runat="server">
            <ContentTemplate>
                <table id="tblMain" runat="server" width="100%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" Font-Bold="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:LinkButton ID="lnlAcknowlege" runat="server" Text="Acknowlege" OnClick="lnkViewServiceAck_OnClick"
                                Visible="false" />
                        </td>
                        <td align="right">
                            <asp:Button ID="ibtnClose" runat="server" AccessKey="C" SkinID="Button" Text="Close"
                                ToolTip="Close" OnClientClick="window.close();" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Xml ID="xmlPatientInfo" runat="server"></asp:Xml>
                            <%-- <span style="color: Red">*</span>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Service Name" />
                        </td>
                        <td>
                            <asp:Label ID="lblServName" runat="server" SkinID="label" />
                            <asp:HiddenField ID="hdnServiceID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblTemplate2" runat="server" SkinID="label" Text="Template" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTemplateMain" runat="server" EmptyMessage="[ Select ]"
                                Width="250px" Filter="Contains" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateMain_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%--<telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007"
                            Width="100%" Height="450px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                            ToolsFile="~/Include/XML/WordProcessorEditorTools.xml" OnClientSelectionChange="OnClientSelectionChange"
                            OnClientLoad="OnClientEditorLoad" OnClientPasteHtml="OnClientPasteHtml">
                            
                            <CssFiles>
                                <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                            </CssFiles>
                            <SpellCheckSettings AllowAddCustom="true" />
                            <ImageManager ViewPaths="~/medical_illustration" />
                        </telerik:RadEditor>--%>
                            <div style="border-width: thin thin thin thin; border-color: Gray; border-style: solid;
                                overflow: scroll; height: 400px;">
                                <asp:Literal ID="literal" runat="server"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblColorCodeForMandatoryTemplate" runat="server" BorderWidth="1px"
                                Text="&nbsp;" Width="20px" />
                            <asp:Label ID="Label14" runat="server" SkinID="label" Text="Investigation Specification(s) Optional"></asp:Label>
                            <asp:Label ID="lblColorCodeForTemplateRequired" runat="server" BorderWidth="1px"
                                Text="&nbsp;" Width="20px" />
                            <asp:Label ID="Label12" runat="server" SkinID="label" Text="Investigation Specification(s) Mandatory"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTemplateMain" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
