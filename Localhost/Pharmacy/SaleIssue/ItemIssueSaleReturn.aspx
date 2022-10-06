<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ItemIssueSaleReturn.aspx.cs" Inherits="Pharmacy_ItemIssueSaleReturn" %>

<%@ Register Src="~/Include/Components/PaymentMode.ascx" TagName="PaymentMode" TagPrefix="ucPaymentMode" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="ucl" TagName="ComboEmployeeSearch" Src="~/Include/Components/EmployeeSearchCombo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
      <link href="../../Include/css/emr.css" rel="stylesheet" />
     <link href="../../Include/css/emr1.css" rel="stylesheet" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" />
    <style>
        #ctl00_ContentPlaceHolder1_ddlPatientType {margin-top:-8px;}
        .let-tp{margin-top:5px;}
    </style>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script src="../../Include/JS/Validate.js" type="text/javascript"></script>

        <script type="text/javascript" language="javascript">
             function OnClientSelectedIndexChangedEventHandler(sender, args) {

                var item = args.get_item();
                $get('<%=hdnEmployeeId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=btnSearchByEmpNo.ClientID%>').click();
            }
            function validateMaxLength() {
                var txt = $get('<%=txtRegistrationNo.ClientID%>');
                if (txt.value > 9223372036854775807) {
                    alert("Value should not be more then 9223372036854775807.");
                    txt.value = txt.value.substring(0, 12);
                    txt.focus();
                }
            }

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
                    case 113:  // F2
                        $get('<%=btnNew.ClientID%>').click();
                        break;
                    case 115:  // F4
                        $get('<%=btnAddNewItem.ClientID%>').click();
                        break;
                    case 120:  // F9
                        $get('<%=btnSaveData.ClientID%>').click();
                        break;
                }
                evt.returnValue = false;
                return false;
            }

            function openRadWindow(strPageNameWithQueryString) {
                var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
            }

            function openRadWindow(e, Purl, wHeight, wWidth) {
                var unicode = e.keyCode ? e.keyCode : e.charCode


                if (unicode == '13') {
                    var oWnd = radopen(Purl);
                    oWnd.setSize(wHeight, wWidth);
                    oWnd.set_modal(true);
                    oWnd.set_visibleStatusbar(false);
                    oWnd.Center();
                }
            }

            //debugger;
            function chekQty(txtstockqty, txtqty, txtsellingprice, txtdiscpersent, txtdiscamt, txtnetamt, txtPatientAmt, txtPayerAmt, hdnCopayPerc, hdnISDHAApproved, txtApprovalCode, e) {

                if (e.keyCode != 13) {
                    var stockqty = Number(0);
                    var qty = Number(0);
                    var Sellingprice = Number(0);
                    var discpersent = Number(0);
                    var discamt = Number(0);
                    var netamt = Number(0);
                    var balqty = Number(0);
                    var hdnISDHAApproved = $get(hdnISDHAApproved).value;

                    var txtApprovalCode = '';

                    if ($get('<%=txtApprovalCode.ClientID%>') != null) {
                        txtApprovalCode = txtApprovalCode.value;
                    }

                    stockqty = $get(txtstockqty).value;
                    qty = $get(txtqty).value;

                    if ((qty * 1) > (stockqty * 1)) {

                        $get(txtqty).value = '';
                        $get(txtnetamt).value = '';
                        $get(txtPatientAmt).value = '';
                        $get(txtPayerAmt).value = '';

                        alert('Sale Quantity Can not be greater than to stock Quantity, Stock Qty is  ' + Number(stockqty) + '  !');
                        return;
                    }
                    Sellingprice = $get(txtsellingprice).value;
                    discpersent = $get(txtdiscpersent).value;

                    discamt = (qty * Sellingprice * discpersent) / 100;
                    $get(txtdiscamt).value = discamt.toFixed(2);

                    netamt = ((qty * Sellingprice) - discamt.toFixed(2)).toFixed(2);

                    //$get(txtnetamt).value = netamt.toFixed(2);
                    $get(txtnetamt).value = netamt;

                    var CopayAmt = 0;
                    if (parseFloat($get(hdnCopayPerc).value) > 0) {
                        CopayAmt = $get(hdnCopayPerc).value / 100 * (netamt);
                    }

                    var defaultCompanyId = $get('<%=hdnDefaultCompanyId.ClientID%>').value;
                    var paymentType = $get('<%=hdnPaymentType.ClientID%>').value;



                    if (CopayAmt > 0) {
                        $get(txtPayerAmt).value = (netamt - CopayAmt).toFixed(2);
                        $get(txtPatientAmt).value = CopayAmt.toFixed(2);
                    }
                    else if (paymentType == 'C') {
                        //$get(txtPatientAmt).value = netamt.toFixed(2);
                        $get(txtPatientAmt).value = netamt;
                        $get(txtPayerAmt).value = 0;
                    }
                    else if (paymentType == 'B') {
                        $get(txtPatientAmt).value = 0;
                        if (hdnISDHAApproved == '1') {
                            //$get(txtPayerAmt).value = netamt.toFixed(2);
                            $get(txtPayerAmt).value = netamt;
                        }
                        else if (length(txtApprovalCode) > 4 && hdnISDHAApproved == '0') {
                            //$get(txtPayerAmt).value = netamt.toFixed(2);
                            $get(txtPayerAmt).value = netamt;
                        }
                        else if (length(txtApprovalCode) <= 4 && hdnISDHAApproved == '0') {
                            $get(hdnCopayPerc).value = '100.00';
                            //$get(txtPatientAmt).value = netamt.toFixed(2);
                            $get(txtPatientAmt).value = netamt;
                        }
                    }



                    // Calculate  Qty,Unit,NetAmtt, patient amt, payer amt of all grid item and total of all items when change Quantity.
                    var ftrQty = Number(0);
                    var ftrunit = Number(0);
                    var ftrTotalDiscount = Number(0);
                    var ftrTotalPatient = Number(0);
                    var ftrTotalPayer = Number(0);
                    var ftrNetAmt = Number(0);
                    var gridview = document.getElementById('<%=gvService.ClientID %>');
                    var length = gridview.rows.length;
                    var rowidx = '';


                    for (i = 0; i < length; i++) {

                        if (i > 1) {
                            if (i < 10) {
                                rowidx = '0' + i.toString();
                            }
                            else {
                                rowidx = i.toString();
                            }
                            var Qty = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtQty').value);
                            // var Unit = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_lblUnit').value);
                            var DiscAmt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value);
                            var charge = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtNetAmt').value);
                            var patientAmt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtPatient').value);
                            var payerAmt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtPayer').value);

                            if (Qty > 0) {
                                var pQty = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtQty').value);
                                ftrQty += pQty;
                            }
                            //                        if (Unit > 0) {
                            //                            var pUnit = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_lblUnit').value);
                            //                            ftrunit += pUnit;
                            //                        }
                            if (DiscAmt > 0) {

                                var Dicamt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value);
                                ftrTotalDiscount += Dicamt;
                            }
                            if (charge > 0) {

                                var Netamt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtNetAmt').value);
                                ftrNetAmt += Netamt;
                            }
                            if (patientAmt > 0) {

                                var ptamt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtPatient').value);
                                ftrTotalPatient += ptamt;
                            }
                            if (payerAmt > 0) {

                                var pyramt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtPayer').value);
                                ftrTotalPayer += pyramt;
                            }
                        }

                    }



                    var footerrow = gridview.rows.length;
                    var footerrowidx = '';
                    if (footerrow < 10) {
                        footerrowidx = '0' + footerrow.toString();
                    }
                    else {
                        footerrowidx = footerrow.toString();
                    }


                    $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotQty').value = Number(ftrQty);
                    $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotDiscount').value = ftrTotalDiscount.toFixed(2);
                    $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotNetamt').value = ftrNetAmt.toFixed(2);
                    $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotalPatient').value = ftrTotalPatient.toFixed(2);
                    $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotalPayer').value = ftrTotalPayer.toFixed(2);

                    $get('<%=txtNetAmount.ClientID%>').value = ftrNetAmt.toFixed(2);
                    $get('<%=txtLAmt.ClientID%>').value = Math.round(ftrNetAmt.toFixed(2));


                    $get('<%=txtRounding.ClientID%>').value = (Math.round(ftrTotalPatient.toFixed(2)) - ftrTotalPatient).toFixed(2);

                    $get('<%=txtReceived.ClientID%>').value = Math.round(ftrTotalPatient.toFixed(2));
                    // Fill Amount in Paymode Grid

                    document.getElementById('ctl00_ContentPlaceHolder1_paymentMode_txtNetBAmount').value = Math.round(ftrTotalPatient.toFixed(2));
                    var tableElement = $find('ctl00_ContentPlaceHolder1_paymentMode_grdPaymentMode');
                    var MasterTable = tableElement.get_masterTableView();

                    var lengthpaymentMode = MasterTable.get_dataItems().length;


                    for (i = 0; i < lengthpaymentMode; i++) {
                        if (i == 0) {
                            var rowElem = MasterTable.get_dataItems()[i];
                            rowElem.findElement("txtAmount").value = Math.round(ftrTotalPatient.toFixed(2));
                            rowElem.findElement("txtBalance").value = 0.00;
                        }
                        else {
                            var rowElem = MasterTable.get_dataItems()[i];
                            rowElem.findElement("txtAmount").value = 0.00;
                            rowElem.findElement("txtBalance").value = 0.00;
                        }
                    }
                }
                else {
                    $get('<%=btnPreviousDate.ClientID%>').click();

                    return false;
                }
            }

            function wndAddService_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlString;
                    $get('<%=hdnxmlString.ClientID%>').value = xmlString;
                }
                $get('<%=btnBindGridWithXml.ClientID%>').click();
            }
            function btnCoPay_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                $get('<%=btnCopayRefresh.ClientID%>').click();
            }

            function SearchDocOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlDocString;
                    $get('<%=txtDocNo.ClientID%>').value = xmlString;
                }
                $get('<%= btnGetSaleDetails.ClientID%>').click();
            }
            function SearchPrescriptionOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var IndentId = arg.IndentId;
                    var IndentNo = arg.IndentNo;
                    var RegistrationId = arg.RegistrationId;
                    var SelectedItems = arg.SelectedItems;
                    
                    $get('<%=hdnIndentId.ClientID%>').value = IndentId;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnIndentNo.ClientID%>').value = IndentNo;
                    $get('<%=txtIndentNo.ClientID%>').value = IndentNo;

                    $get('<%=hdnSelectedItemsFromOPPrescription.ClientID%>').value = SelectedItems;

                }
                $get('<%=btnPrescriptionDetails.ClientID%>').click();
            }

            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;
                    var FacilityId = arg.FacilityId;

                    $get('<%=txtRegistrationNo.ClientID%>').value = RegistrationNo;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnEncounterId.ClientID%>').value = EncounterId;
                    $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;
                    $get('<%=hdnFacilityId.ClientID%>').value = FacilityId;
                    $get('<%=txtEncounter.ClientID%>').value = EncounterNo;

                }
                $get('<%=btnSearchPatient.ClientID%>').click();
            }

            function SearchPatientWardOnClientClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IndentId = arg.IndentId;
                    var IndentNo = arg.IndentNo;
                    var RegistrationId = arg.RegistrationId;
                    var EncounterId = arg.EncounterId;
                    var EncounterNo = arg.EncounterNo;
                    var RegistrationNo = arg.RegistrationNo;
                    var FacilityId = arg.FacilityId;

                    $get('<%=hdnIndentId.ClientID%>').value = IndentId;
                    $get('<%=hdnIndentNo.ClientID%>').value = IndentNo;
                    $get('<%=hdnEncounterId.ClientID%>').value = EncounterId;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;
                    $get('<%=txtRegistrationNo.ClientID%>').value = RegistrationNo;
                    $get('<%=txtEncounter.ClientID%>').value = EncounterNo;
                    $get('<%=hdnFacilityId.ClientID%>').value = FacilityId;
                }
                $get('<%=btnSearchPatientWard.ClientID%>').click();
            }

            function RequestedWardItemsOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlString;
                    var TotalQty = arg.TotalQty;

                    $get('<%=hdnxmlString.ClientID%>').value = xmlString;
                    $get('<%=hdnIssueQty.ClientID%>').value = TotalQty;
                }
                $get('<%=btnRequestedWardItems.ClientID%>').click();
            }
            function AmountCalculated_onkeyup(e) {
                var netamt = Number(0);
                var Collectamt = Number(0);
                var Rfundamt = Number(0);
                var PatientType = $get('<%=ddlPatientType.ClientID%>').value;

                if (PatientType == 'CREDIT SALE') {
                    netamt = $get('<%=txtReceived.ClientID%>').value;
                }
                else {
                    netamt = $get('<%=txtLAmt.ClientID%>').value;

                }
                Collectamt = $get('<%=txtAmountCollected.ClientID%>').value;

                if (Collectamt == '') {
                    $get('<%=txtRefundamount.ClientID%>').value = '';
                }
                else {
                    Rfundamt = (Collectamt * 1) - (netamt * 1);
                    $get('<%=txtRefundamount.ClientID%>').value = Rfundamt.toFixed(2);

                }
            }
            function wndDiscount_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlDiscString;
                    $get('<%=hdndiscountxml.ClientID%>').value = xmlString;
