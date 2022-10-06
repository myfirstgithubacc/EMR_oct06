<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="AppScheduler_New.aspx.cs" Inherits="Appointment_AppScheduler_New" %>
<%@ Register Src="../Include/Components/Legend.ascx" TagName="Legend" TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<%: Styles.Render("~/bundles/AppointmentStyle") %>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="wrapper">
                <div class="appointmentDiv">
                    <div class="container-fluid">
                        <div class="row">
                            <!-- Appointment Details Start -->
                            <div class="col-md-2 ADLeft">
                                <div class="AD-leftPart">
                                    <div class="AdLeft-Time">
                                        <h4>
                                            <img src="../Images/time-icon.png" border="0" alt="Time" title="Current Time"></h4>
                                        <h2>
                                            <asp:Label ID="lblDate" runat="server"></asp:Label></h2>
                                        <h3>
                                            <asp:Label ID="lblTime" runat="server"></asp:Label></h3>
                                    </div>
                                    <div class="AdLeft-Calender">
                                        <asp:Calendar ID="dtpDate" runat="server" CellPadding="2" BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" Height="180px" ForeColor="Black" DayNameFormat="Short" Width="99%" BackColor="White" OnSelectionChanged="OnSelectedDateChanged_dtpDate">
                                            <SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
                                            <NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
                                            <DayHeaderStyle Font-Size="7pt" Font-Bold="True"></DayHeaderStyle>
                                            <TitleStyle Font-Bold="True" BorderColor="Black" ForeColor="White" BackColor="#0eaf82"></TitleStyle>
                                            <OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
                                        </asp:Calendar>
                                    </div>
                                    <div class="AdLeft-box">
                                        <div class="AdLeft-Facility">
                                            <h3>Facility</h3>
                                            <telerik:RadComboBox ID="ddlFacility" AppendDataBoundItems="true" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"></telerik:RadComboBox>
                                        </div>
                                        <div class="AdLeft-Facility">
                                            <h3>Specialization</h3>
                                            <telerik:RadComboBox ID="ddlSpecilization" runat="server" AppendDataBoundItems="true" Filter="Contains" AutoPostBack="true" DropDownWidth="300px" OnSelectedIndexChanged="ddlSpecilization_SelectedIndexChanged" />
                                            <asp:Label ID="lblSpecilization" runat="server" SkinID="label" Text="" Visible="false"></asp:Label>
                                        </div>
                                        <div class="AdLeft-Facility">
                                            <h3>Doctor List</h3>
                                            <telerik:RadComboBox ID="RadLstDoctor" runat="server" AllowCustomText="True" HighlightTemplatedItems="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" DropDownWidth="300px" Height="300px" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="RadLstDoctor_SelectedIndexChanged" OnItemDataBound="RadLstDoctor_ItemDataBound" />
                                            <asp:CheckBox ID="chkShowAllProviders" runat="server" AutoPostBack="true" Font-Bold="True" OnCheckedChanged="chkShowAllProviders_CheckedChanged" Text="Show All Provider" />
                                            <asp:HiddenField ID="hdnNoOfEncounter" runat="server" />
                                        </div>
                                        <div class="AdLeft-Btn">
                                            <span class="flatLeft01"><a><i class="fa fa-search fa-1g"></i></a>
                                                <asp:Button ID="btnRefresh" runat="server" CssClass="SearchBtn" Text="Search" OnClick="btnRefresh_OnClick" /></span>
                                            <span class="flatLeft01"><a><i class="fa fa-refresh fa-1g"></i></a>
                                                <asp:Button ID="btnClear" runat="server" CssClass="SearchBtn" Text="Clear All" OnClick="btnClear_OnClick" /></span>
                                            <asp:CheckBox ID="chkOptionView" runat="server" AutoPostBack="true" Text="Work Days" Font-Bold="true" OnCheckedChanged="chkOptionView_CheckedChanged" Visible="false" />
                                        </div>
                                    </div>
                                    <div class="AD-leftPart">
                                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="AppSearchBtn" Text="List Appointment Details" Font-Bold="true" OnClick="lnkSearchAppointment_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                        <asp:LinkButton ID="LinkButton4" runat="server" Text="Online Appointment Details" CssClass="AppSearchBtn" Font-Bold="true" OnClick="lnkOnlineAppointment_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                    </div>
                                </div>
                            </div>
                            <!-- Appointment Details Ends -->

                            <!-- Appointment Day Start -->
                            <div class="col-md-9">
                                <div class="Appoint-Day">
                                    <div class="App-firstDiv">
                                        <div class="App-DayBtn">
                                            <asp:Label ID="lblMessage" runat="server" /></div>
                                    </div>
                                    <div class="Appoint-table">
                                        <div class="demo-container no-bg">
                                            <telerik:RadAjaxLoadingPanel runat="Server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
                                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                                <AjaxSettings>
                                                    <telerik:AjaxSetting AjaxControlID="Configuratorpanel1">
                                                        <UpdatedControls>
                                                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                                            <telerik:AjaxUpdatedControl ControlID="Configuratorpanel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                                        </UpdatedControls>
                                                    </telerik:AjaxSetting>
                                                </AjaxSettings>
                                            </telerik:RadAjaxManager>

                                            <asp:Label ID="lablecraete" Visible="false" runat="server" Text="No provider(s) found! " Font-Bold="True" Font-Size="14px"></asp:Label>
                                            <asp:LinkButton ID="btnlink" Visible="false" runat="server" Text="Create a provider!" Font-Bold="True" Font-Size="11px" PostBackUrl="../MPages/Employee.aspx"></asp:LinkButton>
                                            <telerik:RadScheduler ID="RadScheduler1" runat="server" Skin="Metro" AllowInsert="false"
                                                OnClientDataBound="OnClientDataBound" HoursPanelTimeFormat="hh:mm tt" OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
                                                OnAppointmentCreated="RadScheduler1_AppointmentCreated" Width="100%" Height="550px"
                                                CustomAttributeNames="Code,StatusId, ColorName, StatusColor, RegistrationId,RegistrationNo, DoesPatientOweMoney,CompanyCode, ValidPaymentCaseId, TooTipText, DoctorId, Type, EligibilityChecked, VisitType,InvoiceId,RecurrenceRule,RealDateOfBirth, NoPAForInsurancePatient, NoBillForPrivatePatient, AuthorizationNo,PackageId,PackageType "
                                                ShowFooter="true" OnClientAppointmentClick="OnClientAppointmentClick" EnableDescriptionField="true"
                                                ShowNavigationPane="false" OnAppointmentDelete="RadScheduler1_AppointmentDelete"
                                                DataSubjectField="Subject" DataStartField="FromTime" DataEndField="ToTime" AllDayRow="false"
                                                OnClientTimeSlotClick="OnClientTimeSlotClick" AllowEdit="False" DataKeyField="AppointmentId"
                                                DataDescriptionField="StatusColor" OnTimeSlotContextMenuItemClicked="RadScheduler1_TimeSlotContextMenuItemClicked"
                                                AllowDelete="false" OnAppointmentContextMenuItemClicked="RadScheduler1_AppointmentContextMenuItemClicked"
                                                OnNavigationCommand="RadScheduler1_NavigationCommand" OnTimeSlotCreated="RadScheduler1_TimeSlotCreated"
                                                OnClientAppointmentContextMenu="OnClientAppointmentContextMenu" ShowAllDayRow="true"
                                                AppointmentStyleMode="Default">
                                                <AppointmentContextMenuSettings EnableDefault="false" />
                                                <AdvancedForm Modal="true" />
                                                <TimelineView UserSelectable="false" />
                                                <AppointmentTemplate>
                                                    <%# Eval("Subject")%>
                                                    <asp:Label runat="server" ID="Teacher" />
                                                    <asp:Label runat="server" ID="Students" />
                                                </AppointmentTemplate>

                                                <AppointmentContextMenus>
                                                    <telerik:RadSchedulerContextMenu ID="SchedulerAppointmentContextMenu" runat="server">
                                                    </telerik:RadSchedulerContextMenu>
                                                    <telerik:RadSchedulerContextMenu ID="BreakContextMenu" runat="server" DataTextField="Status">
                                                    </telerik:RadSchedulerContextMenu>
                                                </AppointmentContextMenus>

                                                <TimeSlotContextMenus>
                                                    <telerik:RadSchedulerContextMenu ID="SchedulerTimeSlotContextMenu" runat="server">
                                                    </telerik:RadSchedulerContextMenu>
                                                </TimeSlotContextMenus>
                                                <TimeSlotContextMenuSettings EnableDefault="False" />
                                            </telerik:RadScheduler>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <!-- Appointment Day Ends -->

                            <!-- Appointment Summary Start -->
                            <div class="col-md-1 ADLeft">
                                <div class="App-Summary">
                                    <asp:Button ID="btnFindHours" runat="server" CssClass="NextPageBtn" Text="Find Next" OnClick="btnFindNext_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                    <asp:Button ID="btnViewProviderTimings" CssClass="NextPageBtn" runat="server" Text="Work Hours" OnClick="btnViewProviderTimings_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                </div>
                                <div class="App-Summary">
                                    <div class="AS-Heading">
                                        <h2>Summary</h2>
                                    </div>
                                    <div class="AS-chartY" onclick="OnClientNewAppointment(this)">
                                        <div class="AS-chart-left">
                                            <h3>
                                                <asp:Label ID="lblNewPatient" runat="server" Font-Bold="true" Text="0"></asp:Label></h3>
                                        </div>
                                        <div class="AS-chart-right">
                                            <h6>
                                                <img src="../Images/SNew-Patient.png" width="36" height="36" alt="New Patient" title="New Patient" border="0"></h6>
                                        </div>
                                        <h4>
                                            <asp:LinkButton ID="lnkNewPatient" runat="server" Text="New Patient" OnClick="lnkNewPatient_OnClick"></asp:LinkButton></h4>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="AS-chartG" onclick="OnClientAppointment(this)">
                                        <div class="AS-chart-left">
                                            <h3>
                                                <asp:Label ID="lblAppoint" runat="server" Font-Bold="true" Text="0"></asp:Label></h3>
                                        </div>
                                        <div class="AS-chart-right">
                                            <h6>
                                                <img src="../Images/SAppointments.png" width="36" height="36" alt="Appointments" title="Appointment" border="0"></h6>
                                        </div>
                                        <h4>
                                            <asp:LinkButton ID="lnkAppointment" runat="server" Text="Appointment" OnClick="lnkAppointment_OnClick"></asp:LinkButton></h4>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="AS-chartP" onclick="OnClientConfirmdAppointment(this)">
                                        <div class="AS-chart-left">
                                            <h3>
                                                <asp:Label ID="lblConfirmed" runat="server" Font-Bold="true" Text="0"></asp:Label></h3>
                                        </div>
                                        <div class="AS-chart-right">
                                            <h6>
                                                <img src="../Images/SConfirmed.png" width="36" height="36" alt="Confirmed" title="Confirmed" border="0"></h6>
                                        </div>
                                        <h4>
                                            <asp:LinkButton ID="lnkConfirmed" runat="server" Text="Confirmed" OnClick="lnkConfirmed_OnClick"></asp:LinkButton></h4>
                                        <div class="clearfix"></div>
                                    </div>

                                    <div class="AS-chartGreen" onclick="OnClientCheckedInAppointment(this)">
                                        <div class="AS-chart-left">
                                            <h3>
                                                <asp:Label ID="lblCheckIn" runat="server" Font-Bold="true" Text="0"></asp:Label></h3>
                                        </div>
                                        <div class="AS-chart-right">
                                            <h6>
                                                <img src="../Images/checked.png" width="36" height="36" alt="Others" title="Checked In" border="0"></h6>
                                        </div>
                                        <h4>
                                            <asp:LinkButton ID="lnlCheckIn" runat="server" Text="Checked In" OnClick="lnlCheckIn_OnClick"></asp:LinkButton></h4>
                                        <div class="clearfix"></div>
                                    </div>

                                    <div class="AS-chartPink" onclick="OnClientCancelleddAppointment(this)">
                                        <div class="AS-chart-left">
                                            <h3>
                                                <asp:Label ID="lblCancelled" runat="server" Font-Bold="true" Text="0"></asp:Label></h3>
                                        </div>
                                        <div class="AS-chart-right">
                                            <h6>
                                                <img src="../Images/SCancelled.png" width="36" height="36" alt="Cancelled" title="Cancelled" border="0"></h6>
                                        </div>
                                        <h4>
                                            <asp:LinkButton ID="lnkCancelled" runat="server" Text="Cancelled" OnClick="lnkCancelled_OnClick"></asp:LinkButton></h4>
                                        <div class="clearfix"></div>
                                    </div>

                                    <div class="AS-chartPayment" onclick="OnClientPaymentAppointment();">
                                        <div class="AS-chart-left">
                                            <h3>
                                                <asp:Label ID="lblPayment" runat="server" Font-Bold="true" Text="0"></asp:Label></h3>
                                        </div>
                                        <div class="AS-chart-right">
                                            <h6>
                                                <img src="../Images/checked.png" width="36" height="36" alt="Others" title="Checked In" border="0"></h6>
                                        </div>
                                        <h4>
                                            <asp:LinkButton ID="lnkPayment" runat="server" Text="Payment" OnClick="lnkPayment_OnClick"></asp:LinkButton></h4>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div>
                                        <asp:Table ID="tblSummary" runat="server" CellPadding="5" Style="padding-left: 10px;" Visible="false"></asp:Table>
                                    </div>
                                </div>
                            </div>
                            <!-- Appointment Summary Ends -->
                            <div class="clearfix"></div>
                        </div>
                        <div class="row">
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </div>

                    </div>
                </div>
                <!-- Appointment Part Ends -->
            </div>

            <div id="dvDeleteRecurring" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 150px; left: 450px; top: 300px">
                <asp:UpdatePanel ID="updDelete" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblDeleteMessage" runat="server" Text="Delete ?"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblRecurringDeleteMessage" runat="server" ForeColor="Red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Reason : "></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlRemarks" runat="server" AppendDataBoundItems="true" SkinID="DropDown"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Remarks :"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtRecurRemark" runat="server" SkinID="textbox" TextMode="MultiLine" Style="max-height: 70px; min-height: 70px; max-width: 200px; min-width: 200px;"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDeleteAllApp" SkinID="Button" runat="server" Text="Cancel All" OnClick="btnDeleteAllApp_Click" /></td>
                                <td>
                                    <asp:Button ID="btnDeleteThisApp" SkinID="Button" runat="server" Text="Cancel This" OnClick="btnDeleteThisApp_Click" /></td>
                                <td>
                                    <asp:Button ID="btnCancelRecurrApp" SkinID="Button" runat="server" Text="Close" OnClick="btnCancelRecurrApp_OnClick" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers></Triggers>
                </asp:UpdatePanel>
            </div>

            <div id="dvDelete" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 150px; left: 450px; top: 300px">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
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
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Reason : "></asp:Label><font color='Red'>*</font></td>
                                <td>
                                    <asp:DropDownList ID="ddlRemarkss" runat="server" AppendDataBoundItems="true" Width="200px" SkinID="DropDown"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Remarks :"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtCancel" runat="server" SkinID="textbox" TextMode="MultiLine" Style="max-height: 70px; min-height: 70px; max-width: 200px; min-width: 200px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnDeleteApp" SkinID="Button" runat="server" Text="Cancel" OnClick="btnDeleteApp_Click" />
                                    <asp:Button ID="btnCancelApp" SkinID="Button" runat="server" Text="Close" OnClick="btnCancelApp_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers></Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="divDeleteBreak" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 75px; left: 450px; top: 300px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label2" runat="server" Text="Are You Sure You want to delete all future Break or Only this Break ?" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDeleteAllBreak" SkinID="Button" runat="server" Text="Cancel All" OnClick="btnDeleteAllBreak_Click" /></td>
                                <td>
                                    <asp:Button ID="btnDeleteBreak" SkinID="Button" runat="server" Text="Cancel This" OnClick="btnDeleteBreak_Click" /></td>
                                <td>
                                    <asp:Button ID="btnClose" SkinID="Button" runat="server" Text="Close" OnClick="btnClose_Click" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers></Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="divNoSLot" runat="server" visible="false" class="Appt-popupBg">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="No slot define! Do you still want to give appointment "></asp:Label></td>
                            </tr>
                            <tr class="cancelDiv01">
                                <td class="ApptBtn">
                                    <asp:Button ID="btnCancel" CssClass="AppointmentNewBtn01" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                    <asp:Button ID="btnMakeAppointment" CssClass="AppointmentNewBtn01" runat="server" Text="Appointment" OnClick="btnMakeAppointment_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers></Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:HiddenField ID="hdnFilterID" runat="server" />
            <asp:HiddenField ID="hdnProviderID" runat="server" />
            <asp:TextBox ID="txtQualityIds" runat="server" Style="visibility: hidden;" Width="80"
                SkinID="textbox"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>

