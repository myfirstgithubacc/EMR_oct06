<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceAckDetails.aspx.cs"
    Inherits="Pharmacy_Components_PasswordCheckerAllUser" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Acknowlege Datails</title>
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
            oArg.Ack_Remakrs = document.getElementById("hdnIsValidPassword").value;
            oArg.IsAcknowledge = document.getElementById("hdnIsAcknowledge").value;
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
                <table cellpadding="2" cellspacing="2">
                    <tr id="tr1" runat="server">
                        <td>
                            <asp:Label ID="lblServiceName" runat="server" SkinID="label" Text='Service Name'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_vServiceName" runat="server" SkinID="label" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr2" runat="server">
                        <td>
                            <asp:Label ID="lblAcknowlageby" runat="server" SkinID="label" Text="Acknowledge By"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_vAcknowlageby" runat="server" SkinID="label" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr3" runat="server">
                        <td>
                            <asp:Label ID="lblAcknowlagedate" runat="server" SkinID="label" Text='Acknowledge Date'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_vAcknowlagedate" runat="server" SkinID="label" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr5" runat="server">
                        <td>
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text='Canceled By'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCanceledBy" runat="server" SkinID="label" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text='Canceled Date'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCancelDate" runat="server" SkinID="label" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRemarks" runat="server" SkinID="label" Text="Remarks"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" TextMode="MultiLine"
                                Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDoctorNameLabel" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, doctor%>'></asp:Label>
                        </td>
                        <td>
                           <asp:Label ID="lblDoctorName" SkinID="label" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr id="trChangedDoctorBy" runat="server">
                        <td>
                            <asp:Label ID="lblNewDoctor" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, doctor%>'></asp:Label>
                        </td>
                        <td>
                          <telerik:RadComboBox ID="ddlPerformingDoctor" runat="server" Width="200px" DropDownWidth="300px" AllowCustomText="true" Filter="Contains">
                          </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSave" runat="server" SkinID="Button" ToolTip="Save" Text="OK"
                                OnClick="btnSave_OnClick" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClick="btnClose_OnClick" />
                        </td>
                    </tr>
                </table>
                <table border="0" width="99%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td colspan="2" align="center" style="height: 20px; color: green; font-size: 12px;
                            font-weight: bold;">
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" style="height: 20px; color: green; font-size: 12px;
                            font-weight: bold;">
                            <asp:Label ID="lblEmployeeName" SkinID="label" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnIsAcknowledge" runat="server" />
                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
