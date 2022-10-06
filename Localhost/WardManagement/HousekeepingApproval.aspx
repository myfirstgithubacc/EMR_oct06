<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="HousekeepingApproval.aspx.cs" Inherits="WardManagement_HousekeepingApproval" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">

        function OnClientPrintClose(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=btnSearch.ClientID%>').click();

        }


    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>


            <div class="container-fluid header_main  margin_bottom">
                <div class="col-md-3">
                    <h2><span style="width: 230px">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" Text="&nbsp;Housekeeping Approval" />
                    </span></h2>
                </div>

                <div class="col-md-9 text-center">
                    <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                </div>
            </div>

            <table border="0" cellpadding="4" cellspacing="2" style="margin-left: 20px;">
                <tr>
                    <td style="width: 70px">
                        <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                    </td>
                    <td style="width: 130px">
                        <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="120px" SkinID="DropDown"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Bed No." Value="BED" Selected="true" />
                                <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, RegNo%>" Value="REG" />
                                <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, EncounterNo%>" Value="ENC" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 140px">
                        <asp:Panel ID="Panel4" runat="server" DefaultButton="btnSearch">
                            <asp:TextBox ID="txtSearchNumeric" runat="server" SkinID="textbox" Width="120px" MaxLength="9" Visible="false" />
                            <asp:TextBox ID="txtSearch" runat="server" SkinID="textbox" Width="120px" MaxLength="13" />
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                FilterType="Custom" TargetControlID="txtSearchNumeric" ValidChars="0123456789" />
                        </asp:Panel>
                    </td>
                    <td style="width: 40px">
                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="From" />
                    </td>
                    <td style="width: 110px">
                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                    </td>
                    <td style="width: 30px">
                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="To" />
                    </td>
                    <td style="width: 110px">
                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                    </td>
                    <td style="width: 50px">
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text='Status' />
                    </td>
                    <td style="width: 140px">
                        <telerik:RadComboBox ID="ddlApproved" runat="server" Width="120px" SkinID="DropDown"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlApproved_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="" Selected="True" />
                                <telerik:RadComboBoxItem Text="Request" Value="R" />
                                <telerik:RadComboBoxItem Text="Acknowledged" Value="A" />
                                <telerik:RadComboBoxItem Text="Closed" Value="C" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 90px">
                        <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" ToolTip="Clear Filter"
                            Width="80px" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" ToolTip="Filter" Text="Filter"
                            Width="80px" OnClick="btnSearch_OnClick" />
                    </td>
                </tr>
            </table>

            <div class="container-fluid">
                <asp:Panel ID="pnlgrid" runat="server" Height="470px" Width="100%" BorderWidth="1"
                    BorderColor="SkyBlue" ScrollBars="Auto">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvEncounter" runat="server" SkinID="gridview" AllowPaging="true"
                                PageSize="10" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvEncounter_OnRowCommand"
                                OnRowDataBound="gvEncounter_RowDataBound" OnPageIndexChanging="gvEncounter_OnPageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText='Bed No.' HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedNo" runat="server" SkinID="label" Text='<%#Eval("BedNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, RegNo%>' HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Text='<%#Eval("RegistrationNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="95px" ItemStyle-Width="95px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Patient&nbsp;Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" SkinID="label" Text='<%#Eval("PatientName") %>' />
                                            <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Eval("RequestId") %>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                            <asp:HiddenField ID="hdnIsAcknowledged" runat="server" Value='<%#Eval("IsAcknowledged") %>' />
                                            <asp:HiddenField ID="hdnIsClosed" runat="server" Value='<%#Eval("IsClosed") %>' />
                                            <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason / Remarks" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReason" runat="server" SkinID="label" Text='<%#Eval("Reason") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Requested By" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestedBy" runat="server" SkinID="label" Text='<%#Eval("RequestedBy") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestDateTime" runat="server" SkinID="label" Text='<%#Eval("RequestDateTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acknowledged By" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcknowledgedBy" runat="server" SkinID="label" Text='<%#Eval("AcknowledgedBy") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acknowledged Date" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcknowledgedDateTime" runat="server" SkinID="label" Text='<%#Eval("AcknowledgedDateTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Closed By" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosedBy" runat="server" SkinID="label" Text='<%#Eval("ClosedBy") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Closed Date" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosedDateTime" runat="server" SkinID="label" Text='<%#Eval("ClosedDateTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAcknowledge" runat="server" Text='Acknowledge' CommandName="REQUESTSELECT"
                                                CommandArgument='<%#Eval("RequestId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to revert back"
                                                CommandName="REQUESTDELETE" CausesValidation="false" CommandArgument='<%#Eval("RequestId")%>'
                                                ImageUrl="~/Images/DeleteRow.png" Height="16px" Width="16px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvEncounter" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <table cellspacing="0" cellpadding="0" style="margin-left: 20px">
                <tr>
                    <td style="width: 100px; text-align: center;">
                        <asp:Label ID="legAcknowledge" runat="server" CssClass="LegendColor" BackColor="LightCyan" Text="Request" Width="100%" />
                    </td>
                    <td>&nbsp;&nbsp;</td>
                    <td style="width: 100px; text-align: center;">
                        <asp:Label ID="Label1" runat="server" CssClass="LegendColor" BackColor="LightPink" Text="Acknowledge" Width="100%" />
                    </td>
                    <td>&nbsp;&nbsp;</td>
                    <td style="width: 100px; text-align: center;">
                        <asp:Label ID="Label3" runat="server" CssClass="LegendColor" BackColor="LightGreen" Text="Close" Width="100%" />
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <asp:Label ID="lblTotalRecordCount" runat="server" Text="" Font-Bold="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
