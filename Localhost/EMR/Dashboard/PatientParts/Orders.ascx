<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders.ascx.cs" Inherits="EMR_Dashboard_Parts_Orders" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel4" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="GDOrders" ForeColor="#333333" GridLines="Both" runat="server" AllowPaging="true"
                                PageSize="10" Width="95%" AutoGenerateColumns="false" CellPadding="0" ShowHeader="true"
                                SkinID="gridview" OnPageIndexChanging="GDOrders_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Order Date" DataField="OrderDate" />
                                    <asp:BoundField HeaderText="Service Name" DataField="ServiceName" />
                                    <asp:BoundField HeaderText="Lab Status" DataField="LabStatus" />
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
                        <asp:LinkButton ID="lnkAddOrders" runat="server" Text="Add Orders" Visible="false"  OnClick="lnkOrder_OnClick"  ></asp:LinkButton>
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
