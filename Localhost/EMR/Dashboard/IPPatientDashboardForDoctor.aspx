<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMasterWithTopDetails.master" AutoEventWireup="true" CodeFile="IPPatientDashboardForDoctor.aspx.cs" Inherits="EMR_Dashboard_IPPatientDashboardForDoctor" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />

    <%--<link href='https://fonts.googleapis.com/css?family=Roboto' rel='stylesheet'>--%>
    <link href="/Include/css/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="/Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />--%>
    <link href="/Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <%--<script src="../../Include/JS/bootstrap.min.js"></script>--%>

    <%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>--%>
    <link href="/Include/css/all.min.css" rel="Stylesheet" type="text/css" />
    <script src="/Include/JS/all.min.js"></script>
    <style type="text/css">
        .fst-box {
            width: 100%;
            padding: 10px 0px 10px 10px;
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
                margin: 0 0 0 8px !important;
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

        /*.RadWindow_Metro .rwCorner .rwTopLeft,  
.RadWindow_Metro .rwTitlebar,  
.RadWindow_Metro .rwCorner .rwTopRight,  
.RadWindow_Metro .rwIcon,
.RadWindow_Metro table .rwTopLeft,  
.RadWindow_Metro table .rwTopRight,  
.RadWindow_Metro table .rwFooterLeft,  
.RadWindow_Metro table .rwFooterRight,  
.RadWindow_Metro table .rwFooterCenter,  
.RadWindow_Metro table .rwBodyLeft,  
.RadWindow_Metro table .rwBodyRight,  
.RadWindow_Metro table .rwTitlebar,  
.RadWindow_Metro table .rwTopResize, 
.RadWindow_Metro table .rwStatusbar,  
.RadWindow_Metro table .rwStatusbar .rwLoading
{    
    display: none !important;   
}*/

        /*.RadWindow_Metro { background: #fff;
    border-radius: 10px;
    padding: 10px;
    height: 88vh !important;
    bottom: 0;
    margin: auto;
    display: table;
}*/

        iframe[name="RadWindowForNew"] {
            overflow-y: hidden;
            height: 70vh !important;
        }

        /*div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew { bottom:0; right: 0; left: 0 !important; top: 0 !important; margin: auto; width: 90% !important;}*/
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
            float: left;
        }

        /*div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew {
            margin: 0 !important;
            position: absolute !important;
            top: 50% !important;
            left: 50% !important;
            -ms-transform: translate(-50%, -50%) !important;
            transform: translate(-50%, -50%) !important;
            height: auto !important;
        }


        .RadWindow iframe[name="RadWindowForNew"] {
            height: 450px !important;
        }*/
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

        .RadWindow_Metro {
            top: 0 !important;
            bottom: 0;
            margin: auto;
            width: 96vw !important;
            height: 96vh !important;
            border: 0;
        }

        /* Ends Data Saved Popup */
    </style>
    <script type="text/javascript">
        window.onbeforeunload = function (evt) {
            var IsUnsave = $get('<%=hdnIsTransitDataEntered.ClientID%>').value;
            //if (IsUnsave != 0) {
            //    return false;
            //}
        }

        function SetIsTransitDataEntered(CTRL) {

            $get('<%=hdnIsTransitDataEntered.ClientID%>').value = '1';

            var timer = $find('<%=TimerAutoSaveDataInTransit.ClientID%>');

            timer.set_interval(600000);

            var isTimerEnabled = timer.get_enabled();
            if (!isTimerEnabled) {
                timer.set_enabled(true);
                timer._startTimer();
            }

            $get('<%=hdnCurrentControlFocused.ClientID%>').value = CTRL.name;

            document.getElementById("<%=editorChiefComplaints.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtHeight.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=TxtWeight.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtHC.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=TxtTemperature.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtRespiration.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtPulse.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtBPSystolic.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtBPDiastolic.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtMAC.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtSpO2.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtWHistory.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtPHistory.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtWPrevTreatment.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtWExamination.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtWNutritionalStatus.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtWPlanOfCare.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=editorNonDrugOrder.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=txtWCostAnalysis.ClientID%>").style.backgroundColor = "AntiqueWhite";
            document.getElementById("<%=editorProvDiagnosis.ClientID%>").style.backgroundColor = "AntiqueWhite";

            CTRL.style.backgroundColor = "AntiqueWhite";
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
        function addChiefComplaintsOnClientClose(oWnd, args) {
            BindPatientChifComplaints();
            $get('<%=btnAddChiefComplaintsClose.ClientID%>').click();
        }
        function addAllergiesOnClientClose(oWnd, args) {
            // Write code
            GetPatientDetail();
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
            <%--var sConString = '<%=System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString.ToString()%>';--%>
            if (document.getElementById('<%=txtHeight.ClientID%>').value != '' && document.getElementById('<%=TxtWeight.ClientID%>').value != '') {
                var txtunit = document.getElementById(ctrlFindText);
                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    PageMethods.CalculateBMIAndBSA("", document.getElementById('<%=txtHeight.ClientID%>').value, document.getElementById('<%=hdnHeight.ClientID%>').value, document.getElementById('<%=TxtWeight.ClientID%>').value, document.getElementById('<%=hdnWeight.ClientID%>').value, OnSucceeded, OnFailed);
                    //$get('<%=btnCalculate.ClientID%>').click();
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
            alert(error);
        }
        //        function CalculateBMI(ctrlFindText) {
        //            var txtunit = document.getElementById(ctrlFindText);
        //            if ((txtunit.value != "") && (txtunit.value != "0")) {
        //                $get('<%=btnCalculate.ClientID%>').click();
        //            }
        //        }
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
    <%--<script type="text/javascript">

        function editorChiefComplaints_OnClientLoad(editor, args) {

            var oFun = function() {
                var oValue = editor.get_html(true); //get the HTML content
                if (oValue.length > 2000) {
                    editor.set_html(oValue.substring(0, 2000));
                    alert("Maximum length is " + 2000 + " characters only.");
                }
            };
            editor.attachEventHandler("onkeyup", oFun);
            editor.attachEventHandler("onkeydown", oFun);

            editor.attachEventHandler("onkeydown", function(e) {

                switch (e.keyCode) {
                    case 114:  // F3
                        $get('<%=btnSave.ClientID%>').click();
                        break;
                    case 115:  // F4
                        $get('<%=btnSaveAsSigned.ClientID%>').click();
                        break;
                }
            });
        }        
        
    </script>--%>
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



    <div class="new_class">
        <a href="#" class="btn btn-danger theme1" id="theme1"></a>
        <a href="#" class="btn btn-success theme2" id="theme2"></a>
        <a href="#" class="btn btn-primary theme3" id="theme3"></a>
        <a href="#" class="btn btn-warning theme4" id="theme4"></a>
    </div>

    <div class="container-fluid emr-main-container">
        <div class="row">

            <div class="col-md-6 pt-dtls-block">


                <div class="row pt-block">
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-3 col-xs-3">
                                <img src="../../Images/PImageBackGround.gif" class="img-circle" width="50" />
                            </div>
                            <div class="col-md-9 col-xs-9">
                                <div class="visit-date">Date: 30 Oct 2018</div>
                                <%--<div class="pt-name" data-toggle="tooltip" title="Rahul Saxena">Rahul Saxena</div>--%>
                                <div class="pt-name"></div>
                                <div class="pt-dtls">male/35 Yr, DOB: 28/05/1984 ID: 100001529 | Enc #: 2609</div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-5 allery-col">
                        <div class="row">
                            <h4>Allergy<a href="#" class="edit-icon pull-left" onclick="Allergies();"><img src="../../Images/edit.svg" width="12" /></a></h4>


                            <ul id="ulAllergy">
                                <%-- <li>Sulphur contain drugs</li>
                                <li>Food Additives</li>
                                <li>Cow milk/lactose intolerance</li>

                                <li>Sulphur contain drugs</li>
                                <li>Cow milk/lactose intolerance</li>--%>
                            </ul>





                        </div>
                    </div>
                    <div class="col-md-3 bg-danger text-danger">

                        <div id="cronics" class="cronics-div">
                            <div id="nocronic">No Chronic Diagnosis</div>
                        </div>
                    </div>
                </div>

                <div class="subheading_main row">

                    <div class="col-md-12 text-center">
                        <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="col-md-3">
                                <asp:LinkButton ID="lbtnExpand" Font-Bold="true" Font-Size="Smaller" runat="server"
                                    Text="Expand All" Font-Overline="false" OnClick="lbtnExpand_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="col-md-9 text-right">

                        <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                            <%--    <button type="button" >1</button>
                            <button type="button" >2</button>--%>

                            <asp:LinkButton ID="lbtnPastClinicalNotes" Font-Bold="true" runat="server" Font-Size="Smaller"
                                Text="Past Clinical Notes" Font-Underline="false" Font-Overline="false" OnClick="lbtnPastClinicalNotes_Click" CssClass="btn btn-links hidden" />

                            <asp:LinkButton ID="lnkpreauth" runat="server" Text="Pre-Auth" Font-Size="Smaller" Font-Bold="true"
                                OnClick="lnkpreauth_Click" Visible="false" CssClass="btn btn-links" class="btn btn-secondary" style="font-size: 12px;"></asp:LinkButton>


                            
                                    <asp:LinkButton ID="btnSave" Text="Save As Draft" runat="server"
                                        OnClick="btnSaveDashboard_OnClick" class="dropdown-item btn-sm btn-primary" />
                                    <asp:LinkButton ID="btnSaveAsSigned" Text="Save As Signed" runat="server" class="dropdown-item btn-sm btn-primary"
                                        OnClick="btnSaveAsSigned_OnClick" Visible="false" />
                                    <asp:LinkButton ID="lnkCopyLastPrescription" runat="server"
                                        Text="Last OPD Summary" class="dropdown-item btn-sm btn-primary" ToolTip="Copy Last Prescription" OnClick="lnkCopyLastPrescription_Click" />
                                    <asp:Button ID="btnBackToMenu" Text="Back To Menu" runat="server" SkinID="Button"
                                        Font-Size="Smaller" OnClick="btnBackToMenu_OnClick" Visible="false" CssClass="dropdown-item btn-sm btn-primary" />

                                    <asp:LinkButton ID="lnkIPExtension" runat="server" Text="Online Extension" OnClick="lnkIPExtension_Click" CssClass="dropdown-item btn-sm btn-primary" Visible="false"></asp:LinkButton>

                                    <asp:LinkButton ID="btnAssigntoMe" runat="server" Text="Assign to me" Visible="false" CssClass="dropdown-item btn-sm btn-primary" OnClick="btnAssigntoMe_Click" />

                                    <asp:LinkButton ID="lnktriageform" runat="server" Text="Triage form" OnClick="lnktriageform_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="btnDefinalise" Text="Definalized" runat="server" CssClass="dropdown-item btn-sm btn-primary"
                                        OnClick="btnDefinalise_OnClick" Visible="false" />
                                    <asp:LinkButton ID="btnICCA" runat="server" CausesValidation="false" CssClass="dropdown-item btn-sm btn-primary"
                                        Text="ICCA Viewer" OnClick="btnICCA_OnClick" Visible="false" />
                                    <asp:HiddenField ID="hdnButtonId" runat="server" />
                                    <asp:LinkButton ID="btnEnableControl" runat="server" Style="visibility: hidden;" OnClick="btnEnableControl_OnClick" />

                        </div>


                    </div>


                    <ul class="col-md-10 list-inline fixed-left-icons hidden">
                        <li ><a href="#ctl00_ContentPlaceHolder1_tblVitals" id="IcVitals" style="display: none;" data-toggle="tooltip" title="Vitals">
                            <img src="../../Images/vital-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_tblChiefComplaints" id="IclChiefComplaints" style="display: none;" data-toggle="tooltip" title="Chief Complaints">
                            <img src="../../Images/chief-complaint-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trHistory" data-toggle="tooltip" id="IcHisPIllness" style="display: none;" title="History of Present Illness">
                            <img src="../../Images/present-illness-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trPastHistory" data-toggle="tooltip" id="IcPastHistory" style="display: none;" title="Past History">
                            <img src="../../Images/past-history-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trExamination" data-toggle="tooltip" id="IcExamination" style="display: none;" title="Examination">
                            <img src="../../Images/examination-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trOtherNotes" data-toggle="tooltip" id="IcCareTemplate" style="display: none;" title="Care Template">
                            <img src="../../Images/care-templates-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trProvisionalDiagnosis" data-toggle="tooltip" id="IcProvisionalDiagnosis" style="display: none;" title="Provisional Diagnosis">
                            <img src="../../Images/diagnosis-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_divDiagnosisDetails" data-toggle="tooltip" title="Diagnosis" id="IcDiagnosis" style="display: none;">
                            <img src="../../Images/diagnosis2-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trPlanOfCare" data-toggle="tooltip" title="Plan Of Care" id="IcPlanOfCare" style="display: none;">
                            <img src="../../Images/plan-care-icon.svg" width="22" /></a></li>

                        <li><a href="#ctl00_ContentPlaceHolder1_trOrdersAndProcedures" data-toggle="tooltip" title="Orders and Procedures" id="IcOrdersandProcedures" style="display: none;">
                            <img src="../../Images/orders-procedures-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trPrescriptions" data-toggle="tooltip" title="Prescriptions" id="IcPrescriptions" style="display: none;">
                            <img src="../../Images/prescription-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trNonDrugOrder" data-toggle="tooltip" title="Other Order" id="IcOtherOrder" style="display: none;">
                            <img src="../../Images/other-order-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trPatientFamilyEducationCounseling" data-toggle="tooltip" title="Patient and family education and counselling" id="IcPatientandfamilyeducationandcounselling" style="display: none;">
                            <img src="../../Images/counseling-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trReferralsReplyToReferrals" data-toggle="tooltip" title="Referrals &amp; Reply to referrals"  id="IcReferralsReplytoreferrals" style="display: none;">
                            <img src="../../Images/referrals-icon.svg" width="22" /></a></li>
                        <li><a href="#ctl00_ContentPlaceHolder1_trMultidisciplinaryEvaluationPlanOfCare" data-toggle="tooltip" title="Multidisciplinary Evaluation And Plan Of Care" id="IcMultidisciplinaryEvaluationAndPlanOfCare" style="display: none;">
                            <img src="../../Images/multidisciplinary-evaluation-icon.svg" width="22" /></a></li>
                    </ul>



                    <div class="col-md-2 text-right hidden" style="margin-top: 10px;">
                        <b style="font-size: 12px; margin-right: 10px;">Text Note</b>
                        <label class="switch">
                            <input type="checkbox" checked>
                            <span class="slider round"></span>
                        </label>
                    </div>




                </div>


                <div class="accordion-section">
                    <div class="accord-inner">
                        <div class="block-contain">
                        <h2 style=""><span>Current Visit</span> </h2>     
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="dvErdata" runat="server" visible="false" style="color: red; font-size: medium;">
                                </div>
                                <asp:Label ID="Label21" Text=" " runat="server" CssClass="text-center" />

                                <div id="trVitals" runat="server">

                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                        <ContentTemplate>
                                            <div id="tblVitals" runat="server">

                                                <div class="container-fluid emrPart-Green">

                                                    <p>
                                                        <asp:ImageButton ID="imgVbtnVital" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Vitals" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label14" runat="server" SkinID="label" Text="Vitals" /></h3>
                                                    <span id="spnVitalsStar" class="red" visible="false" runat="server">*</span>

                                                    <asp:ImageButton ID="imgBtnAddVitals" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Add Vitals" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddVitals_OnClick" />
                                                    <asp:Label ID="lblVitalMessage" runat="server" Text="" CssClass="pull-right" />
                                                </div>

                                                <div class="table-responsive" style="clear: both;">
                                                    <table class="table-bordered table input-vitals">
                                                        <tr>
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
                                                                <asp:HiddenField ID="hdnSpO2" Value="27" runat="server" />
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
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="TxtWeight" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="txtHC" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" />

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtTemperature" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" />

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRespiration" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="txtPulse" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="txtBPSystolic" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="txtBPDiastolic" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="txtMAC" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:TextBox ID="txtSpO2" runat="server" Width="40px" MaxLength="6" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: White; font-size: 11px;"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" /></td>
                                                            <td>
                                                                <asp:HiddenField ID="hdnBMIValue" runat="server" />
                                                                <asp:TextBox ID="txtBMI" runat="server" Width="55px" ReadOnly="true" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: LightGray; font-size: 11px;" /></td>
                                                            <td>
                                                                <asp:HiddenField ID="hdnBSAValue" runat="server" />
                                                                <asp:TextBox ID="txtBSA" runat="server" Width="55px" ReadOnly="true" Style="font-family: arial, verdana, helvetica, sans-serif; border: 1px solid #6699CC; color: Black; vertical-align: middle; background: LightGray; font-size: 11px;" /></td>

                                                        </tr>
                                                    </table>
                                                </div>









                                                <asp:Panel ID="pnlVitals" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvVitals" CssClass="table table-bordered table-custom" runat="server" AutoGenerateColumns="false"
                                                                ShowHeader="true" Width="100%" Height="100%" HeaderStyle-Height="3px" OnRowDataBound="gvVitals_RowDataBound"
                                                                OnRowCommand="gvVitals_OnRowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Vital Date" HeaderStyle-Width="130px" HeaderStyle-ForeColor="Black">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text='<%#Eval("Vital Date")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label29" runat="server" Text="HT" ToolTip="Height" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkHT" runat="server" CommandName="HT" Text='<%#Eval("HT")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label30" runat="server" Text="WT" ToolTip="Weight" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkWT" runat="server" CommandName="WT" Text='<%#Eval("WT")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label31" runat="server" Text="HC" ToolTip="Head Circumference" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkHC" runat="server" CommandName="HC" Text='<%#Eval("HC")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label32" runat="server" Text="T" ToolTip="Temperature" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkT" runat="server" CommandName="T" Text='<%#Eval("T")%>' Width="100%"
                                                                                Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                            <asp:HiddenField ID="hdnT_ABNORMAL_VALUE" runat="server" Value='<%#Eval("T_ABNORMAL_VALUE")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label33" runat="server" Text="R" ToolTip="Respiration" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkR" runat="server" CommandName="R" Text='<%#Eval("R")%>' Width="100%"
                                                                                Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                            <asp:HiddenField ID="hdnR_ABNORMAL_VALUE" runat="server" Value='<%#Eval("R_ABNORMAL_VALUE")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label34" runat="server" Text="P" ToolTip="Pulse" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkP" runat="server" CommandName="P" Text='<%#Eval("P")%>' Width="100%"
                                                                                Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                            <asp:HiddenField ID="hdnP_ABNORMAL_VALUE" runat="server" Value='<%#Eval("P_ABNORMAL_VALUE")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label35" runat="server" Text="BPS" ToolTip="BP Systolic" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBPS" runat="server" CommandName="BPS" Text='<%#Eval("BPS")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label36" runat="server" Text="BPD" ToolTip="BP Diastolic" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBPD" runat="server" CommandName="BPD" Text='<%#Eval("BPD")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label37" runat="server" Text="MAC" ToolTip="Mid Arm Circumference"
                                                                                ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkMAC" runat="server" CommandName="MAC" Text='<%#Eval("MAC")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label38" runat="server" Text="SpO2" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSpO2" runat="server" CommandName="SpO2" Text='<%#Eval("SpO2")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label39" runat="server" Text="BMI" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBMI" runat="server" CommandName="BMI" Text='<%#Eval("BMI")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="Label40" runat="server" Text="BSA" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBSA" runat="server" CommandName="BSA" Text='<%#Eval("BSA")%>'
                                                                                Width="100%" Font-Underline="false" ForeColor="Black" Font-Size="Smaller" />
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
                                                </asp:Panel>

                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                                <div id="trChiefComplaints" runat="server">

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
                                                <div class="container-fluid emrPart-Green form-group">
                                                    <p>
                                                        <asp:ImageButton ID="imgbtnChiefComplaints" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Chief Complaints" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label20" runat="server" CssClass="Label3" Text="Chief Complaints (Free Text)" />
                                                        <span id="spnChiefComplaintsStar" class="red" visible="false" runat="server">*</span></h3>

                                                    <asp:ImageButton ID="imgBtnAddChiefComplaints" runat="server" ImageUrl="~/Images/add.png" CssClass="margin_Top01"
                                                        data-toggle="tooltip" title="Add Chief Complaints" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddChiefComplaints_OnClick" />
                                                    <asp:Button ID="btnCheifComplainthistory" CssClass="btn btn-link  copy-data" runat="server"
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

                                                        <div class="row form-group">
                                                            <div class="col-md-12">
                                                                <asp:TextBox ID="editorChiefComplaints" runat="server" TextMode="MultiLine" CssClass="form-group" Height="135px"
                                                                    Width="100%" Style="resize: none;" onkeyup="return MaxLenTxt(this,1500);"
                                                                    onkeydown="return SetIsTransitDataEntered(this);" />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <asp:GridView ID="gvProblemDetails" runat="server" CssClass="table table-bordered" Width="100%"
                                                                    AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="gvProblemDetails_RowDataBound"
                                                                    OnRowCommand="gvProblemDetails_RowCommand" OnPageIndexChanging="gvProblemDetails_PageIndexChanging"
                                                                    OnRowCancelingEdit="gvProblemDetails_OnRowCancelingEdit" OnRowUpdating="gvProblemDetails_OnRowUpdating"
                                                                    OnRowEditing="gvProblemDetails_OnRowEditing">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Chief Complaints" HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="editorProblem" runat="server" TextMode="MultiLine" Width="100%"
                                                                                    Height="20px" Style="resize: none" onkeyup="return MaxLenTxt(this,1500);" />
                                                                                <asp:HiddenField ID="hdnProblemId" runat="server" Value='<%#Eval("Id")%>' />
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
                                                                        <asp:TemplateField HeaderText="Entered By" ItemStyle-Width="150px" ItemStyle-VerticalAlign="Top"
                                                                            HeaderStyle-ForeColor="Black">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("EnteredBy")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                            ItemStyle-Width="10px" HeaderStyle-Width="10px" />
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

                                                        </div>

                                                    </div>

                                                    <asp:HiddenField ID="prblmID" runat="server" />
                                                    <asp:HiddenField ID="hdnID" runat="server" />
                                                    <div id="dvConfirmCancelOptions" runat="server" style="width: 300px; z-index: 200; border: 4px solid #cccccc; background-color: #FFF8DC; position: fixed; bottom: 0; height: 100px; left: 18%; margin: auto; top: 200px; text-align: center;">
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
                                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Allergies" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="Allergies" />
                                                        <span id="spnAllergiesStar" class="red" visible="false" runat="server">*</span>
                                                    </h3>

                                                    <asp:ImageButton ID="imgBtnAddAllergies" runat="server" ImageUrl="~/Images/add.png"
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
                                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Immunisation History" Height="16px" Enabled="false" Width="16px" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="Immunisation History" />
                                                        <span id="spnImmunisationHistory" class="red" visible="false" runat="server">*</span>
                                                    </h3>

                                                    <asp:ImageButton ID="lnkImmunisationHistory" runat="server" ImageUrl="~/Images/add.png"
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
                                                        <asp:ImageButton ID="imbtnHistory" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="History of Present illness" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label17" runat="server" SkinID="label" Text="History of Present illness" />
                                                        <span id="spnHistoryStar" class="red" visible="false" runat="server">*</span></h3>

                                                    <asp:ImageButton ID="ImageButton1" runat="server" CssClass="margin_Top" ImageUrl="~/Images/add.png" data-toggle="tooltip" title="Add Templates" data-placement="left"
                                                        OnClick="lnkAddTemplatesHistory_OnClick" />

                                                    <asp:Button ID="btnShowHistory" runat="server" Text="Copy Past Data" CssClass="btn btn-link copy-data" OnClick="btnShowHistory_Click" />

                                                    <asp:Label ID="lblHistoryMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="text-right" />


                                                </div>
                                                <div class="clearfix"></div>
                                                <asp:Panel ID="pnlHistory" runat="server" CssClass="form-group">
                                                    <div class="fst-box form-group">
                                                        <asp:TextBox ID="txtWHistory" runat="server" TextMode="MultiLine" Text=""
                                                            Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);"
                                                            onkeydown="return SetIsTransitDataEntered(this);" Height="135px" />
                                                    </div>
                                                    <asp:Panel ID="Panel9" runat="server">
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
                                                                        <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
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
                                                        <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Past History" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label42" runat="server" SkinID="label" Text="Past History" />
                                                        <span id="spnPastHistory" class="red" visible="false" runat="server">*</span>
                                                    </h3>
                                                    <asp:ImageButton ID="lnkPastHistory" runat="server" ImageUrl="~/Images/add.png" data-toggle="tooltip" title="Past History" data-placement="left"
                                                        OnClick="lnkPastHistory_OnClick" />
                                                    <asp:Label ID="lblPHistoryMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                </div>

                                                <%--<div class="clearfix"></div>--%>
                                                <asp:Panel ID="Panel23" CssClass="form-group" runat="server">
                                                    <div class="fst-box form-group">
                                                        <asp:TextBox ID="txtPHistory" runat="server" TextMode="MultiLine" Height="135px"
                                                            Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                    </div>

                                                    <asp:Panel ID="Panel24" runat="server">
                                                        <asp:GridView ID="gvPHistory" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
                                                            OnRowDataBound="gvPHistory_OnDataBinding" Width="100%" Height="100%" AllowPaging="false"
                                                            PageSize="1" OnPageIndexChanging="gvPHistory_PageIndexChanging" OnRowCancelingEdit="gvPHistory_OnRowCancelingEdit"
                                                            OnRowUpdating="gvPHistory_OnRowUpdating" OnRowEditing="gvPHistory_OnRowEditing">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Past History" HeaderStyle-ForeColor="black">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="editorHistory" runat="server" TextMode="MultiLine" Width="100%"
                                                                            Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" Height="75px" />
                                                                        <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                        <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-VerticalAlign="Top"
                                                                    ItemStyle-Width="10px" HeaderStyle-Width="10px" HeaderStyle-ForeColor="black" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>

                                                    <%--</table>--%>
                                                </asp:Panel>

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
                                                        <asp:ImageButton ID="imgbtnTemplate" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Template" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label19" runat="server" SkinID="label" Text="Examination" />
                                                        <span id="spnExaminationStar" class="red" visible="false" runat="server">*</span>
                                                    </h3>
                                                    <asp:ImageButton ID="imgBtnTemplates" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Add Templates" data-placement="left" Height="20px" Width="20px" OnClick="lnkAddTemplates_OnClick" />
                                                    <%--<asp:Button ID="btnCopyExamination" runat="server" CssClass="btn btn-primary margin_Top" 
                                        Text="Copy Past Data" OnClick="btnCopyExamination_Click" />--%>
                                                    <asp:Label ID="lblExamMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                </div>
                                                <div class="clearfix"></div>
                                                <asp:Panel ID="Panel10" runat="server">
                                                    <div class="fst-box form-group">
                                                        <asp:TextBox ID="txtWExamination" runat="server" TextMode="MultiLine" Height="135px"
                                                            Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                    </div>

                                                    <asp:Panel ID="pnlExamination" runat="server">
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                    HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                        <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                        <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
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
                                                        <asp:ImageButton ID="ibtNutritionalStatus" runat="server" ImageUrl="~/Images/Expand.jpg"
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
                                                    <div class="fst-box form-group">
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                    HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                        <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                        <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
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
                                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Expand.jpg"
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
                                                    <div class="fst-box form-group">
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                    HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                        <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                        <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
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
                                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/Expand.jpg"
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
                                                    <div class="fst-box">
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                    HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                        <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                        <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
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
                                                        <asp:ImageButton ID="imgbtntherNotes" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Care Templates" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label7" runat="server" SkinID="label" Text="Care Templates&nbsp;" /></h3>
                                                    <asp:ImageButton ID="lnkAddTemplates_All" runat="server" ImageUrl="~/Images/add.png"
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
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Entered By" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                                HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                    <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                    <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                    <asp:HiddenField ID="hdnTemplateType" runat="server" Value='<%#Eval("TemplateType")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="View" Text="View" OnClick="lnkView_OnClik" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnlEdit" runat="server" Text="Edit" OnClick="lnlEdit_OnClik" />
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
                                                        <asp:ImageButton ID="imgbtnProvisionalDiagnosies" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Provisional Diagnosis" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label15" runat="server" SkinID="label" Text="Provisional&nbsp;Diagnosis" />
                                                        <span id="spnProvisionalDiagnosisStar" class="red" visible="false" runat="server">*</span>
                                                    </h3>
                                                    <asp:ImageButton ID="imgBtnProvisionalDiagnosis" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Add Provisional Diagnosis" data-placement="left" Height="20px" Width="20px" OnClick="imgBtnProvisionalDiagnosis_OnClick" />
                                                    <asp:Label ID="lblProvDiag" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                </div>

                                                <div class="clearfix"></div>


                                                <asp:Panel ID="pnlProvisionalDiagnosis" runat="server">
                                                    <div class="container-fluid margin_Top">

                                                        <div class="col-md-4">
                                                            <div class="row form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Label ID="Label12" runat="server" Text="Search Keyword" SkinID="label" />&nbsp;<span
                                                                        style="color: #FF0000">*</span>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <asp:DropDownList ID="ddlDiagnosisSearchCodes" runat="server" SkinID="DropDown" Width="120px"
                                                                        DropDownWidth="250px" onchange="return SetIsTransitDataEntered(this);" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <div class="row form-group">
                                                                <div class="col-md-3">
                                                                    <asp:LinkButton ID="btnAddDiag" runat="server" SkinID="Button" Text="Add New Search Keyword"
                                                                        CausesValidation="false" OnClick="btnAddDiag_OnClick" />
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <asp:TextBox ID="editorProvDiagnosis" runat="server" Style="resize: none;" onkeyup="return MaxLenTxt(this,500);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>




                                                    <div class="container-fluid">
                                                        <asp:GridView ID="gvData" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                            AllowPaging="false" PageSize="1" Width="100%" Height="100%" OnRowCommand="gvData_OnRowCommand"
                                                            OnRowDataBound="gvData_RowDataBound" OnRowCancelingEdit="gvData_OnRowCancelingEdit"
                                                            OnRowUpdating="gvData_OnRowUpdating" OnRowEditing="gvData_OnRowEditing" OnPageIndexChanging="gvData_PageIndexChanging">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Provisional Diagnosis" HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="editorProvisionalDiagnosis" runat="server" TextMode="MultiLine"
                                                                            Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,500);" />
                                                                        <asp:HiddenField ID="hdnProvisionalDiagnosis" runat="server" Value='<%#Eval("ProvisionalDiagnosis")%>' />
                                                                        <asp:HiddenField ID="hdnProvisionalDiagnosisID" runat="server" Value='<%#Eval("Id")%>' />
                                                                        <asp:HiddenField ID="hdnDiagnosisSearchId" runat="server" Value='<%#Eval("SearchKeyWordId")%>' />
                                                                        <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top"
                                                                    HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="120px" HeaderText="Entered By" ItemStyle-VerticalAlign="Top"
                                                                    HeaderStyle-ForeColor="Black">
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
                                                        </asp:GridView>
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
                                                    <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                        ToolTip="Final Diagnosis" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                </p>
                                                <h3>
                                                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Diagnosis" /></h3>
                                                <asp:ImageButton ID="imgBtnFinalDiagnosis" runat="server" ImageUrl="~/Images/add.png"
                                                    data-toggle="tooltip" title="Final Diagnosis" data-placement="left" OnClick="imgBtnFinalDiagnosis_OnClick" />
                                            </div>

                                            <asp:Panel ID="pnlDiagnosis" runat="server">

                                                <asp:GridView ID="gvDiagnosisDetails" runat="server" AutoGenerateColumns="false"
                                                    HeaderStyle-HorizontalAlign="Left" CssClass="table table-bordered" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="60px" HeaderStyle-Width="60px"
                                                            HeaderStyle-ForeColor="Black">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true" HeaderStyle-ForeColor="Black">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Primary" HeaderStyle-ForeColor="Black">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:GridView>


                                            </asp:Panel>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <div id="trPlanOfCare" runat="server">

                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <div id="tblPlanOfCare" runat="server">

                                                <div class="container-fluid emrPart-Green">
                                                    <p>
                                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Plan Of Care" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label24" runat="server" SkinID="label" Text="Plan Of Care" />
                                                        <span id="spnPlanOfCareStar" class="red" visible="false" runat="server">*</span>
                                                    </h3>
                                                    <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/add.png" data-toggle="tooltip" title="Plan Of Care" data-placement="left"
                                                        OnClick="lnkPlanOfCare_OnClick" />
                                                    <asp:Label ID="lblPlanOfCareMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="pull-right" />
                                                </div>
                                                <div class="clearfix"></div>
                                                <asp:Panel ID="Panel13" runat="server">
                                                    <div class="fst-box">
                                                        <asp:TextBox ID="txtWPlanOfCare" runat="server" TextMode="MultiLine" Height="135px"
                                                            Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
                                                    </div>
                                                    <asp:Panel ID="pnlPlanOfCare" runat="server">
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="120px" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px"
                                                                    HeaderText="Entered By" HeaderStyle-ForeColor="Black">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblencodedby" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                                        <asp:HiddenField ID="hdnTemplateID" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                                        <asp:HiddenField ID="hdnRecordId" runat="server" Value='<%#Eval("RecordId")%>' />
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

                                <div id="trOrdersAndProcedures" runat="server">

                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                        <ContentTemplate>
                                            <div id="tblOrdersAndProcedures" runat="server">

                                                <div class=" emrPart-Green container-fluid">
                                                    <p>
                                                        <asp:ImageButton ID="imgbtnOrdersAndProcedures" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Orders And Procedures" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label13" runat="server" SkinID="label" Text="Orders&nbsp;And&nbsp;Procedures&nbsp;" /></h3>
                                                    <asp:ImageButton ID="imgbtnAddOrdersAndProcedures" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Add Orders And Procedures" data-placement="left" OnClick="lnkAddOrdersAndProcedures_OnClick" />
                                                </div>

                                                <asp:Panel ID="pnlOrderProcedures" runat="server">
                                                    <asp:GridView ID="gvOrdersAndProcedures" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="true" Width="100%" Height="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="130px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblODate" runat="server" SkinID="label" Text='<%#Eval("OrderDate")%>' />
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
                                                    <asp:Repeater ID="rptPagerOrdersAndProcedures" runat="server">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPageOrdersAndProcedures" runat="server" Text='<%#Eval("Text")%>'
                                                                Font-Bold="true" CommandArgument='<%#Eval("Value")%>' Enabled='<%#Eval("Enabled")%>'
                                                                OnClick="lnkPageOrdersAndProcedures_OnClick" />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
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
                                                        <asp:ImageButton ID="imgbtnPrescription" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Prescriptions" Height="16px" Width="16px" OnClick="ViewHide_OnClick" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label16" runat="server" SkinID="label" Text="Prescriptions&nbsp;" /></h3>
                                                    <asp:ImageButton ID="imgBtnAddPrescriptions" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Add Prescriptions" data-placement="left" OnClick="lnkAddPrescriptions_OnClick" />
                                                </div>

                                                <asp:Panel ID="pnlPrescription" runat="server" CssClass="table-col-auto">
                                                    <asp:GridView ID="gvPrescriptions" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="true" Width="100%" Height="100%">
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSDate" runat="server" SkinID="label" Text='<%#Eval("StartDate")%>' />
                                                                    <asp:HiddenField ID="hdnItemName" runat="server" Value='<%#Eval("ItemName")%>' />
                                                                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="200px" HeaderText="Store Name" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStoreName" runat="server" SkinID="label" Text='<%#Eval("StoreName")%>' />
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
                                                    <asp:Repeater ID="rptPagerPrescriptions" runat="server">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPagePrescriptions" runat="server" Text='<%#Eval("Text")%>'
                                                                Font-Bold="true" CommandArgument='<%#Eval("Value")%>' Enabled='<%#Eval("Enabled")%>'
                                                                OnClick="lnkPagePrescriptions_OnClick" />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
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
                                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/Expand.jpg"
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


                                                    <asp:Label ID="Label5" runat="server" Text="Order Type" />
                                                    <span style="color: Red">*</span>

                                                    <asp:DropDownList ID="ddlOrderType" runat="server" SkinID="DropDown" Width="70px"
                                                        onchange="return SetIsTransitDataEntered(this);">
                                                        <asp:ListItem Text="Routine" Value="R" />
                                                        <asp:ListItem Text="Urgent" Value="U" />
                                                        <asp:ListItem Text="Stat" Value="S" />
                                                        <asp:ListItem Text="SOS" Value="O" />
                                                    </asp:DropDownList>

                                                    <asp:Label ID="lblDoctor" runat="server" Text="Doctor" />
                                                    <span style="color: Red">*</span>

                                                    <asp:DropDownList ID="ddlDoctor" runat="server" SkinID="DropDown" Width="170px" DropDownWidth="250"
                                                        onchange="return SetIsTransitDataEntered(this);" />
                                                    <div class="fst-box">
                                                        <asp:TextBox ID="editorNonDrugOrder" runat="server" TextMode="MultiLine" Height="90px"
                                                            Width="100%" Style="resize: none" onkeyup="return MaxLenTxt(this,8000);" onkeydown="return SetIsTransitDataEntered(this);" />
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
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Entered By" ItemStyle-VerticalAlign="Top" HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>' />
                                                                    <asp:HiddenField ID="hdnNonDrugOrderId" runat="server" Value='<%#Eval("NonDrugOrderId")%>' />
                                                                    <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%#Eval("OrderType")%>' />
                                                                    <asp:HiddenField ID="hdnNurseId" runat="server" Value='<%#Eval("NurseId")%>' />
                                                                    <asp:HiddenField ID="hdnAcknowledgeBy" runat="server" Value='<%#Eval("AcknowledgeBy")%>' />
                                                                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
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



                                <div id="trPatientFamilyEducationCounseling" runat="server">

                                    <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                        <ContentTemplate>
                                            <div id="Table12" runat="server">

                                                <div class="container-fluid emrPart-Green">
                                                    <p>
                                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Patient and family education and counselling " Enabled="false" Height="16px"
                                                            Width="16px" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="Patient and family education and counselling" />
                                                        <span id="spnPatientFamilyEducationCounseling" class="red" visible="false" runat="server">*</span>
                                                    </h3>
                                                    <asp:ImageButton ID="lnkEducationCounseling" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Patient and family education" data-placement="left"
                                                        OnClick="lnkEducationCounseling_OnClick" />

                                                </div>

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
                                                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Referrals & Reply to referrals" Enabled="false" Height="16px" Width="16px" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label26" runat="server" SkinID="label" Text="Referrals & Reply to referrals" />
                                                    </h3>
                                                    <asp:ImageButton ID="lnkReferrals" runat="server" ImageUrl="~/Images/add.png" data-toggle="tooltip" title="Referrals & Reply to referrals" data-placement="left"
                                                        OnClick="lnkReferrals_OnClick" />
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
                                                        <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Anaesthesia and Critical care notes" Enabled="false" Height="16px" Width="16px" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label27" runat="server" SkinID="label" Text="Anaesthesia and Critical care notes" /></h3>

                                                    <asp:ImageButton ID="lnkAnaesthesiaCritical" runat="server" ImageUrl="~/Images/add.png"
                                                        ToolTip="Anaesthesia and Critical care notes" OnClick="lnkAnaesthesiaCritical_OnClick" />

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
                                                        <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="~/Images/Expand.jpg"
                                                            ToolTip="Multidisciplinary evaluation and plan of care" Enabled="false" Height="16px"
                                                            Width="16px" />
                                                    </p>
                                                    <h3>
                                                        <asp:Label ID="Label28" runat="server" SkinID="label" Text="Multidisciplinary evaluation and plan of care" /></h3>
                                                    <asp:ImageButton ID="lnkMultidisciplinaryEvaluation" runat="server" ImageUrl="~/Images/add.png"
                                                        data-toggle="tooltip" title="Multidisciplinary Evaluation" data-placement="left"
                                                        OnClick="lnkMultidisciplinaryEvaluation_OnClick" />

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
                                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                                        <Windows>
                                                            <telerik:RadWindow ID="RadWindowForNew" Skin="Metro" runat="server" EnableViewState="false" Height="620px" Width="620px"
                                                                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" VisibleStatusbar="false" Behaviors="Close" />
                                                        </Windows>
                                                    </telerik:RadWindowManager>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel44" runat="server">
                                                <ContentTemplate>
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
                                                    <asp:Button ID="btnProvisionalDiagnosisClose" runat="server" Style="visibility: hidden;"
                                                        OnClick="btnProvisionalDiagnosisClose_OnClick" />
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
                                <div style="width: 154px; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px" class="fade">
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

                    <asp:UpdatePanel ID="UpdatePanel42" runat="server">
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

            <a href="#" id="btn-expand-collapse"><i class="fas fa-angle-right"></i></a>

            <div class="col-md-5 mid-box">
                <div class="row">

                    <div class="col-md-12 item">

                         <div class="block-contain" style="padding: 0 10px;">
                        <h2><span>Patient Visit</span> </h2> 

                        
                        <nav id="menu-container" class="arrow">
            <div id="btn-nav-previous"><i class="fas fa-chevron-circle-left"></i></div>
            <div id="btn-nav-next"><i class="fas fa-chevron-circle-right"></i></div>  
            <div class="menu-inner-box">
                        <ul id="content-slider" class="menu">
                            <%--<li class="active"><a data-toggle="tab" href="#home1">Home</a></li>
                        <li><a data-toggle="tab" href="#menu1">Menu 1</a></li>
                        <li><a data-toggle="tab" href="#menu2">Menu 2</a></li>
                        <li><a data-toggle="tab" href="#menu3">Menu 3</a></li>--%>
                        </ul>
                </div>
                            </nav>

             
                           
                 

                        <div class="tab-content" id="tabPasthistory">
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
                 


                    <div class="col-md-12" style="padding-right: 0;">
                        <div class="block-contain">
                        <h2><span>Previous Visit</span> </h2>                   

                    <div class="col-md-6 box">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-title bg-primary ">DIAGNOSIS <a class="pull-right" data-toggle="tooltip" data-placement="left" title="Diagnosis History" onclick="Diagnosis();" style="cursor: pointer;"><i class="far fa-clone"></i></a></h5>
                                <img src="/Images/ajax-loader3.gif" id="loder1" class="img" />
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
                                    <h5>Record not found</h5>
                                </div>
                            </div>
                        </div>





                    </div>
                    <div class="col-md-6 box">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-title  bg-primary">MEDICATION <a class="pull-right" data-toggle="tooltip" data-placement="left" title="Medication History" style="cursor: pointer;" onclick="Prescriptions()"><i class="far fa-clone"></i></a></h5>
                                <img src="/Images/ajax-loader3.gif" id="loder2" style="display: none;" class="img" />
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
                                    <h5>Record not found</h5>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 box">
                        <div class="card">
                            <div class="card-body">

                                <h5 class="card-title  bg-primary">IMMUNIZATION <a class="pull-right" style="cursor: pointer;" onclick="Immunization()" data-toggle="tooltip" data-placement="left" title="Immunization History"><i class="far fa-clone"></i></a></h5>
                                <img src="/Images/ajax-loader3.gif" id="loder3" style="display: none;" class="img" />
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
                                <div id="msg3" style="display: none;">
                                    <h5>Record not found</h5>
                                </div>
                            </div>
                        </div>





                    </div>
                    <div class="col-md-6 box">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-title bg-primary">LAB ORDER <a class="pull-right" style="cursor: pointer;" onclick="LabOrders();" data-toggle="tooltip" data-placement="left" title="Lab Order History"><i class="far fa-clone"></i></a></h5>
                                <img src="/Images/ajax-loader3.gif" id="loder4" style="display: none;" class="img" />
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
                                    <h5>Record not found</h5>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 box">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-title bg-primary">CHIEF COMPLAINTS <a class="pull-right" onclick="ChiefComplaints();" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Chief Complaint History"><i class="far fa-clone"></i></a></h5>
                                <img src="/Images/ajax-loader3.gif" id="loder5" style="display: none;" class="img" />
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
                                    <h5>Record not found</h5>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 box">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-title bg-primary">PAST HISTORY <a class="pull-right" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Past History" onclick="PastHistory();"><i class="far fa-clone"></i></a></h5>
                                <img src="/Images/ajax-loader3.gif" id="loder6" style="display: none;" class="img" />
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
                                    <h5>Record not found</h5>
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
                    <li><a onclick="PastClinicalNotes();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Past Clinical Notes">
                        <span class="far fa-edit"></span></a></li>

                    <li><a onclick="Vitals();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Vitals"><i class="fas fa-heartbeat"></i></a></li>
                    <li><a onclick="GrowthChart();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Growth Chart"><i class="fas fa-chart-line"></i></a></li>
                    <li><a onclick="Attachment();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Attachment"><i class="fas fa-paperclip"></i></a></li>
                    <li><a onclick="RIS();" style="cursor: pointer" data-toggle="tooltip" data-placement="left" title="Radiology Results"><i class="fas fa-radiation"></i></a></li>
                    <li><a onclick="LIS();" style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Lab Results"><i class="fa fa-flask"></i></a></li>
                    <li><a style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Video"><i class="fa fa-video"></i></a></li>
                    <li><a style="cursor: pointer;" data-toggle="tooltip" data-placement="left" title="Microphone"><i class="fas fa-microphone-alt"></i></a></li>
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
                                            <div class="form-group col-md-8">
                                                <label>Patient</label>
                                                <input type="text" id="txtpatient" placeholder="" class="form-control" readonly="readonly" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label>Visit No</label>
                                                <input type="text" id="txtvisit" placeholder="" class="form-control" readonly="readonly" />

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
        </div>

        <script src="/Include/JS/StringBuilder.js"></script>
        <script type="text/javascript">
            window.scrollTo = function () { }
            window.onload = setTimeout;
            var str ="";
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';
            $(document).ready(function () {
           
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
                $('#AlertnotificationId').click(function(){                    
                    QueryResponse();
                });
                $('#labResultId1').click(function(){
                    LabResults();
            
                });

                EmrTemplateIcone();

            });


            //document.addEventListener('DOMContentLoaded', function () {
            //    // your code here
            //    BindPatientDiagnosis();
            //}, false);

            setTimeout(function () {
                BindPatientDiagnosis();           
            }, 1000);

            function BindPatientDiagnosis() {
    
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
                        // $('#tbl_diagnosis').html('');
                        $("#tbl_diagnosis").find("tr:gt(0)").remove();
                        for (var i = 0; i < tableData.length; i++)
                        {
                            if (tableData[i].IsChronic == true || tableData[i].IsChronic)
                            {
                                $('#nocronic').css('display','none');                           
                                sb1.append('<li style="display:inline-block">'+tableData[i].Description+'</li>');
                            }                        
                            sb.append('<tr><td>'+ tableData[i].Description +'</td>');                    
                            sb.append('<td>'+tableData[i].VisitType+'</td>'); 
                            sb.append('<td>'+tableData[i].Date+'</td></tr>'); 
                      
                        }    
                        $('#cronics').append(sb1.toString());
                        str = sb.toString();                    
                        if(str.length)
                        {
                            $("#tbl_diagnosis > tbody:last").children().remove();
                            $('#tbl_diagnosis tbody').append(sb.toString());
                            $('#loder1').css('display', 'none');
                            $('#tbl_diagnosis').css('display', 'block');
                        }
                        else
                        {
                            $('#msg1').css('display','block');
                            $('#loder1').css('display', 'none');
                        }                   
                   
                    
                        $('#loder2').css('display', 'block');
                        setTimeout(function () {
                            BindPatientMedication();
                       
                        }, 1000);
                    }

                })
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
                        }
                        else
                        {
                            $('#loder2').css('display', 'none');
                            $('#msg2').css('display','block');
                        }
                   
                   
                        $('#loder3').css('display', 'block');
                        setTimeout(function () {
                            BindPatientImmunization();
                        }, 1000);
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
                if (age <= 5)
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
                            for (var i = 0; i < tableData.length; i++) {
                                sb.append('<tr><td>'+ tableData[i].ImmunizationName +'</td>');                    
                                sb.append('<td>'+tableData[i].Duedate+'</td>');
                                sb.append('<td>'+tableData[i].GivenDate+'</td></tr>');
                            }
                            str = sb.toString(); 
                    
                            if(str.length)
                            {
                                $("#tbl_imu > tbody:last").children().remove();
                                $('#tbl_imu tbody').append(sb.toString());
                                $('#loder3').css('display', 'none');
                                $('#tbl_imu').css('display', 'block');
                            }
                            else
                            {
                                $('#loder3').css('display', 'none');
                                $('#msg3').css('display','block');
                            }

                   
                            //$('#loder4').css('display', 'block');
                            //setTimeout(function () {
                            //    BindPatientLabOrder();
                            //}, 1000);
                        }

                    })
                }
                else
                {
                    $('#loder3').css('display', 'none');
                    $('#msg3').css('display','block');
                }
                $('#loder4').css('display', 'block');
                setTimeout(function () {
                    BindPatientLabOrder();
                }, 1000);
            }

            function BindPatientLabOrder() {
        
                let sb = new StringBuilder();
                let obj = new Object();
                obj.Registraionid = <%=Session["RegistrationId"]%>;
                obj.HospitalLocationid = <%=Session["HospitalLocationId"]%>;;
                obj.Encounterid= <%=Session["EncounterId"]%>; 
                $.ajax({
                    url: url + "api/EMRAPI/GetPatientOrderProcedure",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (res) {
                        let tableData = res;
                        // $('#tbl_diagnosis').html('');
                        for (var i = 0; i < tableData.length; i++) {
                            sb.append('<tr><td>'+ tableData[i].ServiceName +'</td>');                    
                            sb.append('<td>'+tableData[i].Date+'</td></tr>');
                        }
                        str = sb.toString();   
                        if(str.length)
                        {
                           // alert('kuldeep');
                           // $("#tbl_lab tbody").find("tr:gt(0)").remove();
                            $("#tbl_lab > tbody:last").children().remove();
                            $('#tbl_lab tbody').append(sb.toString());
                            $('#loder4').css('display', 'none');
                            $('#tbl_lab').css('display', 'block');
                        }
                        else
                        {
                            $('#loder4').css('display', 'none');
                            $('#msg4').css('display','block');
                        }
                   
                        $('#loder5').css('display', 'block');
                        setTimeout(function () {
                            BindPatientChifComplaints();
                        }, 1000);
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
                        }
                        else
                        {
                            $('#loder5').css('display', 'none');
                            $('#msg5').css('display','block');
                        
                        }
                        $('#loder6').css('display', 'block');
                        setTimeout(function () {
                            BindPatientPastHistory();
                        }, 1000);
                    
                    }

                })
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
               
                    //alert('Modal Popup');
                    $('#MessageModel').show();
                    $('#btnMsg').trigger('click');
                    $('#home').addClass('active in');
                    $('#chat').removeClass('active in');
             
                    $('#liChat').css('display','none');

                });
                $('#btnCloseMessageModel').on('click',function(){
                    $('#liMessage').addClass('active');
                    $('#MessageModel').hide();
                }); 
            });
            

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

                
                debugger;
                $.ajax({
                    url: url + "api/EMRAPI/GetSingleScreenPatientPastHistory",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (response) {

                        //alert(JSON.stringify(response));
                        let tableData = response;
                        let sb = new StringBuilder();
                        for (var i = 0; i < tableData.length; i++) 
                        {
                            let day = tableData[i].EncDay.toString();
                            let month = tableData[i].EncMonth;
                            
                            if(i==0)
                            {
                                sb.append('<li class="active"><a data-toggle="tab" href="#home'+ i +'">'+ tableData[i].EncDay+'<span style="display: block;font-weight: bold;">'+ tableData[i].EncMonth+'</span>  </a></li>')
                                localStorage.setItem("vDay", tableData[i].EncDay.toString());
                                localStorage.setItem("vMonth", tableData[i].EncMonth);
                            }
                            else
                            {
                                
                                if(day == localStorage.getItem("vDay") && month== localStorage.getItem("vMonth"))
                                {
                                    // sb.append('<li><a data-toggle="tab" href="#home'+ i +'"> '+ tableData[i].EncDay+'<span style="display: block;font-weight: bold;">'+ tableData[i].EncMonth+'</span> </a></li>')
                                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
                                    localStorage.setItem("vMonth", tableData[i].EncMonth);
                                } 
                                else
                                {
                                    sb.append('<li><a data-toggle="tab" href="#home'+ i +'"> '+ tableData[i].EncDay+'<span style="display: block;font-weight: bold;">'+ tableData[i].EncMonth+'</span> </a></li>')
                                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
                                    localStorage.setItem("vMonth", tableData[i].EncMonth);
                                }
                               
                                
                            }
                       
                        }
                        $("#content-slider").empty();

                        $('#content-slider').append(sb.toString());

                        let sb1 = new StringBuilder();
                        var OPIP;

                        for (var i = 0; i < tableData.length; i++) {
                            
                            let day = tableData[i].EncDay.toString();
                            let month = tableData[i].EncMonth;

                            if(i==0)
                            {
                                if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}

                                sb1.append('<div id="home'+ i +'"  class="tab-pane fade in active">')
                                sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
                                sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
                                sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
                                sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
                                sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i]) +")'><i class='fa fa-file-alt'></i></a></li>");
                                sb1.append('</ul>');
                                localStorage.setItem("vDay", tableData[i].EncDay.toString());
                                localStorage.setItem("vMonth", tableData[i].EncMonth);
                                localStorage.setItem ("Index",i);

                            }
                            else
                            {
                                if(day == localStorage.getItem("vDay") && month== localStorage.getItem("vMonth"))
                                {
                                    
                                    if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}

                                    // sb1.append('<div id="home'+ localStorage.getItem ("Index") +'"  class="tab-pane fade in active">')
                                    sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
                                    sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
                                    sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
                                    sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
                                    sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i]) +")'><i class='fa fa-file-alt'></i></a></li>");
                                    sb1.append('</ul>');
                                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
                                    localStorage.setItem("vMonth", tableData[i].EncMonth);
                                    localStorage.setItem ("Index",localStorage.getItem ("Index",i));
                                }
                                else
                                {
                                    sb1.append('</div>');
                                    
                                    if(tableData[i].OPIP == 'O'){OPIP='OP'}else if(tableData[i].OPIP == 'O'){OPIP='IP'}else{OPIP = tableData[i].OPIP}                   

                                    sb1.append('<div id="home'+ i +'"  class="tab-pane fade">')
                                    sb1.append("<ul class='list-inline' style='margin-left: 10px;'><li> <span style='display: block;font-weight: bold;'>ENC No</span>"+ tableData[i].EncounterNo + "</li>");
                                    sb1.append("<li><span style='display: block;font-weight: bold;'>Doctor Name</span>"+ tableData[i].DoctorName + "</li>");
                                    sb1.append("<li> <span style='display: block;font-weight: bold;'>Visit</span>"+ OPIP + "</li>");
                                    sb1.append("<li><span style='display: block;font-weight: bold;'>Diagnosis</span>"+ tableData[i].Diagnosis + "</li>");
                                    sb1.append("<li><span style='display: block;font-weight: bold;'>Case Sheet</span><a onclick='caseSheet(this,"+ JSON.stringify(tableData[i])+")'><i class='fa fa-file-alt'></i></a></li>");
                                    sb1.append('</ul>');
                                    localStorage.setItem("vDay", tableData[i].EncDay.toString());
                                    localStorage.setItem("vMonth", tableData[i].EncMonth);
                                }
                            }
                        }
                        $("#tabPasthistory").empty();
                        $('#tabPasthistory').append(sb1.toString());                  
                    
                    },
                    error: function (response) {
                        console.log(response);
                    }

                });


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
                // PatientChatdetail();
                MessageNotification();
            }, 2000);  
            function MessageNotification() {
    
                let obj = new Object();
                obj.DoctorId = '<%=Session["EmployeeId"]%>';
                obj.RegistrationNo  =0
                obj.EncounterId = 0
                obj.mchar = 'N'
                $.ajax({
                    url: ApiUrl + "api/Chat/GetNotification",
                    type: 'post',
                    dataType: 'json',
                    data: obj,
                    success: function (res) {

                        // alert(JSON.stringify(res));
                        let tableData = res;
                        let sb = new StringBuilder();

                        $("#tblMessage").find("tr:gt(0)").remove();
                        if (tableData.length) {
                            for (var i = 0; i < tableData.length; i++) {
                                if(tableData[i].unread == 0)
                                {
                                    sb.append('<tr style="font-weight: bold"><td>'+ tableData[i].RegistrationNo +'</td><td>'+ tableData[i].EncounterId +'</td><td>'+ tableData[i].PatientName +'</td><td>'+ tableData[i].VisitType +'</td><td>'+ tableData[i].MessageText +'</td>')
                                    sb.append("<td><a data-toggle='pill' href='#chat' onclick='clickMe(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-reply'></i> </a></td></tr>")
                        
                    
                                }
                                else
                                {
                                    sb.append('<tr class="read"><td>'+ tableData[i].RegistrationNo +'</td><td>'+ tableData[i].EncounterId +'</td><td>'+ tableData[i].PatientName +'</td><td>'+ tableData[i].VisitType +'</td><td>'+ tableData[i].MessageText +'</td>')
                                    sb.append("<td><a data-toggle='pill' href='#chat' onclick='clickMe(this," + JSON.stringify(tableData[i]) + ")' class='btn btn-primary'><i class='fa fa-reply'></i> </a></td></tr>")
                        
                       
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
                        else {

                       
                            sb.append("<tr><td'> No Record Found</td></tr>");
                        
                        }

                        $('#tblMessage').append(sb.toString());
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
            // alert(JSON.stringify(res));
            if(xhr.status=200)
            {
                
                if (IconVisible(res,'VTL')) // 1
                {
                    $('#IcVitals').css('display','inline');
                }
                if (IconVisible(res,'COM')) // 2
                {
                    $('#IclChiefComplaints').css('display','inline'); 
                }
                if (IconVisible(res,'HIS')) // 3
                {
                    $('#IcHisPIllness').css('display','inline');

                }
                if (IconVisible(res,'PH')) // 4
                {
                    $('#IcPastHistory').css('display','inline');
                }
                if (IconVisible(res,'EXM')) // 5
                {
                    $('#IcExamination').css('display','inline');
                }
                if (IconVisible(res,'PDG'))
                {
                    $('#IcProvisionalDiagnosis').css('display','inline');
                }
                if (IconVisible(res,'DGN')) // 6
                {
                    $('#IcDiagnosis').css('display','inline');
                }
                if (IconVisible(res,'ORD')) // 7
                {
                    $('#IcOrdersandProcedures').css('display','inline');
                
                }
                if (IconVisible(res,'PRS')) // 8
                {
                    $('#IcPrescriptions').css('display','inline');
                }
                if (IconVisible(res,'NDO')) // 9
                {
                    $('#IcOtherOrder').css('display','inline');
                }
                
                if (IconVisible(res,'ATM')) // 10
                {
                    $('#IcVitals').css('display','inline');
                }
                if (IconVisible(res,'PFE')) // 11
                {
                    $('#IcPatientandfamilyeducationandcounselling').css('display','inline');
                }
                if (IconVisible(res,'RRR')) // 12
                {
                    $('#IcReferralsReplytoreferrals').css('display','inline');
                }
                if (IconVisible(res,'MEP')) // 13
                {
                    $('#IcMultidisciplinaryEvaluationAndPlanOfCare').css('display','inline');
                }  
                if (IconVisible(res,'poc')) // 14
                {
                    $('#IcPlanOfCare').css('display','inline');
                }  
                if (IconVisible(res,'OTN')) // 15
                {
                    $('#IcCareTemplate').css('display','inline');
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
            function PastClinicalNotes()
            {
                $get('<%=lbtnPastClinicalNotes.ClientID%>').click();
            }            

            function caseSheet(controler,Data)
            {         
              //  var oWnd = $find("<%=RadWindowForNew.ClientID%>");   
                var url = '/Editor/WordProcessor.aspx?From=POPUP&DoctorId="<%=Session["DoctorID"]%>"&OPIP="<%=Session["OPIP"]%>"&EncounterDate='+Data.EncDate+'&EncounterType='+Data.OPIP+'';
                OpenPopup(url);            
            }


            function Diagnosis()
            {  

                // $('#<%=imgBtnFinalDiagnosis.ClientID%>').trigger('click');
                var url='/EMR/Assessment/DiagnosisHistory.aspx?From=POPUP';
                OpenPopup(url);
   
            }
            function OpenPopup(url)
            {
                var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                var url = url;              
                oWnd.setUrl(url);
                oWnd.setSize(1300,100);           
                oWnd.show();
            }

            function ChiefComplaints()
            {  

                // $('#<%=imgBtnAddChiefComplaints.ClientID%>').trigger('click');    
                var url=' /EMR/Problems/ViewPastPatientProblems.aspx?MP=NO';
                OpenPopup(url);
            }


            function QueryResponse()
            {    
                //var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                var url = '/Approval/QueryResponse.aspx';
                OpenPopup(url);  
            }

            function LabResults()
            {
                // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                var url = '/EMR/Dashboard/ProviderParts/LabResults.aspx?From=POPUP&mainRegNo=<%=Session["RegistrationNo"]%>';
                OpenPopup(url);
            }
            function GrowthChart()
            {
   
                var url = '/EMR/Vitals/GrowthChart.aspx?MP=NO';
                OpenPopup(url);    
           
            }

            function Immunization()
            {
                // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                var url = '/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP';
                OpenPopup(url);   
           
            }       
     
            function Allergies()
            {
                $('#<%=imgBtnAddAllergies.ClientID%>').trigger('click'); 
            }

            function Prescriptions()
            {
                var url = '/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=False';
                OpenPopup(url);
                
               // $('#<%=imgBtnAddPrescriptions.ClientID%>').trigger('click'); 
            }

            function RIS()
            {
               // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                var url = '/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=<%= Session["EncounterId"]%>&RegNo=<%=Session["RegistrationNo"]%>&Source=O&Flag=RIS&Station=All';
                OpenPopup(url);  
            }
            function LIS()
            {
               // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                var url = '/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=<%= Session["EncounterId"]%>&RegNo=<%=Session["RegistrationNo"]%>&Source=O&Flag=LIS&Station=All';        
                OpenPopup(url); 
            }
            function Attachment()
            {           
               // var oWnd = $find("<%=RadWindowForNew.ClientID%>");
                //var url = '/EMR/AttachDocument.aspx?MASTER=No';
                var url = '/EMR/AttachDocumentFTP.aspx?MASTER=No';
                OpenPopup(url);
            }


            function LabOrders()
            {         
                 var url = '/EMRBilling/Popup/Servicedetails.aspx?Deptid=0&EncId=<%= Session["EncounterId"]%>&RegNo=<%=Session["RegistrationNo"]%>&sBillId=0&PType=WD&rwndrnd=0.5007572308194614'
                OpenPopup(url);
                //$('#<%=imgbtnAddOrdersAndProcedures.ClientID%>').trigger('click'); 
          
            }
            function PastHistory()
            {
                $('#<%=lnkPastHistory.ClientID%>').trigger('click');
            }



            $('#btn-expand-collapse').click(function(e) {
                $('.emr-main-container').toggleClass('collapsed');
                $('.RadWindow_Metro').toggleClass('collapsed');

            });

            function Message(msg)
            {
                if(msg !='')
                {
                    $('#msgModal').show();
                    $('#pMsg').html(msg);
                }
        
            }
            function messageclose()
            {
                $('#msgModal').hide();
                $('#pMsg').html('');
            }
            $(".round").click(function(){
                $(".accord-inner").toggleClass("main");
                $(".free-text").toggleClass("main");

            });

            $('#btn-nav-previous').click(function(){
                $(".menu-inner-box").animate({scrollLeft: "-=100px"});
            });
        
            $('#btn-nav-next').click(function(){
                $(".menu-inner-box").animate({scrollLeft: "+=100px"});
            });

 </script>
    
      

</asp:Content>

