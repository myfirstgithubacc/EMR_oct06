<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddVitalValues.aspx.cs" Inherits="EMR_Vitals_AddVitalValues" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellpadding="0" cellspacing="0" class="clsheader">
            <tr>
                <td align="left" style="width: 200px; padding-left: 10px;">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="true" />
                </td>
                <td align="center">
                    <div>
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" SkinID="Button" ValidationGroup="SaveData"
                            Text="Save" Width="100px" OnClick="btnSaveData_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClose" Text="Close" runat="server" SkinID="Button" CausesValidation="false"
                            OnClientClick="window.close();" />
                    </div>
                </td>
            </tr>
        </table>
        <table border="0" width="97%" cellpadding="2" cellspacing="0">
            <tr>
                <td colspan="2" align="center" style="height: 13px; color: green; font-size: 12px;
                    font-weight: bold;">
                    <asp:Label ID="lblMsg" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblValueName" runat="server" Text="Name<span style='color: Red'>*</span>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValueName" SkinID="textbox" runat="server" Width="225px"> 
                                </asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Upload Images"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="fileUplaod" runat="server" Width="220px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" SkinID="gridview"
                        OnRowUpdating="gvDetails_RowUpdating" OnRowEditing="gvDetails_RowEditing" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                        DataKeyNames="ValueID" Width="100%" OnRowDeleting="gvDetails_RowDeleting" CellPadding="4"
                        OnRowDataBound="gvDetails_RowDataBound" ForeColor="#333333" GridLines="None">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:BoundField DataField="ValueID" HeaderText="ValueID" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Vital&nbsp;Value">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" Text='<%#Eval("Name") %>' Width="200px" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:ImageField DataImageUrlField="ImagePath" ReadOnly="true" ControlStyle-Width="50"
                                ControlStyle-Height="50" HeaderText="Images">
                            </asp:ImageField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:ButtonField CommandName="Delete" Text="Delete" />
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
