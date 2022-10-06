<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientDetails.aspx.cs" Inherits="Pharmacy_PatientDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patient Details</title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/scrollbar.css" rel="stylesheet" />

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.RegistrationId = document.getElementById("hdnRegistrationId").value;
            oArg.RegistrationNo = document.getElementById("hdnRegistrationNo").value
            oArg.EncounterNo = document.getElementById("hdnEncounterNo").value;
            oArg.EncounterId = document.getElementById("hdnEncounterId").value
            oArg.FacilityId = document.getElementById("hdnFacilityId").value


            var oWnd = GetRadWindow();
            oWnd.close(oArg);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }


    </script>

    <style type="text/css">
        .radioo td {
            margin-right: 0px;
        }

        div#pnlgrid td, th {
            border: 1px solid #a4a4a4 !important;
            white-space: nowrap;
        }
      
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
               
                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-md-3 col-sm-6 col-6">
                            <div class="row">
                                <div class="col-4">
                                    <asp:Label ID="lblfacility" runat="server" Text="Facility" SkinID="label"></asp:Label>
                                </div>
                                <div class="col-8">
                                    <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="true" SkinID="DropDown" Width="100%" Enabled="False"></asp:DropDownList>

                            </div>
                        </div>
                    </div>

                        <div class="col-md-3 col-sm-6 col-6">
                            <div class="row">
                                <div class="col-4">
                                    <asp:Label ID="lblEntrySite" runat="server" Text="Entry Site" SkinID="label"></asp:Label>
                                </div>
                                <div class="col-8">
                                    <asp:DropDownList ID="ddlEntrySite" runat="server" AppendDataBoundItems="true" SkinID="DropDown" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 col-sm-12 col-12 text-right mt-2 m-md-0">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter"
                                OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" cssClass="btn btn-primary"
                                OnClientClick="window.close();" />
                    </div>
                </div>
               
                <div class="row text-center form-group">
                    <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="" />
                </div>

                    <div class="row form-group">
                        <div class="col-md-6 col-sm-7 col-xs-12 box-col-checkbox">
                            <asp:RadioButtonList ID="rdoSearch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                RepeatLayout="Flow" OnSelectedIndexChanged="rdoSearch_SelectedIndexChanged" CssClass="radioo">
                                <asp:ListItem Text="Search on Criteria" Value="0" Selected="True" />
                                <asp:ListItem Text="Search All (Date Range)" Value="1" />
                            </asp:RadioButtonList>
                    </div>

                        <div class="col-md-6 col-sm-5 col-xs-12 box-col-checkbox">
                            <asp:RadioButtonList ID="rdoRegEnc" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="rdoRegEnc_OnSelectedIndexChanged" CssClass="radioo">
                            </asp:RadioButtonList>

                    </div>

                </div>



                    <div id="tblsearch" runat="server" visible="true">

                        <div class="row">
                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-4 text-nowrap">
                                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, UHID%>" />
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtRegistrationNo" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-4 text-nowrap">
                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, EncounterNo%>" />
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtEncounterNo" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-4 text-nowrap">
                                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, PatientName%>" />
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtPatientName" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-4 text-nowrap">
                                        <asp:Label ID="Label12" runat="server" SkinID="label" Text="Date of Birth" />
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtDob" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-4 text-nowrap">
                                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, phone%>" />
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtPhoneNo" Width="100%" runat="server" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, mobile%>" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtMobileNo" Width="100%" runat="server" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bedno%>" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtBedNo" Width="100%" runat="server" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label13" runat="server" SkinID="label" Text="E-Mail Id" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtEmailId" Width="100%" runat="server" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label9" runat="server" SkinID="label" Text="Company" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtCompany" Width="100%" runat="server" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label10" runat="server" SkinID="label" Text="Passport No" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtPassportno" Width="100%" runat="server" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="Identity No" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtCprno" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label14" runat="server" SkinID="label" Text="Old Reg No" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtOldRegistrationno" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label19" runat="server" SkinID="label" Text="Mother Name" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtMotherName" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="Label20" runat="server" SkinID="label" Text="Father Name" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtFatherName" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="lblGardianname" runat="server" Text="Guardian Name" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtParentof" runat="server" Width="100%" SkinID="textbox" MaxLength="140" />
                                        <AJAX:FilteredTextBoxExtender ID="Custom" runat="server" TargetControlID="txtParentof"
                                            FilterMode="InvalidChars" InvalidChars="!@#$%^&amp;*()~?><|\';:">
                                        </AJAX:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-4 col-4 text-nowrap">
                                        <asp:Label ID="lblAddress" runat="server" Text="Address" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtAddress" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                 


                        </div>
                    </div>

                    <div class="row" id="tblDate" runat="server" visible="false">
                        <div class="col-md-6 col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-md-6 col-6">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-3 col-4 text-nowrap">
                                            <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-6">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-3 col-4 text-nowrap">
                                            <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" Width="100%"
                                                DateInput-ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                            <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="100%" BorderWidth="0" ScrollBars="Auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvEncounter" runat="server" CssClass="table table-condensed" BorderWidth="0"
                                            PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                            Width="100%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                            AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                            AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" AllowSorting="true"
                                            OnItemCommand="gvEncounter_OnItemCommand" OnPageIndexChanged="gvEncounter_OnPageIndexChanged"
                                            AllowCustomPaging="true" OnPreRender="gvEncounter_PreRender">
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="true" />
                                            <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false"
                                                Width="100%">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.
                                                    </div>
                                                </NoRecordsTemplate>
                                                <ItemStyle Wrap="true" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                        AllowFiltering="false" HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="OPIP" HeaderText="OP/IP" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="OPIP">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo" SortExpression="REGID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterNo" SortExpression="EncounterNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="Name" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                        SortExpression="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnKinName" runat="server" Value='<%#Eval("KinName")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="GenderAge" SortExpression="GenderAge">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="DoctorName" SortExpression="DoctorName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CurrentBedNo" HeaderText="Bed No" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="CurrentBedNo" SortExpression="CurrentBedNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncDate" HeaderText="Admission Date" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="EncDate" SortExpression="EncDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncDate" runat="server" Text='<%#Eval("EncDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DischargeStatus" HeaderText="Discharge Status"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="DischargeStatus" SortExpression="DischargeStatus">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDischargeStatus" runat="server" Text='<%#Eval("DischargeStatus")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterStatus" HeaderText="Encounter Status"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterStatus" SortExpression="EncounterStatus">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterStatus" runat="server" Text='<%#Eval("EncounterStatus")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CompanyName" HeaderText="Company" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="CompanyName" SortExpression="CompanyName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PhoneHome" HeaderText="PhoneHome" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="PhoneHome" SortExpression="PhoneHome">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPhoneHome" runat="server" Text='<%#Eval("PhoneHome")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="MobileNo" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="MobileNo" SortExpression="MobileNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DOB" HeaderText="DOB" ShowFilterIcon="false"
                                                        Visible="true" DefaultInsertValue="" DataField="DOB" SortExpression="RegistrationNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PatientAddress" HeaderText="PatientAddress"
                                                        Visible="true" ShowFilterIcon="false" DefaultInsertValue="" DataField="PatientAddress"
                                                        SortExpression="PatientAddress">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPatientAddress" runat="server" Text='<%#Eval("PatientAddress")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="REGID" HeaderText="REGID" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="REGID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("REGID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ENCID" HeaderText="ENCID" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="ENCID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblENCID" runat="server" Text='<%#Eval("ENCID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CompanyCode" HeaderText="CompanyCode" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="CompanyCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompanyCode" runat="server" Text='<%#Eval("CompanyCode")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="InsuranceCode" HeaderText="InsuranceCode"
                                                        Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="InsuranceCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsuranceCode" runat="server" Text='<%#Eval("InsuranceCode")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CardId" HeaderText="CardId" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="CardId">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCardId" runat="server" Text='<%#Eval("CardId")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RowNo" HeaderText="RowNo" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RowNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNoOld" HeaderText="Old Reg No"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNoOld" SortExpression="MobileNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNoOld" runat="server" Text='<%#Eval("RegistrationNoOld")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Width="60px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="MotherName" HeaderText="Mother Name" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="MotherName" HeaderStyle-Width="50px" ItemStyle-Width="100px"
                                                        SortExpression="MotherName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMotherName" runat="server" Text='<%#Eval("MotherName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="FatherName" HeaderText="Father Name" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="FatherName" HeaderStyle-Width="50px" ItemStyle-Width="100px"
                                                        SortExpression="FatherName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFatherName" runat="server" Text='<%#Eval("FatherName")%>'></asp:Label>
                                                            <asp:HiddenField ID="hfExternalPatient" runat="server" Value='<%#Eval("ExternalPatient")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>

                                        <script type="text/javascript">
                                            function ShowPatientDetails(lblName, lblPatientAddress, lblKinName) {
                                                document.getElementById("<%=lblPatientName.ClientID%>").innerHTML = $get(lblName).innerHTML;
                                                document.getElementById("<%=lblPatientAddress.ClientID%>").innerHTML = $get(lblPatientAddress).innerHTML;
                                                document.getElementById("<%=lblPatientKin.ClientID%>").innerHTML = $get(lblKinName).value;
                                            }
                                        </script>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                        <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCompanyCode" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnInsuranceCode" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCardId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterDate" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnAgeGender" runat="server" />
                            <asp:HiddenField ID="hdnPhoneHome" runat="server" />
                            <asp:HiddenField ID="hdnMobileNo" runat="server" />
                            <asp:HiddenField ID="hdnPatientName" runat="server" />
                            <asp:HiddenField ID="hdnDOB" runat="server" />
                            <asp:HiddenField ID="hdnAddress" runat="server" />
                            <asp:HiddenField ID="hdnFacilityId" runat="server" />
                    </div>
      
                       <div class="row m-t">
                           <div class="col-md-12 col-sm-12 col-xs-12">
                               <h2 style="background:#cae2eb;color:#000;padding:3px 10px;margin-bottom:0px;font-size:14px;"><b>Patient Information</b></h2>
                           </div>
                       </div>
                    <div class="row">
                        <div class="col-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="Name :" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                </div>
                                <div class="col-md-10 col-sm-10 col-xs-9">
                                    <asp:Label ID="lblPatientName" runat="server" SkinID="label" ForeColor="Brown" Font-Names="Tahoma"
                                                Font-Size="8pt" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                    <asp:Label ID="Label8" runat="server" SkinID="label" Text="Address :" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                </div>
                                <div class="col-md-10 col-sm-10 col-xs-9">
                                    <asp:Label ID="lblPatientAddress" runat="server" SkinID="label" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                    <asp:Label ID="lblPatientKin" runat="server" SkinID="label" Text="Kin" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                </div>
                                <div class="col-md-10 col-sm-10 col-xs-9 text-nowrap"></div>
                            </div>
                        </div>
                    </div> 
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
