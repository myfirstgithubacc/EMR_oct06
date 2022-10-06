<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ReportOTBooking.aspx.cs" Inherits="OTScheduler_ReportOTBooking" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />  

    <div class="container-fluid header_main">
        <div class="col-md-3"><h2><asp:Label ID="lblHeader" runat="server" Text="Report OT Booking Details" /></h2></div>
        <div class="col-md-9 text-center"><asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></div>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-offset-3 col-md-5 header_main m-t">
                <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                       <asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8" id="dttdfrmdate" runat="server">
                                        <telerik:RadDatePicker ID="dtpfromdate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label3" runat="server" Text="To Date"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                       <telerik:RadDatePicker ID="dtpTodate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                    
                </div>

                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label4" runat="server" Text="OT Status"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                       <asp:DropDownList ID="ddlStatus" runat="server" CssClass="drapDrowHeight" Width="100%">
                                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">Confirmed </asp:ListItem>
                                <asp:ListItem Value="2">Unconfirmed </asp:ListItem>
                            </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-5 col-sm-5 col-xs-5 text-nowrap box-col-checkbox">
                                        <asp:CheckBox ID="chkExport" runat="server" Text="Export" />
                                    </div>
                                    <div class="col-md-7 col-sm-7 col-xs-7">
                                        <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Preview Report" CausesValidation="false" OnClick="btnPrintreport_OnClick" CssClass="btn btn-primary" Text="Preview" />
                                    </div>
                                </div>
                            </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow></Windows>
                        </telerik:RadWindowManager>
                    </div>
                </div>
                
                    </div>
            </div>
        </div>
    </div>
</asp:Content>