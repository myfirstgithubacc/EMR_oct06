<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ENMCodes.aspx.cs" Inherits="EMR_Assessment_CNMCodes"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <title></title>
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="smDiagnosis" runat="server">
        </asp:ScriptManager>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="clssubtopicbar" align="right" valign="Middle" style="background-color: #E0EBFD;
                    padding-right: 10px;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl_Msg" runat="server" ForeColor="Green" Font-Bold="true" Style="padding-right: 100px;"></asp:Label>
                            <asp:LinkButton ID="lnkDiagnosis" runat="server" CausesValidation="false" Text="CPT® Coding"
                                Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkDiagnosis_OnClick"></asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton ID="lnkMedication" runat="server" CausesValidation="false" Text="Medication Coding"
                                Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkMedication_OnClick"></asp:LinkButton>

                            <script language="JavaScript" type="text/javascript">
                                function LinkBtnMouseOver(lnk) {
                                    document.getElementById(lnk).style.color = "red";
                                }
                                function LinkBtnMouseOut(lnk) {
                                    document.getElementById(lnk).style.color = "blue";
                                }
                            </script>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="2" border="0" width="100%">
            <tr>
                <td>
                    <div style="float: left;">
                        <asp:UpdatePanel ID="update" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnAllServices" runat="server" Text="All Services" SkinID="Button"
                                    OnClick="btnAllServices_Click" />&nbsp;
                                <asp:Button ID="btnFavorites" runat="server" Text="Favorites" SkinID="Button" OnClick="btnFavorites_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div style="float: right;">
                        <asp:Button ID="btnNew" runat="server" SkinID="Button" Text="New" TabIndex="6" OnClick="btnNew_Click"
                            CausesValidation="false" />
                        <button id="img1" runat="server" class="buttonBlue" onclick="javascript:window.close();">
                            Close</button>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upRx" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvDrug" />
                        </Triggers>
                        <ContentTemplate>
                            <div style="float: left;">
                                <telerik:RadComboBox ID="ddlOption" TabIndex="8" runat="server" AllowCustomText="True"
                                    MarkFirstMatch="true" Width="100px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Anywhere" Value="0" Selected="True" />
                                        <telerik:RadComboBoxItem Text="Starts With" Value="1" Selected="False" />
                                        <telerik:RadComboBoxItem Text="Ends With" Value="2" Selected="False" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:TextBox ID="txtSearch" runat="server" Columns="26" CssClass="Textbox" SkinID="textbox"></asp:TextBox>&nbsp;<asp:Button
                                    ID="btnSearch" TabIndex="10" Text="Search" Width="64px" runat="server" OnClick="btnSearch_Click"
                                    SkinID="Button" CausesValidation="false" />
                                Facility
                                <telerik:RadComboBox ID="ddlFacility" runat="server" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"
                                    Width="120px" Skin="Outlook" AutoPostBack="true">
                                </telerik:RadComboBox>
                                Provider
                                <telerik:RadComboBox ID="ddlProviders" runat="server" Width="120px" Skin="Outlook"
                                    AutoPostBack="false">
                                </telerik:RadComboBox>
                            </div>
                            <div style="float: right;">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:UpdatePanel ID="upDrug" runat="server">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvDrug" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                PageSize="5" Skin="Office2007" Width="100%" AllowSorting="False" AllowMultiRowSelection="False"
                                AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                GridLines="none" OnPageIndexChanged="gvDrug_PageIndexChanged" OnSelectedIndexChanged="gvDrug_SelectedIndexChanged"
                                OnItemDataBound="gvDrug_ItemDataBound">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                </ClientSettings>
                                <MasterTableView Width="100%">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="SubName" FieldAlias=":" SortOrder="None"></telerik:GridGroupByField>
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="SubName" SortOrder="None"></telerik:GridGroupByField>
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <Columns>
                                        <telerik:GridBoundColumn SortExpression="CPTCode" HeaderText="CPT®Code" DataField="CPTCode">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="ServiceName" ItemStyle-Width="350px" HeaderStyle-Width="350px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn Text="Select" CommandName="Select" ItemStyle-Width="200px"
                                            HeaderText="Select">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                                        ResizeGridOnColumnResize="False"></Resizing>
                                </ClientSettings>
                                <GroupingSettings ShowUnGroupButton="true" />
                            </telerik:RadGrid>
                            <asp:Button ID="btnAddFavorite" runat="server" Text="Add To Favorite" SkinID="Button"
                                OnClick="btnAddFavorite_Click" />
                            <asp:Button ID="btnDeleteFavorite" runat="server" Text="Delete Favorite" SkinID="Button"
                                OnClick="btnDeleteFavorite_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="2" width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upd" runat="server">
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <br />
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="80px" valign="top" align="left">
                                                                <asp:Label ID="lblServiceId" runat="server" Text="CPT®Code"></asp:Label>
                                                                <span class="style1">*</span>
                                                                <asp:HiddenField ID="hdnID" runat="server" Value="0" />
                                                            </td>
                                                            <td valign="top" width="60px">
                                                                <asp:TextBox ID="txtCPT" SkinID="textbox" TabIndex="11" runat="server" ToolTip="CPT Code"
                                                                    Width="60px" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;Description &nbsp;
                                                                <asp:TextBox ID="txtDescription" runat="server" SkinID="textbox" TabIndex="12" Width="280px"
                                                                    ToolTip="Service Description" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:CheckBox ID="chkPullaSuperbill" runat="server" Text="Pull Forward From Prior Exam"
                                                                    TextAlign="Right" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="70px" valign="top">
                                                                Modifier
                                                            </td>
                                                            <td valign="top" width="60px">
                                                                <asp:TextBox ID="txtModifier" SkinID="textbox" runat="server" TabIndex="13" ToolTip="Modifier"
                                                                    MaxLength="2" Width="60px"></asp:TextBox>
                                                                <AJAX:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtModifier"
                                                                    PopupControlID="pnlModifierCode" Position="Right" OffsetX="5">
                                                                </AJAX:PopupControlExtender>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;Units&nbsp;<span class="style1">*</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                                                                    ID="txtUnit" runat="server" SkinID="textbox" TabIndex="14" MaxLength="5" Text="1"
                                                                    Width="50px" ToolTip="Unit"></asp:TextBox><asp:RequiredFieldValidator ValidationGroup="PAC"
                                                                        ID="rfv1" runat="server" ControlToValidate="txtUnit" ErrorMessage="Units" SetFocusOnError="true"
                                                                        Display="None"></asp:RequiredFieldValidator><AJAX:FilteredTextBoxExtender ID="ftb1"
                                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtUnit"
                                                                            ValidChars="+-_">
                                                                        </AJAX:FilteredTextBoxExtender>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;Unit&nbsp;Charge&nbsp;<span class="style1">*</span>
                                                                <asp:TextBox ID="txtUnitCharge" runat="server" SkinID="textbox" Width="50px" TabIndex="15"
                                                                    MaxLength="8" Text="" ToolTip="Unit&nbsp;Charge"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ValidationGroup="PAC" ID="RequiredFieldValidator1" runat="server"
                                                                    Display="None" ControlToValidate="txtUnitCharge" ErrorMessage="Unit Charge" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtUnitCharge" ValidChars="$0123456789.">
                                                                </AJAX:FilteredTextBoxExtender>
                                                            </td>
                                                            <td valign="top">
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;From&nbsp;
                                                                <telerik:RadDatePicker ID="rdpFrom" runat="server" TabIndex="17" Width="100px" DateInput-ReadOnly="true">
                                                                </telerik:RadDatePicker>
                                                                <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                                                            </td>
                                                            <td valign="top">
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;To&nbsp;
                                                                <telerik:RadDatePicker ID="rdpTo" runat="server" TabIndex="17" Width="100px" DateInput-ReadOnly="true">
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top" width="60px">
                                                                ICD&nbsp;Codes<span class="style1">*</span>
                                                            </td>
                                                            <td valign="top" width="50px">
                                                                <input id="hdnICDCode" value='<%# Eval("ICDCodes")%>' type="hidden" runat="server"
                                                                    style="width: 1px" />
                                                                <input id="hdnExitOrNot" value='<%# Eval("ExitOrNot")%>' type="hidden" runat="server"
                                                                    style="width: 1px" />
                                                                <asp:TextBox ID="txtICDCode" runat="server" SkinID="textbox" Wrap="true" Width="200px"
                                                                    ToolTip="Link&nbsp;ICD&nbsp;Codes" TabIndex="18"></asp:TextBox>
                                                                <AJAX:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtICDCode"
                                                                    PopupControlID="pnlICDCodes" Position="Right" OffsetX="5">
                                                                </AJAX:PopupControlExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                &nbsp;<asp:CheckBox ID="chkIsBillable" runat="server" TabIndex="19" Text="Billable"
                                                                    Checked="true" Enabled="True" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddToList" runat="server" ValidationGroup="PAC" TabIndex="20"
                                                                    Text="Add To List" SkinID="button" ToolTip="Add To List" OnClick="btnAddToList_Click" />
                                                                <asp:ValidationSummary ID="vsSummary" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                                    ValidationGroup="PAC" />
                                                                <asp:HiddenField ID="hdnServiceAmount" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnDoctorAmount" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" Value="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvDrug" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="gvAddService" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:UpdatePanel ID="updAddService" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvAddService" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                PageSize="5" Skin="Office2007" Width="100%" AllowSorting="False" AllowMultiRowSelection="False"
                                AllowPaging="False" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                GridLines="none" OnItemDataBound="gvAddService_RowDataBound" OnItemCommand="gvAddService_ItemCommand"
                                OnSelectedIndexChanged="gvDrug_SelectedIndexChanged">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <MasterTableView Width="100%">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="SubName" FieldAlias=":" SortOrder="None"></telerik:GridGroupByField>
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="SubName" SortOrder="None"></telerik:GridGroupByField>
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="CPT®Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCPTCode" runat="server" Text='<%#Eval("CPTCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ServiceName" ItemStyle-Width="350px" HeaderStyle-Width="350px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                                <asp:HiddenField ID="hdnModifierCode" runat="server" Value='<%#Eval("ModifierCode") %>' />
                                                <asp:HiddenField ID="hdnICDId" runat="server" Value='<%#Eval("ICDId") %>' />
                                                <asp:HiddenField ID="hdnFromDate" runat="server" Value='<%#Eval("FromDate") %>' />
                                                <asp:HiddenField ID="hdnToDate" runat="server" Value='<%#Eval("ToDate") %>' />
                                                <asp:HiddenField ID="hdnIsBillable" runat="server" Value='<%#Eval("IsBillable") %>' />
                                                <asp:HiddenField ID="hdnServiceAmount" runat="server" Value='<%#Eval("ServiceAmount") %>' />
                                                <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" Value='<%#Eval("ServiceDiscountAmount") %>' />
                                                <asp:HiddenField ID="hdnDoctorAmount" runat="server" Value='<%#Eval("DoctorAmount") %>' />
                                                <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" Value='<%#Eval("DoctorDiscountAmount") %>' />
                                                <asp:HiddenField ID="hdnPullForward" runat="server" Value='<%#Eval("PullForwardEMCodes") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Units">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnits" runat="server" Text='<%#Eval("Units") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Unit Charge" ItemStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitAmount" runat="server" Text='<%#Eval("ServiceAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Edit" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit1"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Delete" ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                    ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="13px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <GroupingSettings ShowUnGroupButton="true" />
                            </telerik:RadGrid>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddToList" />
                            <asp:AsyncPostBackTrigger ControlID="gvAddService" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="Center">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="cmdSave" runat="server" SkinID="Button" Text="Save" OnClick="cmdSave_OnClick"
                                            CausesValidation="false" Visible="false" />
                                        <asp:HiddenField ID="hdnFavorite" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Panel BorderStyle="Solid" BorderWidth="1px" ID="pnlICDCodes" Style="visibility: hidden;
            position: absolute;" BackColor="#E0EBFD" runat="server" Height="180px" ScrollBars="Auto"
            Width="400">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <aspl:ICD ID="icd" runat="server" width="400" PanelName="pnlICDCodes" ICDTextBox="txtICDCode">
                    </aspl:ICD>
                    <asp:HiddenField ID="hdnGridClientId" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Panel ID="pnlModifierCode" runat="server" ScrollBars="Auto" BackColor="#E0EBFD"
            Style="visibility: hidden; position: absolute;" BorderStyle="Solid" BorderWidth="1px">
            <table width="300px" border="0">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 15%">
                                    Modifier List
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlModifier" runat="server" AutoPostBack="true" Width="300px"
                                        AppendDataBoundItems="true" OnSelectedIndexChanged="ddlModifier_SelectedIndexChanged">
                                        <asp:ListItem Text="Select Modifier" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img id="imgOkModifier" runat="server" alt="Ok" style="cursor: pointer;" src="/Images/Ok.jpg" />
                                    &nbsp;<img id="imgCloseModifier" runat="server" alt="Close" style="cursor: pointer;"
                                        src="/Images/close.jpg" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>

    <script type="text/javascript">
        function CloseScreen() {
            window.close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function Close() {
            var oWindow = GetRadWindow();
            oWindow.argument = null;
            oWindow.close();
        }

        function CheckEmptyDate() {
            if ($get('txtFrom').value == "" || $get('txtTo').value == "") {
                alert("Please select Date first.");
                return false;
            }
        }

        function ShowICDPanel(ctrlPanel, txt1) {
            var ICDarr = new Array();
            var txt = document.getElementById('<%=txtICDCode.ClientID%>');
            ICDarr = txt.value.split(',');

            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
            var tableElement = document.getElementById('rptrICDCodes');
            if (tableElement != null) {
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    col.checked = false;
                }

                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                    for (var j = 0; j < ICDarr.length; j++) {
                        if (chklabel.innerText == ICDarr[j]) {
                            col.checked = true;
                        }
                    }
                }
            }
        }




        function HideLeftPnl() {
        }

        function CheckEmptyDate() {
            if ($get('txtFrom').value == "" || $get('txtTo').value == "") {
                alert("Please select Date first.");
                return false;
            }
        }

        function ShowModifierPanelOnChangeDropDown(CtrlDDL, CtrlNewText, ctrlname) {
            var DropdownList = document.getElementById(CtrlDDL);
            var txt = document.getElementById(CtrlNewText);
            var dd = DropdownList.value;
            txt.value = dd;
            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
            DropdownList.tooltip = DropdownList.selecteditem;

        }

        function showModifierPanel(ctrlPanel) {
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
        }

        function HideICDPanel(ctrlPanel) {
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'hidden';
        }
    </script>


</body>
</html>