//                    alert(xmlString);
                    var xmlString1 = arg.xmlDiscStringItemWise;
                    $get('<%=hdndiscountxmlItemWise.ClientID%>').value = xmlString1;
//                    alert(xmlString1);
                }
                $get('<%=btngetdiscount.ClientID%>').click();
            }
            function OnClientIsValidPasswordClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                }
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }
            function CheckRegCondition(objregno) {
                var regno = Number(0);
                regno = $get('<%=txtRegistrationNo.ClientID%>').value;
                if (regno > 0) {
                    $get('<%= txtPatientName.ClientID%>').value = '';
                    $get('<%= txtPatientName.ClientID%>').disabled = true;

                }
                else {
                    $get('<%=txtRegistrationNo.ClientID%>').value = '';
                    $get('<%= txtPatientName.ClientID%>').value = '';
                    var saletype = $get('<%= hdnSaleType.ClientID%>').value;
                    if (saletype = 'CASH SALE') {
                        $get('<%= txtPatientName.ClientID%>').disabled = false;
                    }
                    else {
                        $get('<%= txtPatientName.ClientID%>').disabled = true;
                    }
                }
            }
            function CheckEmpCondition() {
                $get('<%=txtRegistrationNo.ClientID%>').value = '';
            }
            function BindInsuranceOnclose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var payerId = arg.payerId;
                    var SponsorId = arg.SponsorId;
                    var CardId = arg.CardId;
                    var CardValidDate = arg.CardValidDate;
                    var OPCreditLimit = arg.OPCreditLimit;
                    var OPCopayMaxlimit = arg.OPCopayMaxlimit;
                    var PharmacyCreditlimit = arg.PharmacyCreditlimit;
                    var PharmacyOpCoPay = arg.PharmacyOpCoPay;
                    var PharmacyIpCoPay = arg.PharmacyIpCoPay;
                    if (payerId != '') {
                        $get('<%=hdnPayer.ClientID%>').value = payerId;
                        $get('<%=hdnSponsor.ClientID%>').value = SponsorId;
                        $get('<%=hdnCardId.ClientID%>').value = CardId;
                        $get('<%=hdnCardValidDate.ClientID%>').value = CardValidDate;
                        $get('<%=hdnOPCreditLimit.ClientID%>').value = OPCreditLimit;
                        $get('<%=hdnOPCopayMaxlimit.ClientID%>').value = OPCopayMaxlimit;
                        $get('<%=hdnPharmacyCreditlimit.ClientID%>').value = PharmacyCreditlimit;
                        $get('<%=hdnphrOPCopay.ClientID%>').value = PharmacyOpCoPay;
                        $get('<%=hdnphrIPCopay.ClientID%>').value = PharmacyIpCoPay;
                    }
                    document.getElementById('<%=btnFillInsurance.ClientID%>').click();

                }
            }
            function OnClientIsValidClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidRegistrationId = arg.IsValidPassword;
                    var IsBtnstatus = arg.BtnClose
                    
                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidRegistrationId;
                    $get('<%=hdnbtnStatus.ClientID%>').value = IsBtnstatus;
                }
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }
        </script>

    </telerik:RadCodeBlock>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div>
                <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSaveData" />
                        <asp:AsyncPostBackTrigger ControlID="btnBindGridWithXml" />
                    </Triggers>
                    <ContentTemplate>--%>
                        
                                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" />
                              

                <div class="container-fluid header_main"  style="margin-bottom:0; padding-bottom:0px;">
                     <div class="col-md-1 PaddingRightSpacing">
                      <h2>
                          <span id="tdHeader"  runat="server"><asp:Label ID="lblHeader" runat="server" SkinID="label" Text="" ToolTip="" /></span>
                          
                      </h2>
                     </div>
				 
                     <div class="col-md-5"> 
                         <asp:Button ID="btnFindPatient" runat="server" CausesValidation="false" OnClick="btnFindPatient_OnClick"
                                        cssClass="btn btn-primary" Text="Patient Search" Visible="false" />
                          <asp:Label ID="lblStore" runat="server" Text="Store" SkinID="label" />
                              <telerik:RadComboBox ID="ddlStore" Width="50%"  runat="server" AutoPostBack="true"
                                                                MarkFirstMatch="true" EmptyMessage="[ Select ]" DropDownWidth="350px"   EnableLoadOnDemand="true"/>
                                    <asp:Button ID="btnRequestFromWard" runat="server" CausesValidation="false" OnClick="btnRequestFromWard_OnClick"
                                        cssClass="btn btn-primary" Text="Request From Ward/ED" />
                                    <asp:Button ID="btnPendingRequestFromWard" runat="server" OnClick="btnPrintRequest_OnClick"
                                        cssClass="btn btn-primary" Text="Pending Request " />
                                    <asp:Button ID="btnGetInfo" runat="server" Text="GetInfo" CausesValidation="false"
                                        SkinID="button" Style="visibility: hidden; display:none" OnClick="btnGetInfo_Click" Width="1px"
                                        Height="1px"  cssClass="hide" />
                                    <asp:Button ID="btnCopayHidden" runat="server" Text="GetInfo" CausesValidation="false"
                                        SkinID="button" Style="visibility: hidden; display:none" OnClick="btnCopayHidden_Click" Width="1px"
                                        Height="1px" cssClass="hide" />
                                    <%--<asp:Button ID="btnGetBalance" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                        OnClick="btnGetBalance_OnClick" Text="" Width="1px" Height="1px" />--%>
                                    <asp:Button ID="btnFindItem" runat="server" Text="GetInfo" CausesValidation="false"
                                        SkinID="button" Style="visibility: hidden; display:none" OnClick="btnFindItem_Click" Width="1px"
                                        Height="1px"  cssClass="hide" />
                                    <asp:TextBox ID="txtRegNo" runat="server" Style="visibility: hidden; display:none" Width="1px"
                                        Height="1px" CssClass="hide" ></asp:TextBox>
                                    <asp:TextBox ID="txtRegId" runat="server" Style="visibility: hidden; display:none" Width="1px"
                                        Height="1px" cssClass="hide" ></asp:TextBox>
                                    <asp:TextBox ID="txt_hdn_PName" runat="server" Style="visibility: hidden; display:none" Width="1px"
                                        Height="1px" cssClass="hide" ></asp:TextBox>
                                    <asp:Button ID="btnBindGridWithXml" runat="server" CausesValidation="false" Style="visibility: hidden; display:none"
                                        OnClick="btnBindGridWithXml_OnClick" Text="" Width="1px" CssClass="hide" />
                                    <asp:Button ID="btnPreviousDate" runat="server" CausesValidation="false" Style="visibility: hidden; display:none"
                                        OnClick="btnPreviousDate_OnClick" Text="" Width="1px"  CssClass="hide"/>
                                    <asp:Button ID="btnGetSaleDetails" runat="server" CausesValidation="false" Style="visibility: hidden; display:none"
                                        OnClick="btnGetSaleDetails_OnClick" Text="" Width="1px" CssClass="hide" />
                                    <asp:Button ID="btngetdiscount" runat="server" CausesValidation="false" Style="visibility: hidden; display:none"
                                        OnClick="btngetdiscount_OnClick" Text="" Width="1px"  CssClass="hide" />
                        
                         
                          <asp:Panel ID="panel6" runat="server" ScrollBars="None" DefaultButton="btnPrescriptionDetails" Width="200px" CssClass="pull-right">
                                        <span id="tblPrescription" runat="server" visible="false">
                                            
                                                    <asp:LinkButton ID="lnkBtnPrescription" runat="server" ToolTip="Open Search Window of Prescription"
                                                        Text="Presc.&nbsp;No" OnClick="lnkBtnPrescription_Click" />
                                               
                                                    <asp:TextBox ID="txtIndentNo" SkinID="textbox" runat="server" Width="70px" MaxLength="20"
                                                        ReadOnly="false" />
                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtIndentNo" ValidChars="0123456789" />
                                                
                                                    <asp:Button ID="btnPrescriptionDetails" runat="server" OnClick="btnPrescriptionDetails_OnClick"
                                                        Width="5px" Style="visibility: hidden; display:none;" />
                                                
                                        </span>
                                    </asp:Panel>
                         
                     </div>
                 
                     <div class="col-md-6 text-right pull-right"> 
                         <telerik:RadComboBox ID="ddlPatientType" runat="server" HighlightTemplatedItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPatientType_SelectedIndexChanged"
                                        Width="100px" CssClass="">
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <%# DataBinder.Eval(Container, "Text")%>
                                                    </td>
                                                    <td visible="false">
                                                        <%# DataBinder.Eval(Container, "Attributes['SetupTypeCode']")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                          <asp:Button ID="btnBounce" runat="server" Text="Bounce Item" cssClass="btn btn-primary" OnClick="btnBounce_OnClick" Visible="false" />
                                    <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" cssClass="btn btn-primary"
                                        Text="New ( F2 )" OnClick="btnNew_OnClick" />
                                    
                                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                        cssClass="btn btn-primary" ValidationGroup="SaveData" Text="Save ( F9 )" />
                                    
                                    

                                    <asp:Button ID="btnPrint" runat="server" ToolTip="Print Data" OnClick="btnPrint_OnClick"
                                        cssClass="btn btn-primary" CausesValidation="false" Text="Print" />
                                    <asp:Button ID="btnLabelPrint" runat="server" cssClass="btn btn-primary" Text="Print Label"
                                        OnClick="lnkLabelPrint_OnClick" Visible="false" />
                                    <asp:Button ID="btnBarcodePrinting" runat="server" cssClass="btn btn-primary" Text="Barcode Print" ToolTip="Click to print Issue wise barcode"
                                        OnClick="btnBarcodePrinting_OnClick" Visible="false"/> 
                                    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                        Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                                    <asp:Button ID="lnkPatientHistory" runat="server" cssClass="btn btn-primary" OnClick="lnkPatientHistory_OnClick"
                                        Text="History" Visible="false"></asp:Button>
                                    <asp:Button ID="btnViewOrders" runat="server" cssClass="btn btn-primary" Text="View Orders" OnClick="btnViewOrders_Click" Visible="false"></asp:Button>
                         <AJAX:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" ConfirmOnFormSubmit="true"
                                        ConfirmText="Are you sure that you want to save ? " TargetControlID="btnSaveData">
                                    </AJAX:ConfirmButtonExtender>
                         <AJAX:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmOnFormSubmit="true"
                                        ConfirmText="Are you sure that you want to create new ? " TargetControlID="btnNew">
                                    </AJAX:ConfirmButtonExtender>
                       <div style="width:110px; float:right">  <asp:CheckBox ID="chktoprinter" runat="server" Text="Direct to Printer" Checked="true" /></div>

                     </div>
                </div>

                <div class="container-fluid header_main" style="padding-top:0px;">
                    <div class="col-md-1"><h2><asp:Label ID="lblEmployee" runat="server" SkinID="label" Text="Employee"  Visible="false" /></h2></div>
                    <div class="col-md-3"> <ucl:ComboEmployeeSearch ID="ComboEmployeeSearch" runat="server" Visible="false" /></div>
                </div>
               
                                

                                            <div class="container-fluid subheading_main">
                                                <div class="col-md-4">
                                                    
                                                    

                                                    <span id="tdPdetail" runat="server">
                                                <asp:Panel ID="Panel1" runat="server">
                                                    
                                                    
                                                    
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"><span id="tdRegNo" runat="server">
                                                                <asp:Label ID="lbtnSearchPatient" runat="server" Text="<%$ Resources:PRegistration, Regno%>"></asp:Label>
                                                                <asp:Label ID="lblShowStar" Style="color: Red;" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                            </span></div>
                                                                <div class="col-md-8"><span id="tdPname" runat="server">
                                                                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByUHID" ScrollBars="None">
                                                                                <asp:TextBox ID="txtRegistrationNo" onblur="CheckRegCondition(this)" runat="server"
                                                                                    MaxLength="13" SkinID="textbox" Width="100%" onkeyup="return validateMaxLength();" />
                                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    FilterType="Custom" TargetControlID="txtRegistrationNo" ValidChars="0123456789" />
                                                                            </asp:Panel>
                                                                            <asp:Panel ID="Panel5" runat="server"  ScrollBars="None">
                                                                                <asp:TextBox ID="txtEmpNo" Visible="false" SkinID="textbox" runat="server" Width="60px"
                                                                                    MaxLength="10" />
                                                                                <%--<AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                    FilterType="Custom" TargetControlID="txtEmpNo" ValidChars="0123456789" />--%>
                                                                            </asp:Panel>
                                                                        </span></div>
                                                            </div>
                                                        </div>
                                                    </div>



                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"><asp:Label ID="lblEncNo" runat="server" SkinID="label" Text='<%$ Resources:PRegistration,EncounterNo %>' /></div>
                                                                <div class="col-md-8"><asp:Panel ID="Panel4" runat="server" DefaultButton="btnSearchByEncounterNo" ScrollBars="None">
                                                                                <asp:TextBox ID="txtEncounter" SkinID="textbox" runat="server" Width="100%" MaxLength="15" />
                                                                            </asp:Panel>
                                                                    <asp:Label ID="lblPackagePatient" runat="server" SkinID="label" Text="Package Patient" BackColor="Yellow" Visible="false"/>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> <asp:Label ID="Label2" runat="server" SkinID="label" Text="Name" />
                                                                <span style="color: Red">*</span></div>
                                                                <div class="col-md-8"><asp:TextBox ID="txtPatientName" onkeypress="CheckEmpCondition()" Enabled="false"
                                                                    Style='text-transform: uppercase' SkinID="textbox" runat="server" Width="100%"
                                                                    MaxLength="100" /></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> <asp:Label ID="lblAddress" runat="server" Text="Address" SkinID="label" /></div>
                                                                <div class="col-md-8">
                                    
                                                                            <asp:TextBox ID="txtaddress" runat="server" placeholder="Enter Address" TextMode="MultiLine" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> <asp:Label ID="lblmobile" runat="server" Text="Mobile" SkinID="label" /></div>
                                                                <div class="col-md-8">
                        
                                                                            <asp:TextBox ID="txtMobile" runat="server" placeholder="Mobile Number eg(8978589685)"  />
                                                                   <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                    FilterType="Custom" TargetControlID="txtMobile" ValidChars="0123456789" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     
                                                    <%--Rahul's Change--%>
                                                    
                                                     <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row" runat="server" id="rwIndent" visible="false">
                                                                <div class="col-md-4"> <asp:Label ID="lblIndent" runat="server" Text="Indent No." SkinID="label" /></div>
                                                                <div class="col-md-8">
                        
                                                                          <asp:Label ID="lblIndentNo" runat="server" Text="" SkinID="label" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-8">
                        
                                                                     <asp:CheckBox ID="chkCashOutStanding" runat="server" Text="Cash sale Outstanding" Visible="false" AutoPostBack="true" OnCheckedChanged="chkCashOutStanding_OnCheckedChanged" />
                                                                  
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                
                                                    
                                                </asp:Panel>
                                            </span>

                                                </div>
                                                <div class="col-md-4">

                                                <asp:Panel ID="Panel2" runat="server" ScrollBars="None">
                                                   
                                                    
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                             <asp:Panel ID="panel11" runat="server" ScrollBars="None" DefaultButton="btnFindDoc"
                                                                    Width="100%">
                                                             <div class="row"  runat="server" id="trDocDetails">
                                                                <div class="col-md-4"><asp:Label ID="lblDocNo" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, IssueNo%>' /></div>
                                                                <div class="col-md-8"> <asp:TextBox ID="txtDocNo" SkinID="textbox" runat="server" Width="70%" MaxLength="20"
                                                                                    ReadOnly="false" />
                                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                    FilterType="Custom" TargetControlID="txtDocNo" ValidChars="0123456789/-DCARPHSEdcarphse" />
                                                                                <asp:ImageButton ID="imgBtnSearchDoc" runat="server" ImageUrl="~/Images/Binoculr.ico"
                                                                                    ToolTip="Open Search Window" Width="20px" Height="17px" OnClick="imgBtnSearchDoc_Click" />
                                                                     <asp:Button ID="btnFindDoc" runat="server" OnClick="btnFindDoc_Click" Width="5px"
                                                                                    Style="visibility: hidden;" />
                                                                </div>
                                                            </div>
                                                                     </asp:Panel>
                                                            </div>
                                                    </div>

                                                    <div class="row form-group hide">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> <asp:Label ID="lblBedNo" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, bedno%>' /></div>
                                                                <div class="col-md-8"> <asp:TextBox ID="txtBedNo" runat="server" ReadOnly="true" Width="100%"></asp:TextBox> </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> <asp:Label ID="lblDocDate" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, IssueDate%>' /></div>
                                                                <div class="col-md-8"> <telerik:RadDatePicker ID="dtpDocDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                                                                    DateInput-DateFormat="dd/MM/yyyy" Enabled="false" /></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    
                                                    <div class="row form-group hide">
                                                        
                                                         <asp:Button ID="btnSearchPatient" runat="server" CausesValidation="false" OnClick="btnSearchPatient_OnClick"
                                                                    cssClass="btn btn-primary" Style="visibility: hidden;" Text="SearchPatient" Width="1px" />
                                                                <asp:Button ID="btnSearchPatientWard" runat="server" CausesValidation="false" OnClick="btnSearchPatientWard_OnClick"
                                                                    cssClass="btn btn-primary" Style="visibility: hidden;" Text="SearchPatient" Width="1px" />
                                                                <asp:Button ID="btnRequestedWardItems" runat="server" CausesValidation="false" OnClick="btnRequestedWardItems_OnClick"
                                                                    cssClass="btn btn-primary" Style="visibility: hidden;" Text="SearchPatient" Width="1px" />
                                                                <asp:Button ID="btnSearchByUHID" runat="server" CausesValidation="false" OnClick="btnSearchByUHID_OnClick"
                                                                    cssClass="btn btn-primary" Style="visibility: hidden;" Text="Filter" Width="1px" />
                                                                <asp:Button ID="btnSearchByEncounterNo" runat="server" CausesValidation="false" OnClick="btnSearchByEncounterNo_OnClick"
                                                                    cssClass="btn btn-primary" Style="visibility: hidden;" Text="Filter" Width="1px" />
                                                                <asp:Button ID="btnSearchByEmpNo" runat="server" CausesValidation="false" OnClick="btnSearchByEmpNo_OnClick"
                                                                    cssClass="btn btn-primary" Style="visibility: hidden;" Text="Filter" Width="1px" />
                                                    </div>
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> <asp:Label ID="Label11" runat="server" Text="Height&nbsp;(Cm)" SkinID="label" /></div>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox ID="txtHeight" runat="server" ReadOnly="true"  Width="32%" />
                                                                       
                                                                            <asp:TextBox ID="lbl_Weight" runat="server" ReadOnly="true"   placeholder="Weight (Kg)" Width="32%" />
                                                                       
                                                                            <asp:TextBox ID="lbl_BSA" runat="server" ReadOnly="true"  placeholder="BSA" Width="32%" />

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"><span id="Span2" runat="server">
                                                                    <asp:Label ID="lblAgeGender" runat="server" SkinID="label" Text="Age/Gender"></asp:Label>
                                                                    <asp:Label ID="lblAgeStar" Style="color: Red;" runat="server" Visible="false" Text="*" ForeColor="Red"></asp:Label>
                                                                </span></div>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox ID="txtAge"  runat="server" Width="33%" MaxLength="3" />
                                                                <telerik:RadComboBox ID="ddlAgeType" runat="server" Width="30%" EmptyMessage="[ Select ]">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="0"  />
                                                                        <telerik:RadComboBoxItem Text="Year" Value="Y" Selected="true" />
                                                                        <telerik:RadComboBoxItem Text="Month" Value="M" />
                                                                        <telerik:RadComboBoxItem Text="Day(s)" Value="D" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <AJAX:FilteredTextBoxExtender ID="txtAge_fxb" runat="server" Enabled="True" FilterType="Custom"
                                                                    TargetControlID="txtAge" ValidChars="0123456789" />
                                                                <telerik:RadComboBox ID="ddlGender" runat="server" Width="33%" EnableLoadOnDemand="false"
                                                                    MarkFirstMatch="true" EmptyMessage="[ Select ]">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="0" Selected="true" />
                                                                        <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, MALE%>" Value="2" />
                                                                        <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, FEMALE%>" Value="1" />
                                                                        <telerik:RadComboBoxItem Text="Other" Value="3" />
                                                                        <telerik:RadComboBoxItem Text="Unknown" Value="4" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"> 
                                                                    <asp:CheckBox ID="chkDoctor" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="chkDoctor_OnCheckedChanged" />
                                                                <asp:Label ID="lblProvider" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, Doctor %>' />
                                                                <asp:Label ID="lblForIp" runat="server" Visible="false" SkinID="label" Text="*" ForeColor="Red" /></div>
                                                                <div class="col-md-8"><asp:TextBox ID="txtProvider" runat="server" SkinID="textbox" MaxLength="100" Visible="false"
                                                                    Width="100%"></asp:TextBox>
                                                                <%--<telerik:RadComboBox ID="ddlDoctor" MarkFirstMatch="true" runat="server" SkinID="DropDown"
                                                                    AllowCustomText="true" MaxLength="50" TabIndex="1" Width="100%">
                                                                </telerik:RadComboBox>--%>

                                                                <telerik:RadComboBox ID="ddlDoctor" Width="100%"  runat="server" AutoPostBack="true"
                                                                MarkFirstMatch="true" EmptyMessage="[ Select ]" DropDownWidth="350px"   EnableLoadOnDemand="true"/>
                                                                </div>

                                                                

                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-4"><asp:Label ID="Label13" runat="server" SkinID="label" Text="Bar Code Value&nbsp;" CssClass="pull-left" /></div>
                                                                <div class="col-md-8"><asp:Panel ID="Panel7" runat="server" DefaultButton="btnBarCodeValue">
                                                                    <asp:TextBox ID="txtBarCodeValue" runat="server"  CssClass="pull-left" Width="100%" MaxLength="200"/>
                                                                </asp:Panel>
                                                                <asp:Button ID="btnCopayRefresh" runat="server" Text="F" cssClass="btn btn-primary" OnClick="btnCopayRefresh_OnClick"
                                                                    Style="visibility: hidden; display:none" Width="1px" />
                                                                <asp:Button ID="btnBarCodeValue" runat="server" Text="" cssClass="btn btn-primary" OnClick="btnBarCodeValue_OnClick"
                                                                    Style="visibility: hidden; display:none" /></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    
                                                    

                                                </asp:Panel>
                                           
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Panel ID="Panel3" runat="server" ScrollBars="None">
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-5">
                                                                    <span id="trCreditDetail" style="vertical-align: middle;" runat="server">
                                                                <asp:Label ID="lblPayor" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, company %>' />
                                                            </span>
                                                                </div>
                                                                <div class="col-md-7"> <asp:Label ID="lblPayer" Font-Bold="true" runat="server" SkinID="label" Text="" />
                                                                <telerik:RadComboBox ID="ddlPayer" AutoPostBack="true" OnSelectedIndexChanged="ddlPayer_OnSelectedIndexChanged"
                                                                    runat="server" Filter="Contains" Visible="False" MarkFirstMatch="true" Width="100%">
                                                                </telerik:RadComboBox></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-5"><asp:Label ID="Label4" Width="100px" runat="server" SkinID="label" Text="Patient Type" /></div>
                                                                <div class="col-md-7"> <asp:Label ID="lblPayertype" Font-Bold="true" runat="server" SkinID="label" Text="" /></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-5"> <asp:Label ID="Label3" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, remarks %>' />
                                                                    <asp:Label ID="lblRemarkS" runat="server" Visible="false" SkinID="label" Text="*" ForeColor="Red" /></div>
                                                                <div class="col-md-7">   <asp:TextBox ID="txtRemark" runat="server"  onkeyup="return AutoChange();" /></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-5"><asp:Label ID="Label5" runat="server" SkinID="label" Text="Provisional Diagnosis" CssClass="pull-left" /></div>
                                                                <div class="col-md-7"><asp:TextBox ID="txtProvisionalDiagnosis" runat="server" SkinID="textbox" MaxLength="1000"
                                                                     Width="100%"  onkeyup="return MaxLenTxt(this, 1000);" /></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                        <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-5"><asp:Label ID="lblFollowupDate" runat="server" SkinID="label" Text="Followup Date" CssClass="pull-left" /></div>
                                                                <div class="col-md-7"> <telerik:RadDatePicker ID="dtpFollowupDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                                                                    DateInput-DateFormat="dd/MM/yyyy" /></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                               <div class="row form-group">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-5"> <asp:Label ID="Label12" runat="server" SkinID="label" Text="Treatment/Available Limit" CssClass="pull-left" /></div>
                                                        <div class="col-md-7"><asp:Label ID="lblTreatLimit" runat="server" Visible="true" Text="0.00"></asp:Label>&nbsp;/&nbsp;<asp:Label ID="lblAvailLimit" runat="server" Visible="true" Text="0.00" ></asp:Label></div>
                                                    </div>
                                                </div>
                                            </div> 
                                             <div class="row form-group">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-5"> <asp:Label ID="Label16" runat="server" SkinID="label" Text="Ward/Bed Name" CssClass="pull-left" /></div>
                                                        <div class="col-md-7"><asp:Label ID="lblWardName" runat="server" Visible="true" Text="" ForeColor="Green" ></asp:Label>&nbsp;/&nbsp;<asp:Label ID="lblBedNo1" runat="server" Visible="true" Text="" ForeColor="Green"></asp:Label></div>
                                                    </div>
                                                </div>
                                            </div> 
                                                   

                                                </asp:Panel>
                                           
                                                </div>
                                            </div>
                                            
                 <span id="trpatientype" runat="server">
                   <asp:TextBox ID="txtpatientaatributes" runat="server" Width="10px" SkinID="textbox" Style="visibility: hidden; display:none" Height="5px"></asp:TextBox>
                </span>
                                            
                    <div class="container-fluid">
                        <div class="row form-group">
                            <div class="col-md-2">
                                <asp:Button ID="btnAddNewItem" runat="server" Text="Add New Item (F4)" cssClass="btn btn-primary" CausesValidation="false" OnClick="btnAddNewItem_OnClick" />
                                 <asp:Button ID="btndDiscount" runat="server" Text="Discount" cssClass="btn btn-primary" CausesValidation="false" OnClick="btndDiscount_OnClick"/>
                            </div>

                            <div class="col-md-5">
                                <div class="row" id="BLKDiscount" runat="server">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5 PaddingRightSpacing"><asp:Label ID="lblDiscountType" runat="server" SkinID="label" Text="Discount Type&nbsp;" /></div>
                                            <div class="col-md-7 PaddingLeftSpacing"><telerik:RadComboBox ID="ddlDiscountType" MarkFirstMatch="true" runat="server" SkinID="DropDown" DropDownWidth="150px" MaxLength="50" TabIndex="1" Width="100%" OnSelectedIndexChanged="ddlDiscountType_SelectedIndexChanged" AutoPostBack="true" ></telerik:RadComboBox></div>
                                            <div class="col-md-5 PaddingRightSpacing"><asp:Label ID="lblDiscPerc" runat="server" SkinID="label" Text="Disc.(%)&nbsp;" /></div>
                                            <div class="col-md-7 PaddingLeftSpacing"><asp:TextBox ID="txtDiscPerc" runat="server" Width="50px" MaxLength="3" /></div>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                        TargetControlID="txtDiscPerc" FilterType="Custom, LowercaseLetters , UppercaseLetters"
                                                        ValidChars="1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Custom,Numbers"
                                ValidChars="1234567890" TargetControlID="txtDiscPerc">
                            </AJAX:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDiscPerc"
                                ErrorMessage="Enter Discount" Display="None" ValidationGroup="s"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDiscount" runat="server" ControlToValidate="txtDiscPerc"
                                MaximumValue="100" MinimumValue="0" Type="Currency" ErrorMessage="Enter Discount Between 0 - 100"
                                Display="None" ValidationGroup="s"></asp:RangeValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                             <div class="col-md-4 PaddingRightSpacing"><asp:Label ID="lblEmpNo" Text="Emp. No:" runat="server" Visible="false" /></div>
                                             <div class="col-md-8 PaddingRightSpacing">
                                                 <div class="row">
                                                     <div class="col-md-8 PaddingRightSpacing"><asp:TextBox ID="txtBLKId" runat="server" Width="100%" MaxLength="200" Visible="false" style="text-transform:uppercase" /></div>
                                                     <div class="col-md-4"><asp:Button ID="btnDiscountApply" runat="server" Text="Apply Disc." cssClass="btn btn-primary" OnClick="btngetdiscount1_OnClick"/></div>
                                                 </div>
                                             </div>
                                         </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <asp:Panel ID="pnlCopayment" runat="server" Visible="false">
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-5"><asp:Button ID="btnCopay" runat="server" Text="Co-Pay" cssClass="btn btn-primary" OnClick="btnCoPay_OnClick" /></div>
                                                <div class="col-md-7"><asp:LinkButton ID="lnkInsuranceDetails" runat="server" Visible="false" OnClick="lnkInsuranceDetails_Click">Insurance&nbsp;Details</asp:LinkButton></div>
                                            </div>
                                              
                                            
                                        </div>

                                        <div class="col-md-5">
                                            <div class="row">
                                                <div class="col-md-6"><asp:Label ID="lblApprovalCode" runat="server" Text="Approval Code"></asp:Label>
                                                <asp:Label ID="lblApprovalCodeS" runat="server" Visible="false" SkinID="label" Text="*" ForeColor="Red" /></div>
                                                <div class="col-md-6"><asp:TextBox ID="txtApprovalCode" runat="server" SkinID="textbox"></asp:TextBox></div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>




                            <div class="col-md-2 text-center" runat="server" id="Divphrclear">
                                <span id="tblPendingPharmacyClearance" runat="server" >
                                    <asp:LinkButton ID="btnPendingPharmacyClearance" runat="server" Text="Pharmacy Clearance" onclick="btnPendingPharmacyClearance_Click"/>
                                    <asp:Label ID="lblPharmacyClearance" Font-Size="11px" runat="server" ForeColor="Red" />
                                </span>
                            </div>


                            <div id="idMarkForDischarge" runat="server" class="col-md-3">
                                <div class="row">
                                    <div class="col-md-6 PaddingSpacing" id="tblMarkedForDischarge" runat="server" visible="false">
                                        <asp:LinkButton ID="btnMarkedForDischarge" runat="server" Text="Marked For Discharge" OnClick="btnMarkedForDischarge_OnClick" />
                                        <asp:Label ID="lblNoOfDischarge" Font-Size="11px" runat="server" ForeColor="Red" />
                                    </div>
                                    <div class="col-md-6" id="tdDrugAllergy" runat="server">
                                        <asp:LinkButton ID="lnkDrugAllergy" runat="server" OnClick="lnkDrugAllergy_OnClick" BackColor="#82CAFA" Text="Drug&nbsp;Allergy" ToolTip="Drug&nbsp;Allergy" Font-Bold="true" Visible="false" />
                                        <asp:Button ID="btnSurgicalKit" runat="server" Text="Item Kits" cssClass="btn btn-primary" CausesValidation="false" OnClick="btnSurgicalKit_OnClick" Visible="false" />
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>






                         <div class="container-fluid">
                             <div class="row form-group">
                                 
                               
                                 
                                 

                                 

                              



                               

                                
                             </div>
                             <div class="row">
                               
                                   
                                  
                             </div>
                         </div>
                
                           
                        
                        
                       
                          
                        
                    
                <div id="tabPaymodedetails" runat="server" class="container-fluid">
                    <div class="row">
                                    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Office2007" SelectedIndex="0"
                                        MultiPageID="RadMultiPage1" Height="100%">
                                        <Tabs>
                                            <telerik:RadTab Text="Item Details" ToolTip="Item Details">
                                            </telerik:RadTab>
                                            <telerik:RadTab Text="Payment" ToolTip="Payment Details">
                                            </telerik:RadTab>
                                        </Tabs>
                                    </telerik:RadTabStrip>
                                    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="250px"
                                        ScrollBars="Auto" Width="100%" Style="background: #e0ebfd; border-width: 1px">
                                        <telerik:RadPageView ID="rpvItem" runat="server" Style="background: #e0ebfd; border-width: 1px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="background: #e0ebfd; border-width: 1px"
                                                width="100%">
                                                <tr>
                                                    <td id="tdGrid" runat="server" valign="top" style="width: 100%;">
                                                        <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                                            AlternatingRowStyle-BackColor="#E6E6FA" SkinID="gridview2" ShowFooter="true"
                                                            Width="100%" OnRowDataBound="gvService_OnRowDataBound" OnRowCommand="gvService_RowCommand">
                                                            <EmptyDataTemplate>
                                                                <div style="font-weight: bold; color: Red;">
                                                                    No Record Found.</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:PRegistration, ItemName%>" HeaderStyle-Width="160px" ItemStyle-Width="150px"
                                                                    ItemStyle-Wrap="true">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkItemName" runat="server" Width="100%" Text='<%#Eval("ItemName")%>'
                                                                            ToolTip='<%#Eval("ItemName")%>' CommandName="ItemSelect" />
                                                                        <asp:HiddenField ID="hdnPrescriptionDetailsId" runat="server" Value='<%#Eval("PrescriptionDetailsId")%>' />
                                                                        <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%#Eval("GenericId")%>' />
                                                                        <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                                        <asp:HiddenField ID="hdnBatchXML" runat="server" Value='<%#Eval("BatchXML")%>' />
                                                                        <asp:HiddenField ID="hdnCopayPerc" runat="server" Value='<%#Eval("CopayPerc")%>' />
                                                                        <asp:HiddenField ID="hdnCopayAmt" runat="server" Value='<%#Eval("CopayAmt")%>' />
                                                                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                                        <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                                        <asp:HiddenField ID="hdnISDHAApproved" runat="server" Value='<%#Eval("ISDHAApproved") %>' />
                                                                        <asp:HiddenField ID="hdnDenialCode" runat="server" Value='<%#Eval("DenialCode") %>' />
                                                                        <asp:HiddenField ID="hdnReusable" runat="server" Value='<%#Eval("Reusable") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, BatchNo %>' HeaderStyle-Width="100px"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnBatchId" runat="server" Value='<%#Eval("BatchId")%>' />
                                                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo")%>' Style="width: 50%;
                                                                            text-align: left; background-color: Transparent;" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ExpiryDate %>' HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate")%>'></asp:Label></ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <span style="font-family: 'times New Roman', Times, serif; font-size: 12px; font-weight: bold;
                                                                            font-style: normal; color: #003871">Total </span>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Required Qty" HeaderStyle-Width="40px" ItemStyle-Wrap="true"
                                                                    ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRequiredQty" runat="server" SkinID="label" Style="width: 100%;
                                                                            text-align: right; background-color: Transparent;" Text='<%#Eval("RequiredQty")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Issue Qty" HeaderStyle-Width="80px" ItemStyle-Wrap="true"
                                                                    ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnStockQty" runat="server" Value='<%#Eval("StockQty")%>' />
                                                                        <asp:TextBox ID="txtQty" runat="server" SkinID="textbox" Style="width: 100%; text-align: right;
                                                                            background-color: Transparent;" Text='<%#Eval("Qty")%>' MaxLength="5" autocomplete="off" ReadOnly="true"></asp:TextBox>
																			<%-- onkeypress="return CheckDecimal(event, this.value)"--%>
                                                                        <AJAX:FilteredTextBoxExtender ID="FTBEServiceStockQty" runat="server" FilterType="Custom,Numbers"
                                                                            ValidChars="1234567890." TargetControlID="txtQty">
                                                                        </AJAX:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotQty" runat="server" MaxLength="8" CssClass="gridInput" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD;" ForeColor="#003871" Font-Size="8"
                                                                            Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Unit %>' HeaderStyle-Width="40px"
                                                                    ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" />
                                                                        <asp:Label ID="lblItemUnitName" runat="server"></asp:Label></ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SellingPrice %>' HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnCostPrice" runat="server" Value='<%#Eval("CostPrice") %>' />
                                                                        <asp:TextBox ID="txtCharge" runat="server" class="gridInput" Style="width: 100%;
                                                                            text-align: right; background-color: Transparent;" ForeColor="#000062" Text='<%#Eval("MRP")%>'
                                                                            ToolTip='<%#Eval("MRP")%>'></asp:TextBox></ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotCharge" runat="server" CssClass="gridInput" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD; visibility: hidden;" ForeColor="#003871"
                                                                            Font-Size="8" Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, DiscountAmount %>' HeaderStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnPercentDiscount" runat="server" Value='<%#Eval("PercentDiscount")%>' />
                                                                        <asp:TextBox ID="txtDiscountAmt" runat="server" CssClass="gridInputNumber" Enabled="false"
                                                                            Style="width: 100%; text-align: right; background-color: Transparent;" ForeColor="#000062"
                                                                            Text='<%#Eval("DiscAmt")%>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotDiscount" runat="server" CssClass="gridInputNumber" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD;" type="text" ForeColor="#003871"
                                                                            Font-Size="8" Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Tax %>' HeaderStyle-Width="40px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtTax" Enabled="false" runat="server" CssClass="gridInputNumber"
                                                                            ForeColor="#000062" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                                            Text='<%#Eval("TaxAmt")%>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotTax" runat="server" CssClass="gridInput" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD; visibility: hidden;" ForeColor="#003871"
                                                                            Font-Size="8" Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, NetAmount %>' HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtNetAmt" runat="server" CssClass="gridInputNumber" Enabled="false"
                                                                            ForeColor="#000062" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                                            Text='<%#Eval("NetAmt")%>' ToolTip='<%#Eval("NetAmt")%>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotNetamt" runat="server" CssClass="gridInput" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD;" ForeColor="#003871" Font-Size="8"
                                                                            Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, PatientPayable %>' HeaderStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPatient" runat="server" CssClass="gridInputNumber" Style="width: 100%;
                                                                            text-align: right; background-color: Transparent;" ForeColor="#000062" Text='<%#Eval("PatientAmount")%>'
                                                                            ToolTip='<%#Eval("PatientAmount")%>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalPatient" runat="server" CssClass="gridInputNumber" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD;" Enabled="False" ForeColor="#003871"
                                                                            Font-Size="8" Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, PayerPayable %>' HeaderStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPayer" runat="server" CssClass="gridInputNumber" Enabled="false"
                                                                            ForeColor="#000062" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                                            Text='<%#Eval("PayerAmount")%>' ToolTip='<%#Eval("PayerAmount")%>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalPayer" runat="server" CssClass="gridInput" Style="width: 99%;
                                                                            text-align: right; background-color: #E0EBFD;" ForeColor="#003871" Font-Size="8"
                                                                            Font-Bold="True" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                                                            CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                                            Text="Mon." Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                                                            CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                                            Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                                            CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                                            Text="&nbsp;" Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                                                            CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                                            Text="Mon." Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                                                            CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                                            Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                                                            CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                                                            Text="&nbsp;" Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                               <%-- <asp:TemplateField HeaderText="LASA" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        
                                                                        <asp:Label ID="lblLASA_Grid" runat="server" BackColor="Transparent" Text="&nbsp;" Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="High Risk" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        
                                                                        <asp:Label ID="lblHR_Grid" runat="server" BackColor="Transparent" Text="&nbsp;" Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>

                                                                <%--my b--%>
                                                                 <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="100px" ItemStyle-Wrap="true"
                                                                    ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                       
                                                                        <asp:TextBox ID="txtReusableRemarks" runat="server" SkinID="textbox" Style="width: 80%; text-align: right;
                                                                            background-color: Transparent;" Text='<%#Eval("Reusable")%>' Enabled="false" MaxLength="20"></asp:TextBox>
																		 	
                                                                        
                                                                    </ItemTemplate>
                                                                  
                                                                </asp:TemplateField>
                                                             <%--   my e--%>

                                                                <asp:TemplateField HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtndaDelete" runat="server" ToolTip="Click here to delete this row"
                                                                            CommandName="Del" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                                            ImageUrl="~/Images/DeleteRow.png" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RequestedItemId" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRequestedItemId" runat="server" Text='<%#Eval("RequestedItemId")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <RowStyle Wrap="False" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="rpvPayment" runat="server">
                                            <table cellpadding="0px" border="0" style="background: #e0ebfd; border-width: 1px"
                                                width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <ucPaymentMode:PaymentMode ID="paymentMode" runat="server" billsettlementchk="0"/>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadPageView>
                                    </telerik:RadMultiPage>
                        </div>
                    <div class="row">
                            <table border="0" cellpadding="0" cellspacing="1" width="100%" style="background: #4534AC;">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Literal7" Font-Bold="true" runat="server" SkinID="label" Text="Total Amt"
                                            ForeColor="White" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNetAmount" runat="server" Width="80px" ReadOnly="true" Text="0.00"
                                            SkinID="textbox" Style="text-align: right" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="true" SkinID="label" Text="Round Off"
                                            ForeColor="White" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRounding" runat="server" Width="30px" ReadOnly="true" Text="0.00"
                                            SkinID="textbox" Style="text-align: right" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="true" SkinID="label" Text="Net Amt"
                                            ForeColor="White" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLAmt" runat="server" Width="80px" ReadOnly="true" Text="0.00"
                                            SkinID="textbox" Style="text-align: right" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Literal4" runat="server" Font-Bold="true" SkinID="label" Text="Received"
                                            ForeColor="White" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReceived" runat="server" MaxLength="10" Width="80px" Text="0.00"
                                            SkinID="textbox" ReadOnly="true" autocomplete="off" Style="text-align: right" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom,Numbers"
                                            ValidChars="1234567890." TargetControlID="txtReceived">
                                        </AJAX:FilteredTextBoxExtender>
                                    </td>
                                    <td align="right" style="width: 400px">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Cash Collected" SkinID="label"
                                                        ForeColor="White" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmountCollected" runat="server" SkinID="textbox" MaxLength="8"
                                                        Text="0.00" autocomplete="off" onkeyup="AmountCalculated_onkeyup(event)" Width="50px"
                                                        Style="text-align: right" />
                                                    <AJAX:FilteredTextBoxExtender ID="FTBEServiceStockQty" runat="server" FilterType="Custom,Numbers"
                                                        ValidChars="1234567890." TargetControlID="txtAmountCollected" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label22" runat="server" Font-Bold="true" Text="Cash To Be Returned"
                                                        SkinID="label" ForeColor="White" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRefundamount" runat="server" SkinID="textbox" Text="0.00" ReadOnly="true"
                                                        Width="50px" Style="text-align: right" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                </div>
                <table cellpadding="1" cellspacing="1" width="90%">
                    <tr>
                        <td>
                            <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="0"
                                Style="margin-left: 2px">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="Bisque" SkinID="label"
                                            Width="22px" Height="14px" />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="Label21" runat="server" SkinID="label" Text="Generic Prescribed" />
                                    </asp:TableCell></asp:TableRow></asp:Table></td><td>
                            <ucl:legend ID="Legend1" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblColorDHA" runat="server" BorderWidth="1"    SkinID="label" Text="...." BackColor="LightCoral"
                                ForeColor="LightCoral" Width="15px" Height="14px" BorderStyle="Solid" BorderColor="Black" ></asp:Label><asp:Label ID="lbl" runat="server" Text="DHA Approval Rejected"></asp:Label></td><td>
                            <asp:Label ID="lblLASA" runat="server" BorderWidth="1px" Text="...." BackColor="#FCFF0E"    Width="15px" Height="14px" BorderStyle="Solid" BorderColor="Black" CssClass="pull-left let-tp"
                                ForeColor="#FCFF0E"></asp:Label><asp:Label ID="Label15" runat="server" Text="LASA" ></asp:Label></td><td>
                            <asp:Label ID="lblHighRisk" runat="server" BorderWidth="1px" Text="...."   SkinID="label" BackColor="#99ff99" Width="15px" Height="14px" BorderStyle="Solid" BorderColor="Black"
                                ForeColor="#99ff99"></asp:Label><asp:Label ID="Label17" runat="server" Text="High Risk"></asp:Label></td></table><table border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td>
                            <div id="dvInteraction" runat="server" visible="false" style="width: 400px; z-index: 200;
                                border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                                border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
                                bottom: 0; height: 75px; left: 300px; top: 160px">
                                <table width="100%" cellspacing="2" cellpadding="0">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="Label25" Font-Size="12px" runat="server" Font-Bold="true" Text="This drug has interaction with prescribed medicines !" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp; </td></tr><tr>
                                        <td align="center">
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnInteractionView" cssClass="btn btn-primary" runat="server" Text="Interaction View"
                                                OnClick="btnInteractionView_OnClick" />
                                            &nbsp; <asp:Button ID="btnMonographView" cssClass="btn btn-primary" runat="server" Text="Monograph View"
                                                OnClick="btnMonographView_OnClick" />
                                            &nbsp; <asp:Button ID="btnInteractionContinue" cssClass="btn btn-primary" runat="server" Text="Continue"
                                                OnClick="btnInteractionContinue_OnClick" />
                                            &nbsp; <asp:Button ID="btnInteractionCancel" cssClass="btn btn-primary" runat="server" Text="Cancel"
                                                OnClick="btnInteractionCancel_OnClick" />
                                        </td>
                                        <td align="center">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="99%" cellpadding="1" cellspacing="1" style="background: #e0ebfd;">
                    <tr>
                        <td align="left">
                            <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>--%>
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Pin, Move, Close, Maximize, Resize" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                                        Behaviors="Close,Move,Pin,Resize,Maximize">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIndentNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnEncounterDate" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnAgeGender" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnPhoneHome" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnMobileNo" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnPatientName" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnDOB" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnAddress" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCompanyCode" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnSponsorName" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnInsCode" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCardId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnPayerName" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnOrderNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotQty" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotUnit" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotCharge" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotDiscAmt" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotTax" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotNetAmt" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotPatientAmt" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnTotPayerAmt" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnxmlString" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnSelectedGenericId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnSelectedItemId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnIssueQty" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                                    <asp:HiddenField ID="hdnEncId" runat="server" />
                                    <asp:HiddenField ID="hdnIssueId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnFacilityId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnRequestId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                                    <asp:HiddenField ID="hdnPriceType" runat="server" />
                                    <asp:HiddenField ID="hdnItemIssueId" runat="server" />
                                    <asp:HiddenField ID="hdnServiceTaxPerc" runat="server" />
                                    <asp:HiddenField ID="hdnIsReceiptAllow" runat="server" />
                                    <asp:HiddenField ID="hdnDiscountPerc" runat="server" />
                                    <asp:HiddenField ID="hdnItemIds" runat="server" />
                                    <asp:HiddenField ID="hdnAgeType" runat="server" />
                                    <asp:HiddenField ID="hdnPageId" runat="server" />
                                    <asp:HiddenField ID="hdndiscountxml" runat="server" Value="" />
                                    <asp:HiddenField ID="hdndiscountxmlItemWise" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnDiscount" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnAuthorizedId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnNarration" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnMainStoreId" runat="server" Value="10001" />
                                    <asp:HiddenField ID="hdnDefaultCompanyId" runat="server" />
                                    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                    <asp:HiddenField ID="hdnPaymentType" runat="server" />
                                    <asp:HiddenField ID="hdnSaleType" runat="server" />
                                    <asp:HiddenField ID="hdnPayer" runat="server" />
                                    <asp:HiddenField ID="hdnSponsor" runat="server" />
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCardValidDate" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnOPCreditLimit" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnOPCopayMaxlimit" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                                    <asp:HiddenField ID="hdnCIMSType" runat="server" />
                                    <asp:HiddenField ID="hdnVIDALItemId" runat="server" />
                                    <asp:Button ID="btnFillInsurance" runat="server" Enabled="true" OnClick="btnFillInsurance_Click"
                                        SkinID="button" Style="visibility: hidden;" Text="" />
                                    <asp:HiddenField ID="hdnPharmacyCreditlimit" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnphrOPCopay" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnphrIPCopay" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnRoundOffPatient" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnRoundOffPayer" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnERCompanyId" runat="server" />
                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                    <asp:HiddenField ID="hdnIsStripWiseSaleRequired" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdnIsDoctorMandatoryinOPSale" runat="server" Value="N" />
                                    <asp:HiddenField ID="hdnIsAgeMandatoryinOPSale" runat="server" Value="N" />
                                     <asp:HiddenField ID="hdnSaveIssueId" runat="server" Value="N" />
                             <asp:HiddenField ID="hdnSelectedItemsFromOPPrescription" runat="server" Value="" />

                            <asp:HiddenField ID="hdnbtnStatus" runat="server"  Value="Y"/>
                             <asp:HiddenField ID="hdnEmployeeId" runat="server"  />
                                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvConfirm" runat="server" visible="false" style="width: 400px; z-index: 200;
                border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000;
                border-top: 1px solid #000000; background-color: #FFFFCC; position: absolute;
                bottom: 0; height: 75px; left: 300px; top: 150px">
                <table width="100%" cellspacing="2">
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to print ?"></asp:Label></td></tr><tr>
                        <td colspan="3">
                            &nbsp; </td></tr><tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="btnYes" cssClass="btn btn-primary" runat="server" Text="Yes" OnClick="btnYes_OnClick" />
                            &nbsp; <asp:Button ID="btnCancel" cssClass="btn btn-primary" runat="server" Text="No" OnClick="btnCancel_OnClick" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvConfirmProfileItem" runat="server" visible="false" style="width: 500px;
                z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC;
                border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC;
                position: absolute; bottom: 0; height: 75px; left: 300px; top: 180px">
                <table border="0" width="100%" cellspacing="2" cellpadding="0">
                    <tr>
                        <td style="width: 80px">
                            <asp:Label ID="Label6" runat="server" SkinID="label" Text="Profile Item" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlProfileItem" runat="server" Width="400px" EmptyMessage="[ Select ]"
                                MarkFirstMatch="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp; </td></tr><tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="btnOkProfileItem" cssClass="btn btn-primary" runat="server" CausesValidation="false"
                                Text="Ok" OnClick="btnOkProfileItem_OnClick" />
                            &nbsp; <asp:Button ID="btnCancelProfileItem" cssClass="btn btn-primary" CausesValidation="false" runat="server"
                                Text="Cancel" OnClick="btnCancelProfileItem_OnClick" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvPending" runat="server" visible="false" style="width: 400px; z-index: 200;
                border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
                bottom: 0; height: 100px; left: 300px; top: 180px">
                <table border="0" width="100%" cellspacing="2" cellpadding="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="From Date" SkinID="label"></asp:Label></td><td>
                            <telerik:RadDatePicker ID="dtFromDate" runat="server" Width="110px" DateInput-ReadOnly="true"
                                DateInput-DateFormat="dd/MM/yyyy" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="To Date" SkinID="label"></asp:Label></td><td>
                            <telerik:RadDatePicker ID="dtToDate" runat="server" Width="110px" DateInput-ReadOnly="true"
                                DateInput-DateFormat="dd/MM/yyyy" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="btnPendingPreview" cssClass="btn btn-primary" runat="server" Text="Print" OnClick="btnPendingPreview_OnClick" />
                            &nbsp; <asp:Button ID="btnPendingCancel" cssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnPendingCancel_OnClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvMessage" runat="server" visible="false" style="width: 400px; z-index: 200;
                border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000;
                border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute;
                bottom: 0; height: 100px; left: 300px; top: 150px">
                <table width="100%" cellspacing="2">
                    <tr>
                        <td height="70px" valign="top">
                            <asp:DataList ID="dlMissingDocument" runat="server" CssClass="ListBox" Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="lblMissingDocument" runat="server" Text='<%# Eval("Notes") %>'>
                                    </asp:Label><br /></ItemTemplate></asp:DataList></td></tr><tr>
                        <td align="center">
                            <asp:Button ID="btnOk" cssClass="btn btn-primary" runat="server" Text="Ok" OnClick="btnOk_OnClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvLabelPrint" runat="server" visible="false" style="width: 400px; z-index: 200;
                border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000;
                border-top: 1px solid #000000; background-color: #FFFFCC; position: absolute;
                bottom: 0; height: 75px; left: 430px; top: 150px">
                <table width="100%" cellspacing="2">
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblConfirm1" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to print label ?"></asp:Label></td></tr><tr>
                        <td colspan="3">
                            &nbsp; </td></tr><tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="btnLabelPrintYes" cssClass="btn btn-primary" runat="server" Text="Yes" OnClick="btnLabelPrintYes_OnClick" />
                            &nbsp; <asp:Button ID="btnLabelPrintCancel" cssClass="btn btn-primary" runat="server" Text="No" OnClick="btnLabelPrintCancel_OnClick" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divBarcode" runat="server" visible="false" style="width: 400px; z-index: 200;
            border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
            border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
            bottom: 0; height: 75px; left: 450px; top: 150px">
            <table cellspacing="2" style="height: 75px; width: 100%">
                <tr>
                    <td colspan="3" align="center">
                        <asp:Label ID="Label14" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to print barcode ?"></asp:Label></td></tr><tr>
                    <td colspan="3">
                        &nbsp; </td></tr><tr>
                    <td align="center">
                    </td>
                    <td align="center">
                        <asp:Button ID="btnYes1" cssClass="btn btn-primary" runat="server" Text="Yes" OnClick="btnBarcodePrinting_OnClick" />
                        &nbsp; <asp:Button ID="btnCancel1" cssClass="btn btn-primary" runat="server" Text="No" OnClick="btnCancel1_OnClick" CommandArgument="No" />
                    </td>
                    <td align="center">
                    </td>
                </tr>
            </table>
        </div>

            <div id="divWarnigMessage" runat="server" visible="false" style="width: 400px; z-index: 200;
            border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
            border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
            bottom: 0; height: 75px; left: 450px; top: 150px">
            <table cellspacing="2" style="height: 100%; width: 100%">
                <tr>
                    <td colspan="3" align="center">
                        <asp:Label ID="Label18" Font-Size="12px" runat="server" Font-Bold="true" Text="You are trying to open Ward Request from OP Sale. Do you want to continue?"></asp:Label></td></tr><tr>
                    <td colspan="3">
                        &nbsp; </td></tr><tr>
                    <td align="center">
                    </td>
                    <td align="center">
                        <asp:Button ID="btnWarning" cssClass="btn btn-primary" runat="server" Text="Yes" OnClick="btnRequestFromWard_OnClick" />
                        &nbsp; <asp:Button ID="btnWarningCancel" cssClass="btn btn-primary" runat="server" Text="No" OnClick="btnWarningCancel_OnClick" CommandArgument="No" />
                    </td>
                    <td align="center">
                    </td>
                </tr>
            </table>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="UpdatePanel4"
        DisplayAfter="2000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div style="width: 154; position: absolute; bottom: 0; height: 60; left: 550px; top: 270px">
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
