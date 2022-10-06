<%@ Page Title="Drug/Consumable Order" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PrescribeMedication.aspx.cs" Inherits="EMR_Medication_PrescribeMedication" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
    <style>
        body#ctl00_Body1{
            overflow-x:hidden!important;
        }
        #ctl00_ContentPlaceHolder1_lblMessage {
            width: 100% !important;
            position: relative !important;
            margin: 0 !important;
        }

        td.rcbInputCell.rcbInputCellLeft .rcbInput {
            padding: 2px 10px !important;
        }

        div#ctl00_ContentPlaceHolder1_ddlGeneric {
            width: 100% !important;
        }
        

        .RadComboBox .rcbArrowCell a {
            height: 22px !important;
        }
       
        #ctl00_ContentPlaceHolder1_ddlBrand {
            margin-left: 0px !important;
        }

        .RadComboBox table td.rcbArrowCell {
            position: absolute;
            right: 6px !important;
            top: 0px !important;
        }

        textarea#ctl00_ContentPlaceHolder1_txtIsReadBackNote {
           
            display: inline-table !important;
            height: 23px !important;
        }

        textarea#ctl00_ContentPlaceHolder1_txtCustomMedication {
            width: 100% !important;
        }

        input#ctl00_ContentPlaceHolder1_chkFavouriteOnly {
            margin-top: 5px !important;
        }

        input#ctl00_ContentPlaceHolder1_ibtnFavourite {
            height: 20px !important;
            position: absolute !important;
        }
        table#ctl00_ContentPlaceHolder1_gvItem th{
            white-space:nowrap!important;
        }
    </style>


    <script language="javascript" type="text/javascript">
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
                    <%-- case 115:  // F4
                    $get('<%=btnAddItem.ClientID%>').click();
                    break;--%>
                case 118:  // F7
                    $get('<%=btnAddItem.ClientID%>').click();
                    break;
                case 119:  // F8
                    $get('<%=btnclose.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrint.ClientID%>').click();
                    break;

            }
            evt.returnValue = false;
            return false;
        }
        function OnClientItemsRequesting(sender, eventArgs) {
            var ddlgeneric = $find('<%= ddlGeneric.ClientID %>');
            var value = ddlgeneric.get_value();
            var context = eventArgs.get_context();
            context["GenericId"] = value;
        }



        function ddlGeneric_OnClientSelectedIndexChanged(sender, args) {
            var ddlbrand = $find("<%=ddlBrand.ClientID%>");
            ddlbrand.clearItems();
            ddlbrand.set_text("");
            ddlbrand.get_inputDomElement().focus();

            var item = args.get_item();
            $get('<%=hdnGenericId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnGenericName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=btnGetInfoGeneric.ClientID%>').click();
        }

        function ddlBrand_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnItemName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";
            $get('<%=hdnRestrictedItemForPanel.ClientID%>').value = item != null ? item.get_attributes().getAttribute("RestrictedItemForPanel") : "";
            $get('<%=hdnItemFlagName.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ItemFlagName") : "";
            $get('<%=hdnItemFlagCode.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ItemFlagCode") : "";
            $get('<%=hdnConversionFactor2Globel.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ConversionFactor2") : "";
            $get('<%=hdnLowCost.ClientID%>').value = item != null ? item.get_attributes().getAttribute("LowCost") : sender.text();

            $get('<%=btnGetInfo.ClientID%>').click();
        }
        function ddlBrandOnClientDropDownClosedHandler(sender, args) {

            if (sender.get_text().trim() == "") {

                $get('<%=hdnItemId.ClientID%>').value = "";
                $get('<%=hdnItemName.ClientID%>').value = "";

                $get('<%=hdnCIMSItemId.ClientID%>').value = "";
                $get('<%=hdnCIMSType.ClientID%>').value = "";
                $get('<%=hdnVIDALItemId.ClientID%>').value = "";
                $get('<%=hdnConversionFactor2Globel.ClientID%>').value = "";
                $get('<%=hdnLowCost.ClientID%>').value = "";

                $get('<%=btnGetInfo.ClientID%>').click();
            }
        }
        function OnClientClose(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnReturnIndentIds.ClientID%>').value = arg.IndentIds;
            $get('<%=hdnReturnItemIds.ClientID%>').value = arg.ItemIds;
            $get('<%=btnRefresh.ClientID%>').click();
        }
        function OnClientCloseFavourite(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnItemId.ClientID%>').value = arg.ItemIds;
            $get('<%=btnGetFavourite.ClientID%>').click();
        }
        function MaxLenTxt(TXT) {
            if (TXT.value.length > 200) {
                alert("Text length should not be greater then 200 ...");

                TXT.value = TXT.value.substring(0, 200);
                TXT.focus();
            }
        }

        function clickEnterInGrid(obj, event) {
            var keyCode;
            if (event.keyCode > 0) {
                keyCode = event.keyCode;
            }
            else if (event.which > 0) {
                keyCode = event.which;
            }
            else {
                keycode = event.charCode;
            }
            if (keyCode == 13) {
                document.getElementById(obj).focus();
                return false;
            }
            else {
                return true;
            }
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
                myButton.className = "ICCAViewerBtn";
                myButton.value = "Processing...";
            }
            return true;
        }

        function OnClientMedicationOverrideClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsOverride = arg.IsOverride;
                var OverrideComments = arg.OverrideComments;
                var DrugAllergyScreeningResult = arg.DrugAllergyScreeningResult;

                $get('<%=hdnIsOverride.ClientID%>').value = IsOverride;
                $get('<%=hdnOverrideComments.ClientID%>').value = OverrideComments;
                $get('<%=hdnDrugAllergyScreeningResult.ClientID%>').value = DrugAllergyScreeningResult;
            }

            $get('<%=btnMedicationOverride.ClientID%>').click();
        }
        function CalculateQty(txtPackSize, hdnConversionFactor, txtTotqty) {
            var totalQty = Number(0);
            var PackSize = Number(0);
            var ConversionFactor = Number(0);
            var balQty = Number(0);
            var retQty = Number(0);
            PackSize = $get(txtPackSize).value;
            ConversionFactor = $get(hdnConversionFactor).value;
            balQty = $get(txtTotqty).value;
            //retQty = $get(_returnQty).value;
            //if ((PackSize * ConversionFactor) > balQty) {
            //    {
            //        alert("Approved Quantity cann't be greater then Requested Quantity !");
            //        $get(_returnQty).value = retQty;
            //        return;
            //    }
            //}
            totalQty = PackSize * ConversionFactor;

            if (totalQty <= 0) {
                totalQty = 0;
            }

            $get(txtTotqty).value = totalQty;
        }

        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main">
                <div class="row">
                    <div class="col-md-2 col-sm-3 col-12">
                        <h2>
                            <%-- <asp:Image ID="Image1" ImageUrl="/Images/Assessment.png" Height="18px" Width="18px" runat="server" />--%>
                            <%--<asp:Image ID="Image1"  Height="18px" Width="18px" runat="server" />--%>
                            <asp:Label ID="lblTitle" runat="server" Text="" />
                        </h2>
                    </div>
                    <div class="col-lg-5 col-md-6 col-sm-9 col-12">
                        <div class="row">
                            <%-- <div class="col-md-4 PaddingRightSpacing label2">
                            <asp:Label ID="Label3" runat="server" Text="Store&nbsp;" />
                        </div>--%>
                            <div class="col-md-5 col-sm-5 col-4 PaddingRightSpacing" style="margin-top: 2px;">
                                <asp:CheckBox ID="ChkReqestFromOtherWard" OnCheckedChanged="ChkReqestFromOtherWard_OnCheckedChanged" Visible="true" AutoPostBack="true" runat="server" />
                                <asp:Label ID="lblRequestFromOtherWard" runat="server" Visible="true" Text="Request From Other Ward" />
                            </div>
                            <div class="col-md-3 col-sm-3 col-3">
                                <telerik:RadComboBox ID="ddlReqestFromOtherWard" runat="server" DropDownWidth="100%" Width="100%"
                                    EmptyMessage="[ Select ]" AutoPostBack="true" Visible="false" />
                            </div>
                            <div class="col-md-1 col-sm-1 col-1 PaddingRightSpacing">
                                <asp:Label ID="Label7" runat="server" Text="Store&nbsp;" />
                            </div>
                            <div class="col-md-3 col-sm-3 col-4" style="margin-top: 2px;">
                                <asp:DropDownList ID="ddlStore" runat="server" SkinID="DropDown" Width="100%"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-5 col-md-4 text-right mt-2 mt-md-0">

                        <asp:LinkButton ID="lnkStopMedication" runat="server" CssClass="btn btn-primary" OnClick="lnkStopMedication_OnClick" Text="Stop Medication" ToolTip="Click to see stop medication" />
                        <asp:LinkButton ID="btnPreviousMedications" runat="server" CssClass="btn btn-primary" OnClick="btnPreviousMedications_Click" Text="Previous Medications" />
                        <asp:Button ID="btnSave" Text="Save " ToolTip="Save(Ctrl+F3)" runat="server" OnClick="btnSave_Onclick"
                            CssClass="btn btn-primary" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" ToolTip="Print (Ctrl+F9)" OnClick="btnPrint_Click"
                            Visible="false" CausesValidation="false" />
                        <asp:Button ID="btnclose" Text="Close " ToolTip="Close (Ctrl+F8)" runat="server" CssClass="btn btn-primary" Visible="false"
                            OnClientClick="window.close();" />
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <span class="col-12 text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="" /></span>
                </div>
            </div>
            <div class="row form-group">
                <div class="col-12">
                    <%--<asp:Label ID="lblPatientDetail" runat="server" Text=""></asp:Label>--%>
                    <asplUD:UserDetails ID="asplUD" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <hr style="height: -12px; margin: 0 0 5px; padding: 0;" />
                </div>
            </div>
            <div class="container-fluid" style="background-color: #fff !important;">

                <div class="row form-groupTop01">

                    <div class="col-md-4">
                        <div class="row" id="dvParas" runat="server" visible="false">
                            <div class="col-md-4 label2">
                                <asp:Label ID="Label1" runat="server" Text="Unique Category" />
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlUniqueCategory" CssClass=" vertical-top" runat="server" Width="100%"
                                    ShowMoreResultsBox="true" DropDownWidth="200px" EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-6">
                        <div class="row" id="trGeneric" runat="server">
                            <div class="col-lg-2 col-md-3 label2">
                                <asp:Label ID="Label16" runat="server" Text="Generic" />
                            </div>
                            <div class="col-lg-10 col-md-9">
                                <telerik:RadComboBox ID="ddlGeneric" runat="server" Height="300px"
                                    EmptyMessage="" AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true"
                                    OnItemsRequested="ddlGeneric_OnItemsRequested" DataTextField="GenericName" DataValueField="GenericId"
                                    Skin="Office2007" ShowMoreResultsBox="true" Width="100%" EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-6">
                        <div class="row">
                            <div class="col-lg-5 col-md-5 label2 ">
                                <asp:Label ID="lblIndentType" runat="server" Text="Indent type"></asp:Label>
                            </div>
                            <div class="col-lg-7 col-md-7" >
                                <telerik:RadComboBox ID="ddlIndentType" runat="server" Filter="Contains" Skin="Office2007" MarkFirstMatch="true" Width="100%"></telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-6">
                        <div class="row">
                            <div class="col-lg-4 col-md-5 label2">
                                <asp:Label ID="Label18" runat="server" Text="Advising&nbsp;Doctor" />
                            </div>
                            <div class="col-md-7 col-lg-8">
                                <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" Skin="Office2007" EmptyMessage="[ Select ]" MarkFirstMatch="true" Width="100%" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-6" style="display: none">
                        <div class="row">
                            <div class="col-md-5  label2">
                                <asp:Label ID="Label2" runat="server" Text="Provisional&nbsp;Diagnosis" />
                            </div>
                            <div class="col-md-7">
                                <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" MaxLength="1000" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this, 1000);" />
                                <div id="tdDrugAllergy" runat="server">
                                    <asp:LinkButton ID="lnkDrugAllergy" runat="server" OnClick="lnkDrugAllergy_OnClick" BackColor="#82CAFA" Text="Drug&nbsp;Allergy" ToolTip="Drug&nbsp;Allergy" Font-Bold="true" Visible="false" />
                                </div>

                            </div>
                        </div>
                    </div>
               
                <div id="dvConfirm" runat="server" visible="false" style="width: 240px; z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed; bottom: 35%; height: 115px; left: 35%;">
                    <table width="100%" cellspacing="2" border="0">
                        <tr>
                            <td style="width: 50px"></td>
                            <td colspan="2">
                                <asp:Label ID="lblConfirmHighValue" Style="font-size: 12px; font-weight: bold; margin: 0.5em 0 0; padding: 0; width: 100%; float: left;"
                                    runat="server" Text="This is high value item do you want to proceed" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;&nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnYes" CssClass="ICCAViewerBtn" runat="server" Text="Yes" OnClick="btnYes_OnClick" Width="50px" />
                                &nbsp; 
                                <asp:Button ID="btnCancel" CssClass="ICCAViewerBtn" runat="server" Text="No" OnClick="btnCancel_OnClick" Width="50px" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                
                    <div class="col-md-4 col-6">
                        <div class="row" id="trDrugs" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblInfoBrand" runat="server" Text="Brand" Font-Bold="true" />
                            </div>
                            <div class="col-md-10">
                                <telerik:RadComboBox ID="ddlBrand" runat="server" HighlightTemplatedItems="true"
                                    Width="100%" DropDownWidth="500px" EmptyMessage="[ Search Brands By Typing Minimum 3 Characters ]"
                                    AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlBrand_OnItemsRequested"
                                    Skin="Office2007" OnClientItemsRequesting="OnClientItemsRequesting" ShowMoreResultsBox="true"
                                    OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged" Height="300px" OnClientDropDownClosed="ddlBrandOnClientDropDownClosedHandler"
                                    EnableVirtualScrolling="true">
                                    <HeaderTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 80%" align="left">
                                                    <asp:Literal ID="Literal2" runat="server" Text="Item Name"></asp:Literal>
                                                </td>
                                                <%--<td style="width: 10%" align="center">Stock
                                                </td>--%>
                                                <td style="width: 10%" align="right" runat="server" id="tdClosingBalance">
                                                    <asp:Label ID="lblClosingBalance" runat="server" Text="QTY" />
                                                </td>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />
                                                </td>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                </td>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
                                                </td>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttConversionFactor2" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['ConversionFactor2']")%> />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 80%" align="left">
                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                </td>
                                                <td style="width: 10%" align="left">
                                                    <%--     <%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>--%>
                                                    <asp:Label ID="lblClosingBalanceItem" runat="server" Text='<%# DataBinder.Eval(Container, "Attributes[\"ClosingBalance\"]")%>' />
                                                </td>
                                                <td style="width: 10%" align="left" runat="server" visible="false">
                                                    <%# DataBinder.Eval(Container, "Attributes['LowCost']")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="row" id="trCustomMedication" runat="server" visible="false">
                            <div class="col-md-2 label2">
                                <asp:Label ID="Label23" runat="server" Text="Medicine" />
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCustomMedication" runat="server" MaxLength="1000"
                                    Height="20px" TextMode="MultiLine"  onkeyup="return MaxLenTxt(this, 1000);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-5 col-12" style="display: inline-table;">

                        <div class="PD-TabRadio margin_z">
                            <asp:CheckBox ID="chkCustomMedication" runat="server" AutoPostBack="true" OnCheckedChanged="chkCustomMedication_OnCheckedChanged" Text="Custom&nbsp;Medication" />
                        </div>
                        <div class="PD-TabRadio margin_z" style="margin-left: 8px!Important;">
                            <asp:CheckBox ID="chkNotToPharmacy" runat="server" Text="Own Medication" />
                        </div>
                        <div class="PD-TabRadio margin_z" style="margin-left: 8px!Important;">
                            <asp:CheckBox ID="chkApprovalRequired" runat="server" Text="Verbal/Telephonic" AutoPostBack="true" TextAlign="Right" Visible="false" OnCheckedChanged="chkApprovalRequired_OnCheckedChanged" />
                        </div>
                         <div class="PD-TabRadio margin_z" style="margin-left: 8px!Important;">
                              <asp:CheckBox ID="chkIsReadBack" runat="server" TabIndex="43" Text="Read Back" TextAlign="Right" Visible="false" CssClass="pull-right" />
                         </div>
                        
                       
                    </div>
             
                    <div class="col-lg-3 col-6">
                        <div class="row">
                            <div class="col-md-4 col-5">
                                <asp:Label ID="lblReadBackNote" runat="server" Text="Read&nbsp;Back&nbsp;Note" Visible="false" CssClass="vertical-top"></asp:Label>
                            </div>
                            <div class="col-md-8 col-7">
                                <asp:TextBox ID="txtIsReadBackNote" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Visible="false" CssClass="vertical-top"></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-5 text-right">
                                <asp:Label ID="lblGenericName" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                            </div>
                            <div class="col-md-8 col-7 text-right">
                                <asp:LinkButton ID="lnkLabHistory" runat="server" OnClick="lnkLabHistory_OnClick" Text="DIAGNOSTIC HISTORY" CssClass="btn" Visible="false" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-6" style="display: none">
                        <div class="row">
                            <div class="col-md-2 label2">
                                <asp:Label ID="Label8" runat="server" Text="Form" />
                            </div>
                            <div class="col-md-10">
                                <telerik:RadComboBox ID="ddlFormulation" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]" Width="100%" DropDownWidth="230px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-6" style="display: none">
                        <div class="row">
                            <div class="col-md-2 label2">
                                <asp:Label ID="Label9" runat="server" Text="Route" />
                            </div>
                            <div class="col-md-10">
                                <telerik:RadComboBox ID="ddlRoute" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]" Width="100%" DropDownWidth="230px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-6" style="display: none;">
                        <div class="row">
                            <div class="col-md-2 label2">
                                <asp:Label ID="Label10" runat="server" Text="Strength" />
                            </div>
                            <div class="col-md-10">
                                <telerik:RadComboBox ID="ddlStrength" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]" Width="100%" />
                            </div>
                        </div>
                    </div>




              
                    <div class="col-md-4 col-6">
                        <div class="row" style="vertical-align: top">
                            <div class="col-md-6 col-6 label2">
                                <asp:CheckBox ID="chkFavouriteOnly" runat="server" SkinID="checkbox" Text="Favourite Drug Only"
                                    AutoPostBack="true" OnCheckedChanged="chkFavouriteOnly_OnCheckedChanged"  style="white-space: nowrap;" />
                            </div>
                            <div class="col-md-6 col-6" style="vertical-align: top; margin-top: 5px;">
                                <asp:Button ID="btnAddtoFav" runat="server" SkinID="button" Text="Add To Favourite"
                                    ToolTip="Add To Favourite" OnClick="btnAddtoFav_Click" />
                                <asp:ImageButton ID="ibtnFavourite" runat="server" ImageUrl="~/Images/PopUp.jpg" ToolTip="Add new Favourite" OnClick="ibtnFavourite_Click" CausesValidation="false" />

                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row" id="dvReason" runat="server" visible="false">
                            <div class="col-md-5 label2">
                                <asp:Label ID="Label3" runat="server" Text="Reason" /><%--<span style="color: Red">*</span>--%>
                            </div>
                            <div class="col-md-7">
                                <telerik:RadComboBox ID="ddlReason" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-12 mt-2 mb-2">
                        <div class="row">
                            <div class="col-lg-2 col-md-4 col-2 label2">Remarks</div>
                            <div class="col-lg-10 col-md-8 col-10">
                                <asp:TextBox ID="txtremarks" runat="server" MaxLength="250" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>




            </div>
            <div class="row">
                <div class="col-12">
                <hr style="height: -12px; margin: 0; padding: 0;" />
                    </div>
            </div>
            <div class="container-fluid">
                <div class="row form-group">

                    <div class="col-md-7">
                        <div class="row form-groupTop">
                            <div class="col-md-3 label2" style="display: none;">
                                <asp:Label ID="lblDrugDetail" runat="server" Text="Drug Detail" Font-Underline="true" Font-Bold="true"></asp:Label>
                            </div>

                            <div class="col-md-9">
                                <div class="row" style="display: none">
                                    <div class="col-md-3">
                                        <div class="row">
                                            <div class="col-md-8 label2">
                                                <asp:Label ID="Label11" runat="server" Text="Height&nbsp;(Cm)" Font-Bold="true" />
                                            </div>
                                            <div class="col-md-4 PaddingLeftSpacing">
                                                <asp:TextBox ID="txtHeight" runat="server" BackColor="#D3D3D3" ReadOnly="true" Width="100%" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="row">
                                            <div class="col-md-7 label2">
                                                <asp:Label ID="Label12" runat="server" Text="&nbsp;Weight&nbsp;(Kg)" Font-Bold="true" />
                                            </div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="lbl_Weight" runat="server" ReadOnly="true" BackColor="#D3D3D3" Width="100%" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="row">
                                            <div class="col-md-5 label2">
                                                <asp:Label ID="Label19" runat="server" Text="&nbsp;BSA" Font-Bold="true" />
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="lbl_BSA" runat="server" ReadOnly="true" BackColor="#D3D3D3" Width="100%" />
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class="col-md-3">
                                        <asp:Button ID="btnSurgicalKit" runat="server" Text="Item Kits" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnSurgicalKit_OnClick" />
                                    </div>--%>
                                </div>
                                <%--<div class="row">
                                    <div class="col-md-3">
                                        <asp:Button ID="btnSurgicalKit" runat="server" Text="Item Kits" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnSurgicalKit_OnClick" />
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>


                    <%-- <div class="col-md-5 text-right">
                        <div class="row form-groupTop">
                            <div class="col-md-12">
                                <asp:Label ID="lblGenericName" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                <asp:LinkButton ID="lnkLabHistory" runat="server" OnClick="lnkLabHistory_OnClick" Text="DIAGNOSTIC HISTORY" CssClass="btn" Visible="false" />
                            </div>
                        </div>

                    </div>--%>
                </div>
                <div id="divgvItemDetails" runat="server">
                    <div class="col-md-12">
                        <div class="row">
                            <asp:GridView ID="gvItemDetails" CssClass="table-responsive" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                SkinID="gridviewOrderNew" AutoGenerateColumns="False" CellSpacing="0" CellPadding="0"
                                OnRowDataBound="gvItemDetails_OnRowDataBound" OnRowCommand="gvItemDetails_OnRowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                        ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:HiddenField ID="HdnId" runat="server" Value='<%# Eval("Id") %>' />
                                            <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                            <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                            <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%# Eval("RouteId") %>' />
                                            <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%# Eval("StrengthId") %>' />
                                            <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                            <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                            <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                            <asp:HiddenField ID="hdnQty" runat="server" Value='<%#Eval("Qty") %>' />
                                            <asp:HiddenField ID="hdnXMLData" runat="server" />
                                            <asp:HiddenField ID="hdnCustomMedication" runat="server" Value='<%#Eval("CustomMedication") %>' />
                                            <asp:HiddenField ID="hdnNotToPharmcy" runat="server" Value='<%#Eval("NotToPharmacy") %>' />
                                            <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                            <asp:HiddenField ID="hdnRouteName" runat="server" Value='<%#Eval("RouteName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Frequency" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="ddlFrequencyId" runat="server" Width="100%" DropDownWidth="200px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Referance Drug" Visible="false" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="ddlReferanceItem" runat="server" Width="100%" DropDownWidth="400px"
                                                AppendDataBoundItems="true">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Diluents" Value="0" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Duration" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDuration" runat="server" Text='<%#Eval("Duration") %>' Width="30px" Style="text-align: right"
                                                AutoPostBack="true" OnTextChanged="txtDuration_OnTextChanged" MaxLength="2" />
                                            <telerik:RadComboBox ID="ddlPeriodType" runat="server" Width="85px" DropDownWidth="120px"
                                                OnSelectedIndexChanged="ddlPeriodType_OnSelectedIndexChanged" AutoPostBack="true">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Minute(s)" Value="N" />
                                                    <telerik:RadComboBoxItem Text="Hour(s)" Value="H" />
                                                    <telerik:RadComboBoxItem Text="Day(s)" Value="D" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="Week(s)" Value="W" />
                                                    <telerik:RadComboBoxItem Text="Month(s)" Value="M" />
                                                    <telerik:RadComboBoxItem Text="Year(s)" Value="Y" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtDuration" ValidChars="">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instructions" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInstructions" runat="server" Height="20px" Width="100%"
                                                TextMode="MultiLine" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dose" HeaderStyle-Width="30px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDose" runat="server" Text='<%#Eval("Dose") %>'
                                                Width="100%" MaxLength="7" Style="text-align: right" />
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtDose" ValidChars=".">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="60px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="ddlUnit" Filter="Contains" runat="server" Width="100%" DropDownWidth="130px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dose Type" HeaderStyle-Width="60px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="ddlDoseType" runat="server" Width="100%" DropDownWidth="100px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDoseType_OnSelectedIndexChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Food Relationship" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <telerik:RadComboBox ID="ddlFoodRelation" runat="server" Width="100%" DropDownWidth="160px"
                                                ToolTip="Relationship to Food" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <telerik:RadDatePicker ID="txtStartDate" runat="server" Width="100%" AutoPostBack="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                                                OnSelectedDateChanged="txtStartDate_OnSelectedDateChanged" DbSelectedDate='<%#Eval("StartDate")%>'>
                                                <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" />
                                            </telerik:RadDatePicker>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                                        Visible="false">
                                        <ItemTemplate>
                                            <telerik:RadDatePicker ID="txtEndDate" runat="server" Width="100%" Enabled="false"
                                                DbSelectedDate='<%#Eval("EndDate")%>'>
                                                <DateInput ID="DateInput2" runat="server" DateFormat="dd/MM/yyyy" />
                                            </telerik:RadDatePicker>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="New&nbsp;Dose" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAddNewRow" runat="server" Text="New&nbsp;Dose" Font-Underline="true"
                                                OnClick="lnkAddNewRow_OnClick" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("Id")%>'
                                                ImageUrl="~/Images/DeleteRow.png" Width="12px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 pull-right">
                    <div class="form-group">
                        <asp:Button ID="btnSurgicalKit" runat="server" Text="Item Kits" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnSurgicalKit_OnClick" />
                        &nbsp; &nbsp;
                    <asp:Button ID="btnAddItem" runat="server" CssClass="btn btn-primary" Text="Add To List" ToolTip="Add To List  (Ctrl+F7)" OnClick="btnAddItem_OnClick" />
                    </div>
                </div>
                <div id="dvConfirmAlreadyExistOptions" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 150px; left: 270px; top: 200px;">
                    <table cellspacing="2" cellpadding="2" width="400px">
                        <tr>
                            <td style="width: 30%; text-align: left;">
                                <asp:Label ID="lblSn" runat="server" Text="Drug Name :" ForeColor="#990066" />
                            </td>
                            <td style="width: 70%; text-align: left;">
                                <asp:Label ID="lblItemName" runat="server" ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; text-align: left;">
                                <asp:Label ID="Label62" runat="server" Text="Already Order By :" ForeColor="#990066" />
                            </td>
                            <td style="width: 70%; text-align: left;">
                                <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr style="border-bottom-style: solid; border-bottom-width: 1px;">
                            <td style="width: 30%; text-align: left;">
                                <asp:Label ID="Label63" runat="server" Text="Order date :" ForeColor="#990066" />
                            </td>
                            <td style="width: 70%; text-align: left;">
                                <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%; text-align: center;">
                                <asp:Label ID="lblAlertMsg" runat="server" Font-Size="12px" Text="Do you wish to continue...?"
                                    ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%; text-align: center;">
                                <asp:Button ID="btnAlredyExistProceed" runat="server" Text="Proceed" Font-Size="Smaller"
                                    OnClick="btnAlredyExistProceed_OnClick" SkinID="Button" />
                                &nbsp;&nbsp;
                            <asp:Button ID="btnAlredyExistCancel" runat="server" Text="Cancel" Font-Size="Smaller"
                                OnClick="btnAlredyExistCancel_OnClick" SkinID="Button" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblPrescription" runat="server" Text="Prescription" Font-Underline="true" Font-Bold="true" Visible="false" />
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-8 PaddingSpacing">
                        <asp:GridView ID="gvItem" runat="server" Width="100%" Height="100%" AllowPaging="false"
                            SkinID="gridviewOrderNew" AutoGenerateColumns="False" OnRowDataBound="gvItem_OnRowDataBound"
                            OnRowCommand="gvItem_OnRowCommand" CssClass="table table-condensed table-bordered" HeaderStyle-BackColor="#ccccff">
                            <Columns>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                        <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                        <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                        <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                        <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                        <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                        <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                        <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                        <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                        <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                        <asp:HiddenField ID="hdnXMLData" runat="server" Value='<%#Eval("XMLData") %>' />
                                        <asp:HiddenField ID="hdnCustomMedication" runat="server" Value='<%#Eval("CustomMedication") %>' />
                                        <asp:HiddenField ID="hdnNotToPharmcy" runat="server" Value='<%#Eval("NotToPharmacy") %>' />
                                        <asp:HiddenField ID="hdnItemFlagName" runat="server" Value='<%# Eval("ItemFlagName") %>' />
                                        <asp:HiddenField ID="hdnItemFlagCode" runat="server" Value='<%# Eval("ItemFlagCode") %>' />
                                        <%--<asp:HiddenField ID="hdnRestrictedItemForPanel" runat="server" Value='<%#Eval("RestrictedItemForPanel") %>' />--%>
                                        <%-- <asp:HiddenField ID="hdnCustomId" runat="server" Value='<%#Eval("Id") %>'/>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--HeaderStyle-Width="250px" ItemStyle-Width="250px"--%>
                                <asp:TemplateField HeaderText="Drug Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'
                                            Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndentType" runat="server" Text='<%# Eval("IndentType") %>' />
                                        <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText='Pack&nbsp;Size' HeaderStyle-Width="40px" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPackSize" runat="server" Text='<%#Eval("PackSize")%>' />
                                        <asp:HiddenField ID="hdnConversionFactor2" runat="server" Value='<%#Eval("ConversionFactor2")%>' />
                                        <AJAX:FilteredTextBoxExtender ID="FTBtxtPackSize" runat="server" FilterType="Numbers" TargetControlID="txtPackSize"></AJAX:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                    <ItemStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Req. Qty." HeaderStyle-Width="60px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotalQty" runat="server" Width="100%" MaxLength="7"
                                            Text='<%#Eval("Qty") %>' Style="text-align: right" />
                                        <%--<asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="50px"></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="60px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemUnit" runat="server" Width="100%" SkinID="label" Text='<%# Eval("UnitName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                            CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                            Text="Mon." Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                            CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                            Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                            CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                            Text="&nbsp;" Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                            CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                            Text="Mon." Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                            CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                            Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                            CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                            Text="&nbsp;" Width="100%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ibtnEdit" runat="server" CommandName="Select" CausesValidation="false"
                                            CommandArgument='<%#Eval("ItemId")%>' Text="Edit" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                            CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                            ImageUrl="~/Images/DeleteRow.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-md-4">
                        <div style="height: auto; width: 100%; overflow: auto;">
                            <asp:Label ID="lblCurrentMedication" runat="server" Text="Current Medication" Font-Underline="true"
                                Font-Bold="true" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
                <div>

                    <table cellpadding="0" cellspacing="4">
                        <tr>

                            <td>
                                <asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="LightGray" Width="18px"
                                    Height="18px" />
                            </td>
                            <td>
                                <asp:Label ID="Label70" runat="server" Height="14px" Text="LASA" />&nbsp;&nbsp;
                            </td>
                            &nbsp;&nbsp;
                                    <td>
                                        <asp:Label ID="Label71" runat="server" BorderWidth="1px" BackColor="Yellow" Width="18px"
                                            Height="18px" />&nbsp;&nbsp;
                                    </td>
                            <td>
                                <asp:Label ID="Label72" runat="server" Height="14px" Text="Schedule H1, High value, High alert" />&nbsp;&nbsp;
                            </td>

                             &nbsp;&nbsp;
                                    <td>
                                        <asp:Label ID="Label4" runat="server" BorderWidth="1px" BackColor="Olive" Width="18px"
                                            Height="18px" />&nbsp;&nbsp;
                                    </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Height="14px" Text="High Risk" />&nbsp;&nbsp;
                            </td>

                        </tr>
                    </table>
                </div>

                <div class="row form-group">
                    <asp:GridView ID="gvPrevious" runat="server" Width="100%" Height="100%" AllowPaging="false"
                        SkinID="gridviewOrderNew" AutoGenerateColumns="False" OnRowDataBound="gvPrevious_OnRowDataBound"
                        OnRowCommand="gvPrevious_OnRowCommand" Visible="false">
                        <Columns>
                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                    <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                    <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                    <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                    <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                    <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                    <%-- <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId") %>' />--%>
                                    <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                    <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                    <asp:HiddenField ID="hdnXMLData" runat="server" />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="60px" ItemStyle-Width="60px" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentNo" runat="server" Text='<%# Eval("IndentNo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentDate" runat="server" Text='<%# Eval("IndentDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--HeaderStyle-Width="250px" ItemStyle-Width="250px"--%>
                            <asp:TemplateField HeaderText="Drug Name" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentType" runat="server" Text='<%# Eval("IndentType") %>' />
                                    <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="40px" ItemStyle-Width="40px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="40px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="110px" ItemStyle-Width="170px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="70px" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stop Remarks" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRemarks" runat="server" Height="20px" Width="100%"
                                        TextMode="MultiLine" Text="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                        CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                        Text="Mon." Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                        CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                        Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                        CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                        Text="&nbsp;" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                        CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                        Text="Mon." Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                        CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                        Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                        CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                        Text="&nbsp;" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" HeaderText="Stop">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnStop" runat="server" ToolTip="Click here to stop this drug"
                                        CommandName="ItemStop" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                        ImageUrl="~/Images/Redtick71.GIF" Width="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Cancel">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click here to cancel this drug"
                                        CommandName="ItemCancel" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                        ImageUrl="~/Images/close_new-old.jpg" Width="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div id="dvConfirmStop" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 100px; left: 520px; top: 220px">
                        <table width="100%" cellspacing="2" cellpadding="0">
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblCancelStopMedicationRemarks" Font-Size="12px" runat="server" Font-Bold="true"
                                        Text="Cancel Medication Remarks" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="txtStopRemarks" SkinID="textbox" runat="server" TextMode="MultiLine"
                                        Style="min-height: 45px; max-height: 45px; min-width: 390px; max-width: 390px;"
                                        MaxLength="200" onkeyup="return MaxLenTxt(this, );" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="btnStopMedication" SkinID="Button" runat="server" Text="Cancel" OnClick="btnStopMedication_OnClick" />
                                    &nbsp;
                                        <asp:Button ID="btnStopClose" SkinID="Button" runat="server" Text="Close" OnClick="btnStopClose_OnClick" />
                                </td>
                                <td align="center"></td>
                            </tr>
                        </table>
                    </div>
                </div>

            </div>







            <table border="0" cellpadding="0" cellspacing="1" width="100%" runat="server" visible="false">
                <tr>
                    <td width="305px" valign="top">
                        <asp:Panel ID="Panel4" runat="server" Style="border-width: 1px; border-color: LightBlue; border-style: solid;"
                            Width="100%" Height="290px" ScrollBars="Auto" Visible="false">
                            <table border="0" width="99%" cellpadding="0" cellspacing="1" style="margin-left: 2px">
                                <tr>
                                    <td id="tdTemplate" runat="server"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="2">
                <tr>
                    <td>
                        <div id="dvInteraction" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 300px; top: 160px">
                            <table width="100%" cellspacing="2" cellpadding="0">
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Label ID="Label25" Font-Size="12px" runat="server" Font-Bold="true" Text="This drug has interaction with prescribed medicines !" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"></td>
                                    <td align="center">
                                        <asp:Button ID="btnInteractionView" CssClass="btn btn-primary" runat="server" Text="Interaction View"
                                            OnClick="btnInteractionView_OnClick" />
                                        <asp:Button ID="btnMonographView" CssClass="btn btn-primary" runat="server" Text="Monograph View"
                                            OnClick="btnMonographView_OnClick" />
                                        <asp:Button ID="btnInteractionContinue" CssClass="btn btn-primary" runat="server" Text="Continue"
                                            OnClick="btnInteractionContinue_OnClick" />
                                        <asp:Button ID="btnInteractionCancel" CssClass="btn btn-primary" runat="server" Text="Cancel"
                                            OnClick="btnInteractionCancel_OnClick" />
                                    </td>
                                    <td align="center"></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="display: none;">
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnLowCost" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnReturnIndentIds" runat="server" />
                        <asp:HiddenField ID="hdnReturnItemIds" runat="server" />
                        <asp:HiddenField ID="hdnStoreId" runat="server" />
                        <asp:HiddenField ID="hdnGenericId" runat="server" />
                        <asp:HiddenField ID="hdnGenericName" runat="server" />
                        <asp:HiddenField ID="hdnItemId" runat="server" />
                        <asp:HiddenField ID="hdnItemName" runat="server" />
                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                        <asp:HiddenField ID="hdnCIMSType" runat="server" />
                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" />
                        <asp:HiddenField ID="hdnConversionFactor2Globel" runat="server" />
                        <asp:HiddenField ID="hdnTotalQty" runat="server" />
                        <asp:HiddenField ID="hdnInfusion" runat="server" />
                        <asp:HiddenField ID="hdnRestrictedItemForPanel" runat="server" />

                        <asp:HiddenField ID="hdnItemFlagName" runat="server" />
                        <asp:HiddenField ID="hdnItemFlagCode" runat="server" />

                        <asp:Button ID="btnGetInfo" runat="server" Text="" CausesValidation="false" SkinID="button"
                            Style="visibility: hidden;" OnClick="btnGetInfo_Click"  OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        <asp:Button ID="btnGetInfoGeneric" runat="server" Text="" CausesValidation="false"
                            SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfoGeneric_Click" />
                        <asp:Button ID="btnRefresh" runat="server" Style="visibility: hidden;" OnClick="btnRefresh_OnClick" />
                        <asp:Button ID="btnGetFavourite" runat="server" Style="visibility: hidden;" OnClick="btnGetFavourite_OnClick" />
                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
            <div id="dvConfirmProfileItem" runat="server" visible="false" style="width: 500px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 300px; top: 180px">
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
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center"></td>
                        <td align="center">
                            <asp:Button ID="btnOkProfileItem" SkinID="Button" runat="server" CausesValidation="false"
                                Text="Ok" OnClick="btnOkProfileItem_OnClick" />
                            &nbsp;
                            <asp:Button ID="btnCancelProfileItem" SkinID="Button" CausesValidation="false" runat="server"
                                Text="Cancel" OnClick="btnCancelProfileItem_OnClick" />
                        </td>
                        <td align="center"></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnMedicationOverride" runat="server" Text="" CausesValidation="true"
        SkinID="button" Style="visibility: hidden;" OnClick="btnMedicationOverride_OnClick" />
    <asp:HiddenField ID="hdnIsOverride" runat="server" Value="" />
    <asp:HiddenField ID="hdnOverrideComments" runat="server" Value="" />
    <asp:HiddenField ID="hdnDrugAllergyScreeningResult" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
        Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
</asp:Content>
