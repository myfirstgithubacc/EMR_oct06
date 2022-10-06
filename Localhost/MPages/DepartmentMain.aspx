<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"  AutoEventWireup="true"
    CodeFile="DepartmentMain.aspx.cs" Inherits="MPages_DepartmentMain" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" cellpadding="0" cellspacing="0" style="background-color: White;">
        <tr>
            <td class="clsheader" style="height: 20px;">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="30%">
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="width: 270px; padding-left: 10px;" align="left">
                                        Department Information
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left">
                            <table cellspacing="3">
                                <tr>
                                    <td>
                                        <%--<asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="/images/file_check.png" OnClick="New_OnClick"
                                            ToolTip="New Data..." CausesValidation="false" />--%>
                                        <asp:Button ID="ibtnNew" runat="server" OnClick="New_OnClick" CssClass="button" ToolTip="New Data..."
                                            CausesValidation="false" Text="New" />
                                    </td>
                                    <td>
                                        <%--<asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="/images/edit.png" OnClick="Edit_OnClick"
                                            ToolTip="Edit Selected Data..." CausesValidation="false" />--%>
                                        <asp:Button ID="ibtnEdit" runat="server" CssClass="button" OnClick="Edit_OnClick"
                                            ToolTip="Edit Selected Data..." CausesValidation="false" Text="Edit" />
                                    </td>
                                    <td>
                                        <%--<asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="/images/save.gif" OnClick="SaveDepartmentMain_OnClick"
                                            ValidationGroup="saveupdate" />--%>
                                        <asp:Button ID="ibtnSave" runat="server" CssClass="button" OnClick="SaveDepartmentMain_OnClick"
                                            ValidationGroup="saveupdate" Text="Save" />
                                    </td>
                                    <td>
                                        <%--<asp:ImageButton ID="ibtnUpdate" runat="server" ImageUrl="/images/Update.png" OnClick="UpdateDepartmentMain_OnClick"
                                            ValidationGroup="saveupdate" />--%>
                                        <asp:Button ID="ibtnUpdate" runat="server" CssClass="button" OnClick="UpdateDepartmentMain_OnClick"
                                            ValidationGroup="saveupdate" Text="Update" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                    ShowSummary="False" ValidationGroup="saveupdate" />
                                <AJAX:ConfirmButtonExtender ID="CBE1" runat="server" ConfirmOnFormSubmit="false"
                                    ConfirmText="Are You Sure That You Want To Save ? " TargetControlID="ibtnSave">
                                </AJAX:ConfirmButtonExtender>
                                <AJAX:ConfirmButtonExtender ID="CBE2" runat="server" ConfirmOnFormSubmit="false"
                                    ConfirmText="Are You Sure That You Want To Update ? " TargetControlID="ibtnUpdate">
                                </AJAX:ConfirmButtonExtender>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td valign="top" align="left" style="width: 100%; background-color: White">
                <asp:UpdatePanel ID="UPD1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvDepartmentMain" EventName="SelectedIndexChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr valign="top">
                                <td>
                                    &nbsp;
                                </td>
                                <td style="width: 30%">
                                    <asp:Panel ID="pnlDepartmentShow" runat="server" BackColor="White" Height="485" ScrollBars="Vertical">
                                        <table width="93%" cellpadding="0" cellspacing="2">
                                            <tr valign="top">
                                                <td style="width: 100%;">
                                                    <asp:GridView ID="gvDepartmentMain" SkinID="gridview" runat="server" AutoGenerateColumns="false"
                                                        DataKeyNames="departmentid" AllowPaging="false" PageSize="20" HorizontalAlign="Left"
                                                        Width="98%" RowStyle-Height="25px" ShowFooter="true" OnRowDataBound="gvDepartmentMain_OnRowDataBound"
                                                        OnSelectedIndexChanged="gvDepartmentMain_OnSelectedIndexChanged" AlternatingRowStyle-BackColor="#E0EBFD">
                                                        <%-- <RowStyle BackColor="#EFF3FB" />
                                                <EditRowStyle ForeColor="White" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle CssClass="clsGridheader" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />--%>
                                                        <Columns>
                                                            <asp:BoundField DataField="departmentid" HeaderText="subdeptid" Visible="true" />
                                                            <asp:BoundField DataField="departmentname" HeaderText="DEPARTMENTS" Visible="true"
                                                                HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                        <%--<SelectedRowStyle BackColor="#BEDBFF" />--%>
                                                        <HeaderStyle BackColor="#367CD2" Height="20px" BorderColor="White" />
                                                        <RowStyle BorderColor="Black" />
                                                        <AlternatingRowStyle BorderWidth="2" BorderStyle="Solid" BorderColor="Black" />
                                                        <PagerStyle BackColor="#EBF3FF" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#367CD2" Height="20px" BorderColor="White" />
                                                    </asp:GridView>
                                                </td>
                                                <td style="width: 1%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <%--           <AJAX:RoundedCornersExtender ID="RCE_pnlDepartmentShow" runat="server" Enabled="True"
                                        TargetControlID="pnlDepartmentShow" Corners="All" Radius="10">
                                    </AJAX:RoundedCornersExtender>--%>
                                </td>
                                <td valign="top">
                                    <asp:Panel ID="pnlDepartmentEntry" runat="server" Height="500px" ScrollBars="Auto">
                                        <table width="100%" style="background-color: #EFF3FB">
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td style="width: 6%">
                                                    &nbsp;
                                                </td>
                                                <td style="width: 25%; font-weight: bold;">
                                                    <asp:Literal ID="ltrlDeptName" runat="server" Text="Department Name"> </asp:Literal><span
                                                        style="color: Red">*</span>
                                                </td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtDeptName" SkinID="textbox" runat="server" Width="80%" MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ControlToValidate="txtDeptName"
                                                        ErrorMessage="Department Name Cannot Be Blank" Display="None" SetFocusOnError="true"
                                                        ValidationGroup="saveupdate">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 6%">
                                                    &nbsp;
                                                </td>
                                                <td style="width: 20%; font-weight: bold;">
                                                    <asp:Literal ID="ltrlShrtName" runat="server" Text="Short Name"></asp:Literal>
                                                </td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtShrtName" SkinID="textbox" runat="server" MaxLength="5" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 6%">
                                                    &nbsp;
                                                </td>
                                                <td style="width: 20%; font-weight: bold;">
                                                    <asp:Literal ID="ltrlHeadName" runat="server" Text="Head Name "></asp:Literal>
                                                </td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtHeadName" SkinID="textbox" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 6%">
                                                    &nbsp;
                                                </td>
                                                <td style="width: 20%; font-weight: bold;">
                                                    <asp:Literal ID="ltrlStatus" runat="server" Text="Status "></asp:Literal>
                                                </td>
                                                <td style="width: 55%">
                                                    <%--<asp:CheckBox ID="chkStatus" runat="server" Text="" />--%>
                                                    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" CellSpacing="3"
                                                        CellPadding="3">
                                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="In Active" Value="0"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 6%">
                                                    &nbsp;
                                                </td>
                                                <td style="width: 20%; font-weight: bold;">
                                                    <asp:Literal ID="ltrlEncodedBy" runat="server" Text="Encoded By/Date "></asp:Literal>
                                                </td>
                                                <td style="width: 55%">
                                                    <asp:Literal ID="ltrlEncodedByDisplay" runat="server" Text=""></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 6%">
                                                    &nbsp;
                                                </td>
                                                <td style="width: 20%; font-weight: bold;">
                                                    <br />
                                                    <asp:Literal ID="ltrlUpdateBy" runat="server" Text="Modified By/Date "></asp:Literal>
                                                </td>
                                                <td style="width: 55%">
                                                    <br />
                                                    <asp:Literal ID="ltrlUpdateByDisplay" runat="server" Text=""></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:ValidationSummary ID="VS1" runat="server" DisplayMode="List" ShowMessageBox="true"
                                                        ShowSummary="False" BackColor="White" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <%--  <AJAX:RoundedCornersExtender ID="RCE_pnlDepartmentEntry" runat="server" Enabled="True"
                                        TargetControlID="pnlDepartmentEntry" Corners="All" Radius="10">
                                    </AJAX:RoundedCornersExtender>--%>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <%--<tr>
            <td align="left" class="CollapsablePanelTop">
                Department Info...
            </td>
        </tr>--%>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
