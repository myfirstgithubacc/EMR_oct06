<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpectedDateOfDischarge.aspx.cs"
    Inherits="EMRBILLING_Popup_ExpectedDateOfDischarge" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Expected Date Of Discharge Deatils</title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="Table1" runat="server" border="0" class="clsheader" width="100%" cellpadding="0"
            cellspacing="0">
            <tr>
                <td width="150px">
                    &nbsp;&nbsp;<asp:Label ID="lblDate" runat="server" SkinID="label" Text="Date" />
                    <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged"
                        SkinID="DropDown" Width="100px" CausesValidation="false">
                        <Items>
                            <telerik:RadComboBoxItem Value="DD0" Text="Today" Selected="true" />
                            <telerik:RadComboBoxItem Value="DD1" Text="Tomorrow " />
                            <telerik:RadComboBoxItem Value="A" Text="All" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td width="400px">
                    <asp:Button ID="btnFilter" runat="server" Text="Filter" SkinID="Button" OnClick="btnFilter_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="btnEdod" runat="server" Text="Marked For Discharge Request"></asp:Label>
                    <asp:Label ID="lblNoOfDischarge" Font-Size="11px" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td align="left">
                    <table id="tblDateRange" runat="server">
                        <tr>
                            <td align="left">
                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="110px" />
                            </td>
                            <td>
                                &nbsp;To&nbsp;
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="110px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="180px">
                    <asp:Button ID="btnExport" runat="server" AccessKey="C" SkinID="Button" Text="ExportToExcel"
                        ToolTip="Export Data" OnClick="btnExport_OnClick" />
                    &nbsp;
                    <asp:Button ID="ibtnClose" runat="server" AccessKey="C" SkinID="Button" Text="Close"
                        ToolTip="Cancel" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlAdmission" runat="server" Height="550px" Width="100%" ScrollBars="Vertical">
                        <telerik:RadGrid ID="gvAdmission" runat="server" Skin="Office2007" BorderWidth="0"
                            PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                            Width="100%" AutoGenerateColumns="false" ShowStatusBar="true" EnableLinqExpressions="false"
                            GridLines="Both" AllowPaging="false" AllowAutomaticDeletes="false"
                            ShowFooter="false" AllowSorting="false" > <%--AllowCustomPaging="true"--%>
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
                                </Scrolling>
                            </ClientSettings>
                            <PagerStyle ShowPagerText="true" />
                            <MasterTableView AllowFilteringByColumn="false" ItemStyle-Wrap="false">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                        DataField="RegistrationNo" SortExpression="RegistrationNo" HeaderStyle-Width="60px"
                                        ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                        DataField="EncounterNo" SortExpression="EncounterNo" HeaderStyle-Width="60px"
                                        ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PName" HeaderText="Patient Name" DataField="PName"
                                        SortExpression="PName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPName" runat="server" Text='<%#Eval("PName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" DataField="GenderAge"
                                        SortExpression="GenderAge" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AdmittingDoctorName" HeaderText="Admitting Doctor Name"
                                        DataField="AdmittingDoctorName" SortExpression="AdmittingDoctorName" HeaderStyle-Width="160px"
                                        ItemStyle-Width="160px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdmittingDoctorName" runat="server" Text='<%#Eval("AdmittingDoctorName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="Ward Name" DataField="MobileNo"
                                        SortExpression="WardName" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BedNo" HeaderText="Bed No" DataField="BedNo"
                                        SortExpression="BedNo" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="Expected Date Of Discharge"
                                        DataField="ExpectedDateOfDischarge" SortExpression="ExpectedDateOfDischarge"
                                        HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpectedDateOfDischarge" runat="server" Text='<%#Eval("ExpectedDateOfDischarge")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncodedBy" HeaderText="Endoded By" DataField="EncodedBy"
                                        SortExpression="EncodedBy" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
