<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientRequestFromWard.aspx.cs"
    Inherits="Pharmacy_SaleIssue_PatientRequestFromWard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Request From Ward</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function returnToParent() {
            var oArg = new Object();
            oArg.IndentId = document.getElementById("hdnIndentId").value;
            oArg.IndentNo = document.getElementById("hdnIndentNo").value;
            oArg.RegistrationId = document.getElementById("hdnRegistrationId").value;
            oArg.RegistrationNo = document.getElementById("hdnRegistrationNo").value
            oArg.EncounterNo = document.getElementById("hdnEncounterNo").value;
            oArg.EncounterId = document.getElementById("hdnEncounterId").value
            oArg.FacilityId = document.getElementById("hdnFacilityId").value

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        
    </script>

</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript" language="javascript">

        function validateMaxLength() {
            var txt = $get('<%=txtSearchRegNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
            
        
        }
    </script>

    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
       <%-- <div>
            <asp:Button ID="btnPickListPrint" runat="server" OnClick="btnPickListPrint_OnClick"
                                Width="5px" Style="visibility: hidden;" />
        </div>--%>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr style="background-color: #E0EBFD">
                        <td align="center" style="font-size: 12px;">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </td>
                        <td align="right" style="width: 200px">
                            <asp:Button ID="btnSearch" runat="server" SkinID="Button" Text="Filter" OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server" SkinID="Button" Text="Clear Filter"
                                OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />&nbsp;
                            
                        </td>
                      
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="2" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Search On" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSearchOn" SkinID="DropDown" runat="server" Width="140px"
                                AutoPostBack="true" OnTextChanged="ddlSearchOn_OnTextChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, ipno%>" Value="1" Selected="true" />
                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, regno%>" Value="2" />
                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, bedno%>' Value="3" />
                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, PatientName%>" Value="4" />
                                     <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, ward%>" Value="5" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                <asp:TextBox ID="txtSearch" runat="server" SkinID="textbox" Width="100px" MaxLength="20" />
                                <asp:TextBox ID="txtSearchRegNo" runat="server" SkinID="textbox" Width="100px" MaxLength="13"
                                    Visible="false" onkeyup="return validateMaxLength();" />

                                <telerik:RadComboBox ID="ddlWard" SkinID="DropDown" runat="server" Width="100px" Visible="false" DropDownWidth="200px" >
                                </telerik:RadComboBox>

                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtSearchRegNo" ValidChars="0123456789" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text="Date Filter" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTime" SkinID="DropDown" runat="server" Width="140px"
                                AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="All" Selected="true"/>
                                    <telerik:RadComboBoxItem Text="Today" Value="Today" />
                                    <telerik:RadComboBoxItem Text="Last Week" Value="LastWeek" />
                                    <telerik:RadComboBoxItem Text="Last Two Weeks" Value="LastTwoWeeks" />
                                    <telerik:RadComboBoxItem Text="Last One Month" Value="LastOneMonth" />
                                    <telerik:RadComboBoxItem Text="Last Three Months" Value="LastThreeMonths" />
                                    <telerik:RadComboBoxItem Text="Last Year" Value="LastYear" />
                                    <telerik:RadComboBoxItem Text="Date Range" Value="DateRange" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <table id="tblDate" runat="server" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" colspan="2">
                            <asp:CheckBox ID="chkMarkForDischarge" runat="server" SkinID="checkbox" Font-Bold="true"
                                Text="Marked&nbsp;For&nbsp;Discharge" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="Order Type" />
                        </td>
                        <td colspan="2">
                            <telerik:RadComboBox ID="ddlOrderType" SkinID="DropDown" runat="server" Width="220px"
                                AutoPostBack="true" />
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" SkinID="label" Text="Order Status" Visible="false" />
                            <asp:Label ID="Label29" runat="server" SkinID="label" Text="Store" />
                        </td>
                        <td width="200px">
                            <telerik:RadComboBox ID="ddlOrderStatus" SkinID="DropDown" runat="server" Width="140px"  Visible="false">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                    <telerik:RadComboBoxItem Text="Partial Pending" Value="P" />
                                    <telerik:RadComboBoxItem Text="Complete Pending" Value="C"  />
                                </Items>
                            </telerik:RadComboBox>
                            <%--Added by Rakesh on 4/9/2013 start--%>

                            <telerik:RadComboBox ID="ddlStore" style="width:200px;" SkinID="DropDown" MarkFirstMatch="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" runat="server" Width="140px" />
                        </td>
                        <td align="right">
                            <asp:Label ID="Label10" runat="server" SkinID="label" Text="Indent Type" />
                        </td>
                        <td >
                            <telerik:RadComboBox ID="ddlDrugOrderType" SkinID="DropDown" runat="server" Width="130px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="0" />
                                    <telerik:RadComboBoxItem Text="Consumable Order" Value="CO" BackColor="Cyan" />
                                    <telerik:RadComboBoxItem Text="Drug Order" Value="DO" BackColor="DarkSeaGreen" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        

                        <td>
                            <asp:Label ID="Label16" runat="server" SkinID="label" Text="Patient Type" />
                        </td>
                        <td >
                            <telerik:RadComboBox ID="ddlPatientType" SkinID="DropDown" runat="server" Width="130px">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="A" />
                                    <telerik:RadComboBoxItem Text="IP" Value="I" />
                                    <telerik:RadComboBoxItem Text="ED" Value="E"  />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        

                    </tr>
                    <tr>
                        <td colspan="9">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel3" BorderWidth="0" BorderStyle="Solid" ScrollBars="Auto" runat="server">
                                            <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="0">
                                                <asp:TableRow>

                                                     <asp:TableCell>
                                                        <asp:Label ID="Label27" runat="server" BorderWidth="1px" BackColor="SpringGreen" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label28" runat="server" SkinID="label" Text="Partial Pending  " />
                                                    </asp:TableCell>

                                                    <asp:TableCell>
                                                        <asp:Label ID="Label19" runat="server" BorderWidth="1px" BackColor="#ff99ff" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label24" runat="server" SkinID="label" Text="New Admission  " />
                                                    </asp:TableCell>

                                                    <asp:TableCell>
                                                        <asp:Label ID="Label7" runat="server" BorderWidth="1px" BackColor="#FFFBC7" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="Routine  " />
                                                    </asp:TableCell>

                                                    <asp:TableCell>
                                                        <%--<asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="#EDD5B7" SkinID="label"
                                                            Width="15px" Height="15px" />--%>
                                                        <asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="PaleVioletRed" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label21" runat="server" SkinID="label" Text="STAT  " />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label22" runat="server" BorderWidth="1px" BackColor="#0BF063" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label23" runat="server" SkinID="label" Text="Discharge  " />
                                                    </asp:TableCell>
                                                    <%--<asp:TableCell>
                                                        <asp:Label ID="Label5" runat="server" BorderWidth="1px" BackColor="#FC0404" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="Urgent" />
                                                    </asp:TableCell>--%>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label8" runat="server" BorderWidth="1px" BackColor="#87CEFA" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label9" runat="server" SkinID="label" Text="Marked For Discharge  " />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                        <asp:Label ID="Label14" runat="server" BorderWidth="1px" BackColor="Cyan" SkinID="label"
                                                            Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label12" runat="server" SkinID="label" Text="Consumable  " />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label15" runat="server" BorderWidth="1px" BackColor="DarkSeaGreen"
                                                            SkinID="label" Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label13" runat="server" SkinID="label" Text="Drug  " />
                                                    </asp:TableCell>

                                                    <asp:TableCell>
                                                        <asp:Label ID="Label5" runat="server" BorderWidth="1px" BackColor="Yellow"
                                                            SkinID="label" Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="Indent more than 1 hour" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label25" runat="server" BorderWidth="1px" BackColor="Green"
                                                            SkinID="label" Width="15px" Height="15px" />
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="Label26" runat="server" SkinID="label" Text="Narcotic  " />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <%--Added by Rakesh on 4/9/2013 end	--%>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="99%" BorderWidth="0"
                                BorderColor="SkyBlue" ScrollBars="Auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvEncounter" runat="server" Skin="Office2007" BorderWidth="0"
                                            HeaderStyle-Wrap="false" PagerStyle-ShowPagerText="true" AllowFilteringByColumn="false"
                                            AllowMultiRowSelection="false" Width="100%" AutoGenerateColumns="False" ShowStatusBar="true"
                                            EnableLinqExpressions="false" GridLines="Both" AllowPaging="false" OnItemDataBound="gvEncounter_OnItemDataBound"
                                            AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" PageSize="17" OnItemCommand="gvEncounter_OnItemCommand"
                                            OnPageIndexChanged="gvEncounter_OnPageIndexChanged">
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="true" />
                                            <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <ItemStyle Wrap="true" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Select" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnSelect" runat="server" Text="Select" CommandName="Select" />
                                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                                            <asp:HiddenField ID="hdnIndentNo" runat="server" Value='<%#Eval("IndentNo")%>' />
                                                            <asp:HiddenField ID="hdnFacilityID" runat="server" Value='<%#Eval("FacilityID")%>' />
                                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                            <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%#Eval("OrderType")%>' />
                                                            <asp:HiddenField ID="hdnEncounterStatusCode" runat="server" Value='<%# Eval("EncounterStatusCode") %>' />
                                                            <asp:HiddenField ID="hdnIsIndentMorethan1Hour" runat="server" Value='<%# Eval("IsIndentMorethan1Hour") %>' />
                                                            
                                                            <asp:HiddenField ID="hdnIsStat" runat="server" Value='<%# Eval("IsStat") %>' />
                                                            <%--Added by rakesh on 3/09/2013 for color start--%>
                                                            <asp:HiddenField ID="hdnColorCode" runat="server" Value='<%# Eval("Colorcode") %>' />
                                                            <%--Added by rakesh on 3/09/2013 for color end--%>
                                                            <asp:HiddenField ID="hdnIsConsumable" runat="server" Value='<%# Eval("IsConsumable") %>' />
                                                            <asp:HiddenField ID="hdnIsPickListPrinted" runat="server" Value='<%# Eval("IsPickListPrinted") %>' />
                                                            <asp:HiddenField ID="hdnIsApproved" runat="server" Value='<%# Eval("IsApproved") %>' />
                                                           <asp:HiddenField ID="hdnPending" runat="server" Value='<%# Eval("Pending") %>' /> 
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                        HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, ipno%>'
                                                        HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="IndentNo" HeaderText="Indent No" HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentNo" runat="server" Text='<%#Eval("IndentNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="IndentDate" HeaderText="Indent Date" HeaderStyle-Width="90px"
                                                        DataField="IndentDate" />
                                                    <telerik:GridBoundColumn UniqueName="PatientName" HeaderText="Patient Name" DataField="PatientName"
                                                        HeaderStyle-Width="130px" />
                                                    <telerik:GridBoundColumn UniqueName="BedNo" HeaderText="Bed No" HeaderStyle-Width="50px"
                                                        DataField="BedNo" />
                                                    <telerik:GridBoundColumn UniqueName="WardName" HeaderText="Ward Name" HeaderStyle-Width="100px"
                                                        DataField="WardName" />
                                                    <telerik:GridBoundColumn UniqueName="EncounterStatus" HeaderText="Status" HeaderStyle-Width="40px"
                                                        DataField="EncounterStatus" />
                                                    <telerik:GridBoundColumn UniqueName="RequestedBy" HeaderText="Requested By" HeaderStyle-Width="130px"
                                                        DataField="RequestedBy" />
                                                    <telerik:GridBoundColumn UniqueName="EncodedBy" HeaderText="Encoded By" HeaderStyle-Width="50px"
                                                        DataField="EncodedBy" />
                                                    <telerik:GridBoundColumn UniqueName="ApprovedBy" HeaderText="Approved By" HeaderStyle-Width="40px"
                                                        DataField="ApprovedBy" />
                                                    <telerik:GridTemplateColumn HeaderText="Print" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblPrint" CommandName="Print" runat="server" Text="Print"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn HeaderText="Pick List" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblPickList" CommandName="PickList" runat="server" Text="Pick List"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                     <telerik:GridTemplateColumn HeaderText="Case Sheet" HeaderStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblCaseSheet" CommandName="CaseSheet" runat="server" Text="Case Sheet"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                     <telerik:GridTemplateColumn HeaderText="Diagnostic History" HeaderStyle-Width="130px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblDiagnosticHis" CommandName="DiagnosticHistory" runat="server" Text="Diagnostic History"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvEncounter" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblTotalCountConsumable" Font-Bold="true" runat="server" SkinID="label"
                                Text="0" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblTotalCountNonConsumable" Font-Bold="true" runat="server" SkinID="label"
                                Text="0" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblTotalRowCount" Font-Bold="true" runat="server" SkinID="label" Text="0" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnIndentId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnIndentNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnFacilityId" runat="server" />
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </td>
                    </tr>
                </table>
            </ContentTemplate> 
            <Triggers>
               <asp:PostBackTrigger ControlID="ddlTime" />
                  <asp:PostBackTrigger ControlID="txtFromDate" />
                  <asp:PostBackTrigger ControlID="txtToDate" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
