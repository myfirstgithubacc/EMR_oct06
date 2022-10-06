<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PatientSearchCombo.ascx.cs"
    Inherits="Include_Components_PatientSearchCombo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadScriptBlock ID="RadCodeBlock1" runat="server" >
    <script language="javascript" type="text/javascript">
        
    </script>        
 </telerik:RadScriptBlock>

<telerik:RadComboBox runat="server" ID="RadCmbPatientSearch" Width="250px" Height="150px" EnableLoadOnDemand="true" ZIndex="50000"
    HighlightTemplatedItems="true" EmptyMessage="Type Name, DOB or Home# to find patient"
    DropDownWidth="470px" OnClientSelectedIndexChanged="OnClientSelectedIndexChangedEventHandler"
    OnItemsRequested="RadComboBoxProduct_ItemsRequested" ShowMoreResultsBox="true"
                EnableVirtualScrolling="true">
    <HeaderTemplate>
        <table style="width: 440px" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 65px" align="left">
                    Account #
                </td>
                <td style="width: 150px" align="left">
                    Patient Name
                </td>
                <td style="width: 75px" align="left">
                    Date Of Birth
                </td>
                <td style="width: 75px" align="left">
                    Age/Gender
                </td>
                <td style="width: 75px" align="left">
                    Home Phone
                </td>
            </tr>
        </table>
    </HeaderTemplate>
    <ItemTemplate>
        <table style="width: 440px" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 65px" align="center">
                    <%# DataBinder.Eval(Container,"Attributes['Account']" )%>
                </td>
                <td style="width: 150px;" align="left">
                    <%# DataBinder.Eval(Container, "Text")%>
                </td>
                <td style="width: 75px">
                    <%# DataBinder.Eval(Container, "Attributes['DOB']")%>
                </td>
                <td style="width: 75px">
                    <%# DataBinder.Eval(Container, "Attributes['Gender']")%>
                </td>
                <td style="width: 75px">
                    <%# DataBinder.Eval(Container, "Attributes['PhoneHome']")%>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</telerik:RadComboBox>
