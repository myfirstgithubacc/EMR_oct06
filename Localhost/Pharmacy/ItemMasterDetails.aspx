<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ItemMasterDetails.aspx.cs" Inherits="Pharmacy_ItemMasterDetails" Title=""
    EnableViewState="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="ItemCbo" TagName="ItemCombo" Src="~/Pharmacy/Components/ItemCombo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>

        <script type="text/javascript">

            function MaxLenTxt(TXT, cLen) {
                if (TXT.value.length > cLen) {
                    alert("Text length should not be greater then " + cLen + " ...");

                    TXT.value = TXT.value.substring(0, cLen);
                    TXT.focus();
                }
            }

            function OnClientFindClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var ItemID = arg.ItemID;
                    $get('<%=hdnItemID.ClientID%>').value = ItemID;
                }
                $get('<%=btnFindClose.ClientID%>').click();
            }

            function OnClientItemUnitClose(oWnd, args) {
                $get('<%=btnItemUnitClose.ClientID%>').click();
            }

            function OnClientAdditionalFieldClose(oWnd, args) {
                $get('<%=btnAdditionalFieldClose.ClientID%>').click();
            }
            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }

            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }

            function checkMaxPercentage(ctrl) {
                var txt = document.getElementById(ctrl);
                if (txt.value > 100) {
                    alert("Value should not be greater then 100 %");

                    txt.value = txt.value.substring(0, 2);
                    txt.focus();
                }
            }

            function maxLength(ctrl, intMax) {
                if (ctrl.value.length > intMax) {
                    ctrl.value = ctrl.value.substr(0, intMax);
                    alert("Maximum Length is " + intMax + " characters only.");
                }
            }

            function OnClientSelectionChange(editor, args) {
                var tool = editor.getToolByName("RealFontSize");
                if (tool && !$telerik.isIE) {
                    setTimeout(function() {
                        var value = tool.get_value();

                        switch (value) {
                            case "12px":
                                value = value.replace("12px", "9pt");
                                break;
                            case "14px":
                                value = value.replace("14px", "11pt");
                                break;
                            case "16px":
                                value = value.replace("16px", "12pt");
                                break;
                            case "18px":
                                value = value.replace("18px", "14pt");
                                break;
                            case "24px":
                                value = value.replace("24px", "18pt");
                                break;
                            case "26px":
                                value = value.replace("26px", "20pt");
                                break;
                            case "32px":
                                value = value.replace("32px", "24pt");
                                break;
                            case "34px":
                                value = value.replace("34px", "26pt");
                                break;
                            case "48px":
                                value = value.replace("48px", "36pt");
                                break;
                        }
                        tool.set_value(value);
                    }, 0);
                }
            }

            function cboItem_OnClientSelectedIndexChanged(sender, args) {
                var item = args.get_item();
                $get('<%=hdnItemID.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=btnGetInfo.ClientID%>').click();
            }
            function checkVatPercentage() {
                var vatPercentage = $get('<%=txtVatOnSale.ClientID%>').value;
                if (vatPercentage > 100) {
                    alert("Vat On Sale  should not be greater then 100 %");
                    $get('<%=txtVatOnSale.ClientID%>').value = "";

                }
            }
            
        </script>

        <div>
            <asp:UpdatePanel ID="upItemMaster" runat="server">
                <ContentTemplate>
                    <table width="100%" border="0" cellpadding="3" cellspacing="0">
                        <tr class="clsheader">
                            <td id="tdHeader" align="left" style="width: 110px" runat="server">
                                <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Item&nbsp;Master&nbsp;Details"
                                    ToolTip="Item&nbsp;Master&nbsp;Details" Font-Bold="true" />
                            </td>
                            <td align="right" style="width: 250px">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" ToolTip="Search" SkinID="Button"
                                    CausesValidation="false" OnClick="btnSearch_OnClick" />
                                <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" SkinID="Button"
                                    Text="New" OnClick="btnNew_OnClick" />
                                <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" OnClick="btnSaveData_OnClick"
                                    SkinID="Button" ValidationGroup="SaveData" Text="Save" />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                                <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                            </td>
                            <td align="right" style="width: 800px">
                                <asp:LinkButton ID="lnkReusable" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" Visible="false" Text="Reusable Items"
                                    OnClick="lnkreusable_OnClick" />
                                &nbsp;|&nbsp;
                                <asp:LinkButton ID="lnkPanelPriceRequired" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" Visible="false" Text="Panel Price Required"
                                    OnClick="lnkPanelPriceRequired_OnClick" />
                                &nbsp;|&nbsp;
                                <asp:LinkButton ID="lnkItemFlag" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" Text="Flag Tagging" OnClick="lnkItemFlag_OnClick" />
                                &nbsp;|&nbsp;
                                <asp:LinkButton ID="lnkItemSupplier" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" Text="Supplier Tagging"
                                    OnClick="lnkItemSupplier_OnClick" />
                                &nbsp;|&nbsp;
                                <asp:HyperLink ID="HyperLink1" Style="text-decoration: none;" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" NavigateUrl="/Pharmacy/ItemBrandMaster.aspx"
                                    runat="server"><u>Item Master</u></asp:HyperLink>&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="margin-left: 10px" cellpadding="2" cellspacing="0">
                        <tr>
                            <td class="clssubtopic">
                                <asp:Label ID="lblDetailsHeading" runat="server" Text="Item&nbsp;Information" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="margin-left: 10px" cellpadding="2" cellspacing="0">
                        <tr>
                            <td style="width: 140px">
                                <asp:Label ID="label37" runat="server" SkinID="label" ToolTip="Item Name" Text="Item Name" />
                                <span style='color: Red'>*</span>
                            </td>
                            <td colspan="3">
                                <ItemCbo:ItemCombo ID="cboItem" runat="server" />
                            </td>
                            <td rowspan="5" valign="top">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Image ID="PatientImage" runat="server" ImageUrl="/Images/logo/ImageNotAvailable.jpg"
                                                BorderWidth="1" EnableViewState="true" BorderColor="Gray" Width="95px" Height="110px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" SkinID="button" ToolTip="Upload Image"
                                                CausesValidation="false" Font-Size="7" Width="48px" />
                                            <AJAX:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnUpload"
                                                PopupControlID="pnlUpload" BackgroundCssClass="modalBackground" CancelControlID="ibtnClose"
                                                DropShadow="true" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnRemove" runat="server" Text="Remove" SkinID="button" ToolTip="Remove Image"
                                                CausesValidation="false" Font-Size="7" Width="48px" OnClick="btnRemove_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label1" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, ItemNo%>" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemNo" SkinID="textbox" runat="server" Width="245px" MaxLength="30"
                                    Enabled="false" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label38" runat="server" SkinID="label" ToolTip="Requested&nbsp;Location"
                                    Text="<%$ Resources:PRegistration, RequestedFacility%>" />
                                <%--<span style='color: Red'>*</span>--%>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlRequestedFacility" SkinID="DropDown" runat="server" Width="250px"
                                    EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestedFacility_OnSelectedIndexChanged" />
                            </td>
                            <td colspan="2">
                                <table border="0" width="100%" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td style="width: 100px">
                                            <%--<asp:Label ID="label10" runat="server" SkinID="label" Font-Bold="true" Text="Add&nbsp;New&nbsp;Item&nbsp;Unit" />--%>
                                        </td>
                                        <td>
                                            <%--<asp:ImageButton ID="ibtnNewItemUnit" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                ToolTip="Add&nbsp;New&nbsp;Item&nbsp;Unit" Height="18px" Width="16px" OnClick="ibtnNewItemUnit_Click"
                                                Visible="true" CausesValidation="false" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label31" runat="server" SkinID="label" Text="Recommended&nbsp;By" />
                            </td>
                            <td colspan="4">
                                <telerik:RadComboBox ID="ddlRecommendedBy" SkinID="DropDown" runat="server" Width="400px"
                                    EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                            </td>
                            <%--<td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label33" runat="server" SkinID="label" Text="Shelf&nbsp;Life" />
                            </td>
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtShelfLifeYears" SkinID="textbox" Style="text-align: right;" runat="server"
                                                Width="40px" MaxLength="3" />
                                            <asp:Label ID="label8" runat="server" SkinID="label" Text="Year(s)" />
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                FilterType="Custom" TargetControlID="txtShelfLifeYears" ValidChars="0123456789" />
                                            &nbsp;
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="ddlShelfLifeMonths" SkinID="DropDown" runat="server" Width="45px"
                                                MarkFirstMatch="true" />
                                            <asp:Label ID="label9" runat="server" SkinID="label" Text="Month(s)" />
                                            &nbsp;
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="ddlShelfLifeDays" SkinID="DropDown" runat="server" Width="45px"
                                                MarkFirstMatch="true" />
                                            <asp:Label ID="label12" runat="server" SkinID="label" Text="Day(s)" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRack" runat="server" SkinID="label" ToolTip="" Text="Rack" />
                                <%--<span style='color: Red'>*</span>--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRack" runat="server" SkinID="textbox" Width="250px" />
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="label36" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, Specification%>' />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSpecification" SkinID="textbox" runat="server" Style="min-height: 40px;
                                    max-height: 40px; min-width: 400px; max-width: 400px;" MaxLength="500" TextMode="MultiLine"
                                    onkeyup="return MaxLenTxt(this, 500);" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" SkinID="label" Text="Depreciation Days" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDepreciationDays" runat="server" SkinID="textbox"
                                    MaxLength="5" Width="50px" Style="text-align: right;"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom,Numbers"
                                    ValidChars="1234567890." TargetControlID="txtDepreciationDays">
                                </AJAX:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" SkinID="label" Text="Depreciation %" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDepreciationPerc" runat="server" SkinID="textbox"
                                    MaxLength="5" Width="50px" Style="text-align: right;"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom,Numbers"
                                    ValidChars="1234567890." TargetControlID="txtDepreciationPerc">
                                </AJAX:FilteredTextBoxExtender>
                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVatOnSale" ErrorMessage="Enter VAT Perventage For Sale" Display="None"
                        ValidationGroup="s"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        
                        
                        
                        
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="80px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Active" Value="1" />
                                        <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="VAT(%) On Sale" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtVatOnSale" runat="server" onkeyup="checkVatPercentage()" SkinID="textbox"
                                    MaxLength="5" Width="50px" Style="text-align: right;"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FTBEVatOnSale" runat="server" FilterType="Custom,Numbers"
                                    ValidChars="1234567890." TargetControlID="txtVatOnSale">
                                </AJAX:FilteredTextBoxExtender>
                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVatOnSale" ErrorMessage="Enter VAT Perventage For Sale" Display="None"
                        ValidationGroup="s"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label100" runat="server" SkinID="label" Text="Dose" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDose" runat="server" Text='<%#Eval("Dose") %>' SkinID="textbox"
                                    Width="35px" MaxLength="5" Style="text-align: right" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender100" runat="server" Enabled="True"
                                    FilterType="Custom, Numbers" TargetControlID="txtDose" ValidChars="." />
                            </td>
                            <td>
                                <asp:Label ID="Label111" runat="server" SkinID="label" Text="Unit" />
                            </td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="ddlUnit" runat="server" MarkFirstMatch="true" Filter="Contains"
                                    Width="250px" Height="200px" EmptyMessage="[ Select ]" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" SkinID="label" Text="Form" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlFormulation" runat="server" MarkFirstMatch="true" Filter="Contains"
                                    EmptyMessage="[ Select ]" AutoPostBack="true" OnSelectedIndexChanged="ddlFormulation_OnSelectedIndexChanged"
                                    Width="250px" Height="200px" />
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" SkinID="label" Text="Route" />
                            </td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="ddlRoute" runat="server" MarkFirstMatch="true" Filter="Contains"
                                    EmptyMessage="[ Select ]" Width="250px" Height="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="clssubtopic">
                                <asp:Label ID="Label29" runat="server" Text="Item Details & Store Wise Stock" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table border="0" width="100%">
                                    <tr>
                                        <td valign="top" style="width: 200px">
                                            <asp:Panel ID="Panel1" runat="server" Height="220px" BorderWidth="0" Width="100%"
                                                ScrollBars="Auto">
                                                <telerik:RadGrid ID="gvTax" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                    AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                                    ShowHeader="true" Width="200px" AutoGenerateColumns="False" ShowStatusBar="true"
                                                    EnableLinqExpressions="false" GridLines="Both" AllowPaging="false" Height="99%"
                                                    PageSize="10" OnPreRender="gvTax_PreRender" OnItemDataBound="gvTax_OnItemDataBound">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                            AllowColumnResize="false" />
                                                    </ClientSettings>
                                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">
                                                                No Record Found.</div>
                                                        </NoRecordsTemplate>
                                                        <ItemStyle Wrap="true" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="ChargeId" HeaderText="ChargeId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChargeId" runat="server" Text='<%#Eval("ChargeId")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ChargeName" HeaderText="Charge(s)" HeaderStyle-Width="60%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChargeName" runat="server" Text='<%#Eval("ChargeName")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ChargeValue" HeaderText="Value(%)" HeaderStyle-Width="40%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtChargeValue" runat="server" MaxLength="7" Width="100%" SkinID="textbox"
                                                                        Style="text-align: right" onkeyup="return checkMaxPercentage(this.id);" Text='<%#Eval("ChargeValue")%>' />
                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                        FilterType="Custom" TargetControlID="txtChargeValue" ValidChars="0123456789." />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                        <td valign="top">
                                            <asp:Panel ID="Panel2" runat="server" Height="220px" Width="100%" ScrollBars="Auto">
                                                <telerik:RadGrid ID="gvItemCurrentStock" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                    AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                                    ShowHeader="true" Width="400px" AutoGenerateColumns="False" ShowStatusBar="true"
                                                    EnableLinqExpressions="false" GridLines="Both" AllowPaging="false" Height="99%"
                                                    PageSize="10" OnPreRender="gvItemCurrentStock_PreRender">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                            AllowColumnResize="false" />
                                                    </ClientSettings>
                                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">
                                                                No Record Found.</div>
                                                        </NoRecordsTemplate>
                                                        <ItemStyle Wrap="true" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="FacilityName" HeaderText="Facility" HeaderStyle-Width="40%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Store" HeaderText="Store" HeaderStyle-Width="40%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStore" runat="server" Text='<%#Eval("Store")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ClosingBalance" HeaderText="Closing Balance"
                                                                HeaderStyle-Width="30%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClosingBalance" runat="server" Text='<%#Eval("ClosingBalance")%>'
                                                                        Width="100%" Style="text-align: right" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblChallanAccept" runat="server" SkinID="label" Visible="true" Text="Fractional Issue Allow" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdoIsFractionalIssue" runat="server" Visible="true" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Text="Yes" Value="1" />
                                                <asp:ListItem Text="No" Value="0" Selected="True" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="Profile&nbsp;Allow" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdoIsProfile" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Text="Yes" Value="1" />
                                                <asp:ListItem Text="No" Value="0" Selected="True" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblIsSubstituteNotAllowed" runat="server" SkinID="label" Text="Substitute" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdoIsSubstituteNotAllowed" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Text="Not Allowed" Value="1" />
                                                <asp:ListItem Text="Allowed" Value="0" Selected="True" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" SkinID="label" Visible="false" Text="Vat&nbsp;Include" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdoIsVatInclude" runat="server" Visible="false" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                                <asp:ListItem Text="No" Value="0" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="trlblPanelpriceRequired" runat="server">
                                        <td>
                                            <asp:Label ID="lblPanelpriceRequired" runat="server" SkinID="label" Text="Panel Price Required" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkPanelpriceRequired" runat="server" SkinID="checkbox" TextAlign="Left" />
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server">
                                        <td>
                                            <asp:Label ID="lblReusable" runat="server" SkinID="label" Text="Reusable" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkReusable" runat="server" SkinID="checkbox" TextAlign="Left" />
                                        </td>
                                    </tr>
                                    <tr id="tr2" runat="server">
                                        <td>
                                            <asp:Label ID="Label5" runat="server" SkinID="label" Text="Consumable" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkConsumable" runat="server" SkinID="checkbox" TextAlign="Left" />
                                        </td>
                                    </tr>
                                    <tr id="tr3" runat="server">
                                        <td>
                                            <asp:Label ID="Label7" runat="server" SkinID="label" Text="Use for BPL Patient" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkUseforbplpatient" runat="server" SkinID="checkbox" TextAlign="Left" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trAdditionalField" runat="server">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="clssubtopic" width="100px">
                                            <asp:Label ID="Label16" runat="server" Text="Custom&nbsp;Fields" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" SkinID="label" Text="&nbsp;&nbsp;Add&nbsp;New&nbsp;Custom&nbsp;Field" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnAdditionalFields" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                ToolTip="Add New Additional Field" Height="18px" Width="17px" CausesValidation="false"
                                                OnClick="ibtnAdditionalFields_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" valign="top">
                                <table style="width: 550px">
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="gvAdditionalField" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                                ShowHeader="false" Width="100%" AutoGenerateColumns="False" ShowStatusBar="true"
                                                EnableLinqExpressions="false" GridLines="Both" AllowPaging="false" Height="99%"
                                                OnPreRender="gvAdditionalField_PreRender" OnItemDataBound="gvAdditionalField_ItemDataBound">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Field Found.</div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="FieldId" HeaderText="FieldId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFieldId" runat="server" Text='<%#Eval("FieldId")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FieldType" HeaderText="FieldType" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFieldType" runat="server" Text='<%#Eval("FieldType")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FieldLength" HeaderText="FieldLength" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFieldLength" runat="server" Text='<%#Eval("FieldLength")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="GroupId" HeaderText="GroupId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupId" runat="server" Text='<%#Eval("GroupId")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="ValueId" HeaderText="ValueId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValueId" runat="server" Text='<%#Eval("ValueId")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DecimalPlaces" HeaderText="DecimalPlaces"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDecimalPlaces" runat="server" Text='<%#Eval("DecimalPlaces")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FieldName" HeaderText="Field(s)" HeaderStyle-Width="170px"
                                                            ItemStyle-VerticalAlign="Top">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Value" HeaderText="Value(s)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtN" runat="server" SkinID="textbox" Width="80px" MaxLength="7"
                                                                    Style="text-align: right;" onkeypress="checkRange(this, 0, 0);" onkeyup="checkRange(this, 0, 0);"
                                                                    Visible="false" Text='<%#Eval("ValueText")%>' />
                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtN" ValidChars="0123456789." />
                                                                <asp:TextBox ID="txtT" runat="server" SkinID="textbox" Width="350px" MaxLength='<%#Convert.ToInt32(Eval("FieldLength"))%>'
                                                                    Visible="false" Text='<%#Eval("ValueText")%>' />
                                                                <asp:TextBox ID="txtM" runat="server" SkinID="textbox" Style="min-height: 60px; max-height: 60px;
                                                                    min-width: 350px; max-width: 350px;" MaxLength='<%#Convert.ToInt32(Eval("FieldLength"))%>'
                                                                    TextMode="MultiLine" Visible="false" Text='<%#Eval("ValueText")%>' onkeyup="return MaxLenTxt(this, 1000);" />
                                                                <telerik:RadComboBox ID="ddlD" SkinID="DropDown" runat="server" Width="200px" AppendDataBoundItems="true"
                                                                    EmptyMessage="[ Select ]" MarkFirstMatch="true" Visible="false" />
                                                                <telerik:RadEditor ID="txtW" runat="server" EditModes="Design" EnableEmbeddedSkins="true"
                                                                    EnableResize="true" Skin="Vista" Height="200px" Width="350px" MaxTextLength="3000"
                                                                    ToolbarMode="ShowOnFocus" ToolsFile="~/Include/XML/BasicTools.xml" Visible="false"
                                                                    Content='<%#Eval("ValueWordProcessor")%>' OnClientSelectionChange="OnClientSelectionChange">
                                                                    <Modules>
                                                                        <telerik:EditorModule Visible="false" />
                                                                    </Modules>
                                                                </telerik:RadEditor>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin,Close,Move,Reload" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:HiddenField ID="hdnItemID" runat="server" />
                    <asp:Button ID="btnFindClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        OnClick="OnClientFindClose_OnClick" />
                    <asp:Button ID="btnItemUnitClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        OnClick="btnItemUnitClose_OnClick" />
                    <asp:Button ID="btnAdditionalFieldClose" runat="server" CausesValidation="false"
                        Style="visibility: hidden;" OnClick="OnClientAdditionalFieldClose_OnClick" />
                    <asp:Button ID="btnGetInfo" runat="server" Text="GetInfo" CausesValidation="false"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfo_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlUpload" runat="server" Style="display: none" CssClass="modalPopup">
                            <div>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <p style="color: Black; font-weight: bold;">
                                                            Add an Image
                                                        </p>
                                                    </td>
                                                    <td align="right">
                                                        <p style="color: Red; font-weight: bold;">
                                                            <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="/Images/icon-close.jpg"
                                                                CausesValidation="false" OnClientClick="return false;" />
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: solid 1 gray;">
                                            <asp:FileUpload ID="fUpload1" runat="server" Width="250px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="Button1" runat="server" Height="22px" Text="Upload" ValidationGroup="Upload"
                                                OnClick="Upload_OnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
