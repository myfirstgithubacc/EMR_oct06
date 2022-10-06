<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ReferralRequestHistory.aspx.cs" Inherits="WardManagement_ReferralRequestHistory"
    Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="radblock" runat="server">
        <link href="../Include/css/open-sans.css" rel="stylesheet" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../Include/css/mainNew.css" rel="stylesheet" />
        <style type="text/css">
            .blink {
                text-decoration: blink;
            }

            .blinkNone {
                text-decoration: none;
            }

            .findPatientInput-Mobile {
                width: 100% !important;
                padding: 2px 5px !important;
            }

            tr.clsGridheaderorderNew th {
                color: #fff !important;
                background: #337ab7 !important;
                padding: 6px 10px !important;
               white-space:nowrap!important;
            }
            table#ctl00_ContentPlaceHolder1_gvPatientDurgStatus{
                 border: 1px solid #ccc !important;
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" style="overflow: hidden;">
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="row">
                    <div class="col-md-3">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="Cross consultation (Referral) List" />
                        </h2>
                    </div>
                    <div class="col-md-7 text-center">
                        <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
                    </div>
                    <div class="col-md-2 text-right">
                        <%--<asp:Button ID="BtnAcknowledge" runat="server" Text="Acknowledge" CssClass="btn btn-primary" OnClick="BtnAcknowledge_Click" />--%>
                        <%-- <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print Prescription" OnClick="btnPrint_Click" CausesValidation="false" />
                    <asp:Button ID="btnPrinLable" runat="server" SkinID="Button" Text="Print Label" OnClick="btnPrinLable_Click" CausesValidation="false" />--%>
                    </div>
                </div>
            </div>
            <div class="container-fluid" style="background-color: #fff;">
                <div class="row form-groupTop">
                    <div class="col-12">
                        <div class="row">
                            <div class="col-lg-2 col-md-3 col-6">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label4" runat="server" Text="Status&nbsp" />
                                    </div>
                                    <div class="col-md-9">
                                        <telerik:RadComboBox ID="ddlStatus" runat="server" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-5 col-6">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                                    </div>
                                    <div class="col-md-5 col-6">
                                        <telerik:RadComboBox ID="ddlName" runat="server" Width="100%"
                                            AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-4 col-6 pl-0">
                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnFilter">
                                            <asp:TextBox ID="txtSearch" Visible="false" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                                Width="100%" />
                                            <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" runat="server" Text=""
                                                MaxLength="13" onkeyup="return validateMaxLength();" Width="100%" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="ltrltodoctor" runat="server" Text="Resident" />
                                    </div>
                                    <div class="col-md-9">
                                        <telerik:RadComboBox ID="ddlRequestToDoctor" runat="server" SkinID="DropDown" TabIndex="3" DropDownWidth="250px"
                                            EnableLoadOnDemand="true" Filter="Contains" Width="100%" Height="300px" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-6">
                                <div class="row">
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-md-4 ">
                                                <span id="Span1" runat="server">&nbsp;From&nbsp;</span>
                                            </div>
                                            <div class="col-md-8 pr-md-0">
                                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="100%" AutoPostBack="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-md-3 ">
                                                <span id="spTo" runat="server">&nbsp;To&nbsp;</span>
                                            </div>
                                            <div class="col-md-9 ">
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-8 col-12 text-right">
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnClearFilter" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="container-fluid" style="background-color: #fff;">
                <div class="row form-group margin_Top">
                    <div class="col-12">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvPatientDurgStatus" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" Height="350px" Width="100%" ScrollBars="Auto"
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
                                                    <asp:HiddenField ID="hdnReferredByUserId" runat="server" Value='<%#Eval("ReferredByUserId")%>' />
                                                    <asp:HiddenField ID="hdnConcludeReferral" runat="server" Value='<%#Eval("ConcludeReferral")%>' />
                                                    <asp:HiddenField ID="hdnConcludeDate" runat="server" Value='<%#Eval("ConcludeDate")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Request Date' HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestDate" runat="server" Text='<%#Eval("RequestDate")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Patient Name' HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Resident' HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestToDoctorName" runat="server" Text='<%#Eval("RequestToDoctorName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Nurse Remarks'>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestRemarks" runat="server" Text='<%#Eval("RequestRemarks")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Nurse' HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestedByName" runat="server" Text='<%#Eval("RequestedByName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Referred Doctor' HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReferToDoctor" runat="server" Text='<%#Eval("ReferToDoctor")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Referral Remarks' HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReferralRemarks" runat="server" Text='<%#Eval("ReferralRemarks")%>' ToolTip='<%#Eval("ReferralRemarks")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Status' HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestStatusName" runat="server" Text='<%#Eval("RequestStatusName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Complete' HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnComplete" runat="server" Text="Complete" CommandName="COM" />
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
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
