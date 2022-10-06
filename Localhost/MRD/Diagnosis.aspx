<%@ Page Language="C#" Theme="DefaultControls" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="Diagnosis.aspx.cs" Inherits="EMR_Diagnosis_Default"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="Left" Src="~/Include/Components/ucTree.ascx" %>
<%@ Register TagPrefix="asp1" TagName="Top1" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var ilimit = 60;
        function SearchPatientOnClientClose(oWnd, args) {
            $get('<%=btnfind.ClientID%>').click();
        }
        function AutoChange(txtRemarks) {
            var txt = document.getElementById(txtRemarks);

            if (txt.value.length >= 10) {
                if (txt.value.length >= 60 * txt.rows) {
                    txt.rows = txt.rows + 1;
                    ilimit = 0;
                }
                else if (txt.value.length < 60 * (txt.rows - 1)) {
                    txt.rows = Math.round(txt.value.length / 60) + 1;
                }
                else if (txt.value.length >= 500) {
                    txt.value.length = txt.value.substring(0, 500)
                    return false;
                }
                else {
                    if (txt.value.length <= ilimit * txt.rows && txt.rows >= ilimit) {
                        txt.cols = (txt.cols * 1) + 1;
                    }
                }
            }
            return true;
        }


        // Code Start for Quality Dropdown with Checkbox ------------------------
        //prevent default selection in the combobox and deselect all checkboxes
        var cancelDropDownClosing = false;
        function StopPropagation(e) {
            //cancel bubbling
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }
        window.onbeforeunload = function (evt) {
            var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            if (IsUnsave == 1) {
                return false;
            }
        }

        function On_ddlQualityClosing() {
            cancelDropDownClosing = false;
        }




        //this method removes the ending comma from a string
        function removeLastComma(str) {
            return str.replace(/,$/, "");
        }
        function OnClientDropDownClosingHandler(sender, e) {
            //do not close the second combo if 
            //a checkbox from the first is clicked
            e.set_cancel(cancelDropDownClosing);
        }



        function OnTextChange(sender) {
            $get('<%=txtIcdCodes.ClientID%>').value = '';
        }


    </script>

    <script language="javascript" type="text/javascript">
        function ValidateDate(CDate, CCurDate) {
            var arrEnteredDate = new Array();
            arrEnteredDate = $get(CDate).value.split("/");
            var dateEnteredString = arrEnteredDate[2].toString() + "/" + arrEnteredDate[1].toString() + "/" + arrEnteredDate[0].toString();  // yyyy/MM/dd
            var myEnteredDate = new Date(dateEnteredString);

            var arrCurrentDate = new Array();
            arrCurrentDate = $get(CCurDate).value.split("/");
            var dateCurrentString = arrCurrentDate[2].toString() + "/" + arrCurrentDate[1].toString() + "/" + arrCurrentDate[0].toString();  // yyyy/MM/dd
            var myCurrentDate = new Date(dateCurrentString);

            if (myEnteredDate > myCurrentDate) {
                alert("Date Cannot Be Greater Than Current Date");
                $get(CDate).focus();
                return false;
            }
            else {
                return true;
            }
        }

        function OnClientClose(oWnd, args) {

            $get('<%=btnGetCondition.ClientID%>').click();
        }
        function ddlDiagnosiss_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();

            var DiagICDID = item.get_attributes().getAttribute("ICDID");
            var DiagICDCode = item.get_attributes().getAttribute("ICDCode");

            var DiagICDDIscription = item.get_attributes().getAttribute("ICDDescription");

            $get('<%=hdnDiagnosisId.ClientID%>').value = DiagICDID;
            $get('<%=txtIcdCodes.ClientID%>').value = DiagICDCode;

            $get('<%=hdnDiagnosisId.ClientID%>').value = item != null ? item.get_value() : sender.value();

            $get('<%=btnCommonOrder.ClientID%>').click();
            //alert('test');
        }
        //Added By Ashutosh Prashar:13/05/2013
        //Confirmation Message box before Delete Button Press.
        function alertBeforeDelete() {
            confirm('Do you want to delete!');
        }
        //End Here

        function SearchPatientOnClientClose() {
            //window.close();
        }

    </script>

    <style type="text/css">
        .Gridheader {
            font-family: Verdana;
            background-image: url(/Images/header.gif);
            height: 24px;
            color: black;
            font-weight: normal;
            position: relative;
        }

        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        .form-group {
            padding-bottom: 23px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlRecordVisit" runat="server">
                <table width="100%" class="clsheader" style="display: none">
                    <tr>
                        <td>
                            <%--Patient Diagnosis(s)--%>
                        </td>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                        </td>
                        <td align="right">
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                                    <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="0" style="background-color: White">
                    <tr>
                        <td id="idhistory" runat="server" style="width: 100%;" valign="top">
                            <table cellpadding="0" cellspacing="0" width="50%">
                                <tr>
                                    <td align="left" valign="top" width="30%" style="padding-left: 10px;">
                                        <table cellpadding="0" cellspacing="0" width="840px">
                                            <tr>
                                                <td valign="top" style="padding-bottom: 5px; width: 95%;">
                                                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True">
                                                        <asp:ListItem Selected="True" Value="0">Diagnosis</asp:ListItem>
                                                        <asp:ListItem Value="1">Surgery/Procedure</asp:ListItem>
                                                        <asp:ListItem Value="2">Deficiency Entry(Both)</asp:ListItem>
                                                        <asp:ListItem Value="3">Deficiency Entry(OP)</asp:ListItem>
                                                        <asp:ListItem Value="4">Deficiency Entry(IP)</asp:ListItem>
                                                        <asp:ListItem Value="5">Other </asp:ListItem>
                                                        <asp:ListItem Value="6">MLC & Legal Details</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="width: 5%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Button ID="btnSaveMRDFile" runat="server"  BackColor="#FFCBA4" CssClass="buttonBlue"
                                                        OnClick="btnSaveMRDFile_Click" Text="Save File (Alt+S)" ToolTip="Save MRD File" AccessKey="S"  />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnl3" runat="server">
                                                        <asp:Label runat="server" BackColor="LightBlue" Font-Bold="true" Text="Deficiency Entry" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnDeficiency" runat="server" BackColor="#FFCBA4" Font-Bold="true"
                                                        OnClick="btnDeficiency_OnClick" Text="Save Deficiency" />
                                                        <%--raghuvir--%>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="Label2" runat="server" BackColor="LightBlue" Font-Bold="true" Text="Select Deficiency" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <telerik:RadComboBox ID="ddlAddDeficiency" AppendDataBoundItems="true" Filter="Contains"
                                                        Width="400px" runat="server" />
                                                        <asp:Button ID="btnAddDeficiency" runat="server" BackColor="#FFCBA4" Font-Bold="true"
                                                            OnClick="btnAddDeficiency_OnClick" Text="Add" />
                                                        <br />
                                                        <asp:Panel ID="pnlvvr" runat="server" Width="100%" Height="150px" ScrollBars="Auto"
                                                            BorderWidth="1px" BorderColor="LightBlue">
                                                            <asp:GridView ID="gvDeficiency" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                SkinID="gridview" OnRowDataBound="gvDeficiency_OnRowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Form Name" HeaderStyle-Width="140px" ItemStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFormName" runat="server" SkinID="Label" Text='<%#Eval("FormName") %>' />
                                                                            <asp:HiddenField ID="hdnQualityFormId" runat="server" Value='<%#Eval("QualityFormId") %>' />
                                                                            <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("OpenStatus") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" Text='<%#Eval("Remarks") %>'
                                                                                ToolTip='<%#Eval("Remarks") %>' TextMode="MultiLine" Width="100%" Height="20px" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px" ItemStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDown">
                                                                                <asp:ListItem Text="Active" Value="1" Selected="True" />
                                                                                <asp:ListItem Text="InActive" Value="0" />
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Open/Close" HeaderStyle-Width="70px" ItemStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlActive" runat="server" SkinID="DropDown">
                                                                                <asp:ListItem Text="Open" Value="1" Selected="True" />
                                                                                <asp:ListItem Text="Close" Value="0" />
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" BackColor="LightBlue" Font-Bold="true" Text="Added By Consultant Doctor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="Label3" runat="server" BackColor="LightBlue" Font-Bold="true" Text="CPT Code"></asp:Label>
                                                            &nbsp;
                                                        <telerik:RadComboBox ID="ddlCPTCode" Filter="Contains" AutoPostBack="false" EnableLoadOnDemand="true"
                                                            Width="500px" runat="server" OnItemsRequested="ddlCPTCode_OnItemsRequested" />
                                                            <%-- <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="false" DataTextField="DISPLAY_NAME"
                                                        DataValueField="DiagnosisId" DropDownWidth="500" EmptyMessage="Search Diagnosis by Text"
                                                        EnableLoadOnDemand="true" EnableVirtualScrolling="true" Height="300px" HighlightTemplatedItems="true"
                                                        OnClientSelectedIndexChanged="ddlDiagnosiss_OnClientSelectedIndexChanged" OnClientTextChange="OnTextChange"
                                                         ShowMoreResultsBox="true" Width="300px">--%>
                                                            <%--raghuvir--%>
                                                            <asp:Button ID="btnTag" runat="server" BackColor="#FFCBA4" Font-Bold="true" OnClick="btnTag_OnClick"
                                                                Text="Tag" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr id="TrgvDiagnosis" runat="server">
                                                <td valign="top">
                                                    <asp:GridView ID="gvDiagnosis" Width="100%" runat="server" AutoGenerateColumns="false"
                                                        SkinID="gridview">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="ICD Code" HeaderStyle-Width="150px" DataField="ICDCode" />
                                                            <asp:BoundField HeaderText="Description" DataField="Description" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr id="TrgvSurgeryProcedure" runat="server">
                                                <td valign="top">
                                                    <asp:GridView ID="gvSurgeryProcedure" Width="100%" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvSurgeryProcedure_RowDataBound"
                                                        SkinID="gridview" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvSurgeryProcedure_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Service Type" HeaderStyle-Width="150px" DataField="ServiceType">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <%--   <asp:BoundField HeaderText="Service Name" DataField="ServiceName" />--%>
                                                            <asp:TemplateField HeaderText="Service Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnDetailid" runat="server" Value='<%#Eval("detailId")%>' />
                                                                    <asp:HiddenField ID="hdnMrdCptId" runat="server" Value='<%#Eval("MrdCptId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Package Name" DataField="PackageName" />
                                                            <asp:BoundField HeaderText="Doctor Name" DataField="DoctorName" />
                                                            <asp:BoundField HeaderText="Surgery Date" DataField="SurgeryDate" />

                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr id="trBirthReg" runat="server" visible="true">
                                                <td>
                                                    <asp:Panel ID="pnlBirthRegDetail" runat="server">
                                                        <div class="container-fluid header_main" style="padding-left: 0px;">
                                                            <div style="float: left; text-align: left">
                                                                <asp:Label ID="Label4" runat="server" Font-Bold="true" Font-Size="Medium" Text="Birth Register Detail"></asp:Label>
                                                                &nbsp;
                                                           <%-- <asp:Button ID="btnSaveDetail" runat="server" OnClick="btnSaveDeathOnlineReg_Click" Text="Save Detail" BackColor="#FFCBA4" CssClass="buttonBlue" />--%>
                                                            </div>
                                                            <div class="col-md-8 col-sm-8 col-xs-8 text-right pull-right" style="padding-left: 0px;">
                                                                <asp:Button ID="btnSaveDetail" runat="server" OnClick="btnSaveDeathOnlineReg_Click" Text="Save Detail" BackColor="#FFCBA4" CssClass="buttonBlue" />
                                                            </div>
                                                        </div>
                                                        &nbsp;
                                                        <div class="row">
                                                            <div class="form-group">
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="lblBirthRegNo" runat="server" Text="Online Reg. No:"></asp:Label>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <asp:TextBox ID="txtBirthRegno" runat="server" CssClass="Textbox" Width="100%"></asp:TextBox>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="lblbirthRegdate" runat="server" Text="Online Reg. Date:"></asp:Label>
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <telerik:RadDatePicker ID="dtonlinebirthRegDate" DateInput-ReadOnly="false" DatePopupButton-Visible="false" Calendar-EnableAjaxSkinRendering="True" ShowPopupOnFocus="true" runat="server"></telerik:RadDatePicker>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="Label8" runat="server" Text="Delivery Type:"></asp:Label>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <telerik:RadComboBox ID="ddlDeliveryType" MarkFirstMatch="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" runat="server" Width="100%"></telerik:RadComboBox>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="Label9" runat="server" Text="Delivery Date:"></asp:Label>
                                                                </div>
                                                                <div class="col-sm-4">

                                                                    <telerik:RadDateTimePicker ID="dtpDeliveryDateTime" runat="server" AutoPostBackControl="Both"
                                                                        DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Width="50px">
                                                                    </telerik:RadComboBox>


                                                                </div>

                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="lblMother2158" runat="server" Text="Weight(Kg)" />
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <asp:TextBox ID="txtWeight" runat="server" MaxLength="7" CssClass="Textbox" />
                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender12552" runat="server" Enabled="True"
                                                                        FilterType="Custom,Numbers" TargetControlID="txtWeight" ValidChars="0123456789." />
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="lblMother2159" runat="server" Text="Height(cm)" />
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:TextBox ID="txtHeight" runat="server" CssClass="Textbox" MaxLength="7" />
                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender12553" runat="server" Enabled="True"
                                                                        FilterType="Custom,Numbers" TargetControlID="txtHeight" ValidChars="0123456789." />
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <asp:Label ID="Label10" runat="server" Text="Gender" />
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:DropDownList ID="rcbGender" runat="server" CssClass="RegText" Skin="Outlook" TabIndex="4" ValidationGroup="Save" Width="100%">
                                                                        <asp:ListItem Text=" " Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="<%$ Resources:PRegistration, MALE%>" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="<%$ Resources:PRegistration, FEMALE%>" Value="1"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>


                                                            <div class="form-group">

                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="Label11" runat="server" Text="Gestational Week :"></asp:Label>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <telerik:RadComboBox ID="rcbGestationalWeek" runat="server" AutoPostBack="True" Height="300px"
                                                                        OnSelectedIndexChanged="rcbGestationalWeek_SelectedIndexChanged" Width="100%">
                                                                    </telerik:RadComboBox>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:Label ID="Label12" runat="server" Text="Other Specification "></asp:Label>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <asp:TextBox ID="txtOtherSpecification" runat="server" CssClass="Textbox"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr id="trDeathReg" runat="server" visible="false">
                                                <td>
                                                    <asp:Panel ID="pnlDeatReg" runat="server" Width="100%">
                                                        <%--<div id="tblDeatReg" runat="server" style="text-align: left;">--%>
                                                        <div class="container-fluid header_main" style="padding-left: 0px;">
                                                            <div class="col-sm-8 text-left" style="padding-left: 0px;">
                                                                <asp:Label ID="Label5" runat="server" Font-Bold="true" Font-Size="Medium" Text="Death Register Detail"></asp:Label>
                                                                &nbsp;
                                                            <%--<asp:Button ID="btnSaveDeathOnlineReg" runat="server" OnClick="btnSaveDeathOnlineReg_Click" Text="Save Detail" BackColor="#FFCBA4" CssClass="buttonBlue" />--%>
                                                            </div>
                                                            <div class="col-sm-2 text-right pull-right" style="padding-left: 0px;">
                                                                <asp:Button ID="btnDeathOnlineReg" runat="server" OnClick="btnSaveDeathOnlineReg_Click" Text="Save Detail" BackColor="#FFCBA4" CssClass="buttonBlue" />
                                                            </div>

                                                        </div>
                                                        &nbsp;
                                                        <div class="row form-group">
                                                            <div class="col-sm-6">
                                                                <div class="row">
                                                                    <div class="col-sm-6">
                                                                        <asp:Label ID="lblOnLineDeathregistrationNo" Font-Bold="true" runat="server" Text="Online Registration No   :"></asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-6" style="padding-left: 0px;">
                                                                        <asp:TextBox ID="txtonlineregNo" runat="server" CssClass="Textbox" Width="100%"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <div class="col-sm-2">
                                                                </div>
                                                                <div class="col-sm-10">
                                                                    <asp:CheckBox ID="chkDeathRelated" Text="Death Related to Pregnancy/Maternal Related:" runat="server" />

                                                                </div>

                                                            </div>
                                                            <div class="col-sm-6" style="padding-top: 5px;">
                                                                <div class="row">
                                                                    <div class="col-sm-6">
                                                                        <asp:Label ID="Label6" runat="server" Font-Bold="true" Text="Online Registration Date :"></asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-6" style="padding-left: 0px;">
                                                                        <telerik:RadDatePicker ID="drdponlineDeathRegDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" Calendar-EnableAjaxSkinRendering="True" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6" style="padding-top: 5px;">
                                                                <div class="col-sm-2">
                                                                </div>
                                                                <div class="col-sm-10">
                                                                    <asp:CheckBox ID="chkInfantDeath" Text="Infant Death (0 -12 Months):" runat="server" />

                                                                </div>

                                                            </div>
                                                            <div class="col-sm-6" style="padding-top: 5px;">
                                                                <div class="row" style="display: none">
                                                                    <div class="col-sm-6">
                                                                        <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Anesthesia related Death :"></asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-6" style="padding-left: 0px;">
                                                                        <%--<asp:CheckBox ID="chkAnesthesiarelatedDeath" runat="server" AutoPostBack="true" OnCheckedChanged="chkAnesthesiarelatedDeath_CheckedChanged" />--%>
                                                                        <asp:DropDownList ID="rcbAnesthesiarelatedDeath" runat="server" Width="100%">
                                                                            <asp:ListItem Text="" Value="2" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-sm-6" style="padding-top: 5px;">

                                                                <div class="col-sm-2">
                                                                </div>
                                                                <div class="col-sm-10">
                                                                    <asp:CheckBox ID="chkAnesthesiarelatedDeath" Text="Anesthesia related Death" runat="server" />

                                                                </div>

                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>


                                            <tr id="trMLCDetails" runat="server" visible="false">
                                                <td>
                                                    <asp:Panel ID="pnlMLCDetails" runat="server" Width="100%">

                                                        <div class="container-fluid header_main" style="padding-left: 0px;">
                                                            <div class="col-sm-8 text-left" style="padding-left: 0px;">
                                                                <asp:Label ID="Label13" runat="server" Font-Bold="true" Font-Size="Medium" Text="MLC Patient Detail"></asp:Label>

                                                                <%--<asp:Button ID="btnSaveDeathOnlineReg" runat="server" OnClick="btnSaveDeathOnlineReg_Click" Text="Save Detail" BackColor="#FFCBA4" CssClass="buttonBlue" />--%>
                                                            </div>
                                                            <div class="col-sm-2 text-right pull-right" style="padding-left: 0px;">
                                                                <asp:Button ID="btnMLCDetails" runat="server" Text="Save MLC Detail" BackColor="#FFCBA4" OnClick="btnMLCDetails_Click" CssClass="buttonBlue" />
                                                            </div>

                                                        </div>

                                                        &nbsp;
                                                     <div class="row form-group">
                                                         <div class="col-sm-6">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="Label14" Font-Bold="true" runat="server" Text="Received Date:"></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <telerik:RadDateTimePicker ID="rdtReceivedDate" runat="server" AutoPostBackControl="Both"
                                                                         DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                                                 </div>
                                                             </div>
                                                         </div>

                                                         <div class="col-sm-6">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="Label15" Font-Bold="true" runat="server" Text="PI No.:"></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <asp:TextBox ID="txtPIno" runat="server" CssClass="Textbox"></asp:TextBox>
                                                                 </div>
                                                             </div>
                                                         </div>

                                                         <div class="col-sm-6">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="Label16" Font-Bold="true" runat="server" Text="MLR No."></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <asp:TextBox ID="txtmlrno" runat="server" CssClass="Textbox"></asp:TextBox>
                                                                 </div>
                                                             </div>
                                                         </div>

                                                         <div class="col-sm-6">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="Label17" Font-Bold="true" runat="server" Text="Remark : "></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine" CssClass="Textbox"></asp:TextBox>
                                                                 </div>
                                                             </div>
                                                         </div>
                                                     </div>
                                                    </asp:Panel>

                                                    <asp:Panel ID="pnlLegalDetails" runat="server" Width="100%">

                                                        <div class="container-fluid header_main" style="padding-left: 0px;">
                                                            <div class="col-sm-8 text-left" style="padding-left: 0px;">
                                                                <asp:Label ID="Label18" runat="server" Font-Bold="true" Font-Size="Medium" Text="Legal Patient Details"></asp:Label>
                                                                &nbsp;
                                                            <%--<asp:Button ID="btnSaveDeathOnlineReg" runat="server" OnClick="btnSaveDeathOnlineReg_Click" Text="Save Detail" BackColor="#FFCBA4" CssClass="buttonBlue" />--%>
                                                            </div>
                                                            <div class="col-sm-2 text-right pull-right" style="padding-left: 0px;">
                                                                <asp:Button ID="btnSaveLegalDetail" runat="server" Text="Save Legal Detail" BackColor="#FFCBA4" CssClass="buttonBlue" OnClick="btnSaveLegalDetail_Click" />
                                                            </div>

                                                        </div>
                                                        &nbsp;
                                                     <div class="row form-group">
                                                         <div class="col-sm-6">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="lblCaseBetween" Font-Bold="true" runat="server" Text="Case Between:"></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <asp:TextBox ID="txtCaseBetween" runat="server" CssClass="Textbox"></asp:TextBox>
                                                                 </div>
                                                             </div>
                                                         </div>

                                                         <div class="col-sm-6">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="Label19" Font-Bold="true" runat="server" Text="Name of Judge:"></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <asp:TextBox ID="txtNameofJudge" runat="server" CssClass="Textbox"></asp:TextBox>
                                                                 </div>
                                                             </div>
                                                         </div>

                                                         <div class="col-sm-6" style="padding-top: 10px;">
                                                             <div class="row">
                                                                 <div class="col-sm-6">
                                                                     <asp:Label ID="Label20" Font-Bold="true" runat="server" Text="Date of Evidance:"></asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-6" style="padding-left: 0px;">
                                                                     <telerik:RadDateTimePicker ID="rdtDateofEvidance" runat="server" AutoPostBackControl="Both"
                                                                         DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                                                 </div>
                                                             </div>
                                                         </div>
                                                     </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="50%" class="clssubtopicbar">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="50%">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="50%">
                                        <asp:Panel ID="pnl1" runat="server">
                                            <table id="tbl1" runat="server" cellpadding="0" cellspacing="0" width="800px">
                                                <tr>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Literal ID="Literal1" runat="server" Text="Group"></asp:Literal>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                            Skin="Outlook" Width="300px" Style="height: 22px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Literal ID="Literal3" runat="server" Text="Sub Group"></asp:Literal>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <telerik:RadComboBox ID="ddlSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged"
                                                            Skin="Outlook" Width="300px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Literal ID="Literal2" runat="server" Text="Diagnosis"></asp:Literal>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <telerik:RadComboBox ID="ddlDiagnosiss" runat="server" AutoPostBack="false" DataTextField="DISPLAY_NAME"
                                                            DataValueField="DiagnosisId" DropDownWidth="500" EmptyMessage="Search Diagnosis by Text"
                                                            EnableLoadOnDemand="true" EnableVirtualScrolling="true" Height="300px" HighlightTemplatedItems="true"
                                                            OnClientSelectedIndexChanged="ddlDiagnosiss_OnClientSelectedIndexChanged" OnClientTextChange="OnTextChange"
                                                            OnItemsRequested="ddlDiagnosiss_OnItemsRequested" ShowMoreResultsBox="true" Width="300px">
                                                            <HeaderTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>Diagnosis Display Name
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td id="Td4" runat="server" visible="true" style="width: 80px">
                                                                            <%# DataBinder.Eval(Container, "Attributes['ICDCode']")%>
                                                                        </td>


                                                                        <td align="left">
                                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                                        </td>
                                                                        <td id="Td1" runat="server" visible="false">
                                                                            <%# DataBinder.Eval(Container, "Attributes['ICDID']")%>
                                                                        </td>
                                                                        <td id="Td2" runat="server" visible="false">
                                                                            <%# DataBinder.Eval(Container, "Attributes['ICDCode']")%>
                                                                        </td>
                                                                        <td id="Td3" runat="server" visible="false">
                                                                            <%# DataBinder.Eval(Container, "Attributes['ICDDescription']")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Literal ID="Literal5" runat="server" Text="ICD Code"></asp:Literal>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Panel ID="pnlDiagnoSearch" runat="server" DefaultButton="btnSearchICDCode">
                                                            <asp:TextBox ID="txtIcdCodes" runat="server" CssClass="Textbox"></asp:TextBox>
                                                            &nbsp;<asp:Button ID="btnSearchICDCode" runat="server" OnClick="btnSearchICDCode_Click"
                                                                SkinID="Button" Text="Search By ICD" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">&nbsp;
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <%--   <asp:Button ID="btnExtenalEdu0" runat="server" OnClick="btnExtenalEdu_Click" 
                                                    SkinID="Button" Text="Education Materials" ToolTip="External Education" 
                                                    ValidationGroup="CreateGroup" />--%>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Button ID="btnCommonOrder" runat="server" OnClick="btnCommonOrder_OnClick" Style="visibility: hidden;"
                                                            Width="10px" />
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                       <%-- <asp:Button ID="btnAddtogrid" runat="server" BackColor="#FFCBA4" Font-Bold="true"
                                                            OnClick="btnAddtogrid_Click" Text="Add To List" />--%>

                                                         <asp:Button ID="btnAddtogrid" runat="server" BackColor="#FFCBA4" CssClass="buttonBlue"
                                                            OnClick="btnAddtogrid_Click" Text="Add To List" />
                                                    </td>                                                 
                                                </tr>
                                                <tr style="display: none">
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Label ID="Lablefacility" runat="server" SkinID="label" Text="Facility" Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <telerik:RadComboBox ID="ddlFacility" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"
                                                            Skin="Outlook" Visible="false" Width="120px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <asp:Label ID="Lable1" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, Doctor%>"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 10px; padding-bottom: 5px;" valign="top">
                                                        <telerik:RadComboBox ID="ddlProviders" runat="server" AutoPostBack="false" DropDownWidth="200px"
                                                            Skin="Outlook" Visible="false" Width="150px">
                                                        </telerik:RadComboBox>
                                                        <asp:CheckBox ID="chkPrimarys" runat="server" Checked="True" Text="Primary" TextAlign="Right" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="50%">
                                        <asp:Panel ID="pnl2" runat="server">
                                            <table id="tbl2" runat="server" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <asp:Label ID="lblMrdLabel" runat="server" BackColor="LightBlue" Font-Bold="true"
                                                            Text="Added By MRD "></asp:Label>
                                                        <div id="dvDiagnosis" runat="server" style="text-align: left; height: 200px">
                                                            <asp:Panel ID="pnlgrid" runat="server" Width="100%">
                                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvDiagnosisDetails" runat="server" AutoGenerateColumns="False"
                                                                            HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDiagnosisDetails_RowCommand"
                                                                            OnRowDataBound="gvDiagnosisDetails_RowDataBound" OnSelectedIndexChanged="gvDiagnosisDetails_SelectedIndexChanged"
                                                                            SkinID="gridview" Width="800px">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="30px" HeaderText="ICD Code" ItemStyle-Width="30px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="30px" />
                                                                                    <ItemStyle Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="300px" HeaderText="Diagnosis" ItemStyle-Width="300px"
                                                                                    ItemStyle-Wrap="true">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="300px" />
                                                                                    <ItemStyle Width="300px" Wrap="True" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Side">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="30px" Visible="false" HeaderText="Primary"
                                                                                    ItemStyle-Width="30px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="30px" />
                                                                                    <ItemStyle Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Chronic">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>'></asp:Label>
                                                                                        <asp:HiddenField ID="hdnIsQuery" runat="server" Value='<%#Eval("IsQuery") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Type">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Condition">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Resolved">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Provider">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Onset Date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>'></asp:Label>
                                                                                        <asp:DropDownList ID="ddlProvider" runat="server" AppendDataBoundItems="true" SkinID="DropDown"
                                                                                            Visible="false">
                                                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <asp:HiddenField ID="HdnOnsetDate" runat="server" Value='<%#Eval("OnsetDateWithoutFormat") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Facility">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>'></asp:Label>
                                                                                        <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="true" SkinID="DropDown"
                                                                                            Visible="false">
                                                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Remarks">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:CommandField ButtonType="Link" CausesValidation="false" ControlStyle-Font-Underline="true"
                                                                                    ControlStyle-ForeColor="Blue" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                                                    HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" SelectText="Edit"
                                                                                    ShowSelectButton="true">
                                                                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:CommandField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                                                    HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                                            ToolTip="Delete" Width="13px" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:GridView>
                                                                        <table border="0" cellpadding="0" cellspacing="2">
                                                                            <tr>
                                                                                <td>
                                                                                    <div id="dvConfirmDeletion" runat="server" style="width: 200px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 60px; left: 300px; top: 150px"
                                                                                        visible="false">
                                                                                        <table cellpadding="0" cellspacing="2" width="100%">
                                                                                            <tr>
                                                                                                <td align="center" colspan="3">
                                                                                                    <asp:HiddenField ID="hdnlblId" runat="server" Value="0" />
                                                                                                    <strong>Delete?</strong>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="3">&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center"></td>
                                                                                                <td align="center">
                                                                                                    <asp:Button ID="btnYes" runat="server" OnClick="btnDeletion_OnClick" SkinID="Button"
                                                                                                        Text="Yes" />
                                                                                                    &nbsp;
                                                                                                <asp:Button ID="btnNo" runat="server" OnClick="btnCancelDeletion_OnClick" SkinID="Button"
                                                                                                    Text="No" />
                                                                                                </td>
                                                                                                <td align="center"></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </asp:Panel>
                                                            <asp:CheckBox ID="chkPullDiagnosis" runat="server" Text="Pull Forward From Prior Exam"
                                                                TextAlign="Right" Visible="false" />
                                                            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                                                                <Windows>
                                                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                                                    </telerik:RadWindow>
                                                                </Windows>
                                                            </telerik:RadWindowManager>
                                                            <asp:TextBox ID="hdn_DRUG_SYN_ID" runat="server" Style="visibility: hidden;" Width="30px"></asp:TextBox>
                                                            <asp:TextBox ID="hdn_DRUG_ID" runat="server" Style="visibility: hidden;" Width="30px" />
                                                            <asp:TextBox ID="hdn_GENPRODUCT_ID" runat="server" Style="visibility: hidden;" Width="30px" />
                                                            <asp:TextBox ID="hdn_SYNONYM_TYPE_ID" runat="server" Style="visibility: hidden;"
                                                                Width="30px"></asp:TextBox>
                                                            <asp:TextBox ID="lbl_DISPLAY_NAME" runat="server" Style="visibility: hidden;" Width="30"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnProblems" runat="server" />
                                                            <asp:HiddenField ID="hdnRowIndex" runat="server" />
                                                            <asp:TextBox ID="txtid" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                                            <asp:TextBox ID="txtIcdId" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnDiagnosisId" runat="server" />
                                                            <asp:HiddenField ID="hdnGenericId" runat="server" />
                                                            <asp:HiddenField ID="hdnCurrentRowId" runat="server" />
                                                            <asp:TextBox ID="txtstatusIds" runat="server" SkinID="textbox" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                                            <asp:Button ID="btnGetCondition" runat="server" CausesValidation="false" OnClick="btnGetCondition_Click"
                                                                Style="visibility: hidden;" />
                                                        </div>
                                                        <div id="dvSurgey" runat="server" style="text-align: left; height: 200px">
                                                            <asp:Panel ID="pnlSurgery" runat="server" Width="100%">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvSurgery" runat="server" AutoGenerateColumns="False"
                                                                            HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvSurgery_RowCommand"
                                                                            OnRowDataBound="gvSurgery_RowDataBound" OnSelectedIndexChanged="gvSurgery_SelectedIndexChanged"
                                                                            SkinID="gridview" Width="800px">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="30px" HeaderText="CPT Code" ItemStyle-Width="50px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCPTCode" runat="server" Text='<%#Eval("CPTCode") %>'></asp:Label>
                                                                                        <asp:HiddenField ID="hdnCPTId" runat="server" Value='<%#Eval("CPTId") %>' />
                                                                                        <asp:HiddenField ID="hdnOrderDetailId" runat="server" Value='<%#Eval("OrderDetailId") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="50px" />
                                                                                    <ItemStyle Width="50px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="45%" HeaderText="Description"
                                                                                    ItemStyle-Wrap="true">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblcptDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="300px" />
                                                                                    <ItemStyle Width="300px" Wrap="True" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="45%" HeaderText="Service Name"
                                                                                    ItemStyle-Wrap="true">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="300px" />
                                                                                    <ItemStyle Width="300px" Wrap="True" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                                                    HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                                            ToolTip="Delete" Width="13px" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:GridView>
                                                                        <table border="0" cellpadding="0" cellspacing="2">
                                                                            <tr>
                                                                                <td>
                                                                                    <div id="Div1" runat="server" style="width: 200px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 60px; left: 300px; top: 150px"
                                                                                        visible="false">
                                                                                        <table cellpadding="0" cellspacing="2" width="100%">
                                                                                            <tr>
                                                                                                <td align="center" colspan="3">
                                                                                                    <asp:HiddenField ID="hdnCPTid" runat="server" Value="0" />
                                                                                                    <strong>Delete?</strong>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="3">&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center"></td>
                                                                                                <td align="center">
                                                                                                    <asp:Button ID="btnDeletioncpt" runat="server" OnClick="btnDeletioncpt_OnClick" SkinID="Button"
                                                                                                        Text="Yes" />
                                                                                                    &nbsp;
                                                                                                <asp:Button ID="btnCancelDeletioncpt" runat="server" OnClick="btnCancelDeletioncpt_OnClick" SkinID="Button"
                                                                                                    Text="No" />
                                                                                                </td>
                                                                                                <td align="center"></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </asp:Panel>

                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
