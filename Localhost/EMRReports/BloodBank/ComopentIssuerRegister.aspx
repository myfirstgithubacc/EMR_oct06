<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComopentIssuerRegister.aspx.cs"
    MasterPageFile="~/Include/Master/EMRMaster.master" Inherits="EMRReports_BloodBank_ComopentIssuerRegister" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:UpdatePanel ID="upanelCom_Issu_register" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="3" cellspacing="0">
                <tr class="clsheader">
                    <td id="tdHeader" align="left" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="" Font-Bold="true" />
                    </td>
                    <td align="center">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td align="center">
                        <table cellpadding="2" border="0" cellspacing="0" style="color: black; background: #e0ebfd;">
                            <tr>
                               <td style="padding-left: 5px; width: 20%;">
                                    <asp:Label ID="lblReportType" runat="server" Text="Report Type" SkinID="label"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlReportType" runat="server" AutoPostBack="true" Width="300px"
                                        OnSelectedIndexChanged="ddlReportType_selectedIndexchanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Select Report" Value="0" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Blood component Issue & Transfuse details" Value="BCITD" />
                                            <telerik:RadComboBoxItem Text="CELL GROUPING REGISTER" Value="CGR" />
                                            <telerik:RadComboBoxItem Text="Componet Division" Value="CD" />
                                            <telerik:RadComboBoxItem Text="Daily consumption of blood bag & Apheresis" Value="DCBBA" />
                                            <telerik:RadComboBoxItem Text="Deferral Donor Registration Record" Value="DDRR" />
                                            <telerik:RadComboBoxItem Text="DISCARD REGISTER REPORT" Value="DRR" />
                                            <telerik:RadComboBoxItem Text="Donor Registration Record with Patient Details" Value="DRRPD" />
                                            <telerik:RadComboBoxItem Text="Donation Due Status Reports" Value="DDSR" />
                                            <telerik:RadComboBoxItem Text="Donor Registration Details" Value="DRD" />
                                            <telerik:RadComboBoxItem Text="Nearest Expiry Unit" Value="NEU" />
                                            <telerik:RadComboBoxItem Text="Stock Register for component" Value="SRC" />
                                            <telerik:RadComboBoxItem Text="Voluntary Donor List" Value="VDL" />
                                            <telerik:RadComboBoxItem Text="TAT For Blood Bank" Value="TAT" />
                                            <telerik:RadComboBoxItem Text="Bag History" Value="BH" />
                                            <telerik:RadComboBoxItem Text="Reserve Unit Stock" Value="RUC" />
                                            <telerik:RadComboBoxItem Text="Daily Stock Unit Wise" Value="DSUW" />
                                            <telerik:RadComboBoxItem Text="Blood Component Issue Register" Value="BCIR" />
                                            <telerik:RadComboBoxItem Text="Unscreened Blood Stock" Value="USBS" />
                                            <telerik:RadComboBoxItem Text="Donor Registration Blood Group Wise" Value="DRBGW" />
                                            <telerik:RadComboBoxItem Text="Results Of Elisa Screening" Value="ROES" />
                                            <telerik:RadComboBoxItem Text="Bag Screening Net Results" Value="BSNR" />
                                            <telerik:RadComboBoxItem Text="Cross Match Charge Slip Report" Value="CMCSR" />
                                            <telerik:RadComboBoxItem Text="Blood bank services Report" Value="BBSR" />
                                            <telerik:RadComboBoxItem Text="Discard Report" Value="BDR" />
                                            <telerik:RadComboBoxItem Text="Component Division Report" Value="CDR" />
                                            <telerik:RadComboBoxItem Text="NAT Report" Value="NTR" />
                                            <telerik:RadComboBoxItem Text="DonorRegistrationList" Value="DRL" />
											<%--
                                                 <telerik:RadComboBoxItem Text="Component Cross Match" Value="CMM" />
                                            <telerik:RadComboBoxItem Text="Donor List Gender Wise" Value="DLGW" />
                                                <telerik:RadComboBoxItem Text="Blood Issue Register Componet Wise" Value="BIRCW" />
                                            <telerik:RadComboBoxItem Text="Component Issue Register"       Value="CIR" />
                                            <telerik:RadComboBoxItem Text="Componet Division"              Value="CD"  />
                                            <telerik:RadComboBoxItem Text="BagDetails"                     Value="BD" />
                                            <telerik:RadComboBoxItem Text="Component Division Bag Wise"    Value="BCDBW" />
                                            <telerik:RadComboBoxItem  Text="Blood Transfer Date Wise"      Value="BTDW" />
                                            <telerik:RadComboBoxItem Text="Blood Transfer Report"          Value="BTR" /> 
                                            <telerik:RadComboBoxItem Text="Camp Wise Donor Detail"         Value="CWDD" />
                                            <telerik:RadComboBoxItem Text="Compatibility Report"           Value="CBR" />
                                            <telerik:RadComboBoxItem Text="Component Cross Matched Report" Value="CCMR" />
                                            <telerik:RadComboBoxItem Text="Component Recevied Report"      Value="CRR" />
                                            <telerik:RadComboBoxItem Text="Component Stock "               Value="CS" />
                                            <telerik:RadComboBoxItem Text="Daily Stock Report"             Value="DSR" />
                                            <telerik:RadComboBoxItem Text="Daily Discard Detail"           Value="DDD" />
                                            <telerik:RadComboBoxItem Text="Donor Report"                   Value="DR" />
                                            <telerik:RadComboBoxItem Text="Donor Physical Examination"     Value="DPE" />
                                            <telerik:RadComboBoxItem Text="Elisa Screeing"                 Value="ES" />
                                            <telerik:RadComboBoxItem Text="Expiry Detail"                  Value="ED" />
                                            <telerik:RadComboBoxItem Text="Fresh Collection"               Value="FC" />  
                                            <telerik:RadComboBoxItem Text="Kit Stock Report"               Value="KSR" />
                                            <telerik:RadComboBoxItem Text="Tranfusion Report"              Value="TFR" />
                                            <telerik:RadComboBoxItem Text="Transaction Report"             Value="TSR" />
                                            <telerik:RadComboBoxItem Text="UnSceenBlood Stock Report"      Value="UBSR" />
                                            <telerik:RadComboBoxItem Text="Voluntary Genderwise Report"    Value="VGR" />
                                            <telerik:RadComboBoxItem Text="Manufacture Wise Bag Detauls"   Value="MWBD" />  
                                            <telerik:RadComboBoxItem Text="Cell Report"                    Value ="CR" />
                                            <telerik:RadComboBoxItem Text="Secrum Report"                  Value="SR" />
                                            <telerik:RadComboBoxItem  Text="Whole Blood Issue Register"    Value="WBIR" />--%>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkExport" runat="server" Text="Export" SkinID="checkbox" AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPrintPreview" runat="server" ToolTip="Click to Print Preview"
                                        OnClick="btnPrintPreview_Click" SkinID="Button" Text="Print Preview" />
                                </td>
                            </tr>
                            <tr>
                                <td id="tddate" colspan="6" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="6">
                                                <asp:Panel runat="server" ID="pnlReg">
                                                    <%--<div id="pnlReg" runat="server">--%>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="padding-left: 5px; width: 20%;">
                                                                <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Text="UHID"></asp:Label>
                                                                <span style="color: Red">*</span>
                                                            </td>
                                                            <td style="padding-left: 5px; width: 20%;">
                                                                <asp:TextBox ID="txtRegistration" runat="server" SkinID="textbox" Width="200px" AutoPostBack="true"
                                                                    OnTextChanged="txtRegistration_TextChanged"></asp:TextBox>
                                                            </td>
                                                            </tr>
                                                            <tr>
                                                            <td style="width: 110px">
                                                                <asp:Label ID="lblIPNo" runat="server" SkinID="label" Text="IP No"></asp:Label>
                                                            </td>
                                                            <%--<td style="width: 245px">
                                                                <asp:TextBox ID="txtIPNo" runat="server" SkinID="textbox" Width="200px" AutoPostBack="true"
                                                                    OnTextChanged="txtIPNo_TextChanged"></asp:TextBox>
                                                            </td>--%>
                                                            <td style="width: 245px">
                                                            <%--<asp:UpdatePanel ID="upencounterno" runat="server">
                                                            <ContentTemplate>--%>
                                                            
                                                            <asp:DropDownList ID="ddlEncounterNo" runat="server" SkinID="textbox" Width="200px" AutoPostBack="true" 
                                                                    onselectedindexchanged="ddlEncounterNo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <%--</ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                            <td style="width: 74px">
                                                                &nbsp;&nbsp;<asp:Label ID="lblUnitNo" runat="server" SkinID="label" Text="Unit No"></asp:Label>
                                                                <%--<span style="color: Red">*</span>&nbsp;--%>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlUnitNo" runat="server" SkinID="textbox" Width="200px">
                                                                </asp:DropDownList>
                                                                <%--<asp:TextBox ID="txtUnitNo" runat="server" SkinID="textbox" Width="200px"></asp:TextBox>--%>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td colspan="6">
                                                <tr runat="server" id="trUHID">
                                                    <td style="padding-left: 5px; width: 20%;">
                                                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="UHID"></asp:Label> 
                                                    </td>
                                                    <td style="padding-left: 5px; width: 20%;">
                                                        <asp:TextBox ID="txtUHID" runat="server" SkinID="textbox" Width="200px" AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                              <%--  <tr runat="server" id="trBagNo">
                                                    <td style="padding-left: 5px; width: 20%;">
                                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Bag No"></asp:Label> 
                                                    </td>
                                                    <td style="padding-left: 5px; width: 20%;">
                                                        <asp:TextBox ID="txtBagNoForNAT" runat="server" SkinID="textbox" Width="200px" AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                     <td style="padding-left: 5px; width: 20%;">
                                                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="NAT Result"></asp:Label> 
                                                    </td>
                                                   
                                                </tr>--%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="padding-left: 5px; width: 20%;">
                                                <asp:Panel ID="pnlFrom" runat="server">
                                                    <asp:Label ID="lblfromdate" runat="server" SkinID="label" Text="From Date"></asp:Label>
                                                    <span style="color: Red" runat="server" id="span">*</span></asp:Panel>
                                                
                                            </td>
                                            <td style="padding-left: 5px; width: 20%;">
                                                <telerik:RadDatePicker ID="dtpfromdate" runat="server" DateInput-DateFormat="dd/MM/yyyy" Width="120px">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Panel ID="pnl" runat="server">
                                                    <asp:Label ID="lbltodate" runat="server" SkinID="label" Text="To "></asp:Label>
                                                </asp:Panel>
                                            </td>
                                            <td colspan="3" style="text-align: left;">
                                                <telerik:RadDatePicker ID="dtpTodate" runat="server" DateInput-DateFormat="dd/MM/yyyy" Width="120px">
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdtxtCtr" runat="server" colspan="4">
                                    <table>
                                        <tr>
                                            <td style="height: 29px">
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblBagNo" runat="server" Text="Bag No" SkinID="label"></asp:Label>
                                                <span style="color: Red">*</span>
                                            </td>
                                            <td style="height: 29px">
                                                <asp:TextBox ID="txtBagNo" runat="server" SkinID="textbox" Width="200px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rFvtxtBagNo" runat="server" ControlToValidate="txtBagNo"
                                                    ErrorMessage="Please Enter the value">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trOPIP" runat="server" visible="false">
                                <td colspan="6">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:RadioButtonList ID="rblOPIP" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="OPD" Value="O" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="IPD" Value="I"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                
                                            </td>
                                            <td>
                                            <asp:RadioButtonList ID="rblIssueReceive" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Issue Register" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Receive Register" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr4" runat="server">
                                <td colspan="6">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlDep" Height="370px" Width="400px" BackColor="White"
                                                ScrollBars="None" Visible="false">
                                                <telerik:RadGrid ID="gvReporttype" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                                    AllowFilteringByColumn="true" runat="server" Width="99%" AutoGenerateColumns="False"
                                                    PageSize="11" EnableLinqExpressions="False" AllowPaging="false" CellSpacing="0"
                                                    OnPreRender="gvReporttype_PreRender">
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                                        Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                            AllowColumnResize="false" />
                                                    </ClientSettings>
                                                    <PagerStyle ShowPagerText="False" />
                                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">
                                                                No Record Found.</div>
                                                        </NoRecordsTemplate>
                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="StoreId" AllowFiltering="false" ShowFilterIcon="false"
                                                                AutoPostBackOnFilter="false" HeaderText="StoreId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="true" ShowFilterIcon="false"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                                HeaderText=" Name" FilterControlWidth="99%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDepartment" runat="server" />
                                                                    <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("Name")%>' />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkAllDepartment" OnCheckedChanged="chkAllDepartment_CheckedChanged"
                                                                        Font-Bold="true" runat="server" Text="All Select / Unselect " AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                        <EditFormSettings>
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                            </EditColumn>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                    <FilterMenu EnableImageSprites="False">
                                                    </FilterMenu>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" height="10px">
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowFor_Report" runat="server" Behaviors="Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
