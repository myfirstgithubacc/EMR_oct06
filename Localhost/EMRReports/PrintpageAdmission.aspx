<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PrintpageAdmission.aspx.cs" Inherits="EMRReports_PrintpageAdmission" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
         <link href="../Include/EMRStyle.css" rel="stylesheet" />

 <%--   <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />

    <link href="../Include/EMRStyle.css" rel="stylesheet" />
    <link href="~/Include/css/font-awesome.min.css" rel="stylesheet" />--%>
        <style type="text/css">
            .PD-TabRadioNew01 input {
                margin: 4px 4px 0 0px !important;
            }

            .PD-TabRadioNew01 label {
                font-weight: bold !important;
                margin: 1px 4px 0 0px !important;
            }
        </style>
        <script type="text/javascript">
            function SearchPatientOnClientClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {

                    var EncounterNo = arg.EncounterNo;
                    var RegistrationNo = arg.RegistrationNo
                    var RegistrationId = arg.RegistrationId
                    var EcountId = arg.EncounterId
                    $get('<%=txtIpno.ClientID%>').value = EncounterNo;
                    $get('<%=hdnRegistrationNo.ClientID %>').value = RegistrationNo;
                    $get('<%=hdnRegistrationId.ClientID %>').value = RegistrationId;
                    $get('<%=hdnInvEncounterId.ClientID %>').value = EcountId;
                    if (EncounterNo == ' ' || EncounterNo == '' || EncounterNo == null) {

                        $get('<%=txtUHID.ClientID%>').value = RegistrationNo;
                    }
                }
               <%-- $get('<%=btnfind.ClientID%>').click();--%>
            }

            function OnClientCloseSearch(oWnd, args) {
                var org = args.get_argument();
                if (org) {
                    document.getElementById('<%=txtInvoice.ClientID%>').value = org.DocNo;
                }
                $get('<%=btnfindinvoice.ClientID%>').click();

            }
        </script>
    </telerik:RadCodeBlock>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnserviceId" runat="server" />
            <asp:HiddenField ID="hdnServiceAmount" runat="server" />
            <asp:HiddenField ID="hdnDoctorAmount" runat="server" />
            <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdnAmountPayableByPatient" runat="server" />
            <asp:HiddenField ID="hdnAmountPayableByPayer" runat="server" />
            <asp:HiddenField ID="hdnServiceDiscountPercentage" runat="server" />
            <asp:HiddenField ID="hdnDoctorDiscountPercentage" runat="server" />
            <div class="container-fluid header_main" style="background:#C1E5EF !important;margin-bottom:10px;padding:6px 0px !important; ">
                <div class="col-md-3" id="tdHeader" runat="server">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" /></h2>
                </div>
                <div class="col-md-6 text-center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-3 text-right"></div>
            </div>


            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-offset-3 col-md-6" style="background:#C1E5EF;">
                        <div class="col-md-12" id="tradmfrm" runat="server">
                            <div class="row">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnshow" ScrollBars="None">
                                <div class="col-md-8 col-sm-8 col-xs-12">
                                    <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-3 col-sm-3 col-xs-4 no-p-r">
                                             <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text="IP No." OnClick="lbtnSearchPatient_OnClick"
                                        CausesValidation="false" Font-Underline="false" ToolTip="Click to search patient"
                                        Font-Bold="true"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-xs-8">
                                            <asp:TextBox ID="txtIpno" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:LinkButton ID="lnkSearchPatientbyUHID" runat="server" Text="UHID No." OnClick="lnkSearchPatientbyUHID_Click"
                                        CausesValidation="false" Font-Underline="false" ToolTip="Click to search patient"
                                        Font-Bold="true"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                             <asp:TextBox ID="txtUHID" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                        </div>
                                    <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <asp:Label ID="lblfrmdate" runat="server" Text="From Date" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadDatePicker ID="dtFromDate" runat="server" DateInput-DateFormat="dd/MM/yyyy" Visible="false" Width="100%"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <asp:Label ID="lblToDate" runat="server" Text="To Date" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadDatePicker ID="dtTodate" runat="server" DateInput-DateFormat="dd/MM/yyyy" Visible="false" Width="100%"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                        </div>
                                    
                                    </div>
                                <div class="col-md-4 col-sm-4 col-xs-12 p-t-b-5 text-left">
                                    <asp:Button ID="btnshow" runat="server" CssClass="btn btn-primary" Text="Print Preview" OnClick="btnShow_Click" />
                                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                                    <asp:HiddenField ID="hdnInvEncounterId" runat="server" />
                                    <asp:HiddenField ID="hdnBillId" runat="server" />
                                    <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                                    
                                </div>
                                     </asp:Panel>
                            </div>
                            <div class="row">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnshow" ScrollBars="None" Visible="false">
                                <div class="col-md-4 col-sm-4">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4">
                                             <asp:LinkButton ID="lbtnSearchPatientvoice" runat="server" Text="Invoice" OnClick="lbtnSearchPatientvoice_OnClick"
                                        CausesValidation="false" Font-Underline="false" ToolTip="Click to search patient"
                                        Font-Bold="true"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-8 col-sm-8">
                                            <asp:TextBox ID="txtInvoice" runat="server" MaxLength="15"></asp:TextBox>
                                    <asp:HiddenField ID="hdnInvoiceId" runat="server" />

                                    <asp:Button ID="btnfindinvoice" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfindinvoice_Click" />
                                        </div>
                                    </div>
                                </div>
                                     </asp:Panel>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 box-col-checkbox" id="rdoList" runat="server">
                                     <asp:RadioButtonList ID="rdolabelform" runat="server" RepeatDirection="Vertical" CellPadding="1">
                                    <%--<asp:ListItem Value="F" Text="Admission&nbsp;Form" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="L" Text="Admission&nbsp;Label"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Discharge&nbsp;Notification"></asp:ListItem>
                                    <asp:ListItem Value="B" Text="Bar&nbsp;Code"></asp:ListItem>
                                    <asp:ListItem Value="V" Text="Valuable&nbsp;Handover"></asp:ListItem>
                                    <asp:ListItem Value="R" Text="Room&nbsp;Retain"></asp:ListItem>
                                    <asp:ListItem Value="AR" Text="Pre-Admission&nbsp;Cum&nbsp;ConsentForm">  </asp:ListItem>
                                    <asp:ListItem Value="PD" Text="Patient&nbsp;Details"> </asp:ListItem>
                                    <asp:ListItem Value="FS" Text="Form60"> </asp:ListItem>
                                    <asp:ListItem Value="AD" Text="Advance Tagged"> </asp:ListItem>
                                    <asp:ListItem Value="OT" Text="OP Tagged Detail"> </asp:ListItem>
                                    <asp:ListItem Value="OS" Text="OP Tagged Summry"> </asp:ListItem>
                                    <asp:ListItem Value="PS" Text="Patient Satisfication"> </asp:ListItem>
                                    <asp:ListItem Value="EF" Text="Extension Request Form"> </asp:ListItem>--%>
                                </asp:RadioButtonList>
                                &nbsp;&nbsp;
                               
                                <asp:RadioButtonList ID="rdolabelformipbilldetail" runat="server" RepeatDirection="Horizontal" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="rdolabelformipbilldetail_SelectedIndexChanged">
                                    <asp:ListItem Value="N" Text="IP No." Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="I" Text="Invoice"></asp:ListItem>
                                    <%--<asp:ListItem Value="C" Text="Check List For Bills"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                                </div>

                                <div class="col-md-6 col-sm-6 box-col-checkbox">
                                    <asp:RadioButtonList ID="rdoIpbillType" runat="server" RepeatDirection="Vertical" Visible="false" OnSelectedIndexChanged="rdoIpbillType_SelectedIndexChanged1" AutoPostBack="true">
                                    <asp:ListItem Value="S" Text="Summary" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Detail"></asp:ListItem>
                                    <asp:ListItem Value="A" Text="Attendant Pass"></asp:ListItem>
                                    <%--<asp:ListItem Value="R" Text="Referral Tagging"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                                <asp:CheckBox ID="chkIsChargeable" Text="IsChargeable" OnCheckedChanged="chkIsChargeable_CheckedChanged" Visible="false" AutoPostBack="true" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trdate" runat="server">
                                <div class="col-md-4 col-sm-4">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4" id="lbltdfrmdate" runat="server">
                                            <asp:Label ID="Label2" runat="server" Text="From :"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8" id="dttdfrmdate" runat="server">
                                            <telerik:RadDateTimePicker ID="dtpfromdate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" AutoPostBackControl="Calendar" OnSelectedDateChanged="dtpTodate_OnSelectedDateChanged" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDateTimePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4">
                                            <asp:Label ID="Label3" runat="server" Text="To"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8">
                                             <telerik:RadDateTimePicker ID="dtpTodate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" AutoPostBackControl="Calendar" OnSelectedDateChanged="dtpTodate_OnSelectedDateChanged" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDateTimePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:Button ID="btnPrintreport" runat="server" ToolTip="Click to Print Report" CausesValidation="false" OnClick="btnPrintreport_OnClick" CssClass="btn btn-primary" Text="Print Report" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4" id="trl" runat="server" visible="false">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <telerik:RadComboBox ID="ddlTransferReport" CssClass="drapDrowHeight" Width="100%" runat="server">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="A" Text="Doctor/Bed Transfer" Selected="true" />
                                                    <telerik:RadComboBoxItem Value="D" Text="Doctor Transfer" />
                                                    <telerik:RadComboBoxItem Value="P" Text="Bed Transfer" />
                                                    <telerik:RadComboBoxItem Value="T" Text="Bed Transfer Request" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-8" id="trddl" runat="server">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-3" id="tdlblrtype" runat="server">
                                            <asp:Label ID="Lab1" runat="server" Text="Report Type"></asp:Label>
                                        </div>
                                        <div class="col-md-4" id="tdddlrtype" runat="server">
                                            <telerik:RadComboBox ID="ddlReportType" CssClass="drapDrowHeight" Width="100%" DropDownWidth="200px" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Date" Text="Date Wise" Selected="true" />
                                                    <telerik:RadComboBoxItem Value="Doctor" Text="Doctor Wise" />
                                                    <telerik:RadComboBoxItem Value="Doctorteam" Text="Doctor Team Wise" />
                                                    <telerik:RadComboBoxItem Value="Speciality" Text="Speciality Wise" />
                                                    <telerik:RadComboBoxItem Value="Sponsor" Text="Sponsor Wise" />
                                                    <telerik:RadComboBoxItem Value="Bed" Text="Bed Category Wise" />
                                                    <telerik:RadComboBoxItem Value="Ward" Text="Ward Wise" />
                                                    <telerik:RadComboBoxItem Value="Country" Text="Country Wise" />
                                                    <telerik:RadComboBoxItem Value="Source" Text="Source" />
                                                    <telerik:RadComboBoxItem Value="Age" Text="Age Wise" />
                                                    <telerik:RadComboBoxItem Value="ReferByInternal" Text="Refer By Internal" />
                                                    <telerik:RadComboBoxItem Value="ReferByExternal" Text="Refer By External" />
                                                    <telerik:RadComboBoxItem Value="CompanyType" Text="Company Type" />
                                                    <telerik:RadComboBoxItem Value="DischargeTypeWise" Text="Discharge Type Wise" />
                                                    <%-- added by bhakti--%>
                                                    <telerik:RadComboBoxItem Value="WardStation" Text="Ward Station Wise" Visible="false" />
                                                    <%--<telerik:RadComboBoxItem Value="City" Text="City Wise" />
                                                    <telerik:RadComboBoxItem Value="Area" Text="Area Wise" />--%>
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-5 margin_Top">
                                            <asp:CheckBox ID="ChkSummary" runat="server" Text="Summary" SkinID="checkbox" Visible="false" />
                                            <asp:CheckBox ID="chkExport" runat="server" Text="Export" />
                                            <asp:CheckBox ID="chkIsAdmitted" runat="server" Text="Admitted Only" SkinID="checkbox" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                      

                            <div class="row">
                                <div class="col-md-5">
                                    <asp:LinkButton ID="lnkeditAgeRange" runat="server" Text="Edit Age Group" OnClick="lnkeditAgeRange_OnClick" Visible="false" />
                                </div>
                                <div class="col-md-7" id="trloction" runat="server">
                                    <div class="row">
                                        <div class="col-md-4 label2">
                                            <asp:Label ID="lbllocation" runat="server" Text="Location"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <telerik:RadComboBox ID="ddlEntrySite" CssClass="drapDrowHeight" Width="100%" runat="server"></telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                       
                   
                        <div class="row" id="trgrid" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlDep" Height="400px" Width="100%" ScrollBars="Auto">
                                        <asp:Label ID="Label4" runat="server" Text="" Font-Size="11pt" Font-Bold="True"></asp:Label>&nbsp;
                                       
                                        <center>
                                            <telerik:RadGrid ID="gvReporttype" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                                AllowFilteringByColumn="true" runat="server" Width="900px" AutoGenerateColumns="False"
                                                PageSize="12" EnableLinqExpressions="False" AllowPaging="false" CellSpacing="0"
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
                                                        <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
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
                                        </center>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="row">
                            <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" Skin="Office2007" runat="server" Behaviors="Close,Move" InitialBehaviors="Maximize">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </div>

                    <div id="Div2" visible="false" runat="server" style="width: 420px; z-index: 200; border: 4px solid #60AFC3; background-color: #fff; position: fixed; bottom: 44%; height: 85px; left: 38%;">
                        <table cellpadding="2" cellspacing="2" border="0" width="99%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Size="12pt" Font-Bold="true" Font-Names="Arial"
                                        Text="Re Print Reason"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #00CCFF"></td>
                            </tr>
                            <tr>
                                <td align="center" valign="middle">
                                    <asp:HiddenField ID="hdnReasonForPrinting" Value="" runat="server" />
                                    <asp:HiddenField ID="hdnDocumentType" Value="" runat="server" />
                                    <asp:TextBox ID="txtReasonForRePrint" TextMode="MultiLine" runat="server" />
                                    <asp:Button ID="btnPrintDocument" CssClass="btn btn-primary margin_Top01" runat="server" Text="Print Label"
                                        ToolTip="Print Preview" OnClick="btnPrintDocument_Click" />
                                    <asp:Button ID="btnCloseDivForReason" CssClass="btn btn-primary margin_Top01" runat="server" Text="Close"
                                        ToolTip="Close" OnClick="btnCloseDivForReason_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
                    </div>
                </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
