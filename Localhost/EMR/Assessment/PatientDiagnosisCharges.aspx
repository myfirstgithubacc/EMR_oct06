<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientDiagnosisCharges.aspx.cs"
    Inherits="EMR_Assessment_PatientDiagnosisCharges"  Title="" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <title></title>

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
            var txt = document.getElementById("txtICDCode");
            ICDarr = txt.value.split(',');

            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
            var tableElement = document.getElementById("rptrICDCodes");
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
            //$get("pnlLeft").style.visibility = 'hidden';
        }
        function CheckEmptyDate() {
            if ($get('txtFrom').value == "" || $get('txtTo').value == "") {
                alert("Please select Date first.");
                return false;
            }
        }

        //        function ValidateFromDate() {
        //            var txtFrom = $get('txtFrom').value;
        //            var txtTo = $get('txtTo').value;
        //            var dFrom = new Date(txtFrom);
        //            var dTo = new Date(txtTo);
        //            if (dTo < dFrom) {
        //                alert("To Date Should Be Greater Than or equal From Date..");
        //                return false;
        //            }
        //            else {
        //                return true
        //            }
        //        }

        

        function showModifierPanel(ctrlPanel) {
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
        }
    </script>
    <style type="text/css">
    .style1
        {
            color: #FF0000;
        }
    </style>

