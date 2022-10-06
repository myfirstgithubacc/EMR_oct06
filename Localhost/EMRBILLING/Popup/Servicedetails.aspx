<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Servicedetails.aspx.cs" Inherits="EMRBILLING_Popup_Servicedetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Service Details</title>
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        div#dvZone1{
            overflow-x:hidden!important;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function CalculateAmount(txtServiceAmount, txtDoctorAmount, lblUnit, txtDiscountPer, txtDiscountAmt, lblPayableAmt, hdnServiceId, lblAmountPayableByPatient, lblAmountPayableByPayer) {
            var unit = Number(document.getElementById(lblUnit).innerHTML);
            var x = Number(document.getElementById(txtDiscountPer).value);

            if (!validate(document.getElementById(txtServiceAmount).value)) {
                alert('Service Amount Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDoctorAmount).value)) {
                alert('Doctor Amount Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDiscountPer).value)) {
                alert('Discount Percent Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDiscountAmt).value)) {
                alert('Discount Amount Not Correct. Please Check...');
                return;
            }
            if (document.getElementById(txtServiceAmount).value.lemgth == 1 && document.getElementById(txtServiceAmount).value == '.')
                document.getElementById(txtServiceAmount).value = '0.';
            if (document.getElementById(txtDoctorAmount).value.lemgth == 1 && document.getElementById(txtDoctorAmount).value == '.')
                document.getElementById(txtDoctorAmount).value = '0.';
            var samt = Number(document.getElementById(txtServiceAmount).value);
            var damt = Number(document.getElementById(txtDoctorAmount).value);
            var scharge = Number(samt + damt);
            if (x > 100) {
                alert('Discount should not be greater than 100% !');
                document.getElementById(txtDiscountPer).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountPer).focus();
                return false;
            }
            if ((document.getElementById(txtDiscountAmt).value * 1) > ((scharge * 1) * (unit * 1))) {
                alert('Discount amount should not be greater than actual charge !');
                document.getElementById(txtDiscountAmt).focus();
                return false;
            }

            if (isValidDiscPrec(x) == false) {
                x = 0;
                document.getElementById(txtDiscountPer).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountPer).focus();
            }
            var hdnBilltype = document.getElementById("hdnBilltype");

            var sChargeN = (0 * 1).toFixed(2);
            if (x > 0) {
                document.getElementById(txtDiscountAmt).value = (((x * 1) / 100) * ((unit * 1) * (scharge * 1))).toFixed(2);

                sChargeN = (((((unit * 1) * (scharge * 1)) * 1) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2) / 10) * 10;
                var discAmt = (((unit * 1) * (scharge * 1)) - sChargeN).toFixed(2);
                var discPer = ((discAmt / ((unit * 1) * (scharge * 1))) * 100).toFixed(2);

                document.getElementById(txtDiscountAmt).value = (discAmt * 1).toFixed(2);

                document.getElementById(txtDiscountPer).value = (discPer * 1).toFixed(2);




                document.getElementById(lblPayableAmt).innerHTML = (sChargeN * 1).toFixed(2);

                if (hdnBilltype.value = 'C') {
                    document.getElementById(lblAmountPayableByPatient).innerHTML = (sChargeN * 1).toFixed(2);
                }
                else {
                    document.getElementById(lblAmountPayableByPayer).innerHTML = (sChargeN * 1).toFixed(2);
                }

            }
            else {
                document.getElementById(txtDiscountAmt).value = (0 * 1).toFixed(2);
                document.getElementById(lblPayableAmt).innerHTML = (samt * unit).toFixed(2);

                if (hdnBilltype.value = 'C') {
                    document.getElementById(lblAmountPayableByPatient).innerHTML = (samt * unit).toFixed(2);
                }
                else {
                    document.getElementById(lblAmountPayableByPayer).innerHTML = (samt * unit).toFixed(2);
                }
            }
            var ftrTotalDiscount = Number(0);
            var ftrTotalPayableAmt = Number(0);

            var gridview = document.getElementById('gvService');
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
                    var disamt = Number($get('gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value);
                    ftrTotalDiscount += disamt;
                    var ptamt = Number($get('gvService_ctl' + rowidx.toString() + '_lblPayableAmt').innerHTML);
                    ftrTotalPayableAmt += ptamt;
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

            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotDiscountAmount').innerHTML = ftrTotalDiscount.toFixed(2);
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotPayableAmount').innerHTML = ftrTotalPayableAmt.toFixed(2);

            if (hdnBilltype.value = 'C')
                $get('gvService_ctl' + footerrowidx.toString() + '_lblAmountPayableByPatientFooter').innerHTML = ftrTotalPayableAmt.toFixed(2);
            else
                $get('gvService_ctl' + footerrowidx.toString() + '_lblAmountPayableByPayerFooter').innerHTML = ftrTotalPayableAmt.toFixed(2);
        }
        function isValidDiscPrec(PrecCalc) {
            var AuthorizedDiscdPrec = document.getElementById("hdnAuthorizedDiscPercent");
            if ((PrecCalc * 1) <= (AuthorizedDiscdPrec.value * 1)) {
                return true;
            }
            else {
                alert('Discount not authorized, Authorised Percentage is ' + AuthorizedDiscdPrec.value);
                return false;
            }
        }
        function CalculatePercentage(txtServiceAmount, txtDoctorAmount, lblUnit, txtDiscountPer, txtDiscountAmt, lblPayableAmt, hdnServiceId, lblAmountPayableByPatient, lblAmountPayableByPayer) {
            var unit = Number(document.getElementById(lblUnit).innerHTML);
            var samt = Number(document.getElementById(txtServiceAmount).value);
            var damt = Number(document.getElementById(txtDoctorAmount).value);
            var discountamt = Number(document.getElementById(txtDiscountAmt).value);
            var scharge = Number(samt + damt);
            var hdnBilltype = document.getElementById("hdnBilltype");
            if (document.getElementById(txtDiscountPer).value > 100) {
                alert('Discount should not be greater than 100% !');
                document.getElementById(txtDiscountPer).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountPer).focus();
                return false;
            }
            if ((discountamt * 1) > ((scharge * 1) * (unit * 1))) {
                alert('Discount amount should not be greater than actual charge !');
                document.getElementById(txtDiscountPer).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).focus();
                return false;
            }

            if (!validate(document.getElementById(txtServiceAmount).value)) {
                alert('Service Amount Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDoctorAmount).value)) {
                alert('Doctor Amount Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDiscountPer).value)) {
                alert('Discount Percent Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDiscountAmt).value)) {
                alert('Discount Amount Not Correct. Please Check...');
                return;
            }
            if (document.getElementById(txtServiceAmount).value.lemgth == 1 && document.getElementById(txtServiceAmount).value == '.')
                document.getElementById(txtServiceAmount).value = '0.';
            if (document.getElementById(txtDoctorAmount).value.lemgth == 1 && document.getElementById(txtDoctorAmount).value == '.')
                document.getElementById(txtDoctorAmount).value = '0.';
            var sChargeN = (0 * 1).toFixed(2);

            if (discountamt > 0) {
                document.getElementById(txtDiscountPer).value = (((discountamt * 1) * 100) / ((unit * 1) * (scharge * 1))).toFixed(2);

                sChargeN = (((((unit * 1) * (scharge * 1)) * 1) - (discountamt * 1)).toFixed(2) / 10) * 10;
                var discAmt = (((unit * 1) * (scharge * 1)) - sChargeN).toFixed(2);
                var discPer = ((discAmt / ((unit * 1) * (scharge * 1))) * 100).toFixed(2);

                document.getElementById(txtDiscountAmt).value = (discAmt * 1).toFixed(2);

                document.getElementById(txtDiscountPer).value = (discPer * 1).toFixed(2);




                document.getElementById(lblPayableAmt).innerHTML = (sChargeN * 1).toFixed(2);

                if (hdnBilltype.value = 'C') {
                    document.getElementById(lblAmountPayableByPatient).innerHTML = (sChargeN * 1).toFixed(2);
                }
                else {
                    document.getElementById(lblAmountPayableByPayer).innerHTML = (sChargeN * 1).toFixed(2);
                }
            }
            else {
                document.getElementById(txtDiscountPer).value = (0 * 1).toFixed(2);

                document.getElementById(lblPayableAmt).innerHTML = (samt * unit).toFixed(2);

                if (hdnBilltype.value = 'C') {
                    document.getElementById(lblAmountPayableByPatient).innerHTML = (samt * unit).toFixed(2);
                }
                else {
                    document.getElementById(lblAmountPayableByPayer).innerHTML = (samt * unit).toFixed(2);
                }

            }
            var x = document.getElementById(txtDiscountPer).value;

            if (isValidDiscPrec(x) == false) {
                x = 0;
                document.getElementById(txtDiscountPer).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).focus();
            }
            var ftrTotalDiscount = Number(0);
            var ftrTotalPayableAmt = Number(0);

            var gridview = document.getElementById('gvService');
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
                    var disamt = Number($get('gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value);
                    ftrTotalDiscount += disamt;
                    var ptamt = Number($get('gvService_ctl' + rowidx.toString() + '_lblPayableAmt').innerHTML);
                    ftrTotalPayableAmt += ptamt;
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
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotDiscountAmount').innerHTML = ftrTotalDiscount.toFixed(2);
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotPayableAmount').innerHTML = ftrTotalPayableAmt.toFixed(2);

            if (hdnBilltype.value = 'C')
                $get('gvService_ctl' + footerrowidx.toString() + '_lblAmountPayableByPatientFooter').innerHTML = ftrTotalPayableAmt.toFixed(2);
            else
                $get('gvService_ctl' + footerrowidx.toString() + '_lblAmountPayableByPayerFooter').innerHTML = ftrTotalPayableAmt.toFixed(2);
        }


        function CopyPercentToAllAndCalculate(txtPercentDiscount) {
            var discountPercent = Number(document.getElementById(txtPercentDiscount).value);

            var ftrTotalDiscount = Number(0);
            var ftrTotalPayableAmt = Number(0);

            var gridview = document.getElementById('gvService');
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
                    var active = $get('gvService_ctl' + rowidx.toString() + '_hdnActive').value;

                    if (active == '1' || active == 'True') {
                        var unit = Number($get('gvService_ctl' + rowidx.toString() + '_lblUnit').innerHTML);
                        var samt = Number($get('gvService_ctl' + rowidx.toString() + '_txtServiceAmount').value);
                        var damt = Number($get('gvService_ctl' + rowidx.toString() + '_txtDoctorAmount').value);

                        var scharge = Number(samt + damt);





                        var sChargeN = (0 * 1).toFixed(2);


                        var discountAmount = Number(((discountPercent * 1) / 100) * ((unit * 1) * (scharge * 1)));

                        sChargeN = Math.round(((((unit * 1) * (scharge * 1)) * 1) - (discountamt * 1)).toFixed(2) / 10) * 10;

                        var discAmt = (((unit * 1) * (scharge * 1)) - sChargeN).toFixed(2);
                        var discPer = ((discAmt / ((unit * 1) * (scharge * 1))) * 100).toFixed(2);


                        var payableAmount = Number(sChargeN);

                        ftrTotalDiscount += discountAmount;
                        ftrTotalPayableAmt += payableAmount;

                        $get('gvService_ctl' + rowidx.toString() + '_txtPercentDiscount').value = Number(discPer).toFixed(2);
                        $get('gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value = Number(discAmt).toFixed(2);
                        $get('gvService_ctl' + rowidx.toString() + '_lblPayableAmt').innerHTML = Number(payableAmount).toFixed(2);
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
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotDiscountAmount').innerHTML = ftrTotalDiscount.toFixed(2);
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotPayableAmount').innerHTML = ftrTotalPayableAmt.toFixed(2);
        }
        function validate(s) {
            var rgx = /^[0-9]*\.?[0-9]*$/;
            if (s.match(rgx))
                return true;
            else
                return false;
        }
        function CalculateAmountWhenUpdateServiceAmount(txtServiceAmount, txtDoctorAmount, lblUnit, txtDiscountPer, txtDiscountAmt, lblPayableAmt, hdnServiceId, lblAmountPayableByPatient, lblAmountPayableByPayer) {
            var unit = Number(document.getElementById(lblUnit).innerHTML);
            var discountPercent = Number(document.getElementById(txtDiscountPer).value);

            if (!validate(document.getElementById(txtServiceAmount).value)) {
                alert('Service Amount Not Correct. Please Check...');
                return;
            }
            if (!validate(document.getElementById(txtDoctorAmount).value)) {
                alert('Doctor Amount Not Correct. Please Check...');
                return;
            }
            if (document.getElementById(txtServiceAmount).value.length == 1 && document.getElementById(txtServiceAmount).value == '.')
                document.getElementById(txtServiceAmount).value = '0.';
            if (document.getElementById(txtDoctorAmount).value.length == 1 && document.getElementById(txtDoctorAmount).value == '.')
                document.getElementById(txtDoctorAmount).value = '0.';
            var samt = Number(document.getElementById(txtServiceAmount).value);
            var damt = Number(document.getElementById(txtDoctorAmount).value);
            var hdnBilltype = document.getElementById("hdnBilltype");
            var scharge = Number(samt + damt);

            if (discountPercent > 100) {
                alert('Discount should not be greater than 100% !');
                document.getElementById(txtDiscountPer).focus();
                return false;
            }

            if (isValidDiscPrec(discountPercent) == false && discountPercent > 0) {
                discountPercent = 0;
                document.getElementById(txtDiscountPer).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountAmt).value = ("0" * 1).toFixed(2);
                document.getElementById(txtDiscountPer).focus();
            }

            if (discountPercent > 0) {
                document.getElementById(txtDiscountAmt).value = (((discountPercent * 1) / 100) * ((unit * 1) * (scharge * 1))).toFixed(2);
            }
            else {
                document.getElementById(txtDiscountAmt).value = (0 * 1).toFixed(2);
            }

            document.getElementById(lblPayableAmt).innerHTML = (((scharge * 1) * (unit * 1)) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2);


            if (hdnBilltype.value = 'C') {
                document.getElementById(lblAmountPayableByPatient).innerHTML = (((scharge * 1) * (unit * 1)) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2);
            }
            else {
                document.getElementById(lblAmountPayableByPayer).innerHTML = (((scharge * 1) * (unit * 1)) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2);
            }

            var ftrTotalServiceAmt = Number(0);
            var ftrTotalDiscount = Number(0);
            var ftrTotalPayableAmt = Number(0);

            var gridview = document.getElementById('gvService');
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
                    var active = $get('gvService_ctl' + rowidx.toString() + '_hdnActive').value;

                    if (active == '1') {

                        var serviceAmt = Number($get('gvService_ctl' + rowidx.toString() + '_txtServiceAmount').value);
                        ftrTotalServiceAmt += serviceAmt;
                        var disamt = Number($get('gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value);
                        ftrTotalDiscount += disamt;
                        var ptamt = Number($get('gvService_ctl' + rowidx.toString() + '_lblPayableAmt').innerHTML);
                        ftrTotalPayableAmt += ptamt;
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
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotServiceAmount').innerHTML = ftrTotalServiceAmt.toFixed(2);
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotDiscountAmount').innerHTML = ftrTotalDiscount.toFixed(2);
            $get('gvService_ctl' + footerrowidx.toString() + '_lblTotPayableAmount').innerHTML = ftrTotalPayableAmt.toFixed(2);

            if (hdnBilltype.value = 'C')
                $get('gvService_ctl' + footerrowidx.toString() + '_lblAmountPayableByPatientFooter').innerHTML = ftrTotalPayableAmt.toFixed(2);
            else
                $get('gvService_ctl' + footerrowidx.toString() + '_lblAmountPayableByPayerFooter').innerHTML = ftrTotalPayableAmt.toFixed(2);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrpit1" runat="server">
        </asp:ScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="All" runat="server"
            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
        <div id="dvZone1" style="width: 100%">
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-12"></div>
                    <div class="col-md-4 col-sm-4 col-12">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-4">
                                <asp:Label ID="lblDept" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, department%>'
                                            Font-Bold="true" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-8">
                                <telerik:RadComboBox ID="ddlDepartment" runat="server" Skin="Default" MarkFirstMatch="true" EmptyMessage="[Select Department]"
                                                Width="100%" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12 text-right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="Save" />
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text="Close" OnClientClick="window.close();" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row text-center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </div>
                <div class="row" style="background: #F5DEB3;">
                    <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text="IP No:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                                Font-Bold="true"></asp:Label>
                                            <asp:Label ID="Label6" runat="server" Text="Admission Date:" SkinID="label" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label"></asp:Label>
                                            <asp:HiddenField ID="hdnRegId" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel9" class="row" style="width:99vw;overflow:auto;" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="true" AllowSorting="true" PageSize="20" OnSorting="SortRecords" ShowFooter="true"
                                    Width="100%" OnRowDataBound="gvService_OnRowDataBound" OnRowCommand="gvService_RowCommand"
                                    OnPageIndexChanging="gvService_OnPageIndexChanging">
                                    <EmptyDataTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Order Found.
                                        </div>
                                    </EmptyDataTemplate>
                                    <FooterStyle BackColor="LightGray" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:Label ID="lblSno" Visible="false" runat="Server" Text='<%#Eval("SNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderNo" runat="Server" Text='<%#Eval("OrderNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Date" HeaderStyle-Width="5%" SortExpression="OrderDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="Server" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service" HeaderStyle-Width="25%" ItemStyle-Wrap="true" SortExpression="ServiceName">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("DetailId") %>' />
                                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderId")%>' />
                                                <asp:HiddenField ID="hdnUnderPack" runat="server" Value='<%#Eval("UnderPackage") %>' />
                                                <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%#Eval("PackageId") %>' />
                                                <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%#Eval("ServiceType")%>' />
                                                <asp:HiddenField ID="hdnDoctorID" runat="server" Value='<%#Eval("DoctorID")%>' />
                                                <asp:HiddenField ID="hdnDocReq" runat="server" Value='<%#Eval("DoctorRequired")%>' />
                                                <asp:HiddenField ID="hdnDeptId" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                <asp:HiddenField ID="hdnDeptName" runat="server" Value='<%#Eval("Departmentname")%>' />
                                                <asp:HiddenField ID="hdnIsPackageMain" runat="server" Value='<%#Eval("IsPackageMain")%>' />
                                                <asp:HiddenField ID="hdnIsPackageService" runat="server" Value='<%#Eval("IsPackageService")%>' />
                                                <asp:HiddenField ID="hdnMainSurgeryId" runat="server" Value='<%#Eval("MainSurgeryId")%>' />
                                                <asp:HiddenField ID="hdnIsSurgeryMain" runat="server" Value='<%#Eval("IsSurgeryMain")%>' />
                                                <asp:HiddenField ID="hdnIsSurgeryService" runat="server" Value='<%#Eval("IsSurgeryService")%>' />
                                                <asp:HiddenField ID="hdnStatusId" runat="server" Value='<%#Eval("statusId")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("Active")%>' />
                                                <asp:HiddenField ID="hdnBatchId" runat="server" Value='<%#Eval("BatchId")%>' />
                                                <asp:HiddenField ID="hdnSODAID" runat="server" Value='<%#Eval("SODAID")%>' />
                                                <asp:HiddenField ID="hdnPriceEditable" runat="server" Value='<%#Eval("PriceEditable")%>' />
                                                <asp:HiddenField ID="hdnIsDiscountable" runat="server" Value='<%#Eval("IsDiscountable")%>' />
                                                <asp:HiddenField ID="hdnEncodedBy" runat="server" Value='<%#Eval("EncodedBy")%>' />
                                                <asp:HiddenField ID="hdnEncodedDate" runat="server" Value='<%#Eval("EncodedDate")%>' />
                                                <asp:HiddenField ID="hdnLastChangedBy" runat="server" Value='<%#Eval("LastChangedBy")%>' />
                                                <asp:HiddenField ID="hdnLastChangedDate" runat="server" Value='<%#Eval("LastChangedDate")%>' />
                                                <asp:HiddenField ID="hdnServiceDiscountPercentage" runat="server" Value='<%Eval("ServiceDiscountPercentage") %>' />
                                                <asp:HiddenField ID="hdnStatusCode" runat="server" Value='<%#Eval("StatusCode")%>' />
                                                <asp:HiddenField ID="hdnBillCategory" runat="server" Value='<%#Eval("BillCategory")%>' />
                                                <asp:HiddenField ID="hdnClinicalDetailFound" runat="server" Value='<%#Eval("ClinicalDetailsFound")%>' />
                                                <asp:HiddenField ID="hdnManualEntryNo" runat="server" Value='<%#Eval("ManualEntryNo")%>' />
                                                <asp:HiddenField ID="hdLabSampleNotes" runat="server" Value='<%#Eval("LabSampleNotes")%>' />

                                                <%--<asp:HiddenField ID="hdnEnteredBy" runat="server" Value='<%#Eval("EnteredBy")%>' />--%>
                                                <%-- <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' ToolTip='<%#Eval("ServiceName")%>' />--%>
                                                <asp:LinkButton ID="lblServiceName" runat="server" OnClick="lblServiceName_OnClick"
                                                    Text='<%#Eval("ServiceName") %>' ToolTip='<%#Eval("ServiceName")%>' Enabled="true" />
                                                <asp:HiddenField ID="hdnIsMandatory" runat="server" Value='<%#Eval("IsMandatory")%>' />
                                                <asp:HiddenField ID="hdnTemplateTaggingService" runat="server" Value='<%#Eval("TemplateTaggingService")%>' />
                                                <asp:HiddenField ID="hdnSubDeptId" runat="server" Value='<%#Eval("SubDeptId")%>' />

                                                <asp:Label ID="lblForNotes" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"
                                                    Style="text-decoration: blink; font-size: large" Text="*" />
                                                <asp:ImageButton ID="ibtnForNotes" runat="server" ImageUrl="~/Images/NotesNew.png"
                                                    ToolTip="Click to show patient notes." OnClick="lbtnNotes_OnClick" Visible="false" CommandName="sel" CommandArgument='<%#Eval("OrderId")%>' />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <asp:LinkButton ID="lblservicename" runat="server" OnClick="SortRecords" Text="Service" />
                                                    </div>
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <telerik:RadComboBox ID="ddlService" DropDownWidth="400px" Filter="Contains"
                                                                Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlService_OnSelectedIndexChanged">
                                                            </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PRegistration,Provider%>" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProvider" runat="server" Style="width: 99%;" Text='<%#Eval("DoctorName")%>' />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <asp:Label ID="lbldoctor" runat="server" Text="<%$ Resources:PRegistration,Provider%>"></asp:Label>
                                                    </div>
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <telerik:RadComboBox ID="ddlProvider" SkinID="DropDown" DropDownWidth="200px" Filter="Contains"
                                                    Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_OnSelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                    </div>
                                                    </div>
                                                
                                                
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField HeaderText="<%$ Resources:PRegistration,Provider%>" DataField="DoctorName"
                                        HeaderStyle-Width="15%" />--%>
                                        <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnit" runat="server" Style="width: 99%; text-align: center;" Text='<%#Eval("Units","{0:f0}")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Amount" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtServiceAmount" runat="server" Style="width: 99%; text-align: right;"
                                                    MaxLength="10" autocomplete="off" Text='<%#Eval("ServiceAmount","{0:f2}")%>'
                                                    ToolTip='<%#Eval("ServiceAmount","{0:f2}")%>'> </asp:TextBox>
                                                <ajax:FilteredTextBoxExtender ID="FTBEServiceDiscountServiceAmount" runat="server"
                                                    ValidChars="." FilterType="Custom,Numbers" TargetControlID="txtServiceAmount">
                                                </ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotServiceAmount" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Doctor Amount" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDoctorAmount" runat="server" MaxLength="10" Style="width: 99%; text-align: right;"
                                                    autocomplete="off" Enabled="false" Text='<%#Eval("DoctorAmount","{0:f2}")%>'
                                                    ToolTip='<%#Eval("DoctorAmount","{0:f2}")%>'> </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotDoctorAmount" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gross Amt" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrossAmt" runat="server" MaxLength="10" Style="width: 99%; text-align: right;"
                                                    autocomplete="off" Enabled="false" Text='<%#Eval("GrossAmt","{0:f2}")%>' ToolTip='<%#Eval("GrossAmt","{0:f2}")%>'> </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotGrossAmt" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Disc %" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPercentDiscount" MaxLength="6" runat="server" Style="width: 99%; text-align: right"
                                                    autocomplete="off" Text='<%#Eval("ServiceDiscountPercentage","{0:f2}")%>'
                                                    ToolTip="Double click to copy this discount to all active service(s)" />
                                                <ajax:FilteredTextBoxExtender ID="FTBEServiceDiscountP" runat="server" ValidChars="."
                                                    FilterType="Custom,Numbers" TargetControlID="txtPercentDiscount">
                                                </ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Disc. Amt" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDiscountAmt" runat="server" MaxLength="10" Style="width: 99%; text-align: right"
                                                    autocomplete="off" Text='<%#Eval("TotalDiscount","{0:f2}")%>'
                                                    ToolTip='<%#Eval("TotalDiscount","{0:f2}")%>' />
                                                <ajax:FilteredTextBoxExtender ID="FTBEServiceDiscountamt" runat="server" ValidChars="."
                                                    FilterType="Custom,Numbers" TargetControlID="txtDiscountAmt">
                                                </ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotDiscountAmount" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Net Amt" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayableAmt" runat="server" Style="width: 99%; text-align: right;"
                                                    Text='<%#Eval("NetCharge","{0:f2}")%>' ToolTip='<%#Eval("NetCharge","{0:f2}")%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotPayableAmount" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Patient Amt" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountPayableByPatient" runat="server" Style="width: 99%; text-align: right;"
                                                    Text='<%#Eval("AmountPayableByPatient","{0:f2}")%>' ToolTip='<%#Eval("AmountPayableByPatient","{0:f2}")%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAmountPayableByPatientFooter" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payer Amt" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountPayableByPayer" runat="server" Style="width: 99%; text-align: right;"
                                                    Text='<%#Eval("AmountPayableByPayer","{0:f2}")%>' ToolTip='<%#Eval("AmountPayableByPayer","{0:f2}")%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAmountPayableByPayerFooter" runat="server" Text="0.00" />
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ordering Location" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardShortName" runat="Server" Text='<%#Eval("WardShortName")%>'
                                                    ToolTip='<%#Eval("WardShortName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Approval" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIsApproved" runat="Server" Text='<%#Eval("IsApproved")%>'
                                                    ToolTip='<%#Eval("IsApproved")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lblPrint" CommandArgument='<%#Eval("DetailId")%>' CommandName="Print" ToolTip="Click here to Print Reports" ForeColor="Blue" runat="server">Print</asp:LinkButton>--%>
                                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" CommandName="PrintOrder" ToolTip="Click here to Print Reports" ForeColor="Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtndaDelete" runat="server" ToolTip="Click here to delete this row"
                                                    CommandName="Del" CausesValidation="false" CommandArgument='<%#Eval("DetailId")%>'
                                                    ImageUrl="~/Images/DeleteRow.png" OnClientClick="return confirm('Are you sure, you want to cancel?');" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <RowStyle Wrap="False" />
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="gvService" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row m-t" style="background:#e0ebfd;" >
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-4">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Sno: " />
                            </div>
                            <div class="col-md-8 col-sm-8 col-8">
                                <asp:Label ID="lblSno" ForeColor="Maroon" runat="server" Font-Bold="true" SkinID="label" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Order No: " />
                            </div>
                            <div class="col-md-8 col-sm-8 col-8">
                                <asp:Label ID="lblOrderNo" ForeColor="Maroon" runat="server" Font-Bold="true" SkinID="label" />
                            </div>
                        </div>
                    </div>
                   <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Order Date: " />
                            </div>
                            <div class="col-md-8 col-sm-8 col-8">
                                <asp:Label ID="lblOrderDate" ForeColor="Maroon" runat="server" Font-Bold="true" SkinID="label" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="Service: " />
                            </div>
                            <div class="col-md-8 col-sm-8 col-8">
                                <asp:Label ID="lblServiceName" ForeColor="Maroon" runat="server" Font-Bold="true" SkinID="label" Width="400px" />
                            </div>
                        </div>
                    </div>
                   
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-xs-4 text-nowrap">
                               <asp:Label ID="Label12" runat="server" Font-Bold="true" Text="Entered By" />
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-8">
                              <asp:Label ID="lblEncodedBy" runat="server" Width="130px" SkinID="label" ForeColor="Maroon" /> 
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-xs-4 text-nowrap">
                                <asp:Label ID="Label13" runat="server" Font-Bold="true" Text="Entry Date" />
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-8">
                               <asp:Label ID="lblEncodedDate" runat="server" Width="150px" SkinID="label" ForeColor="Maroon" />
                            </div>
                        </div>
                    </div>
                     <div class="clearfix"></div>
                    <hr style="margin:3px 0px;background:#99b0d4;height:1px;" />
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-12 text-nowrap">
                               <asp:Label ID="Label14" runat="server" Font-Bold="true" Text="Changed By" />
                            </div>
                            <div class="col-md-12 col-sm-12 col-12">
                               <asp:Label ID="lblLastChangedBy" runat="server" Width="130px" SkinID="label" ForeColor="Maroon" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-12 text-nowrap">
                               <asp:Label ID="Label15" runat="server" Font-Bold="true" Text="Changed Date" />
                            </div>
                            <div class="col-md-12 col-sm-12 col-12">
                               <asp:Label ID="lblLastChangedDate" runat="server" Width="150px" SkinID="label" ForeColor="Maroon" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-12 text-nowrap">
                               <asp:Label ID="lblBillCatgoryh" runat="server" Font-Bold="true" Text="Bill Category/Bed Category" />
                            </div>
                            <div class="col-md-12 col-sm-12 col-12">
                               <asp:Label ID="lblBillCatgory" runat="server" Width="150px" SkinID="label" ForeColor="Maroon" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 col-12 text-nowrap">
                               <asp:Label ID="Label16" runat="server" Font-Bold="true" Text="Manual Entry No" />
                            </div>
                            <div class="col-md-12 col-sm-12 col-12">
                               <asp:Label ID="lblManualEntry" runat="server" Width="150px" SkinID="label" ForeColor="Maroon" />
                            </div>
                        </div>
                    </div>
                   
                    <div class="col-md-4 col-sm-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-12 text-nowrap">
                                <asp:Label ID="Label21" runat="server" Font-Bold="true" Text="Cancellation Reason: " />
                            </div>
                            <div class="col-md-8 col-sm-8 col-12">
                                <telerik:RadComboBox ID="ddlServiceCancelReason" runat="server" AutoPostBack="true" Width="130px" />
                            </div>
                        </div>
                    </div>
                     <div class="clearfix"></div>
                    <hr style="margin:3px 0px;background:#99b0d4;height:1px;" />
                    <div class="col-md-4 col-sm-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-12 text-nowrap">
                                <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Cancellation Notes: " />
                            </div>
                            <div class="col-md-8 col-sm-8 col-12">
                               <asp:TextBox ID="txtCancelationRemarks" runat="server" TextMode="MultiLine" Width="100%"
                                        Height="35px" onkeyup="return ValidateMaxLength();" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-12">
                        <asp:Label ID="Label8" runat="server" Text="...." Width="10px" SkinID="label" BackColor="Bisque"
                                        ForeColor="Bisque" />
                        <asp:Label ID="Label10" runat="server" Text="Unperformed Services" Font-Bold="true"
                                    SkinID="label" />

                    </div>
                    <script language="javascript" type="text/javascript">
                        function ValidateMaxLength() {
                            var txt = $get('<%=txtCancelationRemarks.ClientID%>');
                            if (txt.value.length > 250) {
                                alert("Text length should not be more then 250 characters.");
                                txt.value = txt.value.substring(0, 250);
                                txt.focus();
                            }
                        }

                        function ShowEncodDetails(ServiceName, EncodedBy, EncodedDate, LastChangedBy, LastChangedDate, Sno, OrderNo, OrderDate, BillCategory, ManualEntryNo) {
                            document.getElementById("<%=lblSno.ClientID%>").innerHTML = $get(Sno).innerHTML;
                            document.getElementById("<%=lblOrderNo.ClientID%>").innerHTML = $get(OrderNo).innerHTML;
                            document.getElementById("<%=lblOrderDate.ClientID%>").innerHTML = $get(OrderDate).innerHTML;
                            document.getElementById("<%=lblServiceName.ClientID%>").innerHTML = $get(ServiceName).innerHTML;
                            document.getElementById("<%=lblEncodedBy.ClientID%>").innerHTML = $get(EncodedBy).value;
                            document.getElementById("<%=lblEncodedDate.ClientID%>").innerHTML = $get(EncodedDate).value;
                            document.getElementById("<%=lblLastChangedBy.ClientID%>").innerHTML = $get(LastChangedBy).value;
                            document.getElementById("<%=lblLastChangedDate.ClientID%>").innerHTML = $get(LastChangedDate).value;
                            document.getElementById("<%=lblBillCatgory.ClientID%>").innerHTML = $get(BillCategory).value;
                            document.getElementById("<%=lblManualEntry.ClientID%>").innerHTML = $get(ManualEntryNo).value;

                        }
                    </script>
                </div>
                <div class="row">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnDetailType" runat="server" Value="I" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="2" />
                                <asp:HiddenField ID="hdnDepartmentId" runat="server" />
                                <asp:HiddenField ID="hdnServiceType" runat="server" />
                                <asp:HiddenField ID="hdnAuthorizedDiscPercent" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnAuthorizedRemark" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnTotUnit" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnTotServiceAmount" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnTotDoctorAmount" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnTotGrossAmt" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnTotDiscountAmount" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnAmountPayableByPayer" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnAmountPayableByPatient" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnBilltype" runat="server" Value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </div>
                <div class="row">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
