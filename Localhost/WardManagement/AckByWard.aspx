<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AckByWard.aspx.cs"
    Inherits="WardManagement_AckByWard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Unperformed Services</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>

    <form id="form1" runat="server" defaultbutton="btnFilter">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <script type="text/javascript" language="javascript">
            function validateMaxLength() {
                var txt = $get('<%=txtSearchRegNo.ClientID%>');
                if (txt.value > 9223372036854775807) {
                    alert("Value should not be more then 9223372036854775807.");
                    txt.value = txt.value.substring(0, 12);
                    txt.focus();
                }
            }
            function OnCollectClientClose(oWnd) {
                $get('<%=btnFilter.ClientID%>').click();
            }
        </script>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="container-fluid header_main form-group">
                    <div class="col-md-3">
                        <%--<h2>
                            <asp:Label ID="Label1" runat="server" ForeColor="Navy" Text="No of Patient " />
                            <asp:Label ID="lblNoOfPatient" runat="server" ForeColor="DarkRed" Text="" />
                        </h2>--%>
                    </div>
                    <div class="col-md-7 text-center"><asp:Label ID="lblMessage" runat="server" Text="" /></div>
                    <div class="col-md-2 text-right">
                        <asp:Button ID="btnAck" runat="server" Text="Acknowledge" CssClass="btn btn-primary" OnClick="btnAck_OnClick" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" OnClientClick="window.close();" /></div>
                </div>

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-7">
                                    <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" Width="100%" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged">
                                        <Items>
                                           <%-- <telerik:RadComboBoxItem Text="Ward" Value="W" Selected="true" />--%>
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, UHID%>' Value="R" Selected="true"/>
                                            <telerik:RadComboBoxItem Text="Encounter No" Value="ENC" />
                                            <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                                <div class="col-md-5 PaddingLeftSpacing">
                                    <asp:TextBox ID="txtSearchRegNo" Width="100%" runat="server" Text="" MaxLength="13" onkeyup="return validateMaxLength();"  />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchRegNo" ValidChars="0123456789" />
                                    <asp:TextBox ID="txtSearch" runat="server" Width="100%" MaxLength="30" Visible="false" />
                                    <%--<telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" Height="350px" Filter="Contains" EmptyMessage="[ All Selected ]" />--%>
                                </div>
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" CssClass="btn btn-primary" OnClick="btnClearFilter_Click" />
                        </div>
                        
                    </div>
                </div>

                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                    </Windows>
               </telerik:RadWindowManager>
                <div class="container-fluid" id="tbldrugorder" runat="server">
                    <div class="row form-group">
                        <asp:GridView ID="gvAckByWardDetails" OnPageIndexChanging="gvAckByWardDetails_OnPageIndexChanging"
                            AllowPaging="true" PageSize="8" runat="server" SkinID="gridviewOrderNew" Width="100%"
                            AutoGenerateColumns="false" OnRowCommand="gvAckByWardDetails_RowCommand">
                            <EmptyDataTemplate>
                                <p style="color: Red;">
                                    No Record(s) Found
                                </p>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='Transfer Date Time'
                                    ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferDateTime" runat="server" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='<%$ Resources:PRegistration, UHID%>'
                                    ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="IP No." ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterno" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Patient Name" ItemStyle-Width="140px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Age/Gender" ItemStyle-Width="140px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                					
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Doctor" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctor" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="From Ward" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromWard" runat="server" Text='<%#Eval("Fromward")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="From Bed" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromBed" runat="server" Text='<%#Eval("FromBedNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="To Ward" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToWard" runat="server" Text='<%#Eval("Toward")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="To Bed" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToBed" runat="server" Text='<%#Eval("ToBedNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Transfer Remarks"
                                    ItemStyle-Width="105px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferRemarks" runat="server" Text='<%#Eval("RequestRemarks") %>'>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                              
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDetails" CommandName="Details" CommandArgument='<%#Eval("Id")%>' runat="server">Select</asp:LinkButton>
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div class="col-md-2">
                             Remarks:
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="1000" Height="70" Width="750px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="col-md-12 left">

            <asp:Label ID="Label1" runat="server" ForeColor="Navy" Text="No of Patient " />
            <asp:Label ID="lblNoOfPatient" runat="server" ForeColor="DarkRed" Text="" />

        </div>
    </form>
</body>
</html>