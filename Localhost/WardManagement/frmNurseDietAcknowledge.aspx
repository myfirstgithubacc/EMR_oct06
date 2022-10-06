<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="frmNurseDietAcknowledge.aspx.cs" Inherits="WardManagement_frmNurseDietAcknowledge" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function setMaxLength(control) {
                    //get the isMaxLength attribute
                    var mLength = control.getAttribute ? parseInt(control.getAttribute("isMaxLength")) : ""

                    //was the attribute found and the length is more than the max then trim it
                    if (control.getAttribute && control.value.length > mLength) {
                        control.value = control.value.substring(0, mLength);
                        alert('Length exceeded');
                    }

                    //display the remaining characters
                    var modid = control.getAttribute("id") + "_remain";
                    if (document.getElementById(modid) != null) {
                        document.getElementById(modid).innerHTML = mLength - control.value.length + " Remains";
                    }
                }
            </script>
            <style>
                span#ctl00_ContentPlaceHolder1_lblTitle {
                    font-size: 16px;
                    font-weight: 700;
                    margin-top: 0px !Important;
                    padding-top: 0px;
                }

                input#ctl00_ContentPlaceHolder1_btnDietAck {
                    padding: 2px 8px;
                }

                input#ctl00_ContentPlaceHolder1_btnFilter {
                    padding: 1px 8px;
                }

                select#ctl00_ContentPlaceHolder1_ddlSlot {
                    height: 22px;
                    padding-right: 13px;
                }

                select#ctl00_ContentPlaceHolder1_ddlFor {
                    height: 22px;
                }

                select#ctl00_ContentPlaceHolder1_ddlSearchBy {
                    height: 22px;
                }

                textarea#ctl00_ContentPlaceHolder1_gvSoftDiet_ctl02_txtremark {
                    width: 100%;
                    
                }
            </style>
            <div class="container-fluid header_main form-group">
                <div class="col-md-2 text-left">
                    <h2 style="margin: 0!important; margin-top: -6px!important;">
                        <asp:Label ID="lblTitle" runat="server" Text="Diet Acknowledge" /></h2>
                </div>
                <div class="col-md-7 text-center">
                    <asp:Label ID="lblMessages" runat="server" Text="" />
                </div>
                <div class="col-md-3 text-right">
                    <asp:Button ID="btnDietAck" runat="server" Text="Acknowledge" CssClass="btn btn-primary" OnClick="btnDietAck_Click"
                        OnClientClick="this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row form-groupTop">
                    <div class="col-md-2">
                        <div class="col-md-4">
                            <asp:Label ID="lblFor" runat="server" Text="For" />
                        </div>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlFor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFor_SelectedIndexChanged">
                                <asp:ListItem Text="Un Acknowledged" Value="UA" Selected="True" />
                                <asp:ListItem Text="Acknowledged" Value="A" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container-fluid" style="margin-top: 6px">
                <div class="row form-groupTop">
                    <div class="col-md-2">
                        <div class="col-md-4">
                            <asp:Label ID="lblSlot" runat="server" Text="Slot" /><span style="color: Red">*</span>
                        </div>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlSlot" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="col-md-5">
                            <asp:Label ID="lblFNBDate" runat="server" Text="F & B Ack Date" /><span style="color: Red">*</span>
                        </div>
                        <div class="col-md-7">

                            <telerik:RadDateTimePicker ID="dtpFNBDate" runat="server"
                                Width="100%" DateInput-ReadOnly="false" TimePopupButton-Visible="false" DatePopupButton-Visible="false"
                                ShowPopupOnFocus="true"
                                DateInput-DateFormat="dd/MM/yyyy" />
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="col-md-3">
                            <asp:Label ID="lblSearchBy" runat="server" Text="Search By" />
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged">
                                <asp:ListItem Text="-- Select Search By --" Value="" />
                                <asp:ListItem Text="Encounter No" Value="E" Selected="True" />
                                <asp:ListItem Text="Registration No" Value="R" />
                                <asp:ListItem Text="Patient Name" Value="P" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 text-left">
                            <asp:TextBox ID="txtEncounterNo" runat="server" Visible="true" />
                            <asp:TextBox ID="txtRegistrationNo" runat="server" Visible="false" />
                            <asp:TextBox ID="txtPatientName" runat="server" Visible="false" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click"
                            OnClientClick="this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>
            <div class="container-fluid" style="margin-top: 8px">
                <div class="row form-group">
                    <div class="container-fluid">
                        <asp:Panel ID="pnlSoftDiet" runat="server" Height="400px" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="gvSoftDiet" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="40"
                                SkinID="gridviewOrderNew" Width="100%" OnPageIndexChanging="gvSoftDiet_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="40px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAck" runat="server" CssClass="checkbox" />
                                            <asp:HiddenField ID="hdnDietEmrrequestID" runat="server" Value='<%#Eval("DietEmrrequestID")%>' />
                                            <asp:HiddenField ID="hdnDietSlotId" runat="server" Value='<%#Eval("DietSlotId")%>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                            <asp:HiddenField ID="hdnDietAckCutOffTimeInMinutes" runat="server" Value='<%#Eval("DietAckCutOffTimeInMinutes")%>' />
                                            <asp:HiddenField ID="hdnSendToPatientCutDateTime" runat="server" Value='<%#Eval("SendToPatientCutDateTime")%>' />
                                            <asp:HiddenField ID="hdnPatientDietCardDetailsID" runat="server" Value='<%#Eval("PatientDietCardDetailsID")%>' />
                                            <asp:HiddenField ID="hdnPatientDietCardMainId" runat="server" Value='<%#Eval("PatientDietCardMainId")%>' />
                                            <asp:HiddenField ID="hdnStatusId" runat="server" Value='<%#Eval("StatusId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EncounterNo" HeaderText="Encounter No" HeaderStyle-Width="100px" />
                                    <asp:BoundField DataField="RegistrationNo" HeaderText="Registration No" HeaderStyle-Width="110px" />
                                    <asp:BoundField DataField="PName" HeaderText="Patient Name" HeaderStyle-Width="150px" />
                                    <asp:BoundField DataField="EncStatus" HeaderText="Encounter Status" HeaderStyle-Width="150px" />
                                    <asp:BoundField DataField="DietSlotName" HeaderText="Slot" HeaderStyle-Width="80px" />
                                    <asp:BoundField DataField="DietName" HeaderText="Diet Name" HeaderStyle-Width="170px" />
                                    <asp:BoundField DataField="Diagnosis" HeaderText="Diagnosis" HeaderStyle-Width="115px" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Requested Remarks" HeaderStyle-Width="140px" />
                                    <asp:BoundField DataField="EncodedBy" HeaderText="Requested By" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="EncodedDate" HeaderText="Requested Date Time" HeaderStyle-Width="140px" />
                                    <asp:BoundField DataField="SendToPatientDate" HeaderText="F & B Ack Date" HeaderStyle-Width="120px" />

                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Patient Feedback">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtremark" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="gvSoftDietA" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="40"
                                SkinID="gridviewOrderNew" Width="100%" OnPageIndexChanging="gvSoftDietA_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnDietEmrrequestID" runat="server" Value='<%#Eval("DietEmrrequestID")%>' />
                                            <asp:HiddenField ID="hdnDietSlotId" runat="server" Value='<%#Eval("DietSlotId")%>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                            <asp:HiddenField ID="hdnDietAckCutOffTimeInMinutes" runat="server" Value='<%#Eval("DietAckCutOffTimeInMinutes")%>' />
                                            <asp:HiddenField ID="hdnSendToPatientCutDateTime" runat="server" Value='<%#Eval("SendToPatientCutDateTime")%>' />
                                            <asp:HiddenField ID="hdnPatientDietCardDetailsID" runat="server" Value='<%#Eval("PatientDietCardDetailsID")%>' />
                                            <asp:HiddenField ID="hdnPatientDietCardMainId" runat="server" Value='<%#Eval("PatientDietCardMainId")%>' />
                                            <asp:HiddenField ID="hdnStatusId" runat="server" Value='<%#Eval("StatusId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EncounterNo" HeaderText="Encounter No" HeaderStyle-Width="95px" />
                                    <asp:BoundField DataField="RegistrationNo" HeaderText="Registration No" HeaderStyle-Width="110px" />
                                    <asp:BoundField DataField="PName" HeaderText="Patient Name" HeaderStyle-Width="150px" />
                                    <asp:BoundField DataField="EncStatus" HeaderText="Encounter Status" HeaderStyle-Width="150px" />
                                    <asp:BoundField DataField="DietSlotName" HeaderText="Slot" HeaderStyle-Width="80px" />
                                    <asp:BoundField DataField="DietName" HeaderText="Diet Name" HeaderStyle-Width="170px" />
                                    <asp:BoundField DataField="Diagnosis" HeaderText="Diagnosis" HeaderStyle-Width="115px" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Requested Remarks" HeaderStyle-Width="140px" />
                                    <asp:BoundField DataField="EncodedBy" HeaderText="Requested By" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="EncodedDate" HeaderText="Requested Date Time" HeaderStyle-Width="140px" />
                                    <asp:BoundField DataField="SendToPatientDate" HeaderText="F & B Ack Date" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="DietAckBy" HeaderText="Acknowledged By" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="AcknowledgeRemarks" HeaderText="Acknowledged Remarks" HeaderStyle-Width="140px" />
                                    <asp:BoundField DataField="DietAckEncodedDate" HeaderText="Acknowledged Date Time" HeaderStyle-Width="140px" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <asp:Panel ID="pnlRemarks" runat="server" Width="100%">
                <div class="container-fluid">
                    <div class="row form-groupTop">
                        <div class="col-md-2">
                            <asp:Label ID="lblAckRemark" runat="server" Text="Acknowledge Remarks" /><span style="color: Red">*</span>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtAckRemark" runat="server" TextMode="MultiLine" onKeyUp="setMaxLength(this)" isMaxLength="1000"
                                Style="max-height: 70px; min-height: 70px; max-width: 700px; min-width: 700px;" />
                            <span id="<%=txtAckRemark.ClientID %>_remain"><%= 1000 - txtAckRemark.Text.Length %> Remains</span>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