<%: Scripts.Render("~/bundles/AppointmentJs") %>
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
                myButton.className = "NextPageBtn";

                myButton.value = "Processing...";
            }
            return true;
        }
    </script>
    <script>
        $(function () {
            $('#ms').change(function () {
                console.log($(this).val());
            }).multipleSelect({

            });
        });
    </script>
    <script type="text/javascript">
        function StopPropagation(e) {
            //cancel bubbling
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }
        function onCheckBoxClick(chk) {
            var combo = $find("<%= RadLstDoctor.ClientID %>");
            //prevent second combo from closing
            cancelDropDownClosing = true;
            //holds the text of all checked items
            var text = "";
            //holds the values of all checked items
            var values = "";
            //get the collection of all items
            var items = combo.get_items();
            //enumerate all items
            var j = 0;
            for (var i = 0; i < items.get_count() ; i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combo.get_id() + "_i" + i + "_chk1");
                if (chk1.checked) {
                    if (j <= items.get_count()) {
                        text += item.get_text() + ",";
                        values += item.get_value() + ",";
                        j = j + 1;
                    }
                    else {
                        chk1.checked = false;
                    }
                }
            }
            //remove the last comma from the string
            text = removeLastComma(text);
            values = removeLastComma(values);
            if (text.length > 0) {
                //set the text of the combobox
                combo.set_text(text);
                $get('<%=txtQualityIds.ClientID%>').value = values;
            }
            else {
                //all checkboxes are unchecked
                //so reset the controls
                combo.set_text("");
                $get('<%=txtQualityIds.ClientID%>').value = '';
            }
        }

        function CloseMe() {
            var combo = $find('<%= RadLstDoctor.ClientID %>');
            combo.hideDropDown();
        }
        function OnClientAppointmentClick(sender, args) {
            sender._contextMenuAppointment = args.get_appointment();
            args.get_appointment().showContextMenu(args.get_domEvent());
        }

        function OnClientClose(oWnd) {
            $get('<%=btnRefresh.ClientID%>').click();
        }

        function OnClientTimeSlotClick(sender, args) {
            sender._showTimeSlotContextMenu(args.get_domEvent(), args.get_targetSlot());

        }
        function OnClientAppointmentContextMenu(sender, args) {
            var app = args.get_appointment();
            var attValue = app.get_attributes().getAttribute('StatusId');
            if (attValue == "0") {
                sender.get_appointmentContextMenus()[0].set_enabled(false);
            }
            else {
                sender.get_appointmentContextMenus()[0].set_enabled(true);
            }
        }
        function OnClientCloseNextSlot(oWnd, args) {
        }

        function visfalse() {
            var scheduler = $find('<%= RadScheduler1.ClientID %>');
            scheduler.style.visibility = 'hidden';
        }
        function vistrue() {
            var scheduler = $find('<%= RadScheduler1.ClientID %>');
            scheduler.style.visibility = 'visible';
        }
        function pageLoad() {
            var d = new Date();
            var scheduler = $find('<%= RadScheduler1.ClientID %>');
            if (d.getHours() > 11) {
                stime = "TimeSlotCssFor" + d.getHours() + "PM";
                var scrolledIntoViewSlot = $telerik.getElementByClassName(scheduler.get_element(), stime, "td");
            }
            else {
                stime = "TimeSlotCssFor" + d.getHours() + "AM";
                var scrolledIntoViewSlot = $telerik.getElementByClassName(scheduler.get_element(), stime, "td");
            }
        }
        function OnClientNewAppointment() {
            $get('<%=lnkNewPatient.ClientID%>').click();
        }
        function OnClientAppointment() {
            $get('<%=lnkAppointment.ClientID%>').click();
        }
        function OnClientConfirmdAppointment() {
            $get('<%=lnkConfirmed.ClientID%>').click();
        }
        function OnClientCheckedInAppointment() {
            $get('<%=lnlCheckIn.ClientID%>').click();
        }
        function OnClientCancelleddAppointment() {
            $get('<%=lnkCancelled.ClientID%>').click();
        }
        function OnClientPaymentAppointment() {
            $get('<%=lnkPayment.ClientID%>').click();
        }


        function OnClientDataBound(sender) {
            try {
                var $ = $telerik.$;
                var now = new Date();
                $(".rsContentTable:visible td", sender.get_element()).each(function (i) {
                    var currentTimeSlot = sender.get_activeModel().getTimeSlotFromDomElement($(this).get(0));
                    if (currentTimeSlot.get_startTime().getHours() == now.getHours()) {
                        var scrolledIntoViewSlot = $(this).get(0);
                        if (scrolledIntoViewSlot)
                            scrolledIntoViewSlot.scrollIntoView();
                    }
                });
            }
            catch (e) {
                alert(e.Message);
            }
        }

    </script>
    <script type="text/javascript">
        function resizeScheduler() {
            var scheduler = $find('<%=RadScheduler1.ClientID %>');
            scheduler.get_element().style.width = "100%";
            scheduler.repaint();
        }
    </script>
</asp:Content>
