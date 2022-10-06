<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddSurgeryV1.aspx.cs" Inherits="EMRBILLING_Popup_AddSurgeryV1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Surgery Order</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />


    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            @media screen and (max-device-width : 768px) {
                div#gvSurgery {
                    overflow-x: auto;
                }
            }

            input#chkStat {
                margin-top: -13px !important;
                margin-right: 5px !important;
            }

                input#chkStat ~ label {
                    margin-right: 8px !important;
                }
        </style>
        <script type="text/javascript">

            function fnCheckOne(me) {

                me.checked = true;

                var chkary = document.getElementsByTagName('input');
                for (i = 0; i < chkary.length; i++) {
                    if (chkary[i].type == 'checkbox') {

                        if (chkary[i].id != me.id || hdncheckboxValue.value == me.id)
                            chkary[i].checked = false;
                    }


                }
                if (hdncheckboxValue.value != me.id) {
                    $get('<%=hdncheckboxValue.ClientID%>').value = me.id;
                }
                else {
                    $get('<%=hdncheckboxValue.ClientID%>').value = null;
                }


            }
            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.xmlString = document.getElementById("hdnXmlString").value;
                oArg.xmlSurgery = document.getElementById("hdnXmlSurgery").value;
                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function wndAddService_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlString;
                    var serviceid = arg.ServiceID;
                    $get('<%=hdnSurServiceID.ClientID%>').value = serviceid;
                    $get('<%=hdnXmlSurgoenShare.ClientID%>').value = xmlString;
                }

                $get('<%=btnBindGridWithXml.ClientID%>').click();
            }


            function wndSurgeonComponent_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlString;
                    var serviceid = arg.ServiceID;
                    $get('<%=hdnSurServiceID.ClientID%>').value = serviceid;
                    $get('<%=hdnXmlSurgeryComponent.ClientID%>').value = xmlString;
                }
                $get('<%=btnBindGridWithXmlsurgeon.ClientID%>').click();
            }





            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function CalculateAmount(txtServiceCharge, hdnServiceActualAmount) {
                var DecimalPlaces = document.getElementById('<%= hdnDecimalPlaces.ClientID%>').value;
                var ServiceActualAmount = document.getElementById(hdnServiceActualAmount).value;
                if (document.getElementById('<%= hdnIsAllowToEditSurgeryAmountOnlyForCash.ClientID%>').value == 'Y') {
                    if (parseFloat(document.getElementById(txtServiceCharge).value) < parseFloat(ServiceActualAmount)) {
                        alert('Service Charge cannot be less than Service Actual Amount');
                        document.getElementById(txtServiceCharge).value = parseFloat(ServiceActualAmount).toFixed(DecimalPlaces);
                        return;
                    }
                }
                if (document.getElementById(txtServiceCharge).value <= 0) {
                    alert('Charge should be greater than 0 !');
                    return false;
                }
                var DecimalPlaces = document.getElementById('<%= hdnDecimalPlaces.ClientID%>').value;
                var grid = $find("<%=gvAddedSurgery.ClientID %>");
                var mainSurgeryCharge = 0;
                var AssistantSurgeon = 0;
                var ASChargePercentage = 0;
                var serviceid = 0; //New
                var ServiceAmountNew = 0; //New
                if (grid) {
                    var MasterTable = grid.get_masterTableView();
                    var Rows = MasterTable.get_dataItems();

                    for (var i = 0; i < Rows.length; i++) {

                        var rowElem = MasterTable.get_dataItems()[i];
                        var hdnIsMainSurgery = rowElem.findElement("hdnIsMainSurgery");
                        var hdnIsPriceEditable = rowElem.findElement("hdnIsPriceEditable");
                        var hdnSurgeonType = rowElem.findElement("hdnSurgeonType");
                        var hdnIsSurgeryService = rowElem.findElement("hdnIsSurgeryService");
                        var hdnServiceDiscountPerc = rowElem.findElement("hdnServiceDiscountPerc");
                        var hdnIsChargeDependent = rowElem.findElement("hdnIsChargeDependent");  //New
                        var hdnServiceId = rowElem.findElement("hdnServiceId");  //New

                        var hdnServiceType = rowElem.findElement("hdnServiceType");
                        var hdnOtChargeCalculationFlag = rowElem.findElement("hdnOtChargeCalculationFlag");
                        var hdnOTOTSlabAmount = rowElem.findElement("hdnOTOTSlabAmount");
                        var hdnOTHours = rowElem.findElement("hdnOTHours");
                        var hdnOTBedChargePerc = rowElem.findElement("hdnOTBedChargePerc"); //OTistimebased
                        var hdnOTistimebased = rowElem.findElement("OTistimebased");
                        var ChargePercentage = parseFloat(rowElem.findElement("lblChargePercentage").innerHTML);                       
                        
                        // alert(hdnOTistimebased.value);
                        if (parseInt(hdnIsMainSurgery.value) == 1) {

                            if (parseInt(hdnIsPriceEditable.value) == 1) {
                                if (hdnSurgeonType.value == 'SR') {
                                    var ServiceAmount = rowElem.findElement("txtServiceCharge");
                                    ServiceAmountNew = ServiceAmount//New
                                    mainSurgeryCharge = (parseFloat(ServiceAmount.value) * parseFloat(ChargePercentage)) / 100;
                                    rowElem.findElement("lblGrossCharge").innerHTML = parseFloat(mainSurgeryCharge).toFixed(DecimalPlaces);
                                    rowElem.findElement("lblNetCharge").innerHTML = parseFloat(mainSurgeryCharge).toFixed(DecimalPlaces);
                                    rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                }
                            }
                        }
                        else {
                            
                            
                            if (parseInt(hdnIsSurgeryService.value) == 1 && hdnSurgeonType.value == 'SR') {
                                rowElem.findElement("lblGrossCharge").innerHTML = parseFloat(parseFloat((parseFloat(rowElem.findElement("txtServiceCharge").value) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                rowElem.findElement("lblNetCharge").innerHTML = parseFloat(parseFloat((parseFloat(rowElem.findElement("txtServiceCharge").value) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                mainSurgeryCharge = (parseFloat(mainSurgeryCharge) + parseFloat(rowElem.findElement("lblNetCharge").innerHTML));
                            }

                            else {
                                if (hdnServiceType.value == 'OT' && hdnOTistimebased.value == 'Y') {
                                    if (document.getElementById('<%= hdnisCalculateOT.ClientID%>').value == 'Y') {
                                        if (hdnOtChargeCalculationFlag.value == 'Hourly') {
                                            rowElem.findElement("txtServiceCharge").value = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnOTOTSlabAmount.value))).toFixed(DecimalPlaces);
                                            rowElem.findElement("lblGrossCharge").innerHTML = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnOTOTSlabAmount.value))).toFixed(DecimalPlaces);
                                            rowElem.findElement("lblNetCharge").innerHTML = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnOTOTSlabAmount.value))).toFixed(DecimalPlaces);
                                            rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                        }
                                        else {
                                            //   alert(hdnOTBedChargePerc.value);
                                            var OTBedChargePerc = hdnOTBedChargePerc.value == 0 ? 1 : hdnOTBedChargePerc.value;
                                            rowElem.findElement("txtServiceCharge").value = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnOTOTSlabAmount.value) * parseFloat(OTBedChargePerc)) / 100).toFixed(DecimalPlaces);
                                            rowElem.findElement("lblGrossCharge").innerHTML = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnOTOTSlabAmount.value) * parseFloat(OTBedChargePerc)) / 100).toFixed(DecimalPlaces);
                                            rowElem.findElement("lblNetCharge").innerHTML = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(hdnOTOTSlabAmount.value) * parseFloat(OTBedChargePerc)) / 100).toFixed(DecimalPlaces);
                                            rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                        }
                                    }
                                }
                                else {
                                    if (hdnSurgeonType.value == 'AN' && document.getElementById('<%= hdnisAnesthetistCalculateAfterAddSurgeonAndAssistantSurgeon.ClientID%>').value == 'Y') {                                    
                                        rowElem.findElement("txtServiceCharge").value = parseFloat(parseFloat(((parseFloat(mainSurgeryCharge) + parseFloat(AssistantSurgeon)) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                        rowElem.findElement("lblGrossCharge").innerHTML = parseFloat(parseFloat(((parseFloat(mainSurgeryCharge) + parseFloat(AssistantSurgeon)) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                        rowElem.findElement("lblNetCharge").innerHTML = parseFloat(parseFloat(((parseFloat(mainSurgeryCharge) + parseFloat(AssistantSurgeon)) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                        rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);

                                    }
                                    else if (hdnSurgeonType.value == 'AS' && document.getElementById('<%= hdnisAssistantSurgeonCalculateonAmount.ClientID%>').value == 'Y') {
                                        ASChargePercentage = ((parseFloat(rowElem.findElement("txtServiceCharge").value) * 100) / mainSurgeryCharge).toFixed(DecimalPlaces);
                                        if (ASChargePercentage <= 100) {
                                            rowElem.findElement("lblChargePercentage").innerHTML = ((parseFloat(rowElem.findElement("txtServiceCharge").value) * 100) / mainSurgeryCharge).toFixed(DecimalPlaces);
                                            //rowElem.findElement("txtServiceCharge").value = parseFloat(parseFloat(((parseFloat(mainSurgeryCharge) + parseFloat(AssistantSurgeon)) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                            rowElem.findElement("lblGrossCharge").innerHTML = parseFloat(rowElem.findElement("txtServiceCharge").value).toFixed(DecimalPlaces);
                                            rowElem.findElement("lblNetCharge").innerHTML = parseFloat(rowElem.findElement("txtServiceCharge").value).toFixed(DecimalPlaces);
                                            rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                        }
                                        else {
                                            alert("Charge cann't greater then Surgeon charge!!!");
                                            rowElem.findElement("txtServiceCharge").value = "0.00";
                                        }
                                    }
                                    else {
                                        rowElem.findElement("txtServiceCharge").value = parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(ChargePercentage)) / 100).toFixed(DecimalPlaces);
                                        rowElem.findElement("lblGrossCharge").innerHTML = parseFloat(parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                        rowElem.findElement("lblNetCharge").innerHTML = parseFloat(parseFloat((parseFloat(mainSurgeryCharge) * parseFloat(ChargePercentage)) / 100)).toFixed(DecimalPlaces);
                                        rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                    }
                                }
                            }

                            if (hdnSurgeonType.value == 'AS') {
                                AssistantSurgeon = (parseFloat(AssistantSurgeon) + parseFloat(rowElem.findElement("lblNetCharge").innerHTML));
                            }
                        }


                        //New code for Nanavati Start
                        if (parseInt(hdnIsPriceEditable.value) == 1 && serviceid == 0) {
                            serviceid = parseInt(hdnServiceId.value);
                        }
                        //                        alert("New" + ServiceAmountNew.value);
                        if (parseInt(hdnServiceId.value) == serviceid) {
                            if (parseInt(hdnIsChargeDependent.value) == 1) {

                                rowElem.findElement("txtServiceCharge").value = parseFloat(ServiceAmountNew.value).toFixed(DecimalPlaces);;
                                rowElem.findElement("lblGrossCharge").innerHTML = parseFloat(parseFloat(ServiceAmountNew.value) * parseFloat(ChargePercentage) / 100).toFixed(DecimalPlaces);
                                rowElem.findElement("lblNetCharge").innerHTML = parseFloat(parseFloat(ServiceAmountNew.value) * parseFloat(ChargePercentage) / 100).toFixed(DecimalPlaces);
                                rowElem.findElement("hdnServiceDiscount").value = parseFloat((parseFloat(rowElem.findElement("lblNetCharge").innerHTML) * parseFloat(hdnServiceDiscountPerc.value)) / 100).toFixed(DecimalPlaces);
                                // alert(rowElem.findElement("hdnServiceDiscount").value);
                            }
                        }

                        ////New code for Nanavati End


                        var totalCharge = parseFloat(rowElem.findElement("lblNetCharge").innerHTML) - parseFloat(rowElem.findElement("hdnServiceDiscount").value);


                        rowElem.findElement("hdntxtServiceCharge").value = rowElem.findElement("txtServiceCharge").value;
                        rowElem.findElement("hdnlblGrossCharge").value = rowElem.findElement("lblGrossCharge").innerHTML;
                        rowElem.findElement("hdnlblNetCharge").value = rowElem.findElement("lblNetCharge").innerHTML;

                        rowElem.findElement("lblDiscPer").innerHTML = parseFloat(rowElem.findElement("hdnServiceDiscountPerc").value).toFixed(DecimalPlaces);
                        rowElem.findElement("lblDiscAmt").innerHTML = parseFloat(rowElem.findElement("hdnServiceDiscount").value).toFixed(DecimalPlaces);


                        var hdnPayerType = $get('<%=hdnPayerType.ClientID%>').value;

                        if (hdnServiceType.value != 'OT'
                            || (hdnServiceType.value == 'OT' && document.getElementById('<%= hdnisCalculateOT.ClientID%>').value == 'Y')) {
                            //if (parseFloat(rowElem.findElement("lblPatientAmount").innerHTML) > 0) {
                            if (hdnPayerType == 0) {
                                rowElem.findElement("lblPatientAmount").innerHTML = parseFloat(totalCharge).toFixed(DecimalPlaces);
                                rowElem.findElement("hdnlblPatientAmount").value = rowElem.findElement("lblPatientAmount").innerHTML;
                            }
                            //if (parseFloat(rowElem.findElement("lblPayerAmount").innerHTML) > 0) {
                            if (hdnPayerType == 1) {
                                rowElem.findElement("lblPayerAmount").innerHTML = parseFloat(totalCharge).toFixed(DecimalPlaces);
                                rowElem.findElement("hdnlblPayerAmount").value = rowElem.findElement("lblPayerAmount").innerHTML;
                            }
                        }
                    }
                }
            }


             function CalculateAmountNew(txtServiceCharge, hdnServiceActualAmount, hdnChargePercentage, NewhdnServiceType, hdnlblGrossCharge,
                hdnlblNetCharge, hdnlblPatientAmount, hdnlblPayerAmount, lblGrossCharge, lblNetCharge, lblPatientAmount, lblPayerAmount, hdnServiceDiscount, RowIndex) {
                var DecimalPlaces = document.getElementById('<%= hdnDecimalPlaces.ClientID%>').value;
                var ServiceActualAmount = document.getElementById(hdnServiceActualAmount).value;
                if (document.getElementById('<%= hdnIsAllowToEditSurgeryAmountOnlyForCash.ClientID%>').value == 'Y') {
                    if (parseFloat(document.getElementById(txtServiceCharge).value) < parseFloat(ServiceActualAmount)) {
                        alert('Service Charge cannot be less than Service Actual Amount');
                        document.getElementById(txtServiceCharge).value = parseFloat(ServiceActualAmount).toFixed(DecimalPlaces);
                        return;
                    }
                }

               
                if (document.getElementById(txtServiceCharge).value <= 0) {
                    alert('Charge should be greater than 0 !');
                    return false;
                }
                var DecimalPlaces = document.getElementById('<%= hdnDecimalPlaces.ClientID%>').value;
                
                

                //Start ----- Mission client required if percentage not define and price editable price type in textbox will save in table
               
                var NewServiceAmount = (parseFloat(document.getElementById(txtServiceCharge).value));
                var NewChargePercentage = (parseFloat(document.getElementById(hdnChargePercentage).value));

                var NewhdnServiceType = document.getElementById(NewhdnServiceType).value;

                //var hdnlblGrossCharge = (parseFloat(document.getElementById(hdnlblGrossCharge).value));
                //var hdnlblNetCharge = (parseFloat(document.getElementById(hdnlblNetCharge).value));
                //var hdnlblPatientAmount = (parseFloat(document.getElementById(hdnlblPatientAmount).value));
               // var hdnlblPayerAmount = (parseFloat(document.getElementById(hdnlblPayerAmount).value));

               // var lblGrossCharge = (parseFloat(document.getElementById(lblGrossCharge).value));
               // var lblNetCharge = (parseFloat(document.getElementById(lblNetCharge).value));
               // var lblPatientAmount = (parseFloat(document.getElementById(lblPatientAmount).value));
               // var lblPayerAmount = (parseFloat(document.getElementById(lblPayerAmount).value));
               // var hdnServiceDiscount = (parseFloat(document.getElementById(hdnServiceDiscount).value));

                var NewhdnPayerType = $get('<%=hdnPayerType.ClientID%>').value;

               
                
                ///End
                 if (parseFloat(NewChargePercentage) == 0 ) {

                    
                    
                    document.getElementById(lblGrossCharge).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                    
                    document.getElementById(lblNetCharge).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);


                    document.getElementById(hdnlblGrossCharge).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                    document.getElementById(hdnlblNetCharge).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                    
                   

                   

                    if (NewhdnPayerType == 0) {

                        document.getElementById(lblPatientAmount).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                        document.getElementById(lblPayerAmount).value = 0;
                        document.getElementById(hdnlblPatientAmount).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                        document.getElementById(hdnlblPayerAmount).value = 0;
                    }
                    //if (parseFloat(rowElem.findElement("lblPayerAmount").innerHTML) > 0) {
                    if (NewhdnPayerType == 1) {

                        document.getElementById(lblPayerAmount).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                        document.getElementById(lblPatientAmount).value = 0;
                        document.getElementById(hdnlblPayerAmount).value = parseFloat(NewServiceAmount).toFixed(DecimalPlaces);
                        document.getElementById(hdnlblPatientAmount).value = 0;
                    }



                }

            }

        </script>

    </telerik:RadCodeBlock>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpmgr" runat="server"></asp:ScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
        <asp:HiddenField ID="hdnPayerType" runat="server" />
        <asp:HiddenField ID="hdnIsAllowToEditSurgeryAmountOnlyForCash" runat="server" />


        <div id="dvZone1" style="width: 100%; overflow-x: hidden;">
            <div class="container-fluid header_main">
                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-6 text-center">
                        <asp:UpdatePanel ID="up5" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="col-md-6 col-sm-6 col-xs-6 text-right">
                        <asp:UpdatePanel ID="up1" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnisAnesthetistCalculateAfterAddSurgeonAndAssistantSurgeon" runat="server" Value="N" />
                                <asp:HiddenField ID="hdnisCalculateOT" runat="server" Value="Y" />
                                <asp:HiddenField ID="hdnisAssistantSurgeonCalculateonAmount" runat="server" Value="N" />

                                <asp:CheckBox ID="chkIsGenerateAdvance" Text="Generate Advance" Checked="false" runat="server"
                                    Visible="false" />
                                <asp:Button ID="btnCancel" Text="New" runat="server" ToolTip="Click here to refresh this window..." AccessKey="N" OnClick="ibtnCancel_OnClick" CssClass="btn btn-primary" />
                                <asp:Button ID="btnClose" Text="Close" runat="server" CausesValidation="false" OnClientClick="window.close();" ToolTip="Click here to discard and close this window" CssClass="btn btn-primary" />
                                <asp:Button ID="btnProceed" Text="Save" runat="server" ValidationGroup="save" AccessKey="P" OnClick="btnProceed_OnClick" CssClass="btn btn-primary" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>



            <div class="container-fluid">
                <div class="row">

                    <div class="table-responsive ">
                        <table class="table table-small-font table-bordered table-striped margin_bottom01">

                            <tr align="center">
                                <td colspan="1" align="left">

                                    <asp:UpdatePanel ID="up2" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:Label ID="Label6" runat="server" Text="UHID:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblUHID" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text="Payer Company:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblPayerCompany" runat="server" Text="" SkinID="label"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>





            <asp:UpdatePanel ID="up3" runat="server">
                <ContentTemplate>


                    <div class="container-fluid borderbottomBox">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-12">
                                <div class="row form-group">
                                    <div class="col-md-3 col-sm-3 col-4 label1">
                                        <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:PRegistration, department %>"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-8">
                                        <telerik:RadComboBox ID="radCmbDepartment" runat="server" AutoPostBack="true" MarkFirstMatch="true" OnSelectedIndexChanged="radCmbDepartment_OnSelectedIndexChanged" Skin="Metro" Width="100%"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-12">
                                <div class="row form-group">
                                    <div class="col-md-3 col-sm-3 col-4 label1 PaddingRightSpacing">
                                        <span style="width: 135px; float: left;">
                                            <asp:Label ID="lblSubDepartment" runat="server" Text="<%$ Resources:PRegistration, SubDepartment %>"></asp:Label></span>
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-8">
                                        <telerik:RadComboBox ID="radCmbSubDepartment" runat="server" AutoPostBack="true" EnableTextSelection="true" MarkFirstMatch="true" OnSelectedIndexChanged="radCmbSubDepartment_OnSelectedIndexChanged" Width="100%" Skin="Metro"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-12">
                                <div class="row form-group">
                                    <div class="col-md-3 col-sm-3 col-4 label1">
                                        <asp:Label ID="lblSurgeryService" runat="server" Text="<%$ Resources:PRegistration, service %>"></asp:Label>
                                        &nbsp;<font color="Red">*</font>
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-8">
                                        <asp:Panel ID="pnlAddService" runat="server" DefaultButton="btnAddServicetoGrid">
                                            <telerik:RadComboBox ID="radCmbSurgeryServices" runat="server" Width="100%" Height="350px" EmptyMessage="[Select Service]" AllowCustomText="true" ShowMoreResultsBox="true" EnableLoadOnDemand="true" OnItemsRequested="ddlService_OnItemsRequested" DataTextField="ServiceName" DataValueField="ServiceId" EnableVirtualScrolling="true" Skin="Metro" EnableItemCaching="false"></telerik:RadComboBox>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-12">
                                <div class="row form-group">
                                    <div class="col-md-2 col-sm-3 col-4 PaddingRightSpacing">
                                        <div class="PD-TabRadioNew01 margin_z">
                                            <asp:Panel ID="Panel2" runat="server" CssClass="blockNew01" DefaultButton="btnAddServicetoGrid">
                                                <asp:CheckBox ID="cbMainSurgery" runat="server" Enabled="true" Text="Main Surgery" />
                                            </asp:Panel>
                                        </div>


                                    </div>
                                    <div class="col-md-10 col-sm-9 col-8">
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAddServicetoGrid">
                                            <asp:Label ID="Label2" runat="server" CssClass="label1" Text="Order Date&nbsp;&nbsp;&nbsp;"></asp:Label>
                                            <telerik:RadDateTimePicker ID="dtOrderDate" CssClass="inlin-bl1New" runat="server" Skin="Metro"></telerik:RadDateTimePicker>
                                            <telerik:RadComboBox ID="ddlOrderMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderMinutes_SelectedIndexChanged" Width="50px" Skin="Metro"></telerik:RadComboBox>
                                            <asp:Button ID="btnAddServicetoGrid" runat="server" Text="Add" OnClick="btnAddServicetoGrid_OnClick" CssClass="btn btn-primary" />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>

                            <asp:CheckBox ID="chkUnClean" runat="server" SkinID="checkbox" Text="UnClean" Visible="false" />
                        </div>
                    </div>


                    <div class="container-fluid">

                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12 col-12">
                                <div class="table-responsive">
                                    <telerik:RadGrid ID="gvSurgery" RenderMode="LightWeight" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                        ShowFooter="false" Width="100%" AutoGenerateColumns="false" runat="server"
                                        OnItemDataBound="gvSurgery_OnItemDataBound" OnItemCommand="gvSurgery_OnItemCommand"
                                        HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="Black" Skin="Windows7">

                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                        </ClientSettings>
                                        <MasterTableView DataKeyNames="ServiceId,ServiceName" TableLayout="Fixed" GroupLoadMode="Client"
                                            AllowFilteringByColumn="false">
                                            <HeaderStyle Width="200px" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="SNo" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <%#Container.ItemIndex+1 %>
                                                        <asp:HiddenField ID="hdnSNO" runat="server" Value='<%#Container.ItemIndex+1 %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Surgery" HeaderStyle-Width="610px">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Main" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnlMain" runat="server" Text='<%#Eval("IsSurgeryMain") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-Width="60px" HeaderText="Move">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSelect" runat="server" Text="Select" CommandName="Select"
                                                            ForeColor="DodgerBlue" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Remove" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                            CommandName="Delete" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DoctorShare" HeaderStyle-Width="80px" HeaderText="DoctorShare">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDoctorShare" runat="server" Text="Add" ForeColor="DodgerBlue"
                                                            CommandName="Sharing" />
                                                        <asp:HiddenField ID="hndxmlSurgoenShare" Value='<%#Eval("SurgoenShare") %>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="SurgeryComponent" HeaderStyle-Width="125px"
                                                    HeaderText="SurgeryComponent">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnAddSurgeryComponent" runat="server" Text="Add" CommandName="AddComponent"
                                                            ForeColor="DodgerBlue" />
                                                        <asp:HiddenField ID="hndxmlSurgeryComponent" Value='<%#Eval("SurgeryComponent") %>'
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                            <%-- <div class="col-md-offset-10 col-md-2 col-sm-offset-9 col-sm-3 col-xs-offset-8 col-xs-4 text-right">--%>
                            <span style="padding: 5px;">

                                <asp:Button ID="btnUp" Text="Move Up" CssClass="btn btn-primary" OnClick="btnUp_OnClick" ToolTip="Click to move up" runat="server" />
                                <asp:Button ID="btnDown" Text="Move Down" CssClass="btn btn-primary" OnClick="btnDown_OnClick" ToolTip="Click to move down" runat="server"  />
                            </span>

                            <%-- </div>--%>
                        </div>

                        <%--<div class="row form-group">
                          
                        </div>--%>
                    </div>




                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-6 PaddingRightSpacing01">
                                <div class="col-md-12 col-sm-12 col-xs-12 header_main margin_bottom01">
                                    <h2 style="color: #333;">
                                        <asp:Label ID="lblOTDetails" runat="server" Text="&nbsp;&nbsp;O.T. Details"></asp:Label></h2>
                                </div>

                                <div class="borderBox">

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label1">
                                            <asp:Label ID="Label8" runat="server" Text="OT Room"></asp:Label>&nbsp;<font color="Red">*</font>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <telerik:RadComboBox ID="radCmbOtRoom" runat="server" Filter="Contains" MarkFirstMatch="true" Width="100%" Skin="Metro"></telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label1 PaddingRightSpacing">
                                            <asp:Label ID="Label10" runat="server" Text="Check In Time"></asp:Label>&nbsp;<font color="Red">*</font>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <div class="row">
                                                <div class="col-xs-9 col-md-4 col-sm-9 col-6">
                                                    <telerik:RadDateTimePicker ID="rdtpOtStartTime" CssClass="inlin-bl1New" runat="server" Skin="Metro" AutoPostBackControl="Both" OnSelectedDateChanged="rdtpOtStartTime_OnSelectedDateChanged" />
                                                </div>
                                                <div class="col-xs-3 col-md-2 col-sm-3 col-2 PaddingLeftSpacing" style="margin-left: 15px">
                                                    <telerik:RadComboBox ID="radCmbOtStartTimeM" runat="server" CssClass="border-LeftRight" AutoPostBack="True" OnSelectedIndexChanged="radCmbOtStartTimeM_SelectedIndexChanged" Width="100%" Skin="Metro" />
                                                </div>
                                                <div class="col-md-4 col-sm-3 col-3">
                                                    <asp:Label ID="lblTimeChkIn" runat="server" Text="Check In" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label1 PaddingRightSpacing">
                                            <asp:Label ID="Label12" runat="server" Text="Check Out Time"></asp:Label><font color="Red">*</font>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <div class="row">
                                                <div class="col-xs-9 col-md-4 col-6">
                                                    <telerik:RadDateTimePicker ID="rdtpOtEndTime" CssClass="inlin-bl1New" runat="server" Skin="Metro" />
                                                </div>
                                                <div class="col-xs-3 col-md-2 col-2 PaddingLeftSpacing" style="margin-left: 15px;">
                                                    <telerik:RadComboBox ID="radCmbOtEndTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbOtEndTimeM_SelectedIndexChanged" Width="100%" Skin="Metro" />
                                                </div>
                                                <div class="col-md-4 col-sm-3 col-3 label1 PaddingRightSpacing">
                                                    <asp:Label ID="lblTimeChkOut" runat="server" Text="Check Out" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>



                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label1 PaddingRightSpacing">
                                            <asp:Label ID="lblInfoEquipment" runat="server" Text="OT Equipments"></asp:Label>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8 AddSpacingLeft">
                                            <span class="">
                                                <telerik:RadComboBox ID="ddlOTEquipments" runat="server" Filter="Contains" MarkFirstMatch="true" Width="100%" Skin="Metro" DropDownWidth="400px"></telerik:RadComboBox>
                                            </span>
                                        </div>


                                    </div>
                                    <div class="row form-group">

                                        <div class="col-md-3 col-sm-3 col-4 label1 PaddingRightSpacing">
                                            <span id="Span1" runat="server">
                                                <asp:Label ID="Label1" runat="server" Visible="false" Text="No. Of Surgery"></asp:Label></span>
                                        </div>
                                        <div class="col-md-3 col-sm-3 col-2 AddSpacingRight">
                                            <span id="Span2" runat="server" class="">
                                                <telerik:RadComboBox ID="radCmbsurgeryClassification" runat="server" MarkFirstMatch="true" Width="100%" Skin="Metro" Visible="false" EmptyMessage="[Select]" AutoPostBack="true" OnSelectedIndexChanged="radCmbsurgeryClassification_SelectedIndexChanged" DropDownWidth="200px">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="All" Value="0" />
                                                        <telerik:RadComboBoxItem Text="Surgery 1" Value="1" />
                                                        <telerik:RadComboBoxItem Text="Surgery 2" Value="2" />
                                                        <telerik:RadComboBoxItem Text="Surgery 3" Value="3" />
                                                        <telerik:RadComboBoxItem Text="Surgery 4" Value="4" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </span>
                                        </div>




                                        <div class="col-md-3 col-sm-3 col-3 label1 PaddingRightSpacing">
                                            <span id="tdResource" runat="server">
                                                <asp:Label ID="lblAddDoctor" runat="server" Text="Add Resource(s)"></asp:Label></span>
                                        </div>
                                        <div class="col-md-3 col-sm-3 col-3 AddSpacingRight">
                                            <span id="tdResource1" runat="server" class="">
                                                <telerik:RadComboBox ID="radCmbDoctorClassification" runat="server" MarkFirstMatch="true" Width="100%" Skin="Metro" EmptyMessage="[Select]" AutoPostBack="true" OnSelectedIndexChanged="radCmbDoctorClassification_OnSelectedIndexChanged" DropDownWidth="200px"></telerik:RadComboBox>
                                            </span>
                                        </div>
                                    </div>


                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label1 PaddingRightSpacing">
                                            <asp:Label ID="lblAnesthesiaType" Visible="false" runat="server" CssClass="label1" Text="Anesthesia Type"></asp:Label>
                                        </div>
                                        <div class="col-md-3 col-sm-3 col-4 AddSpacingRight">
                                            <telerik:RadComboBox ID="rbAnesthesiaType" Visible="false" runat="server" Width="100%" placeholder="AnesthesiaType&nbsp;"
                                                Filter="Contains">
                                            </telerik:RadComboBox>
                                        </div>

                                        <div class="col-md-6 col-sm-6 col-8">
                                            <asp:GridView ID="gvResourceList" ShowHeader="false" Width="100%" AutoGenerateColumns="false"
                                                runat="server" OnRowDataBound="gvResourceList_OnRowDataBound" ShowFooter="false"
                                                OnRowCommand="gvResourceList_OnRowCommand">
                                                <EmptyDataTemplate>
                                                    <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblResourceName" runat="server" Text='<%#Eval("ResourceName") %>'
                                                                Font-Bold="true" /><font color='Red'>*</font>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imbtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                                CommandName="Deleted" CommandArgument='<%#Eval("ID") %>' />
                                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("ID") %>' />
                                                            <asp:HiddenField ID="hdnResourceID" runat="server" Value='<%#Eval("ResourceID") %>' />
                                                            <asp:HiddenField ID="hdnResourceType" runat="server" Value='<%#Eval("ResourceType") %>' />
                                                            <asp:HiddenField ID="hdnSurgeryNo" runat="server" Value='<%#Eval("SurgeryNo") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4"></div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <span id="tdEmergency" class="PD-TabRadio margin_z" runat="server">
                                                <asp:CheckBox ID="chkIsEmergency" runat="server" Text="Is Emergenecy" /></span>
                                        </div>
                                    </div>


                                </div>

                            </div>

                            <div class="col-md-6 col-sm-6 col-12">
                                <div class="col-md-12 col-sm-12 col-12 header_main margin_bottom01">
                                    <h2 style="color: #333;">
                                        <asp:Label ID="lblAnaesthesia" runat="server" Text="&nbsp;&nbsp;Surgery linked Charges"></asp:Label></h2>
                                </div>

                                <div class="borderBox">

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label2">
                                            <asp:Label ID="Label7" runat="server" Text="Anaesthesia/Other&nbsp;Charges"></asp:Label>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <telerik:RadComboBox ID="radCmbAnaesthesia" runat="server" MarkFirstMatch="true" Filter="Contains" Width="100%" Skin="Metro" CheckBoxes="true" CheckedItemsTexts="FitInInput" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label2">
                                            <asp:Label ID="Label9" runat="server" Text="Start Time"></asp:Label>&nbsp;<font color="Red">*</font>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <div class="row">
                                                <div class="col-xs-9 col-md-4 col-6">
                                                    <telerik:RadDateTimePicker ID="rdtpAstartTime" CssClass="inlin-bl1New" runat="server" Skin="Metro" />
                                                </div>
                                                <div class="col-xs-3 col-md-2 col-3 PaddingLeftSpacing" style="margin-left: 15px;">
                                                    <telerik:RadComboBox ID="radCmbAstartTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbAstartTime_SelectedIndexChanged" Width="100%" Skin="Metro" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 label2">
                                            <asp:Label ID="Label11" runat="server" Text="End Time"></asp:Label>&nbsp;<font color="Red">*</font>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <div class="row">
                                                <div class="col-xs-9 col-md-4 col-sm-9 col-6">
                                                    <telerik:RadDateTimePicker ID="rdtpAEndTime" CssClass="inlin-bl1New" runat="server" Skin="Metro" />
                                                </div>
                                                <div class="col-xs-3 col-md-2 col-sm-3 col-3 PaddingLeftSpacing" style="margin-left: 15px;">
                                                    <telerik:RadComboBox ID="radCmbAEndTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbAEndTimeM_SelectedIndexChanged" Width="100%" Skin="Metro" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3 col-sm-3 col-4 PaddingRightSpacing label2">
                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:PRegistration, billingcategory%>"></asp:Label><font color="red">*</font>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-8">
                                            <telerik:RadComboBox ID="radCmbBedCategory" runat="server" MarkFirstMatch="true" Filter="Contains" Width="100%" Skin="Metro"></telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="row form-group">

                                        <div class="col-md-12 col-sm-12 col-12">
                                            <div class="row">
                                                <div class="col-md-5 col-sm-5 col-5 PaddingRightSpacing">
                                                    <div class="PD-TabRadio margin_z">
                                                        <asp:RadioButtonList ID="rdoIncision" CssClass="blockNew01 inlin-bl1New" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoIncision_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Single Incision" Value="0" Selected="True" />
                                                            <asp:ListItem Text="Multi Incision" Value="1" />
                                                        </asp:RadioButtonList>
                                                        <asp:Label ID="lblInfoBillCategory" runat="server" ForeColor="Green"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-7 col-sm-7 col-7 text-right PaddingLeftSpacing">

                                                    <asp:CheckBox ID="chkStat" runat="server" Checked="false" Font-Bold="true" TabIndex="43" Text="Stat" TextAlign="Right" onclick="fnCheckOne(this)" />
                                                    <asp:CheckBox ID="chkIsHighRiskSurgery" runat="server" Checked="false" Font-Bold="true" TabIndex="44" Text="Is High Risk" TextAlign="Right" onclick="fnCheckOne(this)" />
                                                    <asp:CheckBox ID="chkIsHighRiskState" runat="server" Checked="false" Font-Bold="true" TabIndex="44" Text="High Risk Stat" TextAlign="Right" onclick="fnCheckOne(this)" />
                                                    <asp:Button ID="btnAddOtherSuregon" runat="server" OnClick="btnAddOtherSuregon_OnClick" Text="" Visible="false" />
                                                    <asp:Button ID="BtnCalculateCharges" runat="server" Text="Calculate Charges" CssClass="btn btn-primary" OnClick="BtnCalculateCharges_OnClick" />
                                                    <asp:HiddenField ID="hdnISAnesthesiaTypeAndAnaesthesiaOtherChargeMandatory" runat="server" Value="N" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>

                        </div>
                    </div>

                    <div class="container-fluid">
                        <div class="row form-group">
                            <div class="col-md-12 col-xs-12">
                                <asp:UpdatePanel ID="RadWinw" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>





            <div class="container-fluid">

                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <asp:UpdatePanel ID="up6" runat="server" style="overflow-x: auto;">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvAddedSurgery" SkinID="gridviewOrderNew" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                    ShowFooter="false" Width="100%" AutoGenerateColumns="false" runat="server" Skin="Metro"
                                    OnItemDataBound="gvAddedSurgery_OnItemDataBound" OnItemCommand="gvAddedSurgery_OnItemCommand"
                                    HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="LightGray">
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="true" />
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="ServiceId" TableLayout="Fixed" GroupLoadMode="Client"
                                        AllowFilteringByColumn="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="SNo" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%#Container.ItemIndex+1 %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Surgery" HeaderStyle-Width="18%" ItemStyle-Width="18%">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                                    <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                    <asp:HiddenField ID="hdnResourceId" runat="server" Value='<%#Eval("ResourceId") %>' />
                                                    <asp:HiddenField ID="hdnDoctorCharge" runat="server" Value='<%#Eval("DoctorCharge") %>' />
                                                    <asp:HiddenField ID="hdnServiceDiscount" runat="server" Value='<%#Eval("ServiceDiscount") %>' />
                                                    <asp:HiddenField ID="hdnDoctorDiscount" runat="server" Value='<%#Eval("DoctorDiscount") %>' />
                                                    <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%#Eval("ServiceType") %>' />
                                                    <asp:HiddenField ID="hdnMainSurgeryId" runat="server" Value='<%#Eval("MainSurgeryId") %>' />
                                                    <asp:HiddenField ID="hdnIsMainSurgery" runat="server" Value='<%#Eval("IsMainSurgery") %>' />
                                                    <asp:HiddenField ID="hdnIsSurgeryService" runat="server" Value='<%#Eval("IsSurgeryService") %>' />
                                                    <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                                    <asp:HiddenField ID="hdnSubDeptId" runat="server" Value='<%#Eval("SubDeptId") %>' />
                                                    <asp:HiddenField ID="hdnSurgeonType" runat="server" Value='<%#Eval("SurgeonType") %>' />
                                                    <asp:HiddenField ID="hdnDoctorRequired" runat="server" Value='<%#Eval("DoctorRequired") %>' />
                                                    <asp:HiddenField ID="hdnSurgeonTypeId" runat="server" Value='<%#Eval("SurgeonTypeId") %>' />
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                    <asp:HiddenField ID="hdnResourceType" runat="server" Value='<%#Eval("ResourceType") %>' />
                                                    <asp:HiddenField ID="hdnIsPriceEditable" runat="server" Value='<%#Eval("IsPriceEditable") %>' />
                                                    <asp:HiddenField ID="hdnServiceDiscountPerc" runat="server" Value='<%#Eval("ServiceDiscountPerc") %>' />
                                                    <asp:HiddenField ID="hdnActualGrossCharge" runat="server" Value='<%#Eval("ActualGrossCharge") %>' />
                                                    <asp:HiddenField ID="hdnActualNetCharge" runat="server" Value='<%#Eval("ActualNetCharge") %>' />
                                                    <asp:HiddenField ID="hdnSurgeryComponentId" runat="server" Value='<%#Eval("SurgeryComponentId") %>' />
                                                    <asp:HiddenField ID="hdnIsEmergency" runat="server" Value='<%#Eval("IsEmergency") %>' />
                                                    <asp:HiddenField ID="hdnEmergencyPerc" runat="server" Value='<%#Eval("EmergencyPerc")%>' />
                                                    <asp:HiddenField ID="hdnIsChargeDependent" runat="server" Value='<%#Eval("IsChargeDependent")%>' />
                                                    <asp:HiddenField ID="hdnOtChargeCalculationFlag" runat="server" Value='<%#Eval("OtChargeCalculationFlag")%>' />
                                                    <asp:HiddenField ID="hdnOTOTSlabAmount" runat="server" Value='<%#Eval("OTSlabAmount")%>' />
                                                    <asp:HiddenField ID="hdnOTHours" runat="server" Value='<%#Eval("OTHours")%>' />
                                                    <asp:HiddenField ID="hdnOTBedChargePerc" runat="server" Value='<%#Eval("OTBedChargePerc")%>' />
                                                    <asp:HiddenField ID="hdnOTistimebased" runat="server" Value='<%#Eval("OTistimebased")%>' />
                                                    <asp:HiddenField ID="hdnSurgeryId" runat="server" Value='<%#Eval("SurgeryId")%>' />
                                                    <asp:HiddenField ID="hdnHighRiskSurgery" runat="server" Value='<%#Eval("HighRiskSurgery") %>' />
                                                    <asp:HiddenField ID="hdnHighRiskStat" runat="server" Value='<%#Eval("HighRiskStat") %>' />
                                                    <asp:HiddenField ID="hdnStat" runat="server" Value='<%#Eval("Stat") %>' />
                                                     <asp:HiddenField ID="hdnTariffId" runat="server" Value='<%#Eval("TariffId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <%--HeaderStyle-Width="10%" ItemStyle-Width="10%"--%>
                                            <telerik:GridTemplateColumn HeaderText="Resource Type" UniqueName="ResourceType" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResourceType" runat="server" Text='<%#Eval("ResourceType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="120px" />
                                                <ItemStyle Width="120px" />
                                            </telerik:GridTemplateColumn>
                                            <%--HeaderStyle-Width="30%" ItemStyle-Width="30%"--%>
                                            <telerik:GridTemplateColumn HeaderText="Resource Name" UniqueName="ResourceName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResourceName" runat="server" Text='<%#Eval("ResourceName") %>'
                                                        Visible="false"></asp:Label>
                                                    <telerik:RadComboBox ID="ddlResourceName" runat="server" Filter="Contains" MarkFirstMatch="true"
                                                        Width="98%" Skin="Metro" Height="250px" DropDownWidth="350px" OnSelectedIndexChanged="ddlResourceName_OnSelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="180px" />
                                                <ItemStyle Width="180px" />
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Service Charge" UniqueName="ServiceCharge"
                                                HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtServiceCharge" runat="server" Style="text-align: right; width: 98%;"
                                                        Text='<%#Eval("ServiceCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                    <AJAX:FilteredTextBoxExtender ID="FTBEServiceDiscountP" runat="server" ValidChars="."
                                                        FilterType="Custom,Numbers" TargetControlID="txtServiceCharge">
                                                    </AJAX:FilteredTextBoxExtender>
                                                    <asp:HiddenField ID="hdntxtServiceCharge" runat="server" Value='<%#Eval("ServiceCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                    <asp:HiddenField ID="hdnServiceActualAmount" runat="server" Value='<%#Eval("ServiceCharge") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Charge%" UniqueName="ChargePercentage" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChargePercentage" runat="server" SkinID="label" Text='<%#Eval("ChargePercentage","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                     <asp:HiddenField ID="hdnChargePercentage" runat="server" Value='<%#Eval("ChargePercentage") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="70px" />
                                                <ItemStyle Width="70px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Gross Charge" UniqueName="GrossCharge" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGrossCharge" runat="server" Style="text-align: right; width: 98%;"
                                                        Text='<%#Eval("GrossCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                    <asp:HiddenField ID="hdnlblGrossCharge" runat="server" Value='<%#Eval("GrossCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Disc%" UniqueName="ServiceDiscountPerc" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscPer" runat="server" Style="text-align: right; width: 98%;"
                                                        Text='<%#Eval("ServiceDiscountPerc","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle Width="60px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Disc Amt" UniqueName="NetCharge" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscAmt" runat="server" Style="text-align: right; width: 98%;"
                                                        Text='<%#Eval("ServiceDiscount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Net Charge" UniqueName="NetCharge" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNetCharge" runat="server" Style="text-align: right; width: 98%;"
                                                        Text='<%#Eval("NetCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                    <asp:HiddenField ID="hdnlblNetCharge" runat="server" Value='<%#Eval("NetCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Patient Amt" UniqueName="PatientAmount" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientAmount" runat="server" Text='<%#Eval("PatientAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnlblPatientAmount" runat="server" Value='<%#Eval("PatientAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Payer Amt" UniqueName="PayerAmount" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayerAmount" runat="server" Text='<%#Eval("PayerAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnlblPayerAmount" runat="server" Value='<%#Eval("PayerAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Incision Time" UniqueName="IncisionTime">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncisionTime" runat="server" Text='<%#Eval("IncisionTime", "{0:dd/MM/yyyy HH:mmtt}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="120px" />
                                                <ItemStyle Width="120px" />
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                        CommandName="Delete" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnDefaultCompanyId" runat="server" />
                                <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                <asp:HiddenField ID="hdnXmlSurgery" runat="server" Value="" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="" />
                                <asp:HiddenField ID="hdnSurServiceID" runat="server" Value="" />
                                <asp:HiddenField ID="hdncheckboxValue" runat="server" Value="" />
                                <asp:HiddenField ID="hdnXmlSurgoenShare" runat="server" Value="" />
                                <asp:HiddenField ID="hdnXmlSurgeryComponent" runat="server" Value="" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-12 col-xs-12" style="visibility: hidden">
                        <asp:Button ID="btnBindGridWithXml" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnBindGridWithXml_OnClick" />
                        <asp:Button ID="btnBindGridWithXmlsurgeon" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnBindGridWithXmlsurgeon_OnClick" />
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
