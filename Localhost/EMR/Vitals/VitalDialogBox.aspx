<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VitalDialogBox.aspx.cs" Inherits="EMR_Vitals_VitalDialogBox" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center">
        <table cellpadding="0" cellspacing="0" border="0" align="center">
            <tr align="center">
                <td align="center">
                    <asp:Image ID="Image1" runat="server" />
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        
        </table>
    </div>
    </form>
</body>
</html>
