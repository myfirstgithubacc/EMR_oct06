<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrugFavourite.aspx.cs" Inherits="EMR_Medication_DrugFavourite"
    Title="Favourite Drug" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.ItemIds = document.getElementById("hdnPageItemId").value;
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

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" width="100%" class="clsheader" bgcolor="#e3dcco" cellpadding="0"
            cellspacing="0">
            <tr align="left">
                <td>
                    <asp:Image ID="Image1" ImageUrl="/Images/Assessment.png" Height="22" runat="server" />
                </td>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="Drug Name : " SkinID="label" />
                    <asp:Label ID="lblFavouriteName" SkinID="label" runat="server" Text="" />
                </td>
                <td align="right">
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" />
                    <asp:Button ID="btnAddtoFav" Text="Add to Favorities" ToolTip="Add to Favorities"
                        SkinID="Button" OnClick="btnAddtoFav_Click" runat="server" Visible="false" />
                    <asp:Button ID="btnClose" Text="Close" ToolTip="Close" SkinID="Button" OnClientClick="javascript:window.close();"
                        runat="server" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" width="100px">
            <tr>
                <td>
                    <asp:GridView ID="lstFavourite" TabIndex="11" SkinID="gridview" runat="server" Width="100%"
                        AutoGenerateColumns="false" AllowSorting="false" OnSelectedIndexChanged="lstFavourite_SelectedIndexChanged"
                        ShowHeader="True" ShowFooter="false" OnRowCommand="lstFavourite_OnRowCommand">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Drug Name" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemName" runat="server" Width="300px" Text='<%#Eval("ItemName") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                SelectText="Select" CausesValidation="false" ShowSelectButton="true" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="18px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                        CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                        ImageUrl="~/Images/DeleteRow.png" Width="14px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hdnPageItemId" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
