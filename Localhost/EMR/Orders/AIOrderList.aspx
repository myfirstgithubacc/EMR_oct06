<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AIOrderList.aspx.cs" Inherits="EMR_Orders_AIOrderList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page         
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrpit1" runat="server">
        </asp:ScriptManager>
        <div>
            <h1>Order List (Matching with Primary & Secondary Diagnosis)</h1>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="vertical-align: right; text-align: right;">
                    <asp:Button ID="btnAdtoList" runat="server" Text="Add to List" OnClick="btnAdtoList_Click" />&nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Close this window" OnClick="btnClose_Click" />
                </div>
                <div>
                    <telerik:RadGrid ID="gvDrug" AllowMultiRowSelection="true" Skin="Office2007" Width="100%" BorderWidth="0" ShowGroupPanel="false" ShowFooter="false"
                        AllowFilteringByColumn="false" runat="server" AutoGenerateColumns="false" EnableRowHoverStyle="true"
                        ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" AllowPaging="false" PageSize="20" AllowCustomPaging="false">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                        </ClientSettings>
                        <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" CommandItemDisplay="None"
                            CommandItemSettings-ShowAddNewRecordButton="false">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red;">
                                    No Record Found.
                                </div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="chkCollection" CommandName="Select" HeaderStyle-Width="30px" />
                                <telerik:GridTemplateColumn HeaderText="Sno" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <%# Container.ItemIndex + 1 %>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ServiceId" UniqueName="ServiceId" HeaderText="ServiceId" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ServiceName" UniqueName="ServiceName" HeaderText="Service Name" HeaderStyle-Width="300px"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Qty" HeaderStyle-Width="100px" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Width="50px"></asp:TextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <div>
                        <h1>Order List (Matching with Primary Diagnosis)</h1>
                    </div>
                    <telerik:RadGrid ID="gvDrugSeconday" ShowHeader="true" AllowMultiRowSelection="true" Skin="Office2007" Width="100%" BorderWidth="0" ShowGroupPanel="false" ShowFooter="false"
                        AllowFilteringByColumn="false" runat="server" AutoGenerateColumns="false" EnableRowHoverStyle="true"
                        ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" AllowPaging="false" PageSize="20" AllowCustomPaging="false">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                        </ClientSettings>
                        <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" CommandItemDisplay="None"
                            CommandItemSettings-ShowAddNewRecordButton="false">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red;">
                                    No Record Found.
                                </div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="chkCollection" CommandName="Select" HeaderStyle-Width="30px" />
                                <telerik:GridTemplateColumn HeaderText="Sno" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <%# Container.ItemIndex + 1 %>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ServiceId" UniqueName="ServiceId" HeaderText="ServiceId" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ServiceName" UniqueName="ServiceName" HeaderText="Service Name" HeaderStyle-Width="300px"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Qty" HeaderStyle-Width="100px" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Width="50px"></asp:TextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
