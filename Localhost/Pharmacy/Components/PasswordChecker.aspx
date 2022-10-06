<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordChecker.aspx.cs"
    Inherits="Pharmacy_Components_PasswordChecker" Title="Password Authentication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript"> 

        function returnToParent() {
            var oArg = new Object();
            oArg.IsValidPassword = document.getElementById("hdnIsValidPassword").value;

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

    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main" style="background-color: #E0EBFD">
                        <div class="col--md-12 col-sm-12 col-xs-12 p-t-b-5">
                             <asp:Label ID="label1" runat="server" SkinID="label" Text="&nbsp;User&nbsp;:&nbsp;" />
                            <asp:Label ID="lblUserName" runat="server" SkinID="label" />
                        </div>
                    </div>
                    <div class="row text-center">
                        <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                    </div>
                    <div class="row">
                        <div class="col-md-4 col-sm-4 col-xs-12 col-md-offset-3 col-sm-offset-3 col-xs-offset-1">
                            <div class="col-md-12 colsm-12 col-xs-12" style="background-color: #E0EBFD">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <asp:Label ID="label5" runat="server" SkinID="label" Text="Password&nbsp;" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtPassword" runat="server" EnableViewState="true" TextMode="Password"
                                SkinID="textbox" Width="150px" Height="20px" MaxLength="100" />
                                    </div>
                                </div>
                                <div class="row p-t-b-5">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="OK"
                                OnClick="btnSave_OnClick" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary"
                                OnClick="btnClose_OnClick" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
