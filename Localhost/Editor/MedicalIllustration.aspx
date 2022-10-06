<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="MedicalIllustration.aspx.cs" Inherits="Editor_MedicalIllustration" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <style type="text/css">
        .orderText { font: normal 12px Arial,Verdana; margin-top: 6px;}
    </style>
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function OnClientLoad(sender, args) {
            if (document.getElementById('<%= hdnIsCopyCaseSheetAuthorized.ClientID %>').value == "False") {
                $telerik.addExternalHandler(sender.get_contentArea(), "copy", function myfunction(ev) {
                    alert("This content cannot be copied!");
                    $telerik.cancelRawEvent(ev);
                });
                $telerik.addExternalHandler(sender.get_textArea(), "copy", function myfunction(ev) {
                    alert("This content cannot be copied!");
                    $telerik.cancelRawEvent(ev);
                });
            }
            var mode = sender.get_mode();

            switch (mode) {
                case 4:
                    setTimeout(function() {
                        var ImageEditor = sender.getToolByName("ImageEditor");
                        var MedicalIllustration = sender.getToolByName("MedicalIllustration");
                        var ExportToRtf = sender.getToolByName("ExportToRtf");

                        ImageEditor.setState(0);
                        MedicalIllustration.setState(0);
                        ExportToRtf.setState(0);
                    }, 0);
                    break;
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
        function OnClientPasteHtml(sender, args) {
            var commandName = args.get_commandName();
            var value = args.get_value();
            if (commandName == "InsertTable") {
                var div = document.createElement("DIV");
                value = value.trim();
                Telerik.Web.UI.Editor.Utils.setElementInnerHtml(div, value);
                var table = div.firstChild;

                if (!table.style.border) {
                    table.style.border = "solid 1px black";
                    args.set_value(div.innerHTML);
                }
            }

        }
        function GetImageOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var xmlString = arg.xmlString;
                $get('<%=hdnImagePath.ClientID%>').value = xmlString;
            }
            $get('<%=btnRefresh.ClientID%>').click();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlCaseSheet" runat="server">

                <div class="VisitHistoryBorderNew">
                    <div class="container-fluid">
                        <div class="row">
                             <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                        </div>
                    </div>    
                </div>
                <div class="VisitHistoryDivText">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007" 
                                    ContentFilters="None" Width="100%" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                     ToolsFile="~/Include/XML/IllustrationXML.xml" Height="530px"  
                                    OnClientLoad="OnClientLoad" OnClientPasteHtml="OnClientPasteHtml">
                                    <CssFiles><telerik:EditorCssFile Value="~/EditorContentArea.css" /></CssFiles>
                                    <SpellCheckSettings AllowAddCustom="true" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>
                            </div>
                        </div>
                    </div>
                </div>
                 <script type="text/javascript">
                    //<![CDATA[
                    
                    Telerik.Web.UI.Editor.CommandList["ImageEditor"] = function (commandName, editor, args) {
                        $get('<%=btnImage.ClientID%>').click();
                    };
                </script>
            </asp:Panel>
            <asp:HiddenField ID="hdnImagePath" runat="server" />
            <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />
            <asp:Button ID="btnImage" runat="server" Text="" SkinID="button" OnClick="btnImage_Click" Style="visibility: hidden;" Width="0px" />
            <asp:Button ID="btnRefresh" runat="server" Text="" SkinID="button" OnClick="btnRefresh_Click" Style="visibility: hidden;" Width="0px" />
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Height="800px" EnableViewState="false">
                <Windows><telerik:RadWindow ID="RadWindow2" OpenerElementID="btnPrintPdf" Height="500" Width="650" MinWidth="650"  Behaviors="Close,Maximize,Minimize,Move,Pin,Resize" runat="server" /></Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

