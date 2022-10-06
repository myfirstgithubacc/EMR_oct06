<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ResourceAppointmentGView.aspx.cs" Inherits="Appointment_ResourceAppointment_ResourceAppointmentGView"
    Title="Resource Appointments" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; border-left:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px !important;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager { background: #c1e5ef 0 -7000px repeat-x !important; color: #00156e !important;}
    </style> 

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div id="tblMain" runat="server">

                <div class="container-fluid header_main form-group">
                    <div class="col-md-3 col-sm-4"><h2><asp:Label ID="lblHeader" runat="server" Text="Resource Appointments" /></h2></div>
                    <div class="col-md-9 col-sm-8 text-center"><asp:Label ID="lblMsg" runat="server" Text="" /></div>
                </div>



                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-md-3 col-sm-3">
                            <div class="row">
                                <div class="col-md-3 col-sm-4 label2"><asp:Label ID="lblResource" runat="server" Text="Resource" /></div>
                                <div class="col-md-9 col-sm-8">
                                    <telerik:RadComboBox ID="ddlResourceName" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]"
                                        Width="100%" DropDownWidth="300px" Filter="Contains" ForeColor="Black">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <div class="row">
                                <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblRecord" runat="server" Text="Date Range" /></span></div>
                                <div class="col-md-8 col-sm-7">
                                    <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="Select All" />
                                            <telerik:RadComboBoxItem Value="DD0" Text="Today" Selected="true" />
                                            <telerik:RadComboBoxItem Value="WW-1" Text="Last Week" />
                                            <telerik:RadComboBoxItem Value="WW-2" Text="Last Two Week" />
                                            <telerik:RadComboBoxItem Value="MM0" Text="This Month" />
                                            <telerik:RadComboBoxItem Value="MM-1" Text="Last One Month" />
                                            <telerik:RadComboBoxItem Value="YY-1" Text="Last Year" />
                                            <telerik:RadComboBoxItem Value="4" Text="Date Range" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4">
                            <div class="row">
                                <div class="col-md-2 col-sm-2 label2"><asp:Label ID="lblFrom" runat="server" Text="From" /></div>
                                <div class="col-md-10 col-sm-10">
                                    <div class="row">
                                        <div class="col-md-5 col-sm-5 PaddingRightSpacing"><telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                                        <div class="col-md-2 col-sm-2 label2"><asp:Label ID="Label1" runat="server" Text="To" /></div>
                                        <div class="col-md-5 col-sm-5 PaddingLeftSpacing"><telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-2 PaddingLeftSpacing">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_OnClick" />
                            <asp:Button ID="btnClear" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="btnClear_OnClick" />
                        </div>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pnlMain" runat="server" Width="100%">
                            <telerik:RadGrid ID="gvResourceDtl" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                Skin="Office2007" Height="450" ShowFooter="false" GridLines="Both" AllowPaging="true"
                                AllowSorting="true" AllowFilteringByColumn="true" PageSize="15" Width="100%"
                                OnPreRender="gvResourceDtl_PreRender" OnItemDataBound="gvResourceDtl_OnItemDataBound">
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                    Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                </ClientSettings>
                                <MasterTableView DataKeyNames="AppointmentId" Width="100%" TableLayout="Fixed">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="SNo" UniqueName="RowNo" ShowFilterIcon="false"
                                            AllowFiltering="false" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="sno" runat="server" SkinID="label" Text='<%#Eval("RowNo")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="AppointmentDate" UniqueName="AppointmentDate"
                                            SortExpression="AppointmentDate" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" DataField="AppointmentDate" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAppDate" runat="server" Text='<%#Eval("lblAppointmentDate")%>' />
                                                <asp:HiddenField ID="hdnAppointmentId" runat="server" Value='<%#Eval("AppointmentId")%>' />
                                                <asp:HiddenField ID="hdnStatusColor" runat="server" Value='<%#Eval("StatusColor")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Time Slot" UniqueName="ShowTime" SortExpression="ShowTime"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            DataField="ShowTime" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShowTime" runat="server" Text='<%#Eval("ShowTime")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration, regno%>" UniqueName="RegistrationNo"
                                            SortExpression="RegistrationNo" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" DataField="RegistrationNo" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUHID" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration, IpNo%>" UniqueName="EncounterNo"
                                            SortExpression="EncounterNo" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" DataField="EncounterNo" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIpNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient" SortExpression="PatientName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            DataField="PatientName" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="25%" />
                                            <ItemStyle Width="25%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="AgeGender" HeaderText="Age/Sex" SortExpression="AgeGender"
                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            DataField="AgeGender" FilterControlWidth="95%" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Ward Name" UniqueName="WardName" SortExpression="WardName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            DataField="WardName" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardNo" runat="server" Text='<%#Eval("WardName")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="BedNo" UniqueName="BedNo" SortExpression="BedNo"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            DataField="BedNo" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status" SortExpression="Status"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            DataField="Status" FilterControlWidth="95%" AllowFiltering="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusColor" runat="server" Text=". . ." />
                                                &nbsp;
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>