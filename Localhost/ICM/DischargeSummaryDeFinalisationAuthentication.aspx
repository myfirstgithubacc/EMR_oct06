<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DischargeSummaryDeFinalisationAuthentication.aspx.cs" Inherits="ICM_DischargeSummaryDeFinalisationAuthentication" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function maxLen(that, intMax) {
            if (that.value.length > intMax) {
                that.value = that.value.substr(0, intMax);
                alert("Maximum Length is " + intMax + " characters only.");
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr class="clsheader">
                <td id="tdHeader" align="left" style="padding-left: 10px;" runat="server">
                    <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Summary De-Finalisation"
                        Font-Bold="true" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="center" style="font-size: 12px;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="2" cellspacing="2" style="margin-left: 10px">
            <tr valign="top">
                <td>
                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Recommend By (Doctor)" />
                    <span style='color: Red'>*</span>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlDeFinalizeRecommendBy" runat="server" SkinID="DropDown"
                        EmptyMessage="[ Select ]" Width="370px" Height="300px" MarkFirstMatch="true" />
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:Label ID="Label5" runat="server" SkinID="label" Text="Reason" />
                    <span style='color: Red'>*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtDeFinalizeReason" SkinID="textbox" runat="server" TextMode="MultiLine"
                        Style="min-height: 80px; max-height: 80px; min-width: 365px; max-width: 365px;"
                        onkeyup="return maxLen(this,2000);" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr style="font-size: 12px; font-weight: bold;">
                <td>
                    <asp:Label ID="label2" runat="server" SkinID="label" Text="User" />
                </td>
                <td>
                    <asp:Label ID="lblUserName" runat="server" SkinID="label" />
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Password" />
                    <span style='color: Red'>*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" EnableViewState="true" TextMode="Password"
                        SkinID="textbox" Width="200px" Height="20px" MaxLength="100" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Definalized&nbsp;Data" OnClick="btnSaveData_OnClick"
                        SkinID="Button" Text="Definalized" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false"
                        SkinID="Button" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
