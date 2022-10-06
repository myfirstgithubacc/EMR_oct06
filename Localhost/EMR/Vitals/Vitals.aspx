<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMasterWithTopDetails.master"
    MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Vitals.aspx.cs"
    Inherits="EMR_Vitals_Vitals" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register TagPrefix="aspl" TagName="Left" Src="~/Include/Components/ucTree.ascx" %>--%>
<%--<%@ Register TagPrefix="aspl" TagName="Top" Src="~/Include/Components/MasterComponent/TopPanel.ascx" %>
<%@ Register TagPrefix="asp1" TagName="Top1" Src="~/Include/Components/TopPanel.ascx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />--%>
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />



    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript" language="javascript">

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

            function show(ctrlpnl) {
                var pnl = document.getElementById(ctrlpnl);
                pnl.style.visibility = 'visible';
            }
            function ValidateDate(CtrlSDate) {

                var SDate = CtrlSDate;
                var now = new Date();

                var alertReason1 = 'Date must be less than or equal to current Date.'
                var startDate = new Date(SDate);
                alert(CtrlSDate);
                alert(now.format("dd/MM/yyyy HH:mm"));

                if (CtrlSDate > now.format("dd/MM/yyyy HH:mm")) {
                    alert(alertReason1);

                    return false;
                }
                return false;
            }
            function ShowCancelDetail(ctrlpnl) {

                var pnl = document.getElementById(ctrlpnl);
                pnl.style.visibility = 'visible';

                pnl.style.top = 450;
                pnl.style.left = 10;
            }
            function ShowCancelPanel(ctrlpnl) {
                var pnl = document.getElementById(ctrlpnl);
                pnl.style.visibility = 'visible';
            }
            function HideCancelPnl(ctrlPnl) {

                var pnl = document.getElementById(ctrlPnl);
                pnl.style.visibility = 'hidden';

            }

            function showvalue(ctrlname, txtvalue, ctrlLength) {

                var tt = document.getElementById(ctrlname);
                //                        alert(tt.value.length);
                //                        alert(ctrlLength);
                if (tt.value.length < ctrlLength) {
                    tt.value = tt.value + txtvalue;
                }
                return true;
            }
            function HidePane(ctrlname) {
                //alert(ctrlname);
                var tt = document.getElementById(ctrlname);
                tt.style.visibility = 'hidden';
            }

            <%--window.onbeforeunload = function (evt) {
                var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
                if (IsUnsave == 1) {
                    return false;
                }
            }--%>
            function HideUnitPane(ctrlPanel, ctrlRadioButton, ctrlTextbox, ctrltxtUnitId) {
                var tt = document.getElementById(ctrlPanel);
                tt.style.visibility = 'hidden';
                var tableElement = document.getElementById(ctrlRadioButton);
                var txtunit = document.getElementById(ctrlTextbox);
                var txtUnitId = document.getElementById(ctrltxtUnitId);
                //alert(tableElement.rows.length);
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    //alert(col.checked);
                    if (col.checked == true) {
                        var queryString = new String();
                        queryString = col.value;
                        var arr1 = new Array();
                        arr1 = queryString.split("|");
                        //alert(arr1[0].toString());
                        txtunit.value = arr1[1].toString();
                        txtUnitId.value = arr1[0].toString();
                    }
                }

            }
            function ShowPane(ctrlPanel, ctrlRadioButton, ctrltxtUnitId) {
                var tt = document.getElementById(ctrlPanel);
                tt.style.visibility = 'visible';
            }
            function ShowRadioPane(ctrlPanel, ctrlRadioButton, ctrltxtUnitId) {
                var tt = document.getElementById(ctrlPanel);
                var tableElement = document.getElementById(ctrlRadioButton);
                var txtUnitId = document.getElementById(ctrltxtUnitId);
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var queryString = new String();
                    queryString = col.value;
                    var arr1 = new Array();
                    arr1 = queryString.split("|");

                    if (arr1[0].toString() == txtUnitId.value) {
                        col.checked = true;
                    }
                }
                tt.style.visibility = 'visible';
            }
            function erasevalue(ctrlname) {
                //alert(ctrlname);
                var tt = document.getElementById(ctrlname);
                tt.value = tt.value.substring(0, tt.value.length - 1);
                //tt.style.visibility = 'visible';
            }
            function CalculateBMI(ctrlFindText) {
                var txtunit = document.getElementById(ctrlFindText);
                $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;

                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    document.getElementById('ctl00_ContentPlaceHolder1_btnCalculate').click();
                    nSat = 1;
                }
                if (txtunit.value != "") {
                    nSat = 1;
                }


            }

            function CalculateMAP(ctrlFindText) {
                var txtunit = document.getElementById(ctrlFindText);
                $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;

                   if ((txtunit.value != "") && (txtunit.value != "0")) {
                       document.getElementById('ctl00_ContentPlaceHolder1_btnCalculate').click();
                       nSat = 1;
                   }
                   if (txtunit.value != "") {
                       nSat = 1;
                   }


               }
            //--------- Copy Last Vital Value ----------------------

            function Copylastvitalvalue() {

                var gridview = document.getElementById('<%=gvvitals.ClientID %>');
                var length = gridview.rows.length;
                var rowidx = '';

                $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
                for (i = 2; i < length + 1; i++) {
                    if (i < 10) {
                        rowidx = '0' + i.toString();
                    }
                    else {
                        rowidx = i.toString();
                    }

                    txtfindings1 = $get('ctl00_ContentPlaceHolder1_gvvitals_ctl' + rowidx.toString() + '_txtfindings1');
                    hdnlValues = $get('ctl00_ContentPlaceHolder1_gvvitals_ctl' + rowidx.toString() + '_hdnlValues');
                    hdnDisplayName = $get('ctl00_ContentPlaceHolder1_gvvitals_ctl' + rowidx.toString() + '_hdnDisplayName');
                    dtpdate = $get('ctl00_ContentPlaceHolder1_gvvitals_ctl' + rowidx.toString() + '_dtpdate_dateInput');

                    var valuetext = hdnlValues.value;

                    if (hdnDisplayName.value == "HT" && hdnlValues.value != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "WT" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "HC" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "T" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "R" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "P" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "BPS" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "BPD" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "MAC" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value.toUpperCase() == "PAIN" && parseInt(hdnlValues.value) != "") {
                        //txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value.toUpperCase() == "SPO2" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "FBS" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "RBS" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "SG" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "PH" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "LEU" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "NIT" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "PRO" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "GLU" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "KET" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "UBG" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "BIL" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "ERY" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "HB" && parseInt(hdnlValues.value) != "") {
                        txtfindings1.value = valuetext;
                    }
                    else if (hdnDisplayName.value == "LMP" && parseInt(hdnlValues.value) != "") {
                      //  dtpdate.value = valuetext;
                        //alert(valuetext);
                        //dtpdate. = valuetext;
                      //  dtpdate.value = valuetext;
                      //  dtpdate.innerText = '15/05/2019';
                       // alert(dtpdate.value);
                        //var LMPDate = new Date(valuetext);
                        //dtpdate.value =
                      //  dtpdate.value(valuetext);
                    }

                }
                document.getElementById('ctl00_ContentPlaceHolder1_btnCalculate').click();
                $get('<%=btnCopyDDL.ClientID%>').click();
            }

            //-------------- END Last Value Copy -----------------------


            //// Alert Msg fore color change --------------------------
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
                else if (DisplayName.value == "R") {
                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }

                }
                else if (DisplayName.value == "P") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "BPS") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "BPD") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }

                else if (DisplayName.value == "MAC") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "Pain") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "SPO2") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "SG") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "PH") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";
                        }
                    }
                }
                else if (DisplayName.value == "LEU") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "NIT") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "PRO") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "GLU") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "KET") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "UGB") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "BIL") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "ERY") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "HB") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }
                else if (DisplayName.value == "FBS") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }

                else if (DisplayName.value == "RBS") {

                    if (txtunit.value != "") {
                        if (parseInt(mnvalue.value) > parseInt(txtunit.value) || parseInt(mxvalue.value) < parseInt(txtunit.value)) {

                            txtunit.style.color = "Red";
                        }
                        else {
                            txtunit.style.color = "Black";

                        }
                    }
                }


                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    document.getElementById('ctl00_ContentPlaceHolder1_btntemprature').click();
                    nSat = 1;
                }
                if (txtunit.value != "") {
                    nSat = 1;
                }
            }

            /// END Alert

            function CalculatRespiration(ctrlFindText) {
                var txtunit = document.getElementById(ctrlFindText);
                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    document.getElementById('ctl00_ContentPlaceHolder1_btnRespiration').click();
                    nSat = 1;
                }
                if (txtunit.value != "")
                    nSat = 1;
            }
            function CalculatPulse(ctrlFindText) {
                var txtunit = document.getElementById(ctrlFindText);
                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    document.getElementById('ctl00_ContentPlaceHolder1_btnpulse').click();
                    nSat = 1;
                }
                if (txtunit.value != "")
                    nSat = 1;
            }
            function CalculatBPSBPD(ctrlFindText) {
                var txtunit = document.getElementById(ctrlFindText);
                if ((txtunit.value != "") && (txtunit.value != "0")) {
                    document.getElementById('ctl00_ContentPlaceHolder1_btnBPSBPD').click();
                    nSat = 1;
                }
                if (txtunit.value != "")
                    nSat = 1;
            }
            // Call Graph

            function setValue(val, valName, IsInvest, DisplayInGraph) {
                $get('<%=hdnVitalvalue.ClientID%>').value = val;
                $get('<%=hdnVitalName.ClientID%>').value = valName;
                if (IsInvest == 0 && DisplayInGraph == 1) {
                    var oWnd = radopen("/EMR/Vitals/Vitalgraph.aspx?Value=" + $get('<%=hdnVitalvalue.ClientID%>').value +
                                        "&Name=" + $get('<%=hdnVitalName.ClientID%>').value, "RadWindowForNew");

                    oWnd.setSize(1000, 650)
                    oWnd.center();
                    oWnd.VisibleStatusbar = "false";
                    oWnd.set_status(""); // would like to remove statusbar, not just blank it
                }
                else {

                }
            }
            //End Graph

            function VitalDetailsClose(oWnd) {
                $get('<%=btnfind.ClientID%>').click();
            }

            function OnClientVitalDetailsClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var VitalDetailsId = arg.VitalDetailsId;
                    var VitalDate = arg.VitalDate;

                    $get('<%=hdnVitalDetailsId.ClientID%>').value = VitalDetailsId;
                    $get('<%=hdnVitalDate.ClientID%>').value = VitalDate;
                }
                $get('<%=btnfind.ClientID%>').click();
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
                    evt = window.event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {
                    case 114:  // F3
                        $get('<%=btnsave.ClientID%>').click();
                    break;

            }
            evt.returnValue = false;
            return false;
        }


        </script>

        <style type="text/css">
            .blink {
                text-decoration: blink;
            }

            .blinkNone {
                text-decoration: none;
            }
        </style>



    </telerik:RadCodeBlock>
    <div>

        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">

                        <div class="col-md-6 text-center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-6 text-right">
                            <asp:LinkButton ID="lnktriageform" runat="server" Text="Triage form" OnClick="lnktriageform_Click" Visible="false"></asp:LinkButton>

                            <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" CssClass="btn btn-primary"
                                Text="New" OnClick="btnNew_OnClick" />

                            <asp:Button ID="btnvitaldetal" runat="server" Text="Edit / Cancel" CssClass="btn btn-primary" ToolTip="Patient all Vitals Detail"
                                OnClick="btnvitaldetal_Click" />

                            <asp:Button ID="btnHistory" runat="server" Text="History" ToolTip="Vital History"
                                OnClick="btnHistory_Click" CssClass="btn btn-primary" />

                            <asp:Button ID="btnsave" runat="server" Text="Save (F3)" ValidationGroup="Save" ToolTip="Save"
                                OnClick="btnsave_Click" CssClass="btn btn-primary" Font-Bold="false" />

                            <asp:Button ID="btnClose" CssClass="btn btn-primary" Visible="false" runat="server" Text="Close"
                                OnClientClick="window.close();" />

                            <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                            <telerik:RadWindowManager ID="RadWindowManager1" Skin="Metro" EnableViewState="false" runat="server" Height="150px">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close, Move" Height="50px" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnsave" />
                <asp:AsyncPostBackTrigger ControlID="gvvitals" />
                <asp:AsyncPostBackTrigger ControlID="btnfind" />
                <asp:AsyncPostBackTrigger ControlID="lnkcalculator" />
                <asp:AsyncPostBackTrigger ControlID="lnkAlerts" />
                <asp:AsyncPostBackTrigger ControlID="btnCopyDDL" />
            </Triggers>
        </asp:UpdatePanel>
        <%--  <div class="container-fluid text-center bg-warning form-group">
            <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
        </div>--%>


        <asp:Panel ID="pnlLock" runat="server" Width="100%" CssClass="contentscroll-auto">
                <div id="trCurrentVitalTemplate" runat="server">

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-8  col-12">
                            <div class="row">
                                <div class="col-md-3 col-sm-3 col-3">
                                    <strong>
                                        <asp:Literal ID="ltrlCur" runat="server" Text="Current Vitals"></asp:Literal></strong>
                                </div>
                                <div class="col-md-9 col-sm-9 col-9">
                                    <asp:UpdatePanel ID="updtPnlDt" runat="server">
                                        <ContentTemplate>

                                            <div class="row">
                                                <div class="col-md-2 col-2">
                                                    <asp:DropDownList ID="ddlTemplate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged"
                                                        SkinID="DropDown" Visible="false">
                                                    </asp:DropDownList>
                                                    Date <span style="color: Red">*</span>
                                                </div>

                                                <div class="col-md-6 col-sm-4 col-6">
                                                    <telerik:RadDateTimePicker ID="RadDateTimePicker1" runat="server" AutoPostBackControl="Both"
                                                        TabIndex="37" Width="100%" DateInput-ReadOnly="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />

                                                    <asp:RequiredFieldValidator ID="rfvtxtDate" runat="server" ControlToValidate="RadDateTimePicker1"
                                                        Display="None" ErrorMessage="Please fill vitals date !" SetFocusOnError="true"
                                                        ValidationGroup="Save" />
                                                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                                                        ShowSummary="False" ValidationGroup="Save" />
                                                </div>
                                                <div class="col-md-4 col-sm-3 col-4">
                                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="300px"
                                                        OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Width="50%">
                                                    </telerik:RadComboBox>
                                                    <asp:Literal ID="ltDateTime" runat="server" Text="HH   MM"></asp:Literal>&nbsp;
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

                        <div class="col-md-4 col-12 p-t-b-5 text-right">
                            <asp:UpdatePanel ID="updatecalc" runat="server" style="display: inline-flex;">
                                <ContentTemplate>

                                        <asp:LinkButton ID="lnkcalculator" runat="server" Text="Calculator" OnClick="lnkcalculator_Click"
                                            Font-Underline="false" Font-Bold="false" CssClass="btn btn-info"></asp:LinkButton>

                                        <asp:LinkButton ID="lnkAlerts" runat="server" Text="Patient Alert" OnClick="lnkAlerts_OnClick"
                                            Visible="false" CssClass="btn btn-info" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                 <asp:Button ID="btnFetch" runat="server" Text="Fetch" CssClass="btn btn-info" OnClick="btnFetch_Click" />
                                <input type="button" value="Copy Last Vital Value" id="btncopydata" runat="server" class="btn btn-info"
                                    onclick="return Copylastvitalvalue();"/>
                                        

                                
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-8 col-sm-8 col-xs-12 gridview table-responsive">
                                <asp:GridView ID="gvvitals" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                                    OnRowDataBound="gvvitals_RowDataBound" Width="100%" CssClass="vitals-section ">
                                    <Columns>
                                        <asp:BoundField DataField="SlNo" HeaderText="S&nbsp;No" Visible="false" />
                                        <asp:BoundField DataField="Vital" HeaderText="Vital" ItemStyle-Width="110px" />
                                        <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnvitalid" runat="server" Value='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Vitalid")) %>' />
                                                <asp:TextBox ID="txtfindings1" runat="server"
                                                    MaxLength='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"MaxLength")) %>'
                                                    Text='<%# Eval("DefaultValue") %>'
                                                    SkinID="textbox" Width="80px" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                    FilterType="Custom, Numbers" TargetControlID="txtfindings1" ValidChars="." />
                                                <%-- <asp:DropDownList ID="ddlFaceScanDrop" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFaceScanDrop_OnSelectedIndexChanged"
                                                                    Visible="false" Width="185px">
                                                                    <asp:ListItem Text="Select" Value="0" />
                                                                </asp:DropDownList>--%>
                                                <telerik:RadComboBox ID="ddlFaceScanDrop" runat="server" AllowCustomText="true" MaxHeight="297px"
                                                    Width="200px" Skin="Default" AutoPostBack="False" HighlightTemplatedItems="True"
                                                    Visible="false">
                                                </telerik:RadComboBox>
                                                <asp:DropDownList ID="ddlPainScanDrop" runat="server" Visible="false" Width="185px">
                                                    <asp:ListItem Text="Select" Value="0" />
                                                </asp:DropDownList>
                                                <telerik:RadDatePicker ID="dtpdate" Width="100px" runat="server" Visible="false" DateInput-DateFormat="dd/MM/yyyy" DateInput-DateDisplayFormat="dd/MM/yyyy" MinDate="01/01/1900" />

                                                <asp:HiddenField ID="hdnAbNormalmin" runat="server" Value='<%# Eval("MinValue") %>' />
                                                <asp:HiddenField ID="hdnAbNormalmax" runat="server" Value='<%# Eval("MaxValue") %>' />
                                                <asp:HiddenField ID="hdnDisplayName" runat="server" Value='<%# Eval("DisplayName")%>' />
                                                <asp:HiddenField ID="hdnTemplateData" runat="server" Value='<%# Eval("TemplateData")%>' />
                                                <asp:HiddenField ID="hdnIsMandatory" runat="server" Value='<%# Eval("IsMandatory")%>' />
                                                <asp:HiddenField ID="hdnValueID" runat="server" Value='<%# Eval("ValueID")%>' />
                                                <cc1:FilteredTextBoxExtender ID="txtfindingsFilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                    TargetControlID="txtfindings1" ValidChars=".">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:Label ID="txtunit1" runat="server" BorderStyle="None" ReadOnly="true" SkinID="textbox"
                                                    Text='<%# Eval("Unit1") %>' MaxLength="15"></asp:Label>
                                                <input id="hdtxtUnitId1" runat="server" type="hidden" value='<%# Eval("unitid1") %>' />
                                                <asp:Panel ID="pnlUnit1" runat="server" BackColor="#E0EBFD" BorderStyle="Solid" BorderWidth="1px"
                                                    Style="visibility: hidden; position: fixed;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <strong>Unit</strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButtonList ID="rdUnit1" runat="server">
                                                                    <asp:ListItem Text="cm" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Inches" Value="2"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <img id="imgUnitOk" runat="server" alt="Ok" src="/Images/Ok.jpg" style="cursor: pointer;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:TextBox ID="txtfindings2" runat="server" MaxLength='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"MaxLength")) %>'
                                                    SkinID="textbox" Width="50px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    FilterType="Custom, LowercaseLetters,UppercaseLetters,Numbers" TargetControlID="txtfindings2"
                                                    ValidChars=".">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtunit2" runat="server" BorderStyle="None" Columns="1" ReadOnly="true"
                                                    SkinID="textbox" Text='<%# Eval("Unit2") %>'></asp:TextBox>
                                                <input id="hdtxtUnitId2" runat="server" type="hidden" value='<%# Eval("unitid2") %>' />
                                                <asp:Panel ID="pnlUnit2" runat="server" BackColor="#E0EBFD" BorderStyle="Solid" BorderWidth="1px"
                                                    Style="visibility: hidden; position: fixed;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <strong>Unit</strong>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButtonList ID="rdUnit2" runat="server">
                                                                    <asp:ListItem Text="cm" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Inches" Value="2"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <img id="imgUnitOk2" runat="server" alt="Ok" src="/Images/Ok.jpg" style="cursor: pointer;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qualifires">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlqualifires" runat="server">
                                                    <asp:ListItem Text="[Select]" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="updtChk" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkBillable" runat="server" Checked="true" AutoPostBack="true"
                                                            OnCheckedChanged="chkBillable_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LastValue" HeaderText="" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnlValues" runat="server" Value='<%# Eval("LastValues") %>' />
                                                <asp:TextBox ID="txtremarks" runat="server" MaxLength="30" SkinID="textbox" Width="120px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="vitalid" HeaderText="" />
                                        <asp:TemplateField HeaderText="unitid">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitId1" runat="server" Text='<%# Eval("unitid1") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DisplayName" HeaderText="" />
                                        <asp:TemplateField HeaderText="unitid2">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitId2" runat="server" Text='<%# Eval("unitid2") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VitalType" HeaderText="VitalType" ItemStyle-Width="110px" />
                                        <asp:BoundField DataField="AbNormal" HeaderText="AbNormal" />
                                        <asp:TemplateField HeaderText="Reference value" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCategory" runat="server" Text='<%# Eval("Category") %>  '></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="txtminvalue" runat="server" Text='<%# Eval("MinValue") %>  '></asp:Label>
                                                <asp:Label ID="txtmaxvalue" runat="server" Text='<%# Eval("MaxValue") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnIsInvestigation" runat="server" Value='<%#Eval("IsInvestigation") %>' />
                                                <asp:HiddenField ID="hdnDisplayInGraph" runat="server" Value='<%#Eval("DisplayInGraph") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                                <asp:Button ID="btnCopyDDL" runat="server" Text="copy" Style="visibility: hidden;"
                                    OnClick="btnCopyDDL_OnClick" />
                                <asp:UpdatePanel ID="Updapanelcalculate" runat="server">
                                    <ContentTemplate>
                                       <%-- <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Style="visibility: hidden;"
                                            Text="" Width="1px" />--%>
                                        <asp:Button ID="btntemprature" runat="server" OnClick="btntemprature_Click" Style="visibility: hidden;"
                                            Text="" Width="1px" />
                                    </ContentTemplate>
                                    <Triggers>
                                       <%-- <asp:AsyncPostBackTrigger ControlID="btnCalculate" />--%>
                                        <asp:AsyncPostBackTrigger ControlID="btntemprature" />
                                        <asp:AsyncPostBackTrigger ControlID="gvvitals" />
                                        <asp:AsyncPostBackTrigger ControlID="btnfind" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCopyDDL" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                    <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Style="visibility: hidden;"
                                            Text="" Width="1px" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12 gridview">
                                 <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvAutoCalculation" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                            ForeColor="#333333" HeaderStyle-HorizontalAlign="Left" Width="100%" OnRowDataBound="gvAutoCalculation_RowDataBound"
                                            SkinID="gridview">
                                            <Columns>
                                                <asp:BoundField DataField="DisplayName" HeaderText="Vital" />
                                                <asp:BoundField HeaderText="Value" DataField="Value" />
                                                <asp:BoundField HeaderText="Category" DataField="Category" />
                                                <asp:TemplateField HeaderText="Chart">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnchart" runat="server" OnClick="btnchart_Click" CssClass="btn btn-primary"
                                                            Text="Chart" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="vitalid" HeaderText="" />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvvitals" />
                                       <%-- <asp:AsyncPostBackTrigger ControlID="btnCalculate" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:LinkButton ID="lnkGRBS" runat="server" Text="GRBS Monitor" OnClick="lnkGRBS_Click" Visible="false"></asp:LinkButton>


                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Style="margin-top: 10px;">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>

                                            <%--yogesh 7/05/2022--%>

                                            <asp:GridView ID="gvchart" runat="server" AutoGenerateColumns="True" CellPadding="1"
                                                ForeColor="#333333" HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" SkinID="gridview" Width="100%">
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                                <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                                <asp:HiddenField ID="hdnVitalName" runat="server" />
                                <asp:HiddenField ID="hdnVitalDetailsId" runat="server" />
                                <asp:HiddenField ID="hdnVitalDate" runat="server" />
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 15px;">

                      
                            <div class="col-md-9 col-xs-12 form-group">
                                <div class="row">
                                
                                    </div>
                            </div>

                            <div class="col-md-3 col-xs-12">
                                <div class="row">
                               
                                    </div>
                            </div>
                        </div>
                    </div>



                    <div class="col-md-12" id="trMewsDetails" runat="server" visible="false">
                        <div class="subheading_main">
                            <div class=" col-md-12">
                                <asp:LinkButton ID="lnkmews" runat="server" Text="MEWS" OnClick="lnkmews_Click"></asp:LinkButton>
                                <asp:TextBox ID="txtLastMEWSscore" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <table class="table table-bordered">
                            <tr style="background: #337ab7; color: #fff;">
                                <th>MEWS Score
                                </th>
                                <th>Patients location
                                </th>
                                <th>Observation
                                </th>
                                <th>Report
                                </th>
                            </tr>
                            <tr>
                                <td>0
                                </td>
                                <td>Ward
                                </td>
                                <td>4 Hourly
                                </td>
                                <td>Not Applicable
                                </td>
                            </tr>
                            <tr>
                                <td>1 to 2
                                </td>
                                <td>Ward
                                </td>
                                <td>2 Hourly
                                </td>
                                <td>Nurse Incharge/Head Nurse
                                </td>
                            </tr>
                            <tr>
                                <td>3
                                </td>
                                <td>ICU
                                </td>
                                <td>1 Hourly
                                </td>
                                <td>Nurse Nurse Incharge/Head Nurse, Unit Physician / MRP, Nursing supervisor, CNO
                                </td>
                            </tr>
                            <tr>
                                <td>= 4
                                </td>
                                <td>ICU
                                </td>
                                <td>Close observation with  continuous monitoring and  ½ to 1 Hourly recording
                                </td>
                                <td>Nurse Nurse Incharge/Head Nurse, Unit Physician / MRP, Nursing supervisor, CNO, Medical Director  
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
        </asp:Panel>

        <table border="0" cellpadding="0" cellspacing="2">
            <tr>
                <td>
                    <div id="dvMandatory" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                        <table width="100%" cellspacing="2" cellpadding="0">
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="lblMandatoryMessage" Font-Size="12px" runat="server" Font-Bold="true"
                                        Text="Mandatory Message" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="btnMandatoryOk" CssClass="btn btn-primary" runat="server" Text="Ok" OnClick="btnMandatoryOk_OnClick"
                                        Visible="false" />
                                    &nbsp;
                                    <asp:Button ID="btnMandatoryCancel" CssClass="btn btn-primary" runat="server" Text="Cancel"
                                        OnClick="btnMandatoryCancel_OnClick" />
                                </td>
                                <td align="center"></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>

        <div id="dvTriage" runat="server" visible="false" class="col-md-4 col-sm-12">
        <div class="well" style="margin-top: 10px">
                     <asp:Label ID="Label26" runat="server" Text="Triage" />
        <telerik:RadComboBox ID="ddlTriage" EmptyMessage="--Select--" runat="server" Width="150px" style="margin-left:10px;">
                                            </telerik:RadComboBox>
    </div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:HiddenField ID="hdnMandatoryStar" runat="server" Value="<span runat='server' style='color: Red; font-weight: bold;'>*</span>" />
                </td>
            </tr>
        </table>
    </div>

     <script>
         $(document).ready(function () {

             document.body.style.overflow = "hidden";
         });
    </script>
</asp:Content>
