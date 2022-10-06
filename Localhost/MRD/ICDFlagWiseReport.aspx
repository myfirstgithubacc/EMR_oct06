<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ICDFlagWiseReport.aspx.cs" Inherits="MRD_ICDFlagWiseReport" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-xs-12 text-nowrap" id="tdHeader" runat="server">
                         <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" Text="ICD Flag Wise Report" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-12 text-right">
                        <asp:CheckBox ID="chkExport" runat="server" Text="Export" />&nbsp; 
                        <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Print Report" 
                                 CssClass="btn btn-primary" Text="Print Report" onclick="btnPrintreport_Click" />
                         
                    </div>
                </div>
                <div class="row m-t">
                    <div class="col-md-4 col-sm-4 col-xs-12 col-md-offset-4">
                        <div class="col-md-12 bg-info">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap" id="lbltdfrmdate" runat="server">
                                            <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8" id="dttdfrmdate" runat="server">
                                            <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" Width="100%"
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
                                            <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" Width="100%"
                                        DateInput-DateFormat="dd/MM/yyyy">
                                    </telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="lblUnderPackage" runat="server" SkinID="label" Text="ICD Flag(s)"  />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadComboBox Width="100%" ID="ddlICDFlag" runat="server" 
                                    DropDownWidth="400px" EmptyMessage="[ICD Flags]" >
                                       
                                    </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="lblSource" runat="server" SkinID="label" Text="Source"/>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadComboBox ID="ddlOPIP" runat="server" Width="100%" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="OP" Value="O" />
                                            <telerik:RadComboBoxItem Text="IP" Value="I" />
                                            <telerik:RadComboBoxItem Text="Both" Value="B" Selected="true" />
                                        </Items>
                                    </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:Panel runat="server" ID="pnlGroup" Visible="false">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Group" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadComboBox ID="ddlGroup" runat="server" Width="100%"
                                                        EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" 
                                                        onselectedindexchanged="ddlGroup_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="lblSubGroup" runat="server" SkinID="label" Text="Sub Group" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadComboBox ID="ddlSubGroup" runat="server" Width="100%" AutoPostBack="true"
                                    EmptyMessage="[ Select ]" MarkFirstMatch="true" CheckBoxes="true"  EnableCheckAllItemsCheckBox="true" />
                                        </div>
                                    </div>
                                </div>
                                     </asp:Panel>
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

