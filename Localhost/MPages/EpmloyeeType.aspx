<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EpmloyeeType.aspx.cs"  Inherits="MPages_EmployeeType"
    Title="Employee Type Master" %>
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
        .RadGrid_Office2007 .rgPager {background: #c1e5ef 0 -7000px repeat-x !important;color: #00156e !important;}
    </style>
           
    <asp:UpdatePanel ID="updGeneric" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-3"><h2>Employee Type Master</h2></div>
                <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" Font-Bold="true" /></div>
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
                            <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblDescription" runat="server" Text="Description" /><span style="color: Red">*</span></div>
                            <div class="col-md-8 col-sm-8">
                                <asp:TextBox ID="txtDescription" SkinID="textbox" runat="server" Width="100%" MaxLength="50" />
                                <ajax:FilteredTextBoxExtender ID="ftbetxtTitleName" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" /-()%." TargetControlID="txtDescription"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"><span class="textName"><asp:Label ID="lblEpmloyeeType" runat="server" Text="Epmloyee Type" ></asp:Label></span></div>
                            <div class="col-md-8 col-sm-8"><asp:TextBox ID = "txtEpmloyeeType" runat="server" MaxLength="5" ></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-3 col-sm-4 label2"><asp:Label ID="Label6" runat="server" Text='<%$ Resources:PRegistration, status%>' /></div>
                            <div class="col-md-9 col-sm-8">
                                <telerik:RadComboBox ID="ddlStatus" runat="server" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Active" Value="1" />
                                        <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                   
                </div>
            </div>



            <table border="0" width="98%" cellpadding="3" cellspacing="0" style="margin-left: 4px;
                margin-right: 4px">

                <tr>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-left: 0px;">
                <tr>
                    <td valign="top">
                        <telerik:RadGrid ID="gvEpmloyeeType" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                            AllowFilteringByColumn="true" AllowMultiRowSelection="false" runat="server" Width="100%"
                            AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                            GridLines="Both" AllowPaging="True" Height="350px" PageSize="10" OnPageIndexChanged="gvUnit_OnPageIndexChanged"
                            OnItemCommand="gvUnit_OnItemCommand" OnPreRender="gvUnit_PreRender" OnItemDataBound="gvUnit_ItemDataBound">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.</div>
                                </NoRecordsTemplate>
                                <ItemStyle Wrap="true" />
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="TitleId" HeaderText="TitleId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTitleId" runat="server" Text='<%#Eval("Id")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Description" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="Description" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="Description" FilterControlWidth="99%"
                                        ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("Description")%>' />
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
                                        <telerik:GridTemplateColumn UniqueName="EmployeeType" AllowFiltering="false" HeaderText="Employee Type"
                                        HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGenderStatus" runat="server" Text='<%#Eval("EmployeeType")%>' Width="100px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                        
                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="10%"
                                        HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

