<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MLCPopup.aspx.cs" Inherits="WardManagement_MLCPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
        <table class="clsheader" width="100%">
            <tr>
                <td>
                <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="ibtnClose" runat="server" AccessKey="C" SkinID="Button" Text="Close"
                        ToolTip="Cancel" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                  <%--  <asp:Panel ID="pnlAdmission" runat="server" Height="550px" Width="100%">--%>
                        <telerik:RadGrid ID="gvAdmission" runat="server" Skin="Office2007" BorderWidth="0"
                            PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                            Width="100%" Height="530px" AutoGenerateColumns="false" ShowStatusBar="true" EnableLinqExpressions="false"
                            GridLines="Both" AllowPaging="false" AllowAutomaticDeletes="false" ShowFooter="false"
                            AllowSorting="false">
                            <%--AllowCustomPaging="true"--%>
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
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                            <asp:HiddenField ID="hdnMLC" runat="server" Value='<%#Eval("MLC")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                        DataField="EncounterNo" SortExpression="EncounterNo" HeaderStyle-Width="60px"
                                        ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" DataField="PatientName"
                                        SortExpression="PatientName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AdmissionDate" HeaderText="Admission Date"
                                        DataField="AdmissionDate" SortExpression="AdmissionDate" HeaderStyle-Width="100px"
                                        ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdmissionDate" runat="server" Text='<%#Eval("AdmissionDate")%>'></asp:Label>
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
                                    <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor Name" DataField="DoctorName"
                                        SortExpression="DoctorName" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                  <%--  </asp:Panel>--%>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
