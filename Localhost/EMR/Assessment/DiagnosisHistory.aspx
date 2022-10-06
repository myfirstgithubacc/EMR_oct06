<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DiagnosisHistory.aspx.cs" Inherits="EMR_Assessment_DiagnosisHistory"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="aspl" TagName="Left" Src="~/Include/Components/ucTree.ascx" %>
<%@ Register TagPrefix="aspl" TagName="Top" Src="~/Include/Components/MasterComponent/TopPanel.ascx" %>
<%@ Register TagPrefix="asp1" TagName="Top1" Src="~/Include/Components/TopPanel.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script src="../../Include/JS/bootstrap.js"></script>
    <script type="text/javascript" language="javascript">

        function ValidateDateRange() {

            try {
                // make sure start date is before the end date

                var date1obj = $get('ctl00_ContentPlaceHolder1_txtFromDate');
                var date2obj = $get('ctl00_ContentPlaceHolder1_txtToDate');

                if (date1obj == null || date2obj == null) {
                    //could not get date object.

                    return true;
                }

                if (date1obj.value.length == 0 || date2obj.value.length == 0) {
                    //required fields.
                    //alert("Dates are required fields.")
                    alert('test')
                    return true;
                }
                else {

                    var fromDate = date1obj.value;
                    var toDate = date2obj.value
                    if (Date.parse(fromDate) > Date.parse(toDate)) {
                        alert("Invalid Date Range!")
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            catch (e) {
                return;
            }
        } 


    </script>

        <div class="container-fluid">
                    <div class="row header_main">

       <div class="col-md-4 col-sm-4 col-xs-4"><h2 class="hidden"">Diagnosis(s) History</h2></div>
                <div class="col-md-8 col-sm-8 col-xs-8 text-right">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Add To Today's Diagnosis" OnClick="btnSave_Click" CssClass="btn btn-xs btn-primary" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn btn-xs btn-primary" />
                        <asp:Button ID="btnRefresh" runat="server" Text="Filter" CssClass="btn btn-xs btn-primary" OnClick="btnRefresh_Click"
                                OnClientClick="ValidateDateRange()" />
                            <asp:Button ID="btnClearFilter" runat="server" Text="Clear&nbsp;Filter" Visible="True"
                                CssClass="btn btn-xs btn-primary" OnClick="btnClearFilter_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
                        </div>
            <div class="row" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" style="position: relative;margin: 0px;padding: 0px 0px;width: 100%;" runat="server" Text="" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>

<div class="row">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="row">
                        <div class="col-md-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Label3" runat="server" Text="Facility" SkinID="label"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadComboBox ID="ddlFacility" Width="100%" runat="server" AutoPostBack="false">
                            </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'
                                SkinID="label"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadComboBox ID="ddlProvider" runat="server" Width="100%"
                                AutoPostBack="false">
                            </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Label1" runat="server" Text="Status" SkinID="label" Visible="false"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <div class="row">
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadComboBox ID="ddlStatus" runat="server" AutoPostBack="false"
                                Visible="false">
                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <asp:CheckBox ID="chkChronics" runat="server" Text="Chronic" TextAlign="Right" Font-Bold="true" style="margin-right: 5px; vertical-align: top;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Label2" runat="server" Text=" Visit Type" SkinID="label"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadComboBox ID="ddlSource" Width="100%" runat="server" AutoPostBack="false">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" Selected="True" />
                                    <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                    <telerik:RadComboBoxItem Text="IPD" Value="I" />
                                    <telerik:RadComboBoxItem Text="ER" Value="E" />
                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, MHC%>' Value="M" />
                                    <%-- <telerik:RadComboBoxItem Text="Package" Value="P" />--%>
                                </Items>
                            </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-md-4 col-sm-4 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Label4" runat="server" Text=" Date Range" SkinID="label"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadComboBox ID="ddldateRange" runat="server"  Width="100%"
                                OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Select All" Value="" runat="server" />
                                    <telerik:RadComboBoxItem Text="Today" Value="DD0" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Week" Value="WW-1" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Month" Value="MM-1" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Six Months" Value="MM-6" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Year" Value="YY-1" runat="server" />
                                    <telerik:RadComboBoxItem Text="Date Range" Value="6" runat="server" />
                                </Items>
                            </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-md-4 col-sm-4 col-xs-12">
                            <asp:UpdatePanel ID="updaterng" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="lblFrom" runat="server" SkinID="label" Text="From"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadDatePicker ID="dtpfromDate" Width="100%" runat="server" MinDate="1900-01-01 00:00">
                                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="lblTo" runat="server" SkinID="label" Text="To"></asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadDatePicker ID="dtpToDate" Width="100%" runat="server" MinDate="1900-01-01 00:00">
                                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                                </div>
                            </div>
                                        </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddldateRange" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                
                     
            </ContentTemplate>
               
        </asp:UpdatePanel>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvDiagnosisHistory" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                AllowFilteringByColumn="false" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="18" EnableLinqExpressions="False" AllowPaging="false"
                                AllowMultiRowSelection="true" CellSpacing="0" OnItemCommand="gvPatientHistory_OnItemCommand"
                                OnItemDataBound="gvPatientHistory_OnItemDataBound">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                    Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <PagerStyle ShowPagerText="False" />
                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="View">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle Width="30px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EncounterDate" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Visit Date" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterDate" runat="server" Text='<%#Eval("EncounterDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px" />
                                            <ItemStyle Width="70px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="VisitType" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Visit Type" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVisitTypeName" runat="server" Text='<%#Eval("VisitType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="60px" />
                                            <ItemStyle Width="60px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EncounterNo" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Enc.#" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FacilityName" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Facility Name" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityName") %>'></asp:Label>
                                                <asp:DropDownList ID="ddlLocation" runat="server" SkinID="DropDown" Width="140px"
                                                    AppendDataBoundItems="true" Visible="false">
                                                    <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="DoctorName" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Doctor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="120px" />
                                            <ItemStyle Width="120px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ICDCode" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="ICDCode" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' Width="60px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ICDDescription" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Diagnosis" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription")%>'
                                                    OnClick="lnkbtnProblem_OnClick"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" />
                                            <ItemStyle Width="250px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" AutoPostBackOnFilter="false"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                <asp:HiddenField ID="TypeId" runat="server" Value='<%#Eval("TypeId")%>' />
                                                <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                <asp:HiddenField ID="hdnFacilityId" runat="server" Value='<%#Eval("FacilityId")%>' />
                                                <asp:HiddenField ID="hdnChronic" runat="server" Value='<%#Eval("IsChronic")%>' />
                                                <asp:HiddenField ID="hdnConditionIds" runat="server" Value='<%#Eval("ConditionIds")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                            <ItemStyle Width="300px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Onset" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Onset" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Location" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Location" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSide" runat="server" Text='<%#Eval("Location") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="120px" />
                                            <ItemStyle Width="120px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Chronic" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Chronic" FilterControlWidth="99%" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("Chronic") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="DiagnosisType" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Diagnosis Type" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("DiagnosisType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Conditions" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Status" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("Conditions") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="IsPrimary" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Primary" Visible="true" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="IsResolved" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="IsResolved" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Remarks" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Remarks" FilterControlWidth="99%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" Width="200px" Style="word-wrap: break-word;" runat="server"
                                                    Text='<%#Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" Skin="Metro" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
               </div>
        </div>
    </div>
</asp:Content>
