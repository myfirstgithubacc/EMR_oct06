<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopupDiagnosisdetails.aspx.cs"
    Inherits="EMR_Assessment_PopupDiagnosisdetails" %>

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
      
        <table width="100%" cellpadding="6" cellspacing="0" border="1" style="border-collapse: collapse; border: 1px solid #ccc;">
            <tr>
                <td>
                    <span style="font-weight: bold; color: #003399">DiagNosis Name</span>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:Label ID="lblDiagnosisname" runat="server" Font-Bold="true" ForeColor="#003399"></asp:Label>
                </td>
            </tr>
                <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" SkinID="label" Text="Chronic"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblChronic" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
                <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="ICD Code"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblICDCode" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            
            
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Onset Date"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblOnsetDate" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
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
                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="Type"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblType" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Condition"></asp:Label>
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
                    <asp:Label ID="Label6" runat="server" SkinID="label" Text="Primary"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblPrimary" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" SkinID="label" Text="Resolved"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblResolved" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" SkinID="label" Text="Remarks"></asp:Label>
                </td>
                <td width="10px">
                    :
                </td>
                <td>
                    <asp:Label ID="lblRemarks" runat="server" SkinID="label" Font-Size="12px"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
