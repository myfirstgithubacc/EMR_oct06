<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DepartmenConsumption.aspx.cs" Inherits="Pharmacy_DepartmenConsumption" %>

<%@ Register Src="~/Include/Components/PaymentMode.ascx" TagName="PaymentMode" TagPrefix="ucPaymentMode" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="aspl" TagName="PatientQView" Src="~/Include/Components/PatientQView.ascx" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">


        <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <style type="text/css">
            div#ctl00_ContentPlaceHolder1_pnlItem {
                width: 100vw;
            }
            table.table-responsive{
                width:100vw!important;    
            }
            tr.clsGridRow td{
                white-space:nowrap;
            }
            tr.clsGridheader th{
                white-space:nowrap;
            }
        </style>

        <script type="text/javascript" language="javascript">



            function chekQty(txtstockqty, txtqty, txtsellingprice, txtnetamt) {
                var stockqty = Number(0);
                var qty = Number(0);
                var Sellingprice = Number(0);
                var netamt = Number(0);

                stockqty = $get(txtstockqty).value;
                qty = $get(txtqty).value;
                if ((qty * 1) > (stockqty * 1)) {

                    $get(txtqty).value = '';
                    $get(txtnetamt).value = '';

                    alert('Issue Quantity Can not be greater than stock Quantity !');
                    return;
                }
                Sellingprice = $get(txtsellingprice).value;

                netamt = (qty * Sellingprice);
                $get(txtnetamt).value = netamt;



                // Calculate  Qty,Unit,NetAmtt, patient amt, payer amt of all grid item and total of all items when change Quantity.
                var ftrQty = Number(0);
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
                        var charge = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtNetAmt').value);

                        if (Qty > 0) {
                            var pQty = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtQty').value);
                            ftrQty += pQty;
                        }
                        if (charge > 0) {
                            var Netamt = Number($get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtNetAmt').value);
                            ftrNetAmt += Netamt;
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
                $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotNetamt').value = ftrNetAmt.toFixed(2);
                $get('<%=txtNetAmount.ClientID%>').value = ftrNetAmt.toFixed(2);

            }


            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }
            function wndAddService_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var xmlString = arg.xmlString;
                    $get('<%=hdnxmlString.ClientID%>').value = xmlString;
                }
                $get('<%=btnBindGridWithXml.ClientID%>').click();
            }

            function AutoChange() {
                //
                var txt = $get('<%=txtRemarks.ClientID%>');

                if (txt.value.length > 100) {
                    alert("Text length should not be greater then 100.");

                    txt.value = txt.value.substring(0, 100);
                    txt.focus();


                }
                //txt.focus();
            }
            function Search_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var DocId = arg.DocId;
                    var DocNo = arg.DocNo;
                    $get('<%=txtDocNo.ClientID%>').value = DocNo;
                    $get('<%=btnSearchByDocNo.ClientID%>').click();
                }
            }
            function OnClientIsValidPasswordClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                }
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }
            function SearchPatientOnClientClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;

                    var DocNo = arg.DocNo;

                    var cmbvalue = Number(0);
                    var comboBox1 = $find('<%= ddlSearchOn.ClientID %>');
                    cmbvalue = Number(comboBox1.get_value());
                    if (cmbvalue == 0) {
                        $get('<%=txtRegistrationNo.ClientID%>').value = RegistrationNo;
                    }
                    if (cmbvalue == 1) {
                        $get('<%=txtRegistrationNo.ClientID%>').value = EncounterNo;
                    }

                    if (cmbvalue == 2) {
                        $get('<%=txtRegistrationNo.ClientID%>').value = DocNo;
                    }

                }
                $get('<%=btnSearchByUHID.ClientID%>').click();
            }
        </script>

    </telerik:RadCodeBlock>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <asp:HiddenField ID="hdnDec" runat="server" Value="2" />

                <div class="container-fluid header_main margin_bottom">
                    <div class="row">
                        <div class="col-md-3 col-5">
                            <h2>
                                <span id="tdHeader" align="left" style="padding-left: 10px; width: 210px" runat="server">
                                    <asp:Label ID="lblHeader" runat="server" Text="Department Consumption" ToolTip="" Font-Bold="true" />
                                </span>
                            </h2>
                        </div>
                        <div class="col-md-4 col-7">
                            <div class="row">
                                <div class="col-4">
                                    <asp:Label ID="lblStore" runat="server" Text="Store" SkinID="label" />
                                </div>
                                <div class="col-8">
                                    <telerik:RadComboBox ID="ddlStore" Width="100%" runat="server" AutoPostBack="true"
                                        MarkFirstMatch="true" EmptyMessage="[ Select ]" DropDownWidth="266px" EnableLoadOnDemand="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-4 text-center">
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                            <asp:Button ID="btnSaveAndPost" runat="server" ToolTip="Click here to post." Text="Post"
                                CssClass="btn btn-primary" OnClick="btnSaveAndPost_OnClick" />
                            <AJAX:ConfirmButtonExtender ID="cbsaveandpost" runat="server" ConfirmOnFormSubmit="true"
                                ConfirmText="Are You Sure? You Want To Post" TargetControlID="btnSaveAndPost">
                            </AJAX:ConfirmButtonExtender>
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" ToolTip="Click here to cancel" CssClass="btn btn-primary"
                                Text="Cancel" OnClick="btnCancel_OnClick" />
                        <AJAX:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmOnFormSubmit="true"
                            ConfirmText="Are You Sure? You Want To Cancel" TargetControlID="btnCancel">
                        </AJAX:ConfirmButtonExtender>
                        <asp:Button ID="Button1" runat="server" CausesValidation="false" Style="visibility: hidden;"
                            Text="" Width="5px" />


                    </div>

                        <div class="col-md-3 col-8 text-right" style="margin-top:-6px;">
                            <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary" Text="New" OnClick="btnNew_OnClick"
                                ToolTip="Click here to create new document" />
                            <AJAX:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmOnFormSubmit="true"
                                ConfirmText="Are You Sure? You Want To Discard And Create New" TargetControlID="btnNew">
                            </AJAX:ConfirmButtonExtender>
                            <asp:Button ID="btnSaveData" runat="server" OnClick="btnSaveData_OnClick" CssClass="btn btn-primary"
                                ValidationGroup="SaveData" Text="Save" ToolTip="Click here to save" />
                            <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_OnClick" CssClass="btn btn-primary"
                                ValidationGroup="SaveData" Text="Edit" ToolTip="Click here to Edit" Visible="false" />
                            <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_OnClick" CssClass="btn btn-primary"
                                CausesValidation="false" Text="Print" ToolTip="Click here to print" />
                            <asp:Button ID="btnBindGridWithXml" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                OnClick="btnBindGridWithXml_OnClick" Text="" Width="5px" />
                            &nbsp;
                            <AJAX:ConfirmButtonExtender ID="cmdSaveCnfrm" runat="server" ConfirmOnFormSubmit="true"
                                TargetControlID="btnSaveData" ConfirmText="Are You Sure? You Want To Save">
                            </AJAX:ConfirmButtonExtender>
                        <AJAX:ConfirmButtonExtender ID="cmdEdit" runat="server" ConfirmOnFormSubmit="true"
                            TargetControlID="btnEdit" ConfirmText="Are You Sure? You Want To Edit">
                        </AJAX:ConfirmButtonExtender>


                    </div>
                </div>

                </div>



                <div class="container-fluid text-center">
                    <div class="row">
                        <div class="col-12">
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" />
                        </div>
                    </div>
                </div>



                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                    DecorationZoneID="dvSearchZone"></telerik:RadFormDecorator>
                <div id="dvSearchZone" style="width: 100%">


                    <div class="container-fluid">
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-4 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, DocNo%>' />
                                    </div>
                                    <div class="col-md-5 col-7">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearchByDocNo">
                                                    <asp:TextBox ID="txtDocNo" Width="100%" runat="server" MaxLength="20" SkinID="textbox"></asp:TextBox>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-3 col-5 pl-0">
                                        <asp:Button ID="btnSearchByDocNo" runat="server" Text="Go" Width="40px" OnClick="btnSearchByDocNo_OnClick" CssClass="btn btn-primary" />
                                        <asp:LinkButton ID="lbtnSearchAllDoc" runat="server" Text="Search..." ForeColor="DodgerBlue"
                                            ToolTip="Click to search all department consumption from list" OnClick="lbtnSearchAllDoc_OnClick"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-6  form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label2" runat="server" Text='<%$ Resources:PRegistration, department%>' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtDepartmentName" Width="100%" runat="server" Enabled="false"
                                            SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-4 col-sm-4 col-6  form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label5" runat="server" Text="Remarks" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtRemarks" Width="100%" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-4 col-sm-4 col-6  form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label4" runat="server" Text='<%$ Resources:PRegistration, DocDate%>' />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadDatePicker ID="dtpDocDate" Enabled="false" DateInput-DateFormat="dd/MM/yyyy"
                                            runat="server" MinDate="01/01/1900" Width="100%">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-4 col-sm-4 col-6  form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label3" runat="server" Text='<%$ Resources:PRegistration, Facility%>' />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFacilityName" Width="100%" runat="server" Enabled="false" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-4 col-sm-4 col-6  form-group" style="align-self: end;">
                                <div class="row">
                                    <div class="col-md-4 col-5">
                                        <asp:ImageButton ID="ibtnFind" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/Binoculr.ico"
                                            OnClick="ibtnFind_OnClick" Width="20px" />
                                        <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="100%">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, UHID%>" Value="0" />
                                                <telerik:RadComboBoxItem Selected="True" Text="IP No" Value="1" />
                                                <%--  <telerik:RadComboBoxItem Text="OP Bill" Value="2" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-8 col-7">
                                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByUHID">
                                            <asp:TextBox ID="txtRegistrationNo" runat="server" MaxLength="9" SkinID="textbox"
                                                Style="padding-left: 1px;" Width="100%" />
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-4 col-sm-4 col-6  form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblPostingDate" runat="server" Text="PostingDate" Visible="false" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadDatePicker ID="RdlPostingDate" DateInput-DateFormat="dd/MM/yyyy" Visible="false"
                                            runat="server" MinDate="01/01/1900" Width="100%">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-4 col-6 form-group">
                                <div class="row">
                                    <div class="col-md-9">
                                        <asp:Label ID="lblDetail" runat="server" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSearchByUHID" runat="server" OnClick="btnSearchByUHID_OnClick"
                                            CssClass="btn btn-primary" Text="Search" CausesValidation="false" Style="visibility: hidden;"
                                            Width="1" />
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-4 col-6 form-group ">
                                <div class="row">
                                    <div class="col-md-4 col-4">
                                        <asp:Label ID="Label9" runat="server" Text="Item(s)&nbsp;Details" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:Button ID="btnAddNewItem" runat="server" Text="Add New Item" CssClass="btn btn-primary"
                                            CausesValidation="false" OnClick="btnAddNewItem_OnClick" ToolTip="Click here to add new item" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <table border="0" class="table-responsive" style="margin-left: 0px; margin-top: 1px; background: #e0ebfd; border-width: 1px"
                    cellpadding="1" cellspacing="1" width="99%">
                    <tr>
                        <td colspan="2" valign="top">
                            <asp:Panel ID="pnlItem" runat="server" SkinID="gridview" Height="360px"
                                BorderWidth="0px" BorderColor="LightBlue">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                            AlternatingRowStyle-BackColor="#E6E6FA" SkinID="gridview2" ShowFooter="true"
                                            Width="100%" OnRowDataBound="gvService_OnRowDataBound" OnRowCommand="gvService_RowCommand">
                                            <EmptyDataTemplate>
                                                <div style="font-weight: bold; color: Red;">
                                                    No Record Found.
                                                </div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PRegistration, ItemName%>" HeaderStyle-Width="200px"
                                                    ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnConsumptionDetailsId" runat="server" Value='<%#Eval("Id")%>' />
                                                        <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                        <asp:Label ID="txtItemname" runat="server" CssClass="gridInputNumber" Style="width: 100%; text-align: right;"
                                                            Text='<%#Eval("ItemName")%>' ToolTip='<%#Eval("ItemName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, BatchNo %>' HeaderStyle-Width="100px"
                                                    Visible="true">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnBatchId" runat="server" Value='<%#Eval("BatchId")%>' />
                                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo")%>' Width="70px" />
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Quantity %>' HeaderStyle-Width="40px"
                                                    ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnStockQty" runat="server" Value='<%#Eval("StockQty")%>' />
                                                        <asp:TextBox ID="txtQty" runat="server" SkinID="textbox" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Text='<%#Eval("Qty")%>' MaxLength="5" autocomplete="off"></asp:TextBox>
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEServiceStockQty" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="1234567890." TargetControlID="txtQty">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtTotQty" runat="server" MaxLength="8" CssClass="gridInput" Style="width: 99%; text-align: right; background-color: #E0EBFD;"
                                                            ForeColor="#003871" Font-Size="8"
                                                            Font-Bold="True" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Unit %>' HeaderStyle-Width="40px"
                                                    ItemStyle-HorizontalAlign="Right" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnUnitId" runat="server" />
                                                        <asp:Label ID="lblItemUnitName" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, CostPrice %>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCostPrice" runat="server" class="gridInput" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            ForeColor="#000062" Text='<%#Eval("CostPrice")%>'
                                                            ToolTip='<%#Eval("CostPrice")%>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SalePrice" HeaderStyle-Width="50px" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSellingPrice" runat="server" CssClass="gridInputNumber" Enabled="false"
                                                            Style="width: 100%; text-align: right; background-color: Transparent;" ForeColor="#000062"
                                                            Text='<%#Eval("SalePrice")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MRP" HeaderStyle-Width="50px" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMRP" runat="server" CssClass="gridInputNumber" Enabled="false"
                                                            Style="width: 100%; text-align: right; background-color: Transparent;" ForeColor="#000062"
                                                            Text='<%#Eval("MRP")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, NetAmount %>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNetAmt" runat="server" CssClass="gridInputNumber" Enabled="false"
                                                            ForeColor="#000062" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Text='<%#Eval("NetAmt")%>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtTotNetamt" runat="server" CssClass="gridInput" Style="width: 99%; text-align: right; background-color: #E0EBFD;"
                                                            ForeColor="#003871" Font-Size="8"
                                                            Font-Bold="True" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tax" HeaderStyle-Width="100px" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTax" runat="server" class="gridInput" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            ForeColor="#000062" Text='<%#Eval("SaleTaxPercent")%>'
                                                            ToolTip='<%#Eval("SaleTaxPercent")%>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtndaDelete" runat="server" ToolTip="Click here to delete this row"
                                                            CommandName="Del" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                            ImageUrl="~/Images/DeleteRow.png" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <RowStyle Wrap="False" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr class="clssubtopic" style="height: 22px;">
                        <td align="right" style="padding-right: 120px;">
                            <asp:Label ID="Literal7" runat="server" Text="Net Amt" ForeColor="White" Font-Bold="true"></asp:Label>
                            &nbsp;<asp:TextBox ID="txtNetAmount" runat="server" Width="100px" ReadOnly="true"
                                Text="0.00" SkinID="textbox" Font-Bold="True" Style="text-align: right;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="0" style="background: #e0ebfd;">
                    <tr>
                        <td align="left">
                            <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>--%>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" Skin="Office2007" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" InitialBehaviors="Maximize" Skin="Office2007" Behaviors="Pin, Move, Reload, Close" />
                                </Windows>
                            </telerik:RadWindowManager>
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
                            <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                            <asp:HiddenField ID="hdnServiceTaxPerc" runat="server" />
                            <asp:HiddenField ID="hdnDiscountPerc" runat="server" />
                            <asp:HiddenField ID="hdnItemIds" runat="server" />
                            <asp:HiddenField ID="hdnUniqueSessionId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnConsumptionId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnProcessStatus" runat="server" Value="OPEN" />
                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
