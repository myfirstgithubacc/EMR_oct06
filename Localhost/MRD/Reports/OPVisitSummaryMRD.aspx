<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="OPVisitSummaryMRD.aspx.cs" Inherits="MRD_Reports_OPVisitSummaryMRD" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

            <div class="container-fluid">
                <div class="row header_main">
                    <div class="colmd-12 col-sm-12 col-xs-12" id="tdHeader" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" Text="OP Visit Summary" />
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                        <div class="col-md-12 bg-info">
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap" id="lbltdfrmdate" runat="server">
                                         <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label>   
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8" id="dttdfrmdate" runat="server">
                                            <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" Width="100px"
                                        DateInput-DateFormat="dd/MM/yyyy"> </telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5 col-sm-5 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                           <asp:Label ID="Label3" runat="server" Text="To Date" SkinID="label"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                           <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" Width="100px"
                                        DateInput-DateFormat="dd/MM/yyyy"> </telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2 col-sm-2 col-xs-12 p-t-b-5">
                                    <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Print Report" OnClick="btnPrintreport_OnClick"
                                       CssClass="btn btn-primary" Text="Print Report" />
                                    <asp:Button ID="btnExportToExcel" runat="server" ToolTip="Export To Excel" OnClick="btnExportToExcel_OnClick"
                                        CssClass="btn btn-primary" Text="Export To Excel" Visible="false" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                          <asp:Label ID="lblUnderPackage" runat="server" SkinID="label" Text="Under Package"  />   
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadComboBox ID="ddlUnderPackage" Width="100%" runat="server"  >
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Both" Value="2" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Package Included" Value="1" />
                                            <telerik:RadComboBoxItem Text="Package Excluded" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-6 col-sm-6 col-xs-6 box-col-checkbox">
                                           <asp:CheckBox ID="chkExport" runat="server" Text="Export" />
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-6">
                                           
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
                        </div>
                    </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
