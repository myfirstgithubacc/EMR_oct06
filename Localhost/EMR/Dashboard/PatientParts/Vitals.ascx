<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Vitals.ascx.cs" Inherits="EMR_Dashboard_Parts_Vitals" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

    <script type="text/javascript">

        function setValue(val, valName) {

            $get('<%=hdnVitalvalue.ClientID%>').value = val;
            $get('<%=hdnVitalName.ClientID%>').value = valName;

            var oWnd = radopen("/EMR/Vitals/Vitalgraph.aspx?Value=" + $get('<%=hdnVitalvalue.ClientID%>').value +
                                    "&Name=" + $get('<%=hdnVitalName.ClientID%>').value, "RadWindowForNew");

            oWnd.setSize(1000, 650)
            oWnd.center();
            oWnd.VisibleStatusbar = "false";
            oWnd.set_status(""); // would like to remove statusbar, not just blank it
        }

    </script>
</telerik:RadCodeBlock>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr valign="top">
        <td style="width: 100%">
            <asp:Panel ID="pnlVitalGrid" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="updVital" runat="server">
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="GDVitals" runat="server" AllowPaging="true" PageSize="7" ForeColor="#333333"
                                SkinID="gridview" GridLines="Both" Width="95%" AutoGenerateColumns="true" CellPadding="0"
                                ShowHeader="true" OnPageIndexChanging="GDVitals_PageIndexChanging" OnRowDataBound="GDVitals_OnRowDataBound">
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <AlternatingRowStyle BackColor="White" />
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
                        <asp:LinkButton ID="lnkAddVitals" runat="server" Text="Add Vitals" Visible="false"  OnClick="lnkAddVitals_OnClick"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:HiddenField ID="hdnVitalvalue" runat="server" />
            <asp:HiddenField ID="hdnVitalName" runat="server" />
        </td>
    </tr>
</table>
<asp:TextBox ID="hdnToDate" runat="server" Style="visibility: hidden; position: absolute;" />
<asp:TextBox ID="hdnFromDate" runat="server" Style="visibility: hidden; position: absolute;" />
<asp:TextBox ID="hdnEncounterNumber" runat="server" Style="visibility: hidden; position: absolute;" />
<asp:TextBox ID="hdnDateVale" runat="server" Style="visibility: hidden; position: absolute;" />
