<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SupplierMaster.aspx.cs" Inherits="Pharmacy_SupplierMaster" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>

        <script type="text/javascript" src="/Include/JS/Functions.js" language="javascript">
        </script>

        <script type="text/javascript">

            function OnClientFindClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var MasterId = arg.MasterId;

                    $get('<%=hdnMasterId.ClientID%>').value = MasterId;
                }
                $get('<%=btnFindClose.ClientID%>').click();

            }

            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }
            function OnClientTaxClose(oWnd, args) {
                $get('<%=btnTaxClose.ClientID%>').click();
            }

        </script>

        <div>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr class="clsheader">
                            <td id="tdHeader" align="left" style="padding-left: 10px; width: 110px" runat="server">
                                <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Supplier&nbsp;Master"
                                    ToolTip="Supplier&nbsp;Master" Font-Bold="true" />
                            </td>
                            <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                                <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                            </td>
                            <td align="right" style="width: 250px">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" ToolTip="Search" SkinID="Button"
                                    CausesValidation="false" OnClick="btnSearch_OnClick" />
                                <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" SkinID="Button"
                                    Text="New" OnClick="btnNew_OnClick" />
                                <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                    SkinID="Button" ValidationGroup="SaveData" Text="Save" />
                                <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                    OnClientClick="window.close();" />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="98%" style="margin-left: 10px" cellpadding="2" cellspacing="0">
                        <tr>
                            <td class="clssubtopic" width="170px">
                                <asp:Label ID="lblDetailsHeading" runat="server" Text="Supplier&nbsp;Details" />
                            </td>
                            <td align="right">
                                <asp:LinkButton ID="lnkTagging" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" Text="Supplier Manufacture Tagging"
                                    OnClick="lnkTagging_OnClick" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="margin-left: 10px" cellpadding="2" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="label11" runat="server" SkinID="label" ToolTip="Supplier&nbsp;Name"
                                    Text="Name" />&nbsp;<span style='color: Red'>*</span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtName" SkinID="textbox" runat="server" Width="100%" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label21" runat="server" SkinID="label" Text="Code" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" SkinID="textbox" runat="server" Width="150px" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label12" runat="server" SkinID="label" Text="Short&nbsp;Name" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtShortName" SkinID="textbox" runat="server" Width="150px" MaxLength="20" />
                            </td>
                            <td>
                                <asp:Label ID="lblChallanAccept" runat="server" SkinID="label" Text="Challan&nbsp;Allow" />
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdoChallanAccept" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                    <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                    <asp:ListItem Text="No" Value="0" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trType" runat="server">
                            <td>
                                <asp:Label ID="lblSupplierType" runat="server" SkinID="label" Text="Supplier&nbsp;Type" />
                                <span style='color: Red'>*</span>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlSupplierType" SkinID="DropDown" AutoPostBack="True" OnSelectedIndexChanged="ddlSupplierType_OnSelectedIndexChanged"
                                    runat="server" Width="150px" EmptyMessage="[ Select ]" MarkFirstMatch="True" />
                            </td>
                            <td style="width: 100px">
                                <%-- <asp:Label ID="label13" runat="server" Visible="false" SkinID="label" Text='<%$ Resources:PRegistration, storetype%>' />--%>
                                <asp:Label ID="lblPeriod" runat="server" SkinID="label" Text="Validity Period (Days)" />
                                <asp:Label ID="lblPeriodStar" runat="server" Visible="false" ForeColor="Red" Text="*" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPeriod" Width="100px" Enabled="false" runat="server" MaxLength="3"
                                    SkinID="textbox"></asp:TextBox>
                                <asp:Label ID="lblValidUpTo" runat="server" Visible="false" Font-Bold="true" Text="" />
                                <AJAX:FilteredTextBoxExtender ID="FTBEPeriod" runat="server" FilterType="Custom,Numbers"
                                    ValidChars="1234567890" TargetControlID="txtPeriod">
                                </AJAX:FilteredTextBoxExtender>
                                <telerik:RadComboBox ID="ddlSupplierCategoryType" SkinID="DropDown" Visible="false"
                                    runat="server" Width="150px" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label18" runat="server" SkinID="label" Text="Payment Mode " />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlMode" SkinID="DropDown" CheckBoxes="true" runat="server"
                                    Width="150px" DataSourceID="SQLMode" DataTextField="Name" DataValueField="Id" />
                                <asp:SqlDataSource ID="SQLMode" runat="server" ConnectionString="<%$ ConnectionStrings:akl %>"
                                    EnableCaching="True" SelectCommand="SELECT Id, Name FROM PaymentMode ORDER BY ID">
                                </asp:SqlDataSource>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="label13" runat="server" SkinID="label" Text="Credit Period" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlPaymentMonths" SkinID="DropDown" runat="server" Width="45px"
                                    MarkFirstMatch="true" />
                                <asp:Label ID="label14" runat="server" SkinID="label" Text="Month(s)" />
                                <telerik:RadComboBox ID="ddlPaymentDays" SkinID="DropDown" runat="server" Width="45px"
                                    MarkFirstMatch="true" />
                                <asp:Label ID="label19" runat="server" SkinID="label" Text="Day(s)" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label26" runat="server" SkinID="label" Text="Address&nbsp;1" />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAdd1" SkinID="textbox" runat="server" Width="100%" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label27" runat="server" SkinID="label" Text="Address&nbsp;2" />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAdd2" SkinID="textbox" runat="server" Width="100%" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label28" runat="server" SkinID="label" Text="Address&nbsp;3" />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAdd3" SkinID="textbox" runat="server" Width="100%" MaxLength="100" />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:Label ID="label1" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, email%>' />
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEMail" SkinID="textbox" runat="server" Width="100%" MaxLength="100" />
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEMail"
                                    ValidationGroup="SaveData" SetFocusOnError="true" ErrorMessage="Invalid EMail ID Format"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Litral1" runat="server" Text='<%$ Resources:PRegistration, country%>'
                                    SkinID="label" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlcountryname" AutoPostBack="true" runat="server" OnSelectedIndexChanged="LocalCountry_SelectedIndexChanged"
                                    SkinID="DropDown" Width="150px" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text='<%$ Resources:PRegistration, state%>'
                                    SkinID="label" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="dropLState" SkinID="DropDown" Width="150px" runat="server"
                                    OnSelectedIndexChanged="LocalState_SelectedIndexChanged" AutoPostBack="true"
                                    MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text='<%$ Resources:PRegistration, city%>'
                                    SkinID="label" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="dropLCity" SkinID="DropDown" runat="server" Width="150px"
                                    AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="LocalCity_OnSelectedIndexChanged"
                                    MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text='<%$ Resources:PRegistration, pin%>'
                                    SkinID="label" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlZip" SkinID="DropDown" runat="server" Width="150px" AppendDataBoundItems="true"
                                    MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label15" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, mobile%>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobile" SkinID="textbox" runat="server" Width="150px" MaxLength="20" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtMobile" ValidChars="0123456789-,/" />
                                <%--<AJAX:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="true"
                                    AcceptNegative="None" AutoComplete="true" ClearMaskOnLostFocus="false" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="false"
                                    InputDirection="LeftToRight" Mask="999-999-9999" MaskType="Number" MessageValidatorTip="false"
                                    TargetControlID="txtMobile">
                                </AJAX:MaskedEditExtender>--%>
                            </td>
                            <td>
                                <asp:Label ID="label3" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, phone%>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone" SkinID="textbox" runat="server" Width="150px" MaxLength="30" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtPhone" ValidChars="0123456789-,/" />
                                <%--<AJAX:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AcceptAMPM="true"
                                    AcceptNegative="None" AutoComplete="true" ClearMaskOnLostFocus="false" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="false"
                                    InputDirection="LeftToRight" Mask="999-999-9999" MaskType="Number" MessageValidatorTip="false"
                                    TargetControlID="txtPhone">
                                </AJAX:MaskedEditExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label6" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, companyfax%>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax" SkinID="textbox" runat="server" Width="150px" MaxLength="20" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtFax" ValidChars="0123456789-,/" />
                                <%--<AJAX:MaskedEditExtender ID="MaskedEditExtender3" runat="server" AcceptAMPM="true"
                                    AcceptNegative="None" AutoComplete="true" ClearMaskOnLostFocus="false" CultureAMPMPlaceholder=""
                                    CultureCurrencySymbolPlaceholder="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="false"
                                    InputDirection="LeftToRight" Mask="999-999-9999" MaskType="Number" MessageValidatorTip="false"
                                    TargetControlID="txtFax">
                                </AJAX:MaskedEditExtender>--%>
                            </td>
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
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="clssubtopic" width="170px">
                                            <asp:Label ID="Label17" runat="server" Text="Contact&nbsp;Person&nbsp;Details" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Vendor Posting Group" />
                                <span id="spnVenderPosting" runat="server" style='color: Red'>*</span>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlVenderPosting" SkinID="DropDown" runat="server" Width="150px"
                                    AppendDataBoundItems="true" AutoPostBack="false" MarkFirstMatch="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label24" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, contactpersonname%>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPerson" SkinID="textbox" runat="server" Width="150px"
                                    MaxLength="30" />
                            </td>
                            <td>
                                <asp:Label ID="label25" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, contactpersondesignation%>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDesignation" SkinID="textbox" runat="server" Width="150px" MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label2" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, contactpersonmobile%>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonMobile" SkinID="textbox" runat="server" Width="150px"
                                    MaxLength="20" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtContactPersonMobile" ValidChars="0123456789-" />
                            </td>
                        </tr>
                        <tr id="trMedical" runat="server" visible="false">
                            <td>
                                Medical Representative’s Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtMRName" SkinID="textbox" runat="server" Width="150px" vi MaxLength="50" />
                            </td>
                        </tr>
                        <tr id="trArea" runat="server" visible="false">
                            <td>
                                Area Sale’s Manager
                            </td>
                            <td>
                                <asp:TextBox ID="txtASMgr" SkinID="textbox" runat="server" Width="150px" MaxLength="50" />
                            </td>
                        </tr>
                        <tr id="trZonal" runat="server" visible="false">
                            <td>
                                Zonal Sales Manager
                            </td>
                            <td>
                                <asp:TextBox ID="txtZSMgr" SkinID="textbox" runat="server" Width="150px" MaxLength="50" />
                            </td>
                        </tr>
                        <tr id="trRegional" runat="server" visible="false">
                            <td>
                                Regional Sales Manager
                            </td>
                            <td>
                                <asp:TextBox ID="txtRSMgr" SkinID="textbox" runat="server" Width="150px" MaxLength="50" />
                            </td>
                        </tr>
                        <tr id="trTaxDetails" runat="server">
                            <td colspan="2">
                                <table id="tbl1" runat="server">
                                    <tr>
                                        <td class="clssubtopic" width="100px">
                                            <asp:Label ID="Label16" runat="server" Text="Custom Fields" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" SkinID="label" Text="&nbsp;Add&nbsp;New&nbsp;Custom&nbsp;Field" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnPopup" runat="server" ImageUrl="~/Images/PopUp.jpg" ToolTip="Add New Tax Field"
                                                Height="18px" Width="17px" CausesValidation="false" OnClick="ibtnPopup_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td class="txt06">
                                <asp:Label ID="Label7" runat="server" Text="Select Facility" /><span id="spnFacility"  runat="server" style="color: red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" style="width: 450px">
                                <%--     <telerik:RadGrid ID="gvTax" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                    AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                    ShowHeader="false" Width="100%" AutoGenerateColumns="False" ShowStatusBar="true"
                                    EnableLinqExpressions="false" GridLines="Both" AllowPaging="True" Height="99%"
                                    PageSize="10" OnPreRender="gvTax_PreRender">
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
                                            <telerik:GridTemplateColumn UniqueName="TaxId" HeaderText="TaxId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxId" runat="server" Text='<%#Eval("StatusId")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Tax" HeaderText="Tax(s)" HeaderStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTax" runat="server" Text='<%#Eval("Status")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="TaxValue" HeaderText="Value(s)" HeaderStyle-Width="60%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxValue" runat="server" MaxLength="30" Width="100%" SkinID="textbox"
                                                        Style="text-transform: uppercase" Text='<%#Eval("TaxValue")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>--%>
                                <telerik:RadGrid ID="gvTax" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                    AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                    ShowHeader="false" Width="99%" AutoGenerateColumns="False" ShowStatusBar="true"
                                    EnableLinqExpressions="false" GridLines="Both" AllowPaging="false" Height="99%"
                                    OnPreRender="gvTax_PreRender" OnItemDataBound="gvTax_ItemDataBound">
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
                                                    <asp:TextBox ID="txtN" runat="server" SkinID="textbox" Width="200px" MaxLength="7"
                                                        Style="text-align: right;" onkeypress="checkRange(this, 0, 0);" onkeyup="checkRange(this, 0, 0);"
                                                        Visible="false" Text='<%#Eval("ValueText")%>' />
                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtN" ValidChars="0123456789." />
                                                    <asp:TextBox ID="txtT" runat="server" SkinID="textbox" Width="200px" MaxLength='<%#Convert.ToInt32(Eval("FieldLength"))%>'
                                                        Visible="false" Text='<%#Eval("ValueText")%>' />
                                                    <asp:TextBox ID="txtM" runat="server" SkinID="textbox" Style="min-height: 60px; max-height: 60px;
                                                        min-width: 200px; max-width: 200px;" MaxLength='<%#Convert.ToInt32(Eval("FieldLength"))%>'
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
                            <td style="width: 30px">
                            </td>
                            <td id="tdCheckBoxListFacility" valign="top" runat="server">
                                <div style="height: 220px; width: 230px; overflow: auto; border: solid 1px black;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBoxList ID="cblfacility" runat="server" RepeatDirection="Vertical">
                                            </asp:CheckBoxList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:HiddenField ID="hdnMasterId" runat="server" />
                    <asp:Button ID="btnFindClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        OnClick="OnClientFindClose_OnClick" />
                    <asp:Button ID="btnTaxClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        OnClick="OnClientTaxClose_OnClick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
