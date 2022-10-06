<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeFacility.aspx.cs" Inherits="Include_Components_MasterComponent_ChangeFacility" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script type="text/javascript">
        function pageLoad() {
            document.getElementById("hdnParentPageURL").value = top.location.href;
        }
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            var oWnd = GetRadWindow();
            top.location.href = document.getElementById("hdnParentPageURL").value;
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>

    <title>Change Facility</title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid">
            <asp:ScriptManager ID="script1" runat="server">
            </asp:ScriptManager>



            <div class="row">
                <div class="col-md-12">&nbsp;</div>
            </div>

            <div class="row">
                <div class="col-md-4 hidden">
                    <asp:Label ID="lblPageType" runat="server" Text="Change Facility" Font-Bold="true" SkinID="label" /></div>
                <div class="col-md-8">
                    <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" SkinID="label" /></div>
                <asp:HiddenField ID="hdnParentPageURL" runat="server" />
            </div>

            <div class="row">
                <div class="col-md-12">&nbsp;</div>
            </div>

            <div class="row">
                <label class="col-md-6 col-xs-2">
                    <asp:Label ID="lblFacility" runat="server" SkinID="label" Text="Facility"></asp:Label></label>
                <div class="col-md-6 col-xs-6">
                    <telerik:RadComboBox ID="ddlFacility" runat="server" Width="100%" Filter="Contains" />
                </div>
                <div class="col-md-2 col-xs-4">
                    <asp:Button ID="BtnChangeFacility" runat="server" CssClass="btn btn-xs btn-primary" Text="Change Facility" OnClick="BtnChangeFacility_onClick" />
                    <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-xs btn-primary" Text="Close" ToolTip="Cancel" OnClientClick="window.close();" />
                </div>
            </div>


        </div>


    </form>
</body>
</html>
