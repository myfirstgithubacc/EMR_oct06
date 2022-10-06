<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PrintBillDetails.aspx.cs" Inherits="EMRReports_PrintBillDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    </telerik:RadCodeBlock>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <style type="text/css">
            div#ctl00_ContentPlaceHolder1_UpdatePanel2{
                overflow:hidden;
            }
        </style>

        <script type="text/javascript">

           <%-- function SearchPatientOnClientClose(oWnd, args) {

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
                }
                $get('<%=btnfind.ClientID%>').click();
            }--%>

           <%-- function OnClientCloseSearch(oWnd, args) {
                var org = args.get_argument();
                if (org) {
                    document.getElementById('<%=txtInvoice.ClientID%>').value = org.DocNo;
                }
                $get('<%=btnfindinvoice.ClientID%>').click();

            }--%>
        </script>

    </telerik:RadCodeBlock>
       
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main  margin_bottom" style="padding: 5px 12px;">
                <div class="row">
                    <div class="col-sm-3 col-3">
                        <h2><span id="tdHeader" align="left" runat="server">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" />
                        </span></h2>
                    </div>
                    <div class="col-sm-6 col-6 text-center ">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="col-sm-3 col-3 text-right">
                        <asp:Button ID="btnshow" runat="server" CssClass="btn btn-primary" Text="Print Preview" OnClick="btnShow_Click" />

                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="row form-group" id="tradmfrm" runat="server">
                    <div class="col-sm-4 col-6">
                        <div class="row">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnshow" ScrollBars="None">
                                <div class="col-md-3 col-sm-4">
                                    IP No.
                                </div>
                                <div class="col-md-9 col-8">
                                    <asp:TextBox ID="txtIpno" Width="100%" runat="server" SkinID="textbox" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <div class="col-sm-3 col-6" style="align-self: center;">
                        <asp:RadioButtonList ID="rdoIpbillType" runat="server" RepeatDirection="horizontal" CssClass="radioo">
                            <asp:ListItem Value="S" Text="Summary" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="D" Text="Detail"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>


                    <div class="col-sm-3">
                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </div>
                </div>
            </div>



            
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:HiddenField ID="hdnEncId" runat="server" />
</asp:Content>

