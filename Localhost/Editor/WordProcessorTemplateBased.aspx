<%@ Page Theme="DefaultControls" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="WordProcessorTemplateBased.aspx.cs" Inherits="EMR_Templates_WordProcessor"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .orderText
        {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function printdiv(PrintPreview) {
            var printContent = $get('<%=lblContent.ClientID%>').innerHTML;
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

            printWindow.document.write(printContent);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
            var preview = $get('<%=pnlPrint.ClientID%>');
            var Casesheet = $get('<%=pnlCaseSheet.ClientID%>');

            if (PrintPreview == 'Preview') {
                preview.style.visibility = "visible";
                Casesheet.style.visibility = "hidden";
            }
            else {
                preview.style.visibility = "hidden";
                Casesheet.style.visibility = "visible";
            }
        }
    </script>

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
            style.fontSize = 11 + 'pt';

            var mode = editor.get_mode();

            switch (mode) {
                case 4:
                    setTimeout(function() {
                        var ImageEditor = editor.getToolByName("ImageEditor");
                        var MedicalIllustration = editor.getToolByName("MedicalIllustration");
                        var ExportToRtf = editor.getToolByName("ExportToRtf");

                        ImageEditor.setState(0);
                        MedicalIllustration.setState(0);
                        ExportToRtf.setState(0);
                    }, 0);
                    break;
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
        function openWin() {
            var oWnd = radopen("Addendum.aspx", "RadWindow1");
        }

        function openLetters() {
            var oWnd = radopen("/Emr/Letters/Default.aspx", "RadWindow1");
            oWnd.setSize(900, 600);
            oWnd.Center();
        }
        function openOldForm() {
            var oWnd = radopen("/Emr/Letters/OldForm.aspx", "RadWindow1");
            oWnd.setSize(900, 600);
            oWnd.Center();
        }


        function fun1() {
            $get("ctl00_ContentPlaceHolder1_RTF1Center").innerText = $get("ctl00_ContentPlaceHolder1_RTF1Center").innerText + "Satvinder";
        }
        function OnClientClose(oWnd, args) {
            $get('<%=btnCheck.ClientID%>').click();
        }

        function OnCloseSentenceGalleryRadWindow(oWnd, args) {
            var arg = args.get_argument();
            if (arg == null)
                return;
            if (arg) {
                var Sentence = arg.Sentence;
            }
            $get('<%=hdSen.ClientID%>').value = Sentence;
            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(Sentence);
        }
        function confirm_sign() {
            var sign = $get('<%=btnSigned.ClientID%>').value;
            if (sign == "Sign") {
                if (confirm("Sign the form?\n (Note: Form once signed cannot be edited.") == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if (confirm("UnSign the form?") == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        function OnClientLoad(RadWindow, args) {
            makeUnselectable(RadWindow.get_element());
        }
        function makeUnselectable(element) {
            $telerik.$("*", element).attr("unselectable", "on");
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

            function OnClientClose(oWnd, args) {
                $get('<%=btnclose.ClientID%>').click();
            }

            //Cancel the event if you want to prevent pasteHtml to execute
            /*
            args.set_cancel(true);
            */
        }
        function GetImageOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var xmlString = arg.xmlString;
                $get('<%=hdnImagePath.ClientID%>').value = xmlString;
            }
            $get('<%=btnRefresh.ClientID%>').click();
        }

        function PrintSetupContent() {
            var ReportContent = $get('<%=hdnReportContent.ClientID%>')

            var WindowObject = window.open('', 'PrintWindow2', 'width=750,height=650,top=50,left=50,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(ReportContent.value);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            //WindowObject.close();
        }
    </script>
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <div id="dis" runat="server" width="100%" style="vertical-align: top;">
                 <telerik:RadWindowManager ID="RadWindowManager4" Skin="Office2007" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowPrint" Skin="Office2007" runat="server" />
                                </Windows>
                            </telerik:RadWindowManager>
                <asp:Panel ID="pnlCaseSheet" runat="server">
                    <div style="font-weight: bold;float: right;margin-right: 31px !important;margin: 10px;}">
                     <asp:Button ID="btnPrintPDFReport" Text="Print Report" runat="server" OnClick="btnPrintPDFReport_Click" CssClass="btn btn-primary" Visible="false"  Font-Bold="true"  />
                        </div>
                   <%-- <table border="0" cellpadding="0" cellspacing="0" class="clsheader" width="100%">
                        <tr>
                            <td style="width: 120px">
                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="&nbsp;Word&nbsp;Processor" />
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1" class="clsheader">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" SkinID="label" Font-Bold="true" Text="Report:&nbsp;" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="ddlReport" runat="server" Width="100px" EmptyMessage="[ Select ]"
                                                AutoPostBack="true" DropDownWidth="200px" OnSelectedIndexChanged="ddlReport_OnSelectedIndexChanged" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnPrintReport" SkinID="button" runat="server" Text="Print&nbsp;Report"
                                                ToolTip="Print Report" OnClientClick="javaScript:PrintSetupContent();" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnPrintPdf" runat="server" SkinID="button" Text="Print" OnClick="btnPrintPDF_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient : " SkinID="label" />
                                <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label5" runat="server" Text="DOB : " SkinID="label" />
                                <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label15" runat="server" Text="Mobile No : " SkinID="label" />
                                <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="lblIpno" runat="server" Text="IP No : " SkinID="label" />
                                <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label7" runat="server" Text="Admission Date : " SkinID="label" />
                                <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                            </td>
                            <td align="right">
                                <table border="0" cellpadding="0" cellspacing="1" class="clsheader">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddendum" Text="Addendum" runat="server" SkinID="Button" Enabled="True"
                                                OnClick="btnAddendum_OnClick" Visible="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSeenByDoctor" Text="Patient Seen" runat="server" BackColor="#FFCBA4"
                                                Font-Bold="true" Enabled="True" OnClick="btnSeenByDoctor_OnClick"
                                                Visible="true" />
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" Text="Save" ToolTip="Save" SkinID="Button" runat="server"
                                                OnClick="btnSave_Click" Visible="false" BackColor="#FFCBA4" />
                                                
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="2">
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="From" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" SkinID="label" Text="To" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="Template" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlTemplatePatient" runat="server" EmptyMessage="[ Select ]"
                                    Width="250px" MarkFirstMatch="true" />
                                <%-- Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplatePatient_SelectedIndexChanged" 
                            </td>--%>
                    <table >
                    <tr>
                            <td>
                            <asp:Button ID="btnSigned" Text="Sign" runat="server" BackColor="#FFCBA4"  Visible="false"  
                                                Font-Bold="true"  OnClientClick="return confirm_sign();" />
                            
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblMessage" ForeColor="Green" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="2" cellspacing="2" style="margin-left: 10px;">
                        <tr runat="server" visible="false">
                            <td>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Icons/EHR.jpg" />
                                <asp:LinkButton ID="lnkAddProblem" runat="server" Text="Chief&nbsp;Complaint" Font-Bold="true"
                                    OnClick="lnkAddProblem_OnClick" />
                            </td>
                            <td>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Icons/phlebotomy.jpg" />
                                <asp:LinkButton ID="lnkAddVital" runat="server" Text="Vital" Font-Bold="true" OnClick="lnkAddVital_OnClick" />
                            </td>
                            <td>
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/Icons/clinical_master.jpg" />
                                <asp:LinkButton ID="lnkAddAllergy" runat="server" Text="Allergy" Font-Bold="true"
                                    OnClick="lnkAddAllergy_OnClick" />
                            </td>
                            <td>
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Icons/appointmant.jpg" />
                                <asp:LinkButton ID="lnkAddDiagnosis" runat="server" Text="Diagnosis" Font-Bold="true"
                                    OnClick="lnkAddDiagnosis_OnClick" />
                            </td>
                            <td>
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Icons/RIS.jpg" />
                                <asp:LinkButton ID="lnkAddOrder" runat="server" Text="Orders" Font-Bold="true" OnClick="lnkAddOrder_OnClick" />
                            </td>
                            <td>
                                <asp:Image ID="Image6" runat="server" ImageUrl="~/Icons/pharmacy.jpg" />
                                <asp:LinkButton ID="lnkAddMedication" runat="server" Text="Medication" Font-Bold="true"
                                    OnClick="lnkAddMedication_OnClick" />
                            </td>
                            <td>
                                <asp:Image ID="Image7" runat="server" ImageUrl="~/Icons/pharmacy_reports.jpg" />
                                <asp:LinkButton ID="lnkAddNote" runat="server" Text="Note" Font-Bold="true" OnClick="lnkAddNote_OnClick" />
                            </td>
                            <td>
                                &nbsp;|&nbsp;
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkGeneralTemplate" runat="server" Text="General&nbsp;Template"
                                                Font-Bold="true" OnClick="lnkGeneralTemplate_OnClick" />&nbsp;
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkSpecilityTemplate" runat="server" Text="Specility&nbsp;Template"
                                                Font-Bold="true" OnClick="lnkSpecilityTemplate_OnClick" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadComboBox ID="ddlGeneralTemplate" SkinID="DropDown" runat="server" Width="200px"
                                                EmptyMessage="[ Select ]" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="ddlSpecilityTemplate" SkinID="DropDown" runat="server" Width="200px"
                                                EmptyMessage="[ Select ]" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <telerik:RadWindowManager ID="RadWindowManager2" ShowContentDuringLoad="false" VisibleStatusbar="false"
                                    ReloadOnShow="true" runat="server" Skin="Office2007">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" OnClientClose="OnClientClose" Skin="Office2007" Width="650" Height="445"
                                            Modal="true" NavigateUrl="Addendum.aspx" runat="server" />
                                    </Windows>
                                </telerik:RadWindowManager>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td valign="top">
                                <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007"
                                    Width="100%" Height="500px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                    ToolsFile="~/Include/XML/Visiting.xml" OnClientSelectionChange="OnClientSelectionChange"
                                    OnClientLoad="OnClientEditorLoad" OnClientPasteHtml="OnClientPasteHtml">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                    </CssFiles>
                                    <SpellCheckSettings AllowAddCustom="true" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>
                            </td>
                        </tr>
                    </table>

                    <script type="text/javascript">
                        //<![CDATA[
                        Telerik.Web.UI.Editor.CommandList["ExportToRtf"] = function(commandName, editor, args) {
                            $get('<%=btnExporttoWord.ClientID%>').click();
                        };
                        Telerik.Web.UI.Editor.CommandList["ImageEditor"] = function(commandName, editor, args) {
                            //var args = editor.get_html() //returns the HTML of the selection.
                        
                            var myCallbackFunction = function(sender, args) {

                                editor.pasteHtml(String.format("<table><tbody><tr><td><img src='{0}' width='250px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table>", args.image));
                            }

                            editor.showExternalDialog('ImageEditor.aspx', args, 970, 600, myCallbackFunction, null, 'Image Editor', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, true, true);
                        };
                        Telerik.Web.UI.Editor.CommandList["MedicalIllustration"] = function(commandName, editor, args) {
                            $get('<%=btnImage.ClientID%>').click();
                        };
                        //]]>
                    </script>

                    <asp:Button ID="btnCheck" Text="" SkinID="Button" OnClick="btnCheck_Onclick" runat="server"
                        BackColor="Transparent" BorderColor="Transparent" BorderWidth="0" Height="0"
                        Width="0" />
                    <asp:Button ID="btnAddSen" Text="" SkinID="Button" OnClick="btnAddSen_Onclick" runat="server"
                        BackColor="Transparent" BorderColor="Transparent" BorderWidth="0" Height="0"
                        Width="0" />
                    <asp:HiddenField ID="hdSen" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlPrint" runat="server">
                    <div style="float: right; vertical-align: top">
                        <button onclick="javascript:printdiv('Preview');" class="buttonBlue">
                            Print</button>&nbsp;<asp:Button ID="btnBackToWordProcessor" runat="server" Text="Back"
                                SkinID="Button" OnClick="btnBackToWordProcessor_Click" /></div>
                    <asp:Label ID="lblContent" runat="server" />
                </asp:Panel>
            </div>
            <asp:HiddenField ID="txtHosID" runat="server" />
            <asp:HiddenField ID="txtFacID" runat="server" />
            <asp:HiddenField ID="txtProvID" runat="server" />
            <asp:HiddenField ID="txtProviderName" runat="server" />
            <asp:HiddenField ID="txtEncID" runat="server" />
            <asp:HiddenField ID="txtPatID" runat="server" />
            <%-- <input id="btnGetParameters" type="button" value="Get Parameters" onclick="javascript:setEncounterParams(); return false;" />--%>
            <asp:HiddenField ID="hdnDoctorImage" runat="server" />
            <asp:HiddenField ID="hdnSignedId" runat="server" />
            <asp:HiddenField ID="hdnUnSignedId" runat="server" />
            <asp:HiddenField ID="hiddomainname" runat="server" />
            <asp:HiddenField ID="hidvalue" runat="server" />
            <asp:HiddenField ID="hdnImagePath" runat="server" />
            <asp:Button ID="btnImage" runat="server" Text="" SkinID="button" OnClick="btnImage_Click"
                Style="visibility: hidden;" Width="0px" />
            <asp:Button ID="btnExporttoWord" runat="server" Text="Export to Word" SkinID="button"
                OnClick="btnExporttoWord_Click" Style="visibility: hidden;" Width="0px" />
            <asp:Button ID="btnRefresh" runat="server" Text="" SkinID="button" OnClick="btnRefresh_Click"
                Style="visibility: hidden;" Width="0px" />
            <asp:Button ID="btnDefaultTemplate" runat="server" Style="visibility: hidden;" Text="Default Template"
                SkinID="button" OnClick="btnDefaultTemplate_Click" />
            <asp:Button ID="btnLetter" runat="server" SkinID="button" Style="visibility: hidden;"
                Text="Letters" OnClick="btnLetter_Click" />
            <asp:Button ID="btnDictionary" ToolTip="Dictionary" runat="server" Style="visibility: hidden;"
                Text="Dictionary" SkinID="button" OnClick="btnDictionary_Click" />
            <asp:Button ID="btnSentenceGallery" ToolTip="Sentence Gallery" Style="visibility: hidden;"
                runat="server" Text="Sentence Gallery" OnClick="btnSentenceGallery_OnClick" SkinID="button" />
            <asp:HiddenField ID="hdnReportContent" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:Button ID="btnclose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                OnClick="btnclose_OnClick" />
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindow2" OpenerElementID="btnPrintPdf" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" OnClientShow="OnClientLoad" OpenerElementID="cmd_OpenAppointment"
                        runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
