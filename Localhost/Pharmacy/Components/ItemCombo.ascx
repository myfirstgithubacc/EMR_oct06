<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ItemCombo.ascx.cs" Inherits="Pharmacy_Components_ItemCombo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

    <script language="javascript" type="text/javascript">        
    </script>

</telerik:RadScriptBlock>
<telerik:RadComboBox ID="cboItem" runat="server" Width="400px" Height="150px" EnableLoadOnDemand="true"
    ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="[ Search items by typing minimum 2 characters ]" 
    DropDownWidth="550px" OnClientSelectedIndexChanged="cboItem_OnClientSelectedIndexChanged"
    OnItemsRequested="cboItem_ItemsRequested" ShowMoreResultsBox="true" EnableVirtualScrolling="true">
    <HeaderTemplate>
        <table style="width: 99%" cellspacing="2" cellpadding="0">
            <tr>
                <td style="width: 115px" align="left">
                    Item #
                </td>
                <td align="left">
                    Item Name
                </td>
            </tr>
        </table>
    </HeaderTemplate>
    <ItemTemplate>
        <table style="width: 99%" cellspacing="2" cellpadding="0">
            <tr>
                <td style="width: 115px" align="left">
                    <%# DataBinder.Eval(Container,"Attributes['ItemNo']" )%>
                </td>
                <td align="left">
                    <%# DataBinder.Eval(Container, "Text")%>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</telerik:RadComboBox>

