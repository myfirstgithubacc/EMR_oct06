<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DischargeSummary.aspx.cs" Inherits="WardManagement_DischargeSummary" Title="Discharge Summary" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; border-left:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px !important;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager { background: #c1e5ef 0 -7000px repeat-x !important; color: #00156e !important;}
    </style>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main form-group">
                <div class="col-md-9 col-sm-9 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                    <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-default" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                </div>
            </div>
            
            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-6 col-sm-6">
                        <div class="row">
                            <div class="col-md-2 col-sm-2 label2"><span class="textName"><asp:Label ID="Label3" runat="server" Text="Search On" /></span></div>
                            <div class="col-md-10 col-sm-10">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                            <asp:ListItem Text="<%$ Resources:PRegistration, EncounterNo%>" Value="1" />
                                            <asp:ListItem Text="<%$ Resources:PRegistration, UHID%>" Value="2" />
                                            <asp:ListItem Text="<%$ Resources:PRegistration, phone%>" Value="5" />
                                            <asp:ListItem Text="<%$ Resources:PRegistration, mobile%>" Value="6" />
                                            <asp:ListItem Text="<%$ Resources:PRegistration, PatientName%>" Value="4" />
                                            <asp:ListItem Text='<%$ Resources:PRegistration, bedno%>' Value="3" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 col-sm-4 PaddingLeftSpacing">
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                            <asp:TextBox ID="txtSearch" runat="server" Width="100%" MaxLength="20" />
                                            <asp:TextBox ID="txtRegNo" runat="server" Width="100%" MaxLength="20" Visible="false" />
                                            <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                        </asp:Panel>
                                    </div>
                                    <div class="col-md-4 col-sm-4 PaddingLeftSpacing">
                                        <asp:DropDownList ID="ddlTime" runat="server" AutoPostBack="True"
                                            Width="100%" CausesValidation="false" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                            <%--<asp:ListItem Text="All" Value="All" />--%>
                                            <asp:ListItem Text="Today" Value="Today" Selected="true" />
                                            <asp:ListItem Text="Last Week" Value="LastWeek" />
                                            <asp:ListItem Text="Last Two Weeks" Value="LastTwoWeeks" />
                                            <asp:ListItem Text="Last One Month" Value="LastOneMonth" />
                                            <asp:ListItem Text="Last Three Months" Value="LastThreeMonths" />
                                            <asp:ListItem Text="Last Year" Value="" />
                                            <asp:ListItem Text="Date Range" Value="DateRange" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-3">
                        <div class="row" id="tblDate" runat="server" visible="false">
                            <div class="col-md-2 col-sm-2 label2"><asp:Label ID="Label17" runat="server" Text="From" /></div>
                            <div class="col-md-10 col-sm-10">
                                <div class="row">
                                    <div class="col-md-5 col-sm-5 PaddingRightSpacing"><telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                    <div class="col-md-2 col-sm-2 label2"><asp:Label ID="Label18" runat="server" Text="To" /></div>
                                    <div class="col-md-5 col-sm-5 PaddingLeftSpacing"><telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-3 margin_Top PaddingLeftSpacing">
                        <div class="row">
                            <div class="col-md-6 col-sm-6"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkDeathSummary" runat="server" Text="Death Summary"/></div></div>
                            <div class="col-md-6 col-sm-6 PaddingLeftSpacing"><asp:Label ID="lblTotRecord" runat="server" ForeColor="Red" Visible="true" Font-Bold="true"></asp:Label></div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="100%" BorderWidth="0"
                        BorderColor="SkyBlue" ScrollBars="Auto">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvEncounter" runat="server" Skin="Office2007" BorderWidth="0"
                                    PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                    Width="100%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                    GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                    AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" PageSize="10" OnItemCommand="gvEncounter_OnItemCommand"
                                    OnPageIndexChanged="gvEncounter_OnPageIndexChanged" AllowCustomPaging="true">
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                    </ClientSettings>
                                    <PagerStyle ShowPagerText="true" />
                                    <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false"
                                        Width="100%">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <ItemStyle Wrap="true" />
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" 
                                                HeaderText="Edit" HeaderStyle-Width="30px"
                                                ItemStyle-Width="30px" >
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="IbtnSelect" runat="server" Text="Edit" CommandName="Select" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="OPIP" HeaderText="OP/IP" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="OPIP" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterNo" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="Name" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdnPatientAddress" runat="server" Value='<%#Eval("PatientAddress")%>' />
                                                    <asp:HiddenField ID="hdnKinName" runat="server" Value='<%#Eval("KinName")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="GenderAge" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="DoctorName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CurrentBedNo" HeaderText="Bed No" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CurrentBedNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="EncDate" HeaderText="Admission Date" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="EncDate" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncDate" runat="server" Text='<%#Eval("EncDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="DischargeStatus" HeaderText="Discharge Status"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="DischargeStatus">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDischargeStatus" runat="server" Text='<%#Eval("DischargeStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="EncounterStatus" HeaderText="Encounter Status"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="EncounterStatus" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterStatus" runat="server" Text='<%#Eval("EncounterStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CompanyName" HeaderText="Company" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CompanyName" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PhoneHome" HeaderText="PhoneHome" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="PhoneHome" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhoneHome" runat="server" Text='<%#Eval("PhoneHome")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="MobileNo" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="MobileNo" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="DOB" HeaderText="DOB" ShowFilterIcon="false" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                Visible="false" DefaultInsertValue="" DataField="DOB" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PatientAddress" HeaderText="PatientAddress" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                ShowFilterIcon="false" AllowFiltering="false" DefaultInsertValue="" DataField="PatientAddress" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientAddress" runat="server" Text='<%#Eval("PatientAddress")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="REGID" HeaderText="REGID" Visible="false" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="REGID" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("REGID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ENCID" HeaderText="ENCID" Visible="false" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="ENCID" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblENCID" runat="server" Text='<%#Eval("ENCID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CompanyCode" HeaderText="CompanyCode" Visible="false" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="CompanyCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompanyCode" runat="server" Text='<%#Eval("CompanyCode")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="InsuranceCode" HeaderText="InsuranceCode" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="InsuranceCode" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInsuranceCode" runat="server" Text='<%#Eval("InsuranceCode")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CardId" HeaderText="CardId" Visible="false" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="CardId" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCardId" runat="server" Text='<%#Eval("CardId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="RowNo" HeaderText="RowNo" Visible="false" HeaderStyle-Width="0%" FilterControlWidth="0%"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="RowNo" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("RowNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="DischargeDate" HeaderText="Discharge Date"
                                                Visible="true" ShowFilterIcon="false" DefaultInsertValue="" DataField="DischargeDate" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDischargeDate" runat="server" Text='<%#Eval("DischargeDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="FinalizeDate" HeaderText="FinalizeDate"
                                                Visible="true" ShowFilterIcon="false" DefaultInsertValue="" DataField="FinalizeDate" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFinalizeDate" runat="server" Text='<%#Eval("FinalizeDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" Visible="true" AllowFiltering="false"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFinalize" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>

                                <script type="text/javascript">
                                    function ShowPatientDetails(PatientName, PatientAddress, KinName) {
                                        document.getElementById("<%=lblPatientName.ClientID%>").innerHTML = $get(PatientName).innerHTML;
                                        document.getElementById("<%=lblPatientAddress.ClientID%>").innerHTML = $get(PatientAddress).value;
                                        document.getElementById("<%=lblPatientKin.ClientID%>").innerHTML = $get(KinName).value;
                                    }
                                </script>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>

            </div>

            <div class="container-fluid header_mainGray margin_z">
                <div class="col-md-12 col-sm-12"><h2>Patient Information</h2></div>
            </div>

            <table cellspacing="0" class="table table-small-font table-bordered table-striped" style="background-color:#ffffff !important;">
                <tbody>
                    <tr align="left">
                        <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1"><asp:Label ID="Label1" runat="server" Text="Name " ForeColor="Brown" Font-Size="8pt" />:&nbsp;<asp:Label ID="lblPatientName" runat="server" ForeColor="Brown" Font-Size="8pt" /></td>
                        <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2"><asp:Label ID="Label2" runat="server" Text="Address " ForeColor="Brown" Font-Size="8pt" />:&nbsp;<asp:Label ID="lblPatientAddress" runat="server" ForeColor="Brown" Font-Size="8pt" /></td>
                        <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3"><asp:Label ID="Label4" runat="server" Text="Other" ForeColor="Brown" Font-Size="8pt" />:&nbsp;<asp:Label ID="lblPatientKin" runat="server" ForeColor="Brown" Font-Size="8pt" /></td>
                    </tr>    
                </tbody>
            </table>

            <div class="container-fluid">
                <div class="row">
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
                     <asp:HiddenField ID="hdnDoctorName" runat="server" />
                    <asp:HiddenField ID="hdnCurrentBedNo" runat="server" />

                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>