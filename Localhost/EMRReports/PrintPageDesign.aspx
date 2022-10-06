<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PrintPageDesign.aspx.cs" Inherits="EMRReports_OutstandingSummary" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr class="clsheader">
            <td id="tdHeader" align="left" runat="server">
                <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" />
            </td>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="right"></td>
        </tr>
    </table>
    <table cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table cellpadding="2" cellspacing="0" style="color: black; background: #e0ebfd;">
                            <tr id="trOutStan" runat="server">
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rdlsummary" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdlsummary_SelectedIndexChanged">
                                        <asp:ListItem Text="Summary" Value="S" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Detail" Value="D"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2">
                                    <%--<asp:UpdatePanel ID="updatepaneldatewise" runat="server">
                                <ContentTemplate>--%>
                                    <asp:RadioButtonList ID="rdldatewise" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rdldatewise_SelectedIndexChanged">
                                        <asp:ListItem Text="Date Period" Value="D"></asp:ListItem>
                                        <asp:ListItem Text="As On" Value="A"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr id="trhealth" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Report" SkinID="label"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <telerik:RadComboBox ID="ddlReporttype" runat="server" EmptyMessage="[ Select ]"
                                        MarkFirstMatch="true" AutoPostBack="false" Width="350px">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="S" Selected="true" Text="Health Package Summary Details" />
                                            <telerik:RadComboBoxItem Value="D" Text="Health Package Details Company Wise" />
                                            <telerik:RadComboBoxItem Value="B" Text="Health Package Report Bill No Wise" />
                                            <telerik:RadComboBoxItem Value="C" Text="Health Package Comparision Report" />
                                            <%--  <telerik:RadComboBoxItem Value="PS" Text="Cash Patient Settlement Wise Report" />--%>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr id="trtype" runat="server">
                                <td>
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Source " />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlOPIP" runat="server" Skin="Outlook" Width="100px" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="OP" Value="O" />
                                            <telerik:RadComboBoxItem Text="IP" Value="I" />
                                            <telerik:RadComboBoxItem Text="Both" Value="B" Selected="true" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadComboBox ID="ddlInpatientType" runat="server" AutoPostBack="false" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="S" Selected="true" Text="Department Wise" />
                                            <telerik:RadComboBoxItem Value="D" Text="Doctor Wise" />
                                            <telerik:RadComboBoxItem Value="B" Text="Bed Category Wise" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="lable1" runat="server" Text="Patient Type" SkinID="label"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlPatienttype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPatienttype_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="A" Text="All" Selected="true" />
                                            <telerik:RadComboBoxItem Value="C" Text="Cash" />
                                            <telerik:RadComboBoxItem Value="B" Text="Credit" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadComboBox ID="ddlAuthorization" runat="server" AutoPostBack="false" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="OI" Text="OP/IP" />
                                            <telerik:RadComboBoxItem Value="AW" Text="Authority Wise" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr id="trdate" runat="server">
                                <td id="lbltdfrmdate" runat="server">
                                    <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label>&nbsp;
                                </td>
                                <td id="dttdfrmdate" runat="server">
                                    <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy">
                                    </telerik:RadDatePicker>
                                </td>
                                <td>&nbsp;&nbsp;
                            <asp:Label ID="Label3" runat="server" Text="To Date" SkinID="label"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy">
                                    </telerik:RadDatePicker>
                                </td>
                                <td>
                                    <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Print Report" OnClick="btnPrintreport_OnClick"
                                        SkinID="Button" Text="Print Report" />
                                    <asp:Button ID="btnExportToExcel" runat="server" ToolTip="Export To Excel" OnClick="btnExportToExcel_OnClick"
                                        SkinID="Button" Text="Export To Excel" Visible="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnToExcel" runat="server" ToolTip="Export To Excel" OnClick="btnToExcel_Click"
                                        SkinID="Button" Text="Export To Excel" Visible="false" />
                                </td>
                            </tr>

                            <tr id="trdatetime" runat="server" visible="false">
                                <td id="Td1" runat="server">
                                    <asp:Label ID="Label5" runat="server" Text="From Date" SkinID="label"></asp:Label>&nbsp;
                                </td>
                                <td id="Td2" runat="server">
                                    <telerik:RadDateTimePicker ID="dtfromdate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                        AutoPostBackControl="Both" OnSelectedDateChanged="OnSelectedDateChanged_change">
                                    </telerik:RadDateTimePicker>
                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="170px"
                                        MarkFirstMatch="true" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"
                                        Skin="Outlook" Width="50px" Visible="false">
                                    </telerik:RadComboBox>
                                </td>
                                <td>&nbsp;&nbsp;
                            <asp:Label ID="Label6" runat="server" Text="To Date" SkinID="label"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    <telerik:RadDateTimePicker ID="dttodate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                        AutoPostBackControl="Both" OnSelectedDateChanged="OnSelectedDateChanged_change">
                                    </telerik:RadDateTimePicker>
                                    <telerik:RadComboBox ID="RadComboBox2" runat="server" AutoPostBack="True" Height="170px"
                                        MarkFirstMatch="true" OnSelectedIndexChanged="RadComboBox2_SelectedIndexChanged"
                                        Skin="Outlook" Width="50px" Visible="false">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnPrintreport1" runat="server" ToolTip="Click to Print Report" OnClick="btnPrintreport1_OnClick"
                                        SkinID="Button" Text="Print Report" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblOTStatus" runat="server" Text="Booking Status" SkinID="label" Visible="false" />
                                    <asp:Label ID="lblUnderPackage" runat="server" SkinID="label" Text="Under Package"
                                        Visible="false" />
                                    <asp:Label ID="lblMode" runat="server" SkinID="label" Text="Mode" Visible="false" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlOTStatus" runat="server" Width="135px" CheckBoxes="true"
                                        MarkFirstMatch="true" AllowCustomText="true" EnableCheckAllItemsCheckBox="true"
                                        Visible="false" />
                                    <telerik:RadComboBox ID="ddlUnderPackage" runat="server" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Both" Value="2" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Package Included" Value="1" />
                                            <telerik:RadComboBoxItem Text="Package Excluded" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadComboBox ID="ddlMode" runat="server"
                                        CssClass="gridInput" CheckBoxes="true"
                                        EnableCheckAllItemsCheckBox="true" Visible="false" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkdetail" runat="server" Text="Details" SkinID="checkbox" />&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>&nbsp;&nbsp;
                            <asp:CheckBox ID="chkExport" runat="server" Text="Export" SkinID="checkbox" />
                                    <asp:CheckBox ID="ChkCompany" runat="server" Text="Company Wise" SkinID="checkbox"
                                        OnCheckedChanged="ChkCompany_OnCheckedChanged" AutoPostBack="true" Visible="false" />
                                    <asp:CheckBox ID="chkSummary" runat="server" Text="Summary" SkinID="checkbox" Visible="false" />
                                    <asp:CheckBox ID="chkAccountSummary" runat="server" Text="Account Summary" SkinID="checkbox"
                                        Visible="false" AutoPostBack="True"
                                        OnCheckedChanged="chkAccountSummary_CheckedChanged" />
                                    <asp:CheckBox ID="chkModewise" runat="server" Text="Mode Wise" SkinID="checkbox"
                                        Visible="false" AutoPostBack="True" OnCheckedChanged="chkModewise_OnCheckedChanged" />
                                    <asp:CheckBox ID="chkSpeciality" runat="server" Text="Doctor Wise" SkinID="checkbox" Visible="false" OnCheckedChanged="chkSpeciality_CheckedChanged" AutoPostBack="true" />
                                    <asp:CheckBox ID="chkcompanytype" runat="server" Text="Company Type Wise" SkinID="checkbox" Visible="false" OnCheckedChanged="chkcompanytype_CheckedChanged" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Entry Site" SkinID="label" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="Radentry" runat="server" AutoPostBack="true" Width="180px"
                                        CheckBoxes="true" MarkFirstMatch="true" AllowCustomText="true" Visible="false"
                                        OnSelectedIndexChanged="cmbentrysite_selectedchange">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="Chkentrysite" runat="server" Text="All" AutoPostBack="true" OnCheckedChanged="chkentrysite_CheckedChanged"
                                        Visible="false" SkinID="checkbox" />
                                </td>
                                <td>
                                    <asp:Label ID="lblpaymenttype" runat="server" Text="Payment Type" SkinID="label" Visible="false"></asp:Label>
                                </td>
                                <td align="left" colspan="3">
                                    <asp:DropDownList ID="DropDownPaymentType" runat="server" SkinID="DropDown" Visible="false" Width="100px">
                                        <asp:ListItem Value="A">All</asp:ListItem>
                                        <asp:ListItem Value="C">Cash</asp:ListItem>
                                        <asp:ListItem Value="B">Credit</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trloction" runat="server">
                                <td>
                                    <asp:Label ID="lbllocation" runat="server" Text="Location" SkinID="label"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlEntrySite" Width="250px" runat="server" SkinID="DropDown">
                                    </telerik:RadComboBox>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblType" runat="server" Text="Type" SkinID="label" Visible="false"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlType" runat="server" SkinID="DropDown" Visible="false">
                                        <asp:ListItem Value="A">All</asp:ListItem>
                                        <asp:ListItem Value="D">Credit Note</asp:ListItem>
                                        <asp:ListItem Value="W">Write Off</asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                            </tr>
                            <tr id="trDeficiencyCategory" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="lblDeficiencyCategory" runat="server" Text="Category" SkinID="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDeficiencyCategory" runat="server" SkinID="DropDown" Width="250px" />
                                </td>

                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:RadioButtonList ID="rblReportWise" runat="server" RepeatDirection="Horizontal"
                                        Font-Bold="true" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblReportWise_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Department" Value="D" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Doctor" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Speciality" Value="S"></asp:ListItem>
                                        <asp:ListItem Text="Bed Category" Value="B"></asp:ListItem>
                                        <asp:ListItem Text="Ward/Floor" Value="W"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trgrid" runat="server">
                                <td align="center" colspan="4" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlDep" Height="400px" Width="400px" ScrollBars="None">
                                                &nbsp; &nbsp;
                                        <asp:Label ID="Label4" runat="server" Text="" SkinID="label" Font-Bold="true"></asp:Label>&nbsp;
                                        <telerik:RadGrid ID="gvReporttype" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                            AllowFilteringByColumn="true" runat="server" Width="99%" AutoGenerateColumns="False"
                                            PageSize="12" EnableLinqExpressions="False" AllowPaging="false" CellSpacing="0"
                                            OnPreRender="gvReporttype_PreRender">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                                Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="False" />
                                            <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.
                                                    </div>
                                                </NoRecordsTemplate>
                                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="StoreId" AllowFiltering="false" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="false" HeaderText="StoreId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="true" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                        HeaderText=" Name" FilterControlWidth="99%">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDepartment" runat="server" />
                                                            <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("Name")%>' />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAllDepartment" OnCheckedChanged="chkAllDepartment_CheckedChanged"
                                                                Font-Bold="true" runat="server" Text="All Select / Unselect " AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <EditFormSettings>
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                </EditFormSettings>
                                            </MasterTableView>
                                            <FilterMenu EnableImageSprites="False">
                                            </FilterMenu>
                                        </telerik:RadGrid>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" height="10px">
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExportToExcel" />
                        <asp:PostBackTrigger ControlID="btnToExcel" />

                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
