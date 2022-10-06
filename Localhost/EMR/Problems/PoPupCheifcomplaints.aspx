<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PoPupCheifcomplaints.aspx.cs"
    Inherits="EMR_Problems_PoPupCheifcomplaints" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr class="clsheader" style="height: 20px;">
                <td>
                    Problem Details
                </td>
                <td align="right">
                    <asp:Button ID="btnBack" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />&nbsp;
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="2" cellspacing="4" style="border: 1px solid #000000;">
            <tr>
                <td>
                    <span style="font-weight: bold; color: #003399">Problem</span>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:Label ID="lblproblem" runat="server" Font-Bold="true" ForeColor="#003399"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Chronic"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblChronics" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="140px">
                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="Location"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblLocation" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="Side"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblSide" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Onset"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblOnset" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" SkinID="label" Text="Duration"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblDuration" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" SkinID="label" Text="Quality"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblQuality" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="Context"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblContext" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" SkinID="label" Text="Severity"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblSeverity" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" SkinID="label" Text="Condition"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblCondition" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" SkinID="label" Text="%age"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblPercentage" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                   <span style="font-weight: bold; color: #003399">Associated Problems</span>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label11" runat="server" SkinID="label" Text="Problem"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblAssociatedProblem1" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" SkinID="label" Text="Problem"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblAssociatedProblem2" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label13" runat="server" SkinID="label" Text="Problem"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblAssociatedProblem3" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label14" runat="server" SkinID="label" Text="Problem"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblAssociatedProblem4" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label15" runat="server" SkinID="label" Text="Problem"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblAssociatedProblem5" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
