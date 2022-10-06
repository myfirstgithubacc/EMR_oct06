<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeSearchCombo.ascx.cs"
    Inherits="Include_Components_EmployeeSearchCombo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

    <script language="javascript" type="text/javascript">
        
    </script>

</telerik:RadScriptBlock>
<telerik:RadComboBox runat="server" ID="RadCmbEmployeeSearch" Width="250px" Height="400px"
    EnableLoadOnDemand="true" HighlightTemplatedItems="true" EmptyMessage="Type Name,Employee Type, Mobile# or Email-Id to Find Employee "
    DropDownWidth="600px" OnClientSelectedIndexChanged="OnClientSelectedIndexChangedEventHandler"
    OnItemsRequested="RadComboBoxProduct_ItemsRequested" ShowMoreResultsBox="true"
    EnableVirtualScrolling="true">
    <HeaderTemplate>
        <table style="width: 570px" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 100px" align="left">
                    Employee No
                </td>
                <td style="width: 200px" align="left">
                    Employee Name
                </td>
                <td style="width: 100px" align="left">
                    Employee Type
                </td>
                <td style="width: 120px" align="left">
                    Mobile #
                </td>
                <td style="width: 150px" align="left">
                    Email-Id
                </td>
            </tr>
        </table>
    </HeaderTemplate>
    <ItemTemplate>
        <table style="width: 570px" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 100px">
                    <%# DataBinder.Eval(Container, "Attributes['EmployeeNo']")%>
                </td>
                <td style="width: 200px;" align="left">
                    <%# DataBinder.Eval(Container, "Text")%>
                </td>
                <td style="width: 100px">
                    <%# DataBinder.Eval(Container, "Attributes['EmployeeType']")%>
                </td>
                <td style="width: 120px">
                    <%# DataBinder.Eval(Container, "Attributes['Mobile']")%>
                </td>
                <td style="width: 150px">
                    <%# DataBinder.Eval(Container, "Attributes['Email']")%>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</telerik:RadComboBox>
