<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ResultFinalization.aspx.cs" Inherits="LIS_Phlebotomy_ResultFinalization" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
            border: solid #868686 1px !important;
            border-top: none !important;
            color: #333;
            background: 0 -2300px repeat-x #c1e5ef !important;
        }

        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {
            background-color: #fff !important;
        }

        #ctl00_ContentPlaceHolder1_Panel1 {
            background-color: #c1e5ef !important;
        }
        th.rgHeader{
            white-space:nowrap!important;
        }
       div#ctl00_ContentPlaceHolder1_gvResultFinal{
            overflow-x:auto!important;
            table-layout:auto;
        }
       table#ctl00_ContentPlaceHolder1_gvResultFinal_ctl00_Header{
            overflow-x:auto!important;
            table-layout:auto!important;
       }
       span.Lable2{
           margin-right:5px!important;
       }
    </style>


    <script type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtSearchCretriaNumeric.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }
        function OnClientResultEntryClose(oWnd, args) {
            $get('<%=btnRefresh.ClientID%>').click();
        }
        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false) {
                    return false;
                }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.value = "Processing...";
            }

            return true;
        }
        function Validatetextbox(myButton) {
            var ddlSearch = $get('<%=ddlSearch.ClientID%>');
            var txtSearch = $get('<%=txtSearchCretria.ClientID%>');
            if (txtSearch != null) {
                if (txtSearch.value != '') {
                    if (ddlSearch.value == 'Patient Name') {
                        var filter = /^[a-zA-Z-\s]+$/;
                        if (!filter.test(txtSearch.value)) {
                            txtSearch.value = '';
                            alert("Invalid Search Criteria! Please Enter The Character Value");
                            return false;
                        }
                        if (txtSearch.value.length < 3) {
                            alert("Invalid Search Criteria! Please Enter atleast 3 Charachter");
                            return false;
                        }
                    }
                    if ((ddlSearch.value == 'MR#') || (ddlSearch.value == 'Accession #') || (ddlSearch.value == 'Lab No')) {
                        var n2 = txtSearch.value;
                        txtSearch.value = parseFloat(txtSearch.value);
                        if (txtSearch.value == 'NaN' && n2 != txtSearch.value) {
                            txtSearch.value = '';
                            alert("Invalid Search Criteria!  Please Enter The Numeric Value");
                            return false;
                        }
                    }
                }
            }

            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false) {
                    return false;
                }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.value = "Processing...";
            }

            return true;
        }
        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "red";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "blue";
        }
        function OnClientAuthenticationSaveClose(oWnd, args) {
            $get('<%=btnAuthenticateSave.ClientID%>').click();
        }
        function onClientSaveTatDelayReason(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var TatDelayReason = arg.TatDelayReason;

                $get('<%=hdnTatDealyReason.ClientID%>').value = TatDelayReason;
                $get('<%=btnTatDelayFinalize.ClientID%>').click();

            }
        }

        function OnClientDoctorSaveClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var EmployeeLeft = arg.EmployeeLeft;
                var EmployeeRight = arg.EmployeeRight;
                var EmployeeCenter = arg.EmployeeCenter;
                $get('<%=hdnLeftDoctor.ClientID%>').value = EmployeeLeft;
                $get('<%=hdnCenterDoctor.ClientID%>').value = EmployeeCenter;
                $get('<%=hdnRightDoctor.ClientID%>').value = EmployeeRight;
                $get('<%=btnEmployee.ClientID%>').click();
            }
        }
    </script>

    <script type="text/javascript">
        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }
        function executeCode(evt) {
            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 120:  // F9
                    $get('<%=BtnPrint.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }
    </script>

    <style type="text/css">
        .RadGrid_Office2007 .rgSelectedRow {
            background-color: #ffcb60 !important;
        }
    </style>
    <asp:UpdatePanel ID="upd1cvsdgfsdf" runat="server" style="overflow:hidden;">
        <ContentTemplate>


            <div class="container-fluid header_main">
                <div class="col-md-4">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="Result&nbsp;Finalization" /></h2>
                </div>
                <div class="col-md-8 text-right">
                    <div style="display: none;">
                        <asp:LinkButton ID="lnkVisitHistory" runat="server" CssClass="btn" Text="Patient&nbsp;Visit&nbsp;History" OnClick="lnkVisitHistory_OnClick"></asp:LinkButton>
                    </div>
                    <asp:LinkButton ID="lnkConsolidateLabReport" runat="server" CssClass="btn" Text="Consolidate Lab Report" OnClick="lnkConsolidateLabReport_OnClick" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="lnkRelayDetails" runat="server" Text="Relay&nbsp;Details" CssClass="btn" OnClick="lnkRelayDetails_OnClick" />
                    <asp:LinkButton ID="lnkPackageDetail" runat="server" Text="Package&nbsp;Details" CssClass="btn" OnClick="lnkPackageDetails_OnClick" Visible="false" />
                    <asp:Button ID="btnResultFinalization" runat="server" CssClass="btn btn-default" Text="Release" ToolTip="Click&nbsp;to&nbsp;Release" OnClick="btnResultFinalization_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />
                    <asp:Button ID="btnCancelResult" runat="server" ToolTip="Cancel Result" CssClass="btn btn-default" Text="Cancel Result" OnClick="btnCancelResult_OnClick" />
                    <asp:Button ID="BtnCancelProvisionalResult" runat="server" ToolTip="Cancel Provisional Release" CssClass="btn btn-default" Text="Cancel Provisional Release" OnClick="BtnCancelProvisionalResult_OnClick" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="SaveData" />
                    <asp:Button ID="BtnPrint" runat="server" CssClass="btn btn-primary" Text="Print (Ctrl+F9)" ToolTip="print" OnClick="btnprint_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" Visible="false" />
                </div>
            </div>

            <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="all" ControlsToSkip="Buttons"
                runat="server" DecorationZoneID="dvSearchZone" Skin="Office2007"></telerik:RadFormDecorator>
            <div id="dvSearchZone">
                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnRefresh">

                    <div class="container-fluid">
                        <div class="row form-group">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMessage" runat="server" SkinID="label" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>


                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label2" runat="server" Text="Source" Visible="True" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlSource" SkinID="DropDown" runat="server" Width="100%"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSource_OnSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="ALL" Value="A" Selected="True" />
                                                <telerik:RadComboBoxItem Text="OPD" Value="OPD" />
                                                <telerik:RadComboBoxItem Text="IPD" Value="IPD" />
                                                <telerik:RadComboBoxItem Text="Package" Value="PACKAGE" />
                                                <telerik:RadComboBoxItem Text="ER" Value="ER" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-7 pr-0">
                                        <asp:Label ID="Label5" runat="server" CssClass="mr-2 float-left" Text="Result&nbsp;From" />
                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="90px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                    <div class="col-5 pl-0">

                                        <asp:Label ID="Label6" runat="server" CssClass="mr-2" Text="To" />
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="90px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label4" runat="server" Text="Status" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlStatus" runat="server" EmptyMessage="[ ALL ]" Width="100%" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblTest" runat="server" Text="Test Priority" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="DdlResultType" runat="server" EmptyMessage="[ ALL ]" Width="100%">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="" Text="" Selected="true" />
                                                <telerik:RadComboBoxItem Value="R" Text="Routine" />
                                                <telerik:RadComboBoxItem Value="S" Text="Stat" />
                                                <telerik:RadComboBoxItem Value="D" Text="Day Care" />
                                                <%--<telerik:RadComboBoxItem Value="C" Text="Critical" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 PaddingRightSpacing">
                                        <asp:Label ID="Label7" runat="server" Text="Search By" />
                                    </div>
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-6 PaddingRightSpacing">
                                                <telerik:RadComboBox ID="ddlSearch" SkinID="DropDown" EmptyMessage="[ Select ]" runat="server"
                                                    Width="100%" AutoPostBack="true" OnTextChanged="ddlSearch_OnTextChanged">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, LABNO%>' Value="LN" Selected="true" />
                                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="RN" />
                                                        <telerik:RadComboBoxItem Text='Manual Lab No' Value="MLN" />
                                                        <telerik:RadComboBoxItem Text="Patient Name" Value="PN" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                            <div class="col-6">
                                                <asp:TextBox ID="txtSearchCretria" SkinID="textbox" Width="100%" runat="server" Text=""
                                                    MaxLength="20" />
                                                <asp:TextBox ID="txtSearchCretriaNumeric" SkinID="textbox" Width="100%" runat="server"
                                                    Text="" MaxLength="13" Visible="false" onkeyup="return validateMaxLength();" />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    FilterType="Custom" TargetControlID="txtSearchCretriaNumeric" ValidChars="0123456789" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label3" runat="server" Text="&nbsp;Facility" Visible="false" />
                                        <asp:Label ID="Label15" runat="server" Text="Entry&nbsp;Site" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlFacility" runat="server" Width="100%" Enabled="false" EmptyMessage="[ ALL ]" Visible="false" />
                                        <telerik:RadComboBox ID="ddlEntrySitesActual" SkinID="DropDown" runat="server" Width="100%"
                                            EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label14" runat="server" Text="Lab&nbsp;Entry&nbsp;Site" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlEntrySites" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlReportType" runat="server" Width="100%" MarkFirstMatch="true" EmptyMessage="[Select Report Type]"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>


                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label16" runat="server" Text="Ward" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" AutoPostBack="false" EmptyMessage="[ Select ]" Filter="Contains" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row">
                                    <div class="col-md-4 PaddingRightSpacing">
                                        <asp:Label ID="Label8" runat="server" Text='<%$ Resources:PRegistration, SubDepartment%>'></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlSubDepartment" runat="server" MarkFirstMatch="true" EmptyMessage="[All Sub Departments]"
                                            Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label13" runat="server" Text="Service Name" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlServiceName" runat="server" Width="100%" AutoPostBack="false" EmptyMessage="[ Select ]" Filter="Contains" />

                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row">
                                    <div class="col-lg-9">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div class="PD-TabRadio" style="float: none !important; margin: 0.0em 1em 0.6em -5px !important;">
                                                        <asp:CheckBox ID="chkNotFinalized" Checked="true" runat="server" Text="Not&nbsp;finalized" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="PD-TabRadio" style="margin-left: -28px; float: none !important; margin: 0.0em 1em 0.6em -5px !important;">
                                                        <asp:CheckBox ID="chkAssignedToMeOnly" Checked="false" runat="server" Text="Assigned&nbsp;to&nbsp;me" Visible="false" />
                                                        <asp:CheckBox ID="chkOutSourceTest" Checked="false" runat="server" Text="Outsource&nbsp;Test" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="col-lg-3 col-12 text-right">

                                        <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" Text="Refresh" ToolTip="Refresh"
                                            OnClick="btnRefresh_OnClick" OnClientClick="Validatetextbox(this)" UseSubmitBehavior="false" />

                                    </div>
                                </div>
                            </div>

                        </div>

                        <%--<div class="row form-group">
                            <div class="col-md-11" align="right">
                                <div class="row">
                                        <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" Text="Refresh" ToolTip="Refresh"
                                    OnClick="btnRefresh_OnClick" OnClientClick="Validatetextbox(this)"
                                    UseSubmitBehavior="false" />
                                </div>
                            </div>
                        </div>--%>
                    </div>

                </asp:Panel>
            </div>

            <div class="container-fluid">
                <div class="row">
                    <asp:Panel ID="PanelN" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                        Width="100%" Height="390px" ScrollBars="Auto" >
                        <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" Skin="Office2007"
                            BorderWidth="0px" AllowPaging="True" AllowCustomPaging="True" Height="99%" AllowMultiRowSelection="True"
                            AutoGenerateColumns="False" ShowStatusBar="True" EnableLinqExpressions="False"
                            GridLines="None" PageSize="200" OnItemDataBound="gvResultFinal_OnItemDataBound"
                            OnItemCommand="gvResultFinal_OnItemCommand" OnPageIndexChanged="gvResultFinal_OnPageIndexChanged"
                            CellSpacing="0">
                            <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView TableLayout="auto" GroupLoadMode="Client">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.
                                    </div>
                                </NoRecordsTemplate>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                                <%--<GroupHeaderItemStyle Font-Bold="true" />--%>
                                <GroupHeaderItemStyle ForeColor="Navy" />
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="LabNo" FieldName="LabNo" HeaderText='<%$ Resources:PRegistration, LABNO%>' FormatString="" />
                                            <telerik:GridGroupByField FieldAlias="RegistrationNo" FieldName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>' FormatString="" />
                                            <%--<telerik:GridGroupByField FieldAlias="Facility" FieldName="FacilityName" HeaderText="Facility" FormatString="" />--%>
                                            <telerik:GridGroupByField FieldAlias="PatientName" FieldName="PatientName" HeaderText="Patient" FormatString="" />
                                            <telerik:GridGroupByField FieldAlias="AgeGender" FieldName="AgeGender" HeaderText="Age/Gender" FormatString="" />
                                            <%--<telerik:GridGroupByField FieldAlias="ManualLabNo" FieldName="ManualLabNo" HeaderText="Manual Lab No" FormatString="" />--%>
                                            <telerik:GridGroupByField FieldAlias="OrderDate" FieldName="OrderDate" HeaderText="Order Date" FormatString="" />
                                            <telerik:GridGroupByField FieldAlias="EncounterNo" FieldName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>' FormatString="" />
                                            <telerik:GridGroupByField FieldAlias="Source" FieldName="Source" HeaderText='<%$ Resources:PRegistration, Source%>' FormatString="" />
                                            <telerik:GridGroupByField FieldAlias="Ward" FieldName="Ward" HeaderText='Ward/Bed' FormatString="" />
                                            <telerik:GridGroupByField FieldAlias="ReferredBy" FieldName="ReferredBy" HeaderText='Ref. By' />
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="LabNo" SortOrder="None" />
                                            <telerik:GridGroupByField FieldName="RegistrationNo" SortOrder="None" />
                                            <%--<telerik:GridGroupByField FieldName="FacilityName" SortOrder="None" />--%>
                                            <telerik:GridGroupByField FieldName="PatientName" SortOrder="None" />
                                            <telerik:GridGroupByField FieldName="AgeGender" SortOrder="None" />
                                            <%--<telerik:GridGroupByField FieldName="ManualLabNo" SortOrder="None" />--%>
                                            <telerik:GridGroupByField FieldName="OrderDate" SortOrder="None" />
                                            <telerik:GridGroupByField FieldName="EncounterNo" SortOrder="None" />
                                            <telerik:GridGroupByField FieldName="Source" SortOrder="None" />
                                            <telerik:GridGroupByField FieldName="Ward" SortOrder="None" />
                                            <telerik:GridGroupByField FieldName="ReferredBy" SortOrder="None" />
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True" />
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True" />
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderStyle-Width="30px" />
                                    <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation"
                                        FilterControlWidth="99%" HeaderStyle-Width="20%" Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkResultHistory" runat="server" Text='<%#Eval("ServiceName") %>'
                                                CommandName="ResultHistory" CommandArgument="None" Visible="true" CausesValidation="false"
                                                Font-Underline="false" />
                                            <%-- <asp:Label ID="lblCriticalIndication" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                Font-Size="X-Large" ForeColor="Red" />--%>
                                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%#Eval("EncounterNo") %>' />
                                            <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                            <asp:HiddenField ID="hdnLabNo" runat="server" Value='<%#Eval("LabNo") %>' />
                                            <asp:HiddenField ID="hdnSource" runat="server" Value='<%#Eval("Source") %>' />
                                            <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<%#Eval("DiagSampleId") %>' />

                                            <asp:HiddenField ID="hdnTatlimitation" runat="server" Value='<%#Eval("TAT") %>' />
                                            <asp:HiddenField ID="hdnSampleCollectedDate" runat="server" Value='<%#Eval("SampleCollectedDate") %>' />

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="FieldName" DefaultInsertValue="" HeaderText="Field&nbsp;Name"
                                        DataField="FieldName" SortExpression="FieldName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="20%" ShowFilterIcon="false"
                                        FilterControlWidth="99%" AllowFiltering="true" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ScanInTime" DefaultInsertValue="" HeaderText="Scan&nbsp;In&nbsp;Time"
                                        DataField="LabTechnician" CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblScanInTime" runat="server" Text='<%#Eval("ScanInTime") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ScanOutTime" DefaultInsertValue="" HeaderText="Scan&nbsp;Out&nbsp;Time"
                                        DataField="LabTechnician" CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblScanOutTime" runat="server" Text='<%#Eval("ScanOutTime") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabTechnician" DefaultInsertValue="" HeaderText="Lab&nbsp;Technician"
                                        DataField="LabTechnician" SortExpression="LabTechnician" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                        FilterControlWidth="99%" AllowFiltering="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabTechnician" runat="server" Text='<%#Eval("LabTechnician") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ManualLabNo" DefaultInsertValue="" HeaderText=""
                                        DataField="ManualLabNo" SortExpression="ManualLabNo" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="5%" ShowFilterIcon="false"
                                        Visible="false" FilterControlWidth="99%" AllowFiltering="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblManualLabNo" runat="server" Text='<%#Eval("ManualLabNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabInCharge" DefaultInsertValue="" HeaderText="Lab&nbsp;In&nbsp;Charge"
                                        DataField="LabInCharge" SortExpression="LabInCharge" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                        FilterControlWidth="99%" AllowFiltering="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabInCharge" runat="server" Text='<%#Eval("LabInCharge") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabSupervisor" DefaultInsertValue="" HeaderText="Lab&nbsp;Supervisor"
                                        DataField="LabSupervisor" SortExpression="LabSupervisor" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                        FilterControlWidth="99%" AllowFiltering="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabSupervisor" runat="server" Text='<%#Eval("LabSupervisor") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabDoctor" DefaultInsertValue="" HeaderText="Lab Doctor"
                                        DataField="LabDoctor" SortExpression="LabDoctor" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                        FilterControlWidth="99%" AllowFiltering="true" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabDoctor" runat="server" Text='<%#Eval("LabDoctor") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabDirector" DefaultInsertValue="" HeaderText="Lab Director"
                                        DataField="LabDoctor" SortExpression="LabDoctor" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                        FilterControlWidth="99%" AllowFiltering="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabDirector" runat="server" Text='<%#Eval("LabDirector") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PackageName" DefaultInsertValue="" HeaderText="Package Name"
                                        DataField="PackageName" SortExpression="PackageName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                        FilterControlWidth="99%" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPackageName" runat="server" Text='<%#Eval("PackageName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="TATDelay" DefaultInsertValue="" HeaderText="Delay(HH:mm)" HeaderTooltip="TAT Delay(HH:MM)"
                                        DataField="TATDelay" SortExpression="TATDelay" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="10%"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTAT" runat="server" Text='<%#Eval("TATDelay") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                                        DataField="Result" SortExpression="Result" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="10%"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                            <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result"
                                                CommandArgument="None" Visible="true" ForeColor="Black" CausesValidation="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Details" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="45px" HeaderText="Details" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtDetails" runat="server" Text="Details" CommandName="Details" />&nbsp;
                                            <asp:Label ID="lblForNotes" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"
                                                Style="text-decoration: blink; font-size: large" Text="*"></asp:Label>
                                            <asp:Label ID="lblNotesAvailable" runat="server" Visible="false" Text='<%#Eval("NotesAvailable") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
				    <telerik:GridTemplateColumn UniqueName="IsConfidential" DefaultInsertValue="" HeaderText="DoNotShowInWard&nbsp;Color"
                                            HeaderStyle-Width="28px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIsConfidential" runat="server" Text='<%#Eval("DoNotShowInWard") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ViewHistroy" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px" HeaderText="Result&nbsp;History" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewHistory" runat="server" Text="View" CommandName="View" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PViewHistroy" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="70px" HeaderText="Visit&nbsp;History" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPatientViewHistory" runat="server" Text="Visit" CommandName="VisitHistory" ToolTip="View Patient Visit History" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="IsPrintPatientDiagnosis" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="30px" HeaderText="DX" AllowFiltering="false" HeaderTooltip="Print Patient Diagnosed">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsPrintPatientDiagnosis" runat="server" Checked='<%#Eval("IsPrintPatientDiagnosis") %>' />
                                            <asp:HiddenField ID="hdnIsPrintPatientDiagnosisMaster" runat="server" Value='<%#Eval("IsPrintPatientDiagnosisMaster") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Print" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" HeaderText="Print" AllowFiltering="false" HeaderTooltip="Print Result">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnResultHTML" runat="server" Value='<%#Eval("ResultHTML") %>' />
                                            <asp:LinkButton ID="lnkResultHTML" runat="server" Text='Print' CommandName="ResultHTML" CommandArgument='<%#Eval("DiagSampleId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="UniqueName1" DefaultInsertValue="" HeaderText="RegistrationId"
                                        AllowFiltering="false" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" HeaderText='Lab&nbsp;No.'
                                        DataField="LabNo" SortExpression="LabNo" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                            <asp:HiddenField ID="hdnResulAlert" runat="server" Value='<%#Eval("ResultAlert") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="FacilityName" DefaultInsertValue="" HeaderText='Facility'
                                        DataField="FacilityName" SortExpression="FacilityName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                        FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                        DataField="RegistrationNo" SortExpression="RegistrationNo" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                        FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                                        DataField="PatientName" SortExpression="PatientName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                        FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="UniqueName7" DefaultInsertValue="" HeaderText="Age/Gender"
                                        AllowFiltering="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="UniqueName10" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                            <asp:Label ID="lblStat" runat="server" Text='<%#Eval("Stat") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="UniqueName11" DefaultInsertValue="" HeaderText="Sample&nbsp;ID"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="UniqueName12" DefaultInsertValue="" HeaderText="Status&nbsp;ID"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="UniqueName14" DefaultInsertValue="" HeaderText="ServiceId"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AbnormalValue" DefaultInsertValue="" HeaderText="AbnormalValue"
                                        AllowFiltering="false" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="CriticalValue" DefaultInsertValue="" HeaderText="CriticalValue"
                                        AllowFiltering="false" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ResultRemarksId" DefaultInsertValue="" HeaderText="ResultRemarksId"
                                        AllowFiltering="false" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultRemarksId" runat="server" Text='<%#Eval("ResultRemarksId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="StatusCode" DefaultInsertValue="" HeaderText="StatusCode"
                                        AllowFiltering="false" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                        </telerik:RadGrid>
                        <asp:HiddenField ID="hf1" runat="server" />
                        <asp:HiddenField ID="hdnAssignDiagSampleId" runat="server" />
                    </asp:Panel>
                </div>
            </div>


            <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="RadioButtons"
                runat="server" DecorationZoneID="dvAssignZone" Skin="Office2007"></telerik:RadFormDecorator>

            <div id="dvAssignZone">
                <asp:Panel ID="Panel1" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                    Width="100%" Height="26px" ScrollBars="None" Style="padding-top: 0px; border-top-width: 0px; background-color: Lavender;">

                    <div class="container-fluid" id="trgrdfooter" runat="server">
                        <div class="row form-groupTop01 margin_bottom01">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" Text="Release&nbsp;Stage" />
                            </div>
                            <div class="col-md-4">
                                <asp:UpdatePanel ID="upReleaseStage" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlEmployee" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="ddlReleaseStage" SkinID="DropDown" runat="server" EmptyMessage="[ Select ]"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlReleaseStage_OnSelectedIndexChanged"
                                            Width="150px" MarkFirstMatch="true" Filter="Contains">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Provisional" Value="P" />
                                                <telerik:RadComboBoxItem Text="Finalize" Value="F" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblInfoAssignTo" runat="server" Text="Assign To" />
                            </div>
                            <div class="col-md-4">
                                <asp:UpdatePanel ID="upddlEmployee" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlEmployee" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="ddlEmployee" SkinID="DropDown" runat="server" EmptyMessage="[ Select Employee ]"
                                            Width="250px" MarkFirstMatch="true" Filter="Contains" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Button ID="btnSignatories" runat="server" ToolTip="Signatories" Visible="false"
                                    SkinID="Button" Text="Signatories" />
                            </div>
                        </div>
                    </div>


                </asp:Panel>
            </div>


            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server"
                                    Behaviors="Close">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowPopup" Skin="Office2007" InitialBehaviors="Maximize" runat="server"/>
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:Button ID="btnAuthenticateSave" runat="server" Text="" SkinID="Button" Style="visibility: hidden; float: left; margin: 0; padding: 0; height: 1px;"
                                    OnClick="btnAuthenticateSave_Click" Width="0" />
                                <asp:Button ID="btnTatDelayFinalize" runat="server" Text="" SkinID="Button" Style="visibility: hidden; float: left; margin: 0; padding: 0; height: 1px;"
                                    OnClick="btnTatDelayFinalization_OnClick" Width="0" />
                                <asp:Button ID="btnEmployee" runat="server" Text="" SkinID="Button" Style="visibility: hidden; float: left; margin: 0; padding: 0; height: 1px;"
                                    OnClick="btnEmployee_Click" Width="0" />
                                <asp:HiddenField ID="hdnTatDealyReason" runat="server" />
                                <asp:HiddenField ID="hdnLeftDoctor" runat="server" />
                                <asp:HiddenField ID="hdnRightDoctor" runat="server" />
                                <asp:HiddenField ID="hdnCenterDoctor" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>


            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-12" style="margin-top: 10px;">

                        <%--<asp:Label ID="Label9" runat="server" SkinID="label" Text="" Height="14" Width="15" BorderWidth="1px" />--%>

                        <%--<asp:Label ID="Label11" runat="server" SkinID="label" Text="" BackColor="Red" Height="14" Width="15" BorderWidth="1px" />--%>

                        <table>
                            <tr>
                                <td>
                                    <ucl:legend ID="Legend1" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" CssClass="LegendColor" Text="Abnormal" BackColor="DarkViolet" /></td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" CssClass="LegendColor" Text='<%$ Resources:PRegistration, PanicValue%>' BackColor="Red" /></td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
            <div id="dvPatientResultHistory" runat="server" class="container-fluid" visible="false" style="width: 610px; height: 540px; left: 500px; top: 95px; bottom: 0; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;">
                <div class="row">
                    <div class="container-fluid header_main">
                        <div class="col-md-10">
                            <asp:Label ID="lblResultHistoryPatientName" Font-Size="12px" runat="server" Font-Bold="true" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnResultHistoryClose" runat="server" CssClass="btn btn-primary" CausesValidation="false" Text="Close" OnClick="btnResultHistoryClose_OnClick" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lblResultHistoryServiceName" Font-Size="12px" runat="server" Font-Bold="true" />
                </div>
                <div class="row">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <%--<div style="width: 595px; height: 395px; overflow: scroll;">--%>
                            <div id="divScroll" style="overflow: auto; width: 595px; height: 475px; border: solid; border-width: 1px;">
                                <%--<asp:GridView ID="gvPatientResultHistory" runat="server" SkinID="gridviewOrderNew" Width="100%" Height="100%" AutoGenerateColumns="true"
                                    AllowSorting="true" AllowPaging="false" OnRowDataBound="gvPatientResultHistory_OnRowDataBound">
                                </asp:GridView>--%>

                                <telerik:RadGrid ID="gvPatientResultHistory" runat="server" Width="100%" Height="96%" Skin="Office2007"
                                    AutoGenerateColumns="true" HeaderStyle-Font-Size="8pt" ItemStyle-Font-Size="8pt" AlternatingItemStyle-Font-Size="8pt"
                                    BorderWidth="0" CellPadding="0" CellSpacing="0" HeaderStyle-HorizontalAlign="Center"
                                    OnItemDataBound="gvPatientResultHistory_OnRowDataBound">
                                    <ClientSettings AllowColumnsReorder="false" Scrolling-FrozenColumnsCount="1" Scrolling-AllowScroll="true"
                                        Scrolling-UseStaticHeaders="true" EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                    </ClientSettings>

                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView TableLayout="Auto" GroupLoadMode="Client">
                                        <HeaderStyle Font-Bold="true" />
                                        <FooterStyle BackColor="Yellow" />
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No&nbsp;Record&nbsp;Found.
                                            </div>
                                        </NoRecordsTemplate>
                                        <GroupHeaderItemStyle Font-Bold="true" />
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <div style="background-color: lightblue; width: 80px; align-content: center; border: solid; border-width: thin;"><b>Current Test</b></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvResultFinal" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="upd1cvsdgfsdf"
        DisplayAfter="1000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div style="width: 154px; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px">
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
