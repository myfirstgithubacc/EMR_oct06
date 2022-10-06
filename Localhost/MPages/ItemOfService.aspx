<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ItemOfService.aspx.cs" Inherits="MPages_ItemOfService" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        .textName {
            width: 150px;
            float: left;
        }

        #ctl00_ContentPlaceHolder1_ddlsubDept {
            width: 100% !important;
            float: left !important;
        }

        @media (min-width: 768px) and (max-width: 991px) {
        }
    </style>


    <script type="text/javascript" language="javascript">
        function fnActiveInActive(lblActiveInActive, chkStatus) {
            var field = document.getElementById(lblActiveInActive);
            var field1 = document.getElementById(chkStatus);
            if (field.innerText == 'Active') {
                field.innerText = 'Inactive';
                field1.checked = false;
            }
            else if (field.innerText == 'Inactive') {
                field.innerText = 'Active';
                field1.checked = true;
            }
            return true;
        }

        function fnOnMouseOver(lblActiveInActive) {
            document.getElementById(lblActiveInActive).style.color = "blue";
            document.getElementById(lblActiveInActive).style.cursor = "hand";
        }

        function fnOnMouseOut(lblActiveInActive) {
            document.getElementById(lblActiveInActive).style.color = "black";
        }

        function Visiblity() {
            var chk = document.getElementById('ctl00_ContentPlaceHolder1_chkpnl5SampleRequired');
            var fld1 = document.getElementById('ctl00_ContentPlaceHolder1_ddlpnl5SampleRequired');
            if (chk.checked) {
                fld1.disabled = false;
            }
            else {
                fld1.disabled = true;
            }
            return true;
        }
    </script>

    <asp:UpdatePanel ID="updpanelserviceitem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>


            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-3">
                    <h2>Service Information</h2>
                </div>
                <div class="col-md-6 col-sm-6 text-center">
                    <asp:Label ID="lblmsg" runat="server" Font-Bold="true"></asp:Label></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:Button ID="ibtnNew" runat="server" CssClass="btn btn-primary" OnClick="New_OnClick" ToolTip="New Data..."
                        Text="New" />
                    <%--   <asp:Button ID="ibtnEdit" runat="server"  CssClass="btn btn-primary" OnClick="Edit_OnClick"
                    ToolTip="Edit Selected Data..." Text="Edit" />
                    --%>
                    <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" OnClick="btnsave_OnClick"
                        ValidationGroup="save" Text="Save" />
                    <asp:ValidationSummary ID="vailidationsummary3" runat="server" ShowMessageBox="true"
                        ShowSummary="false" ValidationGroup="save" />
                    <asp:Button ID="ibtnUpdate" runat="server" CssClass="btn btn-primary" OnClick="UpdateItemOfService_OnClick"
                        ValidationGroup="save" Text="Update" />
                    <asp:HiddenField ID="hdnServiceid" runat="server" />
                </div>
            </div>



            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-12 col-sm-12 text-right">
                        <asp:LinkButton ID="lnkEmployeeLookup" runat="server" CausesValidation="false" Text="Service Look Up"
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="lnkEmployeeLookup_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkFacilityServiceFlagTagging" runat="server" CausesValidation="false"
                            Text="Flag Tagging" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                            onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkFacilityServiceFlagTagging_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Text="Service Template Tagging"
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="LinkButton1_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" Text="Surgery Component Tagging"
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="LinkButton2_OnClick"></asp:LinkButton>
                        <%--<asp:LinkButton ID="LinkButton5" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" OnClientClick="javascript:openRadWindow('ServiceTemplateTagging.aspx?Master=I',1000,610);return false;"
                                Text="Service Template Tagging" />--%>
                        <asp:LinkButton ID="lnkChargetype" runat="server" CausesValidation="false" Text="Charge Type"
                            CssClass="btnNew" Visible="false" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="lnkChargetype_OnClick"></asp:LinkButton>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>

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
            </div>



            <asp:Panel ID="pnlDataEntry" runat="server" ScrollBars="None" Width="100%">
                <div class="container-fluid">

                    <div class="row form-group">
                        <asp:UpdatePanel ID="Updatepanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlMain" runat="server" GroupingText="">
                                    <div class="col-md-4 col-sm-4">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6 label2">
                                                <asp:Literal ID="ltrlMainDept" runat="server" Text="Department"></asp:Literal></div>
                                            <div class="col-md-8 col-sm-6">
                                                <telerik:RadComboBox ID="ddlMainDept" runat="server" AllowCustomText="true"
                                                    MarkFirstMatch="true" Width="100%" OnSelectedIndexChanged="ddlMainDept_OnSelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing">
                                                <asp:Literal ID="ltrlSubDept" runat="server" Text="Sub Department "></asp:Literal></div>
                                            <div class="col-md-8 col-sm-6">
                                                <telerik:RadComboBox ID="ddlsubDept" runat="server" AllowCustomText="true"
                                                    MarkFirstMatch="true" Width="100%" OnSelectedIndexChanged="ddlsubDept_OnSelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4">
                                        <div class="row">
                                            <%--<div class="col-md-4 label2"> <asp:Label ID="lblItemOfService"  runat="server" Text="Service Name "></asp:Label><br /></div>--%>
                                            <%-- <div class="col-md-8"><asp:DropDownList ID="ddlItemOfService" width="200px" SkinID="DropDown" runat="server"></asp:DropDownList></div>--%>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6 label2">
                                                <asp:Literal ID="literlcpt" runat="server" Text="CPT Code"></asp:Literal></div>
                                            <div class="col-md-8 col-sm-6">
                                                <asp:TextBox ID="txtcptcode" runat="server" Width="100%" MaxLength="8"></asp:TextBox></div>
                                        </div>


                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                            <%--  <Triggers >
                                <asp:AsyncPostBackTrigger ControlID="ddlMainDept" />
                                <asp:AsyncPostBackTrigger ControlID="ddlsubDept" />
                                <asp:AsyncPostBackTrigger ControlID="ddlItemOfService" />
                                </Triggers>--%>
                        </asp:UpdatePanel>
                    </div>

                    <div class="row form-group">
                        <div class="col-md-8 col-sm-8">
                            <div class="row">
                                <div class="col-md-2 col-sm-3 label2">
                                    <asp:Literal ID="ltrlServiceName" runat="server" Text="Service Name"></asp:Literal><span style="color: Red">*</span></div>
                                <div class="col-md-10 col-sm-9">
                                    <asp:TextBox ID="txtServiceName" runat="server" Width="100%" MaxLength="245"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="Requiredfield1" runat="server" ControlToValidate="txtServiceName"
                                        Display="None" ValidationGroup="save" ErrorMessage="Enter Service Name" Text="Enter Service Name"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 PaddingRightSpacing label2"><span class="textName">
                                    <asp:Literal ID="ltrlServiceCode" runat="server" Text="Reference Service Code"></asp:Literal></span></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:TextBox ID="txtRefServiceCode" runat="server" Width="100%" MaxLength="10"></asp:TextBox></div>
                            </div>
                        </div>

                    </div>
                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2">
                                    <asp:Literal ID="Literal1" runat="server" Text="J CODE"></asp:Literal></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:TextBox ID="txtJCode" Width="100%" runat="server" MaxLength="8"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2">
                                    <asp:Label ID="lblAccountId" runat="server" Text="FA AccountId" /></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:TextBox ID="txtFAAccountId" runat="server" Width="100%" MaxLength="8"></asp:TextBox>
                                    <AJAX:FilteredTextBoxExtender ID="FtbtxtFAAccountId" runat="server" FilterType="Numbers"
                                        ValidChars="1234567890." TargetControlID="txtFAAccountId">
                                    </AJAX:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2">
                                    <asp:Literal ID="Literal2" runat="server" Text="LONIC Code"></asp:Literal></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:TextBox ID="txtLONICCode" Width="100%" runat="server" MaxLength="10"></asp:TextBox></div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing">
                                    <asp:Label ID="Label1" runat="server" Text="Report Tagging" /></div>
                                <div class="col-md-8 col-sm-6">
                                    <telerik:RadComboBox ID="ddlReportTagging" runat="server" Width="100%"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2"><span class="textName">
                                    <asp:Literal ID="Literal11" runat="server" Text="Consultation Type"></asp:Literal></span></div>
                                <div class="col-md-8 col-sm-6">
                                    <telerik:RadComboBox ID="ddlConsultationType" runat="server" Width="100%"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2"><span class="textName">
                                    <asp:Label ID="lblMHCReportFormatId" runat="server" Text="Default MHC Format" /></span></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:DropDownList ID="ddlMHCReportFormatId" runat="server" SkinID="DropDown" Width="100%" /></div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2">
                                    <asp:Label ID="Label4" runat="server" Text="Applicable For" /></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:DropDownList ID="ddlApplicableFor" runat="server" Width="100%">
                                        <asp:ListItem Selected="True" Text="Both" Value="B" />
                                        <asp:ListItem Text="OP" Value="O" />
                                        <asp:ListItem Text="IP" Value="I" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2">
                                    <asp:Label ID="lblAccounttype" runat="server" Text="Activity Type" /></div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:DropDownList ID="ddlAccountType" runat="server" Width="100%">
                                        <asp:ListItem Selected="True" Text="Service Code" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="CPT" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="HCPCS" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Dental" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6">
                                    <asp:Literal ID="ltrlStatus" runat="server" Text="Status"></asp:Literal></div>
                                <div class="col-md-8 col-sm-6 margin_Top01">
                                    <div class="PD-TabRadioNew01 margin_z">
                                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="0" AutoPostBack="true">
                                            <asp:ListItem Text="Active" Value="1" ></asp:ListItem>
                                            <asp:ListItem Text="In Active" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>

                        </div>
                            <div id="divSACCode" runat="server" class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 label2">
                                    <asp:Literal ID="Literal13" runat="server" Text="SAC Code"></asp:Literal>
                                    <span id="spnSACCode" runat="server" style="color: Red">*</span>
                                </div>
                                <div class="col-md-8 col-sm-6">
                                    <asp:TextBox ID="txtSACCode" runat="server" style="text-transform:uppercase" Width="100%" MaxLength="8"></asp:TextBox></div>
                            </div>
                        </div>
                        
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-md-8 col-sm-8">
                            <div class="row">
                                <div class="col-md-2 col-sm-3 label2"><span class="textName">
                                    <asp:Literal ID="lblTextRemarks" runat="server" Text="Bed Category Remarks"></asp:Literal></span></div>
                                <div class="col-md-10 col-sm-9">
                                    <asp:TextBox ID="txtTextRemarks" runat="server" Width="100%" Height="40px" TextMode="MultiLine" MaxLength="200"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Literal ID="lblIsExpressBillingExcluded" runat="server" Text="Is Express Billing Excluded"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                            <asp:CheckBox ID="chkIsExpressBillingExcluded" runat="server" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                     <div class="col-md-12 col-sm-12">
                                        <asp:Literal ID="lblisExcludedForOrdering" runat="server" Text="Is Excluded For Ordering"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                            <asp:CheckBox ID="chkisExcludedForOrdering" runat="server" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <%--<div class="col-md-4 col-sm-6"></div>--%>
                                <div class="col-md-12 col-sm-12">
                                    <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                        <asp:CheckBox ID="chkIsAutoInsertVisitCharges" runat="server" Text="Auto Insert Visit Charges (With Surgery)" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>



                </div>


                <asp:CheckBox runat="server" ID="chkremarkmanadatory" Text="Service Remark Mandatory" Visible="False" />


                <asp:Panel ID="pnlChkGrp1" runat="server" Width="100%" Height="100%">
                    <div class="container-fluid border-LeftRight">

                        <div class="row margin_Top" id="pnl1" runat="server">
                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Literal ID="ltrlpnl1PriceEditable" runat="server" Text="Price Editable"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                            <asp:CheckBox ID="chkpnl1PriceEditable" runat="server" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Literal ID="ltrlpnl1DoctorReq" runat="server" Text="Doctor Required"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                            <asp:CheckBox ID="chkpnl1DoctorReq" runat="server" Text="" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Literal ID="Literal3" runat="server" Text="Compulsary on Bill"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                            <asp:CheckBox ID="chkCompulsaryonBill" runat="server" Text="" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Literal ID="Literal14" runat="server" Text="Is Appointment Required"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z margin_Top01">
                                            <asp:CheckBox ID="ChkAppointmentRequired" runat="server" Text="" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-5 col-sm-6 label2 PaddingLeftSpacing"><span class="textName">
                                        <asp:Literal ID="Literal4" runat="server" Text="Resource Scheduling"></asp:Literal></span></div>
                                    <div class="col-md-7 col-sm-6">
                                        <asp:TextBox ID="txtTime" runat="server" MaxLength="2" Width="30px"></asp:TextBox>&nbsp;&nbsp;Minutes.
                                        <AJAX:FilteredTextBoxExtender ID="flt_txtTime" runat="server" FilterType="Numbers" TargetControlID="txtTime"></AJAX:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing"><span class="textName">
                                        <asp:Literal ID="Literal5" runat="server" Text="Estimated Price"></asp:Literal></span></div>
                                    <div class="col-md-8 col-sm-6">
                                        <asp:TextBox ID="txtEstimatedPrice" runat="server" MaxLength="5" Width="100%"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                            ValidChars="1234567890." TargetControlID="txtEstimatedPrice">
                                        </AJAX:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-6 label2">
                                        <asp:Literal ID="ltrlPnl3Classification" runat="server" Text="Classification"></asp:Literal></div>
                                    <div class="col-md-8 col-sm-6">
                                        <asp:DropDownList ID="ddlPnl3Classification" runat="server" Width="100%"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="requiredval3" runat="server" ControlToValidate="ddlPnl3Classification"
                                            InitialValue="0" ValidationGroup="save" Display="None" ErrorMessage="Select the surgery classification type to continue..."
                                            Text="Select the surgery classification type to continue..."></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3 col-sm-3"></div>

                            <div class="col-md-3 col-sm-3">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <asp:Literal ID="LtlServiceTax" runat="server" Text="Service Tax"></asp:Literal></div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtServiceTax" runat="server" Width="100%"></asp:TextBox></div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <asp:Literal ID="ltlVat" runat="server" Text="VAT"></asp:Literal></div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtVat" runat="server" Width="100%"></asp:TextBox></div>
                                </div>
                            </div>

                        </div>




                        <hr style="padding: 0; margin: 10px 0;" />



                        <div class="row" id="pnl5" runat="server">
                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing">
                                        <asp:Literal ID="ltrlpnl5ReportType" runat="server" Text="Report Type"></asp:Literal></div>
                                    <div class="col-md-8 col-sm-6">
                                        <asp:DropDownList ID="ddlpnl5ReportType" runat="server" Width="100%">
                                            <asp:ListItem Text="[ Select ]" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Numeric" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Profile" Value="P"></asp:ListItem>
                                            <asp:ListItem Text="Text" Value="T"></asp:ListItem>
                                            <asp:ListItem Text="Finding" Value="F"></asp:ListItem>
                                            <asp:ListItem Text="Special" Value="S"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- <asp:Button ID="btnReportFormat" SkinID="Button" runat="server" Text="Report Format" Width="88px" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-5 col-sm-6 label2"><span class="PD-TabRadioNew01 margin_z textName">
                                        <asp:CheckBox ID="chkpnl5SampleRequired" runat="server" Text="Sample Required" Checked="false" TextAlign="Left" /></span></div>
                                    <div class="col-md-7 col-sm-6">
                                        <%--                                                                            <asp:DropDownList ID="ddlpnl5SampleRequired" SkinID="DropDown" runat="server" DataSourceID="sqlSampleRequired"
                                            DataTextField="Name" DataValueField="ID" Width="200px" Enabled="false" Font-Size="9">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="sqlSampleRequired" runat="server" ConnectionString="<%$ connectionStrings:akl %>"
                                            SelectCommand="select ' [ Select ] ' as Name , '' as ID union Select Name,ID from SampleMaster where Active=@Active order by name">
                                            <SelectParameters>
                                                <asp:Parameter Name="Active" DefaultValue="1" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>--%>
                                        <asp:DropDownList ID="ddlpnl5SampleRequired" runat="server" Width="100%"
                                            Enabled="false" Font-Size="9">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-6 label2 PaddingRightSpacing">
                                        <asp:Literal ID="ltrlTimeBased" runat="server" Text="Time Based"></asp:Literal></div>
                                    <div class="col-md-8 col-sm-6">
                                        <asp:DropDownList ID="ddlTimeBased" runat="server" Width="100%">
                                            <asp:ListItem Text="[ Select ]" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="One Time" Value="OT"></asp:ListItem>
                                            <asp:ListItem Text="Daily Basis" Value="DB"></asp:ListItem>
                                            <asp:ListItem Text="Slab" Value="SL"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-3 col-sm-3">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-6"><span class="textName">
                                        <asp:Literal ID="ltrlpnl5SpecialPrecaution" runat="server" Text="Special Precaution"></asp:Literal></span></div>
                                    <div class="col-md-8 col-sm-6">
                                        <asp:TextBox ID="txtpnl5SpecialPrecaution" runat="server" Text="" TextAlign="Left" TextMode="MultiLine" Rows="3" MaxLength="200" Width="100%" /></div>
                                </div>
                            </div>


                        </div>
                    </div>



                    <div class="container-fluid border-LeftRight">
                        <div class="row form-group margin_Top">
                            <div class="col-md-4 col-sm-4">
                                <div class="row" id="pnl6" runat="server">
                                    <div class="col-md-6 col-sm-6">
                                        <asp:Literal ID="ltrlDifferbybedcategory" runat="server" Text="Charge Differ By Bed Category"></asp:Literal></div>
                                    <div class="col-md-6 col-sm-6">
                                        <div class="PD-TabRadioNew01 margin_z">
                                            <asp:RadioButtonList ID="optDifferbybedcategory" runat="server" RepeatDirection="Vertical"
                                                AutoPostBack="false" BorderStyle="None" BorderWidth="0">
                                                <asp:ListItem Text="No Variation" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Fixed Variation" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Percentage Variation" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-8 col-sm-8" id="pnlTax" runat="server">
                                <div class="row form-group">
                                    <div class="col-md-3 col-sm-3">
                                        <asp:Literal ID="Literal6" runat="server" Text="Is Service Tax Applicable"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z">
                                            <asp:CheckBox ID="chkIsServiceTaxApplicable" runat="server" TextAlign="Left" Checked="false" /></div>
                                    </div>
                                    <div class="col-md-3 col-sm-3">
                                        <asp:Literal ID="Literal7" runat="server" Text="Is Luxury Tax Applicable"></asp:Literal>
                                        <div class="PD-TabRadioNew01 margin_z">
                                            <asp:CheckBox ID="chkIsLuxuryTaxApplicable" runat="server" TextAlign="Left" Checked="false" /></div>
                                    </div>


                                    <div class="col-md-6 col-sm-6">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <asp:Literal ID="Literal12" runat="server" Text="Service Instructions"></asp:Literal></div>
                                            <div class="col-md-8 col-sm-6">
                                                <asp:TextBox ID="txtServiceInstructions" runat="server" Width="100%" Height="30px" TextMode="MultiLine"></asp:TextBox></div>
                                        </div>
                                    </div>

                                </div>






                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <div class="row">
                                            <div class="col-md-2 col-sm-3">
                                                <asp:Literal ID="Literal8" runat="server" Text="Remarks"></asp:Literal></div>
                                            <div class="col-md-10 col-sm-9">
                                                <asp:TextBox ID="txtremarks" runat="server" Width="100%" Height="40px" TextMode="MultiLine"></asp:TextBox></div>
                                        </div>
                                    </div>


                                </div>


                            </div>


                        </div>



                    </div>






                </asp:Panel>



                <div class="container-fluid">
                    <div class="row">
                        <asp:Panel ID="pnlChargesDetailsExpand" runat="server" Width="100%">
                            <asp:GridView ID="gvDefaultServiceCharges" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="BedcategoryId" AllowPaging="false" Width="100%" BorderWidth="1"
                                CellPadding="0" ShowFooter="true" OnRowDataBound="gvDefaultServiceCharges_OnRowDataBound"
                                OnRowCreated="gvDefaultServiceCharges_OnRowCreated">
                                <HeaderStyle ForeColor="Black" />
                                <Columns>
                                    <asp:BoundField DataField="BedcategoryId" HeaderText="BedCatID" />
                                    <asp:BoundField DataField="serviceid" HeaderText="SERVICE CODE" />
                                    <asp:BoundField DataField="BedCategoryName" HeaderText="Bed Category" Visible="true"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="320px" />
                                    <asp:TemplateField HeaderText="Bed Type %" HeaderStyle-Width="90px" ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBedTypePercent" SkinID="textbox" runat="server" Text='<%# FormatNumber(DataBinder.Eval(Container,"DataItem.BedTypePercentage")) %>'
                                                Columns="5" MaxLength="8" Style="text-align: right" Width="90px"></asp:TextBox>
                                            <asp:RangeValidator ID="RV5" runat="server" ErrorMessage="Discount % Should Be Between 0 - 100"
                                                ControlToValidate="txtBedTypePercent" SetFocusOnError="true" Display="Dynamic"
                                                MinimumValue="0.00" MaximumValue="100.00" Type="Double"></asp:RangeValidator>
                                            <AJAX:FilteredTextBoxExtender ID="FTE1" runat="server" Enabled="True" TargetControlID="txtBedTypePercent"
                                                FilterType="Custom , Numbers" ValidChars=".">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHospitalCharges" runat="server" Text='<%#Eval("ServiceCharge","{0:f2}")%>'
                                                Columns="5" MaxLength="8" Style="text-align: right" Width="90px" SkinID="textbox"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="FTE2" runat="server" Enabled="True" TargetControlID="txtHospitalCharges"
                                                FilterType="Custom , Numbers" ValidChars=".">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibtnSaveGridCharges" runat="server" ImageUrl="~/Images/save.gif"
                                                ToolTip="Save Charges" CausesValidation="false" OnClick="SaveCharges_OnClick" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doctor" HeaderStyle-Width="90px" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDoctorCharge" runat="server" Text='<%# FormatNumber(DataBinder.Eval(Container,"DataItem.DrCharge")) %>'
                                                Columns="5" MaxLength="8" Style="text-align: right" Width="90px" SkinID="textbox"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="FTE3" runat="server" Enabled="True" TargetControlID="txtDoctorCharge"
                                                FilterType="Custom , Numbers" ValidChars=".">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valid From" HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValidFrom" runat="server" Text='<%#Eval("Validfrom")%>' Columns="8"
                                                MaxLength="8" Width="90px" SkinID="textbox"></asp:TextBox>
                                            <AJAX:MaskedEditExtender ID="txtValidFrom_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                Enabled="True" TargetControlID="txtValidFrom" MessageValidatorTip="true" AcceptAMPM="true"
                                                AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                ErrorTooltipEnabled="true" InputDirection="RightToLeft">
                                            </AJAX:MaskedEditExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter Proper Date"
                                                ControlToValidate="txtValidFrom" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^(((0[1-9]|[1-2][0-9]|3[0-1])\/(0[13578]|(10|12)))|((0[1-9]|[1-2][0-9])\/02)|((0[1-9]|[1-2][0-9]|30)\/(0[469]|11)))\/(19|20)\d\d$"
                                                ValidationGroup="B"></asp:RegularExpressionValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OPD" HeaderStyle-Width="60px" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOPDDiscPercent" runat="server" Text='<%# FormatNumber(DataBinder.Eval(Container,"DataItem.OPDisc")) %>'
                                                Columns="5" MaxLength="8" Style="text-align: right" Width="55px" SkinID="textbox"></asp:TextBox>
                                            <asp:RangeValidator ID="RV2" runat="server" ErrorMessage="Discount % Should Be Between 0 - 100"
                                                ControlToValidate="txtOPDDiscPercent" SetFocusOnError="true" Display="Dynamic"
                                                MinimumValue="0.00" MaximumValue="100.00" Type="Double"></asp:RangeValidator>
                                            <AJAX:FilteredTextBoxExtender ID="FTE4" runat="server" Enabled="True" TargetControlID="txtOPDDiscPercent"
                                                FilterType="Custom , Numbers" ValidChars=".">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IPD" HeaderStyle-Width="60px" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIPDDiscPercent" runat="server" Text='<%# FormatNumber(DataBinder.Eval(Container,"DataItem.IPDisc")) %>'
                                                Columns="5" MaxLength="8" Style="text-align: right" Width="55px" SkinID="textbox"></asp:TextBox>
                                            <asp:RangeValidator ID="RV1" runat="server" ErrorMessage="Discount % Should Be Between 0 - 100"
                                                ControlToValidate="txtIPDDiscPercent" SetFocusOnError="true" Display="Dynamic"
                                                MinimumValue="0.00" MaximumValue="100.00" Type="Double"></asp:RangeValidator>
                                            <AJAX:FilteredTextBoxExtender ID="FTE5" runat="server" Enabled="True" TargetControlID="txtIPDDiscPercent"
                                                FilterType="Custom , Numbers" ValidChars=".">
                                            </AJAX:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TableID" HeaderText="TableID" Visible="true" />
                                    <asp:TemplateField HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <label id="ActiveInactive" skinid="label" runat="server">
                                                <%#Eval("Active") %></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkActiveInactive" SkinID="checkbox" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Status")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="White" Height="25px" />
                                <FooterStyle BackColor="#5EA0F4" Height="19px" BorderColor="#5EA0F4" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>


            </asp:Panel>


            <table width="100%">
                <tr valign="top" align="center">
                    <td width="35%">
                        <div id="pnl2" runat="server">
                            <br />
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" style="width: 2%">&nbsp;</td>
                                    <td align="left" style="width: 35%">
                                        <asp:Literal ID="ltrlPnl2PriceEditable" runat="server" Text="Price Editable" Visible="false"></asp:Literal></td>
                                    <td align="left" style="width: 20%">
                                        <asp:CheckBox ID="chkPnl2PriceEditable" SkinID="checkbox" runat="server" Text="" TextAlign="Left" Checked="false" Visible="false" /></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 2%"></td>
                                    <td align="left" style="width: 35%">
                                        <asp:Literal ID="ltrlPnl2DoctorReq" runat="server" Text="Doctor Required" Visible="false"></asp:Literal></td>
                                    <td align="left" style="width: 20%">
                                        <asp:CheckBox ID="chkPnl2DoctorReq" SkinID="checkbox" runat="server" Text="" TextAlign="Left" Checked="false" Visible="false" /></td>
                                </tr>

                                <%--
                                            <tr>
                                                <td align="left" style="width: 2%"></td>
                                                <td align="left" style="width: 35%"><asp:Literal ID="ltrlPnl2ChargeType" runat="server" Text="Charge Type"></asp:Literal></td>
                                                <td align="left" style="width: 20%">
                                                    <asp:DropDownList ID="ddlPnl2ChargeType" SkinID="DropDown" runat="server" Width="100px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPnl2ChargeType_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="One Time" Value="OT" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Daily Basic" Value="DB"></asp:ListItem>
                                                        <asp:ListItem Text="Time Slab" Value="SL"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 2%"></td>
                                                <td align="left" style="width: 35%"><asp:Literal ID="ltrlPnl2Slab" runat="server" Text="Charge Type"></asp:Literal></td>
                                                <td align="left" style="width: 20%">
                                                    <asp:DropDownList ID="ddlPnl2Slab" SkinID="DropDown" runat="server" Width="100px">
                                                        Visible="false">
                                                        <asp:ListItem Text="Hourly" Value="Hourly"></asp:ListItem>
                                                        <asp:ListItem Text="Slabs" Value="Slabs"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                --%>
                            </table>
                        </div>

                        <%--<div id="pnl3" runat="server">
                                        <br />
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" style="width: 2%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" style="width: 35%">
                                                    <asp:Literal ID="ltrlPnl3PriceEditable" runat="server" Text="Price Editable"></asp:Literal>
                                                </td>
                                                <td align="left" style="width: 20%">
                                                    <asp:CheckBox ID="chkPnl3PriceEditable" SkinID="checkbox" runat="server" Text=""
                                                        TextAlign="Left" Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 2%">
                                                </td>
                                                <td align="left" style="width: 35%">
                                                    <asp:Literal ID="ltrlPnl3Classification" runat="server" Text="Classification"></asp:Literal>
                                                </td>
                                                <td align="left" style="width: 20%">
                                                    <asp:DropDownList ID="ddlPnl3Classification" SkinID="DropDown" runat="server" Width="100px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="requiredval3" runat="server" ControlToValidate="ddlPnl3Classification"
                                                        InitialValue="0" ValidationGroup="save" Display="None" ErrorMessage="Select the surgery classification type to continue..."
                                                        Text="Select the surgery classification type to continue..."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:LinkButton ID="lnkPrePostNotes" runat="server" Font-Underline="false" ForeColor="Black"
                                                        Visible="false">Pre-post operative notes</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>--%>
                    </td>

                    <td>
                        <div>
                            <table cellpadding="0" cellspacing="0">
                                <%--Done by ujjwal 23March2015 to capture billing category of rooms start--%>
                                <tr id="trisBillingCategory" runat="server" visible="false">
                                    <td style="width: 2%">&nbsp;</td>
                                    <td align="left">
                                        <asp:Literal ID="Literal9" runat="server" Text="Is Billing Category"></asp:Literal></td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkIsBillingCategory" SkinID="checkbox" runat="server" TextAlign="Left"
                                            AutoPostBack="true" Checked="false" OnCheckedChanged="chkIsBillingCategory_CheckedChanged" />
                                    </td>
                                                 <td> &nbsp;&nbsp; &nbsp;Minimum Advance Amount
                                                    <asp:TextBox ID="txtMinAdvanceAmount" runat="server" text="0.00" Width="50px"  ></asp:TextBox>

                                                </td>
                                </tr>

                                <tr id="trBillingCategory" runat="server" visible="false">
                                    <td style="width: 2%">&nbsp;</td>
                                    <td align="left">
                                        <asp:Literal ID="Literal10" runat="server" Text="Billing Category"></asp:Literal></td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlBillingCategory" runat="server" SkinID="DropDown">
                                        </asp:DropDownList>
                                    </td>
