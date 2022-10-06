<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="OTAppointment.aspx.cs" Inherits="OT_Scheduler_OTAppointment" Title="Akhil Systems Pvt. Ltd." %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/LegendV1.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%: Styles.Render("~/bundles/MainNewStyleWithBootstrap") %>

    <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/bootstrap.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/mainNew.css" media="all" />

      
                       
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main form-group">
                <div class="col-md-3">
                    <h2>OT Scheduler</h2>                   
                </div>
                
                <div class="col-md-9 text-right">
                      <asp:UpdatePanel ID="UpdatePanel5" runat="server" >
                   <ContentTemplate>
                      <asp:Button ID="btnOTReqList" runat="server" AccessKey="L" CssClass="btn btn-primary" Text="OT Request List"
                           ToolTip="Click to open OT Request List window" OnClick="btnOTReqList_Click" style="text-align:right;"/>
                    </ContentTemplate>
                 </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Green" style="width: auto; margin-top: 0;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                   
                </div>
            </div>
             
           
                <div class="row">
                    <div class="col-md-4">
                        <div class="row form-group">
                            <div class="col-md-2 label2">Date</div>
                            <div class="col-md-5">
                                <telerik:RadDatePicker ID="dtpDate" AutoPostBack="true" OnSelectedDateChanged="OnSelectedDateChanged_dtpDate"
                                    runat="server" MinDate="" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                    <Calendar ID="Calendar1" UseColumnHeadersAsSelectors="False" runat="server" UseRowHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                    <DateInput ID="DateInput1" AutoPostBack="True" runat="server" DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </div>
                            <div class="col-md-2 PaddingLeftSpacing">
                                <asp:Button ID="btnToday" CssClass="btn btn-primary" runat="server" Text="Today" OnClick="btnToday_Click" />
                            </div>
                            <div class="col-md-3 PaddingLeftSpacing">
                                <telerik:RadComboBox ID="ddlColor" runat="server" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="[All Status]" Value="" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-2"></div>

                    <div class="col-md-6 text-right pull-right">
                        <ucl:legend ID="Legend1" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="row form-group">
                            <div class="col-md-2 label2">Facility</div>
                            <div class="col-md-10">
                                <telerik:RadComboBox ID="ddlFacility" runat="server" AutoPostBack="true" CssClass="drapDrowHeight" Width="100%"
                                    OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <telerik:RadComboBox ID="ddlPatient" runat="server" Width="100%" Height="400px"
                             CssClass="drapDrowHeight" EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true"
                            EmptyMessage="Search by Regno, Name,Doctor,Speciality..." DropDownWidth="650px"
                            OnItemsRequested="ddlPatient_ItemsRequested" ShowMoreResultsBox="true" AllowCustomText="true"
                            EnableVirtualScrolling="true" DataValueField="RegistrationId" DataTextField="SearchedColumn"
                            OnClientSelectedIndexChanged="ddlPatient_OnClientSelectedIndexChanged">
                            <HeaderTemplate>
                                <table style="width: 690px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 80px">
                                            <asp:Literal ID="Literal1" runat="server" Text='<%$ Resources:PRegistration, Regno%>' />
                                        </td>
                                        <td style="width: 150px">Name
                                        </td>
                                        <td style="width: 150px">Doctor
                                        </td>
                                        <td style="width: 110px">Speciality
                                        </td>
                                        <td style="width: 200px">Surgery
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 690px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 80px">
                                            <%# DataBinder.Eval(Container,"Attributes['Account']" )%>
                                        </td>
                                        <td style="width: 150px;">
                                            <%# DataBinder.Eval(Container, "Attributes['PatientName']")%>
                                        </td>
                                        <td style="width: 150px;">
                                            <%# DataBinder.Eval(Container, "Attributes['Doctor']")%>
                                        </td>
                                        <td style="width: 110px;">
                                            <%# DataBinder.Eval(Container, "Attributes['Specialisation']")%>
                                        </td>
                                        <td style="width: 200px;">
                                            <%# DataBinder.Eval(Container, "Attributes['Surgery']")%>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-md-3">
                        <div class="row form-group">
                            <div class="col-md-3 label2">OT&nbsp;Name</div>
                            <div class="col-md-9">
                                <telerik:RadComboBox ID="ddlTheater" runat="server" CheckBoxes="true" CssClass="drapDrowHeight" Width="100%"
                                    EnableCheckAllItemsCheckBox="true">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 form-group">
                        <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_OnClick" CssClass="btn btn-primary" Text="Filter" />
                        <asp:Button ID="btnprint" Width="1px" CssClass="btn btn-primary" runat="server" Text="Print" Visible="false" OnClick="btnbtnprint_Click" />
                        <asp:CheckBox ID="chkOptionView" runat="server" AutoPostBack="true" Text="Weekly View" Visible="false" OnCheckedChanged="chkOptionView_CheckedChanged" />
                    </div>
                </div>

                <div class="row" id="table1">
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Default">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
           
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <asp:SqlDataSource ID="RoomsDataSource" EnableCaching="true" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>" SelectCommandType="Text" SelectCommand="select id,dbo.getdoctorname(id) as DoctorName from Employee"></asp:SqlDataSource>
                    <asp:Label ID="lablecraete" Visible="false" runat="server" Text="No provider(s) found!" Font-Bold="True" Font-Size="14px"></asp:Label>
                    <br />
                    <asp:LinkButton ID="btnlink" Visible="false" runat="server" Text="Create a provider!" Font-Bold="True" Font-Size="11px" PostBackUrl="../MPages/Employee.aspx"></asp:LinkButton>

                    <asp:Panel ID="pnlRadScheduler" runat="server" Width="100%" Height="100%">
                        <telerik:RadScheduler AllowInsert="false" HoursPanelTimeFormat="hh:mm tt" OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
                            OnAppointmentCreated="RadScheduler1_AppointmentCreated" runat="server" ID="RadScheduler1"
                            Skin="Windows7" Width="100%" CustomAttributeNames="StatusId,ColorName,StatusColor,StatusCode,EncounterId,RegistrationId,TooTipText,DoctorId,Type,BillClearance,OrderId,IsPackage,EncounterNo,RegistrationNo,CheckInOutTimeStatus,EncounterDate,OPIP,AppointmentNo,IsEmergency"
                            ShowFooter="true" SelectedDate="2010-03-18" OnClientAppointmentClick="OnClientAppointmentClick"
                            EnableDescriptionField="true" ShowNavigationPane="false" DataSubjectField="Subject"
                            DataStartField="FromTime" DataEndField="ToTime" ShowAllDayRow="false" OnClientTimeSlotClick="OnClientTimeSlotClick"
                            AllowEdit="False" DataKeyField="AppointmentId" DataDescriptionField="StatusColor"
                            OnTimeSlotContextMenuItemClicked="RadScheduler1_TimeSlotContextMenuItemClicked"
                            AllowDelete="false" OnAppointmentContextMenuItemClicked="RadScheduler1_AppointmentContextMenuItemClicked"
                            OnNavigationCommand="RadScheduler1_NavigationCommand" OnTimeSlotCreated="RadScheduler1_TimeSlotCreated"
                            OnClientAppointmentContextMenu="OnClientAppointmentContextMenu" AppointmentStyleMode="Default" style="overflow:auto;height:460px;">
                            <AppointmentContextMenuSettings EnableDefault="false" />
                            <AdvancedForm Modal="true" />
                            <TimelineView UserSelectable="false" />
                            <AppointmentTemplate>
                                <%# Eval("Subject")%>
                                <asp:Label runat="server" ID="Teacher" />
                                <asp:Label runat="server" ID="Students" />
                            </AppointmentTemplate>
                            <AppointmentContextMenus>
                                <telerik:RadSchedulerContextMenu runat="server" ID="SchedulerAppointmentContextMenu">
                                    <%--<Items>
                                    DataTextField="Status" DataValueField="Code"
                                        <telerik:RadMenuItem Text="Edit" Value="Edit" />
                                        <telerik:RadMenuItem Text="Cut" Value="Cut" />
                                        <telerik:RadMenuItem Text="Copy" Value="Copy" />
                                        <telerik:RadMenuItem IsSeparator="True" />
                                        <telerik:RadMenuItem Text="Surgery Posting" Value="Post" />
                                        <telerik:RadMenuItem Text="OT Equipment Charge" Value="OTC" />
                                        <telerik:RadMenuItem Text="Service Requisition" Value="SR" />
                                        <telerik:RadMenuItem IsSeparator="True" />
                                        <telerik:RadMenuItem Text="Consumable Order" Value="DO" />
                                        <telerik:RadMenuItem IsSeparator="True" />
                                        <telerik:RadMenuItem Text="Drug/Consumable Return" Value="DR" />
                                        <telerik:RadMenuItem Text="OT Check In Time" Value="OTIT" />
                                        <telerik:RadMenuItem Text="OT Check Out Time" Value="OTOT" />
                                        <telerik:RadMenuItem IsSeparator="True" />
                                    </Items>--%>
                                </telerik:RadSchedulerContextMenu>
                                <telerik:RadSchedulerContextMenu ID="BreakContextMenu" runat="server" DataTextField="Status"
                                    DataValueField="Code" AppendDataBoundItems="true">
                                    <Items>
                                        <telerik:RadMenuItem Text="Edit" Value="Edit" />
                                        <telerik:RadMenuItem IsSeparator="True" />
                                        <telerik:RadMenuItem Text="Cancel" Value="Delete" />
                                    </Items>
                                </telerik:RadSchedulerContextMenu>
                            </AppointmentContextMenus>
                            <TimeSlotContextMenus>
                                <telerik:RadSchedulerContextMenu runat="server" ID="SchedulerTimeSlotContextMenu">
                                    <%--<Items>
                                        <telerik:RadMenuItem Text="OT Booking" Value="OTBOOK" />
                                        <telerik:RadMenuItem IsSeparator="true" />
                                        <telerik:RadMenuItem Text="Paste" Value="Paste" />
                                        <telerik:RadMenuItem IsSeparator="true" />
                                        <telerik:RadMenuItem Text="Add Break" Value="Break" />
                                    </Items>--%>
                                </telerik:RadSchedulerContextMenu>
                            </TimeSlotContextMenus>
                            <TimeSlotContextMenuSettings EnableDefault="False" />
                        </telerik:RadScheduler>
                    </asp:Panel>
                </div>
                    </div>
           
            <div id="dvDelete" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 130px; left: 450px; top: 300px">
                <asp:UpdatePanel ID="updDelete" runat="server">
                    <ContentTemplate>
                        <table width="400px">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDeleteMessage" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="trCancelReason" runat="server" visible="false">
                                <td valign="top">
                                    <asp:Label ID="Label3" runat="server" Text="Reason"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlReason" runat="server" Visible="false" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged"></telerik:RadComboBox>
                                    <br />
                                    <asp:TextBox ID="txtCancelRemarks" runat="server" TextMode="MultiLine" Height="50px" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" style="padding-top: 10px;">
                                    <asp:Button ID="btnDeleteThisApp" SkinID="Button" runat="server" Text="Yes" OnClick="btnDeleteThisApp_Click" />
                                    <asp:Button ID="btnCancelApp" SkinID="Button" runat="server" Text="No" OnClick="btnCancelApp_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="divDeleteBreak" runat="server" visible="false" style="width: 300px; z-index: 100; background-color: White; position: absolute; bottom: 0; height: 75px; left: 520px; top: 240px;border-radius:5px;border:1px solid #ccc;box-shadow:0px 0px 5px #ccc;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 text-center">
                                    <asp:Label ID="Label2" runat="server" Text="Are You Sure You want to delete all future Break or Only this Break ?"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 text-center">
                                    <asp:Button ID="btnDeleteAllBreak" CssClass="btn btn-success" runat="server" Text="Delete All"
                                        OnClick="btnDeleteAllBreak_Click" />
                               
                                    <asp:Button ID="btnDeleteBreak" CssClass="btn btn-danger" runat="server" Text="Delete This"
                                        OnClick="btnDeleteBreak_Click" />
                               
                                    <asp:Button ID="btnClose" CssClass="btn btn-default" runat="server" Text="Cancel" OnClick="btnClose_Click" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="dvUpdateStatus" runat="server" visible="false" style="width: 300px; z-index: 100;background-color: White;position: absolute; bottom: 0; height: 120px; left: 520px; top: 240px;border-radius:5px;border:1px solid #ccc;box-shadow:0px 0px 5px #ccc;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <table width="99%" border="0" cellpadding="0" cellspacing="2px">
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Please add a reason : "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <font color='Red'>*</font>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtCancelationReasons" runat="server" SkinID="textbox" TextMode="MultiLine"
                                        Width="100%" Height="60px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnUpdateStatus" SkinID="Button" runat="server" Text="Update Status"
                                        OnClick="btnUpdateStatus_Click" /><%-- --%>
                                </td>
                                <td>
                                    <asp:Button ID="btnCancelUpdateStatus" SkinID="Button" runat="server" Text="Cancel"
                                        OnClick="btnCancelUpdateStatus_Click" />
                                </td>
                                <%--<td>
                                    <asp:Button ID="Button3" SkinID="Button" runat="server" Text="Cancel" OnClick="btnCancelApp_Click" />
                                </td>--%>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="divOTCheckinCheckoutTime" runat="server" visible="false" style="width: 400px; height:180px; z-index: 100; border: 4px solid #2fb2d4; background-color: White; position: absolute; bottom: 0; height: 285px; left: 38%; top: 250px">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>

                        <div class="container-fluid header_main">
                            <div class="col-md-12 text-center">
                                <asp:Label ID="Label4" runat="server"></asp:Label>
                            </div>
                        </div>



                        <div class="container-fluid">
                            <div class="row form-groupTop01">
                                <div class="col-md-12 text-center">
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="lblChkIN" runat="server" Text="Check In Time:" />
                                </div>
                                <div class="col-md-7 PaddingCenterSpacing">
                                    <telerik:RadDateTimePicker ID="dtDateTimeForOT" TimeView-Columns="6" runat="server"
                                        OnSelectedDateChanged="dtDateTimeForOT_SelectedDateChanged" AutoPostBackControl="TimeView" DateInput-DateFormat="dd/MM/yyyy HH:mm" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" CssClass="drapDrowHeight" Width="130px">
                                    </telerik:RadDateTimePicker>
                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="170px"
                                        MarkFirstMatch="true" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"
                                        Skin="Outlook" Width="50px" Visible="false" TimeView-Columns="6">
                                    </telerik:RadComboBox>
                                </div>
                                <%--<asp:DropDownList ID="ddlminut" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlminut_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="lblIsUnplannedSurgery" runat="server" Text="IsUnplanned Surgery" Visible="false" />
                                </div>
                                <div class="col-md-7">
                                    <div class="PD-TabRadioNew01 margin_z">
                                        <asp:RadioButtonList ID="rblIsUnplannedOT" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" Visible="false" OnSelectedIndexChanged="rblIsUnplannedOT_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                            <asp:ListItem Text="No" Value="0" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="lblIschargeAbleOt" runat="server" Text="IsChargeable OT" Visible="false" />
                                </div>
                                <div class="col-md-7">
                                    <div class="PD-TabRadioNew01 margin_z">
                                        <asp:RadioButtonList ID="rblIsChargeAbleOT" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" Visible="false">
                                            <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                            <asp:ListItem Text="No" Value="0" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="Label6" runat="server" Text="Change OT(Only for Report)" />
                                </div>
                                <div class="col-md-7 PaddingCenterSpacing">


                                    <telerik:RadComboBox ID="ddlAnotherOT" runat="server" AutoPostBack="false" Height="170px"
                                        MarkFirstMatch="true" EnableLoadOnDemand="true" Width="150px" DropDownWidth="250px">
                                    </telerik:RadComboBox>


                                </div>
                            </div>



                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="lblUnplannedSurgeryRemarks" runat="server" Text="Remarks" Width="100%" />
                                </div>
                                <div class="col-md-7 PaddingCenterSpacing">
                                    <asp:TextBox ID="txtUnplannedSurgeryRemarks" runat="server" TextMode="MultiLine" SkinID="textbox" Width="220px" Height="60px"></asp:TextBox>
                                </div>
                            </div>


                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="lblCheckoutTime" runat="server" Text="Last CheckOut Time" Width="100%" Visible="false" />
                                </div>
                                <div class="col-md-7">
                                    <asp:Label ID="lblLastChkOutTime" runat="server" />
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-5 label2 PaddingCenterSpacing">
                                    <asp:Label ID="lblDiffTime" runat="server" Text="Time Difference: " Visible="false" />
                                </div>
                                <div class="col-md-7">
                                    <asp:Label ID="lblTimeDiff" runat="server" /><asp:Label ID="Label5" runat="server" />
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-12 text-center">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                                </div>
                            </div>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                </div>
            <asp:HiddenField ID="hdnFilterID" runat="server" />
            <asp:HiddenField ID="hdnNoOfEncounter" runat="server" />
            <asp:HiddenField ID="hdnCheckoutTimecheckIgnored" runat="server" />
            <asp:HiddenField ID="hdnProviderID" runat="server" />
            <asp:HiddenField ID="hdnTempData" runat="server" Value="1" />
            <asp:HiddenField ID="hdnOTBookingdate" runat="server" />
            <asp:TextBox ID="txtQualityIds" runat="server" Style="visibility: hidden;" Width="80"
                SkinID="textbox" />
            <asp:Button ID="btnPatientSearch" runat="server" Style="visibility: hidden;" OnClick="btnPatientSearch_OnClick" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function ddlPatient_OnClientSelectedIndexChanged(sender, args) {
            $get('<%=btnPatientSearch.ClientID%>').click();
        }

        function StopPropagation(e) {
            //cancel bubbling
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }
        function onCheckBoxClick(chk) {
            var combo = $find("<%= ddlTheater.ClientID %>");
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
            var combo = $find('<%= ddlTheater.ClientID %>');
            combo.hideDropDown();
        }
        function OnClientAppointmentClick(sender, args) {
            debugger;
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
    </script>

    <script type="text/javascript">
        function resizeScheduler() {
            var scheduler = $find('<%=RadScheduler1.ClientID %>');
            scheduler.get_element().style.width = "100%";
            scheduler.repaint();
        }
    </script>
</asp:Content>
