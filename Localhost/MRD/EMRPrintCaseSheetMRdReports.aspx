<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EMRPrintCaseSheetMRdReports.aspx.cs" Inherits="MRD_EMRPrintCaseSheetMRdReports" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:HiddenField ID="hdnHeaderName" runat="server" />
    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-6 col-sm-6 col-xs-12" id="tdHeader" runat="server">
                <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" Text="Print MRD Template" />
                <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" />
            </div>
            <div class="col-md-6 col-sm-6 col-xs-12 text-right">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text=" Search Reports" OnClick="btnSearch_Click" />
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print Reports" OnClick="btnPrint_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lblTemplate2" runat="server" Text="Template" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlTemplateMain" DataTextField="TemplateName" DataValueField="TemplateId" runat="server" EmptyMessage="Select"
                                Width="100%" Height="400px" DropDownWidth="350px" Filter="Contains" OnSelectedIndexChanged="ddlTemplateMain_SelectedIndexChanged" AutoPostBack="true" />
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label3" runat="server" Text="Search On" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <div class="row">
                            <div class="col-md-5 col-sm-5 col-xs-5 no-p-r">
                                <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                <asp:ListItem Text="<%$ Resources:PRegistration, EncounterNo%>" Value="1" />
                                <asp:ListItem Text="<%$ Resources:PRegistration, UHID%>" Value="2" />


                            </asp:DropDownList>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-7">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                <asp:TextBox ID="txtSearch" SkinID="textbox" runat="server" Width="100%" MaxLength="20" />
                                <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="100%" MaxLength="20" Visible="false" />
                                <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                            </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-4 col-sm-4 col-xs-12">
                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-6">
                        <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" Width="100%"
                    DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
                    </div>
                </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6">
                        <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label1" runat="server" Text="To Date" SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" Width="100%"
                    DateInput-DateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
                    </div>
                </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lblFildName" runat="server" Text="Column Name" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlTemplateColumn" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select All"
                                ShowMoreResultsBox="false" AppendDataBoundItems="true" Width="100%" Height="400px" DropDownWidth="350px" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row m-t">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                <telerik:RadGrid ID="gvBindMRDTemplate" runat="server" AllowFilteringByColumn="true" AllowMultiRowSelection="true"
                AllowSorting="true"  BorderWidth="0" AlternatingItemStyle-Font-Size="9pt"
                EnableLinqExpressions="false" GridLines="Both" ItemStyle-Font-Size="9pt"
                AllowPaging="true"  HeaderStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" PagerStyle-ShowPagerText="false"
                ShowStatusBar="true" CssClass="table table-condensed" Visible="true" Width="100%"
                CellPadding="0"  AutoPostBackOnFilter="true"  OnPreRender="gvBindMRDTemplate_PreRender">
                <GroupingSettings CaseSensitive="false" />
               
                <MasterTableView TableLayout="Fixed" CssClass="MasterTableText" Width="100%">
                    <NoRecordsTemplate>
                        <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">
                            No Record Found.
                        </div>
                    </NoRecordsTemplate>

                </MasterTableView>
            </telerik:RadGrid>
            </div>
        </div>

    </div>
</asp:Content>

