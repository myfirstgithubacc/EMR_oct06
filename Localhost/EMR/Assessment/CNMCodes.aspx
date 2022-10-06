<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CNMCodes.aspx.cs" Inherits="EMR_Assessment_CNMCodes" Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="smDiagnosis" runat="server">
    </asp:ScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0">
    <tr>    
        <td class="clssubtopicbar" align="right" valign="Middle" style="background-color:#E0EBFD; padding-right:10px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                <asp:Label ID="lbl_Msg" runat="server" ForeColor="Green"  Font-Bold="true" style="padding-right:100px;"></asp:Label>
                <asp:LinkButton ID="lnkDiagnosis" runat="server" CausesValidation="false" Text="Diagnosis Coding"
                Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                OnClick="lnkDiagnosis_OnClick"></asp:LinkButton>&nbsp;|&nbsp;
                <asp:LinkButton ID="lnkMedication" runat="server" CausesValidation="false" Text="Medication Coding"
                    Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkMedication_OnClick"></asp:LinkButton>                           
                <script language="JavaScript" type="text/javascript">
                    function LinkBtnMouseOver(lnk) {
                        document.getElementById(lnk).style.color = "red";
                    }
                    function LinkBtnMouseOut(lnk) {
                        document.getElementById(lnk).style.color = "blue";
                    }
                </script>
                </ContentTemplate>
               </asp:UpdatePanel>
                        </td>
    </tr>
    </table>
    </div>
    </form>

</body>
</html>
