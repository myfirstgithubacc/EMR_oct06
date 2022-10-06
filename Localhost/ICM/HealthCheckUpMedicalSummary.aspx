<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="HealthCheckUpMedicalSummary.aspx.cs" Inherits="ICM_HealthCheckUpMedicalSummary" ValidateRequest="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />

    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        .orderText {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }
    </style>

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
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }
    </script>

    <script type="text/javascript">
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
                        case "10px":
                            value = value.replace("10px", "10pt");
                            break;
                        case "11px":
                            value = value.replace("11px", "10pt");
                            break;
                        case "12px":
                            value = value.replace("12px", "10pt");
                            break;
                        case "13px":
                            value = value.replace("13px", "10pt");
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
            editor.get_contentArea().style.fontFamily = $get('<%=hdnFontName.ClientID%>').value
            editor.get_contentArea().style.fontSize = '10'
        }
    </script>

    <script language="javascript" type="text/javascript">
        function OnClientClose(oWnd, args) {

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

            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(value);

        }
        function wndAddService_OnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var ServiceId = arg.ServiceId;
                var LabData = arg.LabData;
                $get('<%=hdnServiceId.ClientID%>').value = ServiceId;
                $get('<%=hdnLabNo.ClientID%>').value = LabData;
            }
            $get('<%=btnBindLabService.ClientID%>').click();
            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);
        }

        function btnPrint_OnClick() {
            var popup;
            var ddlReportFormat = document.getElementById('<%= hdnReportId.ClientID%>').value;

            popup = window.open("/ICM/HealthCheckUpCheckList.aspx?Master=NO&PrintAllowed=1&ReportId=" + ddlReportFormat, "Popup", "height=600,width=1300,left=10,top=10, status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }
        function btnCheckListOnClick() {
            var popup;
            var ddlReportFormat = document.getElementById('<%= hdnReportId.ClientID%>').value;
            popup = window.open("/ICM/HealthCheckUpCheckList.aspx?Master=NO&PrintAllowed=0&ReportId=" + ddlReportFormat, "Popup", "height=600,width=1300,left=10,top=10, status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }

        function btnChangeDoctorNameOnClick() {
            var popup;

            popup = window.open("/EMR/ChangeDoctorInInvoice.aspx?IsEMRPopUp=1", "Popup", "height=600,width=1300,left=10,top=10, status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }

        function OnClientIsValidPasswordCancelSummary(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordCancelSummary.ClientID%>').click();

        }

    </script>


    <asp:HiddenField ID="hdnFontName" runat="server" />

    <div id="dis" runat="server" width="100%" style="vertical-align: top">

        <asp:Panel ID="pnlCaseSheet" runat="server">

            <asp:UpdatePanel ID="upRad" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow2" CssClass="HealthCheckupPage" OpenerElementID="btnPrintPdf" runat="server"></telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>

                </ContentTemplate>
            </asp:UpdatePanel>

            <div id="DivCancelRemarks" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 7px solid #ADD8E6; border-left: 7px solid #ADD8E6; background-color: White; border-right: 7px solid #ADD8E6; border-top: 7px solid #ADD8E6; position: absolute; background-color: #FFF8DC; bottom: 0; height: 75px; left: 450px; top: 300px;">
                <table width="96%" style="margin-left: 2%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCancelRemarks" runat="server" Text="Cancel Remarks" />
                            <asp:Label ID="lblCancelRemarksStar" runat="server" Text="*" ForeColor="Red" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCancelRemarks" SkinID="textbox" Width="168px" Style="resize: none;"
                                Height="30px" MaxLength="150" TextMode="MultiLine" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel Summary" ToolTip="Cancel Summary"
                                SkinID="Button" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnCloseDiv" runat="server" Text="Close" ToolTip="Close" SkinID="Button"
                                OnClick="btnCloseDiv_Click" />
                        </td>
                    </tr>
                </table>
            </div>




            <div class="WordProcessorDiv">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-2 col-sm-2">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="lbltitle" runat="server" Text="Health Check Up - Medical Summary"></asp:Label></h2>
                            </div>
                        </div>

                        <div class="col-md-6 col-sm-6">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <%-- <Triggers><asp:PostBackTrigger ControlID="btnLabResult" /></Triggers>--%>
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnFollowUpappointment" runat="server" CssClass="PatientLabBtn01" Text="Follow-up Appointment" OnClick="btnFollowUpappointment_OnClick"></asp:LinkButton>
                                    <asp:LinkButton ID="btnOther" runat="server" CssClass="PatientLabBtn01" Text="Other Results" OnClick="btnOther_OnClick"></asp:LinkButton>
                                    <asp:LinkButton ID="btnRadiology" runat="server" CssClass="PatientLabBtn01" Text="Radiology Results" OnClick="btnRadiology_OnClick" />
                                    <asp:LinkButton ID="btnLabResult" runat="server" CssClass="PatientLabBtn01" Text="Lab Results" OnClick="btnLabResult_OnClick" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="col-md-4 col-sm-4">

                            <asp:Button ID="btnFinalize" runat="server" Text="Finalized" CssClass="PatientLabBtn01" OnClick="btnFinalize_Click" />
                            <asp:Button ID="btnPrintPdf" runat="server" CssClass="PatientLabBtn01" Text="Print" OnClick="btnPrintPDF_Click" />
                            <asp:Button ID="btnSave" Text="Save (Ctrl+F3)" ToolTip="Save Discharge Summary" CssClass="PatientLabBtn01" runat="server" OnClick="btnSave_Click" CausesValidation="false" />
                            <asp:Button ID="btnCancelSummary" Text="Cancel Summary" ToolTip="Cancel Summary" CssClass="PatientLabBtn01" runat="server" OnClick="btnCancelSummary_Click" />
                            <asp:HiddenField ID="hdnMHCFinalize" runat="server" Value="" />

                            <asp:Button ID="btnExporttoWord" runat="server" Text="Export to Word" ToolTip="Export to Word" CssClass="PatientLabBtn01" OnClick="btnExporttoWord_Click" Style="display: none;" Width="0px" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" CssClass="PatientLabBtn01" OnClientClick="window.close();" />
                            <asp:HiddenField ID="hdnReturnLab" runat="server" Value="" />
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



            <div class="EMR-HealthPreparedBorder">
                <div class="container-fluid">

                    <div class="row">
                        <div class="col-md-3">
                            <div class="EMR-HealthPrepared">
                                <h2>Prepared By :</h2>
                                <h3>
                                    <asp:Label ID="lblPreparedBy" runat="server" Text=""></asp:Label></h3>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="LabbgTopText-Message">
                                <h2>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" CssClass="EMR-HealthPreparedMessage" ForeColor="Green" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h2>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-5">
                                    Review Required :
                                </div>
                                <div class="col-md-7">
                                    <asp:RadioButtonList ID="rdoIsReviewRequired" runat="server" RepeatDirection="horizontal" onclick="ReviewRequiredClicked();">
                                        <asp:ListItem Text="Yes" Value="1" />
                                        <asp:ListItem Text="No" Value="0" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-9">
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label ID="lblReviewRemarks" runat="server" Text="Review Remarks :" Style="display:none;" />
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtReviewRemarks" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                        TextMode="MultiLine" Style="display:none; min-height: 20px; max-height: 20px; min-width: 600px; max-width: 600px; background-color: #fff !important;"
                                        MaxLength="4000" onkeyup="return MaxLenTxt(this,4000);"  />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="EMR-HealthCheckBox01" id="divFormat" runat="server">
                                <h2>Format</h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlReportFormat" runat="server" SkinID="DropDown" Width="182px" OnSelectedIndexChanged="ddlReportFormat_SelectedIndexChanged" AutoPostBack="true"></telerik:RadComboBox>
                                </h3>

                            </div>

                            <div class="EMR-HealthCheckBox01" id="divFormatLabel" runat="server" visible="false">
                                <h2>Format:</h2>
                                <h5>
                                    <asp:Label ID="lblReportFormat" runat="server" Text=""></asp:Label></h5>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="EMR-HealthCheck">
                                <h2>Signatory Doctor</h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlDoctorSign" Width="140px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select"></telerik:RadComboBox>
                                </h3>
                                <h4>
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="PatientLabBtn03" OnClick="btnRefresh_OnClick" Text="Refresh" />
                                </h4>

                            </div>
                        </div>

                        <div class="col-md-5 PaddingLeftSpacing">
                            <div class="EMR-HealthCheck">
                                <h5>
                                    <asp:Label ID="ltrldatetime" runat="server" Text="Date &amp; Time"></asp:Label>
                                    <%--<span style="color: Red">*</span>--%></h5>
                                <h3>
                                    <asp:UpdatePanel ID="udpdateofdeath" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadDateTimePicker ID="dtpdate" runat="server" DateInput-DateFormat="dd/MM/yyyy hh:mm tt" DateInput-DateDisplayFormat="dd/MM/yyyy hh:mm tt" Calendar-DayNameFormat="FirstLetter" TabIndex="0" AutoPostBackControl="Both" Calendar-EnableAjaxSkinRendering="True" Width="155px" PopupDirection="BottomRight" OnSelectedDateChanged="dtpdate_SelectedDateChanged" Enabled="true"></telerik:RadDateTimePicker>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h3>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkCheckLists" runat="server" Text="Check Lists" CssClass="PatientLabBtn03" OnClientClick="btnCheckListOnClick()"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkChangeDoctorName" runat="server" CssClass="PatientLabBtn03" Text="Change Doctor Name" OnClientClick="btnChangeDoctorNameOnClick()"></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="VisitHistoryDivText">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblTemp" runat="server"></asp:Label>
                            <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007" Width="100%" Height="400px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                ToolsFile="~/Include/XML/DischargeSummary.xml" OnClientSelectionChange="OnClientSelectionChange"
                                OnClientLoad="OnClientEditorLoad" OnClientPasteHtml="OnClientPasteHtml" EditModes="All">
                                <%--OnClientCommandExecuted="OnClientCommandExecuted"--%>
                                <CssFiles>
                                    <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                </CssFiles>
                                <SpellCheckSettings AllowAddCustom="true" />
                                <ImageManager ViewPaths="~/medical_illustration" />
                            </telerik:RadEditor>
                        </div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">

                function ReviewRequiredClicked() {
                    var radios = document.getElementById('<%= rdoIsReviewRequired.ClientID %>').getElementsByTagName('input');
                    var ReviewRemarks = document.getElementById("<%=txtReviewRemarks.ClientID%>");
                    var labelReviewRemarks = document.getElementById("<%=lblReviewRemarks.ClientID%>");


                    for (i = 0; i < radios.length; i++) {
                        if (radios[i].checked) {
                            if (radios[i].value == '1') {
                                ReviewRemarks.style.display = 'block';
                                labelReviewRemarks.style.display = 'block';
                            }
                            else {
                                ReviewRemarks.style.display = 'none';
                                labelReviewRemarks.style.display = 'none';
                            }
                        }
                    }
                }

            </script>

            <script type="text/javascript">
                //<![CDATA[
                Telerik.Web.UI.Editor.CommandList["ExportToRtf"] = function (commandName, editor, args) {
                    $get('<%=btnExporttoWord.ClientID%>').click();
                };
                Telerik.Web.UI.Editor.CommandList["ImageEditor"] = function (commandName, editor, args) {
                    //var args = editor.get_html() //returns the HTML of the selection.

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
                    //var args = editor.get_html() //returns the HTML of the selection.
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
                //]]>

                // Set Editor page Size



                function MaxLenTxt(textBox, maxLength) {
                    if (parseInt(textBox.value.length) >= parseInt(maxLength)) {

                        alert("Max characters allowed are " + maxLength);

                        textBox.value = textBox.value.substr(0, maxLength);
                    }
                }

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
                    //Set content area to be one inch high
                    var iframe = editor.get_contentAreaElement();
                    iframe.style.height = "350px";
                    iframe.style.border = "1px solid red";

                    editor.AllowedHeight = iframe.offsetHeight;

                    var resizeFnRef = function (e) { checkEditor(editor, e) };

                    editor.attachEventHandler("keydown", resizeFnRef);
                }

            </script>

        </asp:Panel>
    </div>

    <asp:HiddenField ID="hndFlaglnkChangeDoctorName" runat="server" Value="" />
    <asp:HiddenField ID="hdnSummaryID" runat="server" />
    <asp:HiddenField ID="hdnTemplateData" runat="server" />
    <asp:HiddenField ID="hdnDoctorSignID" runat="server" />
    <asp:HiddenField ID="hdnFinalize" runat="server" />
    <asp:HiddenField ID="hdnEncodedBy" runat="server" />
    <asp:HiddenField ID="hdnFrom" runat="server" />
    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
    <asp:HiddenField ID="hdnReportId" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:HiddenField ID="hdnServiceId" runat="server" Value="" />
            <asp:HiddenField ID="hdnLabNo" runat="server" Value="" />
            <asp:Button ID="btnBindLabService" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnBindLabService_OnClick" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" />

    <asp:Button ID="btnIsValidPasswordCancelSummary" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnIsValidPasswordCancelSummary_OnClick" />

</asp:Content>

