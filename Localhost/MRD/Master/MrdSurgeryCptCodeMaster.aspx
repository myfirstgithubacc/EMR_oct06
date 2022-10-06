<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="~/MRD/Master/MrdSurgeryCptCodeMaster.aspx.cs" Inherits="MRD_MrdSurgeryCptCodeMaster"
    Title="CPT Code Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div align="center" style="width: 100%;">
        <asp:UpdatePanel ID="updMain" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" class="clsheader">
                    <tr>
                        <td>
                        </td>
                        <td style="width: 270px; padding-left: 10px;" align="left">
                            <asp:Label ID="Label1" runat="server" Text='CPT Code'></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnSave" runat="server" SkinID="Button" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" runat="server" SkinID="Button" Text="Clear" OnClick="btnClear_Click" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="100%" align="left">
                    <tr>
                        <td>
                            CPT Code
                        </td>
                        <td>
                            <asp:TextBox ID="txtCptCode" runat="server" SkinID="textbox" MaxLength="10"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblDescription" runat="server" Text='Description'></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" SkinID="textbox" MaxLength="200"
                                Width="500px"></asp:TextBox>
                        </td>
                        <td>
                            Status
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlstatus" SkinID="DropDown" runat="server">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="ACTIVE" />
                                    <telerik:RadComboBoxItem Value="0" Text="IN-ACTIVE" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Panel ID="pnlCPT" runat="server" Width="100%" Height="490px" ScrollBars="Horizontal">
                                <asp:GridView ID="grvCPTCode" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvCPTCode_RowDataBound"
                                    SkinID="Gridview" Width="100%" AllowPaging="true" AllowSorting="true" PageSize="23"
                                    PagerSettings-Mode="Numeric" OnPageIndexChanging="grvCPTCode_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="" Visible="false" />
                                        <asp:BoundField DataField="CPTCode" HeaderText='CPTCode' HeaderStyle-Width="5%" />
                                        <asp:BoundField DataField="Description" HeaderText='CPT Description' />
                                        <asp:BoundField DataField="Active" HeaderText="Active" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbSelect" runat="server" OnClick="lbSelect_Click">Select</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
