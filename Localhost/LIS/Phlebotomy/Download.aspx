<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Download.aspx.cs" Inherits="LIS_Phlebotomy_Download" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>External Report Download</title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <table border="0" cellpadding="1" cellspacing="1" width="100%">
            <tr height="25px" align="right">
                <td class="clssubtopic">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                
                            </td>
                            <td align="right">
                                <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                    OnClientClick="window.close();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <telerik:RadGrid ID="gvDownload" runat="server" Width="100%" Skin="Office2007" EnableEmbeddedSkins="true"
        BorderWidth="0" AllowFilteringByColumn="false" ShowGroupPanel="false" AllowPaging="true"
        Height="99%" AllowMultiRowSelection="true" AutoGenerateColumns="false" ShowStatusBar="true"
        EnableLinqExpressions="false" GridLines="None" PageSize="15" OnItemCommand="gvDownload_OnItemCommand">
        <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
            <NoRecordsTemplate>
                <div style="font-weight: bold; color: Red;">
                    No Record Found.</div>
            </NoRecordsTemplate>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="File Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                    ShowFilterIcon="false" FilterControlWidth="99%" HeaderStyle-Width="10%" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="DocumentName" Visible="false" AutoPostBackOnFilter="true"
                    CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlWidth="99%"
                    HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DocumentName") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                    DataField="Download" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                    ShowFilterIcon="false" FilterControlWidth="99%" HeaderStyle-Width="10%" Visible="true">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                            CommandArgument="None" Visible="true" CausesValidation="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <div>
    </div>
    </form>
</body>
</html>
