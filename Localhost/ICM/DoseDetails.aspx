<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DoseDetails.aspx.cs" Inherits="DoseDetails"
    Title="Administration Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>

    <script src="/Include/JS/Functions.js" type="text/jscript"></script>

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = document.getElementById("hdnxmlString").value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>

    <style type="text/css">
        .newt {
            color: Black;
            font-family: arial;
            height: 35px;
            font-weight: bold;
            font-size: 12px;
            background-color: #A0CFEC;
            background-repeat: repeat-x;
            margin-bottom: 0px;
            letter-spacing: 1px;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%" class="newt">
                        <tr>
                            <td align="left" width="75%">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient : " SkinID="label" />
                                <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label5" runat="server" Text="DOB : " Visible="false" SkinID="label" />
                                <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label4" runat="server" Text="Mobile No : " SkinID="label" />
                                <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <%--  </td>
                    </tr>
                    <tr>
                        <td>--%>
                                <asp:Label ID="lblIpno" runat="server" Text="IP No : " SkinID="label" />
                                <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label6" runat="server" Text="Admission Date : " SkinID="label" />
                                <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                                <asp:Label ID="Label29" runat="server" Text="UHID : " SkinID="label" />
                                <asp:Label ID="lblRegNo" runat="server" Text="" SkinID="label" ForeColor="#990066" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" bgcolor="white" cellspacing="0" cellpadding="0" align="center">
                        <tr>
                            <td align="left" colspan="5">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Underline="true" Font-Size="12px"
                                    SkinID="label" Visible="false" />
                                &nbsp;
                            <asp:Label ID="lblPrescriptionLabel" runat="server" Font-Bold="true" Font-Size="12px"
                                SkinID="label" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <%--  <telerik:RadDatePicker ID="dtpScheduleDate" runat="server" MinDate="01/01/1901" Width="100px">
                                <DateInput ID="DateInput4" runat="server" DisplayDateFormat="dd/MM/yyyy" DateFormat="MM/dd/yyyy">
                                </DateInput>
                            </telerik:RadDatePicker>--%>
                            </td>
                            <td>
                                <%--  <telerik:RadTimePicker ID="dtpdtpScheduleTime" runat="server" DateInput-ReadOnly="true"
                                PopupDirection="BottomLeft" TimeView-Columns="6" Width="90px" />--%>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <%--  <asp:Label ID="Label16" runat="server" Text="Medication administered" SkinID="label" />
                            &nbsp;--%>
                                <%-- <asp:DropDownList ID="ddlYes" runat="server" SkinID="DropDown" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlYes_OnSelectedIndexChanged">
                                <asp:ListItem Text="Yes" Value="Y" Selected="True" />
                                <asp:ListItem Text="No" Value="N" />
                            </asp:DropDownList>--%>
                                <asp:Label ID="Label7" runat="server" Text="Schedule Date&Time - " SkinID="label" />
                                <asp:Label ID="lblScheduleDateTime" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                            </td>
                            <td align="right" colspan="2">
                                <asp:Button ID="btnSubmit" runat="server" Text="Update" OnClick="btnSubmit_Click"
                                    SkinID="Button" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" SkinID="Button" OnClick="btnClose_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="width: 100%" align="left">
                                <table border="0" width="600px" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td align="left" style="width: 150px">
                                            <asp:Literal ID="Literal1" runat="server" Text="Administration Status"></asp:Literal><span
                                                style="color: Red">*</span>
                                        </td>
                                        <td align="left" colspan="4">
                                            <asp:DropDownList ID="ddlYes" SkinID="DropDown" runat="server" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlYes_OnSelectedIndexChanged">
                                                <asp:ListItem Text="-- Select --" Value="" />
                                                <asp:ListItem Text="Administered" Value="Y" Selected="True" />
                                                <asp:ListItem Text="Not Administered" Value="N" />
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblDose" Visible="true" runat="server" Text="Dose: " />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="width: 100%" align="left">
                                <table id="tblNormailYes" runat="server" cellpadding="1" visible="false" cellspacing="1"
                                    border="0" style="border-color: Blue" width="600px">
                                    <tr>
                                        <td style="width: 150px">
                                            <asp:Label ID="Label2" runat="server" Text="Administration Date&Time" SkinID="label" />
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadDatePicker ID="dtpNormalDate" runat="server" MinDate="01/01/1901" Width="100px">
                                                            <DateInput ID="DateInput1" runat="server" ReadOnly="true" DisplayDateFormat="dd/MM/yyyy"
                                                                DateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <telerik:RadTimePicker ID="dtpNormalTime" runat="server" DateInput-ReadOnly="false"
                                                            PopupDirection="BottomLeft" TimeView-Columns="8" Width="90px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px">
                                            <%--  Vitals--%>
                                        </td>
                                        <td colspan="3">
                                            <table id="Table1" width="350px" runat="server" cellpadding="2" visible="false">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" runat="server" Text="HR" SkinID="label" />
                                                        <asp:TextBox ID="txtHR" runat="server" SkinID="textbox" Width="50px" MaxLength="10"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                            TargetControlID="txtHR" FilterType="Custom, Numbers" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="RR" SkinID="label" />
                                                        <asp:TextBox ID="txtRR" runat="server" SkinID="textbox" Width="50px" MaxLength="10"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtRR" FilterType="Custom, Numbers" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label10" runat="server" Text="DBP" SkinID="label" />
                                                        <asp:TextBox ID="txtDBP" runat="server" SkinID="textbox" Width="50px" MaxLength="10"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtDBP" FilterType="Custom, Numbers" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Text="SBP" SkinID="label" />
                                                        <asp:TextBox ID="txtSBP" runat="server" SkinID="textbox" Width="50px" MaxLength="10"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                            TargetControlID="txtSBP" FilterType="Custom, Numbers" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <%--      <tr>
                                    <td align="left" style="width: 150px">
                                   
                                        <asp:Literal ID="Literal1" runat="server" Text="Administration Status"></asp:Literal><span style="color: Red">*</span>
                                        
                                    </td>
                                    <td colspan="3">
                                    <asp:DropDownList ID="ddlYes" SkinID="DropDown" runat="server"  Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlYes_OnSelectedIndexChanged">
                                    <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Administered" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="Not Administered" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                      
                                    </td> --%><%-- <asp:TextBox ID="txtRemark" runat="server" Height="30px" SkinID="textbox" TextMode="MultiLine"
                                            Width="350px" MaxLength="500"></asp:TextBox>--%>
                                    <tr>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" SkinID="label" runat="server" Text="Reason for Not administered" />
                                                    </td>
                                                    <td>
                                                        <div id="dvNOtAdministered" runat="server" style="color: Red" visible="false">
                                                            *
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- <asp:CheckBox ID="chkNotAdministered" AutoPostBack="true" OnCheckedChanged="chkNotAdministered_OnCheckedChanged"
                                            runat="server" SkinID="checkbox" />--%>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtNormalReasonForNotAdministered" runat="server" Enabled="false"
                                                SkinID="textbox" TextMode="MultiLine" Width="350px" Height="30px" MaxLength="500"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label12" SkinID="label" runat="server" Text="Reason for delay" />
                                                    </td>
                                                    <td>
                                                        <div id="divReasionDelay" runat="server" style="color: Red">
                                                            *
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtReasonForDelay" Height="30px" runat="server" SkinID="textbox"
                                                TextMode="MultiLine" Width="350px" MaxLength="500"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trInstruction" runat="server">
                                        <td>
                                            <asp:Label ID="Label16" SkinID="label" runat="server" Text="Instruction By Doctor" />
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtInstructions" TextMode="MultiLine" Width="350px" Height="30px"
                                                ReadOnly="true" SkinID="textbox" Enabled="false" runat="server" Text=""></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label31" SkinID="label" runat="server" Text="Remarks" />
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtNormalRemarks" TextMode="MultiLine" Width="350px" Height="30px"
                                                SkinID="textbox" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%--<asp:Label ID="Label13" SkinID="label" runat="server" Text="Administered By" />--%>
                                            <asp:Label ID="Label122" SkinID="label" runat="server" Text="User ID" />
                                        </td>
                                        <td style="width: 130px">
                                            <asp:Label ID="lblNormalUserName" SkinID="label" runat="server" />
                                        </td>
                                        <td style="width: 110px">
                                            <asp:Label ID="lblPassword" SkinID="label" runat="server" Text="Password" />
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNormalUserPassword" Width="100px" runat="server" SkinID="textbox"
                                                TextMode="Password"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trWitness" runat="server">
                                        <td>
                                            <asp:Label ID="Label14" SkinID="label" runat="server" Text="Witness User ID ( In case of High alert / High risk / Narcotics drug )" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHightAlertWitnessUserName" Width="100px" runat="server" SkinID="textbox" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" SkinID="label" runat="server" Text="Password" />
                                            <span id="spnHightAlertUserPassword" runat="server" style="color: Red">*</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtHightAlertWitnessPassword" Width="100px" runat="server" SkinID="textbox"
                                                TextMode="Password" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="width: 100%" align="left">
                                <table id="tblIVInfusion" runat="server" cellpadding="1" visible="false" cellspacing="1"
                                    border="0" style="border-color: Blue" width="600px">
                                    <tr id="trIVINFVolume" runat="server">
                                        <td style="width: 200px">
                                            <asp:Label ID="Label17" runat="server" Text="Medication Start Date&Time" SkinID="label" />
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dtpIVINFUMedHangedDate" runat="server" MinDate="01/01/1901"
                                                Width="100px">
                                                <DateInput ID="DateInput2" runat="server" ReadOnly="true" DisplayDateFormat="dd/MM/yyyy"
                                                    DateFormat="dd/MM/yyyy">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <telerik:RadTimePicker ID="dtpIVINFUMedHangedTime" runat="server" DateInput-ReadOnly="false"
                                                PopupDirection="BottomLeft" TimeView-Columns="6" Width="90px" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label20" runat="server" Text="Volume" SkinID="label" />
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td style="width: 100%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtVolumnHanged" runat="server" Width="40px" SkinID="textbox" MaxLength="20"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftbVolumnHooked" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="1234567890." TargetControlID="txtVolumnHanged">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label27" runat="server" SkinID="label" Text="Unit" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlVolumeUnit" runat="server" MarkFirstMatch="true" Width="100px"
                                                            EmptyMessage="[ Select ]" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px">
                                            <asp:Label ID="Label22" runat="server" Text="Medication Completed/Discontinued" SkinID="label" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dtpIVINFUCompletedDate" runat="server" MinDate="01/01/1901"
                                                Width="100px">
                                                <DateInput ID="DateInput3" runat="server" ReadOnly="true" DisplayDateFormat="dd/MM/yyyy"
                                                    DateFormat="dd/MM/yyyy">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <telerik:RadTimePicker ID="dtpIVINFUCompletedTime" runat="server" DateInput-ReadOnly="false"
                                                PopupDirection="BottomLeft" TimeView-Columns="6" Width="90px" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label23" runat="server" Text="Vol. Infused" SkinID="label" />
                                        </td>
                                        <td style="width: 100%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtVolumnInfusion" Width="40px" runat="server" SkinID="textbox"
                                                            MaxLength="20"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEVolumnInfusion" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="1234567890." TargetControlID="txtVolumnInfusion">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label24" runat="server" SkinID="label" Text="Unit" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlVolInfusionUnit" runat="server" MarkFirstMatch="true"
                                                            Width="100px" EmptyMessage="[ Select ]" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="Label21" runat="server" Text="Vol. discarded" SkinID="label" />
                                        </td>
                                        <td style="width: 100%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtVolumeDiscard" Width="40px" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="1234567890." TargetControlID="txtVolumeDiscard">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label25" runat="server" SkinID="label" Text="Unit" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlvolDiscardUnit" runat="server" MarkFirstMatch="true"
                                                            Width="100px" EmptyMessage="[ Select ]" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label15" SkinID="label" runat="server" Text="Reason for Not administered" />
                                                    </td>
                                                    <td>
                                                        <div id="divIVINFNotAdministered" runat="server" style="color: Red" visible="false">
                                                            *
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--<asp:CheckBox ID="chkIVINFNotAdministered" AutoPostBack="true" OnCheckedChanged="chkIVINFNotAdministered_OnCheckedChanged"
                                            runat="server" SkinID="checkbox" />--%>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtIVINFReasonForNotAdministered" runat="server" SkinID="textbox"
                                                Enabled="false" TextMode="MultiLine" Width="300px" Height="30px" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                
                                <td><asp:Literal ID="Literal2" runat="server" Text="Administration Status"></asp:Literal><span style="color: Red">*</span> </td>
                                <td colspan="4">                                                                             
                                             <asp:DropDownList ID="ddlIVINFYes" SkinID="DropDown" runat="server"  Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlIVINFYes_OnSelectedIndexChanged">
                                    <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Administered" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="Not Administered" Value="N"></asp:ListItem>
                                    </asp:DropDownList></td>
                                </tr>--%>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label26" SkinID="label" runat="server" Text="Reason for delay" />
                                                    </td>
                                                    <td>
                                                        <div id="divIVINFReasionDelay" runat="server" style="color: Red">
                                                            *
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtIVINFUReasonForDelay" runat="server" SkinID="textbox" TextMode="MultiLine"
                                                Width="300px" Height="30px" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label19" SkinID="label" runat="server" Text="Infusion Order" />
                                        </td>
                                        <td colspan="4">
                                            <asp:Label ID="lblInfusionOrder" SkinID="label" runat="server" Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" SkinID="label" runat="server" Text="Instruction By Doctor" />
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtIVINFInstruction" TextMode="MultiLine" Width="300px" Height="30px"
                                                ReadOnly="true" Enabled="false" SkinID="textbox" runat="server" Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>s
                                        <asp:Label ID="Label32" SkinID="label" runat="server" Text="Remarks" />
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtIVINFRemarks" TextMode="MultiLine" Width="350px" Height="30px"
                                                SkinID="textbox" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%-- <asp:Label ID="Label27" SkinID="label" runat="server" Text="Administered By" />--%>
                                            <asp:Label ID="Label28" SkinID="label" runat="server" Text="User ID" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIVINFUserName" SkinID="label" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label30" SkinID="label" runat="server" Text="Password" /><span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIVINFPassword" Width="100px" runat="server" SkinID="textbox"
                                                TextMode="Password"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trIVINFWitness" runat="server">
                                        <%-- <td >
                                        <asp:Label ID="Label31" SkinID="label" runat="server" Text="If high alert drug, witnessed by" />
                                    </td>--%>
                                        <td>
                                            <asp:Label ID="lblIVINFWitnessUserName" SkinID="label" runat="server" Text="Witness User ID ( In case of high alert drug )" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIVINFWitnessUseName" Width="50px" runat="server" SkinID="textbox" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" SkinID="label" runat="server" Text="Password" />
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIVINFWitnessPasswod" runat="server" Width="50px" SkinID="textbox"
                                                TextMode="Password" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                <asp:HiddenField ID="hdnxmlString" runat="server" Value="" />
                                <asp:HiddenField ID="hdnBeforeTime" runat="server" />
                                <asp:HiddenField ID="hdnIVINFUMedHangedTime" runat="server" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
