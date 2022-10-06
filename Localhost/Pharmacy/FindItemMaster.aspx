<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindItemMaster.aspx.cs" Inherits="Pharmacy_FindItemMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Item</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>

        <script type="text/javascript">

            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.ItemID = document.getElementById("hdnItemID").value;

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
        </script>

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr style="background-color: #E0EBFD">
                        <td align="center" style="font-size: 12px;">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </td>
                        <td align="right" style="width: 200px">
                            <asp:HiddenField ID="hdnItemID" runat="server" />
                            <asp:Button ID="btnSearch" runat="server" SkinID="Button" ToolTip="Filter" Text="Filter"
                                OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server" SkinID="Button" ToolTip="Clear Filter"
                                Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />&nbsp;
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="1" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" SkinID="label" Text="Item No/Name" />
                        </td>
                        <td>
                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearch">
                                <asp:TextBox ID="txtName"  runat="server" SkinID="textbox" Width="150px" MaxLength="50" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlgrid" runat="server" Height="440px" Width="450px" ScrollBars="Auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvItem" runat="server" SkinID="gridview" HeaderStyle-Wrap="false"
                                            AutoGenerateColumns="False" Width="100%" AllowPaging="true" PageSize="20" OnPageIndexChanging="gvItem_OnPageIndexChanging"
                                           OnSelectedIndexChanged="gvItem_SelectedIndexChanged" OnRowDataBound="gvItem_RowDataBound">
                                            
                                            <EmptyDataTemplate>
                                            <div style="font-weight: bold; color: Red;">No Record Found.</div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                            
                                           
                                                <asp:CommandField ControlStyle-ForeColor="Blue" SelectText="Select" ShowSelectButton="true"
                                                    ItemStyle-Width="30px">
                                                    <ControlStyle ForeColor="Blue" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                <asp:BoundField DataField="ItemNo" HeaderText='<%$ Resources:PRegistration, ItemNo%>'
                                                    HeaderStyle-Width="50px" ItemStyle-Width="50px" />
                                                <asp:BoundField DataField="ItemName" HeaderText='<%$ Resources:PRegistration, ItemName%>' />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvItem" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
