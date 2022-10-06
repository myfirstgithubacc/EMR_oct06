<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordCheckerAllUser.aspx.cs"
    Inherits="Pharmacy_Components_PasswordCheckerAllUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Password Authentication</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
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
                <table border="0" width="99%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td colspan="2" align="center" style="height: 20px; color: green; font-size: 12px;
                            font-weight: bold;">
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" style="/*height: 20px;*/ color: green; font-size: 12px;
                            font-weight: bold;">
                            <asp:Label ID="lblEmployeeName" SkinID="label" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="label1" runat="server" SkinID="label" Text="User" />
                        </td>
                        <td>
                            <asp:Panel ID="Panel1"  runat="server" DefaultButton="btnSave">
                                <asp:TextBox ID="txtUserName" runat="server" EnableViewState="true" SkinID="textbox"
                                    Width="150px" Height="20px" MaxLength="50" autocomplete="off" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="label5" runat="server" SkinID="label" Text="Password&nbsp;" />
                        </td>
                        <td>
                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSave">
                                <asp:TextBox ID="txtPassword" runat="server" EnableViewState="true" TextMode="Password"
                                    SkinID="textbox" Width="150px" Height="20px" MaxLength="50" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSave" runat="server" SkinID="Button" ToolTip="Save" Text="OK"
                                OnClick="btnSave_OnClick" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
