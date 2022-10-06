<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DischargeSummaryNew.aspx.cs" Inherits="ICM_DischargeSummaryNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />



    <link rel="stylesheet" href="../Include/cssEditor/font-awesome.min.4.4.0.css">
    <link rel="stylesheet" href="../Include/cssEditor/froala_editor.css">
    <link rel="stylesheet" href="../Include/cssEditor/froala_style.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/code_view.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/draggable.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/colors.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/emoticons.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/image_manager.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/image.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/line_breaker.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/table.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/char_counter.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/video.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/fullscreen.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/file.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/help.css">
    <link rel="stylesheet" href="../Include/cssEditor/plugins/special_characters.css">
    <link rel="stylesheet" href="../Include/cssEditor/codemirror.min.5.3.0.css">

    <style>
        .fr-element td {
            padding: 5px 10px 5px 5px;
        }

            .fr-element td p,
            .fr-element p span,
            .fr-element p,
            .fr-element span .fr-text-spaced span {
                line-height: 1.4;
            }


        div#editor {
            width: 95%;
            margin: auto;
            text-align: left;
        }

        .ss {
            background-color: red;
        }
    </style>


    <style type="text/css">
        .EMR-HealthCheck h2 {
            width: 126px !important;
        }

        .orderText {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }

        #fullscreen-1,
        #color-1,
        #inlineStyle-1,
        /*#paragraphStyle-1,*/
        /*#outdent-1,*/
        /*#indent-1,*/
        #quote-1,
        #insertLink-1,
        #insertImage-1,
        .fr-separator,
        /*#paragraphFormat-1,*/
        #insertImage-1,
        #insertVideo-1,
        #insertFile-1,
        #emoticons-1,
        #insertHR-1,
        /*#selectAll-1,*/
        #print-1,
        #spellChecker-1
        /*,#help-1*/ 
        {
            display: none;
        }

        #test {
            overflow-y: scroll;
            max-height: 300px;
        }

        input#ctl00_ContentPlaceHolder1_chkExport {
            float: none !important;
        }

        #ctl00_ContentPlaceHolder1_lblMessage {
            float: none;
            margin: 0;
            padding: 0;
            font-size: 14px;
           
            font-weight: bold;
            width: 100%;
            position: relative;
          
        }
    </style>

    <script type="text/javascript">
        function IsFinalizeScript() {
            var IsFinal = $('#<%= hdnIsFinalize.ClientID%>').val();
            // alert(IsFinal);
            if (IsFinal == "1") {
                //   alert("finalize");
                $('.fr-element').attr('contenteditable', 'true');
                $('.fr-toolbar').css('pointer-events', 'visible');
            }
            else {
                //    alert("definalize");
                $('.fr-element').attr('contenteditable', 'false');
                $('.fr-toolbar').css('pointer-events', 'none');
            }
        }

        function showDischargeSummary() {
            // $("#fullscreen-1").remove();
            //$("").remove();
            //$("#inlineStyle-1").remove();
            //$("#paragraphStyle-1").remove();
            //$("#outdent-1").remove();
            //$("#indent-1").remove();
            //$("#quote-1").remove();
            //$("#insertLink-1").remove();
            //$("#insertImage-1").remove();



            $('.fr-element').attr('id', 'test');
            //$(".note-editable").css("-webkit-user-select", "text");
            var temp = $('#<%= DischargeSummaryCode.ClientID%>').val();

            //$('.note-editable').append(temp);
            $('.fr-element').append(temp);

        }

        function ChangeFormatDischargeSummary() {

            $('.fr-element').attr('id', 'test');
            var temp = $('#<%= hdnTemplateData.ClientID%>').val();

            $('.fr-element').append(temp);
        }

        function getCaretCharacterOffsetWithin(element) {
            var caretOffset = 0;
            var doc = element.ownerDocument || element.document;
            var win = doc.defaultView || doc.parentWindow;
            var sel;
            if (typeof win.getSelection != "undefined") {
                sel = win.getSelection();
                if (sel.rangeCount > 0) {
                    var range = win.getSelection().getRangeAt(0);
                    var preCaretRange = range.cloneRange();
                    preCaretRange.selectNodeContents(element);
                    preCaretRange.setEnd(range.endContainer, range.endOffset);
                    caretOffset = preCaretRange.toString().length;
                }
            } else if ((sel = doc.selection) && sel.type != "Control") {
                var textRange = sel.createRange();
                var preCaretTextRange = doc.body.createTextRange();
                preCaretTextRange.moveToElementText(element);
                preCaretTextRange.setEndPoint("EndToEnd", textRange);
                caretOffset = preCaretTextRange.text.length;
            }
            return caretOffset;
        }

        function getCaretPosition() {
            if (window.getSelection && window.getSelection().getRangeAt) {
                var range = window.getSelection().getRangeAt(0);
                var selectedObj = window.getSelection();
                var rangeCount = 0;
                var childNodes = selectedObj.anchorNode.parentNode.childNodes;
                for (var i = 0; i < childNodes.length; i++) {
                    if (childNodes[i] == selectedObj.anchorNode) {
                        break;
                    }
                    if (childNodes[i].outerHTML)
                        rangeCount += childNodes[i].outerHTML.length;
                    else if (childNodes[i].nodeType == 3) {
                        rangeCount += childNodes[i].textContent.length;
                    }
                }
                return range.startOffset + rangeCount;
            }
            return -1;
        }

        function showCaretPos() {
            var el = document.getElementById("test");
            var caretPosEl = document.getElementById("caretPos");
            caretPosEl.innerHTML = "Caret position: " + getCaretPosition(); //getCaretCharacterOffsetWithin(el);
        }

        document.body.onkeyup = showCaretPos;
        document.body.onmouseup = showCaretPos;

        function returnToParent() {

            var oArg = new Object();
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function changeMode(mode) {  // mode=1 --design , mode=2 --html  , mode=4 -- preview
            //setting edit mode value after postback
         <%-- var editor = $find("<%=RTF1.ClientID %>");
            editor.set_mode(mode);--%>
        }
        function OnClientClose(oWnd, args) {
             <%-- document.getElementById('<%= Timer1.ClientID%>').disabled=false;
            document.getElementById('<%= chkAutoSave.ClientID%>').checked=true;--%>
            $get('<%=btnEnableTimer.ClientID%>').click();
        }

        function MaxLenTxt(TXT, MAX) {
            if (TXT.value.length > MAX) {
                alert("Text length should not be greater then " + MAX + " ...");

                TXT.value = TXT.value.substring(0, MAX);
                TXT.focus();
            }
        }

        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();

        }

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


        function OnClientPasteHtml(sender, args) {
            var commandName = args.get_commandName();
            var value = args.get_value(); //here is the content that will be inserted via the InsertTable dialog
            if (commandName == "InsertTable") {
                //Set border to the inserted table elements
                var div = document.createElement("DIV");
                value = value.trim();
                Telerik.Web.UI.Editor.Utils.setElementInnerHtml(div, value);
                var table = div.firstChild;

                if (!table.style.border) {
                    table.style.border = "solid 1px black";
                    table.style.width = "60%";
                    var vTD = div.getElementsByTagName("TD");
                    for (var j = 0; j < vTD.length; j++) {
                        var oTd = vTD[j];
                        oTd.style.border = "solid 1px black";
                    }
                    args.set_value(div.innerHTML);
                }
            }
        }
    </script>

    <script type='text/javascript'>
        function OnClientEditorLoad(editor, args) {
            var tool = editor.getToolByName("FontName");
            tool.set_value(document.getElementById('<%= hdnFontName.ClientID%>').value);
            editor.get_contentArea().style.fontFamily = document.getElementById('<%= hdnFontName.ClientID%>').value
            editor.get_contentArea().style.fontSize = '11pt'
        }
    </script>

    <%-- <script type='text/javascript'>
        function OnClientEditorLoad(editor, args) {
            var tool = editor.getToolByName("FontName");
            tool.set_value("Candara");
            editor.get_contentArea().style.fontFamily = 'Candara'
            editor.get_contentArea().style.fontSize = '11pt'
        } 
    </script>--%>
    <script type="text/javascript">
        function getHTMLCode_Click() {
            //$('#<%= DischargeSummaryCode.ClientID%>').val($('.note-codable').val());
            $('.fr-element.fr-view').find('table').attr('border', '1')
            $('#<%= DischargeSummaryCode.ClientID%>').val($('.fr-element.fr-view')[0].innerHTML);
        }
    </script>

    <script language="javascript" type="text/javascript">
        function OnClientClose(oWnd, args) {

        }

        function ShowError(sender, args) {
            alert("Enter a Valid Date");
            sender.focus();
        }
    </script>

    <script type="text/javascript">
        function OnClientLoad(combobox, args) {
            makeUnselectable(combobox.get_element());
        }

        function OnClientItemsRequested(combobox, args) {
            $telerik.$("*", combobox.get_dropDownElement()).attr("unselectable", "on");
        }

        //Make all combobox items unselectable to prevent selection in editor being lost
        function makeUnselectable(element) {
            $telerik.$("*", element).attr("unselectable", "on");
        }

        function getSelectedItemValue(combobox, args) {
            var value = combobox.get_value();

        <%--   var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(value);--%>

        }

        function wndAddService_OnClientClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var ServiceId = arg.ServiceId;
                var LabData = arg.LabData;
                //alert(arg.LabData);
                $get('<%=hdnServiceId.ClientID%>').value = ServiceId;
                $get('<%=hdnLabNo.ClientID%>').value = LabData;

                getCode_Click(LabData);
            }
         <%-- var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);--%>

            OnClientSelectionChange(oWnd, args);
            $get('<%=btnEnableTimer.ClientID%>').click();
        }

        function wndAddMedication_OnClientClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                //alert(arg.LabData);
                var LabData = arg.LabData;
                $get('<%=hdnLabNo.ClientID%>').value = LabData;
                getCode_Click(LabData);
            }

            $get('<%=btnBindMedication.ClientID%>').click();
       <%--   var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);--%>
            $get('<%=btnEnableTimer.ClientID%>').click();
        }
    </script>

    <script type="text/javascript">
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
                case 113: //F2
                    $get('<%=btnFinalize.ClientID%>').click();
                    break;
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                    break;
                case 119:  // F8
                    $get('<%=btnClose.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrintPDFNew.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }

        function OnClientDeFinalisationClose(oWnd, args) {
            $get('<%=btnDeFinalisationClose.ClientID%>').click();
        }
    </script>


    <script type="text/javascript">



        $(document).ready(function () {
            $('.fr-element').children()[0].remove();
            $('.fr-wrapper').find('a').remove()



            $('.fr-tooltip').next().on('click', '.fr-buttons #bold-1', function () {
                alert("New Button Clicked");
            });



        });
    </script>
    <asp:HiddenField ID="hdnFontName" runat="server" />

    <div id="dis" runat="server" width="100%" style="vertical-align: top">
        <asp:Panel ID="pnlCaseSheet" runat="server">

            <asp:UpdatePanel ID="upRad" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Office2007" runat="server" EnableViewState="false">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow2" Skin="Office2007" CssClass="HealthCheckupPage" OpenerElementID="btnPrintPdf" runat="server" />
                        </Windows>
                    </telerik:RadWindowManager>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="WordProcessorDiv">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-lg-2 col-md-3 col-6">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="lbltitle" runat="server" Text="Discharge Summary" /></h2>
                            </div>
                        </div>
                        <div class="col-md-2 col-6 text-right">
                            <asp:CheckBox ID="chkExport" runat="server" Text="Export" AutoPostBack="true" />
                        </div>
                        <div class=" col-lg-8 col-md-7 col-sm-12 text-right">

                            <asp:LinkButton ID="btnLabResult" runat="server" CssClass="btn btn-primary" Text="Lab Results" OnClick="btnLabResult_OnClick" />
                            <asp:LinkButton ID="btnRadiology" runat="server" CssClass="btn btn-primary" Text="Radiology Results" OnClick="btnRadiology_OnClick" />
                            <asp:LinkButton ID="btnOther" runat="server" CssClass="btn btn-primary" Text="Other Results" OnClick="btnOther_OnClick" />
                            <asp:LinkButton ID="btnFollowUpappointment" runat="server" CssClass="btn btn-primary" Text="Follow-up Appointment" OnClick="btnFollowUpappointment_OnClick" />
                            <asp:LinkButton ID="lnkBtnMedication" runat="server" Font-Bold="true" CssClass="btn btn-primary" Text="Medications" OnClick="lnkBtnMedication_OnClick" Visible="true" />

                            <%-- <asp:Label ID="lblExport" runat="server" Text="Export" />--%>

                            <asp:Button ID="btnCancelSummary" Text="Cancel Summary" ToolTip="Cancel Summary" CssClass="btn btn-primary" runat="server" OnClick="btnCancelSummary_Click" />
                            <asp:Button ID="btnFinalize" runat="server" ToolTip="Finalized(Ctrl+F2)" Text="Finalized " CssClass="btn btn-primary" OnClick="btnFinalize_Click" />

                            <asp:Button ID="btnPrintPdf" runat="server" CssClass="btn btn-primary" Text="Print " ToolTip="Print(Ctrl+F9)" OnClick="btnPrintPDF_Click" Visible="false" />
                            <asp:Button ID="btnPrintPDFNew" runat="server" CssClass="btn btn-primary" Text="Preview " ToolTip="Preview(Ctrl+F9)" OnClick="btnPrintPDFNew_Click" />
                            <asp:Button ID="btnExporttoWord" runat="server" Text="Export to Word" ToolTip="Export to Word" CssClass="btn btn-primary" OnClick="btnExporttoWord_Click" Style="visibility: hidden;" Width="0px" Visible="false" />
                            <asp:Button ID="btnEnableTimer" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnEnableTimer_OnClick" />
                            <asp:Button ID="btnSave" Text="Save " ToolTip="Save Discharge Summary (Ctrl+F3)" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" OnClientClick="getHTMLCode_Click();" />
                            <asp:HiddenField ID="hdnFinalizeDeFinalize" runat="server" Value="" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" OnClientClick="window.close();" />
                            <asp:Button ID="btnRadWinClose" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnRadWinClose_OnClick" />

                            <asp:HiddenField ID="hdnReturnLab" runat="server" Value="" />
                        </div>
                    </div>
                </div>
            </div>


            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />--%>
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </div>

                </div>
            </div>


            <div class="VisitHistoryDivText">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">


                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="text-center">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" ForeColor="Green" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>


                </div>
            </div>
            <div class="EMR-HealthPreparedBorder">
                <div class="container-fluid">

                    <div class="row">
                        <div class="col-md-3">
                            <div class="EMR-HealthPrepared">
                                <h2>Prepared By :</h2>
                                <h3>
                                    <asp:Label ID="lblPreparedBy" runat="server" Text="" /></h3>
                            </div>
                        </div>



                        <div class="col-md-9">
                            <div class="row">
                                <asp:UpdatePanel ID="UpdatePanel234" runat="server" class="col-12">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkIsMultiDepartmentCase" />
                                    </Triggers>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="row">
                                                    <div class="col-4">
                                                        <asp:CheckBox ID="chkAutoSave" runat="server" ToolTip="Auto Save" Text="Auto Save" Checked="true" />
                                                    </div>
                                                    <div class="col-8">
                                                        <asp:CheckBox ID="chkIsMultiDepartmentCase" runat="server" Text="Is Multi Department Case" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkIsMultiDepartmentCase_OnCheckedChanged" /></h2>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 ">
                                                <div class="row">
                                                    <div class="col-4 " id="Td1" runat="server" visible="false">
                                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Department" />
                                                        <span style="color: Red">*</span>
                                                    </div>
                                                    <div class="col-8" id="Td2" runat="server" visible="false">
                                                        <telerik:RadComboBox ID="ddlDepartment" Width="100%" Height="300px" runat="server" Skin="Office2007" EmptyMessage="[ Select ]" MarkFirstMatch="true" CheckBoxes="true" Enabled="false" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-5 col-6">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label3" runat="server" Text="Summary Type" />
                                                        <span style="color: Red">*</span></h2>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <telerik:RadComboBox ID="ddlDepartmentCase" Width="100%" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" Enabled="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>






                        <div class="col-md-3 col-6">
                            <div class="row" id="divFormat" runat="server">
                                <div class="col-lg-3 col-md-4">
                                    <h2 style="font-size: 12px">Format</h2>
                                </div>
                                <div class="col-lg-9 col-md-8">
                                    <telerik:RadComboBox ID="ddlReportFormat" runat="server" SkinID="DropDown" Width="100%" DropDownWidth="350px" OnSelectedIndexChanged="ddlReportFormat_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="EMR-HealthCheckBox01" id="divFormatLabel" runat="server" visible="false">
                                <h2>Format:</h2>
                                <h5>
                                    <asp:Label ID="lblReportFormat" runat="server" Text="" Font-Bold="True"></asp:Label></h5>
                            </div>

                        </div>



                        <div class="col-md-3 col-6">
                            <div class="row">
                                <div class="col-md-5">
                                    <asp:Label ID="lblJuniorDoctor" runat="server" Text="JR/SR/JC Doctors"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <telerik:RadComboBox ID="ddlJuniorDoctorSign" Width="100%" DropDownWidth="250px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" />
                                </div>

                            </div>
                        </div>
                        <div class="col-md-3 col-6">
                            <div class="row">
                                <div class="col-md-5">
                                    <asp:Label ID="lblSignatoryDoctor" runat="server" Text="Signatory Doctor"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:UpdatePanel ID="UpdatePanel50" runat="server">
                                        <Triggers>
                                            <%-- <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />--%>
                                        </Triggers>
                                        <ContentTemplate>
                                            <telerik:RadComboBox ID="ddlDoctorSign" Width="100%" DropDownWidth="250px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>


                        <div class="col-md-3 col-6">
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="ltrldatetime" runat="server" Text="Date" />
                                    <span id="spnStar" runat="server" style="color: Red">*</span>
                                </div>
                                <div class="col-md-8">
                                    <asp:UpdatePanel ID="udpdateofdeath" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadDatePicker ID="dtpdate" Width="100%" runat="server" DateInput-DateFormat="dd/MM/yyyy" DateInput-DateDisplayFormat="dd/MM/yyyy" MinDate="01/01/1900">
                                                <DateInput ID="DateInput2" runat="server">
                                                    <ClientEvents OnError="ShowError" />
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:Label ID="lblTime" Width="30px" runat="server" Text="Time" Visible="false" />
                                            <telerik:RadTimePicker ID="RadTimeFrom" runat="server" AutoPostBack="True" DateInput-ReadOnly="true" OnSelectedDateChanged="RadTimeFrom_SelectedIndexChanged" PopupDirection="BottomLeft" TimeView-Columns="6" Width="95px" Visible="false" />
                                            <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Height="300px" Skin="Outlook" Width="50px" Visible="false" />
                                            <asp:Literal ID="ltDateTime" runat="server" Text="HH MM" Visible="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-12">

                            <asp:Button ID="btnRefresh" runat="server" CssClass="PatientLabBtn02" Text="Refresh" OnClick="btnRefresh_OnClick" />
                        </div>

                    </div>


                </div>
            </div>











            <table>

                <!-- Part Not used Start -->
                <tr>
                    <td>
                        <telerik:RadSpell ID="spell1" runat="server" Visible="false" ButtonType="PushButton" ControlToCheck="RTF1" DictionaryPath="~/App_Data/RadSpell" SpellCheckProvider="TelerikProvider" FragmentIgnoreOptions="All" SupportedLanguages="en-US,English,fr-FR,French,en-Custom,Custom" WordIgnoreOptions="None" />
                    </td>
                    <td align="left" style="display: none;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                            <ContentTemplate>
                                Templates
                                <telerik:RadComboBox ID="ddlTemplates" runat="server" SkinID="DropDown" EmptyMessage="Select" OnClientSelectedIndexChanged="getSelectedItemValue" EnableLoadOnDemand="true" OnClientLoad="OnClientLoad" Width="150px" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTemplates" />
                                <asp:PostBackTrigger ControlID="btnRefresh" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>

                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkDateWise" runat="server" Text="Date Wise" TextAlign="Right" Font-Bold="true" SkinID="checkbox" AutoPostBack="true" OnCheckedChanged="chkDateWise_OnCheckedChanged" />
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" MinDate="01/01/1900" TabIndex="6" Enabled="false" Width="168px" AutoPostBack="true" OnSelectedDateChanged="dtpFromDate_SelectedDateChanged">
                                    <DateInput ID="ID1" runat="server" DateFormat="dd/MM/yyyy" />
                                </telerik:RadDatePicker>
                                &nbsp;&nbsp;To&nbsp;&nbsp;
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" TabIndex="6" Enabled="false" Width="168px" AutoPostBack="true" OnSelectedDateChanged="dtpToDate_SelectedDateChanged">
                                    <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" />
                                </telerik:RadDatePicker>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="dtpFromDate" />
                                <asp:AsyncPostBackTrigger ControlID="dtpToDate" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Timer ID="Timer1" runat="server" OnTick="AotoSave_OnClick" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Timer1" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>
                <!-- Part Not used Ends -->
            </table>



            <div class="VisitHistoryDivText">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblTemp" runat="server" />


                            <div id="editor">
                                <div id='edit' style="margin-top: 30px;">
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>


            <div class="VisitHistoryDivText">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-6">
                            <div class="EMR-DischargeSumBox03">
                                <h2>
                                    <asp:Label ID="lblsynopsis" Text="Synopsis :" runat="server" /></h2>
                                <h3>
                                    <asp:TextBox ID="txtsynopsis" CssClass="DischargeTextarea" TextMode="MultiLine" MaxLength="500" runat="server" /></h3>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="EMR-DischargeSumBox03">
                                <h2>
                                    <asp:Label ID="Label1" Text="Addendum :" runat="server" /></h2>
                                <h3>
                                    <asp:TextBox ID="txtAddendum" CssClass="DischargeTextarea" TextMode="MultiLine" MaxLength="500" onkeyup="return MaxLenTxt(this, 2000);" runat="server" /></h3>
                            </div>
                        </div>

                    </div>
                </div>
            </div>


            <script type="text/javascript">

                Telerik.Web.UI.Editor.CommandList["ExportToRtf"] = function (commandName, editor, args) {
                    $get('<%=btnExporttoWord.ClientID%>').click();
                };
                Telerik.Web.UI.Editor.CommandList["ImageEditor"] = function (commandName, editor, args) {

                    var myCallbackFunction = function (sender, args) {

                        editor.pasteHtml(String.format("<table><tbody><tr><td><img src='{0}' width='250px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table>", args.image));
                    }

                    editor.showExternalDialog(
             '/Editor/ImageEditor.aspx',
            args,
             970,
            600,
            myCallbackFunction,
            null,
            'Image Editor',
            true,
            Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
            true,
            true);
                };
                Telerik.Web.UI.Editor.CommandList["MedicalIllustration"] = function (commandName, editor, args) {

                    var myCallbackFunction1 = function (sender, args) {
                        editor.pasteHtml(String.format("<table><tbody><tr><td><img src='{0}' width='250px' height='250px' border='0' align='middle' alt='Medical Illustrations' /></td></tr></tbody></table>", args.image));
                    }

                    editor.showExternalDialog(
            '/ImageEditor/Annotator.aspx?Page=I',
            args,
             970,
            600,
            myCallbackFunction1,
            null,
            'Medical Illustrations',
            true,
            Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
            true,
            true);
                };



                function checkEditor(editor, ev) {
                    var targetHeight = editor.get_document().body.scrollHeight;
                    var allowedHeight = editor.AllowedHeight;


                    if (targetHeight > allowedHeight) {
                        alert("Maximum allowed size content height is 350 pixels");

                        if (ev) {
                            ev.returnValue = false;
                            ev.cancelBubble = true;
                        }

                        return false;
                    }
                    return true;
                }

                function OnClientCommandExecuted(editor, commandName, tool) {
                    var allow = checkEditor(editor);

                    if (false == allow) {
                        editor.undo(1);
                    }
                }

                function OnClientLoad(editor) {

                    var iframe = editor.get_contentAreaElement();
                    iframe.style.height = "350px";
                    iframe.style.border = "1px solid red";

                    editor.AllowedHeight = iframe.offsetHeight;

                    var resizeFnRef = function (e) { checkEditor(editor, e) };

                    editor.attachEventHandler("keydown", resizeFnRef);
                };

                function pasteHtmlAtCaret(html, selectPastedContent) {
                    var sel, range;
                    if (window.getSelection) {
                        // IE9 and non-IE
                        sel = window.getSelection();
                        if (sel.getRangeAt && sel.rangeCount) {
                            range = sel.getRangeAt(0);
                            range.deleteContents();

                            // Range.createContextualFragment() would be useful here but is
                            // only relatively recently standardized and is not supported in
                            // some browsers (IE9, for one)
                            var el = document.createElement("div");
                            el.innerHTML = html;
                            var frag = document.createDocumentFragment(), node, lastNode;
                            while ((node = el.firstChild)) {
                                lastNode = frag.appendChild(node);
                            }
                            var firstNode = frag.firstChild;
                            range.insertNode(frag);

                            // Preserve the selection
                            if (lastNode) {
                                range = range.cloneRange();
                                range.setStartAfter(lastNode);
                                if (selectPastedContent) {
                                    range.setStartBefore(firstNode);
                                } else {
                                    range.collapse(true);
                                }
                                sel.removeAllRanges();
                                sel.addRange(range);
                            }
                        }
                    } else if ((sel = document.selection) && sel.type != "Control") {
                        // IE < 9
                        var originalRange = sel.createRange();
                        originalRange.collapse(true);
                        sel.createRange().pasteHTML(html);
                        if (selectPastedContent) {
                            range = sel.createRange();
                            range.setEndPoint("StartToStart", originalRange);
                            range.select();
                        }
                    }
                }

                function getCode_Click(LabData) {
                    document.getElementById('test').focus();
                    //var selectPastedContent = document.getElementById('selectPasted').checked;
                    pasteHtmlAtCaret(LabData, true);
                    return false;
                };
                function SetTarget() {

                    document.forms[0].target = "_blank";

                }
            </script>
            <script type="text/javascript" src="../Include/jsEditor/jquery.min.1.11.0.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/codemirror.min.5.3.0.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/xml.min.5.3.0.js"></script>

            <script type="text/javascript" src="../Include/jsEditor/froala_editor.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/align.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/char_counter.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/code_beautifier.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/code_view.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/colors.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/draggable.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/emoticons.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/entities.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/file.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/font_size.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/font_family.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/fullscreen.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/image.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/image_manager.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/line_breaker.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/inline_style.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/link.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/lists.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/paragraph_format.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/paragraph_style.min.js"></script>

            <script type="text/javascript" src="../Include/jsEditor/plugins/quote.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/table.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/save.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/url.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/video.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/help.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/print.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/third_party/spell_checker.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/special_characters.min.js"></script>
            <script type="text/javascript" src="../Include/jsEditor/plugins/word_paste.min.js"></script>
            <script src="../Include/jsEditor/plugins/lineheight/plugin.js"></script>

            <script src="../Include/jsEditor/include.js"></script>
            <script>
                $(function () {
                    $('#edit').froalaEditor()
                });
                $(document).ready(function () {

                    $('#test').spellAsYouType();

                    $(function () {
                        $('#test').spellAsYouType(defaultDictionary = 'Espanol', checkGrammar = true);
                    });
                })


                // for arrow key movement

                $('table').keydown(function (e) {
                    
                    var $table = $(this);
                    var $active = $('input:focus,select:focus', $table);
                    var $next = null;
                    var focusableQuery = 'input:visible,select:visible,textarea:visible';
                    var position = parseInt($active.closest('td').index()) + 1;
                    console.log('position :', position);
                    switch (e.keyCode) {
                        case 37: // <Left>
                            $next = $active.parent('td').prev().find(focusableQuery);
                            break;
                        case 38: // <Up>                    
                            $next = $active
                                .closest('tr')
                                .prev()
                                .find('td:nth-child(' + position + ')')
                                .find(focusableQuery)
                            ;

                            break;
                        case 39: // <Right>
                            $next = $active.closest('td').next().find(focusableQuery);
                            break;
                        case 40: // <Down>
                            $next = $active
                                .closest('tr')
                                .next()
                                .find('td:nth-child(' + position + ')')
                                .find(focusableQuery)
                            ;
                            break;
                    }
                    if ($next && $next.length) {
                        $next.focus();
                    }
                });
            </script>
            <script>
                $(function () {
                    $.FroalaEditor.DefineIcon('my_dropdown', { NAME: 'cog' });
                    $.FroalaEditor.RegisterCommand('my_dropdown', {
                        title: 'Line Height',
                        type: 'dropdown',
                        focus: false,
                        undo: false,
                        refreshAfterCallback: true,
                        options: {
                            'v1': 'Option 1',
                            'v2': 'Option 2'
                        },
                        callback: function (cmd, val) {
                            console.log(val);
                        },
                        // Callback on refresh.
                        refresh: function ($btn) {
                            console.log('do refresh');
                        },
                        // Callback on dropdown show.
                        refreshOnShow: function ($btn, $dropdown) {
                            console.log('do refresh when show');
                        }
                    });

                    $('div#froala-editor').froalaEditor({
                        toolbarButtons: ['my_dropdown']
                    })
                });


            </script>

        </asp:Panel>
    </div>




    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
        <ContentTemplate>


            <div id="DivCancelRemarks" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 7px solid #ADD8E6; border-left: 7px solid #ADD8E6; background-color: White; border-right: 7px solid #ADD8E6; border-top: 7px solid #ADD8E6; position: absolute; background-color: #FFF8DC; bottom: 0; height: 75px; left: 450px; top: 300px;">
                <table width="96%" style="margin-left: 2%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCancelRemarks" runat="server" Text="Cancel Remarks" />
                            <asp:Label ID="lblCancelRemarksStar" runat="server" Text="*" ForeColor="Red" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCancelRemarks" SkinID="textbox" Width="168px" Style="resize: none;" Height="30px" MaxLength="150" TextMode="MultiLine" runat="server" /></td>
                    </tr>

                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel Summary" ToolTip="Cancel Summary" SkinID="Button" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnCloseDiv" runat="server" Text="Close" ToolTip="Close" SkinID="Button" OnClick="btnCloseDiv_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnSummaryID" runat="server" />
            <asp:HiddenField ID="hdnTemplateData" runat="server" />
            <asp:HiddenField ID="hdnDoctorSignID" runat="server" />
            <asp:HiddenField ID="hdnSignJuniorDoctorID" runat="server" />

            <asp:HiddenField ID="hdnFinalize" runat="server" />
            <asp:HiddenField ID="hdnEncodedBy" runat="server" />
            <asp:HiddenField ID="hdnFrom" runat="server" />
            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            <asp:HiddenField ID="DischargeSummaryCode" runat="server" />
            <asp:HiddenField ID="hdnIsFinalize" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager2" Skin="Office2007" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" Skin="Office2007" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:HiddenField ID="hdnServiceId" runat="server" Value="" />
            <asp:HiddenField ID="hdnLabNo" runat="server" Value="" />
            <asp:Button ID="btnBindLabService" runat="server" CausesValidation="false" Style="visibility: hidden;"
                OnClick="btnBindLabService_OnClick" />
            <asp:Button ID="btnBindMedication" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnBindMedication_OnClick" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
        Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" />
    <asp:Button ID="btnDeFinalisationClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
        OnClick="btnDeFinalisationClose_OnClick" />
</asp:Content>
