<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DischargePatientSummaryData.aspx.cs" Inherits="ICM_DischargePatientSummaryData" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <script type="text/javascript">
                function OnClientSelectionChange(editor, args) {
                    var tool = editor.getToolByName("RealFontSize");
                    if (tool && !$telerik.isIE) {
                        setTimeout(function () {
                            var value = tool.get_value();

                            switch (value) {
                                case "11px":
                                    value = value.replace("11px", "9pt");
                                    break;
                                case "12px":
                                    value = value.replace("12px", "9pt");
                                    break;
                                case "14px":
                                    value = value.replace("14px", "11pt");
                                    break;
                                case "15px":
                                    value = value.replace("15px", "11pt");
                                    break;
                                case "16px":
                                    value = value.replace("16px", "12pt");
                                    break;
                                case "18px":
                                    value = value.replace("18px", "14pt");
                                    break;
                                case "19px":
                                    value = value.replace("19px", "14pt");
                                    break;
                                case "24px":
                                    value = value.replace("24px", "18pt");
                                    break;
                                case "26px":
                                    value = value.replace("26px", "20pt");
                                    break;
                                case "27px":
                                    value = value.replace("27px", "20pt");
                                    break;
                                case "32px":
                                    value = value.replace("32px", "24pt");
                                    break;
                                case "34px":
                                    value = value.replace("34px", "26pt");
                                    break;
                                case "35px":
                                    value = value.replace("35px", "26pt");
                                    break;
                                case "48px":
                                    value = value.replace("48px", "36pt");
                                    break;
                            }
                            tool.set_value(value);
                        }, 0);
                    }
                }

                function OnClientEditorLoad(editor, args) {

                    var tool = editor.getToolByName("FontName");
                    tool.set_value("Candara");
                    editor.get_contentArea().style.fontFamily = 'Candara'
                    editor.get_contentArea().style.fontSize = '11pt'
                }
            </script>

            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr class="clsheader">
                            <td id="tdHeader" align="left" style="padding-left: 10px;" runat="server">
                                <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Discharge Summary Before Definalized"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center" style="font-size: 12px;">
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="2" cellspacing="2" style="margin-left: 10px">
                        <tr valign="top">
                            <td>
                                <telerik:RadEditor ID="RTF1" runat="server" EnableTextareaMode="false" Skin="Office2007"
                                    Width="100%" Height="450px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                    ToolsFile="~/Include/XML/WordProcessorEditorTools.xml" OnClientSelectionChange="OnClientSelectionChange"
                                    OnClientLoad="OnClientEditorLoad">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                    </CssFiles>
                                    <SpellCheckSettings AllowAddCustom="true" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
