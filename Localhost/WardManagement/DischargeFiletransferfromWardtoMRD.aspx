<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DischargeFiletransferfromWardtoMRD.aspx.cs" Inherits="WardManagement_DischargeFiletransferfromWardtoMRD" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />
    <style>
        select#ctl00_ContentPlaceHolder1_ddlSearchOn {
            height:22px!important;
}
         select#ctl00_ContentPlaceHolder1_ddlStatus {
            height:22px!important;
}
          select#ctl00_ContentPlaceHolder1_ddlTime{
            height:22px!important;
}
          .checkbox{
              margin-right:18px!important;
          }
    </style>
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <asp:Panel ID="pnlgrid" runat="server" Width="100%">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row header_main">
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <h2><asp:Label ID="lblHeader" runat="server" Text="Discharge file transfer from Ward to MRD" /></h2>
                        </div>
                        <div class="col-md-2 col-sm-2 col-xs-3">
                                <%--<asp:CheckBox ID="chklimittime" runat="server" TextAlign="Left" Text="Limit time" OnCheckedChanged="chklimittime_CheckedChanged" AutoPostBack="true" />--%>
                            </div>
                        <div class="col-md-4 col-sm-4 col-xs-12 text-right">
                            <asp:Button ID="btnReject"  runat="server" CssClass="btn btn-primary" Text="Reject File" OnClick="btnReject_Click" />                          
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearSearch_Click" />
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Sent to MRD" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSaveOtherremark" runat="server" CssClass="btn btn-primary" Text="Save Other Remark" OnClick="btnSaveOtherremark_Click" />

                        </div>
                    </div>
                        <div class="row text-center">
                            <asp:Label ID="lblMessage" runat="server" Width="100%" Text="&nbsp;" />
                        </div>
                    
                        <div class="row">
                            <div class="col-md-8 col-sm-8">
                        <div class="row">
                            <div class="col-md-2 col-sm-2 col-xs-3 label2 p-t-b-5">
                                <asp:Label ID="Label3" runat="server" Text="Search On" />
                            </div>
                            <div class="col-md-10 col-sm-10 col-xs-9">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                                    <asp:ListItem Text="<%$ Resources:PRegistration, EncounterNo%>" Value="1" />
                                                    <asp:ListItem Text="<%$ Resources:PRegistration, UHID%>" Value="2" />
                                                    <%--  <asp:ListItem Text="<%$ Resources:PRegistration, phone%>" Value="5" />--%>
                                                    <%--<asp:ListItem Text="<%$ Resources:PRegistration, mobile%>" Value="6" />--%>
                                                    <asp:ListItem Text="<%$ Resources:PRegistration, PatientName%>" Value="4" />
                                                    <asp:ListItem Text="Rack Number" Value="7" />
                                                    <%--<asp:ListItem Text='<%$ Resources:PRegistration, bedno%>' Value="3" />--%>
                                                </asp:DropDownList>

                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtSearch" runat="server" Width="100%" MaxLength="20" />
                                                    <asp:TextBox ID="txtRegNo" runat="server" Width="100%" MaxLength="20" Visible="false" />
                                                    <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                                </asp:Panel>
                                    </div>
                                            </div>
                                        </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True"
                                                    Width="100%" CausesValidation="false" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                    <asp:ListItem Text="Open" Value="1" />
                                                    <asp:ListItem Text="Unacknowledged" Value="2" />
                                                    <asp:ListItem Text="Acknowledged" Value="3" />
                                                    <asp:ListItem Text="Reject" Value="5" />

                                                </asp:DropDownList>

                                    </div>
                                    <div class="col-md-6 col-sm-6">
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
                        </div>
                    </div>


                            <div class="col-md-3 col-sm-3">
                        <div class="row" id="tblDate" runat="server" visible="false">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-xs-4">
                                <asp:Label ID="Label17" runat="server" Text="From" />
                            </div>
                                    <div class="col-md-9 col-sm-9 col-xs-8">
                                       <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-xs-4">
                                       <asp:Label ID="Label18" runat="server" Text="To" />
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-xs-8">
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                    </div>
                                </div>
                            
                        </div>
                    </div>
                            </div>

                          
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label1" runat="server" Text="Ward Name" />
                            </div>
                            <div class="col-8 col-sm-8 col-xs-8">
                               <telerik:RadComboBox ID="ddlWard" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select All"
                                                    ShowMoreResultsBox="false" AppendDataBoundItems="true" Width="100%" />
                            </div>
                        </div>
                    </div>

                                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-4 col-sm-4 col-xs-4 text-nowrap">
                               <asp:Label ID="Label2" runat="server" Text="Company Name" />
                            </div>
                            <div class="col-8 col-sm-8 col-xs-8">
                               <telerik:RadComboBox ID="ddlInsuranceCompany" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                    runat="server" ShowMoreResultsBox="false" AppendDataBoundItems="true" Width="100%" />
                            </div>
                        </div>
                    </div>
                                </div>
                        <div class="row m-t">
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                                <div style="overflow:scroll;width:96vw;float:left;">
                        <asp:GridView ID="gvEncounter" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20"
                            Width="100%" OnPageIndexChanging="gvEncounter_PageIndexChanging" OnRowCommand="gvEncounter_RowCommand" OnRowDataBound="gvEncounter_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAck" runat="server" CssClass="checkbox" />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("ENCID")%>' />
                                        <asp:HiddenField ID="hdnorderno" runat="server" Value='<%#Eval("OrderNo")%>' />
                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("REGID")%>' />
                                        <asp:HiddenField ID="hdnMoreThenTimeLimit" runat="server" Value='<%#Eval("MoreThenTimeLimit")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="Encounter No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="Patient Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("NAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField  HeaderText="Admission Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAdmissionDate" runat="server" Text='<%#Eval("AdmissionDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discharge Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDischargeDate" runat="server" Text='<%#Eval("DischargeDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField  HeaderText="MRD Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSendtoMrdNew" Text='<%#Eval("SendtoMrdNew") %>' runat="server"></asp:Label>
                                        <asp:Label ID="lblSendtoMRDStatus" Visible="false" Text='<%#Eval("SendtoMrd") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="Date Sent From MRD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSendMRDIssueDate" Text='<%#Eval("SendMRDIssueDate") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="MRD Acknowledged Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAcknowledmentStatusNew" Text='<%#Eval("AcknowledmentStatusNew") %>' runat="server"></asp:Label>
                                        <asp:Label ID="lblAcknowledmentStatus" Visible="false" Text='<%#Eval("AcknowledmentStatus") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="MRD Acknowledged Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAcknowledmentDate" Text='<%#Eval("AcknowledmentDate") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Doctor Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ward Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("CurrentWard")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discharge Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDischargeStatus" runat="server" Text='<%#Eval("DischargeStatus")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bed No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rack Number">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRackNumber" Visible="false" Text='<%#Eval("RackNumber") %>' runat="server"></asp:TextBox>

                                        <asp:Label ID="lblRackNumber" Visible="true" Text='<%#Eval("RackNumber") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark (Sent from Ward)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtremark" TextMode="MultiLine" Text='<%#Eval("SendtoMRDRemrk") %>' runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherRemark" TextMode="MultiLine" ToolTip='<%#Eval("OtherRemark") %>' Text='<%#Eval("OtherRemark") %>' runat="server" onfocus="this.value=''"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField  HeaderText="Print">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblPrint" Visible="false" CommandArgument='<%#Eval("OrderNo")%>' CommandName="Print" ToolTip="Click here to Print Reports" ForeColor="Blue" runat="server">Print</asp:LinkButton>

                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                       </div>
                                </div>
                            </div>
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
                               <%--  <asp:HiddenField ID="hdnRequestID" runat="server" />--%>

                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                    </Windows>
                                </telerik:RadWindowManager>
                            </div>
                        </div>
                
                
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </telerik:RadScriptBlock>
</asp:Content>

