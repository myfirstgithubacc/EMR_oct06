<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordCheckerV1.aspx.cs"
    Inherits="Pharmacy_Components_PasswordChecker" Title="Password Authentication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
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
                
                
                <div class="container-fluid header_main">
                                 <div class="col-xs-3">
                                  <asp:Label ID="label1" runat="server"  Text="&nbsp;User&nbsp;:&nbsp;" />
                                 </div>
				 
                                 <div class="col-xs-5 text-left"> <asp:Label ID="lblUserName" runat="server"  Font-Bold="true" /></div>
                 
                                 <div class="col-xs-3 text-right pull-right"> </div>
                </div>
             

                <asp:Label ID="lblMessage"  runat="server" Text="&nbsp;" />


               


                <div class="col-xs-12">
                    <div class="row form-group">
                    <div class="col-xs-4"><asp:Label ID="label5" runat="server"  Text="Password&nbsp;" /></div>
                    <div class="col-xs-8"><asp:TextBox ID="txtPassword" runat="server" EnableViewState="true" TextMode="Password"
                                 Width="150px" Height="20px" MaxLength="100" /></div></div>
                </div>
                <div class="col-xs-12 text-center">
                    <div class="row">
                    <asp:Button ID="btnSave" runat="server" cssClass="btn btn-primary" ToolTip="Save" Text="OK"
                                OnClick="btnSave_OnClick" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" cssClass="btn btn-default"
                                OnClick="btnClose_OnClick" />
                    </div>
                </div>
                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
