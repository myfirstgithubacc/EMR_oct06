<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FindPatient.ascx.cs" Inherits="Include_Components_MasterComponent_FindPatient" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Import Namespace="System.Web.Optimization" %>--%>
<%--<link href="Include/css/font-awesome.css" rel="stylesheet" />
<link href="Include/css/font-awesome.min.css" rel="stylesheet" />
<link href="../../../Icons/css/font-awesome.css" rel="stylesheet" />
--%>
<link href="../../../Icons/css/font-awesome.min.css" rel="stylesheet" />
<link href="../../New/font-awesome.css" rel="stylesheet" />
<link href="../../New/font-awesome.min.css" rel="stylesheet" />
<%--<link href="../../css/bootstrap.min.css" rel="stylesheet" />--%>
<link href="../../bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
<link href="../../css/mainNew.css" rel="stylesheet" />
<link href="../../EMRStyle.css" rel="stylesheet" />
<telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
    <%-- <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/EMRMasterWithTopDetailsCss") %>
    </asp:PlaceHolder>--%>
    <style type="text/css">
        .style1 {
            width: 50%;
        }

        .style2 {
            width: 100%;
        }

        div#ctl00_fp1_pnlGrid {
            height: 48vh !important;
        }
        /*sanyamtanwar*/
        input#ctl00_fp1_txtSearchN {
            padding: 4.2px 10px !important;
        }
       
        /*sanyamtanwar*/
        .FindPatiendCheckBox > * {
            float: left;
            border: 1px solid red;
        }

        .FindPatiendCheckBox {
            
            padding: 0px 5px;
            font-size: 11px;
            min-height: 28px;
        }
        

        .FindPatiendCheckBox  {
            background: #8fbc8f;
            padding: 0px 5px;
            font-size: 11px;
            min-height: 28px;
        }

        .FindPatiendCheckBox h4 {
            background: #c9ffe5;
            padding: 0px 5px;
            font-size: 11px;
            min-height: 28px;
        }

        .FindPatiendCheckBox h5 {
            background: #dda0dd;
            padding: 0px 5px;
            font-size: 11px;
            min-height: 28px;
        }

        .FindPatiendCheckBox h3 {
            background: #ffe4c4;
            padding: 0px 5px;
            font-size: 11px;
            min-height: 28px;
        }

        .FindPatiendCheckBox [type="checkbox"] {
            margin-top: 5px;
        }

        .FindPatiendCheckBox label {
            color: #000;
            font-weight: normal;
            padding-left: 5px;
        }

        .RadComboBox .rcbArrowCell a {
            height: auto !important;
        }

        .VitalHistory-Div01 .btn-chotu {
            margin-top: 10px;
        }

        div#ctl00_fp1_ddlLocation input.rcbInput {
            padding: 4px 10px !important;
        }

        div#ctl00_fp1_pnlGrid {
            height: 62vh !important;
        }

        #ctl00_rdpAppList tr.rspSlideHeader {
            display: none !important;
        }

        .findPatientSelect td.rcbArrowCell.rcbArrowCellRight {
            bottom: 0;
            right: 10px;
        }

        span.findPatientSelect {
            position: relative;
        }

        div#ctl00_fp1_pnlsearch td.rcbInputCell.rcbInputCellLeft .rcbInput {
            padding: 4px 10px !important;
        }

        div#ctl00_fp1_pnlsearch div#ctl00_fp1_ddlLocation input.rcbInput {
            padding: 4px 10px !important;
        }

        #ctl00_fp1_pnlsearch .RadComboBox {
            border: 0;
        }

        input#ctl00_fp1_txtSearchN {
            padding: 5px 10px;
            border: 1px solid #ccc;
            border-radius: 3px;
        }

        .form-group {
    margin-bottom: 5px!important;
}

        .VitalHistory-Div01 .btn.btn-chotu {
    margin-top: 0;
}

        .hidden
        {
            display:none;
        }
        span.mlabel {
margin: 8px 10px;
display: inline-block;
}
    </style>
