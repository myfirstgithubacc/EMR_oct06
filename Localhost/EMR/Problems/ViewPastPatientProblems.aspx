<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ViewPastPatientProblems.aspx.cs" Inherits="EMR_Problems_ViewPastPatientProblems_" Title="Complaint(s) History" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />


    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-xs-4"></div>
            <div class="col-xs-8 text-right">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Add To Todays Problem" CssClass="btn btn-xs btn-primary"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-xs btn-primary" Text="Filter" OnClick="btnFilter_Click" />
                        <asp:Button ID="btnrefresh" runat="server" CssClass="btn btn-xs btn-primary" Text="Clear Filter" OnClick="btnrefresh_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-xs btn-primary" OnClick="btnBack_Click" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-xs btn-primary" OnClientClick="window.close();" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row text-center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                <asp:UpdatePanel ID="UpdatePanel2" style="margin:0px;" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" style="position:relative;margin:0px;width:100%;" runat="server" Text="&nbsp;" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
 
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel4" class="col-md-12 col-sm-12 col-xs-12" runat="server">
                <ContentTemplate>
                     <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <label class="col-md-3 col-sm3 col-xs-4">
                                <asp:Label ID="Label3" runat="server" Text="Facility" SkinID="label"></asp:Label></label>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <telerik:RadComboBox ID="ddlFacility" runat="server" Width="100%" AutoPostBack="false"
                                    Skin="Default">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <label class="col-md-3 col-sm3 col-xs-4">
                                <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'
                                    SkinID="label"></asp:Label></label>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <telerik:RadComboBox ID="ddlProvider" runat="server" Width="100%"
                                    TabIndex="0" Font-Size="11px" AutoPostBack="false" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <label class="col-md-3 col-sm3 col-xs-4">
                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="Visit Type" style="white-space: nowrap;" /></label>
                           <div class="col-md-9 col-sm-9 col-xs-8">
                                <telerik:RadComboBox ID="ddlSource" Width="100%" runat="server"
                                    AutoPostBack="false">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="A" Selected="True" />
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
                         </div>
                     <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <label class="col-md-3 col-sm3 col-xs-4">
                                <asp:Label ID="lblDate" runat="server" SkinID="label" Text="Date"></asp:Label></label>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <telerik:RadComboBox ID="ddlrange" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged"
                                    SkinID="DropDown" CausesValidation="false">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>


                        <div class="col-md-2 col-sm-2 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-xs-12 box-col-checkbox">
                                <asp:CheckBox ID="chkChronics" runat="server" Text="Chronic" TextAlign="Right" Font-Bold="true" /></div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <asp:Label ID="lblfrndate" runat="server" Text="From" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <telerik:RadDatePicker ID="dtpfrmdate" runat="server" Width="100%">
                                </telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <asp:Label ID="lbltodate" runat="server" Text="To" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <telerik:RadDatePicker ID="dtpTodate" runat="server" Width="100%">
                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                         </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
   

        <div class="row m-t">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div runat="server" id="divmessage" class="bg-danger well-sm text-center">
                            <label>No Chip Complaint Found</label>
                        </div>
                        <telerik:RadGrid ID="gvPatientHistory" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                            AllowFilteringByColumn="false" runat="server" Width="100%" AutoGenerateColumns="False"
                            PageSize="18" EnableLinqExpressions="False" AllowPaging="false"
                            AllowMultiRowSelection="true" CellSpacing="0" OnItemCommand="gvPatientHistory_OnItemCommand"
                            OnItemDataBound="gvPatientHistory_OnItemDataBound" CssClass="table-ui">
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
                                        No Record Found.
                                    </div>
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
                                            <asp:Label ID="lblEncounterDate" runat="server" SkinID="label" Text='<%#Eval("EncounterDate")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle Width="80px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="VisitType" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Visit Type" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVisitType" runat="server" SkinID="label" Text='<%#Eval("VisitType")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Enc.#" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle Width="80px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="FacilityName" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Facility Name" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFacilityName" runat="server" SkinID="label" Text='<%#Eval("FacilityName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DoctorName" AllowFiltering="false" ShowFilterIcon="false"
                                        AutoPostBackOnFilter="false" HeaderText="Doctor Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="160px" />
                                        <ItemStyle Width="160px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ProblemDescription" AllowFiltering="false"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="false" HeaderText="Problem Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnProblem" runat="server" Text='<%#Eval("ProblemDescription")%>'
                                                OnClick="lnkbtnProblem_OnClick">
                                            </asp:LinkButton>
                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id")%>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                            <asp:HiddenField ID="HdnProblemId" runat="server" Value='<%#Eval("ProblemId")%>' />
                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                            <asp:HiddenField ID="hdnFacilityId" runat="server" Value='<%#Eval("FacilityId")%>' />
                                            <asp:HiddenField ID="hdnChronic" runat="server" Value='<%#Eval("IsChronic")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="300px" />
                                        <ItemStyle Width="300px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Chronic" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Chronic" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChronic" runat="server" SkinID="label" Text='<%#Eval("Chronic")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Location" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Location" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" SkinID="label" Text='<%#Eval("Location")%>'></asp:Label><asp:Label
                                                ID="lblLocationId" runat="server" SkinID="label" Text='<%#Eval("LocationId")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="120px" />
                                        <ItemStyle Width="120px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Onset" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Onset" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOnset" runat="server" SkinID="label" Text='<%#Eval("Onset")%>'>
                                            </asp:Label><asp:Label ID="lblOnsetId" runat="server" SkinID="label" Text='<%#Eval("OnsetId")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Duration" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Duration" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDuration" runat="server" SkinID="label" Text='<%#Eval("Duration")%>'></asp:Label><asp:Label
                                                ID="lblDurationId" runat="server" SkinID="label" Text='<%#Eval("DurationId")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Quality" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Quality" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuality" runat="server" SkinID="label" Text='<%#Eval("Quality")%>'></asp:Label>
                                            <asp:Label ID="lblQualityId" runat="server" SkinID="label" Text='<%#Eval("QualityId1")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Context" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Context" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContext" runat="server" SkinID="label" Text='<%#Eval("Context")%>'></asp:Label><asp:Label
                                                ID="lblContextId" runat="server" SkinID="label" Text='<%#Eval("ContextId")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Severity" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Severity" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSeverity" runat="server" SkinID="label" Text='<%#Eval("Severity")%>'></asp:Label><asp:Label
                                                ID="lblSeverityId" runat="server" SkinID="label" Text='<%#Eval("SeverityID")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AssociatedProblem1" AllowFiltering="false"
                                        ShowFilterIcon="false" HeaderText="AssociatedProblem1" FilterControlWidth="99%"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociatedProblemID1" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID1")%>'
                                                Visible="false"></asp:Label><asp:Label ID="lblAssociatedProblem1" runat="server"
                                                    SkinID="label" Text='<%#Eval("AssociatedProblem1")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AssociatedProblem2" AllowFiltering="false"
                                        ShowFilterIcon="false" HeaderText="AssociatedProblem2" FilterControlWidth="99%"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociatedProblemID2" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID2")%>'
                                                Visible="false"></asp:Label><asp:Label ID="lblAssociatedProblem2" runat="server"
                                                    SkinID="label" Text='<%#Eval("AssociatedProblem2")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AssociatedProblem3" AllowFiltering="false"
                                        ShowFilterIcon="false" HeaderText="AssociatedProblem3" FilterControlWidth="99%"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociatedProblemID3" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID3")%>'
                                                Visible="false"></asp:Label><asp:Label ID="lblAssociatedProblem3" runat="server"
                                                    SkinID="label" Text='<%#Eval("AssociatedProblem3")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AssociatedProblem4" AllowFiltering="false"
                                        ShowFilterIcon="false" HeaderText="AssociatedProblem4" FilterControlWidth="99%"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociatedProblemID4" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID4")%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblAssociatedProblem4" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblem4")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AssociatedProblem5" AllowFiltering="false"
                                        ShowFilterIcon="false" HeaderText="AssociatedProblem5" FilterControlWidth="99%"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociatedProblemID5" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID5")%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblAssociatedProblem5" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblem5")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="IsPrimary" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="IsPrimary" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrimary" runat="server" SkinID="label" Text='<%#Eval("IsPrimary")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="SNOMEDCode" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="SNOMED Code" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSCTId" runat="server" SkinID="label" Text='<%#Eval("SNOMEDCode")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="SideDescription" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="SideDescription" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSideDescription" runat="server" SkinID="label" Text='<%#Eval("SideDescription")%>'></asp:Label>
                                            <asp:Label ID="lblSide" runat="server" SkinID="label" Text='<%#Eval("Side")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Condition" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Condition" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCondition" runat="server" SkinID="label" Text='<%#Eval("Condition")%>'></asp:Label><asp:Label
                                                ID="lblConditionID" runat="server" SkinID="label" Text='<%#Eval("ConditionID")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Percentage" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="Percentage" FilterControlWidth="99%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPercentage" runat="server" SkinID="label" Text='<%#Eval("Percentage")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EnteredBy" HeaderText="Entered By" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnteredBy" SkinID="label" runat="server" Width="98%" Text='<%#Eval("EnteredBy")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="EnteredDate" HeaderText="Entered Date" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncodedDate" SkinID="label" runat="server" Width="98%" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnPDurations" runat="server" Value='<%#Eval("Durations") %>' />
                                            <asp:HiddenField ID="hdnPDurationType" runat="server" Value='<%#Eval("DurationType") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                    <%--      <telerik:GridTemplateColumn UniqueName="View" AllowFiltering="false" ShowFilterIcon="false"
                                        HeaderText="View" FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkview" runat="server" Text="View Case sheet" CommandName="Add"></asp:LinkButton>
                                            <asp:HiddenField ID="hdnPDurations" runat="server" Value='<%#Eval("Durations") %>' />
                                            <asp:HiddenField ID="hdnPDurationType" runat="server" Value='<%#Eval("DurationType") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </telerik:GridTemplateColumn>--%>
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