<td></td>
                                </tr>
                                <%--Done by ujjwal 23March2015 to capture billing category of rooms end--%>
                            </table>
                        </div>

                        <div id="pnl4" runat="server">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" style="width: 35%">
                                        <asp:Literal ID="ltrlPnl4BedType" runat="server" Text="Bed Type" Visible="false"></asp:Literal></td>
                                    <td align="left" style="width: 20%">
                                        <asp:DropDownList ID="ddlPnl4BedType" SkinID="DropDown" runat="server" Width="100px"
                                            Visible="false">
                                            <asp:ListItem Text="Normal" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="ICU" Value="I"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 35%">
                                        <asp:Literal ID="ltrlPnl4ExcludeFromOccupancy" runat="server" Text="Exclude Occupancy" Visible="false"></asp:Literal></td>
                                    <td align="left" style="width: 20%">
                                        <asp:CheckBox ID="chkPnl4ExcludeFromOccupancy" SkinID="checkbox" runat="server" Text="" TextAlign="Left" Visible="false" /></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>

            </table>

            <table width="100%" cellpadding="0" cellspacing="2">

                <tr valign="top">
                    <td valign="top" align="left"></td>
                    <td style="width: 1%"></td>
                </tr>

                <tr>
                    <td colspan="2" align="center">
                        <asp:HiddenField ID="txtServiceCode" runat="server" />
                    </td>
                </tr>

                <tr valign="top">
                    <td>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 2%">&nbsp;</td>
                                <td style="width: 10%;">
                                    <asp:Literal ID="ltrlEncodedBy" runat="server" Text="Encoded By/Date "></asp:Literal></td>
                                <td style="width: 30%">
                                    <asp:Literal ID="ltrlEncodedByDisplay" runat="server" Text=""></asp:Literal></td>
                                <td style="width: 10%;">
                                    <asp:Literal ID="ltrlUpdateBy" runat="server" Text="Modified By/Date "></asp:Literal></td>
                                <td style="width: 30%;">
                                    <asp:Literal ID="ltrlUpdateByDisplay" runat="server" Text=""></asp:Literal></td>
                                <td style="width: 3%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="False" />
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ibtnNew" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" />
            <asp:AsyncPostBackTrigger ControlID="ibtnUpdate" />
            <asp:AsyncPostBackTrigger ControlID="ddlMainDept" />
            <asp:AsyncPostBackTrigger ControlID="ddlsubDept" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
