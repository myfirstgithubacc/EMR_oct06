<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="MRDIssueRegisterReports.aspx.cs" Inherits="MRD_Reports_MRDIssueRegisterReports" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-4 col-sm-4 col-xs-12">
                    <asp:Label ID="lblHeader" runat="server" Text="MRD Issue Register Reports" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-12 text-center">
                    <%--<asp:UpdatePanel ID="upErrorMessage" runat="server"><ContentTemplate>--%>
                    <asp:Label ID="lblMessage" runat="server" />
                    <%-- </ContentTemplate></asp:UpdatePanel>--%>
                </div>
            </div>

                <div class="row m-t">
                    <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                        <div class="col-md-12 bg-info">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap" id="lbltdfrmdate" runat="server">
                                         <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8" id="dttdfrmdate" runat="server">
                                           <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" Width="100px"
                                            DateInput-DateFormat="dd/MM/yyyy">
                                        </telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                          <asp:Label ID="Label3" runat="server" Text="To Date" SkinID="label"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                          <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" Width="100px"
                                            DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                         <asp:Label ID="lbllocation" runat="server" Text="Location" SkinID="label"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                           <telerik:RadComboBox ID="ddlEntrySite" Width="100%" runat="server" SkinID="DropDown">
                                         </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                          <asp:Label ID="Label1" runat="server" Text="Report Type" SkinID="label"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                          <telerik:RadComboBox ID="ddlReportType" Width="100%" runat="server" SkinID="DropDown">
                                         <Items>
                                             <telerik:RadComboBoxItem Text="MLC Register" Value="1" />
                                              <telerik:RadComboBoxItem Text="Medical Record Legal Issue Register" Value="2" />
                                              <telerik:RadComboBoxItem Text="Medical Record Issue Register" Value="3" />
                                              <telerik:RadComboBoxItem Text="Application Register" Value="4" />
                                         </Items>
                                        </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                               
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap box-col-checkbox">
                                         <asp:CheckBox ID="chkExport" Checked="true" runat="server" Text="Export" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox">
                                           <asp:CheckBox ID="chkMLC" Visible="false" runat="server" Text="MLC" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                          
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                          <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Print Report" OnClick="btnPrintreport_Click"
                                           CssClass="btn btn-primary" Text="Print Report" />
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

