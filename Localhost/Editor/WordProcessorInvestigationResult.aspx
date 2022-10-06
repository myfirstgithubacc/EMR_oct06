<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true" CodeFile="WordProcessorInvestigationResult.aspx.cs" Inherits="Editor_WordProcessorInvestigationResult" %>

    <%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../Include/JS/tinymce/jquery.tinymce.min.js"></script>
    <style>#ctl00_ContentPlaceHolder1_FreeTextBox1_TabRow{display:none;}</style>
    <%--<script type="text/javascript"  src="../Include/JS/tinymce/tinymce.min.js"></script>--%>

    <%--<script type="text/javascript" src="../Include/JS/tinymce/tinymce.min.js"></script>--%>
 
 <%--   <script type="text/javascript" language="javascript">
        tinyMCE.init({
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups",
           
        });
    </script>--%>



    <%-- <script type="text/javascript">
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
    </script>--%>

     <div class="VisitHistoryDiv">
        <div class="container-fluid">

           
            <div class="row">
                  
                <div class="col-md-3 col-sm-3">
                    <div class="WordProcessorDivText">
                        <h2>
                            <asp:Label ID="Label5" runat="server" Text="&nbsp;Investigation Results"></asp:Label></h2>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6">
                    <div class="WordProcessorDivText">
                        <h4>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Text="&nbsp;" /></h4>
                    </div>
                </div>
                <div class="col-md-9 col-sm-6">
                    <div class=" text-right">
                        <asp:Button ID="btnClose" runat="server"  CssClass="btn btn-primary"  OnClientClick ="window.close();" Text="Close" />
                    </div>
                </div>
                

            </div>
        </div>
        <div class="VisitHistoryBorderNew">
            <div class="container-fluid">
                <div class="row">
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />

                </div>
            </div>
        </div>
    </div>
   <%-- <style>td#ctl00_ContentPlaceHolder1_FreeTextBox1_designModeTab {
    visibility: hidden;
    display: none;
}</style>--%>

       <div class="VitalHistory-Div02">
        <div class="container-fluid">
            <div class="row">

                <div class="col-md-12">
                    <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />
<%--               <telerik:RadEditor runat="server" ID="RTF1"  Skin="Office2007"  EnableTextareaMode="false" ContentFilters="None" Width="100%" AutoResizeHeight="false" ToolsFile="~/Include/XML/WordProcessorEditorTools.xml"  OnClientSelectionChange="OnClientSelectionChange" OnClientLoad="OnClientLoad" OnClientPasteHtml="OnClientPasteHtml">                       --%>
                                      <%--   <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007" ContentFilters="None" Width="100%" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage" ToolsFile="~/Include/XML/WordProcessorEditorTools.xml" OnClientLoad="OnClientLoad" OnClientPasteHtml="OnClientPasteHtml">

                    <CssFiles><telerik:EditorCssFile Value="~/EditorContentArea.css" /></CssFiles>
                                    <SpellCheckSettings AllowAddCustom="true" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>--%>
                    <%--<asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" ></asp:TextBox>--%>
                       <FTB:FreeTextBox ID="FreeTextBox1" runat="server" EnableHtmlMode="True" Height="385px" Width="100%" EnableToolbars="False" StartMode="HtmlMode" ToolbarLayout="">
</FTB:FreeTextBox>

                            </div>
                   </div>
        </div>
    </div>
   
</asp:Content>

