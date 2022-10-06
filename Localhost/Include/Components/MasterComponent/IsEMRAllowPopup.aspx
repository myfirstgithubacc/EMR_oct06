<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IsEMRAllowPopup.aspx.cs"
    Inherits="Include_Components_MasterComponent_IsEMRAllowPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td class="clssubtopicbar" valign="middle" style="background-color: #E0EBFD;">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblPatientDetail" runat="server" SkinID="label"></asp:Label>
                        </td>
                        <td align="right">
                            <button id="Button2" runat="server" class="buttonBlue" causesvalidation="false" onclick="javascript:window.close();">
                                Close</button>&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    
                    <tr>
                        <td colspan="2">
                          <asp:Label ID="Label3" runat="server" Text="Access to patient EMR not allowed!"
                                ForeColor="Red" />
                        </td>
                    </tr>
                    <tr >
                        <td colspan="2" class="clssubtopicbar" valign="middle" style="background-color: #E0EBFD;">
                            <asp:Label ID="Label1" runat="server" Text="Send request to access patient EMR" /><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label8" runat="server" Text="Remarks" />
                            <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red" />
                            <asp:Label ID="Label4" runat="server" Text=":" />
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtRemark" runat="server" SkinID="textbox" TextMode="MultiLine"
                                Style="max-height: 70px; min-height: 70px; max-width: 200px; min-width: 200px;" />
                                
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Send Request" SkinID="Button" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
