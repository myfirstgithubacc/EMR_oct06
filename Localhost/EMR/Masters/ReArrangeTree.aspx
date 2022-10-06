<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReArrangeTree.aspx.cs" Inherits="EMR_Masters_ReArrangeTree"
    Theme="DefaultControls" Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body onunload="if($get('hdn1').value!=''){window.opener.document.forms(0).submit();window.close();}">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:HiddenField ID="hdn1" runat="server" Value="" />
    <div>
        <table>
            <tr>
                <td valign="bottom">
                    <asp:Label ID="lblTemplate" runat="server" />
                </td>
                <td valign="bottom">
                    <asp:Label ID="Label1" Text="Set Order" runat="server" />
                </td>
                <td align="right">
                   <%-- <asp:ImageButton ID="imgBtnClose" OnClientClick="self.close(); return false;" ImageUrl="~/Images/icon-close.jpg"
                        ToolTip="Close" runat="server" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlCategoryView" runat="server" BackColor="White" Width="189px" Height="270px"
                        BorderWidth="1px" BorderColor="Black" ScrollBars="Auto">
                        <asp:UpdatePanel ID="updSections" runat="server">
                            <ContentTemplate>
                                <asp:TreeView ID="tvTree" runat="server" ImageSet="Msdn" Font-Size="10px" OnSelectedNodeChanged="tvTree_OnSelectedNodeChanged"
                                    NodeIndent="10">
                                    <ParentNodeStyle Font-Bold="False" />
                                    <HoverNodeStyle Font-Underline="True" BackColor="#97B1D0" BorderColor="#888888" BorderStyle="Solid"
                                        BorderWidth="0px" />
                                    <SelectedNodeStyle BackColor="#5078B3" ForeColor="White" Font-Underline="False" HorizontalPadding="3px"
                                        VerticalPadding="1px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                        NodeSpacing="1px" VerticalPadding="2px" />
                                </asp:TreeView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
                <td valign="top">
                    <asp:UpdatePanel ID="updlst" runat="server">
                        <ContentTemplate>
                            <asp:ListBox ID="lstTemplate" Width="189px" SkinID="listbox" Height="278px" runat="server">
                            </asp:ListBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tvTree" />
                            <asp:AsyncPostBackTrigger ControlID="btnUp" />
                            <asp:AsyncPostBackTrigger ControlID="btnDown" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnUp" Text="Move Up" SkinID="Button" OnClick="btnUp_OnClick" Width="74px"
                                    ToolTip="Move Up" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDown" Text="Move Down" SkinID="Button" OnClick="btnDown_OnClick"
                                    ToolTip="Move Down" Width="74px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
