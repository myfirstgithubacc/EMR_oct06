using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
public partial class Appointment_ViewNextTimeSlot : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;

    #region
    private Int32 RegId
    {
        get
        {
            return (ViewState["RegId"] == null ? 0 : Convert.ToInt32(ViewState["RegId"]));
        }
        set
        {
            ViewState["RegId"] = value;
        }
    }


    #endregion

    private void PopulateFacility()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intUserId", Session["UserID"]);
            HashIn.Add("@intGroupId", Session["GroupID"]);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspgetfacilitylist", HashIn);
            ddlFacility.DataSource = dr;
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlFacility_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        PopulateDoctor();
    }

    private void PopulateDoctor()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@HospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intfacilityId", ddlFacility.SelectedValue);

            DataSet dt = dl.FillDataSet(CommandType.StoredProcedure, "uspgetdoctorlist", HashIn);
            if (Request.QueryString["Mpg"] == "P781")
            {
                DataView dv = new DataView();
                dv = new DataView(dt.Tables[0]);
                dv.RowFilter = "DoctorId=" + Session["DoctorId"].ToString();
                RadLstDoctor.DataSource = dv.ToTable();
                RadLstDoctor.DataTextField = "DoctorName";
                RadLstDoctor.DataValueField = "DoctorId";
                RadLstDoctor.DataBind();
                foreach (RadListBoxItem item in RadLstDoctor.Items)
                {
                    item.Checked = true;
                }
            }
            else
            {
                RadLstDoctor.DataSource = dt.Tables[0];
                RadLstDoctor.DataTextField = "DoctorName";
                RadLstDoctor.DataValueField = "DoctorId";
                RadLstDoctor.DataBind();
            }

        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankTiemSlot()
    {
        try
        {
            DataTable datatable = CreateDataTable();
            DataRow datarow = datatable.NewRow();
            DataRow datarow1 = datatable.NewRow();
            datatable.Rows.Add(datarow);
            datatable.Rows.Add(datarow1);
            ViewState["BlankGrid"] = "True";
            gvTimeSlot.DataSource = datatable;
            gvTimeSlot.DataBind();
            ViewState["BlankGrid"] = null;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataTable CreateDataTable()
    {
        DataTable datatable = new DataTable();
        datatable.Columns.Add("DoctorId");
        datatable.Columns.Add("FacilityId");
        datatable.Columns.Add("DoctorName");
        datatable.Columns.Add("Facility");
        datatable.Columns.Add("AppDate");
        datatable.Columns.Add("AppTime");
        datatable.Columns.Add("DayName");
        return datatable;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["RegId"] == null)
            {
                trTop.Visible = false;

            }
            else
            {
                trTop.Visible = true;
            }

            Cache.Remove("AvailableSlot");
            ucPatientQView.Visible = false;
            if (Request.QueryString["RegId"] != null)
            {
                ucPatientQView.Visible = true;
                RegId = Convert.ToInt32(Request.QueryString["RegId"]);
                ucPatientQView.ShowPatientDetails(RegId);
            }
            PopulateFacility();
            ddlFacility.SelectedValue = Session["FacilityId"].ToString();
            PopulateDoctor();
            BindBlankTiemSlot();
            rblDatePeriod_OnSelectedIndexChanged(this, null);
            //////  populateVisitType();
            populateVisitType(false, 0);

            //fromDate.DateInput.DateFormat = Application["InputDateFormat"].ToString();
            //Added by vineet for selecting UTC TimeZone
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intfacilityid", Convert.ToInt32(Session["FacilityID"]));
            string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"].ToString());

                ViewState["SELECTED_DATE"] = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddMinutes(timezone);
                fromDate.SelectedDate = (DateTime)ViewState["SELECTED_DATE"];
            }
            else
            {

                ViewState["SELECTED_DATE"] = common.myDate(DateTime.Now);
                Alert.ShowAjaxMsg("Please Enter Offset Time in Facilitymaster Table in Database", Page);
                return;
            }
            ////////////////////////////////////////////////////////////////////
        }
    }

    protected void btnFindNext_Click(object sender, EventArgs e)
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            StringBuilder sb = new StringBuilder();
            StringBuilder strweekdays = new StringBuilder();

            double periodnoofdays = 1;
            if (RadLstDoctor.CheckedItems.Count == 0)
            {
                Alert.ShowAjaxMsg("Select Doctor.", Page);
                return;
            }
            foreach (RadComboBoxItem item in RadLstDoctor.CheckedItems)
            {
                sb.Append("<Table1><c1>");
                sb.Append(item.Value);
                sb.Append("</c1></Table1>");
            }

            foreach (ListItem citem in cblWeekday.Items)
            {
                if (citem.Selected == true)
                {
                    strweekdays.Append("<Table1><c1>");
                    strweekdays.Append(citem.Value);
                    strweekdays.Append("</c1></Table1>");

                }
            }
            if (strweekdays.Length == 0)
            {
                Alert.ShowAjaxMsg("Select Day(s).", Page);
                return;
            }

            if (ddlAppointmentType.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Select Appointment Type.", Page);
                return;
            }

            //Changed to UTC by vineet
            //DateTime dtCurrentDate = DateTime.Now.Date;

            String strStartDateTime = "";

            if (Convert.ToString(rblDatePeriod.SelectedValue) == "D")
            {
                if (fromDate.SelectedDate != null)
                {
                    strStartDateTime = fromDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Date From the Calender", Page);
                    return;
                }
            }
            else
            {
                if (txtPeriodTime.Text.Length > 0)
                {
                    if (ddlPeriodTime.SelectedValue == "D")
                    {
                        periodnoofdays = Convert.ToInt32(txtPeriodTime.Text);
                    }
                    else if (ddlPeriodTime.SelectedValue == "W")
                    {
                        periodnoofdays = Convert.ToInt32(txtPeriodTime.Text) * 7;
                    }
                    else if (ddlPeriodTime.SelectedValue == "M")
                    {
                        periodnoofdays = Convert.ToInt32(txtPeriodTime.Text) * 30;
                    }
                    else if (ddlPeriodTime.SelectedValue == "Y")
                    {
                        periodnoofdays = Convert.ToInt32(txtPeriodTime.Text) * 365;
                    }
                }

                DateTime dtCurrentDate = fromDate.SelectedDate.Value.AddDays(periodnoofdays);

                strStartDateTime = dtCurrentDate.ToString("dd/MM/yyyy");

                //dtStartDateTime
            }

            HashIn.Add("@xmlDoctorIDs", sb.ToString());
            HashIn.Add("@intFacilityId", ddlFacility.SelectedValue);
            HashIn.Add("@intVisitTypeId", ddlAppointmentType.SelectedValue);
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@chrStartDateTime", strStartDateTime);
            HashIn.Add("@bitAMOnly", chkAm.Checked);
            HashIn.Add("@bitPMOnly", chkPm.Checked);
            HashIn.Add("@xmlWeekDays", strweekdays.ToString());
            HashIn.Add("@inySlots", Radnoofslots.Text);
            //HashIn.Add("@inyMonths", ddlTimeFrame.SelectedValue);

            //HashIn.Add("@xmlDoctorIDs", "<Table1><c1>1</c1></Table1><Table1><c1>2</c1></Table1>");
            //HashIn.Add("@intFacilityId", 1);
            //HashIn.Add("@inyHospitalLocationId", 1);
            //HashIn.Add("@inyMonths", 1);
            DataSet dt = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorNextAvailableSlot", HashIn);
            if (dt.Tables[0].Rows.Count > 0)
            {
                gvTimeSlot.DataSource = dt;
                gvTimeSlot.DataBind();
                //Cache.Insert("AvailableSlot", dt, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                //Cache.Remove("AvailableSlot");
                BindBlankTiemSlot();
                Alert.ShowAjaxMsg("No Working time for this Doctor", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void gvTimeSlot_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            bool IsDoctor = false;

            string strShiftFirstFrom = "", strShiftFirstTo = "", strTimeTaken = "";

            string facilityId = gvTimeSlot.DataKeys[e.NewSelectedIndex].Values[1].ToString();
            string doctorid = gvTimeSlot.DataKeys[e.NewSelectedIndex].Values[0].ToString();

            string appdate = gvTimeSlot.Rows[e.NewSelectedIndex].Cells[2].Text;
            string apptime = gvTimeSlot.Rows[e.NewSelectedIndex].Cells[3].Text;
            string[] arrapptime = apptime.Split(':');

            DateTime date = common.myDate(appdate);

            DataSet ds = GetTime(doctorid, date.ToString("yyyy/MM/dd"), facilityId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strShiftFirstFrom = ds.Tables[0].Rows[0]["ShiftFirstFrom"].ToString();

                strShiftFirstTo = ds.Tables[0].Rows[0]["ShiftFirstTo"].ToString();
                strTimeTaken = ds.Tables[0].Rows[0]["TimeTaken"].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                IsDoctor = Convert.ToBoolean(ds.Tables[2].Rows[0]["IsDoctor"]);
            }
            string[] FirstFrom = strShiftFirstFrom.Split(':');
            string[] FirstTo = strShiftFirstTo.Split(':');

            //Appointment/AppointmentDetails.aspx?FacilityId=" + ddlFacility + "&StTime=" + e.TimeSlot.Start.Hour + "&EndTime=" + RadScheduler1.DayEndTime + "&appDate=" + e.TimeSlot + "&appid=0&doctorId=" + e.Time + "&TimeInterval=" + RadScheduler1 + "&FromTimeHour=" + e.TimeSlot + "&FromTimeMin=" + e.TimeSlot. + "&PageId=" + Request.QueryString["Mpg"];

            RadWindowForNew.NavigateUrl = "~/Appointment/AppointmentDetails.aspx?FacilityId=" + facilityId + "&StTime=" + FirstFrom[0].ToString() + "&EndTime=" + FirstTo[0].ToString() + "&appDate=" + date.ToString("MM/dd/yyyy") + "&appid=0&doctorId=" + doctorid + "&TimeInterval=" + strTimeTaken + "&FromTimeHour=" + arrapptime[0] + "&FromTimeMin=" + arrapptime[1] + "&RegId=" + RegId + "&recrule=" + null + "&IsDoctor=" + IsDoctor + "";
            RadWindowForNew.Height = 520;
            RadWindowForNew.Width = 700;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataSet GetTime(string strDoctorId, string strForDate, string strFacilityId)
    {
        DataSet dsGetTime = new DataSet();
        try
        {
            BaseC.Patient formatdate = new BaseC.Patient(sConString);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder objXML = new StringBuilder();
            DL_Funs dlFun = new DL_Funs();
            string strForDated = formatdate.FormatDateMDY(strForDate);
            DateTime date = common.myDate(strForDated);
            objXML.Append("<Table1><c1>");
            objXML.Append(strDoctorId);
            objXML.Append("</c1></Table1>");
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intFacilityId", strFacilityId);
            hshInput.Add("@xmlDoctorIds", objXML.ToString());
            hshInput.Add("@chrForDate", date.ToString("yyyy/MM/dd"));

            dsGetTime = dl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointments", hshInput);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dsGetTime;
    }

    protected void gvTimeSlot_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTimeSlot.PageIndex = e.NewPageIndex;
        //gvTimeSlot.DataSource = (DataSet )Cache["AvailableSlot"];
        //gvTimeSlot.DataBind();
        btnFindNext_Click(sender, e);
    }

    protected void gvTimeSlot_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)//e.Row.RowType == DataControlRowType.DataRow)// || e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["BlankGrid"] != null && ViewState["BlankGrid"].ToString() == "True")
            {
                e.Row.Cells[5].Visible = false;
            }
        }
    }



    private void populateVisitType(bool bResult, int iPlanTypeId)
    {
        try
        {

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ddlAppointmentType.Items.Clear();
            if (bResult == false)
            {
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@iHositalLocationID", Session["HospitalLocationID"]);
                hshIn.Add("@iFacilityId", Convert.ToInt16(Request.QueryString["FacilityId"]));
                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRVisitTypeWithCondition", hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlAppointmentType.DataSource = ds.Tables[0];
                    ddlAppointmentType.DataValueField = "visitTypeId";
                    ddlAppointmentType.DataTextField = "Type";
                    ddlAppointmentType.DataBind();

                }
            }
            else
            {
                BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                DataTable dt = appoint.CheckServiceCoverForClient(Convert.ToInt16(Request.QueryString["FacilityId"]), iPlanTypeId);
                if (dt.Rows.Count > 0)
                {
                    ddlAppointmentType.DataSource = ds.Tables[0];
                    ddlAppointmentType.DataValueField = "visitTypeId";
                    ddlAppointmentType.DataTextField = "Type";
                    ddlAppointmentType.DataBind();

                }

            }

        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void populateVisitType()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(2000);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@HospID", Session["HospitalLocationID"]);
            hshIn.Add("@Active", "1");

            str.Append(" select visitTypeId,Type from emrvisittype where hospitallocationid=@HospID AND Active=@Active");
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, str.ToString(), hshIn);
            if (dr.HasRows == true)
            {
                ddlAppointmentType.DataSource = dr;
                ddlAppointmentType.DataValueField = "visitTypeId";
                ddlAppointmentType.DataTextField = "Type";
                ddlAppointmentType.DataBind();
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void rblDatePeriod_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(rblDatePeriod.SelectedValue) == "P")
        {
            trPeriod.Visible = true;
            trDate.Visible = false;
        }
        else
        {
            trPeriod.Visible = false;
            trDate.Visible = true;
        }
    }

    protected void lnkViewNextSlot_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewNextTimeSlot.aspx?RegId=" + Request.QueryString["RegId"], false);
    }

    protected void lnkRepeatAppointment_Click(object sender, EventArgs e)
    {
        try
        {
            string strShiftFirstFrom = "", strShiftFirstTo = "", strTimeTaken = "";
            string registrationID = Session["RegistrationID"].ToString();
            string doctorID = Session["DoctorID"].ToString();
            string facilityID = Session["FacilityID"].ToString();
            string appointmentDate = Session["AppointmentDate"].ToString();
            DataSet ds = GetTime(Session["DoctorID"].ToString(), Session["AppointmentDate"].ToString(), Session["FacilityID"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                strShiftFirstFrom = ds.Tables[0].Rows[0]["ShiftFirstFrom"].ToString();

                strShiftFirstTo = ds.Tables[0].Rows[0]["ShiftFirstTo"].ToString();
                strTimeTaken = ds.Tables[0].Rows[0]["TimeTaken"].ToString();
            }
            string[] FirstFrom = strShiftFirstFrom.Split(':');
            string[] FirstTo = strShiftFirstTo.Split(':');
            Response.Redirect("RepeatAppointment.aspx?RegId=" + Request.QueryString["RegId"] + "&StTime=" + FirstFrom[0].ToString() + "&EndTime=" + FirstTo[0].ToString() + "&TimeInterval=" + strTimeTaken, false);
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
