<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DischargeSummaryAuditTrail.aspx.cs" Inherits="ICM_DischargeSummaryAuditTrail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
    </script>

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr class="clsheader">
            <td id="tdHeader" align="left" style="padding-left: 10px; width: 110px" runat="server">
                <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Patient Summary Audit Trail"
                    Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="center" style="font-size: 12px;">
                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="2" style="margin-left: 10px">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="label" Text="Registration No" />
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" SkinID="textbox" runat="server" Width="120px"
                    MaxLength="9" />
                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                    FilterType="Custom,Numbers" TargetControlID="txtRegistrationNo" ValidChars="0123456789" />
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="label" Text="Patient Name" />
            </td>
            <td>
                <asp:TextBox ID="txtPatientName" SkinID="textbox" runat="server" Width="300px" MaxLength="30" />
            </td>
            <td>
                <table cellspacing="4">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text="(Definalized Date) From" />
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="To" />
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="label" Text="IP No." />
            </td>
            <td>
                <asp:TextBox ID="txtEncounterNo" SkinID="textbox" runat="server" Width="120px" MaxLength="13" />
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="label" Text="Recommend By (Doctor)" />
            </td>
            <td>
                <telerik:RadComboBox ID="ddlDeFinalizeRecommendBy" runat="server" SkinID="DropDown"
                    EmptyMessage="[ Select ]" Width="305px" Height="300px" MarkFirstMatch="true" />
            </td>
            <td>
                <table cellspacing="4">
                    <tr>
                        <td>
                            <asp:Button ID="btnFilter" runat="server" ToolTip="Filter&nbsp;Data" OnClick="btnFilter_OnClick"
                                SkinID="Button" Text="Filter" />
                        </td>
                        <td>
                            <asp:Button ID="btnExport" runat="server" SkinID="Button" CausesValidation="false"
                                Text="Export" OnClick="btnExport_OnClick" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" width="99%">
        <tr>
            <td>
                <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%">
                    <asp:GridView ID="gvDetails" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="True" PageSize="15" OnRowCommand="gvDetails_RowCommand" OnPageIndexChanging="gvDetails_PageIndexChanging">
                        <Columns>
                             <asp:TemplateField HeaderText="Format" HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="lblFormat" runat="server" SkinID="label" Text='<%#Eval("Format") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reg. No." HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Text='<%#Eval("RegistrationNo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IP No." HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Patient Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblPatientName" runat="server" SkinID="label" Text='<%#Eval("PatientName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finalized Date Time" HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinalizeDateTime" runat="server" SkinID="label" Text='<%#Eval("FinalizeDateTime") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finalized By" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinalizeByName" runat="server" SkinID="label" Text='<%#Eval("FinalizeByName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Definalized Date Time" HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeFinalizeDateTime" runat="server" SkinID="label" Text='<%#Eval("DeFinalizeDateTime") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Definalized By" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeFinalizeByName" runat="server" SkinID="label" Text='<%#Eval("DeFinalizeByName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recommend By (Doctor)" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeFinalizeRecommendByName" runat="server" SkinID="label" Text='<%#Eval("DeFinalizeRecommendByName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason" HeaderStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeFinalizeReason" runat="server" SkinID="label" Text='<%#Eval("DeFinalizeReason") %>'
                                        HeaderStyle-Width="40px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" SkinID="label" CommandName="VIEW" CommandArgument='<%#Eval("SummaryId") %>'
                                        Text="View" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="RadWindow2" OpenerElementID="btnPrintPdf" runat="server">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>

