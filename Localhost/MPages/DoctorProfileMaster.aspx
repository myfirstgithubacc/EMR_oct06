<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DoctorProfileMaster.aspx.cs" Inherits="EMR_DoctorProfile_DoctorProfileMaster"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*.RadEditor table, .RadEditor.reWrapper table td { background: #c1e5ef !important;}*/
    </style>


    <script language="javascript" type="text/javascript" src="RichTextBox.js"></script>
    <script type="text/javascript">
        function OnClientSelectionChange(editor, args) {
        var tool = editor.getToolByName("RealFontSize");
        if (tool && !$telerik.isIE) {
            setTimeout(function() {
                var value = tool.get_value();

                switch (value) {
                    case "11px":
                        value = value.replace("11px", "9pt");
                        break;
                    case "12px":
                        value = value.replace("12px", "9pt");
                        break;
                    case "14px":
                        value = value.replace("14px", "11pt");
                        break;
                    case "15px":
                        value = value.replace("15px", "11pt");
                        break;
                    case "16px":
                        value = value.replace("16px", "12pt");
                        break;
                    case "18px":
                        value = value.replace("18px", "14pt");
                        break;
                    case "19px":
                        value = value.replace("19px", "14pt");
                        break;
                    case "24px":
                        value = value.replace("24px", "18pt");
                        break;
                    case "26px":
                        value = value.replace("26px", "20pt");
                        break;
                    case "27px":
                        value = value.replace("27px", "20pt");
                        break;
                    case "32px":
                        value = value.replace("32px", "24pt");
                        break;
                    case "34px":
                        value = value.replace("34px", "26pt");
                        break;
                    case "35px":
                        value = value.replace("35px", "26pt");
                        break;
                    case "48px":
                        value = value.replace("48px", "36pt");
                        break;
                }
                tool.set_value(value);
            }, 0);
        }
    }
    </script>
    <script type='text/javascript'>
        function OnClientEditorLoad(editor, args) {
            var style = editor.get_contentArea().style;
            style.fontFamily = 'Tahoma';
            style.fontSize = 11 + 'pt';
        } 
    </script> 



    <div class="container-fluid header_main">
        <div class="col-md-2 col-sm-3"><h2>Employee&nbsp;Profile</h2></div>
        <div class="col-md-3 col-sm-3">
            <div class="row">
                <div class="col-md-3 col-sm-3 label2">Employee</div>
                <div class="col-md-9 col-sm-9">
                    <asp:DropDownList ID="ddlDoctorName" runat="server" CssClass="drapDrowHeight" Width="100%" DataTextField="EmployeeName"
                        DataValueField="EmployeeID" OnSelectedIndexChanged="ddlDoctorName_OnSelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDoctorName" runat="server" ControlToValidate="ddlDoctorName" InitialValue ="0"
                        ValidationGroup="SaveUpdate" ErrorMessage="Please select provider." Display="None"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="col-md-7 col-sm-6 text-right">
            <span class="pull-right margin_Top01">
                <asp:LinkButton ID="lnkEmployee" runat="server" CausesValidation="false" Text="Employee"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployee_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkAppointmentTemplate" runat="server" CausesValidation="false"
                    Text="Appointment Template" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                    onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkAppointmentTemplate_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkProviderDetails" runat="server" CausesValidation="false"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Doctor Details" OnClick="lnkProviderDetails_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkEmployeeLookup" runat="server" CausesValidation="false" Text="Employee Look Up"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployeeLookup_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkClassification" runat="server" CausesValidation="false"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Classification" OnClick="lnkClassification_OnClick"></asp:LinkButton>
                <script language="JavaScript" type="text/javascript">
                    function LinkBtnMouseOver(lnk) {
                        document.getElementById(lnk).style.color = "SteelBlue";
                    }
                    function LinkBtnMouseOut(lnk) {
                        document.getElementById(lnk).style.color = "SteelBlue";
                    }
                </script>
                <asp:Button ID="ibtnSaveDoctorProfile" runat="server" CausesValidation="true" OnClick="SaveDoctorProfile_OnClick"
                    ToolTip="Save" ValidationGroup="SaveUpdate" Text="Save" CssClass="btn btn-primary"  />
                <asp:ValidationSummary ID="vsPageError" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="SaveUpdate" />
            </span>
            
        </div>
    </div>
    

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 text-center">
                <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDoctorName" />
                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveDoctorProfile" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Green"  />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <div class="container-fluid">
        <div class="row">
            <asp:UpdatePanel ID="updDoctorProfile" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadEditor runat="server" ID="RTF1" Skin="Office2007" EditModes="Design" OnClientSelectionChange="OnClientSelectionChange" OnClientLoad="OnClientEditorLoad"  ToolsFile="~/Include/XML/EditorTools.xml" Height="520" Width="100%">
                    </telerik:RadEditor>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ibtnSaveDoctorProfile" />
                    <asp:AsyncPostBackTrigger ControlID="ddlDoctorName" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>