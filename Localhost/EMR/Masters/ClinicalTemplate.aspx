<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClinicalTemplate.aspx.cs"
    Inherits="EMR_Masters_ClinicalTemplate" MasterPageFile="~/Include/Master/EMRMaster.master"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <style>
        body {
            overflow-y: hidden;
        }
    </style>
    <script type="text/javascript" src="../../Include/JS/Functions.js"></script>

    <script type="text/javascript">
        function OnClientCloseSearchTemplate(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var TemplateId = arg.TemplateId;
                var TemplateTypeId = arg.TemplateTypeId;

                $get('<%=hdnTemplateId.ClientID%>').value = TemplateId;
                $get('<%=hdnTemplateTypeId.ClientID%>').value = TemplateTypeId;
            }
            $get('<%=btnGetInfo.ClientID%>').click();
        }

        function colorChanged(sender) {
            sender.get_element().style.color = "#" + sender.get_selectedColor();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main">
                <div class="col-md-2">
                    <h2>Templates Master</h2>
                </div>
                <div class="col-md-5 pull-right text-right">
                    <asp:Button ID="btnTagging" ToolTip="Tagging Template" CssClass="btn btn-primary" Text="Tagging Template"
                        OnClick="btnTagging_OnClick" runat="server" />
                    <asp:Button ID="btnSearchTemplate" ToolTip="New Template" CssClass="btn btn-primary" Text="Search Template"
                        OnClick="btnSearchTemplate_OnClick" runat="server" />
                    <asp:Button ID="btnTemplateNew" ToolTip="New Template" CssClass="btn btn-primary" Text="New"
                        OnClick="btnTemplateNew_OnClick" runat="server" />
                    <asp:Button ID="ibtnTemplateSave" Text="Save" runat="server" CssClass="btn btn-primary" ToolTip="Save Template"
                        CausesValidation="true" OnClick="SaveTemplate_OnClick" ValidationGroup="SaveUpdateTemplate" />&nbsp;
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="SaveUpdateTemplate" />
                </div>
            </div>
            <asp:UpdatePanel ID="UpdTemplateView" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibtnTemplateSave" />
                </Triggers>
                <ContentTemplate>
                    <div class="container-fluid subheading_main">
                        <div class="col-md-3">Template</div>
                        <div class="col-md-9">
                            <asp:Label ID="lblTemplateMessage" runat="server" />
                        </div>
                    </div>
                    <asp:UpdatePanel ID="Upd2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnTemplateNew" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-4 form-group">
                                        <div class="col-md-4">
                                            <asp:Literal ID="ltrlName" runat="server" Text="Template" />
                                            <span class="red">*</span>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtTemplateName" SkinID="textbox" runat="server" Text="" MaxLength="50"
                                                Columns="27" TabIndex="1" onkeydown="Tab();" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom, LowercaseLetters , UppercaseLetters" TargetControlID="txtTemplateName" ValidChars="1234567890_- " />
                                            <asp:RequiredFieldValidator ID="RFVtxtTemplateName" runat="server" ErrorMessage="Please enter Template Name."
                                                ValidationGroup="SaveUpdateTemplate" Display="None" ControlToValidate="txtTemplateName"
                                                SetFocusOnError="true" />
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <div class="col-md-4">
                                            <asp:Literal ID="Literal3" runat="server" Text="Specialisation" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="ibtnTemplateSave">
                                                <asp:DropDownList ID="ddlSpecialisation" runat="server" AppendDataBoundItems="true"
                                                    onkeydown="Tab();" TabIndex="2" DataTextField="Name" DataValueField="Id" SkinID="DropDown">
                                                    <asp:ListItem Text=" [ Select ] " Value="0" Selected="True" />
                                                </asp:DropDownList>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="col-md-4  form-group">
                                        <div class="col-md-4">
                                            <asp:Literal ID="ltrlTemplateType" runat="server" Text="Template&nbsp;Type" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Panel ID="pnltemp" runat="server" DefaultButton="ibtnTemplateSave">
                                                <asp:DropDownList ID="ddlTemplateType" runat="server" AppendDataBoundItems="true"
                                                    onkeydown="Tab();" TabIndex="4" DataTextField="TypeName" DataValueField="ID"
                                                    SkinID="DropDown" />
                                                <%--<asp:RangeValidator ID="RVddlTemplateType" runat="server" ControlToValidate="ddlTemplateType"
                                                                    SetFocusOnError="true" Display="None" ErrorMessage="Please select any Type."
                                                                    MaximumValue="99999" MinimumValue="1" ValidationGroup="SaveUpdateTemplate"></asp:RangeValidator>--%>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <!--end of row -->
                                <div class="row">
                                    <div class="col-md-4  form-group">
                                        <div class="col-md-4">
                                            <asp:Literal ID="ltrlAsignCode" runat="server" Text="Assign Code" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtAssignCode" SkinID="textbox" runat="server" Text="" MaxLength="50"
                                                onkeydown="Tab();" TabIndex="5" Columns="27" />
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <div class="col-md-4">
                                            <asp:Label ID="Label3" runat="server" Text="Entry Type" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="ddlEntryType" runat="server" SkinID="DropDown">
                                                <asp:ListItem Text="Multiple entries in one visit" Value="M" />
                                                <asp:ListItem Text="Visit wise entry" Value="V" Selected="True" />
                                                <asp:ListItem Text="Single entry" Value="S" />
                                                <asp:ListItem Text="Episode" Value="E" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="col-md-4">
                                            <asp:Label ID="Label4" runat="server" Text="Template For" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:RadioButtonList ID="rdoApplicableFor" runat="server" RepeatDirection="horizontal" CssClass="radioo">
                                                <asp:ListItem Text="OP" Value="O" />
                                                <asp:ListItem Text="IP" Value="I" />
                                                <asp:ListItem Text="Both" Value="B" Selected="True" />
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                                <!--end of row -->
                                <div class="row">
                                    <div class="col-md-4 form-group">
                                        <div class="col-md-4">
                                            <asp:Literal ID="lblStatusT" runat="server" Text="Status" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDown" onkeydown="Tab();"
                                                TabIndex="3">
                                                <asp:ListItem Text="Active" Value="1" Selected="True" />
                                                <asp:ListItem Text="In-Active" Value="0" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4  form-group">
                                        <div class="col-md-4">
                                            <asp:Literal ID="Literal1" runat="server" Text="Document No." />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtDocumentNo" SkinID="textbox" runat="server" Text="" MaxLength="50"
                                                onkeydown="Tab();" TabIndex="5" Columns="27" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkTitleNote" runat="server" onkeydown="Tab();" TabIndex="6" Text="Display&nbsp;title&nbsp;in&nbsp;Notes" CssClass="block" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkDispInEMR" runat="server" Text="Display In EMR Menu" CssClass="block" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkDispInNB" runat="server" Text="Display In Nurse Menu" CssClass="block" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkIsConfidentail" runat="server" Text="Is Sensitive/Confidential" CssClass="block" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkIsMandatoryForIP" OnCheckedChanged="chkIsMandatoryForIP_CheckedChanged" AutoPostBack="true" runat="server" Text="Is Mandatory For IP" Visible="true" CssClass="block" />
                                    </div>
                                    <div class="col-md-2" style="padding: 0; margin: 0;">
                                        <div class="col-lg-4" style="padding: 0; margin: 0;">
                                            <asp:Label ID="Label5" Text="IPTAT Hours" runat="server" />
                                        </div>
                                        <div class="col-lg-8" style="padding: 0; margin: 0;">
                                            <asp:TextBox ID="txtIPTATHours" Width="100%" Enabled="false" CssClass="RegText" MaxLength="2" runat="server"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtIPTATHours" ValidChars="." />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkIsShowCloseEncounter" AutoPostBack="true" runat="server" Text="Show Close Encounter" Visible="true" CssClass="block" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="chkIsMandatoryForMarkForDischarge" AutoPostBack="true" runat="server" Text="Mandatory For Mark For Discharge" Visible="true" CssClass="block" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="chkIsMandatory" runat="server" Text="Is Mandatory" Visible="false" CssClass="block" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="container-fluid">
                        <div class="col-md-12 subheading_main">
                            <div class="col-md-12">Font Style</div>
                        </div>
                    </div>
                    <!-- row ends -->
                    <!-- end of container fluid-->
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <asp:UpdatePanel ID="updPanel1" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkTitleNote" />
                                            <asp:AsyncPostBackTrigger ControlID="btnTemplateNew" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div id="trTemplate" runat="server">
                                                <div class="col-md-4 from-group">
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label2" Text="Template" runat="server" />
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkBoldTemplate" Text="B" Checked="false" runat="server" CssClass="block" />
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkItalicTemplate" Text="I" runat="server" CssClass="block" />
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkUnderlineTemplate" Text="U" Checked="false" Font-Underline="true" CssClass="block" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="col-md-8">
                                                        <asp:LinkButton ID="btnForeColorTemplate" runat="server" Text="Font Color" ToolTip="Click to pick Color" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtColorTemplate" runat="server" Width="0px" BorderColor="Transparent"
                                                            BackColor="Transparent" Text="000000" Font-Size="1px" Height="0px" />
                                                        <AJAX:ColorPickerExtender ID="ColorPickerExtender2" runat="server" TargetControlID="txtColorTemplate"
                                                            PopupButtonID="btnForeColorTemplate" PopupPosition="TopRight" SampleControlID="lblForeColorTemplate"
                                                            OnClientColorSelectionChanged="colorChanged" Enabled="True" />
                                                        <asp:Label ID="lblForeColorTemplate" runat="server" Width="30px" BorderColor="Black"
                                                            BorderWidth="1px" BorderStyle="Solid" Height="18px" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2 form-group">
                                                    <div class="col-md-4">
                                                        <asp:Literal ID="Literal11" runat="server" Text="Size" /><span class="red">*</span>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:DropDownList ID="ddlFontSizeTemplate" runat="server" SkinID="dropdown" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4 form-group">
                                                    <div class="col-md-4">
                                                        <asp:Literal ID="Literal6" runat="server" Text="Font" /><span class="red">*</span>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:DropDownList ID="ddlFontTypeTemplate" runat="server" SkinID="dropdown" />
                                                    </div>
                                                </div>
                                                <asp:Literal ID="ltrlTemspace" runat="server" Text="Line Space" Visible="false" />
                                                <asp:DropDownList ID="DdlTemSpace" SkinID="DropDown" Width="40px" runat="server" Visible="false">
                                                    <asp:ListItem Text="1" Value="1" Selected="True" />
                                                    <asp:ListItem Text="2" Value="2" />
                                                    <asp:ListItem Text="3" Value="3" />
                                                    <asp:ListItem Text="4" Value="4" />
                                                </asp:DropDownList>
                                            </div>
                                            <!-- row ends -->
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div id="trSection" runat="server">
                                        <div class="col-md-4">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblSections" Text="Section(s)" runat="server" />
                                            </div>
                                            <div class="col-md-8">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkBoldSections" Text="B" Checked="true" runat="server" CssClass="block" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkItalicSections" Text="I" runat="server" CssClass="block" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkUnderlineSections" Text="U" Checked="true" runat="server" CssClass="block" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="col-md-8">
                                                <asp:LinkButton ID="btnForeColorSections" runat="server" Text="Font Color" ToolTip="Click to pick Color" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtColorSections" runat="server" Width="0px" BorderColor="Transparent"
                                                    BackColor="Transparent" Text="000000" Font-Size="1px" Height="0px" />
                                                <AJAX:ColorPickerExtender ID="ajax1" runat="server" TargetControlID="txtColorSections"
                                                    PopupButtonID="btnForeColorSections" PopupPosition="TopRight" SampleControlID="lblForeColorSections"
                                                    OnClientColorSelectionChanged="colorChanged" Enabled="True" />
                                                <asp:Label ID="lblForeColorSections" runat="server" Width="30px" BorderColor="Black"
                                                    BorderWidth="1px" BorderStyle="Solid" Height="18px" />
                                            </div>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <div class="col-md-4">
                                                <asp:Literal ID="Literal5" runat="server" Text="Size" /><span class="red">*</span>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlFontSizeSections" runat="server" SkinID="dropdown" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 form-group">
                                            <div class="col-md-4">
                                                <asp:Literal ID="Literal4" runat="server" Text="Font" />
                                                <span class="red">*</span>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlFontTypeSections" runat="server" SkinID="dropdown" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div id="trFields" runat="server">
                                        <div class="col-md-4">
                                            <div class="col-md-4">
                                                <asp:Label ID="Label1" Text="Field(s)" Width="60px" runat="server" />
                                            </div>
                                            <div class="col-md-8">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkBoldFields" Text="B" Checked="true" runat="server" CssClass="block" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkItalicFields" Text="I" runat="server" CssClass="block" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:CheckBox ID="chkUnderlineFields" Text="U" runat="server" CssClass="block" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="col-md-8">
                                                <asp:LinkButton ID="btnForeColorFields" runat="server" Text="Font Color" ToolTip="Click to pick Color" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtColorFields" runat="server" Width="0px" BorderColor="Transparent"
                                                    BackColor="Transparent" Text="000000" Font-Size="1px" Height="0px" />
                                                <AJAX:ColorPickerExtender ID="ColorPickerExtender1" runat="server" TargetControlID="txtColorFields"
                                                    PopupButtonID="btnForeColorFields" PopupPosition="TopRight" SampleControlID="lblForecolorFields"
                                                    OnClientColorSelectionChanged="colorChanged" Enabled="True" />

                                                <asp:Label ID="lblForecolorFields" runat="server" Width="30px" BorderColor="Black"
                                                    BorderWidth="1px" BorderStyle="Solid" Height="18px" />
                                            </div>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <div class="col-md-4">
                                                <asp:Literal ID="Literal10" runat="server" Text="Size" /><span class="red">*</span>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlFontSizeFields" runat="server" SkinID="dropdown" />
                                            </div>
                                        </div>
                                        <div class="col-md-4 form-group">
                                            <div class="col-md-4">
                                                <asp:Literal ID="Literal9" runat="server" Text="Font" /><span class="red">*</span>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlFontTypeFields" runat="server" SkinID="dropdown" />
                                            </div>
                                        </div>
                                    </div>
                                    <!-- row ends -->
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlTemplateView" runat="server" Width="100%" Height="410px">
                        <asp:UpdatePanel ID="updatepanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdnRegNo" runat="server" Value="" />
                        <asp:UpdatePanel ID="updatepanelclose" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnclose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                    OnClick="btnclose_OnClick" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Button ID="btnGetInfo" runat="server" CausesValidation="false" Enabled="true"
                            OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden;" TabIndex="103"
                            Text="Assign" Width="10px" />
                        <asp:HiddenField ID="hdnTemplateId" runat="server" />
                        <asp:HiddenField ID="hdnTemplateTypeId" runat="server" />
                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
