<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Doctorwiseprocedure.aspx.cs" Inherits="EMRReports_Doctorwiseprocedure"
    Title="Doctor wise procedure" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/Reports.css" rel="stylesheet" type="text/css" />
    
    
    


    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            
            <div class="ReportsDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-6" id="tdHeader" runat="server">
                            <div class="ReportsDivText"><h2><asp:Label ID="lblHeader" runat="server" Text="" /></h2></div>
                        </div>
                        <div class="col-md-4"><div class="ReportsMessage01"><asp:Label ID="lblMessage" runat="server"></asp:Label></div></div>
                        <div class="col-md-2">
                            <div class="ReportsAP-Div01">
                                <asp:Button ID="btnPrintPreview" runat="server" ToolTip="Click to Print Preview" OnClick="btnPrintData_OnClick" CssClass="PrintPreviewBtn" Text="Print Preview" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            
           
            <div class="ReportsAPDiv">
                <div class="container-fluid">
                
                    <div class="row">
                        <div class="col-12">
                            <div class="ReportsAP-Div">
                                <h2><asp:Label ID="lblEntrySite" Visible="false" runat="server" Text="Entry Site" CssClass="caption_Gray" /></h2>
                                <h3><telerik:RadComboBox ID="ddlEntrySite" Visible="false" runat="server" Width="180px" Skin="Metro" /></h3>
                            </div>
                        </div>
                    </div>
                    
                    
                    <div class="row">
                        <div class="col-md-4">
                            <div class="ReportsAP-Div">
                                <h2><asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label></h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlReportType" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Appointment List" Value="AL" Selected="True" />
                                            <telerik:RadComboBoxItem Text="Cancelled Appointment" Value="CA" Selected="True" />
                                            <telerik:RadComboBoxItem Text="Break And Block" Value="BAB" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </h3>
                            </div>
                        </div>
                        
                        <div class="col-md-7">
                            <div class="ReportsAP-Div02">
                                <h2><asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label></h2>
                                <h3><telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker></h3>
                                <h2><asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label></h2>
                                <h3><telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker></h3>
                            </div>
                        </div>
                        
                        
                    </div>
                    
                    <div class="row">
                        <div class="col-md-4">
                            <div class="ReportsAP-Div">
                                <h2>&nbsp;</h2>
                                <h4><asp:CheckBox ID="cbIsDetail" runat="server" Text="Detail" /></h4>
                                <h4><asp:CheckBox ID="cbIsBilled" runat="server" Text="Only Billed" /></h4>
                                <h4><asp:CheckBox ID="chkExport" runat="server" Text="Export" /></h4>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="ReportsAP-Div">&nbsp;</div>
                        </div>
                        <div class="col-md-4">
                            <div class="ReportsAP-Div">&nbsp;</div>
                        </div>
                    </div>
                    
                    
                    
                    
                    <div class="row">
                        <div class="ReportsBorder">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlDep" Height="400px" Width="100%" ScrollBars="None">
                                            <span class="ReportsDoctor"><asp:Label ID="Label3" runat="server" CssClass="" Text="Doctors"></asp:Label></span>&nbsp;
                                            
                                            <telerik:RadGrid ID="gvReporttype" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                                AllowFilteringByColumn="true" runat="server" Width="99%" AutoGenerateColumns="False"
                                                PageSize="12" EnableLinqExpressions="False" AllowPaging="false" CellSpacing="0"
                                                OnPreRender="gvReporttype_PreRender">
                                                
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="false" />
                                                </ClientSettings>
                                                <PagerStyle ShowPagerText="False" />
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate><div style="font-weight: bold; color: Red;">No Record Found.</div></NoRecordsTemplate>
                                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True"></RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True"></ExpandCollapseColumn>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="StoreId" AllowFiltering="false" ShowFilterIcon="false" AutoPostBackOnFilter="false" HeaderText="StoreId" Visible="false">
                                                            <ItemTemplate><asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' /></ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name" HeaderText=" Name" FilterControlWidth="99%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDepartment" runat="server" />
                                                                <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("Name")%>' />
                                                            </ItemTemplate>
                                                            <HeaderTemplate><asp:CheckBox ID="chkAllDepartment" OnCheckedChanged="chkAllDepartment_CheckedChanged" Font-Bold="true" runat="server" Text="All Select / Unselect " AutoPostBack="true" /></HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                                
                                                <FilterMenu EnableImageSprites="False"></FilterMenu>
                                            </telerik:RadGrid>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        
                        </div>
                        
                        
                        
                        <div class="col-md-12">
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow></Windows>
                            </telerik:RadWindowManager>
                        </div>
                    </div>
                    
                
                </div>
            </div>    
           
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
