<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CurrentMedications.ascx.cs"
    Inherits="EMR_Dashboard_Parts_CurrentMedications" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel6" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMsg" Visible="false" ForeColor="Red" Font-Bold="false" runat="server"></asp:Label>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="GDMedications" ForeColor="#333333" GridLines="Both" runat="server"
                                AllowPaging="true" PageSize="5" Width="95%" AutoGenerateColumns="false" CellPadding="0"
                                ShowHeader="true"  OnPageIndexChanging="GDMedications_PageIndexChanging"
                                SkinID="gridview">
                                <HeaderStyle HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField HeaderText="Medication" DataField="ItemName" />
                                    <asp:BoundField HeaderText="Start Date" DataField="StartDate" />
                                    <asp:BoundField HeaderText="Dose" DataField="Dose" />
                                    <asp:BoundField HeaderText="Units" DataField="UnitName" />
                                    <asp:BoundField HeaderText="Qty" DataField="Qty" />
                                  
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
                        <asp:LinkButton ID="lnkAddMedication" runat="server" Text="Add Current Medication"
                            OnClick="lnkAddMedication_Click"></asp:LinkButton>
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
