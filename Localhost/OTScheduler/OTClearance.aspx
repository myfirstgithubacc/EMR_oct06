<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTClearance.aspx.cs" Inherits="OTScheduler_OTClearance" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="radblock" runat="server">
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../Include/EMRStyle.css" rel="stylesheet" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" />
        <script language="javascript" type="text/javascript">
            function returnToParent() {
                var oArg = new Object();
                oArg.IndentId = $get('<%=hdnSelectedIndentId.ClientID%>').value;
                oArg.IndentNo = $get('<%=hdnSelectedIndentNo.ClientID%>').value;
                oArg.RegistrationId = $get('<%=hdnSelectedRegistrationId.ClientID%>').value;
                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function Blink() {
                if (document.getElementById("lnkAllergyDetails"))
                    //Here you have to mention control name instead of blinkme
                {
                    var d = document.getElementById("lnkAllergyDetails");
                    //Here you have to mention control name instead of blinkme
                    d.style.color = (d.style.color == 'red' ? 'white' : 'red');

                    setTimeout('Blink()', 1000);
                }
            }
        </script>
        <style type="text/css">
            .blink {
                text-decoration: blink;
            }

            .blinkNone {
                text-decoration: none;
            }
        </style>
    </telerik:RadCodeBlock>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-2 col-sm-2 col-xs-12">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="OT Clearance" /></h2>
                </div>
                <div class="col-md-7 col-sm-7 col-xs-12 text-center">
                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" /></div>
                <div class="col-md-3 col-sm-3 col-xs-12 text-right">
                    <asp:Button ID="BtnAcknowledge" runat="server" Text="OT Clearance" CssClass="btn btn-primary" OnClick="BtnAcknowledge_Click" />
                    <%-- <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print Prescription" OnClick="btnPrint_Click" CausesValidation="false" />
                    <asp:Button ID="btnPrinLable" runat="server" SkinID="Button" Text="Print Label" OnClick="btnPrinLable_Click" CausesValidation="false" />--%>
                    <asp:Button ID="BtnUnAcknowledge" runat="server" CssClass="btn btn-default" Text="Cancel Clearance" OnClick="BtnUnAcknowledge_Click" />
                </div>
            </div>


            
                <div class="row">
                  <div class="col-md-2 col-sm-2 col-xs-12">
                      <div class="row p-t-b-5">
                          <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap"><asp:Label ID="Label4" runat="server" Text="Status&nbsp" /></div>
                          <div class="col-md-9 col-sm-9 col-xs-8">
                              <telerik:RadComboBox ID="ddlAcknowledge" runat="server" AutoPostBack="true" CssClass="drapDrowHeight" Width="100%" OnSelectedIndexChanged="ddlAcknowledge_SelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Acknowleged" Value="A" />
                                                <telerik:RadComboBoxItem Text="UnAcknowleged" Value="U" Selected="true" />
                                            </Items>
                                        </telerik:RadComboBox>
                          </div>
                      </div>
                  </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                      <div class="row p-t-b-5">
                          <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                              <telerik:RadComboBox ID="ddlSearchOn" runat="server" CssClass="drapDrowHeight" Width="100%">
                                            <Items>
                                                <%--<telerik:RadComboBoxItem Text="All" Value="ALL"   />--%>
                                                <telerik:RadComboBoxItem Text="UHID" Value="R" />
                                                <telerik:RadComboBoxItem Text="IP No" Value="I" Selected="true" />
                                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                          </div>
                          <div class="col-md-8 col-sm-8 col-xs-8">
                              <asp:TextBox ID="txtSearchOn" runat="server" MaxLength="50" CssClass="drapDrowHeight" Width="100%"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                    <div class="col-md-5 col-sm-5 col-xs-12">
                        <asp:Panel ID="Panel3" CssClass="row" runat="server" DefaultButton="btnFilter">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                      <div class="row p-t-b-5">
                          <div class="col-md-5 col-sm-5 col-xs-4 text-nowrap">
                              <asp:Label ID="Label9" runat="server" Text="Booking Date From" />
                          </div>
                          <div class="col-md-7 col-sm-7 col-xs-8">
                              <telerik:RadDatePicker ID="dtpOTDateFrom" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" MinDate="01/01/1900">
                                                <DateInput ID="DateInput1" runat="server" AutoPostBack="True" DisplayDateFormat="dd/MM/yyyy"
                                                    DateFormat="MM/dd/yyyy" ForeColor="Black">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                          </div>
                      </div>
                  </div>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                      <div class="row p-t-b-5">
                          <div class="col-md-5 col-sm-5 col-xs-4 text-nowrap">
                              <asp:Label ID="Label1" runat="server" Text="Booking Date To" />
                          </div>
                          <div class="col-md-7 col-sm-7 col-xs-8">
                              <telerik:RadDatePicker ID="dtpOTDateTo" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" MinDate="01/01/1900">
                                                <DateInput ID="DateInput2" runat="server" AutoPostBack="True" DisplayDateFormat="dd/MM/yyyy"
                                                    DateFormat="MM/dd/yyyy" ForeColor="Black">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                          </div>
                      </div>
                  </div>
                    </asp:Panel>
                        </div>
                    <div class="col-md-2 col-sm-2 col-xs-12 p-t-b-5">
                        <%-- <asp:UpdatePanel ID="btnupd1" runat="server"><ContentTemplate >--%>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" />
                        <asp:Button ID="btnClearFilter" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                        <asp:HiddenField ID="hdnMRegistrationId" runat="server" />
                        <asp:HiddenField ID="hdnMEncounterId" runat="server" />
                        <%-- <asp:Button ID="btnFetchforInventory" runat="server" SkinID="Button" Text="Fetch for Inventory" OnClick="btnFetchforInventory_Click" />--%>
                        <%--</ContentTemplate></asp:UpdatePanel> --%>
                    </div>
                </div>

                
                
                
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPatientDurgStatus" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Height="380px" Width="100%" ScrollBars="Auto"
                                BorderWidth="1px" BorderColor="LightBlue">
                                <asp:GridView ID="gvPatientDurgStatus" CssClass="table table-condensed" runat="server" AutoGenerateColumns="False"
                                    Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="true"
                                    PageSize="15" OnPageIndexChanging="gvPatientDurgStatus_OnPageIndexChanging"
                                    OnSelectedIndexChanged="gvPatientDurgStatus_SelectedIndexChanged"
                                    OnRowDataBound="gvPatientDurgStatus_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" OnClick="lnkSelect_OnClick"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText='Facility' HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFacilityShortName" runat="server" Width="100%" Text='<%#Eval("FacilityShortName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Ward Name.' HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTheatreName" runat="server" Width="100%" Text='<%#Eval("TheatreName")%>' />
                                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                <asp:HiddenField ID="lblRegistrationNo" runat="server" Value='<%#Eval("RegistrationNo")%>' />
                                                <asp:HiddenField ID="lblIPNo" runat="server" Value='<%#Eval("EncounterNo")%>' />
                                                <asp:HiddenField ID="lblPatientName" runat="server" Value='<%#Eval("PatientName")%>' />
                                                <asp:HiddenField ID="lblDoneBy" runat="server" Value='<%#Eval("Users")%>' />
                                                <asp:HiddenField ID="lblBookingDate" runat="server" Value='<%#Eval("bookingdate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Theatre Name' HeaderStyle-Width="0px">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                             <HeaderTemplate>
                                             <asp:Label ID="lbl1" runat="server" Text='<%#Session["RegistrationLabelName"]%>'/>
                                             </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationNo" runat="server" Width="100%" Text='<%#Eval("RegistrationNo")%>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Encounter No' HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" Width="100%" Text='<%#Eval("EncounterNo")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Patient Name' HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Width="100%" Text='<%#Eval("PatientName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='OT Name' HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTheatreName" runat="server" Width="100%" Text='<%#Eval("TheatreName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Admission Date' HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdmissionDate" runat="server" Width="100%" Text='<%#Eval("AdmissionDate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Acknowleged By' HeaderStyle-Width="280px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTBookingAckBy" runat="server" Width="100%" Text='<%#Eval("OTBookingAckBy")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Acknowleged Date' HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTBookingAckDate" runat="server" Width="100%" Text='<%#Eval("OTBookingAckDate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                    </div>

                <div class="row form-group">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPatientDurgStatus" />
                            <asp:AsyncPostBackTrigger ControlID="BtnAcknowledge" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" Height="190px" Width="100%" ScrollBars="Auto"
                                BorderWidth="1px" BorderColor="LightBlue">
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>


            <%--  <asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="0">
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="Bisque" SkinID="label" Width="22px" Height="14px" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label21" runat="server" SkinID="label" Text="Ac&nbsp;Dispensed" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label22" runat="server" BorderWidth="1px" BackColor="LightSteelBlue" SkinID="label" Width="22px" Height="14px" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label23" runat="server" SkinID="label" Text="Dispensed" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label7" runat="server" BorderWidth="1px" BackColor="PaleTurquoise" SkinID="label" Width="22px" Height="14px" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label8" runat="server" SkinID="label" Text="Hospital Formulary" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label9" runat="server" BorderWidth="1px" BackColor="DarkOliveGreen" SkinID="label" Width="22px" Height="14px" /></asp:TableCell>
                    <asp:TableCell><asp:Label ID="Label10" runat="server" SkinID="label" Text="Billed" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>--%>

            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnSelectedIndentId" runat="server" />
                        <asp:HiddenField ID="hdnSelectedIndentNo" runat="server" />
                        <asp:HiddenField ID="hdnSelectedRegistrationId" runat="server" />
                        <asp:HiddenField ID="hndSelectedIsInsuranceCompany" runat="server" />
                        <asp:HiddenField ID="hdnItemId" runat="server" />
                        <asp:HiddenField ID="hndItemName" runat="server" />
                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <div id="divPrescriptionRemarks" runat="server" visible="false" style="width: 350px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: Silver; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 130px; left: 340px; top: 230px">
                            <table width="100%" border="0" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label13" runat="server" Font-Bold="true" Text="Prescription Remarks" /></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:TextBox ID="txtPrescriptionRemarks" ReadOnly="true" runat="server" SkinID="textbox" MaxLength="1000" TextMode="MultiLine" Style="min-height: 75px; max-height: 75px; min-width: 340px; max-width: 340px;" Width="340px" Height="75px" onkeyup="return MaxLenTxt(this, 250);" /></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
