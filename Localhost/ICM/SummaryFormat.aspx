<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SummaryFormat.aspx.cs" Inherits="ICM_SummaryFormat" Title="Akhil Systems Pvt. Ltd." %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function OnClientSelectionChange(editor, args) {
            var tool = editor.getToolByName("RealFontSize");
            if (tool && !$telerik.isIE) {
                setTimeout(function() {
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
        
        
    </script>

    <script type='text/javascript'>
        function OnClientEditorLoad(editor, args) {
            var style = editor.get_contentArea().style;
            style.fontFamily = 'Tahoma';
            style.fontSize = '11pt';
        } 
    </script>

    <script language="javascript" type="text/javascript">

        function OnClientLoad(RadWindow, args) {
            makeUnselectable(RadWindow.get_element());
        }
        function OnClientPasteHtml(sender, args) {
            var commandName = args.get_commandName();
            var value = args.get_value();
            if (commandName == "InsertTable") {
                //Set border to the inserted table elements
                var div = document.createElement("DIV");

                //Remove extra spaces from begining and end of the tag
                value = value.trim();

                Telerik.Web.UI.Editor.Utils.setElementInnerHtml(div, value);
                var table = div.firstChild;

                if (!table.style.border) {
                    table.style.border = "solid 1px black";
                    //Set new content to be pasted into the editor
                    args.set_value(div.innerHTML);
                }
            }
        }
    </script>

    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Summary Format</strong>
                            <telerik:RadComboBox ID="ddlSummaryFormat" SkinID="DropDown" runat="server" AllowCustomText="true"
                                EmptyMessage=" " AutoPostBack="true" Filter="StartsWith" OnSelectedIndexChanged="ddlSummaryFormat_OnSelectedIndexChanged"
                                Width="220px" />
                        </td>
                        <td>
                            <asp:Button ID="btnSummarySave" runat="server" SkinID="Button" Text="Save Format"
                                OnClick="btnSummarySave_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007"
                                Width="100%" Height="450px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                ToolsFile="~/Include/XML/WordProcessorEditorTools.xml" OnClientSelectionChange="OnClientSelectionChange"
                                OnClientLoad="OnClientEditorLoad" OnClientPasteHtml="OnClientPasteHtml">
                                <CssFiles>
                                    <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                </CssFiles>
                                <SpellCheckSettings AllowAddCustom="true" />
                                <ImageManager ViewPaths="~/medical_illustration" />
                            </telerik:RadEditor>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="FormatID" runat="server" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
