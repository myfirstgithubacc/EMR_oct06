<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="EmployeeSequenceOrder.aspx.cs" Inherits="MPages_EmployeeSequenceOrder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />


    <style>
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {border: solid #8b8b8b 1px; color:#333; border-top:none !important; background: 0 -2300px repeat-x #c1e5ef;}
    </style>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-4"><h2><asp:Label ID="lblHeader" runat="server" Text="Set Employee Sequence" /></h2></div>
                <div class="col-md-6 col-sm-5"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" ForeColor="Green" Font-Bold="true" /></div>
                
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:LinkButton ID="lnkEmployee" runat="server" CausesValidation="false" Text="Employee"
                        CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployee_OnClick" />
                    <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" CssClass="btn btn-primary"
                        Text="New" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnSave" runat="server" ToolTip="Save&nbsp;Record" CssClass="btn btn-primary"
                        Text="Save" OnClick="btnSave_OnClick" />
                </div>
            </div>



            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-6 col-sm-9">
                        
                        <div class="row form-group">
                            <div class="col-md-3 col-sm-4 PaddingRightSpacing"><asp:Label ID="Label3" runat="server" Text="Department Case Type" /></div>
                            <div class="col-md-9 col-sm-8">
                                <div class="PD-TabRadioNew01 margin_z">
                                    <asp:RadioButtonList ID="rdoDepartmentCaseType" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rdoDepartmentCaseType_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Single Department Case" Value="S" Selected="True" />
                                        <asp:ListItem Text="Multi Department Case" Value="M" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        
                        <span id="tblSingleDepartmentCase" runat="server">
                            <div class="row form-group">
                                <div class="col-md-3 col-sm-4"><asp:Label ID="Label2" runat="server" Text="Employee Type" /></div>
                                <div class="col-md-9 col-sm-8">
                                    <telerik:RadComboBox ID="ddlEmployeeType" EnableTextSelection="true"
                                        MarkFirstMatch="true" runat="server" Width="100%" Height="300px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlMainDept_OnSelectedIndexChanged" />
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-3 col-sm-4"><asp:Label ID="Label1" runat="server" Text="Department" /></div>
                                <div class="col-md-9 col-sm-8">
                                    <telerik:RadComboBox ID="ddlMainDept" EnableTextSelection="true"
                                        MarkFirstMatch="true" runat="server" Width="100%" Height="300px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlMainDept_OnSelectedIndexChanged" />
                                    <asp:Button ID="btnFileter" runat="server" Visible="false" Text="Filter" OnClick="btnFilter_OnClick" />
                                </div>
                            </div>
                        </span>

                        <div class="row form-group" id="tblMultiDepartmentCase" runat="server" visible="false">
                            <div class="col-md-3 col-sm-4"><asp:Label ID="Label4" runat="server" Text="Department Case" /><span style="color: Red">*</span></div>
                            <div class="col-md-9 col-sm-8">
                                <telerik:RadComboBox ID="ddlDepartmentCase" Width="100%" runat="server"
                                    Skin="Office2007" EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlDepartmentCase_OnSelectedIndexChanged" />
                            </div>
                        </div>

                    </div>

                    <div class="col-md-6 col-sm-3"></div>
                   
                </div>
            </div>



            <div class="container-fluid">
                <div class="row form-group">
                    <asp:UpdatePanel ID="updSelected" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvEmployeeSequcence" Skin="Office2007" BorderWidth="1" AllowFilteringByColumn="false"
                                PagerStyle-ShowPagerText="false" ShowHeader="true" PagerStyle-Visible="true" 
                                AllowPaging="false" PageSize="12" runat="server" AutoGenerateColumns="False"
                                ShowStatusBar="true" EnableLinqExpressions="false" Width="100%" OnItemCommand="gvSelectedValues_ItemCommand"
                                OnItemDataBound="gvEmployeeSequcence_OnItemDataBound" GroupingSettings-CaseSensitive="false">
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                </ClientSettings>
                                <MasterTableView AllowFilteringByColumn="false" TableLayout="Auto">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="SNo." DataField="SNo" HeaderStyle-Width="40px"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%--  <asp:Literal ID="ltrSno" runat="server" Text='<%#Container.ItemIndex + 1 %>'></asp:Literal>--%>
                                                <asp:Literal ID="ltrSno" runat="server" Text='<%# Eval("SequenceNo") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EmployeeName" DefaultInsertValue="" HeaderText="Employee Name"
                                            DataField="EmployeeName" SortExpression="EmployeeName" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                            FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:HiddenField ID="hdnSequenceNo" runat="server" Value='<%# Eval("SequenceNo") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ColumnSequenceNo" DefaultInsertValue="" HeaderText="Column No."
                                            HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtColumnSequenceNo" runat="server" MaxLength="1" Width="20px" Style="text-align: center;"
                                                    Text='<%#Eval("ColumnSequenceNo") %>' />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    FilterType="Custom" TargetControlID="txtColumnSequenceNo" ValidChars="123" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="StatusDown" AllowFiltering="false" HeaderText="Down"
                                            HeaderTooltip="Up" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDown" runat="server" Width="15Px" ImageUrl="/images/group_arrow_top.png"
                                                    CommandName="Down" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="StatusUp" AllowFiltering="false" HeaderText="Up"
                                            HeaderTooltip="Up" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgUp" runat="server" Width="15Px" ImageUrl="/images/group_arrow_bottom.png"
                                                    CommandName="Up" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>