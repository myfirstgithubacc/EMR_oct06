<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Employee.aspx.cs" Inherits="MPages_Employee" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="ucl" TagName="ComboEmployeeSearch" Src="~/Include/Components/EmployeeSearchCombo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        label {
            font-weight: normal !important;
        }

        .textName {
            width: 150px;
            float: left;
        }

        .textName01 {
            width: 150px;
            float: left;
        }

        .SelectHeight {
            height: 270px;
            overflow: auto;
        }

        .SelectHeight01 {
            height: 270px;
            overflow: auto;
            border-right: solid 5px #d6d6d6;
        }

        @media (min-width: 768px) and (max-width: 991px) {
            .photoSpacing {
                padding: 0 0 2px 0;
            }

            .SelectHeight {
                height: 490px;
                overflow: auto;
            }

            .SelectHeight01 {
                height: 490px;
                overflow: auto;
            }
        }
    </style>

    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

        <script language="javascript" type="text/javascript">
            function openRadWindow(strPageNameWithQueryString) {
                var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
            }

            function openRadWindow(e, Purl, wHeight, wWidth) {
                var unicode = e.keyCode ? e.keyCode : e.charCode



                var oWnd = radopen(Purl);
                oWnd.setSize(wHeight, wWidth);
                oWnd.set_modal(true);
                oWnd.set_visibleStatusbar(false);
                oWnd.Center();

            }
            function OnClientSelectedIndexChangedEventHandler(sender, args) {

                var item = args.get_item();

                $get('<%=txtemployeeno.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=hdnemployeeno.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=btnfind.ClientID%>').click();
            }

            function OnClientShow(sender, eventArgs) {
                logEvent("ToolTip with ID " + sender.get_id() + " showed.");

            }

            function cheknotification() {

                var a = document.getElementById('ctl00_ContentPlaceHolder1_txtExpiryPeriod').value;
                var b = document.getElementById('ctl00_ContentPlaceHolder1_txtExpirynotification').value;

                if (a <= b) {
                    alert("It should be less than Expiry Period.");

                    return false;
                }
            }
        </script>

    </telerik:RadScriptBlock>

    <div style="overflow-y: hidden; overflow-x: hidden;">

        <div class="container-fluid header_main">
            <div class="col-md-3 col-sm-4 PaddingRightSpacing">
                <div class="row">
                    <div class="col-md-3 col-sm-4">
                        <h2>Employee</h2>
                    </div>
                    <div class="col-md-8 col-sm-8">
                        <ucl:ComboEmployeeSearch ID="ComboEmployeeSearch" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-3 col-sm-3 PaddingRightSpacing">
                <asp:Label ID="Literal1" runat="server" Text="<%$ Resources:PRegistration, employeeno &nbsp;&nbsp; %>"></asp:Label>
                <asp:Label ID="lblEmployeeno" runat="server" Text="*" Visible="false" ForeColor="Red" />
                <asp:TextBox ID="txtemployeeno" Width="50%" runat="server" MaxLength="20"></asp:TextBox>
            </div>

            <div class="col-md-3 col-sm-3 PaddingRightSpacing">
                <asp:Label ID="lblEmpCode" Visible="false" runat="server" Text="Employee Code &nbsp;"></asp:Label>
                <asp:TextBox ID="txtEmpCode" Visible="false" Width="40%" runat="server" MaxLength="10" />

                <asp:TextBox ID="hdnemployeeno" Style="visibility: hidden; width: 1px; height: 1px; float: left;" runat="server" MaxLength="10"></asp:TextBox>
                <asp:Button ID="btnfind" runat="server" Text="Search" CssClass="button" Enabled="true"
                    OnClick="btnfind_Click" Style="visibility: hidden; width: 1px; height: 1px; float: left;" />
            </div>



            <div class="col-md-3 col-sm-2 text-right PaddingLeftSpacing">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnNew" CssClass="btn btn-default" runat="server" CausesValidation="false" Text="New" OnClick="btnNew_Click" />
                        <asp:Button ID="btnSaveEmployee" ToolTip="Save" runat="server" CssClass="btn btn-primary" OnClick="btnSaveEmployee_Click" ValidationGroup="save" CausesValidation="true" Text="Save" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>



        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="container-fluid PaddingSpacing03 margin_z" id="DivMenu" runat="server">
                    <div class="col-md-12 col-sm-12 text-center">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2 col-sm-2">

                    <div class="row form-group margin_Top">
                        <div class="col-md-3 col-sm-3">
                            <asp:Label ID="Label10" runat="server" Text="Title" />
                        </div>
                        <div class="col-md-9 col-sm-9">
                            <asp:DropDownList ID="ddltitle" runat="server" Width="100%"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-md-12 col-sm-12 text-center">
                            <asp:Image ID="PatientImage" runat="server" ImageUrl="/Images/patientLeft.jpg" BorderWidth="1" BorderColor="Gray" Width="43%" Height="43%" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12 text-center photoSpacing">
                            <asp:Button ID="ibtnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" ToolTip="Upload Image"
                                CausesValidation="false" Font-Size="7" />
                            <AJAX:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="ibtnUpload"
                                PopupControlID="pnlUpload" BackgroundCssClass="modalBackground" CancelControlID="ibtnClose"
                                DropShadow="true" />
                            <asp:Button ID="ibtnRemove" runat="server" Text="Remove" CssClass="btn btn-primary" ToolTip="Remove Image"
                                CausesValidation="false" OnClick="RemoveImage_OnClick" Font-Size="7" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12 text-center">
                            <asp:Panel ID="pnlUpload" runat="server" Style="display: none" CssClass="modalPopup">


                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <p style="color: Black; font-weight: bold;">
                                                            Add an Image
                                                        </p>
                                                    </td>
                                                    <td align="right">
                                                        <p style="color: Red; font-weight: bold;">
                                                            <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="/Images/icon-close.jpg"
                                                                CausesValidation="false" OnClientClick="return false;"></asp:ImageButton>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: solid 1 gray;">
                                            <asp:FileUpload ID="FileUploader1" runat="server" Width="250px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnUpload" runat="server" Height="22px" Text="Upload" ValidationGroup="Upload"
                                                OnClick="Upload_OnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>
                                </table>

                            </asp:Panel>
                        </div>
                    </div>

                </div>


                <div class="col-md-10 col-sm-10">
                    <div class="row header_mainGray PaddingSpacing03 margin_bottomNone borderBottom">
                        <div class="col-md-3 col-sm-3">
                            <h2>Employee Details</h2>
                        </div>
                        <div class="col-md-9 col-sm-9 text-right">
                            <asp:LinkButton ID="lnkEmployeeLookup" runat="server" CausesValidation="false" Text="Employee Look Up"
                                onmouseover="LinkBtnMouseOver(this.id);" CssClass="btnNew" Font-Bold="true" onmouseout="LinkBtnMouseOut(this.id);"
                                OnClick="lnkEmployeeLookup_OnClick"></asp:LinkButton>

                            <asp:LinkButton ID="lnkProviderProfile" runat="server" CausesValidation="false"
                                Text="Employee Profile" CssClass="btnNew" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkProviderProfile_OnClick"></asp:LinkButton>

                            <asp:LinkButton ID="lnkAppointmentTemplate" runat="server" CssClass="btnNew" Font-Bold="true" CausesValidation="false"
                                Visible="false" Text="Appointment Template" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkAppointmentTemplate_OnClick"></asp:LinkButton>

                            <asp:LinkButton ID="lnkProviderDetails" runat="server" CausesValidation="false" Visible="false"
                                CssClass="btnNew" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                Text="Doctor Details" OnClick="lnkProviderDetails_OnClick"></asp:LinkButton>

                            <asp:LinkButton ID="lnkClassification" runat="server" CausesValidation="false" Visible="false"
                                CssClass="btnNew" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                Text="Classification" OnClick="lnkClassification_OnClick"></asp:LinkButton>

                            <asp:LinkButton ID="lnkEmployeeSequence" runat="server" CausesValidation="false"
                                CssClass="btnNew" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                Text="Employee Sequence" OnClick="lnkEmployeeSequence_OnClick"></asp:LinkButton>
                            <asp:LinkButton ID="lnkUserDepartmentTagging" runat="server" CausesValidation="false" Visible="false"
                                Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                Text="User Department Tagging" OnClick="lnkUserDepartmentTagging_Click"></asp:LinkButton>
                            <script language="JavaScript" type="text/javascript">
                                function LinkBtnMouseOver(lnk) {
                                    document.getElementById(lnk).style.color = "SteelBlue";
                                }
                                function LinkBtnMouseOut(lnk) {
                                    document.getElementById(lnk).style.color = "SteelBlue";
                                }
                            </script>
                        </div>
                    </div>

                    <div class="row border-LeftRight">
                        <div class="col-md-12 col-sm-12 PaddingSpacing">
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop01">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:PRegistration, firstname %>"></asp:Literal><span style="color: Red">&nbsp;*&nbsp; </span>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtfirstname" runat="server" AutoPostBack="False" CssClass="drapDrowHeight" MaxLength="150" Width="100%"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" Enabled="True" FilterType="Custom, LowercaseLetters , UppercaseLetters" TargetControlID="txtfirstname" ValidChars="1234567890/ "></AJAX:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" runat="server" ControlToValidate="txtfirstname" ValidationGroup="save" Display="None" ErrorMessage="Enter First Name">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop01">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:PRegistration, middlename %>"></asp:Literal>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtmiddlename" runat="server" MaxLength="50" CssClass="drapDrowHeight" Width="100%"></asp:TextBox><AJAX:FilteredTextBoxExtender
                                            ID="FilteredTextBoxExtender30" runat="server" Enabled="True" FilterType="Custom, LowercaseLetters , UppercaseLetters"
                                            TargetControlID="txtmiddlename" ValidChars="1234567890 ">
                                        </AJAX:FilteredTextBoxExtender>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop01">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:PRegistration, lastname %>"></asp:Literal>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtlastname" runat="server" MaxLength="50" CssClass="drapDrowHeight" Width="100%"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                            FilterType="Custom, LowercaseLetters , UppercaseLetters" TargetControlID="txtlastname"
                                            ValidChars="1234567890 ">
                                        </AJAX:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-sm-12 PaddingSpacing">
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 PaddingRightSpacing label2">
                                        <span class="textName">
                                            <asp:Label ID="Label2" runat="server" Text="Employment Status"></asp:Label><span style="color: Red"> *</span></span>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:DropDownList ID="ddlEmploymentstatus" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="employmentvalidation" runat="server" ControlToValidate="ddlEmploymentstatus"
                                            InitialValue="0" Display="None" ErrorMessage="Select Employment Status" ValidationGroup="save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <span class="textName">
                                            <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:PRegistration, employeetype %>"></asp:Literal></span>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:UpdatePanel ID="UpdEmptypePanel" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadComboBox ID="ddlemployeetype" runat="server" CssClass="drapDrowHeight" AutoPostBack="true" Width="100%"
                                                    OnSelectedIndexChanged="ddlemployeetype_OnSelectedIndexChanged">
                                                </telerik:RadComboBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <asp:Label ID="Label9" runat="server" Text="Lab Station"></asp:Label>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadComboBox ID="ddlStation" runat="server" CssClass="drapDrowHeight" EmptyMessage="[Select]" AllowCustomText="true"
                                                    DropDownWidth="200px" Width="100%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStationId" runat="server" Text='<%#Eval("StationName") %>' />
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-sm-12 PaddingSpacing">
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Label ID="lblEducation" runat="server" Text="Education"></asp:Label>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtEducation" runat="server" CssClass="drapDrowHeight" MaxLength="98" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:PRegistration, department %>" Visible="true"></asp:Literal>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:DropDownList ID="ddldepartment" runat="server" CssClass="drapDrowHeight" Width="100%" Visible="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <span class="textName">
                                            <asp:Label ID="lblDiscountAuthorised" runat="server" Text="Discount % Authorised"></asp:Label></span>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtDiscountPercentAuthorised" MaxLength="5" runat="server" CssClass="drapDrowHeight"
                                            Width="100%"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="fte1" runat="server" FilterType="Custom,Numbers"
                                            ValidChars="1234567890." TargetControlID="txtDiscountPercentAuthorised">
                                        </AJAX:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-sm-12 PaddingSpacing">
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2 PaddingRightSpacing">
                                        <span class="textName">
                                            <asp:Label ID="lblInteractionPlausibility" CssClass="drapDrowHeight" runat="server" Text="Interaction Plausibility"></asp:Label></span>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:DropDownList ID="ddlAllergyPlausibility" runat="server" Width="100%" CssClass="drapDrowHeight">
                                            <asp:ListItem Text="Select" Value="0" />
                                            <asp:ListItem Text="Major Only" Value="1" />
                                            <asp:ListItem Text="Moderate and Major" Value="2" />
                                            <asp:ListItem Text="Major, Moderate and Minor" Value="3" />
                                            <asp:ListItem Text="No Allert" Value="4" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">

                                    <div class="col-md-5 col-sm-6 label2">
                                        <span class="textName">
                                            <asp:Label ID="lblMedicationSeverity" runat="server" Text="Medication Severity"></asp:Label></span>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:DropDownList ID="ddlMedicationSeverity" runat="server" Width="100%" CssClass="drapDrowHeight">
                                            <asp:ListItem Text="Select" Value="0" />
                                            <asp:ListItem Text="Major Only" Value="1" />
                                            <asp:ListItem Text="Moderate and Major" Value="2" />
                                            <asp:ListItem Text="Major, Moderate and Minor" Value="3" />
                                            <asp:ListItem Text="No Allert" Value="4" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:DropDownList ID="ddlstatus" runat="server" Width="100%" CssClass="drapDrowHeight">
                                            <asp:ListItem Selected="True" Text="Active" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-sm-12 PaddingSpacing">
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Label ID="Label11" runat="server" Text="Designation" />
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="drapDrowHeight" MaxLength="50" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Label ID="Label12" runat="server" Text="Room No." />
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtRoomNo" runat="server" CssClass="drapDrowHeight" MaxLength="50" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Label ID="lbldoj" runat="server" Text="Date of Joining" />
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <telerik:RadDatePicker ID="rdpDateFrom" runat="server" MinDate="01/01/1900" Width="100%" DateInput-DateFormat="dd/MM/yyyy"
                                            TabIndex="37" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-6 label2">
                                        <asp:Label ID="lbldol" runat="server" Text="Date Of Leaving" />
                                    </div>
                                    <div class="col-md-7 col-sm-6">
                                        <telerik:RadDatePicker ID="rdpDateto" runat="server" MinDate="01/01/1900" Width="100%" DateInput-DateFormat="dd/MM/yyyy"
                                            TabIndex="37" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>
                </div>
            </div>



        </div>




        <div class="container-fluid header_mainGray margin_z margin_bottomNone borderBottom">
            <div class="col-md-4 col-sm-4">
                <h2>Mailing Address</h2>
            </div>
            <div class="col-md-4 col-sm-4">
                <h2>Visit Detail</h2>
            </div>
            <div class="col-md-4 col-sm-4">
                <h2>&nbsp;&nbsp;Other Permissions</h2>
            </div>
        </div>





        <asp:Panel ID="pnlPatientAddress" runat="server">
            <div class="container-fluid border-LeftRight">
                <div class="row">
                    <div class="col-md-4 col-sm-4">
                        <div class="relativ">
                            <div style="position: absolute; right: -16px; width: 1px; min-height: 101%; max-height: 111%; background: #ccc;"></div>

                            <div class="row form-groupTop02" style="display: none;">
                                <div class="col-md-3 col-sm-4 label2">
                                    <asp:Literal ID="Literal23" runat="server" Visible="false" Text="&nbsp;&nbsp;Address" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <asp:TextBox ID="txtLAddress" MaxLength="250" runat="server" Visible="false" Text="" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row PaddingSpacing02">
                                <div class="col-md-3 col-sm-4 label2">
                                    <asp:Literal ID="Literal4" runat="server" Text="&nbsp;&nbsp;Address" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <asp:TextBox ID="txtLAddress2" MaxLength="250" runat="server" Text="" Height="45px" Columns="75" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-groupTop03">
                                <div class="col-md-3 col-sm-4">
                                    <asp:Literal ID="Literal26" runat="server" Text="&nbsp;&nbsp;Country" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <asp:DropDownList ID="ddlcountry" runat="server" AppendDataBoundItems="true" Width="100%" CssClass="drapDrowHeight" AutoPostBack="true" OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged"></asp:DropDownList>
                                    <%--<asp:UpdatePanel ID="UPDCOuntry" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlcountry" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlstate" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlcity" />
                                    </Triggers>
                                    <ContentTemplate>--%>
                                    <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-3 col-sm-4 label2">
                                    <asp:Literal ID="Literal27" runat="server" Text="&nbsp;&nbsp;State" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <%-- <asp:UpdatePanel ID="UPDState" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlcountry" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlstate" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlcity" />
                                        </Triggers>
                                        <ContentTemplate>--%>
                                    <asp:DropDownList ID="ddlstate" runat="server" AutoPostBack="true" CssClass="drapDrowHeight"
                                        AppendDataBoundItems="true" Width="100%" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged">
                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>

                            <div class="row form-groupTop03">
                                <div class="col-md-3 col-sm-4 label2">
                                    <asp:Literal ID="Literal28" runat="server" Text="&nbsp;&nbsp;City" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <%--<asp:UpdatePanel ID="UPDCity" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlcountry" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlstate" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlcity" />
                                    </Triggers>
                                    <ContentTemplate>--%>
                                    <asp:DropDownList ID="ddlcity" runat="server" CssClass="drapDrowHeight"
                                        OnSelectedIndexChanged="LocalCity_OnSelectedIndexChanged" AutoPostBack="true"
                                        AppendDataBoundItems="true" Width="100%">
                                        <asp:ListItem Text="[Select]" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </div>
                            </div>

                            <div class="row form-groupTop03">
                                <div class="col-md-3 col-sm-4 label2">
                                    <asp:Literal ID="Literal2" runat="server" Text="&nbsp;&nbsp;Zip" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <div class="row">
                                        <div class="col-md-5 col-sm-6 PaddingRightSpacing">
                                            <asp:TextBox ID="txtPin" runat="server" Text="" CssClass="drapDrowHeight" Width="100%" MaxLength="10"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="PinFilteredTextBoxExtender" runat="server" Enabled="True"
                                                TargetControlID="txtPin" FilterType="Custom, Numbers">
                                            </AJAX:FilteredTextBoxExtender>
                                            <%-- <asp:UpdatePanel ID="UPDZip" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlcountry" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlstate" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlcity" />
                                            </Triggers>
                                            <ContentTemplate>--%>
                                            <asp:DropDownList ID="ddlZip" runat="server" Width="100%" CssClass="drapDrowHeight"
                                                AutoPostBack="true" AppendDataBoundItems="true" TabIndex="19" Visible="false">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 col-sm-6 text-left">
                                            <asp:Button ID="btnZipSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnZipSearch_OnClick" />
                                            <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                        </div>
                                        <div class="col-md-4 col-sm-12 pull-right PaddingRightSpacing">
                                            <div class="PD-TabRadio margin_z textName">
                                                <asp:CheckBox ID="chkLocked" runat="server" Text=" IsLocked" Font-Size="11px" />
                                                <asp:ValidationSummary ID="vs1" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                    ValidationGroup="save" />
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>

                            <div class="row form-groupTop03">
                                <div class="col-md-3 col-sm-4 label2">
                                    <asp:Literal ID="Literal29" runat="server" Text="&nbsp;&nbsp;Home" />
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <div class="row">
                                        <div class="col-md-5 col-sm-4 PaddingRightSpacing">
                                            <asp:TextBox ID="txtPhoneHome" MaxLength="30" CssClass="drapDrowHeight" runat="server" Width="100%" Columns="30"></asp:TextBox>
                                            <%--<AJAX:FilteredTextBoxExtender ID="PhoneFilteredTextBoxExtender" runat="server" Enabled="True"
                                                TargetControlID="txtPhoneHome" FilterType="Custom, Numbers">
                                            </AJAX:FilteredTextBoxExtender>--%>
                                        </div>
                                        <div class="col-md-2 col-sm-3">
                                            <asp:Literal ID="Literal43" runat="server" Text="Mobile" />
                                        </div>
                                        <div class="col-md-5 col-sm-5">
                                            <asp:TextBox ID="txtMobile" MaxLength="30" CssClass="drapDrowHeight" runat="server" Width="100%" Columns="30"></asp:TextBox>
                                            <%--<AJAX:FilteredTextBoxExtender ID="MobileFilteredTextBoxExtender" runat="server" Enabled="True"
                                            TargetControlID="txtMobile" FilterType="Custom, Numbers">
                                        </AJAX:FilteredTextBoxExtender>--%>
                                        </div>
                                    </div>


                                </div>
                            </div>

                            <div class="row form-groupTop03">
                                <div class="col-md-3 col-sm-4 label2">
                                    <span class="textName">
                                        <asp:Literal ID="Literal44" runat="server" Text="&nbsp;&nbsp;E-Mail ID" /></span>
                                </div>
                                <div class="col-md-9 col-sm-8">
                                    <asp:TextBox ID="txtEmailID" MaxLength="50" runat="server" CssClass="drapDrowHeight" Width="100%" Columns="30"></asp:TextBox>
                                    <%--<asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtEmailID"
                                    ValidationGroup="save" SetFocusOnError="true" ErrorMessage="Invalid Email ID Format"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
                                    <span style="float: left; margin: 0; padding: 0; width: 100%; font-size: 9px; text-align: right; height: 8px;">
                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtEmailID"
                                            ValidationGroup="save" SetFocusOnError="true" ErrorMessage="Invalid Email ID Format"
                                            ValidationExpression="(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*(;|,)\s*|\s*$))*"></asp:RegularExpressionValidator>
                                    </span>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4">
                        <div class="relativ">
                            <asp:UpdatePanel ID="updVisit" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div style="position: absolute; right: -16px; width: 1px; min-height: 103%; max-height: 111%; background: #ccc;"></div>

                                    <div class="row form-groupTop01">
                                        <div class="col-md-4 col-sm-6 label2">
                                            <asp:Literal ID="Literal45" runat="server" Text="First Visit" />
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="ddlFirstVisit" runat="server" CssClass="drapDrowHeight" AppendDataBoundItems="true" AutoPostBack="false"
                                                Width="100%">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2">
                                            <asp:Literal ID="Literal46" runat="server" Text="Second Visit" />
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="ddlFollowupVisit" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="false" Width="100%" CssClass="drapDrowHeight">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2">
                                            <asp:Literal ID="Literal47" runat="server" Text="Free Visit" />
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="ddlFreeVisit" runat="server" CssClass="drapDrowHeight" AppendDataBoundItems="true" AutoPostBack="false" Width="100%">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2">
                                            <span class="textName">
                                                <asp:Literal ID="Literal10" runat="server" Text="Other First Visit" /></span>
                                        </div>
                                        <div class="col-md-8 col-sm-6">

                                            <asp:DropDownList ID="ddlOtherFirstVisit" CssClass="drapDrowHeight" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="false" Width="100%">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2">
                                            <asp:Literal ID="Literal48" runat="server" Text="IP First Visit" />
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="ddlIpFirstvisit" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="false" Width="100%" CssClass="drapDrowHeight">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2">
                                            <span class="textName">
                                                <asp:Literal ID="Literal49" runat="server" Text="IP Second Visit" /></span>
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="ddlIPSecondVisit" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="false" Width="100%" CssClass="drapDrowHeight">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing">
                                            <span class="textName">
                                                <asp:Literal ID="Literal12" runat="server" Text="IP 1rd Emergency Visit" /></span>
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="DropFEmergencyVisit" runat="server" CssClass="drapDrowHeight" AppendDataBoundItems="true"
                                                AutoPostBack="false" Width="100%">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row form-groupTop03">
                                        <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing">
                                            <span class="textName">
                                                <asp:Literal ID="Literal11" runat="server" Text="IP 2nd Emergency Visit" /></span>
                                        </div>
                                        <div class="col-md-8 col-sm-6">
                                            <asp:DropDownList ID="DropSEmergencyVisit" runat="server" CssClass="drapDrowHeight" AppendDataBoundItems="true"
                                                AutoPostBack="false" Width="100%">
                                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4" style="height: 195px; overflow: auto;">
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12">
                                <div class="col-md-6 col-sm-6 PaddingRightSpacing">
                                    <div class="row">
                                        <div class="PD-TabRadioNew02">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkResource" runat="server" CssClass="Margin_y" Text="Resource" Width="100%" />
                                                    <asp:CheckBox ID="chkAccessAllResource" runat="server" CssClass="Margin_y" Text="Access All Resource" Width="100%" />
                                                    <asp:CheckBox ID="chkAccessSpecialisationResource" runat="server" CssClass="Margin_y" Text="Access Specialisation Resource" Width="100%" />
                                                    <asp:CheckBox ID="chkIsAccessAllEncounter" runat="server" CssClass="Margin_y" Text="Access All Encounter" Width="100%" />
                                                    <asp:CheckBox ID="chkCopyClinicalCaseshet" runat="server" CssClass="Margin_y" Text="Copy Clinical Case Sheet" Width="100%" />
                                                    <asp:CheckBox ID="chkPrintCaseSheet" runat="server" CssClass="Margin_y" Text="Print Case Sheet" Width="100%" />
                                                    <asp:CheckBox ID="chkIsEMRSuperUser" runat="server" CssClass="Margin_y" Text="Is EMR Super User" Width="100%" />
                                                    <asp:CheckBox ID="chkIsPrintOrignalReceipt" runat="server" CssClass="Margin_y" Text="Is Print Original OP/IP Receipt" Width="100%" />
                                                    <asp:CheckBox ID="chkSendOrders" runat="server" CssClass="Margin_y" Text="Send Orders" Width="100%" />
                                                    <asp:CheckBox ID="chkPrescribeMedication" runat="server" CssClass="Margin_y" Text="Prescribe Medication" Width="100%" />
                                                    <asp:CheckBox ID="chkEncounterReOpen" runat="server" CssClass="Margin_y" Text="Encounter Re-Open" Width="100%" />
                                                    <asp:CheckBox ID="chkIsMultipleFirstVisit" runat="server" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="chkIsMultipleFirstVisit_OnCheckedChanged" CssClass="Margin_y" Text="Multiple&nbsp;FirstVisit" Width="100%" />
                                                    <asp:CheckBox ID="chkAllowPanelExcludItems" runat="server" TextAlign="Right" Text="Allow Panel Excluded Items" Width="100%" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkIsMultipleFirstVisit" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 col-sm-6 PaddingRightSpacing">
                                    <div class="row">
                                        <div class="PD-TabRadioNew02">
                                            <asp:UpdatePanel ID="updChek" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkIsMedicalProvider" CssClass="Margin_y" runat="server" Text="Appointment Schedule"
                                                        AutoPostBack="true" OnCheckedChanged="chkIsMedicalProvider_OnCheckedChanged" Width="100%" />
                                                    <asp:CheckBox ID="chkIsNoAppointmentAllowBeyondTime" runat="server" Text="No Appointment Beyond Slots" Width="100%" />
                                                    <asp:CheckBox ID="chkMultipleAppointment" runat="server" CssClass="Margin_y" Text="Multiple Appointment Per Slot" Width="100%" />
                                                    <asp:CheckBox ID="Chkopdprovider" runat="server" CssClass="Margin_y" Text="Providing OP Service" Width="100%" />
                                                    <asp:CheckBox ID="Chkipdprovider" runat="server" CssClass="Margin_y" Text="Providing IP Service" Width="100%" />
                                                    <asp:CheckBox ID="chkCanDownloadPatientDocument" CssClass="Margin_y" runat="server" Text="Download Patient Document" Width="100%" />
                                                    <asp:CheckBox ID="chkEmail" runat="server" CssClass="Margin_y" Text="Is EMail Patient Lab Report" Width="100%" />
                                                    <asp:CheckBox ID="chkAdmissionAuthorised" CssClass="Margin_y" runat="server" Text="Admission Authorised" Width="100%" />
                                                    <asp:CheckBox ID="chkRefundAuthorizationAboveMaxLimit" CssClass="Margin_y" runat="server" Text="Refund Above Max Limit" Width="100%" />
                                                    <asp:CheckBox ID="chkIncludeForDischargeSummary" runat="server" CssClass="Margin_y" Text="Include For Discharge Summary" Width="100%" />
                                                    <asp:CheckBox ID="chkDecisionSupport" runat="server" TextAlign="Right" CssClass="Margin_y" Text="Decision&nbsp;Support" Width="100%" />
                                                    <asp:CheckBox ID="chkCareNotification" runat="server" TextAlign="Right" CssClass="Margin_y" Text="Care&nbsp;Notification" Width="100%" />
                                                    <asp:CheckBox ID="chkDose" runat="server" TextAlign="Right" CssClass="Margin_y" Text="Check&nbsp;Dose" Width="100%" />

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkIsMedicalProvider" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </asp:Panel>


        <div class="container-fluid header_mainGray margin_z margin_bottomNone borderBottom">
            <div class="col-md-3 col-sm-3">
                <h2>Login Details</h2>
            </div>
            <div class="col-md-3 col-sm-3">
                <h2>Select User Group <span style="color: red;">*</span></h2>
            </div>
            <div class="col-md-3 col-sm-3">
                <h2>Select Facility <span style="color: red;">*</span></h2>
            </div>
            <div class="col-md-3 col-sm-3">
                <h2>Select Entry Site <span style="color: red;">*</span></h2>
            </div>
        </div>



        <div class="container-fluid border-LeftRight">
            <div class="row form-group">

                <div class="col-md-3 col-sm-3">
                    <asp:UpdatePanel ID="up1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAvailability" />
                        </Triggers>
                        <ContentTemplate>

                            <div class="row form-groupTop03">
                                <div class="col-md-12 col-sm-12">
                                    <div class="PD-TabRadioNew02">
                                        <asp:CheckBox ID="chkUpdateLogin" runat="server" Text="Update credentials" OnCheckedChanged="chkUpdateLogin_Click" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlUserCredentaila" runat="server">

                                <div class="row form-groupTop01">
                                    <div class="col-md-5 col-sm-12 label2">
                                        <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label><span style="color: red;">*</span>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <div class="row">
                                            <div class="col-md-7 col-sm-7 PaddingRightSpacing01">
                                                <asp:TextBox ID="txtUserName" runat="server" Width="100%" CssClass="drapDrowHeight" Columns="28" MaxLength="50"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5 col-sm-5 PaddingSpacing">
                                                <asp:Button ID="btnAvailability" runat="server" CssClass="btn btn-primary" Text="Availability"
                                                    ValidationGroup="username" OnClick="btnAvailability_Click" ToolTip="Check User Availability" /><br />
                                                <asp:Label ID="lblAvailabilityMessage" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" runat="server"
                                        ValidationGroup="save" ControlToValidate="txtUserName" Display="None" ErrorMessage="Enter User Name">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2">
                                        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label><span style="color: red;">*</span>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:TextBox ID="txtPassword" runat="server" Columns="28" Width="100%" TextMode="Password"
                                            EnableViewState="false" MaxLength="100" ToolTip="Password" CssClass="drapDrowHeight"></asp:TextBox>
                                        <asp:HiddenField ID="hdnPassword" runat="server" />
                                        <%-- <telerik:RadToolTip ID="RadToolTip1" runat="server" EnableShadow="true" RelativeTo="Element"
                                        Height="40px" Width="300px" TargetControlID="txtPassword" BackColor="#FFFFD5"
                                        Position="TopCenter">
                                    </telerik:RadToolTip>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="None" ControlToValidate="txtPassword"
                                            ValidationGroup="save" runat="server" ErrorMessage="Enter Password"></asp:RequiredFieldValidator>
                                        <%--<asp:RegularExpressionValidator ID="regexpName" Display="None" runat="server" ErrorMessage="Password is not in correct format."
                                        ValidationGroup="save" ControlToValidate="txtPassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^*&+=]).*$"></asp:RegularExpressionValidator>
                                    <AJAX:PasswordStrength ID="PasswordStrength1" DisplayPosition="BelowLeft" CalculationWeightings="50;15;15;20"
                                        TextStrengthDescriptions="Weak;Average;Strong;Excellent" TargetControlID="txtPassword"
                                        MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                                        MinimumUpperCaseCharacters="1" runat="server">
                                    </AJAX:PasswordStrength>--%>
                                        <%--<asp:Button ID="btnpasshelp" runat="server" Text="?" ToolTip="Password"
                                        CausesValidation="false" SkinID="Button" />--%>
                                        <%--<telerik:RadToolTip ID="RadToolTip5" runat="server" EnableShadow="true" RelativeTo="Element"
                                        Height="40px" Width="300px" TargetControlID="btnpasshelp" BackColor="#FFFFD5"
                                        Position="TopCenter">
                                    </telerik:RadToolTip>--%>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2 PaddingRightSpacing">
                                        <span class="textName">
                                            <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label><span style="color: red;">*</span></span>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:TextBox ID="txtConfirmPassword" EnableViewState="false" runat="server" Width="100%" CssClass="drapDrowHeight"
                                            Columns="28" TextMode="Password" MaxLength="100" ToolTip="Password"></asp:TextBox>
                                        <%--<telerik:RadToolTip ID="RadToolTip2" runat="server" EnableShadow="true" RelativeTo="Element"
                                        Height="40px" Width="300px" TargetControlID="txtConfirmPassword" BackColor="#FFFFD5"
                                        Position="TopCenter">
                                    </telerik:RadToolTip>--%>
                                    </div>
                                    <asp:CompareValidator ID="CompareValidator2" ValidationGroup="save" runat="server"
                                        ErrorMessage="Password and Confirm Password Should be Same" Display="None" ControlToValidate="txtConfirmPassword"
                                        ControlToCompare="txtPassword" SetFocusOnError="true" Operator="Equal"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" SetFocusOnError="true" runat="server"
                                        ControlToValidate="txtConfirmPassword" ValidationGroup="save" Display="None"
                                        ErrorMessage="Enter Confirm Password"></asp:RequiredFieldValidator>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2 label2">
                                        <span class="textName">
                                            <asp:Label ID="Label5" runat="server" Text="Hint Questions"></asp:Label></span>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:DropDownList ID="ddlHintQuestions" Width="100%" runat="server" CssClass="drapDrowHeight">
                                            <asp:ListItem Value="0" Text="--Select Hint Question--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="What is your father's middle name"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="What is your mother's maiden name"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="What is name of your first school"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Which year did you finish school"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="What is exact time of your birth"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="What is your favourite pastime"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="What is your favourite movie"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Which is your favourite restaurant"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="What is your favourite food"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="What is your favourite song"></asp:ListItem>
                                            <asp:ListItem Value="11" Text="What is your favourite color"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" runat="server"
                                        ControlToValidate="ddlHintQuestions" ValidationGroup="save" Display="None" InitialValue="0"
                                        ErrorMessage="Select Hint Question"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2 label2">
                                        <span class="textName">
                                            <asp:Label ID="Label6" runat="server" Text="Or Create Other"></asp:Label></span>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:TextBox ID="txtOtherHintQuestions" runat="server" CssClass="drapDrowHeight" Width="100%" Columns="28" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2">
                                        <asp:Label ID="Label7" runat="server" Text="Hint Answer"></asp:Label>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:TextBox ID="txtHintAnswer" runat="server" Width="100%" CssClass="drapDrowHeight" Columns="28" MaxLength="200"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2">Default URL</div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:DropDownList ID="ddldfurl" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row form-groupTop01">
                                    <div class="col-md-5 col-sm-12 label2 label2">Default Page</div>
                                    <div class="col-md-7 col-sm-12">
                                        <telerik:RadComboBox ID="ddldefaultpage" Width="100%" runat="server" AppendDataBoundItems="true"
                                            DataTextField="PageTitle" DataValueField="PageId" CssClass="drapDrowHeight" DataSourceID="sqDSource">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="sqDSource" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                                            SelectCommandType="Text" SelectCommand="select PageID, pageTitle from secmodulepages where homepage=1"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2 label2">
                                        <span class="textName01">
                                            <asp:Label ID="lblExpiryPeriod" runat="server" Text="Expiry Period"></asp:Label><span style="color: red;">*</span></span>
                                    </div>
                                    <div class="col-md-7 col-sm-12">
                                        <asp:UpdatePanel ID="upexpirypriod" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6">
                                                        <asp:TextBox ID="txtExpiryPeriod" runat="server" CssClass="drapDrowHeight" MaxLength="3" Width="100%"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 PaddingLeftSpacing label2">
                                                        <asp:Literal ID="day" runat="server" Text="&nbsp;(Days)"></asp:Literal>
                                                    </div>
                                                </div>


                                                <AJAX:FilteredTextBoxExtender ID="AjaxFilternum" runat="server" Enabled="true" TargetControlID="txtExpiryPeriod"
                                                    FilterType="Numbers">
                                                </AJAX:FilteredTextBoxExtender>
                                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" runat="server"
                                            ControlToValidate="txtExpiryPeriod" ValidationGroup="save" Display="None" ErrorMessage="Enter Expiry Period">*</asp:RequiredFieldValidator>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-5 col-sm-12 label2 label2 PaddingRightSpacing">
                                        <span class="textName">
                                            <asp:Label ID="Label8" runat="server" Text="Expiry Notification"></asp:Label><span style="color: red;">*</span></span>
                                    </div>
                                    <div class="col-md-7 col-sm-12 PaddingLeftSpacing">
                                        <asp:UpdatePanel ID="upExpityNotification" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-6 col-sm-6">
                                                    <asp:TextBox ID="txtExpirynotification" runat="server" CssClass="drapDrowHeight" MaxLength="3" Width="112%"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6 col-sm-6 PaddingLeftSpacing label2">
                                                    <asp:Literal ID="Literal9" runat="server" Text="&nbsp;&nbsp;(Days)"></asp:Literal>
                                                </div>
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true"
                                                    TargetControlID="txtExpirynotification" FilterType="Numbers">
                                                </AJAX:FilteredTextBoxExtender>
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" runat="server"
                                            ControlToValidate="txtExpirynotification" ValidationGroup="save" Display="None" ErrorMessage="Enter Expiry Notification">*</asp:RequiredFieldValidator>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="row form-groupTop03">
                                    <div class="col-md-4 col-sm-12"></div>
                                    <div class="col-md-8 col-sm-12 PaddingRightSpacing">
                                        <div class="PD-TabRadio margin_z">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtExpiryPeriod" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtExpirynotification" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkDoNotExpire" runat="server" Text=" Not expire password"
                                                        Checked="false" AutoPostBack="true" OnCheckedChanged="chkDoNotExpire_OnCheckedChanged" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="update12" runat="server">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Pin, Move, Close, Maximize, Resize" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                            Behaviors="Close,Move,Pin,Resize,Maximize">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="col-md-9 col-sm-9">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 border-LeftRight">
                            <div class="row">

                                <div class="col-md-4 col-sm-4 SelectHeight">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBoxList ID="cblUserGroup" runat="server" Font-Bold="false" RepeatDirection="Vertical"></asp:CheckBoxList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <div class="col-md-4 col-sm-4 SelectHeight01">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBoxList ID="cblfacility" runat="server" Font-Bold="false" RepeatDirection="Vertical"></asp:CheckBoxList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:TextBox ID="txtImageId" runat="server" Style="visibility: hidden;" Text=""></asp:TextBox>
                                </div>

                                <div class="col-md-4 col-sm-4 SelectHeight">
                                    <asp:UpdatePanel ID="upEntrySite" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBoxList ID="cbEntrySite" runat="server" Font-Bold="false" RepeatDirection="Vertical"></asp:CheckBoxList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>

            </div>
        </div>

    </div>

</asp:Content>
