<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PatientTransferWardtoOTList.aspx.cs" Inherits="WardManagement_PatientTransferWardtoOTList" Title="" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <link href="../Include/EMRStyle.css" rel="stylesheet" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-2 col-sm-3 col-xs-12"><asp:Label ID="Label2" runat="server" Text="Patient Transfer Ward to OT" /></div>
                <div class="col-md-7 col-sm-6 col-xs-12 text-center"> <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                <div class="col-md-3 col-sm-3 col-xs-12 text-right">
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" ToolTip="Filter" Text="Filter" OnClick="btnSearch_OnClick" />
                    <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" ToolTip="Clear Filter" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" ToolTip="Save Approval" Text="Save" OnClick="btnSave_OnClick" Visible="false" />
                </div>
            </div>

           
                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 label2"> <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" /></div>
                            <div class="col-md-9">
                                <asp:Panel ID="Panel4" runat="server" CssClass="row" DefaultButton="btnSearch">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <telerik:RadComboBox ID="ddlName" CssClass="findPatientSelect-Mobile" runat="server"
                                        AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                            <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtSearch" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                        Visible="false" />
                                    <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" runat="server" Text=""
                                        MaxLength="10" Visible="false" onkeyup="return validateMaxLength();" />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                    </div>
                                    
                                    
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                  
                    <div class="col-md-8 col-sm-8 col-xs-12 box-col-checkbox p-t-b-5">
                                <asp:RadioButtonList ID="rdoTransferStatus" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdoTransferStatus_OnSelectedIndexChanged">
                                    <asp:ListItem Text="Patient From Ward&nbsp;&nbsp;" Value="0" Selected="True" />
                                    <asp:ListItem Text="Pre-Operative Patient List&nbsp;&nbsp;" Value="1" />
                                    <asp:ListItem Text="Patient In OT&nbsp;&nbsp;" Value="2" />
                                    <asp:ListItem Text="Post-Operative Patient List&nbsp;&nbsp;" Value="3" />
                                </asp:RadioButtonList>
                            </div>
                </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <asp:Panel ID="pnlgrid" runat="server" Height="470px" Width="100%" BorderWidth="1"
                        BorderColor="SkyBlue" ScrollBars="Auto">
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                            <ContentTemplate>--%>
                                <asp:GridView ID="gvEncounter" runat="server" CssClass="table table-condensed" AllowPaging="false"
                                    PageSize="10" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvEncounter_OnRowCommand"
                                    OnRowDataBound="gvEncounter_RowDataBound" OnPageIndexChanging="gvEncounter_OnPageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Theatre Name" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTheatreName" runat="server" Text='<%#Eval("TheatreName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, RegNo%>' HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Patient&nbsp;Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                <asp:HiddenField ID="hdnTransferId" runat="server" Value='<%#Eval("TransferId") %>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                <asp:HiddenField ID="hdnTransferToWardBy" runat="server" Value='<%#Eval("TransferToWardBy") %>' />
                                                <asp:HiddenField ID="hdnWardAckBy" runat="server" Value='<%#Eval("WardAckBy") %>' />
                                                <asp:HiddenField ID="hdnOTBookingID" runat="server" Value='<%#Eval("OTBookingID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bed No" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                
                                            <asp:TemplateField HeaderText="Ward Name" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ward Remarks" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardRemarks" runat="server" Text='<%#Eval("WardRemarks") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ward to OT By" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardtoOTByName" runat="server" Text='<%#Eval("WardtoOTByName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ward to OT Date" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardtoOTDate" runat="server" Text='<%#Eval("WardtoOTDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OT By" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTAckCancelByName" runat="server" Text='<%#Eval("OTAckCancelByName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OT Date" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTAckCancelDate" runat="server" Text='<%#Eval("OTAckCancelDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OT Remarks" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOTAckCancelRemarks" runat="server" Text='<%#Eval("OTAckCancelRemarks") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transfer to Ward By" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransferToWardByName" runat="server" Text='<%#Eval("TransferToWardByName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                
                                                
                                                
                                        <asp:TemplateField HeaderText="Transfer to Ward Date" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransferToWardDate" runat="server" Text='<%#Eval("TransferToWardDate") %>' />
                                                <%--WardAckByName 
                                                WardAckDate--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Acknowledge' HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkAcknowledge" runat="server" Text='Acknowledge' CommandName="Acknowledge"
                                                    CommandArgument='<%#Eval("TransferId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Cancel' HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkCancel" runat="server" Text='Cancel' CommandName="Cancel1"
                                                    CommandArgument='<%#Eval("TransferId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Send&nbsp;to&nbsp;Ward' HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSendtoWard" runat="server" Text='Send&nbsp;to&nbsp;Ward' CommandName="SendtoWard"
                                                    CommandArgument='<%#Eval("TransferId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='Ward Ack' HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkWardAck" runat="server" Text='Ward&nbsp;Ack' CommandName="WardAck"
                                                    CommandArgument='<%#Eval("TransferId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            <%--</ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvEncounter" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </asp:Panel>
                </div>
                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>