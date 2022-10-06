<%@ Page Language="C#" Theme="DefaultControls" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ProviderDetails.aspx.cs" Inherits="MPages_ProviderDetails"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        .textName {
            width: 165px;
            float: left;
        }

        @media (min-width: 768px) and (max-width: 991px) {
        }
    </style>



    <script language="JavaScript" type="text/javascript">
        function BindCombo(oWnd, args) {
            document.getElementById('<%=btnFillCombo.ClientID%>').click();
        }
    </script>

    <%--  <asp:UpdatePanel ID="UpdatePane1" runat="server">
        <ContentTemplate>--%>


    <div class="container-fluid header_main form-group">
        <div class="col-md-2 col-sm-3">
            <h2>Doctor Other Details</h2>
        </div>
        <div class="col-md-3 col-sm-3">
            <div class="row">
                <div class="col-md-3 col-sm-3">
                    <asp:Literal ID="ltrlDoctorName" runat="server" Text="Doctor"></asp:Literal><span style="color: Red">*</span></div>
                <div class="col-md-9 col-sm-9">
                    <telerik:RadComboBox ID="ddlDoctorName" runat="server" Width="100%" AllowCustomText="true"
                        Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDoctorName_SelectedIndexChanged">
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RFVddlDoctorName" runat="server" ErrorMessage="Enter Provider"
                        SetFocusOnError="true" ControlToValidate="ddlDoctorName" Display="None" InitialValue="0">
                    </asp:RequiredFieldValidator>
                    <asp:Button ID="btnFillCombo" runat="server" CausesValidation="false" Enabled="true"
                        OnClick="btnFillCombo_Click" SkinID="button" Style="visibility: hidden; float: left; margin: 0; padding: 0; height: 1px; width: 1px;" Text="Assign" />
                </div>
            </div>
        </div>
        <div class="col-md-7 col-sm-6 text-right PaddingLeftSpacing">
            <asp:Button ID="btnSaveDetails" CssClass="btn btn-primary pull-right" runat="server" Text="Save" OnClick="btnSaveDetails_Click" />
            <span id="DivMenu" runat="server" class="margin_Top pull-right">
                <asp:LinkButton ID="lnkEmployee" runat="server" CausesValidation="false" Text="Employee" CssClass="btnNew"
                    Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployee_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkEmployeeLookup" runat="server" CausesValidation="false" Text="Employee Look Up"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployeeLookup_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkAppointmentTemplate" runat="server" CausesValidation="false"
                    Text="Appointment Template" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                    onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkAppointmentTemplate_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkProviderProfile" runat="server" CausesValidation="false"
                    Text="Employee Profile" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                    onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkProviderProfile_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkClassification" runat="server" CausesValidation="false" CssClass="btnNew"
                    onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Classification" OnClick="lnkClassification_OnClick"></asp:LinkButton>
                <script language="JavaScript" type="text/javascript">
                    function LinkBtnMouseOver(lnk) {
                        document.getElementById(lnk).style.color = "SteelBlue";
                    }
                    function LinkBtnMouseOut(lnk) {
                        document.getElementById(lnk).style.color = "SteelBlue";
                    }
                </script>
            </span>
        </div>


    </div>


    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-12 col-sm-12 text-center">
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Green" Font-Bold="true" /></div>
        </div>
    </div>


    <div class="container-fluid">

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Specialization<span style="color: Red">*</span></div>
                    <div class="col-md-9 col-sm-7">
                        <div class="row">
                            <div class="col-md-11 col-sm-11 PaddingRightSpacing">
                                <%-- <asp:DropDownList ID="ddlSpecialisation" SkinID="DropDown" runat="server" Width="175px"></asp:DropDownList>--%>
                                <telerik:RadComboBox ID="ddlSpecialisation" runat="server" Filter="Contains" Width="95%"
                                    AutoPostBack="false">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RFVddlSpecialisation" runat="server" ErrorMessage="Enter Specialization"
                                    SetFocusOnError="true" ControlToValidate="ddlSpecialisation" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-1 col-sm-1 PaddingLeftSpacing margin_Top01">
                                <asp:ImageButton ID="ibtnSpecilalization" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                    ToolTip="Add New Specialization" Width="15px" OnClick="ibtnSpecilalization_onClick"
                                    Visible="true" CausesValidation="false" />
                            </div>
                        </div>


                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">NPI</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtNPI" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">UPIN</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtUPIN" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5 PaddingRightSpacing">Group Medicare</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtGroupMedicare" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">DEA</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtDEA" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5 PaddingRightSpacing">Taxonomy Code</div>
                    <div class="col-md-9 col-sm-7">
                        <telerik:RadComboBox ID="ddlTaxonomyCode" runat="server" AllowCustomText="true" MarkFirstMatch="true"
                            Width="100%" EmptyMessage="[ Select ]">
                        </telerik:RadComboBox>
                        <%--<asp:TextBox ID="txtTaxonomyCode" SkinID="textbox" MaxLength="50" Width="175px" runat="server"></asp:TextBox>--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">State Medicaid</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtStateMedicaid" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Champus</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtChampus" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">BCBS</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtBCBS" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Other Doctor 1</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtotherProvider1" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Other Doctor 2</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtotherProvider2" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Other Doctor 3</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtotherProvider3" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Medi Pass</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtMediPass" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">ChampVA</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtChampVA" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">FECA</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtFECA" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">State Lic</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtStateLic" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Speciality Lic</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtSpecialityLic" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">State Contr Lic</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtStateContrLic" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5 PaddingRightSpacing"><span class="textName">Business Name (HCFA 33)</span></div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtBusinessName" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Sure Script Id</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtSureScrptId" MaxLength="20" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5 PaddingRightSpacing"><span class="textName">Doctor Federal Tax Id</span></div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtProviderTaxId" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5 PaddingRightSpacing"><span class="textName">Surgery Doctor Classification</span></div>
                    <div class="col-md-9 col-sm-7">
                        <asp:DropDownList ID="ddlSurgeryDoctorTypeId" runat="server" Width="100%"></asp:DropDownList>
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Surgery Doctor Classification"
                                SetFocusOnError="true" ControlToValidate="ddlSurgeryDoctorTypeId" Display="None"
                                InitialValue="0">
                            </asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>

        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5 PaddingRightSpacing">Business Federal Tax Id</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtBusinessTaxId" MaxLength="50" Width="100%" runat="server"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 text-right">
                <div class="col-md-3 col-sm-0"></div>
                <div class="col-md-9 col-sm-12">
                    <div class="PD-TabRadioNew01 margin_z">
                        <asp:RadioButtonList ID="rblBusinessTaxIdType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="File as Practice" Value="Practice"></asp:ListItem>
                            <asp:ListItem Text="File as Provider" Value="Provider"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="PD-TabRadioNew01 margin_z">
                        <asp:RadioButtonList ID="rblProviderTaxIdType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="EIN" Value="EIN"></asp:ListItem>
                            <asp:ListItem Text="SSN" Value="SSN"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Physician Signature (HCFA 31)</div>
                    <div class="col-md-9 col-sm-7">
                        <div class="PD-TabRadioNew01 margin_z">
                            <asp:CheckBox Text="Signature on File" runat="server" ID="chkSignatureOnFile" /></div>
                        <div class="PD-TabRadioNew01 margin_z">
                            <asp:CheckBox Text="Signature With DateTime" runat="server" ID="chkSignatureWithDateTime" /></div>
                        <div class="PD-TabRadioNew01 margin_z">
                            <asp:CheckBox Text="Secondary Doctor Required" runat="server" ID="chkSecondaryDoctor" /></div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Doctor Notes</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtDoctorNotes" runat="server" Rows="3" TextMode="MultiLine" TabIndex="39" Width="100%" />
                        <asp:Button ID="ibtnpreviousnotes" runat="server" Text="Previous Notes" CssClass="btn btn-primary" ToolTip="Previous notes"
                            CausesValidation="false" Font-Size="7" Width="100px" OnClick="ibtnpreviousnotes_Click" />
                        <%--asp:Button ID="ibtnRemove" runat="server" Text="Remove" SkinID="button" ToolTip="Remove Image"
                                CausesValidation="false" Font-Size="7" Width="45px" />--%>


                        <asp:Label ID="lblBoolSignature" runat="server" Text="False" Visible="false"></asp:Label>
                        <asp:ValidationSummary ID="VSHospital" runat="server" ShowMessageBox="True" ShowSummary="False" />
                        <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Signature Line1</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtSignatureLine1" MaxLength="250" Width="100%" runat="server" /></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Signature Line2</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtSignatureLine2" MaxLength="250" Width="100%" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Signature Line3</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtSignatureLine3" MaxLength="100" Width="100%" runat="server" /></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Signature Line4</div>
                    <div class="col-md-9 col-sm-7">
                        <asp:TextBox ID="txtSignatureLine4" MaxLength="250" Width="100%" runat="server" /></div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-6 col-sm-6">
                <div class="row">
                    <div class="col-md-3 col-sm-5">Doctor Signature</div>
                    <div class="col-md-9 col-sm-7">
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12">
                                <asp:UpdatePanel ID="updRemoveImage" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Image ID="PatientImage" runat="server" ImageUrl="/Images/Signature.jpg" BorderWidth="1"
                                            BorderColor="Gray" Width="100%" Height="60px" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ibtnRemove" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 text-right">
                                <asp:Button ID="ibtnUpload" runat="server" Text="UpLoad" CssClass="btn btn-primary" ToolTip="Upload Image"
                                    CausesValidation="false" />
                                <asp:Button ID="ibtnRemove" runat="server" Text="Remove" CssClass="btn btn-primary" ToolTip="Remove Image"
                                    CausesValidation="false" OnClick="RemoveImage_OnClick" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6"></div>
        </div>



        <asp:Panel ID="pnlUpload" runat="server" Style="display: none" CssClass="modalPopup">
            <div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <p style="color: Black; font-weight: bold;">
                                            Add Signature
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
                            <asp:FileUpload ID="FileUploader" runat="server" Width="250px" />
                        </td>
                        <td>
                            <asp:Button ID="btnUpload" runat="server" Height="22px" Text="Upload" ValidationGroup="Upload"
                                OnClick="Upload_OnClick" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <AJAX:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="ibtnUpload"
            PopupControlID="pnlUpload" BackgroundCssClass="modalBackground" CancelControlID="ibtnClose"
            DropShadow="true" PopupDragHandleControlID="Panel3" />

    </div>



    <%--   
                <asp:LinkButton ID="lnkEmployeeDetails" runat="server" CausesValidation="false" Text="Employee Details"
                    Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployeeDetails_OnClick"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="lnkAppTemplate" runat="server" CausesValidation="false"
                    Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Appointment Template" OnClick="lnkAppTemplate_OnClick"></asp:LinkButton>

                <script language="JavaScript" type="text/javascript">
                                function LinkBtnMouseOver(lnk) {
                                    document.getElementById(lnk).style.color = "red";
                                }
                                function LinkBtnMouseOut(lnk) {
                                    document.getElementById(lnk).style.color = "blue";
                                }
                </script>
    --%>
    <%-- </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUpload" />
            <asp:AsyncPostBackTrigger ControlID="ibtnClose" />
            <asp:AsyncPostBackTrigger ControlID="ibtnUpload" />
            <asp:AsyncPostBackTrigger ControlID="ibtnRemove" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
