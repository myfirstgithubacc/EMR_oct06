<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppointmentDetails.aspx.cs"
    Inherits="Appointment_AppointmentDetails" Title="Appointment" %>

<%--<%@ Register TagPrefix="ddlps" TagName="ComboPatientSearch" Src="~/Include/Components/PatientApptSearch.ascx" %>--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="../Include/Components/AlertBlock.ascx" TagName="AlertBlock" TagPrefix="uc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/registrationNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>
    <script src="/Include/JS/Functions.js" type="text/jscript"></script>
    <script type="text/javascript">
        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.value = "Processing...";
            }
            return true;
        }
    </script>
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
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

        </script>

        <script type="text/javascript">
            function validateMaxLength() {
                var txt = $get('<%=txtAccountNo.ClientID%>');
                if (txt.value > 9223372036854775807) {
                    alert("Value should not be more than 9223372036854775807.");
                    txt.value = txt.value.substring(0, 12);
                    txt.focus();
                }
            }

            function openRadWindow(strPageNameWithQueryString) {
                var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
            }
            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                }
                $get('<%=btnGetInfo.ClientID%>').click();
            }
            function SearchPatientOnClientCloseOnlineRequest(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnPPAppointmentId.ClientID%>').value = arg.PPAppointmentId;
                }
                $get('<%=btnGetInfo.ClientID%>').click();
            }
            function OnClientClose(oWnd, args) {
                var arg = args.get_argument();

            }
        </script>

        <script language="javascript" type="text/javascript">
            function OnClientFormCreated(sender, eventArgs) {
                $telerik.$(".rsRecurrenceOptionList li:first-child").hide();
            }
            function setDDLValue() {
                debugger;
                checkinvalue = $get('<%= hdCheckIn.ClientID %>').value;
                unconvalue = $get('<%= hdUnCon.ClientID %>').value;
                chk = $get('<%= chkWalkIn.ClientID %>');
                var combo = $find("<%= ddlAppointmentStatus.ClientID %>");
                var sep;
                var txtAccountNo = $get('<%= txtAccountNumber.ClientID %>').value;
                if (txtAccountNo == '') {
                    chk.checked = false;
                }
                else {
                    if (chk.checked == true) {
                        sep = combo.findItemByValue(checkinvalue);
                    }
                    else {
                        sep = combo.findItemByValue(unconvalue);
                    }
                    var item = combo.get_items().getItem(sep.get_index());
                    combo.enable();
                    combo.trackChanges();
                    item.select();
                    combo.commitChanges();
                    item.scrollIntoView();
                    combo.disable();
                }
            }

            function enableDisable() {

                var radio = document.getElementsByName("<%=rboselectDob.ClientID%>");

                var isChecked = 0;

                for (var j = 0; j < radio.length; j++) {
                    if (radio[j].checked)
                        isChecked = radio[j].value;
                }
                //var radio = chkStatus.getElementsByTagName("input");

                var trDOB = document.getElementById("trDOB");
                var trYMD = document.getElementById("trYMD");

                if (isChecked == 0) {
                    trDOB.style.visibility = 'visible';
                    trYMD.style.visibility = 'hidden';
                    trDOB.style.position = 'static';
                    trYMD.style.position = 'absolute';
                }
                else if (isChecked == 1) {
                    trYMD.style.visibility = 'visible';
                    trDOB.style.visibility = 'hidden';
                    trYMD.style.position = 'static';
                    trDOB.style.position = 'absolute';
                }
            }

            function chkme() {
                if (confirm('continue'))
                    return true;
                else
                    return false;
            }
        </script>

        <script type="text/javascript">
            function CloseScreen() {
                window.close();
            }
        </script>

        <script type="text/javascript">
            function pageLoad() {
                var recurrenceEditor = $find('<%=RadSchedulerRecurrenceEditor1.ClientID %>');
                if (recurrenceEditor.get_recurrenceRule() == null) {
                    $telerik.$(".rsAdvWeekly_WeekDays li span input").each(function (index, item) {
                        item.checked = false;
                    });
                }
                $telerik.$(".rsRecurrenceOptionList li:first-child").hide();
            }
            function BindCombo(oWnd, args) {
                document.getElementById('<%=btnFillCombo.ClientID%>').click();
            }
        </script>

    </telerik:RadScriptBlock>

    <style type="text/css">
        .style2 {
            display: none;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: 0;
        }

        input#ddlSelectionFacility_Input {
            height: 26px;
        }

        a.btnSearchMobAppt {
            padding: 13.4px 18px !important;
        }

        #txtFname, #dropTitle, #txtMName, #txtLame, #dropSex, #dtpDateOfBirth, #txtYear, #txtMonth, #txtDays, #txtParentof {
            border: 1px solid #ced4da !important;
        }

        div.dropheight {
            height: calc(1em + 0.75rem + 2px) !important;
            padding: 0.1rem 4px !important;
            font-size: 13px !important;
            width: 100% !important;
            font-weight: 400 !important;
            line-height: 1.5 !important;
            color: #495057 !important;
            background-color: #fff !important;
            background-clip: padding-box !important;
            border: 1px solid #ced4da !important;
            border-radius: 0.25rem !important;
            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out !important;
        }

        input#ddlGender_Input, input#ddlFacility_Input, input#ddlAppointment_Input, input#ddlProvider_Input, input#ddlAppointmentType_Input, input#ddlAppointmentStatus_Input, input#dropReferredType_Input, input#ddlreferringphysician_Input, input#ddlSubDept_Input, input#ddlService_Input {
            border: none !important;
        }

        .form-group {
            margin-bottom: 0.3rem !important;
        }

        textarea {
            border: 1px solid #ced4da !important;
            border-radius: 0.25rem !important;
        }

        @media only screen and (max-width: 680px) {
            .RecurrenceEditor .rsAdvRecurrenceFreq {
                width: 100% !important;
                height: 40px !important;
            }

            ul.rsRecurrenceOptionList {
                display: flex !important;
                justify-content: space-around !important;
            }

            div#RadSchedulerRecurrenceEditor1_RecurrencePatternPanel {
                padding-left: 0px !important;
            }

            .RecurrenceEditor_Default ul.rsRecurrenceOptionList {
                border-right: none !important;
                border-bottom: 1px solid #ababab !important;
            }
        }

        @media only screen and (max-width: 945px) {
            ul.rsAdvWeekly_WeekDays {
                display: block !important;
            }
        }

        ul.rsAdvWeekly_WeekDays {
            display: inline-flex;
        }
        #UpdatePanel1{
            margin:0!important;
        }
         .btn {
            padding: 2px 6px!important;
            font-size: 14px!important;
        }   
    </style>
