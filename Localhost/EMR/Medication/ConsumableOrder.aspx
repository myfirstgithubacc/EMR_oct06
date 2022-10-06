<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ConsumableOrder.aspx.cs" Inherits="EMR_Medication_ConsumableOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
    <style>
        #ctl00_ContentPlaceHolder1_lblMessage {
            width: 100% !important;
            position: relative !important;
            margin: 0 !important;
        }

        #ctl00_ContentPlaceHolder1_ddlBrand {
            margin: 0px !important;
        }

        table#ctl00_ContentPlaceHolder1_tblLegend_ItemList {
            width: 100%;
        }

            table#ctl00_ContentPlaceHolder1_tblLegend_ItemList td {
                background: #ff0;
                text-align: center;
                padding: 5px;
                color: #000;
                font-weight: bold;
                border-radius: 4px;
            }

                table#ctl00_ContentPlaceHolder1_tblLegend_ItemList td span {
                    float: none;
                }

                ul.rwControlButtons { width: auto !important;}

                .custom-form-height .form-group { height: 34px;}
       
               
    </style>


    <script language="javascript" type="text/javascript">
        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }

        function ddlBrand_OnClientSelectedIndexChanged(sender, args) {

            var item = args.get_item();

            $get('<%=hdnClosingBalance.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ClosingBalance") : "";
            $get('<%=hdnItemSubCategoryId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ItemSubCategoryId") : "";
            if ($get('<%=hdnItemSubCategoryId.ClientID%>').value == $get('<%=hdnHospitalSetupCostlyItemSubCategoryId.ClientID%>').value) {
                alert("This item comes under costly category");
            }

        }

        function ChangeRowColor(txtTotalQty, ClosingBalanceQty, rowID) {
            var TotalQty = Number(0);
            var ClosingBalance = Number(0);
            var Grid = document.getElementById('<%=gvItem.ClientID%>');

            TotalQty = $get(txtTotalQty).value;

            ClosingBalance = $get(ClosingBalanceQty).value;

            if (Number(TotalQty) > Number(ClosingBalance)) {

                alert("Required quantity is not available in destination store.");

            }
            else {
            }
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


        function OnClientPendingConsumableIndentClose(oWnd, args) {

            $get('<%=btnPendingConsumableIndent.ClientID%>').click();
        }
        function OnClientClose(oWnd, args) {
            var arg = args.get_argument();
             $get('<%=hdnReturnIndentIds.ClientID%>').value = arg.IndentIds;
             $get('<%=hdnReturnItemIds.ClientID%>').value = arg.ItemIds;
             $get('<%=btnRefresh.ClientID%>').click();
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main">
                <div class="col-md-2">
                    <h2>
                         <asp:Button ID="btnRefresh" runat="server" Style="display: none;" OnClick="btnRefresh_Click" />
                        <asp:Label ID="lblTitle" runat="server" Text="" />
                    </h2>
                </div>
                <div class="col-md-5">
                    <div class="row">
                        <div class="col-md-3 PaddingRightSpacing">
                        </div>
                        <div class="col-md-1 PaddingRightSpacing">
                            <asp:Label ID="Label7" runat="server" Text="Store&nbsp;" />
                        </div>
                        <div class="col-md-4 PaddingRightSpacing">
                            <telerik:RadComboBox ID="ddlStore" runat="server" DropDownWidth="180px" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" />
                        </div>
                        <div class="col-md-4 PaddingRightSpacing">
                            <asp:LinkButton ID="lnkBtnPendingConsumableIndent" runat="server" Text="Pending Drug Indent" ToolTip="Pending Drug Indent"
                                OnClick="lnkBtnPendingConsumableIndent_OnClick" />
                        </div>
                    </div>
                </div>
                <div class="col-md-5 text-right">
                        <asp:CheckBox ID="Chktoprinter" runat="server" Text="Direct to printer" class="pull-right"  Visible="false" />
                    <asp:LinkButton ID="lnkStopMedication" runat="server" CssClass="ICCAViewerBtn" OnClick="lnkStopMedication_OnClick" Text="Stop Medication" ToolTip="Click to see stop medication" />
                    <asp:Button ID="btnPreviousMedications" runat="server" CssClass="ICCAViewerBtn" OnClick="btnPreviousMedications_Click" Text="Previous Medications" />
                    <asp:Button ID="btnSave" Text="Save (Ctrl+F3)" runat="server" Width="95px" OnClick="btnSave_Onclick"
                        CssClass="ICCAViewerBtn" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                    <asp:Button ID="btnPrint" runat="server" CssClass="ICCAViewerBtn" Text="Print (Ctrl+F9)" OnClick="btnPrint_Click"
                        Visible="false" CausesValidation="false" />
                    <asp:Button ID="btnclose" Text="Close (Ctrl+F8)" runat="server" CssClass="ICCAViewerBtn" Visible="false"
                        OnClientClick="window.close();" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="">
                    <span class="text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="" /></span>
                </div>
            </div>

            <div class="container-fluid" style="background-color: #fff !important;">
                <div class="row form-group">
                    <div class="">

                        <asplUD:UserDetails ID="asplUD" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <hr style="height: -12px; margin: 0 0 5px; padding: 0;" />
                </div>
            
                  <div>
                    <div class="col-md-6">
                        Surgery Name:   
                        <%--<asp:Label ID="Label3" runat="server" Text="Surgery Name: " Font-Bold="true"></asp:Label>--%>
                        <asp:Label ID="lblSurgeryname" runat="server" Text=""></asp:Label>
                    </div>
                      <div class="col-md-6">
                          Surgeon Name: 
                           <%--<asp:Label ID="Label4" runat="server" Text="Surgeon Name: " Font-Bold="true"></asp:Label>--%>
                          <asp:Label ID="lblSurgeonName" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <div class="row form-group custom-form-height">
                    <div class="col-md-4 form-group">
                        <div class="row" id="trGeneric" runat="server">
                            <div class="col-md-3 label2">
                                <asp:Label ID="lblInfoBrand" runat="server" Text="Consumable" Font-Bold="true" />
                            </div>
                            <div class="col-md-9">
                                <telerik:RadComboBox ID="ddlBrand" runat="server" HighlightTemplatedItems="true"
                                    Width="100%" EmptyMessage="[ Search By Typing Minimum 3 Characters ]"
                                    AllowCustomText="true" MarkFirstMatch="false" EnableLoadOnDemand="true" OnItemsRequested="ddlBrand_OnItemsRequested"
                                    Skin="Office2007" ShowMoreResultsBox="true" AutoPostBack="true" OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged"
                                    EnableVirtualScrolling="true">
                                    <HeaderTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 80%" align="left">
                                                    <asp:Literal ID="Literal2" runat="server" Text="Item Name"></asp:Literal>
                                                </td>
                                                <%--<td style="width: 10%" align="center">Stock
                                                </td>--%>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />
                                                </td>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                </td>
                                                <td style="width: 0%" align="right" visible="false">
                                                    <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
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
                                                    <%--<%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>--%>
                                                    <asp:Label ID="lblClosingBalanceItem" runat="server" Text='<%# DataBinder.Eval(Container, "Attributes[\"ClosingBalance\"]")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 form-group" id="trCustomMedication" runat="server" visible="false">
                        <div class="col-md-4 PaddingRightSpacing">
                            <asp:Label ID="Label23" runat="server" Text="Custom Item" />
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtCustomMedication" runat="server" MaxLength="1000" TextMode="MultiLine"
                                onkeyup="return MaxLenTxt(this, 1000);" Style="width: 100%; height: 22px; line-height: 1.0em" />
                        </div>
                    </div>

                     <div class="col-md-2 form-group">
                       
                                <div class="PD-TabRadio margin_z">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkCustomMedication" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="chkCustomMedication_OnCheckedChanged"
                                                Text="Custom&nbsp;Item" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                          
                    </div>
                    
                    <%--<div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" Text="Qty"  Font-Bold="true"></asp:Label>
                        <div class="row">
                            <asp:TextBox ID="TextBox1" runat="server" SkinID="textbox" TextMode="Number" ></asp:TextBox>
                            </div>
                        </div>--%>
                    <div class="col-md-1 form-group">
                        <div class="row">
                            <%--<asp:Label ID="Label2" runat="server" Text="Qty"  Font-Bold="true"/>--%>
                            <asp:TextBox ID="txtQty" runat="server" SkinID="textbox" Width="60px" Height="25px" placeholder="Qty" Font-Bold="true"></asp:TextBox>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                FilterType="Custom,Numbers" TargetControlID="txtQty" ValidChars="0123456789." />
                        </div>
                    </div>

                    

                    <div class="col-md-3 form-group">
                        <div class="row">

                            <div class="col-md-3 label2">
                                <asp:Label ID="Label18" runat="server" Text="Remarks" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" TextMode="MultiLine"></asp:TextBox>

                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 form-group">
                        <div class="row">
                            <div class="col-md-4 label2">
                                <asp:Label ID="lblIndentType0" runat="server" Text="Indent type"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <telerik:RadComboBox ID="ddlIndentType" runat="server" Filter="Contains" MarkFirstMatch="true" Width="100%"></telerik:RadComboBox>
                            </div>
                        </div>
                        <div id="divClosedByNurse" class="row" runat="server" visible="false">
                            <div class="col-md-5 label2">
                                <asp:Label ID="Label1" runat="server" Text="Close Indent" />
                            </div>
                            <div class="col-md-7">
                                <asp:RadioButtonList ID="rdoClosedByNurse" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 form-group" id="dvAdvisingDoctor" runat="server" visible="false">
                        <div class="row">

                            <div class="col-md-4 col-sm-4 label1">
                                <asp:Label ID="lblAdvisingDoctor" runat="server" Text="Advising&nbsp;Doctor" />
                            </div>
                            <div class="col-md-8 col-sm-5">
                                <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="/Select" DropDownWidth="300px" Width="100%" Skin="Office2007" />
                            </div>

                        </div>
                    </div>

                    <div class="col-md-2 form-group pull-right">
                        <div class="row">
                            <asp:Button ID="btnAddItem" runat="server" CssClass="ICCAViewerBtn" Text="Add To List (Ctrl+F7)" OnClick="btnAddItem_OnClick" />
                            <asp:Button ID="btnSurgicalKit" runat="server" Text="Item Kits" CssClass="ICCAViewerBtn" CausesValidation="false" OnClick="btnSurgicalKit_OnClick" />
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
                <div class="row form-groupTop01">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                        <div class="row">
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="row">
                            <div class="col-md-4 label2"></div>
                            <div class="col-md-8"></div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <hr style="height: -12px; margin: 0; padding: 0;" />
                </div>

                <div class="row form-group">

                    <div class="col-md-7">
                        <div class="row form-groupTop">
                            <div class="col-md-3 label2" style="display: none;">
                                <asp:Label ID="lblDrugDetail" runat="server" Text="Drug Detail" Font-Underline="true" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <div class="row" style="display: none">
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-3">
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-md-12">
                </div>
                <div class="col-md-3 pull-right">
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="row form-group">
                            <div class="col-md-10 PaddingSpacing1">
                                <asp:GridView ID="gvItem" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                    SkinID="gridview2" AutoGenerateColumns="False" CssClass="table table-condensed" OnRowDataBound="gvItem_OnRowDataBound"
                                    OnRowCommand="gvItem_OnRowCommand">
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
                                                  <asp:HiddenField ID="hndItemSubCategoryId" runat="server" Value='<%#Eval("ItemSubCategoryId") %>' />
                                                <%-- <asp:HiddenField ID="hdnCustomId" runat="server" Value='<%#Eval("Id") %>'/>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--HeaderStyle-Width="250px" ItemStyle-Width="250px"--%>
                                        <asp:TemplateField HeaderText="Consumable Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'
                                                    Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="100px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentType" runat="server" Text='<%# Eval("IndentType") %>' />
                                                <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="60px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotalQty" runat="server" Width="100%" MaxLength="7"
                                                    Text='<%#Eval("Qty") %>' Style="text-align: right" />
                                                <%--<asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="50px"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="60px" ItemStyle-Width="100px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemUnit" runat="server" Width="100%" SkinID="label" Text='<%# Eval("UnitName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>' />
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
                            <div class="col-md-2">
                                <asp:GridView ID="gvIndent" runat="server" SkinID="gridview2" Width="100%" AutoGenerateColumns="False"
                                    OnRowCommand="gvIndent_RowCommand" OnRowDataBound="gvIndent_RowDataBound">
                                    <EmptyDataTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="25px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("ItemName")%>' />

                                                <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%#Eval("DetailsId")%>' />
                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                <asp:HiddenField ID="hdnPrescriptionDetail" runat="server" Value='<%#Eval("PrescriptionDetail")%>' />
                                                <asp:HiddenField ID="hdnIsConsumable" runat="server" Value='<%#Eval("IsConsumable")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="0">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="Label14" runat="server" BorderWidth="1px" BackColor="Cyan" SkinID="label"
                                                Width="15px" Height="15px" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="Label12" runat="server" SkinID="label" Text="Consumable  " />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table ID="tblLegend_ItemList" runat="server" border="0" CellPadding="2" CellSpacing="0">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="Label2" runat="server" Text="Costly Item" CssClass="cost-item" />
                                        </asp:TableCell>
                                        
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <div class="row form-group">
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
            <table cellpadding="0" cellspacing="0" style="display: none;">
                <tr>
                    <td>
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
                        <asp:HiddenField ID="hdnTotalQty" runat="server" />
                        <asp:HiddenField ID="hdnInfusion" runat="server" />
                        <asp:HiddenField ID="hdnChangeColor" runat="server" />
                        <asp:HiddenField ID="hdnClosingBalance" runat="server" />
                        <asp:HiddenField ID="hdnItemSubCategoryId" runat="server" />
                        <asp:HiddenField ID="hdnHospitalSetupCostlyItemSubCategoryId" runat="server" />
                        <asp:Button ID="btnPendingConsumableIndent" runat="server" CausesValidation="false" Style="display: none;"
                            OnClick="btnPendingConsumableIndent_OnClick" />
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
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
</asp:Content>

