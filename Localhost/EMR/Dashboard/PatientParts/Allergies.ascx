<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Allergies.ascx.cs" Inherits="EMR_Dashboard_Parts_Allergies" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc;">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel2" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblEmptyData" runat="server" Text="No Allergies" Style="font-weight: bold;
                            color: Red;" Visible="false">
                        </asp:Label>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="GDAllergy" runat="server" AllowPaging="true" ForeColor="#333333"
                                SkinID="gridview" GridLines="Both" PageSize="6" Width="95%" AutoGenerateColumns="false"
                                CellPadding="0" ShowHeader="true" OnRowDataBound="GDAllergy_RowDataBound" OnPageIndexChanging="GDAllergy_PageIndexChanging">
                                <HeaderStyle HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField HeaderText="Allergy Type" DataField="AllergyType" />
                                    <asp:BoundField HeaderText="Allergy Name" DataField="AllergyName" />
                                    <asp:BoundField HeaderText="Reaction" DataField="Reaction" />
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
                        <asp:LinkButton ID="lnkAllergyRedirect" runat="server" Text="Add Allergy" Visible="false" OnClick="lnkAllergy_OnClick"></asp:LinkButton>
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
