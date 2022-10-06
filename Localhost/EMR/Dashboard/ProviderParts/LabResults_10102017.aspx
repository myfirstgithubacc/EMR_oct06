<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="LabResults_10102017.aspx.cs" Inherits="EMR_Dashboard_ProviderParts_LabResults" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function OnClientCloseReviewed(oWnd, args) {
            $get('<%=btnFilter.ClientID%>').click();
        }
    </script>

    <style type="text/css">
        .blink
        {
            text-decoration: blink;
            color: Green;
        }
        .noblink
        {
            text-decoration: inherit;
        }
    </style>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="2" cellspacing="0">
                <tr style="vertical-align: middle;" class="clsheader">
                    <td style="width: 8%;">
                        &nbsp;
                        <asp:Label ID="lblResultfor" runat="server" SkinID="label" Text=" Result For "></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: left;">
                        <telerik:RadComboBox ID="ddlProviders" runat="server" Width="95%" Skin="Outlook"
                            AutoPostBack="True" BorderColor="ActiveBorder" BackColor="AliceBlue" OnSelectedIndexChanged="ddlProviders_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 10%; text-align: center;">
                        <asp:Label ID="Label19" runat="server" SkinID="label" Text="Reviewed&nbsp;Staus" />
                    </td>
                    <td style="width: 20%; text-align: left;">
                        <telerik:RadComboBox ID="ddlReviewedStatus" SkinID="DropDown" runat="server" Width="95%"
                            BorderColor="ActiveBorder" BackColor="AliceBlue">
                            <Items>
                                <telerik:RadComboBoxItem Text="ALL" Value="2" />
                                <telerik:RadComboBoxItem Text="Not Reviewed" Value="0" Selected="true" />
                                <telerik:RadComboBoxItem Text="Reviewed" Value="1" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 35%; text-align: left;" colspan="2">
                        <asp:CheckBox ID="chkAbnormalValue" runat="server" />
                        <asp:Label ID="Label2" runat="server" Text="Abnormal&nbsp;Result(s)" ForeColor="DarkViolet"
                            SkinID="label"></asp:Label>
                        &nbsp;
                        <asp:CheckBox ID="chkCriticalValue" runat="server" /><asp:Label ID="Label1" runat="server"
                            Text="Panic&nbsp;Result(s)" ForeColor="Red" SkinID="label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 8%;">
                        &nbsp;
                        <asp:Label ID="Label7" runat="server" SkinID="label" Text="Search By" />
                    </td>
                    <td style="width: 25%; text-align: left;">
                        <telerik:RadComboBox ID="ddlSearch" SkinID="DropDown" DropDownWidth="150px" EmptyMessage="[ Select ]"
                            runat="server" Width="80px">
                            <Items>
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="RN" Selected="true" />
                                <telerik:RadComboBoxItem Text="Patient Name" Value="PN" />
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, IPNo%>' Value="IP" Visible="false" />
                            </Items>
                        </telerik:RadComboBox>
                        <asp:TextBox ID="txtSearchCretria" SkinID="textbox" Height="23px" Width ="70px"  runat="server"
                            Text="" MaxLength="15" />
                        &nbsp;
                    </td>
                    <td style="width: 10%; text-align: center;">
                        <asp:Label ID="Label20" runat="server" SkinID="label" Text="Date Range:"></asp:Label>
                    </td>
                    <td style="width: 20%; text-align: left;">
                        <telerik:RadComboBox ID="ddlTime" SkinID="DropDown" runat="server" AutoPostBack="True"
                            Width="95%" CausesValidation="false" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                            <Items>
                                <%--added by rakesh for the selected option is last 3 days should be come and become default on 30/04/2014 start--%>
                                <telerik:RadComboBoxItem Text="Last Three Days" Value="LastThreeDays" Selected="true" />
                                <%--added by rakesh for the selected option is last 3 days should be come and become default on 30/04/2014 end--%>
                                <telerik:RadComboBoxItem Text="Today Result" Value="Today" />
                                <telerik:RadComboBoxItem Text="Last Week Result" Value="LastWeek" />
                                <telerik:RadComboBoxItem Text="Last Two Weeks Result" Value="LastTwoWeeks" />
                                <telerik:RadComboBoxItem Text="Last One Month Result" Value="LastOneMonth" />
                                <telerik:RadComboBoxItem Text="Last Three Months Result" Value="LastThreeMonths" />
                                <telerik:RadComboBoxItem Text="Last Year Result" Value="LastYear" />
                                <telerik:RadComboBoxItem Text="Date Range Result" Value="DateRange" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 25%; text-align: left;">
                        <table id="tblDate" runat="server" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" SkinID="label" Text="From " />
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" SkinID="label" Text="To " />
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: center;">
                        <asp:Button ID="btnFilter" SkinID="Button" runat="server" OnClick="btnFilter_Click"
                            Text="Filter" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="" />
                        <asp:TextBox ID="txtProviderId" runat="server" Style="visibility: hidden; position: absolute;" />
                        <asp:TextBox ID="txtRegistrationId" runat="server" Style="visibility: hidden; position: absolute;" />
                    </td>
                </tr>
                <tr class="clsheader" style="height: 20px; text-align: center;">
                    <td colspan="6" style="padding-right: 15px;">
                        <asp:Label ID="lblNew" runat="server" Font-Bold="true" CssClass="noblink"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblResultChanged" Font-Bold="true" runat="server" CssClass="noblink" Visible="false" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="gvResultFinal" runat="server" SkinID="gridview" AlternatingRowStyle-BackColor="Bisque"
                            GridLines="Both" Width="99%" Height="100%" BorderWidth="1" AllowPaging="true"
                            PageSize="15" AllowSorting="true" AllowMultiRowSelection="false" AutoGenerateColumns="false"
                            ShowStatusBar="true" OnRowDataBound="gvResultFinal_OnRowDataBound" OnRowCommand="gvResultFinal_OnRowCommand"
                            OnPageIndexChanging="gvResultFinal_OnPageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Source" Visible="true" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Date" Visible="True" HeaderStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Provider" Visible="true" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, LABNO%>' Visible="true"
                                    HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' Visible="true"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ipno%>' Visible="True"
                                    HeaderStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Patient Name" Visible="true" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Investigation" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Result" Visible="true" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                        <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result"
                                            CommandArgument="None" Visible="true" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkprint" runat="server" CommandName="Print" CommandArgument="None"
                                            Text="Print" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="true" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnResultId" runat="server" Value='<%#Eval("ResultId") %>' />
                                        <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" CommandArgument="None"
                                            Text="Select" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RegistrationId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AbnormalValue" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CriticalValue" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Age/Gender" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status&nbsp;Color" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sample&nbsp;ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status&nbsp;ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="StationId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStationId" runat="server" Text='<%#Eval("StationId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ResultRemarksId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultRemarksId" runat="server" Text='<%#Eval("ResultRemarksId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="StatusCode" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="1" CellSpacing="1"
                Visible="false" />
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
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                            Behaviors="Close">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFilter" />
            <asp:AsyncPostBackTrigger ControlID="gvResultFinal" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
