<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EmployeeLeave.aspx.cs" Inherits="MPages_EmployeeLeave" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="clsheader" width="100%">
                <tr>
                    <td>
                        Employee Leave
                    </td>
                    <td align="center">
                      <asp:Label ID="lblMessage" runat="server"  Text="" SkinID="label"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnNew" runat="server" CausesValidation="false" SkinID="Button" Text="New"
                            OnClick="btnNew_Click" Width="35px" />
                        <asp:Button ID="btnSaveEmployeeLeave" ToolTip="Save" runat="server" SkinID="Button" OnClick="btnSaveEmployeeLeave_Click"
                            ValidationGroup="save" CausesValidation="true" Text="Save" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <td style="height: 32px">
                        <asp:Label ID="Label1" runat="server" Text="Employee" SkinID="label"></asp:Label><span style="color: Red">*</span>
                    </td>
                    <td style="height: 32px">
                        <telerik:RadComboBox ID="ddlEmployee" runat="server" Width="300px" Filter="Contains" AllowCustomText="true"
                            MarkFirstMatch="true" OnSelectedIndexChanged="ddlEmployee_OnSelectedIndexChanged"
                            AutoPostBack="true" Skin="Metro">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="From Date" SkinID="label"></asp:Label><span style="color: Red">*</span>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="dtpFromDate" Width="100px" runat="server" MinDate="">
                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                            </Calendar>
                            <DateInput DateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                        &nbsp;&nbsp; To &nbsp;&nbsp;
                        <telerik:RadDatePicker ID="dtpToDate" Width="100px" runat="server" MinDate="">
                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                            </Calendar>
                            <DateInput  DateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td style="height: 56px">
                        <asp:Label ID="Label3" runat="server" Text="Remarks" SkinID="label"></asp:Label><span style="color: Red">*</span>
                    </td>
                    <td style="height: 56px">
                        <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" TextMode="MultiLine"
                            Height="50px" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td >
                        <asp:GridView ID="gvEmployeeLeaves" runat="server" AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left"
                            DataKeyNames="LeaveId" SkinID="gridview" Width="100%" 
                            OnRowDeleting="gvEmployeeLeaves_RowDeleting" OnRowEditing="gvEmployeeLeaves_RowEditing"
                            OnRowUpdating="gvEmployeeLeaves_RowUpdating"  OnRowCancelingEdit="gvEmployeeLeaves_RowCancelingEdit">
                            <EmptyDataTemplate>
                                <div style="font-weight: bold; color: Red; width: 200px">
                                    No Record Found.</div>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Eval("EmployeeName")%>'> </asp:Label>
                                        <asp:HiddenField ID="hdnEmployeeId" runat="server" Value='<%# Eval("EmployeeId")%>' />
                                        <asp:HiddenField ID="hdnLeaveId" runat="server" Value='<%# Eval("LeaveId")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Date" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("FromDate")%>'> </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadDatePicker ID="dtpFromDate" Width="100px" runat="server" SelectedDate='<%# Eval("DisplayFromDate")%>'>
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Date" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("ToDate")%>'> </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadDatePicker ID="dtpToDate" Width="100px" runat="server" SelectedDate='<%# Eval("DisplayToDate")%>'>
                                            <DateInput  DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'> </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" Text='<%# Eval("Remarks")%>'
                                            MaxLength="200" Width="400px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Link" SelectText="Edit" ShowEditButton="true">
                                    <ItemStyle Width="20px" />
                                </asp:CommandField>
                                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/delete.gif"
                                    ShowDeleteButton="true" >
                                    <ItemStyle Width="20px" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

