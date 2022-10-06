using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

public partial class Appointment_AppSchedulerV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    DAL.DAL dl;
    BaseC.clsBb objBb;
    DataTable dtTemp;
    DataSet Statusdt;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MASTER"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
    }
    //public void setControlHospitalbased()
    //{
    //    string AppointmentFutureDays = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
    //         common.myInt(Session["FacilityId"]), "AppointmentScheduleDate", sConString);

    //    if (AppointmentFutureDays != "")
    //    {
    //        //   dtpDate.SelectedDate = DateTime.Now.AddDays(common.myInt(AppointmentFutureDays));
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lblDate.Text = String.Format("{0:ddd, d MMM, yyyy}", DateTime.Now);
            lblTime.Text = DateTime.Now.ToString("hh:mm tt");
            // Legend1.loadLegend("Appointment", "");
            if (!IsPostBack)
            {
                ViewState["Mpg"] = Request.QueryString["Mpg"];
                // tblDocProfile.Visible = false;

                dtpDate.TodayDayStyle.BackColor = System.Drawing.Color.Red;
                dtpDate.TodayDayStyle.ForeColor = System.Drawing.Color.White;
                dtpDate.TodayDayStyle.Font.Bold = true;
                //  dtpDate.TodayDayStyle.BorderStyle = BorderStyle.Ridge ;


                DateTime today = DateTime.Today;
                dtpDate.TodaysDate = today;
                dtpDate.SelectedDate = dtpDate.TodaysDate;
                #region UnConf status
                BaseC.clsLISPhlebotomy objlis = new BaseC.clsLISPhlebotomy(sConString);
                DataSet dsStatus = objlis.getStatus(common.myInt(Session["HospitalLocationId"]), "Appointment", "U");

                ViewState["UnConfColor"] = "#FFFBC7";
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    ViewState["UnConfColor"] = (common.myStr(dsStatus.Tables[0].Rows[0]["StatusColor"]) == "123") ? "#FFFBC7" : common.myStr(dsStatus.Tables[0].Rows[0]["StatusColor"]);
                }
                #endregion

                BindGrpTagSchedulerTimeSlotContextMenu();
                BindGrpTagSchedulerAppointmentContextMenu();
                BindGrpTagBreakContextMenu();
                //Done By Ujjwal 17 June 2015 to create menu on role based End

                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsinput = new Hashtable();
                hsinput.Add("@intfacilityid", Convert.ToInt32(Session["FacilityID"]));
                string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
                DataSet ds = new DataSet();
                ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"].ToString());
                        ViewState["SELECTED_DATE"] = common.myDate(DateTime.Now);
                        // ViewState["SELECTED_DATE"] = common.myDate(DateTime.UtcNow).AddMinutes(timezone);
                    }
                    else
                    {

                        ViewState["SELECTED_DATE"] = common.myDate(DateTime.Now);
                        Alert.ShowAjaxMsg("Please Enter Offset Time in Facilitymaster Table in Database", Page);
                        return;
                    }
                }

                dtpDate.SelectedDate = Convert.ToDateTime(ViewState["SELECTED_DATE"]);
                //// RadScheduler1.MinutesPerRow = 10;

                // =============== Star Set Current Start hour and End Hour ==================================
                //int Shour = DateTime.Now.Hour;
                //int Ehour =  DateTime.Now.AddHours(6).Hour;
                //RadScheduler1.DayStartTime = new TimeSpan(Shour, 0, 0);
                //RadScheduler1.DayEndTime = new TimeSpan(Ehour, 0, 0);

                // =============== END Set Current Start hour and End Hour ==================================

                BindControl();
                BindSpeciliazation();
                ddlFacility.SelectedValue = Session["FacilityId"].ToString();
                fillRemarks();
                BaseC.Hospital hospital = new BaseC.Hospital(sConString);
                DataSet dsHoliday = hospital.GetHospitalHoliday(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToDateTime(dtpDate.SelectedDate).ToString("dd/MM/yyyy"));
                if (dsHoliday.Tables[0].Rows.Count > 0)
                {
                    ViewState["Holiday"] = true;
                    Alert.ShowAjaxMsg("Public Holiday", Page);
                }
                else
                {
                    ViewState["Holiday"] = false;
                }
                populatColorList();
                BindControlForFollowUpAppointment();
                ViewState["Leave"] = null;
                ViewState["Holiday"] = null;

                if (common.myStr(Session["RedirectedPage"]) == "Yes")
                {
                    // btnRefresh_OnClick(sender, e);

                }
                //ddlFacility_SelectedIndexChanged(this, null);
                RadScheduler1.DayView.HeaderDateFormat = Session["OutputDateFormat"].ToString();//String.Format("{0:ddd, d MMM, yyyy}", DateTime.Now);
                btnRefresh_OnClick(sender, e);
            }
            // setControlHospitalbased();

        }
        catch (Exception ex)
        {


        }
    }

    private void BindControlForFollowUpAppointment()
    {
        if (Session["FollowUpDoctorId"] == null)
        {
            ddlSpecilization.Enabled = true;
            chkShowAllProviders.Enabled = true;
            RadLstDoctor.Enabled = true;
            Session["FollowUpRegistrationId"] = null;
        }
        else
        {
            if (Session["encounterid"] == null)
            {
                Response.Redirect("/Default.aspx?RegNo=0", false);
            }
            DataSet ds = new DataSet();

            chkShowAllProviders.Enabled = true;
            BaseC.Appointment app = new BaseC.Appointment(sConString);
            ds = app.getDoctorSpecialization(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["DoctorId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.SelectedValue = ds.Tables[0].Rows[0]["SpecializationId"].ToString();
                ddlSpecilization_SelectedIndexChanged(this, null);
            }
            if (Session["RegistrationId"] != null)
            {
                // btnRefresh_OnClick(this, null);
                Session["FollowUpRegistrationId"] = Session["RegistrationId"];
            }
            else
            {
                //  btnRefresh.Enabled = false;
            }
        }
    }
    private void BindSpeciliazation()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            Hashtable hsh = new Hashtable();
            hsh.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hsh.Add("@intFacilityId", Convert.ToInt16(ddlFacility.SelectedValue));

            DataSet dsSpeciliazation = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorTimeSpecialisation", hsh);
            if (dsSpeciliazation.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.Items.Clear();
                foreach (DataRowView drReferal in dsSpeciliazation.Tables[0].DefaultView)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)drReferal["NAME"];
                    item.Value = drReferal["ID"].ToString();
                    item.Attributes.Add("Code", common.myStr(drReferal["Code"]));
                    ddlSpecilization.Items.Add(item);
                    item.DataBind();
                }
                ddlSpecilization.Items.Insert(0, new RadComboBoxItem("", "0"));
                if (common.myStr(Session["SpecializationId"]) != "")
                {
                    ddlSpecilization.SelectedValue = common.myStr(Session["SpecializationId"]);
                    if (common.myInt(ddlSpecilization.SelectedValue) != common.myInt(Session["SpecializationId"]))
                    {
                        Session["SpecializationId"] = null;
                    }
                    //Request.QueryString.Clear();
                }


                ddlSpecilization_SelectedIndexChanged(this, null);
                // btnRefresh_OnClick(null, null);
            }
            else
            {
                ddlSpecilization.Text = "";
                ddlSpecilization.Items.Clear();
                Alert.ShowAjaxMsg("Specialization not available", Page);
                return;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
    }
    private void populatColorList()
    {
        objBb = new BaseC.clsBb(sConString);
        try
        {
            RadComboBoxItem lst;
            RadComboBoxItem lstStatusColor;
            Statusdt = new DataSet();
            Statusdt = objBb.GetStatusMaster("Appointment");
            if (Statusdt.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Statusdt.Tables[0].Rows.Count; i++)
                {
                    lst = new RadComboBoxItem();
                    lstStatusColor = new RadComboBoxItem();
                    lst.Attributes.Add("style", "background-color:" + Statusdt.Tables[0].Rows[i].ItemArray[2].ToString() + ";");
                    lst.Text = Statusdt.Tables[0].Rows[i].ItemArray[1].ToString();
                    lstStatusColor.Text = Statusdt.Tables[0].Rows[i].ItemArray[2].ToString();
                    lst.Value = Statusdt.Tables[0].Rows[i].ItemArray[2].ToString();
                    lstStatusColor.Value = Statusdt.Tables[0].Rows[i].ItemArray[1].ToString();
                    //ddlColor.Items.Add(lst);
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
        }
    }

    protected void chkOptionView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOptionView.Checked == true)
        {
            RadScheduler1.FirstDayOfWeek = DayOfWeek.Monday;
            RadScheduler1.LastDayOfWeek = DayOfWeek.Friday;
        }
        else
        {
            RadScheduler1.FirstDayOfWeek = DayOfWeek.Sunday;
            RadScheduler1.LastDayOfWeek = DayOfWeek.Saturday;
        }
    }
    DataSet GetAppointmentStatus()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet Reasondt = new DataSet();
        try
        {
            Hashtable HashIn = new Hashtable();

            Reasondt = dl.FillDataSet(CommandType.Text, "select Code,Status,statusId from GetStatus(" + common.myInt(Session["HospitalLocationId"]) + ",'Appointment')");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
        }
        return Reasondt;
    }
    DataSet GetAppointmentProvider()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet Providerdt = new DataSet();
        try
        {
            Hashtable HashIn = new Hashtable();



            Providerdt = dl.FillDataSet(CommandType.Text, "select id,dbo.getdoctorname(id) as DoctorName from Employee");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
        return Providerdt;
    }
    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        WebControl rsAptInner = (WebControl)e.Container.Parent;
        if (common.myInt(e.Appointment.Attributes["StatusId"]) == 1)
        {
            rsAptInner.Style["background"] = common.myStr(ViewState["UnConfColor"]);// "#FFFBC7";
            //e.Appointmen = false;
        }
        else
        {
            rsAptInner.Style["background"] = e.Appointment.Attributes["StatusColor"]; //"green";     
            //e.Appointment.AllowEdit = false;
        }
    }
    protected void gvLegend_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label colorstatus = new Label();
            colorstatus = (Label)e.Row.FindControl("ColorStatus");
            colorstatus.BackColor = System.Drawing.ColorTranslator.FromHtml(colorstatus.Text);
            e.Row.Cells[1].BackColor = System.Drawing.ColorTranslator.FromHtml(colorstatus.Text);
            colorstatus.Text = "";
        }
    }
    protected void RadScheduler1_TimeSlotCreated(object sender, Telerik.Web.UI.TimeSlotCreatedEventArgs e)
    {
        try
        {
            if ((DateTime.Compare(e.TimeSlot.Start.Date, DateTime.Now.Date) == 0)
                    & e.TimeSlot.Start.Hour == DateTime.Now.Hour)
            {
                int endMinute = e.TimeSlot.End.Minute;
                if (e.TimeSlot.Start.Minute > e.TimeSlot.End.Minute)
                {
                    endMinute = 60;
                }
                if ((e.TimeSlot.Start.Minute <= DateTime.Now.Minute) & (DateTime.Now.Minute < endMinute))
                {

                    ViewState["SlotTime"] = TimeSpan.Parse(e.TimeSlot.Start.Hour + ":" + e.TimeSlot.Start.Minute);
                    e.TimeSlot.CssClass = "CurrentTimeSlotStyle";
                }
            }

            dtTemp = new DataTable();
            dtTemp = (DataTable)ViewState["Disable"];
            if (dtTemp != null)
            {
                if (dtTemp.Rows.Count > 0)
                {
                    DataRow[] dr;
                    string fromtime = "'1900/01/01 " + e.TimeSlot.Start.Hour.ToString() + ":" + e.TimeSlot.Start.Minute + "'";
                    string totime = "'1900/01/01 " + e.TimeSlot.End.Hour.ToString() + ":" + e.TimeSlot.End.Minute + "'";
                    string resounceId = e.TimeSlot.Resource.Key.ToString();

                    string expr = "";
                    if (Convert.ToString(ViewState["CurrentView"]) == "W")
                    {
                        string sday = e.TimeSlot.Start.DayOfWeek.ToString();
                        switch (sday)
                        {
                            case "Sunday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Sun' ";
                                    break;
                                }
                            case "Monday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Mon' ";
                                    break;
                                }
                            case "Tuesday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Tue' ";
                                    break;
                                }
                            case "Wednesday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Wed' ";
                                    break;
                                }
                            case "Thursday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Thu' ";
                                    break;
                                }
                            case "Friday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Fri' ";
                                    break;
                                }
                            case "Saturday":
                                {
                                    expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Sat' ";
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    else
                    {

                        expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") ";


                    }
                    dr = dtTemp.Select(expr);

                    if (dr.Length == 0)
                    {
                        e.TimeSlot.CssClass = "Disabled";
                    }
                }
                else
                {

                    e.TimeSlot.CssClass = "Disabled";

                }
            }
            else
            {

                e.TimeSlot.CssClass = "Disabled";
            }

            int endMinute1 = e.TimeSlot.End.Minute;
            if (e.TimeSlot.Start.Minute > e.TimeSlot.End.Minute)
            {
                endMinute1 = 60;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private DataTable getDisableData(DateTime selectedDate)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            Hashtable HashIn = new Hashtable();

            String strProvider = "";

            if (ddlFacility.SelectedValue == "0")
            {
                if (ddlFacility.SelectedIndex == -1)
                {
                    return null;
                }
                else
                {
                    strProvider = "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";
                }
            }
            else
            {
                foreach (RadListBoxItem currentItem in RadLstDoctor.Items)
                {
                    if (currentItem.Checked == true)
                    {
                        String value = currentItem.Value;

                        strProvider = strProvider + "<Table1><c1>" + value + "</c1></Table1>";
                    }
                }
            }

            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intFacilityId", (ddlFacility.SelectedValue == "0") ? "" : ddlFacility.SelectedValue);

            HashIn.Add("@xmlDoctorIds", strProvider);
            HashIn.Add("@chrForDate", selectedDate.ToString("yyyy/MM/dd"));

            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointments", HashIn);
            ViewState["Disable"] = ds.Tables[3];
            ViewState["Lunch"] = ds.Tables[1];
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
        return ds.Tables[3];
    }
    protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {
        try
        {
            int iAppointmentId = 0;
            e.Appointment.ToolTip = e.Appointment.Attributes["TooTipText"];
            if (common.myInt(e.Appointment.Attributes["InvoiceId"]) > 0)
            {
                iAppointmentId = common.myInt(e.Appointment.Attributes["InvoiceId"]);
            }

            if (e.Appointment.Attributes["Type"].ToString().Trim() != "Break" && e.Appointment.Attributes["Type"].ToString().Trim() != "Block")
            {
                if (e.Appointment.RecurrenceState == RecurrenceState.Occurrence)//if appointment is recurring
                {
                    //if (common.myStr(e.Appointment.Attributes["Code"]) == "P")
                    //{
                    //    e.Appointment.Attributes["StatusId"] = "3";
                    //}

                    string imagePath = "&nbsp;<img src=/Icons/recurring-icon.png width=20px height=20px alt=Image />";
                    if (e.Appointment.Attributes["EligibilityChecked"].ToString() != "")
                    {
                        if (Convert.ToBoolean(e.Appointment.Attributes["EligibilityChecked"].ToString()) == true)
                        {
                            e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%'><td width='100%' ><span>" + e.Appointment.Subject + "</span>&nbsp;" + imagePath + "</td></tr></table>";
                        }
                        else
                        {
                            e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%'><td width='100%' ><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/recurring-icon.png width=20px height=20px alt=Image /></td></tr></table>";
                        }
                    }
                    else
                    {
                        e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%'><td width='100%' ><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/recurring-icon.png width=20px height=20px alt=Image /></td></tr></table>";

                    }
                }
                else
                {
                    if (e.Appointment.Attributes["EligibilityChecked"].ToString() != "")
                    {
                        if (Convert.ToBoolean(e.Appointment.Attributes["EligibilityChecked"].ToString()) == true)
                        {
                            if (e.Appointment.Attributes["DoesPatientOweMoney"].ToString() != "")
                            {
                                if (e.Appointment.Attributes["DoesPatientOweMoney"].ToString() != "0")
                                {

                                    e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/currency_dollar_green.ico width=20px height=16px alt=Image />&nbsp;<img src=/Icons/symbol_check.ico width=20px height=16px alt=Image /></td></tr></table>";
                                }
                                else
                                {
                                    e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/symbol_check.ico width=20px height=16px alt=Image /></td></tr></table>";

                                }
                            }
                            else
                            {
                                e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/symbol_check.ico width=20px height=16px alt=Image /></td></tr></table>";

                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(e.Appointment.Attributes["ValidPaymentCaseId"].ToString()) > 0)
                            {
                                e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</td></tr></table>";
                            }
                            else
                            {
                                e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/symbol_stop.ico width=20px height=16px alt=Image /></td></tr></table>";

                            }

                            //e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' height='100%'><td><img src=/Icons/currency_dollar_green.ico width=20px height=16px alt=Image /><img src=/Icons/symbol_check.ico width=20px height=16px alt=Image />&nbsp;<span>" + e.Appointment.Subject + "</span></td></tr></table>";
                            //e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/symbol_stop.ico width=20px height=16px alt=Image /></td></tr></table>";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(e.Appointment.Attributes["ValidPaymentCaseId"].ToString()) > 0)
                        {
                            e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</td></tr></table>";
                        }
                        else
                        {
                            e.Appointment.Subject = "<table width='100%' height='100%'><tr width='100%' ><td><span>" + e.Appointment.Subject + "</span>&nbsp;<img src=/Icons/symbol_stop.ico width=20px height=16px alt=Image /></td></tr></table>";

                        }
                    }
                }
                e.Appointment.ContextMenuID = "SchedulerAppointmentContextMenu";

            }
            else
            {
                e.Appointment.ContextMenuID = "BreakContextMenu";
                e.Appointment.ForeColor = System.Drawing.Color.White;
                e.Appointment.Font.Bold = true;

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlFilter_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            RadLstDoctor.Text = "";
            CheckBox checkbox = new CheckBox();


            Hashtable hshInput = new Hashtable();
            DataSet dsFilter = new DataSet();

            hshInput.Add("@intID", hdnFilterID.Value);
            hshInput.Add("@inyHospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
            dsFilter = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetAppointmentFilterDetails", hshInput);
            if (dsFilter.Tables[0].Rows.Count > 0)
            {
                ddlFacility.SelectedValue = dsFilter.Tables[0].Rows[0]["FacilityID"].ToString();
                ViewState["CurrentView"] = dsFilter.Tables[0].Rows[0]["DateOption"].ToString();
                ddlSpecilization.SelectedValue = dsFilter.Tables[0].Rows[0]["SpecializationId"].ToString();
                string strProviders = dsFilter.Tables[0].Rows[0]["ProviderID"].ToString();
                ddlSpecilization_SelectedIndexChanged(this, null);
                if (ddlFacility.SelectedValue == "0")
                {
                    RadLstDoctor.SelectedValue = dsFilter.Tables[0].Rows[0]["ProviderID"].ToString();
                }
                else
                {
                    foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                    {
                        if (strProviders.Contains(currentItem.Value) == true)
                        {
                            currentItem.Checked = true;
                        }
                        else
                        {
                            currentItem.Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
    }
    protected void btnFindNext_Click(object sender, EventArgs e)
    {

        RadWindowForNew.NavigateUrl = "~/Appointment/ViewNextTimeSlotV1.aspx?MPG=P22220";
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 950;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnViewProviderTimings_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/Appointment/ViewProviderTimingsV1.aspx?MPG=P22221";
        RadWindowForNew.Height = 555;
        RadWindowForNew.Width = 950;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void RadLstDoctor_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        btnRefresh_OnClick(null, null);

    }
    protected void ddlSpecilization_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
          

            //ddlSpecilization.SelectedValue = "0";
            //if (common.myInt(ddlSpecilization.SelectedValue) == 0)
            //{
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "Please Select Specilization !";               
            //    ddlSpecilization.Focus();
            //    return;
            //}
            Hashtable HashIn = new Hashtable();
            if (chkShowAllProviders.Checked == true)
            {
                HashIn.Add("@intSpecialisationId", Convert.ToInt32(0));
            }
            else
            {
                HashIn.Add("@intSpecialisationId", common.myInt(ddlSpecilization.SelectedValue));
                //start added by om
                lblSpecilization.Visible = false;
                ddlSpecilization.Visible = true;
                ddlSpecilization.Enabled = true;
                //ddlSpecilization.SelectedIndex = 0;
                RadLstDoctor.Items.Clear();
                RadLstDoctor.Text = string.Empty;
                //end
            }
            HashIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityId", common.myInt(ddlFacility.SelectedValue));
            HashIn.Add("@AppointmentDate", dtpDate.SelectedDate.ToString("yyyy-MM-dd"));
            DataSet dt = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorListForAppointment", HashIn);

            if (dt.Tables[0].Rows.Count > 0)
            {
                ViewState["doctorlist"] = dt;
                if (chkShowAllProviders.Checked == true)
                {
                    //RadLstDoctor.CheckBoxes = true;
                    RadLstDoctor.DataSource = dt.Tables[0];

                    RadLstDoctor.EnableCheckAllItemsCheckBox = false;
                    RadLstDoctor.Filter = RadComboBoxFilter.Contains;
                    RadLstDoctor.CheckBoxes = false; 
                    ddlSpecilization.Enabled = false;
                 

                }
                else
                {
                    if (common.myInt(ddlSpecilization.SelectedValue) > 0)
                    {
                        RadLstDoctor.DataSource = dt.Tables[0];
                        RadLstDoctor.CheckBoxes = true;
                        RadLstDoctor.EnableCheckAllItemsCheckBox = true;
                        RadLstDoctor.Filter = RadComboBoxFilter.None;

                        ddlSpecilization.Enabled = true;
                    }
                    // btnRefresh_OnClick(sender, e);
                }

                RadLstDoctor.DataTextField = "DoctorName";
                RadLstDoctor.DataValueField = "DoctorId";
                RadLstDoctor.DataBind();
                if (chkShowAllProviders.Checked == true)
                {
                    RadLstDoctor.Items.Insert(0, new RadComboBoxItem("", "0")); 
                }
                    if (Session["FollowUpDoctorId"] != null)
                    RadLstDoctor.SelectedValue = common.myStr(Session["DoctorId"]);
                else
                    RadLstDoctor.SelectedIndex = 0;
                CheckBox checkbox = new CheckBox();
                if (ddlFacility.SelectedValue == "0")
                {
                    RadLstDoctor.CheckBoxes = false;
                    RadLstDoctor.Text = "Select Doctor";
                }
                else
                {
                    //if (Session["ProviderId"] != null)
                    if (Session["ProviderId"] == null || common.myInt(Session["ProviderId"]) == 0)
                    {
                        foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                        {

                            if (Session["FollowUpDoctorId"] != null)
                            {
                                if (currentItem.Value == Session["FollowUpDoctorId"].ToString())
                                {
                                    currentItem.Checked = true;
                                    RadLstDoctor.SelectedValue = Session["FollowUpDoctorId"].ToString();
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Request.QueryString["ProviderId"]) && common.myStr(Request.QueryString["ProviderId"]) != "0" && !string.IsNullOrEmpty(Request.QueryString["SpecializationId"]) && common.myStr(Request.QueryString["SpecializationId"]) != "0")
                                {
                                    if (currentItem.Value == common.myStr(Request.QueryString["ProviderId"]))
                                    {
                                        currentItem.Checked = true;
                                        RadLstDoctor.SelectedValue = common.myStr(Request.QueryString["ProviderId"]);
                                    }

                                }
                                else
                                {
                                    currentItem.Checked = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                        {

                            if (common.myInt(currentItem.Value) == common.myInt(Session["ProviderId"]))
                            {
                                currentItem.Checked = true;
                                //Session["ProviderId"] = null;
                            }
                        }
                        //foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                        //{
                        //    if (Session["FollowUpDoctorId"] != null)
                        //    {
                        //        if (currentItem.Value == Session["FollowUpDoctorId"].ToString())
                        //        {
                        //            currentItem.Checked = true;
                        //            RadLstDoctor.SelectedValue = Session["FollowUpDoctorId"].ToString();
                        //        }
                        //    }
                        //    else
                        //    {
                        //        currentItem.Checked = true;
                        //    }
                        //}
                    }
                }
                RadLstDoctor_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
    }
    public int speicalizaionID(int DoctorID)
    {
        int SpecalizationId = 0;
        DataSet ds = (DataSet)ViewState["doctorlist"];
        DataView dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "DoctorID=" + DoctorID;
        if (dv.ToTable().Rows.Count > 0)
        {
            SpecalizationId = common.myInt(dv.ToTable().Rows[0]["SpecialisationId"]);
        }
        else
        {
            SpecalizationId = 0;
        }
        return SpecalizationId;
    }
    public void ShowSpeicalizaionName(int DoctorID)
    {
        DataSet ds = (DataSet)ViewState["doctorlist"];
        DataView dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "DoctorID=" + DoctorID;
        if (dv.ToTable().Rows.Count > 0)
        {
            lblSpecilization.Text = common.myStr(dv.ToTable().Rows[0]["Specialisation"]);
            lblSpecilization.Visible = true;
        }
        else
        {
            lblSpecilization.Visible = false;

        }
    }
    protected void ddlFacility_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            BindSpeciliazation();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDay_Click(Object sender, EventArgs e)
    {

        ViewState["CurrentView"] = "D";
        chkOptionView.Visible = false;
        RadScheduler1.DayView.ShowDateHeaders = false;

        // btnRefresh_OnClick(sender, e);
    }
    protected void btnWeek_Click(Object sender, EventArgs e)
    {
        RadScheduler1.WeekView.HeaderDateFormat = Session["OutputDateFormat"].ToString();
        ViewState["CurrentView"] = "W";
        chkOptionView.Visible = true;
        //   btnRefresh_OnClick(sender, e);
    }
    protected void btnMonth_Click(Object sender, EventArgs e)
    {
        ViewState["CurrentView"] = "M";
        chkOptionView.Visible = true;
        OnSelectedDateChanged_dtpDate(sender, e);
        // btnRefresh_OnClick(sender, e);
    }
    protected void btnPrevious_OnClick(Object sender, EventArgs e)
    {

        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        dtpDate.SelectedDate = dtpDate.SelectedDate.AddDays(-1);
        ViewState["SELECTED_DATE"] = dtpDate.SelectedDate;
        dtpDate.SelectedDayStyle.BackColor = System.Drawing.Color.Red;
        dtpDate.SelectedDayStyle.ForeColor = System.Drawing.Color.White;
        dtpDate.SelectedDayStyle.Font.Bold = true;
        OnSelectedDateChanged_dtpDate(sender, e);
        // btnRefresh_OnClick(sender, e);
    }
    protected void btnNext_OnClick(Object sender, EventArgs e)
    {
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        dtpDate.SelectedDate = dtpDate.SelectedDate.AddDays(1);
        ViewState["SELECTED_DATE"] = dtpDate.SelectedDate;
        dtpDate.SelectedDayStyle.BackColor = System.Drawing.Color.Red;
        dtpDate.SelectedDayStyle.ForeColor = System.Drawing.Color.White;
        dtpDate.SelectedDayStyle.Font.Bold = true;
        OnSelectedDateChanged_dtpDate(sender, e);
        //   btnRefresh_OnClick(sender, e);
    }
    protected void btnRefresh_OnClick(Object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Hospital hospital = new BaseC.Hospital(sConString);
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ProviderId"]) && !string.IsNullOrEmpty(Request.QueryString["SpecializationId"]))
            {
                ddlSpecilization.SelectedValue = common.myStr(Request.QueryString["SpecializationId"]);
                ddlSpecilization_SelectedIndexChanged(this, null);
            }
            dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
            DataSet dsHoliday = hospital.GetHospitalHoliday(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToDateTime(dtpDate.SelectedDate).ToString("dd/MM/yyyy"));
            if (dsHoliday.Tables[0].Rows.Count > 0)
            {
                ViewState["Holiday"] = true;
                Alert.ShowAjaxMsg(dsHoliday.Tables[0].Rows[0]["Description"].ToString(), Page);
            }
            else
            {
                ViewState["Holiday"] = false;
            }

            bindAppointmentSummary();

            lblMessage.Text = "";
            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();

            String strProvider = "";

            // if (common.myInt(Session["ProviderId"]) == 0)
            if (common.myInt(Session["ProviderId"]) == 0 || common.myInt(Session["ProviderId"]) != 0 || Session["ProviderId"] == null)
            {

                if (ddlFacility.SelectedValue == "0")
                {
                    if (ddlFacility.SelectedIndex == -1)
                    {
                        lablecraete.Visible = true;
                        btnlink.Visible = false;
                        RadScheduler1.DataSource = null;
                        RadScheduler1.DataBind();
                        return;
                    }
                    else
                    {
                        strProvider = "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";// +"<Table1><c1>" + value + "</c1></Table1>";
                    }
                }
                else
                {
                    String value = String.Empty;
                    CheckBox checkbox = new CheckBox();

                    if (chkShowAllProviders.Checked == false)
                    {

                        foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                        {
                            if (currentItem.Checked == true)
                            {
                                strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                            }
                        }
                    }
                    else
                    {
                        // ddlSpecilization.Enabled = false;
                        if (common.myInt(RadLstDoctor.SelectedValue) > 0)
                            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

                        if (common.myInt(ddlSpecilization.SelectedValue) == 0)
                        {
                            if (common.myInt(speicalizaionID(0)) != 0)
                                lblSpecilization.Text = ddlSpecilization.FindItemByValue(speicalizaionID(common.myInt(RadLstDoctor.SelectedValue)).ToString()).Text.ToString();
                        }

                    }
                    if (strProvider == "")
                    {
                        lablecraete.Visible = true;
                        btnlink.Visible = false;
                        RadScheduler1.DataSource = null;
                        RadScheduler1.DataBind();

                        // return;
                    }
                    else
                    {
                        lablecraete.Visible = false;
                        btnlink.Visible = false;
                    }
                }

                if (chkShowAllProviders.Checked == true)
                {
                    ddlSpecilization.Visible = false;
                    lblSpecilization.Visible = true;

                }
                else
                {
                    ddlSpecilization.Visible = true;
                    lblSpecilization.Visible = false;
                    // btnRefresh_OnClick(sender, e);
                }
                if (common.myInt(ddlSpecilization.SelectedValue).Equals(0) && !chkShowAllProviders.Checked)
                    return;
                Session["ProviderId"] = null;
            }
            DataSet dsLeave = hospital.GetHospitalEmployeeLeave(strProvider, Convert.ToDateTime(dtpDate.SelectedDate).Date.ToString("yyyy/MM/dd"));
            if (dsLeave.Tables[0].Rows.Count > 0)
            {
                ViewState["Leave"] = true;
                for (int i = 0; i < dsLeave.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        ViewState["LeaveEmployeeId"] = dsLeave.Tables[0].Rows[i]["EmployeeId"].ToString();
                    }
                    else
                    {
                        ViewState["LeaveEmployeeId"] = ViewState["LeaveEmployeeId"] + "," + dsLeave.Tables[0].Rows[i]["EmployeeId"].ToString();
                    }
                }
            }
            else
            {
                ViewState["Leave"] = false;
                ViewState["LeaveEmployeeId"] = null;
            }
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            if (ddlFacility.SelectedValue == "0")
            {
                HashIn.Add("@intFacilityId", "0");

            }
            else
            {
                HashIn.Add("@intFacilityId", ddlFacility.SelectedValue);
            }
            HashIn.Add("@xmlDoctorIds", strProvider);
            HashIn.Add("@chrForDate", dtpDate.SelectedDate.ToString("yyyy/MM/dd"));
            HashIn.Add("@chrDateOption", ViewState["CurrentView"] == null ? "D" : ViewState["CurrentView"]);
            if (common.myInt(ddlSpecilization.SelectedValue).Equals(0) && common.myStr(strProvider).Equals(""))
            {
                return;
            }
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointments", HashIn);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[2].Rows.Count > 0)
                {
                    RadScheduler1.SelectedDate = dtpDate.SelectedDate;

                    RadScheduler1.WorkDayStartTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                    RadScheduler1.WorkDayEndTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString());

                    RadScheduler1.DayStartTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                    RadScheduler1.DayEndTime = TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString());
                    int timesnap = 30;
                    if (int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) <= 15)
                    {
                        timesnap = 15;
                    }
                    else if ((int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) > 15) && (int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) <= 45))
                    {
                        timesnap = int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString());
                    }

                    else if ((int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) > 30) && (int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) <= 45))
                    {

                        timesnap = 45;

                    }
                    else if ((int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) > 45) && (int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()) <= 60))
                    {
                        timesnap = 60;
                    }

                    else
                    {
                        Alert.ShowAjaxMsg("Invlid DAteRange Select", this);
                        return;
                    }
                    RadScheduler1.MinutesPerRow = timesnap;
                    RadScheduler1.TimeLabelRowSpan = timesnap / int.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString());

                    RadScheduler1.DataKeyField = "AppointmentId";
                    RadScheduler1.DataStartField = "FromTime";
                    RadScheduler1.DataEndField = "ToTime";
                    RadScheduler1.DataSubjectField = "Subject";
                    RadScheduler1.GroupBy = "Resource";
                    RadScheduler1.DataRecurrenceField = "RecurrenceRule";
                    RadScheduler1.DataRecurrenceParentKeyField = "RecurrenceParentId";
                    RadScheduler1.ResourceTypes.Clear();
                    Session["printds"] = ds.Tables[0];

                    ResourceType rt = new ResourceType("Resource");

                    rt.DataSource = ds.Tables[2];

                    ViewState["IsDoctor"] = ds.Tables[2];
                    if (ViewState["Holiday"] != null && Convert.ToBoolean(ViewState["Holiday"]) == true)
                    {
                        ViewState["Disable"] = null;
                    }
                    else
                    {
                        ViewState["Disable"] = ds.Tables[3];
                    }
                    if (ViewState["Leave"] != null && Convert.ToBoolean(ViewState["Leave"]) == true)
                    {
                        if (ViewState["Disable"] != null)
                        {
                            if (ViewState["LeaveEmployeeId"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["Disable"];
                                DataView dv = new DataView(dt);
                                dv.RowFilter = " ResourceId NOT IN (" + ViewState["LeaveEmployeeId"].ToString() + ")";
                                DataTable dt1 = dv.ToTable();
                                DataSet dsL = new DataSet();
                                dsL.Tables.Add(dt1);
                                ViewState["Disable"] = dsL.Tables[0];
                            }
                            else
                            {

                                ViewState["Disable"] = null;
                            }
                        }
                    }
                    else
                    {
                        ViewState["Disable"] = ds.Tables[3];
                    }

                    ViewState["Lunch"] = ds.Tables[1];
                    rt.KeyField = "ResourceId";
                    if (ddlFacility.SelectedValue == "0")
                    {
                        rt.ForeignKeyField = "FacilityId";
                    }
                    else
                    {
                        rt.ForeignKeyField = "DoctorId";
                    }
                    rt.TextField = "ResourceName";
                    RadScheduler1.ResourceTypes.Add(rt);
                    Session["Printapp"] = ds.Tables[1];
                    RadScheduler1.DataSource = ds.Tables[1];
                    RadScheduler1.DataBind();
                    // santosh
                    DataView dv1 = new DataView(ds.Tables[1]);
                    DataTable dtAppoint = new DataTable();
                    dv1.RowFilter = "StatusId<>96";
                    dtAppoint = dv1.ToTable();
                    if (dtAppoint.Rows.Count > 0)
                    {
                        lblAppoint.Text = common.myStr(dtAppoint.Rows.Count);
                    }
                    else
                    {
                        lblAppoint.Text = "0";
                    }
                    dv1 = new DataView(ds.Tables[1]);
                    dv1.RowFilter = "StatusId=1 ";
                    dtAppoint = new DataTable();
                    dtAppoint = dv1.ToTable();
                    if (dtAppoint.Rows.Count > 0)
                    {
                        lblNewPatient.Text = common.myStr(dtAppoint.Rows.Count);
                    }
                    else
                    {
                        lblNewPatient.Text = "0";
                    }
                    dv1 = new DataView(ds.Tables[1]);
                    dv1.RowFilter = "StatusId=" + 2;
                    dtAppoint = new DataTable();
                    dtAppoint = dv1.ToTable();
                    if (dtAppoint.Rows.Count > 0)
                    {
                        lblConfirmed.Text = common.myStr(dtAppoint.Rows.Count);
                    }
                    else
                    {
                        lblConfirmed.Text = "0";
                    }
                    dv1 = new DataView(ds.Tables[1]);
                    dv1.RowFilter = "StatusId=" + 3;
                    dtAppoint = new DataTable();
                    dtAppoint = dv1.ToTable();
                    if (dtAppoint.Rows.Count > 0)
                    {
                        lblCheckIn.Text = common.myStr(dtAppoint.Rows.Count);
                    }
                    else
                    {
                        lblCheckIn.Text = "0";
                    }
                    //edited by kabir on 30_10_2015
                    if (ds.Tables.Count == 6)
                    {
                        dv1 = new DataView(ds.Tables[5]);
                    }
                    else if (ds.Tables.Count == 5)
                    {
                        dv1 = new DataView(ds.Tables[4]);
                    }
                    dtAppoint = new DataTable();
                    dtAppoint = dv1.ToTable();
                    if (dtAppoint.Rows.Count > 0)
                    {
                        lblCancelled.Text = common.myStr(dtAppoint.Rows.Count);
                    }
                    else
                    {
                        lblCancelled.Text = "0";
                    }

                    dv1 = new DataView(ds.Tables[1]);
                    dv1.RowFilter = "StatusId=" + 417;
                    dtAppoint = new DataTable();
                    dtAppoint = dv1.ToTable();
                    if (dtAppoint.Rows.Count > 0)
                    {
                        lblPayment.Text = common.myStr(dtAppoint.Rows.Count);
                    }
                    else
                    {
                        lblPayment.Text = "0";
                    }

                }
                else
                {
                    Alert.ShowAjaxMsg("There is no facility define for this doctor", Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("There is no time define in selected Doctor and facility.", Page);
            }
            if (chkShowAllProviders.Checked == true)
            {
                ddlSpecilization.Visible = false;
                lblSpecilization.Visible = true;

            }
            else
            {
                ddlSpecilization.Visible = true;
                lblSpecilization.Visible = false;
                // btnRefresh_OnClick(sender, e);
            }
            if (!ddlSpecilization.Visible)
            {
                ShowSpeicalizaionName(common.myInt(RadLstDoctor.SelectedValue));
            }
            //// =============== Star Set Current Start hour and End Hour.   santosh ==================================
            int Shour = DateTime.Now.Hour;
            //int Ehour = DateTime.Now.AddHours(6).Hour;
            //  RadScheduler1.DayStartTime = new TimeSpan(Shour, 0, 0);
            //RadScheduler1.DayEndTime = new TimeSpan(Ehour, 0, 0);         

            //// =============== END Set Current Start hour and End Hour ==================================


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            hospital = null;
            dl = null;
        }
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Appointment/AppSchedulerV1.aspx?MPG=P22080", false);
    }
    protected void RadLstDoctor_ItemDataBound(object o, RadComboBoxItemEventArgs e)
    {
        if (common.myStr(ViewState["FromFilter"]) == "0")
        {
            e.Item.Checked = true;
        }
    }
    private void PopulateDoctor()
    {

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            Hashtable HashIn = new Hashtable();
            HashIn.Add("@HospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intFacilityId", ddlFacility.SelectedValue);

            DataSet dt = dl.FillDataSet(CommandType.StoredProcedure, "uspgetdoctorlist", HashIn);

            RadLstDoctor.DataSource = dt.Tables[0];
            RadLstDoctor.DataTextField = "DoctorName";
            RadLstDoctor.DataValueField = "DoctorId";
            RadLstDoctor.DataBind();

            ViewState["FromFilter"] = 0;
            if (ddlFacility.SelectedValue == "0")
            {
                foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                {
                    currentItem.Visible = false;
                }
                if (RadLstDoctor.Items.Count > 0)
                {
                    RadLstDoctor.SelectedIndex = 0;
                }
            }
            else
            {
                foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                {
                    currentItem.Checked = true;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
        }
    }
    void BindControl()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Patient objBc = new BaseC.Patient(sConString);
        try
        {
            ////populate Nationality drop down control
            //if (Cache["NationalityTable"] == null)
            //{


            //    DataSet objDs = objBc.GetPatientNationality();
            //    Cache.Insert("NationalityTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            //    ddlNationality.DataSource = Cache["NationalityTable"];
            //    ddlNationality.DataTextField = "name";
            //    ddlNationality.DataValueField = "ID";
            //    ddlNationality.DataBind();
            //    RadComboBoxItem rci = new RadComboBoxItem("Select");
            //    ddlNationality.Items.Insert(0, rci);
            //}
            //else
            //{
            //    DataSet objDs = Cache["NationalityTable"];
            //    ddlNationality.DataSource = objDs;
            //    ddlNationality.DataTextField = "name";
            //    ddlNationality.DataValueField = "ID";
            //    ddlNationality.DataBind();
            //    RadComboBoxItem rci = new RadComboBoxItem("Select");
            //    ddlNationality.Items.Insert(0, rci);
            //}

            Hashtable HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intUserId", 0);
            HashIn.Add("@intGroupId", 0);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspGetFacilityList", HashIn);

            ddlFacility.DataSource = dr;
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();
            ddlFacility.SelectedValue = Session["FacilityId"].ToString();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("All Facilites", "0"));
            ddlFacility.Items[0].Value = "0";

            //ddlLanguage.Items.Clear();
            //DataSet ds = new DataSet();
            //ds = objBc.GetLanguage();
            //ddlLanguage.DataSource = ds;
            //ddlLanguage.DataValueField = "LanguageID";
            //ddlLanguage.DataTextField = "Language";
            //ddlLanguage.DataBind();


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
        }
    }
    protected void RadScheduler1_AppointmentDelete(object sender, SchedulerCancelEventArgs e)
    {

    }
    private string FindCurrentDate(string outputCurrentDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string firstCurrentDate = "";
        string newCurrentDate = "";
        string currentdate = formatdate.FormatDateDateMonthYear(System.DateTime.Today.ToShortDateString());
        string strformatCurrDate = formatdate.FormatDate(currentdate, "dd/MM/yyyy", "yyyy/MM/dd");
        firstCurrentDate = strformatCurrDate.Remove(4, 1);
        newCurrentDate = firstCurrentDate.Remove(6, 1);
        return newCurrentDate;
    }
    private string FindFutureDate(string outputFutureDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string strDate = Convert.ToDateTime(outputFutureDate).Date.ToShortDateString();
        string firstApptDate = "";
        string NewApptDate = "";
        string strDateAppointment = formatdate.FormatDateDateMonthYear(strDate);
        string strformatApptDate = formatdate.FormatDate(strDateAppointment, "dd/MM/yyyy", "yyyy/MM/dd");
        firstApptDate = strformatApptDate.Remove(4, 1);
        NewApptDate = firstApptDate.Remove(6, 1);
        return NewApptDate;
    }
    protected void RadScheduler1_TimeSlotContextMenuItemClicked(object sender, TimeSlotContextMenuItemClickedEventArgs e)
    {
        try
        {
            if (RadScheduler1.Resources.Count != 0)
            {
                if (ViewState["Lunch"] != null)
                {
                    DataView dv = new DataView((DataTable)ViewState["Lunch"]);
                    //dv.RowFilter = " DoctorId=" + e.TimeSlot.Resource.Key + " and Type='Block' OR Type='Break'";
                    dv.RowFilter = " DoctorId=" + e.TimeSlot.Resource.Key + " and Type='Block'";
                    DataTable dtlunch = dv.ToTable();
                    for (int i = 0; i < dtlunch.Rows.Count; i++)
                    {
                        if (e.TimeSlot.Start >= Convert.ToDateTime(dtlunch.Rows[i]["FromTime"])
                            && e.TimeSlot.End <= Convert.ToDateTime(dtlunch.Rows[i]["ToTime"]))
                        {
                            //Alert.ShowAjaxMsg("No Activity allow at the Break or Block time slot", Page);
                            Alert.ShowAjaxMsg("No Activity allow at the Block time slot", Page);
                            return;
                        }
                    }
                }

                bool IsDoctor = false;
                bool IsNoAppointmentAllowBeyondTime = false;
                BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                string fromtime = "'1900/01/01 " + e.TimeSlot.Start.Hour.ToString() + ":" + e.TimeSlot.Start.Minute + "'";
                string totime = "'1900/01/01 " + e.TimeSlot.End.Hour.ToString() + ":" + e.TimeSlot.End.Minute + "'";
                string resounceId = e.TimeSlot.Resource.Key.ToString();


                if (ViewState["IsDoctor"] != null)
                {
                    DataTable dt = (DataTable)ViewState["IsDoctor"];
                    DataView dvDoctor = new DataView(dt);
                    dvDoctor.RowFilter = "ResourceID='" + e.TimeSlot.Resource.Key + "'";
                    DataTable dtDoctor = dvDoctor.ToTable();
                    if (dtDoctor.Rows.Count > 0)
                    {
                        IsDoctor = Convert.ToBoolean(dtDoctor.Rows[0]["IsDoctor"]);
                        if (common.myStr(dtDoctor.Rows[0]["IsNoAppointmentAllowBeyondTime"]) == "True" || common.myStr(dtDoctor.Rows[0]["IsNoAppointmentAllowBeyondTime"]) == "1")
                            IsNoAppointmentAllowBeyondTime = true;
                    }
                }

                if (e.MenuItem.Value == "New")
                {

                    string sBreakId = appoint.ExistBreakAndBlock(0, Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt32(resounceId), Convert.ToDateTime(dtpDate.SelectedDate).ToString("yyyy/MM/dd"), (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()), (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));
                    if (sBreakId != "" && sBreakId != null)
                    {
                        Alert.ShowAjaxMsg("Appointment not Allow in this time.", Page);
                        return;
                    }
                    //BaseC.HospitalSetup objHp = new BaseC.HospitalSetup(sConString);
                    //string sHr = "-" + objHp.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "AppointmentGracePeriod");
                    int DateCheck = DateTime.Compare(e.TimeSlot.Start.Date, DateTime.Now.Date);
                    DateTime dtGrace = DateTime.Now; //.AddHours(common.myDbl(sHr));
                    TimeSpan CurrentTime = TimeSpan.Parse(dtGrace.Hour + ":" + DateTime.Now.Minute);
                    TimeSpan SlotTime = TimeSpan.Parse(e.TimeSlot.Start.Hour + ":" + e.TimeSlot.Start.Minute);
                    int comp = TimeSpan.Compare(CurrentTime, SlotTime);
                    if (DateCheck < 0)//
                    {

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Back date & time appointment not allowed !";
                        Alert.ShowAjaxMsg("Back date & time appointment not allowed  !", Page);
                        return;

                    }
                    else if ((comp > 0) && (DateCheck == 0))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Back date & time appointment not allowed !";
                        Alert.ShowAjaxMsg("Back date & time appointment not allowed !", Page);
                        return;
                    }

                    dtTemp = new DataTable();
                    if (ViewState["Disable"] != null)
                    {
                        dtTemp = (DataTable)ViewState["Disable"];
                    }
                    if (dtTemp != null)
                    {
                        if (dtTemp.Rows.Count > 0)
                        {
                            DataRow[] dr;
                            string expr = "";
                            if (Convert.ToString(ViewState["CurrentView"]) == "W")
                            {
                                string sday = e.TimeSlot.Start.DayOfWeek.ToString();
                                switch (sday)
                                {
                                    case "Sunday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Sun' ";
                                            break;
                                        }
                                    case "Monday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Mon' ";
                                            break;
                                        }
                                    case "Tuesday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Tue' ";
                                            break;
                                        }
                                    case "Wednesday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Wed' ";
                                            break;
                                        }
                                    case "Thursday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Thu' ";
                                            break;
                                        }
                                    case "Friday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Fri' ";
                                            break;
                                        }
                                    case "Saturday":
                                        {
                                            expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + ") and Day='Sat' ";
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                expr = "ResourceId = " + resounceId + " and (" + fromtime + " >= ShiftFirstFrom And ShiftFirstTo >= " + totime + " ) ";
                            }
                            dr = dtTemp.Select(expr);

                            if (dr.Length == 0)
                            {
                                if (IsNoAppointmentAllowBeyondTime == false)
                                { divNoSLot.Visible = true; }
                                else
                                {
                                    Alert.ShowAjaxMsg("Appointment Not Allowed Beyond Slots For This Doctor !!!.", Page.Page);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (IsNoAppointmentAllowBeyondTime == false)
                            { divNoSLot.Visible = true; }
                            else
                            {
                                Alert.ShowAjaxMsg("Appointment Not Allowed Beyond Slots For This Doctor !!!.", Page.Page);
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                        //divNoSLot.Visible = true;
                    }


                    string navstr = "";
                    if (ddlFacility.SelectedValue == "0")
                    {
                        navstr = "~/Appointment/AppointmentDetails.aspx?MPG=P22222&FacilityId=" + e.TimeSlot.Resource.Key +
                            "&StTime=" + e.TimeSlot.Start.Hour.ToString() + "&EndTime=23&appDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") + "&appid=0&doctorId=" + RadLstDoctor.SelectedValue + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() + "&PageId=" + Request.QueryString["Mpg"] + "&IsDoctor=" + IsDoctor;
                        ViewState["FacilityId"] = e.TimeSlot.Resource.Key;
                    }
                    else
                    {
                        navstr = "~/Appointment/AppointmentDetails.aspx?MPG=P22222&FacilityId=" + ddlFacility.SelectedValue
                            + "&StTime=" + e.TimeSlot.Start.Hour.ToString()
                            + "&EndTime=23"// + RadScheduler1.DayEndTime.Hours.ToString()
                            + "&appDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy")
                            + "&appid=0&doctorId=" + e.TimeSlot.Resource.Key
                            + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString()
                            + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString()
                            + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString()
                            + "&PageId=" + Request.QueryString["Mpg"]
                            + "&IsDoctor=" + IsDoctor;
                        ViewState["FacilityId"] = ddlFacility.SelectedValue;
                    }
                    ViewState["StTime"] = e.TimeSlot.Start.Hour.ToString();
                    ViewState["EndTime"] = RadScheduler1.DayEndTime.Hours.ToString();
                    ViewState["appDate"] = e.TimeSlot.Start.ToString("MM/dd/yyyy");
                    ViewState["doctorId"] = ddlFacility.SelectedValue == "0" ? RadLstDoctor.SelectedValue : e.TimeSlot.Resource.Key;
                    ViewState["TimeInterval"] = RadScheduler1.MinutesPerRow.ToString();
                    ViewState["FromTimeHour"] = e.TimeSlot.Start.Hour.ToString();
                    ViewState["FromTimeMin"] = e.TimeSlot.Start.Minute.ToString();

                    ViewState["ToTimeHour"] = e.TimeSlot.End.Hour.ToString();
                    ViewState["ToTimeMin"] = e.TimeSlot.End.Minute.ToString();

                    ViewState["PageId"] = Request.QueryString["Mpg"];
                    ViewState["Mpg"] = Request.QueryString["Mpg"];
                    ViewState["IsDoctorPara"] = IsDoctor;

                    if (divNoSLot.Visible == false)
                    {

                        RadWindowForNew.NavigateUrl = navstr;
                        RadWindowForNew.Height = 670;
                        RadWindowForNew.Width = 940;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                }
                else if (e.MenuItem.Value == "Paste")
                {
                    int DateCheck = DateTime.Compare(e.TimeSlot.Start.Date, DateTime.Now.Date);
                    DateTime dtGrace = DateTime.Now; //.AddHours(common.myDbl(sHr));

                    TimeSpan CurrentTime = TimeSpan.Parse(dtGrace.Hour + ":" + DateTime.Now.Minute);
                    TimeSpan SlotTime = TimeSpan.Parse(e.TimeSlot.Start.Hour + ":" + e.TimeSlot.Start.Minute);
                    int comp = TimeSpan.Compare(CurrentTime, SlotTime);

                    string sBreakId = appoint.ExistBreakAndBlock(0, common.myInt(ddlFacility.SelectedValue), common.myInt(resounceId),
                                                Convert.ToDateTime(dtpDate.SelectedDate).ToString("yyyy/MM/dd"),
                                                (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()),
                                                (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));


                    if (sBreakId != "" && sBreakId != null)
                    {
                        Alert.ShowAjaxMsg("Appointment not Allow in this time.", Page);
                        return;
                    }
                    else if ((DateCheck < 0) || (SlotTime < CurrentTime))
                    {
                        if (Convert.ToDateTime(dtpDate.SelectedDate) > DateTime.Now)
                        {

                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Back date & time appointment not allowed !";
                            Alert.ShowAjaxMsg("Back date & time appointment not allowed !", Page);
                            return;
                        }

                    }
                    if (ViewState["CopyRegistrationId"] != null)
                    {
                        string sAppointmentId = appoint.ExistSameDoctorAppointmentForPatient(Convert.ToInt16(Session["HospitalLocationId"]), ViewState["CopyRegistrationId"] == null ? 0 : Convert.ToInt32(ViewState["CopyRegistrationId"]),
                        Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt16(resounceId), Convert.ToDateTime(dtpDate.SelectedDate).ToString("yyyy/MM/dd"),
                        (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()),
                                                                   (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));
                        if (sAppointmentId != "" && sAppointmentId != null)
                        {
                            Alert.ShowAjaxMsg("Appointment already exist to another doctor.", Page);
                            return;
                        }
                        sAppointmentId = appoint.ExistDoctorAppointmentForPatient(Convert.ToInt16(Session["HospitalLocationId"]), ViewState["CopyRegistrationId"] == null ? 0 : Convert.ToInt32(ViewState["CopyRegistrationId"]),
                        Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToDateTime(dtpDate.SelectedDate).ToString("yyyy/MM/dd"),
                        (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()),
                                                                   (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));
                        if (sAppointmentId != "" && sAppointmentId != null)
                        {
                            Alert.ShowAjaxMsg("Already Appointment for this patient in this time.", Page);
                            return;
                        }
                    }
                    else
                    {
                        if (ViewState["CopyAppId"] == null)
                        {
                            Alert.ShowAjaxMsg("Please Copy Appointment to paste", Page);
                            return;
                        }
                    }

                    if (sBreakId != "" && sBreakId != null)
                    {
                        return;
                    }
                    if ((DateTime.Compare(e.TimeSlot.Start.Date, DateTime.Now.Date) == 0)
                        & (e.TimeSlot.Start.Hour <= DateTime.Now.Hour)
                        & (e.TimeSlot.Start.Minute < DateTime.Now.Minute))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Back date & time appointment not allowed !";
                        Alert.ShowAjaxMsg("Back date & time appointment not allowed  !", Page);
                        return;
                    }
                    if (!string.IsNullOrEmpty(common.myStr(ViewState["CopyAppId"])))
                    {
                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                        Hashtable collVal = new Hashtable();
                        collVal.Add("@intDoctorId", e.TimeSlot.Resource.Key);
                        collVal.Add("@intFacilityId", ddlFacility.SelectedValue);
                        collVal.Add("@chrAppointmentDate", e.TimeSlot.Start.ToString("dd/MMM/yyyy"));

                        string strvalidation = "SELECT DoctorId FROM DoctorTime WHERE DoctorID = @intDoctorId AND FacilityId = @intFacilityId AND CONVERT(VARCHAR, CONVERT(SMALLDATETIME, @chrAppointmentDate, 103), 111) BETWEEN CONVERT(VARCHAR, DateFrom, 111) AND CONVERT(VARCHAR, ISNULL(DateTo, CONVERT(SMALLDATETIME, @chrAppointmentDate, 103)), 111) AND SUBSTRING(DATENAME(DW,CONVERT(SMALLDATETIME, @chrAppointmentDate, 103)),1,3) = DAY AND Active = 1";
                        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strvalidation, collVal);

                        if (dr.HasRows == false)
                        {
                            //Alert.ShowAjaxMsg("Provider not available during this period", Page);
                            //return;
                        }

                        //if is valid

                        Hashtable hshtableout = new Hashtable();
                        Hashtable hshtablein = new Hashtable();

                        hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                        hshtablein.Add("intCopyAppointmentID", Convert.ToInt64(ViewState["CopyAppId"]));
                        hshtablein.Add("chrAppFromTime", e.TimeSlot.Start.Hour.ToString() + ":" + e.TimeSlot.Start.Minute.ToString());
                        hshtablein.Add("chrAppointmentDate", e.TimeSlot.Start.ToString("dd/MM/yyyy"));
                        hshtablein.Add("intDoctorId", ddlFacility.SelectedValue == "0" ? RadLstDoctor.SelectedValue : e.TimeSlot.Resource.Key);
                        hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                        // hshtablein.Add("intFacilityID",Convert.ToInt32(ddlFacility.SelectedValue));
                        hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);

                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspCopyDoctorAppointment", hshtablein, hshtableout);
                        btnRefresh_OnClick(sender, e);
                    }
                    if (common.myStr(ViewState["CutAppId"]).Equals("1"))
                    {
                        ViewState["StartHour"] = common.myStr(e.TimeSlot.Start.Hour);
                        ViewState["StartMinute"] = common.myStr(e.TimeSlot.Start.Minute);
                        ViewState["ResourceKey"] = e.TimeSlot.Resource.Key;
                        ViewState["AppointmentDate"] = e.TimeSlot.Start.ToString("dd/MM/yyyy");

                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable collVal = new Hashtable();
                        collVal.Add("@intDoctorId", common.myStr(ViewState["ResourceKey"]));
                        collVal.Add("@intFacilityId", ddlFacility.SelectedValue);
                        collVal.Add("@chrAppointmentDate", common.myStr(ViewState["AppointmentDate"]));
                        //if is valid

                        Hashtable hshtableout = new Hashtable();
                        Hashtable hshtablein = new Hashtable();

                        hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                        hshtablein.Add("intCopyAppointmentID", common.myLong(ViewState["AppId"]));
                        hshtablein.Add("chrAppFromTime", common.myStr(ViewState["StartHour"]) + ":" + common.myStr(ViewState["StartMinute"]));
                        hshtablein.Add("chrAppointmentDate", common.myStr(ViewState["AppointmentDate"]));
                        hshtablein.Add("intDoctorId", ddlFacility.SelectedValue == "0" ? RadLstDoctor.SelectedValue : common.myStr(ViewState["ResourceKey"]));
                        hshtablein.Add("intEncodedBy", common.myInt(Session["UserID"]));
                        // hshtablein.Add("intFacilityID",common.myInt(ddlFacility.SelectedValue));
                        hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);

                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspCopyDoctorAppointment", hshtablein, hshtableout);


                        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        string[] strAppId = common.myStr(ViewState["AppId"]).Split('_');
                        Hashtable hshIn = new Hashtable();
                        if (common.myStr(ViewState["AppId"]).Contains('_'))
                        {
                            //SchedulerNavigationCommand.SwitchToSelectedDay
                            DateTime OrigRecTime = DateTime.Now;
                            string strOrigRecTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                            OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

                            DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                            string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

                            string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                            string OrigRecRule = common.myStr(dl.ExecuteScalar(CommandType.Text, origrule));
                            if (!OrigRecRule.Contains("EXDATE"))
                            {
                                string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                                string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                                dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            }
                            else
                            {
                                string UpdateRule = OrigRecRule + "," + exdate;
                                string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                                dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            }
                        }
                        else
                        {
                            hshIn = new Hashtable();
                            hshIn.Add("@intAppointmentId", strAppId[0]);
                            hshIn.Add("@intLastChangedBy", Session["UserId"]);
                            hshIn.Add("@chvLastChangedDate", DateTime.UtcNow);
                            hshIn.Add("@chvCutReason", "");
                            dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update DoctorAppointment Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate,Cancelreason=@chvCutReason where AppointmentId=@intAppointmentId", hshIn);
                        }
                        btnRefresh_OnClick(sender, e);

                    }
                }
                else if (e.MenuItem.Value == "Break")
                {
                    string sBreakId = appoint.ExistBreakAndBlock(0, Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt32(resounceId), Convert.ToDateTime(dtpDate.SelectedDate).ToString("yyyy/MM/dd"), (e.TimeSlot.Start.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Hour.ToString() : e.TimeSlot.Start.Hour.ToString()) + ":" + (e.TimeSlot.Start.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.Start.Minute.ToString() : e.TimeSlot.Start.Minute.ToString()),
                                                               (e.TimeSlot.End.Hour.ToString().Length == 1 ? "0" + e.TimeSlot.End.Hour.ToString() : e.TimeSlot.End.Hour.ToString()) + ":" + (e.TimeSlot.End.Minute.ToString().Length == 1 ? "0" + e.TimeSlot.End.Minute.ToString() : e.TimeSlot.End.Minute.ToString()));
                    if (sBreakId != "" && sBreakId != null)
                    {
                        Alert.ShowAjaxMsg("Already set Break and Block in this time.", Page);
                        return;
                    }
                    if (dtTemp != null)
                    {
                        RadWindowForNew.NavigateUrl = "~/Appointment/BreakAndBlock_New.aspx?MPG=P4&Category=PopUp&FacilityId=" + ddlFacility.SelectedValue + "&StTime=" + e.TimeSlot.Start.Hour.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&appDate=" + e.TimeSlot.Start.ToString("MM/dd/yyyy") + "&appid=0&DoctorId=" + e.TimeSlot.Resource.Key + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&FromTimeHour=" + e.TimeSlot.Start.Hour.ToString() + "&FromTimeMin=" + e.TimeSlot.Start.Minute.ToString() + "&PageId=" + Request.QueryString["Mpg"] + "&recrule=" + null + "&IsDoctor=" + IsDoctor;
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Width = 730;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;
                    }
                }
                else if (e.MenuItem.Value == "DTP")
                {
                    RadWindowForNew.NavigateUrl = "~/Appointment/DoctorProfileV1.aspx?MPG=P22223&resounceId=" + common.myInt(resounceId);
                    RadWindowForNew.Height = 400;
                    RadWindowForNew.Width = 730;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please select Doctor", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void CalApp_SelectionChanged(object sender, EventArgs e)
    {
        ////if (CalApp.SelectedDate.ToString("dd/MM/yyyy") == "01/01/0001")
        ////{
        ////    CalApp.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        ////}
        ////ViewState["SELECTED_DATE"] = CalApp.SelectedDate;


    }
    protected void OnSelectedDateChanged_dtpDate(object sender, EventArgs e)
    {
        if (dtpDate.SelectedDate.ToString("dd/MM/yyyy") == "01/01/0001")
        {
            dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        }
        dtpDate.TodayDayStyle.BackColor = System.Drawing.Color.White;
        dtpDate.TodayDayStyle.ForeColor = System.Drawing.Color.Black;
        dtpDate.TodayDayStyle.Font.Bold = false;

        dtpDate.SelectedDayStyle.BackColor = System.Drawing.Color.Red;
        dtpDate.SelectedDayStyle.ForeColor = System.Drawing.Color.White;
        dtpDate.SelectedDayStyle.Font.Bold = true;
        ViewState["SELECTED_DATE"] = dtpDate.SelectedDate;

        //  btnRefresh_OnClick(sender, e);

        int SpecilizationId = 0;
        string DoctorId = "0";
        if (chkShowAllProviders.Checked == true)
        {
            SpecilizationId = common.myInt(ddlSpecilization.SelectedValue);
            DoctorId = common.myStr(RadLstDoctor.SelectedValue);

        }
        else
        {
            DoctorId = "";
            SpecilizationId = common.myInt(ddlSpecilization.SelectedValue);

            foreach (RadComboBoxItem Item in RadLstDoctor.Items)
            {
                if (Item.Checked == true)
                {
                    if (Convert.ToString(DoctorId) == "")
                        DoctorId = Item.Value;
                    else
                        DoctorId += "," + Item.Value;

                }
            }

        }
        if (chkShowAllProviders.Checked == true)
        {
            ddlSpecilization.SelectedValue = common.myStr(SpecilizationId);
            RadLstDoctor.SelectedValue = common.myStr(DoctorId);
            // btnRefresh_OnClick(sender, e);  
        }
        else
        {
            string[] arrDoctorId = DoctorId.Split(',');

            foreach (RadComboBoxItem Item in RadLstDoctor.Items)
            {
                Item.Checked = false;
                for (int i = 0; i < arrDoctorId.Length; i++)
                {
                    if (common.myStr(Item.Value) == common.myStr(arrDoctorId[i]))
                    {
                        Item.Checked = true;
                        break;
                    }
                }
            }
        }


    }
    protected void RadScheduler1_AppointmentContextMenuItemClicked(object sender, AppointmentContextMenuItemClickedEventArgs e)
    {
        BaseC.Patient BC = new BaseC.Patient(sConString);
        BaseC.HospitalSetup objHp = new BaseC.HospitalSetup(sConString);
        try
        {
            if ((common.myStr(e.Appointment.Attributes["RegistrationId"]) == "") && ((e.MenuItem.Value == "P")))
            {
                Alert.ShowAjaxMsg("Please Register the patient first", Page);
                return;
            }

            string[] strAppId = e.Appointment.ID.ToString().Split('_');
            Boolean isBreak = false;
            // Boolean isCompanyPatient = false;
            DataTable dtLunchTable = (DataTable)ViewState["Lunch"];
            DataView dv = new DataView(dtLunchTable);
            dv.RowFilter = "AppointmentID=" + strAppId[0] + "";
            DataTable dtBreak = dv.ToTable();
            DataView dvLunch = new DataView(dtBreak);// AND Type='Break' OR Type='Block'";
            dvLunch.RowFilter = " Type='Break' OR Type='Block'";
            DataTable dtLunch = new DataTable();
            dtLunch = dvLunch.ToTable();
            int iAppointmentId = 0;
            string statuscode = common.myStr(e.Appointment.Attributes["Code"]);
            if (common.myInt(e.Appointment.Attributes["InvoiceId"]) > 0)
                iAppointmentId = common.myInt(e.Appointment.Attributes["InvoiceId"]);


            if (dtLunch.Rows.Count > 0)
            {
                isBreak = true;
            }
            //string test = e.Appointment.Attributes["DoesPatientOweMoney"];

            #region Region For No. Of Encounter

            DAL.DAL dlll = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsAutNo = new DataSet();
            if (e.Appointment.Attributes["RegistrationId"] != "")
            {
                dsAutNo = dlll.FillDataSet(CommandType.Text, "SELECT rid.NoOfEncounter, rid.EndDate FROM RegistrationInsuranceDetail rid inner join Registration r on rid.RegistrationId=r.Id WHERE rid.RegistrationID =" + e.Appointment.Attributes["RegistrationId"].ToString() + " AND rid.InsuranceOrder = 'P' and r.defaultpaymentcaseid = 1 AND rid.Active = 1");

                ////if (dsAutNo.Tables[0].Rows.Count > 0)
                ////{
                ////    hdnNoOfEncounter.Value = dsAutNo.Tables[0].Rows[0]["NoOfEncounter"].ToString();
                ////    string EndDate = dsAutNo.Tables[0].Rows[0]["EndDate"].ToString();
                ////    if (hdnNoOfEncounter.Value == "0")
                ////    {
                ////        Alert.ShowAjaxMsg("Patient maximum insurance coverage has been reached. Please address this payment issue.", Page);
                ////    }
                ////    else if (hdnNoOfEncounter.Value != "0" && hdnNoOfEncounter.Value != "")
                ////    {
                ////        Alert.ShowAjaxMsg("Patient has " + hdnNoOfEncounter.Value + " number of appointments remaining under their insurance plan.", Page);
                ////    }
                ////}
            }

            #endregion



            if (e.MenuItem.Value == "VH") // to show patient history always
            {
                RadWindowForNew.NavigateUrl = "~/emr/Masters/PatientHistory.aspx?MPG=P22175&RegId=" + e.Appointment.Attributes["RegistrationId"].ToString() + "&RegNo=" + e.Appointment.Attributes["RegistrationNo"].ToString();
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
            }
            else if (e.MenuItem.Value == "AL")
            {
                if (isBreak != true)
                {

                    RadWindowForNew.NavigateUrl = "/Appointment/AppointmentListV1.aspx?MPG=P22224&regForDetails=" + e.Appointment.Attributes["RegistrationId"].ToString().Trim();

                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;

                }
            }
            else if (e.MenuItem.Value == "Copy")
            {
                if (isBreak != true)
                {
                    ViewState["CutAppId"] = string.Empty;
                    ViewState["CopyRegistrationId"] = e.Appointment.Attributes["RegistrationId"].ToString();
                    ViewState["CopyAppId"] = strAppId[0];
                    if (ViewState["CopyRegistrationId"].ToString().Trim() == "")
                    {
                        ViewState["CopyRegistrationId"] = null;
                    }
                }
            }
            else if (e.MenuItem.Value.Equals("Cut"))
            {
                if (!isBreak.Equals(true))
                {
                    ViewState["CutAppId"] = "1";
                    ViewState["CopyAppId"] = string.Empty;
                    ViewState["AppId"] = common.myStr(e.Appointment.ID);
                    ViewState["WeekDateForRec"] = e.Appointment.Start;
                    ViewState["Rec"] = e.Appointment.RecurrenceRule;
                }
            }
            else if (e.MenuItem.Value == "Print")
            {
                if (isBreak != true)
                {
                    RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?MPG=P22213&RegistrationId=" + common.myInt(e.Appointment.Attributes["RegistrationId"]) + "&AppointmentId=" + common.myInt(strAppId[0]);
                    RadWindowForNew.Height = 580;
                    RadWindowForNew.Width = 750;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.ReloadOnShow = true;
                }
            }
            else if (e.MenuItem.Value == "PatientDetails")
            {
                if (statuscode == "PC")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Activity not allow already billed appointment !";
                    Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                    return;
                }
                if (isBreak != true)
                {

                    bool isNotAllow = false;
                    if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) < 0)
                    {
                        isNotAllow = true;
                    }
                    if (isNotAllow)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow for back day & time appointment !";
                        Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                        return;
                    }


                    if (e.Appointment.Attributes["RegistrationId"].ToString() != "")
                    {
                        Session["CategoryApp"] = "PopUp";
                        RadWindowForNew.NavigateUrl = "~/PRegistration/Demographics.aspx?MPG=P1&Pagetype=AppSche&RNo=" + common.myStr(e.Appointment.Attributes["RegistrationNo"]) + "&mode=E&Category=PopUp&RealDateOfBirth=" + e.Appointment.Attributes["RealDateOfBirth"];
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Width = 1000;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                    else
                    {
                        Session["CategoryApp"] = "PopUp";
                        RadWindowForNew.NavigateUrl = "~/PRegistration/Demographics.aspx?Pagetype=AppSche&AppID=" + e.Appointment.ID + "&mode=N&Mpg=P1&Category=PopUp";
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Width = 1350;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                    }

                }
            }
            else
            {
                string strStatus = "select d.StatusID,s.Code from DoctorAppointment D with (nolock) inner join StatusMaster s on d.StatusId = s.StatusId where d.AppointmentID =" + strAppId[0];
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet dsStatus = new DataSet();
                bool bStatus = false;
                string sStatusCode = "";
                int intStatusID = 0;
                int recparentid = 0;
                string sEncounterId = "";
                string recrule = "";
                DateTime OrigTime = DateTime.Now;
                DateTime OrigDate = DateTime.Today;

                dsStatus = dl.FillDataSet(CommandType.Text, strStatus);
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    //if (Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString()) == 3 || Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString()) == 5)
                    if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "P" || common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
                    {
                        bStatus = true;
                    }
                    else if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
                    {
                        if (e.MenuItem.Value != "Edit")
                            return;
                    }
                    intStatusID = Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString());
                    sStatusCode = common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString());
                }

                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                if (e.Appointment.Attributes["RegistrationId"] != "")
                {
                    string strEncouter = " SELECT ENC.ID, ENC.RegistrationNo , DA.AppointmentID, DA.DoctorId, DA.FacilityID, DA.RegistrationId,  "
                                        + " DA.Gender, ENC.EncounterNo "
                                        + " FROM Encounter ENC INNER JOIN DoctorAppointment DA "
                                        + " On ENC.AppointmentID =DA.AppointmentID "
                                        + " WHERE DA.AppointmentID=" + strAppId[0] + " AND DA.RegistrationId=" + e.Appointment.Attributes["RegistrationId"].ToString() + " ";
                    DataSet ds = new DataSet();
                    ds = dl.FillDataSet(CommandType.Text, strEncouter);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Session["ModuleId"] = 3;
                        int i = 0;
                        DataSet ds1 = (DataSet)Session["ModuleData"];
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            if (common.myStr(dr["ModuleName"]) == "EHR")
                            {
                                Session["ModuleId"] = i;
                            }
                            i++;
                        }
                        Session["Gender"] = ds.Tables[0].Rows[0]["Gender"].ToString();
                        Session["Facility"] = ds.Tables[0].Rows[0]["FacilityID"].ToString();
                        sEncounterId = ds.Tables[0].Rows[0]["ID"].ToString();
                        //Session["EncounterNo"] = ds.Tables[0].Rows[0]["EncounterNo"].ToString();
                        Session["DoctorID"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
                        Session["RegistrationID"] = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
                        Session["RegistrationNo"] = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        Session["AppointmentID"] = ds.Tables[0].Rows[0]["AppointmentID"].ToString();
                    }
                }


                if (e.MenuItem.Value == "Edit")
                {
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }
                    if (e.Appointment.ID.ToString().Contains('_'))
                    {
                        recparentid = common.myInt(e.Appointment.RecurrenceParentID);
                    }
                    else
                    {
                        recrule = "5";
                    }
                    if (isBreak == true)
                    {
                        RadWindowForNew.NavigateUrl = "~/Appointment/BreakAndBlock_New.aspx?MPG=P4&Category=PopUp&DoctorID=" + dtLunch.Rows[0]["DoctorID"].ToString() + " &ID=" + e.Appointment.ID.ToString() + "&appDate=" + e.Appointment.Start.Date.ToString("dd/MM/yyyy") + "&RecRule=" + recrule + "&recparentid=" + recparentid + "&appId=" + e.Appointment.ID.ToString() + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&StTime=" + RadScheduler1.DayStartTime.Hours.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&FacilityId=" + ddlFacility.SelectedValue;
                        RadWindowForNew.Height = 550;
                        RadWindowForNew.Width = 730;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;

                    }
                    else
                    {
                        bool IsDoctor = false;
                        if (ViewState["IsDoctor"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["IsDoctor"];
                            DataView dvDoctor = new DataView(dt);
                            dvDoctor.RowFilter = "ResourceID='" + e.Appointment.Resources[0].Key + "'";
                            DataTable dtDoctor = dvDoctor.ToTable();
                            if (dtDoctor.Rows.Count > 0)
                            {
                                IsDoctor = Convert.ToBoolean(dtDoctor.Rows[0]["IsDoctor"]);
                            }
                        }

                        RadWindowForNew.NavigateUrl = "~/Appointment/AppointmentDetails.aspx?MPG=P22222&StTime=" + RadScheduler1.DayStartTime.Hours.ToString() + "&EndTime=" + RadScheduler1.DayEndTime.Hours.ToString() + "&appDate=" + e.Appointment.Start.Date.ToString("MM/dd/yyyy") + "&appid=" + strAppId[0] + "&RecRule=" + recrule + "&recparentid=" + recparentid + "&doctorId=" + e.Appointment.Resources[0].Key + "&TimeInterval=" + RadScheduler1.MinutesPerRow.ToString() + "&AppId1=" + e.Appointment.ID.ToString() + "&PageId=3" + "&IsDoctor=" + IsDoctor + "&FacilityId=" + ddlFacility.SelectedValue;
                        RadWindowForNew.Height = 550;
                        RadWindowForNew.Width = 950;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                    }
                }

                else if (e.MenuItem.Value == "PatientChart")
                {
                    if (isBreak != true)
                    {
                        if (Convert.ToInt32(Session["encounterid"]) > 0)
                        {
                            Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["encounterid"]), Convert.ToInt16(Session["HospitalLocationID"]));
                            if (intFormId > 0)
                            {
                                Session["formId"] = Convert.ToString(intFormId);
                            }

                            Response.Redirect("/emr/Dashboard/PatientDashboard.aspx?Mpg=P6", false);
                            //Response.Redirect("~/PRegistration/Demographics.aspx?RNo=" + e.Appointment.Attributes["RegistrationId"].ToString().Trim() + "&mode=E&Mpg=P1", false);
                        }
                    }
                }
                else if (iAppointmentId > 0)
                {
                    Alert.ShowAjaxMsg("Already billed appointment..", Page.Page);
                    return;
                }
                else if (e.MenuItem.Value == "PC")
                {
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }
                    if (isBreak != true)
                    {

                        if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) > 0)
                        {
                            Alert.ShowAjaxMsg("Payment Cannot be taken for future appointment", Page);
                            return;
                        }

                        if ((common.myStr(e.Appointment.Attributes["Code"]) != "P"))
                        {
                            Alert.ShowAjaxMsg("Encounter not generated, Please check-in appointment !", Page);
                            return;
                        }
                        if ((common.myStr(e.Appointment.Attributes["Code"]) == "M"))
                        {
                            Alert.ShowAjaxMsg("Patient Status is No Show ! Payment Cannot be Taken", Page);
                            return;
                        }

                        if (intStatusID.ToString() != "1")
                        {
                            if (common.myInt(sEncounterId) > 0)
                            {
                                DataSet ds1 = (DataSet)Session["ModuleData"];
                                int i = 0;
                                foreach (DataRow dr in ds1.Tables[0].Rows)
                                {
                                    if (common.myStr(dr["ModuleName"]) == "Billing")
                                    {
                                        Session["ModuleId"] = i;
                                        Session["ModuleName"] = "Billing";
                                    }
                                    i++;
                                }
                                string strRegNo = common.myStr(dl.ExecuteScalar(CommandType.Text, "Select RegistrationNo from Registration with(NoLock) where id = " + common.myStr(e.Appointment.Attributes["RegistrationId"]) + ""));
                                string sPath = "~/EMRBILLING/OPBill.aspx?MPG=P309&DoctorId=" + common.myStr(e.Appointment.Resources[0].Key) + "&CF=POPUP&RegNo=" + common.myStr(strRegNo) + "&EncId=" + common.myInt(sEncounterId) + "&IsAppoint=1" + "&AppointmentId=" + common.myStr(strAppId[0]) + "&PackageId=" + common.myInt(e.Appointment.Attributes["PackageId"]) + "&PackageType=" + common.myStr(e.Appointment.Attributes["PackageType"]);
                                #region CheckUserPermission
                                UserAuthorisations ua = new UserAuthorisations();
                                string[] strarry = sPath.Split('/');
                                int lengh = strarry.Length;
                                string pageName = strarry[lengh - 1];

                                if (!ua.CheckPermissions("N", pageName))
                                {
                                    Alert.ShowAjaxMsg("You are not authorised for this activity!", Page.Page);
                                    return;
                                }
                                ua.Dispose();
                                #endregion

                                //Response.Redirect("~/EMRBILLING/OPBill.aspx?DoctorId=" + common.myStr(e.Appointment.Resources[0].Key) + "&RegNo=" + common.myStr(strRegNo) + "&EncId=" + common.myInt(sEncounterId) + "&IsAppoint=1" + "&AppointmentId=" + common.myStr(strAppId[0]) + "&PackageId=" + common.myInt(e.Appointment.Attributes["PackageId"]) + "&PackageType=" + common.myStr(e.Appointment.Attributes["PackageType"]), false);
                                RadWindowForNew.NavigateUrl = sPath;

                                RadWindowForNew.OnClientClose = "OnClientClose";
                                RadWindowForNew.Modal = true;
                                RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                                RadWindowForNew.VisibleStatusbar = false;
                                RadWindowForNew.ReloadOnShow = true;
                            }
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("UnConfirm appointments bill not generated, Please check-in appointment !", Page);
                            return;
                        }
                    }
                }

                else if (e.MenuItem.Value == "DelChkIn")
                {
                    if (statuscode == "PC")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Activity not allow already billed appointment !";
                        Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                        return;
                    }
                    if (bStatus == true)
                    {
                        if (strAppId[0].ToString() != "")
                        {
                            if (sStatusCode == "P" || sStatusCode == "PC")
                            {
                                BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                                DataSet ds = appoint.getAppointmentEncounter(Convert.ToInt32(strAppId[0]));
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    int EncounterId = Convert.ToInt32(ds.Tables[0].Rows[0]["EncounterId"]);
                                    int RegistrationId = Convert.ToInt32(ds.Tables[0].Rows[0]["RegistrationId"]);

                                    DataView dvChk = ((DataTable)appoint.GetEncounterStatusToDelete(EncounterId, RegistrationId)).DefaultView;
                                    dvChk.RowFilter = "TemplateName <> 'Allergies'";
                                    DataTable dt = dvChk.ToTable();
                                    if (dt.Rows.Count > 0)
                                    {
                                        lblMessage.Text = "Check-In Status can't delete. Clinical Details Found.";
                                        Alert.ShowAjaxMsg("Check-In Status can't delete. Clinical Details Found !", Page);
                                        return;
                                    }
                                    else
                                    {
                                        ViewState["MenuStatus"] = "CheckedIn";
                                        ViewState["CancelEncounter"] = EncounterId;
                                        ViewState["CancelAppointmentId"] = common.myInt(strAppId[0]);
                                        ViewState["CancelRegistrationId"] = RegistrationId;
                                        lblDeleteApp.Text = "Cancel Appointment Chk-In";
                                        dvDelete.Visible = true;
                                        fillRemarks();
                                    }
                                }
                                else
                                {
                                    ViewState["MenuStatus"] = "CheckedIn";
                                    ViewState["CancelEncounter"] = 0;
                                    ViewState["CancelAppointmentId"] = common.myInt(strAppId[0]);
                                    ViewState["CancelRegistrationId"] = common.myStr(e.Appointment.Attributes["RegistrationId"]);
                                    lblDeleteApp.Text = "Cancel Appointment Chk-In";
                                    dvDelete.Visible = true;
                                    fillRemarks();
                                }
                            }

                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Appointment Is Not Checked-In!", Page);
                        return;
                    }
                }
                else if (e.MenuItem.Value == "Delete")
                {
                    BaseC.HospitalSetup ooSetup = new BaseC.HospitalSetup(sConString);
                    if (ooSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsCancelAppointment", common.myInt(Session["FacilityId"])).Equals("Y"))
                    {
                        if (statuscode == "PC")
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Activity not allow already billed appointment !";
                            Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                            return;
                        }
                        if (bStatus == true && isBreak == false)
                        {
                            Alert.ShowAjaxMsg("Appointment Already Checked-In. Appointment Cannot Be Deleted.", Page.Page);
                            return;
                        }
                        bool isNotAllow = false;
                        if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) < 0)
                        {
                            isNotAllow = true;
                        }
                        if (isNotAllow)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Activity not allow for back day & time appointment !";
                            Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                            return;
                        }

                        lblDeleteApp.Text = "Delete Appointment";
                        ViewState["AppId"] = e.Appointment.ID.ToString();
                        ViewState["WeekDateForRec"] = e.Appointment.Start;
                        ViewState["Rec"] = e.Appointment.Attributes["RecurrenceRule"];
                        ViewState["StatusId"] = e.Appointment.Attributes["StatusId"];
                        ViewState["MenuStatus"] = "Delete";
                        lblDeleteEncounterMessage.Text = "";
                        txtCancel.Text = "";
                        if (isBreak == true)
                        {
                            divDeleteBreak.Visible = true;
                        }
                        else
                        {
                            if (e.Appointment.Attributes["RecurrenceRule"].Trim() != "")
                            {
                                dvDeleteRecurring.Visible = true;
                                fillRemarks();
                            }
                            else
                            {

                                dvDelete.Visible = true;
                                fillRemarks();
                            }
                        }
                    }
                    else
                    {
                        //Alert.ShowAjaxMsg("You can not cancel appointment", Page);
                        Alert.ShowAjaxMsg("You are not authorise persion for cancel appointment", Page);
                        return;
                    }
                }

                else if (e.MenuItem.Value == "Superbill")
                {
                    if (isBreak != true)
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/PatientSuperBill.aspx?MPG=P22216&RegistrationId=" + e.Appointment.Attributes["RegistrationId"].ToString() + "&AppointmentId=" + strAppId[0];
                        RadWindowForNew.Height = 600;
                        RadWindowForNew.Width = 800;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.ReloadOnShow = true;
                    }
                }

                else if (e.MenuItem.Value == "OpenClaim")
                {
                    if (isBreak != true)
                    {
                        if (bStatus == true)
                        {
                            Session["ModuleId"] = 14;
                            Session["ModuleName"] = "Billing";

                            Hashtable hshInputData = new Hashtable();
                            hshInputData.Add("@PatientID", e.Appointment.Attributes["RegistrationId"].ToString().Trim());
                            hshInputData.Add("@EncounterID", common.myInt(Session["encounterid"]));
                            hshInputData.Add("@EncodedBy", Session["UserID"]);
                            DataSet ds1 = dl.FillDataSet(CommandType.StoredProcedure, "Bill_AddNewClaim", hshInputData);
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                Response.Redirect("~/Billing/AddClaim.aspx?BillID=" + ds1.Tables[0].Rows[0]["BillID"].ToString() + "&Mpg=P143", false);
                            }

                        }
                    }
                }
                else if (e.MenuItem.Value == "Orders")
                {
                    if (isBreak != true)
                    {

                        RadWindowForNew.NavigateUrl = "/EMRBILLING/Order.aspx?MPG=P890&regForDetails=" + e.Appointment.Attributes["RegistrationId"].ToString().Trim();
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Width = 980;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;

                    }
                }

                else //if (e.MenuItem.Value == "2") //Confirmed
                {
                    if (isBreak != true)
                    {
                        bool isNotAllow = false;
                        if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) < 0)
                        {
                            isNotAllow = true;
                        }
                        if (isNotAllow)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Activity not allow for back day & time appointment !";
                            Alert.ShowAjaxMsg("Activity not allow for back  day & time appointment !", Page);
                            return;
                        }

                        if (bStatus == true)
                        {
                            Alert.ShowAjaxMsg("Appointment Already Checked-In...", Page.Page);
                            return;
                        }

                        ViewState["MenuSdate"] = e.Appointment.Start.Date;
                        if (e.MenuItem.Value == "P" && Convert.ToInt32(FindFutureDate(Convert.ToString(e.Appointment.Start.Date))) > Convert.ToInt32(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
                        {

                            int x = common.myInt(strAppId[0]);
                            BaseC.Appointment objApp = new BaseC.Appointment(sConString);
                            DataSet ds = objApp.getAppointmentEncounter(x);


                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string EncounterNo = Convert.ToString(ds.Tables[0].Rows[0]["EncounterNo"]);
                                ViewState["EncounterNo"] = EncounterNo;
                            }
                            if (ViewState["EncounterNo"] != null)
                            {
                                string str = "IP patient not allowed to Check-IN";
                                Alert.ShowAjaxMsg(str, Page);
                                return;
                            }

                            string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                            Alert.ShowAjaxMsg(strMessage, Page);
                            return;


                        }
                        if (e.MenuItem.Value == "P")
                        {
                            int x = common.myInt(e.Appointment.Attributes["RegistrationId"]);
                            BaseC.Appointment objApp = new BaseC.Appointment(sConString);
                            DataSet ds = objApp.isCheckAdmittedPatient(x);
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string str = "IP patient (" + common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]) + ") not allowed to Check-IN";
                                    Alert.ShowAjaxMsg(str, Page);
                                    return;
                                }
                            }
                            ds.Dispose();
                            objApp = null;

                            if (statuscode == "PC")
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "Activity not allow already billed appointment !";
                                Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                                return;
                            }
                            if (DateTime.Compare(e.Appointment.Start.Date, DateTime.Now.Date) != 0)
                            {
                                string strMessage = "Warning. You Are Only Allowed To CheckIn For Current Date.";
                                Alert.ShowAjaxMsg(strMessage, Page);
                                return;
                            }
                            // Santosh for dubai
                            BaseC.Appointment appoint = new BaseC.Appointment(sConString);

                            ///Check if Encounter already present

                            //if (e.MenuItem.Value == "P")
                            //{
                            //    string sQuery = "SELECT id FROM Encounter en WITH (NOLOCK) INNER JOIN DOCTORAPPOINTMENT DA WITH (NOLOCK) ON EN.AppointmentId=da.AppointmentId INNER JOIN FacilityMaster fm WITH (NOLOCK) ON en.FacilityId=fm.FacilityId INNER JOIN STATUSMASTER SM WITH (NOLOCK) ON da.StatusId=sm.StatusId " +
                            //    " WHERE en.Registrationid =  @intRegistrationId AND CONVERT(VARCHAR,DATEADD(mi,fm.TIMEZONEOFFSETMINUTES,EncounterDate),111) = CONVERT(VARCHAR,DATEADD(mi,fm.TIMEZONEOFFSETMINUTES,GETUTCDATE()),111) " +
                            //                " AND en.DoctorId = @intDoctorId AND EN.FacilityID = @intFacilityId AND SM.Code = @chvStatusCode AND OPIP ='O' AND EN.Active = 1";
                            //    Hashtable hshEncounter = new Hashtable();
                            //    hshEncounter.Add("@intRegistrationId", e.Appointment.Attributes["RegistrationId"]);
                            //    hshEncounter.Add("@intDoctorId", e.Appointment.Attributes["DoctorId"]);
                            //    hshEncounter.Add("@intFacilityId", ddlFacility.SelectedValue);
                            //    hshEncounter.Add("@chvStatusCode", e.MenuItem.Value);
                            //    DataSet ds = dl.FillDataSet(CommandType.Text, sQuery, hshEncounter);
                            //    if (ds.Tables.Count > 0)
                            //    {
                            //        if (ds.Tables[0].Rows.Count > 0)
                            //        {
                            //            Alert.ShowAjaxMsg("Encounter already open", Page);
                            //            return;
                            //        }
                            //    }
                            //}

                            ////end
                            if (common.myStr(e.Appointment.Attributes["NoPAForInsurancePatient"]) == "False" || common.myStr(e.Appointment.Attributes["NoPAForInsurancePatient"]) == "0")
                            {
                                if (common.myStr(ddlSpecilization.SelectedItem.Attributes["Code"]) != "G")
                                {

                                    bool IsRequiredPreAuthNo = appoint.GetPatientAccountType(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt32(e.Appointment.Attributes["RegistrationId"]), Convert.ToInt32(e.Appointment.ID));
                                    if (IsRequiredPreAuthNo == true)
                                    {
                                        Alert.ShowAjaxMsg("Check-in not allow this patient. PA No. required.", Page);
                                        return;
                                    }
                                    //}
                                }
                            }
                            //if (strAppId[0] != "")
                            //{
                            //    DataTable dsav = appoint.Checkvisittype(common.myInt(strAppId[0]));
                            //    if (dsav.Rows.Count == 0)
                            //    {
                            //        Alert.ShowAjaxMsg("Check-in not allow this patient. Invalid Visit Type.", Page);
                            //        return;
                            //    }

                            //}

                        }
                        if (e.MenuItem.Value == "M")
                        {
                            if (statuscode == "PC")
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "Activity not allow already billed appointment !";
                                Alert.ShowAjaxMsg("Activity not allow already billed appointment !", Page);
                                return;
                            }
                            ViewState["MenuItemValue"] = e.MenuItem.Value;
                            ViewState["MenuAppID"] = e.Appointment.ID.ToString();
                            if (e.Appointment.ID.ToString().Contains('_'))
                            {
                                ViewState["MenuRecurrenceParentID"] = Convert.ToString(e.Appointment.RecurrenceParentID.ToString());
                            }
                            else
                            {
                                ViewState["MenuRecurrenceParentID"] = 0;
                            }
                            ViewState["MenuRecurrenceState"] = e.Appointment.RecurrenceState;

                        }
                        else
                        {
                            string origrule = "Select RecurrenceRule,AppointmentDate from doctorappointment where appointmentid='" + strAppId[0] + "'";
                            string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                            if ((e.Appointment.RecurrenceState == RecurrenceState.Occurrence || e.Appointment.RecurrenceState == RecurrenceState.Exception) && OrigRecRule.Trim() != "")
                            {
                                if (e.Appointment.ID.ToString().Contains('_'))
                                {
                                    recparentid = common.myInt(e.Appointment.RecurrenceParentID);
                                }
                                DateTime dtStart = e.Appointment.Start;
                                Hashtable hshInput = new Hashtable();
                                Hashtable hshOutput = new Hashtable();
                                //
                                hshInput.Add("@intParentAppointmentID", strAppId[0]);
                                hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                                hshInput.Add("@intFacilityID", Session["FacilityID"]);
                                hshOutput.Add("@intStatusID", SqlDbType.Int);
                                hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));
                                hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);
                                if (hshOutput["@intStatusID"].ToString() == "3")
                                {
                                    //btnRefresh_OnClick(sender, e);
                                }
                                else if (hshOutput["@intStatusID"].ToString() == "5")
                                {
                                    //btnRefresh_OnClick(sender, e);
                                }
                                else if (hshOutput["@intStatusID"].ToString() == "2")
                                {
                                    // btnRefresh_OnClick(sender, e);
                                }
                                else
                                {
                                    Hashtable hshIn = new Hashtable();
                                    Hashtable hshtableout = new Hashtable();
                                    hshIn.Add("@intAppointmentId", strAppId[0]);
                                    hshIn.Add("@intEncodedBy", Session["UserId"]);
                                    hshIn.Add("@chrStatusCode", e.MenuItem.Value);
                                    hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);

                                    hshIn.Add("@dtAppointmentDate", dtStart);

                                    hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);

                                    // hshtableout["intRecAppointmentId"].ToString();
                                    string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                                    OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                                    string strOrigDate = "select AppointmentDate,StatusId from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

                                    string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                                    if (!OrigRecRule.Contains("EXDATE"))
                                    {
                                        string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                                    }
                                    else
                                    {
                                        string UpdateRule = OrigRecRule + "," + exdate;
                                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                                    }
                                    // btnRefresh_OnClick(sender, e);
                                }
                            }
                            else
                            {

                                Hashtable hshIn = new Hashtable();
                                hshIn.Add("@intAppointmentId", strAppId[0]);
                                hshIn.Add("@intEncodedBy", Session["UserId"]);
                                hshIn.Add("@chrStatusCode", e.MenuItem.Value);
                                dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                                btnRefresh_OnClick(sender, e);
                            }
                        }
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
            BC = null;
            objHp = null;
        }
    }
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            if (Convert.ToString(ViewState["MenuItemValue"]) == "C")
            {
                if (ddlRemarks.SelectedValue == "0")
                {
                    Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                    return;
                }
                string Remarks = ddlRemarks.SelectedItem.Text;
                DateTime OrigTime = DateTime.Now;
                string[] strAppId = ViewState["MenuAppID"].ToString().Split('_');
                DateTime dtStart = Convert.ToDateTime(ViewState["MenuSdate"].ToString());
                Hashtable hshIn = new Hashtable();
                string origrule = "Select isnull(RecurrenceRule,'') as RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (OrigRecRule.Trim() != null && OrigRecRule.Trim() != "")
                {

                    //DateTime dtStart = e.Appointment.Start;
                    Hashtable hshInput = new Hashtable();
                    Hashtable hshOutput = new Hashtable();
                    hshInput.Add("@intParentAppointmentID", strAppId[0]);
                    hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                    hshInput.Add("@intFacilityID", Session["FacilityID"]);
                    hshOutput.Add("@intStatusID", SqlDbType.Int);

                    hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));

                    hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);

                    if (hshOutput["@intStatusID"].ToString() == "34")
                    {
                        //btnRefresh_OnClick(sender, e);
                    }
                    else
                    {
                        //Hashtable hshIn = new Hashtable();
                        Hashtable hshtableout = new Hashtable();
                        hshIn.Add("@intAppointmentId", strAppId[0]);
                        hshIn.Add("@intEncodedBy", Session["UserId"]);
                        hshIn.Add("@chrStatusCode", Convert.ToString(ViewState["MenuItemValue"]));
                        hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);
                        hshIn.Add("@dtAppointmentDate", dtStart.Date);


                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);
                        string strUpdateRecID = hshtableout["@intRecAppointmentId"].ToString();
                        string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                        OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                        string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

                        //DateTime dtStart = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                        string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                        //if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
                        //{                

                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            string strUpdateNote = "Update doctorappointment set Note= '" + Remarks + "' where appointmentid='" + strUpdateRecID + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            string strUpdateNote = "Update doctorappointment set  Note='" + Remarks + "' where appointmentid='" + strUpdateRecID + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                        }
                    }
                }
                else
                {
                    //Hashtable hshIn = new Hashtable();
                    hshIn.Add("@intAppointmentId", strAppId[0]);
                    hshIn.Add("@intEncodedBy", Session["UserId"]);
                    //hshIn.Add("@intStatusId", e.MenuItem.Value);
                    hshIn.Add("@chrStatusCode", Convert.ToString(ViewState["MenuItemValue"]));
                    dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                    string strUpdateRecRule = "Update doctorappointment set Note='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                    int i = dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }

                ViewState["MenuAppID"] = null;
                ViewState["MenuItemValue"] = null;
                ViewState["MenuSdate"] = null;
            }
            if (Convert.ToString(ViewState["MenuItemValue"]) == "M")
            {
                string Remarks = ddlRemarks.SelectedItem.Text;
                string recparentid = "";
                DateTime OrigTime = DateTime.Now;
                DateTime OrigDate = Convert.ToDateTime(ViewState["MenuSdate"].ToString());
                string[] strAppId = ViewState["MenuAppID"].ToString().Split('_');

                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                //if ((e.Appointment.RecurrenceState == RecurrenceState.Occurrence || e.Appointment.RecurrenceState == RecurrenceState.Exception) && OrigRecRule.Trim() != "")
                if ((ViewState["MenuRecurrenceState"].ToString() == "Occurrence" || ViewState["MenuRecurrenceState"].ToString() == "Exception") && OrigRecRule.Trim() != "")
                {
                    if (ViewState["MenuAppID"].ToString().Contains('_'))
                    {
                        //recparentid = e.Appointment.RecurrenceParentID.ToString();
                        recparentid = ViewState["MenuRecurrenceParentID"].ToString();
                    }
                    DateTime dtStart = Convert.ToDateTime(ViewState["MenuSdate"].ToString());

                    //DateTime dtStart = e.Appointment.Start;
                    Hashtable hshInput = new Hashtable();
                    Hashtable hshOutput = new Hashtable();
                    hshInput.Add("@intParentAppointmentID", strAppId[0]);
                    hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                    hshInput.Add("@intFacilityID", Session["FacilityID"]);
                    hshOutput.Add("@intStatusID", SqlDbType.Int);

                    hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));

                    hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);

                    if (hshOutput["@intStatusID"].ToString() == "6")
                    {
                        //btnRefresh_OnClick(sender, e);
                    }
                    else
                    {
                        Hashtable hshIn = new Hashtable();
                        Hashtable hshtableout = new Hashtable();
                        hshIn.Add("@intAppointmentId", strAppId[0]);
                        hshIn.Add("@intEncodedBy", Session["UserId"]);
                        //hshIn.Add("@intStatusId", e.MenuItem.Value);
                        hshIn.Add("@chrStatusCode", ViewState["MenuItemValue"].ToString());
                        hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);
                        hshIn.Add("@dtAppointmentDate", dtStart);


                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);
                        string strUpdateRecID = hshtableout["@intRecAppointmentId"].ToString();
                        string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                        OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                        string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";

                        //DateTime dtStart = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                        string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                        //if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
                        //{                

                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            string strUpdateNote = "Update doctorappointment set Note= '" + ddlRemarks.SelectedItem.Text + "' where appointmentid='" + strUpdateRecID + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + strAppId[0] + "'";
                            string strUpdateNote = "Update doctorappointment set  Note='" + ddlRemarks.SelectedItem.Text + "' where appointmentid='" + strUpdateRecID + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateNote);
                        }
                        // btnRefresh_OnClick(sender, e);
                    }
                }
                else
                {
                    Hashtable hshIn = new Hashtable();
                    hshIn.Add("@intAppointmentId", strAppId[0]);
                    hshIn.Add("@intEncodedBy", Session["UserId"]);
                    //hshIn.Add("@intStatusId", e.MenuItem.Value);
                    hshIn.Add("@chrStatusCode", ViewState["MenuItemValue"].ToString());
                    dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                    string strUpdateRecRule = "Update doctorappointment set Note='" + ddlRemarks.SelectedItem.Text + "' where appointmentid='" + strAppId[0] + "'";
                    int i = dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);

                    // btnRefresh_OnClick(sender, e);
                }

                //btnRefresh_OnClick(sender, e);

                ViewState["MenuItemValue"] = null;
                ViewState["MenuAppID"] = null;
                ViewState["MenuRecurrenceParentID"] = null;
                ViewState["MenuRecurrenceState"] = null;
            }

            // btnRefresh_OnClick(sender, null);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
    }
    protected void btnCancelUpdateStatus_Click(object sender, EventArgs e)
    {
        ViewState["MenuAppID"] = null;
        ViewState["MenuItemValue"] = null;
        ViewState["MenuSdate"] = null;
        ViewState["MenuRecurrenceParentID"] = null;
        ViewState["MenuRecurrenceState"] = null;

    }

    protected void RadScheduler1_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
    {
        if (e.Command == SchedulerNavigationCommand.SwitchToDayView)
        {
            RadScheduler1.DayView.HeaderDateFormat = Session["OutputDateFormat"].ToString();
            ViewState["CurrentView"] = "D";
            chkOptionView.Visible = false;

        }
        else if (e.Command == SchedulerNavigationCommand.SwitchToWeekView)
        {
            RadScheduler1.WeekView.HeaderDateFormat = Session["OutputDateFormat"].ToString();
            ViewState["CurrentView"] = "W";
            chkOptionView.Visible = true;
        }
        else if (e.Command == SchedulerNavigationCommand.SwitchToMonthView)
        {
            ViewState["CurrentView"] = "M";
            chkOptionView.Visible = true;
        }
        btnRefresh_OnClick(sender, e);
    }
    protected void btnbtnprint_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            foreach (RadListBoxItem Item in RadLstDoctor.Items)
            {
                if (Item.Checked == true)
                {
                    if (Convert.ToString(ViewState["DocId"]) == "")
                        ViewState["DocId"] = Item.Value;
                    else
                        ViewState["DocId"] += "," + Item.Value;
                    ViewState["docname"] = Item.Text;

                }
            }
            String value = String.Empty;
            //CheckBox checkbox = new CheckBox();
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                value = currentItem.Value;
                string strName = currentItem.Text;
                if (currentItem.Checked == true)
                {
                    if (Convert.ToString(ViewState["DocId"]) == "")
                        ViewState["DocId"] = value;
                    else
                        ViewState["DocId"] += "," + value;
                    ViewState["docname"] = strName;
                }
            }
            if (i > 1)
            {
                Alert.ShowAjaxMsg("Please Select Only One Doctor At a Time", Page);
                return;
            }
            //else
            //{
            if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
            {
                #region Log is CPT entered is E&M code and clinical summary is printed or patient portal is active
                Hashtable logHash = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                logHash.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));
                logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
                logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
                logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                logHash.Add("@intFacilityID", Convert.ToInt32(Session["FacilityID"]));
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogCPTInE&MAndClinicalSummary", logHash);
                #endregion
            }
            RadWindowForNew.NavigateUrl = "/Appointment/PrintAppointmentscheduleV1.aspx?MPG=P22225&FacilityName=" + ddlFacility.SelectedItem.Text + "&ProviderName=" + Convert.ToString(ViewState["docname"]) + "&Appdate=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&DocId=" + Convert.ToString(ViewState["DocId"]) + "&FacId=" + ddlFacility.SelectedValue + "";
            RadWindowForNew.Height = 540;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.ReloadOnShow = true;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnDeleteAllBreak_Click(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            DateTime origAppTime = DateTime.Now;
            DateTime origAppDate = DateTime.Today;

            if (ViewState["AppId"].ToString().Contains('_') || ViewState["Rec"].ToString() != "")
            {
                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                //string recrule1 = e.Appointment.Attributes["RecurrenceRule"];                
                string strOrigTime = "select BreakDate from BreakList where ID='" + strAppId[0] + "'";
                origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                string strOrigDate = "select BreakDate from BreakList where ID='" + strAppId[0] + "'";
                origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                TimeSpan daysCount = dtStart.Subtract(origAppDate);

                string exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";

                string origrule = "Select RecurrenceRule from BreakList where ID='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("COUNT"))
                {
                    if (!OrigRecRule.Contains("UNTIL"))
                    {
                        //int newrule = OrigRecRule.IndexOf("INTERVAL");
                        string newRecRule = OrigRecRule.Insert((OrigRecRule.IndexOf("INTERVAL") == -1) ? 0 : OrigRecRule.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                    else
                    {
                        //int newrule = OrigRecRule.IndexOf("UNTIL");
                        string newrule = OrigRecRule.Insert((OrigRecRule.IndexOf("UNTIL") == -1) ? 0 : OrigRecRule.IndexOf("UNTIL"), exdate);
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newrule + "' where ID='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                }
                else
                {
                    if (OrigRecRule.Contains("COUNT"))
                    {
                        int noOfChars = OrigRecRule.IndexOf("INTERVAL") - OrigRecRule.IndexOf("COUNT");
                        string repUntil = OrigRecRule.Remove(OrigRecRule.IndexOf("COUNT"), noOfChars);
                        string newRecRule = repUntil.Insert(repUntil.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                }
            }
            else
            {
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@intBreakId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                dl.ExecuteNonQuery(CommandType.Text, " Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=GETUTCDATE() where Id=@intBreakId", hshIn);
            }
            divDeleteBreak.Visible = false;
            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        divDeleteBreak.Visible = false;
    }
    protected void btnCancelRecurrApp_OnClick(object sender, EventArgs e)
    {
        dvDeleteRecurring.Visible = false;
    }
    protected void btnDeleteBreak_Click(object sender, EventArgs e)
    {

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {

            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            Hashtable hshIn = new Hashtable();
            if (ViewState["AppId"].ToString().Contains('_') || ViewState["Rec"] != null)
            {
                //SchedulerNavigationCommand.SwitchToSelectedDay
                DateTime OrigRecTime = DateTime.Now;
                string strOrigRecTime = "select StartTime from BreakList where ID='" + strAppId[0] + "'";
                OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

                string origrule = "Select RecurrenceRule from BreakList where ID='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("EXDATE"))
                {
                    string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                    string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + strAppId[0] + "'";
                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }
                else
                {
                    string UpdateRule = OrigRecRule + "," + exdate;
                    string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + strAppId[0] + "'";
                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }
            }
            else
            {
                //Hashtable hshIn = new Hashtable();
                hshIn.Add("@intBreakId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                dl.ExecuteNonQuery(CommandType.Text, "Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=GETUTCDATE() where Id=@intBreakId", hshIn);
            }
            divDeleteBreak.Visible = false;
            btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
        }
    }
    protected void btnDeleteThisApp_Click(object sender, EventArgs e)
    {
        try
        {
            //This Recurring Cancel
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            BaseC.Appointment appoint = new BaseC.Appointment(sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            if (ddlRemarks.SelectedItem.Text == "")
            {
                Alert.ShowAjaxMsg("Please type cancel reason", Page);
                return;
            }
            string Remarks = ddlRemarks.SelectedItem.Text + ". " + txtRecurRemark.Text;
            if (ViewState["AppId"].ToString().Contains('_'))
            {
                DateTime OrigRecTime = DateTime.Now;
                string strOrigRecTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("EXDATE"))
                {
                    string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "', CancelReason='" + Remarks + "'  where appointmentid='" + strAppId[0] + "'";
                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }
                else
                {
                    string UpdateRule = OrigRecRule + "," + exdate;
                    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "', CancelReason='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                }
            }
            else
            {
                //Hashtable hshIn = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvCancelRemark", Remarks);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
                Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);

            }

            dvDelete.Visible = false;
            // btnRefresh_OnClick(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnDeleteApp_Click(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Appointment appoint = new BaseC.Appointment(sConString);

        try
        {


            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            if (ddlRemarkss.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                return;
            }
            string Remarks = ddlRemarkss.SelectedItem.Text + ". " + txtCancel;
            if (common.myStr(ViewState["MenuStatus"]) == "CheckedIn")
            {
                hshIn.Add("@iHospitalLocationId", Session["HospitalLocationID"]);
                hshIn.Add("@iFacilityId", common.myInt(ddlFacility.SelectedValue));
                hshIn.Add("@iEncounterId", common.myInt(ViewState["CancelEncounter"]));
                hshIn.Add("@iAppointmentId", common.myInt(ViewState["CancelAppointmentId"]));
                hshIn.Add("@iRegistrationId", common.myInt(ViewState["CancelRegistrationId"]));
                hshIn.Add("@sRemarks", Remarks);
                dl.ExecuteNonQuery(CommandType.StoredProcedure, "uspCancelAppointmentCheckin", hshIn);
            }
            else
            {

                string[] strAppId = ViewState["AppId"].ToString().Split('_');
                //Hashtable hshIn = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvCancelRemark", Remarks);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
                Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);
            }

            dvDelete.Visible = false;
            btnRefresh_OnClick(sender, e);


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
            appoint = null;
        }
    }
    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        dvDelete.Visible = false;
    }
    protected void btnMakeAppointment_Click(object sender, EventArgs e)
    {
        string navstr = "";
        divNoSLot.Visible = false;
        navstr = "~/Appointment/AppointmentDetails.aspx?MPG=P22222&FacilityId=" + common.myStr(ViewState["FacilityId"]) +
            "&StTime=" + common.myStr(ViewState["StTime"])
            + "&EndTime=" + common.myStr("23")
            + "&appDate=" + common.myStr(ViewState["appDate"])
            + "&appid=0&doctorId=" + common.myStr(ViewState["doctorId"])
            + "&TimeInterval=" + common.myStr(ViewState["TimeInterval"])
            + "&FromTimeHour=" + common.myStr(ViewState["FromTimeHour"])
            + "&FromTimeMin=" + common.myStr(ViewState["FromTimeMin"])
            + "&ToTimeHour=" + common.myStr(ViewState["ToTimeHour"])
        + "&ToTimeMin=" + common.myStr(ViewState["ToTimeMin"])
            + "&PageId=" + common.myStr(ViewState["PageId"])
            + "&IsDoctor=" + common.myStr(ViewState["IsDoctorPara"]);


        RadWindowForNew.NavigateUrl = navstr;
        RadWindowForNew.Height = 610;
        RadWindowForNew.Width = 940;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void btnDeleteAllApp_Click(object sender, EventArgs e)
    {

        BaseC.Appointment appoint = new BaseC.Appointment(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            //All Recurring Cancel

            string[] strAppId = ViewState["AppId"].ToString().Split('_');
            DateTime origAppTime = DateTime.Now;
            DateTime origAppDate = DateTime.Today;

            if (ddlRemarks.SelectedValue == "0")
            {
                ddlRemarks.Focus();
                Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                return;
            }
            string Remarks = ddlRemarks.SelectedItem.Text + ". " + txtRecurRemark.Text;
            if (ViewState["StatusId"].ToString() == "3")
            {
                DataSet ds = appoint.getAppointmentEncounter(Convert.ToInt32(strAppId[0]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int EncounterId = Convert.ToInt32(ds.Tables[0].Rows[0]["EncounterId"]);
                    int RegistrationId = Convert.ToInt32(ds.Tables[0].Rows[0]["RegistrationId"]);
                    DataTable dt = appoint.GetEncounterStatusToDelete(EncounterId, RegistrationId);
                    if (dt.Rows.Count > 0)
                    {
                        lblRecurringDeleteMessage.Text = "Appointment Can't Delete. EMR details found.";
                        return;
                    }
                }
            }
            if (ViewState["AppId"].ToString().Contains('_'))
            {
                DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                //string recrule1 = e.Appointment.Attributes["RecurrenceRule"];                
                string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                TimeSpan daysCount = dtStart.Subtract(origAppDate);

                string exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";

                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("COUNT"))
                {
                    if (!OrigRecRule.Contains("UNTIL"))
                    {
                        //int newrule = OrigRecRule.IndexOf("INTERVAL");
                        string newRecRule = OrigRecRule.Insert(OrigRecRule.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newRecRule + "', CancelReason='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                    else
                    {
                        //int newrule = OrigRecRule.IndexOf("UNTIL");
                        string newrule = OrigRecRule.Insert(OrigRecRule.IndexOf("UNTIL"), exdate);
                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newrule + "', CancelReason='" + Remarks + "' where appointmentid='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                }
                else
                {
                    if (OrigRecRule.Contains("COUNT"))
                    {
                        int noOfChars = OrigRecRule.IndexOf("INTERVAL") - OrigRecRule.IndexOf("COUNT");
                        string repUntil = OrigRecRule.Remove(OrigRecRule.IndexOf("COUNT"), noOfChars);
                        string newRecRule = repUntil.Insert(repUntil.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newRecRule + "' where appointmentid='" + strAppId[0] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }

                }
            }
            else
            {
                Hashtable hshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intLastChangedBy", Session["UserId"]);
                hshIn.Add("@chvCancelRemark", Remarks);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
                Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);
            }
            dvDelete.Visible = false;
            //btnRefresh_OnClick(sender, e);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            dl = null;
            appoint = null;
        }
    }
    protected void btnCancel_Click(object sendet, EventArgs e)
    {
        divNoSLot.Visible = false;
    }
    protected void fillRemarks()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "AppCancel", "AppCancel");
        ddlRemarks.Items.Clear();
        ddlRemarkss.Items.Clear();

        ddlRemarks.DataSource = ds;
        ddlRemarks.DataTextField = "Status";
        ddlRemarks.DataValueField = "StatusId";
        ddlRemarks.DataBind();
        ddlRemarks.Items.Insert(0, "Select a Reason");
        ddlRemarks.Items[0].Value = "0";

        ddlRemarkss.DataSource = ds;
        ddlRemarkss.DataTextField = "Status";
        ddlRemarkss.DataValueField = "StatusId";
        ddlRemarkss.DataBind();
        ddlRemarkss.Items.Insert(0, "Select a Reason");
        ddlRemarkss.Items[0].Value = "0";
    }
    protected void chkShowAllProviders_CheckedChanged(object sender, EventArgs e)
    {


        if (chkShowAllProviders.Checked)
        {
          
            ddlSpecilization.Enabled = false;
        }
        else
            ddlSpecilization.Enabled = true;

        ddlSpecilization_SelectedIndexChanged(this, null);

        ddlSpecilization.SelectedIndex = 0;
        RadLstDoctor.SelectedIndex = 0;
        btnRefresh_OnClick(null, null);

    }
    private void bindAppointmentSummary()
    {
        DataSet ds = new DataSet();
        StringBuilder strXmlFacility = new StringBuilder();
        StringBuilder strXmlDoctor = new StringBuilder();
        ArrayList coll = new ArrayList();
        BaseC.Appointment objapp = new BaseC.Appointment(sConString);
        try
        {
            if (common.myInt(RadLstDoctor.SelectedValue) > 0)
            {
                DateTime fDate = common.myDate(dtpDate.SelectedDate);
                DateTime tDate = common.myDate(dtpDate.SelectedDate);

                switch (common.myStr(ViewState["CurrentView"]))
                {
                    case "W":
                        int dow = (int)fDate.DayOfWeek;

                        fDate = fDate.AddDays(0 - dow);
                        tDate = tDate.AddDays(6 - dow);

                        break;
                    case "M":
                        int dom = (int)fDate.Day;

                        fDate = fDate.AddDays(1 - dom);
                        tDate = fDate.AddMonths(1).AddDays(-1);

                        break;
                }

                if (common.myInt(ddlFacility.SelectedValue) == 0)
                {
                    foreach (RadComboBoxItem currentItem in ddlFacility.Items)
                    {
                        if (common.myInt(currentItem.Value) > 0)
                        {
                            coll.Add(currentItem.Value);
                            strXmlFacility.Append(common.setXmlTable(ref coll));
                        }
                    }

                    if (common.myInt(RadLstDoctor.SelectedValue) > 0)
                    {
                        coll.Add(RadLstDoctor.SelectedValue);
                        strXmlDoctor.Append(common.setXmlTable(ref coll));

                    }
                }
                else
                {
                    coll.Add(ddlFacility.SelectedValue);
                    strXmlFacility.Append(common.setXmlTable(ref coll));
                    if (chkShowAllProviders.Checked == false)
                    {

                        foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
                        {
                            if (currentItem.Checked)
                            {
                                if (common.myInt(currentItem.Value) > 0)
                                {
                                    coll.Add(currentItem.Value);
                                    strXmlDoctor.Append(common.setXmlTable(ref coll));
                                }
                            }
                        }
                    }

                    else
                    {
                        coll.Add(RadLstDoctor.SelectedValue);
                        strXmlDoctor.Append(common.setXmlTable(ref coll));
                    }

                }
                ds = objapp.getAppointmentSummary(common.myInt(Session["HospitalLocationId"]),
                                    strXmlFacility.ToString(), strXmlDoctor.ToString(),
                                    fDate.ToString("yyyy/MM/dd"), tDate.ToString("yyyy/MM/dd"));

                DataTable tbl = getAppSummaryTable();

                if (ds.Tables[0].Rows.Count > 0)
                {

                    TableRow rowNew = new TableRow();
                    tblSummary.Controls.Add(rowNew);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TableCell cellNew = new TableCell();
                        LinkButton lnkStatus = new LinkButton();

                        lnkStatus.ID = "lnkStatus" + common.myStr(dr["StatusId"]) + Guid.NewGuid().ToString("N");
                        lnkStatus.Text = common.myStr(dr["StatusCount"]);

                        lnkStatus.Font.Bold = true;
                        lnkStatus.Attributes.Add("OnClick", "Status_OnClick()");
                        cellNew.BackColor = System.Drawing.Color.FromName(common.myStr(dr["StatusColor"]));

                        cellNew.Controls.Add(lnkStatus);
                        rowNew.Controls.Add(cellNew);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        } //modified by sikandar for code optimize
        finally
        {
            objapp = null;
            dl = null;
            ds.Dispose();
        }

    }
    private DataTable getAppSummaryTable()
    {
        DataTable tbl = new DataTable();
        DataColumn cln = new DataColumn("AppointmentSummary");
        tbl.Columns.Add(cln);

        cln = new DataColumn("StatusId");
        tbl.Columns.Add(cln);

        cln = new DataColumn("StatusColor");
        tbl.Columns.Add(cln);

        return tbl;
    }
    protected void gvAppointmentSummary_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnStatusColor = (HiddenField)e.Row.FindControl("hdnStatusColor");
                e.Row.BackColor = System.Drawing.Color.White;

                if (common.myStr(hdnStatusColor.Value).Length > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName(common.myStr(hdnStatusColor.Value));
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void Status_OnClick(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string sId = lnk.ID.ToString().Replace("lnkStatus", "");
        StringBuilder strFacilityIds = new StringBuilder();
        StringBuilder strDoctorIds = new StringBuilder();

        DateTime fDate = common.myDate(dtpDate.SelectedDate);
        DateTime tDate = common.myDate(dtpDate.SelectedDate);

        switch (common.myStr(ViewState["CurrentView"]))
        {
            case "W":
                int dow = (int)fDate.DayOfWeek;

                fDate = fDate.AddDays(0 - dow);
                tDate = tDate.AddDays(6 - dow);

                break;
            case "M":
                int dom = (int)fDate.Day;

                fDate = fDate.AddDays(1 - dom);
                tDate = fDate.AddMonths(1).AddDays(-1);

                break;
        }

        if (common.myInt(ddlFacility.SelectedValue) == 0)
        {
            foreach (RadComboBoxItem currentItem in ddlFacility.Items)
            {
                if (common.myInt(currentItem.Value) > 0)
                {
                    if (strFacilityIds.ToString() != "")
                    {
                        strFacilityIds.Append(",");
                    }

                    strFacilityIds.Append(common.myStr(currentItem.Value));
                }
            }

            if (common.myInt(RadLstDoctor.SelectedValue) > 0)
            {
                strDoctorIds.Append(common.myStr(RadLstDoctor.SelectedValue));
            }
        }
        else
        {
            strFacilityIds.Append(common.myStr(ddlFacility.SelectedValue));

            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked)
                {
                    if (common.myInt(currentItem.Value) > 0)
                    {
                        if (strDoctorIds.ToString() != "")
                        {
                            strDoctorIds.Append(",");
                        }
                        strDoctorIds.Append(common.myStr(currentItem.Value));
                    }
                }
            }
        }
        RadWindowForNew.NavigateUrl = "/Appointment/AppointmentDetailsList.aspx?MPG=P22218&StatusId=" + common.myInt(sId) +
                                    "&fDate=" + common.myDate(fDate).ToString("yyyy/MM/dd") +
                                    "&tDate=" + common.myDate(tDate).ToString("yyyy/MM/dd") +
                                    "&DoctorIds=" + strDoctorIds.ToString() +
                                    "&FacilityIds=" + strFacilityIds.ToString();

        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.Title = "";
        RadWindowForNew.OnClientClose = "";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }
    private void BindContextMenu1()
    {
        CreateDataTableForRadContextMenu1();
        SchedulerTimeSlotContextMenu.DataSource = (DataTable)ViewState["dtable"];
        SchedulerTimeSlotContextMenu.DataTextField = "Text";
        SchedulerTimeSlotContextMenu.DataValueField = "Value";
        SchedulerTimeSlotContextMenu.DataBind();
    }
    protected void CreateDataTableForRadContextMenu1()
    {
        UserAuthorisations ua = new UserAuthorisations();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            DataRow dr;
            if (ua.CheckPermissions("N", "/Appointment/AppointmentDetails.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "New Appointment";
                dr["Value"] = "New";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }
            if (ua.CheckPermissions("N", "/Appointment/EditBreakAndBlock.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Add Break/Block";
                dr["Value"] = "Break";
                dt.Rows.Add(dr);
            }
            dr = dt.NewRow();
            dr["Text"] = "Paste";
            dr["Value"] = "Paste";
            dt.Rows.Add(dr);
            ViewState["dtable"] = dt;

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ua.Dispose();
        }
    }
    private void BindContextMenu2()
    {
        CreateDataTableForRadContextMenu2();
        SchedulerAppointmentContextMenu.DataSource = (DataTable)ViewState["dtable1"];
        SchedulerAppointmentContextMenu.DataTextField = "Text";
        SchedulerAppointmentContextMenu.DataValueField = "Value";
        SchedulerAppointmentContextMenu.DataBind();
    }
    protected void CreateDataTableForRadContextMenu2()
    {
        UserAuthorisations ua = new UserAuthorisations();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            DataRow dr;
            if (ua.CheckPermissions("P", "/Appointment/AppointmentDetails.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Print Appointment";
                dr["Value"] = "Print";
                dt.Rows.Add(dr);
            }
            dr = dt.NewRow();
            dr["Text"] = "Copy";
            dr["Value"] = "Copy";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Text"] = "Cut";
            dr["Value"] = "Cut";
            dt.Rows.Add(dr);

            if (ua.CheckPermissions("E", "/Appointment/AppointmentDetails.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Edit";
                dr["Value"] = "Edit";
                dt.Rows.Add(dr);
            }
            if (ua.CheckPermissions("C", "/Appointment/AppointmentDetails.aspx", true))
            {

                dr = dt.NewRow();
                dr["Text"] = "Cancel Chk-In";
                dr["Value"] = "DelChkIn";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Text"] = "Cancel Appointment";
                dr["Value"] = "Delete";
                dt.Rows.Add(dr);
            }
            dr = dt.NewRow();
            dr["Text"] = "Patient Details";
            dr["Value"] = "PatientDetails";
            dt.Rows.Add(dr);

            Statusdt = new DataSet();
            Statusdt = GetAppointmentStatus();
            if (Statusdt.Tables[0].Rows.Count > 0)
            {
                for (int counter = 0; counter <= Statusdt.Tables[0].Rows.Count - 1; counter++)
                {
                    if (Statusdt.Tables[0].Rows[counter]["Status"].ToString().Equals("Cancel"))
                    {
                        if (ua.CheckPermissions("C", "/Appointment/AppointmentDetails.aspx", true))
                        {
                            dr = dt.NewRow();
                            dr["Text"] = Statusdt.Tables[0].Rows[counter]["Status"].ToString();
                            dr["Value"] = Statusdt.Tables[0].Rows[counter]["Code"].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr["Text"] = Statusdt.Tables[0].Rows[counter]["Status"].ToString();
                        dr["Value"] = Statusdt.Tables[0].Rows[counter]["Code"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
            }
            ViewState["dtable1"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ua.Dispose();
        }
    }
    private void BindContextMenu3()
    {
        CreateDataTableForBreakContextMenu();
        BreakContextMenu.DataSource = (DataTable)ViewState["dtable2"];
        BreakContextMenu.DataTextField = "Text";
        BreakContextMenu.DataValueField = "Value";
        BreakContextMenu.DataBind();
    }
    protected void CreateDataTableForBreakContextMenu()
    {
        UserAuthorisations ua = new UserAuthorisations();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            DataRow dr;
            if (ua.CheckPermissions("E", "/Appointment/EditBreakAndBlock.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Edit";
                dr["Value"] = "Edit";
                dt.Rows.Add(dr);
            }
            if (ua.CheckPermissions("C", "/Appointment/EditBreakAndBlock.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Cancel";
                dr["Value"] = "Delete";
                dt.Rows.Add(dr);
            }
            ViewState["dtable2"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ua.Dispose();
        }
    }
    protected void lnkOnlineAppointment_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/Appointment/OnlineAppointmentRequest.aspx?MPG=P22226&doctorId=0&useFor=Display";
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        //   RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";// "OnClientClose";
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Default;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;

    }
    protected void lnkSearchAppointment_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/Appointment/SearchAppointment.aspx?MPG=P22227";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1150;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.Title = "";
        RadWindowForNew.OnClientClose = "";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }
    private void BindGrpTagSchedulerTimeSlotContextMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {            //1 for Registration module, 2 for Appointment Module, 3 for EMR, 30 for Ward Module
            if (common.myInt(Session["ModuleId"]).Equals(1) || common.myInt(Session["ModuleId"]).Equals(2))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "Blank_Appointment");
            }
            else if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "Blank_FollowupApp");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            else
            {
                SchedulerTimeSlotContextMenu.DataSource = ds.Tables[0];
                SchedulerTimeSlotContextMenu.DataTextField = "OptionName";
                SchedulerTimeSlotContextMenu.DataValueField = "OptionCode";
                SchedulerTimeSlotContextMenu.DataBind();
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }
    private void BindGrpTagSchedulerAppointmentContextMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            //1 for Registration module, 2 for Appointment Module, 3 for EMR, 30 for Ward Module
            if (common.myInt(Session["ModuleId"]).Equals(1) || common.myInt(Session["ModuleId"]).Equals(2))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "Booked_Appointment");
            }
            else if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                   common.myInt(Session["ModuleId"]), "Booked_FollowupApp");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            else
            {
                SchedulerAppointmentContextMenu.DataSource = ds.Tables[0];
                SchedulerAppointmentContextMenu.DataTextField = "OptionName";
                SchedulerAppointmentContextMenu.DataValueField = "OptionCode";
                SchedulerAppointmentContextMenu.DataBind();
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }
    private void BindGrpTagBreakContextMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            //1 for Registration module, 2 for Appointment Module, 3 for EMR, 30 for Ward Module
            if (common.myInt(Session["ModuleId"]).Equals(1) || common.myInt(Session["ModuleId"]).Equals(2))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                    common.myInt(Session["ModuleId"]), "Appointment_Break");
            }
            else if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30))
            {
                ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                common.myInt(Session["ModuleId"]), "FollowupApp_Break");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            else
            {
                BreakContextMenu.DataSource = ds.Tables[0];
                BreakContextMenu.DataTextField = "OptionName";
                BreakContextMenu.DataValueField = "OptionCode";
                BreakContextMenu.DataBind();
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }
    protected void lnkAppointment_OnClick(object sender, EventArgs e)
    {

        string strProvider = "";
        string Status = "OldAp";
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        CheckBox checkbox = new CheckBox();
        if (chkShowAllProviders.Checked == false)
        {
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked == true)
                {
                    strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                }
            }
        }
        else
        {
            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

        }
        if (strProvider == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select provider !";
            return;
        }
        else
        {
            Session["AppDoctor"] = common.myStr(strProvider);
        }
        if (common.myStr(ViewState["CurrentView"]) == "")
        {
            ViewState["CurrentView"] = "D";
        }
        RadWindowForNew.NavigateUrl = "~/Appointment/PatientDetailsV1.aspx?MPG=P22212&Date=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&Provider=" + common.myStr(strProvider) + "&DateOption=" + common.myStr(ViewState["CurrentView"]) + "&Status=" + Status;
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnkNewPatient_OnClick(object sender, EventArgs e)
    {

        string strProvider = "";
        string Status = "NewAp";
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        CheckBox checkbox = new CheckBox();
        if (chkShowAllProviders.Checked == false)
        {
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked == true)
                {
                    strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                }
            }
        }
        else
        {
            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

        }
        if (strProvider == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select provider !";
            return;
        }
        else
        {
            Session["AppDoctor"] = common.myStr(strProvider);
        }
        if (common.myStr(ViewState["CurrentView"]) == "")
        {
            ViewState["CurrentView"] = "D";
        }
        RadWindowForNew.NavigateUrl = "~/Appointment/PatientDetailsV1.aspx?MPG=P22212&Date=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&Provider=" + common.myStr(strProvider) + "&DateOption=" + common.myStr(ViewState["CurrentView"]) + "&Status=" + Status;
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnkConfirmed_OnClick(object sender, EventArgs e)
    {
        string strProvider = "";
        string Status = "Confm";
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        CheckBox checkbox = new CheckBox();
        if (chkShowAllProviders.Checked == false)
        {
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked == true)
                {
                    strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                }
            }
        }
        else
        {
            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

        }
        if (strProvider == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select provider !";
            return;
        }
        else
        {
            Session["AppDoctor"] = common.myStr(strProvider);
        }
        if (common.myStr(ViewState["CurrentView"]) == "")
        {
            ViewState["CurrentView"] = "D";
        }
        RadWindowForNew.NavigateUrl = "~/Appointment/PatientDetailsV1.aspx?MPG=P22212&Date=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&DateOption=" + common.myStr(ViewState["CurrentView"]) + "&Status=" + Status;
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnlCheckIn_OnClick(object sender, EventArgs e)
    {
        string strProvider = "";
        string Status = "ChkIn";
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        //foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
        //{
        //    if (currentItem.Checked == true)
        //    {
        //        strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
        //    }
        //}
        CheckBox checkbox = new CheckBox();
        if (chkShowAllProviders.Checked == false)
        {
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked == true)
                {
                    strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                }
            }
        }
        else
        {
            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

        }
        if (strProvider == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select provider !";
            return;
        }
        else
        {
            Session["AppDoctor"] = common.myStr(strProvider);
        }
        if (common.myStr(ViewState["CurrentView"]) == "")
        {
            ViewState["CurrentView"] = "D";
        }
        RadWindowForNew.NavigateUrl = "~/Appointment/PatientDetailsV1.aspx?MPG=P22212&Date=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&DateOption=" + common.myStr(ViewState["CurrentView"]) + "&Status=" + Status;
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnkCancelled_OnClick(object sender, EventArgs e)
    {
        string strProvider = "";
        string Status = "Cancl";
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        CheckBox checkbox = new CheckBox();
        if (chkShowAllProviders.Checked == false)
        {
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked == true)
                {
                    strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                }
            }
        }
        else
        {
            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

        }
        if (strProvider == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select provider !";
            return;
        }
        else
        {
            Session["AppDoctor"] = common.myStr(strProvider);
        }
        if (common.myStr(ViewState["CurrentView"]) == "")
        {
            ViewState["CurrentView"] = "D";
        }
        RadWindowForNew.NavigateUrl = "~/Appointment/PatientDetailsV1.aspx?MPG=P22212&Date=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&Provider=" + common.myStr(strProvider) + "&DateOption=" + common.myStr(ViewState["CurrentView"]) + "&Status=" + Status;

        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnkPayment_OnClick(object sender, EventArgs e)
    {
        string strProvider = "";
        string Status = "Paym";
        dtpDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
        //foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
        //{
        //    if (currentItem.Checked == true)
        //    {
        //        strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
        //    }
        //}

        CheckBox checkbox = new CheckBox();
        if (chkShowAllProviders.Checked == false)
        {
            foreach (RadComboBoxItem currentItem in RadLstDoctor.Items)
            {
                if (currentItem.Checked == true)
                {
                    strProvider = strProvider + "<Table1><c1>" + currentItem.Value + "</c1></Table1>";
                }
            }
        }
        else
        {
            strProvider = strProvider + "<Table1><c1>" + RadLstDoctor.SelectedValue + "</c1></Table1>";

        }
        if (strProvider == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select provider !";
            return;
        }
        else
        {
            Session["AppDoctor"] = common.myStr(strProvider);
        }
        if (common.myStr(ViewState["CurrentView"]) == "")
        {
            ViewState["CurrentView"] = "D";
        }
        RadWindowForNew.NavigateUrl = "~/Appointment/PatientDetailsV1.aspx?MPG=P22212&Date=" + dtpDate.SelectedDate.ToString("yyyy/MM/dd") + "&DateOption=" + common.myStr(ViewState["CurrentView"]) + "&Status=" + Status;
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientCloseNextSlot";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void dtpDate_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime pastday = e.Day.Date;
        DateTime date = DateTime.Now;
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;
        DateTime today = new DateTime(year, month, day);
        if (pastday.CompareTo(today) < 0)
        {
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
            e.Day.IsSelectable = false;
        }
    }

}
