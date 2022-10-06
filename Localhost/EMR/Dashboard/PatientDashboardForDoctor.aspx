<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMasterWithTopDetails.master"
    MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="PatientDashboardForDoctor.aspx.cs"
    Inherits="EMR_Dashboard_PatientDashboardForDoctor" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="/Include/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <%--<link href='https://fonts.googleapis.com/css?family=Roboto' rel='stylesheet'>--%>

    <link href="/Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="/Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <%--<script src="/Include/JS/bootstrap.min.js"></script>--%>
    <%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>--%>
    <link href="/Include/css/all.min.css" rel="Stylesheet" type="text/css" />
    <link href="library/styles/speech-input-sdk.css" rel="stylesheet" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="/Include/JS/all.min.js"></script>
    <script type="text/javascript">
        
       
        function setCustomPosition(sender, args)
        {
            //sender.moveTo(sender.get_left(), sender.get_top());
            //document.body.style.position = 'fixed';
            sender.moveTo(720, 37);
        }
        function disableBtn(btnID, newText) {
            debugger;
            var btn = document.getElementById(btnID);
            setTimeout("setImage('"+btnID+"')", 10);
            btn.disabled = true;
            btn.value = newText;
        } 
        function setImage(btnID) {

            var btn = document.getElementById(btnID);

            btn.style.background = 'url(12501270608.gif)';

        }
        $('#btnSavePlan').click(function() {
            $("#txtPlanName").focus();
        })
    </script>

    <style type="text/css">
        .fst-box {
            width: 100%;
            padding: 10px 0px 10px 10px;
        }

        .btnOnScreenStyle {
            background-color: Lime;
            color: white;
            font-weight: bold;
        }

        .Labelheader {
            color: Black;
            font-family: arial;
            height: 14px;
            font-weight: bold;
            font-size: 12px;
            background-image: url(/Images/coversheet.gif);
            background-repeat: repeat-x;
            margin-bottom: 0px;
            letter-spacing: 1px;
            vertical-align: middle;
        }

        .paraGraphtext {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            width: 300px;
        }

        .emrPart-Green {
            width: 100%;
            float: left;
            margin: 0 0 0 0;
            padding: 0 0;
            background: #337ab7;
            border: 1px solid #2e6da4;
        }

            .emrPart-Green p {
                float: left;
                margin: 0;
                background: #53656F;
                padding: 5px;
            }

            .emrPart-Green h3 {
                float: left;
                margin: 3px 17px 0 8px !important;
                padding: 0;
                font-size: 12px;
                color: white;
                font-weight: normal;
                line-height: 28px;
            }

        emrPart-Green h5 {
            line-height: 1.7em;
            padding: 0;
            margin: 0;
            margin-top: 0px;
            display: block;
            margin-top: 6px;
        }

        .emrPart-Green .Lable2 {
            font-family: Arial, helvetica, sans-serif;
            visibility: visible;
            letter-spacing: 1px;
            color: #fff;
            vertical-align: middle;
            font-size: 11px;
        }

        .td-noti-style {
            font-weight: bold;
            font-size: small;
            color: #a9a2a2;
        }

            .td-noti-style span {
                display: flex;
                font-size: smaller;
            }

        .emrPart-Green img {
            vertical-align: middle;
            /*margin-left: 33px;*/
        }

        a.tooltips {
            position: relative;
            display: inline;
        }

            a.tooltips span {
                position: absolute;
                width: 140px;
                color: #ffffff;
                background: #000000;
                height: 30px;
                line-height: 30px;
                text-align: center;
                visibility: hidden;
                border-radius: 6px;
            }

                a.tooltips span:after {
                    content: '';
                    position: absolute;
                    top: 100%;
                    left: 50%;
                    margin-left: -8px;
                    width: 0;
                    height: 0;
                    border-top: 8px solid #000000;
                    border-right: 8px solid transparent;
                    border-left: 8px solid transparent;
                }

        a:hover.tooltips span {
            visibility: visible;
            opacity: 0.8;
            bottom: 30px;
            left: 50%;
            margin-left: -76px;
            z-index: 999;
        }

        iframe[name="RadWindowForNew"] {
            overflow-y: hidden;
            height: 74vh !important;
        }

        .col-text-info {
            position: relative;
        }

            .col-text-info span {
                position: absolute;
                background: #ff0000;
                width: 95%;
                height: 1px;
                margin-top: 12px;
            }

                .col-text-info span:after {
                    content: 'Right';
                    text-align: center;
                    display: block;
                    line-height: 0;
                    background: #fff;
                    padding: 4px;
                    WIDTH: 10%;
                    MARGIN: AUTO;
                    POSITION: ABSOLUTE;
                    LEFT: 0;
                    RIGHT: 0;
                    TOP: 0;
                    BOTTOM: 0;
                }

            .col-text-info + .col-text-info span:after {
                content: 'Left';
            }
    </style>

    <style>
        .chatDateTimeUser {
            color: blue;
            font-size: 10px;
            display: block;
            text-align: left;
            color: #8a8a8a;
            font-weight: bold;
        }

        .chatDateTimeDoctor {
            text-align: right;
            display: block;
            color: #8a8a8a;
            font-weight: bold;
            font-size: 10px;
        }

        #chatbox .well {
            font-size: 13px;
        }

        div#ctl00_pd1_UpdatePanel {
            /*float: left;*/
        }
    </style>

    <style>
        .chat-box {
            box-shadow: 0px 3px 5px #a9a9a9;
            -moz-box-shadow: 0px 3px 5px #a9a9a9;
            -webkit-box-shadow: 0px 3px 5px #a9a9a9;
            border-radius: 5px;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            padding: 10px 20px;
            border-top: 2px dashed #666;
            overflow-y: scroll;
            height: 370px;
            overflow-y: auto;
            margin-bottom: 20px;
        }

        .chat-box::-webkit-scrollbar {
                display: none;
            }

        #home .table-responsive {
            height: auto !important;
            max-height: 300px;
        }

        .modal-dialog.modal-sm {
            border: 5px solid;
            border-color: #3b8a3b;
        }

        p#pMsg {
            font-size: 12px;
            color: black;
            font-weight: bold;
        }

        /* Starts Data Saved Popup */
        #msgModal .modal-body div {
            border: 2px solid green;
            display: inline-block;
            width: 60px;
            height: 60px;
            border-radius: 50%;
            line-height: 50px;
            margin-bottom: 10px;
        }


        #msgModal .modal-body .btn-link {
            position: absolute;
            color: #ff0000;
            top: -5px;
            right: -9px;
            font-size: 18px;
        }


        #msgModal.modal.fade.uhid-modal.in {
            background: rgba(0,0,0,0.7);
        }

        #msgModal.modal.fade.uhid-modal.in .modal-dialog {
                margin: 0;
                position: absolute;
                top: 50%;
                left: 50%;
                -ms-transform: translate(-50%, -50%);
                transform: translate(-50%, -50%);
                text-align: center;
                font-size: 42px;
                color: green;
                border: 0;
            }

        #msgModal .modal-body .fa-check {
            font-size: 24px;
        }

        #msgModal p#pMsg {
            color: #3b8a3b;
        }


        .msg_text {
            background: #fff !important;
            color: black !important;
        }

        div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 {
            /*width: 45vw !important;*/
            position: absolute !important;
            right: 22px !important;
            left: auto !important;
            top: 0% !important;
            margin-top: 8%;
            border: 0;
            box-shadow: 0;
            height: 84vh !important;
            z-index: 99999 !important;
        }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1.rwNormalWindow {
                width: 45vw !important;
            }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 ul.rwControlButtons {
                /*display: none;*/
            }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 ul.rwControlButtons {
                display: block !important;
            }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew iframe { /*height: 80vh !important; overflow:hidden;*/
            }

        /* Ends Data Saved Popup */

        table.rwTitlebarControls td:first-child {
            display: none;
        }

        div.RadWindow_Metro .rwTitlebarControls em {
            /*color: #000;*/
        }

        div#RAD_SLIDING_PANE_TAB_ctl00_Radslidingpane4 {
            text-overflow: ellipsis;
            overflow: hidden;
            width: 280px;
            white-space: nowrap;
        }

        .history-icon {
            background: url(../../Images/icon/history-icon.svg);
            font-size: 0;
        }

    </style>

    <script type="text/javascript">
        window.onbeforeunload = function (evt) {
            var IsUnsave = $get('<%=hdnIsTransitDataEntered.ClientID%>').value;
            //if (IsUnsave != 0) {
            //    return false;
            //}
        }

        function SetIsTransitDataEntered(CTRL) {
            debugger
            //added by bhakti 
            document.getElementById(CTRL.id).style.backgroundColor = "AntiqueWhite";
            $get('<%=hdnIsTransitDataEntered.ClientID%>').value = '1';

            var timer = $find('<%=TimerAutoSaveDataInTransit.ClientID%>');

            timer.set_interval(600000);

            var isTimerEnabled = timer.get_enabled();
            if (!isTimerEnabled) {
                timer.set_enabled(true);
                timer._startTimer();
            }

            $get('<%=hdnCurrentControlFocused.ClientID%>').value = CTRL.name;
           
        }

        function SetFocusAtEnd(id) {
            var inputField = document.getElementById(id);
            if (inputField != null && inputField.value.length != 0) {
                if (inputField.createTextRange) {
                    var FieldRange = inputField.createTextRange();
                    FieldRange.moveStart('character', inputField.value.length);
                    FieldRange.collapse();
                    FieldRange.select();
                } else if (inputField.selectionStart || inputField.selectionStart == '0') {
                    var elemLen = inputField.value.length;
                    inputField.selectionStart = elemLen;
                    inputField.selectionEnd = elemLen;
                    inputField.focus();
                }
            } else {
                inputField.focus();
            }
        }

        function MaxLenTxt(TXT, intMax) {
            if (TXT.value.length > intMax) {
                TXT.value = TXT.value.substr(0, intMax);
                alert("Maximum length is " + intMax + " characters only.");
            }
        }
        
        function saveCarePlan() {
            $get('<%=btnSaveCarePlan.ClientID%>').click();
          
        }
       
        
        function OnClientDeleteDoctorImage(oWnd, args) {
            
          
            
           
        }
        function addChiefComplaintsOnClientClose(oWnd, args) {
            
            BindPatientChifComplaints();
            $get('<%=btnAddChiefComplaintsClose.ClientID%>').click();
           
        }
        function btnBindOrderPriscriptionPlaneOfCare(oWnd, args) {
            $get('<%=btnBindOrderPriscriptionPlaneOfCare.ClientID%>').click();
           
        }
        function addEyesVitalOnClientClose(oWnd, args) {            

            $get('<%=ImageBtnEyesVital.ClientID%>').click();
           
        }

        
        function addAllergiesOnClientClose(oWnd, args) {
            // Write code
            GetPatientAllergy();
            $get('<%=btnAddAllergiesClose.ClientID%>').click();

        }
        function addVitalsOnClientClose(oWnd, args) {
            $get('<%=btnAddVitalsClose.ClientID%>').click();
        }
        function addTemplatesOnClientClose(oWnd, args) {
            $get('<%=btnAddTemplatesClose.ClientID%>').click();
        }
        function addPreviousTreatmentOnClientClose(oWnd, args) {
            $get('<%=btnPreviousTreatmentClose.ClientID%>').click();
        }
        function addNutritionalStatusOnClientClose(oWnd, args) {
            $get('<%=btnNutritionalStatusClose.ClientID%>').click();
        }
        function addPlanOfCareOnClientClose(oWnd, args) {
            $get('<%=btnPlanOfCareClose.ClientID%>').click();
        }
        function addCostAnalysisOnClientClose(oWnd, args) {
            $get('<%=btnCostAnalysisClose.ClientID%>').click();
        }
        function CopyLastPrescription(oWnd, args) {
            
            var arg = args.get_argument();
            var value = parseInt(arg.SaveToClose);
            if (value!="" && value > 0)
            {
                $get('<%=btnEnableControl.ClientID%>').click();
            }
            GetPatientDetail();
        }

        function CopyLastPrescription() {
            $get('<%=btnEnableControl.ClientID%>').click();
        }

        function addDiagnosisSerchOnClientClose(oWnd, args) {
            $get('<%=btnAddDiagnosisSerchOnClientClose.ClientID%>').click();
        }
        function addTemplatesOnClientClose_All(oWnd, args) {
            $get('<%=btnAddTemplatesClose_All.ClientID%>').click();
        }
        function addOrdersAndProceduresOnClientClose(oWnd, args) {
            BindPatientLabOrder();
            $get('<%=btnAddOrdersAndProceduresClose.ClientID%>').click();
            
        }
        function addPrescriptionsOnClientClose(oWnd, args) {
            BindPatientMedication();
            $get('<%=btnAddPrescriptionsClose.ClientID%>').click();
        }
        function addHistoryOnClientClose(oWnd, args) {
            $get('<%=btnBindhistory.ClientID%>').click();
        }

        function addPastHistoryOnClientClose(oWnd, args) {
            BindPatientPastHistory();
            $get('<%=btnBindPasthistory.ClientID%>').click();
        }

        function addProvisionalDiagnosisClose(oWnd, args) {
            $get('<%=btnProvisionalDiagnosisClose.ClientID%>').click();
        }
        function addNonDrugOrderOnClientClose(oWnd, args) {
            $get('<%=btnNonDrugOrder.ClientID%>').click();
        }

        function addFinalDiagnosisClose(oWnd, args) {
            // alert('kuldeep dia');
            BindPatientDiagnosis();
            $get('<%=btnFinalDiagnosisClose.ClientID%>').click();
        }

        function ddlBrand_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();

            $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnItemName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnAllergyType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("AllergyType") : "";

            $get('<%=hdnIsTransitDataEntered.ClientID%>').value = '1';

            var timer = $find('<%=TimerAutoSaveDataInTransit.ClientID%>');

            timer.set_enabled(true);

            timer._startTimer();

            $get('<%=hdnCurrentControlFocused.ClientID%>').value = 'ctl00$ContentPlaceHolder1$ddlBrand';
        }

        function ddlBrandOnClientDropDownClosedHandler(sender, args) {
            if (sender.get_text().trim() == "") {
                $get('<%=hdnItemId.ClientID%>').value = "";
                $get('<%=hdnItemName.ClientID%>').value = "";
            }
        }
        function addHPIClose(oWnd, args) {
            $get('<%=btnAddHPIClose.ClientID%>').click();
        }
        function CalculateBMI(ctrlFindText) {
            debugger;
            if (document.getElementById('<%=txtHeight.ClientID%>').value != '' && document.getElementById('<%=TxtWeight.ClientID%>').value != '') {
                var txtunit = document.getElementById(ctrlFindText);
                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    PageMethods.CalculateBMIAndBSA("", document.getElementById('<%=txtHeight.ClientID%>').value, document.getElementById('<%=hdnHeight.ClientID%>').value, document.getElementById('<%=TxtWeight.ClientID%>').value, document.getElementById('<%=hdnWeight.ClientID%>').value, OnSucceeded, OnFailed);
                   
                }
            }
        }
        function OnSucceeded(response) {
            if (response != undefined && response != null && response != '') {
                var response1 = response.split(',');
                document.getElementById('<%=txtBMI.ClientID%>').value = response1[0] != undefined && response1[0] != null ? response1[0] : '';
                document.getElementById('<%=hdnBMIValue.ClientID%>').value = document.getElementById('<%=txtBMI.ClientID%>').value;
                document.getElementById('<%=txtBSA.ClientID%>').value = response1[1] != undefined && response1[1] != null ? response1[1] : '';
                document.getElementById('<%=hdnBSAValue.ClientID%>').value = document.getElementById('<%=txtBSA.ClientID%>').value;
            }
        }
        function OnFailed(error) {
            debugger
            alert(error);
        }
        
    </script>
    <script type="text/javascript">
        //<![CDATA[
        /***********************************************
          
        Splitter examples
        ***********************************************/
        var isSplitterResized = false;

        var SplitterResizeModes = ['AdjacentPane', 'Proportional', 'EndPane'];
        var resizeModeInt = 0;

        /***********************************************
        Pane examples
        ***********************************************/

        //]]>
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
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                    break;
                case 115:  // F4
                    $get('<%=btnSaveAsSigned.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }
    </script>

    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog" style="z-index: 99999;">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Care Plan</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">Plan Name </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:TextBox ID="txtPlanName" CssClass="form-control" Style="height: 28px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%-- <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="row p-t-b-5">
             <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">Template Name</div>
             <div class="col-md-8 col-sm-8 col-xs-8">
                   <telerik:RadComboBox ID="ddlTemplateName" runat="server" Filter="Contains"
                                        Width="100%"  AllowCustomText="true">
                                        
                  </telerik:RadComboBox>
             </div>
                 </div>
                </div>--%>
                    </div>


                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-default" onclick="saveCarePlan(this)" data-dismiss="modal">Save</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <div class="new_class hidden">
        <a href="#" class="btn btn-danger theme1" id="theme1"></a>
        <a href="#" class="btn btn-success theme2" id="theme2"></a>
        <a href="#" class="btn btn-primary theme3" id="theme3"></a>
        <a href="#" class="btn btn-warning theme4" id="theme4"></a>
    </div>

    <div class="container-fluid emr-main-container">
        <div class="row">

            <div class="col-md-6 col-xs-6 pt-dtls-block">


                <div class="row pt-block">
                    <div class="col-md-4 hidden">
                        <div class="row">
                            <div class="col-md-3 col-xs-3 hidden">
                                <img src="/Images/PImageBackGround.gif" class="img-circle" width="50" />
                            </div>
                            <div class="col-md-12 col-xs-12">
                                <div class="visit-date">Date: 30 Oct 2018</div>
                                <%--<div class="pt-name" data-toggle="tooltip" title="Rahul Saxena">Rahul Saxena</div>--%>
                                <div class="pt-name"></div>
                                <div class="pt-dtls">male/35 Yr, DOB: 28/05/1984 ID: 100001529 | Enc #: 2609</div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-8 allery-col">

                        <h4>Allergy<a href="#" class="edit-icon pull-left" onclick="Allergies();"><img src="/Images/edit.svg" width="12" /></a></h4>


                        <ul id="ulAllergy" class="custom-scroller custom-scroller-light">
                            <%-- <li>Sulphur contain drugs</li>
                                <li>Food Additives</li>
                                <li>Cow milk/lactose intolerance</li>

                                <li>Sulphur contain drugs</li>
                                <li>Cow milk/lactose intolerance</li>--%>
                        </ul>






                    </div>
                    <div class="col-md-4 bg-danger text-danger chronics-col">

                        <div id="cronics" class="cronics-div">

                            <h3 id="hdrcronic" style="display: none; margin: 0px;">Chronic</h3>


                            <div id="nocronic" style="display: none;"><i class="fa fa-exclamation-circle"></i>No Chronic Diagnosis</div>
                        </div>
                    </div>
                </div>


                <div class="subheading_main row">


                    <asp:UpdatePanel runat="server" ID="messagePanal">
                        <ContentTemplate>
                            <div class="col-md-3">
                                <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="col-md-9 col-xs-12 text-right btn-group">
                        <input type="button" value="CheckIn" skinid="Button" class="btn btn-xs btn-default pull-right" onclick="CheckIn(1)" id="lnkCheckIn" style="margin: 0 10px 0 4px;" />


                        <asp:UpdatePanel runat="server" ID="buttonPanal">
                            <ContentTemplate>

                                <div role="group" aria-label="Button group with nested dropdown">

                                    <asp:LinkButton ID="lbtnPastClinicalNotes" Font-Bold="true" runat="server" Font-Size="Smaller"
                                        Text="Past Clinical Notes" Font-Underline="false" Font-Overline="false" OnClick="lbtnPastClinicalNotes_Click" CssClass="btn btn-links hidden" />

                                    <input type="button" value="Pre-Auth" skinid="Button" class="btn btn-xs btn-default hidden" onclick="PreAuth()" runat="server" id="lnkpreauth" />

                                    <asp:LinkButton ID="lnkIPExtension" runat="server" Text="Online Extension" OnClick="lnkIPExtension_Click" CssClass="btn btn-xs btn-default" Visible="false"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkCopyLastPrescription" runat="server"
                                        Text="Last OPD Summary" class="btn btn-xs btn-default" ToolTip="Copy Last Prescription" OnClick="lnkCopyLastPrescription_Click" Visible="false" />
                                    <asp:Button ID="btnBackToMenu" Text="Back To Menu" runat="server" SkinID="Button"
                                        Font-Size="Smaller" OnClick="btnBackToMenu_OnClick" Visible="false" CssClass="btn btn-xs btn-default" />

                                    <asp:LinkButton ID="btnAssigntoMe" runat="server" Text="Assign to me" Visible="false" CssClass="btn btn-xs btn-default" OnClick="btnAssigntoMe_Click" />

                                    <asp:LinkButton ID="lnktriageform" runat="server" Text="Triage form" OnClick="lnktriageform_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="btnDefinalise" Text="Definalized" runat="server" CssClass="btn btn-xs btn-default"
                                        OnClick="btnDefinalise_OnClick" Visible="false" />
                                    <asp:LinkButton ID="btnICCA" runat="server" CausesValidation="false" CssClass="btn btn-xs btn-default"
                                        Text="ICCA Viewer" OnClick="btnICCA_OnClick" Visible="false" />
                                    <asp:HiddenField ID="hdnButtonId" runat="server" />
                                    <asp:HiddenField ID="hdnAgeLimitImmulization" runat="server" />
                                    <asp:HiddenField ID="hdnReportContent" runat="server" />
                                    <asp:HiddenField ID="hdnDoctorImage" runat="server" />
                                    <asp:LinkButton ID="btnEnableControl" runat="server" Style="visibility: hidden;" OnClick="btnEnableControl_OnClick" />

                                    <%--change care plan--%>
                                    <asp:Button ID="btnSavePlan" Text="Save Care Plan" runat="server" CausesValidation="false"
                                        data-toggle="modal" data-target="#myModal" class="btn btn-xs btn-default" UseSubmitBehavior="false" />
                                    <%--change care plan--%>
                                    <asp:LinkButton ID="Print" Text="Print Rx" runat="server"
                                        OnClick="btnPrintReport_OnClick" class="btn btn-xs btn-default" />

                                    <asp:Button ID="btnSave" Text="Save As Draft" runat="server" CausesValidation="false"
                                        OnClick="btnSaveDashboard_OnClick" class="btn btn-xs btn-default" OnClientClick="disableBtn(this.id, 'Submitting...')" UseSubmitBehavior="false" />

                                    <asp:LinkButton ID="btnSaveAsSigned" Text="Save As Signed" runat="server" class="btn btn-xs btn-default"
                                        OnClick="btnSaveAsSigned_OnClick" Visible="false" />

                                    <asp:Button ID="btnBindOrderPriscriptionPlaneOfCare" runat="server" Text="" OnClick="btnBindOrderPriscriptionPlaneOfCare_Click" Style="visibility: hidden;" />
                                    <asp:Button ID="btnTreatmentPlan" runat="server" class="btn btn-xs btn-default" Text="TP" ToolTip="Treatment Plan" OnClick="btnTreatmentPlan_Click" />

                                    <%-- <input type="button" id="btnTreatmentPlan" onclick="TreatmentPlan();" title="Care Plan" value="Care Plan" class="btn btn-xs btn-default hidden" />--%>


                                    <asp:LinkButton ID="btnOnScreen" runat="server" CausesValidation="false" CssClass="btn btn-xs btnOnScreenStyle" BackColor="Orange" ToolTip="Preview" UseSubmitBehavior="False" OnClick="btnOnScreen_Click">ON</asp:LinkButton>

                                    <%--<input type="button" id="btnOnScreen" onclick="OpenCloseScreen();" title="On Screen" value="Off" class="btn btn-xs btn-default" />--%>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="col-md-2 text-right hidden">
                            <b style="font-size: 12px; margin-right: 10px;">Text Note</b>
                            <label class="">
                                <input type="checkbox" checked>
                                <span class="slider round"></span>
                            </label>
                        </div>
                        <a href="#" id="btn-expand-collapse"><i class="fas fa-angle-right"></i></a>
                    </div>

                </div>







                <div class="row" runat="server">
                    <ul class="col-md-12 list-inline fixed-left-icons ">
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trOpticals" id="IcOPVitals" onclick="ExpandList(this);">
                            <img src="/Images/icon/ophthalmology.png" /></a>
                        </li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_tblVitals" id="IcVitals" onclick="ExpandList(this);">
                            <img src="/Images/icon/vital-icon.svg" /></a>
                        </li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_tblChiefComplaints" id="IclChiefComplaints" onclick="ExpandList(this);">
                            <img src="/Images/icon/chief-icon.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trHistory" id="IcHisPIllness" onclick="ExpandList(this);">
                            <img src="/Images/icon/illness.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trPastHistory" id="IcPastHistory" onclick="ExpandList(this);">
                            <img src="/Images/icon/history.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trExamination" id="IcExamination" onclick="ExpandList(this);">
                            <img src="/Images/icon/examination-icon.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trOtherNotes" id="IcCareTemplate" onclick="ExpandList(this);">
                            <img src="/Images/icon/care-templates-icon.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trProvisionalDiagnosis" id="IcProvisionalDiagnosis" onclick="ExpandList(this);">
                            <img src="/Images/icon/provisional-diagnosis.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_divDiagnosisDetails" id="IcDiagnosis" onclick="ExpandList(this);">
                            <img src="/Images/icon/diagnosis.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trPlanOfCare" id="IcPlanOfCare" onclick="ExpandList(this);">
                            <img src="/Images/icon/plan-care.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trOrdersAndProcedures" id="IcOrdersandProcedures" onclick="ExpandList(this);">
                            <img src="/Images/icon/orders.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trPrescriptions" id="IcPrescriptions" onclick="ExpandList(this);">
                            <img src="/Images/icon/prescription.svg" /></a></li>
                         <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trFreeText" id="IcFreeText" onclick="ExpandList(this);">
                            <img src="../../Images/icon/remarks.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trPACTemplates" id="IcPacNotes" onclick="ExpandList(this);">
                            <img src="/Images/icon/PACNotes.png" /></a></li>

                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trOTRequest" id="IcORequest" onclick="ExpandList(this);">
                            <img src="/Images/icon/OT.png" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trFollowup" id="IcFollowup" onclick="ExpandList(this);">
                            <img src="../../Images/icon/followup.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trOTRequest" id="IcRemarks" onclick="ExpandList(this);">
                            <img src="../../Images/icon/remarks.svg" /></a></li>

                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trNonDrugOrder" id="IcOtherOrder" onclick="ExpandList(this);">
                            <img src="/Images/other-order-icon.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trPatientFamilyEducationCounseling" id="IcPatientandfamilyeducationandcounselling" onclick="ExpandList(this);">
                            <img src="/Images/icon/counselling.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trReferralsReplyToReferrals" id="IcReferralsReplytoreferrals" onclick="ExpandList(this);">
                            <img src="/Images/icon/referals.svg" /></a></li>
                        <li style="display: none;"><a href="#ctl00_ContentPlaceHolder1_trMultidisciplinaryEvaluationPlanOfCare" id="IcMultidisciplinaryEvaluationAndPlanOfCare" onclick="ExpandList(this);">
                            <img src="/Images/icon/evaluation.svg" /></a></li>
                       
                    </ul>


                </div>

                <div class="pos-fixed row">
                    <div class="head-lebel"><span>Current Visit</span></div>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="expand-main-icon">
                        <ContentTemplate>
                            <div class="col-md-3">
                                <asp:ImageButton ID="lbtnExpand" Font-Bold="true" Font-Size="Smaller" runat="server"
                                    Font-Overline="false" OnClick="lbtnExpand_Click" ImageUrl="~/Images/plus-icon.svg" data-toggle="tooltip" data-placement="left" title="Collapse All" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="row">
                    <div class="accordion-section">
                        <div class="accord-inner">
                            <div class="block-contain">

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div id="dvErdata" runat="server" visible="false" style="color: red; font-size: medium;">
                                        </div>
                                        <asp:Label ID="Label21" Text=" " runat="server" CssClass="text-center" />

                                        <div id="trOpticals" runat="server">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <div id="DivOpticals" runat="server">
                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageOPt1" runat="server" ImageUrl="~/Images/plus-icon.svg"
                                                                    ToolTip="Ophthalmology Vitals" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label10" runat="server" CssClass="Label3" Text="Ophthalmology Vitals" />
                                                                <span id="Span1" class="red" visible="false" runat="server">*</span></h3>
                                                            <asp:ImageButton ID="ImgAddVitalValue" runat="server" ImageUrl="~/Images/add.gif" CssClass="margin_Top01"
                                                                data-toggle="tooltip" title="Add Ophthalmology Vital" data-placement="left" Height="16px" Width="16px" OnClick="ImgAddVitalValue_Click" />
                                                            <asp:ImageButton ID="ImgSaveVital" runat="server" ImageUrl="~/Images/save.gif" CssClass="margin_Top01"
                                                                data-toggle="tooltip" title="Save Vital" data-placement="left" Height="16px" Width="16px" OnClick="ImgSaveVital_Click" />
                                                        </div>
                                                    </div>
                                                    <asp:Panel ID="OpticalsPanel" runat="server" Visible="false">
                                                        <div class="table-responsive" style="clear: both;">
                                                            <table style="width: 100%;" border="1" class="table table-bordered table-striped">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>VP</td>
                                                                        <td>Dist</td>
                                                                        <td>Near</td>
                                                                        <td>W/OD</td>
                                                                        <td>W/ON</td>
                                                                        <td>PIN</td>
                                                                        <td>IOP</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Right Eye</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRDist" runat="server" onClick="return validate(this))"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRNear" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRWOD" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRWON" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRPIN" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRIOP" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnReye" runat="server" Value="0"></asp:HiddenField>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Left Eye</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLDist" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLNear" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLWOD" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLWON" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLPIN" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLIOP" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnLeye" runat="server" Value="0"></asp:HiddenField>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <div id="dvAutoRefractometer">


                                                            <table class="table table-condensed" style="margin-bottom: 0;">
                                                                <tr>
                                                                    <td width="18.2%"><b style="color: #969494;">Auto Refractometer</b></td>
                                                                    <td width="42.9%" align="center" style="background: #f9f9f9; border-right: 1px solid #ccc; font-weight: bold;">Right</td>
                                                                    <td width="42%" align="center" style="background: #f9f9f9; font-weight: bold;">Left</td>
                                                                    <td align="right">
                                                                        <asp:ImageButton ID="ImageAutoRefractometer" runat="server" ImageUrl="~/Images/add.gif" CssClass="margin_Top01"
                                                                            data-toggle="tooltip" title="Add Ophthalmology Vital" data-placement="left" Height="16px" Width="16px" OnClientClick="EyeVital() ;return false" /></td>
                                                                </tr>

                                                            </table>

                                                            <asp:GridView ID="GridViewAutoRefractometer" CssClass="table table-condensed table-bordered table-striped" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-Width="18%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("DocumentDate")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Spl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblspl1" runat="server" Text='<%#Eval("Right_Spl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cyl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcyl1" runat="server" Text='<%#Eval("Right_Cyl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Axis">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblaxis1" runat="server" Text='<%#Eval("Right_Axis")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Spl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblspl2" runat="server" Text='<%#Eval("Left_Spl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cyl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcyl2" runat="server" Text='<%#Eval("Left_Cyl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Axis">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblaxis2" runat="server" Text='<%#Eval("Left_Axis")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <EmptyDataTemplate>No record found</EmptyDataTemplate>
                                                            </asp:GridView>


                                                            <table class="table table-condensed" style="margin-bottom: 0;">
                                                                <tr>
                                                                    <td width="18%"><b style="color: #969494;">Glass Detail</b></td>
                                                                    <td width="46%" align="center" style="background: #f9f9f9; border-right: 1px solid #ccc;">Right</td>
                                                                    <td width="43.6%" align="center" style="background: #f9f9f9;">Left</td>
                                                                    <td align="right">
                                                                        <asp:ImageButton ID="ImageGlassDetail" runat="server" ImageUrl="~/Images/add.gif" CssClass="margin_Top01"
                                                                            data-toggle="tooltip" title="Add Ophthalmology Vital" data-placement="left" Height="16px" Width="16px" OnClientClick="EyeVital() ;return false" /></td>
                                                                </tr>

                                                            </table>



                                                            <asp:GridView ID="GridViewGlass" CssClass="table table-condensed table-bordered table-striped" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-Width="18%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("DocumentDate")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5.5%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblheader" runat="server" Text='<%#Eval("Header")%>' Font-Bold="true"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>



                                                                    <asp:TemplateField HeaderText="Spl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblspl1" runat="server" Text='<%#Eval("Right_Spl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cyl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcyl1" runat="server" Text='<%#Eval("Right_Cyl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Axis">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblaxis1" runat="server" Text='<%#Eval("Right_Axis")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Vision">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="RV" runat="server" Text='<%#Eval("Right_Vision")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Spl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblspl2" runat="server" Text='<%#Eval("Left_Spl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cyl">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcyl2" runat="server" Text='<%#Eval("Left_Cyl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Axis">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblaxis2" runat="server" Text='<%#Eval("Left_Axis")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Vision">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LV" runat="server" Text='<%#Eval("Left_Vision")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>No record found</EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>


                                        <div id="trVitals" runat="server" class="abc">

                                            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblVitals" runat="server">

                                                        <div class="container-fluid emrPart-Green">

                                                            <p>
                                                                <asp:ImageButton ID="imgVbtnVital" runat="server" ImageUrl="~/Images/plus-icon.svg"
                                                                    ToolTip="Vitals" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                                <%--<img src="../../Images/icon/vital-icon.svg" width="16" height="16" />--%>
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label14" runat="server" SkinID="label" Text="Vitals" /></h3>
                                                            <span id="spnVitalsStar" class="red" visible="false" runat="server">*</span>

                                                            <%--                                                    <asp:ImageButton ID="imgVitalsHistory" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Vitals History" data-placement="left" Height="20px" Width="20px" OnClientClick="VitalHistory();" />--%>



                                                            <asp:ImageButton ID="imgBtnAddVitals" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Vitals" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddVitals_OnClick" />


                                                            <%--  <asp:ImageButton ID="imgBtnAddGrowthChart" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Vitals" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddGrowthChart_OnClick" CssClass="fullscreen" />--%>

                                                            <%--   <asp:ImageButton ID="imgBtnAddAdmissionAdvice" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Vitals"   data-placement="left" Height="20px" Width="20px" OnClick="lnkAddAdmissionAdvice_OnClick"  />--%>

                                                            <%--                                                              <asp:ImageButton ID="imgBtnAddReferralRequest" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Vitals" Visible="false" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtnAddReferralRequest_OnClick" />--%>

                                                            <%-- <asp:ImageButton ID="imgBtnAddCaseSheet" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Vitals" Visible="false" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtnAddCaseSheet_OnClick" />--%>




                                                            <asp:Label ID="lblVitalMessage" runat="server" Text="" CssClass="pull-right" />
                                                            <%-- <a href="#" class="btn btn-lg btn-link pull-right" title="Vitals History" data-toggle="tooltip" data-placement="left" style="color: #333;"><i class="fa fa-history" onclick=" VitalHistory();"></i></a>--%>

                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel28">
                                                                <ContentTemplate>
                                                                    <%--<i class="fa fa-history">--%>
                                                                    <asp:ImageButton ID="ImgVitalHis" runat="server" ImageUrl="~/Images/icon/history-icon.svg"
                                                                        data-toggle="tooltip" title="Vital History" data-placement="left" Style="color: #333; width: 20px !important; height: 20px !important; float: right; margin-right: 5px;" OnClick="ImgVitalHis_Click" />
                                                                    <%-- </i>--%>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>



                                                        <div class="table-responsive" style="clear: both;">
                                                            <table class="table-bordered table input-vitals">
                                                                <asp:Panel ID="pnlVitals" runat="server" ScrollBars="Auto" Style="display: inline-block; width: 100%;">

                                                                    <tr>
                                                                        <td colspan="12">
                                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:GridView ID="gvVitals" CssClass="table table-bordered table-custom vital-head" runat="server" AutoGenerateColumns="false"
                                                                                        ShowHeader="false" Width="100%" Height="100%" HeaderStyle-Height="3px" OnRowDataBound="gvVitals_RowDataBound"
                                                                                        OnRowCommand="gvVitals_OnRowCommand" Style="margin: 0;">
                                                                                        <Columns>
                                                                                            <%--   <asp:TemplateField HeaderText="Vital Date" HeaderStyle-Width="130px" HeaderStyle-ForeColor="Black" ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text='<%#Eval("Vital Date")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%--<asp:Label ID="Label29" runat="server" Text="HT" ToolTip="Height" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>

                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label29" runat="server" Text="HT" ToolTip="Height" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkHT" runat="server" CommandName="HT" AlternateText='<%#Eval("HT")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="~/Images/icon/line-graph.svg" />

                                                                                                    <asp:HiddenField ID="hdnAbNormalmin" runat="server" />
                                                                                                    <%-- <asp:HiddenField ID="hdnAbNormalmax" runat="server" Value='<%# Eval("MaxValue") %>' />--%>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%-- <asp:Label ID="Label30" runat="server" Text="WT" ToolTip="Weight" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label30" runat="server" Text="WT" ToolTip="Weight" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkWT" runat="server" CommandName="WT" AlternateText='<%#Eval("WT")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%-- <asp:Label ID="Label31" runat="server" Text="HC" ToolTip="Head Circumference" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label31" runat="server" Text="HC" ToolTip="Head Circumference" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkHC" runat="server" CommandName="HC" AlternateText='<%#Eval("HC")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label32" runat="server" Text="Temp" ToolTip="Temperature" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkT" runat="server" CommandName="T" AlternateText='<%#Eval("T")%>' Width="18"
                                                                                                        Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                    <asp:HiddenField ID="hdnT_ABNORMAL_VALUE" runat="server" Value='<%#Eval("T_ABNORMAL_VALUE")%>' />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%-- <asp:Label ID="Label33" runat="server" Text="RR" ToolTip="Respiration" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label33" runat="server" Text="RR" ToolTip="Respiration" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkR" runat="server" CommandName="R" AlternateText='<%#Eval("R")%>' Width="18"
                                                                                                        Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                    <asp:HiddenField ID="hdnR_ABNORMAL_VALUE" runat="server" Value='<%#Eval("R_ABNORMAL_VALUE")%>' />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%--<asp:Label ID="Label34" runat="server" Text="Pulse" ToolTip="Pulse" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label34" runat="server" Text="Pulse" ToolTip="Pulse" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkP" runat="server" CommandName="P" AlternateText='<%#Eval("P")%>' Width="18"
                                                                                                        Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                    <asp:HiddenField ID="hdnP_ABNORMAL_VALUE" runat="server" Value='<%#Eval("P_ABNORMAL_VALUE")%>' />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%-- <asp:Label ID="Label35" runat="server" Text="BPS" ToolTip="BP Systolic" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label35" runat="server" Text="BPS" ToolTip="BP Systolic" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkBPS" runat="server" CommandName="BPS" AlternateText='<%#Eval("BPS")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%--<asp:Label ID="Label36" runat="server" Text="BPD" ToolTip="BP Diastolic" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label36" runat="server" Text="BPD" ToolTip="BP Diastolic" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkBPD" runat="server" CommandName="BPD" AlternateText='<%#Eval("BPD")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%-- <asp:Label ID="Label37" runat="server" Text="MAC" ToolTip="Mid Arm Circumference"
                                                                                ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label37" runat="server" Text="MAC" ToolTip="Mid Arm Circumference"
                                                                                                        ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkMAC" runat="server" CommandName="MAC" AlternateText='<%#Eval("MAC")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%-- <asp:Label ID="Label38" runat="server" Text="SpO2" ToolTip="Oxygen Saturation" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label38" runat="server" Text="SpO2" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkSpO2" runat="server" CommandName="SpO2" AlternateText='<%#Eval("SpO2")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>

                                                                                                    <%--<asp:Label ID="Label39" runat="server" Text="BMI" ToolTip="Oxygen Saturation" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label39" runat="server" Text="BMI" ToolTip="Body mass index" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkBMI" runat="server" CommandName="BMI" AlternateText='<%#Eval("BMI")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <%--<asp:Label ID="Label40" runat="server" Text="BSA" ToolTip="Oxygen Saturation" ForeColor="Black" />--%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label40" runat="server" Text="BSA" ToolTip="Body surface area" ForeColor="Black" />
                                                                                                    <asp:ImageButton ID="lnkBSA" runat="server" CommandName="BSA" AlternateText='<%#Eval("BSA")%>'
                                                                                                        Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                    <asp:Repeater ID="rptPagerVitals" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkPageVitals" runat="server" Text='<%#Eval("Text")%>' Font-Bold="true"
                                                                                                CommandArgument='<%#Eval("Value")%>' Enabled='<%#Eval("Enabled")%>' OnClick="lnkPageVitals_OnClick" />
                                                                                        </ItemTemplate>
                                                                                    </asp:Repeater>
                                                                                    <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                                                                                    <asp:HiddenField ID="hdnVitalName" runat="server" />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>

                                                                        </td>
                                                                    </tr>
                                                                    <tr class="hidden">
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnHeight" Value="4" runat="server" />
                                                                            <asp:Label ID="lblHeight" Text="&nbsp;&nbsp;&nbsp;&nbsp;Height (cm)" ToolTip="Height" Font-Size="9pt"
                                                                                runat="server" /><asp:Label ID="lblHTUnit" Text="cm" Visible="false" Font-Size="7pt" SkinID="label" runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnWeight" Value="5" runat="server" />
                                                                            <asp:Label ID="lblWeight" Text="&nbsp;Weight (kg)" ToolTip="Weight" Font-Size="9pt" runat="server" />

                                                                            <asp:Label ID="lblWTUnit" Text="kg" Visible="false" Font-Size="7pt" SkinID="label" runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnHC" Value="8" runat="server" />
                                                                            <asp:Label ID="lblHeadC" Text="&nbsp;HC (cm)" ToolTip="Head Circumference" Font-Size="9pt"
                                                                                runat="server" /><asp:Label ID="lblHCUnit" Text="cm" Visible="false" Font-Size="7pt" SkinID="label" runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnTemperature" Value="1" runat="server" />
                                                                            <asp:Label ID="lblTemperature" Text="&nbsp;Temp.(C)" ToolTip="Temperature" Font-Size="9pt"
                                                                                runat="server" />
                                                                            <asp:Label ID="lblTUnit" Text="F" Font-Size="7pt" Visible="false" SkinID="label" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnRespiration" Value="2" runat="server" />
                                                                            <asp:Label ID="lblRespiration" Text="&nbsp;RR (per Min)" ToolTip="Respiration" Font-Size="9pt"
                                                                                runat="server" />

                                                                        </td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnPulse" Value="3" runat="server" />
                                                                            <asp:Label ID="lblPulse" Text="&nbsp;Pulse (per Min)" ToolTip="Pulse" Font-Size="9pt" runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnBPSystolic" Value="6" runat="server" />
                                                                            <asp:Label ID="lblBPSystolic" Text="&nbsp;BP Systolic" ToolTip="BP Systolic" Font-Size="9pt"
                                                                                runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnBPDiastolic" Value="7" runat="server" />
                                                                            <asp:Label ID="lblBPDiastolic" Text="&nbsp;BP Diastolic" ToolTip="BP Diastolic" Font-Size="9pt"
                                                                                runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnMAC" Value="9" runat="server" />
                                                                            <asp:Label ID="lblMAC" Text="&nbsp;Mid Arm Circumference" ToolTip="Mid Arm Circumference" Font-Size="9pt"
                                                                                runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnSpO2" Value="29" runat="server" />
                                                                            <asp:Label ID="lblSpO2" Text="&nbsp;SpO2" ToolTip="Oxygen Saturation" Font-Size="9pt"
                                                                                runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnBMI" Value="10" runat="server" />
                                                                            <asp:Label ID="Label1" Text="&nbsp;BMI" ToolTip="Oxygen Saturation" Font-Size="9pt"
                                                                                runat="server" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnBSA" Value="11" runat="server" />
                                                                            <asp:Label ID="Label2" Text="&nbsp;BSA" ToolTip="Oxygen Saturation" Font-Size="9pt"
                                                                                runat="server" /></td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtHeight" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtHeight" FilterType="Numbers, Custom" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TxtWeight" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                TargetControlID="TxtWeight" FilterType="Numbers, Custom" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtHC" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtHC" FilterType="Numbers, Custom" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TxtTemperature" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />

                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                TargetControlID="TxtTemperature" FilterType="Numbers, Custom" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRespiration" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                FilterType="Custom, Numbers" TargetControlID="txtRespiration" ValidChars="." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPulse" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                FilterType="Custom, Numbers" TargetControlID="txtPulse" ValidChars="." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBPSystolic" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                FilterType="Custom, Numbers" TargetControlID="txtBPSystolic" ValidChars="." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBPDiastolic" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                                FilterType="Custom, Numbers" TargetControlID="txtBPDiastolic" ValidChars="." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMAC" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                                FilterType="Custom, Numbers" TargetControlID="txtMAC" ValidChars="." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSpO2" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                                onkeydown="return SetIsTransitDataEntered(this);" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                TargetControlID="txtSpO2" FilterType="Numbers, Custom" ValidChars="." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnBMIValue" runat="server" />
                                                                            <asp:TextBox ID="txtBMI" runat="server" Width="55px" ReadOnly="true" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: LightGray; font-size: 11px;" /></td>
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnBSAValue" runat="server" />
                                                                            <asp:TextBox ID="txtBSA" runat="server" Width="55px" ReadOnly="true" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: LightGray; font-size: 11px;" /></td>

                                                                    </tr>
                                                                </asp:Panel>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trChiefComplaints" runat="server" class="abc">

                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                <ContentTemplate>

                                                    <div id="dvchfhistory" visible="false" runat="server" class="past-data-popup">

                                                        <div class="pull-left" style="font-size: 14px; padding-bottom: 10px;">Chief Complaints (Past Data)</div>
                                                        <div class="text-right">
                                                            <asp:Button ID="btnCopyselected" runat="server" CssClass="btn btn-xs btn-primary" Text="Copy Selected" OnClick="btnCopyselected_Click" />
                                                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-xs btn-primary" OnClick="btnClose_Click" />
                                                        </div>

                                                        <asp:GridView ID="gvhistoryData" CssClass="table table-bordered" runat="server" SkinID="gridview" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkselect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Visit Date" DataField="EncounterDate" />
                                                                <asp:TemplateField HeaderText="Chief Complaints">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtHistory" runat="server" Text='<%#Eval("CF") %>' ReadOnly="true"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Visit Type" DataField="Visittype" />
                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>


                                                    <div id="tblChiefComplaints" runat="server">
                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="imgbtnChiefComplaints" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                                    ToolTip="Chief Complaints" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label20" runat="server" CssClass="Label3" Text="Chief Complaints" />
                                                                <span id="spnChiefComplaintsStar" class="red" visible="false" runat="server">*</span></h3>

                                                            <asp:ImageButton ID="imgBtnAddChiefComplaints" runat="server" ImageUrl="~/Images/add.gif" CssClass="margin_Top01"
                                                                data-toggle="tooltip" title="Add Chief Complaints" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddChiefComplaints_OnClick" />

                                                            <a class="pull-right hidden" data-toggle="tooltip" data-placement="left" title="Add Chief Complaints" onclick="AddChiefComplaints();" style="cursor: pointer;">
                                                                <img src="../Images/external-link-symbol.svg" /></a>

                                                            <asp:Button ID="btnCheifComplainthistory" CssClass="btn btn-link  copy-data hidden" runat="server"
                                                                Text="Copy Past Data" OnClick="btnShowHistory_Click" />

                                                            <%-- <asp:Label ID="Label10" runat="server" SkinID="label" Text="Complaints Details" />--%>
                                                            <asp:Label ID="lblChiefMessage" runat="server" SkinID="label" Text="" />
                                                        </div>

                                                        <asp:Panel ID="pnlChiefComplaints" runat="server" Visible="true">
                                                            <div id="dvHhostory" visible="false" runat="server" class="past-data-popup">
                                                                <div class="pull-left" style="font-size: 14px; padding-bottom: 10px;">History of Present illness(Past Data)</div>

                                                                <div class="text-right">
                                                                    <asp:Button ID="btnhistoryCopy" runat="server" Text="Copy Selected" CssClass="btn btn-xs btn-primary" OnClick="btnhistoryCopy_Click" />

                                                                    <asp:Button ID="btnhistoryClose" runat="server" Text="Close" CssClass="btn btn-xs btn-primary" OnClick="btnClose_Click" />
                                                                </div>

                                                                <asp:GridView ID="gvhistoryautopop" CssClass="table table-bordered" runat="server" SkinID="gridview" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkselect" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Visit Date" DataField="EncounterDate" />
                                                                        <asp:TemplateField HeaderText="History of Present illness">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtHistory" TextMode="MultiLine" runat="server" Text='<%#Eval("History") %>' ReadOnly="true"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Visit Type" DataField="Visittype" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>

                                                            <div class="container-fluid">

                                                                <asp:Panel ID="Panel1" runat="server">

                                                                    <div class="col-md-12 bg-info diag-entry well-sm">
                                                                        <div class=" row">
                                                                            <div class="form-group col-md-6">
                                                                                <telerik:RadComboBox ID="cmbProblemName" runat="server" MaxHeight="200px" AutoPostBack="false"
                                                                                    Width="100%" EmptyMessage="Search by Text" DataTextField="ProblemDescription"
                                                                                    DataValueField="ProblemId" EnableLoadOnDemand="true" HighlightTemplatedItems="true"
                                                                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnItemsRequested="cmbProblemName_OnItemsRequested"
                                                                                    OnClientSelectedIndexChanged="cmbProblemName_OnClientSelectedIndexChanged">
                                                                                    <HeaderTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>Condition/Symptom
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                                                </td>
                                                                                                <td id="Td1" visible="false" runat="server">
                                                                                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </telerik:RadComboBox>
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <div class="check-box-div">
                                                                                    <asp:DropDownList ID="rdoDurationList" CssClass="" runat="server">
                                                                                        <asp:ListItem Text="0" Value="0" />
                                                                                        <asp:ListItem Text="1" Value="1" />
                                                                                        <asp:ListItem Text="2" Value="2" />
                                                                                        <asp:ListItem Text="3" Value="3" />
                                                                                        <asp:ListItem Text="4" Value="4" />
                                                                                        <asp:ListItem Text="5" Value="5" />
                                                                                        <asp:ListItem Text="6" Value="6" />
                                                                                        <asp:ListItem Text="7" Value="7" />
                                                                                        <asp:ListItem Text="8" Value="8" />
                                                                                        <asp:ListItem Text="9" Value="9" />
                                                                                        <asp:ListItem Text="10" Value="10" />
                                                                                        <asp:ListItem Text="11" Value="11" />
                                                                                        <asp:ListItem Text="12" Value="12" />
                                                                                        <asp:ListItem Text="13" Value="13" />
                                                                                        <asp:ListItem Text="14" Value="14" />
                                                                                        <asp:ListItem Text="15" Value="15" />
                                                                                        <asp:ListItem Text="16" Value="16" />
                                                                                        <asp:ListItem Text="17" Value="17" />
                                                                                        <asp:ListItem Text="18" Value="18" />
                                                                                        <asp:ListItem Text="19" Value="19" />
                                                                                        <asp:ListItem Text="20" Value="20" />
                                                                                        <asp:ListItem Text="21" Value="21" />
                                                                                        <asp:ListItem Text="22" Value="22" />
                                                                                        <asp:ListItem Text="23" Value="23" />
                                                                                        <asp:ListItem Text="24" Value="24" />
                                                                                    </asp:DropDownList>
                                                                                    <asp:DropDownList ID="ddlDurationType" runat="server"
                                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDurationType_SelectedIndexChanged">
                                                                                        <asp:ListItem Text="Day(s)" Value="D" />
                                                                                        <asp:ListItem Text="Week(s)" Value="W" />
                                                                                        <asp:ListItem Text="Month(s)" Value="M" />
                                                                                        <asp:ListItem Text="Year(s)" Value="Y" />
                                                                                        <asp:ListItem Text="Other(s)" Value="O" />
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group col-md-2">
                                                                                <asp:Button ID="btnAddComplaint" runat="server" CssClass="btn btn-xs btn-primary pull-right" Font-Bold="true"
                                                                                    OnClick="btnAddComplaint_Click" Text="Add" />
                                                                            </div>
                                                                        </div>

                                                                    </div>


                                                                </asp:Panel>
                                                                <div class="row form-group">
                                                                    <div class="col-md-12">
                                                                        <asp:GridView ID="gvProblemDetails" runat="server" CssClass="table table-bordered" Width="100%"
                                                                            AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="gvProblemDetails_RowDataBound"
                                                                            OnRowCommand="gvProblemDetails_RowCommand" OnPageIndexChanging="gvProblemDetails_PageIndexChanging"
                                                                            OnRowUpdating="gvProblemDetails_OnRowUpdating"
                                                                            OnRowEditing="gvProblemDetails_OnRowEditing"
                                                                            OnSelectedIndexChanged="gvProblemDetails_SelectedIndexChanged">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Chief Complaints" HeaderStyle-ForeColor="Black">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="editorProblem" runat="server" TextMode="MultiLine" Width="100%"
                                                                                            Height="20px" Style="resize: none" onkeyup="return MaxLenTxt(this,1500);" />
                                                                                        <asp:HiddenField ID="hdnProblemId" runat="server" Value='<%#Eval("Id")%>' />

                                                                                        <asp:HiddenField ID="hdnDurationID" runat="server" Value='<%#Eval("DurationID")%>' />
                                                                                        <asp:HiddenField ID="hdnDurationType" runat="server" Value='<%#Eval("DurationType")%>' />



                                                                                        <asp:HiddenField ID="hdnProblem" runat="server" Value='<%#Eval("ProblemDescription")%>' />
                                                                                        <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Is HPI" HeaderStyle-ForeColor="Black">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lblHPI" runat="server" CommandName="ISHPI" SkinID="label" Text='<%#Eval("IsHPI")%>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Duration" ItemStyle-VerticalAlign="Top" HeaderStyle-ForeColor="Black">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDuration" runat="server" SkinID="label" Text='<%#Eval("Duration")%>'
                                                                                            Visible="true" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="60px" />
                                                                                    <ItemStyle Width="60px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="50px" ItemStyle-VerticalAlign="Top"
                                                                                    HeaderStyle-ForeColor="Black">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblEntryDate" runat="server" SkinID="label" Text='<%#Eval("EntryDate")%>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Entered By" Visible="false" ItemStyle-Width="150px" ItemStyle-VerticalAlign="Top"
                                                                                    HeaderStyle-ForeColor="Black">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("EnteredBy")%>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <%--<asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                                ItemStyle-Width="10px" HeaderStyle-Width="10px" />--%>
                                                                                <asp:CommandField CausesValidation="False" SelectText="Edit" ValidationGroup="Update" ShowSelectButton="True">
                                                                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                                                </asp:CommandField>
                                                                                <asp:TemplateField ItemStyle-Width="10px" ItemStyle-VerticalAlign="Top" HeaderStyle-ForeColor="Black">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="false" CommandName="Del"
                                                                                            ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="13px" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <RowStyle Wrap="False" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <asp:TextBox ID="editorChiefComplaints" Rows="1" data-autoresize="" runat="server" TextMode="MultiLine" CssClass="textarea-custom" onkeyup="return MaxLenTxt(this,1500);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                        <asp:HiddenField ID="hdnProblemId" runat="server" />

                                                                    </div>



                                                                </div>

                                                            </div>

                                                            <asp:HiddenField ID="prblmID" runat="server" />
                                                            <asp:HiddenField ID="hdnID" runat="server" />
                                                            <div id="dvConfirmCancelOptions" runat="server" class="popup-mini">
                                                                <br />
                                                                <strong>Do you want to delete the record?</strong>
                                                                <br />
                                                                <br />
                                                                <div style="text-align: center;">
                                                                    <asp:Button ID="ButtonOk" runat="server" Text="Yes" CssClass="btn btn-primary" OnClick="ButtonOk_OnClick" />
                                                                    <asp:Button ID="ButtonCancel" runat="server" Text="No" CssClass="btn btn-primary" OnClick="ButtonCancel_OnClick" />
                                                                </div>
                                                            </div>

                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trAllergies" runat="server" class="hidden">

                                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblAllergies" runat="server">
                                                        <div class="container-fluid emrPart-Green form-group">
                                                            <p style="width: 30px;">
                                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Allergies" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label11" runat="server" SkinID="label" Text="Allergies" />
                                                                <span id="spnAllergiesStar" class="red" visible="false" runat="server">*</span>
                                                            </h3>

                                                            <asp:ImageButton ID="imgBtnAddAllergies" runat="server" ImageUrl="~/Images/add.png" Height="20px" Width="20px"
                                                                ToolTip="Add Allergies" OnClick="lnkAddAllergies_OnClick" />

                                                            <asp:Label ID="Label41" runat="server" SkinID="label" Text="" />

                                                        </div>

                                                        <div class="col-md-12">
                                                            <asp:Panel ID="pnlAllergies" runat="server">
                                                                <div id="tblAllergiesDetail" runat="server">
                                                                    <div class="col-md-2 emrSpacingLeft">
                                                                        <asp:CheckBox ID="chkNoAllergies" runat="server" CssClass="checkboxes" Text="No Allergies"
                                                                            ForeColor="Red" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkNoAllergies_OnCheckedChanged"
                                                                            onchange="return SetIsTransitDataEntered(this);" />
                                                                    </div>
                                                                    <div class="col-md-4 form-group">
                                                                        <div class="row">
                                                                            <div class="col-md-4">
                                                                                <asp:Label ID="Label8" runat="server" SkinID="label" Text="&nbsp;Allergy&nbsp;Name" />
                                                                                <span style="color: #FF0000">*</span>
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="ddlBrand" />
                                                                                    </Triggers>
                                                                                    <ContentTemplate>
                                                                                        <telerik:RadComboBox ID="ddlBrand" runat="server" HighlightTemplatedItems="true"
                                                                                            Width="100%" Height="250px" EmptyMessage="[ Search Allergy By Typing Minimum 2 Characters ]"
                                                                                            AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlBrand_OnItemsRequested"
                                                                                            Skin="Office2007" ShowMoreResultsBox="true" OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged"
                                                                                            OnClientDropDownClosed="ddlBrandOnClientDropDownClosedHandler" EnableVirtualScrolling="true">
                                                                                            <HeaderTemplate>
                                                                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td style="width: 20%" align="left">
                                                                                                            <asp:Literal ID="Literal1" runat="server" Text="Type" />
                                                                                                        </td>
                                                                                                        <td style="width: 80%" align="left">
                                                                                                            <asp:Literal ID="Literal2" runat="server" Text="Allergy Item" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td style="width: 15%" align="left">
                                                                                                            <%#DataBinder.Eval(Container, "Attributes['AllergyType']")%>
                                                                                                        </td>
                                                                                                        <td style="width: 85%" align="left">
                                                                                                            <%#DataBinder.Eval(Container, "Text")%>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                        </telerik:RadComboBox>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-4 form-group">
                                                                        <div class="row">
                                                                            <div class="col-md-4">
                                                                                <asp:Label ID="Label9" runat="server" SkinID="label" Text="&nbsp;Interaction&nbsp;Severity" />&nbsp;<span
                                                                                    style="color: #FF0000">*</span>
                                                                            </div>
                                                                            <div class="col-md-4">
                                                                                <asp:DropDownList ID="ddlAllergySeverity" runat="server" SkinID="DropDown" Width="100%"
                                                                                    onchange="return SetIsTransitDataEntered(this);">
                                                                                    <asp:ListItem Text="" Value="0" Selected="true" />
                                                                                    <asp:ListItem Text="Major" Value="1" />
                                                                                    <asp:ListItem Text="Moderate" Value="2" />
                                                                                    <asp:ListItem Text="Minor" Value="3" />
                                                                                    <asp:ListItem Text="No Allert" Value="4" />
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <asp:Label ID="lblAllergyMessage" runat="server" SkinID="label" Text="" />

                                                                    </div>


                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-md-12 form-group">
                                                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto">
                                                                <asp:TextBox ID="editorAllergy" runat="server" TextMode="MultiLine" Height="80px"
                                                                    Width="99%" Style="resize: none" Enabled="false" />
                                                            </asp:Panel>
                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trImmunisationHistory" runat="server" class="hidden">

                                            <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table7" runat="server">
                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Immunisation History" Height="16px" Enabled="false" Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label6" runat="server" SkinID="label" Text="Immunisation History" />
                                                                <span id="spnImmunisationHistory" class="red" visible="false" runat="server">*</span>
                                                            </h3>

                                                            <asp:ImageButton ID="lnkImmunisationHistory" runat="server" Height="20px" Width="20px" ImageUrl="~/Images/add.png"
                                                                ToolTip="Immunisation History" OnClick="lnkImmunisationHistory_OnClick" />

                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trHistory" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblHistory" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="imbtnHistory" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="History of Present illness" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label17" runat="server" SkinID="label" Text="History of Present illness" />
                                                                <span id="spnHistoryStar" class="red" visible="false" runat="server">*</span></h3>

                                                            <asp:ImageButton ID="ImageButton1" runat="server" CssClass="margin_Top" Height="20px" Width="20px" ImageUrl="~/Images/add.gif" data-toggle="tooltip" title="Add Templates" data-placement="left"
                                                                OnClick="lnkAddTemplatesHistory_OnClick" />

                                                            <asp:Button ID="btnShowHistory" runat="server" Text="Copy Past Data" CssClass="btn btn-link copy-data hidden" OnClick="btnShowHistory_Click" />

                                                            <asp:Label ID="lblHistoryMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="text-right" />


                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="pnlHistory" runat="server" CssClass="form-group">
                                                            <div class="fst-box col-md-12 form-group">
                                                                <asp:TextBox ID="txtWHistory" Rows="1" runat="server" data-autoresize="" TextMode="MultiLine" Text=""
                                                                    Width="100%" CssClass="textarea-custom" onkeyup="return MaxLenTxt(this,8000);" onfocus="onFocus(this)" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                <asp:HiddenField ID="hdnWRecordId" runat="server" />
                                                            </div>
                                                            <asp:Panel ID="Panel9" runat="server" CssClass="hidden">
                                                                <asp:GridView ID="gvHistory" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvHistory_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvHistory_PageIndexChanging" OnRowCancelingEdit="gvHistory_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvHistory_OnRowUpdating" OnRowEditing="gvHistory_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="History of Present illness" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorHistory" runat="server" TextMode="MultiLine" Width="100%" Height="50px"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>


                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trPastHistory" runat="server" visible="false">

                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table13" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Past History" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label42" runat="server" SkinID="label" Text="Past History" />
                                                                <span id="spnPastHistory" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:ImageButton ID="lnkPastHistory" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px" data-toggle="tooltip" title="Past History" data-placement="left"
                                                                OnClick="lnkPastHistory_OnClick" />
                                                            <asp:Label ID="lblPHistoryMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>

                                                        <%--<div class="clearfix"></div>--%>




                                                        <asp:Panel ID="Panel23" runat="server">

                                                            <div class="fst-box col-md-12 form-group">
                                                                <asp:TextBox ID="txtPHistory" Rows="1" runat="server" TextMode="MultiLine"
                                                                    Width="100%" Style="resize: none" data-autoresize="" CssClass="textarea-custom" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />

                                                                <asp:HiddenField ID="hdnPRecordId" runat="server" />

                                                                <asp:GridView ID="gvPHistory" CssClass="table table-bordered hidden" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvPHistory_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvPHistory_PageIndexChanging" OnRowCancelingEdit="gvPHistory_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvPHistory_OnRowUpdating" OnRowEditing="gvPHistory_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Past History" HeaderStyle-ForeColor="black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorHistory" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" Height="75px" />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" HeaderStyle-ForeColor="black" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>


                                                        </asp:Panel>

                                                        <%--</table>--%>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div id="trExamination" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblExamination" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="imgbtnTemplate" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Template" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label19" runat="server" SkinID="label" Text="Examination" />
                                                                <span id="spnExaminationStar" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:ImageButton ID="imgBtnTemplates" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Templates" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddTemplates_OnClick" />
                                                            <%--<asp:Button ID="btnCopyExamination" runat="server" CssClass="btn btn-primary margin_Top" 
                                        Text="Copy Past Data" OnClick="btnCopyExamination_Click" />--%>
                                                            <asp:Label ID="lblExamMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="Panel10" runat="server">
                                                            <div class="fst-box col-md-12 form-group">
                                                                <asp:TextBox ID="txtWExamination" Rows="1" runat="server" TextMode="MultiLine" onfocus="onFocus(this)" Width="100%" Style="resize: none" data-autoresize="" CssClass="textarea-custom" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                <asp:HiddenField ID="hdnWEmRecordId" runat="server" Value='<%#Eval("RecordId")%>' />

                                                            </div>

                                                            <asp:Panel ID="pnlExamination" runat="server" CssClass="hidden">
                                                                <asp:GridView ID="gvExamination" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvExamination_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvExamination_PageIndexChanging" OnRowCancelingEdit="gvExamination_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvExamination_OnRowUpdating" OnRowEditing="gvExamination_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Examination" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorExamination" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" Height="75px" />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>

                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trNutritionalStatus" runat="server" class="hidden">

                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblNutritionalStatus" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ibtNutritionalStatus" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Nutritional Status" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label23" runat="server" SkinID="label" Text="Nutritional Status" />
                                                                <span id="spnNutritionalStatus" class="red" visible="false" runat="server">*</span>

                                                            </h3>
                                                            <asp:Label ID="lblNutritionalStatusMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />

                                                        </div>

                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="Panel6" runat="server">
                                                            <div class="fst-box col-md-12 form-group">
                                                                <asp:TextBox ID="txtWNutritionalStatus" runat="server" TextMode="MultiLine"
                                                                    Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                            </div>
                                                            <asp:Panel ID="Panel16" runat="server">
                                                                <asp:GridView ID="gvNutritional" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvNutritional_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvNutritional_PageIndexChanging" OnRowCancelingEdit="gvNutritional_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvNutritional_OnRowUpdating" OnRowEditing="gvNutritional_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Nutritional Status" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorNutritional" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>

                                                        </asp:Panel>


                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trPreviousTreatment" runat="server" visible="false" class="hidden">


                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblPreviousTreatment" runat="server">
                                                        <div class="container-flud emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Previous Treatment" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label22" runat="server" SkinID="label" Text="Previous Treatment" />
                                                                <span id="spnPreviousTreatment" class="red" visible="false" runat="server">*</span>
                                                            </h3>

                                                            <asp:Label ID="lblPrevTreatmentMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                                                        </div>
                                                        <div class="clearfix"></div>

                                                        <asp:Panel ID="Panel3" runat="server" CssClass="form-group">
                                                            <div class="fst-box col-md-12 form-group">
                                                                <asp:TextBox ID="txtWPrevTreatment" runat="server" TextMode="MultiLine" Height="135px"
                                                                    Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                            </div>
                                                            <asp:Panel ID="Panel18" runat="server">
                                                                <asp:GridView ID="gvPrevTreatment" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvPrevTreatment_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvPrevTreatment_PageIndexChanging" OnRowCancelingEdit="gvPrevTreatment_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvPrevTreatment_OnRowUpdating" OnRowEditing="gvPrevTreatment_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Previous Treatment" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorPrevTreatment" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" Height="75px" />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>

                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trCostAnalysis" runat="server" visible="true" class="hidden">

                                            <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblCostAnalysis" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Cost Analysis" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label25" runat="server" SkinID="label" Text="Cost Analysis" />
                                                                <span id="spnCostAnalysis" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:Label ID="lblCostAnalysisMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>

                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="Panel14" runat="server">
                                                            <div class="fst-box col-md-12">
                                                                <asp:TextBox ID="txtWCostAnalysis" runat="server" TextMode="MultiLine"
                                                                    Width="100%" Style="resize: none;" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                            </div>
                                                            <asp:Panel ID="Panel19" runat="server">
                                                                <asp:GridView ID="gvCostAnalysis" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvCostAnalysis_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvCostAnalysis_PageIndexChanging" OnRowCancelingEdit="gvCostAnalysis_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvCostAnalysis_OnRowUpdating" OnRowEditing="gvCostAnalysis_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Cost Analysis" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorCostAnalysis" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trOtherNotes" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblOtherNotes" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="imgbtntherNotes" runat="server" ImageUrl="~/Images/plus-icon.svg"
                                                                    ToolTip="Care Templates" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label7" runat="server" SkinID="label" Text="Care Templates&nbsp;" /></h3>
                                                            <asp:ImageButton ID="lnkAddTemplates_All" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Templates" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddTemplates_All_OnClick" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="pnlOtherNotes" runat="server">
                                                            <asp:GridView ID="gvOtherNotes" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                                ShowHeader="true" Width="100%" Height="100%" AllowPaging="true" PageSize="10"
                                                                OnPageIndexChanging="gvOtherNotes_PageIndexChanging" OnRowDataBound="gvOtherNotes_OnDataBinding">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Template Name" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                            <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                            <asp:HiddenField ID="hdnTemplateType" runat="server" Value='<%#Eval("TemplateType")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-ForeColor="Black" HeaderText="View">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="View" Text="View" OnClick="lnkView_OnClik" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-ForeColor="Black" HeaderText="Edit">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnlEdit" runat="server" Text="Edit" OnClick="lnlEdit_OnClik" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Entered By" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                                        HeaderStyle-ForeColor="Black" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trProvisionalDiagnosis" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblProvisionalDiagnosis" runat="server">

                                                        <div class="emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="imgbtnProvisionalDiagnosies" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Provisional Diagnosis" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label15" runat="server" SkinID="label" Text="Diagnosis" />
                                                                <span id="spnProvisionalDiagnosisStar" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:ImageButton ID="imgBtnProvisionalDiagnosis" runat="server" ImageUrl="~/Images/add.gif"
                                                                data-toggle="tooltip" title="Add Provisional Diagnosis" data-placement="left" Height="20px" Width="20px" OnClick="imgBtnProvisionalDiagnosis_OnClick" />
                                                            <asp:Label ID="lblProvDiag" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>

                                                        <div class="clearfix"></div>


                                                        <asp:Panel ID="pnlProvisionalDiagnosis" runat="server">
                                                            <div class="container-fluid margin_Top ">
                                                                <div class="row">

                                                                    <div class="col-md-5" style="display: none;">
                                                                        <div class="row form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label12" runat="server" Text="Search Keyword" SkinID="label" />&nbsp;<span
                                                                                    style="color: #FF0000">*</span>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:DropDownList ID="ddlDiagnosisSearchCodes" runat="server" SkinID="DropDown" Width="120px"
                                                                                    DropDownWidth="250px" onchange="return SetIsTransitDataEntered(this);" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <div class="row form-group">
                                                                            <div class="col-md-5" style="display: none;">
                                                                                <asp:LinkButton ID="btnAddDiag" runat="server" SkinID="Button" Text="Add New Search Keyword"
                                                                                    CausesValidation="false" OnClick="btnAddDiag_OnClick" />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:TextBox ID="editorProvDiagnosis" Rows="1" data-autoresize="" runat="server" TextMode="MultiLine" class="textarea-custom" onkeyup="return MaxLenTxt(this,1500);" onkeydown="return SetIsTransitDataEntered(this);"></asp:TextBox>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 box-col-checkbox check-top-margin" style="font-weight: normal!important; font-size: 11px!important;">
                                                                        <%--    <asp:CheckBox ID="ProvchkPrimarys" runat="server" Text="Primary" />

                                                                    <asp:CheckBox ID="ProvchkChronics" runat="server" Text="Chronic" />--%>


                                                                        <asp:CheckBox ID="ProvchkQuery" runat="server" Text="Provisional" AutoPostBack="true" OnCheckedChanged="ProvchkQuery_CheckedChanged" />

                                                                        <%--<asp:CheckBox ID="ProvchkResolve" runat="server" Text="Resolved" />--%>

                                                                        <asp:CheckBox ID="ProvchkIsFinalDiagnosis" runat="server" Text="Final Diagnosis" AutoPostBack="true" OnCheckedChanged="ProvchkIsFinalDiagnosis_CheckedChanged" />
                                                                    </div>
                                                                </div>

                                                            </div>




                                                            <div class="container-fluid">
                                                                <asp:GridView ID="gvData" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                                    AllowPaging="false" PageSize="1" Width="100%" Height="100%" OnRowCommand="gvData_OnRowCommand"
                                                                    OnRowDataBound="gvData_RowDataBound" OnRowCancelingEdit="gvData_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvData_OnRowUpdating" OnRowEditing="gvData_OnRowEditing"
                                                                    OnPageIndexChanging="gvData_PageIndexChanging" ShowHeaderWhenEmpty="true">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Diagnosis" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorProvisionalDiagnosis" runat="server" TextMode="MultiLine"
                                                                                    Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,500);" />
                                                                                <asp:HiddenField ID="hdnProvisionalDiagnosis" runat="server" Value='<%#Eval("ProvisionalDiagnosis")%>' />
                                                                                <asp:HiddenField ID="hdnProvisionalDiagnosisID" runat="server" Value='<%#Eval("Id")%>' />
                                                                                <asp:HiddenField ID="hdnDiagnosisSearchId" runat="server" Value='<%#Eval("SearchKeyWordId")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Type" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnIsProvisonal" runat="server" Value='<%#Eval("Provisional")%>' />
                                                                                <asp:HiddenField ID="hdnIsFinal" runat="server" Value='<%#Eval("Final")%>' />

                                                                                <asp:DropDownList ID="ddlProvType" runat="server">
                                                                                    <asp:ListItem Text="Provisional" Value="P"></asp:ListItem>
                                                                                    <asp:ListItem Text="Final" Value="F"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%-- <asp:TemplateField HeaderText="IsProvisional" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkProvisional" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="IsFinal" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkFinal" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>--%>
                                                                        <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top"
                                                                            HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" HeaderText="Entered By" ItemStyle-VerticalAlign="Top"
                                                                            HeaderStyle-ForeColor="Black" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                        <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                                    ToolTip="Delete" Width="13px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>Record not found</EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </div>

                                                            <div class="pagination-ui">
                                                                <asp:Repeater ID="rptPageProvisionalDiagnosis" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPageProvisionalDiagnosis" runat="server" Text='<%#Eval("Text")%>'
                                                                            Font-Bold="true" CommandArgument='<%#Eval("Value")%>' Enabled='<%#Eval("Enabled")%>' />
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>

                                                            <div id="dvConfirmCancelOptionsProvisionalDiag" runat="server" style="width: 300px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 100px; left: 600px; top: 200px; text-align: center;">
                                                                <br />
                                                                <strong>Delete?</strong>
                                                                <br />
                                                                <br />
                                                                <div style="text-align: center;">
                                                                    <asp:Button ID="ButtonOkProvisionalDiag" runat="server" CssClass="btn btn-primary" Text="Yes" OnClick="ButtonOkProvisionalDiag_OnClick" />
                                                                    <asp:Button ID="ButtonCancelProvisionalDiag" runat="server" CssClass="btn btn-primary" Text="No" OnClick="ButtonCancelProvisionalDiag_OnClick" />
                                                                </div>
                                                                <br />
                                                                <br />
                                                            </div>

                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div class="emrPart" id="divDiagnosisDetails" runat="server">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table11" runat="server" class="container-fluid emrPart-Green">
                                                        <p>
                                                            <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="~/Images/Expand.png"
                                                                ToolTip="Final Diagnosis" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                        </p>
                                                        <h3>
                                                            <asp:Label ID="Label4" runat="server" SkinID="label" Text="Diagnosis With ICD Code" /></h3>
                                                        <asp:ImageButton ID="imgBtnFinalDiagnosis" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px"
                                                            data-toggle="tooltip" title="Final Diagnosis" data-placement="left" OnClick="imgBtnFinalDiagnosis_OnClick" />
                                                    </div>



                                                    <asp:Panel ID="pnlDiagnosis" runat="server">

                                                        <div class="col-md-12 bg-info diag-entry well-sm">
                                                            <telerik:RadComboBox ID="ddlDiagnosiss" runat="server" Height="300px" Width="100%"
                                                                AutoPostBack="false" EmptyMessage="Search Diagnosis by Text"
                                                                DataTextField="DISPLAY_NAME" DataValueField="DiagnosisId" EnableLoadOnDemand="true"
                                                                HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                                OnItemsRequested="ddlDiagnosiss_OnItemsRequested"
                                                                OnClientSelectedIndexChanged="ddlDiagnosiss_OnClientSelectedIndexChanged"
                                                                EnableVirtualScrolling="true">
                                                                <HeaderTemplate>
                                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>Diagnosis Display Name
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                                            </td>
                                                                            <td id="Td1" visible="false" runat="server">
                                                                                <%# DataBinder.Eval(Container, "Attributes['ICDID']")%>
                                                                            </td>
                                                                            <td id="Td2" visible="false" runat="server">
                                                                                <%# DataBinder.Eval(Container, "Attributes['ICDCode']")%>
                                                                            </td>
                                                                            <td id="Td3" visible="false" runat="server">
                                                                                <%# DataBinder.Eval(Container, "Attributes['ICDDescription']")%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>

                                                            </telerik:RadComboBox>
                                                            <asp:HiddenField ID="hdnDiagnosisId" runat="server" />

                                                            <asp:TextBox ID="txtIcdCodes" runat="server" ReadOnly="true" CssClass="Textbox" Width="80px" placeholder="ICD CODE" />
                                                            <div class="check-box-div">
                                                                <asp:CheckBox ID="chkPrimarys" runat="server" Text="Primary" />

                                                                <asp:CheckBox ID="chkChronics" runat="server" Text="Chronic" />

                                                                <asp:CheckBox ID="chkQuery" runat="server" Text="P" Style="display: none;" />

                                                                <asp:CheckBox ID="chkResolve" runat="server" Text="R" Style="display: none;" />

                                                                <asp:CheckBox ID="chkIsFinalDiagnosis" runat="server" Text="FD" Style="display: none;" />
                                                                <asp:Button ID="btnCommonOrder" runat="server" Style="visibility: hidden; display: none"
                                                                    Width="1px" />
                                                            </div>


                                                            
                                                            <asp:Button ID="btnAddtogrid" runat="server" CssClass="btn btn-xs btn-primary pull-right" Font-Bold="true"
                                                                OnClick="btnAddtogrid_Click" Text="Add" />
                                                            <asp:TextBox ID="txtid" runat="server" Style="visibility: hidden; position: absolute;" />
                                                            <asp:TextBox ID="txtIcdId" runat="server" Style="visibility: hidden; position: absolute;" />
                                                        </div>


                                                      
                                                        <asp:GridView ID="gvDiagnosisDetails" runat="server" AutoGenerateColumns="false"
                                                            HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDiagnosisDetails_RowCommand"
                                                            OnRowDataBound="gvDiagnosisDetails_RowDataBound" SkinID="gridview" OnSelectedIndexChanged="gvDiagnosisDetails_SelectedIndexChanged"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                                        <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                                        <asp:HiddenField ID="hdnIsFinalDiagnosis" runat="server" Value='<%#Eval("IsFinalDiagnosis") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Side">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Primary" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>




                                                                <asp:TemplateField HeaderText="Chronic">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>' />
                                                                        <asp:HiddenField ID="hdnIsQuery" runat="server" Value='<%#Eval("IsQuery") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Condition">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Resolved">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Provider">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Onset Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>' />
                                                                        <asp:DropDownList ID="ddlProvider" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                            Visible="false">
                                                                            <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="HdnOnsetDate" runat="server" Value='<%#Eval("OnsetDateWithoutFormat") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Facility">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                                        <asp:DropDownList ID="ddlLocation" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                            Visible="false">
                                                                            <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remarks">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                
                                                                              <%--//yogesh--%>
                                                                 <asp:TemplateField HeaderText="Provisional">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProvisionalyo" runat="server" Text='<%#Eval("ProvisionalFinalDiagnosis") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                 <asp:TemplateField HeaderText="Final">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinalyo" runat="server" Text='<%#Eval("IsFinalDiagnosis") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                               




                                                                <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                                    SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="40px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                            ToolTip="Delete" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                           
                                                           



                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:GridView>



                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <%--   <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlDiagnosiss" />
                                        </Triggers>--%>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div id="trPlanOfCare" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblPlanOfCare" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Plan Of Care" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label24" runat="server" SkinID="label" Text="Plan Of Care" />
                                                                <span id="spnPlanOfCareStar" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px" data-toggle="tooltip" title="Plan Of Care" data-placement="left"
                                                                OnClick="lnkPlanOfCare_OnClick" />
                                                            <asp:Label ID="lblPlanOfCareMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="Panel13" runat="server">
                                                            <div class="fst-box col-md-12">
                                                                <asp:TextBox ID="txtWPlanOfCare" Rows="1" runat="server" TextMode="MultiLine" Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" data-autoresize="" onfocus="onFocus(this)" CssClass="textarea-custom" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                <asp:HiddenField ID="hdnWPlanRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                            </div>
                                                            <asp:Panel ID="pnlPlanOfCare" runat="server" CssClass="hidden">
                                                                <asp:GridView ID="gvPlanOfCare" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvPlanOfCare_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="1" OnPageIndexChanging="gvPlanOfCare_PageIndexChanging" OnRowCancelingEdit="gvPlanOfCare_OnRowCancelingEdit"
                                                                    OnRowUpdating="gvPlanOfCare_OnRowUpdating" OnRowEditing="gvPlanOfCare_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Plan Of Care" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorPlanOfCare" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" Height="75px" />
                                                                                <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="60px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"
                                                                            HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodeddate" runat="server" SkinID="label" Text='<%#Eval("DocDate")%>' />
                                                                                <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                                <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                            HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>



                                        <asp:Panel BorderStyle="Solid" BorderWidth="1px" ID="pnlICDCodes" Style="visibility: hidden; position: absolute;"
                                            BackColor="#E0EBFD" runat="server" Height="180px" ScrollBars="Auto"
                                            Width="400">
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <%-- <asp:UpdatePanel ID="update"
                                    runat="server"> <ContentTemplate>--%>

                                                        <aspl:ICD ID="icd" runat="server" width="400" PanelName="ctl00_ContentPlaceHolder1_pnlICDCodes"
                                                            ICDTextBox="ctl00_ContentPlaceHolder1_txtICDCode" />
                                                        <asp:HiddenField ID="hdnGridClientId" runat="server" />
                                                        <%-- </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                        <div id="trOrdersAndProcedures" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <div id="tblOrdersAndProcedures" runat="server">

                                                        <div class=" emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="imgbtnOrdersAndProcedures" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Orders And Procedures" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>

                                                                <asp:Label ID="Label13" runat="server" SkinID="label" Text="Orders&nbsp;And&nbsp;Procedures&nbsp;" />
                                                                <span id="SpanOrdersAndProceduresStar" class="red" visible="false" runat="server">*</span>
                                                            </h3>

                                                            <asp:ImageButton ID="imgbtnAddOrdersAndProcedures" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px"
                                                                data-toggle="tooltip" title="Add Orders And Procedures" data-placement="left" OnClick="lnkAddOrdersAndProcedures_OnClick" />
                                                        </div>




                                                        <asp:Panel ID="pnlOrderProcedures" runat="server">
                                                            <div class="col-md-12 bg-info diag-entry well-sm">

                                                                <div class="row">
                                                                    <input id="hdnICDCode" runat="server" type="hidden" value='<%# Eval("ICDCodes")%>' />
                                                                    <input id="hdnExitOrNot" runat="server" type="hidden" value='<%# Eval("ExitOrNot")%>' />
                                                                    <div class="col-md-2">
                                                                        <asp:TextBox ID="txtICDCode" runat="server" EnableViewState="true" SkinID="textbox"
                                                                            TabIndex="19" Width="100%" Placeholder="ICD CODE" />
                                                                        <cc1:PopupControlExtender ID="PopUnit" runat="server" OffsetX="5" PopupControlID="pnlICDCodes"
                                                                            Position="Left" TargetControlID="txtICDCode" />
                                                                        <asp:Label ID="lblModifier" runat="server" SkinID="label" Visible="false" />
                                                                        <asp:TextBox ID="txtModifier" runat="server" SkinID="textbox" TabIndex="20" Visible="false"
                                                                            Width="130px" />
                                                                        <cc1:PopupControlExtender ID="pcEtxtModifier" runat="server" OffsetX="5" PopupControlID="pnlModifierCode"
                                                                            Position="Right" TargetControlID="txtModifier" />
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <telerik:RadComboBox ID="cmbServiceName" runat="server" AutoPostBack="true" DataTextField="ServiceName"
                                                                            DataValueField="ServiceID" EmptyMessage="Search by Text" EnableLoadOnDemand="true"
                                                                            EnableVirtualScrolling="true" Height="350px" HighlightTemplatedItems="true" OnItemsRequested="cmbServiceName_OnItemsRequested"
                                                                            OnSelectedIndexChanged="cmbServiceName_OnSelectedIndexChanged" ShowMoreResultsBox="true"
                                                                            OnClientSelectedIndexChanged="GetSelectedItem"
                                                                            Width="100%">
                                                                            <HeaderTemplate>
                                                                                <table width="100%" cellpadding="0" cellspacing="1">
                                                                                    <tr>
                                                                                        <td>Service(s)
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                                                        </td>
                                                                                        <td id="Td1" runat="server" visible="false">
                                                                                            <%# DataBinder.Eval(Container, "Attributes['CPTCode']")%>
                                                                                        </td>
                                                                                        <td id="Td2" runat="server" visible="false">
                                                                                            <%# DataBinder.Eval(Container, "Attributes['LongDescription']")%>
                                                                                        </td>
                                                                                        <td id="Td3" runat="server" visible="false">
                                                                                            <%# DataBinder.Eval(Container, "Attributes['ServiceType']")%>
                                                                                        </td>
                                                                                        <td id="Td4" runat="server" visible="false">
                                                                                            <%# DataBinder.Eval(Container, "Attributes['DoctorRequired']")%>
                                                                                        </td>
                                                                                        <td id="Td5" runat="server" visible="false">
                                                                                            <%# DataBinder.Eval(Container, "Attributes['IsToothNoMandatory']")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>
                                                                        <telerik:RadComboBox ID="ddlGlobalToothNo" runat="server" SkinID="DropDown" Width="100%"
                                                                            DropDownWidth="300px" Height="250px" Filter="Contains" MarkFirstMatch="true"
                                                                            EmptyMessage="Select" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Visible="false" />
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <asp:CheckBox ID="chkStat" runat="server" Checked="false" Font-Bold="true"
                                                                            TabIndex="43" Text="Stat" TextAlign="Right" />
                                                                        <asp:Button ID="Button2" runat="server" Text="Add" CssClass="btn btn-xs btn-primary"
                                                                            ValidationGroup="ORDER" OnClick="btnUpdate_Click" TabIndex="47" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                    </div>
                                                                    <div class="col-md-4 hidden">
                                                                        <asp:RadioButtonList ID="rdoOrder" CssClass="radioo" runat="server"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="rdoOrder_OnSelectedIndexChanged"
                                                                            RepeatDirection="Horizontal">
                                                                            <asp:ListItem Text="Laboratory" Value="G"></asp:ListItem>
                                                                            <asp:ListItem Text="Radiology" Value="X"></asp:ListItem>
                                                                            <asp:ListItem Text="All" Value="O" Selected="True"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </div>
                                                                    <div class="col-md-4 hidden">
                                                                        <telerik:RadComboBox ID="ddlDepartment" runat="server" AppendDataBoundItems="true"
                                                                            TabIndex="10" SkinID="DropDown" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged"
                                                                            AutoPostBack="true" Width="100%" CssClass="hidden" />

                                                                        <telerik:RadComboBox ID="ddlSubDepartment" runat="server" AppendDataBoundItems="true"
                                                                            TabIndex="10" SkinID="DropDown" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged" />

                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <asp:GridView ID="gvOrdersAndProcedures" CssClass="table table-bordered" OnRowDataBound="gvOrdersAndProcedures_RowDataBound" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="true" Width="100%" Height="100%" Visible="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="130px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblODate" runat="server" SkinID="label" Text='<%#common.myDate(Eval("OrderDate")).ToString("dd/MM/yyyy")%>' />
                                                                            <asp:HiddenField ID="hdnServiceName" runat="server" Value='<%#Eval("ServiceName")%>' />
                                                                            <asp:HiddenField ID="hdnOrderDate" runat="server" Value='<%#Eval("OrderDate")%>' />
                                                                            <asp:HiddenField ID="hdnLabStatus" runat="server" Value='<%#Eval("LabStatus")%>' />
                                                                            <asp:HiddenField ID="hdnstat" runat="server" Value='<%#Eval("Stat")%>' />
                                                                            <asp:HiddenField ID="hdnServiceID" runat="server" Value='<%#Eval("ServiceID")%>' />

                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Service Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text='<%#Eval("ServiceName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" SkinID="label" Text='<%#Eval("LabStatus")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <div class="pagination-ui">
                                                                <asp:Repeater ID="rptPagerOrdersAndProcedures" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPageOrdersAndProcedures" runat="server" Text='<%#Eval("Text")%>'
                                                                            Font-Bold="true" CommandArgument='<%#Eval("Value")%>' Enabled='<%#Eval("Enabled")%>'
                                                                            OnClick="lnkPageOrdersAndProcedures_OnClick" />
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>

                                                            <asp:GridView ID="gvPatientServiceDetail" runat="server" AutoGenerateColumns="false"
                                                                HeaderStyle-HorizontalAlign="Left" OnRowDataBound="gvPatientServiceDetail_RowDataBound"
                                                                OnRowDeleting="gvPatientServiceDetail_RowDeleting" ShowHeader="true" SkinID="gridview"
                                                                Style="margin-bottom: 0px" TabIndex="49" Width="100%" Visible="false">
                                                                <PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="6" />
                                                                <EmptyDataTemplate>
                                                                    <asp:Label ID="lblEmpty" runat="server" Font-Bold="true" ForeColor="Red" Text="No Record Found." />
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:BoundField DataField="CPTCode" HeaderText="RefService Code" Visible="false" />
                                                                    <asp:TemplateField HeaderText="Services">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                                            <asp:HiddenField ID="hdnExcludedServices" runat="server" Value='<%#Eval("IsExcluded")%>' />
                                                                            <asp:HiddenField ID="hdnRequestToDepartment" runat="server" Value='<%#Eval("RequestToDepartment") %>' />
                                                                            <asp:HiddenField ID="hdnServiceID" runat="server" Value='<%#Eval("ServiceID") %>' />
                                                                            <asp:HiddenField ID="hdnresult" runat="server" Value='<%#Eval("result") %>' />
                                                                            <asp:HiddenField ID="hdnStat" runat="server" Value='<%#Eval("Stat") %>' />
                                                                            <asp:HiddenField ID="hdnAlertRequired" runat="server" Value='<%#Eval("AlertRequired") %>' />
                                                                            <asp:HiddenField ID="hdnAlertMessage" runat="server" Value='<%#Eval("AlertMessage") %>' />
                                                                            <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("Stat") %>' />
                                                                            <asp:HiddenField ID="hdnIsTemplateRequired" runat="server" />
                                                                            <asp:HiddenField ID="hdnToothNo" runat="server" Value='<%#Eval("ToothNo") %>' />
                                                                            <asp:HiddenField ID="hdnGDServiceType" runat="server" Value='<%#Eval("ServiceType") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Investigation Date" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTestDate" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy HH:mm tt}",Eval("TestDate")) %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Units" HeaderStyle-Width="10px" ItemStyle-Width="10px"
                                                                        ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtUnit" runat="server" Enabled="false" Width="30px" Text='<%#Eval("Units","{0:f0}")%>'
                                                                                MaxLength="5" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                FilterType="Custom" TargetControlID="txtUnit" ValidChars="0123456789" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Charges" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCharges" runat="server" Text='<%#Eval("Charges","{0:f2}")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="60px" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField ButtonType="Image" ItemStyle-HorizontalAlign="Center" DeleteImageUrl="~/Images/DeleteRow.png"
                                                                        HeaderStyle-Width="20px" ShowDeleteButton="true" HeaderText="Delete" />
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="60px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkAddInvestigationSpecification" runat="server" OnClick="lnkAddInvestigationSpecification_OnClick"
                                                                                CommandName="Template" Text="Request Form" ToolTip="Add Investigation Specification"
                                                                                CommandArgument='<%# Eval("ServiceId") %>' ForeColor="DodgerBlue" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Inv Detail" HeaderStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkServiceName" runat="server" OnClick="lnkServiceName_OnClick"
                                                                                CommandName="Template" Text="Inv. Dt." ToolTip="Investigation Details" CommandArgument='<%# Eval("ServiceId") %>'
                                                                                ForeColor="DodgerBlue" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                   
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:GridView>

                                                            <asp:HiddenField ID="hdnUpdateServiceId" runat="server" />
                                                            <asp:HiddenField ID="hdnUpdateOrderDtlId" runat="server" />
                                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnServiceType" runat="server" Value="0" />
                                                            <asp:HiddenField ID="HiddenField3" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnServiceId" runat="server" />
                                                            <asp:HiddenField ID="hdnServiceName" runat="server" />
                                                            <asp:HiddenField ID="hdnLabStatus" runat="server" />
                                                            <asp:HiddenField ID="hdnOrderId" runat="server" />
                                                            <asp:HiddenField ID="hdnEncodedBy" runat="server" />
                                                            <asp:HiddenField ID="hdnPatientGender" runat="server" />
                                                            <asp:HiddenField ID="hdnLongDescription" runat="server" />
                                                            <asp:HiddenField ID="hdnDoctorRequired" runat="server" />
                                                            <asp:HiddenField ID="hdnDepartmentRequest" runat="server" />
                                                            <asp:HiddenField ID="hdnAlertRequired" runat="server" />
                                                            <asp:HiddenField ID="hdnAlertMessage" runat="server" />
                                                            <asp:HiddenField ID="hdnCharges" runat="server" />
                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rdoOrder" />
                                                </Triggers>
                                            </asp:UpdatePanel>

                                        </div>
                                          <div id="trFreeText" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanelFreeText" runat="server">
                                                <ContentTemplate>
                                                    <div id="Div4" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButtonFreeText" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="FreeText" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label46" runat="server" SkinID="label" Text="Free Text Prescription" />
                                                                <span id="spnFreeText" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <%--<asp:ImageButton ID="lnkRemarks" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px" data-toggle="tooltip" title="Plan Of Care" data-placement="left"
                                                                OnClick="lnkPlanOfCare_OnClick" />--%>
                                                            <asp:Label ID="lblFreeText" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" Text="" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="PanelFreeText" runat="server">
                                                            <div class="fst-box col-md-12">
                                                                <asp:TextBox ID="txtFreeText" Rows="1" runat="server" TextMode="MultiLine" Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" data-autoresize="" onfocus="onFocus(this)" CssClass="textarea-custom" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                <asp:HiddenField ID="hdnFreeTextID" runat="server" />
                                                            </div>

                                                        </asp:Panel>
                                                       
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="trPrescriptions" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblPrescriptions" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="imgbtnPrescription" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Prescriptions" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label16" runat="server" SkinID="label" Text="Prescriptions&nbsp;" /></h3>
                                                            <asp:ImageButton ID="imgBtnAddPrescriptions" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px"
                                                                data-toggle="tooltip" title="Add Prescriptions" data-placement="left" OnClick="lnkAddPrescriptions_OnClick" />
                                                        </div>

                                                        <asp:Panel ID="pnlPrescription" runat="server" CssClass="table-col-auto pagination">
                                                            <asp:GridView ID="gvPrescriptions" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="true" Width="100%" Height="100%">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSDate" runat="server" SkinID="label" Text='<%#common.myDate(Eval("EncodedDate")).ToString("dd/MM/yyyy") %>' />
                                                                            <asp:HiddenField ID="hdnItemName" runat="server" Value='<%#Eval("ItemName")%>' />
                                                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="200px" HeaderText="Store Name" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStoreName" runat="server" SkinID="label" />
                                                                            <%--Text='<%#Eval("StoreName")%>'--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField ItemStyle-Width="300px" HeaderText="Generic Name" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGenericName" runat="server" SkinID="label" Text='<%#Eval("GenericName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField ItemStyle-Width="300px" HeaderText="Drug Name" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text='<%#Eval("ItemName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Prescription Details" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPrescriptionDetail" runat="server" SkinID="label" Text='<%#Eval("PrescriptionDetail")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>

                                                            <div class="pagination-ui">
                                                                <asp:Repeater ID="rptPagerPrescriptions" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPagePrescriptions" runat="server" Text='<%#Eval("Text")%>'
                                                                            Font-Bold="true" CommandArgument='<%#Eval("Value")%>' Enabled='<%#Eval("Enabled")%>'
                                                                            OnClick="lnkPagePrescriptions_OnClick" />
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trNonDrugOrder" runat="server">
                                            <asp:UpdatePanel ID="UpdatePanel34" runat="server">
                                                <ContentTemplate>
                                                    <div id="tblNonDrugOrder" runat="server">

                                                        <div class=" emrPart-Green container-fluid form-group margin_Top">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Other Order" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label3" runat="server" SkinID="label" Text="Other Order" />
                                                                <span id="spnNonDrugOrder" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:ImageButton ID="imgNonDrugOrder" runat="server" ImageUrl="~/Images/add.png"
                                                                data-toggle="tooltip" title="Add Other Order" data-placement="left" Height="20px" Width="20px" OnClick="imgNonDrugOrder_OnClick" />
                                                            <asp:Label ID="lblNonDrugOrder" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>

                                                        <asp:Panel ID="Panel20" runat="server">

                                                            <div class="col-md-6">
                                                                <label>
                                                                    <asp:Label ID="Label5" runat="server" Text="Order Type" />
                                                                    <span style="color: Red">*</span></label>
                                                                <asp:DropDownList ID="ddlOrderType" runat="server" SkinID="DropDown" Width="70px"
                                                                    onchange="return SetIsTransitDataEntered(this);">
                                                                    <asp:ListItem Text="Routine" Value="R" />
                                                                    <asp:ListItem Text="Urgent" Value="U" />
                                                                    <asp:ListItem Text="Stat" Value="S" />
                                                                    <asp:ListItem Text="SOS" Value="O" />
                                                                </asp:DropDownList>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <label>
                                                                    <asp:Label ID="lblDoctor" runat="server" Text="Doctor" />
                                                                    <span style="color: Red">*</span></label>

                                                                <asp:DropDownList ID="ddlDoctor" runat="server" SkinID="DropDown" Width="170px" DropDownWidth="250"
                                                                    onchange="return SetIsTransitDataEntered(this);" />
                                                                <div class="fst-box col-md-12">
                                                                    <asp:TextBox ID="editorNonDrugOrder" runat="server" TextMode="MultiLine" Height="90px"
                                                                        Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                </div>





                                                            </div>


                                                            <asp:GridView ID="gvNonDrugOrder" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                                AllowPaging="false" PageSize="1" Width="100%" Height="100%" OnRowCommand="gvNonDrugOrder_OnRowCommand"
                                                                OnRowDataBound="gvNonDrugOrder_RowDataBound" OnRowCancelingEdit="gvNonDrugOrder_OnRowCancelingEdit"
                                                                OnRowUpdating="gvNonDrugOrder_OnRowUpdating" OnRowEditing="gvNonDrugOrder_OnRowEditing"
                                                                OnPageIndexChanging="gvNonDrugOrder_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Other Orders" HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="edNonDrugOrder" runat="server" TextMode="MultiLine" Width="100%"
                                                                                Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" />
                                                                            <asp:HiddenField ID="hdnPrescription" runat="server" Value='<%#Eval("Prescription")%>' />
                                                                            <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date" ItemStyle-VerticalAlign="Top" HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate")%>' />
                                                                            <asp:HiddenField ID="hdnNonDrugOrderId" runat="server" Value='<%#Eval("NonDrugOrderId")%>' />
                                                                            <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%#Eval("OrderType")%>' />
                                                                            <asp:HiddenField ID="hdnNurseId" runat="server" Value='<%#Eval("NurseId")%>' />
                                                                            <asp:HiddenField ID="hdnAcknowledgeBy" runat="server" Value='<%#Eval("AcknowledgeBy")%>' />
                                                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Entered By" ItemStyle-VerticalAlign="Top" HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>' />

                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                        ItemStyle-Width="10px" HeaderStyle-Width="10px" />
                                                                </Columns>
                                                            </asp:GridView>

                                                        </asp:Panel>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trPACTemplates" runat="server" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <div id="dvPACTemplates" runat="server">
                                                        <div class=" emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="imgpnlPACTemplates" runat="server" ImageUrl="~/Images/plus-icon.svg"
                                                                    ToolTip="PAC Notes" Enabled="false" Height="16px" Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="lblPACNotes" runat="server" SkinID="label" Text="PAC Notes" /></h3>

                                                            <asp:ImageButton ID="ibtnPACNotes" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="PAC Notes" Height="18px" Width="18px" OnClick="ibtnPACNotes_Click" />
                                                            <asp:Button ID="btnPACDone" runat="server" CssClass="btn btn-danger btn-sm margin_Top01" Text="PAC Done" ToolTip="PAC Done"
                                                                OnClick="btnPACDone_Click" />
                                                            <asp:Button ID="btnCancelPACDone" runat="server" CssClass="btn btn-danger btn-sm margin_Top01" Text="Cancel PAC Done" ToolTip="Cancel PAC Done"
                                                                OnClick="btnCancelPACDone_Click" />

                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="pnlPACTemplates" runat="server">
                                                            <%--Ujjwal for Saved PAC Templates Showing--%>
                                                        </asp:Panel>
                                                    </div>

                                                    <div id="dvPAC" visible="false" runat="server" class="pac-popup">
                                                        <div class="container-fluid">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RadioButtonList ID="rblFitForSurgery" runat="server" RepeatDirection="Horizontal" CssClass="table table-noborder table-condensed"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="rblFitForSurgery_SelectedIndexChanged">
                                                                        <asp:ListItem Text="Fit For Surgery" Value="1" Selected="True" />
                                                                        <asp:ListItem Text="Not Fit For Surgery in DSU" Value="2" />
                                                                        <asp:ListItem Text="Not Fit For Surgery" Value="0" />
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Label ID="Label44" runat="server" Text="PAC Done Remarks"></asp:Label>
                                                                    <span style="color: Red;" id="spnPACRemarks" runat="server" visible="false">*</span>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox ID="txtPACRemarks" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12 text-right">
                                                                    <asp:Button ID="btnPACDoneOk" runat="server" CssClass="btn btn-xs btn-primary" Text="PAC Done" ToolTip="PAC Done"
                                                                        OnClick="btnPACDoneOk_Click" />
                                                                    <asp:Button ID="btnClosePAC" runat="server" CssClass="btn btn-xs btn-danger" Text="Close" ToolTip="PAC Done"
                                                                        OnClick="btnClosePAC_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div id="trOTRequest" runat="server" visible="false">

                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <div id="dvOTRequest" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">
                                                        <div class=" emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="imgExpndOTRequest" runat="server" ImageUrl="~/Images/plus-icon.svg"
                                                                    ToolTip="OT Request" Height="16px" OnClick="ViewHide_OnClick"
                                                                    Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label43" runat="server" SkinID="label" Text="OT Request" /></h3>
                                                            <asp:ImageButton ID="imgAddOTRequest" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="OT Request" Height="18px" Width="18px"
                                                                OnClick="imgAddOTRequest_Click" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="pnlOTRequest" runat="server">
                                                            <asp:GridView ID="gvbindEMROTRequest" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CssClass="table table-bordered"
                                                                OnRowDataBound="gvbindEMROTRequest_RowDataBound" OnRowDeleting="gvbindEMROTRequest_RowDeleting">
                                                                <EmptyDataTemplate>
                                                                    <div style="font-weight: bold; color: Red; width: 200px">
                                                                        No Record Found.
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:TemplateField Visible="True" HeaderText="Sl. No">
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:PRegistration, regno %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnOTRequestID" runat="server" Value='<%#Eval("OTRequestID")%>' />
                                                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value=' <%#Eval("RegistrationId")%>' />
                                                                            <asp:HiddenField ID="hdnServiceRemarks" runat="server" Value=' <%#Eval("ServiceRemarks")%>' />
                                                                            <asp:HiddenField ID="hdnStatusId" runat="server" Value=' <%#Eval("StatusId")%>' />
                                                                            <asp:Label ID="lblRegistrationNo" SkinID="label" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                                            <asp:HiddenField ID="hdnDepartmentType" runat="server" Value=' <%#Eval("DepartmentType")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Patient Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFullName" SkinID="label" runat="server" Text=' <%#Eval("FullName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="OT Request Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOTBookingDate" SkinID="label" runat="server" Text=' <%#Eval("OTBookingDate", "{0:dd/MM/yyyy}") +" "+ Eval("FromTime").ToString() %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Duration">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOTDuration" SkinID="label" runat="server" Text=' <%#Eval("OTDuration") +" "+ Eval("OTDurationType").ToString() %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="OT Booking Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBookDate" SkinID="label" runat="server" Text=' <%#Eval("BookDate", "{0:dd/MM/yyyy}")+" "+ Eval("BookFromTime").ToString()%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Theater">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTheatreName" SkinID="label" runat="server" Text=' <%#Eval("TheatreName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" SkinID="label" runat="server" Text=' <%#Eval("Status")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Delete" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ibtnewDelete" runat="server" ToolTip="Click here to Delete this record"
                                                                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete Record ?');" CommandArgument='<%#Eval("OTRequestID")%>' ImageUrl="~/Images/DeleteRow.png" Width="12px" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>


                                                        </asp:Panel>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trFollowup" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                <ContentTemplate>
                                                    <div id="Div3" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButtonFollowup" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Follow up" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="lblFollowup" runat="server" SkinID="label" Text="Follow up" />
                                                                <span id="SpanFollowup" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <%--   <asp:ImageButton ID="lnkRemarks" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px" data-toggle="tooltip" title="Follow up" data-placement="left"
                                                                OnClick="lnkRemarks_Click" />--%>
                                                            <asp:Label ID="Label47" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="PanelFollowup" runat="server">
                                                            <div class="fst-box col-md-12">
                                                                <asp:TextBox ID="txtFollowup" Rows="1" runat="server" TextMode="MultiLine" Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" data-autoresize="" onfocus="onFocus(this)" CssClass="textarea-custom" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                <asp:HiddenField ID="hdnFollowup" runat="server" />
                                                            </div>

                                                        </asp:Panel>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div id="trRemarks" runat="server">

                                            <asp:UpdatePanel ID="UpdatePaneltrRemarks" runat="server">
                                                <ContentTemplate>
                                                    <div id="Div2" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButtonRemarks" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Remarks" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label45" runat="server" SkinID="label" Text="Remarks" />
                                                                <span id="spnRemarks" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <%--<asp:ImageButton ID="lnkRemarks" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px" data-toggle="tooltip" title="Plan Of Care" data-placement="left"
                                                                OnClick="lnkPlanOfCare_OnClick" />--%>
                                                            <asp:Label ID="lblremarks" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <asp:Panel ID="PanelRemarks" runat="server">
                                                            <div class="fst-box col-md-12">
                                                                <asp:TextBox ID="txtRemarks" Rows="1" runat="server" TextMode="MultiLine" Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" data-autoresize="" onfocus="onFocus(this)" CssClass="textarea-custom" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                <asp:HiddenField ID="hdntxtRemarksId" runat="server" />
                                                            </div>

                                                        </asp:Panel>
                                                       
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div id="trReferralsReplyToReferrals" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table8" runat="server" width="99%">

                                                        <div class=" emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Referrals & Reply to referrals" Enabled="false" Height="16px" Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label26" runat="server" SkinID="label" Text="Referrals & Reply to referrals" />
                                                            </h3>
                                                            <asp:ImageButton ID="lnkReferrals" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px" data-toggle="tooltip" title="Referrals & Reply to referrals" data-placement="left"
                                                                OnClick="lnkReferrals_OnClick" />
                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trPatientFamilyEducationCounseling" runat="server">

                                            <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table12" runat="server">

                                                        <div class="container-fluid emrPart-Green">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Patient and family education and counselling " Enabled="false" Height="16px"
                                                                    Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label18" runat="server" SkinID="label" Text="Patient and family education and counselling" />
                                                                <span id="spnPatientFamilyEducationCounseling" class="red" visible="false" runat="server">*</span>
                                                            </h3>
                                                            <asp:ImageButton ID="lnkEducationCounseling" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px"
                                                                data-toggle="tooltip" title="Patient and family education" data-placement="left"
                                                                OnClick="lnkEducationCounseling_OnClick" />

                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trMultidisciplinaryEvaluationPlanOfCare" runat="server" visible="false">

                                            <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table10" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">

                                                        <div class=" emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Multidisciplinary evaluation and plan of care" Enabled="false" Height="16px"
                                                                    Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label28" runat="server" SkinID="label" Text="Multidisciplinary evaluation and plan of care" /></h3>
                                                            <asp:ImageButton ID="lnkMultidisciplinaryEvaluation" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px"
                                                                data-toggle="tooltip" title="Multidisciplinary Evaluation" data-placement="left"
                                                                OnClick="lnkMultidisciplinaryEvaluation_OnClick" />

                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>

                                        <div id="trAnaesthesiaCriticalCareNotes" runat="server" visible="false" class="hidden">

                                            <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                                <ContentTemplate>
                                                    <div id="Table9" runat="server">

                                                        <div class=" emrPart-Green container-fluid">
                                                            <p>
                                                                <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="~/Images/Expand.png"
                                                                    ToolTip="Anaesthesia and Critical care notes" Enabled="false" Height="16px" Width="16px" />
                                                            </p>
                                                            <h3>
                                                                <asp:Label ID="Label27" runat="server" SkinID="label" Text="Anaesthesia and Critical care notes" /></h3>

                                                            <asp:ImageButton ID="lnkAnaesthesiaCritical" runat="server" ImageUrl="~/Images/add.png" Height="20px" Width="20px"
                                                                ToolTip="Anaesthesia and Critical care notes" OnClick="lnkAnaesthesiaCritical_OnClick" />

                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>
                                         
                                        <table width="99%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                                                        <ContentTemplate>
                                                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Skin="Metro">
                                                                <Windows>
                                                                    <telerik:RadWindow ID="RadWindowForNew" Skin="Metro" runat="server" EnableViewState="false" Modal="true" Height="500" Width="650" MinWidth="650" InitialBehaviors="Maximize"
                                                                        ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" OnClientShow="setCustomPosition" Behaviors="Close,Maximize,Minimize,Move,Pin,Resize" VisibleTitlebar="true">
                                                                    </telerik:RadWindow>

                                                                    <telerik:RadWindow ID="RadWindow1" Skin="Metro" runat="server" EnableViewState="false" Height="500" Width="650"
                                                                        ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" VisibleStatusbar="false" Behaviors="Close,Maximize,Minimize,Move,Pin,Resize" VisibleTitlebar="true" />
                                                                </Windows>
                                                            </telerik:RadWindowManager>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel44" runat="server">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hdnPACNotesGroupId" runat="server" />
                                                            <asp:HiddenField ID="hdnAnesthesiaAndCriticalCareGroupId" runat="server" />
                                                            <asp:HiddenField ID="hdnIsToothNoMandatory" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnToothNo" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnItemId" runat="server" />
                                                            <asp:HiddenField ID="hdnItemName" runat="server" />
                                                            <asp:HiddenField ID="hdnAllergyType" runat="server" />
                                                            <asp:HiddenField ID="hdnHistoryRecordId" runat="server" />
                                                            <asp:HiddenField ID="hdnPreviousTreatmentRecordId" runat="server" />
                                                            <asp:HiddenField ID="hdnExaminationRecordId" runat="server" />
                                                            <asp:HiddenField ID="hdnNutritionalStatusRecordId" runat="server" />
                                                            <asp:HiddenField ID="hdnPlanOfCareRecordId" runat="server" />
                                                            <asp:HiddenField ID="hdnCostAnalysisRecordId" runat="server" />
                                                            <asp:Button ID="btnAddChiefComplaintsClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddChiefComplaintsClose_OnClick" />
                                                            <%--change care plan--%>
                                                            <asp:Button ID="btnSaveCarePlan" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnSaveCarePlan_Click" />
                                                            <%--change care plan--%>
                                                            <asp:Button ID="ImageBtnEyesVital" runat="server"
                                                                Style="visibility: hidden;" OnClick="ImageBtnEyesVital_Click1" />
                                                            <asp:Button ID="btnAddAllergiesClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddAllergiesClose_OnClick" />
                                                            <asp:Button ID="btnAddVitalsClose" runat="server" Style="visibility: hidden;" OnClick="btnAddVitalsClose_OnClick" />
                                                            <asp:Button ID="btnAddTemplatesClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddTemplatesClose_OnClick" />
                                                            <asp:Button ID="btnPreviousTreatmentClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnPreviousTreatmentClose_OnClick" />
                                                            <asp:Button ID="btnNutritionalStatusClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnNutritionalStatusClose_OnClick" />
                                                            <asp:Button ID="btnPlanOfCareClose" runat="server" Style="visibility: hidden;" OnClick="btnPlanOfCareClose_OnClick" />
                                                            <asp:Button ID="btnCostAnalysisClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnCostAnalysisClose_OnClick" />
                                                            <asp:Button ID="btnAddDiagnosisSerchOnClientClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddDiagnosisSerchOnClientClose_OnClick" />
                                                            <asp:Button ID="btnAddTemplatesClose_All" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddTemplatesClose_All_OnClick" />
                                                            <asp:Button ID="btnAddOrdersAndProceduresClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddOrdersAndProceduresClose_OnClick" />
                                                            <asp:Button ID="btnAddPrescriptionsClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnAddPrescriptionsClose_OnClick" />
                                                            <asp:Button ID="btnBindhistory" runat="server" Style="visibility: hidden;" OnClick="btnBindhistory_OnClick" />
                                                            <asp:Button ID="btnRefreshTemplate" runat="server" Style="visibility: hidden;" OnClick="btnRefreshTemplate_Click" />
                                                            <asp:Button ID="btnProvisionalDiagnosisClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnProvisionalDiagnosisClose_OnClick" />
                                                            <asp:Button ID="Button1" runat="server" Style="visibility: hidden;" OnClick="btnEnableControl_OnClick" />
                                                            <asp:Button ID="btnNonDrugOrder" runat="server" Style="visibility: hidden;" OnClick="btnNonDrugOrder_OnClick" />
                                                            <asp:Button ID="btnAddHPIClose" runat="server" Style="visibility: hidden;" OnClick="btnAddHPIClose_OnClick" />
                                                            <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Style="visibility: hidden;" />
                                                            <asp:TextBox ID="txtedit" runat="server" Style="visibility: hidden;" />
                                                            <asp:HiddenField ID="hdnNonDrugOrderId" runat="server" />
                                                            <asp:HiddenField ID="hdnPastHistoryRecordId" runat="server" />
                                                            <asp:Button ID="btnBindPasthistory" runat="server" Style="visibility: hidden;" OnClick="btnBindPasthistory_OnClick" />
                                                            <asp:Button ID="btnFinalDiagnosisClose" runat="server" Style="visibility: hidden;"
                                                                OnClick="btnFinalDiagnosisClose_OnClick" />
                                                            <asp:HiddenField ID="hdnIsTransitDataEntered" runat="server" />
                                                            <asp:HiddenField ID="hdnCurrentControlFocused" runat="server" />
                                                            <asp:Timer ID="TimerAutoSaveDataInTransit" runat="server" ClientIDMode="AutoID" Interval="60000" OnTick="TimerAutoSaveDataInTransit_OnTick" />
                                                            <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                DisplayAfter="100" DynamicLayout="true">
                                <ProgressTemplate>
                                    <center>
                                <div style="width: 154px; position: absolute; bottom: 0; height: 60px; left: 500px; top: 300px" class="fade">
                                    <img id="Img1" src="/Images/ajax-loader3.gif" alt="loading" runat="server" />
                                </div>
                             </center>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="free-text">
                            <textarea rows="10" cols="40" class="form-control" placeholder=""></textarea>
                        </div>


                    </div>


                    <div id="Tr6" runat="server">

                        <asp:Label ID="lblmessage1" SkinID="label" runat="server" Text="&nbsp;" CssClass="text-center" Style="display: none;"></asp:Label>

                        <asp:UpdatePanel ID="UpdatePanel42" runat="server" Visible="false">
                            <ContentTemplate>
                                <div class="text-center" style="padding-top: 9px;">
                                    <asp:Button ID="btnSaveDashboard" Text="Save As Draft" runat="server" CssClass="btn btn-xs btn-primary btn-xs"
                                        OnClick="btnSaveDashboard_OnClick" />
                                    <asp:Button ID="btnSaveSign" Text="Save As Signed" runat="server" CssClass="btn btn-xs btn-primary btn-xs"
                                        Visible="false" OnClick="btnSaveAsSigned_OnClick" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>



            <div class="col-md-5 mid-box">
                <a href="#" id="btn-expand-collapse1"><i class="fas fa-angle-left"></i></a>
                <asp:UpdatePanel runat="server" ID="updatePanal6" class="view-mode">
                    <ContentTemplate>
                        <div id="onscreen" class="onscreen custom-scroller custom-scroller-light" runat="server" visible="false">
                            <div id="divVital" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Vitals</h5>
                                            <asp:GridView ID="GridViewvitals" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                Width="100%" Height="100%" HeaderStyle-Height="3px"
                                                Style="margin: 0;">
                                                <Columns>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label29" runat="server" Text="HT" ToolTip="Height" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkHT" runat="server" Text='<%#Eval("HT")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("HT"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label30" runat="server" Text="WT" ToolTip="Weight" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkWT" runat="server" Text='<%#Eval("WT")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("WT"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label31" runat="server" Text="HC" ToolTip="Head Circumference" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkHC" runat="server" Text='<%#Eval("HC")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("HC"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label31" runat="server" Text="Tem" ToolTip="Head Circumference" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkT" runat="server" Text='<%#Eval("T")%>' Width="18"
                                                                Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("T"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                            <asp:HiddenField ID="hdnT_ABNORMAL_VALUE" runat="server" Value='<%#Eval("T_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label33" runat="server" Text="RR" ToolTip="Respiration" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkR" runat="server" Text='<%#Eval("R")%>' Width="18"
                                                                Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("R"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                            <asp:HiddenField ID="hdnR_ABNORMAL_VALUE" runat="server" Value='<%#Eval("R_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label34" runat="server" Text="Pulse" ToolTip="Pulse" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkP" runat="server" Text='<%#Eval("P")%>' Width="18"
                                                                Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("P"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                            <asp:HiddenField ID="hdnP_ABNORMAL_VALUE" runat="server" Value='<%#Eval("P_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label35" runat="server" Text="BPS" ToolTip="BP Systolic" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkBPS" runat="server" Text='<%#Eval("BPS")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("BPS"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label36" runat="server" Text="BPD" ToolTip="BP Diastolic" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkBPD" runat="server" Text='<%#Eval("BPD")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("BPD"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label37" runat="server" Text="MAC" ToolTip="Mid Arm Circumference"
                                                                ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkMAC" runat="server" Text='<%#Eval("MAC")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("MAC"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label38" runat="server" Text="SpO2" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkSpO2" runat="server" Text='<%#Eval("SpO2")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" ImageUrl="/Images/icon/line-graph.svg"
                                                                Visible='<%# (Convert.ToString(Eval("SpO2"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>

                                                            <asp:Label ID="Label39" runat="server" Text="BMI" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkBMI" runat="server" Text='<%#Eval("BMI")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("BMI"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label40" runat="server" Text="BSA" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <asp:Label ID="lnkBSA" runat="server" Text='<%#Eval("BSA")%>'
                                                                Width="18" Font-Underline="false" ForeColor="Black" Font-Size="Smaller"
                                                                Visible='<%# (Convert.ToString(Eval("BSA"))!="")?Convert.ToBoolean("true"):Convert.ToBoolean("false") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%-- added by bhakti 29/12/2020--%>
                            <div id="divChifComplaint" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Chief Complaint</h5>
                                            <asp:GridView ID="GridViewChifComplaint" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left"
                                                CssClass="table table-bordered"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="120px" HeaderText="Provisional Diagnosis" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProblemDescription" runat="server" Text='<%#Eval("ProblemDescription")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duration" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDuration" SkinID="label" runat="server" Text='<%#Eval("Duration")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEntryDate" SkinID="label" runat="server" Text='<%#Eval("EntryDate")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                        SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" Visible="false" />
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="40px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <%-- commented by bhakti 29/12/2020--%>
                                <%--<div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Chief Complaint</h5>
                                            <span>
                                                <asp:Label ID="lblchifcomplaint" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                            <div id="divHpi" runat="server" class="" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">History of Present illness</h5>
                                            <span>

                                                <asp:TextBox ID="lblHpi" runat="server" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divPastHistory" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Past History</h5>
                                            <span>
                                                <asp:Label ID="lblPastHistory" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divExamination" runat="server" class="" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Examination</h5>
                                            <span>
                                                <asp:TextBox ID="lblExamination" runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%-- added by bhakti 30/12/2020--%>
                            <div id="divCareTemplate" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Care Templates </h5>
                                            <asp:GridView ID="GridViewCareTemplates" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left"
                                                CssClass="table table-bordered"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="120px" HeaderText="Template Name" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocDate" SkinID="label" runat="server" Text='<%#Eval("DocDate")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                        SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" Visible="false" />
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="40px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%-- added by bhakti 29/12/2020--%>
                            <div id="divProvisionalDiagnosis" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Provisional Diagnosis</h5>
                                            <asp:GridView ID="GridViewProvisionalDiagnosis" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left"
                                                CssClass="table table-bordered"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="120px" HeaderText="Provisional Diagnosis" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProvisionalDiagnosis" runat="server" Text='<%#Eval("ProvisionalDiagnosis")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncodedDate" SkinID="label" runat="server" Text='<%#Eval("EncodedDate")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                        SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" Visible="false" />
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="40px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divDiagnosis" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Diagnosis</h5>
                                            <asp:GridView ID="GridViewDiagnosis" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDiagnosisDetails_RowCommand"
                                                OnRowDataBound="gvDiagnosisDetails_RowDataBound" SkinID="gridview" OnSelectedIndexChanged="gvDiagnosisDetails_SelectedIndexChanged"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                            <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                            <asp:HiddenField ID="hdnIsFinalDiagnosis" runat="server" Value='<%#Eval("IsFinalDiagnosis") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Side">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Primary" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Chronic">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>' />
                                                            <asp:HiddenField ID="hdnIsQuery" runat="server" Value='<%#Eval("IsQuery") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>




                                                  





                                                    <asp:TemplateField HeaderText="Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Condition">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Resolved">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Provider">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Onset Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>' />
                                                            <asp:DropDownList ID="ddlProvider" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                Visible="false">
                                                                <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="HdnOnsetDate" runat="server" Value='<%#Eval("OnsetDateWithoutFormat") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Facility">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                            <asp:DropDownList ID="ddlLocation" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                Visible="false">
                                                                <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                             
                                                                              <%--//yogesh--%>
                                                                 <asp:TemplateField HeaderText="Provisional">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProvisionalyo" runat="server" Text='<%#Eval("ProvisionalFinalDiagnosis") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                 <asp:TemplateField HeaderText="Final">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinalyo" runat="server" Text='<%#Eval("IsFinalDiagnosis") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>










                                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                        SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" Visible="false" />
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="40px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%-- added by bhakti 29/12/2020--%>
                            <div id="divplanofcare" runat="server" class="" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Plan Of Care</h5>
                                            <span>
                                                <%--change from label to textbox--%>
                                                <asp:TextBox ID="lblPlanofcare" runat="server" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divGridViewOrders" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Order & Procedure</h5>
                                            <%--GridViewOrders--%>
                                            <asp:GridView ID="GridViewOrders" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                ShowHeader="true" Width="100%" Height="100%" Visible="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="130px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblODate" runat="server" SkinID="label" Text='<%#common.myDate(Eval("OrderDate")).ToString("dd/MM/yyyy")%>' />
                                                            <asp:HiddenField ID="hdnServiceName" runat="server" Value='<%#Eval("ServiceName")%>' />
                                                            <asp:HiddenField ID="hdnOrderDate" runat="server" Value='<%#Eval("OrderDate")%>' />
                                                            <asp:HiddenField ID="hdnLabStatus" runat="server" Value='<%#Eval("LabStatus")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text='<%#Eval("ServiceName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" SkinID="label" Text='<%#Eval("LabStatus")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divPriscription" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Prescription</h5>
                                            <asp:GridView ID="GridViewPriscription" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                ShowHeader="true" Width="100%" Height="100%">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-ForeColor="Black" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSDate" runat="server" SkinID="label" Text='<%#Eval("StartDate")%>' />
                                                            <asp:HiddenField ID="hdnItemName" runat="server" Value='<%#Eval("ItemName")%>' />
                                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="200px" HeaderText="Store Name" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-ForeColor="Black" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStoreName" runat="server" SkinID="label" />
                                                            <%--Text='<%#Eval("StoreName")%>'--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="300px" HeaderText="Generic Name" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGenericName" runat="server" SkinID="label" Text='<%#Eval("GenericName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="300px" HeaderText="Drug Name" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text='<%#Eval("ItemName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Prescription Details" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-ForeColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrescriptionDetail" runat="server" SkinID="label" Text='<%#Eval("PrescriptionDetail")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divOtRequest" runat="server" visible="false">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">OT Request</h5>
                                            <asp:GridView ID="GridViewOTRequest" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="table table-bordered">
                                                <EmptyDataTemplate>
                                                    <div style="font-weight: bold; color: Red; width: 200px">
                                                        No Record Found.
                                                    </div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField Visible="True" HeaderText="S.No" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:PRegistration, regno %>">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnOTRequestID" runat="server" Value='<%#Eval("OTRequestID")%>' />
                                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value=' <%#Eval("RegistrationId")%>' />
                                                            <asp:HiddenField ID="hdnServiceRemarks" runat="server" Value=' <%#Eval("ServiceRemarks")%>' />
                                                            <asp:HiddenField ID="hdnStatusId" runat="server" Value=' <%#Eval("StatusId")%>' />
                                                            <asp:Label ID="lblRegistrationNo" SkinID="label" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                            <asp:HiddenField ID="hdnDepartmentType" runat="server" Value=' <%#Eval("DepartmentType")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Patient Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFullName" SkinID="label" runat="server" Text=' <%#Eval("FullName")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="OT Request Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOTBookingDate" SkinID="label" runat="server" Text=' <%#Eval("OTBookingDate", "{0:dd/MM/yyyy}") +" "+ Eval("FromTime").ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duration">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOTDuration" SkinID="label" runat="server" Text=' <%#Eval("OTDuration") +" "+ Eval("OTDurationType").ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="OT Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBookDate" SkinID="label" runat="server" Text=' <%#Eval("BookDate", "{0:dd/MM/yyyy}")+" "+ Eval("BookFromTime").ToString()%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Theater">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTheatreName" SkinID="label" runat="server" Text=' <%#Eval("TheatreName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" SkinID="label" runat="server" Text=' <%#Eval("Status")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Delete" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnewDelete" runat="server" ToolTip="Click here to Delete this record"
                                                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete Record ?');" CommandArgument='<%#Eval("OTRequestID")%>' ImageUrl="~/Images/DeleteRow.png" Width="12px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row" id="viewscreen">

                    <div class="col-md-12 item hidden" id="PatientVisits">
                        <div class="pos-fixed">
                            <div class="head-lebel  hidden" id="PatientLable"><span>Patient Visits</span></div>
                            <div class="head-lebel  hidden" id="DoctorNote"><span>Doctor Progress Note</span></div>

                        </div>
                        <div class="block-contain" style="padding: 0 10px;">
                            <%--<h2><span>Patient Visit</span> </h2> --%>


                            <nav id="menu-container" class="arrow">
                                <div id="btn-nav-previous"><i class="fas fa-chevron-circle-left"></i></div>
                                <div id="btn-nav-next"><i class="fas fa-chevron-circle-right"></i></div>


                                <div class="menu-inner-box">
                                    <ul id="content-slider" class="menu list-inline">
                                    </ul>
                                </div>


                            </nav>





                            <div class="tab-content hidden" id="tabPasthistory">
                                <%--<div id="home1" class="tab-pane fade in active">
                            <h3>HOME</h3>
                            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
                        </div>
                        <div id="menu1" class="tab-pane fade">
                            <h3>Menu 1</h3>
                            <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                        </div>
                        <div id="menu2" class="tab-pane fade">
                            <h3>Menu 2</h3>
                            <p>Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam.</p>
                        </div>
                        <div id="menu3" class="tab-pane fade">
                            <h3>Menu 3</h3>
                            <p>Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.</p>
                        </div>--%>
                            </div>
                        </div>
                    </div>


                    <div class="pos-fixed" style="margin: 0 0 5px 15px;">
                        <div class="head-lebel"><span>Patient History</span></div>
                    </div>

                    <div class="col-md-12">
                        <div class="row">
                            <div class="block-contain widget-block">
                                <%--<h2><span>Previous Visit</span> </h2>--%>

                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary ">Diagnosis
                                             <asp:UpdatePanel runat="server" ID="updatepaneDiagnosisHis" class="poupicon">
                                                 <ContentTemplate>
                                                     <asp:ImageButton ID="ImageButtonDiagnosisHIs" runat="server" CssClass="pull-right" Style="cursor: pointer;"
                                                         ImageUrl="~/Images/external-link-symbol.svg" OnClick="ImageButtonDiagnosisHIs_Click" title="Diagnosis History" Height="16px" Width="16px" />
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>

                                                <%--<a class="pull-right" onclick="Diagnosis();" style="cursor: pointer;">--%>
                                                <span data-toggle="tooltip" data-placement="left" title="Diagnosis History">
                                                    <%--<img src="/Images/external-link-symbol.svg" />--%>

                                                </span>

                                                <%--  </a>--%>

                                            </h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder1" class="img hidden" />
                                            <table class="table" id="tbl_diagnosis" style="display: none">
                                                <thead>
                                                    <tr>
                                                        <th>Diagnosis</th>
                                                        <th>VT</th>
                                                        <th>Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg1" style="display: none;">
                                                <h5 class="msg_text">Record not found</h5>
                                            </div>
                                        </div>
                                    </div>





                                </div>
                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary">Medication
                                                <asp:UpdatePanel runat="server" ID="updatepanelMedication" class="poupicon">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="ImageButton16" runat="server" CssClass="pull-right" Style="cursor: pointer;"
                                                            ImageUrl="~/Images/external-link-symbol.svg" OnClick="lnkPrescriptions_OnClick" title="Medication History" Height="16px" Width="16px" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%-- <a class="pull-right" style="cursor: pointer;" onclick="Prescriptions()">--%>
                                                <span data-toggle="tooltip" data-placement="right" title="Medication History">
                                                    <%--<img src="/Images/external-link-symbol.svg" />--%>
                                                </span></h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder2" style="display: none;" class="img hidden" />
                                            <table class="table" id="tbl_medicine" style="display: none">
                                                <thead>
                                                    <tr>
                                                        <th>Medicine</th>
                                                        <th>Prec Dtl</th>
                                                        <th>Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg2" style="display: none;">
                                                <h5 class="msg_text">Record not found</h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary">Radiology Investigation
                                                <%--<asp:UpdatePanel runat="server" ID="updatepanel28" class="poupicon">
                                                <ContentTemplate>
                                             <asp:ImageButton ID="ImageButton19" runat="server" CssClass="pull-right" style="cursor: pointer;" 
                                              ImageUrl="~/Images/external-link-symbol.svg" OnClick="lnkLabOrders_OnClick" title="Radiology Investigation History" Height="16px" Width="16px" />
                                              </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                                <a class="pull-right" style="cursor: pointer;" onclick="LabOrders('RAD')"><span data-toggle="tooltip" data-placement="left" title="Radiology Investigation History">
                                                    <img src="/Images/external-link-symbol.svg" /></span></a>
                                            </h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder3" style="display: none;" class="img hidden" />
                                            <table class="table" id="tbl_Radiology" style="display: none">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg3" style="display: none;">
                                                <h5 class="msg_text">Record Not Found</h5>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary">Lab Investigation
                                                
                                             

                                                 <a class="pull-right" style="cursor: pointer;" onclick="LabOrders('LAB');"><span data-toggle="tooltip" data-placement="left" title="Lab Order History">
                                                     <img src="/Images/external-link-symbol.svg" /></span></a>

                                            </h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder4" style="display: none;" class="img hidden" />
                                            <table class="table" id="tbl_lab" style="display: none;">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg4" style="display: none;">
                                                <h5 class="msg_text">Record not found</h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary">Chief Complaints
                                                 <asp:UpdatePanel runat="server" ID="updatepanelchiefcomplaint" class="poupicon">
                                                     <ContentTemplate>
                                                         <asp:ImageButton ID="ImageButton15" runat="server" CssClass="pull-right" Style="cursor: pointer;"
                                                             ImageUrl="~/Images/external-link-symbol.svg" OnClick="lnkChiefComplaint_OnClick" title="Chief Complaints" Height="16px" Width="16px" />
                                                     </ContentTemplate>
                                                 </asp:UpdatePanel>
                                                <%-- <a class="pull-right" onclick="ChiefComplaints();" style="cursor: pointer;">--%>
                                                <span data-toggle="tooltip" data-placement="right" title="Chief Complaint History">
                                                    <%--<img src="/Images/external-link-symbol.svg" />--%>

                                                </span></h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder5" style="display: none;" class="img hidden" />
                                            <table class="table" id="tbl_ChifComplaints" style="display: none;">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg5" style="display: none;">
                                                <h5 class="msg_text">Record not found</h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">

                                            <h5 class="card-title  bg-primary">Immunization
                                                <asp:UpdatePanel runat="server" ID="updatepanel23" class="poupicon">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="ImageButton17" runat="server" CssClass="pull-right" Style="cursor: pointer;"
                                                            ImageUrl="~/Images/external-link-symbol.svg" OnClick="lnkImmunizationHistory_OnClick" title="Immunization History" Height="16px" Width="16px" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%--  <a class="pull-right" style="cursor: pointer;" onclick="Immunization()">--%>
                                                <span data-toggle="tooltip" data-placement="left" title="Immunization History">
                                                    <%--<img src="/Images/external-link-symbol.svg" />--%>

                                                </span></h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder7" style="display: none;" class="img hidden" />
                                            <table class="table" id="tbl_imu" style="display: none">
                                                <thead>
                                                    <tr>
                                                        <th>Immunization
                                                        </th>
                                                        <th>Due Date</th>
                                                        <th>Given Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg7" style="display: none;">
                                                <h5 class="msg_text">Immunization not required for this patient</h5>
                                            </div>
                                        </div>
                                    </div>





                                </div>
                                <div class="col-md-6 box">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title bg-primary">Past History
                                               <%-- <a class="pull-right" style="cursor: pointer;" onclick="PastHistory();">--%>
                                                <asp:UpdatePanel runat="server" ID="updatepanelpasthistory" class="poupicon">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="ImageButton18" runat="server" CssClass="pull-right" Style="cursor: pointer;"
                                                            ImageUrl="~/Images/external-link-symbol.svg" OnClick="lnkPastHis_OnClick" title="Past History" Height="16px" Width="16px" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <span data-toggle="tooltip" data-placement="left" title="Past History">
                                                    <%--  <img src="/Images/external-link-symbol.svg" />--%>

                                                </span></h5>
                                            <img src="/Images/ajax-loader3.gif" id="loder6" style="display: none;" class="img hidden" />
                                            <table class="table" id="tbl_Past_History" style="display: none;">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div id="msg6" style="display: none;">
                                                <h5 class="msg_text">Record not found</h5>
                                            </div>
                                            <asp:ImageButton ID="imgPopupHIS" runat="server" ImageUrl="~/Images/add.gif" Height="20px" Width="20px"
                                                data-toggle="tooltip" title="Past History" data-placement="left" Style="visibility: hidden;"
                                                OnClick="lnkPastHistory_OnClick" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>




            <div class="col-md-1 grey-box">
                <ul class="right-icons blue-box">
                    <%-- <li><a id="lnkChifComplaints" onclick="ChiefComplaints();" style="cursor: pointer" data-toggle="tooltip" data-placement="top" title="Chief Complaints">
                        <span class="fas fa-notes-medical"></span></a></li>--%>
                    <li class="setingli"><a href="#" onclick="openNav()" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Setting"><i class="fa fa-cog fa-spin"></i></a></li>
                    <li><a onclick="PastClinicalNotes();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Past Clinical Notes">
                        <img src="/Images/icon/note.svg"></a></li>

                    <li><a onclick="Vitals();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Vitals">
                        <img src="/Images/icon/vital-icon.svg"></a></li>
                    <%--<li><a onclick="GrowthChart();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Growth Chart">
                        <img src="/Images/icon/growth.svg"></a></li>--%>



                    <%-------------------YOGESH---------------------   21/04/22   --%>
                    <asp:UpdatePanel ID="upRad" runat="server">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false" Style="height: 100vh !important;">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow2" CssClass="HealthCheckupPage" OpenerElementID="btnPrintPdf" runat="server" Style="height: 100vh !important;" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-------------------YOGESH---------------------   21/04/22   --%>






                    <asp:UpdatePanel runat="server" ID="UpdatePanel27">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="imgBtGrowthChart" runat="server" ImageUrl="~/Images/icon/growth.svg"
                                    data-toggle="tooltip" title="Growth Chart" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtGrowthChart_OnClick" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="UpdatePanelAttachment">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="ImgBtnAttachment" runat="server" ImageUrl="~/Images/icon/attachment.svg"
                                    data-toggle="tooltip" title="Patient Document" data-placement="left" Height="20px" Width="20px" OnClick="ImgBtnAttachment_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <%--<li><a onclick="Attachment();" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Attachment">
                        <img src="/Images/icon/attachment.svg"></a></li>--%>
                    <%-- <li><a onclick="RIS();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Radiology Results">
                        <img src="/Images/icon/fan.svg"></a></li>--%>

                    <%-- <li><a onclick="LIS();" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Lab Results">
                        <img src="/Images/icon/lab.svg"></a></li>--%>

                    <asp:UpdatePanel runat="server" ID="UpdatePanel25">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="imgBtRLIS" runat="server" ImageUrl="~/Images/icon/fan.svg"
                                    data-toggle="tooltip" title="Radiology Results" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtRLIS_OnClick" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="imgBtnLIS" runat="server" ImageUrl="~/Images/icon/lab.svg"
                                    data-toggle="tooltip" title="Lab Results" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtnLIS_OnClick" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="UpdateimgBtnAddCaseSheet">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="imgBtnAddCaseSheet" runat="server" ImageUrl="~/Images/icon/google-docs.svg"
                                    data-toggle="tooltip" title="Case Sheet" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtnAddCaseSheet_OnClick" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="UpdateimgBtnAddReferralRequest">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="imgBtnAddReferralRequest" runat="server" ImageUrl="~/Images/icon/referral.svg"
                                    data-toggle="tooltip" title="Referral Request" data-placement="left" Height="20px" Width="20px" OnClick="lnkimgBtnAddReferralRequest_OnClick" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <asp:UpdatePanel runat="server" ID="updateAdmissionAdvice">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="imgBtnAddAdmissionAdvice" runat="server" ImageUrl="~/Images/icon/front-desk.svg"
                                    data-toggle="tooltip" title="Admission Advice" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddAdmissionAdvice_OnClick" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <%--<li><a onclick="caseSheet();" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Case Sheet">
                        <img src="/Images/icon/google-docs.svg"></a></li>--%>

                    <li class="hidden"><a onclick="ProviderDashBord();" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Provider DashBord"><i class="fa fa-fa fa-user-md"></i></a></li>
                    <li class="hidden"><a style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Video"><i class="fa fa-video"></i></a></li>
                    <li class="hidden"><a style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Microphone"><i class="fas fa-microphone-alt"></i></a></li>

                    <asp:UpdatePanel runat="server" ID="UpdatePanelOTRequest">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="ImageButtonOTRequest" runat="server" ImageUrl="~/Images/icon/OT.png"
                                    data-toggle="tooltip" title="OT Request" data-placement="left" Height="20px" Width="20px" OnClick="ImageButtonOTRequest_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <%-- <li class=""><a onclick="OTRequest()" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="OT Request">
                        <img src="/Images/icon/mask.svg"></a></li>--%>
                    <%-- <li class=""><a onclick="ReferralRequest()" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Referral Request">
                        <img src="/Images/icon/referral.svg"></a></li>--%>


                    <%-- <li class=""><a onclick="AdmissionAdvice()" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Admission Advice">
                        <img src="/Images/icon/front-desk.svg"></a></li>
                    --%>

                    <li class="hidden"><a style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="OT Scheduler/Booking">
                        <img src="/Images/icon/calendar.svg"></a></li>
                    <%-- <li class=""><a onclick="ProgressNote()" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Progress Note">
                        <img src="/Images/icon/DoctorNote.svg"></a></li>--%>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelProgressNote">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="ImageButtonProgressNote" runat="server" ImageUrl="~/Images/icon/DoctorNote.svg"
                                    data-toggle="tooltip" title="Progress Note" data-placement="left" Height="20px" Width="20px" OnClick="ImageButtonProgressNote_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <li class="anim" style="display: none;"><a onclick="DischargeSummary()" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Discharge Summary">
                        <img src="/Images/icon/dischage.svg"></a></li>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelFollowUpAppointment">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="ImageButtonFollowUpAppointment" runat="server" ImageUrl="~/Images/Follow-Up-Appointment.png"
                                    data-toggle="tooltip" title="Follow Up Appointment" data-placement="left" Height="20px" Width="20px" OnClick="ImageButtonFollowUpAppointment_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-- Added New Menus Here Start--%>
                    <%--Vishal--%>
                        <asp:UpdatePanel runat="server" ID="UpdatePanelDischargeSummary">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="ImageButtonDischargeSummary" runat="server" ImageUrl="~/Images/LabReport.png"
                                    data-toggle="tooltip" title="Discharge Summary" data-placement="left" Height="20px" Width="20px" OnClick="ImageButtonDischargeSummary_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelMedicalIllustration">
                        <ContentTemplate>
                            <li class="anim">
                                <asp:ImageButton ID="ImageButtonMedicalIllustration" runat="server" ImageUrl="~/Images/success.png"
                                    data-toggle="tooltip" title="Medical Illustration" data-placement="left" Height="20px" Width="20px" OnClick="ImageButtonMedicalIllustration_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                      <%-- Added New Menus Here Close--%>  



                    <%-------------------YOGESH---------------------   21/04/22   --%>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel30">
                        <ContentTemplate>
                            <li class="">
                                <asp:ImageButton ID="imgBtnRis" runat="server" ImageUrl="~/Images/icon/RIS_Report_Print.png"
                                    data-toggle="tooltip" title="RIS Report Print" data-placement="left" Height="20px" Width="20px" Visible="false" OnClick="imgBtnRis_Click" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-------------------YOGESH---------------------   21/04/22   --%>

                    <asp:UpdatePanel runat="server" ID="UpdatePanelOpenDmsTab31">
                        <ContentTemplate>
                            <li class="">
                                <asp:LinkButton ID="ImageButtonDms" runat="server" Text="DMS" ForeColor="Black"
                                    data-toggle="tooltip" title="DMS" data-placement="left" Visible="false"   OnClientClick ="OpenDmsTab()" /></li>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ul>


            </div>

            <div id="MessageModel" class="modal fade uhid-modal in" role="dialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" id="btnCloseMessageModel">&times;</button>
                            <h4 class="modal-title text-primary">Inbox</h4>
                        </div>

                        <div class="container-fluid">


                            <div class="modal-body message-modal">

                                <ul class="nav nav-pills">
                                    <li id="liMessage" class="active">
                                        <a data-toggle="pill" href="#home" id="btnMsg">Message</a>

                                    </li>
                                    <li id="liChat">
                                        <a data-toggle="pill" href="#chat" id="idchat">Chat</a>

                                    </li>

                                </ul>

                                <div class="tab-content">
                                    <div id="home" class="tab-pane fade in active">
                                        <div class="table-responsive table-condensed" style="overflow-y: scroll; height: 300px;">
                                            <table id="tblMessage" class="table no-margin table-hover table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Reg No</th>
                                                        <th>Visit No</th>
                                                        <th>Name</th>
                                                        <th>Visit Type</th>
                                                        <th>Message</th>
                                                        <th>Reply</th>
                                                    </tr>
                                                </thead>
                                                <tbody></tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="chat" class="tab-pane fade">

                                        <div class="container-fluid row">
                                            <div class="form-group col-md-6">
                                                <label>Patient</label>
                                                <input type="text" id="txtpatient" placeholder="" class="form-control" readonly="readonly" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label>Visit No</label>
                                                <input type="text" id="txtvisit" placeholder="" class="form-control" readonly="readonly" />

                                            </div>
                                            <div class="form-group col-md-2">

                                                <a href="javascript:void(0)" class="btn btn-primary btn-sm" onclick="CaseSheetAtChat()" style="margin-top: 25px;"><i class="fa fa-file"></i>Case Sheet</a>

                                            </div>
                                        </div>

                                        <div class="container-fluid chat-box">


                                            <div id="chatbox" style="margin-top: 10px;">
                                            </div>

                                            <div class="row">


                                                <div class="form-group col-md-12">
                                                    <label for="comment">Message:</label>
                                                    <textarea class="form-control" rows="5" id="Message" maxlength="500"></textarea>

                                                </div>

                                                <div class="col-md-3">
                                                    <a href="javascript:void(0)" id="btnSend" class="btn btn-primary" onclick="return sendMessage();">Send</a>

                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" id="btnCloseMessageModel">Close</button>
                </div>--%>
                            </div>
                        </div>



                    </div>
                </div>

            </div>


            <div class="modal fade uhid-modal in" role="dialog" id="msgModal">
                <div class="modal-dialog modal-sm">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <%-- <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"  onclick="messageclose();">&times;</button>
                    <h4 class="modal-title">Modal Header</h4>
                </div>--%>

                        <div class="modal-body">
                            <div>
                                <i class="fa fa-check"></i>
                            </div>
                            <p id="pMsg"></p>
                            <button type="button" class="btn btn-link btn-sm" data-dismiss="modal" onclick="messageclose()"><i class="fa fa-window-close"></i></button>
                        </div>
                        <%--<div class="modal-footer">
                    
                </div>--%>
                    </div>

                </div>
            </div>


            <asp:UpdatePanel runat="server" ID="UpdatePanel8" class="view-mode">
                <ContentTemplate>
                    <div id="VitalValue" class="VitalValue custom-scroller custom-scroller-light red" runat="server" visible="false" style="background-color: #fff;">

                        <div class="col-xs-6">
                            <h2>Vital Values</h2>
                        </div>


                        <div class="col-xs-6 text-right">
                            <asp:LinkButton ID="lnkCloseVital" runat="server" CssClass="btn btn-xs btn-danger text-right" OnClick="lnkCloseVital_Click">X</asp:LinkButton>

                        </div>
                        <hr />

                        <div class="container-fluid table-responsive">
                            <asp:GridView ID="GridVitalValue" runat="server" CssClass="table table-condensed table-bordered table-striped " AutoGenerateColumns="false" ShowHeader="true" OnRowCommand="GridVitalValue_RowCommand" OnRowDataBound="GridVitalValue_RowDataBound">
                                <Columns>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <tr>
                                                <th colspan="3" class="text-center">Distance</th>
                                                <th colspan="2" class="text-center">Near</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <td>
                                                <asp:LinkButton ID="lnk" CommandName="Vision_D" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_D")%>'></asp:LinkButton>

                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk1" CommandName="Vision_DP" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_DP")%>'></asp:LinkButton>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lnk2" CommandName="Vision_N" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_N")%>'></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk3" CommandName="Vision_NP" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_NP")%>'></asp:LinkButton>
                                            </td>

                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Vision_DP">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk1" CommandName="Vision_DP" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_DP")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:TemplateField HeaderText="Vision_N">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk2" CommandName="Vision_N" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_N")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%-- <asp:TemplateField HeaderText="Vision_NP">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk3" CommandName="Vision_NP" CssClass="btn btn-xs btn-default" runat="server" Text='<%#Eval("Vision_NP")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div id="mySidepanel" class="sidepanel">
            <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">×</a>
            <div style="padding: 0 10px;">
                <h4 style="color: #000; font-size: 14px; font-weight: bold; margin: 0 0 20px">Template Setting</h4>
                <div class="row">
                    <asp:UpdatePanel ID="updateTemplateSetting" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanalTemplateSetting" runat="server">
                                <div class="col-md-12 form-group">
                                    <div class="row">
                                        <div class="col-md-6" style="color: #333;">All Expand</div>
                                        <div class="col-md-6 text-right">
                                            <label class="switch">
                                                <%--<input type="checkbox" id="checkAll">--%>
                                                <asp:CheckBox runat="server" ID="checkAll" AutoPostBack="true" OnCheckedChanged="checkAll_CheckedChanged" />
                                                <span class="slider round"></span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 form-group">
                                    <div class="row">
                                        <div class="col-md-6" style="color: #333;">Customize</div>
                                        <div class="col-md-6 text-right">
                                            <label class="switch">
                                                <%--<input type="checkbox" id="checkCustomize">--%>
                                                <asp:CheckBox runat="server" ID="checkCustomize" AutoPostBack="true" OnCheckedChanged="checkCustomize_CheckedChanged" />
                                                <span class="slider round"></span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div>
                    <h4 style="color: #000; font-size: 14px; font-weight: bold; margin: 0 0 10px">Other Setting</h4>

                    <div class="">
                        <div class="row">
                            <div class="col-md-8" style="color: #333;">Customize Setting </div>
                            <div class="col-md-4 text-right" style="color: #333;">
                                <a onclick="TemplateSetting();" style="cursor: pointer; padding: 0 !important;"
                                    data-toggle="tooltip" data-placement="left" title="Customize Setting">
                                    <span class="fa fa-wrench"></span></a>
                            </div>
                        </div>
                    </div>


                </div>

            </div>
        </div>
        
    </div>
    
    <script src="/Include/JS/StringBuilder.js"></script>


    <script>
        function openNav() {
            document.getElementById("mySidepanel").style.width = "250px";
        }

        function closeNav() {

            document.getElementById("mySidepanel").style.width = "0";
        }

        function cmbProblemName_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            var SnomedCode = item.get_attributes().getAttribute("SNOMEDCode");
            $get('<%=prblmID.ClientID%>').value = item.get_value();

        }
    </script>
    <script type="text/javascript">
        window.scrollTo = function () { }
        window.onload = setTimeout;
        var str ="";
        let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';
        $(document).ready(function () {
            debugger
            var QueryData = '<%=Session["CountQueryData"]%>';
            if (QueryData !='')
            {
                $('#AlertnotificationId').html(QueryData);
            }
            else
            {
                $('#AlertnotificationId').html(0);
            }       
            GetPatientDetail(); 
            MessageNotification();  
            // CheckIn(0);
            $('#AlertnotificationId').click(function(){                    
                //QueryResponse();
                //return;
                 
                let obj = new Object();
                obj.Qid =0;
                obj.Employeeid =  <%=Session["EmployeeId"]%>; 
              
                $.ajax({
                    url: url + "api/Common/GetGfsJQueryDoc",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (res) { 
                            
                            
                        let tableData = res;
                        let sb = new StringBuilder();                    
                        $("#tblNotificationAlert").find("tr").remove();
                        
                        if (tableData.length) {

                            $('#rnf1').css('display','none');
                            $('#ldrQuery').css('display','none');
                                
                            for (var i = 0; i < tableData.length; i++) {
               
                                sb.append('<tr ><td>'+ tableData[i].UHID +'</td><td>'+ tableData[i].Query +'</td>')
                                sb.append("<td><a onclick='QueryResponse(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-eye'></i> View</a></td></tr>")                            
                            }

                            $('#tblNotificationAlert').append(sb.toString());
                        }
                        else
                        {
                            $('#ldrQuery').css('display','none');
                            $('#rnf1').css('display','block');
                        }

                          
                    }

                })

            });
            $('#LabNotificationId').click(function(){
                    
                //LabResults(); 
                //return
                //kuldeep123
                let obj = new Object();                
                obj.iLoginFacilityId= '<%=Session["FacilityId"]%>';
                obj.iHostId= '<%=Session["HospitalLocationId"]%>';
                obj.fromDate = '';
                obj.toDate='';
                obj.iRegNo='';
                obj.iProviderId = '<%=Session["EmployeeId"]%>';
                obj.iPageSize=15;
                obj.iPageNo=1;
                obj.AbnormalResult = false;
                obj.CriticalResult  = false;
                obj.iStatusId = 0;
                obj.FacilityId=  '<%=Session["FacilityId"]%>'; 
                obj.chvEncounterNo = '<%=Session["EncounterNo"]%>'; 
                obj.ReviewedStatus = 0 ;
                obj.PatientName='';
                obj.iUserId ='<%=Session["UserId"]%>';
              
                $.ajax({
                    url: url + "api/EMRAPI/getPatientJLabResultHistoryDash",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (res) { 
                                                       
                        // alert(JSON.stringify(res));
                        //return;
                        let tableData = res;
                        let sb = new StringBuilder();                    
                        $("#tblLabNotification").find("tr").remove();                        
                        if (tableData.length)
                        {

                            $('#rnf2').css('display','none');
                            $('#ldrLab').css('display','none');
                            for (var i = 0; i < tableData.length; i++) {
               
                                sb.append('<tr ><td>'+ tableData[i].RegistrationNo +'</td><td>'+ tableData[i].ServiceName +'</td>')
                                sb.append("<td><a onclick='LabResults(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-eye'></i> View</a></td></tr>")
                                    
                       
                            }

                            $('#tblLabNotification').append(sb.toString());
                        }
                        else
                        {
                            $('#ldrLab').css('display','none');
                            $('#rnf2').css('display','block');
                        }

                          
                    }

                })

            
            });

            EmrTemplateIcone();
        });
            
        function IsCheckinHospitalsetup()
        {
            let obj = new Object();
            obj.HospitalLocationId = <%=Session["HospitalLocationId"]%>;
            obj.FacilityID = <%=Session["FacilityID"]%>;
            obj.Encounterid= <%=Session["EncounterId"]%>; 
            obj.Flag = "IsCheckinHospitalPatient";
            $.ajax({
                url: url + "api/Common/getHospitalSetupValueAjax",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) { 
                                                   

                }
            });
        }

        function OpenDmsTab()
        {
            var url ="http://mission.ezeefile.in/patientDataRetrival?valid=cTl6OGk4bXJNd3hqbkNBcTF2T05wMHg4Ujh4VUZrYWVQK0tINFBOS21JOD0ezeeFLie&p=1&mr_no="+<%=Session["RegistrationNo"]%>+"";
            
            window.open(url)
        }
        function CheckIn(flag)
        {        
            let obj = new Object();
            obj.HospitalLocationId = <%=Session["HospitalLocationId"]%>;
            obj.FacilityID = <%=Session["FacilityID"]%>;
            obj.Encounterid= <%=Session["EncounterId"]%>; 
            obj.Flag = 'IsCheckinHospitalPatient';
            $.ajax({
                url: url + "api/Common/getHospitalSetupValueAjax",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) { 
                                                   
                    if (res == 'Y')
                    {
                        obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;
                        obj.FacilityID = <%=Session["FacilityID"]%>;
                        obj.Encounterid= <%=Session["EncounterId"]%>; 
                        obj.EncodedBy =  <%=Session["UserId"]%>; 
                        obj.flag = flag
                        $.ajax({
                            url: url + "api/EMRAPI/SetPatinetCheckIn",
                            type: 'post',
                            dataType: 'json',
                            data: obj,
                            success: function (res) 
                            { 
                                if (res == '"CheckedIn"')
                                {
                                    $('#lnkCheckIn').val('Checked In');
                                    $('#lnkCheckIn').addClass('btn btn-success');
                            
                                }
                                else
                                {
                                    var r = confirm("Do you want patient Checked In !!");

                                    if (r == true) 
                                    {
                                        CheckIn(1);

                                    } else 
                                    {
                                        return;
                                    }
                            

                                }
                      
                            }

                        })
                    }
                    else
                    {
                        $('#lnkCheckIn').css('display','none');
                        return;
                    }
                }
            });


        }
        //document.addEventListener('DOMContentLoaded', function () {
        //    // your code here
        //    BindPatientDiagnosis();
        //}, false);

        setTimeout(function () {
            BindPatientDiagnosis();           
        }, 100);

        function BindPatientDiagnosis() {
            //alert('kuldeep');
            let sb = new StringBuilder();
            let sb1 = new StringBuilder();
            let obj = new Object();
            obj.Registraionid = <%=Session["RegistrationId"]%>;
            obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;;
            //obj.Encounterid= <%=Session["EncounterId"]%>; 
            $.ajax({
                url: url + "api/EMRAPI/GetPatientDiagnosisPreHistory",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    let tableData = res;
                    //alert(JSON.stringify(res));
                    // $('#tbl_diagnosis').html('');
                    $("#tbl_diagnosis").find("tr:gt(0)").remove();
                    debugger;
                    for (var i = 0; i < tableData.length; i++)
                    {
                        if (tableData[i].IsChronic == true || tableData[i].IsChronic)
                        {   
                            sb1.append('<li style="display:inline-block">'+tableData[i].Description+'</li>');
                        }   
                        //yogesh 27-09-2022 
                        sb.append('<tr><td>'+ tableData[i].Description +'</td>');                    
                        sb.append('<td>'+tableData[i].VisitType+'</td>'); 
                        if(tableData[i].IsPrimaryDiagnosis == true)
                        {
                            sb.append("<td><a onclick='primaryDiagAlert("+JSON.stringify(tableData[i].Description)+")' Primary'>Primary</a>"+ " "+ tableData[i].Date +"</td>");
                        }
                        else if(tableData[i].IsFinalDiagnosis == true)
                        {
                            sb.append("<td><a onclick='finalDiagAlert("+JSON.stringify(tableData[i].Description)+")' Primary'>Final</a>"+ " "+ tableData[i].Date +"</td>");
                            sb.append('<td>'+'Final'+' '+tableData[i].Date+'</td></tr>');
                        }
                        else
                        {
                            sb.append('<td>'+tableData[i].Date+'</td></tr>');
                        }
  
                      
                    }    
                    $("#cronics").find("li").remove();
                    if(sb1.toString().length)
                    {
                        $('#nocronic').css('display','none'); 
                        $('#hdrcronic').css('display','block'); 
                        $('#cronics').append(sb1.toString());
                    }
                    else
                    {
                        $('#nocronic').css('display','block'); 
                    }
                        

                    str = sb.toString();                    
                    if(str.length)
                    {
                        $("#tbl_diagnosis > tbody:last").children().remove();
                        $('#tbl_diagnosis tbody').append(sb.toString());
                        $('#loder1').css('display', 'none');
                        $('#tbl_diagnosis').css('display', 'block');
                        $('#msg1').css('display','none');
                    }
                    else
                    {
                        $('#msg1').css('display','block');
                        $('#loder1').css('display', 'none');
                    }                   
                   
                    
                    $('#loder2').css('display', 'block');
                    setTimeout(function () {
                        BindPatientMedication();
                       
                    }, 50);
                }

            })
            CheckIn(0);
        }

        //yogesh  New UI 27-09-2022
        function primaryDiagAlert(desc)
        {
           
            Swal.fire(
            'Primary Diagnosis!',
            desc,
            'info'
                )

        }
        //yogesh New UI 27-09-2022
        function finalDiagAlert(desc)
        {
            Swal.fire(
           'Final Diagnosis!',
           desc,
           'info'
               )
        }

        function BindPatientMedication() {           
            str="";
            let sb = new StringBuilder();
            let obj = new Object();
            obj.Registraionid = <%=Session["RegistrationId"]%>;
            obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;;
            obj.Encounterid= <%=Session["EncounterId"]%>; 
            $.ajax({
                url: url + "api/EMRAPI/GetPatientDrugOrder",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    let tableData = res;
                    // $('#tbl_diagnosis').html('');
                    $("#tbl_medicine").find("tr:gt(0)").remove();
                    for (var i = 0; i < tableData.length; i++) {
                        sb.append('<tr><td>'+ tableData[i].Medicine +'</td>');                    
                        sb.append('<td>'+tableData[i].Detail+'</td>');
                        sb.append('<td>'+tableData[i].Date+'</td></tr>');
                        
                    }
                    str = sb.toString();                    
                    if(str.length)
                    {
                        $("#tbl_medicine > tbody:last").children().remove();
                        $('#tbl_medicine tbody').append(sb.toString());
                        $('#loder2').css('display', 'none');
                        $('#tbl_medicine').css('display', 'block');
                        $('#msg2').css('display','none');
                    }
                    else
                    {
                        $('#loder2').css('display', 'none');
                        $('#msg2').css('display','block');
                    }
                   
                   
                    $('#loder3').css('display', 'block');
                    setTimeout(function () {
                        BindPatientLabOrder();
                    }, 50);
                }

            })
        }
        
            
        function over() {
            $('[data-toggle="tooltip"]').tooltip();
        }
        function out() {
            document.write ("Mouse Out");
        }
      
        function BindPatientLabOrder() {
            
            let sbLIS = new StringBuilder();
            let sbRIS = new StringBuilder();
            let obj = new Object();
            obj.Registraionid = <%=Session["RegistrationId"]%>;
            obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;;
            obj.Encounterid = <%=Session["EncounterId"]%>; 
           
            $.ajax({
                url: url + "api/EMRAPI/GetPatientOrderProcedure",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    let tableData = res;
                    //alert(JSON.stringify(res));

                    // $('#tbl_diagnosis').html('');
                    $("#tbl_lab").find("tr:gt(0)").remove();
                    $("#tbl_Radiology").find("tr:gt(0)").remove();
                    for (var i = 0; i < tableData.length; i++) {                                              
                           
                        if(tableData[i].FlagName =="LIS")
                        {
                           
                            if(tableData[i].StatusCode == "RF")
                            {
                                sbLIS.append("<tr><td><a onclick='PrintLabReport(this,"+JSON.stringify(tableData[i])+")' onmouseover='over()' style='display: block;font-weight: bold;' data-toggle='tooltip' data-placement='top' title='Print / Download'> "+ tableData[i].ServiceName +"</a></td>");
                                
                            }
                              
                            else
                            {
                                sbLIS.append('<tr><td>'+ tableData[i].ServiceName +'</td>'); 
                            }
                            sbLIS.append('<td>'+tableData[i].Date+'</td></tr>');
                        }
                        else
                        {
                            
                            
                            
                            if(tableData[i].StatusCode == "RF")
                            {
                                sbRIS.append("<tr><td><a onclick='PrintLabReport(this,"+JSON.stringify(tableData[i])+")' onmouseover='over()' style='display: block;font-weight: bold;' data-toggle='tooltip' data-placement='top' title='Print / Download'> "+ tableData[i].ServiceName +"</a></td>");
                                
                            }
                            else
                            {
                                sbRIS.append('<tr><td>'+ tableData[i].ServiceName +'</td>'); 
                            }
                            sbRIS.append('<td>'+tableData[i].Date+'</td></tr>');
                        }                            
                            
                    }
                    var strLIS = sbLIS.toString();   
                    var strRIS = sbRIS.toString();   
                    if(strLIS.length)
                    {   // alert('kuldeep');
                        // $("#tbl_lab tbody").find("tr:gt(0)").remove();
                        $("#tbl_lab > tbody:last").children().remove();
                        $('#tbl_lab tbody').append(strLIS.toString());
                        $('#loder4').css('display', 'none');
                        $('#tbl_lab').css('display', 'block');
                        $('#msg4').css('display','none');
                    }
                    else
                    {
                        $('#loder4').css('display', 'none');
                        $('#msg4').css('display','block');
                    }
                   
                    $('#loder5').css('display', 'block');

                    if(strRIS.length)
                    {   // alert('kuldeep');
                        // $("#tbl_lab tbody").find("tr:gt(0)").remove();
                        $("#tbl_Radiology > tbody:last").children().remove();
                        $('#tbl_Radiology tbody').append(strRIS.toString());
                        $('#loder3').css('display', 'none');
                        $('#tbl_Radiology').css('display', 'block');
                        $('#msg3').css('display', 'none');
                        
                    }
                    else
                    {
                        $('#loder3').css('display', 'none');
                        $('#msg3').css('display','block');
                    }

                    setTimeout(function () {
                        BindPatientChifComplaints();
                    }, 50);
                }

            })
        }

        function BindPatientChifComplaints() {
          
            let sb = new StringBuilder();
            let obj = new Object();
            obj.Registraionid = <%=Session["RegistrationId"]%>;
            obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;;
            obj.Encounterid= <%=Session["EncounterId"]%>; 
            $.ajax({
                url: url + "api/EMRAPI/GetPatientChifComplaints",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    let tableData = res;
                    // $('#tbl_diagnosis').html('');
                    $("#tbl_ChifComplaints").find("tr:gt(0)").remove();
                    for (var i = 0; i < tableData.length; i++) {
                        sb.append('<tr><td>'+ tableData[i].ProblemDescription +'</td>');                    
                        sb.append('<td>'+tableData[i].EntryDate+'</td></tr>');
                    }
                    str = sb.toString();   
                    if(str.length)
                    {
                        $("#tbl_ChifComplaints > tbody:last").children().remove();
                        $('#tbl_ChifComplaints tbody').append(sb.toString());
                        $('#loder5').css('display', 'none');
                        $('#tbl_ChifComplaints').css('display', 'block');
                        $('#msg5').css('display','none');
                    }
                    else
                    {
                        $('#loder5').css('display', 'none');
                        $('#msg5').css('display','block');
                        
                    }
                    $('#loder6').css('display', 'block');
                    setTimeout(function () {
                        BindPatientImmunization();
                    }, 50);
                    
                }

            })
        }
        function BindPatientImmunization() 
        {
               
            str="";
            let sb = new StringBuilder();
            let obj = new Object();
            obj.Registraionid = <%=Session["RegistrationId"]%>;
            obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;;
            obj.dob= localStorage.getItem('dob'); 
            var year = obj.dob;
            year = year.slice(year.length -4);
            var d = new Date();
            var n = d.getFullYear();
            var age = parseInt(n) - parseInt(year);
            //palendra
            document.getElementById('<%=hdnAgeLimitImmulization.ClientID %>').value=age;
            //palendra
            if (age <= 17)
            {
                $.ajax({
                    url: url + "api/EMRAPI/GetPatientImmunizationDueDatesDashbord",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (res) {

                        //alert(JSON.stringify(res));
                        let tableData = res;
                        // $('#tbl_diagnosis').html('');
                        $("#tbl_imu").find("tr:gt(0)").remove();
                        for (var i = 0; i < tableData.length; i++) {
                            //added by bhakti
                            if(tableData[i].GivenDate!="")
                            {
                                sb.append('<tr><td>'+ tableData[i].ImmunizationName +'</td>');                    
                                sb.append('<td>'+tableData[i].Duedate+'</td>');
                                sb.append('<td>'+tableData[i].GivenDate+'</td></tr>');
                            }
                        }
                        str = sb.toString(); 
                      
                        if(str.length)
                        {
                           
                            $("#tbl_imu > tbody:last").children().remove();
                            $('#tbl_imu tbody').append(sb.toString());
                            $('#loder7').css('display', 'none');
                            $('#tbl_imu').css('display', 'block');
                            $('#msg7').css('display','none');
                        }
                        else
                        {
                          
                            $('#loder7').css('display', 'none');
                            //$('#msg7').css('display','block');
                        }

                   
                        $('#loder6').css('display', 'block');
                        setTimeout(function () {
                            BindPatientPastHistory();
                        }, 50);
                    }

                })
            }
            else
            {
                $('#loder7').css('display', 'none');
                //$('#msg7').css('display','block');
            }
            $('#loder6').css('display', 'block');
            setTimeout(function () {
                BindPatientPastHistory();
            }, 50);
        }
        function BindPatientPastHistory() {
               
            let sb = new StringBuilder();
            let obj = new Object();
            obj.Registraionid = <%=Session["EncounterId"]%>;           
            $.ajax({
                url: url + "api/EMRAPI/GetPatientPastHistory",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    let tableData = res;
                    // $('#tbl_diagnosis').html('');
                    $("#tbl_Past_History").find("tr:gt(0)").remove();
                    for (var i = 0; i < tableData.length; i++) {
                        sb.append('<tr><td>'+ tableData[i].ValueWordProcessor +'</td>');                    
                        sb.append('<td>'+tableData[i].EncodedDate+'</td></tr>');
                    }
                    str = sb.toString();   
                    if(str.length)
                    {
                        $("#tbl_Past_History > tbody:last").children().remove();
                        $('#tbl_Past_History tbody').append(sb.toString());
                        $('#loder6').css('display', 'none');
                        $('#tbl_Past_History').css('display', 'block');
                        $('#msg6').css('display','none');
                    }
                    else
                    {
                        $('#loder6').css('display', 'none');
                        $('#msg6').css('display','block');
                    }
                   
                    //setTimeout(function () {
                    //    $('#tbl_diagnosis tbody').append(sb.toString());
                    //    $('#loder1').css('display', 'none');
                    //    $('#tbl_diagnosis').css('display', 'block');
                    //}, 5000);
                }

            })
        }
    </script>
    <script>        
        $(document).ready(function(){
           
            $("#myNavbar a").on('click', function(event) {
                // Make sure this.hash has a value before overriding default behavior
                if (this.hash !== "") {
                    // Prevent default anchor click behavior
                    event.preventDefault();

                    // Store hash
                    var hash = this.hash;

                    // Using jQuery's animate() method to add smooth page scroll
                    // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
                    $('html, body').animate({
                        scrollTop: $(hash).offset().top
                    }, 800, function(){
   
                        // Add hash (#) to URL when done scrolling (default click behavior)
                        window.location.hash = hash;
                    });
                }  // End if
            });
            $('#Messageid').click(function () {          
               
                // alert('Modal Popup');
                MessageNotification();
                //  $('#MessageModel').show();
                $('#btnMsg').trigger('click');
                $('#home').addClass('active in');
                $('#chat').removeClass('active in');
             
                $('#liChat').css('display','none');

            });
            $('#btnCloseMessageModel').on('click',function(){
                $('#liMessage').addClass('active');
                $('#MessageModel').hide();
            }); 
            
            
            // DoctorPanleSeting();
            
        });
            
        function GetPatientAllergy()
        {
            var obj = new Object();
            obj.RegistrationNo = '<%=Session["RegistrationNo"]%>';
            obj.RegistrationId = '<%=Session["RegistrationID"]%>';
            obj.HospitalLocationId = '<%=Session["HospitalLocationId"]%>';
            obj.FacilityId = '<%=Session["FacilityId"]%>';
            $.ajax({
                url: url + "api/EMRAPI/GetPatientPatientAllergies",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (response) {
                    //alert (JSON.stringify(response));
                    let tableData = response;
                    let sb = new StringBuilder();

                    if(tableData.length)
                    {
                        for (var i = 0; i < tableData.length; i++) {
                            sb.append('<li>'+ tableData[i].Allergies+'</li>') 
                        }

                    }
                    else
                    {
                        sb.append('<li>No Allergy Found</li>') ;
                    }

                    $("#ulAllergy").empty();
                    $('#ulAllergy').append(sb.toString());
                    
                },
                error: function (response) {
                    console.log(response);
                }

            });

        }

        function GetPatientDetail()
        {
                
            var obj = new Object();
            obj.RegistrationNo = '<%=Session["RegistrationNo"]%>';
            obj.RegistrationId = '<%=Session["RegistrationID"]%>';
            obj.HospitalLocationId = '<%=Session["HospitalLocationId"]%>';
            obj.FacilityId = '<%=Session["FacilityId"]%>';
            $.ajax({
                url: url + "api/EMRAPI/GetSingleScreenPatientDetails",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (response) {

                    //alert (JSON.stringify(response));
                    $('.pt-name').html(response[0]["PatientName"]);
                    $('.pt-name').attr('title',response[0]["PatientName"]);
                    $('.pt-dtls').html(response[0]["GenderAge"] +", DOB " + response[0]["DOB"] + ", ID " + response[0]["RegistrationNo"] + "| ENC#:" + response[0]["EncounterNo"])
                    $('.visit-date').html('<%=Session["EncounterDate"]%>');   
                    localStorage.setItem('dob',response[0]["DOB"]);
                    
                },
                error: function (response) {
                    console.log(response);
                }

            });

            GetPatientAllergy();
                
            var EncounterType='<%=Session["OPIP"]%>';   
         
            if(EncounterType=='O' || EncounterType=='E')
            {
                $('#PatientVisits').removeClass('hidden');
                $('#PatientLable').removeClass('hidden');
            
                $.ajax({
                    url: url + "api/EMRAPI/GetSingleScreenPatientPastHistory",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (response) {                       
                       
                        // alert(JSON.stringify(response));
                        var date = '<%=Session["EncounterDate"]%>';  
                        var EncounterNo = '<%=Session["EncounterNo"]%>';                  
                        date = date.replace(/ [0-9]*[0-9]:[0-9][0-9]A*P*M/,'');                    
                        
                        let tableData = response;
                        let sb = new StringBuilder();
                        if(tableData.length > 1)
                        {
                            //$('.mid-box').toggleClass('collapsed1');
                            //$('.pt-dtls-block').toggleClass('collapsed1');
                            ////$('#btn-expand-collapse').trigger('click');
                            //expand();
                        }
                      
                        for (var i = 0; i < tableData.length; i++) {                       
                           
                            let active = tableData[i]["EncDate"];
                            let activeEncounterNo = tableData[i]["EncounterNo"];

                            if(date.trim()==active && EncounterNo==activeEncounterNo)
                            {
                                localStorage.setItem("EncDate",date);
                                localStorage.setItem("EncId",0);
                                sb.append("<li class='active'><a data-toggle='tab' href='#home"+ i +"' onclick='LastOPDVisit(this,"+JSON.stringify(tableData[i])+")'> "+ tableData[i].EncDay+"<span style='font-weight: bold; margin-left: 2px;'>"+ tableData[i].EncMonth.substring(0,3).toUpperCase()+'('+tableData[i].OPIP+')'+"</span> <span style='display: block; font-weight: bold; color: salmon;'>"+ tableData[i].DoctorName.toUpperCase()+"</span>  </a></li>")
                            }
                            else if(tableData[i].OPIP!='OP')
                            {
                                sb.append("<li class='active'><a data-toggle='tab' href='#home"+ i +"' onclick='LastDischargeSummary(this,"+JSON.stringify(tableData[i])+")'> "+ tableData[i].EncDay+"<span style='font-weight: bold; margin-left: 2px;'>"+ tableData[i].EncMonth.substring(0,3).toUpperCase()+'('+tableData[i].OPIP+')'+"</span> <span style='display: block; font-weight: bold; color: salmon;'>"+ tableData[i].DoctorName.toUpperCase()+"</span>  </a></li>")
                            }
                            else
                            {
                                sb.append("<li><a data-toggle='tab'  href='#home"+ i +"' onclick='LastOPDVisit(this,"+JSON.stringify(tableData[i])+")'> "+ tableData[i].EncDay+"<span style='font-weight: bold; margin-left: 2px;'>"+ tableData[i].EncMonth.substring(0,3).toUpperCase()+'('+tableData[i].OPIP+')'+"</span> <span style='display: block; font-weight: bold; color: salmon;'>"+ tableData[i].DoctorName.toUpperCase()+"</span> </a></li>")
                            }
                            //sb.append("<li><a data-toggle='tab' href='#home"+ i +"' onclick='LastOPDVisit(this,"+JSON.stringify(tableData[i])+")'> "+ tableData[i].EncDay+"<span style='font-weight: bold; display: block;'>"+ tableData[i].EncMonth.substring(0,3).toUpperCase()+"</span> <span style='font-weight: bold; color: salmon; display: none;'>"+ tableData[i].DoctorName.toUpperCase()+"</span> </a></li>")
                            
                       
                        }
                       
                        $("#content-slider").empty();

                        $('#content-slider').append(sb.toString());
                      
                        //let sb1 = new StringBuilder();
                        //var OPIP;

                        //for (var i = 0; i < tableData.length; i++) {
                       

                        //    if(i==0)
                        //    {
                        //        if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}

                        //        sb1.append('<div id="home'+ i +'"  class="tab-pane fade in active">')
                        //        sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
                        //        sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
                        //        sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
                        //        //sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
                        //        //sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i]) +")'><i class='fa fa-file-alt'></i></a></li>");
                        //        if (OPIP == "OP")
                        //        {
                        //            sb1.append("<li><span style='display: block;font-weight: bold;' data-toggle='tooltip' data-placement='top' title='Last OPD Summary'>Last OPD Summary</span><a onclick='LastOPDVisit(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
                        //        }
                        //        else
                        //        {
                        //            sb1.append("<li><span style='display: block;font-weight: bold;' data-toggle='tooltip' data-placement='top' title='Discharge Summary' >Discharge Summery</span><a onclick='DischargeSammary(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
                        //        }
                        //        sb1.append('</div>');
                        //    }
                        //    else
                        //    {
                        //        if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}
                    

                        //        sb1.append('<div id="home'+ i +'"  class="tab-pane fade">')
                        //        sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
                        //        sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
                        //        sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
                        //        //sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
                        //        //sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
                        //        if (OPIP == "OP")
                        //        {
                        //            sb1.append("<li><span style='display: block;font-weight: bold;' data-toggle='tooltip' data-placement='top' title='Last OPD Summary'>Last OPD Summary</span><a onclick='LastOPDVisit(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
                        //        }
                        //        else
                        //        {
                        //            sb1.append("<li><span style='display: block;font-weight: bold;' data-toggle='tooltip' data-placement='top' title='Discharge Summary' >Discharge Summery</span><a onclick='DischargeSammary(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
                        //        }
                        //        sb1.append('</div>');
                        //    }
                        //}
                    
                    
                   
                        //$("#tabPasthistory").empty();
                        //$('#tabPasthistory').append(sb1.toString());                  
                    
                    },
                    error: function (response) {
                        console.log(response);
                    }

                });
            }

            else
            {
                
                $('#PatientVisits').removeClass('hidden');
                $('#DoctorNote').removeClass('hidden');
                //$('.mid-box').toggleClass('collapsed1');
                //$('.pt-dtls-block').toggleClass('collapsed1');
                $.ajax({
                    url: url + "api/EMRAPI/GetDoctorProgressNoteForSingleScreen",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (response) {                       
                       
                        // alert(JSON.stringify(response));
                        var date = '<%=Session["EncounterDate"]%>';  
                        var EncounterNo = '<%=Session["EncounterNo"]%>';                  
                        date = date.replace(/ [0-9]*[0-9]:[0-9][0-9]A*P*M/,'');                    
                        
                        let tableData = response;
                        let sb = new StringBuilder();
                        if(tableData.length == 1)
                        {
                            //$('.mid-box').toggleClass('collapsed1');
                            //$('.pt-dtls-block').toggleClass('collapsed1');
                            ////$('#btn-expand-collapse').trigger('click');
                            //expand();
                        }

                        //for (var key in tableData.) {
                        //    if (tableData.hasOwnProperty(key))
                        //    {
                        //        alert(tableData[key].EnteredDate);
                        //    }
                        //}



                        for (var i = 0; i < tableData.length; i++) 
                        {    
                            sb.append("<li><a data-toggle='tab'  href='#home"+ i +"' onclick='DoctorProgressNote(this,"+JSON.stringify(tableData[i])+")'><span style='font-weight: bold; margin-left: 2px;'>"+ tableData[i].EnteredDate.substring(0,10).toUpperCase()+"</span> <span style='display: block; font-weight: bold; color: salmon;'>"+ tableData[i].DoctorName.toUpperCase()+"</span> </a></li>")
                               
                        }
                        debugger;
                        $("#content-slider").empty();

                        $('#content-slider').append(sb.toString());
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }
            // Old before chamge
            //$.ajax({
            //    url: url + "api/EMRAPI/GetSingleScreenPatientPastHistory",
            //    type: 'post',
            //    dataType: 'json',
            //    data: obj,
            //    success: function (response) {

            //        //alert(JSON.stringify(response));
            //        let tableData = response;
            //        let sb = new StringBuilder();
            //        for (var i = 0; i < tableData.length; i++) 
            //        {
            //            let day = tableData[i].EncDay.toString();
            //            let month = tableData[i].EncMonth;
                            
            //            if(i==0)
            //            {
            //                sb.append('<li class="active"><a data-toggle="tab" href="#home'+ i +'">'+ tableData[i].EncDay+'<span style="display: block;font-weight: bold;">'+ tableData[i].EncMonth+'</span>  </a></li>')
            //                localStorage.setItem("vDay", tableData[i].EncDay.toString());
            //                localStorage.setItem("vMonth", tableData[i].EncMonth);
            //            }
            //            else
            //            {
                                
            //                if(day == localStorage.getItem("vDay") && month== localStorage.getItem("vMonth"))
            //                {
            //                    // sb.append('<li><a data-toggle="tab" href="#home'+ i +'"> '+ tableData[i].EncDay+'<span style="display: block;font-weight: bold;">'+ tableData[i].EncMonth+'</span> </a></li>')
            //                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
            //                    localStorage.setItem("vMonth", tableData[i].EncMonth);
            //                } 
            //                else
            //                {
            //                    sb.append('<li><a data-toggle="tab" href="#home'+ i +'"> '+ tableData[i].EncDay+'<span style="display: block;font-weight: bold;">'+ tableData[i].EncMonth+'</span> </a></li>')
            //                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
            //                    localStorage.setItem("vMonth", tableData[i].EncMonth);
            //                }
                               
                                
            //            }
                       
            //        }
            //        $("#content-slider").empty();

            //        $('#content-slider').append(sb.toString());

            //        let sb1 = new StringBuilder();
            //        var OPIP;

            //        for (var i = 0; i < tableData.length; i++) {
                            
            //            let day = tableData[i].EncDay.toString();
            //            let month = tableData[i].EncMonth;

            //            if(i==0)
            //            {
            //                if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}

            //                sb1.append('<div id="home'+ i +'"  class="tab-pane fade in active">')
            //                sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
            //                sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
            //                sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
            //                sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
            //                sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i]) +")'><i class='fa fa-file-alt'></i></a></li>");
            //                sb1.append('</ul>');
            //                localStorage.setItem("vDay", tableData[i].EncDay.toString());
            //                localStorage.setItem("vMonth", tableData[i].EncMonth);
            //                localStorage.setItem ("Index",i);

            //            }
            //            else
            //            {
            //                if(day == localStorage.getItem("vDay") && month== localStorage.getItem("vMonth"))
            //                {
                                    
            //                    if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}

            //                    // sb1.append('<div id="home'+ localStorage.getItem ("Index") +'"  class="tab-pane fade in active">')
            //                    sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
            //                    sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
            //                    sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
            //                    sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
            //                    sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i]) +")'><i class='fa fa-file-alt'></i></a></li>");
            //                    sb1.append('</ul>');
            //                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
            //                    localStorage.setItem("vMonth", tableData[i].EncMonth);
            //                    localStorage.setItem ("Index",localStorage.getItem ("Index",i));
            //                }
            //                else
            //                {
            //                    sb1.append('</div>');
                                    
            //                    if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}                   

            //                    sb1.append('<div id="home'+ i +'"  class="tab-pane fade">')
            //                    sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
            //                    sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
            //                    sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
            //                    sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
            //                    sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
            //                    sb1.append('</ul>');
            //                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
            //                    localStorage.setItem("vMonth", tableData[i].EncMonth);
            //                }
            //            }
            //        }
            //        $("#tabPasthistory").empty();
            //        $('#tabPasthistory').append(sb1.toString());                  
                    
            //    },
            //    error: function (response) {
            //        console.log(response);
            //    }

            //});


        }

        function PatientChatdetail() { 
                
            let obj1 = new Object(); 
            obj1.DoctorId = localStorage.getItem("DoctorId");
            obj1.RegistrationNo  =  localStorage.getItem("RegistrationNo");
            obj1.EncounterId = localStorage.getItem("EncounterId");
            obj1.mchar = 'C';
         
            $.ajax({
                url: url + "api/Chat/GetNotification",
                type: 'post',
                dataType: 'json',
                data: obj1,
                success: function (res1) {
                    //  alert(JSON.stringify(res1));
                    let tableData = res1;
                    let sb = new StringBuilder();
                    for (var i = 0; i < tableData.length; i++) {

                        if (tableData[i].refrenceid == '')
                        {
                            sb.append("<div class='well well-sm'>" + tableData[i].MessageText + ' '+ "<i class='fa fa-user'></i> <i class='chatDateTimeUser'> " + tableData[i].DateTime + " </i> </div>")
                            // sb.append("</br>");
                        }
                        else
                        {
                            sb.append("<div class='well well-sm' style='background: aquamarine; text-align: right;'>" + tableData[i].MessageText +'  '+ "<i class='fa fa-stethoscope'></i> <i class='chatDateTimeDoctor'> " + tableData[i].DateTime + " </i></div>")
                            // sb.append("</br>");
                        }
                        if(tableData[i].RegistrationId > 0)
                        {
                            // alert(tableData[i].EncounterDate);
                            localStorage.setItem("lastchatid",tableData[i].Id);                                
                            localStorage.setItem("EncounterDate",tableData[i].EncounterDate);
                            localStorage.setItem("EncId",tableData[i].EncounterId);
                            localStorage.setItem("RegId",tableData[i].RegistrationId);
                        }


                    }
                    $("#chatbox").empty();

                    $('#chatbox').append(sb.toString());
                },
                error: function (res1) {
                    console.log(res1);
                }

            });
                     
           
        }
        window.setInterval(function () {
            //
            //  MessageNotification();
            // PatientChatdetail();
        }, 2000);  
        function MessageNotification() {
                
            let obj = new Object();
            obj.DoctorId = '<%=Session["EmployeeId"]%>';
            obj.RegistrationNo  =0
            obj.EncounterId = 0
            obj.mchar = 'N'
            $.ajax({
                url: url + "api/Chat/GetNotification",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {

                        
                    let tableData = res;
                    let sb = new StringBuilder(); let sbn = new StringBuilder();

                    $("#tblMessage").find("tr:gt(0)").remove();
                    $("#tblNotificationMessage").find("tr").remove();
                        
                    if (tableData.length) 
                    {

                        $('#rnf').css('display','none');
                        $('#ldrMsg').css('display','none');

                        for (var i = 0; i < tableData.length; i++) {
                            if(tableData[i].unread == 0)
                            {
                                sb.append('<tr style="font-weight: bold"><td>'+ tableData[i].RegistrationNo +'</td><td>'+ tableData[i].EncounterId +'</td><td>'+ tableData[i].PatientName +'</td><td>'+ tableData[i].VisitType +'</td><td>'+ tableData[i].MessageText +'</td>')
                                sb.append("<td><a data-toggle='pill' href='#chat' onclick='clickMe(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-reply'></i> </a></td></tr>")
                                sbn.append('<tr style="font-weight: bold"><td class="td-noti-style">'+ tableData[i].RegistrationNo +' <span class="td-noti-style">'+ tableData[i].PatientName +'</span></td><td>'+ tableData[i].MessageText +'</td>')
                                sbn.append("<td><a data-toggle='pill' href='#chat' onclick='clickMe(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-reply'></i> </a></td></tr>")
                      
                    
                            }
                            else
                            {
                                sb.append('<tr class="read"><td>'+ tableData[i].RegistrationNo +'</td><td>'+ tableData[i].EncounterId +'</td><td>'+ tableData[i].PatientName +'</td><td>'+ tableData[i].VisitType +'</td><td>'+ tableData[i].MessageText +'</td>')
                                sb.append("<td><a data-toggle='pill' href='#chat' onclick='clickMe(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-reply'></i> </a></td></tr>")
                                sbn.append('<tr class="read"><td class="td-noti-style">'+ tableData[i].RegistrationNo +' <span class="td-noti-style span">'+ tableData[i].PatientName +'</span></td><td>'+ tableData[i].MessageText +'</td>')
                                sbn.append("<td><a data-toggle='pill' href='#chat' onclick='clickMe(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-reply'></i> </a></td></tr>")
                        
                       
                            }
                        }
                        if (parseInt(res[0]["total"]) > 0) {

                            // alert ('Grater then 0');

                            $('#MesageNotificationId').html(res[0]["total"]);
                        }
                        else{
                            $('#MesageNotificationId').html(0);
                        }
                        //sb.append("<li class='arrow-up'></li>");


                    }
                    else
                    {                       
                        //sb.append("<tr><td'> No Record Found</td></tr>");
                        $('#ldrMsg').css('display','none');
                        $('#rnf').css('display','block');                           
                            
                    }

                    $('#tblMessage').append(sb.toString());
                    $('#tblNotificationMessage').append(sbn.toString());
                },
                error: function (res) {
                    console.log(res);
                }

            });
        }


        function  DoctorPanleSeting() {
                
            let obj = new Object();
            obj.DoctorId = '<%=Session["EmployeeId"]%>';
            $.ajax({
                url: url + "api/EMRAPI/GetDoctorPanelSetting",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    let tableData = res;  
                    if (tableData.length) 
                    {
                        for (var i = 0; i < tableData.length; i++) {
                            if(tableData[i].DoctorPanelSetting == 'R')
                            {
                                $('.mid-box').toggleClass('collapsed1');
                                $('.pt-dtls-block').toggleClass('collapsed1');
                            }
                            if(tableData[i].DoctorPanelSetting == 'L')
                            {
                                expand();
                            }

                        }


                    }
                  
                    
                },
                error: function (res) {
                    console.log(res);
                }

            });
        }
    </script>
    <script>
        let obj = new Object();

        function clickMe(control,data)
        {           
            // alert(JSON.stringify(data));
            $('#MessageModel').show();
            $('#liMessage').removeClass('active');
            $('#liChat').css('display','block');
            $('#liChat').addClass('active');
            $('#txtpatient').val(data.PatientName);
            $('#txtvisit').val(data.EncounterId);            
            obj.RegistrationNo = data.RegistrationNo;
            obj.EncounterId = data.EncounterId;
            obj.DoctorId = data.DoctorId;
            obj.PatientName = data.PatientName;
            obj.GenderAge = data.GenderAge;
            obj.VisitType = data.VisitType;
            localStorage.setItem("RegistrationNo",data.RegistrationNo);
            localStorage.setItem("EncounterId",data.EncounterId);
            localStorage.setItem("DoctorId",data.DoctorId);
            PatientChatdetail();
            //$('#idchat').trigger('click',data);
    
        }

        function sendMessage() {
           
                
            let message = $('#Message').val();
            let len = message.length;
            if (len < 2) {
                $('#alertid').css('display', 'block');
                alert('Please Enter at Least 2 Character');
                $('#Message').focus();
                return false;
            }
            else {
                $('#alertid').css('display', 'none');
                let text = $('#Message').val();
                obj.MessageText = text;
                obj.Id = localStorage.getItem("lastchatid");
                $.ajax({
                    url: url + "api/Chat/SaveMessageChat",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (res) {
                        let result = res;
                        if (result.toLowerCase().includes("already"))
                            toastr.warning(res, "Warning");
                        else

                            $('#successid').css('display', 'block');
                        $('#sucessMessage').text(res);
                        console.log(JSON.stringify(obj));
                        $('#Message').val('');
                        PatientChatdetail();
                    },
                    error: function (res) {
                        toastr.error(res, "Error");
                    }

                });

            }
        }


        function EmrTemplateIcone()
        {
            obj.SpecialisationId = 0;
            obj.FacilityId = '<%=Session["FacilityId"]%>';
            obj.DoctorId = '<%=Session["EmployeeId"]%>';
            ;
            $.ajax({
                url: url + "api/EMRAPI/getSingleScreenUserTemplatesIcon",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res,xhr) {
                    //alert(JSON.stringify(res));
                    if(xhr.status=200)
                    {
                        if (IconVisible(res,'OVT')) // 0
                        {
                            $('#IcOPVitals').parent().css('display','inline');
                        }

                        if (IconVisible(res,'VTL')) // 1
                        {
                            $('#IcVitals').parent().css('display','inline');
                        }
                        if (IconVisible(res,'COM')) // 2
                        {
                            $('#IclChiefComplaints').parent().css('display','inline'); 
                        }
                        if (IconVisible(res,'HIS')) // 3
                        {
                            $('#IcHisPIllness').parent().css('display','inline');

                        }
                        if (IconVisible(res,'PH')) // 4
                        {
                            $('#IcPastHistory').parent().css('display','inline');
                        }
                        if (IconVisible(res,'EXM')) // 5
                        {
                            $('#IcExamination').parent().css('display','inline');
                        }
                        if (IconVisible(res,'PDG'))
                        {
                            $('#IcProvisionalDiagnosis').parent().css('display','inline');
                        }
                        if (IconVisible(res,'DGN')) // 6
                        {
                            $('#IcDiagnosis').parent().css('display','inline');
                        }
                        if (IconVisible(res,'ORD')) // 7
                        {
                            $('#IcOrdersandProcedures').parent().css('display','inline');
                
                        }
                        if (IconVisible(res,'PRS')) // 8
                        {
                            $('#IcPrescriptions').parent().css('display','inline');
                        }
                    
                        if (IconVisible(res,'NDO')) // 9
                        {
                            $('#IcOtherOrder').parent().css('display','inline');
                        }
                
                        if (IconVisible(res,'ATM')) // 10
                        {
                            $('#IcVitals').parent().css('display','inline');
                        }
                        if (IconVisible(res,'PFE')) // 11
                        {
                            $('#IcPatientandfamilyeducationandcounselling').parent().css('display','inline');
                        }
                        if (IconVisible(res,'RRR')) // 12
                        {
                            $('#IcReferralsReplytoreferrals').parent().css('display','inline');
                        }
                        if (IconVisible(res,'MEP')) // 13
                        {
                            $('#IcMultidisciplinaryEvaluationAndPlanOfCare').parent().css('display','inline');
                        }  
                        if (IconVisible(res,'POC')) // 14
                        {
                            $('#IcPlanOfCare').parent().css('display','inline');
                        }  
                        if (IconVisible(res,'OTN')) // 15
                        {
                            $('#IcCareTemplate').parent().css('display','inline');
                        }
                        if(IconVisible(res,'NDO'))
                        {
                            $('#IcOtherOrder').parent().css('display','inline');
                        }
                        if (IconVisible(res,'PCN')) // 16
                        {
                            $('#IcPacNotes').parent().css('display','inline');
                        }

                        if (IconVisible(res,'OTR')) // 17
                        {
                            $('#IcORequest').parent().css('display','inline');
                        }
                        if (IconVisible(res,'UP')) // 17
                        {
                            $('#IcFollowup').parent().css('display','inline');
                        }
                        if (IconVisible(res,'IN')) // 17
                        {
                            $('#IcRemarks').parent().css('display','inline');
                        }
                        if (IconVisible(res,'FT')) // 17
                        {
                            $('#IcFreeText').parent().css('display','inline');
                        }
                        
                    }
          
                },
                error: function (res) {
                    toastr.error(res, "Error");
                }

            });
        }
     
        function IconVisible(res,val)
        {
                
            for (var index = 0; index < res.length; ++index) {

                var prop = res[index];

                if(prop.TemplateCode == val){
                    hasMatch = true;
                    return true;
                    break;
                }
            }
        }

        function Vitals()
        {
            $('#<%=imgBtnAddVitals.ClientID%>').trigger('click');  
        }

<%--        function GrowthChart()
        {
           // $('#<%=imgBtnAddGrowthChart.ClientID%>').trigger('click');  
        }--%>


        function VitalHistory()
        {

            var url ='/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate';
            OpenPopup(url,"Vitals History"); 
           
        }
        function PastClinicalNotes()
        {
            $get('<%=lbtnPastClinicalNotes.ClientID%>').click();

           <%-- var url = '/WardManagement/VisitHistory.aspx?RNo=<%=Session["RegistrationNo"]%>&Regid=<%=Session["RegistrationId"]%>&RegNo=<%=Session["RegistrationNo"]%>&EncId=<%= Session["EncounterId"]%>&EncNo=<%= Session["EncounterNO"]%>&FromWard=Y&OP_IP=I&Category=PopUp&FromEMR=1';
            OpenPopup(url,"Past Clinical Notes");  --%>
        }            

        function caseSheet()
        {   
            let EncId = localStorage.getItem("EncId");
            var EncDate =  localStorage.getItem("EncDate");  
            $('#<%=imgBtnAddCaseSheet.ClientID%>').trigger('click');  

           <%-- //  var oWnd = $find("<%=RadWindowForNew.ClientID%>");  
            let EncId = localStorage.getItem("EncId");
            var EncDate =  localStorage.getItem("EncDate");            
            if(EncDate==null)
            {
                EncDate = '';
            }
            var url = '/Editor/WordProcessor.aspx?From=POPUP&DoctorId="<%=Session["DoctorID"]%>"&EncId='+<%=Session["EncounterId"]%>+'&OPIP="<%=Session["OPIP"]%>"&EncounterDate='+EncDate+'&EncounterType=O&IsEMRPopUp=1';
            
            OpenPopup(url,"Case Sheet"+" " + EncDate); --%>           
        }

        function CaseSheetAtChat()
        {         
            //  var oWnd = $find("<%=RadWindowForNew.ClientID%>");  
                
               
            let Date =  localStorage.getItem("EncounterDate"); 
            let RegId = localStorage.getItem("RegId");
            let EncId = localStorage.getItem("EncId");
            var url = '/Editor/WordProcessor.aspx?From=POPUP&EncId='+EncId+'&RegId='+RegId+'&DoctorId="<%=Session["DoctorID"]%>"&OPIP="<%=Session["OPIP"]%>"&EncounterDate='+Date+'&EncounterType=O';
            OpenPopup(url,"Case Sheet"+" " + Date );            
        }
       
        function LastOPDVisit(controler,Data)
        {       
           
            var url ='/EMR/Dashboard/PopUpPatientDashboardForDoctorNew.aspx?From=POPUP&CloseButtonShow=Yes&EncounterType='+Data.OPIP+'&EncounterNo='+Data.EncounterNo+'';
            localStorage.setItem("EncDate",Data.EncDate.replace(/ [0-9]*[0-9]:[0-9][0-9]A*P*M/,''));
            localStorage.setItem("EncId",Data.EncounterId);
                
            OpenPopup1(url,CopyLastPrescription);              
        }
        function LastDischargeSummary(controler,Data)
        { 
            
            var url ='/EMR/Dashboard/DashboardDischargeSummary.aspx?From=POPUP&CloseButtonShow=Yes&EncounterType='+Data.OPIP+'&EncounterId='+Data.EncounterId+'&RegistrationId='+Data.RegistrationId +'';
            //var url ='/EMR/Dashboard/DashboardDischargeSummary.aspx?From=POPUP&CloseButtonShow=Yes&EncounterType='+Data.OPIP+'&EncounterId='+Data.EncounterId+'&RegistrationId='+Data.RegistrationId+';

           
            OpenPopup(url,"Discharge Summary");              
        }

        function DoctorProgressNote(controler,Data)
        { 
            var url ='/EMR/Dashboard/ViewDoctorProgressNote.aspx?From=POPUP&CloseButtonShow=Yes&ProgressNoteId='+Data.ProgressNoteId+'';
            OpenPopup(url,"Doctor Note");              
        }
        function DischargeSammary(controler,Data)
        {
            url='/EMRReports/PrintPdf1.aspx?page=Ward&EncId=41877&RegId=18818&ReportId=&For=DISSUM&Finalize=True%20&%20ExportToWord=False'
        }

        function Diagnosis()
        { 
            $('#<%=imgBtnFinalDiagnosis.ClientID%>').trigger('click');
            //var url='/EMR/Assessment/DiagnosisHistory.aspx?From=POPUP';
            //OpenPopup(url,"Diagnosis History");
   
        }
        function OpenPopup(url,Title)
        {
            var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = url;              
            oWnd.setUrl(url);
            oWnd.setSize(650,500);
            oWnd.set_title(Title); 
            oWnd.show();
        }

        function OpenPopup1(url,fun) // RadWindow1
        { 
            var oWnd = $find("<%=RadWindow1.ClientID%>");
            var url = url;              
            oWnd.setUrl(url);
            oWnd.setSize(650,500);           
            oWnd.show();
            oWnd.add_close(fun);
            return false;                
        }
        function OpenPopup2(url,fun,title) // RadWindowForNew
        { 
                
            var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = url;              
            oWnd.setUrl(url);
            oWnd.setSize(1200,1200);
            oWnd.set_title(title);
            oWnd.show();
            oWnd.add_close(fun);
                
        }

        function OpenPopup3(url,fun) // RadWindowForNew
        {                
            var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = url;              
            oWnd.setUrl(url);
            oWnd.setSize(600,500);           
            oWnd.show();
            oWnd.add_close(fun);
                
        }           

        function ChiefComplaints()
        {  
            // $('#<%=imgBtnAddChiefComplaints.ClientID%>').trigger('click');    
            var url=' /EMR/Problems/ViewPastPatientProblems.aspx?MP=NO';
            OpenPopup(url,"Chief Complaint");
        }



        function QueryResponse(control,data)
        {    
            //var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = '/Approval/QueryResponse.aspx?Qid='+data.Id+'&UHID='+data.UHID+'';
            OpenPopup(url,"Insurance Query");  
        }

        function LabResults(control,data)
        {
            // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var date = data.OrderDate              
            var url = '/EMR/Dashboard/ProviderParts/LabResults.aspx?From=POPUP&orderDate='+date.replace(/ [0-9]*[0-9]:[0-9][0-9]A*P*M/,'')+'&mainRegNo='+data.RegistrationNo;
            OpenPopup(url,"Lab Alerts");
        }
        //function GrowthChart()
        //{  
        //    var url = '/EMR/Vitals/GrowthChart.aspx?MP=NO';
        //    OpenPopup(url,"Growth Chart");  
           
        //}

        function Immunization()
        {
            // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = '/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP';
            OpenPopup(url,"Immunization Due Date");             
        }       
     
        function Allergies()
        {
               
            $('#<%=imgBtnAddAllergies.ClientID%>').trigger('click');              
                
           <%-- var url = '/EMR/Allergy/Allergy.aspx?Regno=<%=Session["RegistrationNo"]%>&Encno=<%= Session["EncounterNO"]%>&IsEMRPopUp=1&Source=IPD';
            OpenPopup2(url,addAllergiesOnClientClose)--%>
        }

        function refreshTemplatesetting()
        {
            EmrTemplateIcone();
            $get('<%=btnRefreshTemplate.ClientID%>').click();
            
        }
        function TemplateSetting()
        {
                
            // var url = '/EMR/masters/EMRTemplatesSetup.aspx?IsEMRPopUp=1';
            var url = '/EMR/masters/EMRTemplatesSetup.aspx?IsEMRPopUp=1&SpecialisationId=<%=Session["UserSpecialisationId"]%>&EmployeeId=<%=Session["EmployeeId"]%>';
            OpenPopup2(url,refreshTemplatesetting,"EMR Template Setup");  
        }
        function Prescriptions()
        {
            var url = '/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=False&EMRSingleScreen=True';
            OpenPopup(url,"Prescription History");
                
            // $('#<%=imgBtnAddPrescriptions.ClientID%>').trigger('click'); 
        }

        function RIS()
        {
            // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = '/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=<%= Session["EncounterId"]%>&RegNo=<%=Session["RegistrationNo"]%>&Source=O&Flag=RIS&Station=All';
            OpenPopup(url,"Radiology Result ");  
        }
        function LIS()
        {
            // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
            var url = '/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=<%= Session["EncounterId"]%>&RegNo=<%=Session["RegistrationNo"]%>&Source=O&Flag=LIS&Station=All';        
            OpenPopup(url,"Lab Results");
        }
        function ProviderDashBord()
        {
            var url = '/emr/Dashboard/ProviderDashboard.aspx?MPG=P139&IsEMRPopUp=1';       
            OpenPopup(url); 
        }
        function OTRequest()
        {
            var url = '/OTScheduler/OTRequest.aspx?POPUP=POPUP&IsEMRPopUp=1';        
            OpenPopup(url,"OT Request");
        }
        function ReferralRequest()
        {
            // $('#<%=imgBtnAddReferralRequest.ClientID%>').trigger('click');
            //var url = '/EMR/ReferralSlip.aspx?MASTER=NO&IsEMRPopUp=1';        
            //OpenPopup(url,"Referral Request");
        }

        function AdmissionAdvice()
        {   
           <%-- $('#<%=imgBtnAddAdmissionAdvice.ClientID%>').trigger('click');--%>

            var url = '/ATD/Booking.aspx?From=POPUP&IsEMRPopUp=1';        
            OpenPopup(url,"Admission Advice");
        }

        function ProgressNote()
        {
            
            var url = '/EMR/Templates/ProgressNoteDateWise.aspx?Category=PopUp&IsEMRPopUp=1';        
            OpenPopup(url,"Progress Note");
        }

        function DischargeSummary()
        {
            var url = '/ICM/DischargeSummary.aspx?MASTER=NO&IsEMRPopUp=1';        
            OpenPopup(url,"Discharge Summary");
        }       

        function Attachment()
        {   
            var url = '/EMR/AttachDocumentFTP.aspx?MASTER=No&IsEMRPopUp=1&RNo=' + '<%=Session["RegistrationNo"]%>';
            OpenPopup(url,"Patient Document");
        }

        function LabOrders(Department)
        {
           
            var url = '/EMRBilling/Popup/Servicedetails.aspx?Deptid=0&EncId=<%= Session["EncounterId"]%>&RegNo=<%=Session["RegistrationNo"]%>&sBillId=0&PType=WD&rwndrnd=0.5007572308194614&DeptType='+Department+''
            OpenPopup2(url,addOrdersAndProceduresOnClientClose);
            //$('#<%=imgbtnAddOrdersAndProcedures.ClientID%>').trigger('click');
        }

        function EyeVital()
        { 
            var url = '/EMR/Ophthalmologist/ARPlusGlassDetails.aspx';
            OpenPopup3(url,addEyesVitalOnClientClose);

        }


        function PrintLabReport(controler,Data)
        {

            if(Data.OutsourceInvestigation === true)
            {
                alert('Selected Report is OutSource Report Can not be print');
                return;   
            }
            else
            {     

                let url = '/Include/PdfReport.aspx?&ReportName=OPLabReport&SOURCE='+Data.SOURCE+'&LABNO='+Data.LABNO+'&StationId='+Data.StationId+'&ServiceId='+Data.ServiceIds+'';
                OpenPopup(url,"Investigation Report");
            }
            //popUpObj = window.open(url,
            //"ModalPopUp",

            //"toolbar=no," +

            //"scrollbars=no," +

            //"location=no," +

            //"statusbar=no," +

            //"menubar=no," +

            //"resizable=0," +

            //"width=900," +

            //"height=600," +

            //"left = 50," +

            //"top=50"

            //);
            //    $.ajax({
            //    type: "POST",
            //    url: '/Shared/Services/PrescriptionService.asmx/printLabReport',                   
            //    data: "{'SOURCE': '" + Data.SOURCE + "','LABNO':'" + Data.LABNO + "','StationId':'" + Data.StationId + "','ServiceId':'" + Data.ServiceIds + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (res) {
            //        showModalPopUp();
            //    },
            //    error: function (res) {
            //        console.log(res);
            //    }
            //});
        }


        function showModalPopUp(Data) {
                
            // let reportUrl = "@Util.GetReportPageUrl()";
            // alert(JSON.stringify(Data));
            let url = "/Include/PdfReport.aspx?SOURCE='"+Data.SOURCE+"'&LABNO='"+Data.LABNO+"'&StationId='"+Data.StationId+"'&ServiceId='"+Data.ServiceIds+"' ";
            popUpObj = window.open(url,
            "ModalPopUp",

            "toolbar=no," +

            "scrollbars=no," +

            "location=no," +

            "statusbar=no," +

            "menubar=no," +

            "resizable=0," +

            "width=900," +

            "height=600," +

            "left = 50," +

            "top=50"

            );


        }

        function PastHistory()
        {
            <%--$('#<%=lnkPastHistory.ClientID%>').trigger('click');--%>
            $('#<%=imgPopupHIS.ClientID%>').trigger('click');
        }



        $('#btn-expand-collapse').click(function(e) {
               
            //$('.emr-main-container').toggleClass('collapsed');
            //$('.RadWindow_Metro').toggleClass('collapsed');
            expand();
        });

        function expand()
        {
            //alert('kuldeep');

            $('.emr-main-container').toggleClass('collapsed');

                <%--if( $('#<%=btnOnScreen.ClientID%>').css('display')!='none')
                {
                    $('#<%=btnOnScreen.ClientID%>').css('display','none');
                }
                else
                {
                    
                    $('#<%=btnOnScreen.ClientID%>').css('display','inline-block');
                }--%>

               
            //$('.RadWindow_Metro').toggleClass('collapsed');
        }

        function Message(msg)
        {
                
            if(msg !='')
            {
                $('#msgModal').show();
                $('#pMsg').html(msg);
                window.setInterval(function () {
                    // PatientChatdetail();
                    messageclose();
                }, 10000);  
            }
        
        }
        function messageclose()
        {
            $('#msgModal').hide();
            $('#pMsg').html('');
        }
        // Tamplate Setting
        $("#checkAll").click(function(){

            $(".accord-inner").toggleClass("main");
            $(".free-text").toggleClass("main");

        });
        $("#checkCustomize").click(function(){

            $(".accord-inner").toggleClass("main");
            $(".free-text").toggleClass("main");

        });

        $('#btn-nav-previous').click(function(){
            $(".menu-inner-box").animate({scrollLeft: "-=100px"});
        });
        
        $('#btn-nav-next').click(function(){
            $(".menu-inner-box").animate({scrollLeft: "+=100px"});
        });

        function AddChiefComplaints()
        {
            var url ='/EMR/Problems/ChifComplaint.aspx?IsEMRPopUp=1';
            OpenPopup(url);
        }
        function PreAuth()
        {
            var url ='/EMR/Newpreauth.aspx?EncId=<%= Session["EncounterId"]%>';
            OpenPopup(url,"Pre Auth");
        }


        function TreatmentPlan()
        {
                
            var url ='EMR/ClinicalPathway/OPPatientTreatmentPlan.aspx?POPUP=POPUP&CloseButtonShow=Yes';
            OpenPopup(url,"Care Plan");
            
        }
           

        function btnTreatmentPlan() {              
               
              
            document.getElementById('<%=hdnButtonId.ClientID %>').value = "btnBindOrderPriscriptionPlaneOfCare";
            $get('<%=btnEnableControl.ClientID%>').click();  
            return false

            function auto_grow(element) {
                element.style.height = "5px";
                element.style.height = (element.scrollHeight)+"px";
            }
            
            //document.getElementById("mainDIV").style.opacity = "";
                <%--var x = screen.width / 2 - 1300 / 2;
            var y = screen.height / 2 - 550 / 2;
            var popup;
            popup = window.open("/EMR/Dashboard/TreatmentPlan.aspx?CloseButtonShow=Yes", "Popup", "height=550,width=1300,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
            popup.focus();
            document.getElementById("mainDIV").style.opacity = "0.5";
            document.getElementById('<%=btnBindOrderPriscriptionPlaneOfCare.ClientID %>').disabled = false;
            popup.onunload = function () {
                document.getElementById('<%=hdnButtonId.ClientID %>').value = "btnBindOrderPriscriptionPlaneOfCare";
                $get('<%=btnEnableControl.ClientID%>').click();                
                document.getElementById("mainDIV").style.opacity = "";
            }--%>

           
        }

        function ddlDiagnosiss_OnClientSelectedIndexChanged(sender, args) {
            debugger
            var item = args.get_item();

            var DiagICDID = item.get_attributes().getAttribute("ICDID");
            var DiagICDCode = item.get_attributes().getAttribute("ICDCode");

            var DiagICDDIscription = item.get_attributes().getAttribute("ICDDescription");

           

            $get('<%=hdnDiagnosisId.ClientID%>').value = DiagICDID;
            $get('<%=txtIcdCodes.ClientID%>').value = DiagICDCode;

            $get('<%=hdnDiagnosisId.ClientID%>').value = item != null ? item.get_value() : sender.value();

           <%-- $get('<%=btnCommonOrder.ClientID%>').click();--%>
            //alert('test');
        } 

        (function() {
            jQuery.each(jQuery('textarea[data-autoresize]'), function() {
                var offset = this.offsetHeight - this.clientHeight;
 
                var resizeTextarea = function(el) {
                    jQuery(el).css('height', 'auto').css('height', el.scrollHeight + offset);
                };
                jQuery(this).on('keyup input', function() { resizeTextarea(this); }).removeAttr('data-autoresize');
            });
        });

        function GetSelectedItem(sender, args) {
            var item = args.get_item();

            var IsToothNoMandatory = item.get_attributes().getAttribute("IsToothNoMandatory");

            $get('<%=hdnIsToothNoMandatory.ClientID%>').value = IsToothNoMandatory;
        }

        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.value = "Save (F3)";

                //display message
                //document.getElementById("message-div").style.display = "block";
                //document.getElementById("divbtnSaveConfirm").style.display = "none";

            }
            return true;
        }

            
    </script>
    <script>
          

        var autoExpand = function (field) {
            //alert();
            // Reset field height
            field.style.height = '0';
            // Get the computed styles for the element
            var computed = window.getComputedStyle(field);

            // Calculate the height
            var height = parseInt(computed.getPropertyValue('border-top-width'), 0)
                         + parseInt(computed.getPropertyValue('padding-top'), 0)
                         + field.scrollHeight
                         + parseInt(computed.getPropertyValue('padding-bottom'), 0)
                         + parseInt(computed.getPropertyValue('border-bottom-width'), 0);

            field.style.height = height + 'px';

        };

        document.addEventListener('input', function (event) {
            if (event.target.tagName.toLowerCase() !== 'textarea') return;
            autoExpand(event.target);
        }, false);


        //function OpenCloseScreen()
        //{               
        //    if($('#onscreen').css('display') == 'block')
        //    {
        //        $('#onscreen').css('display','none');
        //        $('#viewscreen').css('display','block');
        //        localStorage.setItem('display','none')
        //        $("#btnOnScreen").prop('value', 'Off');

        //    }
        //    else
        //    {
        //        $('#onscreen').css('display','block');
        //        $('#viewscreen').css('display','none');
        //        localStorage.setItem('display','block')
        //        $("#btnOnScreen").prop('value', 'On');
        //    }

        //    //alert("Kuldeep");
        //}

        function ExpandList(control)
        {
                
            if (control.id == "IcDiagnosis")
            {
                $('#<%=ImageButton13.ClientID%>').click();
            }
            else if(control.id =="IcOPVitals")
            {
                $('#<%=ImageOPt1.ClientID%>').click();
            }

            else if(control.id =="IcVitals")
            {
                $('#<%=imgVbtnVital.ClientID%>').click();
            }
            else if(control.id =="IclChiefComplaints")
            {                    
                $('#<%=imgbtnChiefComplaints.ClientID%>').click();
            }
            else if( control.id=="IcHisPIllness")
            {
                $('#<%=imbtnHistory.ClientID%>').click();
            }
            else if( control.id=="IcPastHistory")
            {
                $('#<%=ImageButton14.ClientID%>').click();
            }
            else if( control.id=="IcExamination")
            {
                $('#<%=imgbtnTemplate.ClientID%>').click();
            }
                
                
            else if( control.id=="IcCareTemplate")
            {
                $('#<%=imgbtntherNotes.ClientID%>').click();
            }
            else if( control.id=="IcProvisionalDiagnosis")
            {
                $('#<%=imgbtnProvisionalDiagnosies.ClientID%>').click();
            }
            else if( control.id=="IcPlanOfCare")
            {
                $('#<%=ImageButton4.ClientID%>').click();
            }
            else if( control.id=="IcOrdersandProcedures")
            {
                $('#<%=imgbtnOrdersAndProcedures.ClientID%>').click();
            }
                
            else if( control.id=="IcPrescriptions")
            {
                $('#<%=imgbtnPrescription.ClientID%>').click();
            }
            else if( control.id=="IcORequest")
            {
                $('#<%=imgExpndOTRequest.ClientID%>').click();
            }
            else if( control.id=="IcFollowup")
            {
                $('#<%=ImageButtonFollowup.ClientID%>').click();
            }
            else if( control.id=="IcRemarks")
            {
                $('#<%=ImageButtonRemarks.ClientID%>').click();
            }
            
            else if( control.id=="IcPacNotes")
            {
                $('#<%=imgpnlPACTemplates.ClientID%>').click();
            }

            else if( control.id=="IcPatientandfamilyeducationandcounselling")
            {
                $('#<%=ImageButton9.ClientID%>').click();
            }
            else if( control.id=="IcReferralsReplytoreferrals")
            {
                $('#<%=ImageButton10.ClientID%>').click();
            }
                
            else if(control.id =="IcMultidisciplinaryEvaluationAndPlanOfCare")
            {                    
                $('#<%=ImageButton12.ClientID%>').click();
            }
            else if( control.id=="IcFreeText")
            {
                $('#<%=ImageButtonFreeText.ClientID%>').click();
             }
                
}


    </script>
    <script type="text/javascript">
        function validate(control) {
            <%--var id = control.id ;
                <%var userName = id;%> 
               '<%Session["UserName"] =  userName ; %>';
               alert('<%=Session["UserName"] %>');--%>
        }
    </script>

    <script>
        $('#btn-expand-collapse1').click(function(){
            $('.mid-box').toggleClass('collapsed1');
            $('.pt-dtls-block').toggleClass('collapsed1');
        });

        // Mouse Handler
        function mouseHandler(e){
            // Add active Class
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
            } else {
                $(".active").removeClass('active');
                $(this).addClass('active');
            } 
        }

        //added by bhakti
        function CalculatTemprature(ctrlFindText, minvalue, maxvalue, strVitalId) {

            var txtunit = document.getElementById(ctrlFindText);
            var mnvalue = document.getElementById(minvalue);
            var mxvalue = document.getElementById(maxvalue);
            var DisplayName = document.getElementById(strVitalId);
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;

            if (DisplayName.value == "T") {
                if (txtunit.value != "") {
                    if ((mnvalue.value) > (txtunit.value) || (mxvalue.value) < (txtunit.value)) {

                        txtunit.style.color = "Red";
                    }
                    else {
                        txtunit.style.color = "Black";

                    }
                }
            }
        }

        function start(){
            // Bind all li
            $('.fixed-left-icons li').bind('click', mouseHandler);
        }

        $(document).ready(start);
      
       
    </script>
    <style>
        textarea {
            max-height: 250px;
            width: 100%;
        }

        .fixed-left-icons li.active {
            background-image: linear-gradient(to top, #687cbe, #1976d2);
        }

            .fixed-left-icons li.active a {
                color: #fff;
            }

        .check-top-margin label {
            margin-top: 3px;
        }
    </style>

    <%--<script type="text/javascript">

        function openMenu() {
            var popup = document.getElementById("menuOptions");
            popup.classList.toggle("show");
        }

        function openMacro() {
            var userTag = '';
            if (typeof URLSearchParams != "undefined") {
                var urlParams = new URLSearchParams(window.location.search);
                if (urlParams.has('UserTag')) {
                    userTag = urlParams.get('UserTag');
                }
            }

            if (userTag == '') {
                $("#UserTagDialog").dialog("open");
                return;
            }
            var macroWindows = window.open("ManageMacro.html?UserTag=" + userTag, "", "scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no,width = 867, height = 500");
        }

        function openPairMic(){
            $("#QRCodeDialog").dialog("open");
        }

        function openCommandHelp() {
            $("#commandHelp").dialog("open");
        }

        $(document).ready(function () {
            $("#mnhtmlform").addClass("active");
            $("#MessageDialog").dialog({
                autoOpen: false,
                modal: true
            });

            $("#UserTagDialog").dialog({
                autoOpen: false,
                modal: true,
            });

            $("#QRCodeDialog").dialog({
                autoOpen: false,
                modal: true,
                width: "307px"
            });

            $("#commandHelp").dialog({
                autoOpen: false,
                modal: true,
                width: "600px"
            });

            $("#MicOffDialog").dialog({
                autoOpen: false,
                modal: true,
                buttons: {
                    "Keep Mic On": function () {
                        clearInterval(stopInterval);
                        MicCloseInSec = 10;
                        $(this).dialog("close");
                    }
                }
            });
            $("#augnitoMicBar").draggable();
        });

        var MicCloseInSec = 10;
        var stopInterval;
        function HandleMicOff() {
            $("#MicOffDialog").dialog("open");
            $("#lblTimer").html(MicCloseInSec);
            stopInterval = setInterval(() => {
                if (MicCloseInSec > 0) {
                    MicCloseInSec--;
                    $("#lblTimer").html(MicCloseInSec);
                } else {
                    clearInterval(stopInterval);
                    MicCloseInSec = 10;
                    $("#btnAugnitoMic").click();
                    $("#MicOffDialog").dialog("close");
                }
            }, 1000)
        }
    </script>--%>
</asp:Content>
