<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPtientdetails.aspx.cs"
    Inherits="OTScheduler_ViewPtientdetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
    <div id="dvZone1" style="width: 100%">
        <table cellpadding="0" cellspacing="2" width="100%">
        <tr>
        <td align="right">  
        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.close();" />&nbsp;
        </td>
        </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gvDetails" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        Height="210" ShowFooter="false" GridLines="Both" AllowPaging="true" PageSize="10">
                        <HeaderStyle HorizontalAlign="Center" />
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                            Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                        </ClientSettings>
                        <MasterTableView  Width="100%" TableLayout="Fixed">
                            <Columns>
                             <telerik:GridTemplateColumn HeaderText="Resource Type" UniqueName="Resource" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="txtResource" runat="server" Text='<%#Eval("Resource")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" UniqueName="Name"  HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="txtName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               
                             
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
