<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingQueDetails.aspx.cs" Inherits="Pharmacy_SaleIssue_PendingQueDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Details</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr style="background-color: #E0EBFD">
                        <td align="left" width="70px">
                            <asp:Label ID="lblfacility" runat="server" Text="Facility" SkinID="label" />
                        </td>
                        <td align="left" width="200px">
                            <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="true" SkinID="DropDown"
                                Width="140px" />
                        </td>
                        <td align="center" style="font-size: 12px;">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </td>
                        <td align="right" style="width: 200px">
                            <asp:Button ID="btnSearch" runat="server" SkinID="Button" Text="Filter" OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server" SkinID="Button" Text="Clear Filter"
                                OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td align="center">
                            <asp:RadioButtonList ID="rdoSearch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                RepeatLayout="Flow" OnSelectedIndexChanged="rdoSearch_SelectedIndexChanged">
                                <asp:ListItem Text="Search on Criteria" Value="0" />
                                <asp:ListItem Text="Search All (Date Range)" Selected="True" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdoRegEnc" runat="server" Visible="false" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="rdoRegEnc_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2" id="tblsearch" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, UHID%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationNo" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, EncounterNo%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEncounterNo" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, PatientName%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPatientName" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" SkinID="label" Text="Date of Birth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDob" runat="server" SkinID="textbox" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, phone%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhoneNo" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, mobile%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMobileNo" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bedno%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBedNo" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" SkinID="label" Text="E-Mail Id" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailId" runat="server" SkinID="textbox" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" SkinID="label" Text="Company" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompany" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" SkinID="label" Text="Passport No" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassportno" runat="server" SkinID="textbox" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="Identity No" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCprno" runat="server" SkinID="textbox" />
                                    </td>
                                     <td>
                                        <asp:Label ID="Label14" runat="server" SkinID="label" Text="Old Reg No" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOldRegistrationno" runat="server" SkinID="textbox" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="tblDate" runat="server" visible="true">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" Width="100px"
                                            DateInput-ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="99%" BorderWidth="1"
                                BorderColor="SkyBlue" ScrollBars="Auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvEncounter" runat="server" Skin="Office2007" BorderWidth="0"
                                            PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                            Width="100%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                            GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
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
                                                        No Record Found.</div>
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
                                                            <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo" SortExpression="REGID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterNo" SortExpression="EncounterNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="Name" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                        SortExpression="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                                            <asp:HiddenField ID="hdnKinName" runat="server" Value='<%#Eval("KinName")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="GenderAge" SortExpression="GenderAge">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="DoctorName" SortExpression="DoctorName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CurrentBedNo" HeaderText="Bed No" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="CurrentBedNo" SortExpression="CurrentBedNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncDate" HeaderText="Admission Date" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="EncDate" SortExpression="EncDate">
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
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterStatus" SortExpression="EncounterStatus">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterStatus" runat="server" Text='<%#Eval("EncounterStatus")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CompanyName" HeaderText="Company" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="CompanyName" SortExpression="CompanyName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PhoneHome" HeaderText="PhoneHome" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="PhoneHome" SortExpression="PhoneHome">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPhoneHome" runat="server" Text='<%#Eval("PhoneHome")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="MobileNo" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="MobileNo" SortExpression="MobileNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DOB" HeaderText="DOB" ShowFilterIcon="false"
                                                        Visible="false" DefaultInsertValue="" DataField="DOB" SortExpression="RegistrationNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PatientAddress" HeaderText="PatientAddress"
                                                        Visible="true" ShowFilterIcon="false" DefaultInsertValue="" DataField="PatientAddress"
                                                        SortExpression="PatientAddress">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPatientAddress" runat="server" Text='<%#Eval("PatientAddress")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="REGID" HeaderText="REGID" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="REGID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("REGID")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ENCID" HeaderText="ENCID" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="ENCID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblENCID" runat="server" Text='<%#Eval("ENCID")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CompanyCode" HeaderText="CompanyCode" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="CompanyCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompanyCode" runat="server" Text='<%#Eval("CompanyCode")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="InsuranceCode" HeaderText="InsuranceCode"
                                                        Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="InsuranceCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsuranceCode" runat="server" Text='<%#Eval("InsuranceCode")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CardId" HeaderText="CardId" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="CardId">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCardId" runat="server" Text='<%#Eval("CardId")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RowNo" HeaderText="RowNo" Visible="false"
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RowNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>' />
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
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <fieldset style="width: 700px;" class="fldset">
                                <legend class="fldsetlegend"><b>Patient Information</b></legend>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 70px;">
                                            <asp:Label ID="Label7" runat="server" SkinID="label" Text="Name :" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPatientName" runat="server" SkinID="label" ForeColor="Brown" Font-Names="Tahoma"
                                                Font-Size="8pt" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" SkinID="label" Text="Address :" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPatientAddress" runat="server" SkinID="label" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblPatientKin" runat="server" SkinID="label" Text="Kin" ForeColor="Brown"
                                                Font-Names="Tahoma" Font-Size="8pt" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
