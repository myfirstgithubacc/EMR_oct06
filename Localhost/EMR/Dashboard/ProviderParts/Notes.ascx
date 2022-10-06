<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Notes.ascx.cs" Inherits="EMR_Dashboard_ProviderParts_Notesl" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr>
        <td>
            <asp:TextBox ID="txtTemp" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel4" runat="server" ScrollBars="Vertical" Width="100%" Height="210px">
                <asp:GridView ID="GVNotes" ForeColor="#333333" runat="server" AllowPaging="True"
                    SkinID="gridview" PageSize="10" Width="96%" AutoGenerateColumns="false" CellPadding="0"
                    ShowHeader="true" EmptyDataText="No Open Notes!" EmptyDataRowStyle-ForeColor="Red"
                    OnPageIndexChanging="GVNotes_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="ltEncDate" runat="server" Text="Date Time"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEncDate" runat="server" Text='<%#(ChangeDateFormat(Eval("EncounterDate").ToString()))%>'
                                    Width="60px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="ltName" runat="server" Text="Patient Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" Font-Strikeout="false" onmouseover="this.style.textDecoration='underline';"
                                    OnClick="lnkName_Click" onmouseout="this.style.textDecoration='none';" Text='<%#Eval("Name")%>'
                                    runat="server" CommandArgument='<%#Eval("AllValues")%>' CommandName="Select"
                                    Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="ltFormName" runat="server" Text="Note"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFormName" runat="server" Text='<%#Eval("FormName")%>' Width="80px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="ltDiagnosis" runat="server" Text="Diagnosis"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDiagnosis" runat="server" Text='<%#Eval("Diagnosis")%>' Width="100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td align="left">
            <table border="0" style="height: 17px; width: 100%;" cellpadding="0" cellspacing="0">
                <tr style="background-color: #EFF3FB">
                    <td>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Panel ID="pnlOrders" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnNotes" runat="server" />
            <asp:HiddenField ID="hdnNotesCtrl" runat="server" />
        </td>
    </tr>
</table>
