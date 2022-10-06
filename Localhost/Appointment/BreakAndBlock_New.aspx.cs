using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

public partial class Appointment_BreakAndBlock : System.Web.UI.Page
{
    BaseC.RestFulAPI objwcfOt ;

    private enum GridBreakAndBlock : byte
    {
        ID = 0,
        DoctorId = 1,
        FacilityId = 2,
        FacilityName = 3,
        Type = 4,
        BreakName = 5,
        BreakDate = 6,
        StartTime = 7,
        EndTime = 8,
        FrequencyType = 9,
        Frequency = 10,
        FrequencLevel = 11,
        WeekDays = 12,
        MonthlyRepeatDay = 13,
        BreakEndOn = 14,
        Edit = 15,
        Delete = 16,
    }
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    Hashtable hshInput;
    DAL.DAL dl;

    /// <summary>
    /// Private properties For BreakId
    /// </summary>

    private Int32 BreakId
    {
        get
        {
            return ViewState["BreakId"] == null ? 0 : Convert.ToInt32(ViewState["BreakId"]);
        }
        set
        {
            ViewState["BreakId"] = value;
        }
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        objwcfOt = new BaseC.RestFulAPI(sConString);
        //try
        //{
        if (!IsPostBack)
        {
            ViewState["UseFor"] = common.myStr(Request.QueryString["UseFor"]).Trim();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BindProvider();
            BindFacility();
            if (common.myStr(Request.QueryString["Category"]).Trim() != string.Empty)
            {
                ddlProvider.AutoPostBack = false;
                btnClose.Visible = true;
            }
            else
            {
                ddlProvider.AutoPostBack = true;
                btnClose.Visible = false;
            }
            string strHospital = "select CONVERT(varchar(2),AppointmentStartTime,108) as StartTime,CONVERT(varchar(2),AppointmentEndTime,108) as EndTime,AppointmentSlot from  HospitalLocation with (nolock) where id=" + common.myInt(Session["HospitalLocationID"]);
            DataSet dsHos = new DataSet();
            dsHos = dl.FillDataSet(CommandType.Text, strHospital);
            if (dsHos.Tables[0].Rows.Count > 0)
            {
                string strStartTime = dsHos.Tables[0].Rows[0]["StartTime"].ToString();
                string strEndTime = dsHos.Tables[0].Rows[0]["EndTime"].ToString();
                string strIntervalTime = dsHos.Tables[0].Rows[0]["AppointmentSlot"].ToString();
                RadTimeFrom.TimeView.Interval = new TimeSpan(0, Convert.ToInt32(strIntervalTime), 0);
                RadTimeTo.TimeView.Interval = new TimeSpan(0, Convert.ToInt32(strIntervalTime), 0);
                RadTimeFrom.TimeView.StartTime = new TimeSpan(int.Parse(strStartTime), 0, 0);

                RadTimeTo.TimeView.StartTime = new TimeSpan(int.Parse(strStartTime), 0, 0);


                RadTimeFrom.TimeView.EndTime = new TimeSpan(int.Parse(strEndTime), 0, 0);
                RadTimeTo.TimeView.EndTime = new TimeSpan(int.Parse(strEndTime), Convert.ToInt32(strIntervalTime), 0);

                TimeSpan ts = RadTimeFrom.TimeView.Interval;
                ViewState["Duration"] = ts;
                dtpDate.SelectedDate = DateTime.Now;
            }
            if (Request.QueryString["Category"] != null && Request.QueryString["ID"] == null)
            {
                RadTimeFrom.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]);
                RadTimeTo.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]).AddMinutes(common.myInt(Request.QueryString["TimeInterval"]));

                RadTimeTo.TimeView.StartTime = new TimeSpan(int.Parse(Request.QueryString["FromTimeHour"]), int.Parse(RadTimeFrom.SelectedDate.Value.ToString("mm")) + common.myInt(Request.QueryString["TimeInterval"]), 0);
                dtpDate.SelectedDate = Convert.ToDateTime(Request.QueryString["appDate"]);
                btnUpdate.Visible = false;
                btnUpdateAll.Visible = false;
                btnDeleteAllBreak.Visible = false;
                btnDeleteBreak.Visible = false;
            }
            else if (Request.QueryString["Category"] != null && Request.QueryString["ID"] != null && Request.QueryString["DoctorID"] != null)
            {
                string BreakId = "";
                if (Request.QueryString["ID"].Contains('_'))
                {
                    string[] SplitBreak = Request.QueryString["ID"].Split('_');
                    BreakId = SplitBreak[0];
                }
                else
                {
                    BreakId = Request.QueryString["ID"];
                }
                BindBreakAndBlockDetail(BreakId);
                btnUpdate.Visible = true;
                btnSave.Visible = false;

                int Hourtime = 0;
                if (RadTimeFrom.SelectedDate != null)
                {
                    if (RadTimeFrom.SelectedDate.Value.ToString("tt").ToString() == "PM")
                    {

                        Hourtime = int.Parse(RadTimeFrom.SelectedDate.Value.ToString("hh")) + 12;
                        if (Hourtime == 24)
                        {
                            Hourtime = 12;
                        }
                    }
                    else
                    {
                        Hourtime = int.Parse(RadTimeFrom.SelectedDate.Value.ToString("hh"));
                    }
                }
                btnSave.Visible = false;
                //btnUpdateAll.Visible = false;
                btnDeleteAllBreak.Visible = false;
                btnDeleteBreak.Visible = false;
            }
            else
            {
                btnUpdate.Visible = false;
                btnUpdateAll.Visible = false;
                btnDeleteAllBreak.Visible = false;
                btnDeleteBreak.Visible = false;
            }
            RadSchedulerRecurrenceEditor1.DateFormat = "dd/MM/yyyy";
            ViewState["Edit"] = common.myStr(Request.QueryString["Edit"]);
            if (common.myStr(ViewState["UseFor"]).Equals("OT"))
            {
                rdoIsBlock.Visible = false;
                RadSchedulerRecurrenceEditor1.Visible = false;

                lblProvider.Text = "OT Name";
                ddlProvider.Visible = false;
                ddlTheater.Visible = true;
                lblHeader.Text = "OT Break";
                PopulateOTName();

                btnUpdate.Visible = false;
                btnUpdateAll.Visible = false;
                btnSave.Visible = true;

                if (common.myLen(Request.QueryString["ID"]) > 0)
                {
                    string BreakId = "";
                    if (Request.QueryString["ID"].Contains('_'))
                    {
                        string[] SplitBreak = Request.QueryString["ID"].Split('_');
                        BreakId = SplitBreak[0];
                    }
                    else
                    {
                        BreakId = Request.QueryString["ID"];
                    }

                    bindOTBreakDetail(common.myInt(BreakId));
                }
            }
            //Done By Ujjwal 09April2015 to Break facility in Resource Appointment start
            if (common.myStr(ViewState["UseFor"]).Equals("ResApp"))
            {
                rdoIsBlock.Visible = false;
                RadSchedulerRecurrenceEditor1.Visible = false;

                lblProvider.Text = "Resource Name";
                ddlProvider.Visible = false;
                ddlTheater.Visible = true;
                lblHeader.Text = "Resource Break";
                PopulateResourceName();
                ddlTheater.Width = 150;
                btnUpdate.Visible = false;
                btnUpdateAll.Visible = false;
                btnSave.Visible = true;
                int a = common.myLen(Request.QueryString["appid"]);
                if (common.myLen(Request.QueryString["appid"]) > 0)
                {
                    string BreakId = "";
                    if (common.myInt(Request.QueryString["appid"]) > 0)
                    {
                        BreakId = Request.QueryString["appid"];
                        bindResourceBreakDetail(common.myInt(BreakId));
                        btnSave.Text = "Update";
                    }

                }
            }
            //Done By Ujjwal 09April2015 to Break facility in Resource Appointment end

        }
        // }
        //catch (Exception ex)
        //{
        //    lblMessage.Text = ex.Message;
        //}
    }
    private void BindBreakAndBlockDetail(string BreakId)
    {
        DataSet dsBreakBlockDetail = new DataSet();
        BaseC.Appointment appoint = new BaseC.Appointment(sConString);
        dsBreakBlockDetail = appoint.BindBreakAndBlockDetailGrid(common.myInt(Request.QueryString["DoctorID"]), common.myInt(BreakId), common.myInt(Request.QueryString["FacilityId"]));
        if (dsBreakBlockDetail.Tables[0].Rows.Count > 0)
        {
            ViewState["BreakId"] = dsBreakBlockDetail.Tables[0].Rows[0]["ID"].ToString();
            ddlFacility.SelectedValue = dsBreakBlockDetail.Tables[0].Rows[0]["FacilityID"].ToString();
            ddlProvider.SelectedValue = dsBreakBlockDetail.Tables[0].Rows[0]["DoctorID"].ToString();

            RadTimeFrom.DbSelectedDate = dsBreakBlockDetail.Tables[0].Rows[0]["StartTime"].ToString().Trim();
            RadTimeTo.DbSelectedDate = dsBreakBlockDetail.Tables[0].Rows[0]["EndTime"].ToString().Trim();
            RadSchedulerRecurrenceEditor1.RecurrenceRuleText = dsBreakBlockDetail.Tables[0].Rows[0]["RecurrenceRule"].ToString();
            RadSchedulerRecurrenceEditor1.DataBind();
            txtBreakName.Text = dsBreakBlockDetail.Tables[0].Rows[0]["BreakName"].ToString().Trim();

            TimeSpan ts = RadTimeTo.SelectedDate.Value - RadTimeFrom.SelectedDate.Value;
            ViewState["Duration"] = ts;
        }
    }

    private void bindOTBreakDetail(int BreakId)
    {
        DataSet dsBreakBlockDetail = new DataSet();
        BaseC.clsOTBooking appoint = new BaseC.clsOTBooking(sConString);

        dsBreakBlockDetail = appoint.getOTBreakDetails(common.myInt(BreakId), common.myInt(Request.QueryString["TheaterId"]), common.myInt(Request.QueryString["FacilityId"]));
        if (dsBreakBlockDetail.Tables[0].Rows.Count > 0)
        {
            ViewState["BreakId"] = common.myInt(dsBreakBlockDetail.Tables[0].Rows[0]["BreakId"]).ToString();
            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myInt(Request.QueryString["FacilityId"]).ToString()));
            ddlTheater.SelectedIndex = ddlTheater.Items.IndexOf(ddlTheater.Items.FindItemByValue(common.myInt(dsBreakBlockDetail.Tables[0].Rows[0]["TheaterId"]).ToString()));

            RadTimeFrom.DbSelectedDate = common.myStr(dsBreakBlockDetail.Tables[0].Rows[0]["StartTime"]).Trim();
            RadTimeTo.DbSelectedDate = common.myStr(dsBreakBlockDetail.Tables[0].Rows[0]["EndTime"]).Trim();

            txtBreakName.Text = common.myStr(dsBreakBlockDetail.Tables[0].Rows[0]["BreakName"]).Trim();
        }
    }

    //Done By Ujjwal 09April2015 to Bind Resource Break Details Start
    private void bindResourceBreakDetail(int BreakId)
    {
        DataSet dsBreakBlockDetail = new DataSet();
        BaseC.Hospital appoint = new BaseC.Hospital(sConString);

        dsBreakBlockDetail = appoint.getResourceBreakDetails(common.myInt(BreakId), common.myInt(Request.QueryString["TheaterId"]), common.myInt(Request.QueryString["FacilityId"]));
        if (dsBreakBlockDetail.Tables[0].Rows.Count > 0)
        {
            ViewState["BreakId"] = common.myInt(dsBreakBlockDetail.Tables[0].Rows[0]["BreakId"]).ToString();
            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myInt(Request.QueryString["FacilityId"]).ToString()));
            ddlTheater.SelectedIndex = ddlTheater.Items.IndexOf(ddlTheater.Items.FindItemByValue(common.myInt(dsBreakBlockDetail.Tables[0].Rows[0]["ResourceID"]).ToString()));

            RadTimeFrom.DbSelectedDate = common.myStr(dsBreakBlockDetail.Tables[0].Rows[0]["StartTime"]).Trim();
            RadTimeTo.DbSelectedDate = common.myStr(dsBreakBlockDetail.Tables[0].Rows[0]["EndTime"]).Trim();

            txtBreakName.Text = common.myStr(dsBreakBlockDetail.Tables[0].Rows[0]["BreakName"]).Trim();
        }
    }
    //Done By Ujjwal 09April2015 to Bind Resource Break Details Start
    private DateTime RecStart()
    {
        DateTime result = dtpDate.SelectedDate.Value.Date;
        TimeSpan time = RadTimeFrom.SelectedDate.Value.TimeOfDay;
        result = result.Add(time);

        return result;
    }

    private DateTime RecEnd()
    {
        DateTime result = dtpDate.SelectedDate.Value.Date;
        TimeSpan time = RadTimeTo.SelectedDate.Value.TimeOfDay;
        result = result.Add(time);
        return result;
    }

    private void UpdateBreak()
    {
        try
        {
            //string[] strAppId = Convert.ToString(Request.QueryString["ID"]).Split('_');
            BaseC.Patient formatdate = new BaseC.Patient(sConString);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshtableout = new Hashtable();
            Hashtable hshtablein = new Hashtable();
            hshtablein.Clear();
            #region for recurrence
            //DateTime start = RecStart();
            RadSchedulerRecurrenceEditor1.StartDate = RecStart();//.ToString("yyyyMMdd");
            RadSchedulerRecurrenceEditor1.EndDate = RecEnd();
            DateTime OrigTime = DateTime.Now;
            if (Convert.ToString(BreakId) != "")
            {
                string strOrigTime = "select dbo.GetDateFormat(StartTime,'T') AS StartTime from BreakList where ID='" + BreakId + "'";
                OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));

            }
            //string exdate = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";        
            string rule = "";

            if (!string.IsNullOrEmpty(RadSchedulerRecurrenceEditor1.RecurrenceRuleText))
            {
                rule = RadSchedulerRecurrenceEditor1.RecurrenceRule.ToString();
            }
            //if (!string.IsNullOrEmpty(Request.QueryString["recparentid"]))
            //{
            //RecParentId = Request.QueryString["recparentid"].ToString();
            //if (RecParentId == "0")
            //{
            // RecParentId = "";
            //}
            //}


            #endregion

            BaseC.ParseData pdDataCheck = new BaseC.ParseData();


            hshtablein.Add("@chrStartTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"));
            hshtablein.Add("@chrEndTime", RadTimeTo.SelectedDate.Value.ToString("hh:mmtt")); //RadTimeTo.DbSelectedDate.ToString("hh:mm tt"));
            hshtablein.Add("@bitIsBlock", rdoIsBlock.SelectedValue);
            hshtablein.Add("@chvBreakName", pdDataCheck.ParseQ(txtBreakName.Text));
            string exdate = "";
            if (dtpDate.SelectedDate.Value.ToShortDateString() != null)
            {
                exdate = Convert.ToDateTime(dtpDate.SelectedDate.Value.ToShortDateString()).ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
            }
            else
            {

                exdate = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
            }
            hshtablein.Add("@chrBreakDate", formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()));  //Request.QueryString["appDate"]);
            hshtablein.Add("@intDoctorID", ddlProvider.SelectedValue);
            hshtablein.Add("@intFacilityID", Session["FacilityID"]);
            hshtablein.Add("@intEncodedBy", Session["UserID"].ToString());
            hshtableout.Add("@intId", SqlDbType.Int);

            string origrule = "Select RecurrenceRule from BreakList where ID='" + BreakId + "'";
            DataSet ds = dl.FillDataSet(CommandType.Text, origrule);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string OrigRecRule = common.myStr(ds.Tables[0].Rows[0]["RecurrenceRule"]);
                if (OrigRecRule.Trim() != "") //if Appointment is recurring and editing master appointment
                {
                    DateTime dtStart = dtpDate.SelectedDate.Value;
                    if (Convert.ToInt64(BreakId) != 0)
                    {
                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "'  where ID='" + BreakId + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + BreakId + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }

                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveBreaks", hshtablein, hshtableout);
                        string strUpdateRecRule1 = "Update BreakList set RecurrenceParentId='" + BreakId + "' where ID='" + hshtableout["@intId"] + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule1);
                        lblMessage.Text = "Break Update successfull!";
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    }
                }
            }
            else
            {
                hshtablein.Add("@intBreakId", Convert.ToInt32(BreakId));
                hshtablein.Add("@chvRecurringRule", rule);
                hshtablein.Add("@intRecurringParentID", DBNull.Value);
                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveBreaks", hshtablein, hshtableout);
                lblMessage.Text = "Break Update successfull!";
            }
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));
            txtBreakName.Text = "";
            dtpDate.SelectedDate = DateTime.Now;

            RadSchedulerRecurrenceEditor1.RecurrenceRuleText = null;
            RadSchedulerRecurrenceEditor1.DataBind();
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnUpdateAll.Visible = false;
            btnDeleteAllBreak.Visible = false;
            btnDeleteBreak.Visible = false;
            ClearAll();
            BreakId = 0;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close('P');", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        BreakId = 0;
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        btnUpdateAll.Visible = false;
        btnDeleteAllBreak.Visible = false;
        btnDeleteBreak.Visible = false;
        txtBreakName.Text = "";
        dtpDate.SelectedDate = DateTime.Now;
        RadTimeFrom.SelectedDate = null;
        RadTimeTo.SelectedDate = null;
        ViewState["Edit"] = null;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateBreak();
    }

    private void UpdateAllBreak()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //string[] strAppId = Convert.ToString(Request.QueryString["ID"]).Split('_');
            DateTime origAppTime = DateTime.Now;
            DateTime origAppDate = DateTime.Today;
            string OrigRecRule = "", origrule = "";


            if (RadSchedulerRecurrenceEditor1.RecurrenceRuleText != null)
            {
                DateTime dtStart = dtpDate.SelectedDate.Value.Date;
                //string recrule1 = e.Appointment.Attributes["RecurrenceRule"];                
                string strOrigTime = "select StartTime from BreakList where ID='" + BreakId + "'";
                origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                string strOrigDate = "select BreakDate from BreakList where ID='" + BreakId + "'";
                origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                TimeSpan daysCount = dtStart.Subtract(origAppDate);
                string exdate = "";
                if (dtpDate.SelectedDate.Value.ToShortDateString() != null)
                {
                    exdate = "UNTIL=" + Convert.ToDateTime(dtpDate.SelectedDate.Value.ToShortDateString()).ToString("yyyyMMdd") + "T000000Z;";
                }
                else
                {
                    exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";
                }
                origrule = "Select RecurrenceRule from BreakList where id='" + BreakId + "'";
                OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (!OrigRecRule.Contains("COUNT") && OrigRecRule.Trim() != "")
                {
                    if (!OrigRecRule.Contains("UNTIL"))
                    {
                        //int newrule = OrigRecRule.IndexOf("INTERVAL");
                        string newRecRule = OrigRecRule.Insert(OrigRecRule.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + BreakId + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                    else
                    {
                        //int newrule = OrigRecRule.IndexOf("UNTIL");
                        string newrule = OrigRecRule.Insert(OrigRecRule.IndexOf("UNTIL"), exdate);
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newrule + "' where ID='" + BreakId + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }

                }
                else if (OrigRecRule.Trim() != "")
                {
                    if (OrigRecRule.Contains("COUNT"))
                    {
                        int noOfChars = OrigRecRule.IndexOf("INTERVAL") - OrigRecRule.IndexOf("COUNT");
                        string repUntil = OrigRecRule.Remove(OrigRecRule.IndexOf("COUNT"), noOfChars);
                        string newRecRule = repUntil.Insert(repUntil.IndexOf("INTERVAL"), exdate);
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + BreakId + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                    }
                }
            }
            else
            {

                //Hashtable hshIn = new Hashtable();
                //hshIn.Add("@intAppointmentId", strAppId[0]);
                //hshIn.Add("@intLastChangedBy", Session["UserId"]);
                //hshIn.Add("@chvLastChangedDate", DateTime.Now);
                //dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate where Id = @intAppointmentId ", hshIn);//or RecurrenceParentId=@intAppointmentId
            }
            BaseC.Patient formatdate = new BaseC.Patient(sConString);

            Hashtable hshtablein = new Hashtable();
            hshtablein.Clear();
            #region for recurrence

            DateTime result = Convert.ToDateTime(dtpDate.SelectedDate.Value.ToShortDateString());
            TimeSpan time = RadTimeFrom.SelectedDate.Value.TimeOfDay;
            result = result.Add(time);

            RadSchedulerRecurrenceEditor1.StartDate = result;//result;//RecStart();//.ToString("yyyyMMdd");

            result = Convert.ToDateTime(dtpDate.SelectedDate.Value.ToShortDateString());
            time = RadTimeTo.SelectedDate.Value.TimeOfDay;
            result = result.Add(time);


            RadSchedulerRecurrenceEditor1.EndDate = result;//RecEnd();
            DateTime OrigTime = DateTime.Now;
            if (BreakId != null)
            {
                string strOrigTime = "select dbo.GetDateFormat(StartTime,'T') AS StartTime from BreakList where ID='" + BreakId + "'";
                OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));

            }
            string exdate1 = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
            string rule = "";


            origrule = "Select RecurrenceRule from BreakList where ID='" + BreakId + "'";
            OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();

            if (!string.IsNullOrEmpty(RadSchedulerRecurrenceEditor1.RecurrenceRuleText))
            {
                rule = Convert.ToString(RadSchedulerRecurrenceEditor1.RecurrenceRule);
            }
            //if (!string.IsNullOrEmpty(Request.QueryString["recparentid"]))
            //{
            //    RecParentId = Request.QueryString["recparentid"].ToString();
            //    if (RecParentId == "0")
            //    {
            //        RecParentId = "";
            //    }
            //}
            #endregion

            BaseC.ParseData pdDataCheck = new BaseC.ParseData();

            //hshtablein.Add("@intBreakId", BreakId);
            hshtablein.Add("@chrStartTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"));
            hshtablein.Add("@chrEndTime", RadTimeTo.SelectedDate.Value.ToString("hh:mmtt")); //RadTimeTo.DbSelectedDate.ToString("hh:mm tt"));
            hshtablein.Add("@intFacilityID", ddlFacility.SelectedValue);
            hshtablein.Add("@chvBreakName", pdDataCheck.ParseQ(txtBreakName.Text));
            hshtablein.Add("@bitIsBlock", rdoIsBlock.SelectedValue);
            hshtablein.Add("@chrBreakDate", formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()));
            hshtablein.Add("@intDoctorId", ddlProvider.SelectedValue);
            Hashtable hshOut = new Hashtable();
            hshOut.Add("@intId", SqlDbType.Int);
            if (rule != "" && OrigRecRule.Trim() != "")
            {
                //hshtablein.Add("@intBreakId", Convert.ToInt32(strAppId[0]));
                hshtablein.Add("@chvRecurringRule", rule);
                hshtablein.Add("@intRecurringParentID", DBNull.Value);
                hshtablein.Add("@intEncodedBy", Session["UserID"].ToString());

                dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveBreaks", hshtablein, hshOut);
                lblMessage.Text = "Break Update successfull!";
            }
            else if (rule != "" && OrigRecRule.Trim() == "")
            {

                hshtablein.Add("@intBreakId", Convert.ToInt32(BreakId));
                hshtablein.Add("@chvRecurringRule", rule);
                hshtablein.Add("@intRecurringParentID", DBNull.Value);
                hshtablein.Add("@intEncodedBy", Session["UserID"].ToString());

                dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveBreaks", hshtablein, hshOut);
                lblMessage.Text = "Break Update successfull!";
            }
            else if (rule == "")
            {
                hshtablein.Add("@chvRecurringRule", "");
                hshtablein.Add("@intRecurringParentID", "");
                hshtablein.Add("@intEncodedBy", Session["UserID"].ToString());
                hshtablein.Add("@intBreakId", Convert.ToInt32(BreakId));
                dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveBreaks", hshtablein, hshOut);
                lblMessage.Text = "Break Update successfull!";
            }
            BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            txtBreakName.Text = "";
            dtpDate.SelectedDate = DateTime.Now;
            //RadTimeFrom.DbSelectedDate = DateTime.Now;
            //RadTimeTo.DbSelectedDate = gvlblEndTime.Text;

            //RadSchedulerRecurrenceEditor1.RecurrenceRuleText = dsBreakBlockDetail.Tables[0].Rows[0]["RecurrenceRule"].ToString();
            //RadSchedulerRecurrenceEditor1.DataBind();

            RadSchedulerRecurrenceEditor1.RecurrenceRuleText = null;
            RadSchedulerRecurrenceEditor1.DataBind();
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnUpdateAll.Visible = false;
            btnDeleteAllBreak.Visible = false;
            btnDeleteBreak.Visible = false;
            ClearAll();
            BreakId = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close('P');", true);
    }

    protected void btnUpdateAll_Click(object sender, EventArgs e)
    {
        UpdateAllBreak();
    }

    protected void RadTimeTo_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        if (ViewState["Duration"] != null)
        {
            TimeSpan ts = (TimeSpan)ViewState["Duration"];
        }
    }

    protected void RadTimeFrom_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        if (ViewState["Duration"] != null)
        {
            TimeSpan ts = (TimeSpan)ViewState["Duration"];
            RadTimeTo.DbSelectedDate = RadTimeFrom.SelectedDate.Value.Add(ts);
        }
    }

    protected void BindProvider()
    {

        try
        {
            hshInput = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intFacilityId", ddlFacility.SelectedValue);
            hshInput.Add("@AppointmentDate", string.Empty);

            
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorListForAppointment", hshInput);
            DataTable dtResource = (DataTable)objDs.Tables[0];
            DataView dv = (DataView)dtResource.DefaultView;
         //   dv.RowFilter = " IsDoctor='True'";
            DataTable dt = dv.ToTable();
            ddlProvider.Items.Clear();
            ddlProvider.DataSource = dv.ToTable();
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataBind();
            ddlProvider.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlProvider.ClearSelection();
            if (Request.QueryString["DoctorId"] != null)
            {
                ddlProvider.SelectedValue = Request.QueryString["DoctorId"];
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindFacility()
    {
        try
        {
            hshInput = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intUserId", Session["UserID"]);
            hshInput.Add("@intGroupId", Session["GroupID"]);
            hshInput.Add("@chvFacilityType", "O");
            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", hshInput);
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            ddlFacility.Items.Clear();
            ddlFacility.DataSource = objDs1;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataBind();
            ddlFacility.ClearSelection();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtBreakName.Text.Trim() == "")
        {
            Alert.ShowAjaxMsg("Please enter the Name", Page);
            return;
        }
        if (dtpDate.SelectedDate == null)
        {
            Alert.ShowAjaxMsg("Please select Break Date", Page);
            return;
        }
        if (RadTimeFrom.SelectedDate == null)
        {
            Alert.ShowAjaxMsg("Please select From Time", Page);
            return;
        }
        if (RadTimeTo.SelectedDate == null)
        {
            Alert.ShowAjaxMsg("Please select To Time", Page);
            return;
        }

        if (common.myStr(ViewState["UseFor"]).Equals("OT"))
        {
            if (ddlTheater.SelectedIndex == 0)
            {
                Alert.ShowAjaxMsg("Please select OT Name", Page);
                return;
            }
        }
        else if (common.myStr(ViewState["UseFor"]).Equals("ResApp"))
        {
            if (ddlTheater.SelectedIndex == 0)
            {
                Alert.ShowAjaxMsg("Please select Resource Name", Page);
                return;
            }
        }
        else
        {
            if (ddlProvider.SelectedIndex == 0)
            {
                Alert.ShowAjaxMsg("Please select Provider", Page);
                return;
            }
        }
        if (ddlFacility.SelectedValue == "")
        {
            Alert.ShowAjaxMsg("Please select Facility", Page);
            return;
        }

        if (common.myStr(ViewState["UseFor"]).Equals("OT"))
        {
            SaveOTBreak();
        }
        else if (common.myStr(ViewState["UseFor"]).Equals("ResApp"))
        {
            SaveResourceBreak();
        }
        else
        {
            SaveRecords();
        }
    }
    private void SaveOTBreak()
    {
        BaseC.Patient objP = new BaseC.Patient(sConString);
        BaseC.clsOTBooking objB = new BaseC.clsOTBooking(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            int intTo = 0;
            int intFrom = 0;

            String FromTime = RadTimeFrom.SelectedDate.Value.ToString("HH:mm");

            String FromTo = RadTimeTo.SelectedDate.Value.ToString("HH:mm");
            if (FromTime.Contains(":"))
            {
                string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                intFrom = common.myInt(strFromTime);
            }

            if (FromTo.Contains(":"))
            {

                string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                intTo = common.myInt(strFromTo);
            }

            if (intTo <= intFrom)
            {
                Alert.ShowAjaxMsg("Please change the To Time", this.Page);
                lblMessage.Text = "Please change the To Time";
                return;
            }


            int BId = objB.ExistOTBreak(common.myInt(ddlFacility.SelectedValue), common.myInt(ddlTheater.SelectedValue),
                                Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                RadTimeFrom.SelectedDate.Value.ToString("HH:mm"), RadTimeTo.SelectedDate.Value.ToString("HH:mm"));
            if (common.myStr(ViewState["Edit"]).Trim() != "Edit")
            {
                if (BreakId == 0)
                {
                    if (BId > 1)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        Alert.ShowAjaxMsg("Already set Break in this time.", this.Page);
                        lblMessage.Text = "Already set Break in this time.";
                        return;
                    }
                }
                else if (BreakId > 0)
                {
                    if (BId == 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        Alert.ShowAjaxMsg("Already set Break in this time.", this.Page);
                        lblMessage.Text = "Already set Break in this time.";
                        return;
                    }
                }
            }


            string strMsg = objB.SaveOTBreaks(BreakId, common.myInt(ddlTheater.SelectedValue), common.myInt(Session["HospitalLocationId"]),
                                     common.myInt(ddlFacility.SelectedValue), common.myStr(txtBreakName.Text).Trim(),
                                     objP.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()),
                                     RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"), RadTimeTo.SelectedDate.Value.ToString("hh:mmtt"),
                                     common.myInt(Session["UserId"]));

            if ((strMsg.ToUpper().Contains(" UPDATED") || strMsg.ToUpper().Contains(" SAVED")) && !strMsg.ToUpper().Contains("USP"))
            {
                //Alert.ShowAjaxMsg(strMsg, this);
                lblMessage.Text = strMsg;
            }

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            txtBreakName.Text = "";
            dtpDate.SelectedDate = DateTime.Now;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objP = null;
            objB = null;
        }
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close('P');", true);
    }

    private void SaveResourceBreak()
    {
        BaseC.Patient objP = new BaseC.Patient(sConString);
        BaseC.Hospital objB = new BaseC.Hospital(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            int intTo = 0;
            int intFrom = 0;

            String FromTime = RadTimeFrom.SelectedDate.Value.ToString("HH:mm");

            String FromTo = RadTimeTo.SelectedDate.Value.ToString("HH:mm");
            if (FromTime.Contains(":"))
            {
                string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                intFrom = common.myInt(strFromTime);
            }

            if (FromTo.Contains(":"))
            {

                string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                intTo = common.myInt(strFromTo);
            }

            if (intTo <= intFrom)
            {
                Alert.ShowAjaxMsg("Please change the To Time", this.Page);
                lblMessage.Text = "Please change the To Time";
                return;
            }

            if (btnSave.Text.ToUpper() == "SAVE")
            {
                int BId = objB.ExistingResourceBreak(common.myInt(ddlFacility.SelectedValue), common.myInt(ddlTheater.SelectedValue),
                                    Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                    RadTimeFrom.SelectedDate.Value.ToString("HH:mm"), RadTimeTo.SelectedDate.Value.ToString("HH:mm"));



                if (BreakId > 0)
                {
                    if (BId > 1)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        Alert.ShowAjaxMsg("Already set Break in this time.", this.Page);
                        lblMessage.Text = "Already set Break in this time.";
                        return;
                    }
                }
                else
                {
                    if (BId > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        Alert.ShowAjaxMsg("Already set Break in this time.", this.Page);
                        lblMessage.Text = "Already set Break in this time.";
                        return;
                    }
                }
            }

            string strMsg = objB.SaveResourceBreaks(BreakId, common.myInt(ddlTheater.SelectedValue), common.myInt(Session["HospitalLocationId"]),
                                     common.myInt(ddlFacility.SelectedValue), common.myStr(txtBreakName.Text).Trim(),
                                     objP.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()),
                                     RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"), RadTimeTo.SelectedDate.Value.ToString("hh:mmtt"),
                                     common.myInt(Session["UserId"]));

            if (strMsg.ToUpper().Contains("UPDATE") || strMsg.ToUpper().Contains("SAVE"))
            {

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            else
            {

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
            lblMessage.Text = strMsg;
            lblMessage.Visible = true;

            txtBreakName.Text = "";
            dtpDate.SelectedDate = DateTime.Now;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            lblMessage.Visible = true;
            objException.HandleException(Ex);
        }
        finally
        {
            objP = null;
            objB = null;
        }


    }
    private void SaveRecords()
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string rule = "";
        try
        {
            BaseC.Appointment appoint = new BaseC.Appointment(sConString);
            int intTo = 0, intFrom = 0;
            String FromTime = RadTimeFrom.SelectedDate.Value.ToString("HH:mm");

            String FromTo = RadTimeTo.SelectedDate.Value.ToString("HH:mm");
            if (FromTime.Contains(":") == true)
            {
                string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                intFrom = Convert.ToInt16(strFromTime);
            }

            if (FromTo.Contains(":") == true)
            {

                string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                intTo = Convert.ToInt16(strFromTo);
            }

            if (intTo <= intFrom)
            {
                Alert.ShowAjaxMsg("Please change the To Time", this.Page);
                return;
            }

            RadSchedulerRecurrenceEditor1.StartDate = RecStart();//.ToString("yyyyMMdd");
            RadSchedulerRecurrenceEditor1.EndDate = RecEnd();

            if (!string.IsNullOrEmpty(RadSchedulerRecurrenceEditor1.RecurrenceRuleText))
            {
                rule = RadSchedulerRecurrenceEditor1.RecurrenceRule.ToString();
            }
            if (rule == "")
            {
                string sBreakId = appoint.ExistBreakAndBlock(BreakId, Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt32(ddlProvider.SelectedValue), Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"), RadTimeFrom.SelectedDate.Value.ToString("HH:mm"), RadTimeTo.SelectedDate.Value.ToString("HH:mm"));
                if (sBreakId != "" && sBreakId != null)
                {
                    Alert.ShowAjaxMsg("Already set Break and Block in this time.", Page);
                    return;
                }
            }
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            Hashtable hshOut = new Hashtable();
            hshInput.Add("@intBreakId", BreakId);
            hshInput.Add("@intDoctorID", Convert.ToInt32(ddlProvider.SelectedValue));
            hshInput.Add("@bitIsBlock", rdoIsBlock.SelectedValue);
            hshInput.Add("@intFacilityID", Convert.ToInt16(ddlFacility.SelectedValue));
            hshInput.Add("@chvBreakName", txtBreakName.Text.Trim());
            hshInput.Add("@chrBreakDate", formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()));

            hshInput.Add("@chrStartTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"));
            hshInput.Add("@chrEndTime", RadTimeTo.SelectedDate.Value.ToString("hh:mmtt"));

            hshInput.Add("@chvRecurringRule ", rule);
            hshInput.Add("@intRecurringParentID ", DBNull.Value);




            hshInput.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
            hshOut.Add("@intId", SqlDbType.Int);
            hshOut.Add("@chvError", SqlDbType.VarChar);// add new output parameter, to get proper message     (Prashant)

            // By this we are not getting output parameter value (@intId,@chvError) 
            //int iCon = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveBreaks", hshInput, hshOut);



            hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveBreaks", hshInput, hshOut);
            string chvError = hshOut["@chvError"].ToString();
            int iCon = Convert.ToInt32(hshOut["@intId"].ToString());
            if (chvError.Contains("Appointment already booked Against this Doctor and time.") || chvError.Contains("Start-Time can not be greater then To-Time."))
            {
                Alert.ShowAjaxMsg(chvError, this);
                return;
            }
            else
            {
                if (iCon == 0)
                {
                    if (BreakId > 0)
                    {
                        lblMessage.Text = "Record(s) Has Been Updated ...";

                    }
                    else
                    {

                        lblMessage.Text = "Record(s) Has Been Saved...";
                    }
                }
            }

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));

            txtBreakName.Text = "";
            dtpDate.SelectedDate = DateTime.Now;
            //RadTimeFrom.DbSelectedDate = DateTime.Now;
            //RadTimeTo.DbSelectedDate = gvlblEndTime.Text;

            //RadSchedulerRecurrenceEditor1.RecurrenceRuleText = dsBreakBlockDetail.Tables[0].Rows[0]["RecurrenceRule"].ToString();
            //RadSchedulerRecurrenceEditor1.DataBind();

            RadSchedulerRecurrenceEditor1.RecurrenceRuleText = null;
            RadSchedulerRecurrenceEditor1.DataBind();
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            btnUpdateAll.Visible = true;
            ClearAll();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close('P');", true);
    }

    private void ClearAll()
    {
        txtBreakName.Text = "";
    }
    protected void ddlProvider_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));
        btnUpdate.Visible = false;
        btnUpdateAll.Visible = false;
        btnDeleteAllBreak.Visible = false;
        btnDeleteBreak.Visible = false;
        btnSave.Visible = true;
    }

    private void BindBreakAndBlockDetailGrid(Int32 iDoctorId)
    {
        try
        {
            gvBreakAndBlockDetails.DataSource = null;
            gvBreakAndBlockDetails.DataBind();
            hshInput = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@intDoctorID", iDoctorId);
            hshInput.Add("@intFacilityId", Convert.ToInt16(ddlFacility.SelectedValue));
            DataSet dsBreakBlockDetail = dl.FillDataSet(CommandType.StoredProcedure, "UspGetDoctorBreak", hshInput);
            if (dsBreakBlockDetail.Tables[0].Rows.Count > 0)
            {
                gvBreakAndBlockDetails.DataSource = dsBreakBlockDetail.Tables[0];
                gvBreakAndBlockDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvBreakAndBlockDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[Convert.ToByte(GridBreakAndBlock.ID)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridBreakAndBlock.DoctorId)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridBreakAndBlock.FacilityId)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridBreakAndBlock.FacilityName)].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            //e.Row.Cells[Convert.ToByte(GridBreakAndBlock.WeekDays)].Visible = false;
            //e.Row.Cells[Convert.ToByte(GridBreakAndBlock.MonthlyRepeatDay)].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label ID = (Label)e.Row.Cells[Convert.ToByte(GridBreakAndBlock.ID)].FindControl("lblID");
            if (ID.Text == "")
            {
                e.Row.Cells[Convert.ToByte(GridBreakAndBlock.Edit)].Enabled = false;
                ImageButton iDelete = (ImageButton)e.Row.Cells[Convert.ToByte(GridBreakAndBlock.ID)].FindControl("ibtnDelete");
                iDelete.Enabled = false;

            }
            // e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(this.gvProblems, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvBreakAndBlockDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Del")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lbId = (Label)row.Cells[Convert.ToByte(GridBreakAndBlock.ID)].FindControl("lblID");
                Label lblRecurring = (Label)row.Cells[8].FindControl("lblRecurrence");
                Label gvlblBreakDate = (Label)row.Cells[5].FindControl("lblBreakDate");
                ViewState["dtStart"] = gvlblBreakDate.Text;

                BreakId = Convert.ToInt32(lbId.Text);
                btnDeleteAllBreak.Visible = true;
                btnDeleteBreak.Visible = true;
                btnUpdate.Visible = false;
                btnUpdateAll.Visible = false;
                btnSave.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvBreakAndBlockDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["Duration"] = null;
            Label gvlblID = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblID");
            Label gvlblDoctorId = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblDoctorId");
            Label gvlblFacilityId = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblFacilityId");
            Label gvlblBreakName = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblBreakName");
            Label gvlblBreakDate = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblBreakDate");
            Label gvlblStartTime = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblStartTime");
            Label gvlblEndTime = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblEndTime");
            Label gvlbllblRecurrence = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblRecurrence");
            Label gvlbllblRecurrenceParentId = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblRecurrenceParentId");
            HiddenField hdBdate = (HiddenField)gvBreakAndBlockDetails.SelectedRow.FindControl("hdBdate");


            BreakId = Convert.ToInt32(gvlblID.Text.Trim());
            ddlProvider.SelectedValue = gvlblDoctorId.Text;
            ddlFacility.SelectedValue = gvlblFacilityId.Text;
            //rdoIsBlock.ClearSelection();
            txtBreakName.Text = gvlblBreakName.Text;
            dtpDate.SelectedDate = Convert.ToDateTime(hdBdate.Value);
            RadTimeFrom.DbSelectedDate = gvlblStartTime.Text;
            RadTimeTo.DbSelectedDate = gvlblEndTime.Text;
            TimeSpan ts = RadTimeTo.SelectedDate.Value - RadTimeFrom.SelectedDate.Value;
            ViewState["Duration"] = ts;
            //RadSchedulerRecurrenceEditor1.RecurrenceRuleText = dsBreakBlockDetail.Tables[0].Rows[0]["RecurrenceRule"].ToString();
            //RadSchedulerRecurrenceEditor1.DataBind();

            RadSchedulerRecurrenceEditor1.RecurrenceRuleText = gvlbllblRecurrence.Text;
            RadSchedulerRecurrenceEditor1.DataBind();
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            btnUpdateAll.Visible = true;
            btnDeleteAllBreak.Visible = false;
            btnDeleteBreak.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvBreakAndBlockDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBreakAndBlockDetails.PageIndex = e.NewPageIndex;
        BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));
    }

    protected void btnDeleteAllBreak_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //string[] strAppId = ViewState["AppId"].ToString().Split('_');
            DateTime origAppTime = DateTime.Now;
            DateTime origAppDate = DateTime.Today;
            if (BreakId != null)
            {
                //Label gvlblBreakDate = (Label)gvBreakAndBlockDetails.SelectedRow.FindControl("lblBreakDate");
                DateTime dtStart = common.myDate(ViewState["dtStart"]);

                string strOrigTime = "select StartTime from BreakList where ID='" + BreakId + "'";
                origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                string strOrigDate = "select BreakDate from BreakList where ID='" + BreakId + "'";
                origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                // TimeSpan daysCount = dtStart.Subtract(origAppDate);

                string exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";

                string origrule = "Select RecurrenceRule from BreakList where ID='" + BreakId + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (OrigRecRule.Trim() != null && OrigRecRule.ToString().Trim() != "")
                {
                    if (!OrigRecRule.Contains("COUNT"))
                    {
                        if (!OrigRecRule.Contains("UNTIL"))
                        {
                            //int newrule = OrigRecRule.IndexOf("INTERVAL");
                            string newRecRule = OrigRecRule.Insert(OrigRecRule.IndexOf("INTERVAL"), exdate);
                            string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + BreakId + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);

                            lblMessage.Text = "Delete sucessfull!";
                        }
                        else
                        {
                            //int newrule = OrigRecRule.IndexOf("UNTIL");
                            string newrule = OrigRecRule.Insert(OrigRecRule.IndexOf("UNTIL"), exdate);
                            string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newrule + "' where ID='" + BreakId + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);

                            lblMessage.Text = "Delete sucessfull!";
                        }
                    }
                    else
                    {
                        if (OrigRecRule.Contains("COUNT"))
                        {
                            int noOfChars = OrigRecRule.IndexOf("INTERVAL") - OrigRecRule.IndexOf("COUNT");
                            string repUntil = OrigRecRule.Remove(OrigRecRule.IndexOf("COUNT"), noOfChars);
                            string newRecRule = repUntil.Insert(repUntil.IndexOf("INTERVAL"), exdate);
                            string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + newRecRule + "' where ID='" + BreakId + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);

                            lblMessage.Text = "Delete sucessfull!";
                        }
                    }
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                else
                {
                    Hashtable hshIn = new Hashtable();
                    hshIn.Add("@intBreakId", BreakId);
                    hshIn.Add("@intLastChangedBy", Session["UserId"]);
                    hshIn.Add("@chvLastChangedDate", DateTime.Now);
                    dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate where Id=@intBreakId", hshIn);
                }
                BreakId = 0;
            }
            BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnUpdateAll.Visible = false;
            btnDeleteAllBreak.Visible = false;
            btnDeleteBreak.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDeleteBreak_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            if (BreakId != null)
            {
                //DateTime dtStart =Convert.ToDateTime(ViewState["dtStart"]);

                DateTime dtStart = common.myDate(ViewState["dtStart"]);
                DateTime OrigRecTime = new DateTime();
                string strOrigRecTime = "select StartTime from BreakList where ID='" + BreakId + "'";
                OrigRecTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigRecTime));

                //DateTime dtStart = (DateTime)ViewState["WeekDateForRec"];
                string exdate = dtStart.ToString("yyyyMMdd") + "T" + OrigRecTime.ToString("HHmmss") + "Z";

                string origrule = "Select RecurrenceRule from BreakList where ID='" + BreakId + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (OrigRecRule != null && OrigRecRule.ToString() != "")
                {
                    if (!OrigRecRule.Contains("EXDATE"))
                    {
                        string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + BreakId + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);

                        lblMessage.Text = "Delete sucessfull!";
                    }
                    else
                    {
                        string UpdateRule = OrigRecRule + "," + exdate;
                        string strUpdateRecRule = "Update BreakList set RecurrenceRule='" + UpdateRule + "' where ID='" + BreakId + "'";
                        dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);

                        lblMessage.Text = "Delete sucessfull!";
                    }
                }
                else
                {
                    hshIn.Add("@intBreakId", BreakId);
                    hshIn.Add("@intLastChangedBy", Session["UserId"]);
                    hshIn.Add("@chvLastChangedDate", DateTime.Now);
                    dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update BreakList Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate where Id=@intBreakId", hshIn);
                }
                BreakId = 0;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            BindBreakAndBlockDetailGrid(Convert.ToInt32(ddlProvider.SelectedValue));
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnUpdateAll.Visible = false;
            btnDeleteAllBreak.Visible = false;
            btnDeleteBreak.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateOTName()
    {
        DataSet ds = new DataSet();
        try
        {
            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue));

            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Active=1";


            ddlTheater.DataSource = dv.ToTable();
            ddlTheater.DataTextField = "TheatreName";
            ddlTheater.DataValueField = "TheatreID";
            ddlTheater.DataBind();

            ddlTheater.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlTheater.SelectedIndex = 0;

            if (common.myInt(Request.QueryString["TheaterId"]) > 0)
            {
                ddlTheater.SelectedIndex = ddlTheater.Items.IndexOf(ddlTheater.Items.FindItemByValue(common.myInt(Request.QueryString["TheaterId"]).ToString()));
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
            ds.Dispose();
        }
    }

    //Done By Ujjwal 09April2015 o popukate Resource Name For Resource Break start

    private void PopulateResourceName()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Hospital hs = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = hs.GetResourceMaster(Convert.ToInt16(common.myInt(Session["HospitalLocationId"])), Convert.ToInt16(common.myInt(ddlFacility.SelectedValue)),
                                            1, 0, 0);

            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Active=1";


            ddlTheater.DataSource = dv.ToTable();
            ddlTheater.DataTextField = "ResourceName";
            ddlTheater.DataValueField = "ResourceId";
            ddlTheater.DataBind();

            ddlTheater.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlTheater.SelectedIndex = 0;

            if (common.myInt(Request.QueryString["TheaterId"]) > 0)
            {
                ddlTheater.SelectedIndex = ddlTheater.Items.IndexOf(ddlTheater.Items.FindItemByValue(common.myInt(Request.QueryString["TheaterId"]).ToString()));
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
            ds.Dispose();
        }
    }
    //Done By Ujjwal 09April2015 o popukate Resource Name For Resource Break start

}
