<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Phlebotomy.aspx.cs" Inherits="LIS_Phlebotomy_Phlebotomy" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
      span#ctl00_ContentPlaceHolder1_lblMessage {
          
        }

        div#ctl00_ContentPlaceHolder1_upd1 {
            overflow-x: hidden !important;
        }
    </style>
    <script type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtLabNo.ClientID%>');
            if (txt.value > 2147483647) {
                alert("Value should not be more then 2147483647.");
                txt.value = txt.value.substring(0, 9);
                txt.focus();
            }
        }

        function Validatetextbox() {

            var ddlSearch = $get('<%=ddlSearch.ClientID%>');
            var txtSearch = $get('<%=txtSearchCretria.ClientID%>');

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
        function OnClientClose(oWnd, args) {
            $get('<%=btnclose.ClientID%>').click();
        }
        function OnClientCloseCollect(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;
                var DailySerialNo = arg.DailySerialNo;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                $get('<%=hdnDailySerialNo.ClientID%>').value = DailySerialNo;
            }
            $get('<%=btnCloseCollect.ClientID%>').click();
        }

        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "red";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "blue";
        }
        function ConfirmDeAcknowledge() {
            if (confirm("Are you sure you want to Proceed?") == true) {
                return true;
            }
            else {
                return false;
            }
        }
        function ConfirmDeCollect() {
            if (confirm("Are you sure you want to de-collect?") == true) {
                return true;
            }
            else {
                return false;
            }
        }
        function SearchPatientOnClientClose(oWnd, args) {

            $get('<%=btnRefresh.ClientID%>').click();
        }
    </script>

    <style type="text/css">
        .RadGrid_Office2007 .rgSelectedRow {
            background-color: #ffcb60 !important;
        }

        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        input#ctl00_ContentPlaceHolder1_chkAutoLable {
            float: none !important;
        }
        /*.rfdLabel.RadForm_Office2007 label { line-height:1.1em}*/
        /*.RadGrid .rgRow, .RadGrid .rgAltRow { background: #fff !important;}*/


        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
            background: #c1e5ef !important;
            border: 1px solid #98abb1 !important;
            border-top: none !important;
            color: #333 !important;
            outline: none !important;
        }

        .RadGrid_Office2007 td.rgPagerCell {
            border: 1px solid #5d8cc9 !important;
            background: #c1e5ef !important;
            outline: none !important;
        }

        .RadGrid .rgPager .rgStatus {
            border: none !important;
        }

        .RadGrid .rgPager .rgStatus {
            width: 0px !important;
            padding: 0 !important;
            margin: 0 !important;
            float: left !important;
        }

        .RadGrid_Office2007 .rgGroupHeader {
            background: #c1e5ef !important;
            color: #171717 !important;
        }

        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {
            background: none !important;
        }

        @media (min-width: 992px) and (max-width: 1199px) {
            #ctl00_ContentPlaceHolder1_txtFromDate_dateInput_wrapper,
            #ctl00_ContentPlaceHolder1_txtToDate_dateInput_wrapper {
                min-width: 58px !important;
                float: left !important;
            }

            #ctl00_ContentPlaceHolder1_Label6 {
                width: 15px !important;
                float: left !important;
                margin: 0 !important;
                padding: 0 !important;
            }

            #ctl00_ContentPlaceHolder1_Label12 {
                float: left;
                width: 100px;
            }
        }

        table#ctl00_ContentPlaceHolder1_gvDetails_ctl00_Header {
            width: 100%;
             table-layout: auto!important; 
            white-space:nowrap!important;
            empty-cells: show;
            
        }
      
        table#ctl00_ContentPlaceHolder1_gvTestDetails_ctl00_Header{
             width: 100%;
             table-layout: auto!important; 
            white-space:nowrap!important;
            empty-cells: show;
        }
      body#ctl00_Body1{
          overflow-x:hidden!important;
      }
    </style>


    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>



            <div class="container-fluid header_main">
                <div class="col-md-4">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="Sample&nbsp;Collection" /></h2>
                </div>

                <div class="col-md-8 text-right">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="SaveData" />
                    <asp:Button ID="btnVisitHistory" runat="server" ToolTip="Click here to Print referal Slip"
                        OnClick="btnVisitHistory_OnClick" CssClass="btn btn-default" Text="Visit History" />
                    <asp:Button ID="btnPrintReferal" runat="server" ToolTip="Click here to Print referal Slip"
                        OnClick="btnPrintReferal_OnClick" CssClass="btn btn-default" Text="Print Referal Slip" />
                    <asp:Button ID="lblSampleDispatch" runat="server" ToolTip="Sample&nbsp;Dispatch"
                        CssClass="btn btn-default" Text="Sample Dispatch" OnClick="lblSampleDispatch_Click" />
                    <asp:Button ID="btnInvReport" runat="server" ToolTip="Investigation&nbsp;Report(s)"
                        CssClass="btn btn-default" Text="Print Investigation(s)" OnClick="btnInvReport_OnClick" />
                    <asp:Button ID="btnTagExternalCenter" runat="server" ToolTip="Click here to tag external center with service(s)"
                        OnClick="btnTagExternalCenter_OnClick" CssClass="btn btn-default" Text="Tag&nbsp;External&nbsp;Center" />
                    <asp:Button ID="btnPatientResultHistory" runat="server" ToolTip="Click here to view selected patient's all result(s)"
                        OnClick="btnPatientResultHistory_OnClick" CssClass="btn btn-default" Text="Diagnostic History" />
                    <asp:Button ID="btnInvResult" runat="server" ToolTip="Investigation&nbsp;Result"
                        OnClick="btnInvResult_OnClick" CssClass="btn btn-primary" Text="Inv.&nbsp;Result" />
                    <asp:CheckBox ID="chkAutoLable" runat="server" Checked="false" Text="Auto" OnCheckedChanged="chkAutolable_click" AutoPostBack="true" />
                    <asp:TextBox ID="txtNooflbl" runat="server" Width="30px"></asp:TextBox>
                    <asp:Button ID="btnCollect" runat="server" ToolTip="Collect" OnClick="btnCollect_OnClick"
                        CssClass="btn btn-primary" Text="Collect" />
                    <asp:Button ID="btnPrintLabel" runat="server" ToolTip="Print Label(s)" OnClick="btnPrintLabel1_OnClick"
                        CssClass="btn btn-primary" Text="Print Label(s)" />
                    <asp:Button ID="btnSampleDeCollect" runat="server" ToolTip="Sample De Collect" OnClick="btnSampleDeCollect_OnClick"
                        CssClass="btn btn-primary" ValidationGroup="decol" Text="Sample De Collect" CausesValidation="true" />
                    <asp:Button ID="btnSampleDeAck" runat="server" ToolTip="Sample UnAcknowledge" OnClick="btnSampleDeAck_OnClick"
                        CssClass="btn btn-primary" ValidationGroup="SaveData" Text="Sample UnAcknowledge" />

                </div>
            </div>

            <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" ControlsToSkip="Buttons"
                runat="server" DecorationZoneID="dvSearchZone" Skin="Office2007"></telerik:RadFormDecorator>



            <div class="container-fluid" id="dvSearchZone">

                <div class="row form-group">
                    <div class="col-md-9 text-center">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnInvResult" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnCollect" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnPrintLabel" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSampleDeCollect" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>


                <div class="row form-group">
                    <div class="col-lg-3 col-sm-3 col-6">
                        <div class="row">
                            <div class="col-6">
                                <div class="row">
                                    <div class="col-lg-3">

                                        <asp:Label ID="Label4" runat="server" Text="From" />
                                    </div>
                                    <div class="col-lg-9">

                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />

                                    </div>
                                </div>
                            </div>
                            <div class="col-6 ">
                                <div class="row">

                                    <div class="col-lg-2 label2">
                                        <asp:Label ID="Label6" runat="server" Text="To" />
                                    </div>
                                    <div class="col-lg-9 ">
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3 col-sm-3 col-6">
                        <div class="row">
                            <div class="col-lg-4 ">
                                <asp:Label ID="Label8" runat="server" Text="Search By" />
                            </div>
                            <div class="col-lg-8">
                                <div class="row">
                                    <div class="col-7">
                                        <telerik:RadComboBox ID="ddlSearch" EmptyMessage="[ Select ]" runat="server" Width="100%"
                                            AutoPostBack="true" OnTextChanged="ddlSearch_OnTextChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, LABNO%>' Value="LN" Selected="true" />
                                                <telerik:RadComboBoxItem Text='Manual Lab No' Value="MLN" />
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="RN" />
                                                <%-- <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, ipno%>' Value="IP"  />--%>
                                                <telerik:RadComboBoxItem Text="Patient Name" Value="PN" />
                                                <telerik:RadComboBoxItem Text="Bed No." Value="BN" />
                                                <telerik:RadComboBoxItem Text="Ward Name" Value="WN" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-5 PaddingLeftSpacing">
                                        <asp:TextBox ID="txtSearchCretria" Width="100%" runat="server" Height="22px" Text=""
                                            MaxLength="20" />
                                        <asp:TextBox ID="txtLabNo" Width="100%" runat="server" Text="" Height="22px" MaxLength="20"
                                            Visible="false" onkeyup="return validateMaxLength();" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                            FilterType="Custom" TargetControlID="txtLabNo" ValidChars="0123456789" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3 col-sm-3 col-6">
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:Label ID="Label1" runat="server" Text="Status" />
                            </div>
                            <div class="col-lg-8">
                                <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" Width="100%" runat="server" EmptyMessage="[ ALL ]" />
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3 col-sm-3 col-6">
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:Label ID="Label2" runat="server" Text="Source" />
                            </div>
                            <div class="col-lg-8">
                                <telerik:RadComboBox ID="ddlSource" SkinID="DropDown" Width="100%" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSource_OnSelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="A" Selected="True" />
                                        <telerik:RadComboBoxItem Text="OPD" Value="OPD" />
                                        <telerik:RadComboBoxItem Text="IPD" Value="IPD" />
                                        <telerik:RadComboBoxItem Text="Package" Value="PACKAGE" />
                                        <telerik:RadComboBoxItem Text="ER" Value="ER" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>


                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnRefresh">
                    <div class="row form-group">

                        <div class="col-lg-3 col-sm-3 col-6">
                            <div class="row">
                                <div class="col-lg-4 ">
                                    <asp:Label ID="lblTest" runat="server" Text="Test Priority" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="DdlTestPriority" SkinID="DropDown" runat="server" Width="100%" EmptyMessage="[ ALL ]">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="A" Text="" Selected="true" />
                                            <telerik:RadComboBoxItem Value="R" Text="Routine" />
                                            <telerik:RadComboBoxItem Value="S" Text="Stat" />
                                            <telerik:RadComboBoxItem Value="D" Text="Day Care" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-sm-3 col-6">
                            <div class="row">
                                <div class="col-lg-4">
                                    <asp:Label ID="Label3" runat="server" Text="&nbsp;Facility" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlFacility" Width="100%" runat="server" EmptyMessage="[ ALL ]" Enabled="false" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-sm-3 col-6">
                            <div class="row" id="Td2" runat="server">
                                <div class="col-lg-4">
                                    <asp:Label ID="Label13" runat="server" Text="Entry&nbsp;Site" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlEntrySites" SkinID="DropDown" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-sm-3 col-6">
                            <div class="row" id="tblEntrySite" runat="server">
                                <div class="col-lg-4 ">
                                    <asp:Label ID="Label10" runat="server" Text="Facility&nbsp;Site&nbsp;" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlFacilityEntrySite" SkinID="DropDown" Width="100%" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlFacilityEntrySite_OnSelectedIndexChanged" runat="server"
                                        EmptyMessage="[ ALL ]" Enabled="false" />
                                </div>
                            </div>

                            <div class="row" id="tblEntrySite1" runat="server">
                                <div class="col-lg-4">
                                    <asp:Label ID="lblEntrySitesDiag" runat="server" Text="Lab&nbsp;Entry&nbsp;Site" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlEntrySitesDiag" SkinID="DropDown" Width="100%" runat="server"
                                        EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-lg-3 col-sm-3 col-6">
                            <div class="row">
                                <div class="col-lg-4 ">
                                    <asp:Label ID="Label12" runat="server" Text='<%$ Resources:PRegistration, SubDepartment%>'></asp:Label>
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlSubDepartment" runat="server" MarkFirstMatch="true" Width="100%" EmptyMessage="[All Sub Departments]"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-6">
                            <div class="row">
                                <div class="col-lg-4 ">
                                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="Service Name" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlServiceName" SkinID="DropDown" runat="server" Width="100%"
                                        AutoPostBack="false" EmptyMessage="[ Select ]" Filter="Contains" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-6">
                            <div class="row" id="Td1" runat="server">
                                <div class="col-lg-4 ">
                                    <asp:Label ID="Label11" runat="server" Text="Review&nbsp;Status" />
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlReviewStatus" runat="server" Width="100%" EmptyMessage="--All--"
                                        MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="2" Text="All" />
                                            <telerik:RadComboBoxItem Value="1" Text="Reviewed" />
                                            <telerik:RadComboBoxItem Value="0" Text="Not Reviewed" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-sm-3 col-6" style="display: none;">
                            <div class="row">
                                <div class="col-lg-4" id="tblFurtherAck" runat="server">
                                    <asp:Label ID="Label5" runat="server" SkinID="label" Text="Further&nbsp;Ack" /><span id="Span1" style='color: Red' runat="server">*</span>
                                </div>
                                <div class="col-lg-8" id="tblFurtherAck1" border="0" runat="server">
                                    <telerik:RadComboBox ID="ddlFurtherAck" SkinID="DropDown" runat="server" EmptyMessage="[ Select ]"
                                        Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="" Value="" />
                                            <telerik:RadComboBoxItem Text="No" Value="N" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Yes" Value="Y" />
                                            <telerik:RadComboBoxItem Text="Cancel" Value="C" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-6">
                            <div class="row">
                                <div class="col-lg-4 ">
                                    <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>
                                </div>
                                <div class="col-lg-8">
                                    <telerik:RadComboBox ID="ddlReportType" runat="server" MarkFirstMatch="true" Width="100%" EmptyMessage="[Select Report Type]"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>

                    </div>



                    <div class="row form-group">
                        <div class="col-md-9">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:LinkButton ID="lnkManualLabNo" runat="server" Text="Other Details" OnClick="lnkManualLabNo_OnClick" Visible="false" />&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkRelayDetails" runat="server" Text="Relay&nbsp;Details" OnClick="lnkRelayDetails_OnClick" />&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkClinicalDetails" runat="server" Text="Clinical&nbsp;Details" Visible="false" OnClick="lnkClinicalDetails_OnClick" />&nbsp;
                                    <asp:LinkButton ID="lnkPackageDetail" runat="server" Text="Package&nbsp;Details" Visible="false" OnClick="lnkPackageDetails_OnClick" />&nbsp;
                                    <asp:LinkButton ID="lnkCancerScreening" runat="server" Text="Cancer Screening" OnClick="lnkCancerScreening_OnClick" Visible="false" />&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkAddendum" runat="server" Text="Addendum" OnClick="lnkAddendum_OnClick" Visible="false" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="col-md-6 text-right PaddingLeftSpacing">
                                    <asp:Label ID="lblSampleCollectedStatus" runat="server" ForeColor="Maroon" />
                                    <asp:Label ID="lblRejectedSampleStart" runat="server" ForeColor="Maroon" />
                                    <asp:LinkButton ID="lnkBtnRejectedSampleOPCount" runat="server" ToolTip="View details of rejected sample for OP"
                                        Font-Underline="false" OnClick="lnkBtnRejectedSampleOPCount_OnClick" />
                                    <asp:LinkButton ID="lnkBtnRejectedSampleIPCount" runat="server" ToolTip="View details of rejected sample for IP"
                                        Font-Underline="false" OnClick="lnkBtnRejectedSampleIPCount_OnClick" />
                                    <asp:Label ID="lblRejectedSampleEnd" runat="server" ForeColor="Maroon" />
                                    <asp:Label ID="lblStat" runat="server" ForeColor="Maroon" />
                                </div>
                            </div>




                        </div>
                        <div class="col-md-3 text-right">
                            <div class="PD-TabRadio margin_z">
                                <asp:CheckBox ID="chkManualrequest" runat="server" CssClass="radioo" Visible="false" Text="Manual Request" TextAlign="right" />&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                            <div class="PD-TabRadio margin_z">
                                <asp:CheckBox ID="chkOutsourceInvestigations" runat="server" Visible="false" CssClass="radioo" Text="Outsource Test" TextAlign="right" />
                            </div>
                            <asp:Button ID="btnRefresh" runat="server" ToolTip="Refresh" CssClass="btn btn-primary" OnClick="btnRefresh_OnClick" Text="Refresh" OnClientClick="return Validatetextbox()" />
                        </div>
                    </div>






                </asp:Panel>
            </div>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <asp:Panel ID="PanelN" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                            Width="100%" Height="173px" ScrollBars="None">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" CssClass="outLine">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvDetails" Skin="Office2007" Width="100%" BorderWidth="0" AllowFilteringByColumn="false"
                                        Height="190px" AllowMultiRowSelection="False" runat="server" AutoGenerateColumns="false"
                                        ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" AllowPaging="true"
                                        PageSize="6" AllowCustomPaging="true" OnPageIndexChanged="gvDetails_PageIndexChanged"
                                        OnItemCommand="gvDetails_OnItemCommand" OnItemDataBound="gvDetails_ItemDataBound" CssClass="outLine">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                            Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-FrozenColumnsCount="5"
                                            Scrolling-SaveScrollPosition="true">
                                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                        </ClientSettings>
                                        <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">
                                                    No Record Found.
                                                </div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="RegistrationId" DefaultInsertValue="" HeaderText="RegistrationId"
                                                    AllowFiltering="false" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="EncounterId" DefaultInsertValue="" HeaderText="EncounterId"
                                                    AllowFiltering="false" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterId" runat="server" Text='<%#Eval("EncounterId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="FacilityName" DefaultInsertValue="" HeaderText="Facility"
                                                    AllowFiltering="false" HeaderStyle-Width="7%" ItemStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="FacilityId" DefaultInsertValue="" HeaderText="FacilityId"
                                                    AllowFiltering="false" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFacilityId" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Source" HeaderText="Source" DefaultInsertValue=""
                                                    AllowFiltering="false" HeaderStyle-Width="7%" ItemStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--    <telerik:GridBoundColumn UniqueName="EncodedDate" DataField="EncodedDate" DefaultInsertValue=""
                                                        HeaderText="Order Date" AllowFiltering="false" DataType="System.DateTime" HeaderStyle-Width="15%"
                                                        ItemStyle-Width="15%" ItemStyle-Wrap="false" />--%>

                                                <telerik:GridTemplateColumn UniqueName="EncodedDate" DataField="EncodedDate" DefaultInsertValue=""
                                                    HeaderText="Order Date" AllowFiltering="false" DataType="System.DateTime" HeaderStyle-Width="15%"
                                                    ItemStyle-Width="15%" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" AutoPostBackOnFilter="false"
                                                    ShowFilterIcon="false" AllowFiltering="false" FilterControlWidth="99%" HeaderStyle-Width="8%"
                                                    ItemStyle-Width="8%">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLabHeader" runat="server" Text='<%$ Resources:PRegistration, LABNO%>'></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                                    FilterControlWidth="99%" HeaderStyle-Width="12%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ManualLabNo" DefaultInsertValue="" HeaderText='Manual LabNo'
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                                    FilterControlWidth="99%" HeaderStyle-Width="11%" ItemStyle-Width="11%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManualLabNo" runat="server" Text='<%#Eval("ManualLabNo") %>' ToolTip="Manual Lab No." />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                    AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false" FilterControlWidth="99%"
                                                    HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" Text='<%#Eval("RegistrationNo") %>' runat="server"></asp:Label>
                                                        <%--<asp:LinkButton ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>'
                                                                OnClick="lnkVisitHistory_OnClick" ToolTip="View Patient Visit History" />--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                                                    AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false" FilterControlWidth="99%"
                                                    HeaderStyle-Width="25%" ItemStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPatientName" runat="server" CommandName="PatientDetails" Text='<%#Eval("PatientName") %>'></asp:LinkButton>
                                                        <asp:Label ID="lblForNotes" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"
                                                            Style="text-decoration: blink; font-size: large" Text="*" />
                                                        <asp:ImageButton ID="ibtnForNotes" runat="server" ImageUrl="~/Images/NotesNew.png"
                                                            ToolTip="Click to add/show patient notes." OnClick="lbtnNotes_OnClick" />
                                                        <asp:Label ID="lblNotesAvailable" runat="server" Visible="false" Text='<%#Eval("NotesAvailable") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="AgeGender" DefaultInsertValue="" HeaderText="Age/Gender"
                                                    AllowFiltering="false" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="BedNo" DefaultInsertValue="" HeaderText="Bed No."
                                                    AllowFiltering="false" HeaderStyle-Width="8%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbedNo" runat="server" Text='<%#Eval("BedNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="WardName" DefaultInsertValue="" HeaderText="Ward Name"
                                                    AllowFiltering="false" HeaderStyle-Width="9%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardNo" runat="server" Text='<%#Eval("WardName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ReferredBy" DefaultInsertValue="" HeaderText="Referred&nbsp;By"
                                                    AllowFiltering="false" HeaderStyle-Width="18%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReferredBy" runat="server" Text='<%#Eval("ReferredBy") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="MedicalAlert" Visible="true" HeaderText="Alerts"
                                                    DefaultInsertValue="" AllowFiltering="false" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkAlerts" runat="server" Text='<%#Eval("MedicalAlert") %>' OnClick="lnkAlerts_OnClick"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridButtonColumn Text="Select" CommandName="Select" HeaderStyle-Width="8%"
                                                    HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <telerik:GridTemplateColumn UniqueName="Stat" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLabStat" runat="server" Text='<%#Eval("Stat") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-12">
                        <asp:Panel ID="Panel1" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                            Width="100%" Height="187px" ScrollBars="None">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvTestDetails" runat="server" BorderWidth="0" ShowGroupPanel="false"
                                        Skin="Office2007" Width="100%" Height="185px" AllowPaging="false" AllowMultiRowSelection="true"
                                        EnableLinqExpressions="false" AutoGenerateColumns="false" EnableEmbeddedSkins="false"
                                        OnPreRender="gvTestDetails_PreRender" OnItemCommand="gvTestDetails_OnItemCommand"
                                        ClientSettings-EnablePostBackOnRowClick="false" OnItemDataBound="gvTestDetails_ItemDataBound"
                                        OnColumnCreated="gvTestDetails_ColumnCreated" CssClass="outLine">
                                        <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                            Scrolling-SaveScrollPosition="true">
                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                        </ClientSettings>
                                        <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">
                                                    No Record Found.
                                                </div>
                                            </NoRecordsTemplate>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias=":" FieldName="SubName" HeaderText="" FormatString="" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="SubDeptId" SortOrder="None" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <Columns>
                                                <telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderStyle-Width="40px"
                                                    ItemStyle-Width="50px" />
                                                <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation"
                                                    HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'
                                                            OnClick="lblServiceName_OnClick" Font-Bold="true" />
                                                        <asp:HiddenField ID="hdnServiceDetailId" runat="server" Value='<%#Eval("ServiceDetailId") %>' />
                                                        <asp:HiddenField ID="hdnIsClinicalTemplateRequired" runat="server" Value='<%#Eval("IsClinicalTemplateRequired") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="PackageName" DefaultInsertValue="" HeaderText="Package Name"
                                                    HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPackageName" runat="server" Text='<%#Eval("PackageName") %>' />
                                                        <asp:HiddenField ID="HdnSampleId" runat="server" Value='<%#Eval("sampleId") %>' />
                                                        <asp:HiddenField ID="hndPackageId" runat="server" Value='<%#Eval("PackageId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="SampleCollectedDate" DefaultInsertValue=""
                                                    HeaderText="Sample Coll.Date" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSampleCollectedDate" runat="server" Text='<%#Eval("SampleCollectedDate") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DispatchDate" DefaultInsertValue="" HeaderText="Dispatch Date"
                                                    HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("DispatchDate") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn UniqueName="Remarks" DefaultInsertValue="" HeaderText="Instruction"
                                                    HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstForPhlebotomy" runat="server" Text='<%#Eval("InstructionForPhlebotomy") %>' />
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="EntrySiteName" DefaultInsertValue="" HeaderText="Entry Site"
                                                    HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntrySiteName" runat="server" Text='<%#Eval("EntrySiteName") %>' />
                                                        <asp:HiddenField ID="hdnEntrySiteId" runat="server" Value='<%#Eval("EntrySiteId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DailySerialNo" DefaultInsertValue="" HeaderText="Dl.SrNo"
                                                    HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDailySerialNo" runat="server" Text='<%#Eval("DailySerialNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Vacutainer" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVacutainer" runat="server" Text='<%#Eval("VacutainerName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Print" UniqueName="Print"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ibtnPrintLabel" runat="server" ToolTip="Print Label" CommandName="Print"
                                                            SkinID="Button" Text="Label" OnClick="btnPrintLabel_OnClick"></asp:LinkButton>
                                                        <asp:HiddenField ID="HdnPrintLabelStatus" runat="server" Value='<%#Eval("PrintLabelStatus") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridButtonColumn Text="Details" CommandName="Select" HeaderStyle-Width="5%"
                                                    HeaderText="Details" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <telerik:GridTemplateColumn UniqueName="DiagSampleId" DefaultInsertValue="" HeaderText="DiagSampleId"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiagSampleId" runat="server" Text='<%#Eval("DiagSampleId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ServiceID" DefaultInsertValue="" HeaderText="ServiceId"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RefServiceCode" DefaultInsertValue="" HeaderText="RefServiceCode"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRefServiceCode" runat="server" Text='<%#Eval("RefServiceCode") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="StatusColor" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="StatusID" DefaultInsertValue="" HeaderText="StatusID"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusId" runat="server" Text='<%#Eval("StatusID") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="StatusCode" DefaultInsertValue="" HeaderText="StatusCode"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ReviewedStatus" HeaderText="Review" DefaultInsertValue=""
                                                    AllowFiltering="false" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="false" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReviewedStatus" runat="server" Text='<%#Eval("ReviewStatus") %>'
                                                            Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Stat" HeaderText="Stat" DefaultInsertValue=""
                                                    AllowFiltering="false" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStat" runat="server" Text='<%#Eval("Stat") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="row form-group">

                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblToWhomInformed" runat="server" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtToWhomInformed" runat="server" ValidationGroup="decol" AutoComplete="off" MaxLength="100" Visible="false" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-6 PaddingRightSpacing">
                                <asp:Label ID="lblRemarks" runat="server" Text="Reason For Sample De Collect" />
                            </div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlReason" runat="server" Visible="false" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <asp:TextBox ID="txtRemarks" runat="server" ValidationGroup="decol" AutoComplete="off" MaxLength="100" />
                    </div>

                    <asp:RequiredFieldValidator ID="rfvtxtToWhomInformed" runat="server" ControlToValidate="txtToWhomInformed"
                        Display="None" ErrorMessage="Please fill to whom informed" ValidationGroup="decol"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvddlReason" runat="server" ControlToValidate="ddlReason"
                        Display="None" ErrorMessage="Please select Reason." ValidationGroup="decol"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfv11" runat="server" ControlToValidate="txtRemarks"
                        Display="None" ErrorMessage="Remarks could not be left blank" ValidationGroup="decol"></asp:RequiredFieldValidator>
                </div>

                <div class="row form-group" style="display: none;">
                    <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="updatepanelclose" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnclose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                OnClick="btnclose_OnClick" />
                            <asp:Button ID="btnCloseCollect" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                OnClick="btnCloseCollect_OnClick" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                    <asp:HiddenField ID="hdnDailySerialNo" runat="server" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-sm-3 col-4">
                <asp:Label ID="idManual" runat="server" CssClass="LegendColor" BackColor="ControlLight" Text="Manual Request" /></td>
            </div>
            <div class="col-sm-3 col-3">
                <asp:Label ID="lgreviewed" CssClass="LegendColor" runat="server" BackColor="LightGreen" Text="Reviewed" /></td>
            </div>
            <div class="col-sm-6 col-12 table-responsive">
                <ucl:legend ID="Legend1" runat="server" />
            </div>



        </div>
    </div>

</asp:Content>
