<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTBooking.aspx.cs" Inherits="OT_Scheduler_OTBooking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />


    <script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>

    <title>OT Booking Details</title>
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

        <script language="javascript" type="text/javascript">
            function ShowError(sender, args) {
                alert("Invalid Date");
                sender.focus();
            }

            function CalCulateDOB(sender, e) {

                if (e.get_newDate() != null) {
                    var todaysDate = new Date(); // gets current date/time
                    var dateTimePicker = $find("<%=dtpDOB.ClientID%>"); // gets selected date/time

                    if (dateTimePicker.get_selectedDate() > todaysDate) {
                        alert("Date of birth cannot be greater than current date..");
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
            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;

                    var cmbvalue = Number(0);
                    var comboBox1 = $find('<%= ddlSearchOn.ClientID %>');
                    cmbvalue = Number(comboBox1.get_value());
                    if (cmbvalue == 0) {
                        $get('<%=txtPatientNo.ClientID%>').value = RegistrationNo;
                    }
                    if (cmbvalue == 1) {
                        $get('<%=txtPatientNo.ClientID%>').value = EncounterNo;
                    }
                    $get('<%=hdnRegistrationId.ClientID %>').value = RegistrationId;
                    $get('<%=hdnRegistrationNo.ClientID %>').value = RegistrationNo;
                    $get('<%=hdnEncounterNo.ClientID %>').value = EncounterNo;
                    $get('<%=hdnEncounterId.ClientID %>').value = EncounterId;
                    $get('<%=btnSearchByPatientNo.ClientID%>').click();
                }
            }
            function addDiagnosisSerchOnClientClose(oWnd, args) {
                $get('<%=btnAddDiagnosisSerchOnClientClose.ClientID%>').click();
            }

            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;

                    var cmbvalue = Number(0);
                    var comboBox1 = $find('<%= ddlSearchOn.ClientID %>');
                    cmbvalue = Number(comboBox1.get_value());
                    if (cmbvalue == 0) {
                        $get('<%=txtPatientNo.ClientID%>').value = RegistrationNo;
                    }
                    if (cmbvalue == 1) {
                        $get('<%=txtPatientNo.ClientID%>').value = EncounterNo;
                    }
                    $get('<%=hdnRegistrationId.ClientID %>').value = RegistrationId;
                    $get('<%=hdnRegistrationNo.ClientID %>').value = RegistrationNo;
                    $get('<%=hdnEncounterNo.ClientID %>').value = EncounterNo;
                    $get('<%=hdnEncounterId.ClientID %>').value = EncounterId;
                    $get('<%=btnSearchByPatientNo.ClientID%>').click();
                }
            }

            function OtRequestOnClientClose(oWnd, args) {
                <%--var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;
                    var OTRequestID = arg.OTRequestID;
                    $get('<%=txtPatientNo.ClientID%>').value = EncounterNo;
                    $get('<%=hdnRegistrationId.ClientID %>').value = RegistrationId;
                    $get('<%=hdnRegistrationNo.ClientID %>').value = RegistrationNo;
                    $get('<%=hdnEncounterNo.ClientID %>').value = EncounterNo;
                    $get('<%=hdnEncounterId.ClientID %>').value = EncounterId;
                    $get('<%=hdnOTRequestID.ClientID %>').value = OTRequestID;

                    $get('<%=btnGetOTRequestDetails.ClientID%>').click();
                }--%>
                $get('<%=btnGetOTRequestDetails.ClientID%>').click();
            }

            function ddlService_OnClientSelectedIndexChanged(sender, args) {

            $get('<%=btnGetInfoService.ClientID%>').click();
        }
        </script>

    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div class="container-fluid">
                <div class="row header_main">
                   
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                 <div class="col-md-5 col-sm-5 col-xs-12 text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" />
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                           
                                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByPatientNo">
                                      <div class="col-md-2 col-sm-2 col-xs-2">
                                <asp:ImageButton ID="ibtnOpenSearchPatientPopup" ImageUrl="~/Images/Binoculr.ico" 
                                        ToolTip="Click to open search patient window" runat="server" OnClick="ibtnOpenSearchPatientPopup_OnClick"
                                        ImageAlign="AbsMiddle" />
                            </div>
                            <div class="col-md-5 col-sm-5 col-xs-5">
                                <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, UHID%>" Value="0" />
                                        <telerik:RadComboBoxItem Selected="True" Text="IP No" Value="1" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                            <div class="col-md-5 col-sm-5 col-xs-5">
                                <asp:TextBox ID="txtPatientNo" runat="server" Width="100%" MaxLength="10"
                                        Style="padding-left: 1px;" ToolTip="Press enter to search" />
                                    <asp:Button ID="btnSearchByPatientNo" runat="server" OnClick="btnSearchByPatientNo_OnClick"
                                        SkinID="Button" Text="Search" CausesValidation="false" Style="visibility: hidden;display:none;"
                                        Width="1" />
                                    <asp:Button ID="btnGetOTRequestDetails" runat="server" OnClick="btnGetOTRequestDetails_Click"
                                        SkinID="Button" Text="Search" CausesValidation="false" Style="visibility: hidden;display:none"
                                        Width="1" />
                            </div>
                          
                                     </asp:Panel>
                           
                        </div>
                    </div>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    <div class="col-md-4 col-sm-4 col-xs-12 text-right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnOTReqList" runat="server" AccessKey="L" CssClass="btn btn-primary" Text="OT Request List"
                                    ToolTip="Click to open OT Request List window" OnClick="btnOTReqList_Click" />

                                <asp:Button ID="btnNew" runat="server" AccessKey="N" CssClass="btn btn-primary" Text="New" ToolTip="Click to refresh window"
                                    ValidationGroup="save" OnClick="btnNew_OnClick" />
                                <asp:Button ID="ibtnSave" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Save"
                                    ToolTip="Click to save this booking" ValidationGroup="save" OnClick="btnSave_OnClick" />
                                <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-primary" Text="Close"
                                    ToolTip="Close this window" OnClientClick="window.close();" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>

                <div class="row">
                    <div class="col-md-9 col-sm-9 col-xs-12" id="Div4" runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;margin-top:5px;height:242px;">
                        <div class="row form-group">
                            <div class="col-md-12" style="background: #337ab7;padding: 3px 10px;font-weight: 600;color: #fff;">
			    <asp:Label ID="Label3" runat="server" Text="Patient Details"></asp:Label></div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="lblInfoRegNo" runat="server" Text="<%$ Resources:PRegistration, regno %>" SkinID="label"></asp:Label>
                                                <asp:Label ID="lblStarRegNo" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                         <asp:TextBox ID="txtRegNo" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label2" runat="server" Text="First Name" SkinID="label"></asp:Label>
                                                <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                         <asp:TextBox ID="txtFName" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label4" runat="server" Text="Middle Name" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtMName" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label5" runat="server" Text="Last Name" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtLName" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label23" runat="server" Text="DOB" SkinID="label"></asp:Label>
                                                <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <telerik:RadDatePicker ID="dtpDOB" Width="80%" runat="server" AutoPostBack="true"
                                                    MinDate="01/01/1900" Skin="Metro" OnSelectedDateChanged="dtpDOB_OnSelectedDateChanged">
                                                    <ClientEvents OnDateSelected="CalCulateDOB" />
                                                    <DateInput ID="DateInput2" runat="server">
                                                        <ClientEvents OnError="ShowError" />
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                                <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                                                <asp:ImageButton ID="imgCalYear" runat="server" ImageUrl="~/Images/insert_table.gif"
                                                    Style="visibility: hidden;" Width="6%" ToolTip="Calculate Year, Month, Days"
                                                    OnClick="imgCalYear_Click" ValidationGroup="DOB" />
                                                <asp:ImageButton ID="btnCalAge" runat="server" ImageUrl="~/Images/insert_table.gif"
                                                    ToolTip="Calculate Age" ValidationGroup="GetAge" OnClick="btnCalAge_Click" Width="1%" />
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label24" runat="server" Text="Age(Y-M-D)" SkinID="label"></asp:Label>
                                                <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:TextBox ID="txtAgeYears" runat="server" MaxLength="3" onchange="CalCulateAge();"
                                                    Width="100%" TabIndex="11"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:TextBox ID="txtAgeMonths" ValidationGroup="Save" runat="server"
                                                    onchange="CalCulateAge();" MaxLength="2" Width="100%" TabIndex="12"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:TextBox ID="txtAgeDays" runat="server" MaxLength="2" onchange="CalCulateAge();"
                                                    Width="100%" TabIndex="10"></asp:TextBox>
                                            </div>  
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label25" runat="server" Text="Gender" SkinID="label"></asp:Label><span
                                                    style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <telerik:RadComboBox ID="ddlGender" runat="server" Width="100%" Filter="Contains"
                                                    Skin="Default" MarkFirstMatch="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                                        <telerik:RadComboBoxItem Text="Female" Value="1" />
                                                        <telerik:RadComboBoxItem Text="Male" Value="2" />
                                                        <telerik:RadComboBoxItem Text="Other" Value="3" />
                                                        <telerik:RadComboBoxItem Text="Unknown" Value="4" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label26" runat="server" SkinID="label" Text="Mobile No."></asp:Label>
                                                <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-6 col-sm-6 col-xs-6 text-nowrap">
                                       <asp:CheckBox ID="chkisBloodRequired" runat="server" AutoPostBack="true" OnCheckedChanged="chkisBloodRequired_CheckedChanged" />  
				       <asp:Label ID="Label30" runat="server" Text="Blood Required" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6 text-nowrap">
                                       <asp:CheckBox ID="chkVentilatorRequired" runat="server" /> 
				       <asp:Label ID="Label32" runat="server" Text="Ventilator Required" SkinID="label"></asp:Label>
				       
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="lblNoOfBloodUnit" runat="server" Text="No. of Blood Unit" Enabled="false" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:DropDownList ID="ddlBloodUnit" runat="server" Width="100%" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="lblOTAdvance" Text="Advance" runat="server" SkinID="label" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtAdvance" runat="server" Width="100%" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="IP No"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:Label ID="lblIPNo" runat="server" SkinID="label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label17" runat="server" SkinID="label" Text="Admission Date"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <asp:Label ID="lblAdmissionDate" runat="server" SkinID="label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-6 col-sm-6 col-xs-6 text-nowrap">
                                        Unplanned Return To OT
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6 box-col-checkbox-table">
                                        <asp:RadioButtonList ID="rblUnplanned" runat="server" AutoPostBack="true" Width="100%" Enabled="false" OnSelectedIndexChanged="rblUnplanned_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1" />
                                                    <asp:ListItem Selected="True" Text="No" Value="0" />
                                                </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                            <div class="col-md-6 col-sm-6 col-xs-6 box-col-checkbox">
                                                <asp:CheckBox ID="chkICUrequired" runat="server" AutoPostBack="true" />
                                                ICU Required</div>
                                            <div class="col-md-6 col-sm-6 col-xs-6 box-col-checkbox">
                                                <asp:CheckBox ID="chkReExploration" runat="server" />
                                                Re-exploration</div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                         <asp:Label ID="Label7" runat="server" SkinID="label" Text="Remarks" />
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                
                                                
                                                    </div>
                                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 colxs-12">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        OT Type
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox-table">
                                        <asp:RadioButtonList ID="rblIsEmergency" runat="server" AutoPostBack="true" Width="100%" Enabled="true" OnSelectedIndexChanged="rblIsEmergency_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Elective" Value="0" Selected="True" />
                                                    <asp:ListItem Text="Emergency" Value="1" />
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 colxs-12">
                                        <div class="row form-group">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblLastChkOutTime" runat="server" Text="Last Check-OutTime" /></div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:Label ID="lblChkoutTime" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                     <%--Akshay--%>
                                    <div class="col-md-4 col-sm-4 colxs-12">
                                        <div class="row form-group">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label34" runat="server" Text="Blood Group" SkinID="label"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                               
                                                <asp:DropDownList ID="ddlBloodGroup" runat="server" OnSelectedIndexChanged="ddlBloodGroup_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                               
                            </div>
                        </div>

                    <div class="col-md-3 col-sm-3 col-xs-12">

                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;margin-top:5px;">
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12 col-xs-12" style="background: #337ab7;padding: 3px 10px;font-weight: 600;color: #fff;">
                                <asp:Label ID="Label13" runat="server" Text="Provisional Diagnosis"></asp:Label>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap text-nowrap">
                                <asp:Label ID="Label16" runat="server" BackColor="Yellow" SkinID="label" Text="Infectious&nbsp;Case"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:RadioButtonList ID="rdoInfectiousCase" Width="100%" runat="server" RepeatDirection="Horizontal" Font-Bold="true" Enabled="false">
                                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                    <asp:ListItem Value="0" Text="No" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap text-nowrap">
			    <asp:Label ID="Label31" runat="server" Text="Infectious Remarks" Enabled="false" /></div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtInfectiousCaseRemarks" runat="server" Width="100%" TextMode="MultiLine" MaxLength="250" Rows="1" Enabled="false" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap text-nowrap">
                                <asp:Label ID="Label29" runat="server" Text="Diagnosis" SkinID="label"></asp:Label>&nbsp;<span
                                                        style="color: #FF0000">*</span>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" MaxLength="200"
                                                         Width="100%" Height="30px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap text-nowrap">
                                <asp:Label ID="Label28" runat="server" Text="Keyword" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <div class="row">
                                    <div class="col-md-9 col-sm-9 col-xs-9 no-p-r">
                                        <telerik:RadComboBox ID="ddlDiagnosisSearchCodes" runat="server"  Width="100%"
                                                    DropDownWidth="250px" AutoPostBack="false">
                                                </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                        <asp:ImageButton ID="ibtnDiagnosisSearchCode" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                        ToolTip="Add New Search Keyword" Style="width:16px;height:16px;float:right;" OnClick="ibtnDiagnosisSearchCode_Click"
                                                        CausesValidation="false" />
                                    </div>
                                </div>
                                
                                                    
                            </div>
                        </div>
                        <div class="row form-group" id="tdLMPDate" runat="server" visible="false">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap text-nowrap">
                                <asp:Label ID="Label33" runat="server" Text="LMP Date" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadDatePicker ID="dtLMPDate" runat="server" Skin="Metro"  Width="100%" MinDate="01/01/1900">
                                                                <DateInput ID="DateInput3" runat="server" AutoPostBack="True" DisplayDateFormat="dd/MM/yyyy"
                                                                    DateFormat="MM/dd/yyyy" ForeColor="Black">
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                                <asp:Panel ID="pnlProv" runat="server" ScrollBars="Vertical" Height="90px">
                                                        <asp:GridView ID="gvProvisionalDiagnosis" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" ShowHeader="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Provisional Diagnosis">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProvisionalDiagnosis" runat="server" Text='<%#Eval("ProvisionalDiagnosis")%>'
                                                                            ToolTip='<%#Eval("ProvisionalDiagnosis")%>' />
                                                                        <asp:HiddenField ID="hdnProvisionalDiagnosisID" runat="server" Value='<%#Eval("id")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                            </div>
                        </div>
                                            </div>
                        </ContentTemplate>
                                </asp:UpdatePanel>
                    </div>

                </div>

                    <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;margin-top:5px;">
                                    <div id="pnl1" runat="server" class="row">
                                        <div class="col-md-7 col-sm-7 col-xs-12" style="border-right:1px solid #ccc;">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-12" style="background: #d5dfe6;padding: 3px 10px;font-weight: 600;color: #000;margin-bottom:3px;">
                                            <b>Theater Details</b>
                                                </div>
                                        </div>
                                        <div class="row form-group">
                                            <div class="col-md-3 co-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label8" runat="server" Text="Theater" SkinID="label"></asp:Label><span
                                                                style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-9 co-sm-9 col-xs-9">
                                                <telerik:RadComboBox ID="ddlOTName" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]"
                                                                Skin="Metro" DataTextField="DoctorName" DataValueField="DoctorId" Width="100%"
                                                                DropDownWidth="300px" Filter="Contains" ForeColor="Black">
                                                            </telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="row form-group">
                                            <div class="col-md-3 co-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label9" runat="server" Text="Date" SkinID="label"></asp:Label><span
                                                                style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-9 co-sm-9 col-xs-9">
                                                <telerik:RadDatePicker ID="dtpOTDate" runat="server" Skin="Metro" Width="80%" MinDate="01/01/1900" OnSelectedDateChanged="dtpOTDate_OnSelectedDateChanged">
                                                                <DateInput ID="DateInput1" runat="server" AutoPostBack="True" DisplayDateFormat="dd/MM/yyyy"
                                                                    DateFormat="MM/dd/yyyy" ForeColor="Black">
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                            </div>
                                        </div>
                                        <div class="row form-group">
                                            <div class="col-md-3 co-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label10" runat="server" Text="Time" SkinID="label"></asp:Label><span
                                                                style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-9 co-sm-9 col-xs-9">
                                                <div class="row">
                                                    <div class="col-md-2 col-sm-2 col-xs-2 text-nowrap">From</div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4 no-p-l no-p-r">
                                                        <telerik:RadTimePicker ID="RadTimeFrom" Skin="Metro" runat="server" AutoPostBack="true"
                                                                            DateInput-ReadOnly="true" OnSelectedDateChanged="RadTimeFrom_SelectedDateChanged"
                                                                            PopupDirection="BottomLeft" TimeView-Columns="6" Width="100%" />
                                                    </div>
                                                    <div class="col-md-2 col-sm-2 col-xs-2 text-nowrap">To</div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4 no-p-l no-p-r">
                                                        <telerik:RadTimePicker ID="RadTimeTo" Skin="Metro" runat="server" AutoPostBack="true" DateInput-ReadOnly="true"
                                                                            OnSelectedDateChanged="RadTimeTo_SelectedDateChanged" PopupDirection="BottomLeft"
                                                                            TimeView-Columns="6" Width="100%" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                            </div>
                                        <div class="col-md-5 col-sm-5 col-xs-12 no-p-r">
                                        <div class="col-md-12 col-sm-12 col-xs-12">                                            
                                            <asp:UpdatePanel ID="updrdoImplant" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-12 col-sm-12 col-xs-12" style="background: #d5dfe6;padding: 3px 10px;font-weight: 600;color: #000;margin-bottom:3px;">
                                                            <b>Implant Details</b></div>
                                                            </div>
                                            <div class="row form-group">
                                            <div class="col-md-4 co-sm-4 col-xs-4 text-nowrap">
                                               Implant Req.
                                            </div>
                                            <div class="col-md-8 co-sm-8 col-xs-8 box-col-checkbox">
                                               <asp:RadioButtonList ID="rdoImpReq" runat="server" Width="100%" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoImpReq_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="Y" Text="Yes" />
                                                                        <asp:ListItem Value="N" Text="No" Selected="True" />
                                                                    </asp:RadioButtonList>
                                            </div>
                                        </div>
                                            <div class="row form-group">
                                            <div class="col-md-3 co-sm-3 col-xs-3 text-nowrap">
                                               <asp:Label ID="lblRemarks" runat="server" Text="Remarks" Visible="false" />
                                                                                <asp:Label ID="lbltxtImpReqRem" runat="server" Text="*" ForeColor="Red" Visible="false" />
                                            </div>
                                            <div class="col-md-9 co-sm-9 col-xs-9">
                                                <asp:TextBox ID="txtImpReqRem" runat="server" TextMode="MultiLine" MaxLength="250" Visible="false" Width="100%" />
                                            </div>
                                                </div>
                                            </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rdoImpReq" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <asp:UpdatePanel ID="updrdoEquipment" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 col-xs-12" style="background: #d5dfe6;padding: 3px 10px;font-weight: 600;color: #000;margin-bottom:3px;">
                                                <b>Equipment Details</b></div>
                                                </div>
                                            <div class="row form-group">
                                            <div class="col-md-5 co-sm-4 col-xs-4 text-nowrap">
                                               Equipment Req.
                                            </div>
                                            <div class="col-md-7 co-sm-8 col-xs-8 box-col-checkbox">
                                               <asp:RadioButtonList ID="rdoEquiReq" runat="server" Width="100%" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoEquiReq_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="Y" Text="Yes" />
                                                                        <asp:ListItem Value="N" Text="No" Selected="True" />
                                                                    </asp:RadioButtonList>
                                            </div>
                                        </div>
                                            <div class="row form-group">
                                            <div class="col-md-3 co-sm-3 col-xs-3 text-nowrap">
                                              <asp:Label ID="lblRemarksEqu" runat="server" Text="Remarks" Visible="false" />
                                                                                <asp:Label ID="lbltxtEquReqRem" runat="server" Text="*" ForeColor="Red" Visible="false" />
                                            </div>
                                            <div class="col-md-9 co-sm-9 col-xs-9">
                                                 <asp:TextBox ID="txtEquiReqRem" runat="server" TextMode="MultiLine" MaxLength="250" Visible="false" Width="100%" />
                                            </div>
                                                </div>
                                                        </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rdoEquiReq" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                    </div>
                                        </div>
                                </div>
                                </div>

                                <div class="col-md-6 col-sm-6 col-xs-12">
                                     <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;margin-top:5px;padding-top:5px;">
                                    <div id="Div1" runat="server">
                                        <div class="row form-group">
                                            <div class="col-md-6 col-sm-6 col-xs-12 box-col-checkbox">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rblServiceType" runat="server" RepeatDirection="Horizontal"
                                                            RepeatLayout="Flow" OnSelectedIndexChanged="rblServiceType_OnSelectedIndexChanged"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Text="Package" Value="IPP"></asp:ListItem>
                                                            <asp:ListItem Text="Surgery" Value="S" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Procedures" Value="P" ></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="rblServiceType" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblDept" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, department %>"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlDepartment" runat="server" Width="100%" Filter="Contains"
                                                            MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged"
                                                            Skin="Default">
                                                        </telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblSubDept" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, SubDepartment %>"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:UpdatePanel ID="up2" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlSubDepartment" runat="server" Width="100%" Filter="Contains" AutoPostBack="true"
                                                            MarkFirstMatch="true" Skin="Default">
                                                        </telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label12" runat="server" SkinID="label" ToolTip="Surgery/Procedure" Text="Surgery/Procedure"></asp:Label><span
                                                    style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadComboBox ID="ddlService" runat="server" Width="100%" Height="300px"
                                                    EmptyMessage="[Type Service Name, Ref Service Name, CPT Code]" AllowCustomText="true" ShowMoreResultsBox="true" EnableLoadOnDemand="true"
                                                    OnItemsRequested="ddlService_OnItemsRequested" EnableVirtualScrolling="true" Skin="Default" DropDownWidth="500px" EnableItemCaching="false"
                                                    OnClientSelectedIndexChanged="ddlService_OnClientSelectedIndexChanged">
                                                    <HeaderTemplate>
                                                        <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 300px" align="left">Service Name
                                                                </td>
                                                                <td style="width: 100px" align="left">Ref Service Code
                                                                </td>
                                                                <td style="width: 100px" align="left">CPT Code
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 300px" align="left">
                                                                    <%# DataBinder.Eval(Container, "Attributes['ServiceName']")%>
                                                                </td>
                                                                <td style="width: 100px;" align="left">
                                                                    <%# DataBinder.Eval(Container, "Attributes['RefServiceCode']")%>
                                                                </td>
                                                                <td style="width: 100px" align="left">
                                                                    <%# DataBinder.Eval(Container, "Attributes['CPTCode']")%>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row form-group">
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <asp:CheckBox ID="cbMainSurgery" runat="server" Checked="true"
                                                    Text="Main" />

                                                <asp:Button ID="btnGetInfoService" runat="server" Text="" CausesValidation="false" SkinID="button"
                                                Style="display: none;" OnClick="btnGetInfoService_OnClick" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8"></div>
                                        </div>
                                    </div>
                                            </div>
                                </div>
                                </div>
                                </div>
                            </div>
                    <div class="row">
                         <div class="col-md-6 col-sm-6 col-xs-12">
                             <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;margin-top:5px;">
                                    <div id="Div2" runat="server">
                                        <div class="row">
                                            <div class="col-md-12" style="background: #d5dfe6;padding: 3px 10px;font-weight: 600;color: #000;margin-bottom:3px">Surgeon Details</div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                        <asp:Label ID="Label14" runat="server" SkinID="label" Text="Surgeon"></asp:Label><span
                                                    style="color: Red;">*</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <telerik:RadComboBox ID="ddlSurgeon" runat="server" DropDownWidth="430px" EmptyMessage="[Select]" Filter="Contains" AutoPostBack="true"
                                                    CheckBoxes="true" Skin="Default" Width="100%" MarkFirstMatch="true" OnItemsRequested="ddlSurgeon_ItemsRequested" >
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                        <asp:Label ID="lblAnesthetist" runat="server" SkinID="label" Text="Anesthetist"></asp:Label>
                                                <asp:Label ID="lblAnesthetiststar" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                         <telerik:RadComboBox ID="ddlAnesthetist" runat="server" DropDownWidth="400px" EmptyMessage="[Select]" Filter="Contains"
                                                    Skin="Default" MarkFirstMatch="true" Width="100%">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                        <asp:Label ID="Label15" runat="server" SkinID="label" Text="Asst. Surgeon 1"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <telerik:RadComboBox ID="ddlAsstSurgeon1" runat="server" DropDownWidth="400px" EmptyMessage="[Select]" Filter="Contains"
                                                    Skin="Default" Width="100%" MarkFirstMatch="true">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                        <asp:Label ID="Label22" runat="server" SkinID="label" Text="Asst. Surgeon 2"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlAsstSurgeon2" runat="server" DropDownWidth="400px" EmptyMessage="[Select]" Filter="Contains"
                                                            Skin="Default" Width="100%" MarkFirstMatch="true">
                                                        </telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                        <asp:Label ID="Label27" runat="server" SkinID="label" Text="Asst.Anesthetist"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <telerik:RadComboBox ID="ddlAssttAnesthetist" runat="server" DropDownWidth="400px" EmptyMessage="[Select]"
                                                    Filter="Contains" Skin="Default" MarkFirstMatch="true" Width="100%">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Anaesthesia"></asp:Label>
                                                <asp:Label ID="lblAnaesthesiastar" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <telerik:RadComboBox ID="ddlAnaesthesia" runat="server" EmptyMessage="[Select]" MarkFirstMatch="true"
                                                    Filter="Contains" Width="100%"  Skin="Default">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <asp:Button ID="btnAddToList" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAddToList_Click" />
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <asp:CheckBox ID="chkEquipmentList" runat="server" Text="Equipment List" AutoPostBack="true"
                                                    ToolTip="Select Equipment List from Drop Down" OnCheckedChanged="chkEquipmentList_OnCheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                </div>
                                 </div>
                                </div>
                           <div class="col-md-6 col-sm-6 col-xs-12">
                               <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;margin-top:5px;">
                               <div id="Div3" runat="server">
                                   <div class="row">
                                            <div class="col-md-12" style="background: #d5dfe6;padding: 3px 10px;font-weight: 600;color: #000;margin-bottom:3px;">Other Resources</div>
                                        </div>
                                   <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                     <asp:Label ID="Label18" runat="server" Text="Perfusionist" SkinID="label"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                       <telerik:RadComboBox ID="ddlPerfusionist" runat="server" CheckBoxes="true" EmptyMessage="[Select]"
                                                    Skin="Default" Width="100%">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                       <asp:Label ID="Label19" runat="server" Text="Scrub Nurse(s)" SkinID="label"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <telerik:RadComboBox ID="ddlScrubNurse" runat="server" CheckBoxes="true" EmptyMessage="[Select]"
                                                    Skin="Default" Width="100%">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                   <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                     <asp:Label ID="Label20" runat="server" ToolTip="<%$ Resources:PRegistration, FloorNurse %>" Text="<%$ Resources:PRegistration, FloorNurse %>" SkinID="label"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                      <telerik:RadComboBox ID="ddlFloorNurse" runat="server" CheckBoxes="true" EmptyMessage="[Select]"
                                                    Skin="Default" Width="100%">
                                                </telerik:RadComboBox> 
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                       <%--<asp:Label ID="Label21" runat="server" ToolTip="<%$ Resources:PRegistration, OTTechnician %>" Text="<%$ Resources:PRegistration, OTTechnician %>" SkinID="label"></asp:Label>--%>
                                                   <asp:Label ID="Label21" runat="server" ToolTip=" OT Technician" Text="OT Technician" SkinID="label"></asp:Label>
                                                        </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <telerik:RadComboBox ID="ddlOTTechnician" runat="server" CheckBoxes="true" EmptyMessage="[Select]"
                                                    Skin="Default" Width="100%">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                   <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                     <asp:Label ID="lblOTEquipments" runat="server" Text="OT Equipment(s)" SkinID="label"></asp:Label>
                                                <asp:Label ID="lblOTEquipmentsStar" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                       <telerik:RadComboBox ID="ddlOTEquipments" CheckBoxes="true"  runat="server"
                                                                EmptyMessage="[Select]" Skin="Default" Width="100%" DropDownWidth="500px" Height="400px"
                                                                NoWrap="true" HighlightTemplatedItems="true" ExpandDirection="Down"
                                                                ExpandDelay="0" CollapseDelay="0" DropDownAutoWidth="Enabled">

                                                                <HeaderTemplate>
                                                                    <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td style="width: 60%" align="left">Equipment Name
                                                                            </td>
                                                                            <td style="width: 20%" align="left">Total Qty.
                                                                            </td>
                                                                            <td style="width: 20%" align="left">Available Qty.
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>

                                                                    <%# DataBinder.Eval(Container, "Attributes['EquipmentName']")%>
                                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td style="width: 60%" align="left"></td>
                                                                            <td style="width: 20%;" align="left">
                                                                                <%# DataBinder.Eval(Container, "Attributes['TotalQty']")%>
                                                                            </td>
                                                                            <td style="width: 20%" align="left">
                                                                                <%# DataBinder.Eval(Container, "Attributes['AvailableQty']")%>
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>

                                                            </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <div class="row form-group">
                                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                       <asp:TextBox ID="txtOTEquipment" runat="server" TextMode="MultiLine"
                                                                Width="100%" Height="40px"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                   <div class="row">
                                       <div class="col-md-6 col-sm-6 col-xs-12">
                                           <div class="row form-group">
                                               <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                   <asp:Label ID="Label6" runat="server" Text="Surgery Added" SkinID="label" ForeColor="White"></asp:Label>
                                               </div>
                                               <div class="col-md-8 col-sm-8 col-xs-8"></div>
                                           </div>
                                       </div>
                                       </div>
                                    
                                </div>    
                        </div>
                               </div>
            </div>
                    

            
                    <div class="clearfix"></div>
                   
                </ContentTemplate>
            </asp:UpdatePanel>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvOTBook" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="table table-condensed" OnSelectedIndexChanged="gvOTBook_OnSelectedIndexChanged">
                                    <EmptyDataTemplate>
                                        <div style="font-weight: bold; color: Red; width: 200px">
                                            No Record Found.
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField Visible="True" HeaderText="Sl. No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Surgery Name">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnOTServiceDetailId" runat="server" Value='<%#Eval("OTServiceDetailId") %>' />
                                                <asp:HiddenField ID="hdnSurgeon" runat="server" Value='<%#Eval("SR")%>' />
                                                <asp:HiddenField ID="hdnAsstSurgeon" runat="server" Value='<%#Eval("AS")%>' />
                                                <asp:HiddenField ID="hdnAnesthetist" runat="server" Value='<%#Eval("AN")%>' />
                                                <asp:HiddenField ID="hdnAssttAnesthetist" runat="server" Value='<%#Eval("AAN")%>' />
                                                <asp:HiddenField ID="hdnAnesthesiaId" runat="server" Value='<%#Eval("AnesthesiaId")%>' />
                                                <asp:HiddenField ID="hdnServiceID" runat="server" Value='<%#Eval("ServiceID")%>' />
                                                <asp:HiddenField ID="hdnScrubNurse" runat="server" Value='<%#Eval("SN")%>' />
                                                <asp:HiddenField ID="hdnFloorNurse" runat="server" Value='<%#Eval("FN")%>' />
                                                <asp:HiddenField ID="hdnPerfusionist" runat="server" Value='<%#Eval("PR")%>' />
                                                <asp:HiddenField ID="hdnTechnician" runat="server" Value='<%#Eval("TC")%>' />
                                                <asp:HiddenField ID="hdnDepartment" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                <asp:HiddenField ID="hdnSubDepartment" runat="server" Value='<%#Eval("SubDepartmentId")%>' />
                                                <asp:HiddenField ID="hdnEquipment" runat="server" Value='<%#Eval("Equipments")%>' />
                                                <asp:HiddenField ID="hdnEquipmentName" runat="server" Value='<%#Eval("EquipmentName")%>' />
                                                <asp:HiddenField ID="hdnBookingID" runat="server" Value='<%#Eval("OTBookingID")%>' />
                                                <%--<asp:HiddenField ID="hdnRemarks" runat="server" Value='<%#Eval("Remarks")%>' />--%>
                                                <asp:HiddenField ID="hdnIsMain" runat="server" Value='<%#Eval("IsMain")%>' />
                                                <asp:HiddenField ID="hdnIsPackage" runat="server" Value='<%#Eval("IsPackage")%>' />
                                                <asp:HiddenField ID="hdnTheatreId" runat="server" Value='<%#Eval("TheatreId")%>' />
                                            <asp:HiddenField ID="hdnBloodGroup" runat="server" />
                                                
                                                <asp:Label ID="lblSurgeryName" SkinID="label" runat="server" Text=' <%#Eval("ServiceName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Surgeon">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSurgeon" SkinID="label" runat="server" Text=' <%#Eval("SurgeonName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" SkinID="label" runat="server" Text=' <%#Eval("OTBookingDateF")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartTime" SkinID="label" runat="server" Text='<%#Eval("FromTime")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndTime" SkinID="label" runat="server" Text='<%#Eval("ToTime")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CausesValidation="true" CommandName="Select" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkViewDetails" runat="server" OnClick="lnkViewDetails_Click"
                                                    CommandName="Name" CommandArgument='<%#Eval("ServiceID")%>' Text="View Details" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Default" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:HiddenField ID="hdnOTBookingID" Value="0" runat="server" />
                                <asp:HiddenField ID="hdnOTBookingNO" Value="0" runat="server" />
                                <asp:HiddenField ID="hdnRegistrationId" Value="0" runat="server" />
                                <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterId" Value="0" runat="server" />
                                <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnSubDepartmentId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnCompanyCode" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnAdvance" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnIsUnplanned" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnOTBookingDateTime" runat="server" />
                                <asp:HiddenField ID="hdnOTRequestID" Value="0" runat="server" />

                                <asp:Button ID="btnAddDiagnosisSerchOnClientClose" runat="server" Style="visibility: hidden;"
                                    OnClick="btnAddDiagnosisSerchOnClientClose_OnClick" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                    </div>
            
        </div>
    
            </form>
</body>
</html>
