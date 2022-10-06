<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ImmuPatientDashBoard.aspx.cs" Inherits="EMR_ImmuPatientDashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/MasterComponent/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <style>
        #ctl00_ContentPlaceHolder1_ddlrange_Input {
            width: 100% !important;
        }
        .findPatientText{
            width:60px!important;
        }
        .FPKeyBtn {
            margin: 0px 3px 0 0px!important;
            padding: 3px 8px!important;
        }
        div#ctl00_ContentPlaceHolder1_ddlStatus{
            width:100px!important;
        }
        span#ctl00_ContentPlaceHolder1_dtpToDate_dateInput_wrapper{
            width:77px!important;
        }
       span#ctl00_ContentPlaceHolder1_dtpfromDate_dateInput_wrapper{
           width:77px!important;
       }
       div#ctl00_ContentPlaceHolder1_ddlrange{
           width:100%!important;
       }
       .findPatientSelect-Mobile{
           margin-left:10px!important;
       }
    </style>
    <asp:UpdatePanel ID="valMain" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main margin_bottom">
                <div class="col-md-4">
                    <h2>Immunisation Dashboard
                    </h2>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-3">
                        <span class="findPatientText">
                            <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                        </span>

                        <telerik:RadComboBox ID="ddlName" CssClass="findPatientSelect-Mobile" runat="server"
                            AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                <telerik:RadComboBoxItem Text="Patient Name" Value="PN" />
                                <telerik:RadComboBoxItem Text="Mobile No." Value="MOB" />
                                <telerik:RadComboBoxItem Text="Mother Name" Value="MN" />
                            </Items>
                        </telerik:RadComboBox>

                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearch">
                            <asp:TextBox ID="txtSearch" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                Visible="false" />
                            <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" runat="server" Text=""
                                MaxLength="10" Visible="false" onkeyup="return validateMaxLength();" />
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                        </asp:Panel>
                    </div>
                    <div class="col-md-3">
                        <span class="findPatientText">
                            <asp:Label ID="lblDate" runat="server" Text="Date" /></span> <span class="findPatientSelect">
                                <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True"
                                    CausesValidation="false" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged" EmptyMessage="Select All" Width="70%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select All" Value="" />
                                        <telerik:RadComboBoxItem Text="Today" Value="T" />
                                        <telerik:RadComboBoxItem Text="Yesterday" Value="Y" />
                                        <telerik:RadComboBoxItem Text="Last Week" Value="LW" />
                                        <telerik:RadComboBoxItem Text="Last Month" Value="LM" />
                                        <telerik:RadComboBoxItem Text="Last Six Months" Value="LSM" />
                                        <telerik:RadComboBoxItem Text="Last One Year" Value="LOY" />
                                        <telerik:RadComboBoxItem Text="Next Day" Value="ND" />
                                        <telerik:RadComboBoxItem Text="Next Week" Value="NW" />
                                        <telerik:RadComboBoxItem Text="Next Month" Value="NM" />
                                        <telerik:RadComboBoxItem Text="Date Range" Value="DR" />
                                    </Items>
                                </telerik:RadComboBox>

                                <div id="tblDateRange" runat="server">
                                    <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="70px" AutoPostBack="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    <span id="spTo" runat="server">&nbsp;To&nbsp;</span>
                                    <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="70px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                </div>
                            </span>
                        
                    </div>
                    <div class="col-sm-2">
                        <span class="findPatientText">
                            <asp:Label ID="lblVisitType" runat="server" Text="VisitType" />
                        </span>
                        <telerik:RadComboBox ID="ddlVisitType" runat="server" EmptyMessage="Both"
                            CausesValidation="false" Width="45%">
                            <Items>
                                <telerik:RadComboBoxItem Selected="true" Text="Both" Value="" />
                                <telerik:RadComboBoxItem Text="IP" Value="I" />
                                <telerik:RadComboBoxItem Text="OP" Value="O" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>

                    <div class="col-md-2">
                        <span class="findPatientText">
                            <asp:Label ID="lblAppointmentStatus" runat="server" Text="Status" /></span>
                        <span class="findPatientSelect">
                            <telerik:RadComboBox ID="ddlStatus" runat="server" AutoPostBack="true" EnableCheckAllItemsCheckBox="true"
                                ShowMoreResultsBox="false" AppendDataBoundItems="true" EmptyMessage="Select All">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value=" " />
                                    <telerik:RadComboBoxItem Text="Due" Value="D" />
                                    <telerik:RadComboBoxItem Text="Given" Value="G" />
                                    <telerik:RadComboBoxItem Text="Refused" Value="R" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                </Windows>
                            </telerik:RadWindowManager>


                        </span>
                    </div>
                    <div class="col-md-2 pull-right text-right"style="padding:0px;">
                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="FPKeyBtn"
                            OnClick="btnSearch_Click" Text="Refresh" />
                        <asp:Button ID="btn_ClearFilter" runat="server" CausesValidation="false" CssClass="FPKeyBtn"
                            Text="Reset Filter" OnClick="btn_ClearFilter_Click" />
                        <asp:Button ID="btnPrint" runat="server" CausesValidation="false" CssClass="FPKeyBtn"
                            Text="Print" OnClick="btnPrint_Click" />
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <telerik:RadGrid ID="gvImmunPatientDashBoardDetails" runat="server" Skin="Office2007" RenderMode="Lightweight" Height="500px" ItemStyle-Height="30px"
                                AutoGenerateColumns="false" AllowPaging="false" CellPadding="0" CellSpacing="0" AllowSorting="false" GridLines="None"
                                AllowMultiRowSelection="false" AllowCustomPaging="false" AllowAutomaticDeletes="false" ShowStatusBar="true" AllowFilteringByColumn="false"
                                ShowFooter="false" EnableLinqExpressions="false" BorderWidth="0" PagerStyle-ShowPagerText="false" HeaderStyle-Wrap="false">
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                    <Selecting UseClientSelectColumnOnly="true" />
                                    <%--AllowRowSelect="True"--%>
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                    <Scrolling AllowScroll="True" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                </ClientSettings>
                                <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" Width="100%">
                                    <%--  <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="EncounterId" SortOrder="Descending" />
                                                </SortExpressions>--%>
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="UHID" HeaderText="Registration No." SortExpression="UHID" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUHID" runat="server" Text='<%#Eval("UHID")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" SortExpression="PatientName" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PatientAgeGender" HeaderText="Age Gender" SortExpression="PatientAgeGender" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("PatientAgeGender")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="MotherName" HeaderText="Mother Name" SortExpression="MotherName"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMotherName" runat="server" Text='<%#Eval("MotherName")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="Mobile No." SortExpression="MobileNo"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="FullAddress" HeaderText="Address" SortExpression="FullAddress" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFullAddress" runat="server" Text='<%#Eval("FullAddress")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Doctor" HeaderText="Doctor" SortExpression="Doctor" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDoctor" runat="server" Text='<%#Eval("Doctor")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="VaccinationName" HeaderText="Vaccination Name" SortExpression="VaccinationName" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVaccinationName" runat="server" Text='<%#Eval("VaccinationName")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="GivenByName" HeaderText="Given By" SortExpression="GivenByName" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGivenByName" runat="server" Text='<%#Eval("GivenByName")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="GivenDateTime" HeaderText="Date" SortExpression="GivenDateTime" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGivenDateTime" runat="server" Text='<%#Eval("GivenDateTime")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="VisitType" HeaderText="Visit Type" SortExpression="OPIP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" SortExpression="Status"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Remarks" HeaderText="Remarks" SortExpression="Remarks" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

