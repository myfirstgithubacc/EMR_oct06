<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Checklist.aspx.cs" Inherits="PRegistration_Checklist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Check List</title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = document.getElementById("hdnchecklist").value;
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

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td class="clsheader" align="right">
                            <asp:Button ID="btnSave" runat="server" Text="Proceed" SkinID="Button" OnClick="btnSave_Click" />&nbsp;
                            <asp:Button ID="btnClose" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                                border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                                cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReg" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                                            SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblregNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                            Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                            Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                            Font-Bold="true"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true">
                                        </asp:Label>
                                        <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>
                                        <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true">
                                        </asp:Label>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                            Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" SkinID="label"
                                            Font-Bold="true">
                                        </asp:Label>
                                        <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="450px" Width="100%">
                                <asp:CheckBoxList ID="chkList" runat="server" CssClass="clsListBox" Width="100%" >
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <asp:HiddenField ID="hdnchecklist" runat="server" Value="" />
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