</telerik:RadScriptBlock>
<style type="text/css">
    .blink {
        text-decoration: blink;
        color: Green;
    }

    .btn-chotu {
        padding: 1px 8px !important;
    }

    .noblink {
        text-decoration: inherit;
    }

    #ctl00_fp1_containerPatientVisits {
        display: inline;
    }

    /*#ctl00_fp1_gvEncounter_GridData {
        height: 500px !important;
    }*/

    #ctl00_fp1_containerAppointment {
        display: inline;
    }

    #ctl00_fp1_containerAppointmentNew {
        display: inline;
    }

    table#ctl00_fp1_gvEncounter_ctl00 th:nth-child(3) {
        /*width: 50px !important;*/
    }


    table#ctl00_fp1_gvEncounter_ctl00 td:nth-child(3) {
        /*width: 50px !important;*/
    }

    div#ctl00_rdpAppList .rspSlideTitle {
        font-size: 0;
    }

    .FindPatiendCheckBox h3, .FindPatiendCheckBox h4, .FindPatiendCheckBox h5, .FindPatiendCheckBox h6, .FindPatiendCheckBox h7 {
        width: 50%;
        padding: 5px;
        box-sizing: border-box;
        margin: 0 !important;
        position: relative;
        border: 1px solid #fff;
    }

    div#ctl00_fp1_ddlName .rcbInput {
        padding: 0 !important;
    }

    table#ctl00_fp1_rblSearchCriteria td {
        display: none;
    }

    .FindPatiendCheckBox [type="checkbox"] {
        /* background: red; 
        -webkit-appearance: none;
        width: 100%;
        height: 100%;
        position: absolute;
        top: 0;
        left: 0;
        z-index: 0;*/
        cursor: pointer;
    }

        .FindPatiendCheckBox [type="checkbox"]:focus, .FindPatiendCheckBox [type="checkbox"]:hover {
            outline: 0;
        }

    .height_auto {
        height: 62vh !important;
        overflow: inherit !important;
    }

    table.rgMasterTable.rgClipCells table td {
        border: 0 !important;
    }

    .call-icon {
        display: block;
        text-align: center;
        font-size: 0;
    }

        .call-icon::before {
            font-size: 16px;
        }

    /*div#ctl00_fp1_gvEncounter_GridData {
        height: 54vh !important;
    }*/

    .back_bg {
        background: transparent;
        border: 0;
        border-top: 1px solid #ccc;
    }

    div#ctl00_fp1_containerAppointmentNew span {
        display: inline-block;
        float: left;
        margin: 0;
        border: 1px solid #fff;
        padding: 5px 10px;
    }

    div#RAD_SLIDING_PANE_CONTENT_ctl00_rdpAppList {
        overflow: hidden !important;
    }

    div#ctl00_fp1_gvEncounter:focus {
        outline: 0;
    }

    div#ctl00_fp1_ddlLocation input.rcbInput {
        padding: 0 5px !important;
        vertical-align: text-top;
    }

    .custom-input input.rcbInput {
        padding: 0 5px !important;
        vertical-align: text-top;
    }
    input#ctl00_fp1_txtSearch {
        width: 118px !important;
        padding: 3px 10px;
        margin-left: 2px;
    }
