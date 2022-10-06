<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Comments.aspx.cs" Inherits="LIS_Phlebotomy_Comments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Comments</title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <table border="0" cellpadding="1" cellspacing="1" width="100%">
            <tr height="25px">
                <td class="clssubtopic">
                    <asp:Label ID="lblPatientDetails" runat="server" SkinID="label" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="padding-left: 5px; padding-bottom: 5px;">
                    
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                        <asp:Label ID="lbl1" runat="server" Text="Sub Department" SkinID="label"></asp:Label>&nbsp;
                            <telerik:RadComboBox ID="ddlSubDepartment" runat="server" MarkFirstMatch="true" Width="400px"
                                EmptyMessage="[Select Sub Department]" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged">
                            </telerik:RadComboBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <telerik:RadEditor ID="txtRemarks" runat="server" EditModes="Design" EnableEmbeddedSkins="true"
                                EnableResize="true" Skin="Vista" Height="300px" Width="98%" ToolbarMode="Default"
                                ToolsFile="~/Include/XML/EditorTools.xml">
                                <Modules>
                                    <telerik:EditorModule Visible="false" />
                                </Modules>
                            </telerik:RadEditor>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" height="35px">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Click to save the comments"
                                SkinID="Button" OnClick="btnSave_OnClick" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" SkinID="Button" ToolTip="Click to cancel the comments."
                                OnClick="btnCancel_OnClick" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Click to close the window"
                                SkinID="Button" OnClick="btnClose_OnClick" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
