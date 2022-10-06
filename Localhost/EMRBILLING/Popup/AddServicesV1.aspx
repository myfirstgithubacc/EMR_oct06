<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true"
    CodeFile="AddServicesV1.aspx.cs" Inherits="EMRBILLING_Popup_AddServicesV1" Title="Investigation Order" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <%--    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/BlankMasterStyle") %>
    </asp:PlaceHolder>--%>
    <style type="text/css">
        table#ctl00_ContentPlaceHolder1_chkStat {
            display: block !important;
            /* width: 10px!important; */
            float: left;
        }

        table#ctl00_ContentPlaceHolder1_Legend1_tblLegend span {
            padding-left: 4px;
            padding-right: 8px;
        }

        div#ctl00_ContentPlaceHolder1_tblMain {
            overflow-x: hidden !important;
        }

        #ctl00_ContentPlaceHolder1_lblMessage {
            float: none !important;
            margin: 0 !important;
            width: 100% !important;
            position: relative !important;
        }

        .RadComboBox_Default td.rcbArrowCellRight, .RadComboBox_Outlook td.rcbArrowCellRight {
            background-position: 0 -88px !important;
            margin-left: 0px !important;
            height: 24px!important;
            margin-top: 0px!important;
            margin-right: -6px !important;
        }
    </style>
    <script type="text/javascript">

        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false) {
                    return false;
                }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.className += " btn btn-primary";
                myButton.value = "Processing...";
            }
            return true;
        }
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = $get('<%=hdnXmlString.ClientID%>').value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function ShowServiceInstructions(ServiceInstructions) {
            document.getElementById("pServiceInstructions").innerHTML = ServiceInstructions;
            return false;
        }

        function CalculateEditablePrice(txtServiceAmount, txtDoctorAmount, txtUnits, txtDiscountPercent, txtDiscountAmt, txtNetCharge, txtAmountPayableByPatient, txtAmountPayableByPayer) {

            var DecimalPlaces = document.getElementById('<%= hdnDecimalPlaces.ClientID%>').value;
            var DiscountPerc = parseFloat(document.getElementById(txtDiscountPercent).value);
            var totalAmount = parseFloat(document.getElementById(txtServiceAmount).value) + parseFloat(document.getElementById(txtDoctorAmount).value);
            if (DiscountPerc > 0) {
                var DiscountAmt = (((DiscountPerc * 1) / 100) * ((document.getElementById(txtUnits).value * 1) * ((totalAmount * 1))).toFixed(DecimalPlaces));
            }
            else {
                var DiscountAmt = (0 * 1).toFixed(DecimalPlaces);
            }

            var ServiceCharge = document.getElementById(txtServiceAmount).value;
            var DoctorCharge = document.getElementById(txtDoctorAmount).value
            var Units = document.getElementById(txtUnits).value
            var totalAmount = (parseFloat(ServiceCharge) + parseFloat(DoctorCharge)).toFixed(DecimalPlaces);

            //            alert(DiscountAmt);
            //            alert(totalAmount);
            //            alert(Units);
            //            alert(ServiceCharge);
            //            alert(DoctorCharge);

            var NetAmount = (parseFloat(Units * 1) * parseFloat(totalAmount));

            document.getElementById(txtDiscountAmt).value = parseFloat(DiscountAmt).toFixed(DecimalPlaces);
            var NetCharge = parseFloat(NetAmount - DiscountAmt);
            document.getElementById(txtNetCharge).value = parseFloat(NetCharge).toFixed(DecimalPlaces);
            //            document.getElementById(txtNetCharge).value = ((parseFloat(Units * 1) * parseFloat(totalAmount)) - parseFloat(DiscountAmt)).toFixed(DecimalPlaces);
            //            alert(ServiceCharge);
            //alert(NetAmount);
            //  var hdnBilltype = document.getElementById("ctl00_ContentPlaceHolder1_hdnBilltype");
            var hdnBilltype = $get('<%=hdnBilltype.ClientID%>')

            if (parseInt(hdnBilltype.value) == 1) {
                document.getElementById(txtAmountPayableByPatient).value = parseFloat(NetCharge).toFixed(DecimalPlaces);

            }
            else {
                document.getElementById(txtAmountPayableByPayer).value = parseFloat(NetCharge).toFixed(DecimalPlaces);

            }
            //document.getElementById('btnGetBalance').click();
            $get('<%=btnGetBalance.ClientID%>').click();
        }
        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPassword.ClientID%>').click();
        }
        function ddlService_OnClientSelectedIndexChanged(sender, args) {

            var item = args.get_item();
            $get('<%=hdnStatValueContainer.ClientID%>').value = item != null ? item.get_attributes().getAttribute("IsStatOrderAllowed") : " ";
            $get('<%=hdnIsServiceRemarkMandatory.ClientID%>').value = item != null ? item.get_attributes().getAttribute("isServiceRemarkMandatory") : " ";
            $get('<%=hdnGlobleServiceType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ServiceType") : " ";
            $get('<%=hdnGlobleStationId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("StationId") : " ";
            $get('<%=hndEquipmentType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("EquipmentType") : " ";
            $get('<%=hndChargesPeriod.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ChargesPeriod") : " ";
            $get('<%=hndGracePeriod.ClientID%>').value = item != null ? item.get_attributes().getAttribute("GracePeriod") : " ";

            document.getElementById('<%=btnGetInfo.ClientID%>').click();
            //document.getElementById('<%=cmdAddtoGrid.ClientID%>').click();
        }

        function UncheckOthers(objchkbox) {
            //Get the parent control of checkbox which is the checkbox list
            var objchkList = objchkbox.parentNode.parentNode.parentNode;
            //Get the checkbox controls in checkboxlist
            var chkboxControls = objchkList.getElementsByTagName("input");
            //Loop through each check box controls
            for (var i = 0; i < chkboxControls.length; i++) {
                //Check the current checkbox is not the one user selected
                if (chkboxControls[i] != objchkbox && objchkbox.checked) {
                    //Uncheck all other checkboxes
                    chkboxControls[i].checked = false;
                }
            }
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

                case 114:  // F3
                    $get('<%=ibtnSave.ClientID%>').click();
                break;




        }
        evt.returnValue = false;
        return false;

    }
    </script>

    <style type="text/css">
        .style1 {
            width: 379px;
        }

        .checkbox-inline {
            padding: 0;
        }

            .checkbox-inline label {
                padding-left: 15px;
                padding-right: 15px;
            }

        span#ctl00_ContentPlaceHolder1_Label4 {
            margin-top: 0;
        }

        .heading-title {
            display: inline-block;
            font-size: 13px;
            margin: 0px;
            padding: 0px;
            line-height: 20px;
            font-weight: bold;
        }

        .PD-TabRadio.margin_z label {
            margin: 3px 10px 0 0;
            font-size: 11px !important;
        }

        div#ctl00_ContentPlaceHolder1_Panel1 td {
            vertical-align: top;
        }
    </style>

    <asp:HiddenField ID="hdnStatValueContainer" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsServiceRemarkMandatory" runat="server" Value="0" />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div id="tblMain" runat="server">

                <div id="Table1" runat="server">
                    <div class="container-fluid" style="background: #C1E5EF; padding: 6px 0;">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-5 col-12">
                                <h2 class="heading-title">
                                    <asp:Label ID="lblPageType" runat="server" Text="" />
                                </h2>
                                <asp:LinkButton ID="lnkViewLabOrders" runat="server" ToolTip="Previous Orders" Text="&nbsp;Previous&nbsp;Order(s)&nbsp;" Visible="false" OnClick="lnkViewLabOrders_OnClick" Font-Bold="true" Style="color: darkred" BorderWidth="1px" BorderStyle="Solid" BorderColor="dodgerblue" />
                                &nbsp;<asp:LinkButton ID="lnkSampleCollection" runat="server" ToolTip="Sample Collection" Text="&nbsp;Sample&nbsp;Collection&nbsp;" OnClick="lnkSampleCollect_OnClick" Font-Bold="true" Style="color: darkgreen" BorderWidth="1px" BorderStyle="Solid" BorderColor="dodgerblue" />
                            </div>
                            <div class="col-lg-1 col-md-1  col-sm-1 col-6" style="float: left; text-align: left">
                                <button id="liAllergyAlert" runat="server" class="btn btn-default" visible="false" style="background: #fff; border: 0px;">
                                    <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" Width="18px" Height="18px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                                </button>
                                <button id="liMedicalAlert" runat="server" visible="false" class="btn btn-default" style="background: #fff; border: 0px;">
                                    <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif" OnClick="imgMedicalAlert_OnClick" CssClass="iconEMRimg" Width="18px" Height="18px" Visible="false" ToolTip="Medical Alert" />
                                </button>
                            </div>
                            <div class="col-lg-7 col-md-7 col-sm-6 col-12 text-right pull-right">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkIsGenerateAdvance" Text="Generate Advance" Checked="false" runat="server" Visible="false" />

                                        <asp:Button ID="btnPrintIPServiceOrder" runat="server" CssClass="btn btn-primary" Text="Print" ToolTip="Print IP Service Order" OnClick="btnPrintIPServiceOrder_Click" />
                                        <asp:Button ID="btnorderprint" runat="server" CssClass="btn btn-primary" Text="Print Order Report" ToolTip="Reprint" Visible="false" OnClick="btnorderprint_Click" />
                                        <asp:Button ID="btnReprint" runat="server" CssClass="btn btn-primary" Text="Reprint" Visible="false" ToolTip="Reprint" OnClick="btnReprint_Click" />

                                        <asp:Button ID="ibtnSave" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Save " ToolTip="Save services...(Ctrl+F3)" ValidationGroup="save" OnClick="ibtSave_OnClick" OnClientClick="ClientSideClick(this)"
                                            UseSubmitBehavior="False" />
                                        <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-primary" Text="Close" ToolTip="Close" OnClientClick="window.close();" />
                                        <asp:Button ID="btnIsValidPassword" runat="server" OnClick="btnIsValidPassword_OnClick" CssClass="BillingFullBtn" Text="Search" CausesValidation="false" Style="visibility: hidden;" Width="1" />
                                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                        <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                                        <asp:HiddenField ID="hdnPasswordScreenType" runat="server" />

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server"  CssClass="  float: none!important; margin: 0 !important; width: 100% !important; position: relative !important;"  Font-Bold="true" Text="" ForeColor="Green" Visible="true" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>


                <div class="container-fluid">
                    <div class="row" style="margin-bottom: 5px; margin-top: 5px; background: #d3e7f9; padding: 6px 0px;">

                        <div class="col-lg-1 col-md-2 col-3">
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:Label ID="lblReg" runat="server" Text='<%$ Resources:PRegistration, Regno%>' Font-Bold="true" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblregNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true" />
                                </div>
                            </div>
                        </div>



                        <div class="col-lg-4 col-md-5 col-8">
                            <div class="row">
                                <div class="col-sm-2">
                                    <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true" />
                                </div>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true" />
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-1 col-md-2 col-sm-4 col-3">
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:Label ID="Label5" runat="server" Text="DOB:" Font-Bold="true" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblDob" runat="server" Text="" />
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-2 col-md-3 col-sm-4 col-3">
                            <div class="row">
                                <div class="col-sm-4 text-nowrap">
                                    <asp:Label ID="Label4" runat="server" Text="Mobile No:" Font-Bold="true" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblMobile" runat="server" Text="" />
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-1 col-md-2 col-sm-4 col-3">
                            <div class="row">
                                <div class="col-sm-4 text-nowrap">
                                    <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" Font-Bold="true" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true" />
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-3 col-md-4 col-sm-6 col-3">
                            <div class="row">
                                <div class="col-md-6 col-sm-4 text-nowrap">
                                    <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" Font-Bold="true" />
                                </div>
                                <div class="col-md-6 col-sm-8">
                                    <asp:Label ID="lblAdmissionDate" runat="server" Text="" />
                                </div>
                            </div>
                        </div>





                        <div class="col-lg-2 col-md-4 col-sm-4 col-xs-4">
                            <asp:HiddenField ID="hdnCompanyId" runat="server" Value="" />
                            <asp:HiddenField ID="hdnInsuranceId" runat="server" Value="" />
                            <asp:HiddenField ID="hdnCardId" runat="server" Value="" />
                            <asp:HiddenField ID="hdnConfirmValue" runat="server" Value="" />
                        </div>


                    </div>

                </div>


                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-4 col-sm-4 col-6 ">
                            <div class="row form-group">
                                <div class="col-lg-3 col-md-4  label1">
                                    <asp:Label ID="lblDept" runat="server" Text='<%$ Resources:PRegistration, department%>' />
                                </div>
                                <div class="col-lg-9 col-md-8 ">
                                    <telerik:RadComboBox ID="ddlDepartment" runat="server" MaxHeight="120px" DropDownCssClass="100%" MarkFirstMatch="true" EmptyMessage="[Select Department]" Width="100%" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row form-group">
                                <div class="col-lg-3 col-md-4  label1 text-nowrap">
                                    <asp:Label ID="lblSubDept" runat="server" Text='<%$ Resources:PRegistration, SubDepartment%>' />
                                </div>
                                <div class="col-lg-9 col-md-8 ">
                                    <telerik:RadComboBox ID="ddlSubDepartment" runat="server" MarkFirstMatch="true" Width="100%" DropDownCssClass="100%" AutoPostBack="true" EmptyMessage="[Select Sub Department]" AppendDataBoundItems="true" />
                                </div>
                            </div>
                        </div>





                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row form-group">
                                <div class="col-lg-3 col-md-4  label1">
                                    <asp:Label ID="Label2" runat="server" CssClass="textLabel" Text="Order&nbsp;Date" />
                                </div>
                                <div class="col-lg-9 col-md-8 ">
                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-lg-7 col-md-8 col-7 ">
                                                      <telerik:RadDateTimePicker ID="dtOrderDate" runat="server" OnSelectedDateChanged="dtOrderDate_SelectedDateChanged" AutoPostBackControl="Both" CssClass="inlin-bl1" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" />
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-4">
                                                       <telerik:RadComboBox ID="ddlOrderMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderMinutes_SelectedIndexChanged" Skin="Office2007" Width="100%" DropDownWidth="100%" Style="min-height: 28px;" />
                                                </div>
                                                
                                            </div>
                                          


                                         

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>



                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row form-group">
                                <div class="col-lg-3 col-md-4  label1">
                                    <asp:Label ID="Label1" runat="server" Text="Service" />
                                </div>
                                <div class="col-lg-9 col-md-8 ">
                                    <asp:Panel ID="Panel1" runat="server" Width="100%" DefaultButton="cmdAddtoGrid">
                                        <table cellpadding="0" cellspacing="1" width="100%">
                                            <tr>
                                                <td>
                                                    <telerik:RadComboBox runat="server" ID="ddlService" Width="98%" Height="150px"
                                                        EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="Select Service"
                                                        DropDownWidth="470px" OnItemsRequested="ddlService_OnItemsRequested" ShowMoreResultsBox="true"
                                                        EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlService_OnClientSelectedIndexChanged">
                                                        <HeaderTemplate>
                                                            <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td style="width: 100%" align="left">Service Name
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td id="Td1" style="width: 150px;" align="left"><%# DataBinder.Eval(Container, "Attributes['ServiceName']")%></td>
                                                                        <td id="Td2" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['IsStatOrderAllowed']")%></td>
                                                                        <td id="Td3" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['ServiceType']")%></td>
                                                                        <td id="Td4" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['StationId']")%></td>
                                                                    </tr>
                                                                </table>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td class="text-right">
                                                    <asp:Button ID="cmdAddtoGrid" runat="server" OnClick="cmdAddtoGrid_OnClick" CssClass="btn btn-primary"
                                                        Style="visibility: visible;" Text="Add" />
                                                    <asp:HiddenField ID="hdServiceType" runat="server" />
                                                    <asp:Button ID="btnGetInfo" runat="server" Text="" CausesValidation="false" SkinID="button"
                                                        Style="visibility: hidden;" OnClick="btnGetInfo_Click" />

                                                    <%--yogesh --%>
                                                     <asp:Button ID="cmdfavorite" runat="server" OnClick="cmdfavorite_Click" CssClass="btn btn-primary"
                                                        Text="Favorite" />

                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row form-group" id="trAssignToEmp" runat="server">
                                <div class="col-lg-3 col-md-4  label1">
                                    <asp:Literal ID="lblAssignToEmpId" runat="server" Text="Finalized By" />
                                </div>
                                <div class="col-lg-9 col-md-8 ">
                                    <telerik:RadComboBox ID="ddlAssignToEmpId" SkinID="DropDown" runat="server" EmptyMessage="[ Select Employee ]"
                                        Width="100%" MarkFirstMatch="true" Filter="Contains" Enabled="false" />
                                </div>
                            </div>
                        </div>


                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row form-group">
                                <div class="col-lg-3 col-md-4 label1">
                                    <asp:Label ID="Label9" runat="server" Text="Provisional Diagnosis"></asp:Label>
                                </div>
                                <div class="col-lg-9 col-md-8 ">
                                    <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" Width="100%" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>



                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row form-group" id="tdAdviserDoctor" runat="server" visible="false">
                                <div class="col-lg-3 col-md-4 label1">
                                    <asp:Label ID="lblAdvisingDoctor" runat="server" Text="Advising&nbsp;Doctor" />
                                </div>
                                <div class="col-lg-9 col-md-8 ">

                                    <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="/Select" DropDownWidth="300px" Width="100%" />

                                </div>

                            </div>


                        </div>


                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <div class="row form-group">
                                <div class="col-md-12 col-sm-12 col-xs-12">

                                    <div class="checkbox-inline">
                                        <asp:CheckBox ID="chkApprovalRequired" runat="server" Visible="false" AutoPostBack="true" CssClass="float-left" Text="&nbsp;Approval Req." TextAlign="Right"
                                            OnCheckedChanged="chkApprovalRequired_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="chkStat" runat="server" RepeatDirection="Horizontal" Font-Bold="true">
                                            <asp:ListItem Text="Stat" Value="STAT" onclick="UncheckOthers(this);"></asp:ListItem>
                                            <asp:ListItem Text="Urgent" Value="URGENT" onclick="UncheckOthers(this);"></asp:ListItem>
                                        </asp:CheckBoxList>

                                        <%--<asp:CheckBox ID="chkStat" runat="server" Checked="false" TabIndex="43" Text="Stat" TextAlign="Right" />--%>
                                        <asp:CheckBox ID="chkIsEmergency" runat="server" Checked="false" Text="IsEmergency" TextAlign="Right" Visible="false" />

                                        <asp:CheckBox ID="chkisbedsidecharges" runat="server" Checked="false" Text="Bed side charges" TextAlign="Right" />

                                        <asp:CheckBox ID="chkPOCRequest" runat="server" TabIndex="43" Text="POC Order" TextAlign="Right" />

                                        <asp:CheckBox ID="chkIsBioHazard" runat="server" TabIndex="43" Text="Is Biohazard" TextAlign="Right" />

                                        <asp:CheckBox ID="chkIsAddOnTest" runat="server" TabIndex="43" Text="AddOn Test" TextAlign="Right" OnCheckedChanged="chkIsAddOnTest_CheckedChanged" AutoPostBack="true" />

                                        <asp:CheckBox ID="chkIsReadBack" runat="server" TabIndex="43" Text="Read Back" TextAlign="Right" Visible="false" />

                                    </div>
                                </div>
                            </div>

                            <div id="divCommonServices" runat="server" visible="false" style="z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed; right: 0; bottom: 5px;">
                                <table width="100%" cellspacing="2" border="0">
                                    <tr>
                                        <td style="width: 60px; padding: 8px;" align="center">Service
                                        </td>
                                        <td>
                                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
                                                <asp:TextBox ID="txtCommonServiceSearch" runat="server" Width="100%" />
                                            </asp:Panel>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnSearch" CssClass="btn btn-primary" ToolTip="Search" runat="server" Text="Search"
                                                OnClick="btnSearch_OnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="gvCommonServices" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto" Style="width: 100%; height: 400px; overflow: scroll">
                                                                <asp:GridView ID="gvCommonServices" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="15" SkinID="gridviewOrder" AutoGenerateColumns="False"
                                                                    OnPageIndexChanging="gvCommonServices_OnPageIndexChanging" HeaderStyle-ForeColor="#15428B"
                                                                    HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee"
                                                                    HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                                                    BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table">
                                                                    <%-- OnRowDataBound="gvPrevious_OnRowDataBound"    OnRowCreated="gvPrevious_OnRowCreated"  OnRowCommand="gvPrevious_OnRowCommand"--%>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkRow" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Common Service(s)" HeaderStyle-Width="70px">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkServiceName" runat="server" OnClick="lnkServiceName_OnClick" Text='<%# Eval("ServiceName") %>' Width="100%" />
                                                                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%# Eval("ServiceId") %>' />
                                                                                <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%# Eval("ServiceType") %>' />
                                                                                <asp:HiddenField ID="hdnRefServiceCode" runat="server" Value='<%# Eval("RefServiceCode") %>' />
                                                                                <asp:HiddenField ID="hdnIsOrderSet" runat="server" Value='<%# Eval("IsOrderSet") %>' />
                                                                                <asp:HiddenField ID="hdnIsLinkService" runat="server" Value='<%# Eval("IsLinkService") %>' />
                                                                                <asp:HiddenField ID="hdnIsStatOrderAllowed" runat="server" Value='<%# Eval("IsStatOrderAllowed") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <asp:GridView ID="gvorder" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                                                    PageSize="15" SkinID="gridviewOrder" AutoGenerateColumns="False"
                                                                    OnPageIndexChanging="gvorder_PageIndexChanging" HeaderStyle-ForeColor="#15428B"
                                                                    HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee"
                                                                    HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                                                    BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table">

                                                                    <%-- <asp:GridView ID="gvorder" runat="server" AutoGenerateColumns="false" DataKeyNames="SetID" Width="100%"
                                            Autopostback="true" HeaderStyle-HorizontalAlign="Left" SkinID="gridviewOrder" Style="margin-bottom: 0px"
                                            OnRowCommand="gvorder_OnRowCommand" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                            HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px"
                                            PageSize="10" OnPageIndexChanging="gvorder_PageIndexChanging" AllowPaging="True">--%>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkRow" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Order Sets" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkOrder" runat="server" CommandName="OrderLIST" Font-Size="12px" Font-Bold="false" Text='<%#Eval("SetName")%>' />
                                                                                <asp:HiddenField ID="hdnProblemId" runat="server" Value='<%#Eval("SetID")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <asp:HiddenField ID="hdnfilterOrderSet" runat="server" Value="0" />

                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Button ID="btnCommonServiceProceed" CssClass="btn btn-primary" OnClick="btnCommonServiceProceed_OnClick" runat="server" Text="Proceed" />
                                            <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" OnClick="btnClose_OnClick" Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-4 col-6" style="clear: both;">
                            <div class="row">
                                <div class="col-lg-3 col-md-4 label1 text-nowrap">Investigation Date</div>
                                <div class="col-lg-9 col-md-8 ">
                                    <div class="row">
                                        <div class="col-lg-6 col-6 pr-0">
                                            <telerik:RadDateTimePicker ID="RadDateTimePicker1" runat="server" AutoPostBackControl="Both" TabIndex="37" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                        </div>
                                        <div class="col-lg-3 col-3">
                                            <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="300px" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" DropDownWidth="100%" Skin="Office2007" Width="100%"></telerik:RadComboBox>
                                            
                                        </div>
                                        <div class="col-lg-3 col-3">
                                            <asp:Literal ID="ltDateTime" runat="server" Text="HH MM"></asp:Literal>
                                        </div>
                                    </div>
                                  
                                </div>
                            </div>

                        </div>
                        <div class="col-md-4 col-sm-8 col-6">
                            <div class="row form-group">
                                <div class="col-lg-12 col-md-12 col-sm-12 label1">
                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks / Rationale / Clinical Indication" />
                                </div>
                                <div class="col-lg-12 col-md-12 col-12">
                                    <asp:TextBox ID="txtMiscellaneousRemarks" runat="server" TabIndex="35" TextMode="MultiLine" Height="40px" Width="100%"></asp:TextBox>
                                    <%--  <asp:DropDownList ID="ddlbImgCntr" runat="server" SkinID="DropDown" Visible="false" Width="1px">
                                                <asp:ListItem Text="Select" Value="0" />
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlOrg" runat="server" SkinID="DropDown" Visible="false" Width="1px">
                                                <asp:ListItem Text="Select" Value="0" />
                                            </asp:DropDownList>--%>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-12 col-12">
                            <div class="row form-group">

                                <div class="col-md-6 col-4">


                                    <asp:LinkButton ID="lnkCommonServices" runat="server" Style="float: left; margin-right: 10px" Text="Common&nbsp;Service(s)" ToolTip="Select Multiple Services" OnClick="lnkCommonServices_OnClick" />
                                    <asp:LinkButton ID="lnkOrderset" runat="server" Style="float: left" Text="Order Set" ToolTip="Select Multiple Order Set" OnClick="lnkOrderset_Click" />


                                </div>

                                <div class="col-md-6 col-8">
                                    <div class="row">
                                        <div class="col-md-12 text-left">

                                            <asp:Label ID="lblReadBackNote" runat="server" Text="Read&nbsp;Back&nbsp;Note" Visible="false"></asp:Label>

                                        </div>
                                        <div class="col-md-12">
                                            <asp:TextBox ID="txtIsReadBackNote" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>


                        <div class="" style="clear: both;">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always">
                                <ContentTemplate>
                                    <div id="dtEquipmentType" runat="server" visible="false" class="">

                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <div class="row">
                                                <div class="col-md-3 col-sm-4 label1">
                                                    <asp:Label ID="Label12" runat="server" CssClass="textLabel" Text="From Date" />
                                                </div>

                                                <div class="col-md-9 col-sm-9">
                                                    <telerik:RadDateTimePicker ID="dtFromDate" runat="server" CssClass="inlin-bl1" Skin="Metro" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="160px" />
                                                    <%--</div>
                                                        <div class="col-md-6 col-sm-6">--%>
                                                    <telerik:RadComboBox ID="dtFromMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dtFromMinutes_SelectedIndexChanged" Width="50px" Skin="Metro" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <div class="row">
                                                <div class="col-md-5 col-sm-5 label1 PaddingRightSpacing">
                                                    <asp:Label ID="Label13" runat="server" CssClass="textLabel" Text="To Date" />
                                                </div>

                                                <div class="col-md-7 col-sm-7">
                                                    <telerik:RadDateTimePicker ID="dtToDate" runat="server" CssClass="inlin-bl1" Skin="Metro" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="160px" />
                                                    <%--  </div>
                                                        <div class="col-md-6 col-sm-6">--%>
                                                    <telerik:RadComboBox ID="dtToMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dtToMinutes_SelectedIndexChanged" Width="50px" Skin="Metro" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </div>
                </div>
                <div class="container-fluid">
                    <div class="form-group">
                        <asp:Panel ID="pnlgvService" runat="server" Width="100%" Height="350px" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                        ShowFooter="true" CssClass="table table-bordered" OnRowDataBound="gvService_RowDataBound"
                                        Width="100%" OnRowCommand="gvService_RowCommand"
                                        HeaderStyle-ForeColor="#ffffff" HeaderStyle-Height="25px"
                                        HeaderStyle-Wrap="false" HeaderStyle-BackColor="#0000ff" HeaderStyle-BorderColor="#ffffff"
                                        HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#ececec" BorderStyle="None" BorderWidth="1px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrId" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                                                    <%--Text='<%#Eval("SNo") %>'--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service" HeaderStyle-Width="100%">
                                                <ItemTemplate>
                                                    <asp:Literal ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                    <asp:LinkButton ID="lnkServiceName" runat="server" Text='<%#Eval("ServiceName") %>'
                                                        ToolTip="Click for Service Instructions" />
                                                    <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                    <asp:HiddenField ID="hdnStat" runat="server" Value='<%#Eval("Stat") %>' />
                                                    <asp:HiddenField ID="hdnUrgent" runat="server" Value='<%#Eval("Urgent") %>' />
                                                    <asp:HiddenField ID="hdnDocReq" runat="server" Value='<%#Eval("DoctorRequired")%>' />
                                                    <asp:HiddenField ID="hdnDeptId" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                    <asp:HiddenField ID="hdnlblOrderId" runat="server" Value='<%#Eval("OrderId") %>' />
                                                    <asp:HiddenField ID="hdnisExcluded" runat="server" Value='<%#Eval("isExcluded") %>' />
                                                    <asp:HiddenField ID="hdispackagemain" runat="server" Value='<%#Eval("ispackagemain")%>' />
                                                    <asp:HiddenField ID="hdUnderPackage" runat="server" Value='<%#Eval("underpackage")%>' />
                                                    <asp:HiddenField ID="hdnlblServType" runat="server" Value='<%# Convert.ToString(Eval("ServiceType")) %>' />
                                                    <asp:HiddenField ID="hdnIsPackageService" runat="server" Value='<%# Convert.ToString(Eval("ispackageservice")) %>' />
                                                    <asp:HiddenField ID="hdnIsPackageMain" runat="server" Value='<%# Convert.ToString(Eval("ispackagemain")) %>' />
                                                    <asp:HiddenField ID="hdnChargePercentage" runat="server" Value='<%# Convert.ToString(Eval("ChargePercentage")) %>' />
                                                    <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%# Convert.ToString(Eval("packageid")) %>' />
                                                    <asp:HiddenField ID="hdnIsPriceEditable" runat="server" Value='<%# Convert.ToString(Eval("IsPriceEditable")) %>' />
                                                    <asp:HiddenField ID="hdnServiceDiscountPercentage" runat="server" Value='<%# Convert.ToString(Eval("ServiceDiscountPercentage")) %>' />
                                                    <asp:HiddenField ID="hdnServiceInstructions" runat="server" Value='<%# Convert.ToString(Eval("ServiceInstructions")) %>' />
                                                    <asp:HiddenField ID="hdnServiceRemarks" runat="server" Value='<%# Convert.ToString(Eval("ServiceRemarks")) %>' />
                                                    <asp:HiddenField ID="hdnPOCRequest" runat="server" Value='<%#Eval("POCRequest") %>' />
                                                    <asp:HiddenField ID="hdnOutsourceInvestigation" runat="server" Value='<%# Convert.ToString(Eval("OutsourceInvestigation")) %>' />
                                                    <asp:HiddenField ID="hdnBiohazard" runat="server" Value='<%#Eval("IsBioHazard") %>' />
                                                    <asp:HiddenField ID="hdnStationId" runat="server" Value='<%#Eval("StationId")%>' />
                                                    <asp:HiddenField ID="hdnAssignToEmpId" runat="server" Value='<%#Eval("AssignToEmpId") %>' />
                                                    <asp:HiddenField ID="hdnAddOnTest" runat="server" Value='<%#Eval("IsAddOnTest") %>' />

                                                    <asp:HiddenField ID="hdnFromDate" runat="server" Value='<%#Eval("FromDate")%>' />
                                                    <asp:HiddenField ID="hdnToDate" runat="server" Value='<%#Eval("ToDate") %>' />
                                                    <asp:HiddenField ID="hdngvEquipmentType" runat="server" Value='<%#Eval("EquipmentType") %>' />
                                                    <asp:HiddenField ID="hdnIsBedChargeService" runat="server" Value='<%#Eval("IsBedChargeService") %>' />

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblInfoTotal" runat="server" Font-Bold="true" SkinID="label" Width="99%"
                                                        ForeColor="White" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Units" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtUnits" runat="server" MaxLength="4" SkinID="textbox" Text='<%#Eval("Units") %>'
                                                        Width="40px" Style="text-align: center;" />
                                                    <ajax:FilteredTextBoxExtender ID="FTBEUnits" runat="server" ValidChars="." FilterType="Custom,Numbers"
                                                        TargetControlID="txtUnits" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PRegistration, Provider %>" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctorID" runat="server" Text='<%#Eval("DoctorID") %>' />
                                                    <telerik:RadComboBox ID="ddlDoctor" runat="server" Filter="Contains" MarkFirstMatch="true"
                                                        Width="150px" Skin="Metro" Height="250px" DropDownWidth="300px">
                                                    </telerik:RadComboBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Amount" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtServiceAmount" runat="server" Enabled="false" SkinID="textbox"
                                                        Style="text-align: right;" Width="90%" Text='<%#Eval("ServiceAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Doctor Amount" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDoctorAmount" runat="server" Enabled="false" SkinID="textbox"
                                                        Style="text-align: right;" Width="90%" Text='<%#Eval("DoctorAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount %" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDiscountPercent" runat="server" Enabled="false" SkinID="textbox"
                                                        Style="text-align: right;" Width="90%" Text='<%#Eval("ServiceDiscountPercentage","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount Amt" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDiscountAmt" runat="server" Enabled="false" SkinID="textbox"
                                                        Style="text-align: right;" Width="90%" Text='<%#Eval("TotalDiscount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotDiscountAmt" runat="server" SkinID="label" ForeColor="White" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Net Charge" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNetCharge" runat="server" SkinID="textbox" Enabled="false" Width="90%"
                                                        Style="text-align: right;" Text='<%#Eval("NetCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Patient Payable" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmountPayableByPatient" runat="server" Enabled="false" SkinID="textbox"
                                                        Style="text-align: right;" Width="90%" Text='<%#Eval("AmountPayableByPatient","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblAmountPayableByPatient" runat="server" SkinID="label" ForeColor="White" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payer Payable" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmountPayableByPayer" runat="server" Enabled="false" SkinID="textbox"
                                                        Style="text-align: right;" Width="90%" Text='<%#Eval("AmountPayableByPayer","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblAmountPayableByPayer" runat="server" SkinID="label" ForeColor="White" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Investigation Date" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvestigationDate" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy HH:mm tt}",Eval("InvestigationDate")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("ServiceRemarks") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hndSerId" runat="server" Value='<%#Eval("ServiceId")%>' />
                                                    <asp:ImageButton ID="ibtnE" runat="server" ImageUrl="~/Images/edit.png" CausesValidation="false"
                                                        ToolTip="Edit" OnClick="ibtnE_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                        CommandArgument='<%#Eval("ServiceId")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <%--// //yogesh 3/10/2022--%>
                           

                             <div id="DivFeb" runat="server" class="col-md-4">
                                        <div class="col-md-12">
                                            <div class="row form-group">
                                                <asp:Panel ID="Panel3" runat="server" DefaultButton="imgSearchFev">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="col-md-2 col-sm-4 label1">
                                                                <asp:Label ID="Label14" runat="server" Text="Search" />
                                                            </div>
                                                            <div class="col-md-10 col-sm-8" style="position: relative;">
                                                                <asp:TextBox ID="txtFevSearch" runat="server" OnTextChanged="txtFevSearch_TextChanged" AutoPostBack="true" SkinID="textbox" />
                                                                <asp:ImageButton ID="imgSearchFev" runat="server" OnClick="imgSearch_Click" ImageUrl="~/Images/searchnew.jpg" Style="position: absolute; right: 0;" CssClass="btn btn-primary" ToolTip="Serching" />
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="">
                                            <asp:GridView ID="gvfaveroite" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                                ShowFooter="true" SkinID="gridviewOrderNew" Width="100%" OnRowCommand="gvfaveroite_RowCommand1"
                                                HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                                HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Favorite Service" HeaderStyle-Width="70%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceNName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                            <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%#Eval("ServiceType")%>' />
                                                            <asp:HiddenField ID="hdnServiceLink" runat="server" Value='<%#Eval("ServiceType")%>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="ibtnAddToList" CommandArgument='<%#Eval("ServiceId")%>' runat="server" CommandName="AddToList" ToolTip="Add To List" Text="Add" Width="20" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnfaDelete" runat="server" CommandName="FavDel" CausesValidation="false"
                                                                CommandArgument='<%#Eval("ServiceId")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                            <asp:HiddenField ID="hdnDocId" runat="server" Value='<%#Eval("ServiceId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
















                        </asp:Panel>
                    </div>

                </div>

                <div class="container-fluid header_main">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <table width="100%">
                            <tr>
                                <td width="30%">
                                    <h2>Service Instructions</h2>
                                </td>
                                <td width="70%"><span id="pServiceInstructions" style="color: red; font-weight: bold;"></span></td>
                            </tr>
                        </table>
                        <h2></h2>

                    </div>
                </div>
                <div class="container-fluid header_main">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <table width="100%">
                            <tr>
                                <td width="30%">
                                    <h2>Service Remarks</h2>
                                </td>
                                <td width="70%">
                                    <asp:Label ID="lblServiceRemarks" runat="server" SkinID="label" Text="Service Remarks" Font-Bold="true" ForeColor="red"></asp:Label></td>
                            </tr>
                        </table>

                    </div>
                </div>

                <div class="container-fluid">
                    <div>
                        <ucl:legend ID="Legend1" runat="server" />


                        <asp:UpdatePanel ID="updDivConfirm" runat="server">
                            <ContentTemplate>

                                <div id="dvConfirmPrintingOptions" runat="server" style="width: 400px; z-index: 200; border: 2px solid #60AFC3; background-color: #A8D9E6; position: fixed; bottom: 35%; height: 155px; left: 38%;">

                                    <div class="col-md-12 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-xs-4 col-md-4">
                                                <asp:Label ID="lblSn" runat="server" Text="Service name :" ForeColor="#990066" />
                                            </div>
                                            <div class="col-xs-8 col-md-8">
                                                <asp:Label ID="lblServiceName" runat="server" ForeColor="#990066" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-xs-4 col-md-4">
                                                <asp:Label ID="Label3" runat="server" Text="Already posted by :" ForeColor="#990066" />
                                            </div>
                                            <div class="col-xs-8 col-md-8">
                                                <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-xs-4 col-md-4">
                                                <asp:Label ID="Label6" runat="server" Text="Posted date :" ForeColor="#990066" />
                                            </div>
                                            <div class="col-xs-8 col-md-8">
                                                <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" />
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-md-12 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-xs-4 col-md-4">
                                                <asp:Label ID="lblAlertMsg" runat="server" Text="Do you wish to continue...?" ForeColor="#990066" />
                                            </div>
                                            <div class="col-xs-8 col-md-8"></div>
                                        </div>
                                    </div>

                                    <hr />
                                    <div class="col-md-12 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-md-12 col-xs-12 text-center">
                                                <asp:Button ID="btnYes" runat="server" Text="Proceed" OnClick="btnAlredyExist_OnClick" CssClass="btn btn-primary" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" CssClass="btn btn-default" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-md-4"></div>
                                            <div class="col-md-8"></div>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <table width="100%" cellpadding="0" cellspacing="0" style="background: none;">
                <tr>
                    <td>
                        <div id="divExcludedService" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 140px; left: 270px; top: 200px;">
                            <table cellspacing="2" cellpadding="2" width="400px">
                                <tr>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:Label ID="Label7" runat="server" Text="Service name :" ForeColor="#990066" />
                                    </td>
                                    <td style="width: 70%; text-align: left;">
                                        <asp:Label ID="lblExcludedServiceName" runat="server" ForeColor="#990066" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%; text-align: center;">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblExcludedService" runat="server" Text="Selected service is excluded for the payer."
                                                        ForeColor="#990066" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" Text="  Do you wish to continue ? " ForeColor="#990066" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: center;" colspan="2">
                                        <asp:Button ID="btnExcludedService" runat="server" Text="Proceed" OnClick="btnExcludedService_OnClick"
                                            SkinID="Button" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnExcludedServiceCancel" runat="server" Text="Cancel" OnClick="btnExcludedServiceCancel_OnClick"
                                            SkinID="Button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="background: none;">
                        <asp:UpdatePanel ID="uphidden" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server"
                                    Width="1200" Height="600" Left="10" Top="10">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close,Minimize,Maximize,Resize,Pin" InitialBehaviors="Maximize"
                                            Width="900" Height="600" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:HiddenField ID="hdnGlobleServiceType" runat="server" Value="" />
                                <asp:HiddenField ID="hdnGlobleStationId" runat="server" Value="" />
                                <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                <asp:HiddenField ID="hdnUniqueId" runat="server" Value="" />
                                <asp:HiddenField ID="hdnIsSaved" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                                <asp:HiddenField ID="hdnBilltype" runat="server" Value="0" />
                                <asp:HiddenField ID="hndEquipmentType" runat="server" Value="0" />
                                <asp:HiddenField ID="hndChargesPeriod" runat="server" Value="0" />
                                <asp:HiddenField ID="hndGracePeriod" runat="server" Value="0" />

                                <asp:Button ID="btnGetBalance" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                    Width="1" OnClick="btnGetBalance_OnClick" Text="" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>

            <table width="100%" cellpadding="0" cellspacing="0" style="background: none;">
                <tr>
                    <td>
                        <div id="divMiscellaneousRemarks" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 100px; left: 270px; top: 200px;">
                            <table cellspacing="2" cellpadding="2" width="400px">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label ID="Label10" runat="server" Text="Miscellaneous Service Remarks :" ForeColor="#990066" /><span style="color: Red">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: center;">
                                        <%--<asp:TextBox ID="txtMiscellaneousRemarks" runat="server" SkinID="textbox" TextMode="MultiLine" Width="95%"
                                            Height="50px" MaxLength="150"></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: center;">
                                        <asp:Button ID="btnContinue" runat="server" Text="Proceed" OnClick="btnContinue_OnClick"
                                            SkinID="Button" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnContinueCancel" runat="server" Text="Cancel" OnClick="btnContinueCancel_OnClick"
                                            SkinID="Button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divConfirmation" runat="server" style="width: 450px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 60px; left: 270px; top: 200px;">
                            <table cellspacing="2" cellpadding="2">
                                <tr>
                                    <td style="width: 100%; text-align: center;">
                                        <asp:Label ID="Label11" runat="server" Text="Selected service is blocked for this company. Do you Want to Continue?" ForeColor="#990066" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: center;">
                                        <asp:Button ID="btnProceed" runat="server" Text="Yes" OnClick="btnProceed_OnClick"
                                            SkinID="Button" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnProceedCancel" runat="server" Text="No" OnClick="btnProceedCancel_OnClick"
                                            SkinID="Button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <%--Start Service limit approval Alert--%>

            <div id="divServicelimit" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 140px; left: 270px; top: 200px;">
                <table cellspacing="2" cellpadding="2" width="400px">
                    <tr>
                        <td style="width: 25%; text-align: left;">
                            <asp:Label ID="lblSNameText" runat="server" Text="Service name :" ForeColor="#990066" />
                        </td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblSName" runat="server" ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            <br />
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblServcieLimit" runat="server" Text="Selected service is not under the limit ."
                                            ForeColor="#990066" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="  Do you wish to continue ? " ForeColor="#990066" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 100%; text-align: center;" colspan="2">
                            <asp:Button ID="btnServicelimit" runat="server" Text="Proceed" OnClick="btnServicelimit_Click"
                                SkinID="Button" />
                            &nbsp;&nbsp;
                                        <asp:Button ID="btnServicelimitCancel" runat="server" Text="Cancel" OnClick="btnServicelimitCancel_Click"
                                            SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>

            <%--END Service limit approval Alert--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
