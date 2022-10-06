<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvestigationLabServiceTag.aspx.cs"
    Inherits="EMR_Masters_InvestigationLabServiceTag" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Investigation Lab Tagging</title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr class="clsheader">
                    <td align="left" style="padding-left: 10px;">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Investigation Tagging"
                            Font-Bold="true" />
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save" OnClick="btnSaveData_OnClick"
                            SkinID="Button" Text="Save" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="SaveData" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center" style="font-size: 12px;">
                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="Service Name<span style='color: Red'>*</span>" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlServiceName" SkinID="DropDown" runat="server" Width="350px"
                            AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlServiceName_OnSelectedIndexChanged" />
                    </td>
                    <td></td>
                   </tr> <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Lab Template<span style='color: Red'>*</span>" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlLabTemplate" SkinID="DropDown" runat="server" Width="350px"
                            AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                    </td>
                    <td>
                    
                            
                            </td>
                </tr>
                <tr>
                    <td colspan="3">
                       <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>--%>
                                <telerik:RadGrid ID="gvSelectedFields" runat="server" Skin="Office2007" Width="100%"
                                    PagerStyle-ShowPagerText="false" AllowSorting="False" AllowMultiRowSelection="False"
                                    EnableLinqExpressions="false" ShowGroupPanel="false" AutoGenerateColumns="False"
                                    GroupHeaderItemStyle-Font-Bold="true" GridLines="none" OnItemCommand="gvSelectedFields_ItemCommand">
                                   
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="307px" />
                                    </ClientSettings>
                                    <MasterTableView Width="100%">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                         <telerik:GridTemplateColumn HeaderText="SNo" DataField="SNo" HeaderStyle-Width="5%"
                                                            UniqueName="SNo" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltrSno" runat="server" Text='<%#Container.ItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                                          
                                             </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="" Visible="false" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Service Name"   ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Field Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartNo" Text='<%#Eval("FieldName") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                               <telerik:GridTemplateColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lbtnDelete" CommandName="Delete" Text="Delete" runat="server"></asp:LinkButton>--%>
                                                     <asp:ImageButton ID="lbtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                    CommandName="Delete" ToolTip="Remove"
                                                    Width="16px" Visible="true" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                          <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
