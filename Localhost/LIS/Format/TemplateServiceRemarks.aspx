<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="TemplateServiceRemarks.aspx.cs" Inherits="LIS_Format_TemplateServiceRemarks" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; border-left:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px !important;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager { background: #c1e5ef 0 -7000px repeat-x !important; color: #00156e !important;}

        .PD-TabRadioNew01 input { margin:4px 4px 0 0px !important;}
        .PD-TabRadioNew01 label { font-weight:bold !important;margin:1px 4px 0 0px !important;}
    </style> 
    <script src="../../Include/JS/FontSizeEditor.js" type="text/javascript"></script>
    <script type="text/javascript">
        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "SteelBlue";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "SteelBlue";
        }
    </script>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="container-fluid header_main form-group">
                    <div class="col-md-3 PaddingRightSpacing" id="tdHeader" runat="server"><h2><asp:Label ID="lblHeader" runat="server" Text="Template&nbsp;Service&nbsp;Remarks" /></h2></div>
                    <div class="col-md-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                    <div class="col-md-3 text-right">
                        <asp:Button ID="cmdNew" runat="server" Text="New" ToolTip="New" CausesValidation="false" CssClass="btn btn-default" OnClick="cmdNew_Click" />
                        <asp:Button ID="cmdSave" runat="server" ToolTip="Save" CssClass="btn btn-primary" Text="" OnClick="cmdSave_Click" />
                    </div>
                </div>




                <asp:MultiView ID="mltVw" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vWNew" runat="server">


                        <div class="container-fluid">
                            <div class="row form-group">
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="row">
                                                <div class="col-md-4 label2"><asp:Label ID="Label1" runat="server" Text="Format&nbsp;Name" />&nbsp;<span style="color: Red">*</span></div>
                                                <div class="col-md-8"><asp:TextBox ID="txtFormatName" runat="server" CssClass="drapDrowHeight" Width="100%" MaxLength="50" /></div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-4 label2"><asp:Label ID="Label2" runat="server" Text="Status" /></div>
                                                <div class="col-md-8">
                                                    <telerik:RadComboBox ID="ddlStatus" runat="server" CssClass="drapDrowHeight" Width="100%">
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

                                <div class="col-md-4">
                                    <asp:LinkButton ID="LinkButton1" runat="server" SkinID="label" Font-Bold="true" onmouseout="LinkBtnMouseOut(this.id);"
                                        onmouseover="LinkBtnMouseOver(this.id);" Text="Service Tagging" OnClick="lnkRemarksService_Click" />
                                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-8 PaddingSpacing"><telerik:RadEditor ID="editorFormat" runat="server" ToolbarMode="Default" Width="100%" Skin="Office2007" Height="394px" OnClientSelectionChange="FontSizeRadEditor" EditModes="Design" ToolsFile="~/Include/XML/BasicTools.xml" BorderWidth="1" /></div>
                                <div class="col-md-4 PaddingRightSpacing">
                                    <telerik:RadGrid ID="gvTextFormat" runat="server" PagerStyle-ShowPagerText="false"
                                        PageSize="14" Skin="Office2007" Width="100%" AllowSorting="False" AllowMultiRowSelection="False"
                                        AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                        OnPreRender="gvTextFormat_PreRender" GridLines="none" OnPageIndexChanged="gvTextFormat_PageIndexChanged"
                                        OnItemCommand="gvTextFormat_ItemCommand" AllowFilteringByColumn="true">
                                        <PagerStyle Mode="NumericPages" />
                                        <ClientSettings ReorderColumnsOnClient="true" AllowDragToGroup="false" AllowColumnsReorder="false">
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="true" ResizeGridOnColumnResize="true" />
                                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                        </ClientSettings>
                                            <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView Width="100%">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn 
                                                UniqueName="FormatName" DefaultInsertValue="" HeaderText="Format&nbsp;Name" 
                                            AllowFiltering="true" DataField="RemarksName" SortExpression="RemarksName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                                    FilterControlWidth="99%">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnRemarksId" runat="server" Value='<%#Eval("RemarksId") %>' />
                                                        <asp:Label ID="lblRemarksName" runat="server" Text='<%#Eval("RemarksName") %>' />
                                                        <asp:HiddenField ID="hdnRemarksFormat" runat="server" Value='<%#Eval("RemarksFormat") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Active" ShowFilterIcon="false" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("Active") %>' />
                                                        <asp:Label ID="lblActive" runat="server" Text='<%# IsActive(Eval("Active").ToString()) %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridButtonColumn Text="Select" CommandName="RowSelect" HeaderText="Select"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="true" ShowSummary="false" />
</asp:Content>
