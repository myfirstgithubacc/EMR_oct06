<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ReportCancelOTBooking.aspx.cs" Inherits="OTScheduler_ReportCancelOTBooking" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />  

    <div class="container-fluid header_main form-group">
        <div class="col-md-3"><h2><asp:Label ID="lblHeader" runat="server" Text="Report Cancel OT Booking Details" /></h2></div>
        <div class="col-md-9 text-center"><asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></div>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-offset-3 col-md-5 header_main main_box">
                <div class="row form-groupTop01">
                    <div class="col-md-12">
                        <div class="col-md-3 label2"><asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label></div>
                        <div class="col-md-3" id="dttdfrmdate" runat="server"><telerik:RadDatePicker ID="dtpfromdate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                        <div class="col-md-3 label2"><asp:Label ID="Label3" runat="server" Text="To Date"></asp:Label></div>
                        <div class="col-md-3"><telerik:RadDatePicker ID="dtpTodate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                    </div>
                </div>

                <div class="row form-groupTop01">
                    <div class="col-md-12">
                        <div class="col-md-3 label2"><asp:Label ID="Label4" runat="server" Text="Operation Theater"></asp:Label></div>
                        <div class="col-md-3">
                             <telerik:RadComboBox ID="ddlTheatre" runat="server" EmptyMessage="ALL" Width="100%"
                                    EnableVirtualScrolling="true" CheckBoxes="false" EnableCheckAllItemsCheckBox="false" CssClass="drapDrowHeight"/>
                        </div>
                        <div class="col-md-4 margin_Top"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkExport" runat="server" Text="Export" /></div></div>
                        <div class="col-md-2"><asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Preview Report" CausesValidation="false" OnClick="btnPrintreport_OnClick" CssClass="btn btn-primary" Text="Preview" /></div>
                    </div>
                </div>

                <div class="row form-groupTop01">
                    <div class="col-md-12">
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow></Windows>
                        </telerik:RadWindowManager>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>