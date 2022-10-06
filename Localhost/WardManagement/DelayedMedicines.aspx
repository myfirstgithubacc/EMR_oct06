<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelayedMedicines.aspx.cs"
    Inherits="WardManagement_Delayed_Medicines" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container-fluid header_main">
        <div class="col-md-3 col-sm-3">
            <h2>
                <asp:Label ID="lblHeader" runat="server" Text="Ward Dashboard" /></h2>
        </div>
        <div class="col-md-6 col-sm-6 text-center">
            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
        <div class="col-md-3 col-sm-3 text-right">
            <asp:Button ID="BtnClose" CssClass="btn btn-primary" Width="30%" Height="25px" runat="server"
                Text="Close" OnClientClick="window.close();" />
        </div>
    </div>
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <telerik:RadGrid ID="gvEMRWardDashBoardDetails" runat="server" Skin="Office2007"
                        BorderWidth="0" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false"
                        AllowMultiRowSelection="false" Width="100%" Height="530px" AutoGenerateColumns="false"
                        ShowStatusBar="true" EnableLinqExpressions="false" GridLines="Both" AllowPaging="false"
                        AllowAutomaticDeletes="false" ShowFooter="false" AllowSorting="false">
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
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                    DataField="EncounterNo" SortExpression="EncounterNo" HeaderStyle-Width="60px"
                                    ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" DataField="PatientName"
                                    SortExpression="PatientName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>'></asp:Label>
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
                                <telerik:GridTemplateColumn UniqueName="ItemName" HeaderText="Item Name" DataField="ItemName"
                                    SortExpression="ItemName" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="FrequencyTime" HeaderText="Frequency Time"
                                    DataField="FrequencyTime" SortExpression="FrequencyTime" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrequencyTime" runat="server" Text='<%#Eval("FrequencyTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvEMRWardDashBoardDetails" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
