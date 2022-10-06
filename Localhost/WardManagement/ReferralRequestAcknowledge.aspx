<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ReferralRequestAcknowledge.aspx.cs" Inherits="WardManagement_ReferralRequestAcknowledge"
    Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="radblock" runat="server">
        <link href="../Include/css/open-sans.css" rel="stylesheet" />
        <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../Include/css/mainNew.css" rel="stylesheet" />
        <style type="text/css">
            .blink {
                text-decoration: blink;
            }

            .blinkNone {
                text-decoration: none;
            }
        </style>
        <script language="javascript" type="text/javascript">
            function OnClearClientClose(oWnd) {
                $get('<%=btnFilter.ClientID%>').click();
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


    </telerik:RadCodeBlock>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="Cross consultation (Referral) Acknowledge" /></h2>
                </div>
                <div class="col-md-8 text-center">
                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
                </div>
                <div class="col-md-1 text-right">
                    <%--<asp:Button ID="BtnAcknowledge" runat="server" Text="Acknowledge" CssClass="btn btn-primary" OnClick="BtnAcknowledge_Click" />--%>
                    <%-- <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print Prescription" OnClick="btnPrint_Click" CausesValidation="false" />
                    <asp:Button ID="btnPrinLable" runat="server" SkinID="Button" Text="Print Label" OnClick="btnPrinLable_Click" CausesValidation="false" />--%>
                </div>
            </div>

            <div class="container-fluid" style="background-color: #fff;">
                <div class="row form-groupTop">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Text="Status&nbsp" />
                        <telerik:RadComboBox ID="ddlStatus" runat="server" Width="150px" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                        <telerik:RadComboBox ID="ddlName" runat="server" Width="120px"
                            AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnFilter">
                            <asp:TextBox ID="txtSearch" Visible="false" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                Width="120px" />
                            <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" runat="server" Text=""
                                MaxLength="13" onkeyup="return validateMaxLength();" Width="120px" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                        </asp:Panel>
                    </div>
                    <div class="col-md-3">
                        <span id="Span1" runat="server">&nbsp;From&nbsp;</span>
                        <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="70px" AutoPostBack="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                        <span id="spTo" runat="server">&nbsp;To&nbsp;</span>
                        <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="70px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" />
                        <asp:Button ID="btnClearFilter" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                    </div>
                </div>
            </div>
            <div class="container-fluid" style="background-color: #fff;">
                <div class="row form-group margin_Top">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPatientDurgStatus" />
                        </Triggers>

                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Height="260px" Width="100%" ScrollBars="Auto"
                                BorderWidth="0px" BorderColor="LightBlue">
                                <asp:GridView ID="gvPatientDurgStatus" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="False"
                                    Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="true"
                                    PageSize="20" OnPageIndexChanging="gvPatientDurgStatus_OnPageIndexChanging" 
                                    OnRowDataBound="gvPatientDurgStatus_RowDataBound" OnRowCommand="gvPatientDurgStatus_OnRowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText='Type' HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestTypeName" runat="server" Text='<%#Eval("RequestTypeName")%>' />
                                                <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Eval("RequestId")%>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                <asp:HiddenField ID="hdnRequestType" runat="server" Value='<%#Eval("RequestType")%>' />
                                                <asp:HiddenField ID="hdnRequestToDoctorId" runat="server" Value='<%#Eval("RequestToDoctorId")%>' />
                                                <asp:HiddenField ID="hdnRequestStatusId" runat="server" Value='<%#Eval("RequestStatusId")%>' />
                                                <asp:HiddenField ID="hdnRequestStatusCode" runat="server" Value='<%#Eval("RequestStatusCode")%>' />
                                                <asp:HiddenField ID="hdnConcludeReferral" runat="server" Value='<%#Eval("ConcludeReferral")%>' />
                                                <asp:HiddenField ID="hdnConcludeDate" runat="server" Value='<%#Eval("ConcludeDate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Request Date' HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestDate" runat="server" Text='<%#Eval("RequestDate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Patient Name' HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Remarks'>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestRemarks" runat="server" Text='<%#Eval("RequestRemarks")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Nurse' HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestedByName" runat="server" Text='<%#Eval("RequestedByName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Status' HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestStatusName" runat="server" Text='<%#Eval("RequestStatusName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Acknowledge' HeaderStyle-Width="80px" ItemStyle-Width="80px" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnAcknowledge" runat="server" Text="Acknowledge" CommandName="ACK" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Referral Slip' HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnReferralSlip" runat="server" Text="Referral Slip" CommandName="RS" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
