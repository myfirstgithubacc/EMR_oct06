<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="SpecialRightMaster.aspx.cs" Inherits="MPages_SpecialRightMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    
    <style>
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {border: solid #ababab 1px; border-top:none; background: 0 -2300px repeat-x #c1e5ef; outline:none;}
        .RadGrid .rgFilterBox {height: 23px !important;}
        .RadGrid_Office2007 .rgFilterRow {background: #effcff;}
        .RadGrid_Office2007 .rgPager {background: #c1e5ef 0 -7000px repeat-x;color: #00156e;}

    </style>
    
    
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-4"><h2>Special Rights Master</h2></div>
                <div class="col-md-7 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" ForeColor="Green" Font-Bold="true" /></div>
                <div class="col-md-2 col-sm-2 text-right">
                	<asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" ToolTip="Save" OnClick="btnSave_OnClick" />
                </div>
            </div>


            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-3 col-sm-3">
                        <div class="row form-group">
                            <div class="col-md-2 col-sm-2">Flag</div>
                            <div class="col-md-10 col-sm-10"><asp:TextBox ID="txtFlag" Width="100%" runat="server" MaxLength="50"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-5 col-sm-5">
                        <div class="row form-group">
                            <div class="col-md-2 col-sm-3">Description</div>
                            <div class="col-md-10  col-sm-9"><asp:TextBox ID="txtDescription" runat="server" Width="100%" TextMode="MultiLine" MaxLength="200"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="PD-TabRadioNew01 margin_z">
                            <asp:CheckBox ID="chkActive" runat="server" Text="Active" Checked="true" />
                        </div>
                    </div>
                </div>
            </div>



            <div class="container-fluid">
                <div class="row" id="DivMenu" runat="server">
                    <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                        AllowFilteringByColumn="true" AllowMultiRowSelection="false" runat="server" Width="100%"
                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                        AllowSorting="true" GridLines="Both" AllowPaging="True" PageSize="15" OnPageIndexChanged="gvData_OnPageIndexChanged"
                        OnPreRender="gvData_PreRender" OnSelectedIndexChanged="gvData_OnSelectedIndexChanged">
                        <GroupingSettings CaseSensitive="false" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <PagerStyle ShowPagerText="False" />
                        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            <ItemStyle Wrap="true" />
                            <%-- <CommandItemSettings ExportToPdfText="Export to PDF" />--%>
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="ID" ShowFilterIcon="false" Visible="false"
                                    DefaultInsertValue="" DataField="ID" HeaderText="ID" ItemStyle-Wrap="true" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>' />
                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("ID")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Flag" ShowFilterIcon="false" DefaultInsertValue=""
                                    DataField="Flag" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                    SortExpression="Flag" HeaderText="Flag" HeaderStyle-Width="30%" ItemStyle-Wrap="true"
                                    HeaderStyle-Wrap="true" FilterControlWidth="100%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlag" runat="server" Text='<%#Eval("Flag")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Description" ShowFilterIcon="false" DefaultInsertValue=""
                                    DataField="Description" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                    SortExpression="Description" HeaderText="Description"  
                                    FilterControlWidth="100%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                    SortExpression="ActiveGV" AllowFiltering="false" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Active")%>' />
                                        <asp:Label ID="labActiv" runat="server" Text='<%#Eval("ActiveGV")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn Text="Edit" CommandName="Select" FooterStyle-ForeColor="Blue"
                                    FooterStyle-Font-Bold="true" HeaderStyle-Width="4%" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:HiddenField ID="hdnFlagId" runat="server" Value="0" />
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
