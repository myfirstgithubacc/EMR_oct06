<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="WardDetails.aspx.cs" Inherits="WardManagement_WardDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlockStyle" runat="server">
        <%: Styles.Render("~/bundles/WardDetailsStyle") %>
        <style type="text/css">
            http://localhost:534/Include/Master/EMRMaster.master.cs
            /*#ctl00_ContentPlaceHolder1_gvWardDtl_GridData {
                height: 300px !important;
            }*/
            div#ctl00_ContentPlaceHolder1_gvWardDtl_Frozen {
                overflow: hidden !important;
            }
             .dropdown-toggle::after {
            display:none!important;
        }
            div#ctl00_ContentPlaceHolder1_gvWardDtl_FrozenScroll {
                width: 100% !important;
            }
            th.rgHeader{
                    color: black!important;
    font-weight: 700;

            }
            div#ctl00_ContentPlaceHolder1_gvEncounter{
                width:96vw;
            }
            div#ctl00_ContentPlaceHolder1_gvWardDtl {
                margin-right: -19px !important;
            }

            .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
                border: solid #868686 1px !important;
                border-top: none !important;
                border-left: none !important;
                outline: none !important;
                color: #333;
                background: 0 -2300px repeat-x #c1e5ef !important;
            }

            .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {
                background-color: #fff !important;
            }

            #ctl00_ContentPlaceHolder1_Panel1 {
                background-color: #c1e5ef !important;
            }

            .RadGrid .rgFilterBox {
                height: 20px !important;
            }

            .RadGrid_Office2007 .rgFilterRow {
                background: #c1e5ef !important;
            }

            .RadGrid_Office2007 .rgPager {
                background: #c1e5ef 0 -7000px repeat-x !important;
                color: #00156e !important;
            }

            .LegendColor1 {
                padding: 10px !important;
            }

            .back_bg {
                bottom: 0 !important;
            }

            #ctl00_fp1_gvEncounter_GridData {
                height: 42vh !important;
            }

            .form-control-section {
                background: #fff;
                padding: 10px 0;
                border-bottom: 1px solid #ececec;
            }

            /*.hidden-pacdashboard {
                display: none;
            }*/

            .container-fluid #divPatient {
                background-color: #fffaf0;
                border-bottom: 1px solid #f5deb3 !important;
            }

            .badge-section {
                border: 1px solid #fefefe;
            }


            ul.list-group li {
                float: left;
                list-style: none;
            }



                ul.list-group span, ul.list-group li span {
                    border: 0;
                }

            ul.list-group li {
                border: 1px solid #ccc;
                margin-top: 5px;
            }

                ul.list-group li input {
                    float: left;
                    margin: 2px 0 2px 4px;
                }

            .LegendColor1 {
                padding: 2px 5px !important;
            }

            button.slide-toggle {
                background: #e66f2e;
                border: 0;
                padding: 2px 8px;
                /*margin-left: 10px;*/
                border-radius: 0 0 4px 4px;
                color: #fff;
            }

            .table-grid-custom {
                width: 100%;
            }

            .rgPager td {
                border: 0 !important;
            }

            /*.check-custom { padding-left: 10px; margin-top: 4px !important;}*/

            div#ctl00_ContentPlaceHolder1_menuStatus_detached .rmScrollWrap.rmGroup.rmLevel1 {
                width: 100% !important;
                height: 72vh;
            }

            .rmScrollWrap.rmGroup.rmLevel1 ul {
                width: 100% !important;
            }

            ul.rmActive.rmVertical li {
                clear: none !important;
            }

            div#ctl00_ContentPlaceHolder1_menuStatus_detached {
                left: 6% !important;
                right: 6% !important;
                width: 88% !important;
                top: 10% !important;
                bottom: 10% !important;
            }

            ul.list-group {
                display: block !important;
            }

           ul.rmActive.rmVertical a.rmLink {
                width: 250px !important;
            }
            ul.rmRootScrollGroup a.rmLink {
                width:auto!important;
                white-space:nowrap!important;
            }

            .RadMenu .rmGroup .rmText {
                padding: 0px 0px 0px 5px !important;
            }

            a.rmLink {
                color:black!important;
                white-space: normal !important;
               
                font-family: sans-serif !important;
                font-weight: 500;
                font-size: 12px !important;
                line-height: 22px !important;
            }

               
            .RadMenu_Metro .rmGroup .rmLink {
                margin: 2px 20px;
                border-bottom: 1px solid #677cbe;
               
               
            }
                .RadMenu_Metro .rmGroup .rmLink:hover{
                    box-shadow:1px 1px 1px 1px #677cbe;
                }

            .RadMenu_Metro .rmGroup .rmDisabled, .RadMenu_Metro .rmGroup .rmDisabled:hover {
                color: #ccc !important;
                background-color: transparent;
                border-bottom: 1px solid #ccc !important;
                
            }

            @media screen and (min-device-width : 460px) and (max-device-width : 1024px) {
                div#ctl00_ContentPlaceHolder1_menuStatus_detached {
                    height: 80vh !important;
                    overflow-y: auto;
                }

                .RadMenu_Metro .rmGroup .rmLink {
                    margin: 2px 3px;
                }

                .rmScrollWrap.rmGroup.rmLevel1 {
                    height: 80vh !important;
                    overflow-y: auto !important;
                }
            }

            div.loader {
                width: 154px;
                position: absolute;
                bottom: 0;
                height: 60px;
                left: 500px;
                top: 300px;
                z-index: 99999;
            }

            .box-inner .check-custom input[type="checkbox"] ~ span {
                margin-top: 0 !important;
            }

            .check-custom input[type="checkbox"] {
                vertical-align: top;
                margin-top: 2px !important;
            }

            .table-grid-custom .rgHeaderDiv {
                table-layout: auto !important;
            }

                .table-grid-custom .rgHeaderDiv table {
                    overflow: auto !important;
                }


            @media screen and (max-device-width : 1024px) {

                .table-grid-custom {
                    width: 95vw;
                }

                .gridview .rgHeaderDiv, .gridview .rgDataDiv {
                    overflow: inherit !important;
                }

                    .gridview .rgHeaderDiv table, .gridview .rgDataDiv table {
                        table-layout: auto !important;
                    }

                    .gridview .rgHeaderDiv tr, .gridview .rgDataDiv tr {
                        display: inline-flex;
                        padding: 0 !important;
                    }

                        .gridview .rgHeaderDiv tr th, .gridview .rgDataDiv tr td {
                            display: table-cell;
                            width: 150px !important;
                            float: left;
                        }
            }

            @media screen and (max-device-width : 768px) {
                div.loader {
                    width: 154px;
                    position: absolute;
                    bottom: 0;
                    height: 60px !important;
                    left: 250px !important;
                    top: 300px !important;
                    z-index: 99999;
                }
            }

            @media screen and (max-width: 768px) {

                .check-custom {
                    height: auto !important;
                }

                [value="Filter"] {
                    width: 100%;
                }
            }


            a.rmBottomArrowDisabled{
                top:0!important;
                bottom:0!important;
            }
        </style>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
        <%--<link rel="stylesheet" type="text/css" href="../Include/css/bootstrap.min.css" />--%>
        <link rel="stylesheet" type="text/css" href="../Include/css/mainNew.css" />
        <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" />
        <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    </telerik:RadCodeBlock>

    <asp:HiddenField ID="hdnFacilityName" runat="server" Value="" />
    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>

            <div class="container-fluid">


                <div class="row">
                    <asp:Panel ID="pnlFilter" runat="server" Width="100%" DefaultButton="btnSearch">

                        <div id="divPatient" style="display: block; /*background-color: #f5deb3!important; */">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12" style="padding-top: 4px; padding-bottom: 4px;">
                                        Patient Name:
                                    <asp:Label ID="jlblPatientname" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                Age/Gender:
                                <asp:Label ID="jlblAGe" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                              <%--  MRD No.:--%>
                                        <asp:Label ID="jlblUHID" runat="server" Text="<%$ Resources:PRegistration, UHID%>"></asp:Label>
                                        <asp:Label ID="jlblMRD" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                Enc #.:
                                <asp:Label ID="jlblEnc" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                 Bed No. :
                                <asp:Label ID="jlblBed" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                 Ward Name :
                                <asp:Label ID="jlblWard" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                     Billing Category :
                                <asp:Label ID="jlblBillCat" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;     
                                Mobile :
                                <asp:Label ID="jlblMob" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;                             
                                    <span style="background-color: red;">
                                        <font color="white"><b>Package : <asp:Label ID="jlblpack" runat="server" ForeColor="white" Text="" Font-Bold="true" /></b></font>
                                    </span>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12 col-sm-12" style="padding-top: 4px; padding-bottom: 4px;">
                                        Secondary Doctor :
                                <asp:Label ID="jlblDoc" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                Company :
                                <asp:Label ID="jlblComp" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                Address :
                                <asp:Label ID="jlblAddress" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        VulnerableType :
                                 <asp:Label ID="jlblVulnerableType" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                Assigned Nurse :
                                <asp:Label ID="jlblAssignedNurseName" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                        &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblProvisionalBillAmt" runat="server" Text="Provisional Bill Amt :" Visible="false" />
                                        <asp:Label ID="jlblProvisionalBillAmt" runat="server" Text="" Font-Bold="true" ForeColor="#990066" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDepositAmt" runat="server" Text="Deposit Amt :" Visible="false" />
                                        <asp:Label ID="jlblDepositAmt" runat="server" Text="" Font-Bold="true" ForeColor="#990066" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblApprovedAmt" runat="server" Text="Approved Amt :" Visible="false" />
                                        <asp:Label ID="jlblApprovedAmt" runat="server" Text="" Font-Bold="true" ForeColor="#990066" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblBalanceAmt" runat="server" Text="Balance Amt :" Visible="false" />
                                        <asp:Label ID="jlblBalanceAmt" runat="server" Text="" Font-Bold="true" ForeColor="#990066" Visible="false" />
                                        <asp:Label ID="lbldSpecialisation" runat="server" Text="Specialisation :" />
                                        &nbsp;&nbsp;
                                    <asp:Label ID="lblDoctorSpecialisation" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />

                                    </div>
                                </div>

                            </div>
                            <div class="clearfix"></div>
                        </div>






                        <div class="box" style="display: none;">
                            <div class="box-inner">
                                <div class="col-md-12 col-sm-12 col-xs-12 form-control-section">

                                    <div class="row">

                                        <div class="col-lg-3 col-md-4 col-sm-6 col-6 form-group">
                                            <div class="row">
                                                <div class="col-sm-3 label2">
                                                    <asp:Label ID="lblWard" runat="server" Text="Ward&nbsp;Name" />
                                                </div>
                                                <div class="col-sm-9" style="display: inherit;">
                                                    <telerik:RadComboBox ID="ddlWard" Skin="Default" runat="server" Width="87%" MaxHeight="220px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlWard_OnSelectedIndexChanged"
                                                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
                                                    <asp:HiddenField ID="hdnStatusCode" runat="server" Value="" />
                                                    <asp:ImageButton ID="imgSaveFavouriteWardTagging" ToolTip="Save Favourite Ward" Width="23px" Height="20px" runat="server" ImageUrl="~/Images/save.gif" OnClick="imgSaveFavouriteWardTagging_Click" Style="vertical-align: top; margin-top: 2px;" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-lg-3 col-md-4 col-sm-6 col-6 form-group">
                                            <div class="row">
                                                <div class="col-sm-3 label2 text-nowrap">
                                                    <asp:Label ID="Label18" Skin="Default" runat="server" Text="Bed Status" />
                                                </div>
                                                <div class="col-sm-9">
                                                    <telerik:RadComboBox ID="ddlBedStatus" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlbedstatus_SelectedIndexChanged">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="All" Value="" Font-Bold="true" ForeColor="Black" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-lg-3 col-md-4 col-sm-6 col-6 form-group" style="display: inherit;">
                                            <div class="row">
                                                <div class="col-sm-3 label2">
                                                    <asp:Label ID="Label1" runat="server" Text="Status&nbsp;" />
                                                </div>
                                                <div class="col-sm-9" style="display: inherit;">
                                                    <telerik:RadComboBox ID="ddlEncounterStatus" Skin="Default" runat="server" Width="87%" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlEncounterStatus_OnSelectedIndexChanged">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text='<%# Eval("StatusColor") %>' Value="" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                    <asp:LinkButton ID="lnkAdvanceSearch" Height="20px" Width="27px" CssClass="btn btn-primary fa fa-search" runat="server" ToolTip="Advance Search"
                                                        OnClick="lnkAdvanceSearch_OnClick" Style="vertical-align: top; margin-top: 1px;" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-3 col-md-4 col-sm-6 col-6 form-group">
                                            <div class="row">
                                                <div class="col-sm-3 label2">
                                                    <asp:Label ID="lblStation" runat="server" Text="Station&nbsp;Name" />
                                                </div>
                                                <div class="col-sm-9">
                                                    <telerik:RadComboBox ID="ddlStation" Skin="Default" runat="server" Width="100%" Height="220px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStation_OnSelectedIndexChanged" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-lg-5 col-md-4 col-sm-9 col-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12 checkbox checkbox-inline check-custom">
                                                    <asp:CheckBox ID="chkNotAck" runat="server" AutoPostBack="true" OnCheckedChanged="btnGo_OnClick" TextAlign="Right" Text="Un-Acknowledged" />
                                                    <asp:CheckBox ID="chkReferral" runat="server" AutoPostBack="true" OnCheckedChanged="chkReferral_OnClick" TextAlign="Right" Text="Referral" />
                                                    <asp:CheckBox ID="chkVIP" runat="server" AutoPostBack="true" OnCheckedChanged="chkVIP_OnClick" TextAlign="Right" Text="VIP" />
                                                    <asp:CheckBox ID="chkUnAssignedToStaff" runat="server" AutoPostBack="true" Text="Nurse not assigned" ToolTip="To filter only un-assigned patients" OnCheckedChanged="chkUnAssignedToStaff_CheckedChanged" />
                                                    <asp:CheckBox ID="chkMLC" runat="server" Text="MLC" AutoPostBack="true" OnCheckedChanged="chkMLC_CheckedChanged" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-lg-1 col-md-2 col-sm-3 col-4 form-group">
                                            <div class="row">
                                                <div class="col-sm-12 label2" style="margin-left: -15px;">
                                                    <asp:CheckBox ID="chkDischarge" runat="server" CssClass="checkbox" AutoPostBack="true" OnCheckedChanged="chkDischarge_OnCheckedChanged" TextAlign="Left" Text="Discharge" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-3 col-md-4 col-sm-6 col-8 form-group">
                                            <div class="row">

                                                <div class="col-6">
                                                    <div class="row">
                                                        <div class="col-3 label2">
                                                            <asp:Label ID="Label17" runat="server" Text="From" />
                                                        </div>
                                                        <div class="col-9">
                                                            <telerik:RadDatePicker ID="txtFromDate" runat="server" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" DateInput-ReadOnly="false" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-6">
                                                    <div class="row">
                                                        <div class="col-3 label2">
                                                            <asp:Label ID="Label2" runat="server" Text="To" />
                                                        </div>
                                                        <div class="col-9">
                                                            <telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" DateInput-ReadOnly="false" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>



                                        <div class="col-lg-3 col-md-4 col-sm-6 col-12 form-group text-right">
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn btn-primary" Style="width: auto;" Text="Filter" OnClick="btnGo_OnClick" />
                                        </div>

                                        <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12 form-group">
                                            <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xl-8 col-lg-8 col-md-6 col-sm-6 col-4" style="float: left;">
                                <button type="button" class="slide-toggle">Search Filter</button>
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-6 col-sm-6 col-8 form-group" style="float: right;">
                                <div class="row">
                                    <div class="col-3 label2">
                                        <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                                    </div>
                                    <div class="col-9" style="display: inherit;">
                                        <telerik:RadComboBox ID="ddlName" Skin="Default" runat="server" Width="50%" DropDownWidth="105px" AutoPostBack="false" Style="display: inline-block;">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                                <%-- <telerik:RadComboBoxItem Text="MRD No" Value="R" />--%>
                                                <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, UHID%>" Value="R" />
                                                <telerik:RadComboBoxItem Text="Encounter No." Value="ENC" />
                                            </Items>
                                        </telerik:RadComboBox>

                                        <asp:Panel ID="Panel3" runat="server" Skin="Default" Width="50%" DefaultButton="btnGo" Style="display: inline-block; margin-top: 1px;">
                                            <asp:TextBox ID="txtSearchContent" runat="server" Width="100%" OnClientSelectedIndexChanged="ddlName_OnClientSelectedIndexChanged" />
                                            <asp:Button ID="btnSearch" runat="server" Width="0px" OnClick="btnSearch_OnClick" Height="0px" BackColor="Transparent" Style="visibility: hidden; height: 0px; float: left;" />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>




                        <div id="dvAdvanceSearch" runat="server" visible="false" style="width: 400px; z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed; top: 25%; height: 180px; left: 35%;">
                            <table width="100%" cellspacing="4" cellpadding="4" style="margin-left: 35px">
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="Label19" runat="server" Style="font-size: 14px; font-weight: bold;" Text="Advance Search" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAll" runat="server" Text="All Admitted Patients" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text='<%$ Resources:PRegistration, specialisation%>' />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlSpecilization" runat="server" Filter="Contains" Skin="Metro" AppendDataBoundItems="true"
                                            Height="210px" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlSpecilization_SelectedIndexChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProvider" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlProvider" runat="server" Skin="Metro" Filter="Contains"
                                            Height="210px" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnUpdate" CssClass="btn btn-primary" Width="100px" runat="server" OnClick="btnUpdate_OnClick" Text="Filter" />
                                        <asp:Button ID="btnClose" CssClass="btn btn-primary" Width="100px" runat="server" Text="Close" OnClick="btnClose_OnClick" />
                                    </td>
                                </tr>
                            </table>
                        </div>


                    </asp:Panel>
                </div>

            </div>


            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 box-care-line p-t-b-5">
                        <p>
                            <asp:LinkButton ID="lnkPatientCarePlan" runat="server" Font-Bold="true" ForeColor="Black" Text="Care Plan" OnClick="lnkPatientCarePlan_Click" />
                            <asp:Label ID="lblNoOfCarePlan" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkBtnReferralRequestHistory" runat="server" ToolTip="Referral" Font-Bold="true" ForeColor="Black" Text="Referral" OnClick="ReferralRequestHistory_OnClick" />
                            <asp:Label ID="lblReferralRequestCount" runat="server" ForeColor="Maroon" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkBedClearance" runat="server" ToolTip="ICU Clearance" Font-Bold="true" ForeColor="Black" Text="ICU Clearance" OnClick="lnkBedClearance_OnClick" />
                            <asp:Label ID="lblBedClearanceCount" runat="server" ForeColor="Maroon" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkBedTransfer" runat="server" Font-Bold="true" ForeColor="Black" Text="Bed Transfer" OnClick="lnkBedTransfer_OnClick" />
                            <asp:Label ID="lblNoOfBedTransfer" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkbloodRe" runat="server" Font-Bold="true" ForeColor="Black" Text="&nbsp;&nbsp;Blood Request" OnClick="lnkbloodRe_OnClick" />
                            <asp:Label ID="lblNoOfRequest" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkunperformedSer" Font-Bold="true" runat="server" Text="&nbsp;&nbsp;Pending Orders" ForeColor="Black" OnClick="lnkunperformedSer_Click" />
                            <asp:Label ID="lblunperformedSer" Font-Size="11px" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkdrugordercount" Font-Size="12px" Font-Bold="true" runat="server" Text="&nbsp;&nbsp;Drug Order" ForeColor="Black" OnClick="lnkdrugordercount_Click" />
                            <asp:Label ID="lbldrugordercount" Font-Size="11px" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkEstimationOrder" Font-Size="12px" Font-Bold="true" runat="server" Text="&nbsp;&nbsp;Exceeded LOS" ForeColor="Black" OnClick="lnkEstimationOrder_Click" />
                            <asp:Label ID="lblEstimationOrder" Font-Size="11px" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnknondrugordercount" Font-Bold="true" runat="server" Text="&nbsp;&nbsp;Non Drug Order" ForeColor="Black" OnClick="lnknondrugordercount_Click" />
                            <asp:Label ID="lblnondrugordercount" Font-Size="11px" Visible="true" runat="server" />
                        </p>
                        <p>
                            <asp:Label ID="lblRejectedSampleStart" runat="server" ForeColor="Black" Font-Bold="true" />
                            <asp:LinkButton ID="lnkBtnRejectedSampleIPCount" runat="server" ForeColor="Black" Font-Bold="true" ToolTip="View details of rejected sample for IP" Font-Underline="false" OnClick="lnkBtnRejectedSampleIPCount_OnClick" />
                            <asp:Label ID="lblRejectedSampleEnd" Font-Size="11px" runat="server" ForeColor="Maroon" />
                        </p>
                        <p>
                            <asp:LinkButton ID="lnkOralGiven" runat="server" Font-Bold="true" ForeColor="Black" Text="&nbsp;&nbsp;Oral Given" OnClick="lnkOralGiven_Click" />
                            <asp:Label ID="lblOralGiven" runat="server" />
                        </p>
                    </div>
                </div>


                <div class="row">
                    <div class="gridview table-responsive" id="dvWardDtl" runat="server">
                        <telerik:RadGrid ID="gvWardDtl" CssClass="table-grid-custom " runat="server" RenderMode="Lightweight"
                            Height="375px" AutoGenerateColumns="false" ItemStyle-Height="30px"
                            BorderWidth="0" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                            ShowStatusBar="true" EnableLinqExpressions="false" AllowPaging="true" AllowAutomaticDeletes="false"
                            ShowFooter="false" PageSize="10" AllowCustomPaging="true" OnPageIndexChanged="gvWardDtl_OnPageIndexChanged"
                            OnItemDataBound="gvWardDtl_OnItemDataBound" MasterTableView-TableLayout="Auto" AllowSorting="true" OnPreRender="gvWardDtl_OnPreRender">
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <ClientEvents OnRowSelected="RowSelected" />
                                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true" />

                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" FrozenColumnsCount="4" />
                            </ClientSettings>
                            <MasterTableView TableLayout="Auto" Width="100%">
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="EncounterId" SortOrder="Descending" />
                                </SortExpressions>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Patient" HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="Patient_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("Patient_Name") %>' />
                                            <asp:ImageButton ID="imgVIP" runat="server" ImageUrl="~/Images/VIP.png" Width="18px" Height="10px" ToolTip="VIP" Visible="false" />
                                            <asp:ImageButton ID="imgHandleWithCare" runat="server" ImageUrl="~/Images/HandleWithCare.ico" Width="18px" Height="10px" ToolTip="Handle With Care" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Age/Gender" HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="AgeGender">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="RegistrationNo" HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="RegistrationNo" />

                                    <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="10%" ItemStyle-Width="10%" SortExpression="EncounterNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                            <asp:ImageButton ID="imgBedTransfer" runat="server" ImageUrl="~/Images/HospitalBed.png" Width="18px" Height="10px" ToolTip="Bed Transfer" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="Admission_Date" HeaderText="Admission Time" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="14%" ItemStyle-Width="14%" SortExpression="Admission_Date" />
                                    <telerik:GridBoundColumn DataField="BedNo" HeaderText="Bed No." HeaderStyle-Width="10%" ItemStyle-Width="10%" SortExpression="BedNo" />
                                    <telerik:GridBoundColumn DataField="Doctor_Name" HeaderText="Doctor Name" HeaderStyle-Width="15%" ItemStyle-Width="15%" SortExpression="Doctor_Name" />
                                    <telerik:GridBoundColumn DataField="EncounterStatus" HeaderText="Encounter Status" HeaderStyle-Width="15%" ItemStyle-Width="15%" />

                                    <telerik:GridBoundColumn DataField="WardName" HeaderText="Ward Name" Visible="true" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                    <telerik:GridBoundColumn DataField="PatientAddress" HeaderText="Address" Visible="false" HeaderStyle-Width="20%" ItemStyle-Width="20%" />
                                    <telerik:GridBoundColumn DataField="SecondaryDoctorName" HeaderText="Secondary Doctor" Visible="false" HeaderStyle-Width="15%" ItemStyle-Width="15%" />
                                    <telerik:GridBoundColumn DataField="Payername" HeaderText="Company" Visible="false" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                    <telerik:GridBoundColumn DataField="CompanyType" HeaderText="Type" Visible="true" HeaderStyle-Width="9%" ItemStyle-Width="9%" />
                                    <telerik:GridTemplateColumn HeaderText="EWS" HeaderTooltip="Early Warning Score" ItemStyle-Font-Bold="true" HeaderStyle-Width="4%" ItemStyle-Width="4%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" SortExpression="EWSTotal">
                                        <ItemTemplate>
                                            <span id="spanEWS" style="cursor: pointer" onclick="showEWSPopup();">
                                                <%--  <asp:Label ID="lblEWSTotal" runat="server" Text='<%#Eval("EWSTotal")%>' />--%>
                                            </span>
                                            <asp:LinkButton ID="lnkEWSScore" Visible="false" ToolTip="Eearly Warning Score" runat="server" Font-Size="" OnClick="lnkEWSScore_Click" Text='<%#Eval("EWSTotal")%>'>&nbsp;
                                            <%-- <asp:ImageButton ID="ibtnEWSPopup" runat="server" Visible="false" ToolTip="Early Warnning Score" Height="15px" ImageUrl="~/Images/PopUp.jpg" OnClick="ibtnEWSPopup_Click" />--%>
                                            </asp:LinkButton>
                                            <asp:ImageButton ID="ibtnEWSGraphPopup" runat="server" Visible="false" ToolTip="EWS Graph" OnClick="ibtnEWSGraphPopup_Click"
                                                Width="13px" Height="13px" ImageUrl="~/Images/Growth-Chart.png" />
                                            <asp:HiddenField ID="hdnEWSEncodedBy" runat="server" Value='<%#Eval("EWSEncodedBy") %>' />
                                            <asp:HiddenField ID="hdnEWSEncodedDate" runat="server" Value='<%#Eval("EWSEncodedDate") %>' />
                                            <asp:HiddenField ID="hdnEWSTemplateId" runat="server" Value='<%#Eval("TemplateId") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Notification" HeaderStyle-Width="11%" ItemStyle-Width="11%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgAllergyAlert1" runat="server" ImageUrl="~/Icons/allergy.gif"
                                                Width="13px" Height="13px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert1_OnClick" />
                                            <asp:ImageButton ID="imgMedicalAlert1" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                Width="13px" Height="13px" ToolTip="Patient Alert" OnClick="imgMedicalAlert1_OnClick" />
                                            <asp:ImageButton ID="imgUnPerformService" runat="server" ImageUrl="~/Images/Lab.png"
                                                Width="13px" Height="13px" ToolTip="View Pending Orders" OnClick="imgUnPerformService_OnClick" />
                                            <asp:ImageButton ID="imgReffHistory" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                Width="13px" Height="13px" ToolTip="Referral History" Visible='<%#(common.myBool(Eval("IsReferralCase")))%>'
                                                OnClick="imgReffHistory_OnClick" />
                                            <asp:ImageButton ID="imgCriticalLabResult" runat="server" ImageUrl="~/Images/PendingBill.png"
                                                Width="13px" Height="13px" ToolTip='<%$ Resources:PRegistration, PanicValue%>' OnClick="imgCriticalLabResult_OnClick" />
                                            <asp:ImageButton ID="imgRejectedDrugs" runat="server" ImageUrl="~/Images/RejectedDrugs.png"
                                                Width="13px" Height="13px" ToolTip="Rejected Drugs" OnClick="imgRejectedDrugs_OnClick" />
                                            <asp:ImageButton ID="imgRejectedSample" runat="server" ImageUrl="~/Images/RejectedSample.png"
                                                Width="13px" Height="13px" ToolTip="Rejected Sample" OnClick="imgRejectedSample_OnClick" />
                                            <asp:ImageButton ID="imgUnAssignedNurse" runat="server" ImageUrl="~/Icons/unAssignedNurse.png"
                                                Width="13px" Height="13px" ToolTip="Nurse not assigned" OnClick="imgUnAssignedNurse_Click" />
                                         <%--   <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Icons/NUTRITIONALASSESSMENT.png"
                                            Height="15px" ToolTip="Nutritional Assessment" Visible='<%#(common.myBool(Eval("ISNUTRITIONALASSESSMENT")))%>'/>--%>

                                            <asp:ImageButton ID="imgCarePlan" runat="server" ImageUrl="~/Icons/CarePlan.png"
                                                Width="13px" Height="13px" ToolTip="Care Plan" OnClick="imgCarePlan_Click" />

                                            <asp:Label ID="lblProbableDischarge" runat="server" Font-Bold="true" BackColor="YellowGreen" Text="PD" Visible="false" Font-Size="Smaller" ToolTip="Probable Discharge" />
                                            <asp:Label ID="lblMLCPatient" runat="server" Font-Bold="true" BackColor="Plum" Text="MLC" Visible="false" Font-Size="Smaller" ToolTip="MLC Patient" />

                                            <asp:HiddenField ID="hdnBedId" runat="server" Value='<%#Eval("BedId") %>' />
                                            <asp:HiddenField ID="hdnBedStatusColor" runat="server" Value='<%#Eval("BedStatusColor") %>' />
                                            <asp:HiddenField ID="hdnBedCatg" runat="server" Value='<%#Eval("BedCatg") %>' />
                                            <asp:HiddenField ID="hdnwardNo" runat="server" Value='<%#Eval("wardNo") %>' />
                                            <asp:HiddenField ID="hdnBedCategoryName" runat="server" Value='<%#Eval("BedCategoryName") %>' />
                                            <asp:HiddenField ID="hdnBedStatus" runat="server" Value='<%#Eval("BedStatus") %>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value='<%#Eval("RegistrationNo") %>' />
                                            <asp:HiddenField ID="hdnEncounterStatusColor" runat="server" Value='<%#Eval("EncounterStatusColor") %>' />
                                            <asp:HiddenField ID="hdnEncounterStatusID" runat="server" Value='<%#Eval("EncounterStatusID") %>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%#Eval("EncounterNo") %>' />
                                            <asp:HiddenField ID="hdnBedCategoryNameForDisplay" runat="server" Value='<%#Eval("BedCategoryNameForDisplay") %>' />
                                            <asp:HiddenField ID="hdnEntrySite" runat="server" Value='<%#Eval("EntrySite") %>' />
                                            <asp:HiddenField ID="hdnConsultingDoctorId" runat="server" Value='<%#Eval("ConsultingDoctorId") %>' />
                                            <asp:HiddenField ID="hdnMobileNo" runat="server" Value='<%#Eval("MobileNo") %>' />
                                            <asp:HiddenField ID="hdnBillCat" runat="server" Value='<%#Eval("BedCategoryNameForDisplay") %>' />

                                            <asp:HiddenField ID="hdnAgeGender" runat="server" Value='<%#Eval("AgeGender") %>' />
                                            <asp:HiddenField ID="hdnPayername" runat="server" Value='<%#Eval("Payername") %>' />
                                            <asp:HiddenField ID="hdnIsHandleWithCare" runat="server" Value='<%#Eval("IsHandleWithCare") %>' />
                                            <asp:HiddenField ID="hdnIsVIPPatient" runat="server" Value='<%#Eval("IsVIPPatient") %>' />
                                            <asp:HiddenField ID="hdnAllergiesAlert" runat="server" Value='<%#Eval("AllergiesAlert") %>' />
                                            <asp:HiddenField ID="hdnMedicalAlert" runat="server" Value='<%#Eval("MedicalAlert") %>' />
                                            <asp:HiddenField ID="hdnOTBillClearancePending" runat="server" Value='<%#Eval("OTBillClearancePending") %>' />
                                            <asp:HiddenField ID="hdnBloodBankRequests" runat="server" Value='<%#Eval("BloodBankRequests") %>' />
                                            <asp:HiddenField ID="hdnMLC" runat="server" Value='<%#Eval("MLC") %>' />
                                            <asp:HiddenField ID="hdnUnPerformedServices" runat="server" Value='<%#Eval("UnPerformedServices") %>' />
                                            <asp:HiddenField ID="hdnCriticalLabResult" runat="server" Value='<%#Eval("CriticalLabResult") %>' />
                                            <asp:HiddenField ID="hdnPatientName" runat="server" Value='<%#Eval("Patient_Name") %>' />
                                            <asp:HiddenField ID="hdnIsAcknowledged" runat="server" Value='<%#Eval("IsAcknowledged") %>' />
                                            <%--palendra--%>
                                            <asp:HiddenField ID="hdnInterHospitalTranfer" runat="server" Value='<%#Eval("InterHospTransfer") %>' />
                                            <asp:HiddenField ID="hdnIsSecondaryFacility" runat="server" Value='<%#Eval("IsSecondaryFacility") %>' />
                                            <asp:HiddenField ID="hdnPrimaryFacility" runat="server" Value='<%#Eval("PrimaryFacility") %>' />
                                            <%--palendra--%>
                                            <asp:HiddenField ID="hdnIsUnAckBedTransfer" runat="server" Value='<%#Eval("IsUnAckBedTransfer") %>' />
                                            <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%#Eval("PackageId") %>' />
                                            <asp:HiddenField ID="hdnIsPanel" runat="server" Value='<%#Eval("IsPanel") %>' />
                                            <asp:HiddenField ID="hdnRefrralDoctor" runat="server" Value='<%#Eval("ReferralDoctorName") %>' />
                                            <asp:HiddenField ID="hdnPatientAddress" runat="server" Value='<%#Eval("PatientAddress") %>' />
                                            <asp:HiddenField ID="hdnSecondaryDoctorName" runat="server" Value='<%#Eval("SecondaryDoctorName") %>' />
                                            <asp:HiddenField ID="hdnRejectedDrugs" runat="server" Value='<%#Eval("RejectedDrugs") %>' />
                                            <asp:HiddenField ID="hdnAllowPanelExcludedItems" runat="server" Value='<%#Eval("AllowPanelExcludedItems") %>' />
                                            <asp:HiddenField ID="hdnPackageIdDetails" runat="server" Value='<%#Eval("PackageIdDetails") %>' />
                                            <asp:HiddenField ID="hdnDiagSample" runat="server" Value='<%#Eval("DiagSample") %>' />
                                            <asp:HiddenField ID="hdnVulnerableType" runat="server" Value='<%#Eval("VulnerableType") %>' />
                                            <asp:HiddenField ID="hdnIsDischargeSummaryFinalized" runat="server" Value='<%#Eval("IsDischargeSummaryFinalized") %>' />
                                            <asp:HiddenField ID="hdnISGeneralWard" runat="server" Value='<%#Eval("ISGeneralWard") %>' />
                                            <asp:HiddenField ID="hdnProbableDischarge" runat="server" Value='<%#Eval("ProbableDischarge") %>' />
                                            <asp:HiddenField ID="hdnCurrentWard" runat="server" Value='<%#Eval("WardName")%>' />
                                            <asp:HiddenField ID="hdnIsUnAssigned" runat="server" Value='<%#Eval("IsUnAssigned") %>' />
                                            <asp:HiddenField ID="hdnAssignedNurseName" runat="server" Value='<%#Eval("AssignedNurseName") %>' />
                                            <asp:HiddenField ID="hdnIsCarePlan" runat="server" Value='<%#Eval("IsCarePlan") %>' />

                                            <asp:HiddenField ID="hdnDepositAmount" runat="server" Value='<%#Eval("DepositAmount")%>' />
                                            <asp:HiddenField ID="hdnApprovedAmount" runat="server" Value='<%#Eval("TotalTreatmentLimitAmount")%>' />
                                            <asp:HiddenField ID="hdnProvisionalBillAmt" runat="server" Value='<%#Eval("ProvisionalBillAmt")%>' />
                                            <asp:HiddenField ID="hdnBalanceAmt" runat="server" Value='<%#Eval("BalanceAmt")%>' />
                                            <asp:HiddenField ID="hdnSponsorIdPayorId" runat="server" Value='<%#Eval("SponsorIdPayorId") %>' />
                                            <asp:HiddenField ID="hdnVIPNarration" runat="server" Value='<%#Eval("VIPNarration") %>' />
                                            <asp:HiddenField ID="hdnDoctorSpecialisation" runat="server" Value='<%#Eval("DoctorSpecialisation") %>' />

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>

                    <telerik:RadContextMenu ID="menuStatus" EnableOverlay="true" EnableTheming="true" runat="server" EnableRoundedCorners="false"
                        EnableShadows="false" Width="100%" ClickToOpen="true" CollapseAnimation-Type="InBounce" EnableAutoScroll="true" EnableImageSprites="false" DefaultGroupSettings-Flow="Horizontal" DefaultGroupSettings-RepeatColumns="4" DefaultGroupSettings-RepeatDirection="Horizontal" Skin="Metro" EnableScreenBoundaryDetection="true" DefaultGroupSettings-ExpandDirection="Left" OnItemClick="menuStatus_ItemClick" DefaultGroupSettings-Width="100%" />
                    <asp:HiddenField ID="hdnMnuRegId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuEncId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuRegNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuEncNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuPatName" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuDocName" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuEncDate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuBedId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuBedNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuDoctorId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuWardName" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuMobileNo" runat="server" Value="" />

                    <asp:HiddenField ID="hdnMnuBillCat" runat="server" Value="" />

                    <asp:HiddenField ID="hdnMnuPayername" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuAgeGender" runat="server" Value="" />
                    <asp:HiddenField ID="hdnInvoiceId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuIsAcknowledged" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuIsUnAckBedTransferk" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuAllergyAlert" runat="server" />
                    <asp:HiddenField ID="hdnMnuMedicalAlert" runat="server" />
                    <asp:HiddenField ID="hdnMnuPackageId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuIsPanel" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuSecondaryDoctorName" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuPatientAdd" runat="server" Value="" />
                    <asp:HiddenField ID="hdnAllowPanelExcludedItems" runat="server" Value="" />
                    <asp:HiddenField ID="hdnIsHandleWithCare" runat="server" Value="" />
                    <asp:HiddenField ID="hdnPackageIdDetails" runat="server" Value="" />
                    <asp:HiddenField ID="hdnWardNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnVulnerableType" runat="server" Value="" />
                    <asp:HiddenField ID="hdnIsGeneralWard" runat="server" Value="" />
                    <asp:HiddenField ID="hdnApolloDhaka" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnMnuAssignedNurseName" runat="server" Value="" />

                    <asp:HiddenField ID="hdnMnuDepositAmount" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuApprovedAmount" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuProvisionalBillAmt" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuBalanceAmt" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuSponsorIdPayorId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuDoctorSpecialisation" runat="server" Value="" />
                    <%--palendra--%>
                    <asp:HiddenField ID="hdnMnuIsSecondaryFacility" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMnuPrimaryFacility" runat="server" Value="" />
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close,pin,maximize,minimize,move" InitialBehaviors="Maximize" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12" id="dvDischarge" runat="server">
                        <telerik:RadGrid ID="gvEncounter" runat="server" RenderMode="Lightweight" Skin="Office2007"
                            PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                            AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                            AllowPaging="true" AllowAutomaticDeletes="false" Height="450px"
                            ShowFooter="false" PageSize="5" AllowCustomPaging="true" OnItemCommand="gvEncounter_OnItemCommand"
                            OnPageIndexChanged="gvEncounter_OnPageIndexChanged" OnItemDataBound="gvEncounter_OnItemDataBound" AllowSorting="true" OnPreRender="gvEncounter_OnPreRender">
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <ClientEvents OnRowSelected="RowSelectedDischarge" />
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" FrozenColumnsCount="4" />
                            </ClientSettings>
                            <PagerStyle ShowPagerText="true" />
                            <MasterTableView TableLayout="Auto" GroupLoadMode="Client">
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="DischargeDate" SortOrder="Descending" />
                                </SortExpressions>
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                                </NoRecordsTemplate>
                                <ItemStyle Wrap="true" />
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderText="Edit" HeaderStyle-Width="30px"
                                        ItemStyle-Width="30px" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="IbtnSelect" runat="server" Text="Edit" CommandName="Select" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="OPIP" HeaderText="OP/IP" ShowFilterIcon="false"
                                        Visible="false" DefaultInsertValue="" DataField="OPIP" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo" SortExpression="RegistrationNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterNo" AllowFiltering="false" SortExpression="EncounterNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="Name" HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                            <asp:HiddenField ID="hdnPatientAddress" runat="server" Value='<%#Eval("PatientAddress")%>' />
                                            <asp:HiddenField ID="hdnKinName" runat="server" Value='<%#Eval("KinName")%>' />
                                            <asp:HiddenField ID="hdnSecondaryDoctorName" runat="server" Value='<%#Eval("SecondaryDoctorName")%>' />
                                            <asp:HiddenField ID="hdnCurrentWard" runat="server" Value='<%#Eval("CurrentWard")%>' />
                                            <asp:HiddenField ID="hdnCommonRemarks" runat="server" Value='<%#Eval("CommonRemarks")%>' />
                                            <asp:HiddenField ID="hdnBedCategoryNameForDisplay" runat="server" Value='<%#Eval("BedCategoryNameForDisplay") %>' />
                                            <%--  <asp:HiddenField ID="hdnBillCat" runat="server" Value='<%#Eval("BedCategoryNameForDisplay") %>' />--%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="GenderAge" AllowFiltering="false" SortExpression="GenderAge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="DoctorName" SortExpression="DoctorName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>' />
                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="CurrentBedNo" HeaderText="Bed No" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="CurrentBedNo" SortExpression="CurrentBedNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="EncDate" HeaderText="Admission Date" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="EncDate" AllowFiltering="false" SortExpression="EncDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncDate" runat="server" Text='<%#Eval("EncDate")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DischargeStatus" HeaderText="Discharge Status"
                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="DischargeStatus" SortExpression="DischargeStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDischargeStatus" runat="server" Text='<%#Eval("DischargeStatus")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterStatus" HeaderText="Encounter Status"
                                        Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterStatus"
                                        AllowFiltering="false" SortExpression="EncounterStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterStatus" runat="server" Text='<%#Eval("EncounterStatus")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="CompanyName" HeaderText="Company" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="CompanyName" AllowFiltering="false" SortExpression="CompanyName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PhoneHome" HeaderText="PhoneHome" ShowFilterIcon="false"
                                        Visible="false" DefaultInsertValue="" DataField="PhoneHome" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhoneHome" runat="server" Text='<%#Eval("PhoneHome")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="MobileNo" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="MobileNo" AllowFiltering="false" SortExpression="MobileNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DOB" HeaderText="DOB" ShowFilterIcon="false"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" Visible="false" DefaultInsertValue=""
                                        DataField="DOB" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PatientAddress" HeaderText="PatientAddress"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" ShowFilterIcon="false" AllowFiltering="false"
                                        DefaultInsertValue="" DataField="PatientAddress" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientAddress" runat="server" Text='<%#Eval("PatientAddress")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="REGID" HeaderText="REGID" Visible="false"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="REGID" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("REGID")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ENCID" HeaderText="ENCID" Visible="false"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="ENCID" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblENCID" runat="server" Text='<%#Eval("ENCID")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="CompanyCode" HeaderText="CompanyCode" Visible="false"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="CompanyCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyCode" runat="server" Text='<%#Eval("CompanyCode")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="InsuranceCode" HeaderText="InsuranceCode"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" Visible="false" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="InsuranceCode" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInsuranceCode" runat="server" Text='<%#Eval("InsuranceCode")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="CardId" HeaderText="CardId" Visible="false"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="CardId" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCardId" runat="server" Text='<%#Eval("CardId")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="RowNo" HeaderText="RowNo" Visible="false"
                                        HeaderStyle-Width="0%" FilterControlWidth="0%" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="RowNo" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DischargeDate" HeaderText="Discharge Date"
                                        Visible="true" ShowFilterIcon="false" DefaultInsertValue="" DataField="DischargeDate"
                                        AllowFiltering="false" SortExpression="DischargeDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDischargeDate" runat="server" Text='<%#Eval("DischargeDate")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="FinalizeDate" HeaderText="FinalizeDate" Visible="true"
                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="FinalizeDate" AllowFiltering="false" SortExpression="FinalizeDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalizeDate" runat="server" Text='<%#Eval("FinalizeDate")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" Visible="true"
                                        AllowFiltering="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="Status" SortExpression="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalize" runat="server" Text='<%#Eval("Status")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-12 badge-section">

                        <ul class="list-group">
                            <li>
                                <asp:Label ID="lblrowCount" runat="server" Text="" CssClass="LegendColor" /></li>
                            <li>
                                <asp:Label ID="idManual" runat="server" CssClass="LegendColor" BackColor="#ff99ff" Text="Un-Acknowledged Patients" /></li>
                            <li>
                                <asp:Label ID="lblVIP" runat="server" CssClass="LegendColor LegendColor1" BackColor="YELLOW" Height="" Text="VIP" /></li>
                            <li>
                                <asp:Label ID="lblVipHandleWithCaseLegend" runat="server" CssClass="LegendColor" BackColor="#fa8072" Text="Handle With Care / Vulnerable Patient" /></li>
                            <li>
                                <asp:Label ID="lblIsDischargeSummary" Text="Discharge Summary Finalized" runat="server" BackColor="#CCFFFF" CssClass="LegendColor" /></li>
                            <li>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Icons/allergy.gif"
                                    Width="14px" Height="14px" ToolTip="Allergy Alert" /><asp:Label ID="Label5" runat="server" CssClass="LegendColor" Text="Allergy Alert" /></li>

                            <li>
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                    Width="14px" Height="14px" ToolTip="Patient Alert" /><asp:Label ID="Label4" runat="server" CssClass="LegendColor" Text="Patient Alert" /></li>

                            <li>
                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/PendingBill.png"
                                    Width="14px" Height="14px" ToolTip='<%$ Resources:PRegistration, PanicValue%>' /><asp:Label ID="Label12" runat="server" CssClass="LegendColor" Text='<%$ Resources:PRegistration, PanicValue%>' /></li>

                            <li>
                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/RejectedDrugs.png"
                                    Width="14px" Height="14px" ToolTip="Rejected Drugs" /><asp:Label ID="Label8" runat="server" CssClass="LegendColor" Text="Rej. Drugs" /></li>

                            <li>
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/Lab.png"
                                    Width="14px" Height="14px" ToolTip="View UnPerformance Service" /><asp:Label ID="Label6" runat="server" CssClass="LegendColor" Text="UnPerformance Service" /></li>
                            <li>
                                <asp:ImageButton ID="imgreferrel" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                    Style="width: 14px; height: 14px;" ToolTip="Referal History" /><asp:Label ID="Label9" runat="server" CssClass="LegendColor" Text="Referal History" /></li>
                            <li>
                                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/RejectedSample.png"
                                    Style="width: 14px; height: 14px;" ToolTip="Rejected Sample" /><asp:Label ID="Label10" runat="server" CssClass="LegendColor" Text="Rej. Sample" /></li>

                            <li>
                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Icons/unAssignedNurse.png"
                                    Style="width: 14px; height: 14px;" ToolTip="Nurse not assigned" />
                                <asp:Label ID="Label11" runat="server" CssClass="LegendColor" Text="Nurse Not Assigned" /></li>
                        </ul>


                    </div>
                </div>


            </div>


            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menuStatus" />
            <asp:AsyncPostBackTrigger ControlID="gvWardDtl" />
            <asp:AsyncPostBackTrigger ControlID="gvEncounter" />
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="lnkAdvanceSearch" />
            <asp:PostBackTrigger ControlID="chkDischarge" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="updMain"
        DisplayAfter="1000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div class="loader" >
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="divEWS" draggable="true" class="modal">

        <div class="modal-content">
            <span class="close">&times;</span>

            <div style="margin: 10px; font-size: 14px" class="text-left">

                <table id="myTable" border="1" style="padding: 10px; text-align: left; border-color: black;">
                    <thead style="background-color: #ffa500; font-size: 16px; font-weight: bold; color: black">
                        <tr>
                            <td style="width: 25%; padding-left: 5px">Eearly Warning Score</td>
                            <td style="width: 70%; padding-left: 5px">Mandatory Action</td>
                        </tr>
                    </thead>
                    <tbody style="font-size: 14px; font-weight: bold; color: black">
                    </tbody>
                </table>


            </div>

        </div>

    </div>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

        <script src="../Include/JS/jquery-1.8.2.js" type="text/javascript"></script>
        <script src="../Include/JS/jquery.pos.js" type="text/javascript"></script>
        <script src="../Include/bootstrap4/js/bootstrap.min.js"></script>
        <script type="text/javascript">
            var CtrlEventKey = "";

            $(function () {
                $('#<%=txtSearchContent.ClientID %>').focus();
                // $(document).pos();
                $(document).on('scan.pos.barcode', function (event) {

                    //access `event.code` - barcode data
                    //alert(event.code.val);
                    var ScannedVal = $('#<%=txtSearchContent.ClientID %>').val();
                    var scanerVal = event.code;
                    scanerVal = scanerVal.replace("\r\n", "");
                    if (ScannedVal != scanerVal) {
                        $('#<%=txtSearchContent.ClientID %>').val("");
                        $('#<%=txtSearchContent.ClientID %>').val(scanerVal);
                        $get('<%=btnUpdate.ClientID%>').click();
                        $('#<%=txtSearchContent.ClientID %>').focus();
                    }
                });

                function GetEWSSetupData() {
                    $.ajax({
                        type: "POST",
                        url: "/Shared/Services/PatientDashboardasmx.asmx/GetEWSData",

                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var result = JSON.parse(response.d);
                            $.each(result, function (i, item) {
                                // console.log(result[i]);
                                $("<tr  valign='top' >").html("<td style='padding-left:5px'>" + result[i].ScoreValue + "</td><td style='padding-left:5px'>" + result[i].MandatoryAction + "</td>").appendTo('#myTable');
                            });
                        },
                        failure: function (response) {
                            console.log(response);
                        }
                    });
                }


                //GetEWSSetupData();
            });

            function showEWSPopup() {

                var modal = document.getElementById('divEWS');
                modal.style.display = "block";

                var span = document.getElementsByClassName("close")[0];
                span.onclick = function () {
                    modal.style.display = "none";
                }
            }
            document.addEventListener('keydown', function (event) {
                if (event.keyCode == 17 || event.keyCode == 74) {
                    if (event.keyCode == 17) {
                        CtrlEventKey = event.keyCode;
                    }
                    if (CtrlEventKey == 17 && event.keyCode == 74) {
                        CtrlEventKey = "";
                        event.preventDefault();
                    }

                }
            });

        </script>


        <script type="text/javascript">
            function showMenu(e, menu, RegId, EncId, RegNo, EncNo, PatName, DocName, EncDate, BedId, BedNo, DoctorId, WardName, MobileNo, BedCategoryNameForDisplay, Payername, AgeGender, IsAcknowledged, IsUnAckBedTransferk, PackageId, StatusCode, IsPanel, SecDoc, PatientAdd, AllowPanelExcludedItems, PackageIdDetails, WardNo, VulnerableType, AllergiesAlert, MedicalAlert, ISGeneralWard, rowIdx, AssignedNurseName, DepositAmount, ApprovedAmount, ProvisionalBillAmt, BalanceAmt, SponsorIdPayorId, InterHospTranfer, IsSecondaryFacility, PrimaryFacility, DoctorSpecialisation) {
                debugger;
                $get('<%=hdnMnuRegId.ClientID%>').value = RegId;
                $get('<%=hdnMnuEncId.ClientID%>').value = EncId;
                $get('<%=hdnMnuRegNo.ClientID%>').value = RegNo;
                $get('<%=hdnMnuEncNo.ClientID%>').value = EncNo;
                $get('<%=hdnMnuPatName.ClientID%>').value = PatName;
                $get('<%=hdnMnuDocName.ClientID%>').value = DocName;
                $get('<%=hdnMnuEncDate.ClientID%>').value = EncDate;
                $get('<%=hdnMnuBedId.ClientID%>').value = BedId;
                $get('<%=hdnMnuBedNo.ClientID%>').value = BedNo;
                $get('<%=hdnMnuDoctorId.ClientID%>').value = DoctorId;
                $get('<%=hdnMnuWardName.ClientID%>').value = WardName;
                $get('<%=hdnMnuMobileNo.ClientID%>').value = MobileNo;

                $get('<%=hdnMnuBillCat.ClientID%>').value = BedCategoryNameForDisplay;

                $get('<%=hdnMnuPayername.ClientID%>').value = Payername;
                $get('<%=hdnMnuAgeGender.ClientID%>').value = AgeGender;
                $get('<%=hdnMnuIsAcknowledged.ClientID%>').value = IsAcknowledged;

                $get('<%=hdnMnuIsUnAckBedTransferk.ClientID%>').value = IsUnAckBedTransferk;

                $get('<%=hdnMnuPackageId.ClientID%>').value = PackageId;

                $get('<%=hdnMnuIsPanel.ClientID%>').value = IsPanel;
                $get('<%=hdnMnuSecondaryDoctorName.ClientID%>').value = SecDoc;

                $get('<%=hdnMnuPatientAdd.ClientID%>').value = PatientAdd;
                $get('<%=hdnPackageIdDetails.ClientID%>').value = PackageIdDetails;
                $get('<%=hdnAllowPanelExcludedItems.ClientID%>').value = AllowPanelExcludedItems;
                $get('<%=hdnWardNo.ClientID%>').value = WardNo;
                $get('<%=hdnVulnerableType.ClientID%>').value = VulnerableType;
                $get('<%=hdnMnuAllergyAlert.ClientID%>').value = AllergiesAlert;
                $get('<%=hdnMnuMedicalAlert.ClientID%>').value = MedicalAlert;
                $get('<%=hdnIsGeneralWard.ClientID%>').value = ISGeneralWard;
                $get('<%=hdnMnuAssignedNurseName.ClientID%>').value = AssignedNurseName;
                $get('<%=hdnMnuDepositAmount.ClientID%>').value = DepositAmount;
                $get('<%=hdnMnuApprovedAmount.ClientID%>').value = ApprovedAmount;
                $get('<%=hdnMnuProvisionalBillAmt.ClientID%>').value = ProvisionalBillAmt;
                $get('<%=hdnMnuBalanceAmt.ClientID%>').value = BalanceAmt;
                $get('<%=hdnMnuSponsorIdPayorId.ClientID%>').value = SponsorIdPayorId;
                $get('<%=hdnMnuDoctorSpecialisation.ClientID%>').value = DoctorSpecialisation;

                $get('<%=hdnMnuIsSecondaryFacility.ClientID%>').value = IsSecondaryFacility;
                $get('<%=hdnMnuPrimaryFacility.ClientID%>').value = PrimaryFacility;

                var menu = $find("<%=menuStatus.ClientID %>");
                if (IsAcknowledged == "False") {

                    var items = menu.get_items();
                    for (var i = 0; i < items.get_count() ; i++) {

                        var VAL = menu.findItemByValue(items.getItem(i).get_value());
                        var menuItem = menu.findItemByText(items.getItem(i).get_text());
                        menuItem.enable();
                        if (items.getItem(i).get_text() != "Acknowledge Patient") {
                            menuItem.disable();
                        }
                        if (IsUnAckBedTransferk == "True") {
                            menuItem.disable();
                        }
                        //else { menuItem.enable(); }
                    }
                }
                else if (InterHospTranfer == "True") {

                    var items = menu.get_items();
                    for (var i = 0; i < items.get_count() ; i++) {

                        var VAL = menu.findItemByValue(items.getItem(i).get_value());
                        var menuItem = menu.findItemByText(items.getItem(i).get_text());
                        debugger
                        var listItem = menuItem.get_attributes().getAttribute("InterhospTransEnable");
                        menuItem.enable();
                        if (listItem != "True") {
                            menuItem.disable();
                        }
                        if (IsUnAckBedTransferk == "True") {
                            menuItem.disable();
                        }
                        //else { menuItem.enable(); }
                    }
                }
                else {

                    var items = menu.get_items();
                    for (var i = 0; i < items.get_count() ; i++) {

                        var VAL = menu.findItemByValue(items.getItem(i).get_value());
                        var FacilityName = $get('<%=hdnFacilityName.ClientID%>').value
                        var menuItem = menu.findItemByText(items.getItem(i).get_text());
                        menuItem.enable();
                        if (FacilityName.indexOf('Venkateshwar') >= 0) {
                            if (StatusCode == "184" || StatusCode == "200" || StatusCode == "628" || StatusCode == "185" || StatusCode == "199" || StatusCode == "186" || StatusCode == "8") {
                                menuItem.enable();
                                if (VAL._text == "Bed Transfer" || VAL._text == "Bed Transfer Requisition" || VAL._text == "Blood Acknowledge" || VAL._text == "Blood Component Return" || VAL._text == "Blood Release Requisition" || VAL._text == "Blood Requisition"
                                    || VAL._text == "Blood Transfusion Details" || VAL._text == "Change Encounter Status" || VAL._text == "Consumable Order" || VAL._text == "Diet Requisition" || VAL._text == "Doctor Visit" || VAL._text == "Drug Administration" || VAL._text == "Drug/Consumable Return" || VAL._text == "Drug Order"
                                    || VAL._text == "Drug Acknowledge" || VAL._text == "Referral Slip" || VAL._text == "Service Order" || VAL._text == "Send to OT" || VAL._text == "Drug/Consumable Order") {

                                    //                        if (VAL._text == "Bed Transfer" || VAL._text == "Bed Transfer Requisition" || VAL._text == "Blood Acknowledge" || VAL._text == "Blood Component Return" || VAL._text == "Blood Release Requisition" || VAL._text == "Blood Requisition"
                                    //|| VAL._text == "Blood Transfusion Details" || VAL._text == "Consumable Order" || VAL._text == "Diet Requisition" || VAL._text == "Doctor Visit" || VAL._text == "Drug Administration" || VAL._text == "Drug/Consumable Return" || VAL._text == "Drug Order"
                                    //|| VAL._text == "Drug Acknowledge" || VAL._text == "Referral Slip" || VAL._text == "Service Order" || VAL._text == "Send to OT" || VAL._text == "Drug/Consumable Order") {
                                    menuItem.disable();
                                }
                                else { menuItem.enable(); }

                            }
                            else { menuItem.enable(); }
                        }
                        else {
                            if (StatusCode == "184" || StatusCode == "197" || StatusCode == "200" || StatusCode == "628" || StatusCode == "185" || StatusCode == "199" || StatusCode == "186" || StatusCode == "8") {
                                menuItem.enable();
                                if (VAL._text == "Bed Transfer" || VAL._text == "Bed Transfer/Interchange" || VAL._text == "Bed Transfer Requisition" || VAL._text == "Blood Acknowledge" || VAL._text == "Blood Component Return" || VAL._text == "Blood Release Requisition" || VAL._text == "Blood Requisition"
                                    || VAL._text == "Blood Transfusion Details" || VAL._text == "Change Encounter Status" || VAL._text == "Consumable Order" || VAL._text == "Diet Requisition" || VAL._text == "Doctor Visit" || VAL._text == "Drug Administration" || VAL._text == "Drug/Consumable Return" || VAL._text == "Drug Order"
                                    || VAL._text == "Drug Acknowledge" || VAL._text == "Referral Slip" || VAL._text == "Service Order" || VAL._text == "Send to OT" || VAL._text == "Drug/Consumable Order" || VAL._text == "OT Request") {

                                    menuItem.disable();
                                }
                                else { menuItem.enable(); }
                            }
                            else { menuItem.enable(); }
                        }

                        //if (items.getItem(i).get_text() == "Acknowledge Patient") {
                        //    menuItem.disable();
                        //}
                        if (IsUnAckBedTransferk == "True") {
                            menuItem.disable();
                        }
                    }
                }
            menu.show(e);
            var grid = $find("<%=gvWardDtl.ClientID%>").get_masterTableView();
            var Index = rowIdx;
            grid.selectItem(Index);
            var row = grid.get_dataItems()[Index];

        }

        function showMenuDischarge(e, menu, RegId, EncId, RegNo, EncNo, PatName, DocName, EncDate, BedId, BedNo, DoctorId, WardName, MobileNo, Payername, AgeGender, IsAcknowledged, IsUnAckBedTransferk, PackageId, StatusCode, IsPanel, SecDoc, PatientAdd, AllowPanelExcludedItems, PackageIdDetails, VulnerableType, rowIdx, BedCategoryName) {

            $get('<%=hdnMnuRegId.ClientID%>').value = RegId;
            $get('<%=hdnMnuEncId.ClientID%>').value = EncId;
            $get('<%=hdnMnuRegNo.ClientID%>').value = RegNo;
            $get('<%=hdnMnuEncNo.ClientID%>').value = EncNo;
            $get('<%=hdnMnuPatName.ClientID%>').value = PatName;
            $get('<%=hdnMnuDocName.ClientID%>').value = DocName;
            $get('<%=hdnMnuEncDate.ClientID%>').value = EncDate;
            $get('<%=hdnMnuBedId.ClientID%>').value = BedId;
            $get('<%=hdnMnuBedNo.ClientID%>').value = BedNo;
            $get('<%=hdnMnuDoctorId.ClientID%>').value = DoctorId;
            $get('<%=hdnMnuWardName.ClientID%>').value = WardName;
            $get('<%=hdnMnuMobileNo.ClientID%>').value = MobileNo;

            $get('<%=hdnMnuPayername.ClientID%>').value = Payername;
            $get('<%=hdnMnuAgeGender.ClientID%>').value = AgeGender;
            $get('<%=hdnMnuIsAcknowledged.ClientID%>').value = IsAcknowledged;

            $get('<%=hdnMnuIsUnAckBedTransferk.ClientID%>').value = IsUnAckBedTransferk;

            $get('<%=hdnMnuPackageId.ClientID%>').value = PackageId;

            $get('<%=hdnMnuIsPanel.ClientID%>').value = IsPanel;
            $get('<%=hdnMnuSecondaryDoctorName.ClientID%>').value = SecDoc;

            $get('<%=hdnMnuPatientAdd.ClientID%>').value = PatientAdd;
            $get('<%=hdnPackageIdDetails.ClientID%>').value = PackageIdDetails;
            $get('<%=hdnAllowPanelExcludedItems.ClientID%>').value = AllowPanelExcludedItems;
            $get('<%=hdnVulnerableType.ClientID%>').value = VulnerableType;
            $get('<%=hdnMnuBillCat.ClientID%>').value = BedCategoryName;
            var menu = $find("<%=menuStatus.ClientID %>");
            if (IsAcknowledged == "False") {

                var items = menu.get_items();
                for (var i = 0; i < items.get_count() ; i++) {

                    var VAL = menu.findItemByValue(items.getItem(i).get_value());
                    var menuItem = menu.findItemByText(items.getItem(i).get_text());


                    if (items.getItem(i).get_text() != "Acknowledge Patient") {
                        menuItem.disable();
                    }
                    if (IsUnAckBedTransferk == "True") {
                        menuItem.disable();
                    }
                    //else { menuItem.enable(); }
                }
            }
            else {

                var items = menu.get_items();
                for (var i = 0; i < items.get_count() ; i++) {

                    var VAL = menu.findItemByValue(items.getItem(i).get_value());
                    var menuItem = menu.findItemByText(items.getItem(i).get_text());

                    if (StatusCode == "184" || StatusCode == "197" || StatusCode == "200" || StatusCode == "628" || StatusCode == "185" || StatusCode == "199" || StatusCode == "186" || StatusCode == "8") {
                        menuItem.enable();
                        if (VAL._text == "Bed Transfer" || VAL._text == "Bed Transfer/Interchange" || VAL._text == "Bed Transfer Requisition" || VAL._text == "Blood Acknowledge" || VAL._text == "Blood Component Return" || VAL._text == "Blood Release Requisition" || VAL._text == "Blood Requisition"
                            || VAL._text == "Blood Transfusion Details" || VAL._text == "Change Encounter Status" || VAL._text == "Consumable Order" || VAL._text == "Diet Requisition" || VAL._text == "Doctor Visit" || VAL._text == "Drug Administration" || VAL._text == "Drug/Consumable Return" || VAL._text == "Drug Order"
                            || VAL._text == "Drug Acknowledge" || VAL._text == "Referral Slip" || VAL._text == "Service Order" || VAL._text == "Send to OT" || VAL._text == "Drug/Consumable Order" || VAL._text == "OT Request") {

                            menuItem.disable();
                        }
                        else { menuItem.enable(); }
                    }
                    else { menuItem.enable(); }

                    //if (items.getItem(i).get_text() == "Acknowledge Patient") {
                    //    menuItem.disable();
                    //}
                    if (IsUnAckBedTransferk == "True") {
                        menuItem.disable();
                    }
                }
            }
            menu.show(e);
            var grid = $find("<%=gvEncounter.ClientID%>").get_masterTableView();
            var Index = rowIdx;
            grid.selectItem(Index);
            var row = grid.get_dataItems()[Index];
        }


        function showDemographicDetails(e, menu, RegId, EncId, RegNo, EncNo, PatName, DocName, EncDate, BedNo, DoctorId, WardName, MobileNo, BedCategoryNameForDisplay, Payername, AgeGender, IsAcknowledged, IsUnAckBedTransfer, PackageId, StatusCode, IsPanel, SecDoc, PatientAdd, AllowPanelExcludedItems, PackageIdDetails, VulnerableType, AllergiesAlert, MedicalAlert, ISGeneralWard, rowIdx, AssignedNurseName, DepositAmount, ApprovedAmount, ProvisionalBillAmt, BalanceAmt) {

            $get('<%=hdnMnuRegId.ClientID%>').value = RegId;
                $get('<%=hdnMnuEncId.ClientID%>').value = EncId;
                $get('<%=hdnMnuRegNo.ClientID%>').value = RegNo;
                $get('<%=hdnMnuEncNo.ClientID%>').value = EncNo;
                $get('<%=hdnMnuPatName.ClientID%>').value = PatName;
                $get('<%=hdnMnuDocName.ClientID%>').value = DocName;
                $get('<%=hdnMnuEncDate.ClientID%>').value = EncDate;
                $get('<%=hdnMnuBedNo.ClientID%>').value = BedNo;
                $get('<%=hdnMnuDoctorId.ClientID%>').value = DoctorId;
                $get('<%=hdnMnuWardName.ClientID%>').value = WardName;
                $get('<%=hdnMnuMobileNo.ClientID%>').value = MobileNo;
                $get('<%=hdnMnuBillCat.ClientID%>').value = BedCategoryNameForDisplay;

                $get('<%=hdnMnuPayername.ClientID%>').value = Payername;
                $get('<%=hdnMnuAgeGender.ClientID%>').value = AgeGender;
                $get('<%=hdnMnuIsAcknowledged.ClientID%>').value = IsAcknowledged;
                $get('<%=hdnMnuIsUnAckBedTransferk.ClientID%>').value = IsUnAckBedTransfer;

                $get('<%=hdnMnuPackageId.ClientID%>').value = PackageId;
                $get('<%=hdnMnuIsPanel.ClientID%>').value = IsPanel;
                $get('<%=hdnMnuSecondaryDoctorName.ClientID%>').value = SecDoc;
                $get('<%=hdnMnuPatientAdd.ClientID%>').value = PatientAdd;
                $get('<%=hdnPackageIdDetails.ClientID%>').value = PackageIdDetails;
                $get('<%=hdnAllowPanelExcludedItems.ClientID%>').value = AllowPanelExcludedItems;
                $get('<%=hdnVulnerableType.ClientID%>').value = VulnerableType;

                $get('<%=hdnMnuAllergyAlert.ClientID%>').value = AllergiesAlert;
                $get('<%=hdnMnuMedicalAlert.ClientID%>').value = MedicalAlert;
                $get('<%=hdnIsGeneralWard.ClientID%>').value = ISGeneralWard;
                $get('<%=hdnMnuAssignedNurseName.ClientID%>').value = AssignedNurseName;
                $get('<%=hdnMnuDepositAmount.ClientID%>').value = DepositAmount;
                $get('<%=hdnMnuApprovedAmount.ClientID%>').value = ApprovedAmount;
                $get('<%=hdnMnuProvisionalBillAmt.ClientID%>').value = ProvisionalBillAmt;
                $get('<%=hdnMnuBalanceAmt.ClientID%>').value = BalanceAmt;

            }

            function OnClearClientClose(oWnd) {
                $get('<%=btnSearch.ClientID%>').click();
            }
            function ddlName_OnClientSelectedIndexChanged(sender, args) {
                document.getElementById('btnGo.ClientID').click();
            }
            function RowSelectedDischarge(sender, eventArgs) {
                var e = eventArgs.get_domEvent();

                ShowPatientDetails();
            }
            function RowSelected(sender, eventArgs) {
                var e = eventArgs.get_domEvent();

                ShowPatientDetails();
            }
            function ShowPatientDetails() {
                debugger;
                document.getElementById('divPatient').style.display = "block";
                //alert("Row: " + eventArgs.get_itemIndexHierarchical() + " selected.");
                $get('<%=jlblPatientname.ClientID%>').textContent = $get('<%=hdnMnuPatName.ClientID%>').value;
                $get('<%=jlblAGe.ClientID%>').textContent = $get('<%=hdnMnuAgeGender.ClientID%>').value;
                $get('<%=jlblMRD.ClientID%>').textContent = $get('<%=hdnMnuRegNo.ClientID%>').value;
                $get('<%=jlblEnc.ClientID%>').textContent = $get('<%=hdnMnuEncNo.ClientID%>').value;
                $get('<%=jlblDoc.ClientID%>').textContent = $get('<%=hdnMnuSecondaryDoctorName.ClientID%>').value;
                $get('<%=jlblAddress.ClientID%>').textContent = $get('<%=hdnMnuPatientAdd.ClientID%>').value;
                $get('<%=jlblBed.ClientID%>').textContent = $get('<%=hdnMnuBedNo.ClientID%>').value;
                $get('<%=jlblMob.ClientID%>').textContent = $get('<%=hdnMnuMobileNo.ClientID%>').value;
                //added by bhakti
                $get('<%=jlblBillCat.ClientID%>').textContent = $get('<%=hdnMnuBillCat.ClientID%>').value;
                $get('<%=jlblVulnerableType.ClientID%>').textContent = $get('<%=hdnVulnerableType.ClientID%>').value;


                if ($get('<%=hdnPackageIdDetails.ClientID%>').value == 0) {
                    $get('<%=jlblpack.ClientID%>').textContent = "No";
                }
                else {
                    $get('<%=jlblpack.ClientID%>').textContent = "Yes";
                }

                <%-- if ($get('<%=hdnMnuPackageId.ClientID%>').value == 0)
                {
                    $get('<%=jlblpack.ClientID%>').textContent = "No";
                }
                else
                {
                    $get('<%=jlblpack.ClientID%>').textContent = "Yes";
                }
                $get('<%=jlblpack.ClientID%>').textContent = $get('<%=hdnMnuPackageId.ClientID%>').value;--%>

                $get('<%=jlblComp.ClientID%>').textContent = $get('<%=hdnMnuPayername.ClientID%>').value;
                $get('<%=jlblWard.ClientID%>').textContent = $get('<%=hdnMnuWardName.ClientID%>').value;
                $get('<%=jlblAssignedNurseName.ClientID%>').textContent = $get('<%=hdnMnuAssignedNurseName.ClientID%>').value;
                $get('<%=lblDoctorSpecialisation.ClientID%>').textContent = $get('<%=hdnMnuDoctorSpecialisation.ClientID%>').value;
                $get('<%=jlblDepositAmt.ClientID%>').textContent = $get('<%=hdnMnuDepositAmount.ClientID%>').value;
                $get('<%=jlblApprovedAmt.ClientID%>').textContent = $get('<%=hdnMnuApprovedAmount.ClientID%>').value;
                $get('<%=jlblBalanceAmt.ClientID%>').textContent = $get('<%=hdnMnuBalanceAmt.ClientID%>').value;
                $get('<%=jlblProvisionalBillAmt.ClientID%>').textContent = $get('<%=hdnMnuProvisionalBillAmt.ClientID%>').value;

            }

            function pageLoad() {
                $(".slide-toggle").click(function () {
                    $(".box").slideToggle();
                });
            }

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
