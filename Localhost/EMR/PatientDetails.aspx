<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientDetails.aspx.cs" Inherits="Pharmacy_PatientDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Patient Details</title>

    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.RegistrationId = document.getElementById("hdnRegistrationId").value;
            oArg.RegistrationNo = document.getElementById("hdnRegistrationNo").value
            oArg.EncounterNo = document.getElementById("hdnEncounterNo").value;
            oArg.EncounterId = document.getElementById("hdnEncounterId").value
            oArg.FacilityId = document.getElementById("hdnFacilityId").value
            oArg.IsMedicalAlert = document.getElementById("hdnIsMedicalAlert").value
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
</head>


<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>


        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                    <div class="container-fluid">

                        <div class="row header_main">
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="lblfacility" runat="server" Text="Facility"></asp:Label>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:DropDownList ID="ddlLocation" Width="100%" CssClass="PRegPop-FacilityInput" runat="server" AppendDataBoundItems="true" Enabled="False"></asp:DropDownList>
                </div>
            </div>
        </div>

                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="lblEntrySite" runat="server" CssClass="PRegPop-FacilityInput" Text="Entry Site" SkinID="label" Visible="false"></asp:Label>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:DropDownList ID="ddlEntrySite" runat="server" CssClass="PRegPop-FacilityInput" AppendDataBoundItems="true" SkinID="DropDown" Width="100%" Visible="false"></asp:DropDownList>
                </div>
            </div>
        </div>

                           

                            

                            <div class="col-md-6 col-sm-6 col-xs-12 text-right">
                                    <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                                    <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                                </div>
                        </div>
                        <div class="row text-center">
                                    <asp:Label ID="lblMessage" runat="server" Text="" />
                                </div>

                        <div class="row">
                            <div class="col-md-12 box-col-checkbox">
                                <asp:RadioButtonList ID="rdoSearch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Flow" OnSelectedIndexChanged="rdoSearch_SelectedIndexChanged">
                                    <asp:ListItem Text="Search on Criteria" Value="0" Selected="True" />
                                    <asp:ListItem Text="Search All (Date Range)" Value="1" />
                                </asp:RadioButtonList>

                                <asp:RadioButtonList ID="rdoRegEnc" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoRegEnc_OnSelectedIndexChanged" Visible="false"></asp:RadioButtonList>
                            </div>
                        </div>

                        <div id="tblsearch" runat="server" visible="true">

                        <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label1" runat="server" Text="<%$ Resources:PRegistration, UHID%>" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label5" runat="server" Text="<%$ Resources:PRegistration, PatientName%>" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtPatientName" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                            
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label12" runat="server" Text="Date of Birth" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtDob" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label4" runat="server" Text="<%$ Resources:PRegistration, mobile%>" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtMobileNo" Width="100%" runat="server"></asp:TextBox>
                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:PRegistration, bedno%>" Visible="false" />
                </div>
            </div>
        </div>
                            </div>
                            <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label13" runat="server" Text="E-Mail Id" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtEmailId" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                            
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label19" runat="server" Text="Mother Name" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtMotherName" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label20" runat="server" Text="Father Name" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtFatherName" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label11" runat="server" Text="Identity No" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtCprno" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                                </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label2" runat="server" Text="<%$ Resources:PRegistration, EncounterNo%>" Visible="false" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtEncounterNo" runat="server" Visible="false"></asp:TextBox>
                </div>
            </div>
        </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label3" runat="server" Visible="false" Text="<%$ Resources:PRegistration, phone%>" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtPhoneNo" Visible="false" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label9" runat="server" Text="Company" Visible="false" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtCompany" runat="server" Visible="false"></asp:TextBox>
                </div>
            </div>
        </div>
                            
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label10" runat="server" Text="Passport No" Visible="false" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtPassportno" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtBedNo" runat="server" Visible="false"></asp:TextBox>
                </div>
            </div>
        </div>
                            
                            <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                   <asp:Label ID="Label14" runat="server" Text="Old Reg No" Visible="false" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                   <asp:TextBox ID="txtOldRegistrationno" runat="server" Visible="false"></asp:TextBox>
                </div>
            </div>
        </div>
                           
                        
                        </div>

                    </div>

                        <div class="row" id="tblDate" runat="server" visible="false">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <asp:Label ID="Label17" runat="server" Text="From&nbsp;Date" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <asp:Label ID="Label18" runat="server" Text="To&nbsp;Date" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

               <div class="row">
                            <div class="col-md-12 col-sm-12 col-xxs-12 gridview">
                                    <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="99%" BorderWidth="0" ScrollBars="Auto">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadGrid ID="gvEncounter" runat="server" CssClass="table table-condensed" BorderWidth="0" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false" Width="100%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false" GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                                    AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" AllowSorting="true" OnItemCommand="gvEncounter_OnItemCommand" OnPageIndexChanged="gvEncounter_OnPageIndexChanged" AllowCustomPaging="true" OnPreRender="gvEncounter_PreRender">
                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="false" />
                                                    </ClientSettings>

                                                    <PagerStyle ShowPagerText="true" />
                                                    <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false" Width="100%">
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">No Record Found.</div>
                                                        </NoRecordsTemplate>
                                                        <ItemStyle Wrap="true" />

                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderText="Select" >
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="OPIP" HeaderText="OP/IP" ShowFilterIcon="false" DefaultInsertValue="" DataField="OPIP">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>' ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo" SortExpression="REGID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnIsMedicalAlert" runat="server" Value='<%#Eval("IsMedicalAlert")%>' />
                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>' ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterNo" SortExpression="EncounterNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false" DefaultInsertValue="" DataField="Name" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnKinName" runat="server" Value='<%#Eval("KinName")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" ShowFilterIcon="false" DefaultInsertValue="" DataField="GenderAge" SortExpression="GenderAge">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor" ShowFilterIcon="false" DefaultInsertValue="" DataField="DoctorName" SortExpression="DoctorName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="CurrentBedNo" HeaderText="Bed No" ShowFilterIcon="false" DefaultInsertValue="" DataField="CurrentBedNo" SortExpression="CurrentBedNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="EncDate" HeaderText="Admission Date" ShowFilterIcon="false" DefaultInsertValue="" DataField="EncDate" SortExpression="EncDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEncDate" runat="server" Text='<%#Eval("EncDate")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="DischargeStatus" HeaderText="Discharge Status" ShowFilterIcon="false" DefaultInsertValue="" DataField="DischargeStatus" SortExpression="DischargeStatus">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDischargeStatus" runat="server" Text='<%#Eval("DischargeStatus")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="EncounterStatus" HeaderText="Encounter Status" ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterStatus" SortExpression="EncounterStatus">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEncounterStatus" runat="server" Text='<%#Eval("EncounterStatus")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="CompanyName" HeaderText="Company" ShowFilterIcon="false" DefaultInsertValue="" DataField="CompanyName" SortExpression="CompanyName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="PhoneHome" HeaderText="PhoneHome" ShowFilterIcon="false" DefaultInsertValue="" DataField="PhoneHome" SortExpression="PhoneHome">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPhoneHome" runat="server" Text='<%#Eval("PhoneHome")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="MobileNo" ShowFilterIcon="false" DefaultInsertValue="" DataField="MobileNo" SortExpression="MobileNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="DOB" HeaderText="DOB" ShowFilterIcon="false" Visible="false" DefaultInsertValue="" DataField="DOB" SortExpression="RegistrationNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="PatientAddress" HeaderText="PatientAddress" ShowFilterIcon="false" DefaultInsertValue="" DataField="PatientAddress" SortExpression="PatientAddress" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPatientAddress" runat="server" Text='<%#Eval("PatientAddress")%>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="REGID" HeaderText="REGID" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="REGID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("REGID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ENCID" HeaderText="ENCID" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="ENCID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblENCID" runat="server" Text='<%#Eval("ENCID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="CompanyCode" HeaderText="CompanyCode" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="CompanyCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCompanyCode" runat="server" Text='<%#Eval("CompanyCode")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="InsuranceCode" HeaderText="InsuranceCode" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="InsuranceCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInsuranceCode" runat="server" Text='<%#Eval("InsuranceCode")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="CardId" HeaderText="CardId" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="CardId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCardId" runat="server" Text='<%#Eval("CardId")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="RowNo" HeaderText="RowNo" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="RowNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="RegistrationNoOld" HeaderText="Old Reg No" ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNoOld" SortExpression="MobileNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRegistrationNoOld" runat="server" Text='<%#Eval("RegistrationNoOld")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="MotherName" HeaderText="Mother Name" ShowFilterIcon="false" DefaultInsertValue="" DataField="MotherName" SortExpression="MotherName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMotherName" runat="server" Text='<%#Eval("MotherName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="FatherName" HeaderText="Father Name" ShowFilterIcon="false" DefaultInsertValue="" DataField="FatherName" SortExpression="FatherName">
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
                            </div>
                       <div class="row">
                            <div class="col-md-12 col-sm-12">
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
                                <asp:HiddenField ID="hdnIsMedicalAlert" runat="server" Value="0" />
                            </div>
                        </div>

                        <div class="row m-b">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <h2 style="background:#d0d5e6;padding:3px 10px;font-size:14px;font-weight:bold;margin:0px;">
                                    Patient Information
                                </h2>
                                <div class="col-md-12 col-sm-12 col-xs-12 border-all">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label7" runat="server" Text="Name :" />
                                            </div>
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <asp:Label ID="lblPatientName" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label8" runat="server" Text="Address :" />
                                            </div>
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <asp:Label ID="lblPatientAddress" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblPatientKin" runat="server" Text="Kin" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
