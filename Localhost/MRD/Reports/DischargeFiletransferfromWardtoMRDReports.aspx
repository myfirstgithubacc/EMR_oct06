<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DischargeFiletransferfromWardtoMRDReports.aspx.cs" Inherits="MRD_Reports_DischargeFiletransferfromWardtoMRDReports" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-xs-12" id="tdHeader" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" Text="Discharge File Transfer From Ward To MRD Reports" />
                    </div>
                    <div class="col-md-5 col-sm-5 col-xs-12 box-col-checkbox">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-6 box-col-checkbox">
                                <asp:CheckBox ID="chklimittime" runat="server" TextAlign="Left" Text="Limit time" AutoPostBack="true" />
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6 box-col-checkbox">
                                <asp:CheckBox ID="chkExport" Checked="true" runat="server" Text="Export" />
                            </div>
                        </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12 text-right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Print Reports" OnClick="btnSearch_Click" />
                    </div>
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
                                            <asp:ListItem Text="<%$ Resources:PRegistration, PatientName%>" Value="4" />
                                            <asp:ListItem Text="Rack Number" Value="7" />
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                            <asp:TextBox ID="txtSearch" SkinID="textbox" runat="server" Width="100%" MaxLength="20" />
                                            <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="100%" MaxLength="20" Visible="false" />
                                            <Ajax:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                        </asp:Panel>
                                    </div>
                                            </div>
                                        </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True"
                                            Width="100%" CausesValidation="false">
                                            <asp:ListItem Text="All" Value="0" />
                                            <asp:ListItem Text="Open" Value="1" />
                                            <asp:ListItem Text="Unacknowledged" Value="2" />
                                            <asp:ListItem Text="Acknowledged" Value="3" />
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
                                                       runat="server"  ShowMoreResultsBox="false" AppendDataBoundItems="true"   Width="100%" />
                            </div>
                        </div>
                    </div>
                    
                    
                </div>
                <div class="row">
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

