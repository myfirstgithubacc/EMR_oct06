<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientAllergy.aspx.cs" Inherits="EMR_Allergy_PatientAllergy"  Title=""%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmPatientAllergy" runat="server">
    <table border="0" width="100%" cellpadding="1" cellspacing="1">
        <tr>
            <td>
                Drug Allergy(s)
            </td>
            <td valign="middle">
                Other Allergy(s)
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lstDrugAllergy" runat="server" SkinID="listbox" Width="245px" Height="120">
                </asp:ListBox>
            </td>
            <td valign="middle">
                <asp:ListBox ID="lstOtherAllergy" runat="server" SkinID="listbox" Width="245px" Height="120">
                </asp:ListBox>
            </td>
        </tr>
        </table> 
    </form>

</body>
</html>
