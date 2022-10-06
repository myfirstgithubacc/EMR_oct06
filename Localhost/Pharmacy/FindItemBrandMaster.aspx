<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindItemBrandMaster.aspx.cs"
    Title="" Inherits="Pharmacy_FindItemBrandMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Item</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    
    
     <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
  
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>

        <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.ItemId = document.getElementById("hdnItemID").value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        </script>

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
            
            <div class="container-fluid header_main margin_bottom">
	        <div class="col-md-3 col-sm-3">
		        <h2><asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" /></h2>
	        </div>
	        <div class="col-md-3 col-sm-3 text-right pull-right">
        	<asp:HiddenField ID="hdnItemID" runat="server" />
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" ToolTip="Filter" Text="Filter"
                                OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server"  CssClass="btn btn-primary" ToolTip="Clear Filter"
                                Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close"  CssClass="btn btn-primary"
                                OnClientClick="window.close();" />
	        </div>

        </div>
            
                
                
           <div class="container-fluid form-group">
                <div class="col-md-6 col-sm-6">
                    <div class="col-md-4 col-sm-4"><asp:Label ID="Label2" runat="server" SkinID="label" Text="Item Name" /></div>
                    <div class="col-md-8 col-sm-8">        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearch">
                                <asp:TextBox ID="txtItemName" runat="server" SkinID="textbox" Width="150px"
                                    MaxLength="50" />
                            </asp:Panel></div>
                </div>
           </div>
                            <%--'<%$ Resources:PRegistration, ItemName%>'--%>
                      
                    
                    
                
                
                            <asp:Panel ID="pnlgrid" runat="server" Width="99%" BorderWidth="1" BorderColor="SkyBlue"
                                ScrollBars="None">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvBrand" runat="server" SkinID="gridview" HeaderStyle-Wrap="false"
                                            AutoGenerateColumns="False" Width="100%" OnSelectedIndexChanged="gvBrand_SelectedIndexChanged"
                                            OnRowDataBound="gvBrand_RowDataBound" AllowPaging="true" PageSize="15" OnPageIndexChanging="gvBrand_OnPageIndexChanging">
                                            <Columns>
                                                <asp:CommandField ControlStyle-ForeColor="Blue" SelectText="Select" ShowSelectButton="true"
                                                    HeaderStyle-Width="30px">
                                                    <ControlStyle ForeColor="Blue" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="ItemId" HeaderText="Item Id" />
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                                <asp:BoundField DataField="ItemSubCategoryName" HeaderText='<%$ Resources:PRegistration, ItemSubCategory%>' />
                                                <asp:BoundField DataField="GenericName" HeaderText='<%$ Resources:PRegistration, Generic%>' HeaderStyle-Width="140px" />
                                                <asp:BoundField DataField="ManufactureName" HeaderText='<%$ Resources:PRegistration, Manufacture%>' HeaderStyle-Width="180px" />
                                                <asp:BoundField DataField="Status" HeaderText='<%$ Resources:PRegistration, status%>' />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvBrand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        
                        
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
