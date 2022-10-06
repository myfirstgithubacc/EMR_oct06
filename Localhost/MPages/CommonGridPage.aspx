<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonGridPage.aspx.cs" Inherits="MPage_CommonGridPage"
    Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Details</title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>

    <script type="text/javascript">

        function SearchPatientOnClientClose(oWnd, args) {

            $get('<%=ibtnShowDetails.ClientID%>').click();
        }
    </script>

    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="scriptmgr1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upMainPannel" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" AutoSizeBehaviors="Width, Height"
                                AutoSize="true" KeepInScreenBounds="false" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <div class="container-fluid text-center bg-warning form-group">
                                    <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding: 5px 0;">
                                <asp:Button ID="btnNewAlert" runat="server" Text="Add Alert" OnClick="btnNewAlert_OnClick" />
                                <asp:Button ID="ibtnClose" runat="server" Text="Close" ToolTip="Close" CssClass="btn btn-xs btn-primary" OnClientClick="window.close();" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table id="tblHelp" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td id="tdhelp" valign="top" style="width: 100%;">
                                            <asp:UpdatePanel ID="udpDetails" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvDetails" HeaderStyle-CssClass="bg-info" CellPadding="2" runat="server" AutoGenerateColumns="false"
                                                        ShowHeader="true" Width="100%" PageSize="15" AllowPaging="true" PagerSettings-Mode="NumericFirstLast"
                                                        ShowFooter="false" PagerSettings-Visible="true" HeaderStyle-HorizontalAlign="Left"
                                                        OnRowDataBound="gvDetails_OnRowDataBound" CssClass="table table-condensed table-bordered">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Patient Alerts" ItemStyle-Width="55%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPatientAlert" runat="server" SkinID="label" Text='<%#Eval("Patient Alerts") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="45%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarks" runat="server" SkinID="label" Text='<%#Eval("Remarks") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:Button ID="ibtnShowDetails" runat="server" OnClick="ibtnShowDetails_OnClick"
                                    Style="visibility: hidden;" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
