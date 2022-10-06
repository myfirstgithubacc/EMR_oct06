<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Notes.ascx.cs" Inherits="EMR_Dashboard_Parts_Notes" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript" language="javascript">
    function SearchPatientOnClientClose(oWnd, args) {
        $get('<%=btnfind.ClientID%>').click();
    }
</script>

<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc;">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel5" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="gvNotes" ForeColor="#333333" GridLines="Both" runat="server" AllowPaging="true"
                                SkinID="gridview" PageSize="6" Width="95%" AutoGenerateColumns="false" CellPadding="0"
                                ShowHeader="true" OnPageIndexChanging="gvNotes_PageIndexChanging">
                                <HeaderStyle HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField HeaderText="Date" DataField="EncounterDate" />
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <HeaderTemplate>
                                            <asp:Literal ID="ltName" runat="server" Text="Provider"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkName" Font-Strikeout="false" onmouseover="this.style.textDecoration='underline';"
                                                OnClick="lnkName_Click" onmouseout="this.style.textDecoration='none';" Text='<%#Eval("DoctorName")%>'
                                                runat="server" CommandArgument='<%#Eval("AllValues")%>' CommandName="Select"
                                                Width="100px" ForeColor="Blue" Font-Underline="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Provider" DataField="DoctorName" />--%>
                                    <asp:BoundField HeaderText="Document" DataField="FormName" />
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td align="left">
            <table border="0" style="height: 17px; width: 100%;" cellpadding="0" cellspacing="0">
                <tr style="background-color: #EFF3FB">
                    <td>
                        <asp:LinkButton ID="lnkNotesRedirect" runat="server" Text="Add Notes" OnClick="lnkNotesRedirect_OnClick"></asp:LinkButton>
                    </td>
                    <td colspan="2" align="center">
                        <asp:TextBox ID="hdnToDate" runat="server" Style="visibility: hidden; position: absolute;" />
                        <asp:TextBox ID="hdnFromDate" runat="server" Style="visibility: hidden; position: absolute;" />
                        <asp:TextBox ID="hdnEncounterNumber" runat="server" Style="visibility: hidden; position: absolute;" />
                        <asp:TextBox ID="hdnDateVale" runat="server" Style="visibility: hidden; position: absolute;" />
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                        <asp:Panel ID="Panel7" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
