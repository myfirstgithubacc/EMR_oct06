<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientDietList.aspx.cs" Inherits="Diet_PatientDietList" Title="Patient Diet List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function OnClientFindClose(oWnd, args) {
            $get('<%=btnRefresh.ClientID%>').click();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td width="70px">
                        &nbsp;
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Date"></asp:Label>
                    </td>
                    <td width="120px">
                        <telerik:RadDatePicker ID="dtpfromdate" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    <td width="80px">
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Ward Station"></asp:Label>
                    </td>
                    <td width="180px">
                        <telerik:RadComboBox ID="ddlWard" runat="server" Width="150" DropDownWidth="250px">
                        </telerik:RadComboBox>
                    </td>
                    <td><asp:CheckBox ID="chkFB" runat="server" Text="F & B" /></td>
                    <td align="center">
                        <asp:Label ID="lblMessage" SkinID="label" runat="server" Font-Bold="true" Text="&nbsp;" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" SkinID="Button" OnClick="btnFilter_Click" />
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" SkinID="Button" OnClick="btnRefresh_Click" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" SkinID="Button" OnClick="btnPrint_Click" />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="gvPatientDetails" runat="server" AutoGenerateColumns="false"
                            AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-VerticalAlign="Top"
                            AllowMultiRowSelection="false" Skin="Office2007" Height="490" GridLines="Both"
                            ForeColor="Black" Width="100%" OnItemDataBound="gvPatientDetails_ItemDataBound"
                            HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top" ClientSettings-Resizing-AllowResizeToFit="true" ClientSettings-Resizing-AllowColumnResize="true"
                            ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="Id" Width="100%" TableLayout="Fixed">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red; width: 100%;">
                                        No Record Found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="BedNo" HeaderText="Bed No" UniqueName="BedNo"
                                        HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                    <telerik:GridBoundColumn DataField="RegNo" HeaderText="<%$ Resources:PRegistration, regno%>"
                                        UniqueName="RegNo" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                    <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration, IpNo%>" UniqueName="encounterno"
                                        DataField="encounterno">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkIpNo" runat="server" OnClick="lnkIPNo_OnClick" Text='<%#Eval("EncounterNo")%>' />
                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("Id")%>' />
                                            <asp:HiddenField ID="hdnEncId" runat="server" Value='<%#Eval("EncounterID") %>' />
                                            <asp:HiddenField ID="hndEMRRequestId" runat="server" Value='<%#Eval("EMRRequestId")%>' />
                                            <asp:HiddenField ID="hdnIsVIP" runat="server" Value='<%#Eval("IsVIP")%>' />
                                            <asp:HiddenField ID="hdnVIPNarration" runat="server" Value='<%#Eval("VIPNarration")%>' />
                                            <asp:HiddenField ID="hdnDieticianRemarks" runat="server" Value='<%#Eval("DieticianRemarks")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle Width="5%" />
                                    </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn UniqueName="AdmissionDate" HeaderText="Admission Date" SortExpression="AdmissionDate"
                                                AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmissionDate" runat="server" Text='<%#Eval("AdmissionDate")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="120px" />
                                                <ItemStyle Width="120px" Wrap="false" />
                                            </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="PatientName" HeaderText="Patient" UniqueName="PatientName" />
                                    <telerik:GridBoundColumn DataField="AgeGender" HeaderText="Age/Sex" UniqueName="AgeGender"
                                        HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                                    <telerik:GridBoundColumn DataField="Diagnosis" HeaderText="Diagnosis" UniqueName="Diagnosis" />
                                    <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category" UniqueName="CategoryName" />
                                    <telerik:GridBoundColumn DataField="DietOrder" HeaderText="Diet Order" UniqueName="DietOrder" />
                                    <telerik:GridBoundColumn DataField="DoctorName" HeaderText="Doctor Name" UniqueName="DoctorName" />
                                    <telerik:GridBoundColumn DataField="Height" HeaderText="Height" UniqueName="Height"
                                        HeaderStyle-Width="4%" ItemStyle-Width="4%" />
                                    <telerik:GridBoundColumn DataField="Weight" HeaderText="Weight" UniqueName="Weight"
                                        HeaderStyle-Width="4%" ItemStyle-Width="4%" />
                                    <telerik:GridBoundColumn DataField="EnteredBy" HeaderText="Entered By" UniqueName="EnteredBy"
                                        HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                    <telerik:GridBoundColumn DataField="EnteredDate" HeaderText="Entered On" UniqueName="EnteredDate"
                                        HeaderStyle-Width="6%" ItemStyle-Width="6%" />
                                    <telerik:GridBoundColumn DataField="MealCardStatus" HeaderText="Status" UniqueName="MealCardStatus"
                                        HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table border="0" width="99%" cellpadding="2" cellspacing="0">
        <tr>
            <td valign="top" style="text-align: left; padding-left: 2px;">
                <asp:Label ID="Label9" runat="server" SkinID="label" Text="" BackColor="Aqua" Height="14"
                    Width="15" BorderWidth="1px" />
                <asp:Label ID="Label10" runat="server" SkinID="label" Text="Diet Advised by Doctor"
                    Font-Size="8" />
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblCount" runat="server" Text="" ForeColor="Maroon" Font-Bold="true" />
            </td>
        </tr>
    </table>
</asp:Content>
