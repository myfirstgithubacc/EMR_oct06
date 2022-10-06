<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="MPages_ChangePassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />       
    
    
    
    
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main form-group margin_bottom">
                <div class="col-md-3 col-sm-3"><h2>Change Password</h2></div>
                <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="lblMessage" Font-Bold="true" runat="server" ForeColor="Green" /></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:HiddenField ID="hdnNextAppDate" runat="server" />
                    <asp:Button ID="ibtnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary"
                        OnClick="ibtnChangePassword_Click" ValidationGroup="save" CausesValidation="true" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>





    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <div class="container-fluid main_box" runat="server" id="tblChangePassword">
                <div class="row">
                    <div class="col-md-offset-3 col-md-5 col-sm-offset-3 col-sm-6">
                        
                        <div class="header_main container-fluid">
                            <div class="col-md-12 col-sm-12">
                                
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label><font color="red">&nbsp;*</font></div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtchUserName" runat="server" AutoCompleteType="None" Wrap="true" MaxLength="50" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtchUserName"
                                            Display="None" ErrorMessage="User name should not be empty" SetFocusOnError="true"
                                            ValidationGroup="save" Text="*"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblOldPassword" runat="server" Text="Old Password"></asp:Label><font color="red">&nbsp;*</font></div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtoldpassword" MaxLength="500" TextMode="Password" runat="server" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="save" Display="None"
                                            ControlToValidate="txtoldpassword" runat="server" ErrorMessage="Enter Password"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblNewPassword" runat="server" Text="New Password"></asp:Label><font color="red">&nbsp;*</font></div>
                                    <div class="col-md-8 col-sm-8">
                                        <div class="row">
                                            <div class="col-md-10 col-sm-10 PaddingRightSpacing01">
                                                <asp:TextBox ID="txtnewpassword" MaxLength="500" TextMode="Password"
                                                    CausesValidation="true" runat="server" Width="100%" ToolTip="Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."></asp:TextBox>
                                            </div>
                                            <div class="col-md-2 col-sm-2 PaddingLeftSpacing">
                                                <asp:Button ID="btnpasshelp" runat="server" CausesValidation="false" OnClick="btnpasshelp_Click"
                                                    CssClass="btn btn-primary" Text="?" ToolTip="  Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."
                                                    Width="15px" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="save" Display="None"
                                                    ControlToValidate="txtnewpassword" runat="server" ErrorMessage="Enter New Password"
                                                    EnableClientScript="true"></asp:RequiredFieldValidator>
                                              <%--  <AJAX:PasswordStrength ID="PasswordStrength1" CalculationWeightings="50;15;15;20"
                                                    TextStrengthDescriptions="Weak;Average;Strong;Excellent" TargetControlID="txtnewpassword"
                                                    MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                                                    MinimumUpperCaseCharacters="1" runat="server">
                                                </AJAX:PasswordStrength>--%>
                                                <%--<asp:RegularExpressionValidator ID="regexpName" ValidationGroup="save" Display="None"
                                                    runat="server" ErrorMessage="Password is not in write format." ControlToValidate="txtnewpassword"
                                                    ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^*&+=]).*$"></asp:RegularExpressionValidator>--%>
                                                <telerik:RadToolTip ID="RadToolTip5" runat="server" EnableShadow="true" RelativeTo="Element"
                                                    Height="40px" Width="300px" TargetControlID="txtnewpassword" BackColor="#FFFFD5"
                                                    Position="TopCenter">
                                                </telerik:RadToolTip>
                                                <telerik:RadToolTip ID="RadToolTip1" runat="server" EnableShadow="true" RelativeTo="Element"
                                                    Height="40px" Width="300px" TargetControlID="btnpasshelp" BackColor="#FFFFD5"
                                                    Position="TopCenter">
                                                </telerik:RadToolTip>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 PaddingRightSpacing label2"><asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label><font color="red">&nbsp;*</font></div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtConfirmPassword" MaxLength="500" TextMode="Password" runat="server" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="None" ControlToValidate="txtConfirmPassword" runat="server" ErrorMessage="Enter Confirm Password" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="New Password and Confirm Password Should be Same" Display="None" ControlToValidate="txtConfirmPassword" ControlToCompare="txtnewpassword" SetFocusOnError="true" Operator="Equal" ValidationGroup="save"></asp:CompareValidator>
                                        <asp:ValidationSummary ID="vs1" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="save" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12 text-center">
                                        <asp:HiddenField ID="usercode" runat="server" />
                                        <%--<asp:LinkButton ID="lbtnCancel" runat="server" Text="&nbsp;Cancel" OnClick="lbtnCancel_Click"
                                            ForeColor="White" CausesValidation="False"></asp:LinkButton>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>