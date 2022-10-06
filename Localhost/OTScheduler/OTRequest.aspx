<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTRequest.aspx.cs" Inherits="OTScheduler_OTRequest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script lang="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>
    <title>OT Booking Details</title>
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <script lang="javascript" type="text/javascript">
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
            <%--function SearchPatientOnClientClose(oWnd, args) {
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
            }--%>
            function addDiagnosisSerchOnClientClose(oWnd, args) {
                $get('<%=btnAddDiagnosisSerchOnClientClose.ClientID%>').click();
            }
        </script>
        <style>
            /*.patientDiv-Photo {
                float:left!important;
                width: 3vw !important;
                padding: 12px 11px 3px;
                background: #f9f9f9;
                
            }

            .patientDiv-Table {
                float: right !important;
                width: 95vw !important;
                
            }*/
            table.table.table-small-font.table-bordered.table-striped {
                margin: 0px !important;
            }
            body{
                overflow-x:hidden!important;
            }
        </style>
    </telerik:RadScriptBlock>


    <div class="row">
        <div class="col-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlSearch" runat="server">
                    <asplUD:UserDetails ID="asplUD" runat="server" />
                    <asp:TextBox ID="txtPatientNo" runat="server" Width="100px" MaxLength="9"
                        Style="padding-left: 1px; visibility: hidden; display: none" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
            </div>
    </div>

    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-4 col-sm-4 col-4">
                <asp:Label ID="Label3" runat="server" Text="Patient Details" />
            </div>

            <div class="col-md-8 col-sm-8 col-8 text-right">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnNew" runat="server" AccessKey="N" CssClass="btn btn-primary" Text="New" ToolTip="Click to refresh window"
                            ValidationGroup="save" OnClick="btnNew_OnClick" />
                        <asp:Button ID="ibtnSave" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Save"
                            ToolTip="Click to save this booking" ValidationGroup="save" OnClick="btnSave_OnClick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row TextCenter">
            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" Style="position: relative; margin: 0px;" runat="server" Text="" Font-Bold="true" Width="100%" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div class="row" id="Div4" runat="server">
                    <div class="col-md-8 col-sm-12 col-xs-12" style="border: solid 1px #dcdcdc; padding: 10px;">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
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
                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label2" runat="server" Text="First Name" SkinID="label"></asp:Label>
                                        <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <asp:TextBox ID="txtFName" runat="server" MaxLength="50" Style="text-transform: uppercase;" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label4" runat="server" Text="Middle Name" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <asp:TextBox ID="txtMName" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label5" runat="server" Text="Last Name" SkinID="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <asp:TextBox ID="txtLName" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label23" runat="server" Text="DOB" SkinID="label"></asp:Label>
                                        <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <telerik:RadDatePicker ID="dtpDOB" Width="100%" runat="server"
                                            MinDate="01/01/1900" Skin="Metro">
                                            <ClientEvents OnDateSelected="CalCulateDOB" />
                                            <DateInput ID="DateInput2" runat="server">
                                                <ClientEvents OnError="ShowError" />
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                                        <asp:ImageButton ID="imgCalYear" runat="server" ImageUrl="~/Images/insert_table.gif"
                                            Style="visibility: hidden;" Width="6%" ToolTip="Calculate Year, Month, Days"
                                            ValidationGroup="DOB" />
                                        <asp:ImageButton ID="btnCalAge" runat="server" ImageUrl="~/Images/insert_table.gif"
                                            ToolTip="Calculate Age" ValidationGroup="GetAge" Width="1%" />
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label24" runat="server" Text="Age(Y-M-D)" SkinID="label"></asp:Label>
                                        <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:TextBox ID="txtAgeYears" runat="server" MaxLength="3" onchange="CalCulateAge();"
                                                    Width="100%" TabIndex="11"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:TextBox ID="txtAgeMonths" ValidationGroup="Save" runat="server"
                                                    onchange="CalCulateAge();" MaxLength="2" Width="100%" TabIndex="12"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:TextBox ID="txtAgeDays" runat="server" MaxLength="2" onchange="CalCulateAge();"
                                                    Width="100%" TabIndex="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label25" runat="server" Text="Gender" SkinID="label"></asp:Label><span
                                            style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <telerik:RadComboBox ID="ddlGender" runat="server" Width="100%" Filter="Contains" MarkFirstMatch="true">
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

                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label26" runat="server" SkinID="label" Text="Mobile No."></asp:Label>
                                        <span style="color: Red;">*</span>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="50" Width="100%" Style="text-transform: uppercase;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-4">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="OT Type"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-12 col-xs-8 box-col-checkbox no-p-l">
                                        <asp:RadioButton GroupName="IsEmergency" runat="server" Text=" Elective" ID="RadioIsElective" Checked="true" />

                                        <asp:RadioButton GroupName="IsEmergency" runat="server" Text=" Emergency" ID="RadioIsEmergency" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="col-md-4" style="border-top: solid 1px #dcdcdc;">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-12">
                                            <asp:Label ID="lblDiagnosis" runat="server" Text="Diagnosis" SkinID="label"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblStarDiagnosis" runat="server" ForeColor="Red" Visible="false"> *</asp:Label>
                                        </div>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtProvisionalDiagnosis" Style="min-height: 91px; max-height: 91px; max-width: 100%;" runat="server" MaxLength="200"
                                                Width="100%" Height="91px" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 col-sm-12 col-xs-12">
                        <div id="pnl1" class="row" runat="server" style="border: solid 1px #dcdcdc;">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label17" runat="server" Text="Theater" SkinID="label" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadComboBox ID="ddlOTTheater" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]"
                                                    DataTextField="DoctorName" DataValueField="DoctorId" Width="100%"
                                                    DropDownWidth="300px" Filter="Contains" ForeColor="Black">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label9" runat="server" Text="OT Date" SkinID="label"></asp:Label><span
                                                    style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadDatePicker ID="dtpOTDate" runat="server" Skin="Metro" Width="100%" MinDate="01/01/1900">
                                                    <DateInput ID="DateInput1" runat="server" AutoPostBack="True" DisplayDateFormat="dd/MM/yyyy"
                                                        DateFormat="MM/dd/yyyy" ForeColor="Black">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label10" runat="server" Text="Time" SkinID="label"></asp:Label><span
                                                    style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadTimePicker ID="RadTimeFrom" Skin="Metro" runat="server" AutoPostBack="true"
                                                    DateInput-ReadOnly="true"
                                                    PopupDirection="BottomLeft" TimeView-Columns="6" Width="100%" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblDuration" runat="server" Text="Duration" SkinID="label"></asp:Label><span
                                                    style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6 col-6 no-p-r">
                                                        <asp:TextBox ID="txtDuration" runat="server" Width="100%" CssClass="pull-left"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-6">
                                                        <telerik:RadComboBox ID="ddlHours" runat="server" Filter="Contains"
                                                            MarkFirstMatch="true" Width="100%">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="Hours" Value="H" />
                                                                <telerik:RadComboBoxItem Text="Minutes" Value="M" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label18" runat="server" SkinID="label" Text="PAC Required"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox">
                                                <asp:RadioButtonList runat="server" ID="rblIsPACRequired" Width="100%" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Selected="True" Value="1" />
                                                    <asp:ListItem Text="No" Value="0" />
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="row">
                                            <div class="col-md-2 col-sm-2 col-xs-3">
                                                <asp:Label ID="Label7" runat="server" SkinID="label" Text="Remarks" />
                                            </div>
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" Rows="2" MaxLength="200" TextMode="MultiLine"> </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="Anaesthesia"></asp:Label>
                                                <asp:Label ID="lblAnaesthesiastar" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <telerik:RadComboBox ID="ddlAnaesthesia" runat="server" EmptyMessage="[Select]" MarkFirstMatch="true"
                                                    Filter="Contains" Width="100%">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label13" runat="server" SkinID="label" Text="Infectious&nbsp;Case"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox">
                                                <asp:RadioButtonList ID="rdoInfectiousCase" runat="server" Width="100%" Font-Bold="true" RepeatDirection="Horizontal"
                                                    OnSelectedIndexChanged="rdoInfectiousCase_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="No" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label ID="Label16" runat="server" Text="Infectious Remarks" Visible="false" />
                                            </div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="txtInfectiousCaseRemarks" runat="server" TextMode="MultiLine" MaxLength="250" Rows="1" Visible="false" Width="100%" />
                                            </div>
                                        </div>
                                    </div>

                                    <asp:UpdatePanel ID="updrdoImplant" runat="server">
                                        <ContentTemplate>
                                            <h2 style="background: #f0f0f0; padding: 3px 10px; margin: 0px; font-size: 14px;">Implant Details</h2>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6 col-4">
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                                Implant Require
                                                            </div>
                                                            <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox">
                                                                <asp:RadioButtonList ID="rdoImpReq" runat="server" RepeatDirection="Horizontal"
                                                                    OnSelectedIndexChanged="rdoImpReq_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="Y" Text="Yes" />
                                                                    <asp:ListItem Value="N" Text="No" Selected="True" />
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-4">
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                                <asp:Label ID="lblRemarks" runat="server" Text="Remarks" Visible="false" />
                                                                <asp:Label ID="lbltxtImpReqRem" runat="server" Text="*" ForeColor="Red" Visible="false" />
                                                            </div>
                                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                                <asp:TextBox ID="txtImpReqRem" runat="server" TextMode="MultiLine" MaxLength="250" Rows="1" Visible="false" Width="100%" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="rdoImpReq" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6 col-sm-12 col-xs-12">
                        <div class="row" style="border-top: solid 1px #dcdcdc;">
                            <div class="col-md-12" id="Div1" runat="server">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-2 col-sm-2 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblDept" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, department %>"></asp:Label>
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-8">
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-5 col-sm-5 col-xs-4 pr-0">
                                                                <telerik:RadComboBox ID="ddlDepartment" runat="server" Width="100%" Filter="Contains"
                                                                    MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                            <div class="col-md-7 col-sm-7 col-xs-7 box-col-checkbox">
                                                                <asp:RadioButtonList ID="rblServiceType" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatLayout="Flow" OnSelectedIndexChanged="rblServiceType_OnSelectedIndexChanged" Width="100%"
                                                                    AutoPostBack="true">
                                                                    <asp:ListItem Text="Package" Value="IPP"></asp:ListItem>
                                                                    <asp:ListItem Text="Surgery" Value="S" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblSubDept" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, SubDepartment %>"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-12 col-xs-8">
                                                <asp:UpdatePanel ID="up2" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlSubDepartment" runat="server" Width="100%" Filter="Contains"
                                                            MarkFirstMatch="true">
                                                        </telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-5 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label12" runat="server" SkinID="label" Text="Surgery/Procedure"></asp:Label><span
                                                    style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-7 col-sm-12 col-xs-8 ">
                                                <telerik:RadComboBox ID="ddlService" runat="server" Width="100%" Height="300px"
                                                    EmptyMessage="[Type Service Name, Ref Service Name, CPT Code]" AllowCustomText="true" Skin="Office2007" ShowMoreResultsBox="true" EnableLoadOnDemand="true"
                                                    OnItemsRequested="ddlService_OnItemsRequested" EnableVirtualScrolling="true" DropDownWidth="500px" EnableItemCaching="false"
                                                    OnSelectedIndexChanged="ddlService_SelectedIndexChanged" AutoPostBack="true">
                                                    <HeaderTemplate>
                                                        <table style="width: 500px;" cellspacing="0" cellpadding="0">
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
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="rblServiceType" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblOTEquipments" runat="server" Text="OT Equipment(s)" SkinID="label"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-12 col-xs-8">
                                                <telerik:RadComboBox ID="ddlOTEquipments" CheckBoxes="true" runat="server"
                                                    EmptyMessage="[Select]" Width="100%" DropDownWidth="500px" Height="400px"
                                                    NoWrap="true" HighlightTemplatedItems="true" ExpandDirection="Down"
                                                    ExpandDelay="0" CollapseDelay="0" DropDownAutoWidth="Enabled">
                                                    <HeaderTemplate>
                                                        <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 60%" align="left">Equipment Name</td>
                                                                <td style="width: 20%" align="left">Total Qty.</td>
                                                                <td style="width: 20%" align="left">Available Qty.</td>
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
                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 box-col-checkbox">
                                                <asp:CheckBox ID="cbMainSurgery" runat="server" Checked="false" Text="Main" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" id="Div2" runat="server">
                                <div class="row">
                                    <h2 style="background: #f0f0f0; padding: 3px 10px; margin: 0px; font-size: 14px;">Surgeon Details</h2>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label14" runat="server" SkinID="label" Text="Surgeon"></asp:Label><span
                                                    style="color: Red;">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-12 col-xs-8">
                                                <telerik:RadComboBox ID="ddlSurgeon" runat="server" EmptyMessage="[Select]" Filter="Contains"
                                                    Width="100%" MarkFirstMatch="true">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label27" runat="server" SkinID="label" Text="Asst.Anesthetist"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-12 col-xs-8">
                                                <telerik:RadComboBox ID="ddlAssttAnesthetist" runat="server" EmptyMessage="[Select]"
                                                    Filter="Contains" MarkFirstMatch="true" Width="100%">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblAnesthetist" runat="server" SkinID="label" Text="Anesthetist"></asp:Label>
                                                <asp:Label ID="lblAnesthetiststar" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                            </div>
                                            <div class="col-md-8 col-sm-12 col-xs-8">
                                                <telerik:RadComboBox ID="ddlAnesthetist" runat="server" EmptyMessage="[Select]" Filter="Contains"
                                                    MarkFirstMatch="true" Width="100%">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-12 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label15" runat="server" SkinID="label" Text="Asst. Surgeon"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-12 col-xs-8">
                                                <div class="row">
                                                    <div class="col-md-8 col-sm-8 col-xs-9">
                                                        <telerik:RadComboBox ID="ddlAsstSurgeon" runat="server" EmptyMessage="[Select]" Filter="Contains"
                                                            Width="100%" MarkFirstMatch="true">
                                                        </telerik:RadComboBox>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-3 text-right">
                                                        <asp:Button ID="btnAddToList" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAddToList_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>




                <table cellpadding="2" cellspacing="2" width="100%" style="display: none">
                    <tr>
                        <td class="clssubtopic" style="height: 15px; width: 100%; text-align: center">
                            <asp:Label ID="Label6" runat="server" Text="Surgery Added" SkinID="label" ForeColor="White"></asp:Label>
                        </td>
                        <td class="clssubtopicbar" width="80%" style="height: 15px;">&nbsp;
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview m-t">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvOTBook" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="gvOTBook_OnSelectedIndexChanged" OnRowDeleting="gvOTBook_RowDeleting">
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
                                        <%--<asp:HiddenField ID="hdnEquipmentName" runat="server" Value='<%#Eval("EquipmentName")%>' />--%>
                                        <asp:HiddenField ID="hdnBookingID" runat="server" Value='<%#Eval("OTRequestID")%>' />
                                        <asp:HiddenField ID="hdnRemarks" runat="server" Value='<%#Eval("Remarks")%>' />
                                        <asp:HiddenField ID="hdnIsMain" runat="server" Value='<%#Eval("IsSurgeryMain")%>' />
                                        <asp:HiddenField ID="hdnOTEquipMentsId" runat="server" Value='<%#Eval("OTEquipMentsId")%>' />
                                        <%-- <asp:HiddenField ID="hdnIsInfectiousCase" runat="server" Value='<%#Eval("IsInfectiousCase")%>' />--%>
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
                                <asp:TemplateField HeaderText="Start Time" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStartTime" SkinID="label" runat="server" Text='<%#Eval("FromTime")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CausesValidation="true" CommandName="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to Remove this record"
                                            CommandName="Delete" CausesValidation="true" OnClientClick="return confirm('Are you sure you want to Removed Record?');" ImageUrl="~/Images/DeleteRow.png" Width="12px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
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
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview ">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                            Width="100%" Height="600" Left="10" Top="10">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                    Width="900" Height="600" />
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
                        <asp:Button ID="btnAddDiagnosisSerchOnClientClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddDiagnosisSerchOnClientClose_OnClick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <asp:UpdatePanel runat="server" ID="UpdatePanel15">
                <ContentTemplate>
                    <div class="col-md-12">
                        <h2 style="background: #d2d2d2; padding: 3px 10px; margin: 0px; font-size: 14px; color: #000;">
                            <asp:Label ID="Label8" runat="server" Text="OT Request" SkinID="label"></asp:Label>
                        </h2>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="col-md-12 col-sm-12 col-xs-12 gridview table-responsive">
                <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                    <ContentTemplate>
                        <asp:GridView ID="gvbindEMROTRequest" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="gvbindEMROTRequest_SelectedIndexChanged" OnRowDataBound="gvbindEMROTRequest_RowDataBound"
                            OnRowDeleting="gvbindEMROTRequest_RowDeleting">
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
                                <asp:TemplateField HeaderText="<%$ Resources:PRegistration, regno %>">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnOTRequestID" runat="server" Value='<%#Eval("OTRequestID")%>' />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value=' <%#Eval("RegistrationId")%>' />
                                        <asp:HiddenField ID="hdnStatusId" runat="server" Value=' <%#Eval("StatusId")%>' />
                                        <asp:Label ID="lblRegistrationNo" SkinID="label" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Patient Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFullName" SkinID="label" runat="server" Text=' <%#Eval("FullName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OT Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOTBookingDate" SkinID="label" runat="server" Text=' <%#Eval("OTBookingDate", "{0:dd/MM/yyyy}") +" "+ Eval("FromTime").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duration">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOTDuration" SkinID="label" runat="server" Text=' <%#Eval("OTDuration") +" "+ Eval("OTDurationType").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OT Booking Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookDate" SkinID="label" runat="server" Text=' <%#Eval("BookDate", "{0:dd/MM/yyyy}")+" "+ Eval("BookFromTime").ToString()%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Theater">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTheatreName" SkinID="label" runat="server" Text=' <%#Eval("TheatreName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" SkinID="label" runat="server" Text=' <%#Eval("Status")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewDetails" runat="server" Text="Select" CausesValidation="true" CommandName="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnewDelete" runat="server" ToolTip="Click here to Delete this record"
                                            CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete Record ?');" CommandArgument='<%#Eval("OTRequestID")%>' ImageUrl="~/Images/DeleteRow.png" Width="12px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>


        <div id="divAllergy" runat="server" visible="false" style="width: 400px; z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed; bottom: 45%; height: auto; padding: 10px; left: 38%;">
            <div style="width: 100%">
                <div class="text-center">
                    <div class="col-md-12">
                        <asp:Label ID="lblAllergyDiv" Style="font-size: 12px; font-weight: bold; margin: 0.5em 0 0; padding: 0; width: 100%; float: left; text-align: center;"
                            runat="server" Text="Do you want to De-Activate"></asp:Label>
                    </div>
                    <br />
                    <div style="width: 100%; height: 1px; background: #94D3E3; margin: 10px 0px 4px 0px"></div>
                    <div class="col-md-12">
                        <asp:Button ID="btnYesAllergy" CssClass="ICCAViewerBtn" runat="server" Text="Yes" />
                        &nbsp;<asp:Button ID="btnNoAllergy" CssClass="ICCAViewerBtn" runat="server" Text="No" />
                    </div>
                </div>
            </div>
            <!-- hundred -->
        </div>
    </div>
</asp:Content>
