<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Problems.ascx.cs" Inherits="EMR_Dashboard_Parts_Problems" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:ScriptManagerProxy ID="scriptProxy" runat="server">
</asp:ScriptManagerProxy>


<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc;">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel5" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="GDProblems" ForeColor="#333333" GridLines="Both" runat="server"
                                AllowPaging="true" PageSize="6" Width="95%" AutoGenerateColumns="false" CellPadding="0"
                                ShowHeader="true" OnRowDataBound="GDProblems_RowDataBound" SkinID="gridview"
                                OnPageIndexChanging="GDProblems_PageIndexChanging">
                                <HeaderStyle HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Problems">
                                        <ItemTemplate>
                                            <asp:Label ID="lnkProblem" Font-Strikeout="false" Text='<%#Eval("ProblemDescription")%>'
                                                runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("EncodedDate")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                        <asp:LinkButton ID="lnkProblemRedirect" runat="server" Text="Add Problem" Visible="false"  OnClick="lnkProblem_OnClick"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="hdnToDate" runat="server" Style="visibility: hidden; position: absolute;" />
            <asp:TextBox ID="hdnFromDate" runat="server" Style="visibility: hidden; position: absolute;" />
            <asp:TextBox ID="hdnEncounterNumber" runat="server" Style="visibility: hidden; position: absolute;" />
            <asp:TextBox ID="hdnDateVale" runat="server" Style="visibility: hidden; position: absolute;" />
        </td>
    </tr>
</table>
