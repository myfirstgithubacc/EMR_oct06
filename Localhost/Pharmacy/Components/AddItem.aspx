<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddItem.aspx.cs" Inherits="Pharmacy_Components_AddItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Item</title>
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        .clsGridheaderorderNew th {
            padding: 0px 8px !important;
        }

        .tble_mk td {
            padding: 2px 8px !important;
        }
        div#upd1{
            overflow:hidden;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>
        <telerik:RadCodeBlock ID="radblock" runat="server">

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
                        //case 113:  // F2
                        case 120:  // F9 
                            $get('<%=btnCloseW.ClientID%>').click();
                            break;
                        case 115:  // F4
                            $get('<%=btnSaveData.ClientID%>').click();
                        break;
                }
                evt.returnValue = false;
                return false;
            }



            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.xmlString = document.getElementById("hdnXmlString").value;
                oArg.TotalQty = document.getElementById("hdnTotalQty").value;
                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function cboItem_OnClientSelectedIndexChanged(sender, args) {
                alert('sdasdas');
                var item = args.get_item();
                $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                 $get('<%=btnGetInfo.ClientID%>').click();
             }
             function ddlBrand_OnClientSelectedIndexChanged(sender, args) {
                 $get('<%=btnAddToGrid.ClientID%>').click();
            }


            function GetItem() {
                var tableElement = $find('ItemDetails_gvItemDetails');
                var MasterTable = tableElement.get_masterTableView();
                var rowElem = MasterTable.get_dataItems()[0];
            }

            function chekQty(txtstockqty, txtqty, txtsellingprice, txtdiscpersent, txtdiscamt, txtnetamt) {
                var PageName = $get('<%=hdnPageName.ClientID%>').value;
                var stockqty = Number(0);
                var qty = Number(0);
                var Sellingprice = Number(0);
                //                var CostPrice = Number(0);
                var discpersent = Number(0);
                var discamt = Number(0);
                var netamt = Number(0);

                stockqty = $get(txtstockqty).value;
                qty = $get(txtqty).value;
                if ((qty * 1) > (stockqty * 1)) {
                    $get(txtqty).value = '';
                    $get(txtdiscamt).value = '';
                    $get(txtnetamt).value = '';

                    alert('Sale quantity can not be greater than stock quantity !');
                    return;
                }
                Sellingprice = $get(txtsellingprice).value;
                //CostPrice = $get(txtcostPrice).value;
                discpersent = $get(txtdiscpersent).value;

                discamt = (qty * Sellingprice * discpersent) / 100;
                $get(txtdiscamt).value = discamt;
                if (PageName == 'Sale') {
                    netamt = ((qty * Sellingprice) - discamt);

                }
                else {
                    //if (CostPrice == 0) {
                    netamt = qty * Sellingprice;
                    //                    }
                    //                    else {
                    //                        netamt = qty * CostPrice;
                    //                    }
                }

                //$get(txtnetamt).value = netamt;
                $get(txtnetamt).value = netamt.toFixed(2);


            }

            function chekQty2(txtstockqty, txtqty, txtsellingprice, txtdiscpersent, txtdiscamt, txtnetamt) {
                var stockqty = Number(0);
                var qty = Number(0);
                var Sellingprice = Number(0);
                var discpersent = Number(0);
                var discamt = Number(0);
                var netamt = Number(0);

                stockqty = $get(txtstockqty).value;
                qty = $get(txtqty).value;
                if ((qty * 1) > (stockqty * 1)) {
                    $get(txtqty).value = '';
                    $get(txtdiscamt).value = '';
                    $get(txtnetamt).value = '';
                    alert('Sale quantity can not be greater than stock quantity !');
                    return;
                }
                Sellingprice = $get(txtsellingprice).value;

                discpersent = $get(txtdiscpersent).value;
                discamt = (qty * Sellingprice * discpersent) / 100;
                $get(txtdiscamt).value = discamt;
                netamt = (qty * Sellingprice) - discamt;
                //$get(txtnetamt).value = netamt;
                $get(txtnetamt).value = netamt.toFixed(2);

            }
            function OnClientItemsRequesting(sender, eventArgs) {
                var ddlgeneric = $find('<%= ddlGeneric.ClientID %>');
                var value = ddlgeneric.get_value();
                var context = eventArgs.get_context();
                context["GenericId"] = value;
            }
            function ddlGeneric_OnClientSelectedIndexChanged(sender, eventArgs) {
                var ddlbrand = $find("<%=ddlBrand.ClientID%>");
                ddlbrand.clearItems();
                ddlbrand.set_text("");
                ddlbrand.get_inputDomElement().focus();
            }
            </script>

        </telerik:RadCodeBlock>
        <div>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>

                    <div class="container-fluid header_main  margin_bottom">
                        <div class="row">


                            <div class="col-sm-2">
                                <h2>
                                    <asp:CheckBox ID="chkSubstitute" runat="server" Visible="false" Text="Show Substitute Item(s)"
                                        AutoPostBack="true" OnCheckedChanged="chkSubstitute_OnCheckedChanged" /></h2>
                            </div>

                            <div class="col-sm-5 text-center">
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="" Font-Bold="true" />
                            </div>

                            <div class="col-sm-5 text-right ">
                                <asp:Button ID="btnGetInfo" runat="server" Text="GetInfo" CausesValidation="false"
                                    SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfo_Click" />
                                <asp:Button ID="btnSaveData" runat="server" ToolTip="Add&nbsp;Item&nbsp;To&nbsp;Basket (F4)"
                                    CausesValidation="true" ValidationGroup="save" OnClick="btnSaveData_OnClick"
                                    CssClass="btn btn-primary" Text="Add To Basket " />
                                <asp:Button ID="btnCloseW" Text="Proceed " runat="server" CausesValidation="false"
                                    ToolTip="Add Item to basket before proceeding to main page...(F9)" CssClass="btn btn-primary"
                                    OnClick="btnCloseW_Click" />
                            </div>
                        </div>
                    </div>






                    <div class="container-fluid form-group">
                          <asp:Panel ID="Panel1" runat="server" Width="99%" DefaultButton="btnAddToGrid">
                        <div class="row form-group">
                            
                            <div class="col-6">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:Label ID="lblInfoGeneric" runat="server" Text="Generic: " SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                         <telerik:RadComboBox ID="ddlGeneric" runat="server" Width="100%" Height="450px" EmptyMessage="[ Search Generic ]"
                                    AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlGeneric_OnItemsRequested"
                                    DataTextField="GenericName" DataValueField="GenericId" Skin="Office2007" ShowMoreResultsBox="true"
                                    EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged">
                                </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                             <div class="col-6">
                                <div class="row">
                                    <div class="col-sm-2">
                                         <asp:Label ID="lblInfoBrand" runat="server" Text="Brand: " SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                         <telerik:RadComboBox ID="ddlBrand" runat="server" HighlightTemplatedItems="true"
                                    Width="100%" ZIndex="50000" EmptyMessage="[ Search Brands By Typing Minimum 3 Characters ]"
                                    AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlBrand_OnItemsRequested"
                                    Skin="Office2007" OnClientItemsRequesting="OnClientItemsRequesting" ShowMoreResultsBox="true"
                                    EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged">
                                    <HeaderTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 14%" align="left">
                                                    <asp:Literal ID="Literal1" runat="server" Text="Group"></asp:Literal>
                                                </td>
                                                <td style="width: 76%" align="left">
                                                    <asp:Literal ID="Literal2" runat="server" Text="Item Name"></asp:Literal>
                                                </td>
                                                <td style="width: 10%" align="right">Stock
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 14%" align="left">
                                                    <%# DataBinder.Eval(Container,"Attributes['ItemSubCategoryShortName']" )%>
                                                </td>
                                                <td style="width: 76%" align="left">
                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                </td>
                                                <td style="width: 10%" align="right">
                                                    <%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                          
                                <asp:Button ID="btnAddToGrid" runat="server" Text="Show Batch" OnClick="btnAddToGrid_OnClick"
                                    CausesValidation="false" Style="visibility: hidden;" />
                            </asp:Panel>
                        </div>

                        <div class="row form-group">
                            <asp:Panel ID="pnlgrid" runat="server" Height="150px" Width="100%" BorderWidth="1"
                                BorderColor="#cccccc" ScrollBars="Vertical" DefaultButton="btnAddToBasket">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvService" runat="server" AutoGenerateColumns="False" SkinID="gridviewOrderNew"
                                            Width="100%" OnRowDataBound="gvService_OnRowDataBound" CssClass="table tble_mk">
                                            <EmptyDataTemplate>
                                                <div style="font-weight: bold; color: Red;">
                                                    No Record Found.
                                                </div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="40px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Itemno%>' HeaderStyle-Width="30px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemNo" runat="server" Text='<%#Eval("ItemId")%>' Width="20px" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ItemName%>' Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" CssClass="gridInputNumber" SkinID="textbox"
                                                            Style="width: 100%; text-align: left; background-color: Transparent;" Text='<%#Eval("ItemName")%>'
                                                            ToolTip='<%#Eval("ItemName")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="200px" />
                                                    <ItemStyle Width="200px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, BatchId%>' HeaderStyle-Width="50px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBatchId" runat="Server" Text='<%#Eval("BatchId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Batchno%>' HeaderStyle-Width="70px"
                                                    Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo")%>' Width="100%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Expirydate%>' HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Stockqty%>' HeaderStyle-Width="100px"
                                                    ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStockQty" runat="server" MaxLength="5" CssClass="gridInput" Style="width: 98%; background-color: Transparent; text-align: right;"
                                                            Text='<%#Eval("StockQty")%>'
                                                            ToolTip='<%#Eval("StockQty")%>' ReadOnly="False" />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtStockQty" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtStockQty">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="70px" />
                                                    <ItemStyle Wrap="True" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Quantity%>' HeaderStyle-Width="40px"
                                                    ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" MaxLength="5" CssClass="gridInput" Style="width: 100%; text-align: right;"
                                                            autocomplete="off" TabIndex='<%# TabIndex %>' />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtQty" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtQty">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemStyle Wrap="True" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Unit%>' Visible="true">--%>
                                                <asp:TemplateField HeaderText="UOM" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnUnitId" runat="server" />
                                                        <asp:Label ID="lblItemUnitName" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, CostPrice%>' HeaderStyle-Width="150px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCostPrice" runat="server" class="gridInput" Style="width: 60px; background-color: Transparent; text-align: right;"
                                                            Text='<%#Eval("CostPrice")%>'
                                                            ToolTip='<%#Eval("CostPrice")%>' ReadOnly="True">
                                                        </asp:TextBox>
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtCostPrice" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtCostPrice">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Sellingprice%>' HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMRP" runat="server" Value='<%#Eval("MRP")%>' />
                                                        <asp:TextBox ID="txtCharge" runat="server" class="gridInput" Style="width: 60px; background-color: Transparent; text-align: right;"
                                                            Text='<%#Eval("SalePrice")%>'
                                                            ToolTip='<%#Eval("SalePrice")%>' ReadOnly="True">
                                                        </asp:TextBox>
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtRate" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtCharge">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, DiscountPersent%>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDiscountPersent" runat="server" CssClass="gridInputNumber" Style="width: 100%; background-color: Transparent; text-align: right"
                                                            Text='<%#Eval("DiscAmtPercent")%>'
                                                            ToolTip='<%#Eval("DiscAmtPercent")%>' ReadOnly="True" />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEServiceDiscPersent" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtDiscountPersent">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, DiscountAmount%>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDiscountAmt" runat="server" CssClass="gridInputNumber" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Enabled="False" ForeColor="#000062" />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEServiceDiscount" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtDiscountAmt">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Tax%>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTax" Enabled="False" runat="server" CssClass="gridInputNumber"
                                                            ForeColor="#000062" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Text='<%#Eval("Tax")%>' />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtTax" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtTax">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, NetAmount%>' HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNetAmt" runat="server" CssClass="gridInputNumber" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Enabled="false" ForeColor="#000062" />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtNetAmt" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtNetAmt">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expiry" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemExpiry" runat="server" SkinID="label" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Text='<%#Eval("ItemExpired")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtndaDelete" runat="server" ToolTip="Click here to delete this row"
                                                            CommandName="Del" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                            ImageUrl="~/Images/DeleteRow.png" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <RowStyle Wrap="False" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Button ID="btnAddToBasket" runat="server" Text="" Width="1px" OnClick="btnSaveData_OnClick"
                                    CssClass="btn btn-primary" Style="visibility: hidden;" Height="1px" />
                            </asp:Panel>
                        </div>


                        <div class="row form-group">
                            <div class="col-sm-2">
                                <asp:Label ID="lblInfoItemBasket" runat="server" Text="Items Basket" />
                            </div>
                            <div class="col-sm-10">
                                <%--<asp:Label ID="Label2" runat="server" BorderWidth="1px"  SkinID="label" Width="22px" Height="16px" />--%>
                                <asp:Label ID="Label3" runat="server" BackColor="LightPink" CssClass="LegendColor" Text="Short Expiry" Font-Italic="true" />

                                <%--<asp:Label ID="Label7" runat="server" BorderWidth="1px" BackColor="Tomato" SkinID="label" Width="22px" Height="16px" />--%>
                                <asp:Label ID="Label11" runat="server" CssClass="LegendColor" BackColor="Tomato" Text="Expired" Font-Italic="true" />
                                <asp:Label ID="lblBalQty" runat="server" SkinID="label" Font-Bold="true" />
                            </div>
                        </div>


                        <div class="row form-group">
                            <asp:Panel ID="Panel2" runat="server" Height="270px" Width="100%" BorderWidth="1" CssClass="table tble_mk"
                                BorderColor="#cccccc" ScrollBars="Vertical">
                                <asp:UpdatePanel ID="uphidden" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="GVItems" runat="server" AutoGenerateColumns="False" SkinID="gridviewOrderNew"
                                            Width="100%" OnRowDataBound="GVItems_OnRowDataBound" OnRowCommand="GVItems_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="40px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Itemno%>' HeaderStyle-Width="30px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemNo" runat="server" Text='<%#Eval("ItemId")%>' Width="20px" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ItemName%>' Visible="true" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" CssClass="gridInputNumber" SkinID="textbox"
                                                            Style="width: 100%; text-align: left; background-color: Transparent;" Text='<%#Eval("ItemName")%>'
                                                            ToolTip='<%#Eval("ItemName")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, BatchId%>' HeaderStyle-Width="50px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBatchId" runat="Server" Text='<%#Eval("BatchId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Batchno%>' HeaderStyle-Width="70px"
                                                    Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo")%>' Width="100%" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Expirydate%>' HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Stockqty%>' HeaderStyle-Width="60px"
                                                    ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStockQty" runat="server" MaxLength="5" CssClass="gridInput" Style="width: 98%; background-color: Transparent; text-align: right;"
                                                            Text='<%#Eval("StockQty")%>'
                                                            ToolTip='<%#Eval("StockQty")%>' ReadOnly="False" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="70px" />
                                                    <ItemStyle Wrap="True" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Quantity%>' HeaderStyle-Width="50px" ItemStyle-Width="100px"
                                                    ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" Text='<%#Eval("Qty")%>' MaxLength="5" CssClass="gridInput"
                                                            Style="width: 100%; text-align: right;" autocomplete="off" />
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtQty" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtQty">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemStyle Wrap="True" />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText='<%$ Resources:PRegistration, Unit%>' HeaderStyle-Width="40px"  Visible="true">--%>
                                                <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="40px" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnUnitId" runat="server" />
                                                        <asp:Label ID="lblItemUnitName" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, CostPrice%>' HeaderStyle-Width="130px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCostPrice" runat="server" class="gridInput" Style="width: 60px; background-color: Transparent; text-align: right;"
                                                            Text='<%#Eval("CostPrice")%>'
                                                            ToolTip='<%#Eval("CostPrice")%>' ReadOnly="True">
                                                        </asp:TextBox>
                                                        <AJAX:FilteredTextBoxExtender ID="FTBEtxtRate" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars=".1234567890" TargetControlID="txtCostPrice">
                                                        </AJAX:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Sellingprice%>' HeaderStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMRP" runat="server" Value='<%#Eval("MRP")%>' />
                                                        <asp:TextBox ID="txtCharge" runat="server" class="gridInput" Style="width: 60px; background-color: Transparent; text-align: right;"
                                                            Text='<%#Eval("SalePrice")%>'
                                                            ReadOnly="True">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, DiscountPersent%>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDiscountPersent" runat="server" CssClass="gridInputNumber" Style="width: 100%; background-color: Transparent; text-align: right"
                                                            Text='<%#Eval("DiscAmtPercent")%>'
                                                            ToolTip='<%#Eval("DiscAmtPercent")%>' ReadOnly="True" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, DiscountAmount%>' HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDiscountAmt" runat="server" CssClass="gridInputNumber" Text='<%#Eval("DisAmt")%>'
                                                            Style="width: 100%; text-align: right; background-color: Transparent;" Enabled="False"
                                                            ForeColor="#000062" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Tax%>' HeaderStyle-Width="100px" ItemStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTax" Enabled="False" runat="server" CssClass="gridInputNumber"
                                                            ForeColor="#000062" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                            Text='<%#Eval("Tax")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, NetAmount%>' HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNetAmt" runat="server" CssClass="gridInputNumber" Text='<%#Eval("NetAmt")%>'
                                                            Style="width: 100%; text-align: right; background-color: Transparent;" Enabled="false"
                                                            ForeColor="#000062" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtndaDelete" runat="server" ToolTip="Click here to delete this row"
                                                            CommandName="Del" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                            ImageUrl="~/Images/DeleteRow.png" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <RowStyle Wrap="False" />
                                        </asp:GridView>
                                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnTotalQty" runat="server" Value="" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>


                    </div>






                    <div id="divConfirmation" runat="server" style="width: 450px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 60px; left: 270px; top: 200px;">
                        <table cellspacing="2" cellpadding="2">
                            <tr>
                                <td style="width: 100%; text-align: center;">
                                    <asp:Label ID="Label5" runat="server" Text="Selected Item is blocked for this company. Do you Want to Continue?" ForeColor="#990066" />
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
                    <asp:HiddenField ID="hdntotQty" runat="server" />
                    <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                    <asp:HiddenField ID="hdnPageName" runat="server" />
                    <asp:HiddenField ID="hdnItemId" runat="server" />
                    <asp:HiddenField ID="hdnIsMainStore" runat="server" Value="0" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
