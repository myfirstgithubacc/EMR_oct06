<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SensitivityResult.aspx.cs"
    Inherits="LIS_Phlebotomy_SensitivityResult" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sensitivity Result</title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function IRSValues(ctrlIRS,ctrlCheck) {
            var txtIRS = document.getElementById(ctrlIRS);
            var ctrlCheck = document.getElementById(ctrlCheck);
            if (txtIRS.value != "") {
                if (txtIRS.value.toUpperCase() == "R" || txtIRS.value.toUpperCase() == "S" || txtIRS.value.toUpperCase() == "I" || txtIRS.value.toUpperCase() == "MS") {
                    ctrlCheck.checked=true;
                }
                else
                {               
                    ctrlCheck.checked=false;
                }
            }
            else
                {
                    ctrlCheck.checked=false;
                }
        }    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr height="25px" class="clssubtopic">
                <td>
                    <asp:Label ID="lblServiceDetails" runat="server" SkinID="label" ForeColor="White"></asp:Label>
                </td>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save" OnClick="btnSave_Click"
                                SkinID="button" />&nbsp;
                            <asp:Button ID="Button1" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="1" cellspacing="1" runat="server">
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="S - "></asp:Label>
                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="SENSITIVE TO" Font-Bold="true">
                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="&nbsp;&nbsp;&nbsp;&nbsp;R - "></asp:Label><asp:Label
                            ID="Label6" runat="server" SkinID="label" Text="RESISTANT TO" Font-Bold="true"></asp:Label>
                        <asp:Label ID="Label7" runat="server" SkinID="label" Text="&nbsp;&nbsp;&nbsp;&nbsp;I - "></asp:Label><asp:Label
                            ID="Label8" runat="server" SkinID="label" Text="INTERPRETATION TO" Font-Bold="true"></asp:Label>
                    </asp:Label><asp:Label ID="Label3" runat="server" SkinID="label" Text="&nbsp;&nbsp;&nbsp;&nbsp;MS - "></asp:Label><asp:Label
                        ID="Label4" runat="server" SkinID="label" Text="MODERATELY SENSITIVE TO" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upgvSensitivityResult" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvSensitivityResult" runat="server" SkinID="gridview" Width="100%"
                                AutoGenerateColumns="False" OnRowDataBound="gvSensitivityResult_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrAntibioticId" runat="server" Text='<%#Eval("AntibioticId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Antibiotic Name" HeaderStyle-Width="300px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAntibioticName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId1" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId1_IsResistant" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblhdrIRS1" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table border="0">
                                                <tr>
                                                    <td width="38px">
                                                        <asp:Label ID="lblIRS1" Text="R/S/I/MS" runat="server" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMIC1" runat="server" Text="MIC" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBreakPoint1" runat="server" Text="Break&nbsp;Point" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRelease1" runat="server" Text="Release" SkinID="label" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="48px" align="center">
                                                        <asp:TextBox ID="txtIRS1" runat="server" Width="20px" MaxLength="2" SkinID="textbox"
                                                            Style="text-transform: uppercase;" />
                                                        <AJAX:FilteredTextBoxExtender ID="filter1" runat="server" Enabled="true" FilterType="Custom"
                                                            TargetControlID="txtIRS1" ValidChars="RSrsXMmIi" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMIC1" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBreakPoint1" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkRelease1" runat="server" SkinID="checkbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId2" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId2_IsResistant" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblhdrIRS2" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td width="38px">
                                                        <asp:Label ID="lblIRS2" Text="R/S/I/MS" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMIC2" runat="server" Text="MIC" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBreakPoint2" runat="server" Text="Break&nbsp;Point" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRelease2" runat="server" Text="Release" SkinID="label" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="48px" align="center">
                                                        <asp:TextBox ID="txtIRS2" runat="server" Width="20px" MaxLength="2" SkinID="textbox"
                                                            Style="text-transform: uppercase;" />
                                                        <AJAX:FilteredTextBoxExtender ID="filter2" runat="server" Enabled="true" FilterType="Custom"
                                                            TargetControlID="txtIRS2" ValidChars="RSrsXMmIi" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMIC2" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBreakPoint2" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkRelease2" runat="server" SkinID="checkbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId3" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId3_IsResistant" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblhdrIRS3" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td width="38px">
                                                        <asp:Label ID="lblIRS3" Text="R/S/I/MS" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMIC3" runat="server" Text="MIC" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBreakPoint3" runat="server" Text="Break&nbsp;Point" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRelease3" runat="server" Text="Release" SkinID="label" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="48px" align="center">
                                                        <asp:TextBox ID="txtIRS3" runat="server" Width="20px" MaxLength="2" SkinID="textbox"
                                                            Style="text-transform: uppercase;" />
                                                        <AJAX:FilteredTextBoxExtender ID="filter3" runat="server" Enabled="true" FilterType="Custom"
                                                            TargetControlID="txtIRS3" ValidChars="RSrsXMmIi" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMIC3" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBreakPoint3" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkRelease3" runat="server" SkinID="checkbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId4" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId4_IsResistant" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblhdrIRS4" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td width="38px">
                                                        <asp:Label ID="lblIRS4" Text="R/S/I/MS" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMIC4" runat="server" Text="MIC" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBreakPoint4" runat="server" Text="Break&nbsp;Point" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRelease4" runat="server" Text="Release" SkinID="label" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="48px" align="center">
                                                        <asp:TextBox ID="txtIRS4" runat="server" Width="20px" MaxLength="2" SkinID="textbox"
                                                            Style="text-transform: uppercase;" />
                                                        <AJAX:FilteredTextBoxExtender ID="filter4" runat="server" Enabled="true" FilterType="Custom"
                                                            TargetControlID="txtIRS4" ValidChars="RSrsXMmIi" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMIC4" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBreakPoint4" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkRelease4" runat="server" SkinID="checkbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId5" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrOrganismId5_IsResistant" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblhdrIRS5" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td width="38px">
                                                        <asp:Label ID="lblIRS5" Text="R/S/I/MS" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMIC5" runat="server" Text="MIC" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBreakPoint5" runat="server" Text="Break&nbsp;Point" SkinID="label" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRelease5" runat="server" Text="Release" SkinID="label" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="48px" align="center">
                                                        <asp:TextBox ID="txtIRS5" runat="server" Width="20px" MaxLength="2" SkinID="textbox"
                                                            Style="text-transform: uppercase;" />
                                                        <AJAX:FilteredTextBoxExtender ID="filter5" runat="server" Enabled="true" FilterType="Custom"
                                                            TargetControlID="txtIRS5" ValidChars="RSrsXMmIi" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMIC5" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBreakPoint5" runat="server" Width="65px" MaxLength="20" SkinID="textbox" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkRelease5" runat="server" SkinID="checkbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td style="padding-top: 5px; padding-bottom: 5px;">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
