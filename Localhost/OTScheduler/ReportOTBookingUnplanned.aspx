<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master" CodeFile="ReportOTBookingUnplanned.aspx.cs" Inherits="OTScheduler_ReportOTBookingUnplanned" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel ID ="updatePanel1" runat="server">
    <ContentTemplate>
        <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../Include/css/open-sans.css" rel="stylesheet" />
        <link href="../Include/css/mainNew.css" rel="stylesheet" />  

        <div class="container-fluid header_main form-group">
            <div class="col-md-3"><h2><asp:Label ID="lblHeader" runat="server" Text="Report OT Booking Details" /></h2></div>
            <div class="col-md-9 text-center"><asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></div>
        </div>

        <div class="container-fluid">
            <div class="row">
                <div class="col-md-offset-4 col-md-4 header_main main_box">

                    <div class="row form-groupTop01">
                        <div class="col-md-12">
                            <div class="col-md-3 label2"><asp:Label ID="Label4" runat="server" Text="Report Type"></asp:Label></div>
                            <div class="col-md-9">
                                <asp:DropDownList id="ddlReportType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" CssClass="drapDrowHeight" Width="100%">
                                    <asp:ListItem Text="Return To OT (Within 7 Days)" Value="0" />  
                                    <asp:ListItem Text="Return To OT (Within 48 Hr.)" Value="1" Selected="True"/>                                                     
                                    <asp:ListItem Text="OT Abort" Value="2" />
                                    <asp:ListItem Text="Door to Balloon" Value="3" />
                                    <%-- <asp:ListItem Text="Register Surgery" Value="4" />--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row form-groupTop01">
                        <div class="col-md-12">
                            <div class="col-md-3 label2"><asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label></div>
                            <div class="col-md-3" id="dttdfrmdate" runat="server"><telerik:RadDatePicker ID="dtpfromdate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                            <div class="col-md-2 label2"><asp:Label ID="Label3" runat="server" Text="To Date"></asp:Label></div>
                            <div class="col-md-4"><telerik:RadDatePicker ID="dtpTodate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                        </div>
                    </div>

                    <div class="row form-groupTop01">
                        <div class="col-md-12 text-center main_box02"><asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Preview Report" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnPrintreport_Click" Text="Preview" /></div>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>