<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="MedicationFrequencyMaster.aspx.cs" Inherits="Pharmacy_MedicationFrequencyMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">        
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr class="clsheader">
                    <td id="tdHeader" align="left" style="padding-left: 10px; width: 250px" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Set Frequency Schedule"
                            Font-Bold="true" />
                    </td>
                    <td align="center" style="font-size: 12px;">
                        <asp:Label ID="lblFrequencyName" runat="server" SkinID="label" Text="&nbsp;" />
                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                    </td>
                    <td align="right">
                        <asp:Button ID="Btnsave1" Text="Save" runat="server" OnClick="Btnsave1_Click" />
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0">
                <tr>
                    <td align="center">
                        <asp:Label ID="label1" runat="server" Text="Facility" />
                        <span style='color: Red'>*</span>
                        <telerik:RadComboBox ID="ddlFacility" runat="server" Width="200px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlFacility_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" valign="top" style="width: 350px">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnl33" runat="server" ScrollBars="Auto" Height="508px" BorderWidth="1px"
                                    BorderColor="LightBlue">
                                    <telerik:RadGrid ID="gvmfrequency" Skin="Office2007" BorderWidth="0" AllowMultiRowSelection="false"
                                        AutoGenerateColumns="False" runat="server" Height="100%" Width="100%" EnableLinqExpressions="false"
                                        GridLines="Both" OnItemCommand="gvmfrequency_ItemCommand1">
                                        <ClientSettings>
                                            <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                        </ClientSettings>
                                        <MasterTableView TableLayout="Fixed">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red;">
                                                    No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="FrequencyId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("FrequencyId")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="description" HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldesc" runat="server" Text='<%#Eval("description")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="frequency" HeaderText="Frequency" HeaderStyle-Width="80px"
                                                    ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfreq" runat="server" Text='<%#Eval("frequency")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="View" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="view" Text="Select" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <table border="0" style="margin-left: 2px" cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="label12" runat="server" Text="Frequency Time" />
                                                <span style='color: Red'>*</span>
                                            </td>
                                            <td>
                                                <telerik:RadTimePicker ID="RedFrequencyTime" runat="server" Width="85px" Style="text-transform: uppercase;"
                                                   TimeView-Columns="6" TimeView-Interval="00:30" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="Active" />
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="80px">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="Active" Value="1" />
                                                        <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFrequencyId" runat="server" Visible="False" SkinID="label" Text=""
                                        Font-Bold="true" />
                                    <asp:Label ID="lblid" runat="server" SkinID="label" Visible="false" Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="tdGrid" runat="server" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="468px" Width="320px"
                                                BorderWidth="1px" BorderColor="LightBlue">
                                                <telerik:RadGrid ID="RadGrid1" Skin="Office2007" BorderWidth="0" AllowMultiRowSelection="false"
                                                    runat="server" Width="100%" Height="100%" AutoGenerateColumns="False" EnableLinqExpressions="false"
                                                    GridLines="Both" OnItemCommand="RadGrid1_ItemCommand">
                                                    <ClientSettings>
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">
                                                                No Record Found.</div>
                                                        </NoRecordsTemplate>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMasterId" runat="server" Text='<%#Eval("FrequencyDetailId")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDetailId" runat="server" Text='<%#Eval("FrequencyId")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Frequency Time">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfTime" runat="server" Text='<%#Eval("FrequencyTime")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Active" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblActiv" runat="server" Text='<%#Eval("Active")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="Lview" runat="server" CommandName="Select" Text="Select" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid></asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:Button ID="btnFindClose" runat="server" CausesValidation="false" Style="visibility: hidden;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