</style>
<body>
    <asp:UpdatePanel ID="update" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlrange" />
            <asp:AsyncPostBackTrigger ControlID="rblSearchCriteria" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="btn_ClearFilter" />
        </Triggers>
        <ContentTemplate>
            <div class="VitalHistory-Div hidden">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2 find-inputs">
                            <span class="findPatientRadio">
                                <asp:RadioButtonList ID="rblSearchCriteria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblSearchCriteria_SelectedIndexChanged"
                                    Font-Bold="true" RepeatDirection="Horizontal" SkinID="radiobuttonlist" CausesValidation="false" />
                            </span>
                        </div>
                        <div class="col-md-2">&nbsp;</div>
                        <div class="col-md-6 text-right">
                        </div>
                        <div class="col-md-2 text-right">
                            <div class="FindPatiendReferral" style="display: none">
                                <h4>
                                    <asp:Label ID="lbtReferralCount" runat="server" Text="" />
                                </h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="VitalHistory-Div" style="margin-bottom: 5px;">
                <div class="container-fluid">
                    <div class="row">
                        <div id="pnlsearch" runat="server" visible="true" class="col-lg-10 col-md-9 col-xs-12">
                            <div class="row">
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="lblLocation" runat="server" Text="Facility" /></span>
                                    <span class="findPatientSelect">
                                        <telerik:RadComboBox ID="ddlLocation" CssClass="custom-select" runat="server" AutoPostBack="false" />
                                    </span>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                                    </span>
                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearch">
                                        <telerik:RadComboBox ID="ddlName" runat="server" Width="80px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged" CssClass="custom-select" Style="float: left;">
                                            <Items>
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                                <telerik:RadComboBoxItem Text="Mobile No." Value="MN" />
                                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                                <telerik:RadComboBoxItem Text="CPR" Value="CPR" Visible="false" />
                                                <telerik:RadComboBoxItem Text="IVF No." Value="IVF" Visible="false" />
                                                <telerik:RadComboBoxItem Text="Patient Age" Value="PA" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:TextBox ID="txtSearch" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                            Width="70px" Visible="false" />
                                        <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" runat="server" Text=""
                                            MaxLength="10" Visible="false" onkeyup="return validateMaxLength();" style="margin-left:1%; width: 49%;" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                        <telerik:RadComboBox ID="ddlPatientAge" runat="server" Visible="false" DropDownWidth="120px" style="width:118px!important; margin-left:2px;">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" Value="0" Selected="true" />
                                                <telerik:RadComboBoxItem Text="Below One Week" Value="7" />
                                                <telerik:RadComboBoxItem Text="Below One Month" Value="30" />
                                                <telerik:RadComboBoxItem Text="Below Six Months" Value="182" />
                                                <telerik:RadComboBoxItem Text="Below One Year" Value="365" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </asp:Panel>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="lblAppointmentStatus" runat="server" Text="Status" /></span>
                                    <span class="findPatientSelect">
                                        <telerik:RadComboBox ID="ddlAppointmentStatus" runat="server" AutoPostBack="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select All"
                                            ShowMoreResultsBox="false" AppendDataBoundItems="true" CssClass="custom-select">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="All" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:Label ID="itemsClientSide" runat="server" /></span>
                                    </span>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="Label1" runat="server" Text="Ward Name" /></span>
                                    <span class="findPatientSelect">
                                        <telerik:RadComboBox ID="ddlWard" runat="server" AutoPostBack="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select All"
                                            ShowMoreResultsBox="false" AppendDataBoundItems="true" Height="300px" Width="100%" CssClass="custom-select" />
                                    </span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="Label2" runat="server" Text='<%$ Resources:PRegistration, specialisation%>' />
                                    </span>
                                    <span class="findPatientSelect">
                                        <telerik:RadComboBox ID="ddlSpecilization" CssClass="custom-select" runat="server" MarkFirstMatch="true" Filter="Contains" AppendDataBoundItems="true"
                                            Height="300px" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSpecilization_SelectedIndexChanged" />
                                    </span>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="lblProvider" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' /></span>
                                    <span class="findPatientSelect">
                                        <telerik:RadComboBox ID="ddlProvider" runat="server" MarkFirstMatch="true" Filter="Contains"
                                            Height="300px" Width="100%" CssClass="custom-select" />
                                    </span>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="lblDate" runat="server" Text="Date" /></span> <span class="findPatientSelect">
                                            <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged"
                                                CausesValidation="false" CssClass="custom-select" />
                                        </span>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <span class="findPatientText">
                                        <asp:Label ID="Label13" runat="server" Text="Visit&nbsp;Type" />
                                    </span>
                                    <span class="findPatientSelect">
                                        <telerik:RadComboBox ID="drpVisitType" runat="server" Width="100%" CssClass="custom-select" AutoPostBack="true" OnSelectedIndexChanged="drpVisitType_SelectedIndexChanged" />
                                    </span>
                                </div>
                            </div>
                           
                        </div>
                        <%--sanyamtanwar--%>
                        <div class="col-lg-2 col-md-3 col-xs-12 no-p-l">
                            <asp:HiddenField ID="hdnFilter" runat="server" />
                            
                                <div class="box-find-patient refer">
                                    <asp:CheckBox ID="ChkReferralOP" Text="Refer OP (0)" runat="server" />
                                </div>
                                <div class="box-find-patient referip">
                                    <asp:CheckBox ID="ChkReferralIP" Text="Refer IP (0)" runat="server" />
                                </div>
                            
                            <div class="box-find-patient mhc">
                                <asp:CheckBox ID="chkMHC" Text="<%$ Resources:PRegistration, MHC%>" runat="server" />
                            </div>
                            <div class="box-find-patient mlc">
                                <asp:CheckBox ID="ChkMLC" Text="MLC (0)" runat="server" />
                            </div>
                            <div class="box-find-patient walk">
                                <asp:CheckBox ID="chkIsWalkInPatient" Text="Walk In (0)" runat="server" />
                            </div>
                            <div class="box-find-patient ippatient">
                                <asp:CheckBox ID="IPPatient" Text="IP Patient(0)" runat="server" />
                            </div>
                            <div class="box-find-patient" style="display:none; width:100%;">
                                <asp:LinkButton ID="lnkOrderApproval" OnClick="lnkOrderApproval_Click" runat="server" Style="visibility: hidden;">Order Approval (0)</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
             <div class="VitalHistory-Div01">
                                <div class="">
                                    <div class="row" style="padding-bottom:.5rem!important;">
                                        <div class="col-lg-3 col-md-3">
                                            <span class="findPatientShowingText" style="padding-left:1.5rem!important;">
                                                <asp:Label ID="lblGridStatus" runat="server" ForeColor="Blue" Font-Bold="true" />
                                            </span>
                                        </div>


                                        <div class="col-lg-5 col-md-5 text-center" style="margin-top:-2px!important; padding-left:17.5rem!important;">

                                            <div id="tblDateRange" runat="server">
                                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="102px" AutoPostBack="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" OnSelectedDateChanged="dtpfromDate_SelectedDateChanged" />
                                                <span id="spTo" runat="server">&nbsp;To&nbsp;</span>
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="102px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-7 pull-right text-right" style="padding-right:3rem!important;">
                                            <div>
                                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="btn btn-primary btn-chotu"
                                                    OnClick="btnSearch_Click" Text="Refresh" Width="95px" OnClientClick="ButtonClientSideClick(this)" UseSubmitBehavior="False" />
                                                <asp:Button ID="btn_ClearFilter" runat="server" CausesValidation="false" CssClass="btn btn-primary btn-chotu"
                                                    Text="Reset Filter" Width="95px" OnClick="btn_ClearFilter_Click" OnClientClick="ButtonClientSideClick(this)" UseSubmitBehavior="False" />
                                                <asp:Button ID="btnCloseQMS" runat="server" CausesValidation="true" CssClass="btn btn-primary btn-chotu"
                                                    OnClick="btnCloseQMS_Click" Text="Close QMS" Width="95px" UseSubmitBehavior="False" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
            <%--sanyamtanwar--%>
            <div class="VitalHistory-Div01">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:LinkButton ID="lnkbtn_Refresh" runat="server" OnClick="lnkbtn_Refresh_OnClick"
                                Text="Refresh" ForeColor="OrangeRed" Visible="false" />

                            <asp:Panel ID="pnlGrid" runat="server" Height="420px" Width="100%" ScrollBars="Auto" CssClass="height_auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvEncounter" runat="server" CellFormatting="gvEncounter_CellFormatting" CssClass="box-find-patient-list" RenderMode="Lightweight" Height="350px" ItemStyle-Height="30px"
                                            AutoGenerateColumns="false" AllowPaging="true" PageSize="100" CellPadding="0" CellSpacing="0" AllowSorting="false" GridLines="None"
                                            AllowMultiRowSelection="false" AllowCustomPaging="false" AllowAutomaticDeletes="false" ShowStatusBar="true" AllowFilteringByColumn="false"
                                            ShowFooter="false" EnableLinqExpressions="false" BorderWidth="0" PagerStyle-ShowPagerText="false" HeaderStyle-Wrap="false"
                                            OnPageIndexChanged="gvEncounter_OnPageIndexChanged" OnPreRender="gvEncounter_OnPreRender"
                                            OnItemDataBound="gvEncounter_OnItemDataBound" OnItemCommand="gvEncounter_OnItemCommand">
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                                <Scrolling AllowScroll="True" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                                <ClientEvents OnRowDblClick="gvEncounter_OnRowDblClick" />
                                            </ClientSettings>
                                            <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" Width="100%">
                                                <%--<SortExpressions>
                                                    <telerik:GridSortExpression FieldName="OPIP" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="AppointmentDate" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="EncounterDate" SortOrder="Descending" />
                                                </SortExpressions>--%>
                                                <Columns>
                                                    <%--<telerik:GridTemplateColumn UniqueName="SNO" HeaderText="S.No." SortExpression="S.No." HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%# Container.DataSetIndex+1 %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>--%>
                                                    <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                                        HeaderText=" Select" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSelect" UniqueName="lnkSelect" runat="server" Text="Select" CommandName="SELECTENCOUNTER" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AppointmentDate" HeaderText="Appointment Dt." HeaderStyle-Width="95px" ItemStyle-Width="95px"
                                                        SortExpression="AppointmentDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAppointmentDate" runat="server" Text='<%#Eval("AppointmentDate")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" SortExpression="Name"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>' />
                                                            <asp:HiddenField ID="hdnEPackageId" runat="server" Value='<%#Eval("PackageId") %>' />
                                                            <asp:HiddenField ID="hdnCriticalLabResult" runat="server" Value='<%#Eval("CriticalLabResult") %>' />
                                                            <asp:HiddenField ID="hdnMedicalAlert" runat="server" Value='<%#Eval("MedicalAlert")%>' />
                                                            <asp:HiddenField ID="hdnAllergiesAlert" runat="server" Value='<%#Eval("AllergiesAlert")%>' />
                                                            <asp:HiddenField ID="hdnEncounterId" Value='<%#Eval("EncounterId")%>' runat="server" />
                                                            <asp:HiddenField ID="hdnAppointmentID" Value='<%#Eval("AppointmentID")%>' runat="server" />
                                                            <asp:HiddenField ID="hdnDoctorID" Value='<%#Eval("DoctorID" )%>' runat="server" />
                                                            <asp:HiddenField ID="hdnRegistrationID" Value='<%#Eval("RegistrationId")%>' runat="server" />
                                                            <asp:HiddenField ID="hdnFacilityID" Value='<%#Eval("FacilityId")%>' runat="server" />
                                                            <asp:HiddenField ID="hdnStatusColor" Value='<%#Eval("StatusColor")%>' runat="server" />
                                                            <asp:HiddenField ID="hdnOPIP" Value='<%#Eval("OPIP")%>' runat="server" />
                                                            <asp:HiddenField ID="hdnIsReferralCase" runat="server" Value='<%#Eval("IsReferralCase")%>' />
                                                            <asp:HiddenField ID="hdnClinicalAddendumAllowed" runat="server" Value='<%#Eval("ClinicalAddendumAllowed")%>' />
                                                            <asp:HiddenField ID="hdnMLC" runat="server" Value='<%#Eval("MLC")%>' />
                                                            <asp:HiddenField ID="hdnUnReviewedLabResults" runat="server" Value='<%#Eval("UnReviewedLabResults")%>' />
                                                            <asp:HiddenField ID="hdnIsWalkInPatient" runat="server" Value='<%#Eval("IsWalkInPatient")%>' />
                                                            <asp:HiddenField ID="hdnMHC" runat="server" Value='<%#Eval("MHC")%>' />
                                                            <asp:HiddenField ID="hdnEncounterStatusColor" runat="server" Value='<%#Eval("EncounterStatusColor")%>' />
                                                            <asp:HiddenField ID="hdnVisitTypeStatusColor" runat="server" Value='<%# Eval("VisitTypeStatusColor") %>' />
                                                            <asp:HiddenField ID="hdnEncounterStatus" runat="server" Value='<%# Eval("EncounterStatus") %>' />
                                                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%# Eval("EncounterNo") %>' />
                                                            <asp:HiddenField ID="hdnReferralColor" runat="server" Value='<%# Eval("ReferralColor") %>' />
                                                            <asp:HiddenField ID="hdnAppointmentSlot" runat="server" Value='<%#Eval("AppointmentSlot")%>' />
                                                            <asp:HiddenField ID="hdnBreakId" runat="server" Value='<%#Eval("BreakId")%>' />
                                                            <asp:HiddenField ID="hdnVisitEntryDone" runat="server" Value='<%#Eval("VisitEntryDone")%>' />
                                                            <asp:HiddenField ID="hdnVisitEntryStatusColor" runat="server" Value='<%#Eval("VisitEntryStatusColor")%>' />
                                                            <asp:HiddenField ID="hdnPatientToolTip" runat="server" Value='<%#Eval("PatientToolTip")%>' />
                                                            <asp:HiddenField ID="hdnIsCarePlan" runat="server" Value='<%#Eval("IsCarePlan") %>' />	
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AgeGender" HeaderText="Age/Gender" SortExpression="AgeGender"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Contact" HeaderText="Mobile No." SortExpression="Contact" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("Contact")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>' SortExpression="RegistrationNo"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>' SortExpression="EncounterNo"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterDate" HeaderText='<%$ Resources:PRegistration, EncounterDate%>' SortExpression="EncounterDate"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="95px" ItemStyle-Width="95px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterDate" runat="server" Text='<%#Eval("EncounterDate")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PatientVisit" HeaderText="Patient Visit" SortExpression="PatientVisit" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPatientVisit" runat="server" Text='<%#Eval("PatientVisit")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="WardNameBedNo" HeaderText="Ward / Bed No" SortExpression="WardNameBedNo"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("WardNameBedNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderStyle-CssClass="doctor-name" FooterStyle-CssClass="doctor-name" ItemStyle-CssClass="doctor-name" HeaderText='<%$ Resources:PRegistration, Doctor%>' SortExpression="DoctorName"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Visit" HeaderText="Visit" SortExpression="OPIP" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVisitType" runat="server" Text='<%#Eval("OPIP")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Notification" HeaderText="Notification" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                        <ItemTemplate>
                                                            <div class="row" style="margin-left: 2px;">

                                                                <asp:ImageButton ID="imgDiagnosticHistory" runat="server" CommandName="DIAGHISTORY" ImageUrl="~/Images/flask.png"
                                                                    Height="14px" Width="16px" ToolTip="View Diagnostic History" AlternateText="" />
                                                                <asp:ImageButton ID="imgPastClinicalNote" runat="server" CommandName="PASTCLINICALNOTE" ImageUrl="~/Images/notepad.png"
                                                                    Height="16px" Width="16px" ToolTip="Past Clinical Notes" AlternateText="" />
                                                                <asp:ImageButton ID="imgLabResult" runat="server" CommandName="SHOWLABRESULT" ImageUrl="~/Icons/read_results.png"
                                                                    Height="16px" Width="16px" ToolTip="Unreviewed Lab Results" AlternateText="" />
                                                                <asp:ImageButton ID="imgCritical" CommandName="CRITICALALERT" runat="server" ImageUrl="~/Icons/Critical.gif"
                                                                    Height="16px" Width="16px" ToolTip="Critical Value" AlternateText="" />
                                                                <asp:ImageButton ID="imgReffHistory" runat="server" CommandName="SHOWREFERRAL" ImageUrl="~/Icons/letter_referral.png"
                                                                    Height="18px" Width="20px" ToolTip="Referral History" AlternateText="" />
                                                                <asp:ImageButton ID="imgMedicalAlert" CommandName="MEDICALALERT" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                    Height="16px" Width="16px" ToolTip="Patient Alert" AlternateText="" />
                                                                <asp:ImageButton ID="imgAllergyAlert" CommandName="ALLERGYALERT" runat="server" ImageUrl="~/Icons/allergy.gif"
                                                                    Height="16px" Width="16px" ToolTip="Allergy Alert" AlternateText="" />


                                                                <asp:ImageButton ID="imgCarePlan" runat="server" ImageUrl="~/Icons/CarePlan.png"
                                                                    Width="13px" Height="13px" ToolTip="Care Plan" OnClick="imgCarePlan_Click" /> 		
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Addendum" HeaderText="Addendum" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAddendum" runat="server" CommandName="ADDENDUM" Text="Addendum" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CALL"  HeaderText="CALL" HeaderStyle-Width="60px" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkCall" runat="server" Text='<%#Eval("QmsAppointmentLink")%>' CommandName="APPOINTMENT" ForeColor="Blue" Font-Bold="true"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="TokenNo" HeaderText="Token No" SortExpression="TokenNo" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblTokenNo" runat="server" Text='<%#Eval("TokenNo")%>' />--%>
                                                            <asp:LinkButton ID="lblTokenNo" runat="server" Text='<%#Eval("TokenNo")%>' ToolTip="Call Patient" OnClick="lnkCalled_OnClick" />
                                                            <asp:HiddenField ID="hdnQMSStatusId" runat="server" Value='<%# Eval("QMSStatusId") %>' />
                                                            <asp:HiddenField ID="hdnQMSColor" runat="server" Value='<%# Eval("QMSColor") %>' />
                                                            <asp:HiddenField ID="hdnQMSCode" runat="server" Value='<%# Eval("QMSCode") %>' />
                                                            <asp:HiddenField ID="hdnIsToken" runat="server" Value='<%#Eval("TokenNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Company" HeaderText="Company" SortExpression="AccountCategory" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccountCategory" runat="server" Text='<%#Eval("AccountCategory")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CPR" HeaderText="CPR" SortExpression="CPR" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCPR" runat="server" Text='<%#Eval("CPR")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ConsultationCharges" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConsultationCharges" runat="server" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true"
                                            EnableShadows="true" Width="100px" OnItemClick="menuStatus_ItemClick" DefaultGroupSettings-Height="470px" />
                                        <asp:HiddenField ID="hdnAppointmentDate" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnTimeSlot" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnRefBreakId" runat="server" Value="" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="menuStatus" />
                                        <asp:AsyncPostBackTrigger ControlID="gvEncounter" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                        <div class="col-md-12">
                            <div class="back_bg">
                                <div id="containerPatientVisits" visible="false" runat="server"></div>
                                <div id="containerAppointment" visible="false" runat="server"></div>
                                <div id="containerAppointmentNew" runat="server"></div>

                          <span class="mlabel">
                         <img src="../../../Icons/MedicalAlert.gif" style="width: 12px; height: 12px; margin-top: -5px; margin-right: 5px;">Medical alert</span>
                         <span class="mlabel">
                         <img src="../../../Icons/allergy.gif" style="width: 12px; height: 12px; margin-top: -3px; margin-right: 5px;">Allergy alert</span>
                            <span class="mlabel">
                                    <img src="../../../Icons/read_results.png" style="width: 12px; height: 12px; margin-top: -3px; margin-right: 5px;">View Lab result</span>
                            <span class="mlabel">
                                    <img src="../../../Icons/letter_referral.png" style="width: 14px; height: 14px; margin-top: -3px; margin-right: 5px;">Referal History</span>
                             <span class="mlabel">
                                    <img src="../../../Icons/Critical.gif" style="width: 14px; height: 14px; margin-top: -3px; margin-right: 5px;">Panic Values</span>
                            </div>

                        </div>
                        
                    </div>
                </div>
            </div>
            <div id="divDeleteBreak" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 75px; left: 450px; top: 300px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label3" runat="server" Text="Are You Sure You want to delete all future Break or Only this Break ?" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDeleteAllBreak" SkinID="Button" runat="server" Text="Cancel All"
                                        Visible="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnDeleteBreak" SkinID="Button" runat="server" Text="Cancel This"
                                        OnClick="btnDeleteBreak_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnClose" SkinID="Button" runat="server" Text="Close" OnClick="btnClose_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false" Skin="Metro" ShowContentDuringLoad="false" Title="Test Results" VisibleStatusbar="false" ReloadOnShow="true">
                <Windows>
                    <telerik:RadWindow runat="server" ID="RadWindow1" Skin="Metro"  Behaviors="Close,Move,Maximize,Resize" Modal="true" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false" Skin="Metro" Title="Lab Results" ShowContentDuringLoad="false" VisibleStatusbar="false" ReloadOnShow="true">
                <Windows>
                    <telerik:RadWindow runat="server" ID="RadWindow2" Skin="Metro" Behaviors="Close,Move,Maximize,Resize" Modal="true" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:Button ID="btnFillData" runat="server" Style="visibility: hidden;" Width="1px"
                OnClick="btnFillData_OnClick" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script  type="text/javascript">
        function showBreakBlock(e, menu, BreakBlock, AppointmentDate, AppointmentSlot, BreakId, rowIdx) {
            $get('<%=hdnAppointmentDate.ClientID%>').value = AppointmentDate;
            $get('<%=hdnTimeSlot.ClientID%>').value = AppointmentSlot;
            $get('<%=hdnRefBreakId.ClientID%>').value = BreakId;

            var menu = $find("<%=menuStatus.ClientID %>");

            var items = menu.get_items();
            for (var i = 0; i < items.get_count() ; i++) {
                var OptionCode = items.getItem(i).get_attributes().getAttribute('Code');
                var VAL = menu.findItemByValue(items.getItem(i).get_value());
                var menuItem = menu.findItemByText(items.getItem(i).get_text());

                if (BreakBlock == "Break") {
                    if (OptionCode != "Delete") {
                        menuItem.disable();
                    }
                    else { menuItem.enable(); }
                }
                else if (BreakBlock == "Reserved") {

                    menuItem.disable();

                }
                else {
                    if (OptionCode != "Break") {
                        menuItem.disable();
                    }
                    else { menuItem.enable(); }
                }

            }
            menu.show(e);
            var grid = $find("<%=gvEncounter.ClientID%>").get_masterTableView();
            var Index = rowIdx;
            grid.selectItem(Index);
            var row = grid.get_dataItems()[Index];
        }

        function validateMaxLength() {
            var txt = $get('<%=txtSearchN.ClientID%>');
            if (txt.value > 2147483647) {
                alert("Value should not be more then 2147483647.");
                txt.value = txt.value.substring(0, 9);
                txt.focus();
            }
        }

        function ButtonClientSideClick(myButton) {
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            if (myButton.getAttribute('type') == 'button') {
                myButton.disabled = true;
                myButton.value = "Processing...";
            }
            return true;
        }

    </script>

    <script type="text/javascript">

        function gvEncounter_OnRowDblClick(sender, eventArgs) {

            var rowIdx = eventArgs.get_itemIndexHierarchical();

            var grid = $find("<%=gvEncounter.ClientID %>");
            var masterTable = grid.get_masterTableView();
            var row = masterTable.get_dataItems()[rowIdx];
            var lnkSelect = row.findElement("lnkSelect");
            lnkSelect.click();
        }

        function HideSlidingPane() {

            //var slidingPaneZone = $get("ctl00_Radslidingzone2");

            var slidingPane = $get("RAD_SPLITTER_SLIDING_PANE_COLLAPSE_ctl00_rdpAppList");

            //slidingPaneZone.set_cancel(true);
        }

        function SingleScreen_OnClientExpanded() {

            var btnEMRSingleScreenIsAllowPopup = document.getElementById('ctl00_ContentPlaceHolder1_btnEMRSingleScreenIsAllowPopup');

            if (btnEMRSingleScreenIsAllowPopup != null) {
                $get('ctl00_ContentPlaceHolder1_btnEMRSingleScreenIsAllowPopup').click();
            }
        }


    </script>

    <script type="text/javascript">
        var CtrlEventKey = "";

        function OnClientClose(oWnd) {
            $get('<%=btnSearch.ClientID%>').click();
        }

        $(function () {
            // $(document).pos();
            $(document).on('scan.pos.barcode', function (event) {

                var ScannedVal = $('#<%=txtSearch.ClientID %>').val();
                var scanerVal = event.code;
                scanerVal = scanerVal.replace("\r\n", "");
                if (ScannedVal != scanerVal) {
                    $('#<%=txtSearch.ClientID %>').val("");
                    $('#<%=txtSearch.ClientID %>').val(scanerVal);
                    $get('<%=btnSearch.ClientID%>').click();
                }
            });

        });

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
        var CtrlEventKey = "";

        $(function () {
            // $(document).pos();
            $(document).on('scan.pos.barcode', function (event) {

                var ScannedVal = $('#<%=txtSearchN.ClientID %>').val();
                var scanerVal = event.code;
                scanerVal = scanerVal.replace("\r\n", "");
                if (ScannedVal != scanerVal) {
                    $('#<%=txtSearchN.ClientID %>').val("");
                    $('#<%=txtSearchN.ClientID %>').val(scanerVal);
                    $get('<%=btnSearch.ClientID%>').click();
                }
            });

        });

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

</body>
