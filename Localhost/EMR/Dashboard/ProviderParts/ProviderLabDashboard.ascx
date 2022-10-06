<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProviderLabDashboard.ascx.cs"
    Inherits="EMR_Dashboard_ProviderParts_ProviderLabDashboard" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>

<script type="text/javascript" language="javascript">
    function OnClientCloseReviewed(oWnd, args) {
        $get('<%=btnCloseReviewed.ClientID%>').click();
    }
</script>

<table width="100%" border="0" cellpadding="2" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="" />
            <asp:TextBox ID="txtProviderId" runat="server" Style="visibility: hidden; position: absolute;" />
            <asp:TextBox ID="txtRegistrationId" runat="server" Style="visibility: hidden; position: absolute;" />
        </td>
    </tr>
</table>
<%--<table border="0" width="100%" cellpadding="2" cellspacing="0">
    <tr>
        <td>
            <asp:Button ID="btnAttachment" runat="server" ToolTip="Link To Attachment" SkinID="Button"
                Text="Link To Attachment" OnClick="btnAttachment_OnClick" />
        </td>
        <td>
            <asp:TextBox ID="txtPath" runat="server" Text="" />
        </td>
        <td>
            <asp:CheckBox ID="chkIncludeResultInNote" runat="server" SkinID="checkbox" Checked="true" />
        </td>
    </tr>
</table>--%>
<table width="100%" border="0" cellpadding="2" cellspacing="0">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label19" runat="server" SkinID="label" Text="Reviewed&nbsp;Staus" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReviewedStatus" SkinID="DropDown" runat="server" Width="110px">
                            <asp:ListItem Text="ALL" Value="2" />
                            <asp:ListItem Text="Not Reviewed" Value="0" Selected="true" />
                            <asp:ListItem Text="Reviewed" Value="1" />
                        </asp:DropDownList>
                    </td>
                    <asp:UpdatePanel ID="pnl" runat="server">
                        <ContentTemplate>
                            <td>
                                <asp:DropDownList ID="ddlTime" SkinID="DropDown" runat="server" AutoPostBack="True"
                                    Width="165px" CausesValidation="false" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                    <asp:ListItem Text="Today Result" Value="Today" />
                                    <asp:ListItem Text="Last Week Result" Value="LastWeek" />
                                    <asp:ListItem Text="Last Two Weeks Result" Value="LastTwoWeeks" />
                                    <asp:ListItem Text="Last One Month Result" Value="LastOneMonth" />
                                    <asp:ListItem Text="Last Three Months Result" Value="LastThreeMonths" Selected="true" />
                                    <asp:ListItem Text="Last Year Result" Value="LastYear" />
                                    <asp:ListItem Text="Date Range Result" Value="DateRange" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <table id="tblDate" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Button ID="btnFilter" SkinID="Button" runat="server" OnClick="btnFilter_Click"
                                    Text="Filter" />
                            </td>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="Panel1" runat="server" Width="99%" Height="280px" BorderWidth="0"
                ScrollBars="None">
                <asp:GridView ID="gvDetails" ForeColor="#333333" runat="server" AllowPaging="True"
                    PageSize="7" SkinID="gridview" Width="99%" AutoGenerateColumns="False" CellPadding="0"
                    EmptyDataText="No Record Found !" EmptyDataRowStyle-ForeColor="Red" OnPageIndexChanging="gvDetails_PageIndexChanging"
                    OnRowDataBound="gvDetails_RowDataBound" OnSelectedIndexChanging="gvDetails_SelectedIndexChanging">
                    <EmptyDataRowStyle ForeColor="Red" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="Select" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" CommandArgument='<%#Eval("ResultId")%>'
                                    onmouseover="this.style.textDecoration='underline';" onmouseout="this.style.textDecoration='none';"
                                    Font-Strikeout="false" Text="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label5" runat="server" Text="Result&nbsp;Date" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("ResultDate") %>' Width="120px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label2" runat="server" Text="Test Name" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>' Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label9" runat="server" Text="Result" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblResult" runat="server" Text='<%#Eval("Result") %>' Width="70px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label10" runat="server" Text="Unit" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("UnitName") %>' Width="50px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label11" runat="server" Text="Reference&nbsp;Range" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReferenceRange" runat="server" Text="" Width="120px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label6" runat="server" Text="Patient&nbsp;Name" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' Width="120px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label7" runat="server" Text="Acc#" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>'
                                    Width="60px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label8" runat="server" Text="LOINC" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLoincCode" runat="server" Text='<%#Eval("LoincCode") %>' Width="60px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label3" runat="server" Text="Order&nbsp;Date" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' Width="120px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label4" runat="server" Text="Sample&nbsp;Collected&nbsp;Date" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSampleCollectedDate" runat="server" Text='<%#Eval("SampleCollectedDate") %>'
                                    Width="120px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label4" runat="server" Text="Lab&nbsp;Flag&nbsp;Value" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLabFlagValue" runat="server" Text='<%#Eval("LabFlagValue") %>'
                                    Width="90px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label4" runat="server" Text="Test&nbsp;Result&nbsp;Status" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTestResultStatus" runat="server" Text='<%#Eval("TestResultStatus") %>'
                                    Width="90px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label4" runat="server" Text="Comments" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReviewedComments" runat="server" Text='<%#Eval("ReviewedComments") %>'
                                    Width="90px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label12" runat="server" Text="MinValue" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMinValue" runat="server" Text='<%#Eval("MinValue") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label13" runat="server" Text="Symbol" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSymbol" runat="server" Text='<%#Eval("Symbol") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label14" runat="server" Text="MaxValue" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMaxValue" runat="server" Text='<%#Eval("MaxValue") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label15" runat="server" Text="AbnormalValue" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label15" runat="server" Text="FieldType" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFieldType" runat="server" Text='<%#Eval("FieldType") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="Label16" runat="server" Text="ReviewedStatus" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReviewedStatus" runat="server" Text='<%#Eval("ReviewedStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </td>
    </tr>
</table>
<br />
<asp:Table ID="tblLegend" runat="server" border="0" CellPadding="1" CellSpacing="1" />
<table>
    <tr>
        <td>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                Behaviors="Close">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" runat="server">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <asp:Button ID="btnCloseReviewed" runat="server" CausesValidation="false" Style="visibility: hidden;"
                OnClick="btnCloseReviewed_OnClick" />
        </td>
    </tr>
</table>
