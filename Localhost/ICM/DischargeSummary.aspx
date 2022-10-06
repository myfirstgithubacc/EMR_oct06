<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DischargeSummary.aspx.cs" Inherits="ICM_DischargeSummary" %>

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
    <%--<link href="library/styles/speech-input-sdk.css" rel="stylesheet" />--%>

    <style type="text/css">
        textarea#ctl00_ContentPlaceHolder1_txtsynopsis {
            height: 25px !important;
        }

        .redStar {
            float: none!important;
            margin: 0!important;
        }
        input#ctl00_ContentPlaceHolder1_chkIslock{
            float:none!important;
        }
        textarea#ctl00_ContentPlaceHolder1_txtAddendum {
            height: 25px !important;
        }

        #RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow2 {
            position: absolute !important;
        }

        .EMR-HealthCheck h2 {
            width: 126px !important;
        }

        .orderText {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }

        .VisitHistoryDivText div#ctl00_ContentPlaceHolder1_RTF1 {
            height: auto !important;
            min-height: inherit !important;
            overflow: auto!important;
        }

        .back_bg {
            bottom: 0 !important;
        }

        div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow2 {
            height: auto !important;
        }

        .EMR-HealthCheckBox01 h3 {
            width: 70%;
        }

        .date-col table td.rcbArrowCell {
            right: auto !important;
        }

        .EMR-HealthPrepared span {
            margin-right: 24px;
        }

        #ctl00_ContentPlaceHolder1_lblMessage {
            z-index: 999 !important;
        }
        div#ctl00_ContentPlaceHolder1_pnlCaseSheet{
            overflow-x:hidden;
        }
    </style>

    <script type="text/javascript">

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
            var editor = $find("<%=RTF1.ClientID %>");
            editor.set_mode(mode);
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
            //editor.get_contentArea().style.fontSize = '11pt'
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

            OnClientSelectionChange(oWnd, args);
            $get('<%=btnEnableTimer.ClientID%>').click();
        }
        function wndRISdata_OnClientClose(oWnd, args) {
            debugger;
            var arg = args.get_argument();
            if (arg) {

                var RisData = arg.RISData;
                var editor = $find("<%=RTF1.ClientID%>");
                editor.pasteHtml(RisData);
            }

        }

        function wndAddMedication_OnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var LabData = arg.LabData;
                $get('<%=hdnLabNo.ClientID%>').value = LabData;
            }

            $get('<%=btnBindMedication.ClientID%>').click();
            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);
            $get('<%=btnEnableTimer.ClientID%>').click();
        }

        function wndAddVital_OnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var LabData = arg.LabData;
                $get('<%=hdnLabNo.ClientID%>').value = LabData;
            }

            $get('<%=btnBindMedication.ClientID%>').click();
            var editor = $find("<%=RTF1.ClientID%>");
            editor.pasteHtml(LabData);
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
                    $get('<%=btnPrintPdf.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }

        function OnClientDeFinalisationClose(oWnd, args) {
            $get('<%=btnDeFinalisationClose.ClientID%>').click();
        }
    </script>

    <asp:HiddenField ID="hdnFontName" runat="server" />
    <asp:HiddenField ID="hdnDischargeSummaryBind" runat="server" />

    <div id="dis" runat="server" width="100%" style="vertical-align: top">
        <asp:Panel ID="pnlCaseSheet" runat="server">

            <asp:UpdatePanel ID="upRad" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Office2007" EnableViewState="false" InitialBehaviors="Maximize" Style="height: 100vh !important;">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow2" CssClass="HealthCheckupPage" Skin="Office2007" OpenerElementID="btnPrintPdf" InitialBehaviors="Maximize" runat="server" Style="height: 100vh !important;" />
                        </Windows>
                    </telerik:RadWindowManager>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />--%>
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </div>

                </div>
            </div>
            <div class="WordProcessorDiv">

                <div class="container-fluid">

                    <div class="row">

                        <div class="col-md-1 col-sm-1" style="display: none;">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="lbltitle" runat="server" Text=" " /></h2>
                            </div>
                        </div>

                        <div class="col-md-3 col-sm-12 mt-1">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>

                                    <asp:CheckBox ID="chkExport" runat="server" Text="Export To Word" AutoPostBack="true" />
                                    <asp:CheckBox ID="chkIslock" runat="server" Text="Dis.Summary Lock" AutoPostBack="true" OnCheckedChanged="chkIslock_CheckedChanged" />



                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="col-md-9 col-sm-12 text-right mt-1">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnLabResult" runat="server" CssClass="btn btn-primary mb-1" Text="Lab Results" OnClick="btnLabResult_OnClick" />
                                    <asp:LinkButton ID="btnRadiology" runat="server" CssClass="btn btn-primary" Text="RIS Results" OnClick="btnRadiology_OnClick" />
                                    <asp:LinkButton ID="btnOther" runat="server" CssClass="btn btn-primary" Text="Other Results" OnClick="btnOther_OnClick" />
                                    <asp:LinkButton ID="btnFollowUpappointment" runat="server" CssClass="btn btn-primary" Text="Follow-up Appt." OnClick="btnFollowUpappointment_OnClick" />
                                    <asp:LinkButton ID="lnkBtnMedication" runat="server" Font-Bold="true" CssClass="btn btn-primary" Text="Medications" OnClick="lnkBtnMedication_OnClick" Visible="true" />
                                    <asp:LinkButton ID="lnkVital" runat="server" Font-Bold="true" CssClass="btn btn-primary" Text="Vital" OnClick="lnkVital_OnClick" Visible="true" />
                                    <asp:Button ID="btnDiagnosticHistory" Text="DX History" ToolTip="Diagnostic History" CssClass="btn btn-primary" runat="server" OnClick="btnDiagnosticHistory_OnClick" />
                                    <asp:Button ID="btnICDCode" Text="ICD Code" ToolTip="ICD Code" CssClass="btn btn-primary" runat="server" OnClick="btnICDCode_Click" />
                                    <asp:Button ID="btnCancelSummary" Text="Cancel Summary" ToolTip="Cancel Summary" CssClass="btn btn-primary" runat="server" OnClick="btnCancelSummary_Click" />
                                    <asp:Button ID="BtnGetRISReport" runat="server" Text="RIS" CssClass="btn btn-primary" OnClick="BtnGetRISReport_Click" Visible="false" />
                                    <asp:Button ID="btnFinalize" runat="server" Text="Finalized " ToolTip="Finalized (Ctrl+F2)" CssClass="btn btn-primary" OnClick="btnFinalize_Click" />

                                    <asp:Button ID="btnPrintPdf" runat="server" CssClass="btn btn-primary" ToolTip="Print(Ctrl+F9)" Text="Print " OnClick="btnPrintPDF_Click" />
                                    <asp:Button ID="btnPrintPDFNew" runat="server" CssClass="btn btn-primary" Text="Print New" OnClick="btnPrintPDFNew_Click" Visible="false" />
                                    <asp:Button ID="btnExporttoWord" runat="server" Text="Export to Word" ToolTip="Export to Word" CssClass="btn btn-primary" OnClick="btnExporttoWord_Click" Style="visibility: hidden;" Width="0px" Visible="false" />
                                    <asp:Button ID="btnEnableTimer" runat="server" CausesValidation="false" Style="display: none" OnClick="btnEnableTimer_OnClick" />
                                    <asp:Button ID="btnSave" Text="Save " ToolTip="Save Discharge Summary(Ctrl+F3)" CssClass="btn btn-primary mb-1" runat="server" OnClick="btnSave_Click" />
                                    <asp:HiddenField ID="hdnFinalizeDeFinalize" runat="server" Value="" />
                                    <asp:Button ID="btnClose" Text="Close " runat="server" ToolTip="Close(Ctrl+F8)" Style="display: none" CssClass="btn btn-primary" OnClientClick="window.close();" />
                                    <asp:Button ID="btnRadWinClose" runat="server" CausesValidation="false" Style="display: none" OnClick="btnRadWinClose_OnClick" />
                                    <asp:HiddenField ID="hdnReturnLab" runat="server" Value="" />
                                    <asp:Button ID="btnExportToPdf" runat="server" Text="Export to PDF" ToolTip="Export to PDF" CssClass="btn btn-primary" OnClick="btnExportToPdf_Click" />
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary mb-1" Text="Refresh" OnClick="btnRefresh_OnClick" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>


                      


                    </div>
                   
                </div>
            </div>

             <div class="row" style="margin-bottom: 15px;">
                      <div class="col-md-12">
                            <div class="LabbgTopText-Message01">
                                <h2>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" ForeColor="Green" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h2>
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

                        <div class="col-md-12" style="background: antiquewhite; padding-top: 5px; margin-bottom: 5px;">
                            <div class="EMR-HealthPrepared">
                                <b>Prepared By :    </b>
                                <asp:Label ID="lblPreparedBy" runat="server" Text="" />
                                <b>Last Updated By :    </b>
                                <asp:Label ID="lblLastUpdatedBy" runat="server" Text="" />
                                <b>Last Updated Date :    </b>
                                <asp:Label ID="lblLastUpdatedDate" runat="server" Text="" />
                                <b>Discharge Type :    </b>
                                <asp:Label ID="lblDischargeType" runat="server" Text="" />
                                <asp:Label ID="lblFinalizedBy" runat="server" Text="" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="EMR-DischargeSumBox02 pull-left pr-0 pr-md-2">
                                <asp:UpdatePanel ID="UpdatePanel234" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkIsMultiDepartmentCase" />
                                    </Triggers>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td style="padding-right: 10px; width: 100px;">
                                                    <asp:CheckBox ID="chkAutoSave" runat="server" ToolTip="Auto Save" Text="Auto Save" Checked="true" />
                                                </td>
                                                <td style="width: 180px">

                                                    <h2>
                                                        <asp:CheckBox ID="chkIsMultiDepartmentCase" runat="server" Text="Is Multi Department Case" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkIsMultiDepartmentCase_OnCheckedChanged" /></h2>
                                                </td>
                                                <td id="Td1" runat="server" visible="false">
                                                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="Department" />
                                                    <span style="color: Red">*</span>
                                                </td>
                                                <td id="Td2" runat="server" visible="false">
                                                    <telerik:RadComboBox ID="ddlDepartment" Width="150px" Height="300px" runat="server" Skin="Office2007" EmptyMessage="[ Select ]" MarkFirstMatch="true" CheckBoxes="true" Enabled="false" />
                                                </td>
                                                <td style="width: 120px;">
                                                    <h2>
                                                        <asp:Label ID="Label3" runat="server" Text="Summary Type" />
                                                        <span class="redStar">*</span></h2>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlDepartmentCase" Width="150px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" Enabled="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class=" col-md-4 col-6 " id="div1" runat="server">
                                    <div class="row">
                                        <div class="col-3 ">
                                            <h2 style="font-size: 13px;">Type</h2>
                                        </div>
                                        <div class="col-8">
                                            <telerik:RadComboBox ID="ddlReportType" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Select" Value="" />
                                                    <telerik:RadComboBoxItem Text="Discharge Summary" Value="DI" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="Medical Summary" Value="CS" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class=" col-md-4 col-4" id="divFormat" runat="server">
                                    <div class="row">
                                        <div class="col-4 ">
                                            <h2 style="font-size: 13px;">Format</h2>
                                        </div>

                                        <div class="col-8">
                                            <telerik:RadComboBox ID="ddlReportFormat" runat="server" SkinID="DropDown" DropDownWidth="350px" OnSelectedIndexChanged="ddlReportFormat_SelectedIndexChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class=" col-md-4 col-4" id="divFormatLabel" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-4 ">
                                            <h2 style="font-size: 13px;">Format:</h2>
                                        </div>
                                        <div class="col-8">
                                            <asp:Label ID="lblReportFormat" runat="server" Text="" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




                    <div class="row">

                        <div class=" col-lg-3 col-md-4 col-12">
                            <div class="row">
                                <div class="col-md-5 col-4">
                                    <asp:Label ID="lblJuniorDoctor" runat="server" Text="JR/SR/JC Doctors"></asp:Label>
                                </div>
                                <div class="col-md-7 col-8">
                                    <telerik:RadComboBox ID="ddlJuniorDoctorSign" Width="100%" DropDownWidth="250px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" />
                                </div>
                            </div>
                        </div>


                        <div class=" col-lg-3 col-md-4 col-12">
                            <div class="row">
                                <div class="col-md-5 col-4">
                                    <asp:Label ID="lblSignatoryDoctor" runat="server" Text="Signatory Doctor"></asp:Label>
                                </div>
                                <div class="col-md-7 col-8">
                                    <telerik:RadComboBox ID="ddlDoctorSign" Width="100%" DropDownWidth="250px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" />
                                </div>
                            </div>
                        </div>


                        <div class=" col-lg-3 col-md-4 col-12 d-none">
                            <div class="row">
                                <div class="col-md-5 col-4">
                                    <asp:Label ID="lblSignThirdDoctor" runat="server" Text="Third Doctors" />
                                </div>
                                <div class="col-md-7 col-8">
                                    <telerik:RadComboBox ID="ddlSignThirdDoctor" Width="100%" DropDownWidth="250px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" />
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-3 col-md-4 col-12 d-none">
                            <div class="row">
                                <div class="col-md-5 col-4">
                                    <asp:Label ID="lblSignFourthDoctor" runat="server" Text="Fourth Doctors" />
                                </div>
                                <div class="col-md-7 col-8">
                                    <telerik:RadComboBox ID="ddlSignFourthDoctor" Width="100%" DropDownWidth="250px" MarkFirstMatch="true" runat="server" SkinID="DropDown" EmptyMessage="Select" />
                                </div>
                            </div>
                        </div>


                        <div class=" date-col col-lg-2 col-md-3 col-12">
                            <div class="row">
                                <div class="col-4">
                                    
                                        <asp:Label ID="ltrldatetime" runat="server" Text="Date" />
                                        <span id="spnStar" runat="server" style="color: Red">*</span>
                                </div>
                                <div class=" col-md-8 col-4">
                                    <asp:UpdatePanel ID="udpdateofdeath" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadDatePicker ID="dtpdate" Width="100%" runat="server" DateInput-DateFormat="dd/MM/yyyy" DateInput-DateDisplayFormat="dd/MM/yyyy" MinDate="01/01/1900">
                                                <DateInput ID="DateInput2" runat="server">
                                                    <ClientEvents OnError="ShowError" />
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            
                                           
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-4">
                                    <asp:Label ID="lblTime" Width="30px" runat="server" Text="Time" Visible="false" />
                                </div>
                                <div class="col-8">
                                     <telerik:RadTimePicker ID="RadTimeFrom" runat="server" AutoPostBack="True" DateInput-ReadOnly="true" OnSelectedDateChanged="RadTimeFrom_SelectedIndexChanged" PopupDirection="BottomLeft" TimeView-Columns="6" Width="95px" Visible="false" />
                                            <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Height="300px" Skin="Outlook" Width="50px" Visible="false" />
                                            <asp:Literal ID="ltDateTime" runat="server" Text="HH MM" Visible="false" />
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
            <div style="height: 200px !important;">
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
                                <%--    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                      <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                               </ContentTemplate>
                            </asp:UpdatePanel>--%>

                                <%--Added ContentAreaMode="Div" by Sanyam for Mission_10082022--%>
                                <telerik:RadEditor runat="server" ID="RTF1" OnClientPasteHtml="OnClientPasteHtml"  EnableTextareaMode="false" Skin="Office2007" Width="100%" AutoResizeHeight="false" ToolsFile="~/Include/XML/DischargeSummary.xml" OnClientSelectionChange="OnClientSelectionChange" OnClientLoad="OnClientEditorLoad">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                    </CssFiles>
                                    <%--<Languages>
                                        <telerik:SpellCheckerLanguage Code="en-US" Title="French" />
                                    </Languages>--%>
                                    <SpellCheckSettings DictionaryPath="~/App_Data/RadSpell" AjaxUrl="~/Telerik.Web.UI.SpellCheckHandler.axd" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>

                            </div>
                        </div>
                    </div>
                </div>


                <div class="VisitHistoryDivText">
                    <div class="container-fluid">
                        <div class="row">


                            <div class="mt-2 mb-2 col-md-6 col-6">
                                <div class="row">
                                    <div class="col-md-3 col-4">
                                        <asp:Label ID="lblsynopsis" Text="Synopsis :" runat="server" /></div>
                                    <div class="col-md-9 col-8">
                                        <asp:TextBox ID="txtsynopsis" CssClass="DischargeTextarea" TextMode="MultiLine" runat="server" /></h3></div>
                                </div>
                            </div>



                            <div class="mt-2 mb-2 col-md-6 col-6">
                                <div class="row">
                                    <div class="col-md-3 col-4">
                                        <asp:Label ID="Label1" Text="Addendum :" runat="server" /></div>
                                    <div class="col-md-9 col-8">
                                        <asp:TextBox ID="txtAddendum" CssClass="DischargeTextarea" TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 2000);" runat="server" /></div>
                                </div>
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

    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
        <ContentTemplate>


            <div id="divDisDateConfirm" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 7px solid #ADD8E6; border-left: 7px solid #ADD8E6; background-color: White; border-right: 7px solid #ADD8E6; border-top: 7px solid #ADD8E6; position: absolute; background-color: #FFF8DC; bottom: 0; height: 75px; left: 450px; top: 300px;">
                <table width="96%" style="margin-left: 2%;">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Do You Want To Change Date" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <asp:Button ID="btnDisDateYes" runat="server" Text="Yes" ToolTip="Change Date Yes" SkinID="Button" OnClick="btnDisDateYes_Click" />
                            <asp:Button ID="btnDisDateNo" runat="server" Text="No" ToolTip="Change Date No" SkinID="Button" OnClick="btnDisDateNo_Click" />
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
            <asp:HiddenField ID="hdnDoctorImage" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager2" Skin="Office2007" runat="server" InitialBehaviors="Maximize" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPopup" Skin="Office2007" InitialBehaviors="Maximize" runat="server" />
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
