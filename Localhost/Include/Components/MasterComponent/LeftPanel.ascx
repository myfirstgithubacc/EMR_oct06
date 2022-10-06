<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftPanel.ascx.cs" Inherits="Include_Components_MasterComponent_LeftPanel" %>
<link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
<link href="/Include/Style.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function pageLoad() {
    }
    function ShowLeftPnl() {

        $get("pnlLeft").style.visibility = 'visible';
    }
    function HideLeftPnl() {
        $get("pnlLeft").style.visibility = 'hidden';
    }
</script>

<style type="text/css">
    .taborderbutton
    {
        background-image: url(/Images/orders.jpg);
        background-repeat: repeat-x;
        height: 22px;
        text-align: center;
    }
    .tabmidbuttonactive
    {
        background-image: url(/Images/Butt.png);
        background-repeat: no-repeat;
        color: Black;
        height: 22px;
        text-align: center;
    }
</style>
<asp:TextBox ID="txtLeftPnl" runat="server" Text="0" Visible="false"></asp:TextBox>
<table >
    <tr>
        <td width="200px" bgcolor="#81A4C7" valign="top">
            <table cellpadding="2" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="link">
                        <table>
                            <tr>
                                <td>
                                    <img align="left" src="/images/add.gif" alt="my pictures" />
                                </td>
                                <td class="sublink">
                                    <asp:Label ID="sModuleName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="pnlModulePages" onmouseover="this.style.cursor='default';" runat="server"
                            ScrollBars="Auto" Height="450px" Width="200px">
                            <asp:TreeView ID="tvCategory" EnableClientScript="true" runat="server" NodeIndent="10"
                                OnSelectedNodeChanged="tvCategory_SelectedNodeChanged" ImageSet="Custom" CollapseImageUrl="/Images/minus.gif"
                                ShowLines="true" ExpandImageUrl="/Images/links.gif">
                                <ParentNodeStyle Font-Bold="False" />
                                <HoverNodeStyle Font-Underline="True" BackColor="#CCCCCC" BorderColor="#888888" BorderStyle="Solid"
                                    BorderWidth="0px" />
                                <SelectedNodeStyle BackColor="gray" ForeColor="White" Font-Underline="False" HorizontalPadding="3px"
                                    VerticalPadding="1px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="white" HorizontalPadding="5px"
                                    NodeSpacing="1px" VerticalPadding="2px" />
                            </asp:TreeView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom">
                        <div id="vista_toolbar1">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvModules" runat="server" AutoGenerateColumns="false" Width="100%"
                                            OnRowDataBound="gvModules_RowDataBound" ShowHeader="false" GridLines="None" AllowPaging="false"
                                            OnSelectedIndexChanged="gvModules_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="ModuleId" />
                                                <asp:BoundField DataField="ModuleName" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="link">
                                                    <ItemTemplate>
                                                        <table width="98%" cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col width="25px" />
                                                            </colgroup>
                                                            <tr id="tblM" runat="server">
                                                                <td align="left">
                                                                    <img align="left" src='<%#Eval("ImageUrl") %>' alt='<%#Eval("ModuleName") %>' />
                                                                </td>
                                                                <td align="left">
                                                                    <a href="#"><span><b>
                                                                        <%#Eval("ModuleName") %></b></span></a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
