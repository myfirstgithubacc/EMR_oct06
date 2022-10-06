<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MedicationOverride.aspx.cs"
    Inherits="EMR_Medication_MedicationOverride" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Medication Override</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">

        function MaxLenTxt(TXT, LEN) {
            if (TXT.value.length > LEN) {
                alert("Text length should not be greater then " + LEN + " ...");

                TXT.value = TXT.value.substring(0, LEN);
                TXT.focus();
            }
        }

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.IsOverride = document.getElementById("hdnIsOverride").value;
            oArg.OverrideComments = document.getElementById("hdnOverrideComments").value;
            oArg.DrugAllergyScreeningResult = document.getElementById("hdnDrugAllergyScreeningResult").value;

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
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="center" style="font-size: 12px;">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="tdbgGray">
                            <asp:Label runat="server" SkinID="label" Text="Drug-Allergy Screening Result" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDrugAllergyScreeningResult" runat="server" SkinID="textbox" MaxLength="1000"
                                TextMode="MultiLine" Style="min-height: 60px; max-height: 60px; min-width: 600px;
                                max-width: 600px;" Width="600px" Height="60px" onkeyup="return MaxLenTxt(this, 1000);" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgGray">
                            <asp:Label ID="Label8" runat="server" SkinID="label" Text="Override Comments" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtOverrideComments" runat="server" SkinID="textbox" MaxLength="500"
                                Width="600px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkOverride" runat="server" SkinID="checkbox" Text="Override" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddToPrescription" runat="server" SkinID="Button" Text="Add To Prescription"
                                            OnClick="btnAddToPrescription_OnClick" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" runat="server" SkinID="Button" Text="Close"
                                            OnClick="btnClose_OnClick" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnIsOverride" runat="server" Value="" />
                <asp:HiddenField ID="hdnOverrideComments" runat="server" Value="" />
                <asp:HiddenField ID="hdnDrugAllergyScreeningResult" runat="server" Value="" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
