<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="IPDItemReturn.aspx.cs" Inherits="Pharmacy_SaleIssue_IPDItemReturn"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

        <link href="../Include/css/mainNew.css" rel="stylesheet" />
        <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
        <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
        <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .gridInput {
                border: 1px solid #000;
            }

            input#ctl00_ContentPlaceHolder1_chkExport {
                float: none;
            }

            span#ctl00_ContentPlaceHolder1_lblMessage {
                float: none;
                margin: 0;
                width: 100%;
                position: relative;
                margin-left: 35px;
            }

            button[disabled], html input[disabled] {
                font-size: 12px;
                padding: 2px 12px;
            }

            input#ctl00_ContentPlaceHolder1_txtBarCodeValue {
                font-size: 12px;
                padding: 2px 12px;
            }

            td.rcbInputCell.rcbInputCellLeft .rcbInput {
                padding: 4px 12px !important;
            }

            table tr.clsGridheader th {
                background: #007bff!important;
               
                color: white!important;
            }
        </style>
        <script type="text/javascript" src="/Include/JS/Functions.js" lang="javascript"></script>

        <script type="text/javascript">

            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
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

            function SearchPatientOnClientClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    $get('<%=hdnRegistrationId.ClientID%>').value = arg.RegistrationId;
                    $get('<%=txtRegistrationNo.ClientID%>').value = arg.RegistrationNo;

                    $get('<%=hdnEncounterId.ClientID%>').value = arg.EncounterId;
                    $get('<%=txtEncounterNo.ClientID%>').value = arg.EncounterNo;
                }
                $get('<%=btnSearchPatient.ClientID%>').click();
            }

            function ddlItem_OnClientSelectedIndexChanged(sender, args) {
                var item = args.get_item();
                $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=btnGetInfo.ClientID%>').click();
            }

            function OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlDocString;
                    var IssueId = arg.IssueId;

                    $get('<%=txtReturnNo.ClientID%>').value = xmlString;
                    $get('<%=hdnReturnId.ClientID%>').value = IssueId;
                }
                $get('<%= btnReturnDetails.ClientID%>').click();
            }
            function OnClientIsValidPasswordClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                }
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }

            function OnClientIsValidUserNameAndPasswordClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                }
                $get('<%=btnIsValidUserNameAndPassword.ClientID%>').click();
            }

            function GetQty(txtPackSize, hdnConversionFactor, _returnQty, _balanceQty, hdnMrp, txtNetAmt) {
                debugger;
                
                var PackSize = Number(0);
                var ConversionFactor = Number(0);
                PackSize = $get(txtPackSize).value;

                ConversionFactor = $get(hdnConversionFactor).value;

                $get(_returnQty).value = PackSize * ConversionFactor;

                //alert(PackSize);

                if (ConversionFactor > 1 && (Math.round(PackSize) != PackSize)) {
                    alert("Fractional Not allowed for Packsize more then 1 !");
                    $get(_returnQty).value = '';
                    $get(txtPackSize).value = '';
                    return;
                }


                retQty = Number($get(_returnQty).value);
                balQty = Number($get(_balanceQty).value);

                if (retQty > balQty) {

                  
                    alert("Return quantity cann't be greater then balance quantity !");
                    $get(_returnQty).value = '';
                    return;
                }

                calcChkQty(_returnQty, _balanceQty, hdnMrp, txtNetAmt);


            }


            function calcChkQty(_returnQty, _balanceQty, hdnMrp, txtNetAmt) {
                debugger;
               
                var retQty = Number(0);
                var balQty = Number(0);
                var mrp = Number(0);
                var netAmt = Number(0);
                var totNetAmt = Number(0);
                var ftrNetAmt = Number(0);

                retQty = Number($get(_returnQty).value);
                balQty = Number($get(_balanceQty).value);
                mrp = Number($get(hdnMrp).value);

                if (retQty > balQty) {
                    alert("Return quantity cann't be greater then balance quantity !");
                    $get(_returnQty).value = '';
                    return;
                }
                else {
                    netAmt = mrp * retQty;
                    $get(txtNetAmt).value = netAmt.toFixed(2);
                    var gridview = document.getElementById('<%=gvStore.ClientID %>');
                    var length = gridview.rows.length;
                    var rowidx = '';
                    for (i = 0; i < length + 1; i++) {
                        if (i > 1) {
                            if (i < 10) {
                                rowidx = '0' + i.toString();
                            }
                            else {
                                rowidx = i.toString();
                            }
                            var netamount = Number($get('ctl00_ContentPlaceHolder1_gvStore_ctl' + rowidx.toString() + '_txtNetAmt').value);
                            ftrNetAmt += netamount;
                        }
                    }
                    $get('<%=txtNetAmount.ClientID%>').value = ftrNetAmt.toFixed(2);

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
                    evt = window.event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {

                    case 113: //F3
                        $get('<%=btnNew.ClientID%>').click();
                        break;

                    case 114: //F3
                        $get('<%=btnSaveData.ClientID%>').click();
                        break;

                    case 119:  // F8
                        $get('<%=btnCloseW.ClientID%>').click();
                        break;
                }
                evt.returnValue = false;
                return false;
            }
        </script>

        <div>
            <asp:HiddenField ID="hdnxmlString" runat="server" Value="" />
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSaveData" />
                        </Triggers>
                        <ContentTemplate>

                            <div class="container-fluid header_main">
                                <div class="col-md-2">
                                    <h2><span id="tdHeader" align="left" style=" width: 100px" runat="server">
                                        <asp:Label ID="lblHeader" runat="server" ToolTip="Item Reorder Level"
                                            Text="IPD&nbsp;Item&nbsp;Return" Font-Bold="true" />
                                    </span></h2>
                                </div>
                                <div class="col-md-5">
                                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
                                </div>
                                <div class="col-md-5 text-right">
                                    <span id="tblMarkedForDischarge" runat="server" cellpadding="0" cellspacing="1">

                                        <asp:LinkButton ID="btnMarkedForDischarge" runat="server" Text="Marked For Discharge"
                                            OnClick="btnMarkedForDischarge_OnClick" />

                                        <asp:Label ID="lblNoOfDischarge" Font-Size="11px" runat="server" ForeColor="Red" />

                                    </span>
                                    <asp:Button ID="btnOpenPatientWnd" runat="server" ToolTip="Click to open patient search window"
                                        CssClass="btn btn-primary" Text="Patient" OnClick="btnOpenPatientWnd_OnClick" />
                                    <asp:Button ID="btnGoReturnNo" runat="server" ToolTip="Click to open return no search window"
                                        CssClass="btn btn-primary" Text="Return No" OnClick="btnOpenReturnNoWnd_OnClick" />
                                    <asp:Button ID="btnCancelReturn" runat="server" ToolTip="Click to Cancel return "
                                        CssClass="btn btn-primary" Text="Cancel Return" OnClick="btnCancelReturn_OnClick" Visible="false" />
                                    <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" CssClass="btn btn-primary"
                                        Text="New (F2)" OnClick="btnNew_OnClick" />
                                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save(Ctrl+F3)" OnClick="btnSaveData_OnClick"
                                        CssClass="btn btn-primary" ValidationGroup="SaveData" Text="Save " />
                                    <asp:Button ID="btnPostData" runat="server" ToolTip="Post" OnClick="btnPostData_OnClick"
                                        CssClass="btn btn-primary" Text="Post" Visible="false" />
                                    <asp:Button ID="btnPrint" runat="server" ToolTip="Print Data(Ctrl+F9)" OnClick="btnPrint_OnClick"
                                        CssClass="btn btn-primary" CausesValidation="false" Text="Print" />
                                    <asp:Button ID="btnCloseW" Text="Close " runat="server" ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary"
                                        OnClientClick="window.close();" Visible="false" />&nbsp;
                                        <asp:CheckBox ID="chkExport" runat="server" Text="Export" />
                                </div>
                            </div>



                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="container-fluid">
                        <div class="row form-group" id="tblStore" runat="server" visible="false">
                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4 label2">
                                        <asp:Label ID="Label3" runat="server" Text="Store&nbsp;" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadComboBox ID="ddlStore" runat="server" CssClass="drapDrowHeight" Width="100%"
                                                    MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlStore_OnSelectedIndexChanged" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"></div>
                                    <div class="col-md-8"></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"></div>
                                    <div class="col-md-8"></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"></div>
                                    <div class="col-md-8"></div>
                                </div>
                            </div>

                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label2" runat="server" Text='Return No.' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnGoReturnNoEnter">
                                            <asp:TextBox ID="txtReturnNo" runat="Server" Width="100%" MaxLength="10"
                                                Style="text-transform: uppercase;" />
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterType="Custom" TargetControlID="txtReturnNo" ValidChars="0123456789/-" />
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label7" runat="server" Text='Return Date' />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadDatePicker ID="txtReturnDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                                            Enabled="false" />
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label4" runat="server" Text='<%$ Resources:PRegistration, ipno %>' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnGoEnter">
                                            <asp:TextBox ID="txtEncounterNo" runat="Server" Width="100%" MaxLength="10"
                                                Style="text-transform: uppercase;" />
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label9" runat="server" Text='<%$ Resources:PRegistration, regno %>' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByUHID" ScrollBars="None">
                                            <asp:TextBox ID="txtRegistrationNo" runat="Server" Width="100%" />
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRemarks" runat="server" Text='Remarks' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtRemarks" runat="Server"
                                            Style="width: 100%; font-size: 12px; padding: 2px 12px;" MaxLength="150" />
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label12" runat="server" Text='<%$ Resources:PRegistration, PatientName %>' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtPatientName" runat="Server" Width="100%" Enabled="false" />
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label11" runat="server" Text='Bed No.' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtBedNo" runat="Server" Width="100%" Enabled="false" />
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label6" runat="server" Text='Ward' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtWard" runat="Server" Width="100%" Enabled="false" />
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="label10" runat="server" Text='Payment Type' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtPaymentMode" runat="Server" Width="100%" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="label1" runat="server" Text='Item&nbsp;' />
                                    </div>
                                    <div class="col-md-10">
                                        <telerik:RadComboBox ID="ddlItem" runat="server" Width="100%" DropDownWidth="500px" Height="300px" EmptyMessage="[ Search Item by Name ]"
                                            AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlItem_OnItemsRequested"
                                            DataTextField="ItemName" DataValueField="ItemId" EnableVirtualScrolling="true"
                                            ShowMoreResultsBox="true" OnClientSelectedIndexChanged="ddlItem_OnClientSelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label13" runat="server" SkinID="label" Text="Bar Code Value&nbsp;" CssClass="pull-left" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Panel ID="Panel7" runat="server" DefaultButton="btnBarCodeValue">
                                            <asp:TextBox ID="txtBarCodeValue" runat="server" CssClass="pull-left" Width="100%" MaxLength="200" />
                                        </asp:Panel>
                                        <asp:Button ID="btnBarCodeValue" runat="server" Text="" CssClass="btn btn-primary" OnClick="btnBarCodeValue_OnClick"
                                            Style="visibility: hidden; display: none" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3 col-6 form-group">
                                <div class="row">
                                    <div class="row form-groupTop01" id="dvReason" runat="server" visible="false">
                                        <div class="col-md-5 label2">
                                            <asp:Label ID="Label5" runat="server" Text="Reason" /><span style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-7 ">
                                            <telerik:RadComboBox ID="ddlReason" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="udp2" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" CssClass="panel_y" Height="385px" ScrollBars="Auto">
                                <asp:GridView ID="gvStore" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                    SkinID="gridview" AutoGenerateColumns="False" OnRowDataBound="gvStore_OnRowDataBound"
                                    OnRowCommand="gvStore_OnRowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText='Issue Date' DataField="IssueDate" HeaderStyle-Width="120px" />
                                        <asp:TemplateField HeaderText="Item" HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Width="99%" Text='<%# Eval("ItemName") %>' />
                                                <asp:HiddenField ID="hdnIssueNo" runat="server" Value='<%# Eval("IssueNo") %>' />
                                                <asp:HiddenField ID="hdnIssuedBy" runat="server" Value='<%# Eval("IssuedBy") %>' />
                                                <asp:HiddenField ID="hdnIndentBy" runat="server" Value='<%# Eval("IndentBy") %>' />
                                                <asp:HiddenField ID="hdnIndentDate" runat="server" Value='<%# Eval("IndentDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Batch No' HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBatchNo" runat="server" Width="100%" Text='<%# Eval("BatchNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Expiryate' HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemExpiryDate" runat="server" Width="100%" Text='<%# Eval("ItemExpiryDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='MRP' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMRP" runat="server" Width="100%" Text='<%# Eval("MRP") %>' />
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                <asp:HiddenField ID="hdnBatchId" runat="server" Value='<%# Eval("BatchId") %>' />
                                                <asp:HiddenField ID="hdnIssueId" runat="server" Value='<%# Eval("IssueId") %>' />
                                                <asp:HiddenField ID="hdnItemExpiryDate" runat="server" Value='<%# Eval("ItemExpiryDate") %>' />
                                                <asp:HiddenField ID="hdnCostPrice" runat="server" Value='<%# Eval("CostPrice") %>' />
                                                <asp:HiddenField ID="hdnSaleTaxPerc" runat="server" Value='<%# Eval("SaleTaxPerc") %>' />
                                                <asp:HiddenField ID="hdnBalanceQty" runat="server" Value='<%# Eval("BalanceQty") %>' />
                                                <asp:HiddenField ID="hdnOnLineReturnQty" runat="server" Value='<%# Eval("OnLineReturnQty") %>' />
                                                <asp:HiddenField ID="hdnMRP" runat="server" Value='<%# Eval("MRP") %>' />
                                                <asp:HiddenField ID="hdnAdvisingDoctorId" runat="server" Value='<%# Eval("AdvisingDoctorId") %>' />
                                                <asp:HiddenField ID="hdnEmpAmt" runat="server" Value='<%# Eval("EmpAmt") %>' />
                                                <asp:HiddenField ID="hdnCompAmt" runat="server" Value='<%# Eval("CompAmt") %>' />
                                                <asp:HiddenField ID="hdnCopayPerc" runat="server" Value='<%# Eval("CopayPerc") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Disc %' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscPerc" runat="server" Width="100%" Text='<%# Eval("DiscPerc") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Issue Qty' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Width="100%" Text='<%# Eval("Qty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Returned Qty' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReturnedQty" runat="server" Width="100%" Text='<%# Eval("ReturnedQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Balance Qty' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceQty" runat="server" Width="100%" Text='<%# Eval("BalanceQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Online Return Qty' ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOnLineReturnQty" runat="server" Width="100%" Text='<%# Eval("OnLineReturnQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Return Qty' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReturnQty" runat="server" Width="100%" Text='<%# Eval("ReturnQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText='Pack Size' HeaderStyle-Width="40px"
                                            ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPackSize" runat="server" MaxLength="3" CssClass="gridInput" Style="width: 100%; text-align: right;"
                                                    autocomplete="off" />
                                                <AJAX:FilteredTextBoxExtender ID="FTBEtxtPackSize" runat="server" FilterType="Custom,Numbers"
                                                    ValidChars="1234567890." TargetControlID="txtPackSize">
                                                </AJAX:FilteredTextBoxExtender>
                                                <asp:HiddenField ID="hdnConversionFactor" runat="server" Value='<%#Eval("ConversionFactor")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px" />
                                            <ItemStyle Wrap="True" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Return Qty" HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReturnQty" runat="server" Width="100%" Style="text-align: right"
                                                    MaxLength="8" autocomplete="off" Text='<%# Eval("EnterReturnQty") %>' />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="0123456789."
                                                    FilterType="Custom" TargetControlID="txtReturnQty" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Returned Net Amt" HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNetAmt" runat="server" Width="100%" Style="text-align: right"
                                                    CssClass="gridInput" Text='<%# Eval("NetAmt") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="25px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                    CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("RowNo")%>'
                                                    ImageUrl="~/Images/DeleteRow.png" Width="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='DetailsId' Visible="false" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueDetailsId" runat="server" Width="100%" Text='<%# Eval("IssueDetailsId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblInfoTotal" runat="server" Text="Net Amount : " Font-Bold="true"></asp:Label>
                            <asp:TextBox ID="txtNetAmount" runat="server" CssClass="gridInput" Font-Bold="true"
                                Text="0.00" Width="100px" Style="text-align: right" autocomplete="off" />
                            <asp:HiddenField ID="hdnNetAmount" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <div id="dvConfirmPrint" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                        <table width="100%" cellspacing="2" cellpadding="0">
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to print ?" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="btnYes" CssClass="btn btn-default" runat="server" Text="Yes" OnClick="btnPrint_OnClick" />
                                    &nbsp;
                                                <asp:Button ID="btnNo" CssClass="btn btn-default" runat="server" Text="No" OnClick="btnCancelPrint_OnClick" />
                                </td>
                                <td align="center"></td>
                            </tr>
                        </table>
                    </div>

                    <span id="tblHiddenOther">

                        <asp:Button ID="btnSearchPatient" runat="server" CausesValidation="false" OnClick="btnSearchPatient_OnClick"
                            CssClass="btn btn-default" Style="visibility: hidden;" Text="SearchPatient" Width="1px" />
                        <asp:Button ID="btnGetInfo" runat="server" Text="GetInfo" CausesValidation="false"
                            SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfo_Click" />
                        <asp:Button ID="btnGoEnter" runat="server" CssClass="btn btn-default" Text="GO" OnClick="btnGoEnter_OnClick"
                            Style="visibility: hidden" />
                        <asp:Button ID="btnGoReturnNoEnter" runat="server" CssClass="btn btn-default" Text="GO" OnClick="btnGoReturnNoEnter_OnClick"
                            Style="visibility: hidden" />
                        <asp:Button ID="btnReturnDetails" runat="server" CssClass="btn btn-default" Text="" OnClick="btnReturnDetails_OnClick"
                            Style="visibility: hidden" />
                        <asp:Button ID="btnSearchByUHID" runat="server" CausesValidation="false" OnClick="btnSearchByUHID_OnClick"
                            CssClass="btn btn-default" Style="visibility: hidden;" Text="Filter" Width="1px" />
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                        <asp:HiddenField ID="hdnEncounterId" runat="server" />
                        <asp:HiddenField ID="hdnItemId" runat="server" />
                        <asp:HiddenField ID="hdnSponsorId" runat="server" />
                        <asp:HiddenField ID="hdnReturnId" runat="server" Value="" />
                        <asp:HiddenField ID="hdnDocumentNoStatus" runat="server" />
                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                        <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                        <asp:HiddenField ID="hdnPasswordScreenType" runat="server" />
                        <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                            Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                        <asp:Button ID="btnIsValidUserNameAndPassword" runat="server" OnClick="btnIsValidUserNameAndPassword_OnClick" CssClass="BillingFullBtn" Text="Search" CausesValidation="false" Style="visibility: hidden;" Width="1" />

                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
