<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="StatusMaster.aspx.cs"  Inherits="MPages_StatusMaster"
    Title="Status Master" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager {background: #c1e5ef 0 -7000px repeat-x !important;color: #00156e;}
    </style>
           
    <asp:UpdatePanel ID="updGeneric" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main form-group">
                <div class="col-md-2 col-sm-3"><h2>Status Master</h2></div>
                <div class="col-md-7 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" Font-Bold="true" /></div>
                <div class="col-md-3 col-sm-3 text-right">
                	<asp:Button ID="btnNew" runat="server" ToolTip="New Record" CssClass="btn btn-default" Text="New" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false" CssClass="btn btn-default" OnClientClick="window.close();" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" OnClick="btnSaveData_OnClick" CssClass="btn btn-primary" Text="Save" />
                </div>
            </div>



            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblStatusType" runat="server" Text="Status Type" /><span style="color: Red">*</span></span></div>
                            <div class="col-md-8 col-sm-7"><telerik:RadComboBox ID="ddlStatusType" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="true" AllowCustomText="true" AutoPostBack="true" onselectedindexchanged="ddlStatusType_SelectedIndexChanged" /></div>
                        </div>
                    </div>

                    <%--<asp:TextBox ID="txtName" SkinID="textbox" runat="server" Width="200px" MaxLength="50" />
                        <ajax:FilteredTextBoxExtender ID="ftbetxtCompanyTypeName" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" /-()%." TargetControlID="txtName"></ajax:FilteredTextBoxExtender>--%>
                    <%--<asp:Label ID="lblCompanyTypeShort" runat ="server" Text = "Short Name" ></asp:Label>
                        <asp:TextBox ID = "txtCompanyTypeShort" SkinID="textbox"  runat ="server" Width="200px" MaxLength = "5" ></asp:TextBox>--%>

                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblStatusName" runat="server" Text="Status Name" /><span style="color: Red">*</span></span></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtStatusName" runat="server" MaxLength="20" Width="100%" />
                                <ajax:FilteredTextBoxExtender ID="ftbetxtStatusName" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" /-()%." TargetControlID="txtStatusName"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblStatusCode" runat="server" Text="Status Code"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtStatusCode" runat="server" MaxLength="10" Width="100%"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblStatusColor" runat="server" Text="Status Color" /></span></div>
                            <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtStatusColor" runat="server" MaxLength="30" Width="100%" /></div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblColorName" runat="server" Text="Color Name"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtColorName" runat="server" MaxLength="30" SkinID="textbox" Width="100%"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="lblSequence" runat="server" Text="Sequence No" /></span></div>
                            <div class="col-md-8 col-sm-7"><asp:TextBox ID="txtSequence" runat="server" MaxLength="10" Width="100%" /></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:PRegistration, status%>"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-7">
                                <telerik:RadComboBox ID="ddlStatus" runat="server" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Active" Value="1" />
                                        <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"></div>
                            <div class="col-md-8 col-sm-7"></div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <telerik:RadGrid ID="gvStatus" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                        AllowFilteringByColumn="true" AllowMultiRowSelection="false" runat="server" Width="100%"
                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                        GridLines="Both" AllowPaging="True" Height="350px" PageSize="10" OnPageIndexChanged="gvStatus_OnPageIndexChanged"
                        OnItemCommand="gvStatus_OnItemCommand" OnPreRender="gvStatus_PreRender" OnItemDataBound="gvStatus_ItemDataBound">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="StatusId" HeaderText="StatusId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusId" runat="server" Text='<%#Eval("StatusId")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="StatusType" ShowFilterIcon="false" DefaultInsertValue=""
                                    DataField="StatusType" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                    HeaderText="Status Type" FilterControlWidth="99%"
                                    ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusype" runat="server" Text='<%#Eval("StatusType")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn UniqueName="Status" AllowFiltering="false" HeaderText="Status"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn UniqueName="StatusCode" AllowFiltering="false" HeaderText="Status Code"
                                    HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("Code")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn UniqueName="StatusColor" AllowFiltering="false" HeaderText="Status Color"
                                    HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn UniqueName="ColorName" AllowFiltering="false" HeaderText="Color Name"
                                    HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblColorName" runat="server" Text='<%#Eval("ColorName")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridTemplateColumn UniqueName="SequenceNo" AllowFiltering="false" HeaderText="Sequence No"
                                    HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSequenceNo" runat="server" Text='<%#Eval("SequenceNo")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                        
                                <telerik:GridTemplateColumn UniqueName="ActiveStatus" AllowFiltering="false" HeaderText='<%$ Resources:PRegistration, status%>'
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActiveStatus" runat="server" Text='<%#Eval("ActiveStatus")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Active" Visible="false" HeaderText="Active"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" Text='<%#Eval("Active")%>' Width="100px" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="10%"
                                    HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>