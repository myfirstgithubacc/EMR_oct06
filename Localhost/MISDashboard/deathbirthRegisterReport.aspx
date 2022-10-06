<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="deathbirthRegisterReport.aspx.cs" Inherits="MISDashboard_deathbirthReport"
    Title="" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-4">&nbsp;&nbsp;</div>
        </div>
        <div class="row" id="trdate" runat="server">
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap" id="lbltdfrmdate" runat="server">
                        <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8" id="dttdfrmdate" runat="server">
                        <telerik:RadDateTimePicker ID="dtpfromdate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy HH:mm">
                </telerik:RadDateTimePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label3" runat="server" Text="To Date" SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadDateTimePicker ID="dtpTodate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy HH:mm">
                </telerik:RadDateTimePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="lblSource" runat="server" Text=" Source " SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       <telerik:RadComboBox ID="ddlSource" runat="server" Width="100%">
                    <Items>
                        <telerik:RadComboBoxItem Text="Both" Value="" Selected="true" />
                        <telerik:RadComboBoxItem Text="OPD" Value="OPD" />
                        <telerik:RadComboBoxItem Text="IPD" Value="IPD" />
                    </Items>
                </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="lblReportType" runat="server" Text=" ReportType " SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox">
                        <asp:RadioButtonList ID="rdoICDCPT" runat="server" Width="100%" RepeatDirection="Horizontal">
                    <asp:ListItem Text="ICD" Value="ICD" Selected="True" />
                    <asp:ListItem Text="CPT" Value="CPT" />
                </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12 p-t-b-5">
                <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Print Report" CausesValidation="false"
                    OnClick="btnPrintreport_OnClick" CssClass="btn btn-primary" Text="Print Report" />
            </div>
        </div>
    </div>
    
    <table cellpadding="2" cellspacing="2" width="100%">
        <tr>
            <td style="width: 100%;">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="true" ShowPrintButton="true"
                    ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowFindControls="False"
                    ShowParameterPrompts="False" ShowPromptAreaButton="False" ShowZoomControl="False"
                    Width="98%">
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
</asp:Content>
