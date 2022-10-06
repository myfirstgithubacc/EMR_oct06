<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ComponentRequisition.aspx.cs" Inherits="BloodBank_SetupMaster_ComponentRequisition" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
   
    <script type="text/javascript">
        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "SteelBlue";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "SteelBlue";
        }

        function openRadWindow(Purl, wHeight, wWidth) {
            var oWnd = radopen(Purl);
            oWnd.setSize(wHeight, wWidth);
            oWnd.set_modal(true);
            oWnd.set_visibleStatusbar(false);
            oWnd.Center();
        }
        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

                <script src="../../Include/JS/jsBloodGroup.js" type="text/javascript"></script>

                <script language="JavaScript" type="text/javascript">
                    function SearchPatientOnClientClose(oWnd, args) {
                        var arg = args.get_argument();
                        if (arg) {
                            var RegistrationId = arg.RegistrationId;
                            var RegistrationNo = arg.RegistrationNo;
                            $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                            $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                        }

                        $get('<%=btnGetInfo.ClientID%>').click();
                    }

                    function FillDetailsForComponentRequisitionOnClientClose(oWnd, args) {
                        var arg = args.get_argument();
                        if (arg) {
                            var RequisitionId = arg.Requisition;
                            var Acknowledge = arg.Acknowledge;
                            $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                            $get('<%=hdnAcknowledge.ClientID%>').value = Acknowledge;
                        }

                        $get('<%=btnGetRequisitionInfo.ClientID%>').click();
                    }

                    function FillComponentGridOnClientClose(oWnd, args) {
                        $get('<%=btnGetInfoForComponent.ClientID%>').click();
                    }
                    function OTHERHOSPITALCLOSE(oWnd, args) {
                        $get('<%=btnGetOTHERHOSPITAL.ClientID%>').click();
                    }


                    function change(id) {
                        identity = document.getElementById(id);
                        identity.style.border = "2px solid #FF0000";
                    }
                    function CalCulateDOB(sender, e) {
                        if (e.get_newDate() != null) {
                            var todaysDate = new Date(); // gets current date/time
                            var dateTimePicker = $find("<%=dtpDateOfBirth.ClientID%>"); // gets selected date/time

                            if (dateTimePicker.get_selectedDate() > todaysDate) {
                                alert("Date Of Birth Cannot Be Greater Than Current Date..");
                                return;
                            }
                            else {
                                document.getElementById('ctl00_ContentPlaceHolder1_imgCalYear').click();
                            }
                        }
                    }
                    function ShowError(sender, args) {
                        alert("Enter a Valid Date");
                        sender.focus();
                    }
                    function CalCulateAge() {
                        var txtYear = document.getElementById('<%=txtYear.ClientID %>').value;
                        var txtMonth = document.getElementById('<%=txtMonth.ClientID %>').value;
                        var txtDays = document.getElementById('<%=txtDays.ClientID %>').value;

                        if (txtYear != "") {
                            if (txtYear > 120) {
                                alert("Year Cannot Be Greater Than 120");
                                document.getElementById('<%=txtYear.ClientID %>').value = '';
                                document.getElementById('<%=txtYear.ClientID %>').focus()
                                return true; //return false;
                            }

                        }
                        if ((txtYear != "") || ((txtMonth != "")) || ((txtDays != ""))) {
                            document.getElementById('ctl00_ContentPlaceHolder1_id_btnCalAge').click();
                            document.getElementById('ctl00_ContentPlaceHolder1_id_dropSex').focus();
                        }
                    }
                    function MaxLenTxt(TXT) {
                        if (TXT.value.length > 15) {
                            alert("Text length should not be greater than 150 characters ...");

                            TXT.value = TXT.value.substring(0, 500);
                            TXT.focus();
                        }
                    }
                </script>
                <script type="text/javascript">
                    if (window.captureEvents) {
                        window.captureEvents(Event.KeyUp);
                        window.onkeyup = executeCode;

                    }
                    else if (window.attachEvent) {
                        document.attachEvent('onkeyup', executeCode);
                    }
                    function executeCode(evt) {
                        if (evt == null) {
                            evt = window.Event;
                        }
                        var theKey = parseInt(evt.keyCode, 10);
                        switch (theKey) {
                            case 113: //F2
                                $get('<%=btnNew.ClientID%>').click();
                                break;
                            case 114:  // F3
                                $get('<%=btnSaveData.ClientID%>').click();
                                break;
                            case 119:  // F8
                                $get('<%=btnclose.ClientID%>').click();
                                break;
                            case 120:  // F9
                                $get('<%=btnprint.ClientID%>').click();
                                break;


                        }
                        evt.returnValue = false;
                        return false;

                    }
                </script>
                <style type="text/css">
                    span#ctl00_ContentPlaceHolder1_lblPatientDetail {
                        font-weight: 700 !important;
                    }

                    b, strong {
                        font-weight: 700 !important;
                    }

                    .gridview-inner-table table th {
                        background: #fdcc6a !important;
                    }

                    .box-col-checkbox td input {
                        margin: 2px 0px !important;
                    }

                    span#ctl00_ContentPlaceHolder1_lblMessage {
                        margin: 0 !important;
                        width: 100%;
                    }

                    @media screen and (min-width: 800px) {
                        .mainContainer {
                           overflow-y:scroll;
                           height:550px;
                              
                        }
                    }
                </style>
            </telerik:RadScriptBlock>

            <div class="container-fluid mainContainer">
                <div class="row header_main">
                    <div class="col-md-3 col-sm-4 col-4" id="Td1" runat="server">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text='<%$ Resources:PRegistration, ComReq%>' ToolTip="" /></h2>
                    </div>
                    <div class="col-md-4 col-sm-8 col-8 text-left">
                        <asp:Image ID="imgProgress" runat="server" ImageUrl="~/Images/progressbar.gif" Style="display: none" />
                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                            <div class="row">

                                <div class="col-md-5 col-sm-5 col-5 text-nowrap">
                                    <asp:Button ID="btnGetInfo" runat="server" CausesValidation="false" Enabled="true"
                                        OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden; height: 1px; float: left;" Text="Assign"
                                        Width="10px" />
                                    <asp:Button ID="btnGetInfoForComponent" runat="server" CausesValidation="false" Enabled="true"
                                        OnClick="btnGetInfoForComponent_Click" SkinID="button" Style="visibility: hidden; height: 1px; float: left;"
                                        Text="Assign" Width="10px" />
                                    <asp:Button ID="btnGetOTHERHOSPITAL" runat="server" CausesValidation="false" Enabled="true"
                                        OnClick="btnGetOTHERHOSPITAL_Click" SkinID="button" Style="visibility: hidden; height: 1px; float: left;"
                                        Text="Assign" Width="10px" />
                                    <asp:Button ID="btnGetRequisitionInfo" runat="server" CausesValidation="false" Enabled="true"
                                        OnClick="btnGetRequisitionInfo_Click" SkinID="button" Style="visibility: hidden; height: 1px; float: left;"
                                        Text="Assign" Width="10px" />
                                    <asp:HiddenField ID="hdnUniqueIdOuter" runat="server" Value='<%# Eval("id") %>' />
                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value="0" />
                                    <asp:LinkButton ID="lbtnSearchRequisition" runat="server" Text='<%$ Resources:PRegistration, reqno%>' Font-Underline="false" ToolTip="Click to search donor" OnClick="lbtnSearchRequisition_Click"></asp:LinkButton>
                                </div>
                                <div class="col-sm-7 col-sm-7 col-6">
                                    <asp:TextBox ID="txtReqNo" runat="server" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="col-md-5 col-sm-12 col-xs-12 text-right mt-2 mt-md-0">
                        <asp:Button ID="btnNew" runat="server" ToolTip="New Record (F2)" CssClass="btn btn-primary" Text="New(F2)" OnClick="btnNew_OnClick" />
                        <asp:Button ID="btnCancelRequisition" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancelRequisition_Click" Visible="false" />
                        <asp:Button ID="btnprint" runat="server" ToolTip="Print(F9)" Text="Print(F9)" CssClass="btn btn-primary" OnClick="btnprint_Click" />
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data (F3)" OnClick="btnSaveData_OnClick" CssClass="btn btn-primary" Text="Save(F3)" />
                        <asp:Button ID="btnclose" Text="Close " runat="server" ToolTip="Close (F8)" CausesValidation="false" CssClass="btn btn-primary" OnClientClick="window.close();" OnClick="btnclose_Click" />
                    </div>
                </div>
                <div class="row" style="background: #fffaf0;">
                    <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                        <asp:Xml ID="xmlPatientInfo" runat="server"></asp:Xml>
                    </div>
                </div>
                <div class="row text-center" style="margin-bottom: 12px!important;">
                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="" />
                </div>
                <div class="row">
                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                        ShowSummary="False" ValidationGroup="Save" />
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border: 1px solid #ccc;">
                            <div class="row">
                                <h2 style="background: #c1e5ef; color: #000; font-size: 13px; padding: 4px 10px; margin: 0px; width: 100%;">
                                    <asp:Label ID="Label33" runat="server" Text="Last Issuing Details:" /></h2>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">

                                            <asp:Label ID="Label34" runat="server" Text="Issue Date" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtIssueDate" runat="server" Width="100%" ReadOnly="false" />

                                            <%--Read Only Change True To False By Himanshu On Date 17/03/2022 Mr. Saifudeen --%>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                            <asp:Label ID="Label36" runat="server" Text="Component" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtBloodComponent" runat="server" ReadOnly="true" Width="100%" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                            <asp:Label ID="lblRequestType" runat="server" Text='<%$ Resources:PRegistration, ReqType%>' /><span style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadComboBox ID="ddlRequestType" runat="server" Width="100%" EmptyMessage="[ Select ]" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value="0" />
                                                    <telerik:RadComboBoxItem Text="Routine" Value="R" />
                                                    <telerik:RadComboBoxItem Text="Urgent" Value="U" />
                                                    <telerik:RadComboBoxItem Text="Immediate" Value="I" />
                                                    <telerik:RadComboBoxItem Text="Emergency" Value="E" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <div class="col-md-6 col-sm-6 col-6" style="display: none;">
                                                <div id="dvEmergencyType" runat="server">
                                                    <telerik:RadComboBox ID="ddlEmergencyType" SkinID="DropDown" runat="server" Width="100%" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-6 col-6" id="divoldhospital" runat="server">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-7 col-sm-12 col-12 box-col-checkbox no-p-r">
                                            <asp:CheckBox ID="chkIsOuterRequest" runat="server" Text='<%$ Resources:PRegistration, ReqFromOther%>' AutoPostBack="true" OnCheckedChanged="chkIsOuterRequest_CheckedChanged" />
                                        </div>
                                        <div class="col-md-5 col-sm-12 col-12">
                                            <telerik:RadComboBox ID="ddlHospitalList" runat="server" Width="100%" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-6" id="divoldDoctorname" runat="server">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                            <asp:Label ID="Label37" runat="server" Text="Doctor Name" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtDoctorName" runat="server" Width="100%" MaxLength="100" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-5 col-sm-5 col-5 text-nowrap">
                                            <asp:Label ID="Label30" runat="server" Text="Provisional Diagnosis" Font-Bold="true" />
                                        </div>
                                        <div class="col-md-7 col-sm-7 col-7">
                                            <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" Height="30px" Width="100%" TextMode="MultiLine" ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6" style="display: none">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 label2">
                                            <asp:Label ID="Label35" runat="server" Text="Blood Group" />
                                        </div>
                                        <div class="col-md-8 col-sm-8">
                                            <asp:TextBox ID="txtBloodGroup" runat="server" Width="100%" ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border: 1px solid #ccc;">
                            <div class="row">
                                <h2 style="background: #c1e5ef; color: #000; font-size: 13px; padding: 4px 10px; margin: 0px; float: left; width: 100%;">
                                    <asp:Label ID="Label4" runat="server" Text='<%$ Resources:PRegistration, ReqFrom%>' />
                                </h2>
                            </div>
                            <div class="row">
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text='<%$ Resources:PRegistration, regno%>'
                                                Font-Underline="false" ToolTip="Click to search donor" OnClick="lbtnSearchDonor_Click"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtRegNo" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                            <asp:LinkButton ID="LinkButton2" runat="server" Text='<%$ Resources:PRegistration, EncounterNo%>'
                                                Font-Underline="false" ToolTip="Click to search donor" OnClick="lbtnSearchDonor2_Click"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtEncounterNo" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                            <asp:Label ID="Label8" runat="server" Text='<%$ Resources:PRegistration, ReqDate%>' />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtRequestDate" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4">
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label10" runat="server" Text='<%$ Resources:PRegistration, PatientName%>' />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtPatientName" runat="server" MaxLength="150" Enabled="false" Width="100%"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqFName" runat="server" ControlToValidate="txtPatientName"
                                                SetFocusOnError="true" Display="None" ErrorMessage="Patient Name Cannot Be Blank"
                                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtPatientName" FilterType="Custom, LowercaseLetters , UppercaseLetters"
                                                ValidChars="1234567890 ">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:HiddenField ID="hdnReqAck" runat="server" />
                                            <asp:HiddenField ID="hdnSampleAck" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label11" runat="server" Text='<%$ Resources:PRegistration, GuardianName%>' />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtGuardian" runat="server" Width="100%" MaxLength="50" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:PRegistration, SEX%>" /><span style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadComboBox ID="ddlSex" runat="server" ValidationGroup="Save"
                                                Width="100%" EmptyMessage="[ Select ]" CausesValidation="false" AutoPostBack="true"
                                                Enabled="false" OnSelectedIndexChanged="ddlSex_SelectedIndexChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value="0" />
                                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, MALE%>" Value="2" />
                                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, FEMALE%>" Value="1" />
                                                    <telerik:RadComboBoxItem Text="Both" Value="3" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator runat="server" InitialValue="[ Select ]" ID="RequiredFieldValidator7"
                                                ControlToValidate="ddlSex" Display="None" ErrorMessage="Please Select Gender!"
                                                ValidationGroup="Save" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:PRegistration, DOB%>" />
                                            <asp:ImageButton ID="imgCalYear" runat="server" ImageUrl="~/Images/insert_table.gif"
                                                Width="1%" ToolTip="Calculate Year, Month, Days" OnClick="imgCalYear_Click" ValidationGroup="DOB"
                                                Style="display: none" />
                                            <asp:ImageButton ID="btnCalAge" runat="server" ImageUrl="~/Images/insert_table.gif"
                                                ToolTip="Calculate Age" ValidationGroup="GetAge" OnClick="btnCalAge_Click" Width="1%"
                                                Style="display: none" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadDatePicker ID="dtpDateOfBirth" runat="server" Enabled="false" MinDate="01/01/1870" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                                <ClientEvents OnDateSelected="CalCulateDOB" />
                                                <DateInput ID="DateInput1" runat="server">
                                                    <ClientEvents OnError="ShowError" />
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label13" runat="server" Style="margin: 0px!important; padding: 0px!important;" Text="<%$ Resources:PRegistration, DOB%>" /><asp:Label ID="Label19" runat="server" Text="Age(Y-M-D)" /><span style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <div class="row">
                                                <%-- <asp:UpdatePanel ID="updYMD" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="imgCalYear" />
                                            </Triggers>
                                            <ContentTemplate>--%>
                                                <div class="col-4 PaddingRightSpacing">
                                                    <asp:TextBox ID="txtYear" runat="server" Columns="1" MaxLength="3" Enabled="false" Width="100%" AutoPostBack="true" OnTextChanged="txtYear_TextChanged"></asp:TextBox>
                                                </div>
                                                <div class="col-4 PaddingRightSpacing">
                                                    <asp:TextBox ID="txtMonth" runat="server" Columns="1" MaxLength="2" Enabled="false" Width="100%" AutoPostBack="true" OnTextChanged="txtMonth_TextChanged"></asp:TextBox>
                                                </div>
                                                <div class="col-4">
                                                    <asp:TextBox ID="txtDays" runat="server" Columns="1" MaxLength="2" Enabled="false" Width="100%" AutoPostBack="true" OnTextChanged="txtDays_TextChanged"></asp:TextBox>
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtYear"
                                                    Display="None" ErrorMessage="please enter date of birth or age !" InitialValue=""
                                                    ValidationGroup="Save" />
                                                <cc1:FilteredTextBoxExtender ID="FTEtxtYear" runat="server" Enabled="True" TargetControlID="txtYear"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTEtxtMonth" runat="server" Enabled="True" TargetControlID="txtMonth"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTEtxtDays" runat="server" Enabled="True" TargetControlID="txtDays"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-6 col-sm-6 col-8 text-nowrap no-p-r">
                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:PRegistration, weight%>" /><%-- <span style="color: #FF3300">*</span>--%>
                                            <div class="box-col-checkbox" style="display: inline-block">
                                                <asp:CheckBox ID="chbIsPediatric" runat="server" Text="Is Female" AutoPostBack="true" OnCheckedChanged="chbIsPediatric_CheckedChanged" />
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-4">
                                            <asp:TextBox ID="txtWeight" runat="server" Width="100%" Style="text-align: right"
                                                Enabled="false" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" ValidChars="0123456789."
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtWeight">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:Panel ID="pnlpediatric" runat="server" Visible="false">
                                                <div class="row form-groupTop01">
                                                    <div class="col-md-4 col-sm-4 label2">Pregnancy</div>
                                                    <div class="col-md-8 col-sm-8 margin_Top01">
                                                        <div class="PD-TabRadioNew01 margin_z">
                                                            <asp:RadioButtonList ID="rbtnpregnancy" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row form-groupTop01">
                                                    <div class="col-md-4 col-4 label2">Miscarriage</div>
                                                    <div class="col-md-8 col-8 margin_Top01">
                                                        <div class="PD-TabRadioNew01 margin_z">
                                                            <asp:RadioButtonList ID="rbtnMiscarriage" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:PRegistration, ward%>" />
                                            <asp:Label ID="Label32" runat="server" Text="*" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtWard" runat="server" Width="100%" MaxLength="150" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:PRegistration, bedno%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtBedNo" runat="server" Width="100%" MaxLength="150" Enabled="false" ReadOnly="True" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:PRegistration, HB%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtHB" runat="server" Width="100%" MaxLength="150" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:PRegistration, PltCount%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtPltCount" runat="server" Width="100%" MaxLength="150" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:PRegistration, PT%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtPT" runat="server" Width="100%" MaxLength="150" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:PRegistration, APTTT%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtAPTT" runat="server" Width="100%" MaxLength="150" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:PRegistration, FIBRINOGEN%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtFIBRINOGEN" runat="server" Width="100%" MaxLength="150" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:PRegistration, CONSULTANT%>" /><span style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadComboBox ID="ddlCounsultant" Filter="Contains" Width="100%"
                                                Enabled="false" runat="server" EmptyMessage="[ Select ]">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="1" Value="1" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:PRegistration, diagnosis%>" /><span style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtDiagnosis" runat="server" Width="100%" MaxLength="20" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="lblChargeSlipNo" runat="server" Text="<%$ Resources:PRegistration, ChargeSlipNo%>" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtChargeSlipNo" runat="server" Width="100%" MaxLength="20" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 ">
                                            <asp:Label ID="Label31" runat="server" ToolTip="<%$ Resources:PRegistration, PatientBloodGroup%>" Text="<%$ Resources:PRegistration, PatientBloodGroup%>" /><span id="spanPBG" runat="server" style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadComboBox ID="ddlPatientBloodGroup" runat="server" Width="100%" EmptyMessage="[ Select ]" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-5 col-sm-5 col-5 text-nowrap">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:PRegistration, MotherBloodGroup%>" /><asp:Label ID="Label15" runat="server" Text="(In case of child)" />
                                        </div>
                                        <div class="col-md-7 col-sm-7 col-7">
                                            <telerik:RadComboBox ID="ddlMotherBloodGroup" runat="server" Width="100%" EmptyMessage="[ Select ]" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3  col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label26" runat="server" Text="Reason" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <telerik:RadComboBox ID="ddlReason" runat="server" Width="100%" EmptyMessage="[ Select ]">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="1" Value="1" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6" id="rowHaematocrit" runat="server">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                            <asp:Label ID="Label27" runat="server" ToolTip="Haematocrit Required" Text="Haematocrit Req." />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-8">
                                            <asp:TextBox ID="txtHaematocritRequired" runat="server" Width="100%" MaxLength="3" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-3 col-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-12 col-sm-12 col-xs-12 box-col-checkbox">
                                            <asp:CheckBox CausesValidation="false" ID="chbIsExchangeTransformation" AutoPostBack="true" runat="server" Text="Exchange Transfusion" OnCheckedChanged="chbIsExchangeTransformation_CheckedChanged" />

                                            <asp:CheckBox CausesValidation="false" ID="isSampleSend" AutoPostBack="true" runat="server" Text="Sample Send" />
                                            <asp:CheckBox ID="ChbAnyTransfusion" runat="server" Text="Any Previous Transfusion History" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6  col-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-12 col-sm-12 col-xs-12 box-col-checkbox">


                                            <asp:CheckBox ID="ChbAnyTransfusionReaction" runat="server" Text="Any Transfusion Reaction" />

                                            <asp:CheckBox ID="ChbIrradiatedComponent" runat="server" Text="Irradiated Component" />
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>


                <div id="otherhospitalDiv" runat="server">
                    <div class="container-fluid header_mainGray margin_z">
                        <div class="col-6">
                            <h2>
                                <asp:Label ID="Label41" runat="server" Text="Other Hospital Details:" /></h2>
                        </div>
                        <div class="col-sm-6"></div>
                    </div>

                    <div class="container-fluid" style="background-color: #fff;">

                        <div class="row form-groupTop01">
                            <div class="col-md-4 col-4">
                                <div class="row">
                                    <div class="col-md-12 col-sm-6 label2 margin_Top margin_z labelText">
                                        &nbsp;  &nbsp;  &nbsp;  &nbsp;
                             <%--   <asp:LinkButton ID="LinkButton3" Width="10%" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" OnClientClick="javascript:openRadWindow('/BloodBank/SetupMaster/HospitalMaster.aspx?MP=NO',950,600);return false;"
                                    CssClass="btnBig btn-default" Text="Add Hospital" ToolTip="Add New Hospital" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8 col-8 margin_z">
                                <div class="row">
                                    <div class="col-md-4 col-4">
                                        <asp:CheckBox ID="chkotherhospitalrequest" runat="server" Text='<%$ Resources:PRegistration, ReqFromOther%>' AutoPostBack="true" OnCheckedChanged="chkotherhospitalrequest_CheckedChanged" />
                                        <asp:Button ID="btn_otherHospital" runat="server" CssClass="btn btn-default" Text="Add Hospital" OnClick="btn_otherHospital_Click" />

                                    </div>
                                    <div class="col-md-8 col-6">
                                        <telerik:RadComboBox ID="ddlotherhospital" runat="server" Width="100%" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row form-groupTop01">
                            <div class="col-md-4 col-4">
                                <div class="row">
                                    <div class="col-md-4 col-4 label2 PaddingRightSpacing">
                                        <asp:Label ID="Label38" runat="server" Text="MRD No" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtothermrdno" runat="server" Width="100%" MaxLength="100" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-8 col-8">
                                <div class="row">
                                    <div class="col-md-4 col-4 label2 PaddingRightSpacing">
                                        <asp:Label ID="Label39" runat="server" Text="Ward Name" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtotherwardname" runat="server" Width="100%" MaxLength="100" />
                                    </div>
                                </div>
                            </div>


                        </div>

                        <div class="row form-groupTop01">
                            <div class="col-md-4 col-4">
                                <div class="row">
                                    <div class="col-md-4 col-4 label2 PaddingRightSpacing">
                                        <asp:Label ID="Label40" runat="server" Text="Bed No" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtotherBedNo" runat="server" Width="100%" MaxLength="100" />
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-8 col-8">
                                <div class="row">
                                    <div class="col-md-4 col-4 label2 PaddingRightSpacing">
                                        <asp:Label ID="Label42" runat="server" Text="Doctor Name" />
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtotherDoctor" runat="server" Width="100%" MaxLength="100" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border: 1px solid #ccc;">
                            <div class="row">
                                <h2 style="background: #c1e5ef; color: #000; font-size: 13px; padding: 4px 10px; margin: 0px; float: left; width: 100%;">
                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:PRegistration, AddComponent%>"></asp:Label>
                                </h2>
                            </div>

                            <div class="row">
                                <div class="col-md-7 col-sm-7 col-12">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="row p-t-b-5">
                                                <div class="col-md-5 col-sm-5 col-5">
                                                    <asp:Label ID="Label28" runat="server" Text="Component Request For"></asp:Label>
                                                </div>
                                                <div class="col-md-7 col-sm-7 col-7 box-col-checkbox" style="padding: 0px;">
                                                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                                        <asp:ListItem Text="Blood/Component" Selected="True" />
                                                        <asp:ListItem Text="Procedure" />
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="row p-t-b-5">
                                                <div class="col-md-6 col-sm-3 col-4" style="white-space: nowrap;">No. of Unit</div>
                                                <div class="col-md-6 col-sm-9 col-8">
                                                    <asp:TextBox ID="txtNoOfUnit" runat="server" Style="text-align: right" Width="100%" MaxLength="5" Text="1" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" TargetControlID="txtNoOfUnit" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-7 col-sm-7 col-12">
                                            <div class="row p-t-b-5">
                                                <div class="col-md-2 col-sm-12 col-4 label2">Component</div>
                                                <div class="col-md-10 col-sm-12 col-8">
                                                    <telerik:RadComboBox ID="ddlComponent" AutoPostBack="true" EmptyMessage="[ Select ]" runat="server" DropDownWidth="340px" Width="100%" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged"></telerik:RadComboBox>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("id") %>' />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 col-sm-5 col-xs-12">
                                            <div class="row p-t-b-5">
                                                <div class="col-md-4 col-sm-4 col-4 text-nowrap label2">Required Date</div>
                                                <div class="col-md-8 col-sm-8 col-8">
                                                    <telerik:RadDatePicker ID="dtpRequestDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" OnSelectedDateChanged="dtpRequestDate_SelectedDateChanged" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-9 col-sm-7 col-xs-12">
                                            <div class="row p-t-b-5">
                                                <div class="col-md-4 col-5 label2 PaddingRightSpacing" style="display: none;">Qty (ml)</div>
                                                <div class="col-md-8 col-12 col-xs-12 p-t-b-5">
                                                    <asp:TextBox ID="txtSize" runat="server" Width="100%" MaxLength="5" Style="text-align: right; display: none;" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" TargetControlID="txtSize" ValidChars="0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3 col-sm-5 col-xs-12 text-right">
                                            <asp:Button ID="lnkAddComponent" CssClass="btn btn-primary" runat="server" Text='Add Component'
                                                OnClick="lnkAddComponent_Click" CommandName="AddComponent" />
                                            <asp:Label ID="lblAcknowledge" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row m-t">
                                        <div class="col-md-12 col-sm-12 col-xs-12 gridview-inner-table">
                                            <asp:UpdatePanel ID="udp2" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvNewComponent" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="panel_y" Height="100%" ScrollBars="Auto">
                                                        <asp:GridView ID="gvNewComponent" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                                            SkinID="gridviewOrderNew" AutoGenerateColumns="False" OnRowDataBound="gvNewComponent_OnRowDataBound"
                                                            EmptyDataText="No Record" OnRowCommand="gvNewComponent_OnRowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Component" HeaderStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblComponent" runat="server" Width="100%" SkinID="label" Text='<%# Eval("ComponentName") %>' />
                                                                        <asp:HiddenField ID="hdnComponentId" runat="server" Value='<%# Eval("ComponentId") %>' />
                                                                        <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Eval("DetailsId") %>' />
                                                                        <asp:HiddenField ID="hdnUniqueId" runat="server" Value='<%# Eval("id") %>' />
                                                                        <asp:HiddenField ID="hdnCrossMatchServiceId" runat="server" Value='<%# Eval("CrossMatchServiceId") %>' />
                                                                        <asp:HiddenField ID="hdnIsCrossMatchRequired" runat="server" Value='<%# Eval("IsCrossMatchRequired") %>' />
                                                                        <asp:HiddenField ID="hdnIsBillingRequired" runat="server" Value='<%# Eval("IsBillingRequired") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQty" runat="server" SkinID="label" Text='<%# Eval("Qty") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Required Date" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%--<asp:Label ID="lblRequestDate" runat="server" SkinID="label" Text='<%# Eval("RequestDate","{0:dd/MM/yyyy}") %>' />--%>
                                                                        <asp:Label ID="lblRequestDate" runat="server" SkinID="label" Text='<%#Eval("RequestDate")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qty (ml)" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSizeML" runat="server" SkinID="label" Text='<%# Eval("SizeML","{0:F0}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText='Edit' HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text='Edit' CommandName="Select" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                                            CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("DetailsId")%>'
                                                                            ImageUrl="~/Images/DeleteRow.png" Width="16px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                </div>
                                <div class="col-md-5 col-sm-5 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-12 col-5 text-nowrap label2">
                                            <asp:Label ID="Label2" runat="server" Text="Indication for Transfusion" /><span id="spanIT" runat="server" style="color: Red">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-12 col-7">
                                            <telerik:RadComboBox ID="ddlIndicationforTransfusion" runat="server"
                                                Width="100%" DropDownWidth="300px" EmptyMessage="[ Select ]">
                                                <%-- <Items>
                                                <telerik:RadComboBoxItem Text="1" Value="1" />
                                            </Items>--%>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-12 col-5 label2">
                                            <asp:Label ID="Label3" runat="server" Text="Consent Date" />
                                        </div>
                                        <div class="col-md-8 col-sm-12 col-7">
                                            <telerik:RadDatePicker ID="dtpConsentDate" runat="server" MinDate="01/01/1870" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                        </div>
                                    </div>

                                    <div class="row" style="display: none;">
                                        <div class="col-md-4 col-6 label2 PaddingRightSpacing" style="display: none;">
                                            <asp:Label ID="Label5" runat="server" Text="Consent Taken By" />
                                        </div>
                                        <div class="col-md-8 col-6" style="display: none;">
                                            <telerik:RadComboBox ID="ddlConsentTakenBy" runat="server" Width="100%"
                                                DropDownWidth="300px" EmptyMessage="[ Select ]">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="1" Value="1" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-12 col-4">
                                            <asp:Label ID="Label6" runat="server" Text="Remarks" />
                                        </div>
                                        <div class="col-md-8 col-sm-12 col-8">
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" MaxLength="20" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-12 gridview-inner-table">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvPreviousHistory" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:Panel ID="Panel2" runat="server" CssClass="panel_y" Height="100%" ScrollBars="Auto">
                                                        <asp:GridView ID="gvPreviousHistory" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                                            SkinID="gridviewOrderNew" AutoGenerateColumns="False" OnRowDataBound="gvPreviousHistory_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Question" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuestion" runat="server" Width="100%" SkinID="label" Text='<%# Eval("QuestionDescription") %>' />
                                                                        <asp:HiddenField ID="hdnQuestionId" runat="server" Value='<%# Eval("QuestionId") %>' />
                                                                        <asp:HiddenField ID="hdnIsDefault" runat="server" Value='<%# Eval("IsDefault") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Answer">
                                                                    <ItemTemplate>
                                                                        <asp:RadioButtonList ID="radAnswer" Width="100%" CssClass="box-t-inner" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Text="Yes" Value="1" />
                                                                            <asp:ListItem Text="No" Value="0" />
                                                                        </asp:RadioButtonList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>



           


                <table width="100%" border="0" cellpadding="1" cellspacing="1" style="background-color: #fff;">
                    <tr>
                        <td colspan="4" align="left">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <%-- <asp:UpdatePanel ID="aa14" runat="server">
                                <ContentTemplate>--%>
                                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" runat="server" EnableViewState="false">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize">
                                                </telerik:RadWindow>
                                            </Windows>
                                        </telerik:RadWindowManager>
                                        <asp:TextBox ID="lblPharmacyId" runat="server" Text="0" Width="0" Style="visibility: hidden; height: 1px;"></asp:TextBox>
                                        <asp:TextBox ID="txtPatientImageId" runat="server" Style="visibility: hidden; height: 1px;" Text=""></asp:TextBox>
                                        <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" InitialBehaviors="Maximize" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdReg" runat="server" style="width: 120px">
                            <asp:Label ID="lblRegistration" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnIndication" runat="server" />
                            <asp:HiddenField ID="hdnAcknowledge" runat="server" />
                            <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                            <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                        </td>
                    </tr>
                </table>



                <%--By Naushad Start 08-07-2014 --%>
                <div id="Div1" runat="server" style="width: 420px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 100px; left: 350px; top: 150px">
                    <table cellpadding="2" cellspacing="2" border="0" width="99%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label18" runat="server" Font-Size="12pt" Font-Bold="true" Font-Names="Arial"
                                    Text="Print Options"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #00CCFF"></td>
                        </tr>
                        <tr>
                            <td align="center" valign="middle">
                                <asp:Button ID="btnPrintBarCode" SkinID="Button" runat="server" Text="Print Bar Code"
                                    ToolTip="Print Bar Code" OnClick="btnPrintDetail_OnClick" Width="100px" />
                                <asp:Button ID="btnClosepopup" SkinID="Button" runat="server" Text="Close" OnClick="btnClosepopup_OnClick"
                                    Width="100px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--By Naushad End 08-07-2014 --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
