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
using System.Data.SqlClient;
using BaseC;

public partial class EMR_Dashboard_ProviderParts_Appointmentl : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DL_Funs fun = new DL_Funs();
    Hashtable hsTb = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        dtpfromDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
        dtpToDate.DateInput.DateFormat = Session["OutputDateformat"].ToString();
        int Utctime = BindUTCTime();
        btnFilter_Click(sender, e);
        tdDateRange.Visible = false;
    }

    public void BindGVAppointment(Int32 EmpId)
    {
        try
        {
            if (txtApp.Text != "" && txtApp.Text != "0")
            {
                string[] str = getToFromDate();
                string sFromDate = str[0];
                string sToDate = str[1];

                BaseC.Dashboard dsh = new BaseC.Dashboard();
                DataSet ds = new DataSet();
                GVAppointment.DataSource = dsh.getPDAppointment(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(txtApp.Text.ToString()), ddlTime.SelectedValue, sFromDate, sToDate, Convert.ToInt32(Session["FacilityID"]));
                GVAppointment.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        if (ddlTime.SelectedValue == "4")
        {
            if (dtpfromDate.SelectedDate == null || dtpToDate.SelectedDate == null)
            {
                lblMessage.Text = "<b>Please Enter Date </b>";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }
        BindGVAppointment(Convert.ToInt32(txtApp.Text.ToString()));
    }

    private string[] datecalculate()
    {
        string DayName = DateTime.Now.DayOfWeek.ToString();
        string fromdate = "";
        string todate = "";

        string[] str = new string[2];

        switch (DayName)
        {
            case "Sunday":
                fromdate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Monday":
                //fromdate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 00:00";
                //todate = DateTime.Now.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";

                fromdate = DateTime.Now.AddDays(-1).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(5).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Tuesday":
                fromdate = DateTime.Now.AddDays(-2).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(4).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Wednesday":
                fromdate = DateTime.Now.AddDays(-3).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(3).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Thursday":
                fromdate = DateTime.Now.AddDays(-4).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(2).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Friday":
                fromdate = DateTime.Now.AddDays(-5).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Saturday":
                fromdate = DateTime.Now.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
                break;
            //case "Sunday":

            //    fromdate = DateTime.Now.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
            //    todate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
            //    break;
        }

        str[0] = fromdate;
        str[1] = todate;
        return str;
    }

    private string[] getToFromDate()
    {
        int timezone = BindUTCTime();
        string sFromDate = "", sToDate = "";
        string[] str = new string[2];

        if (ddlTime.SelectedValue == "4")
        {
            tdDateRange.Visible = true;
            if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                sFromDate = Convert.ToDateTime(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd") + " 00:00";
                sToDate = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd") + " 23:59";
            }
            str[0] = sFromDate;
            str[1] = sToDate;
        }
        else if (ddlTime.SelectedValue == "WW0")
        {
            str = datecalculate();
        }
        else if (ddlTime.SelectedValue == "WW+1")
        {
            DateTime dtto = new DateTime();
            dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
            dtto = dtto.AddDays(7);
            dtpToDate.SelectedDate = dtto;
            string NextWeekdayname = dtpToDate.SelectedDate.Value.DayOfWeek.ToString();
            string fromdate = "";
            string todate = "";
            switch (NextWeekdayname)
            {
                case "Sunday":
                    fromdate = dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";
                    break;
                case "Monday":
                    fromdate = dtpToDate.SelectedDate.Value.AddDays(-1).Date.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.AddDays(5).ToString("yyyy/MM/dd") + " 23:59";
                    break;
                case "Tuesday":
                    fromdate = dtpToDate.SelectedDate.Value.AddDays(-2).Date.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.AddDays(4).ToString("yyyy/MM/dd") + " 23:59";
                    break;
                case "Wednesday":
                    fromdate = dtpToDate.SelectedDate.Value.AddDays(-3).Date.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.AddDays(3).ToString("yyyy/MM/dd") + " 23:59";
                    break;
                case "Thursday":
                    fromdate = dtpToDate.SelectedDate.Value.AddDays(-4).Date.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.AddDays(2).ToString("yyyy/MM/dd") + " 23:59";
                    break;
                case "Friday":
                    fromdate = dtpToDate.SelectedDate.Value.AddDays(-5).Date.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.AddDays(1).ToString("yyyy/MM/dd") + " 23:59";
                    break;
                case "Saturday":
                    fromdate = dtpToDate.SelectedDate.Value.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
                    todate = dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd") + " 23:59";
                    break;
            }
            str[0] = fromdate;
            str[1] = todate;
        }
        else if (ddlTime.SelectedValue == "MM0")
        {
            sFromDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01" + " 00:00";
            sToDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " 23:59";

            str[0] = sFromDate;
            str[1] = sToDate;
        }
        //else if (ddlTime.SelectedValue == "DD0")
        //{
        //    sFromDate = DateTime.Today.ToString();
        //    sToDate = DateTime.Today.ToString();
        //    //sFromDate = Convert.ToString(common.myDate(DateTime.Now).AddMinutes(timezone));
        //    //sToDate = Convert.ToString(common.myDate(DateTime.Now).AddMinutes(timezone));
        //    str[0] = sFromDate;
        //    str[1] = sToDate;
        //}
        return str;
    }

    protected void GVAppointment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVAppointment.PageIndex = e.NewPageIndex;
        btnFilter_Click(sender, e);
    }

    protected void ddlTime_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlTime.SelectedValue == "0")
        {
            ViewState["daterange"] = "";
            tdDateRange.Visible = false;

            dtpfromDate.SelectedDate = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 00:00");
            dtpToDate.SelectedDate = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
        }
        else if (ddlTime.SelectedValue == "1")
        {
            ViewState["daterange"] = "";
            tdDateRange.Visible = false;
            dtpfromDate.SelectedDate = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 00:00");
            string dttodate;
            dttodate = System.DateTime.Now.DayOfWeek.ToString();

            if (dttodate == "Monday")
            {
                DateTime dtto = new DateTime();
                dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
                dtto = dtto.AddDays(6);
                dtpToDate.SelectedDate = dtto;
            }
            else if (dttodate == "Tuesday")
            {

                DateTime dtto = new DateTime();
                dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
                dtto = dtto.AddDays(5);
                dtpToDate.SelectedDate = dtto;
            }
            else if (dttodate == "Wednesday")
            {
                DateTime dtto = new DateTime();
                dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
                dtto = dtto.AddDays(4);
                dtpToDate.SelectedDate = dtto;
            }
            else if (dttodate == "Thursday")
            {
                DateTime dtto = new DateTime();
                dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
                dtto = dtto.AddDays(3);
                dtpToDate.SelectedDate = dtto;
            }
            else if (dttodate == "Friday")
            {
                DateTime dtto = new DateTime();
                dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
                dtto = dtto.AddDays(2);
                dtpToDate.SelectedDate = dtto;
            }
            else if (dttodate == "Saturday")
            {
                DateTime dtto = new DateTime();
                dtto = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
                dtto = dtto.AddDays(1);
                dtpToDate.SelectedDate = dtto;
            }
            else
            {
                dtpToDate.SelectedDate = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
            }

        }
        else if (ddlTime.SelectedValue == "2")
        {
            ViewState["daterange"] = "";
            tdDateRange.Visible = false;
            string dttomonth;
            dttomonth = System.DateTime.Now.Month.ToString();

            dtpfromDate.SelectedDate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd") + " 00:00");
            DateTime dttom = new DateTime();
            dttom = Convert.ToDateTime(System.DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59");
            dtpToDate.SelectedDate = LastDate(dttom);
        }

        else if (ddlTime.SelectedValue == "4")
        {
            ViewState["daterange"] = "";
            tdDateRange.Visible = true;
        }
        else
        {
            ViewState["daterange"] = ddlTime.SelectedValue;
            tdDateRange.Visible = false;
        }
    }

    private DateTime LastDate(DateTime dt)
    {
        return dt.AddMonths(1).AddDays(-dt.Day);
    }

    protected void btnUpdateYes_Click(object sender, EventArgs e)
    {
        try
        {
            dvUpdateStatus.Visible = false;
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            string[] strAppId = Session["AppointmentID"].ToString().Split('_');
            DateTime OrigTime = DateTime.Now;
            DateTime OrigDate = DateTime.Today;
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
            string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
            if (OrigRecRule != null && OrigRecRule != " ")
            {
                Label apptDate = (Label)GVAppointment.FindControl("lblApptDate");
                string strApptDate = Session["ApptDate"].ToString();
                Hashtable hshIn = new Hashtable();
                Hashtable hshtableout = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intEncodedBy", Session["UserId"]);
                hshIn.Add("@intStatusId", Convert.ToString("3"));
                hshtableout.Add("@intRecAppointmentId", SqlDbType.Int);
                hshIn.Add("@dtAppointmentDate", Convert.ToDateTime(strApptDate).Date.ToShortDateString());
                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshtableout);
                string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + Convert.ToInt32(hshtableout["intRecAppointmentId"]) + "'";
                string exdate = Convert.ToDateTime(strApptDate).Date.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
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
                string strEncounter = "select ID  from Encounter where AppointmentID=" + hshtableout["@intRecAppointmentId"];
                DataSet dsEncounter = dl.FillDataSet(CommandType.Text, strEncounter);
                if (dsEncounter.Tables[0].Rows.Count > 0)
                {
                    Session["EncounterID"] = dsEncounter.Tables[0].Rows[0]["ID"].ToString();
                }

            }
            else
            {
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@intAppointmentId", strAppId[0]);
                hshIn.Add("@intEncodedBy", Session["UserId"]);
                hshIn.Add("@chrStatusCode", Convert.ToString("P"));
                dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);

                string strEncounter = "select ID  from Encounter where AppointmentID=" + Session["AppointmentID"].ToString();
                DataSet dsEncounter = dl.FillDataSet(CommandType.Text, strEncounter);
                if (dsEncounter.Tables[0].Rows.Count > 0)
                {
                    Session["EncounterID"] = dsEncounter.Tables[0].Rows[0]["ID"].ToString();
                }
                dvUpdateStatus.Visible = false;
            }
            //Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["EncounterID"]), Convert.ToInt16(Session["HospitalLocationID"]));
            //if (intFormId > 0)
            //{
                //Session["formId"] = Convert.ToString(intFormId);
            //}
            Response.Redirect("~/Editor/WordProcessor.aspx?Mpg=P130", false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnUpdateNo_Click(object sender, EventArgs e)
    {
       // dvUpdateStatus.Visible = false;
        //Response.Redirect("/emr/Dashboard/PatientDashboard.aspx?Mpg=P6", false);
    }

    protected void lbtnName_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            string[] strvalue;
            LinkButton lnkPatient1 = sender as LinkButton;

            strvalue = lnkPatient1.CommandArgument.Split('|');

            //Session["ModuleId"] = 3;
            int i = 0;
            DataSet ds = (DataSet)Session["ModuleData"];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (common.myStr(dr["ModuleName"]) == "EHR")
                {
                    Session["ModuleId"] = i;
                }
                i++;
            }
            Session["Gender"] = strvalue[2];
            Session["encounterid"] = strvalue[3];
            Session["Facility"] = strvalue[4];
            Session["DoctorID"] = strvalue[5];
            Session["RegistrationID"] = strvalue[6];
            Session["AppointmentID"] = strvalue[7];
            Session["ApptDate"] = strvalue[8];
            Int32 intFormId = objPatient.GetPatientDefaultFormId(Convert.ToInt32(Session["encounterid"]), Convert.ToInt16(Session["HospitalLocationID"]));
            if (intFormId > 0)
            {
                Session["formId"] = Convert.ToString(intFormId);
            }
            if (Session["encounterid"].ToString() != "0")
            {
                Response.Redirect("~/Editor/WordProcessor.aspx?Mpg=P130", false);
            }
            //else
            //{
            //    dvUpdateStatus.Visible = true;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GVAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Byte A;
        Byte R;
        Byte G;
        Byte B;
        String htmlHexColorValue = "";
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[10].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[10].Visible = false;
            Label lblStatusColor =(Label) e.Row.FindControl("lblStatusColor");
            Label lbl_StatusColor = new Label();

            if (lblStatusColor.Text!= "Blank" && lblStatusColor.Text!= "&nbsp;")
            {
                lbl_StatusColor.BackColor = System.Drawing.ColorTranslator.FromHtml(lblStatusColor.Text);

                A = lbl_StatusColor.BackColor.A;
                B = lbl_StatusColor.BackColor.B;
                R = lbl_StatusColor.BackColor.R;
                G = lbl_StatusColor.BackColor.G;
                htmlHexColorValue = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(A, R, G, B));
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlHexColorValue.ToString());
                }
            }
        }
    }

    protected void GVAppointment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            string sttime, endtime, timeinterval, strFacilityId, strDoctorId, strAppId;
            string recurrApt = "";
            DateTime dAppDate = new DateTime();
            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            string[] strvalue;

            LinkButton lnkPatient1 = (LinkButton)GVAppointment.Rows[e.NewSelectedIndex].FindControl("lnkEncdate"); //sender as LinkButton;

            DateTime appointdate = common.myDate(lnkPatient1.Text);
            dAppDate = appointdate;
            strvalue = lnkPatient1.CommandArgument.Split('|');

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
            Session["Gender"] = strvalue[2];
            Session["encounterid"] = strvalue[3];
            Session["Facility"] = strvalue[4];
            Session["DoctorID"] = strvalue[5];
            Session["RegistrationID"] = strvalue[6];
            Session["AppointmentID"] = strvalue[7];
            Session["ApptDate"] = dAppDate;
            //Response.Redirect("~/Appointment/AppointmentDetails.aspx?appDate=" + appointdate.ToString("MM/dd/yyyy") + "&appid=" + Convert.ToInt32(Session["AppointmentID"]) + "&doctorId=" + Convert.ToInt32(Session["DoctorID"]) + "&RegId=" + Convert.ToInt32(Session["RegistrationID"]) + "&StTime=" + Convert.ToDateTime(txtfrmtime.Text).ToString("hh") + "&EndTime=" + Convert.ToDateTime(txttotime.Text).ToString("hh") + "&TimeInterval=15");
            strDoctorId = Session["DoctorID"].ToString();
            strFacilityId = Session["Facility"].ToString();
            strAppId = Session["AppointmentID"].ToString();
            bool bitIsDoctor = false;
            string strRecurr = "select RecurrenceParentId,IsDoctor from DoctorAppointment where AppointmentID=" + strAppId + "";
            DataSet dsRecurr = new DataSet();
            dsRecurr = dl.FillDataSet(CommandType.Text, strRecurr);
            if (dsRecurr.Tables[0].Rows.Count > 0)
            {
                recurrApt = dsRecurr.Tables[0].Rows[0]["RecurrenceParentId"].ToString();
                bitIsDoctor = Convert.ToBoolean(dsRecurr.Tables[0].Rows[0]["IsDoctor"].ToString());
            }

            //strDoctorId = "<Table1><c1>" + strDoctorId + "</c1></Table1>";
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intFacilityId", strFacilityId);
            HashIn.Add("@xmlDoctorIds", strDoctorId);
            HashIn.Add("@chrForDate", appointdate.ToString("yyyy/MM/dd"));
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointments", HashIn);
            if (ds.Tables[0].Rows.Count > 0)
            {

                sttime = Convert.ToString(TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString()).Hours);
                endtime = Convert.ToString(TimeSpan.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString()).Hours);
                timeinterval = ds.Tables[0].Rows[0].ItemArray[2].ToString();

                RadWindowForNew.NavigateUrl = "~/Appointment/AppointmentDetails.aspx?StTime=" + sttime + "&EndTime=" + endtime + "&appDate=" + dAppDate.ToString("MM/dd/yyyy") + "&appid=" + strAppId + "&recparentid=" + recurrApt + "&RecRule=" + "5" + "&doctorId=" + strDoctorId + "&TimeInterval=" + timeinterval + "&IsDoctor=" + bitIsDoctor + "";
                RadWindowForNew.Height = 630;
                RadWindowForNew.Width = 680;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected int BindUTCTime()
    {
        int timezone = 0;
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intfacilityid", Convert.ToInt32(Session["FacilityID"]));
            string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
            timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"].ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return timezone;
    }
}
