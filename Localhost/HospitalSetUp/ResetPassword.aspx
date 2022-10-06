<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/HSEMRMaster.master"
    AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="HospitalSetUp_ResetPassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table width="100%" class="clsheader">
                <tr>
                    <td>
                        Reset Password
                    </td>
                    <td align="center">
                        <asp:Label ID="lblmsg" Font-Bold="true" ForeColor="Green" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="cmdSave" runat="server" ToolTip="Save text template" SkinID="Button"
                            Text="Save" OnClick="cmdSave_Click" />
                    </td>
                </tr>
                <tr>
                <td align="left">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="clssubtopic" width="27%">
                                <asp:Label ID="lblDemographics" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="clssubtopicbar" align="right" valign="bottom">
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkCreateNewOrganization" runat="server" CausesValidation="false" Text="Create New Organization"
                                            Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                            OnClick="lnkCreateNewOrganization_OnClick"  Font-Underline="false"></asp:LinkButton>
                                        <script language="JavaScript" type="text/javascript">
                                            function LinkBtnMouseOver(lnk) {
                                                document.getElementById(lnk).style.color = "red";
                                            }
                                            function LinkBtnMouseOut(lnk) {
                                                document.getElementById(lnk).style.color = "blue";
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%" cellpadding="2" cellspacing="2">
        <colgroup>
            <col width="40%" />
            <col width="60%" />
        </colgroup>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblOrganisation" runat="server" Text="Organization" SkinID="label"></asp:Label>
                        <telerik:RadComboBox ID="rdOrganization" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdOrganization_SelectedIndexChanged"
                            CausesValidation="false">
                        </telerik:RadComboBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvEmployee" runat="server" PageSize="20" Skin="Office2007" Width="100%"
                            AllowSorting="False" AllowMultiRowSelection="False" AllowPaging="True" ShowGroupPanel="false"
                            AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true" GridLines="none"
                            OnItemCommand="gvEmployees_ItemCommand" OnPreRender="gvEmployees_PreRender"
                            OnPageIndexChanged="gvEmployees_PageIndexChanged" OnSortCommand="gvEmployees_SortCommand"
                            OnPageSizeChanged="gvEmployees_PageSizeChanged" OnSelectedIndexChanged="gvEmployees_SelectedIndexChanged">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <ClientSettings EnablePostBackOnRowClick="true">
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                            <MasterTableView Width="100%">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Employee Name" SortExpression="EmployeeName">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnEmployeeID" runat="server" Value='<%#Eval("EmployeeId") %>'>
                                            </asp:HiddenField>
                                            <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EmployeeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ButtonType="LinkButton" ></telerik:GridButtonColumn>
                                    <telerik:GridTemplateColumn HeaderText="Add to list" HeaderTooltip="Add to create formula">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select1" CommandArgument='<%#Eval("EmployeeId") %>'
                                                Text="Select" ToolTip="Select employee to reset password" CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table width="100%" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server" SkinID="label" Text="User Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" SkinID="textbox" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Old Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txtOldPassword" runat="server" SkinID="textbox" Text="********"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="New Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" SkinID="textbox" TextMode="Password"
                                        MaxLength="500" ToolTip=" Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&=*' letters."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter Password"
                                        Display="None" ControlToValidate="txtPassword" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <telerik:RadToolTip ID="RadToolTip1" runat="server" EnableShadow="true" RelativeTo="Element"
                                        Height="40px" Width="300px" TargetControlID="txtPassword" BackColor="#FFFFD5"
                                        Position="TopCenter">
                                    </telerik:RadToolTip>
                                    <asp:RegularExpressionValidator ID="regexpName" Display="None" runat="server" ErrorMessage="Password is not in correct format"
                                        ValidationGroup="save" ControlToValidate="txtPassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^*&+=]).*$"></asp:RegularExpressionValidator>
                                    <AJAX:PasswordStrength ID="PasswordStrength1" DisplayPosition="BelowLeft" CalculationWeightings="50;15;15;20"
                                        TextStrengthDescriptions="Weak;Average;Strong;Excellent" TargetControlID="txtPassword"
                                        MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                                        MinimumUpperCaseCharacters="1" runat="server">
                                    </AJAX:PasswordStrength>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="Confirm Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" SkinID="textbox" TextMode="Password"
                                        MaxLength="500"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Confirm Password"
                                        Display="None" ControlToValidate="txtConfirmPassword" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="comPassword" runat="server" ControlToValidate="txtPassword"
                                        ControlToCompare="txtConfirmPassword" Operator="Equal" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Both password should be same"></asp:CompareValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="true" ShowSummary="false" />
</asp:Content>