</head>

<body style="background-color: White;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <table width="100%" bgcolor="white" cellspacing="0" cellpadding="0" align="center">
            <tr>
                <td width="100%">
                    <div class="AppointmentPage001">
                        <asp:Panel ID="Panel1" runat="server" Width="100%">
                            <div class="AppointmentPage">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblMessage" runat="server" Text="" />
                                            <input id="hdnregno" type="hidden" runat="server" />
                                            <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="AppointmentPage001">
                                <div class="AppointmentPage001">
                                    <asp:Panel ID="Panel2" runat="server" Width="100%">
                                        <%----------------- Start Top Part  ----------------------------%>
                                        <div class="AppointmentPage-Center">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-4">

                                                        <span class="AppointmentFacility">
                                                            <asp:Literal ID="Literal5" runat="server" Text="Facility"></asp:Literal>
                                                            <telerik:RadComboBox ID="ddlSelectionFacility" CssClass="AppointFacilitySelect " runat="server" AppendDataBoundItems="true" TabIndex="10">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="All" Value="0" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </span>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <span class="AppointmentUHID">
                                                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                                                                <asp:Button ID="btnGetInfo" runat="server" CausesValidation="false" Enabled="true" OnClick="btnGetInfo_Click" Style="visibility: hidden;" TabIndex="103" Text="Assign" Width="1px" />

                                                                <asp:LinkButton ID="lbtnSearchPatient" runat="server" Font-Underline="false" CssClass="lbtnSearchPatient" Text='<%$ Resources:PRegistration, Regno%>' ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click" />
                                                                <asp:TextBox ID="txtAccountNo" CssClass=" form-control" runat="server" Width="100px" MaxLength="13" onkeyup="return validateMaxLength();" />
                                                                <asp:LinkButton ID="lnkUHID" CssClass="btnSearchMobAppt" runat="server" Font-Underline="false" ToolTip="Click to search patient" OnClientClick="return validateMobileNo();" OnClick="lnkUHID_Click"></asp:LinkButton>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtAccountNo" ValidChars="0123456789" />
                                                                <asp:Label ID="lblMobileSearch" runat="server" CssClass="lblMobileSearch" Text="Mobile No"></asp:Label>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMob" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchMobile" ValidChars="0123456789" />
                                                                <asp:TextBox ID="txtSearchMobile" runat="server" CssClass="form-control txtSearchMobileNo" MaxLength="10"></asp:TextBox>

                                                                <asp:LinkButton ID="btnSearchMob" runat="server" CssClass="btnSearchMobAppt" Font-Underline="false" OnClick="btnSearchMob_Click" OnClientClick="return validateMobileNo();" ToolTip="Click to search patient"></asp:LinkButton>
                                                                <asp:TextBox ID="txtRegNo" SkinID="textbox" runat="server" Width="10px" Style="visibility: hidden;" TabIndex="0" />
                                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdnPPAppointmentId" runat="server" Value="0" />
                                                            </asp:Panel>
                                                        </span>
                                                    </div>
                                                    <div class="col-md-3 text-right">
                                                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnGetOldRegInfo">
                                                            <asp:Label ID="lblRegNoName" runat="server" Text="Old Reg. No" Visible="false"></asp:Label>
                                                            &nbsp;&nbsp;
                                                            <asp:TextBox ID="txtOldRegNo" SkinID="textbox" runat="server" Visible="false"></asp:TextBox>
                                                            <asp:Button ID="btnGetOldRegInfo" runat="server" CausesValidation="false" Enabled="true" OnClick="btnGetOldRegInfo_Click" Style="visibility: hidden;" Text="Assign" Width="10px" />
                                                            &nbsp;
                                                            <asp:LinkButton ID="lnkAppointmentlist" CssClass="btn btn-primary" runat="server" Text="Appointment&nbsp;List" OnClick="lnkAppointmentlist_OnClick"></asp:LinkButton>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%----------------- Ends Top Part  ----------------------------%>

                                        <%----------------- Start Patient Details Top Part  ----------------------------%>
                                        <div class="AppointmentPage-BGColor">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <h2>Patient Details : <span>( For New Patient )</span></h2>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="AppointmentPage-Border">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-lg-3 col-md-4 col-6">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-4">
                                                                <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, Regno%>'></asp:Label>
                                                            </div>
                                                            <div class="col-lg-6 col-md-5 col-7">
                                                                <asp:TextBox ID="txtAccountNumber" Columns="15" CssClass=" form-control" runat="server"></asp:TextBox>

                                                            </div>
                                                            <div class="col-2 pl-0">
                                                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Reset" OnClick="btnClear_OnClick" />
                                                            </div>
                                                            <span id="trtitle" runat="server" visible="false">
                                                                <asp:Literal ID="Literal3" runat="server" Text="Select Title"></asp:Literal>
                                                            </span>
                                                            <span class="NewAppointText"></span>
                                                            <span class="NewAppointInput"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="Literal1" runat="server" Text="First Name"></asp:Literal>
                                                                <span class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtFName" runat="server" CssClass="form-control" Style="text-transform: uppercase;" onkeydown="Tab();" MaxLength="50" TabIndex="5"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Save" ControlToValidate="txtFName" runat="server" ErrorMessage="First Name" Display="None"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="Literal2" runat="server" Text="Middle Name"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtMName" runat="server" CssClass="form-control" Style="text-transform: uppercase;" onkeydown="Tab();" MaxLength="50" TabIndex="7"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="ltrName" runat="server" Text="Last Name"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Style="text-transform: uppercase;" onkeydown="Tab();" MaxLength="50" TabIndex="7"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row" id="trDOB">
                                                            <div class="col-lg-3 col-md-4">
                                                                <asp:RadioButtonList ID="rboselectDob" Visible="false" onclick="enableDisable();" RepeatDirection="Horizontal" TabIndex="8" runat="server">
                                                                    <asp:ListItem Text="DOB" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>


                                                                <asp:Literal ID="ltrlDOB" runat="server" Text="DOB"></asp:Literal>
                                                            </div>
                                                            <div class="col-lg-9 col-md-8">
                                                                <telerik:RadDatePicker ID="dtpDOB" runat="server" Width="100%" MinDate="01/01/1870" AutoPostBack="true" TabIndex="9">
                                                                    <ClientEvents OnDateSelected="CalCulateDOB" />
                                                                    <DateInput ID="DateInput2" runat="server" CssClass="form-control" DateFormat="dd/MM/yyyy"></DateInput>
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row" id="trYMD">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="ltrAge" runat="server" Text="Age (Y-M-D)"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="row">
                                                                    <div class="col-4">
                                                                        <asp:TextBox ID="txtAgeYears" runat="server" MaxLength="3" onchange="CalCulateAge();" CssClass="AppointDOB" TabIndex="11" Width="100%" />
                                                                    </div>
                                                                    <div class="col-4">
                                                                        <asp:TextBox ID="txtAgeMonths" ValidationGroup="Save" runat="server" onchange="CalCulateAge();" MaxLength="2" CssClass="AppointDOB" TabIndex="12" Width="100%" />
                                                                    </div>
                                                                    <div class="col-4">
                                                                        <asp:TextBox ID="txtAgeDays" runat="server" MaxLength="2" onchange="CalCulateAge();" CssClass="AppointDOB" TabIndex="10" Width="100%" />
                                                                    </div>
                                                                </div>



                                                                <asp:HiddenField ID="hdnCalculateDOB" runat="server" />

                                                                <cc1:FilteredTextBoxExtender ID="FilteredamtExtender" runat="server" Enabled="True" TargetControlID="txtAgeYears" FilterType="Numbers"></cc1:FilteredTextBoxExtender>

                                                                <asp:RangeValidator ID="RangeValidator1" Display="None" Type="Integer" ValidationGroup="Save" runat="server" ControlToValidate="txtAgeMonths" MaximumValue="12" MinimumValue="0" ErrorMessage="Month is not greater than 12 and less than 0."></asp:RangeValidator>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtAgeMonths" FilterType="Numbers"></cc1:FilteredTextBoxExtender>

                                                                <asp:RangeValidator ID="RangeValidator3" Display="None" Type="Integer" ValidationGroup="Save" runat="server" ControlToValidate="txtAgeDays" MaximumValue="30" MinimumValue="0" ErrorMessage="Days is not greater than 30 and less than 0."></asp:RangeValidator>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtAgeDays" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                                <asp:ImageButton ID="btnCalAge" runat="server" Style="visibility: hidden; display: none;" ImageUrl="~/Images/insert_table.gif" ToolTip="Calculate Age" ValidationGroup="GetAge" OnClick="btnCalAge_Click" />

                                                                <script language="javascript" type="text/javascript">
                                                                    function CalCulateDOB(sender, e) {
                                                                        if (e.get_newDate() != null) {
                                                                            var todaysDate = new Date(); // gets current date/time
                                                                            var dateTimePicker = $find("<%=dtpDOB.ClientID%>"); // gets selected date/time

                                                                            if (dateTimePicker.get_selectedDate() > todaysDate) {
                                                                                alert("Date Of Birth Cannot Be Greater Than Current Date..");
                                                                                return;
                                                                            }
                                                                            else {
                                                                                document.getElementById('<%=imgCalYear.ClientID %>').click();
                                                                            }
                                                                        }
                                                                    }
                                                                    function CalCulateAge() {
                                                                        var txtYear = document.getElementById('<%=txtAgeYears.ClientID %>').value;
                                                                        var txtMonth = document.getElementById('<%=txtAgeMonths.ClientID %>').value;
                                                                        var txtDays = document.getElementById('<%=txtAgeDays.ClientID %>').value;

                                                                        if (txtYear != "") {
                                                                            if (txtYear > 120) {
                                                                                alert("Year Cannot Be Greater Than 120");
                                                                                document.getElementById('<%=txtAgeYears.ClientID %>').value = '';
                                                                                return false;
                                                                            }
                                                                        }
                                                                        if ((txtYear != "") || ((txtMonth != "")) || ((txtDays != ""))) {
                                                                            document.getElementById('btnCalAge').click();
                                                                        }
                                                                    }
                                                                </script>

                                                            </div>





                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="ltrGender" runat="server" Text="Gender"></asp:Literal>
                                                                <span class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlGender" Width="100%" TabIndex="13" CssClass="dropheight" EmptyMessage="" runat="server">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="0" />
                                                                        <telerik:RadComboBoxItem Text="Female" Value="1" />
                                                                        <telerik:RadComboBoxItem Text="Male" Value="2" />
                                                                        <telerik:RadComboBoxItem Text="Other" Value="3" />
                                                                        <telerik:RadComboBoxItem Text="Unknown" Value="4" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Save" runat="server" ControlToValidate="ddlGender" Display="None" ErrorMessage="Select Gender" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="ltrMobile" runat="server" Text="Mobile"></asp:Literal>
                                                                <span class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtMobile" runat="server" onkeydown="Tab();" CssClass="form-control" MaxLength="15" TabIndex="14"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtMobile" ValidChars="+-"></cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMobile" SetFocusOnError="true" Display="None" ErrorMessage="Mobile no cannot be blank" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-4">
                                                                <asp:Literal ID="ltrEmail" runat="server" Text="Email"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" onkeydown="Tab();" MaxLength="50" TabIndex="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="Literal7" runat="server" Text="Note"></asp:Literal>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtNote" Width="100%" Height="26px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                <script type="text/javascript">

                                                                    var textarea = document.getElementById("txtNote");
                                                                    var limit = 200;

                                                                    textarea.oninput = function () {
                                                                        textarea.style.height = "26px";
                                                                        textarea.style.height = Math.min(textarea.scrollHeight, 200) + "px";
                                                                    };
                                                                </script>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">

                                                        <telerik:RadSchedulerRecurrenceEditor ID="RadSchedulerRecurrenceEditor1" runat="server" />


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%----------------- Ends Patient Details Top Part  ----------------------------%>

                                        <%----------------- Start Appointment Details Top Part  ----------------------------%>
                                        <div class="AppointmentPage-BGColor">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <h2>Appointment Details :</h2>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="AppointmentPage-Border">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="ltrlFacility" runat="server" Text="Facility"></asp:Literal>
                                                                <span class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlFacility" AutoPostBack="true" Width="100%" CssClass="dropheight" Visible="true" runat="server" OnSelectedIndexChanged="ddlFacility_OnSelectedIndexChanged" AppendDataBoundItems="true" DataTextField="Name" DataValueField="FacilityID" Enabled="false"></telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlFacility" Display="None" ErrorMessage="Select Facility" ValidationGroup="Save" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">Type <span class="NewAppointred">*</span></div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlAppointment" CssClass="dropheight" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAppointment_OnSelectedIndexChanged" Visible="true" Width="100%" Enabled="false">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="Select" Value="" />
                                                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, Provider%>' Value="0" Selected="true" />
                                                                        <telerik:RadComboBoxItem Text="Resource" Value="1" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Label ID="lblProOrResour" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' />
                                                                <span class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlProvider" runat="server" CssClass="dropheight" AppendDataBoundItems="true" AutoPostBack="true" DataTextField="DoctorName" DataValueField="DoctorId" OnSelectedIndexChanged="ddlProvider_OnSelectedIndexChanged" Visible="true" Width="100%" Enabled="false">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="0" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlProvider" Display="None" ErrorMessage="Select Provider" ValidationGroup="Save" /><br />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="ltrAppointmentType" runat="server" Text="Visit Type"></asp:Literal>
                                                                <span class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlAppointmentType" TabIndex="15" CssClass="dropheight" EmptyMessage="" DropDownWidth="300px" AppendDataBoundItems="true" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlAppointmentType_SelectedIndexChanged">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="0" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Save" runat="server" ControlToValidate="ddlAppointmentType" Display="None" ErrorMessage="Select Appointment Type" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group" id="trSubDept" runat="server" visible="false">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Label ID="lblSubDep" runat="server" Text="Department"></asp:Label>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlSubDept" CssClass="dropheight" Width="100%" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" OnSelectedIndexChanged="ddlSubDept_OnSelectedIndexChanged" DropDownWidth="300px"></telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group" id="trService" runat="server" visible="false">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Label ID="lblPackageService" runat="server"></asp:Label><span style="color: Red">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlService" CssClass="dropheight" Width="100%" runat="server" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" DropDownWidth="300px"></telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">Appt Date</div>
                                                            <div class="col-md-8">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <telerik:RadDatePicker ID="dtpDate" AutoPostBack="true" Width="100%" runat="server" MinDate="01/01/1901">
                                                                            <DateInput ID="DateInput1" runat="server" DisplayDateFormat="dd/MM/yyyy" DateFormat="MM/dd/yyyy"></DateInput>
                                                                        </telerik:RadDatePicker>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="dtpDate" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <span>
                                                                <asp:Label ID="lblAptmntDuration" runat="server" Visible="true"></asp:Label></span>
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="Literal4" runat="server" Text="Appt Time"></asp:Literal>
                                                                <span class="NewAppointred">*</span>

                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="row">
                                                                    <div class="col-5">
                                                                        <telerik:RadTimePicker ID="RadTimeFrom" DateInput-ReadOnly="true" AutoPostBack="true"
                                                                            OnSelectedDateChanged="RadTimeFrom_SelectedDateChanged" PopupDirection="BottomLeft"
                                                                            TimeView-Columns="6" Width="100%" runat="server" />
                                                                    </div>
                                                                    <div class="col-2">
                                                                        <span>To</span>
                                                                    </div>
                                                                    <div class="col-5">
                                                                        <telerik:RadTimePicker ID="RadTimeTo" Width="100%" DateInput-ReadOnly="true" PopupDirection="BottomLeft"
                                                                            TimeView-Columns="6" runat="server" OnSelectedDateChanged="RadTimeTo_SelectedDateChanged"
                                                                            AutoPostBack="true" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-4">Walk-in</div>
                                                            <div class="col-8">
                                                                <asp:CheckBox ID="chkWalkIn" TabIndex="17" CssClass="AppointWalk" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">Reason for visit</div>
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtreason" CssClass="NewAppointInputFull" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                Appointment Status
                                                                <asp:HiddenField ID="hdnStatusCode" runat="server" />
                                                            </div>
                                                            <div class="col-md-8">
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <telerik:RadComboBox ID="ddlAppointmentStatus" CssClass="dropheight" AutoPostBack="true" EmptyMessage="" Width="100%" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlAppointmentStatus_OnSelectedIndexChanged"></telerik:RadComboBox>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlAppointmentStatus" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:PRegistration, REFERALTYPE%>" />
                                                                <span id="SReferredType" runat="server" class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="dropReferredType" CssClass="dropheight" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="RefType_SelectedIndexChanged" EmptyMessage=""></telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-6 form-group">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Literal ID="Literal6" runat="server" Text="Referring Physician"></asp:Literal><span id="sRefPhy" runat="server" class="NewAppointred">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <telerik:RadComboBox ID="ddlreferringphysician" CssClass="dropheight" Visible="true" MarkFirstMatch="true" DropDownWidth="350" AppendDataBoundItems="true" runat="server" Width="100%" AllowCustomText="true" Filter="Contains">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="Select" Value="0" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:ImageButton ID="lnkAddName" runat="server" ImageUrl="~/Images/PopUp.jpg" ToolTip="Add New Doctor" Width="15px" OnClick="lnkAddName_OnClick" Visible="false" CausesValidation="false" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <%----------------- Ends Appointment Details Top Part  ----------------------------%>
                                    </asp:Panel>
                                    <div class="AppointmentPage001">
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class=" col-md-12 text-right">
                                                    <asp:Button ID="btnclose" runat="server" CausesValidation="false" CssClass="btn btn-primary" Text="Close" OnClientClick="window.close();" />
                                                    <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" TabIndex="17" CssClass="btn btn-primary" Text="Make Appointment" ValidationGroup="Save" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />

                                                    <asp:Button ID="btnChangeDate" runat="server" Visible="false" TabIndex="17" Text="Make Appointment" ValidationGroup="Save" />
                                                    <asp:Button ID="btnUpdate" class="AppointmentMakeBtn" runat="server" TabIndex="17" Text="Update Appointment" ValidationGroup="Save" Visible="false" OnClick="btnUpdate_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                                    <asp:Button ID="btnUpdateAll" runat="server" TabIndex="17" Text="Update All Appointment" ValidationGroup="Save" Visible="false" OnClick="btnUpdateAll_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <table id="Table1" cellpadding="0" cellspacing="0" class="style2" runat="server" visible="false">
                                    <tr valign="top">
                                        <td></td>
                                        <td valign="top">&nbsp;&nbsp;
                                                            <asp:Label ID="lblDuration" SkinID="label" runat="server" Text="" Font-Bold="true" Font-Size="11"></asp:Label>
                                            <asp:HiddenField ID="hdnDuration" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnFromTime" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnCompanyId" runat="server" Value="" />
                                            <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_OnClick" CssClass="button" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td></td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Literal ID="ltrPhoneHome" runat="server" Text="Phone Residence"></asp:Literal></td>
                                        <td>
                                            <asp:TextBox ID="txtPhoneHome" runat="server" SkinID="textbox" Width="130px" onkeydown="Tab();" MaxLength="30" TabIndex="14"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>SSN</td>
                                        <td>
                                            <asp:TextBox ID="txtssno" runat="server" SkinID="textbox" Width="130px" onkeydown="Tab();" MaxLength="30" TabIndex="16"></asp:TextBox></td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Literal ID="ltrIdentityType" runat="server" Text="Other Identity Type"></asp:Literal></td>
                                        <td>
                                            <telerik:RadComboBox ID="ddlIdentityType" Width="132px" runat="server"></telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Literal ID="ltrIdentityNo" runat="server" Text="Other Identity No."></asp:Literal></td>
                                        <td>
                                            <asp:TextBox ID="txtIdentityNo" SkinID="textbox" runat="server" Width="130px" onkeydown="Tab();" MaxLength="20" TabIndex="14"></asp:TextBox></td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Literal ID="ltrRemarks" runat="server" Text="Remarks"></asp:Literal></td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" Rows="4" Width="340px" onkeydown="Tab();" MaxLength="8" TextMode="MultiLine" TabIndex="16"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Patient Check-In</td>
                                        <td>
                                            <asp:CheckBox ID="chkCheckIn" runat="server" /></td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Literal ID="ltrReferralDoctorType" runat="server" Text="Referral Doctor"></asp:Literal></td>
                                        <td>
                                            <table cellpadding="2" cellspacing="2">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList runat="server" ID="rblReferralDoctorType" RepeatDirection="Horizontal"
                                                            onkeydown="Tab();" BorderStyle="None" BorderWidth="0" OnSelectedIndexChanged="rblReferralDoctorType_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="17" Width="233px">
                                                            <asp:ListItem Text="Internal Doctor" Value="I"></asp:ListItem>
                                                            <asp:ListItem Text="External Doctor" Value="E"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="cbInsurance" runat="server" onkeydown="Tab();" SkinID="checkbox" TabIndex="18" Text="Insurance Patient" Visible="false" /></td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlReferralDoctor" runat="server" Width="250px" AppendDataBoundItems="true"></telerik:RadComboBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                    <input id="hdCheckIn" runat="server" value="3" type="hidden" />
                    <input id="hdUnCon" runat="server" value="1" type="hidden" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlfunction" runat="server" Width="95%">
                        <table cellpadding="2" cellspacing="2" width="95%">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Following Fields are mandatory." ShowMessageBox="True" ShowSummary="False" ValidationGroup="Save" TabIndex="19" Height="63px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <table align="center">
            <tr>
                <td align="left">
                    <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                    <asp:ImageButton ID="imgCalYear" runat="server" ImageUrl="~/Images/insert_table.gif" Width="6%" ToolTip="Calculate Year, Month, Days" OnClick="imgCalYear_Click" ValidationGroup="DOB" />
                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </td>
            </tr>
        </table>
        <div id="dvMessage" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 100px; left: 300px; top: 150px">
            <table width="100%" cellspacing="2">
                <tr>
                    <td height="70px" valign="top">
                        <asp:DataList ID="dlMissingDocument" runat="server" CssClass="ListBox" Width="100%">
                            <ItemTemplate>
                                <asp:Label ID="lblMissingDocument" runat="server" Text='<%# Eval("Notes") %>'></asp:Label><br />
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:HiddenField ID="hdnNoteType" runat="server" />
                        <asp:Button ID="btnOk" SkinID="Button" runat="server" Text="Ok" OnClick="btnOk_OnClick" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <div id="divPackageDetails" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 100px; left: 300px; top: 150px">
                    <table width="100%" cellspacing="2">
                        <tr>
                            <td height="200px" valign="top">
                                <asp:DataList ID="dlPackageDetails" runat="server" CssClass="ListBox" Width="100%">
                                    <ItemTemplate>
                                        Package Name:
                                        <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label><br />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td valign="top">
                                <asp:Button ID="btnPackageRendering" SkinID="Button" runat="server" Text="Ok" OnClick="btnPackageRendering_OnClick" /></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers></Triggers>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
        <asp:HiddenField ID="hdnPatientCategoryId" runat="server" />
        <asp:HiddenField ID="hdnPlanTypeId" runat="server" />
        <asp:HiddenField ID="hdnERegistrationId" runat="server" />
        <asp:Button ID="btnFillCombo" runat="server" CausesValidation="false" Enabled="true" OnClick="btnFillCombo_Click" SkinID="button" Style="visibility: hidden;" Text="Assign" Width="10px" />

        <%----------------- Start show appointment list on same page  ----------------------------%>
        <div class="AppointmentPage001">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <div class="AppointmentPage-BGColor01">
                            <asp:Label ID="lbl" runat="server" Text="Appointment For :"></asp:Label>
                            <asp:DropDownList ID="ddlrange" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlrange_OnSelectedIndexChanged">
                                <asp:ListItem Text="Last 10" Value="L" Selected="True" />
                                <asp:ListItem Text="Future" Value="F" />
                                <asp:ListItem Text="All" Value="A" />
                            </asp:DropDownList>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Button1" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" Visible="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="AppointmentPage-Border01 table-responsive">
                            <asp:GridView ID="gvAppointment" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="False" SkinID="gridview" AllowPaging="true" PageSize="20" OnRowCommand="gvAppointment_OnRowCommand" OnRowDataBound="gvAppointment_OnRowDataBound" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Appointment Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppDate" runat="server" Text='<%#Eval("AppointmentDate") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("Id")%>' />
                                            <asp:HiddenField ID="hdnAppointmentId" runat="server" Value='<%#Eval("AppointmentId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Appointment Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppTime" runat="server" Text='<%#Eval("AppointmentTimeFromTo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Appointment Day">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppointmentDay" runat="server" Text='<%#Eval("AppointmentDay") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doctor Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnStatusCode" runat="server" Value='<%#Eval("StatusCode")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booked By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookedBy" runat="server" Text='<%#Eval("BookedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booked Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookedDate" runat="server" Text='<%#Eval("BookedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RecurrenceRule" HeaderText="Recurrence" />
                                    <asp:TemplateField HeaderText="Print">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintSlip" runat="server" CommandName="Print" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cancel">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="CancelApp" ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="13px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <table cellpadding="0px" cellspacing="0" border="0" width="100%">
            <tr>
                <td colspan="3"></td>
            </tr>
        </table>
        <div id="dvDelete" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 150px; left: 450px; top: 300px">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDeleteApp" runat="server" Text="Delete Appointment ?"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDeleteEncounterMessage" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Reason : "></asp:Label><font color='Red'>*</font></td>
                    <td>
                        <asp:DropDownList ID="ddlRemarkss" runat="server" AppendDataBoundItems="true" Width="200px" SkinID="DropDown"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Remarks"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtCancel" runat="server" SkinID="textbox" TextMode="MultiLine" Style="max-height: 70px; min-height: 70px; max-width: 200px; min-width: 200px;"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnDeleteApp" SkinID="Button" runat="server" Text="Cancel" OnClick="btnDeleteApp_Click" />
                        <asp:Button ID="btnCancelApp" SkinID="Button" runat="server" Text="Close" OnClick="btnCancelApp_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <%----------------- End show appointment list on same page  ----------------------------%>
    </form>
</body>
</html>
