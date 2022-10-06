<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTRequestList.aspx.cs" Inherits="OTScheduler_OTRequestList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>
    <style type="text/css">
        .rgAdvPart {
            display: none;
        }
    </style>
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">


        <script language="javascript" type="text/javascript">
            function validateMaxLength() {
                var txt = $get('<%=txtSearchN.ClientID%>');
                if (txt.value > 2147483647) {
                    alert("Value should not be more then 2147483647.");
                    txt.value = txt.value.substring(0, 9);
                    txt.focus();
                }
            }
            function returnToParent() {
                var oArg = new Object();
          <%--  oArg.RegistrationId = document.getElementById(<%=hdnRegistrationId.ClientID %>).value;
            oArg.RegistrationNo = document.getElementById("hdnRegistrationNo").value
            oArg.EncounterNo = document.getElementById("hdnEncounterNo").value;
            oArg.EncounterId = document.getElementById("hdnEncounterId").value
            oArg.FacilityId = document.getElementById("hdnFacilityId").value
            oArg.OTRequestID = document.getElementById("hdnOTRequestID").value--%>

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function OnClientClose(oWnd) {
                $get('<%=btnRefresh.ClientID%>').click();
            }
        </script>
    </telerik:RadScriptBlock>

    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Default">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-3 col-sm-3 col-xs-4">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="OT Request Lists"></asp:Label>
                        </h2>
                    </div>
                        <div class="col-md-9 col-sm-9 col-xs-8 text-right">
                            <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" OnClick="btnRefresh_Click" Text="Filter" />
                        </div>
                </div>

               
                    <div class="row">
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 colxs-4 text-nowrap">
                                    <asp:Label ID="Label4" runat="server" Text="Requesting Facility" />
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadComboBox ID="ddlFacility" CssClass="drapDrowHeight" Width="100%" runat="server" AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 colxs-4 text-nowrap">
                                    <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                                </div>
                                <div class="col-md-4 col-sm-4 colxs-4 no-p-l">
                                    <telerik:RadComboBox ID="ddlName" runat="server"
                                        AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                            <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                                <div class="col-md-4 col-sm-4 colxs-4 no-p-l">
                                    <%--<asp:Panel ID="Panel22" runat="server">--%>
                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="100%"
                                        Visible="false" />
                                    <asp:TextBox ID="txtSearchN" runat="server" Text=""
                                        MaxLength="10" Visible="false" onkeyup="return validateMaxLength();" Width="100%" />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                    <%-- </asp:Panel>--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 colxs-4 text-nowrap">
                                <asp:Label ID="Label1" runat="server" Text="Booking Status" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlOTBookingStatus" CssClass="findPatientSelect-Mobile" runat="server"
                                    AppendDataBoundItems="true" style="width:100% !important;">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="A" />
                                        <telerik:RadComboBoxItem Text="Booked" Value="B" />
                                        <telerik:RadComboBoxItem Text="Pending" Value="P" Selected="true" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                            </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label17" runat="server" Text="From" Style="float: none;" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadDatePicker ID="txtFromDate" runat="server" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                                        Width="100%" DateInput-ReadOnly="false" Style="float: none;" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label2" runat="server" Text="To" Style="float: none;" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" DatePopupButton-Visible="false"
                                        ShowPopupOnFocus="true" Width="100%" DateInput-ReadOnly="false" Style="float: none;" />
                                        </div>
                                    </div>
                                </div>
                                 </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 box-legendcolor">
                            <ucl:legend ID="Legend1" runat="server" />
                        </div>
                    </div>
                    <div class="row m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                             <asp:Label ID="lblRecord" runat="server" Visible="false"></asp:Label>
                                <telerik:RadGrid ID="gvOTRequestList" runat="server" BorderWidth="0" RenderMode="Lightweight"
                                    PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                    AutoGenerateColumns="False" ShowStatusBar="false" EnableLinqExpressions="false"
                                    GridLines="none" AllowPaging="true" OnItemDataBound="gvOTRequestList_ItemDataBound"
                                    AllowAutomaticDeletes="false" ShowFooter="false" OnItemCommand="gvOTRequestList_ItemCommand"
                                    AllowSorting="true" Height="99%" PageSize="20" OnPageIndexChanged="gvOTRequestList_PageIndexChanged" OnPreRender="gvOTRequestList_PreRender">
                                    <MasterTableView AllowFilteringByColumn="false" TableLayout="Auto">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridButtonColumn UniqueName="Select" Text="Select" CommandName="Select"
                                                HeaderStyle-Width="60px" ItemStyle-ForeColor="Blue" HeaderText="Select"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridButtonColumn UniqueName="Book" Text="Book" CommandName="Book"
                                                HeaderStyle-Width="60px" ItemStyle-ForeColor="Blue" HeaderText="Select"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridButtonColumn UniqueName="Consent" Text="Consent Form" CommandName="Consent"
                                                HeaderStyle-Width="60px" ItemStyle-ForeColor="Blue" HeaderText="Consent Form"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <telerik:GridTemplateColumn UniqueName="Facility" HeaderText="Requesting Facility"
                                                DataField="Facility" SortExpression="Facility" HeaderStyle-Width="30px"
                                                ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnOTRequestID" runat="server" Value='<%#Eval("OTRequestID") %>' />
                                                    <asp:HiddenField ID="hdnIsEmergency" runat="server" Value='<%#Eval("IsEmergency") %>' />
                                                    <asp:HiddenField ID="hdnOTBookingID" runat="server" Value='<%#Eval("OTBookingID") %>' />
                                                    <asp:HiddenField ID="hdnOTBookingNo" runat="server" Value='<%#Eval("OTBookingNo") %>' />
                                                    <asp:HiddenField ID="hdnPACClearanceBy" runat="server" Value='<%#Eval("PACClearanceBy") %>' />
                                                    <asp:HiddenField ID="hdnIsInsuranceApprovalDone" runat="server" Value='<%#Eval("IsInsuranceApprovalDone") %>' />
                                                    <asp:Label ID="lblFacility" runat="server" Text='<%#Eval("Facility")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                DataField="RegistrationNo" SortExpression="RegistrationNo" HeaderStyle-Width="60px"
                                                ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                    <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("RegistrationID")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                DataField="EncounterNo" SortExpression="EncounterNo" HeaderStyle-Width="60px"
                                                ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                    <asp:Label ID="lblEncounterID" runat="server" Text='<%#Eval("EncounterID")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" DataField="Name"
                                                SortExpression="Name" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="AgeGender" HeaderText="Age Gender" DataField="AgeGender"
                                                SortExpression="AgeGender" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="Mobile No" DataField="MobileNo"
                                                SortExpression="MobileNo" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Surgeon" DataField="DoctorName"
                                                SortExpression="DoctorName" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                    <asp:Label ID="lblDoctorId" runat="server" Text='<%#Eval("DoctorId")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="OTRequestDate" HeaderText="OT Request Date" DataField="Date"
                                                SortExpression="OTRequestDate" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("OTRequestDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ServiceName" HeaderText="Service Name" DataField="ServiceName"
                                                SortExpression="ServiceName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BookingDate" HeaderText='OT Booking Date' DataField="BookingDate"
                                                SortExpression="BookingDate" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOTBookingDate" runat="server" Text='<%#Eval("BookingDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ProvisionalDiagnosis" HeaderText='Diagnosis' DataField="ProvisionalDiagnosis"
                                                SortExpression="ProvisionalDiagnosis" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProvisionalDiagnosis" runat="server" Text='<%#Eval("ShortProvisionalDiagnosis")%>'
                                                        ToolTip='<%#Eval("ProvisionalDiagnosis")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PACRequired" HeaderText='PAC Required' DataField="PACRequired"
                                                SortExpression="PACRequired" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPACRequired" runat="server" Text='<%#Eval("PACRequired")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="FitForSurgery" HeaderText='Fit For Surgery' DataField="FitForSurgery"
                                                SortExpression="FitForSurgery" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFitForSurgery" runat="server" Text='<%#Eval("FitForSurgery")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PACEmp" HeaderText='PAC Done By' DataField="PACEmp"
                                                SortExpression="PACEmp" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPACEmp" runat="server" Text='<%#Eval("PACEmp")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PACRemarks" HeaderText='PAC Remarks' DataField="PACRemarks"
                                                SortExpression="PACRemarks" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPACRemarks" runat="server" Text='<%#Eval("PACRemarks")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="InsuranceApproval" HeaderText='Insurance Approval' DataField="InsuranceApproval"
                                                SortExpression="InsuranceApproval" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInsuranceApproval" runat="server" Text='<%#Eval("InsuranceApproval")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                        </div>
                        <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterDate" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnAgeGender" runat="server" />
                                <asp:HiddenField ID="hdnPhoneHome" runat="server" />
                                <asp:HiddenField ID="hdnMobileNo" runat="server" />
                                <asp:HiddenField ID="hdnPatientName" runat="server" />
                                <asp:HiddenField ID="hdnFacilityId" runat="server" />
                                <asp:HiddenField ID="hdnDoctorId" runat="server" />
                    </div>
                    <div class="row m-t">
                        <div class="col-md-12">
                            <asp:Button ID="btnGetInfo" runat="server" Enabled="true" OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden; float: left; width: 0px; margin: 0; padding: 0;" Text="Assign" />
                        </div>
                    </div>
                </div>

                
            </ContentTemplate>
        </asp:UpdatePanel>
  
</asp:Content>

