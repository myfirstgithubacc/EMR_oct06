<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupDepartmentsTagging.aspx.cs"
    Inherits="MPages_GroupDepartmentsTagging" Title="Group Department(s) Tagging" %>

<%@ Import Namespace="System.Drawing" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager { background: #c1e5ef 0 -7000px repeat-x !important; color: #00156e !important;}
    </style>  


</head>
<body>
    <form id="form2" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main">
                <div class="col-xs-8"><asp:Label ID="Label1" runat="server" Text="Group Name : " />&nbsp;<asp:Label ID="lblGroupName" runat="server" Columns="50" Font-Bold="true" /></div>
                <div class="col-xs-4 text-right">
                    <asp:Button ID="cmdNew" runat="server" Text="New" ToolTip="New" CausesValidation="false" CssClass="btn btn-default" OnClick="cmdNew_Click" Visible="false" />
                    <asp:Button ID="cmdSave" runat="server" ToolTip="Save" CssClass="btn btn-primary" Text="Save" OnClick="cmdSave_Click" />
                    <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false" CssClass="btn btn-default" OnClientClick="window.close();" />
                </div>
            </div>

            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-xs-12 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-6 PaddingSpacing">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvFields" runat="server" PagerStyle-ShowPagerText="false" PageSize="10"
                            Skin="Office2007" Width="100%" AllowSorting="False" AllowMultiRowSelection="False"
                            EnableLinqExpressions="false" AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False"
                            GroupHeaderItemStyle-Font-Bold="true" GridLines="none" AllowFilteringByColumn="true"
                            ShowStatusBar="true" OnPreRender="gvFields_PreRender" OnSortCommand="gvFields_SortCommand"
                            OnPageIndexChanged="gvFields_PageIndexChanged" OnItemCommand="gvFields_ItemCommand"
                            OnItemDataBound="gvFields_ItemDataBound">
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <PagerStyle PagerTextFormat="None" PageButtonCount="5" />
                            <MasterTableView Width="100%">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="DepartmentName" HeaderText="Department Name"
                                        DataField="DepartmentName" SortExpression="DepartmentName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                        FilterControlWidth="99%">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                            <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Add to List" HeaderTooltip="Add to List"
                                        AllowFiltering="false" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAdd" runat="server" CommandName="AddToList" CommandArgument='<%#Eval("DepartmentId") %>'
                                                Text="Add to List" ToolTip="Add to List" CausesValidation="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-6 PaddingSpacing">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvSelectedFields" runat="server" Skin="Office2007" Width="100%"
                            PagerStyle-ShowPagerText="false" AllowSorting="False" AllowMultiRowSelection="False"
                            EnableLinqExpressions="false" ShowGroupPanel="false" AutoGenerateColumns="False"
                            GroupHeaderItemStyle-Font-Bold="true" GridLines="none" OnItemCommand="gvSelectedFields_ItemCommand"
                            OnItemDataBound="gvSelectedFields_ItemDataBound">
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView Width="100%">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Department Name">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                            <%--<asp:HiddenField ID="hdnSeqNo" runat="server" Value='<%#Eval("SequenceNo") %>' />--%>
                                            <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Delete" HeaderTooltip="Delete a row" HeaderStyle-Width="40px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" CommandName="Delete1"
                                                ImageUrl="/images/DeleteRow.png" ToolTip="Click here to delete a row" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <table width="100%" cellpadding="2" cellspacing="2">
        <colgroup>
            <col width="50%" valign="top" />
            <col width="50%" valign="top" />
        </colgroup>
        <tr>
            <td valign="top">
                
            </td>
            <td valign="top">
                
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
