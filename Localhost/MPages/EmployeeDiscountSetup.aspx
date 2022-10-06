<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EmployeeDiscountSetup.aspx.cs" Inherits="MPages_EmployeeDiscountSetup" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr class="clsheader">
                        <td align="left" style="padding-left: 5px;">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Employee Discount Approval Setup" Font-Bold="true" />
                        </td>
                        
                       <td align="center">
                        <asp:Button ID="btnNew" runat="server" CssClass="button" OnClick="btnNew_OnClick"
                                SkinID="Button" ToolTip="New" CausesValidation="False" Text="New" />
                            <asp:Button ID="btnSave" SkinID="Button" runat="server" Text="Save" 
                               onclick="btnSave_Click" />  
                       </td>
                    </tr>
  </table>
  
  <table border="0" cellpadding="2" cellspacing="2" width="100%">
  <tr>
 <td colspan="8" align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                        </td>
  </tr>
                                <tr>
                                    <td width="90px" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Facility" SkinID="label" />
                                        <span style='color: Red'>*</span>
                                    </td>
                                    <td style="width:100px">
                                        <telerik:RadComboBox ID="ddlFacility" runat="server" Width="180px" EmptyMessage="[ Select ]"
                                            MarkFirstMatch="true" />
                                    </td>
                                    <td width="90px" align="right">
                                        <asp:Label ID="Label2" runat="server" Text="Level" SkinID="label" />
                                        <span style='color: Red'>*</span>
                                    </td>
                                    <td width="170px">
                                        <telerik:RadComboBox ID="ddlLevel" runat="server" Width="120px" EmptyMessage="[ Select ]"
                                            MarkFirstMatch="true">
                                           </telerik:RadComboBox>
                                    </td>
                                    <td width="90px" align="right">
                                        <asp:Label ID="Label3" runat="server" Text="Employee" SkinID="label" />
                                        <span style='color: Red'>*</span>
                                    </td>
                                    <td width="200px">
                                        <telerik:RadComboBox ID="ddlEmployee" runat="server" Width="220px" EmptyMessage="[ Select ]"
                                            MarkFirstMatch="true" />
                                    </td>
                                    <td width="90px" align="right">
                                        <asp:Label ID="Label10" runat="server" SkinID="label" Text="Status" />
                                        <span style='color: Red'>*</span>
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
                                
                                <tr>
                        <td colspan="8" align="center" style="padding-top:20px">
                            <asp:Panel ID="Panel1" runat="server" BackColor="White" Height="360px" Width="600px"
                                ScrollBars="Auto" BorderWidth="1px" BorderStyle="Solid" BorderColor="LightBlue">
                                <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                    AllowFilteringByColumn="true" AllowMultiRowSelection="false" runat="server" Width="600px"
                                    AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                    GridLines="Both" AllowPaging="True" Height="99%" PageSize="10"
                                     OnPageIndexChanged="gvData_OnPageIndexChanged" OnItemCommand="gvData_OnItemCommand">
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                    </ClientSettings>
                                    <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <ItemStyle Wrap="true" />
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="FacilityName" ShowFilterIcon="false" DefaultInsertValue=""
                                                DataField="FacilityName" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                HeaderText="Facility" FilterControlWidth="99%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName")%>' />
                                                     <asp:HiddenField ID="hdnFacilityid" runat="server" Value='<%#Eval("Facilityid")%>' />
                                                    <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ApprovalLevel" ShowFilterIcon="false" DefaultInsertValue=""
                                                DataField="ApprovalLevel" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                HeaderText="Approval Level" FilterControlWidth="99%" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnApprovalId" runat="server" Value='<%#Eval("ApprovalId")%>' />
                                                    <asp:Label ID="lblApprovalLevel" runat="server" Text='<%#Eval("ApprovalLevel")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="EmployeeName" ShowFilterIcon="false" DefaultInsertValue=""
                                                DataField="EmployeeName" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                HeaderText="Employee" FilterControlWidth="99%" HeaderStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnEmployeeId" runat="server" Value='<%#Eval("EmployeeId")%>' />
                                                    <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EmployeeName")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="40px"
                                                HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </asp:Panel>
                        </td>
                    </tr>
</table>
</asp:Content>

