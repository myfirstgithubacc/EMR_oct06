<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ConsolidateLabReport.aspx.cs" Inherits="LIS_Phlebotomy_ConsolidateLabReport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
   
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

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

    <script type="text/javascript">
        function wndAddService_OnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var ServiceId = arg.ServiceId;
                var LabData = arg.LabData;
                $get('<%=hdnServiceId.ClientID%>').value = ServiceId;
                $get('<%=hdnLabNo.ClientID%>').value = LabData;
            }
            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);
        }
    </script>
    
    <script  type="text/javascript">
             if (window.captureEvents) {
                 window.captureEvents(Event.KeyUp);
                 window.onkeyup = executeCode;

             }
             else if (window.attachEvent) {
                 document.attachEvent('onkeyup', executeCode);
             }
             function executeCode(evt) {
                 if (evt == null) {
                     evt = window.Event;
                 }
                 var theKey = parseInt(evt.keyCode, 10);
                 switch (theKey) {
                     case 119:  // F8
                         $get('<%=btnClose.ClientID%>').click();
                         break;
                     case 120:  // F9
                         $get('<%=btnPrintPdf.ClientID%>').click();
                         break;
                 }
                 evt.returnValue = false;
                 return false;

             }
    </script>
        <asp:Panel ID="pnlCaseSheet" runat="server">
           <div class="container-fluid">
               <div class="row header_main">
                   <div class="col-md-4 col-sm-4 col-4">
                       <h2><asp:Label ID="lbltitle" runat="server" Text="Consolidate Lab Report"></asp:Label></h2>
                   </div>
                   <div class="col-md-8 col-sm-8 col-8 text-right">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnLabResult" runat="server" CssClass="btn btn-primary" Text="Lab Results" OnClick="btnLabResult_OnClick"></asp:LinkButton>
                            <asp:Button ID="btnPrintPdf" runat="server" CssClass="btn btn-primary" Text="Print" ToolTip="Print(Ctrl+F9)" OnClick="btnPrintPDF_Click" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" OnClientClick="window.close();" />
                            <asp:HiddenField ID="hdnReturnLab" runat="server" Value="" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
               </div>
               </div>
               <div class="row text-center"><asp:Label ID="lblMessage" ForeColor="Green" runat="server" /></div>
          
                <div class="row" style="background:#ffe8d0;">
                    <div class="col-md-12 p-t-b-5"><asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label></div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                    <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" EnableResize="false" Skin="Office2007" 
                        Width="100%" Height="520px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                        ToolsFile="~/Include/XML/DischargeSummary.xml" OnClientSelectionChange="OnClientSelectionChange" EditModes="Design">
                        <CssFiles>
                            <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                        </CssFiles>
                        <SpellCheckSettings AllowAddCustom="true" />
                    </telerik:RadEditor>
                </div>
                    </div>
            </div>
        </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:HiddenField ID="hdnServiceId" runat="server" Value="" />
            <asp:HiddenField ID="hdnLabNo" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