</head>
<body style="height:850">
    <form id="frmPatientDiagnosisCharges" runat="server" >
        <div style="height:850">
        <asp:ScriptManager ID="smDiagnosis" runat="server">
        </asp:ScriptManager>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="clssubtopicbar" align="right" valign="Middle" style="background-color: #E0EBFD;
                    padding-right: 10px;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl_Msg" runat="server" ForeColor="Green" Font-Bold="true" Style="padding-right: 100px;"></asp:Label>
                            <asp:LinkButton ID="lnkENMCodes" runat="server" CausesValidation="false" Text="E&amp;M Coding"
                                Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkENMCodes_OnClick"></asp:LinkButton>&nbsp;|&nbsp;
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
            <tr>
                <td >
                </td>
            </tr>
            <tr>
                <td >
                    <table width="100%" cellpadding="3" cellspacing="3">
                        <tr>
                            <td colspan="3">
                            <div style="float:left;">
                              <asp:Button ID="btnAll" runat="server" Text="All Services" SkinID="Button" TabIndex="1"
                                                OnClick="btnAll_Click" />
                              <asp:Button ID="btnPast" runat="server" Text="Past Services" TabIndex="2" SkinID="Button"
                               OnClick="btnPast_Click" />
                                <asp:Button ID="btnPastPatient" runat="server" Text="Past Patient Services" TabIndex="3"
                                                SkinID="Button" OnClick="btnPastPatient_Click" />
                               <asp:Button ID="btnOrderSets" runat="server" Text="Order Sets" TabIndex="4" SkinID="Button"
                                                OnClick="btnOrderSets_Click" />
                                                 <asp:Button ID="btnFavourites" runat="server" SkinID="Button" Text="Favorites" TabIndex="5"
                                                OnClick="btnFavourites_Click" />
                              
                                </div>
                                  <div style="float:right;">
                                                <asp:Button ID="btnNew" runat="server" SkinID="Button" Text="New" TabIndex="6" CausesValidation="false" 
                                                OnClick="btnNew_Click" />
                                                <button id="img1" runat="server" class="buttonBlue" onclick="javascript:window.close();">
                                                Close</button>
                                         </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updInvSetName" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Literal ID="ltrlInvSetName" runat="server" Text="&nbsp;&nbsp;Order Sets"></asp:Literal>&nbsp;
                                        <asp:Literal ID="ltrlInvCategory" runat="server" Text="Category"></asp:Literal>
                                        &nbsp;<asp:DropDownList ID="ddlInvSetName" SkinID="DropDown" runat="server" AutoPostBack="true"
                                            TabIndex="6" Font-Size="12px" Width="230px" OnSelectedIndexChanged="ddlInvSetName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlSubDepartment" runat="server" AppendDataBoundItems="true"
                                            TabIndex="7" SkinID="DropDown" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged"
                                            AutoPostBack="true" Width="230px">
                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnOrderSets" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAll" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPast" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPastPatient" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                        <asp:AsyncPostBackTrigger ControlID="btnFavourites" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <%--<asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">--%>
                                <table width="70%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 30%">
                                            <asp:DropDownList ID="ddlSearchCriteria" runat="server" TabIndex="8" SkinID="DropDown">
                                                <asp:ListItem Text="Anywhere" Value="1" />
                                                <asp:ListItem Text="Starts With" Value="2" />
                                                <asp:ListItem Text="Ends With" Value="3" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 40%">
                                            <asp:TextBox ID="txtSearch" Width="130px" SkinID="textbox" TabIndex="9" runat="server"
                                                ToolTip="Enter&nbsp;search&nbsp;criteria" />
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Button ID="btnSearch" SkinID="button" Text="Search" TabIndex="10" runat="server"
                                                ToolTip="Search" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <%--   </asp:Panel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                        &nbsp;</td>
                                    
                                    <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updService" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlServices" runat="server" Width="100%" ScrollBars="Auto">
                                <telerik:RadGrid ID="gvServices" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                    PageSize="5" Skin="Office2007" Width="99%" AllowSorting="False" AllowMultiRowSelection="False"
                                    AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                    GridLines="none" OnItemDataBound="gvServices_OnRowDataBound" OnPageIndexChanged="gvServices_OnPageIndexChanging"
                                    OnSelectedIndexChanged="gvServices_SelectedIndexChanged">
                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                    </ClientSettings>
                                    <MasterTableView Width="100%">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="CPT®Code" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCPTCode" SkinID="label" Text='<%#Eval("CPTCode")%>' ToolTip='<%#Eval("LongDescription") %>'
                                                        Width="60px" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Service Name" HeaderStyle-Width="500px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceName" SkinID="label" Text='<%#Eval("ServiceName")%>' ToolTip='<%#Eval("LongDescription") %>'
                                                        Width="500px" runat="server" />
                                                    <asp:HiddenField ID="hdnServiceID" runat="server" Value='<%#Eval("ServiceID")%>' />
                                                    <asp:HiddenField ID="hdnLongDescription" runat="server" Value='<%#Eval("LongDescription") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridButtonColumn Text="Select" CommandName="Select" ItemStyle-Width="100px"
                                                HeaderText="Select">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                                            ResizeGridOnColumnResize="False"></Resizing>
                                    </ClientSettings>
                                </telerik:RadGrid>
                                <%--<asp:GridView ID="gvServices" SkinID="gridview" CellPadding="2" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="ServiceID" ShowHeader="true" PageSize="6" Width="100%" AllowPaging="true"
                                    HeaderStyle-HorizontalAlign="Left" PagerSettings-Mode="NumericFirstLast" PageIndex="0"
                                    ShowFooter="false" PagerSettings-Visible="true" OnRowDataBound="gvServices_OnRowDataBound"
                                    OnPageIndexChanging="gvServices_OnPageIndexChanging" OnSelectedIndexChanged="gvServices_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Service ID" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceID" Text='<%#Eval("ServiceID")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CPTCode" HeaderText="CPT&nbsp;Code" Visible="true" ReadOnly="true"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" Text='<%#Eval("ServiceName")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceNameLongDescription" Text='<%#Eval("LongDescription")%>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="Select" ButtonType="Link" ControlStyle-ForeColor="Blue"
                                            ControlStyle-Font-Underline="true" SelectText="Select" CausesValidation="false"
                                            HeaderStyle-HorizontalAlign="Left" ShowSelectButton="true">
                                            <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                        </asp:CommandField>
                                    </Columns>
                                    <PagerSettings PageButtonCount="6" />
                                </asp:GridView>--%></asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                            <asp:AsyncPostBackTrigger ControlID="btnAll" />
                            <asp:AsyncPostBackTrigger ControlID="btnPast" />
                            <asp:AsyncPostBackTrigger ControlID="btnPastPatient" />
                            <asp:AsyncPostBackTrigger ControlID="btnOrderSets" />
                            <asp:AsyncPostBackTrigger ControlID="btnFavourites" />
                            <asp:AsyncPostBackTrigger ControlID="ddlInvSetName" />
                            <asp:AsyncPostBackTrigger ControlID="ddlSubDepartment" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upd" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top" width="85px" align="left">
                                                    <asp:Label ID="lblServiceId" runat="server" Text="CPT®Code"></asp:Label>
                                                    <span class="style1">*</span>
                                                    <asp:HiddenField ID="hdnID" runat="server" Value="0" />&nbsp;
                                                
                                                </td>
                                                <td valign="top" width="60px" align="left">
                                                    <asp:TextBox ID="txtCPT" SkinID="textbox" TabIndex="11" runat="server" ReadOnly="true"
                                                        ToolTip="CPT Code" Width="60px"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="left">
                                                    &nbsp;Description &nbsp;
                                                    <asp:TextBox ID="txtDescription" runat="server" SkinID="textbox" TabIndex="12" Width="280px"
                                                        ToolTip="Service Description" ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="left">
                                                    <%--<asp:RequiredFieldValidator ID="revtxtDescription" runat="server" ControlToValidate="txtDescription"
                                                    ErrorMessage="Enter Service Name." ValidationGroup="PAC" Display="None"></asp:RequiredFieldValidator>--%>
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
                                                    <asp:TextBox ID="txtModifier" SkinID="textbox" runat="server" TabIndex="13" 
                                                        ToolTip="Modifier" ValidationGroup="PAC"
                                                        MaxLength="2" Width="60px"   ></asp:TextBox>
                                                    <AJAX:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtModifier"
                                                        PopupControlID="pnlModifierCode" Position="Right" OffsetX="5">
                                                    </AJAX:PopupControlExtender>
                                                </td>
                                                <td valign="top">
                                                    &nbsp;Units&nbsp;<span class="style1">*</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                                                        ID="txtUnit" Text="1" ReadOnly="false" runat="server" SkinID="textbox" TabIndex="14" ValidationGroup="PAC"
                                                        MaxLength="5" Width="50px" ToolTip="Unit"></asp:TextBox><%--   <asp:RegularExpressionValidator ID="revUnits" runat="server" ControlToValidate="txtUnit"
                                                        Display="None" ErrorMessage="Only Number(0-9) in Units" ValidationExpression="^\d+$"
                                                        ValidationGroup="PAC"></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator ValidationGroup="PAC"
                                                        ID="rfv1" runat="server" ControlToValidate="txtUnit" ErrorMessage="Units" SetFocusOnError="true"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                    <AJAX:FilteredTextBoxExtender TargetControlID="txtUnit" FilterType="Numbers" runat="server"
                                                        ID="filterUnit">
                                                    </AJAX:FilteredTextBoxExtender>
                                                </td>
                                                <td valign="top">
                                                    Unit&nbsp;Charge&nbsp;<span class="style1">*</span>
                                                   <asp:TextBox ID="txtUnitCharge" runat="server" ValidationGroup="PAC" 
                                                        SkinID="textbox" Width="50px" TabIndex="15"
                                                        MaxLength="8" ToolTip="Unit&nbsp;Charge" 
                                                        ></asp:TextBox>
                                                   <asp:RequiredFieldValidator ValidationGroup="PAC" ID="RequiredFieldValidator2" runat="server"
                                                    Display="None" ControlToValidate="txtUnitCharge" ErrorMessage="Unit Charge" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    <AJAX:FilteredTextBoxExtender TargetControlID="txtUnitCharge" FilterType="Custom"
                                                        runat="server" ID="FilteredTextBoxExtender1" ValidChars="$0123456789.">
                                                    </AJAX:FilteredTextBoxExtender>
                                                </td>
                                                <td valign="top">
                                                </td>
                                                <td valign="top">
                                                    &nbsp;From&nbsp;
                                                    <telerik:RadDatePicker ID="rdpFrom" runat="server" TabIndex="17" Width="100px" DateInput-ReadOnly="true">
                                                    </telerik:RadDatePicker>
                                                    <%--<asp:TextBox ID="txtFrom" SkinID="textbox" runat="server" Columns="8" onkeydown="NextTab();"></asp:TextBox>
                                                <AJAX:CalendarExtender ID="CEtxtOnsetDate" runat="server" TargetControlID="txtFrom"
                                                    CssClass="MyCalendar" Format="dd/MM/yyyy" PopupPosition="Left">
                                                </AJAX:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="REVtxtOnsetDate" runat="server" ControlToValidate="txtFrom"
                                                    ValidationExpression="^(((0[1-9]|[1-2][0-9]|3[0-1])\/(0[13578]|(10|12)))|((0[1-9]|[1-2][0-9])\/02)|((0[1-9]|[1-2][0-9]|30)\/(0[469]|11)))\/(19|20)\d\d$"
                                                    SetFocusOnError="true" Display="None" ValidationGroup="PAC" ErrorMessage="From Date Should Be In dd/MM/yyyy Format">
                                                </asp:RegularExpressionValidator>
                                                <AJAX:FilteredTextBoxExtender ID="FBEtxtOnsetDate" runat="server" TargetControlID="txtFrom"
                                                    FilterType="Custom, Numbers" ValidChars="_/">
                                                </AJAX:FilteredTextBoxExtender>
                                               --%>
                                                    <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                                                </td>
                                                <td valign="top">
                                                </td>
                                                <td valign="top">
                                                    &nbsp;To&nbsp;
                                                    <telerik:RadDatePicker ID="rdpTo" runat="server" TabIndex="17" Width="100px" DateInput-ReadOnly="true">
                                                    </telerik:RadDatePicker>
                                                    <%-- <asp:TextBox ID="txtTo" runat="server" SkinID="textbox" Width="60px" TabIndex="17"></asp:TextBox>
                                                <AJAX:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo"
                                                    CssClass="MyCalendar" Format="dd/MM/yyyy" PopupPosition="Left">
                                                </AJAX:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTo"
                                                    ValidationExpression="^(((0[1-9]|[1-2][0-9]|3[0-1])\/(0[13578]|(10|12)))|((0[1-9]|[1-2][0-9])\/02)|((0[1-9]|[1-2][0-9]|30)\/(0[469]|11)))\/(19|20)\d\d$"
                                                    SetFocusOnError="true" Display="None" ValidationGroup="PAC" ErrorMessage="To Date Date Should Be In dd/MM/yyyy Format">
                                                </asp:RegularExpressionValidator>
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTo"
                                                    FilterType="Custom, Numbers" ValidChars="_/">
                                                </AJAX:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table>
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
                                                    <asp:CheckBox ID="chkIsBillable" runat="server" Checked="true" TabIndex="19" Text="Billable"
                                                        Enabled="True" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAddToList" runat="server" ValidationGroup="PAC" TabIndex="20" CausesValidation="true"
                                                        Text="Add To List" SkinID="button" ToolTip="Add To List" OnClick="btnAddToList_Click" />
                                                    <asp:ValidationSummary ID="vsSummary" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                                    ValidationGroup="PAC" />   
                                                <asp:HiddenField ID="hdnServiceAmount" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnDoctorAmount" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnModifierID" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServices" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="gvAddService" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                
                                <asp:Panel ID="pnlAddService" runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:UpdatePanel ID="updAddService" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                              
                                        <telerik:RadGrid ID="gvAddService" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                            PageSize="5" Skin="Office2007" Width="99%" AllowSorting="False" AllowMultiRowSelection="False"
                                            AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                            GridLines="none" OnItemDataBound="gvAddService_RowDataBound" OnItemDeleted="gvAddService_ItemDeleted"
                                            OnSelectedIndexChanged="gvAddService_SelectedIndexChanged" OnItemCommand="gvAddService_RowCommand">
                                            <PagerStyle Mode="NumericPages"></PagerStyle>
                                            <MasterTableView Width="100%">
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="CPT®Code" HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCPTCode" SkinID="label" Text='<%#Eval("CPTCode")%>' ToolTip='<%#Eval("LongDescription") %>'
                                                                Width="60px" runat="server" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Service Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceName" SkinID="label" Text='<%#Eval("ServiceName")%>' runat="server" />
                                                            <asp:HiddenField ID="lblServiceID" runat="server" Value='<%#Eval("ServiceID")%>' />
                                                            <asp:HiddenField ID="lblId" runat="server" Value='<%#Eval("Id") %>' />
                                                            <asp:HiddenField ID="lblModifierCode" Value='<%#Eval("ModifierCode")%>' runat="server" />
                                                            <asp:HiddenField ID="lblICDId" runat="server" Value='<%#Eval("ICDId")%>' />
                                                            <asp:HiddenField ID="lblFromDate" runat="server" Value='<%#Eval("FromDate")%>' />
                                                            <asp:HiddenField ID="lblToDate" runat="server" Value='<%#Eval("ToDate")%>' />
                                                            <asp:HiddenField ID="chkIsBillable" runat="server" Value='<%#Eval("IsBillable") %>' />
                                                            <asp:HiddenField ID="hdnServiceAmount"  runat="server" Value='<%#Eval("ServiceAmount") %>' />
                                                            <asp:HiddenField ID="hdnServiceDiscountAmount"  runat="server" Value='<%#Eval("ServiceDiscountAmount") %>' />
                                                            <asp:HiddenField ID="hdnDoctorAmount"  runat="server" Value='<%#Eval("DoctorAmount") %>' />
                                                            <asp:HiddenField ID="hdnDoctorDiscountAmount"  runat="server" Value='<%#Eval("DoctorDiscountAmount") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                    <telerik:GridTemplateColumn HeaderText="Units">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnits" Text='<%#Eval("Units")%>' runat="server" />
                                                    </ItemTemplate>
                                                   </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Unit Charge">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitAmount" runat="server" Text='<%#Eval("ServiceAmount") %>' />
                                                    </ItemTemplate>
                                                   </telerik:GridTemplateColumn>
                                                   
                                                   <telerik:GridTemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Select"></asp:LinkButton>
                                                        </ItemTemplate>
                                                   </telerik:GridTemplateColumn>
     
                       
                                                    <telerik:GridTemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="false" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" Width="13px" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                           
                                        </telerik:RadGrid>
                                        
                                             
                    
                                            <%--<asp:GridView ID="gvAddService" SkinID="gridview" CellPadding="4" runat="server"
                                                DataKeyNames="Id" AutoGenerateColumns="false" ShowHeader="true" PageSize="10"
                                                Width="100%" AllowPaging="false" HeaderStyle-HorizontalAlign="Left" PagerSettings-Mode="NumericFirstLast"
                                                PageIndex="0" ShowFooter="false" PagerSettings-Visible="true" OnSelectedIndexChanged="gvAddService_SelectedIndexChanged"
                                                OnRowCommand="gvAddService_RowCommand" OnRowDataBound="gvAddService_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service ID" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceID" Text='<%#Eval("ServiceID")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CPTCode" HeaderText="CPT&nbsp;Code" Visible="true" ReadOnly="true"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceName" Text='<%#Eval("ServiceName")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Modifier" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModifierCode" Text='<%#Eval("ModifierCode")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Units" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnits" Text='<%#Eval("Units")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit&nbsp;Charge" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnitAmount" Text='<%#Eval("ServiceAmount")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICDCodes" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblICDId" Text='<%#Eval("ICDId")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFromDate" Text='<%#Eval("FromDate")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblToDate" Text='<%#Eval("ToDate")%>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Billable" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsBillable" runat="server" Text='<%#Eval("IsBillable") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True">
                                                        <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                    </asp:CommandField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" Width="13px" /></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ServiceDiscountAmount" />
                                                    <asp:BoundField DataField="DoctorAmount" />
                                                    <asp:BoundField DataField="DoctorDiscountAmount" />
                                                </Columns>
                                            </asp:GridView>--%>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAddToList" />
                                            <asp:AsyncPostBackTrigger ControlID="gvAddService" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSave" runat="server" Text="Save" SkinID="button" ToolTip="Save"
                                    TabIndex="21" OnClick="btnSave_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Panel BorderStyle="Solid" BorderWidth="1px" ID="pnlICDCodes" Style="visibility: hidden;
            position: absolute;" BackColor="#E0EBFD" runat="server" Height="180px" ScrollBars="Auto"
            Width="400">
            <table width="100%" border="0">
                <tr>
                    <td>
                    <asp:UpdatePanel ID="update" runat="server">
                    <ContentTemplate>
                     <aspl:ICD ID="icd" runat="server" width="400" PanelName="pnlICDCodes" ICDTextBox="txtICDCode"></aspl:ICD>
                           <asp:HiddenField ID="hdnGridClientId" runat="server" />
                     </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlModifierCode" runat="server" ScrollBars="Auto" BackColor="#E0EBFD"
            Style="visibility: hidden; position: absolute;" BorderStyle="Solid" BorderWidth="1px">
            <table width="300px" border="0">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 15%" colspan="2">
                                    Modifier List
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlModifier" runat="server" AutoPostBack="true" Width="300px"
                                        AppendDataBoundItems="true" 
                                        onselectedindexchanged="ddlModifier_SelectedIndexChanged">
                                        <asp:ListItem Text="Select Modifier" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img id="imgOkModifier" runat="server" alt="Ok" style="cursor: pointer;" src="/Images/Ok.jpg" />
                                    &nbsp;
                                    <img id="imgCloseModifier" runat="server" alt="Close" style="cursor: pointer;" src="/Images/close.jpg" />
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
        function ShowModifierPanelOnChangeDropDown(CtrlDDL, CtrlNewText, ctrlname) {
            var DropdownList = document.getElementById(CtrlDDL);
            var txt = document.getElementById(CtrlNewText);
            var dd = DropdownList.value;
            txt.value = dd;
            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
            DropdownList.tooltip = DropdownList.selecteditem;
            
        }

        function HideICDPanel(ctrlname) {
            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
        }
    </script>

</body>
</html>