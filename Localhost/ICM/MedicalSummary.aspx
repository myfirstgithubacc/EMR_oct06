<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="MedicalSummary.aspx.cs" Inherits="ICM_MedicalSummary" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link id="Link1" href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link id="Link2" href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .orderText
        {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }
    </style>

    <script type="text/javascript">
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
            var tool = editor.getToolByName("FontName");
            tool.set_value("Candara");
            editor.get_contentArea().style.fontFamily = 'Candara'
            editor.get_contentArea().style.fontSize = '11pt'
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
            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);



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
                    $get('<%=btnPrintPdf.ClientID%>').click();
                    break;


            }
            evt.returnValue = false;
            return false;

        }
    </script>

    <div id="dis" runat="server" width="100%" style="vertical-align: top">
        <asp:Panel ID="pnlCaseSheet" runat="server">
            <asp:UpdatePanel ID="upRad" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                        <Windows>
                            <%--<telerik:RadWindow ID="RadWindow2" CssClass="HealthCheckupPage" OpenerElementID="btnPrintPdf"
                                runat="server" />--%>
                        </Windows>
                    </telerik:RadWindowManager>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="WordProcessorDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2 col-sm-2">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="lbltitle" runat="server" Text="Medical Summary" /></h2>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnLabResult" runat="server" CssClass="PatientDisBtn02" Text="Lab Results"
                                        OnClick="btnLabResult_OnClick" />
                                    <asp:LinkButton ID="btnRadiology" runat="server" CssClass="PatientDisBtn02" Text="Radiology Results"
                                        OnClick="btnRadiology_OnClick" />
                                    <asp:LinkButton ID="btnOther" runat="server" CssClass="PatientDisBtn02" Text="Other Results"
                                        OnClick="btnOther_OnClick" />
                                    <asp:LinkButton ID="btnFollowUpappointment" runat="server" CssClass="PatientDisBtn02"
                                        Text="Follow-up Appointment" OnClick="btnFollowUpappointment_OnClick" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <asp:Button ID="btnCancelSummary" Text="Cancel Summary" ToolTip="Cancel Summary"
                                CssClass="PatientLabBtn01" runat="server" OnClick="btnCancelSummary_Click" />
                            <asp:Button ID="btnFinalize" runat="server" Text="Finalized (Ctrl+F2)" CssClass="PatientDisBtn01"
                                OnClick="btnFinalize_Click" />
                            <asp:Button ID="btnPrintPdf" runat="server" CssClass="PatientDisBtn01" Text="Print (Ctrl+F9)"
                                OnClick="btnPrintPDF_Click" />
                            <asp:Button ID="btnExporttoWord" runat="server" Text="Export to Word" ToolTip="Export to Word"
                                CssClass="PatientDisBtn01" OnClick="btnExporttoWord_Click" Style="visibility: hidden;"
                                Width="0px" Visible="false" />
                            <asp:Button ID="btnSave" Text="Save (Ctrl+F3)" ToolTip="Save Discharge Summary" CssClass="PatientDisBtn01"
                                runat="server" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClose" Text="Close (Ctrl+F8)" runat="server" ToolTip="Close" CssClass="PatientDisBtn02"
                                OnClientClick="window.close();" />
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
            <div class="EMR-HealthPreparedBorder">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="EMR-HealthPrepared">
                                <h2>
                                    Prepared By :</h2>
                                <h3>
                                    <asp:Label ID="lblPreparedBy" runat="server" Text="" /></h3>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="LabbgTopText-Message01">
                                <h2>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" ForeColor="Green" runat="server" /></ContentTemplate>
                                    </asp:UpdatePanel>
                                </h2>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="EMR-DischargeSumBox02">
                                <asp:UpdatePanel ID="UpdatePanel234" runat="server">
                                    <%-- <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkIsMultiDepartmentCase"  />
                                        </Triggers>--%>
                                    <ContentTemplate>
                                        <table cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td style="width: 230px">
                                                    <h2>
                                                        <%--<asp:CheckBox ID="chkIsMultiDepartmentCase" runat="server" Text="Is Multi Department Case" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkIsMultiDepartmentCase_OnCheckedChanged" />--%>
                                                    </h2>
                                                </td>
                                                <td id="Td1" runat="server" visible="false">
                                                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="Department" />
                                                    <span style="color: Red">*</span>
                                                </td>
                                                <td id="Td2" runat="server" visible="false">
                                                    <telerik:RadComboBox ID="ddlDepartment" Width="150px" Height="300px" runat="server"
                                                        Skin="Office2007" EmptyMessage="[ Select ]" MarkFirstMatch="true" CheckBoxes="true"
                                                        Enabled="false" />
                                                </td>
                                                <td>
                                                    <h2>
                                                        <%--<asp:Label ID="Label3" runat="server" Text="Summary Type" />--%>
                                                        <%--<span class="redStar">*</span>--%></h2>
                                                </td>
                                                <td>
                                                    <%--<telerik:RadComboBox ID="ddlDepartmentCase" Width="150px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" Enabled="false" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="EMR-HealthCheckBox01" id="divFormat" runat="server">
                                <h2>
                                    Format</h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlReportFormat" runat="server" SkinID="DropDown" Width="250px"
                                        OnSelectedIndexChanged="ddlReportFormat_SelectedIndexChanged" AutoPostBack="true" />
                                </h3>
                            </div>
                            <div class="EMR-HealthCheckBox01" id="divFormatLabel" runat="server" visible="false">
                                <h2>
                                    Format:</h2>
                                <h5>
                                    <asp:Label ID="lblReportFormat" runat="server" Text="" Font-Bold="True"></asp:Label></h5>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="EMR-HealthCheck">
                                <h2>
                                    Signatory Doctor</h2>
                                <h3>
                                    <telerik:RadComboBox ID="ddlDoctorSign" Width="140px" MarkFirstMatch="true" runat="server"
                                        SkinID="DropDown" EmptyMessage="Select" />
                                </h3>
                                <h4>
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="PatientLabBtn02" Text="Refresh"
                                        OnClick="btnRefresh_OnClick" /></h4>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="EMR-DischargeSumBox01">
                                <h2>
                                    <asp:Label ID="ltrldatetime" runat="server" Text="Date" />
                                    <span style="color: Red">*</span></h2>
                                <h3>
                                    <asp:UpdatePanel ID="udpdateofdeath" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadDatePicker ID="dtpdate" Width="100px" runat="server" DateInput-DateFormat="dd/MM/yyyy"
                                                DateInput-DateDisplayFormat="dd/MM/yyyy" MinDate="01/01/1900">
                                                <DateInput ID="DateInput2" runat="server">
                                                    <ClientEvents OnError="ShowError" />
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:Label ID="lblTime" runat="server" Text="Time" Visible="false" />
                                            <telerik:RadTimePicker ID="RadTimeFrom" runat="server" AutoPostBack="True" DateInput-ReadOnly="true"
                                                OnSelectedDateChanged="RadTimeFrom_SelectedIndexChanged" PopupDirection="BottomLeft"
                                                TimeView-Columns="6" Width="95px" Visible="false" />
                                            <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"
                                                Height="300px" Skin="Outlook" Width="50px" Visible="false" />
                                            <asp:Literal ID="ltDateTime" runat="server" Text="HH MM" Visible="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h3>
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                </div>
            </div>
            <table>
                <!-- Part Not used Start -->
                <tr>
                    <td>
                        <telerik:RadSpell ID="spell1" Visible="false" runat="server" ButtonType="PushButton"
                            ControlToCheck="RTF1" SupportedLanguages="en-US,English,fr-FR,French,en-Custom,Custom" />
                    </td>
                    <td align="left" style="display: none;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                            <ContentTemplate>
                                Templates
                                <telerik:RadComboBox ID="ddlTemplates" runat="server" SkinID="DropDown" EmptyMessage="Select"
                                    OnClientSelectedIndexChanged="getSelectedItemValue" EnableLoadOnDemand="true"
                                    OnClientLoad="OnClientLoad" Width="150px" />
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
                                <asp:CheckBox ID="chkDateWise" runat="server" Text="Date Wise" TextAlign="Right"
                                    Font-Bold="true" SkinID="checkbox" AutoPostBack="true" OnCheckedChanged="chkDateWise_OnCheckedChanged" />
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" MinDate="01/01/1900" TabIndex="6"
                                    Enabled="false" Width="168px" AutoPostBack="true" OnSelectedDateChanged="dtpFromDate_SelectedDateChanged">
                                    <DateInput ID="ID1" runat="server" DateFormat="dd/MM/yyyy" /></telerik:RadDatePicker>
                                &nbsp;&nbsp;To&nbsp;&nbsp;
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" TabIndex="6"
                                    Enabled="false" Width="168px" AutoPostBack="true" OnSelectedDateChanged="dtpToDate_SelectedDateChanged">
                                    <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" /></telerik:RadDatePicker>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="dtpFromDate" />
                                <asp:AsyncPostBackTrigger ControlID="dtpToDate" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                </tr>
                <!-- Part Not used Ends -->
            </table>
            <div class="VisitHistoryDivText">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblTemp" runat="server" />
                            <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007"
                                Width="100%" AutoResizeHeight="false" ToolsFile="~/Include/XML/DischargeSummary.xml"
                                OnClientSelectionChange="OnClientSelectionChange" OnClientLoad="OnClientEditorLoad"
                                OnClientPasteHtml="OnClientPasteHtml" EditModes="Design">
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
            <div class="VisitHistoryDivText">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="EMR-DischargeSumBox03">
                                <h2>
                                    <asp:Label ID="lblsynopsis" Text="Synopsis :" runat="server" /></h2>
                                <h3>
                                    <asp:TextBox ID="txtsynopsis" CssClass="DischargeTextarea" TextMode="MultiLine" MaxLength="500"
                                        runat="server" /></h3>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="EMR-DischargeSumBox03">
                                <h2>
                                    <asp:Label ID="Label1" Text="Addendum :" runat="server" /></h2>
                                <h3>
                                    <asp:TextBox ID="txtAddendum" CssClass="DischargeTextarea" TextMode="MultiLine" MaxLength="500"
                                        onkeyup="return MaxLenTxt(this, 2000);" runat="server" /></h3>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">

                Telerik.Web.UI.Editor.CommandList["ExportToRtf"] = function(commandName, editor, args) {
                    $get('<%=btnExporttoWord.ClientID%>').click();
                };
                Telerik.Web.UI.Editor.CommandList["ImageEditor"] = function(commandName, editor, args) {

                    var myCallbackFunction = function(sender, args) {

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
                Telerik.Web.UI.Editor.CommandList["MedicalIllustration"] = function(commandName, editor, args) {

                    var myCallbackFunction1 = function(sender, args) {
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

                    var resizeFnRef = function(e) { checkEditor(editor, e) };

                    editor.attachEventHandler("keydown", resizeFnRef);
                
            </script>

        </asp:Panel>
    </div>
    <div id="DivCancelRemarks" runat="server" visible="false" style="width: 300px; z-index: 100;
        border-bottom: 7px solid #ADD8E6; border-left: 7px solid #ADD8E6; background-color: White;
        border-right: 7px solid #ADD8E6; border-top: 7px solid #ADD8E6; position: absolute;
        background-color: #FFF8DC; bottom: 0; height: 75px; left: 450px; top: 300px;">
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
    <asp:HiddenField ID="hdnSummaryID" runat="server" />
    <asp:HiddenField ID="hdnTemplateData" runat="server" />
    <asp:HiddenField ID="hdnDoctorSignID" runat="server" />
    <asp:HiddenField ID="hdnFinalize" runat="server" />
    <asp:HiddenField ID="hdnEncodedBy" runat="server" />
    <asp:HiddenField ID="hdnFrom" runat="server" />
    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" runat="server" />
                  <telerik:RadWindow ID="RadWindow2" runat="server" />
                     <telerik:RadWindow ID="RadWindow3" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:HiddenField ID="hdnServiceId" runat="server" Value="" />
            <asp:HiddenField ID="hdnLabNo" runat="server" Value="" />
            <asp:Button ID="btnBindLabService" runat="server" CausesValidation="false" Style="visibility: hidden;"
                OnClick="btnBindLabService_OnClick" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
        Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" />
</asp:Content>
