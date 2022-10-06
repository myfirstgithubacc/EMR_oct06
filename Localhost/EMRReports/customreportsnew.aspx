<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="customreportsnew.aspx.cs" Inherits="EMRReports_customreportsnew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/bootstrap.css" rel="Stylesheet" />
    <link href="../Include/css/emr_new.css" rel="Stylesheet" />
    <asp:UpdatePanel ID="uPnl" runat="server">
        <ContentTemplate>



            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2><span id="tdHeader" align="left" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Custom Reports" Font-Bold="true" /></span></h2>
                </div>

                <div class="col-md-5 text-center">
                    <asp:Label ID="lblMessage" Font-Bold="true" runat="server"></asp:Label></div>

              <%--  <div class="col-md-3 text-right pull-right">
                    <asp:Button ID="btnPrint" runat="server" Visible="false"  CssClass="btn btn-primary" Text="Print Report" OnClick="btnPrint_OnClick" />
                    <asp:CheckBox ID="chkExport" Visible ="false"  runat="server" Text="Export" />
                </div>--%>
            </div>






            <div class="container-fluid subheading_main" runat="server" id="tblCustom">
                


                <div class="form-group">


                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblFilterType" runat="server" Text="Filter Type" Visible ="false" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlFiltertype" Visible="true" runat="server" SkinID="DropDown"
                                    Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltertype_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Date Wise" Value="D" />
                                        <telerik:RadComboBoxItem Text="Monthly" Value="M" />
                                        <telerik:RadComboBoxItem Text="Quarterly" Value="Q" />
                                        <telerik:RadComboBoxItem Text="Yearly" Value="Y" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>


                </div>

                <div class="row form-group" id="trDateWise" runat="server">


                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblDtpFromDate" runat="server" Text="From Date" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"
                                    Width="100%">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblDtpToDate" runat="server" Text="To Date" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"
                                    Width="100%">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>


                </div>

                <div class="row form-group" id="trMonthWise" runat="server">
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblYearMonth" runat="server" Text="Select Year" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlYearMonth" runat="server" EmptyMessage="Select Year" AppendDataBoundItems="true"
                                    Width="100px">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblMonthMonth" runat="server" Text="Select Month" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlMonthMonth" runat="server" EmptyMessage="Select Month" AppendDataBoundItems="true"
                                    Width="120px">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>


                </div>

                <div class="row form-group" id="trQuatrlyWise" runat="server">
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblYearQuatrly" runat="server" Text="Select Year" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlYearQuatrly" runat="server" EmptyMessage="Select Year" AppendDataBoundItems="true"
                                    Width="100px">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblMonthQuatrly" runat="server" Text="Select Quarterly" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlMonthQuatrly" runat="server" EmptyMessage="Select Quarter" AppendDataBoundItems="true"
                                    Width="120px">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row form-group" id="trYearWise" runat="server">
                    <div class="col-md-6">
                        <div class="">
                            <div class="col-md-2">
                                <asp:Label ID="lblYearYear" runat="server" Text="Select Year" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlYearYear" runat="server" AutoPostBack="true"  EmptyMessage="Select Year" AppendDataBoundItems="true"
                                    Width="100px" OnSelectedIndexChanged="ddlYearYear_SelectedIndexChanged">
                                </telerik:RadComboBox>
                               <%-- <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Button" />--%>

                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4"></div>
                            <div class="col-md-8"></div>
                        </div>
                    </div>


                </div>

            </div>


            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6" style="max-height:400px;">
                         <rsweb:ReportViewer ID="ReportViewerOPVisit" runat="server" Height="350px" Width="100%"  ShowToolBar="false"  >
                </rsweb:ReportViewer>
                    </div>
                    <div class="col-md-6" style="max-height:400px;">
                        <rsweb:ReportViewer ID="ReportViewerAdmission" runat="server" Height="350px" Width="100%" ShowToolBar="false"     >
                </rsweb:ReportViewer>
                    </div>
                     <div class="col-md-6" style="max-height:400px; ">
                        <rsweb:ReportViewer ID="ReportViewerRevenueComparision" runat="server" Height="350px" Width="100%"  ShowToolBar="false"  >
                </rsweb:ReportViewer>
                    </div>
                    <div class="col-md-6" style="max-height:400px; ">
                         <rsweb:ReportViewer ID="ReportViewerOrder" runat="server" Height="350px" Width="100%" ShowToolBar="false"   >
                </rsweb:ReportViewer>
                    </div>
                </div>
            </div>

           


            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

