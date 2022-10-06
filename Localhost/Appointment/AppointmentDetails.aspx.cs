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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;
using System.Net;
using System.IO;
using System.Drawing;
using BaseC;

public partial class Appointment_AppointmentDetails : System.Web.UI.Page
{
    protected UserControl uc1;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    BaseC.ParseData pdDataCheck = new BaseC.ParseData();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Patient objBc = new BaseC.Patient(sConString);


        if (!IsPostBack)
        {
            //try
            //{
            Session["RecApptID"] = null;

            ddlAppointmentStatus.Enabled = false;
            ViewState["NewRegNo"] = "0";
          
            if (common.myStr(Session["RegIdFromRegistration"]) != "") // used for direct appointment from registration
            {
                txtAccountNo.Text = common.myStr(Session["RegIdFromRegistration"]);
                //  hdnRegistrationId.Value = common.myStr(Session["RegIdFromRegistration"]);
                btnGetInfo_Click(null, null);
                Session["RegIdFromRegistration"] = null;
            }
            else if (Session["FollowUpRegistrationId"] != null)
            {
                txtAccountNo.Text = common.myStr(Session["RegistrationNo"]);
                hdnRegistrationId.Value = common.myStr(Session["FollowUpRegistrationId"]);
                btnGetInfo_Click(null, null);
            }
            if (Request.QueryString["RegId"] != null)
            {
                txtAccountNo.Text = Request.QueryString["RegId"].ToString();
                btnGetInfo_Click(null, null);
            }

            DataSet ds = dl.FillDataSet(CommandType.Text, "select * from GetStatus(" + Session["HospitalLocationId"] + ",'Appointment')");
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdCheckIn.Value = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                hdUnCon.Value = ds.Tables[0].Rows[1].ItemArray[0].ToString();
            }
            txtAccountNumber.Attributes.Add("readonly", "true");
            RadTimeFrom.TimeView.Interval = new TimeSpan(0, common.myInt(Request.QueryString["TimeInterval"]), 0);
            RadTimeTo.TimeView.Interval = new TimeSpan(0, common.myInt(Request.QueryString["TimeInterval"]), 0);

            RadTimeFrom.TimeView.StartTime = new TimeSpan(common.myInt(Request.QueryString["StTime"]), 0, 0);
            RadTimeTo.TimeView.StartTime = new TimeSpan(common.myInt(Request.QueryString["StTime"]), 0, 0);

            RadTimeFrom.TimeView.EndTime = new TimeSpan(common.myInt(Request.QueryString["EndTime"]), 50, 0);
            RadTimeTo.TimeView.EndTime = new TimeSpan(common.myInt(Request.QueryString["EndTime"]), 50, 0);

            // RadTimeTo.TimeView.EndTime = new TimeSpan(common.myInt(Request.QueryString["EndTime"]), common.myInt(Request.QueryString["TimeInterval"]), 0);

            hdnCurrDate.Value = DateTime.Today.ToString("dd/MM/yyyy");
            populateAppointmentStatus();
            getDoctorID();
            populateFacility();


            populateVisitType(false, 0);
            populateDuration();
            PopulateIdentityTypes();

            //ViewState["IsDoctor"] = Convert.ToBoolean(Request.QueryString["IsDoctor"]);
            ViewState["IsDoctor"] = common.myBool(Request.QueryString["IsDoctor"]);

            populateReferringDoctor();

            if (common.myBool(ViewState["IsDoctor"]) && common.myInt(Request.QueryString["appid"]) == 0)
            {
                ddlAppointment.SelectedIndex = 1;
                //ddlReferringDoctor.SelectedValue = Request.QueryString["DoctorId"];
                // ddlReferringDoctor.SelectedIndex = ddlReferringDoctor.Items.IndexOf(ddlReferringDoctor.Items.FindItemByValue(common.myInt(Request.QueryString["DoctorId"]).ToString()));

            }
            else if (common.myBool(ViewState["IsDoctor"]) && common.myInt(Request.QueryString["appid"]) != 0)
            {
                ddlAppointment.SelectedIndex = 1;
                //ddlReferringDoctor.SelectedValue = Request.QueryString["DoctorId"];
                //ddlReferringDoctor.SelectedIndex = ddlReferringDoctor.Items.IndexOf(ddlReferringDoctor.Items.FindItemByValue(common.myInt(Request.QueryString["DoctorId"]).ToString()));
            }
            else
            {
                ddlAppointment.SelectedIndex = 2;
                if (Request.QueryString["appid"].ToString() == "0")
                {
                    populateProvider();
                }
            }
            populateProvider();
            BindRefType();

            //RadComboBoxItem item = new RadComboBoxItem("[Select]", "0");
            //dropReferredType.Items.Insert(0, item);

            if (common.myInt(Request.QueryString["appid"]) > 0)
            {
                EnableControlFalse();
                ShowAppointmentData();

                int Hourtime = 0;
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

                //RadTimeTo.TimeView.StartTime = new TimeSpan(Hourtime, int.Parse(RadTimeFrom.SelectedDate.Value.ToString("mm")) + Convert.ToInt32(Request.QueryString["TimeInterval"]), 0);

            }
            else
            {
                RadTimeFrom.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]);
                RadTimeTo.SelectedDate = Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]).AddMinutes(common.myInt(Request.QueryString["TimeInterval"]));

                //RadTimeTo.TimeView.StartTime = new TimeSpan(int.Parse(Request.QueryString["FromTimeHour"]), int.Parse(RadTimeFrom.SelectedDate.Value.ToString("mm")) + common.myInt(Request.QueryString["TimeInterval"]), 0);

                if (rblReferralDoctorType.Items.Count > 0)
                {
                    rblReferralDoctorType.SelectedIndex = 0;
                    rblReferralDoctorType_SelectedIndexChanged(rblReferralDoctorType, new System.EventArgs());
                }


                ddlFacility.SelectedValue = Request.QueryString["FacilityId"];
                Session["FacilityId"] = Request.QueryString["FacilityId"];
                dtpDate.DateInput.DateFormat = "dd/MM/yyyy";

                dtpDate.SelectedDate = Convert.ToDateTime(Request.QueryString["appDate"]);

                ddlProvider.SelectedValue = Request.QueryString["doctorId"];
            }

            PopulateRoom();
            hdnDuration.Value = "";
            hdnFromTime.Value = "";
            //populateReferringDoctor();

            RadSchedulerRecurrenceEditor1.DateFormat = "dd/MM/yyyy";



            txtRegNo.Focus();
            if (common.myInt(Request.QueryString["appid"]) == 0)
            {
                txtRegNo.Enabled = true;
                Page.SetFocus(txtRegNo);
                btnsave.Text = "Make Appointment";
                //btnPayment.Visible = false;
            }
            else
            {

                txtRegNo.Enabled = false;
                btnUpdate.Visible = true;
                //btnPayment.Visible = false;
                //Added by vineet
                RadSchedulerRecurrenceEditor1.StartDate = RecStart();//.ToString("yyyyMMdd");
                RadSchedulerRecurrenceEditor1.EndDate = RecEnd();
                if (RadSchedulerRecurrenceEditor1.RecurrenceRuleText != null)
                {
                    btnUpdateAll.Visible = false;
                }
                else
                {
                    btnUpdateAll.Visible = true;
                }
                btnsave.Text = "Update Appointment";
                btnsave.Visible = false;

            }

            //}
            // catch(Exception ex)
            //{

            //} 
            DateTime apdate = Convert.ToDateTime(Request.QueryString["appDate"]);
            ////common.myDate(apdate.ToString("dd/MM/yyyy")) != Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")) &&

            if (common.myInt(Request.QueryString["appid"]) > 0)
            {
                if (apdate.Date < DateTime.Now.Date || (apdate.Date == DateTime.Now.Date && common.myStr(hdnStatusCode.Value) == "P"))
                {
                    DisableAllControls();
                    BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                    bool IsRequiredPreAuthNo = appoint.GetPatientAccountTypeOnEdit(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(hdnERegistrationId.Value), common.myInt(Request.QueryString["appid"]));
                    if (IsRequiredPreAuthNo == true)
                    {
                        string AppointmentDayPeriod = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "AppointmentDayPeriod", sConString);
                        string day = "-" + AppointmentDayPeriod;
                        DateTime dtGrace = DateTime.Now.AddDays(common.myDbl(day));
                        ////if (apdate.Date >= dtGrace.Date)
                           //// txtauthorization.Enabled = true;
                    }
                }
            }



        }
        setControlHospitalbased(); 

    }



    /// <summary>
    /// Will Define Controls and values based on Hospital based 
    /// Auther :Naushad
    /// </summary>
    public void setControlHospitalbased()
    {
        string AppointmentFutureDate = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "AppointmentScheduleDate", sConString);

        if (AppointmentFutureDate != "")
        {

            dtpDate.MaxDate = DateTime.Now.AddDays(common.myInt(AppointmentFutureDate));
        }

    }

    private void DisableAllControls()
    {
        lbtnSearchPatient.Enabled = false;
        txtAccountNo.Enabled = false;
        txtOldRegNo.Enabled = false;
        txtAccountNumber.Enabled = false;
        txtFName.Enabled = false;
        txtMName.Enabled = false;
        txtMName.Enabled = false;
        dtpDOB.EnableAriaSupport = false;
        txtAgeYears.Enabled = false;
        txtAgeMonths.Enabled = false;
        txtAgeDays.Enabled = false;
        ddlGender.Enabled = false;
        txtMobile.Enabled = false;
        txtEmail.Enabled = false;
        txtNote.Enabled = false;
        ddlFacility.Enabled = false;
        ddlAppointment.Enabled = false;
        ddlProvider.Enabled = false;
        ddlAppointmentType.Enabled = false;
        dtpDate.Enabled = false;
        RadTimeFrom.Enabled = false;
        RadTimeTo.Enabled = false;
        //ddlRoom.Enabled = false;
     chkWalkIn.Enabled = false;
        txtreason.Enabled = false;
        ddlAppointmentStatus.Enabled = false;
        dropReferredType.Enabled = false;
        ddlreferringphysician.Enabled = false;
        RadSchedulerRecurrenceEditor1.Enabled = false;
        ////txtauthorization.Enabled = false;
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
        string firstApptDate = "";
        string NewApptDate = "";
        string strDateAppointment = formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString());
        string strformatApptDate = formatdate.FormatDate(strDateAppointment, "dd/MM/yyyy", "yyyy/MM/dd");
        firstApptDate = strformatApptDate.Remove(4, 1);
        NewApptDate = firstApptDate.Remove(6, 1);
        return NewApptDate;
    }
    private void BindProviderList()
    {
        try
        {
            ddlreferringphysician.Items.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@intHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            if (dropReferredType.SelectedValue != "")
            {
                if (dropReferredType.SelectedValue == "2")
                {
                    HashIn.Add("@chvRefName", "Provider");
                }
                else if (dropReferredType.SelectedValue == "1")
                {
                    HashIn.Add("@chvRefName", "Organization");
                }
                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetProviderList", HashIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (dropReferredType.SelectedValue == "2")
                    {
                        ddlreferringphysician.SelectedIndex = -1;
                        ddlreferringphysician.DataSource = ds;
                        ddlreferringphysician.DataTextField = "Name";
                        ddlreferringphysician.DataValueField = "ContactID";
                        ddlreferringphysician.DataBind();
                    }
                    else
                    {
                        ddlreferringphysician.SelectedIndex = -1;
                        ddlreferringphysician.DataSource = ds;
                        ddlreferringphysician.DataTextField = "CompanyName";
                        ddlreferringphysician.DataValueField = "ContactID";
                        ddlreferringphysician.DataBind();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkAddName_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/PRegistration/Status.aspx?CtrlDesp=ExternalDoctor&CountId=" + dropReferredType.SelectedValue.ToString() + "";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        ViewState["Tag"] = "ExternalDoctor";

        RadWindowForNew.OnClientClose = "BindCombo";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnFillCombo_Click(object sender, EventArgs e)
    {
        try
        {
            fillReferalBy();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindRefType()
    {
        try
        {
            dropReferredType.Items.Clear();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet dsReferal = dl.FillDataSet(CommandType.StoredProcedure, "uspGetReferby");
            foreach (DataRowView drReferal in dsReferal.Tables[0].DefaultView)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)drReferal["ReferralTypeName"];
                item.Value = drReferal["ReferralType"].ToString();
                item.Attributes.Add("Id", common.myStr(drReferal["Id"]));
                dropReferredType.Items.Add(item);
                item.DataBind();

            }
            RadComboBoxItem rci = new RadComboBoxItem("Select");
            dropReferredType.Items.Insert(0, rci);

            dropReferredType.SelectedValue = "S";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void RefType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lnkAddName.Visible = false;
            if (dropReferredType.SelectedValue == "S")
            {
                SReferredType.Visible = false;
                sRefPhy.Visible = false;
            }
            else
            {
                SReferredType.Visible = true;
                sRefPhy.Visible = true;
            }
            if (dropReferredType.SelectedValue.ToString() == "D" || dropReferredType.SelectedValue.ToString() == "H")
                if (common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "ConnectedToCentralDB", sConString)) == "Y")
                    lnkAddName.Visible = true;


            fillReferalBy();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void fillReferalBy()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsInput = new Hashtable();
        hsInput.Add("intHospitalId", Session["HospitalLocationID"]);
        hsInput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
        hsInput.Add("chvRefType", dropReferredType.SelectedValue);

        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetReferByDoctor", hsInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlreferringphysician.Text = "";
            ddlreferringphysician.Items.Clear();
            RadComboBoxItem item = new RadComboBoxItem();
            item.Value = "0";
            item.Text = "Select";
            ddlreferringphysician.Text = "";
            ddlreferringphysician.Items.Add(item);

            ddlreferringphysician.DataSource = ds;
            ddlreferringphysician.DataTextField = "Name";
            ddlreferringphysician.DataValueField = "Id";
            ddlreferringphysician.DataBind();

        }
        else
        {
            ddlreferringphysician.Text = "";
            ddlreferringphysician.Items.Clear();
            ddlreferringphysician.DataSource = null;
            ddlreferringphysician.DataBind();
        }
    }
    protected void RadTimeFrom_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        // int Hourtime=0;
        //if (RadTimeFrom.SelectedDate.Value.ToString("tt").ToString() == "PM")
        //{
        //    Hourtime = int.Parse(RadTimeFrom.SelectedDate.Value.ToString("hh")) + 12;
        //    if (Hourtime == 24)
        //    {
        //        Hourtime = 12;
        //    }
        //}
        //else
        //{
        //    Hourtime = int.Parse(RadTimeFrom.SelectedDate.Value.ToString("hh")) ;
        //}

        //RadTimeTo.TimeView.StartTime = new TimeSpan(Hourtime, int.Parse(RadTimeFrom.SelectedDate.Value.ToString("mm")) + Convert.ToInt32(Request.QueryString["TimeInterval"]), 0);

        //RadTimeTo.SelectedDate = Convert.ToDateTime(RadTimeTo.TimeView.StartTime.ToString());// Convert.ToDateTime(Request.QueryString["FromTimeHour"] + ":" + Request.QueryString["FromTimeMin"]).AddMinutes(Convert.ToInt32(Request.QueryString["TimeInterval"]));
        if (common.myInt(Request.QueryString["appid"]) != 0)
        {
            if (ViewState["Duration"] != null)
            {
                TimeSpan ts = (TimeSpan)ViewState["Duration"];
                RadTimeTo.DbSelectedDate = RadTimeFrom.SelectedDate.Value.Add(ts);
            }
        }
    }

    protected void btnPayment_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/billing/paymentsadd.aspx?RegistrationId=" + ViewState["RegistrationId"];
        RadWindowForNew.Height = 530;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Payment";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
    }
    private void populateAppointmentStatus()
    {
        try
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            String SqlStr = "Select * From GetStatus(@HospitalLocationId,'Appointment')";
            DataSet objDs = dl.FillDataSet(CommandType.Text, SqlStr, hshInput);
            ddlAppointmentStatus.DataSource = objDs;
            ddlAppointmentStatus.DataTextField = "Status";
            ddlAppointmentStatus.DataValueField = "StatusId";
            ddlAppointmentStatus.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnCalAge_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BaseC.ParseData bc = new BaseC.ParseData();
            Hashtable hshIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            String SqlStr = "select dbo.DOBFromAge(" + val(bc.ParseQ(txtAgeYears.Text)) + "," + val(bc.ParseQ(txtAgeMonths.Text)) + "," + val(bc.ParseQ(txtAgeDays.Text)) + ")";

            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, SqlStr, hshIn);
            dr.Read();
            hdnCalculateDOB.Value = common.myDate(dr[0]).ToString("dd/MM/yyyy");
            //dtpDate.SelectedDate = DateTime.Now;
            //dtpDOB.SelectedDate = Convert.ToDateTime(dr[0].ToString());
            ddlGender.Focus();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    Double val(String value)
    {
        Double intData = 0;
        Boolean blnTemp = Double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat, out intData);
        return intData;
    }
    protected void dtpDOB_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);
            txtAgeYears.Text = "";
            txtAgeMonths.Text = "";
            txtAgeDays.Text = "";
            string[] result = bC.CalculateAge(dtpDOB.SelectedDate.Value.ToShortDateString());

            if (result.Length == 2)
            {
                if (result[1] == "Yr")
                {
                    txtAgeYears.Text = result[0];
                }
                else if (result[1] == "Mnth")
                {
                    txtAgeMonths.Text = result[0];

                }
                else
                {
                    txtAgeDays.Text = result[0];
                }
            }

            if (result.Length == 4)
            {
                //txtAgeYears.Text = result[0];
                txtAgeMonths.Text = result[0];
                txtAgeDays.Text = result[2];
            }
            if (result.Length == 6)
            {
                txtAgeYears.Text = result[0];
                txtAgeMonths.Text = result[2];
                txtAgeDays.Text = result[4];
            }

            if (txtAgeYears.Text == "")
            {
                txtAgeYears.Text = "0";
            }
            if (txtAgeMonths.Text == "")
            {
                txtAgeMonths.Text = "0";
            }
            if (txtAgeDays.Text == "")
            {
                txtAgeDays.Text = "0";
            }

            ddlGender.Focus();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void imgCalYear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);
            txtAgeYears.Text = "";
            txtAgeMonths.Text = "";
            txtAgeDays.Text = "";
            DateTime datet = dtpDOB.SelectedDate.Value;
            string[] result = bC.CalculateAge(datet.ToString("MM/dd/yyyy"));

            if (result.Length == 2)
            {
                if (result[1] == "Yr")
                {
                    txtAgeYears.Text = result[0];
                }
                else if (result[1] == "Mnth")
                {
                    txtAgeMonths.Text = result[0];

                }
                else
                {
                    txtAgeDays.Text = result[0];
                }
            }

            if (result.Length == 4)
            {
                //txtAgeYears.Text = result[0];
                txtAgeMonths.Text = result[0];
                txtAgeDays.Text = result[2];
            }
            if (result.Length == 6)
            {
                txtAgeYears.Text = result[0];
                txtAgeMonths.Text = result[2];
                txtAgeDays.Text = result[4];
            }

            if (txtAgeYears.Text == "")
            {
                txtAgeYears.Text = "0";
            }
            if (txtAgeMonths.Text == "")
            {
                txtAgeMonths.Text = "0";
            }
            if (txtAgeDays.Text == "")
            {
                txtAgeDays.Text = "0";
            }


            ddlGender.Focus();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void PopulateRoom()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hshInput = new Hashtable();

            hshInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
            hshInput.Add("@intFacilityID", ddlFacility.SelectedValue);
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetRoomNo", hshInput);

            //ddlRoom.Items.Clear();
            //ddlRoom.DataSource = objDs;
            //ddlRoom.DataTextField = "RoomNo";
            //ddlRoom.DataValueField = "RoomID";
            //ddlRoom.DataBind();
            //ddlRoom.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void populateFacility()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active=1";

            ddlFacility.Items.Clear();
            ddlFacility.DataSource = dv;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "Name";
            ddlFacility.DataBind();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));


            ddlSelectionFacility.Items.Clear();
            ddlSelectionFacility.DataSource = dv;
            ddlSelectionFacility.DataValueField = "FacilityID";
            ddlSelectionFacility.DataTextField = "Name";
            ddlSelectionFacility.DataBind();
            ddlSelectionFacility.Items.Insert(0, new RadComboBoxItem("All", "0"));

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void populateProvider()
    {
        try
        {
            DataSet objDs;
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            if (common.myInt(Session["FacilityId"]) != 0)
            {
                hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            }
            else
            {
                hshInput.Add("@intFacilityId", common.myInt(ddlFacility.SelectedValue));
            }
            if (!common.myBool(ViewState["IsDoctor"]))
            {
                hshInput.Add("@IsDoctor", false);
                //lblProOrResour.Text = "Calender ";
            }
            else
            {
                hshInput.Add("@IsDoctor", true);
                //lblProOrResour.Text = "Provider ";
            }

            objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetResourceList", hshInput);

            if (objDs.Tables[0].Rows.Count > 0)
            {
                ddlProvider.Items.Clear();
                ddlProvider.DataSource = objDs;
                ddlProvider.DataValueField = "DoctorId";
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataBind();
                ddlProvider.Items.Insert(0, new RadComboBoxItem("", "0"));
                if (common.myBool(ViewState["IsDoctor"]))
                {
                    ddlProvider.SelectedValue = common.myStr(Request.QueryString["doctorId"]);
                }

            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void populateReferringDoctor()
    {
        try
        {
            // ddlReferringDoctor.DataSource = null;
            // ddlReferringDoctor.DataBind();
            DataSet objDs;
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            if (common.myInt(Session["FacilityId"]) != 0)
            {
                hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            }
            else
            {
                hshInput.Add("@intFacilityId", common.myInt(ddlFacility.SelectedValue));
            }
            hshInput.Add("@intSpecialisationId", 0);
            
            objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);

            DataTable dtResource = (DataTable)objDs.Tables[0];
            DataView dv = (DataView)dtResource.DefaultView;
            dv.RowFilter = "IsDoctor=1";

            //ddlReferringDoctor.DataSource = dv.ToTable();

            //ddlReferringDoctor.DataValueField = "DoctorId";
            //ddlReferringDoctor.DataTextField = "DoctorName";
            //ddlReferringDoctor.DataBind();

            //ddlReferringDoctor.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void txtRegNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnregno.Value != "B")
            {
                ddlAppointmentType.SelectedIndex = 0;
                txtName.Text = "";
                dtpDOB.SelectedDate = null;
                txtAgeYears.Text = "";
                txtAgeMonths.Text = "";
                txtAgeDays.Text = "";
                ddlGender.SelectedIndex = 0;
                txtPhoneHome.Text = "";
                txtMobile.Text = "";
                ddlIdentityType.SelectedIndex = 0;
                txtIdentityNo.Text = "";
                txtEmail.Text = "";
                txtRemarks.Text = "";
                rblReferralDoctorType.Items[0].Selected = true;
                ddlReferralDoctor.SelectedIndex = 0;
            }
            hdnregno.Value = "";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void getDoctorID()
    {
        try
        {
            BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
            SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));
            if (dr.HasRows == true)
            {
                dr.Read();
                String strSelID = dr[0].ToString();
                if (strSelID != "")
                {
                    char[] chArray = { ',' };
                    string[] serviceIdXml = strSelID.Split(chArray);
                    ViewState["DoctorID"] = serviceIdXml[0].ToString();

                }
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = (string)dr["Type"];
                        item.Value = dr["VisitTypeID"].ToString();
                        item.Attributes.Add("IsPackageVisit", common.myStr(dr["Code"]));
                        ddlAppointmentType.Items.Add(item);
                        item.DataBind();
                    }
                }
            }
            else
            {
                BaseC.Appointment appoint = new BaseC.Appointment(sConString);
                DataTable dt = appoint.CheckServiceCoverForClient(Convert.ToInt16(Request.QueryString["FacilityId"]), iPlanTypeId);
                foreach (DataRow dr in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dr["Type"];
                    item.Value = dr["VisitTypeID"].ToString();
                    item.Attributes.Add("IsPackageVisit", common.myStr(dr["IsPackageVisit"]));
                    ddlAppointmentType.Items.Add(item);
                    item.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateDuration()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(2000);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@inyHospitalLocatiOnID", Session["HospitalLocationID"]);
            hshIn.Add("@intDoctorID", ViewState["DoctorID"].ToString());
            hshIn.Add("@intVisitTypeId", "0");

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetDoctorVisitDuration", hshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Duration"] = ds.Tables[0].Rows[0]["Duration"];
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void ShowAppointmentData()
    {
        try
        {
            //dropReferredType.Items.Clear();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Patient objdob = new BaseC.Patient(sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(2000);
            BaseC.ParseData parse = new BaseC.ParseData();
            Hashtable hsInput = new Hashtable();
            hsInput.Add("@intAppointmentId", Request.QueryString["appid"]);
            hsInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "USPEMRGetAppointmentDetails", hsInput);

            if (dr.HasRows == true)
            {
                dr.Read();

                if (common.myStr(dr["RegistrationNo"]).Trim().Length > 0)
                {
                    txtAccountNumber.Text = common.myStr(dr["RegistrationNo"]).Trim();
                }

                ddlFacility.SelectedValue = dr["FacilityID"].ToString();//
                ddlProvider.SelectedValue = dr["DoctorId"].ToString(); //
                hdnERegistrationId.Value = dr["RegistrationId"].ToString();
                Session["FacilityId"] = dr["FacilityID"].ToString();
                RadTimeFrom.DbSelectedDate = dr["FromTime"].ToString().Trim();
                RadTimeTo.DbSelectedDate = dr["ToTime"].ToString().Trim();
                ViewState["ToTime"] = dr["ToTime"].ToString();
                ViewState["FromTime"] = dr["FromTime"].ToString();
                //Added by vineet/////////////////////////////////
                TimeSpan ts = RadTimeTo.SelectedDate.Value - RadTimeFrom.SelectedDate.Value;
                ViewState["Duration"] = ts;
                ///////////////////////////////////
                hdnPatientCategoryId.Value = dr["CompanyCode"].ToString();
                hdnCompanyId.Value = dr["CompanyId"].ToString();
                hdnPlanTypeId.Value = dr["PlanTypeID"].ToString();
                if (hdnPatientCategoryId.Value == "FFS" && Convert.ToInt16(hdnPlanTypeId.Value) > 0)
                {
                    populateVisitType(true, Convert.ToInt16(hdnPlanTypeId.Value));
                }
                else
                {
                    populateVisitType(false, 0);
                }
                ddlAppointmentType.SelectedValue = dr["VisitTypeId"].ToString();

                trSubDept.Visible = false;
                trService.Visible = false;
                if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P" || common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "H")
                {
                    trSubDept.Visible = true;
                    trService.Visible = true;
                    ddlAppointmentType_SelectedIndexChanged(null, null);
                    ddlSubDept.SelectedValue = dr["SubDeptId"].ToString();
                    ddlSubDept_OnSelectedIndexChanged(null, null);
                    ddlService.SelectedValue = dr["PackageId"].ToString();
                }
                dtpDate.SelectedDate = Convert.ToDateTime(Request.QueryString["appDate"]);
                txtName.Text = dr["LastName"].ToString();
                txtFName.Text = dr["FirstName"].ToString();
                txtMName.Text = dr["MiddleName"].ToString();
                txtMobile.Text = dr["MobileNo"].ToString();
                txtNote.Text = dr["Note"].ToString();
                //string sDateOfBirth = Convert.ToDateTime(dr["DateofBirth"]).Date.ToString("dd/MM/yyyy");
                if (dr["DateofBirth"].ToString() != "")
                {
                    dtpDOB.SelectedDate = Convert.ToDateTime(dr["DateofBirth"]);
                }
                ////ddlRoom.SelectedValue = dr["RoomID"].ToString();
                //ddlcasename.SelectedIndex = ddlcasename.Items.IndexOf(ddlcasename.Items.FindItemByValue(dr["DefaultPaymentCaseId"].ToString().Trim()));
                ////txtauthorization.Text = dr["AuthorizationNo"].ToString().Trim();

                if (common.myInt(dr["AgeYear"]) > 0)
                {
                    txtAgeYears.Text = dr["AgeYear"].ToString();
                }

                if (common.myInt(dr["AgeMonth"]) > 0)
                {
                    txtAgeMonths.Text = dr["AgeMonth"].ToString();
                }

                if (common.myInt(dr["AgeDays"]) > 0)
                {
                    txtAgeDays.Text = dr["AgeDays"].ToString();
                }

                ddlGender.SelectedValue = dr["Gender"].ToString();
                txtPhoneHome.Text = dr["PhoneHome"].ToString();
                txtEmail.Text = dr["Email"].ToString();
                txtRemarks.Text = dr["Remarks"].ToString();

                chkWalkIn.Checked = common.myBool(dr["walkinpatient"]);
                txtreason.Text = dr["Remarks"].ToString();

                if (Request.QueryString["AppId1"] != null)
                {
                    string[] strAppId = common.myStr(Request.QueryString["AppId1"]).Split('_');

                    if (strAppId.Length > 1)
                    {
                        ddlAppointmentStatus.SelectedValue = "1";
                    }
                    else
                    {
                        ddlAppointmentStatus.SelectedValue = dr["StatusId"].ToString();
                        hdnStatusCode.Value = dr["StatusCode"].ToString();
                    }
                }
                //    dropReferredType.SelectedValue = dr["ReferredTypeID"].ToString();
                dropReferredType.SelectedValue = dr["ReferralType"].ToString();

                RefType_SelectedIndexChanged(null, null);
                //BindRefType();

                if (dropReferredType.SelectedValue != "0" && dropReferredType.SelectedValue != "")
                {
                    //BindProviderList();
                    ddlreferringphysician.SelectedValue = dr["ReferredByID"].ToString();
                }

                else
                {
                    ddlreferringphysician.Items.Insert(0, new RadComboBoxItem("Select"));
                    ddlreferringphysician.Items[0].Value = "0";
                }
                populateReferringDoctor();
                ddlAppointmentStatus.Enabled = false;

                //if (common.myInt(dr["ReferrerDoctor"]) > 0)
                //{
                //    //ddlReferringDoctor.SelectedValue = dr["ReferrerDoctor"].ToString();
                //    ddlReferringDoctor.SelectedIndex = ddlReferringDoctor.Items.IndexOf(ddlReferringDoctor.Items.FindItemByValue(common.myStr(dr["ReferrerDoctor"])));
                //}
                hsInput = new Hashtable();
                if (common.myStr(dr["RegistrationNo"]).Trim().Length > 0)
                {
                    hsInput.Add("@RegistrationId", common.myStr(dr["RegistrationId"]));
                    string strSQL = "Select Note, ShowNoteInAppointment from RegistrationOtherDetail where RegistrationId = @RegistrationId";
                    DataSet ds = dl.FillDataSet(CommandType.Text, strSQL, hsInput);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["ShowNoteInAppointment"].ToString() != "")
                        {
                            if (common.myBool(ds.Tables[0].Rows[0]["ShowNoteInAppointment"]))
                            {
                                txtNote.Text = ds.Tables[0].Rows[0]["Note"].ToString();
                            }
                        }

                        else
                        {
                            txtNote.Text = common.myStr(dr["Note"]);
                        }
                    }
                    else
                    {
                        txtNote.Text = common.myStr(dr["Note"]);
                    }
                }
                RadSchedulerRecurrenceEditor1.RecurrenceRuleText = dr["RecurrenceRule"].ToString();
                RadSchedulerRecurrenceEditor1.DataBind();

                fillGridData();

            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlrange_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fillGridData();
    }


    //------Start newely added (event for appointment list on same page) ---------------

    protected void gvAppointment_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        Int32 rowind = gvRow.RowIndex;
        if (e.CommandName == "Print")
        {

            HiddenField hdnRegistrationId = (HiddenField)gvRow.FindControl("hdnRegistrationId");
            HiddenField hdnAppointmentId = (HiddenField)gvRow.FindControl("hdnAppointmentId");

            RadWindowForNew.NavigateUrl = "/EMRReports/AppointmentSlip.aspx?RegistrationId=" + common.myInt(hdnRegistrationId.Value) + "&AppointmentId=" + common.myInt(hdnAppointmentId.Value);
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
        if (e.CommandName == "CancelApp")
        {
            HiddenField hdnAppointmentId = (HiddenField)gvRow.FindControl("hdnAppointmentId");
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsStatus = new DataSet();
            bool bStatus = false;
            ViewState["AppId"] = hdnAppointmentId.Value.Trim();
            string strStatus = "select d.StatusID,s.Code from DoctorAppointment D with (nolock) inner join StatusMaster s on d.StatusId = s.StatusId where d.AppointmentID =" + hdnAppointmentId.Value;

            dsStatus = dl.FillDataSet(CommandType.Text, strStatus);
            if (dsStatus.Tables[0].Rows.Count > 0)
            {
                //if (Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString()) == 3 || Convert.ToInt32(dsStatus.Tables[0].Rows[0]["StatusID"].ToString()) == 5)
                if (common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "P" || common.myStr(dsStatus.Tables[0].Rows[0]["Code"].ToString()) == "PC")
                {
                    bStatus = true;
                }
            }

            if (bStatus == true)
            {
                Alert.ShowAjaxMsg("Appointment Already Checked-In. Appointment Cannot Be Deleted.", Page.Page);
                return;
            }
            else
            {
                dvDelete.Visible = true;
                fillRemarks();
            }
        }
    }

    protected void gvAppointment_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAppDate = (Label)e.Row.FindControl("lblAppDate");
            HiddenField hdnStatusCode = (HiddenField)e.Row.FindControl("hdnStatusCode");
            if (common.myDate(common.myStr(lblAppDate.Text).Trim()) < common.myDate(DateTime.Now.Date)
                || hdnStatusCode.Value == "PC")
            {

                ImageButton lnkCancel = (ImageButton)e.Row.FindControl("ibtnDelete");

                lnkCancel.Visible = false;
            }
        }
    }

    protected void fillGridData()
    {
        try
        {
            lblMessage.Text = "";
            Hashtable hsInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsInput.Add("@intLoginFacilityId", common.myInt(Session["FacilityId"]));
            hsInput.Add("@intRegistrationId", common.myInt(ViewState["NewRegID"]));
            hsInput.Add("@cShowAll", common.myStr(ddlrange.SelectedValue));

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspAppointmentSlip", hsInput);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    ds.AcceptChanges();
                }
                gvAppointment.DataSource = ds.Tables[0];
                gvAppointment.DataBind();

                //}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

        }
    }

    protected void btnDeleteApp_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Appointment appoint = new BaseC.Appointment(sConString);

            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            if (ddlRemarkss.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Please select the cancellation reason", Page);
                return;
            }



            //Hashtable hshIn = new Hashtable();
            hshIn.Add("@intAppointmentId", common.myInt(ViewState["AppId"]));
            hshIn.Add("@intLastChangedBy", Session["UserId"]);
            hshIn.Add("@chvCancelRemark", txtCancel.Text);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", hshIn, hshOut);
            //  Alert.ShowAjaxMsg(hshOut["@chvErrorStatus"].ToString(), Page);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = hshOut["@chvErrorStatus"].ToString();
            fillGridData();
            dvDelete.Visible = false;


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        dvDelete.Visible = false;
    }

    protected void fillRemarks()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        DataSet ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "AppCancel", "AppCancel");

        ddlRemarkss.Items.Clear();

        ddlRemarkss.DataSource = ds;
        ddlRemarkss.DataTextField = "Status";
        ddlRemarkss.DataValueField = "StatusId";
        ddlRemarkss.DataBind();
        ddlRemarkss.Items.Insert(0, "Select a Reason");
        ddlRemarkss.Items[0].Value = "0";
    }

    //------End newely added event for appointment list on same page ---------------
    ////private void fillInsuranceData(int registrationNo)
    ////{
    ////    ManageInsurance objMi = new ManageInsurance();
    ////    DataSet ds = objMi.FillPreviousDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
    ////        common.myInt(common.myInt(registrationNo)));
    ////    gvPreviousDetails.DataSource = ds;
    ////    gvPreviousDetails.DataBind();
             
    ////}
    ////private void FillRegistrationDAtaofPatientsFromMobileNo(int RegsistrationNo, string mobileno)
    ////{
    ////    ManageInsurance objMi = new ManageInsurance();
    ////    DataSet ds = objMi.FindPatientusingMobileNo(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
    ////        common.myStr(mobileno),common.myInt(common.myInt(RegsistrationNo)));
    ////    gvRegistrationMobileDetails.DataSource = ds;
    ////    gvRegistrationMobileDetails.DataBind();
    ////}
    private DataTable GetData(string text)
    {
        DataSet dataset = new DataSet();
        try
        {
            string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
            hashtable.Add("@strName", text);
            dataset = objSave.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", hashtable);
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dataset.Tables[0];
    }

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

    protected void btnsave_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (ddlService.SelectedValue != "" && ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"] == "H")
            {
                BaseC.Package package = new Package(sConString);
                ds = package.GetPackageServiceLimit(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlService.SelectedValue),
                    common.myInt(hdnCompanyId.Value), common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "The detail of this package does not define in package detail";
                    Alert.ShowAjaxMsg("The detail of this package does not define in package detail..", Page);
                    return;
                }
            }

            BaseC.Appointment appoint = new BaseC.Appointment(sConString);
            if (dtpDate.SelectedDate == null || dtpDate.SelectedDate.ToString() == "")
            {
                Alert.ShowAjaxMsg("Please select appointment date..", Page.Page);
                return;
            }
            string sBreakId = appoint.ExistBreakAndBlock(0, Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt32(ddlProvider.SelectedValue),
                Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"), RadTimeFrom.SelectedDate.Value.ToString("HH:mm"), RadTimeTo.SelectedDate.Value.ToString("HH:mm"));

            if (sBreakId != "" && sBreakId != null)
            {
                Alert.ShowAjaxMsg("Appointment not Allow in this time.", Page);
                return;
            }


            //string sAppointmentId = appoint.ExistSameDoctorAppointmentForPatient(Convert.ToInt16(Session["HospitalLocationId"]), ViewState["NewRegID"] == null ? 0 : Convert.ToInt32(ViewState["NewRegID"]),
            //Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToInt16(ddlProvider.SelectedValue), Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
            //RadTimeFrom.SelectedDate.Value.ToString("HH:mm"), RadTimeTo.SelectedDate.Value.ToString("HH:mm"));
            //if (sAppointmentId != "" && sAppointmentId != null)
            //{
            //    Alert.ShowAjaxMsg("Appointment already exist to another doctor.", Page);
            //    return;
            //}
            //sAppointmentId = appoint.ExistDoctorAppointmentForPatient(Convert.ToInt16(Session["HospitalLocationId"]), ViewState["NewRegID"] == null ? 0 : Convert.ToInt32(ViewState["NewRegID"]),
            //Convert.ToInt16(ddlFacility.SelectedValue), Convert.ToDateTime(dtpDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
            //RadTimeFrom.SelectedDate.Value.ToString("HH:mm"), RadTimeTo.SelectedDate.Value.ToString("HH:mm"));
            //if (sAppointmentId != "" && sAppointmentId != null)
            //{
            //    Alert.ShowAjaxMsg("Already Appointment for this patient in this time", Page);
            //    return;
            //}
            if ((ddlAppointmentStatus.SelectedValue == "3" || ddlAppointmentStatus.SelectedValue == "5")
                && common.myInt(FindFutureDate(Convert.ToString(dtpDate.SelectedDate))) > common.myInt(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
            {
                string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                Alert.ShowAjaxMsg(strMessage, Page);
                return;
            }
            BaseC.Patient formatdate = new BaseC.Patient(sConString);

            string txt = txtAccountNumber.Text;

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
            if (common.myInt(Request.QueryString["appid"]) != 0)
            {
                string strOrigTime = "select dbo.GetDateFormat(Fromtime,'T') AS FromTime from DoctorAppointment where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));

            }
            string exdate = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
            string rule = "";
            string RecParentId = "";
            if (!string.IsNullOrEmpty(RadSchedulerRecurrenceEditor1.RecurrenceRuleText))
            {
                rule = RadSchedulerRecurrenceEditor1.RecurrenceRule.ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["recparentid"]))
            {
                RecParentId = Request.QueryString["recparentid"].ToString();
                if (RecParentId == "0")
                {
                    RecParentId = "";
                }
            }
            if (ddlAppointment.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please select the appointment.", Page);
                return;
            }
            //if (common.myInt(ddlReferringDoctor.SelectedValue) == 0)
            //{
            //    Alert.ShowAjaxMsg("Please select a Billing Provider", Page);
            //    return;
            //}
            #endregion




            int intTo = 0, intFrom = 0;
            String FromTime = RadTimeFrom.SelectedDate.Value.ToString("HH:mm");

            String FromTo = RadTimeTo.SelectedDate.Value.ToString("HH:mm");
            if (FromTime.Contains(":") == true)
            {
                string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                intFrom = common.myInt(strFromTime);
            }

            if (FromTo.Contains(":") == true)
            {

                string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                intTo = common.myInt(strFromTo);
            }

            if (intTo <= intFrom)
            {
                Alert.ShowAjaxMsg("Please change the To Time", Page);
                return;
            }

            if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P" || common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "H")
            {
                if (common.myInt(ddlService.SelectedValue) == 0 || common.myStr(ddlService.SelectedValue) == "")
                {
                    Alert.ShowAjaxMsg("Please Select Package or Procedure Service...", Page);
                    return;
                }
                else
                {
                    hshtablein.Add("@iPackageId", common.myInt(ddlService.SelectedValue));
                }
            }


            hshtablein.Add("chrAppFromTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"));
            hshtablein.Add("chrAppToTime", RadTimeTo.SelectedDate.Value.ToString("hh:mmtt")); //RadTimeTo.DbSelectedDate.ToString("hh:mm tt"));
            hshtablein.Add("intVisitTypeId", ddlAppointmentType.SelectedValue.ToString());
            hshtablein.Add("@chvLastName", pdDataCheck.ParseQ(txtName.Text));
            hshtablein.Add("@chvFirstName", pdDataCheck.ParseQ(txtFName.Text));
            hshtablein.Add("@chvMiddleName", pdDataCheck.ParseQ(txtMName.Text));
            hshtablein.Add("inyGender", ddlGender.SelectedValue); //ddlGender.Text.Substring(0, 1));
            //if (ddlcasename.SelectedValue != "")
            //{
            hshtablein.Add("@intPaymentCaseId", null);
            //}
            hshtablein.Add("@chvAuthorizationNo", "");           
           
            ////if (ddlRoom.SelectedValue != "0")
            ////{
            ////    hshtablein.Add("intRoomId", ddlRoom.SelectedValue);
            ////}


            if (dtpDOB.SelectedDate == null && (txtAgeYears.Text == "0" || txtAgeYears.Text == ""))
            {
                Alert.ShowAjaxMsg("Please enter Date Of Birth or Age", Page);
                return;
            }
            else if (dtpDOB.SelectedDate == null)
            {
                if (txtAgeYears.Text != "")
                {
                    hshtablein.Add("chrDOB", hdnCalculateDOB.Value);
                    hshtablein.Add("intAgeYear", txtAgeYears.Text);
                    hshtablein.Add("@bitRealDateOfBirth", 0);
                }
            }
            else if (dtpDOB.SelectedDate != null)
            {
                hshtablein.Add("chrDOB", formatdate.FormatDateDateMonthYear(dtpDOB.SelectedDate.Value.ToShortDateString()));
                hshtablein.Add("intAgeYear", txtAgeYears.Text);
                hshtablein.Add("@bitRealDateOfBirth", 1);
            }

            if (Session["FollowUpRegistrationId"] != null)
            {
                hshtablein.Add("@iSourceEncounterId", common.myStr(Session["EncounterId"]));
            }
            hshtablein.Add("@IsDoctor", common.myBool(ViewState["IsDoctor"]));
            hshtablein.Add("@intReferrerDoctor", null);

            hshtablein.Add("intAgeMonth", txtAgeMonths.Text);
            hshtablein.Add("intAgeDay", txtAgeDays.Text);

            if (txtPhoneHome.Text.Trim() != "___-___-____")
            {
                hshtablein.Add("chvResidencePhone", txtPhoneHome.Text);
            }
            else
            {
                hshtablein.Add("chvResidencePhone", DBNull.Value);
            }

            if (txtreason.Text.Trim().Length > 0)
            {
                hshtablein.Add("chvRemarks", pdDataCheck.ParseQ(txtreason.Text.Trim()));

            }
            else
            {
                hshtablein.Add("chvRemarks", DBNull.Value);
            }

            if (txtMobile.Text.Trim() != "___-___-____")
            {
                hshtablein.Add("chvMobile", txtMobile.Text);
            }
            else
            {
                hshtablein.Add("chvMobile", DBNull.Value);
            }

            if (txtEmail.Text.Trim().Length > 0)
            {
                hshtablein.Add("chvEmail", pdDataCheck.ParseQ(txtEmail.Text));
            }
            else
            {
                hshtablein.Add("chvEmail", DBNull.Value);
            }

            hshtablein.Add("bitWalkInPatient", chkWalkIn.Checked);
            hshtablein.Add("intFacilityId", Convert.ToInt16(ddlFacility.SelectedValue));
            hshtablein.Add("intStatusId", ddlAppointmentStatus.SelectedValue);
            if (dropReferredType.SelectedValue == "0" || dropReferredType.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please Select Referred Type", Page);
                return;
            }
            else
            {
                hshtablein.Add("intReferredType ", dropReferredType.SelectedItem.Attributes["Id"]);
            }
            if ((ddlreferringphysician.SelectedValue == "0" || ddlreferringphysician.SelectedValue == "") && dropReferredType.SelectedValue != "S")
            {
                Alert.ShowAjaxMsg("Please Select Referring Physician", Page);
                return;
            }
            else
            {
                hshtablein.Add("intreferringphysician ", ddlreferringphysician.SelectedValue);
            }



            hshtablein.Add("@intLoginFacilityId", Session["FacilityId"]);
            hshtablein.Add("@intPageId", 3);  // Request.QueryString["PageId"].ToString().Substring(1)
            hshtablein.Add("@chvNote", common.myStr(txtNote.Text));
            if (ddlAppointmentStatus.SelectedValue == "34")
            {
                hshtablein.Add("@chvCancelRemark", common.myStr(txtNote.Text));
            }
            hshtablein.Add("chrAppointmentDate", formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()));  //Request.QueryString["appDate"]);
            if (common.myInt(Request.QueryString["appid"]) == 0 || RecParentId != "")
            {
                //hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);

                hshtablein.Add("intDoctorId", ddlProvider.SelectedValue);
                //Request.QueryString["doctorid"]);
                //hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                bool val = true;
                if (rule != "")//Editing Whole Recurring Appointment 
                {
                    if (common.myInt(Request.QueryString["appid"]) != 0)
                    {
                        hshtablein.Add("intLastChangedBy", Session["UserID"].ToString());
                        hshtablein.Add("intAppointmentID", Request.QueryString["appid"]);
                        hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                        if (rule != null && rule != "")
                        {
                            hshtablein.Add("nchvRecurrenceRule", rule);
                        }
                        else
                        {
                            hshtablein.Add("nchvRecurrenceRule", "");
                        }

                        hshtablein.Add("RecurrenceParentId", "");
                        hshtableout.Add("@bitReturn", SqlDbType.Bit);
                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspModifyDoctorAppointment", hshtablein, hshtableout);
                        lblMessage.Text = hshtableout["@chvErrorStatus"].ToString();


                        if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                        {
                            //Alert.ShowAjaxMsg(hshtableout["@chvErrorStatus"].ToString(), Page);
                            return;
                        }
                        val = false;

                    }
                    else
                    {
                        hshtablein.Add("nchvRecurrenceRule", rule);
                        hshtablein.Add("RecurrenceParentId", DBNull.Value);
                    }
                }
                else if (rule == "" && RecParentId != "")
                {
                    val = true;
                    if (common.myInt(Request.QueryString["appid"]) != 0)
                    {
                        string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                        string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                    }
                    hshtablein.Add("nchvRecurrenceRule", "");
                    hshtablein.Add("RecurrenceParentId", common.myInt(RecParentId));

                }

                else if (rule == "" && RecParentId == "" && common.myInt(Request.QueryString["appid"]) == 0)
                {
                    val = true;
                    hshtablein.Add("nchvRecurrenceRule", "");
                    hshtablein.Add("RecurrenceParentId", DBNull.Value);
                }
                else if (RecParentId == "" && common.myInt(Request.QueryString["appid"]) != 0)
                {
                    hshtablein.Add("intLastChangedBy", Session["UserID"].ToString());
                    hshtablein.Add("intAppointmentID", Request.QueryString["appid"]);

                    hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                    if (rule != null && rule != "")
                    {
                        hshtablein.Add("nchvRecurrenceRule", rule);
                    }
                    else
                    {
                        hshtablein.Add("nchvRecurrenceRule", "");
                    }

                    hshtablein.Add("RecurrenceParentId", "");
                    hshtableout.Add("@bitReturn", SqlDbType.Bit);
                    hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspModifyDoctorAppointment", hshtablein, hshtableout);
                    lblMessage.Text = hshtableout["@chvErrorStatus"].ToString();
                    if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                    {
                        //Alert.ShowAjaxMsg(hshtableout["@chvErrorStatus"].ToString(), Page);
                        return;
                    }

                }
                if (val)
                {


                    if (Session["RecApptID"] == null)
                    {
                        hshtablein.Add("intRegistrationNo", txtAccountNumber.Text);
                        hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                        //hshtablein.Add("intRegistrationNo", txtAccountNumber.Text);
                        hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                        if (txtAccountNumber.Text != "")
                        {
                            hshtablein.Add("@intRegistrationId", common.myInt(txtAccountNumber.Text));
                        }
                        hshtableout.Add("@intAppointmentId", SqlDbType.Int);
                        hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                        hshtableout.Add("@bitReturn", SqlDbType.Bit);
                        if (common.myInt(hdnPPAppointmentId.Value) > 0)
                        {
                            hshtablein.Add("@intPPAppointmentId", common.myInt(hdnPPAppointmentId.Value));
                        }            
            
                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorAppointment", hshtablein, hshtableout);

                        lblMessage.Text = hshtableout["chvErrorStatus"].ToString();
                        if (lblMessage.Text.Trim().ToUpper().Contains("APPOINTMENT CREATED") == true)
                        {

                            if (common.myStr(txtMobile.Text) != "")
                            {
                                string name = txtFName.Text.Trim() + " " + txtMName.Text.Trim() + " " + txtName.Text.Trim();
                                Email SendSms = new Email(sConString);
                                SendSms.SMS(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(txtMobile.Text), "Thank you for contacting Aster Medcity your appointment is confirmed for " + formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()) + " at " + RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt") + " with Dr. " + common.myStr(ddlProvider.SelectedItem.Text));
                            }
                        }
                        // btnPayment.Enabled = false;
                        ViewState["AppointmentID"] = hshtableout["@intAppointmentId"].ToString();
                        Session["RecApptID"] = hshtableout["@intAppointmentId"].ToString();
                        if (rule != "")
                        {
                            string strUpdateStatus = "update DoctorAppointment set StatusId=1 where AppointmentId=" + ViewState["AppointmentID"] + "";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateStatus);

                            if (chkWalkIn.Checked == true)
                            {
                                Hashtable hshIn = new Hashtable();
                                Hashtable hshout = new Hashtable();
                                hshIn.Add("@intAppointmentId", ViewState["AppointmentID"]);
                                hshIn.Add("@intEncodedBy", Session["UserId"]);
                                hshIn.Add("@intStatusId", 3);
                                hshout.Add("@intRecAppointmentId", SqlDbType.Int);

                                hshIn.Add("@dtAppointmentDate", Convert.ToDateTime(dtpDate.SelectedDate.Value.ToShortDateString()));


                                hshout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", hshIn, hshout);

                                string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + hshtableout["@intAppointmentId"].ToString() + "'";
                                OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                                string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + common.myInt(ViewState["AppointmentID"]) + "'";

                                DateTime dtStart = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                                string exdateNew = dtStart.ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";

                                //if (Convert.ToInt64(Request.QueryString["appid"]) != 0)
                                //{                

                                if (!rule.Contains("EXDATE"))
                                {
                                    string UpdateRule = rule + "EXDATE:" + exdateNew;
                                    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + ViewState["AppointmentID"] + "'";
                                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                                }
                                else
                                {
                                    string UpdateRule = rule + "," + exdateNew;
                                    string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + ViewState["AppointmentID"] + "'";
                                    dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                                }
                            }
                        }
                        if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                        {
                            //Alert.ShowAjaxMsg(hshtableout["chvErrorStatus"].ToString(), Page);
                            return;
                        }
                    }
                }
            }
            else
            {
                string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                if (OrigRecRule.Trim() != "") //if Appointment is recurring and editing master appointment
                {
                    //Hashtable hshIn = new Hashtable();
                    //hshIn.Add("@intAppointmentId", Request.QueryString["appid"]);
                    //hshIn.Add("@intEncodedBy", Session["UserID"].ToString());
                    //hshIn.Add("@intStatusId", ddlAppointmentStatus.SelectedValue);
                    //dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", hshIn);
                    if (common.myInt(Request.QueryString["appid"]) != 0)
                    {
                        hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                        hshtablein.Add("intRegistrationNo", txtAccountNumber.Text);
                        hshtablein.Add("intDoctorId", ddlProvider.SelectedValue);

                        hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                        hshtablein.Add("nchvRecurrenceRule", "");
                        hshtablein.Add("RecurrenceParentId", "");
                        if (txtAccountNumber.Text != "")
                        {
                            hshtablein.Add("@intRegistrationId", common.myInt(txtAccountNumber.Text));
                        }

                        if (!OrigRecRule.Contains("EXDATE"))
                        {
                            string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        else
                        {
                            string UpdateRule = OrigRecRule + "," + exdate;
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        hshtableout.Add("@intAppointmentId", SqlDbType.Int);
                        hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                        hshtableout.Add("@bitReturn", SqlDbType.Bit);
                        if (txtAccountNumber.Text != "")
                        {
                            hshtablein.Add("@intRegistrationId", common.myInt(txtAccountNumber.Text));
                        }
                        if (common.myInt(hdnPPAppointmentId.Value) > 0)
                        {
                            hshtablein.Add("@intPPAppointmentId", common.myInt(hdnPPAppointmentId.Value));
                        }   
                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorAppointment", hshtablein, hshtableout);

                        lblMessage.Text = hshtableout["chvErrorStatus"].ToString();
                        if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                        {
                            //Alert.ShowAjaxMsg(hshtableout["chvErrorStatus"].ToString(), Page);
                            return;
                        }
                    }
                }

                else
                {
                    string strvalidation = "SELECT App.DoctorId,convert(varchar(10),App.AppointmentDate,103) AppointmentDate FROM DoctorAppointment app inner Join  Encounter Enc on App.AppointmentId=Enc.AppointmentId WHERE app.AppointmentID =" + Request.QueryString["appid"] + " And app.Active = 1";
                    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strvalidation);
                    if (dr.HasRows == true)
                    {
                        dr.Read();
                        if (ddlProvider.SelectedValue != dr["DoctorId"].ToString())
                        {
                            lblMessage.Text = "Change Doctor Not Allowed. Encounter Already Opened.";
                            return;
                        }

                        if (formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()) != dr["AppointmentDate"].ToString())
                        {
                            lblMessage.Text = "Change Date Not Allowed. Encounter Already Opened.";
                            return;
                        }
                    }
                    hshtablein.Add("intLastChangedBy", Session["UserID"].ToString());
                    hshtablein.Add("intAppointmentID", Request.QueryString["appid"]);
                    hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                    hshtableout.Add("@bitReturn", SqlDbType.Bit);
                    if (rule != null && rule != "")
                    {
                        hshtablein.Add("nchvRecurrenceRule", rule);
                    }
                    else
                    {
                        hshtablein.Add("nchvRecurrenceRule", "");
                    }

                    hshtablein.Add("RecurrenceParentId", "");
                    hshtablein.Add("intDoctorId", ddlProvider.SelectedValue);//Request.QueryString["doctorid"]);
                    hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspModifyDoctorAppointment", hshtablein, hshtableout);
                    lblMessage.Text = hshtableout["@chvErrorStatus"].ToString();
                    if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                    {
                        //Alert.ShowAjaxMsg(hshtableout["chvErrorStatus"].ToString(), Page);
                        return;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
        btnsave.BackColor = System.Drawing.Color.LightBlue;
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
    }

    protected void ddlAppointment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAppointment.SelectedValue == "0")
        {
            ViewState["IsDoctor"] = "True";
        }
        else if (ddlAppointment.SelectedValue == "1")
        {
            ViewState["IsDoctor"] = "False";
        }
        if (ddlAppointment.SelectedValue == "")
        {
            ddlProvider.Text = "";
            ddlProvider.Items.Clear();
        }
        if (ddlAppointment.SelectedValue != "")
        {
            populateProvider();
        }
    }

    protected void ddlAppointmentStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAppointmentStatus.SelectedValue != "0")
        {
            btnUpdateAll.Visible = false;
        }
        else
        {
            btnUpdateAll.Visible = true;
        }
    }

    protected void rblReferralDoctorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblReferralDoctorType.SelectedItem.Value == "I")
        {
            PopulateInternalDoctor();
            ScriptManager1.SetFocus("rblReferralDoctorType_0");
        }
        else
        {
            PopulateExternalDoctor();
            ScriptManager1.SetFocus("rblReferralDoctorType_1");
        }
    }

    private void PopulateInternalDoctor()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(2000);

            ddlReferralDoctor.Items.Clear();

            str.Append("select 0 as 'Id', ' [Select Internal Doctor]' as 'Name' union all select Id,FirstName + isnull(' ' + MiddleName,'') + isnull(' ' + LastName,'') as 'Name'");
            str.Append(" from employee where employeetype = 1 and hospitallocationid = " + Session["HospitalLocationID"] + " and active = 1 order by Name");

            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, str.ToString());

            ddlReferralDoctor.DataSource = dr;
            ddlReferralDoctor.DataTextField = "Name";
            ddlReferralDoctor.DataValueField = "Id";
            ddlReferralDoctor.DataBind();

            dr.Close();

            if (ddlReferralDoctor.Items.Count > 0)
            {
                ddlReferralDoctor.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateExternalDoctor()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(2000);

            ddlReferralDoctor.Items.Clear();

            str.Append("select 0 as 'Id', ' [Select External Doctor]' as 'Name' union all select t1.Id, Name from ReferDoctorMaster t1");
            str.Append(" inner join ReferTypeMaster t2 on t1.ReferralTypeId = t2.id and t2.ReferralType = 'D'");
            str.Append(" where t1.Active = 1");

            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, str.ToString());

            ddlReferralDoctor.DataSource = dr;
            ddlReferralDoctor.DataTextField = "Name";
            ddlReferralDoctor.DataValueField = "Id";
            ddlReferralDoctor.DataBind();

            dr.Close();

            if (ddlReferralDoctor.Items.Count > 0)
            {
                ddlReferralDoctor.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateIdentityTypes()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(1000);

            ddlIdentityType.Items.Clear();

            str.Append("select 0 as 'Id', ' [Select Type]' as 'Name' union all select Id,Description as 'Name'");
            str.Append(" from patientidentitytype where active = 1 order by Id");

            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, str.ToString());

            ddlIdentityType.DataSource = dr;
            ddlIdentityType.DataTextField = "Name";
            ddlIdentityType.DataValueField = "Id";
            ddlIdentityType.DataBind();

            dr.Close();

            if (ddlIdentityType.Items.Count > 0)
            {
                ddlIdentityType.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnGetOldRegInfo_Click(object sender, EventArgs e)
    {
        if (txtOldRegNo.Text != "")
        {
            populateControlsForOldReg();
        }
    }
    private void populateControlsForOldReg()
    {
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        DataSet ds = objPatient.GetPatientRecord(0, txtOldRegNo.Text.Trim());
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAccountNo.Text = "";
            hdnRegistrationId.Value = "";
            txtAccountNumber.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
            ViewState["NewRegNo"] = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
            ViewState["NewRegID"] = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
            hdnERegistrationId.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
            txtName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
            txtFName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
            txtMName.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            if (ds.Tables[0].Rows[0]["DateofBirth"].ToString() != "")
            {
                dtpDOB.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateofBirth"].ToString());
                if (common.myInt(txtAgeYears.Text) == 0 && common.myInt(txtAgeMonths.Text) == 0 && common.myInt(txtAgeDays.Text) == 0)
                    dtpDOB_Click(null, null);

            }
            else
            {
                dtpDOB.SelectedDate = null;
            }
            ddlGender.SelectedValue = ds.Tables[0].Rows[0]["gender"].ToString();
            txtMobile.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();
            dropReferredType.SelectedValue = ds.Tables[0].Rows[0]["ReferredTypeID"].ToString();

            hdnPatientCategoryId.Value = ds.Tables[0].Rows[0]["CompanyCode"].ToString();
            hdnPlanTypeId.Value = ds.Tables[0].Rows[0]["PlanTypeID"].ToString();
            if (hdnPatientCategoryId.Value == "FFS" && Convert.ToInt16(hdnPlanTypeId.Value) > 0)
            {
                populateVisitType(true, Convert.ToInt16(hdnPlanTypeId.Value));
            }
            else
            {
                populateVisitType(false, 0);
            }
            txtAgeYears.Text = ds.Tables[0].Rows[0]["AgeYear"].ToString();
            txtAgeMonths.Text = ds.Tables[0].Rows[0]["AgeMonth"].ToString();
            txtAgeDays.Text = ds.Tables[0].Rows[0]["AgeDays"].ToString();

            BindRefType();
            if (dropReferredType.SelectedValue != "0" && dropReferredType.SelectedValue != "")
            {
                BindProviderList();
                ddlreferringphysician.SelectedValue = ds.Tables[0].Rows[0]["ReferredByID"].ToString();
            }

            else
            {
                ddlreferringphysician.Items.Insert(0, new RadComboBoxItem("Select"));
                ddlreferringphysician.Items[0].Value = "0";
            }
            EnableControlFalse();

        }
        else
        {
            ClearAll();
            EnableControlTrue();
            dropReferredType.Items.Insert(0, new RadComboBoxItem("Select"));
            dropReferredType.Items[0].Value = "0";

            ddlreferringphysician.Items.Insert(0, new RadComboBoxItem("Select"));
            ddlreferringphysician.Items[0].Value = "0";
            lblmsg.Text = "No records found.";
            ViewState["NewRegNo"] = "0";
        }


        //txtAccountNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationNo"]);
        //txtRegNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);

        //AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(txtAccountNo.Text), 0, Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
        //txtFname.Text = objDs.Tables[0].Rows[0]["FirstName"].ToString().Trim();
        //txtMName.Text = objDs.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
        //txtLame.Text = objDs.Tables[0].Rows[0]["LastName"].ToString().Trim();
        //ViewState["PName"] = txt_hdn_PName.Text;
        //txtPreviousName.Text = objDs.Tables[0].Rows[0]["DisplayName"].ToString().Trim();
        //txtOldRegNo.Text = objDs.Tables[0].Rows[0]["RegistrationNoOld"].ToString().Trim();
        //string[] str = Convert.ToString(objDs.Tables[0].Rows[0]["PreviousName"].ToString()).Split('#');
        //if (str.Length == 2)
        //{
        //    ddlParentof.SelectedValue = str[0].ToString().Trim();
        //    txtParentof.Text = str[1].ToString().Trim();
        //}
        //if (chkNewBorn.Checked == false)
        //{
        //    if (common.myBool(objDs.Tables[0].Rows[0]["AgeIdentification"].ToString()) == false)
        //    {
        //        dtpDateOfBirth.DateInput.Text = "";
        //    }
        //    else
        //    {
        //        dtpDateOfBirth.SelectedDate = Convert.ToDateTime(objPatient.FormatDate(objDs.Tables[0].Rows[0]["DateofBirth"].ToString().Trim(), Application["OutputDateFormat"].ToString(), "dd/MM/yyyy"));
        //    }
        //    txtYear.Text = objDs.Tables[0].Rows[0]["AgeYear"].ToString().Trim();
        //    txtMonth.Text = objDs.Tables[0].Rows[0]["AgeMonth"].ToString().Trim();
        //    txtDays.Text = objDs.Tables[0].Rows[0]["AgeDays"].ToString().Trim();
        //    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindItemByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
        //    if (Convert.ToString(objDs.Tables[0].Rows[0]["Gender"]) != "")
        //    {
        //        dropSex.SelectedIndex = dropSex.Items.IndexOf(dropSex.Items.FindItemByValue(Convert.ToString(objDs.Tables[0].Rows[0]["Gender"])));
        //    }
        //    if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
        //    {
        //        btnCalAge_Click(null, null);
        //    }
        //    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindItemByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
        //}
        //else
        //{
        //    dropTitle.SelectedValue = common.myStr(ViewState["IsnewBornTitle"]);
        //}
        //txtMobile.Text = objDs.Tables[0].Rows[0]["MobileNo"].ToString();

        //txtEmail.Text = objDs.Tables[0].Rows[0]["Email"].ToString();

    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(txtAccountNo.Text) > 9223372036854775807)
            {
                Alert.ShowAjaxMsg("Value should not be more than 9223372036854775807.", this.Page);
                txtAccountNo.Text = txtAccountNo.Text.Substring(0, 12);
                //lblMessage.Text = "Invalid UHID No.";
                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }

            if (common.myStr(txtAccountNo.Text).Trim().Length > 0 || common.myInt(hdnRegistrationId.Value) > 0)
            {
                //raghuvir
                
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds1 = new DataSet();
                Hashtable ht1 = new Hashtable();
                ht1.Add("@registrationNo", common.myStr(txtAccountNo.Text));
                ds1 = dl.FillDataSet(CommandType.StoredProcedure, "uspPatientDeathRecord", ht1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    Alert.ShowAjaxMsg("You Cannot Create Visit For Death patient!!!!", Page.Page);
                    return;
                }

                dropReferredType.Items.Clear();
                ddlreferringphysician.Items.Clear();
                ViewState["regid"] = txtAccountNumber.Text.Trim();
                Hashtable hshIn = new Hashtable();
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                BaseC.ParseData parse = new BaseC.ParseData();
                hshIn.Add("@intRegistrationId", common.myInt(hdnRegistrationId.Value));
                hshIn.Add("@chvRegistrationNo", common.myStr(txtAccountNo.Text));
                
                hshIn.Add("@intFacilityId", common.myInt(ddlSelectionFacility.SelectedValue));

                hdnregno.Value = "";

                hshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));

                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "USPSearchRegistration", hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        txtAccountNo.Text = "";
                        hdnRegistrationId.Value = "";
                        txtAccountNumber.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        ViewState["NewRegNo"] = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        ViewState["NewRegID"] = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
                        hdnERegistrationId.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
                        txtName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                        txtFName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                        txtMName.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                        txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                        if (ds.Tables[0].Rows[0]["DateofBirth"].ToString() != "")
                        {
                            dtpDOB.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DateofBirth"].ToString());
                        }
                        else
                        {
                            dtpDOB.SelectedDate = null;
                        }
                        ddlGender.SelectedValue = ds.Tables[0].Rows[0]["gender"].ToString();
                        txtMobile.Text = ds.Tables[0].Rows[0]["mobile"].ToString();
                        dropReferredType.SelectedValue = ds.Tables[0].Rows[0]["ReferredTypeID"].ToString();

                        hdnCompanyId.Value=ds.Tables[0].Rows[0]["CompanyId"].ToString();

                        hdnPatientCategoryId.Value = ds.Tables[0].Rows[0]["CompanyCode"].ToString();
                        hdnPlanTypeId.Value = ds.Tables[0].Rows[0]["PlanTypeID"].ToString();
                        if (hdnPatientCategoryId.Value == "FFS" && Convert.ToInt16(hdnPlanTypeId.Value) > 0)
                        {
                            populateVisitType(true, Convert.ToInt16(hdnPlanTypeId.Value));
                        }
                        else
                        {
                            populateVisitType(false, 0);
                        }
                        txtAgeYears.Text = ds.Tables[0].Rows[0]["AgeYear"].ToString();
                        txtAgeMonths.Text = ds.Tables[0].Rows[0]["AgeMonth"].ToString();
                        txtAgeDays.Text = ds.Tables[0].Rows[0]["AgeDays"].ToString();

                        if (common.myInt(txtAgeYears.Text) == 0 && common.myInt(txtAgeMonths.Text) == 0 && common.myInt(txtAgeDays.Text) == 0 && ds.Tables[0].Rows[0]["DateofBirth"].ToString().Trim() != "")
                            dtpDOB_Click(null, null);

                        BindRefType();
                        if (dropReferredType.SelectedValue != "0" && dropReferredType.SelectedValue != "")
                        {
                            BindProviderList();
                            ddlreferringphysician.SelectedValue = ds.Tables[0].Rows[0]["ReferredByID"].ToString();
                        }

                        else
                        {
                            ddlreferringphysician.Items.Insert(0, new RadComboBoxItem("Select"));
                            ddlreferringphysician.Items[0].Value = "0";
                        }
                        if (ds.Tables[0].Rows[0]["ShowNoteInAppointment"] != null && ds.Tables[0].Rows[0]["ShowNoteInAppointment"].ToString() != "")
                        {
                            if (common.myBool(ds.Tables[0].Rows[0]["ShowNoteInAppointment"]))
                            {
                                hshIn = new Hashtable();
                                hshIn.Add("@RegistrationId", ViewState["NewRegID"]);
                                string strSQL = "Select Note from RegistrationOtherDetail where RegistrationId =@RegistrationId";
                                DataSet dsNote = dl.FillDataSet(CommandType.Text, strSQL, hshIn);
                                if (dsNote.Tables[0].Rows.Count > 0)
                                {
                                    txtNote.Text = dsNote.Tables[0].Rows[0]["Note"].ToString();
                                }
                            }

                        }
                       // //fillInsuranceData(common.myInt(ViewState["NewRegNo"]));
                       //// FillRegistrationDAtaofPatientsFromMobileNo(common.myInt(ViewState["NewRegNo"]), common.myStr(common.myInt(txtMobile.Text)));    
                        EnableControlFalse();
                    }
                    //lnkAppointmentlist.ForeColor = Color.Blue; ;
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (common.myInt(ds.Tables[1].Rows[0]["AppointmentCount"]) != 0)
                            {
                                lnkAppointmentlist.Text = "Appointment List" + "  (" + common.myInt(ds.Tables[1].Rows[0]["AppointmentCount"]) + ") ";
                                lnkAppointmentlist.Style.Add("text-decoration", "blink");
                                //lnkAppointmentlist.ForeColor = Color.Red;
                            }
                            else
                            {
                                lnkAppointmentlist.Text = "Appointment List";
                                lnkAppointmentlist.Style.Remove("text-decoration");
                            }
                        }
                    }
                    lblmsg.Text = "";
                    /*
                    AlertBlock1.lblmsg = "HELLO ";
                    if(AlertBlock1.lblmsg.Trim().Length>0)
                    {

                    }
                    
                   */
                    if (ds.Tables.Count > 2)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            dlPackageDetails.DataSource = ds.Tables[2];
                            dlPackageDetails.DataBind();
                            divPackageDetails.Visible = true;
                        }
                    }
                    BindMessage();
                }
                else
                {
                    ClearAll();
                    EnableControlTrue();
                    dropReferredType.Items.Insert(0, new RadComboBoxItem("Select"));
                    dropReferredType.Items[0].Value = "0";

                    ddlreferringphysician.Items.Insert(0, new RadComboBoxItem("Select"));
                    ddlreferringphysician.Items[0].Value = "0";
                    lblmsg.Text = "No records found.";
                    ViewState["NewRegNo"] = "0";
                }
                fillGridData();//new added (display appointment list on same page)---  prashant
            }
            else
            {
                lblmsg.Text = " ";
                ViewState["NewRegNo"] = "0";
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void EnableControlFalse()
    {
        txtFName.Enabled = true;
        txtMName.Enabled = true;
        txtName.Enabled = true;
        //txtMobile.Enabled = false;
        dtpDOB.Enabled = true;
        txtAgeYears.Enabled = false;
        txtAgeMonths.Enabled = false;
        txtAgeDays.Enabled = false;
        ddlGender.Enabled = true;
        //lnkAddName.Enabled = false;
        txtRegNo.ReadOnly = true;
        btnClear.Enabled = false;
    }
    private void EnableControlTrue()
    {
        txtFName.Enabled = true;
        txtMName.Enabled = true;
        txtName.Enabled = true;
        txtMobile.Enabled = true;
        dtpDOB.Enabled = true;
        txtAgeYears.Enabled = true;
        txtAgeMonths.Enabled = true;
        txtAgeDays.Enabled = true;
        ddlGender.Enabled = true;
    }
    protected void btnCalculate_OnClick(object sender, EventArgs e)
    {
    }
    protected void btn_click(object sender, EventArgs e)
    {

    }

    protected void ddlAppointmentType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            if (common.myInt(ddlAppointmentType.SelectedValue) > 0)
            {
                trService.Visible = false;
                trSubDept.Visible = false;
                if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P" || common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "H")
                {
                    BindSubDept();
                    ddlSubDept_OnSelectedIndexChanged(sender, e);
                }
                ViewState["Duration"] = null;
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                System.Text.StringBuilder str = new System.Text.StringBuilder(2000);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@inyHospitalLocatiOnID", Session["HospitalLocationID"]);
                hshIn.Add("@intDoctorID", ddlProvider.SelectedValue); //ViewState["DoctorID"].ToString());
                hshIn.Add("@intVisitTypeId", ddlAppointmentType.SelectedValue);

                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetDoctorVisitDuration", hshIn);
                string duration = ds.Tables[0].Rows[0]["Duration"].ToString();

                int intduration = common.myInt(duration);
                int inthour = RadTimeFrom.SelectedDate.Value.Hour;
                int intmnt = RadTimeFrom.SelectedDate.Value.Minute;
                //lblAptmntDuration.Text = " Duration: " + duration + " (Mins)";
                if (duration != "0")
                {
                    lblAptmntDuration.Text = " Duration: " + duration + " (Mins)";
                    intmnt += intduration;
                    while (intmnt >= 60)
                    {
                        inthour += 1;
                        intmnt -= 60;
                    }
                    DateTime dt = new DateTime(2010, 12, 12, inthour, intmnt, 00);
                    RadTimeTo.SelectedDate = dt;
                    //Added by vineet/////////////////////////////////
                    TimeSpan ts = RadTimeTo.SelectedDate.Value - RadTimeFrom.SelectedDate.Value;
                    ViewState["Duration"] = ts;
                    ///////////////////////////////////
                }
                else
                {
                    //lblAptmntDuration.Text = "Duration not defined.";
                    lblAptmntDuration.Text = "";
                }
            }
            else
            {
                //lblAptmntDuration.Text = "Duration not defined.";
                lblAptmntDuration.Text = "";
            }

            //ddlRoom.Focus();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlService_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        
    }

    protected void ddlProvider_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlAppointmentType.SelectedValue = "0";
    }

    protected void ddlFacility_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlFacility.SelectedValue != "0")
        {
            Session["FacilityId"] = ddlFacility.SelectedValue;
            PopulateRoom();
            populateProvider();
            populateReferringDoctor();
        }
        else
        {
            ddlProvider.Items.Clear();
            ddlProvider.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlRoom.Items.Clear();
            //ddlRoom.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (ddlService.SelectedValue != "" && ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"] == "H")
            {
                BaseC.Package package = new Package(sConString);
                ds = package.GetPackageServiceLimit(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlService.SelectedValue),
                    common.myInt(hdnCompanyId.Value), common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "The detail of this package does not define in package detail";
                    Alert.ShowAjaxMsg("The detail of this package does not define in package detail..", Page);
                    return;
                }
            }

            if ((ddlAppointmentStatus.SelectedValue == "3" || ddlAppointmentStatus.SelectedValue == "5")
                && common.myInt(FindFutureDate(Convert.ToString(dtpDate.SelectedDate))) > common.myInt(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
            {
                string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                Alert.ShowAjaxMsg(strMessage, Page);
                return;
            }

            if (dtpDate.SelectedDate == null || dtpDate.SelectedDate.ToString() == "")
            {
                Alert.ShowAjaxMsg("Please select appointment date..", Page.Page);
                return;
            }

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
            if (common.myInt(Request.QueryString["appid"]) != 0)
            {
                string strOrigTime = "select dbo.GetDateFormat(Fromtime,'T') AS FromTime from DoctorAppointment where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));

            }
            //string exdate = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";        
            string rule = "";
            string RecParentId = "";
            if (!string.IsNullOrEmpty(RadSchedulerRecurrenceEditor1.RecurrenceRuleText))
            {
                rule = RadSchedulerRecurrenceEditor1.RecurrenceRule.ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["recparentid"]))
            {
                RecParentId = Request.QueryString["recparentid"].ToString();
                if (RecParentId == "0")
                {
                    RecParentId = "";
                }
            }
            #endregion



            BaseC.ParseData pdDataCheck = new BaseC.ParseData();

            int intTo = 0, intFrom = 0;
            String FromTime = RadTimeFrom.SelectedDate.Value.ToString("H:mm");

            String FromTo = RadTimeTo.SelectedDate.Value.ToString("H:mm");
            if (FromTime.Contains(":") == true)
            {
                string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                intFrom = common.myInt(strFromTime);
            }

            if (FromTo.Contains(":") == true)
            {

                string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                intTo = common.myInt(strFromTo);
            }

            if (intTo <= intFrom)
            {
                Alert.ShowAjaxMsg("Please change the To Time", Page);
                return;
            }
            #region Appointment Rule

            ////string strType = "";
            //Hashtable hshcheck = new Hashtable();
            //hshcheck.Add("@intVisitTypeID", ddlAppointmentType.SelectedValue);
            //hshcheck.Add("@intDoctorID", ddlProvider.SelectedValue);
            //hshcheck.Add("@inyHositalLocationID", Session["HospitalLocationID"]);
            //hshcheck.Add("@intFacilityID", Convert.ToInt32(ddlFacility.SelectedValue));
            //DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspCheckAppointmentRule", hshcheck);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    string strFrom = ds.Tables[0].Rows[0]["VisitBeginTime"].ToString();
            //    string strTo = ds.Tables[0].Rows[0]["VisitEndTime"].ToString();
            //    string strDoctorID = ds.Tables[0].Rows[0]["DoctorID"].ToString();
            //    string strFacilityID = ds.Tables[0].Rows[0]["FacilityID"].ToString();

            //    int FromT = Convert.ToInt32(strFrom.Remove(strFrom.IndexOf(":"), 1));
            //    int ToT = Convert.ToInt32(strTo.Remove(strTo.IndexOf(":"), 1));
            //    bool bitTrue = false;


            //    if (intFrom >= FromT && intTo <= ToT 
            //        && strDoctorID.Trim()==ddlProvider.SelectedValue.Trim() 
            //        && strFacilityID.Trim()==ddlFacility.SelectedValue.Trim())
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            if (ds.Tables[0].Rows[i]["Day"].ToString() ==
            //                dtpDate.SelectedDate.Value.DayOfWeek.ToString().ToUpper().Substring(0, 3))
            //            {
            //                bitTrue = true;
            //                if (ds.Tables[1].Rows.Count > 0)
            //                {
            //                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
            //                    {
            //                        if (ds.Tables[0].Rows[i]["Day"].ToString().Trim() == Convert.ToDateTime(ds.Tables[1].Rows[j]["AppointmentDate"]).DayOfWeek.ToString().ToUpper().Substring(0, 3).Trim())
            //                        {
            //                            if (ds.Tables[1].Rows[j]["AppointmentDate"].ToString().Trim() == Convert.ToDateTime(dtpDate.SelectedDate.Value.ToShortDateString()).ToString("yyyy/MM/dd").Trim())
            //                            {
            //                                //if (Convert.ToInt32(ds.Tables[1].Rows.Count) >= Convert.ToInt32(ds.Tables[0].Rows[0]["MaxAppointmentNo"]))
            //                                //{
            //                                //    lblMessage.Text = "No additional appointments can be set at this time.";
            //                                //    return;
            //                                //}
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //        }
            //        if (bitTrue == false)
            //        {
            //            int Count = 0;
            //            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            //            {
            //                if (Count == 0)
            //                {
            //                    lblMessage.Text = "Appointment days are available in ";
            //                    lblMessage.Text += ds.Tables[0].Rows[k]["Day"].ToString();
            //                    Count++;
            //                }
            //                else
            //                {
            //                    lblMessage.Text += ", " + ds.Tables[0].Rows[k]["Day"].ToString();
            //                }
            //            }
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        lblMessage.Text = "Appointment slots are available from "
            //            + ds.Tables[0].Rows[0]["BeginTime"].ToString() + " to "
            //            + ds.Tables[0].Rows[0]["EndTime"].ToString() + ".";
            //        return;
            //    }
            //}

            #endregion
            if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P" || common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "H")
            {
                if (common.myInt(ddlService.SelectedValue) == 0 || common.myStr(ddlService.SelectedValue) == "")
                {
                    Alert.ShowAjaxMsg("Please Select Package Service...", Page);
                    return;
                }
                else
                {
                    hshtablein.Add("@iPackageId", common.myInt(ddlService.SelectedValue));
                }
            }
            hshtablein.Add("chrAppFromTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"));
            hshtablein.Add("chrAppToTime", RadTimeTo.SelectedDate.Value.ToString("hh:mmtt")); //RadTimeTo.DbSelectedDate.ToString("hh:mm tt"));
            hshtablein.Add("intVisitTypeId", ddlAppointmentType.SelectedValue.ToString());
            hshtablein.Add("@chvLastName", pdDataCheck.ParseQ(txtName.Text));
            hshtablein.Add("@chvFirstName", pdDataCheck.ParseQ(txtFName.Text));
            hshtablein.Add("@chvMiddleName", pdDataCheck.ParseQ(txtMName.Text));
            hshtablein.Add("inyGender", ddlGender.SelectedValue); //ddlGender.Text.Substring(0, 1));
            //if (ddlcasename.SelectedValue != "")
            //{
            hshtablein.Add("@intPaymentCaseId", null);
            //}
            hshtablein.Add("@chvAuthorizationNo", "");
           // hshtablein.Add("@chvAuthorizationNo", txtauthorization.Text.Trim());
            //if (ddlRoom.SelectedValue != "0")
            //{
            //    hshtablein.Add("intRoomId", ddlRoom.SelectedValue);
            //}

            if (dtpDOB.SelectedDate == null && (txtAgeYears.Text == "0" || txtAgeYears.Text == ""))
            {
                Alert.ShowAjaxMsg("Please enter Date Of Birth or Age", Page);
                return;
            }
            else if (dtpDOB.SelectedDate == null)
            {
                if (txtAgeYears.Text != "")
                {
                    hshtablein.Add("chrDOB", hdnCalculateDOB.Value);
                    hshtablein.Add("intAgeYear", txtAgeYears.Text);
                    hshtablein.Add("@bitRealDateOfBirth", 0);
                }
            }
            else if (dtpDOB.SelectedDate != null)
            {
                hshtablein.Add("chrDOB", formatdate.FormatDateDateMonthYear(dtpDOB.SelectedDate.Value.ToShortDateString()));
                hshtablein.Add("intAgeYear", txtAgeYears.Text);
                hshtablein.Add("@bitRealDateOfBirth", 1);
            }


            hshtablein.Add("intAgeMonth", txtAgeMonths.Text);
            hshtablein.Add("intAgeDay", txtAgeDays.Text);

            if (txtPhoneHome.Text.Trim() != "___-___-____")
            {
                hshtablein.Add("chvResidencePhone", txtPhoneHome.Text);
            }
            else
            {
                hshtablein.Add("chvResidencePhone", DBNull.Value);
            }

            if (txtreason.Text.Trim().Length > 0)
            {
                hshtablein.Add("chvRemarks", pdDataCheck.ParseQ(txtreason.Text.Trim()));

            }
            else
            {
                hshtablein.Add("chvRemarks", DBNull.Value);
            }

            if (txtMobile.Text.Trim() != "___-___-____")
            {
                hshtablein.Add("chvMobile", txtMobile.Text);
            }
            else
            {
                hshtablein.Add("chvMobile", DBNull.Value);
            }

            if (txtEmail.Text.Trim().Length > 0)
            {
                hshtablein.Add("chvEmail", pdDataCheck.ParseQ(txtEmail.Text));
            }
            else
            {
                hshtablein.Add("chvEmail", DBNull.Value);
            }

            hshtablein.Add("bitWalkInPatient", chkWalkIn.Checked);
            hshtablein.Add("intFacilityId", Convert.ToInt16(ddlFacility.SelectedValue));
            hshtablein.Add("intStatusId", ddlAppointmentStatus.SelectedValue);

            if (dropReferredType.SelectedValue == "0" || dropReferredType.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please Select Referred Type", Page);
                return;
            }
            else
            {
                hshtablein.Add("intReferredType ", dropReferredType.SelectedItem.Attributes["Id"]);
            }
            if ((ddlreferringphysician.SelectedValue == "0" || ddlreferringphysician.SelectedValue == "") && dropReferredType.SelectedValue != "S")
            {
                Alert.ShowAjaxMsg("Please Select Referring Physician", Page);
                return;
            }
            else
            {
                hshtablein.Add("intreferringphysician ", ddlreferringphysician.SelectedValue);
            }


            hshtablein.Add("@intLoginFacilityId", Session["FacilityId"]);
            hshtablein.Add("@intPageId", 3);  // Request.QueryString["PageId"].ToString().Substring(1)
            hshtablein.Add("@chvNote", common.myStr(txtNote.Text));
            hshtablein.Add("@IsDoctor", common.myBool(ViewState["IsDoctor"]));
            string exdate = "";
            hshtablein.Add("@intReferrerDoctor", null);
            if (Request.QueryString["appDate"] != dtpDate.SelectedDate.Value.ToShortDateString())
            {
                exdate = Convert.ToDateTime(Request.QueryString["appDate"]).ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
            }
            else
            {
                exdate = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
            }
            hshtablein.Add("chrAppointmentDate", formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()));  //Request.QueryString["appDate"]);

            string origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
            string OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();

            //if (OrigRecRule.Trim() != "")
            if (rule != "" || OrigRecRule.Trim() != "") //if Appointment is recurring and editing master appointment
            {
                DateTime dtStart = dtpDate.SelectedDate.Value;
                Hashtable hshInput = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                //
                hshInput.Add("@intParentAppointmentID", Convert.ToInt64(Request.QueryString["appid"]));
                hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                hshInput.Add("@intFacilityID", ddlFacility.SelectedValue);
                hshOutput.Add("@intStatusID", SqlDbType.Int);
                hshInput.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));
                hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", hshInput, hshOutput);
                if (hshOutput["@intStatusID"].ToString() == "2")
                {
                    //btnRefresh_OnClick(sender, e);
                }
                else if (hshOutput["@intStatusID"].ToString() == "3")
                {
                    //btnRefresh_OnClick(sender, e);
                }
                else if (hshOutput["@intStatusID"].ToString() == "5")
                {
                    //btnRefresh_OnClick(sender, e);
                }
                else if (hshOutput["@intStatusID"].ToString() == "6")
                {
                    //btnRefresh_OnClick(sender, e);
                }
                else if (hshOutput["@intStatusID"].ToString() == "34")
                {
                    //btnRefresh_OnClick(sender, e);
                }
                else
                {
                    if (Session["RecApptID"] == null)
                    {
                        if (common.myInt(Request.QueryString["appid"]) != 0)
                        {
                            hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                            hshtablein.Add("intRegistrationNo", txtAccountNumber.Text);
                            hshtablein.Add("intDoctorId", ddlProvider.SelectedValue);
                            //Request.QueryString["doctorid"]);
                            hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                            hshtablein.Add("nchvRecurrenceRule", "");
                            hshtablein.Add("RecurrenceParentId", "");
                            //string exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";
                            if (!OrigRecRule.Contains("EXDATE"))
                            {
                                string UpdateRule = OrigRecRule + "EXDATE:" + exdate;
                                string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "'  where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                                dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            }
                            else
                            {
                                string UpdateRule = OrigRecRule + "," + exdate;
                                string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + UpdateRule + "' where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                                dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                            }
                            hshtableout.Add("@intAppointmentId", SqlDbType.Int);
                            hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                            hshtableout.Add("@bitReturn", SqlDbType.Bit);
                            if (txtAccountNumber.Text != "")
                            {
                                hshtablein.Add("@intRegistrationId", common.myInt(txtAccountNumber.Text));
                            }
                            if (common.myInt(hdnPPAppointmentId.Value) > 0)
                            {
                                hshtablein.Add("@intPPAppointmentId", common.myInt(hdnPPAppointmentId.Value));
                            }   
                            hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorAppointment", hshtablein, hshtableout);
                            string strUpdateRecRule1 = "Update doctorappointment set RecurrenceParentId='" + common.myInt(Request.QueryString["appid"]) + "' where appointmentid='" + hshtableout["@intAppointmentId"] + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule1);

                            Session["RecApptID"] = hshtableout["@intAppointmentId"].ToString();
                            lblMessage.Text = hshtableout["chvErrorStatus"].ToString();
                            if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                            {
                                //Alert.ShowAjaxMsg(hshtableout["@chvErrorStatus"].ToString(), Page);
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                string strvalidation = "SELECT App.DoctorId,convert(varchar(10),App.AppointmentDate,103) AppointmentDate FROM DoctorAppointment app inner Join  Encounter Enc on App.AppointmentId=Enc.AppointmentId WHERE app.AppointmentID =" + Request.QueryString["appid"] + " And app.Active = 1";
                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strvalidation);
                if (dr.HasRows == true)
                {
                    dr.Read();
                    if (ddlProvider.SelectedValue != dr["DoctorId"].ToString())
                    {
                        lblMessage.Text = "Change Doctor Not Allowed. Encounter Already Opened.";
                        return;
                    }

                    if (formatdate.FormatDateDateMonthYear(dtpDate.SelectedDate.Value.ToShortDateString()) != dr["AppointmentDate"].ToString())
                    {
                        lblMessage.Text = "Change Date Not Allowed. Encounter Already Opened.";
                        return;
                    }
                }
                hshtablein.Add("intLastChangedBy", Session["UserID"].ToString());


                hshtablein.Add("intAppointmentID", Request.QueryString["appid"]);
                hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                if (rule != null && rule != "")
                {
                    hshtablein.Add("nchvRecurrenceRule", rule);
                }
                else
                {
                    hshtablein.Add("nchvRecurrenceRule", "");
                }

                hshtablein.Add("RecurrenceParentId", "");
                hshtablein.Add("intDoctorId", ddlProvider.SelectedValue);//Request.QueryString["doctorid"]);
                hshtableout.Add("@bitReturn", SqlDbType.Bit);
                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspModifyDoctorAppointment", hshtablein, hshtableout);
                lblMessage.Text = hshtableout["chvErrorStatus"].ToString();
                if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                {
                    //Alert.ShowAjaxMsg(hshtableout["@chvErrorStatus"].ToString(), Page);
                    return;
                }

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
        btnUpdate.BackColor = System.Drawing.Color.LightBlue;
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
    }
    protected void btnUpdateAll_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (ddlService.SelectedValue != "" && ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"] == "H")
            {
                BaseC.Package package = new Package(sConString);
                ds = package.GetPackageServiceLimit(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlService.SelectedValue),
                    common.myInt(hdnCompanyId.Value), common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMessage.Text = "The detail of this package does not define in package detail";
                    Alert.ShowAjaxMsg("The detail of this package does not define in package detail..", Page);
                    return;
                }
            }
            if (RadSchedulerRecurrenceEditor1.RecurrenceRuleText != null)
            {
                BaseC.Patient formatdate = new BaseC.Patient(sConString);
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                string[] strAppId = common.myStr(Request.QueryString["AppId1"]).Split('_');
                DateTime origAppTime = DateTime.Now;
                DateTime origAppDate = DateTime.Today;
                string OrigRecRule = "", origrule = "";

                int intTo = 0, intFrom = 0;
                String FromTime = RadTimeFrom.SelectedDate.Value.ToString("H:mm");

                String FromTo = RadTimeTo.SelectedDate.Value.ToString("H:mm");
                if (FromTime.Contains(":") == true)
                {
                    string strFromTime = FromTime.Remove(FromTime.IndexOf(":"), 1);
                    intFrom = common.myInt(strFromTime);
                }

                if (FromTo.Contains(":") == true)
                {
                    string strFromTo = FromTo.Remove(FromTo.IndexOf(":"), 1);
                    intTo = common.myInt(strFromTo);
                }

                if (intTo <= intFrom)
                {
                    Alert.ShowAjaxMsg("Please change the To Time", Page);
                    return;
                }



                //#endregion
                if (Request.QueryString["AppId1"].ToString().Contains('_'))
                {
                    DateTime dtStart = dtpDate.SelectedDate.Value.Date;
                    //string recrule1 = e.Appointment.Attributes["RecurrenceRule"];                
                    string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                    origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                    string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                    origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                    TimeSpan daysCount = dtStart.Subtract(origAppDate);
                    string exdate = "";
                    if (Request.QueryString["appDate"] != dtpDate.SelectedDate.Value.ToShortDateString())
                    {
                        exdate = "UNTIL=" + Convert.ToDateTime(Request.QueryString["appDate"]).ToString("yyyyMMdd") + "T000000Z;";
                    }
                    else
                    {
                        exdate = "UNTIL=" + dtStart.ToString("yyyyMMdd") + "T000000Z;";
                    }



                    origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                    OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();
                    if (!OrigRecRule.Contains("COUNT"))
                    {
                        if (!OrigRecRule.Contains("UNTIL"))
                        {
                            //int newrule = OrigRecRule.IndexOf("INTERVAL");
                            string newRecRule = OrigRecRule.Insert(OrigRecRule.IndexOf("INTERVAL"), exdate);
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newRecRule + "' where appointmentid='" + strAppId[0] + "'";
                            dl.ExecuteNonQuery(CommandType.Text, strUpdateRecRule);
                        }
                        else
                        {
                            //int newrule = OrigRecRule.IndexOf("UNTIL");
                            string newrule = OrigRecRule.Insert(OrigRecRule.IndexOf("UNTIL"), exdate);
                            string strUpdateRecRule = "Update doctorappointment set RecurrenceRule='" + newrule + "' where appointmentid='" + strAppId[0] + "'";
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
                    hshIn.Add("@intAppointmentId", strAppId[0]);
                    hshIn.Add("@intLastChangedBy", Session["UserId"]);
                    hshIn.Add("@chvLastChangedDate", DateTime.Now);
                    dl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update DoctorAppointment Set Active=0,LastChangedBy=@intLastChangedBy, LastChangedDate=@chvLastChangedDate where AppointmentId = @intAppointmentId ", hshIn);//or RecurrenceParentId=@intAppointmentId
                }

                /////////////////////
                if ((ddlAppointmentStatus.SelectedValue == "3" || ddlAppointmentStatus.SelectedValue == "5")
                    && common.myInt(FindFutureDate(Convert.ToString(dtpDate.SelectedDate))) > common.myInt(FindCurrentDate(Convert.ToString(System.DateTime.Today))))
                {
                    string strMessage = "Warning. You are checking a patient in for a future appointment. You are not allowed to check the patient in or out.";
                    Alert.ShowAjaxMsg(strMessage, Page);
                    return;
                }

                Hashtable hshtableout = new Hashtable();
                Hashtable hshtablein = new Hashtable();
                hshtablein.Clear();
                #region for recurrence
                //DateTime start = RecStart();

                //string strOrigTime = "select FromTime from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                // origAppTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                //  string strOrigDate = "select AppointmentDate from DoctorAppointment where appointmentid='" + strAppId[0] + "'";
                //  origAppDate = RecStart();
                //origAppDate = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigDate));
                //TimeSpan daysCount = dtStart.Subtract(origAppDate);

                DateTime result = Convert.ToDateTime(Request.QueryString["appDate"].ToString());
                TimeSpan time = RadTimeFrom.SelectedDate.Value.TimeOfDay;
                result = result.Add(time);

                RadSchedulerRecurrenceEditor1.StartDate = result;//result;//RecStart();//.ToString("yyyyMMdd");

                result = Convert.ToDateTime(Request.QueryString["appDate"].ToString());
                time = RadTimeTo.SelectedDate.Value.TimeOfDay;
                result = result.Add(time);

                RadSchedulerRecurrenceEditor1.EndDate = result;//RecEnd();
                DateTime OrigTime = DateTime.Now;
                if (common.myInt(Request.QueryString["appid"]) != 0)
                {
                    string strOrigTime = "select dbo.GetDateFormat(Fromtime,'T') AS FromTime from DoctorAppointment where appointmentid='" + common.myInt(Request.QueryString["appid"]) + "'";
                    OrigTime = Convert.ToDateTime(dl.ExecuteScalar(CommandType.Text, strOrigTime));
                }
                string exdate1 = RecStart().ToString("yyyyMMdd") + "T" + OrigTime.ToString("HHmmss") + "Z";
                string rule = "";
                string RecParentId = "";

                origrule = "Select RecurrenceRule from doctorappointment where appointmentid='" + strAppId[0] + "'";
                OrigRecRule = dl.ExecuteScalar(CommandType.Text, origrule).ToString();

                if (!string.IsNullOrEmpty(RadSchedulerRecurrenceEditor1.RecurrenceRuleText))
                {
                    rule = Convert.ToString(RadSchedulerRecurrenceEditor1.RecurrenceRule);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["recparentid"]))
                {
                    RecParentId = Request.QueryString["recparentid"].ToString();
                    if (RecParentId == "0")
                    {
                        RecParentId = "";
                    }
                }

                #endregion



                BaseC.ParseData pdDataCheck = new BaseC.ParseData();

                hshtablein.Add("chrAppFromTime", RadTimeFrom.SelectedDate.Value.ToString("hh:mmtt"));
                hshtablein.Add("chrAppToTime", RadTimeTo.SelectedDate.Value.ToString("hh:mmtt")); //RadTimeTo.DbSelectedDate.ToString("hh:mm tt"));
                hshtablein.Add("intVisitTypeId", ddlAppointmentType.SelectedValue.ToString());
                hshtablein.Add("@chvLastName", pdDataCheck.ParseQ(txtName.Text));
                hshtablein.Add("@chvFirstName", pdDataCheck.ParseQ(txtFName.Text));
                hshtablein.Add("@chvMiddleName", pdDataCheck.ParseQ(txtMName.Text));
                hshtablein.Add("inyGender", ddlGender.SelectedValue); //ddlGender.Text.Substring(0, 1));
                //if (ddlcasename.SelectedValue != "")
                //{
                hshtablein.Add("@intPaymentCaseId", null);
                //}
                hshtablein.Add("@chvAuthorizationNo", "");
               // hshtablein.Add("@chvAuthorizationNo", txtauthorization.Text.Trim());
                ////if (ddlRoom.SelectedValue != "0")
                ////{
                ////    hshtablein.Add("intRoomId", ddlRoom.SelectedValue);
                ////}

                if (dtpDOB.SelectedDate == null && (txtAgeYears.Text == "0" || txtAgeYears.Text == ""))
                {
                    Alert.ShowAjaxMsg("Please enter Date Of Birth or Age", Page);
                    return;
                }
                else if (dtpDOB.SelectedDate == null)
                {
                    if (txtAgeYears.Text != "")
                    {
                        hshtablein.Add("chrDOB", hdnCalculateDOB.Value);
                        hshtablein.Add("intAgeYear", txtAgeYears.Text);
                        hshtablein.Add("@bitRealDateOfBirth", 0);
                    }
                }
                else if (dtpDOB.SelectedDate != null)
                {
                    hshtablein.Add("chrDOB", formatdate.FormatDateDateMonthYear(dtpDOB.SelectedDate.Value.ToShortDateString()));
                    hshtablein.Add("intAgeYear", txtAgeYears.Text);
                    hshtablein.Add("@bitRealDateOfBirth", 1);
                }

                hshtablein.Add("intAgeMonth", txtAgeMonths.Text);
                hshtablein.Add("intAgeDay", txtAgeDays.Text);

                if (txtPhoneHome.Text.Trim() != "___-___-____")
                {
                    hshtablein.Add("chvResidencePhone", txtPhoneHome.Text);
                }
                else
                {
                    hshtablein.Add("chvResidencePhone", DBNull.Value);
                }

                if (txtreason.Text.Trim().Length > 0)
                {
                    hshtablein.Add("chvRemarks", pdDataCheck.ParseQ(txtreason.Text.Trim()));

                }
                else
                {
                    hshtablein.Add("chvRemarks", DBNull.Value);
                }

                if (txtMobile.Text.Trim() != "___-___-____")
                {
                    hshtablein.Add("chvMobile", txtMobile.Text);
                }
                else
                {
                    hshtablein.Add("chvMobile", DBNull.Value);
                }

                if (txtEmail.Text.Trim().Length > 0)
                {
                    hshtablein.Add("chvEmail", pdDataCheck.ParseQ(txtEmail.Text));
                }
                else
                {
                    hshtablein.Add("chvEmail", DBNull.Value);
                }

                hshtablein.Add("bitWalkInPatient", chkWalkIn.Checked);
                hshtablein.Add("intFacilityId", Convert.ToInt16(ddlFacility.SelectedValue));
                hshtablein.Add("intStatusId", ddlAppointmentStatus.SelectedValue);

                if (dropReferredType.SelectedValue == "0" || dropReferredType.SelectedValue == "")
                {
                    Alert.ShowAjaxMsg("Please Select Referred Type", Page);
                    return;
                }
                else
                {
                    hshtablein.Add("intReferredType ", dropReferredType.SelectedItem.Attributes["Id"]);
                }
                if ((ddlreferringphysician.SelectedValue == "0" || ddlreferringphysician.SelectedValue == "") && dropReferredType.SelectedValue != "S")
                {
                    Alert.ShowAjaxMsg("Please Select Referring Physician", Page);
                    return;
                }
                else
                {
                    hshtablein.Add("intreferringphysician ", ddlreferringphysician.SelectedValue);
                }

                hshtablein.Add("@intLoginFacilityId", Session["FacilityId"]);
                hshtablein.Add("@intPageId", 3);  // Request.QueryString["PageId"].ToString().Substring(1)
                hshtablein.Add("@chvNote", common.myStr(txtNote.Text));
                hshtablein.Add("@intReferrerDoctor", null);

                hshtablein.Add("chrAppointmentDate", formatdate.FormatDateDateMonthYear(origAppDate.ToShortDateString()));  //Request.QueryString["appDate"]);
                ////////////////////////

                hshtablein.Add("intDoctorId", ddlProvider.SelectedValue);
                //Request.QueryString["doctorid"]);
                //hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                if (rule != "")//Editing Whole Recurring Appointment 
                {
                    if (Session["RecApptID"] == null)
                    {
                        hshtablein.Add("nchvRecurrenceRule", rule);
                        hshtablein.Add("RecurrenceParentId", DBNull.Value);

                        hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                        hshtablein.Add("intRegistrationNo", txtAccountNumber.Text);
                        hshtablein.Add("intEncodedBy", Session["UserID"].ToString());
                        hshtablein.Add("@IsDoctor", ViewState["IsDoctor"]);
                        hshtableout.Add("@intAppointmentId", SqlDbType.Int);
                        hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
                        hshtableout.Add("@bitReturn", SqlDbType.Bit);

                        if (txtAccountNumber.Text != "")
                        {
                            hshtablein.Add("@intRegistrationId", common.myInt(txtAccountNumber.Text));
                        }
                        if (common.myInt(hdnPPAppointmentId.Value) > 0)
                        {
                            hshtablein.Add("@intPPAppointmentId", common.myInt(hdnPPAppointmentId.Value));
                        }   
                        hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorAppointment", hshtablein, hshtableout);

                        lblMessage.Text = "Appointment Updated.";
                        Session["RecApptID"] = hshtableout["@intAppointmentId"].ToString();
                        if (Convert.ToBoolean(hshtableout["@bitReturn"]) == true)
                        {
                            //Alert.ShowAjaxMsg(hshtableout["@chvErrorStatus"].ToString(), Page);
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
        btnUpdateAll.BackColor = System.Drawing.Color.LightBlue;
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
    }

    protected void RadTimeTo_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        if (common.myInt(Request.QueryString["appid"]) != 0)
        {
            if (ViewState["Duration"] != null)
            {
                TimeSpan ts = (TimeSpan)ViewState["Duration"];
                //RadTimeFrom.DbSelectedDate = RadTimeTo.SelectedDate.Value.Add(-ts);
            }
        }
    }

    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0";
      //RadWindowForNew.NavigateUrl = "/EMR/PatientDetailsNew.aspx?OPIP=O&RegEnc=0";
        RadWindowForNew.Height = 580;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ClearAll()
    {
        txtAccountNo.Text = "";

        hdnRegistrationId.Value = "";
        txtAccountNumber.Text = "";
        ViewState["NewRegNo"] = "";
        ViewState["NewRegID"] = "";

        txtName.Text = "";
        txtFName.Text = "";
        txtMName.Text = "";
        dtpDOB.SelectedDate = null;

        ddlGender.SelectedIndex = 0;
        txtMobile.Text = "";
        dropReferredType.SelectedIndex = 0;
        ddlreferringphysician.Items.Insert(0, new RadComboBoxItem("Select"));
        ddlreferringphysician.Items[0].Value = "0";

    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        ClearAll();
    }

    protected void BindSubDept()
    {
        trService.Visible = false;
        trSubDept.Visible = false;
        if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P" || common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "H")
        {
            string VisitType = "'OPP'";
            lblPackageService.Text = "Package";
            trService.Visible = true;
            trSubDept.Visible = true;
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ddlSubDept.Items.Clear();
            BaseC.EMRMasters objE = new BaseC.EMRMasters(sConString);
            if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P")
            {
                VisitType = "'P'";
                lblPackageService.Text = "Service";
            }

            DataSet ds = objE.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), 0, VisitType, 0);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSubDept.DataSource = ds;
                    ddlSubDept.DataTextField = "SubName";
                    ddlSubDept.DataValueField = "SubDeptId";
                    ddlSubDept.DataBind();
                }

            }
            RadComboBoxItem lst = new RadComboBoxItem();
            lst.Selected = true;
            lst.Text = "All";
            lst.Value = "0";
            ddlSubDept.Items.Insert(0, lst);

        }

    }

    protected void ddlSubDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //if (common.myInt(ddlSubDept.SelectedValue) > 0)
        //{
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string VisitType = "'OPP'";
        if (common.myStr(ddlAppointmentType.SelectedItem.Attributes["IsPackageVisit"]) == "P")
        {
            VisitType = "'P'";
        }
        BaseC.EMRMasters objE = new BaseC.EMRMasters(sConString);
        DataSet ds = objE.GetService(common.myInt(Session["HospitalLocationId"]), common.myStr(ddlSubDept.SelectedValue), VisitType, common.myInt(Session["FacilityId"]));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlService.Items.Clear();
                ddlService.DataSource = ds;
                ddlService.DataTextField = "ServiceName";
                ddlService.DataValueField = "ServiceId";
                ddlService.DataBind();
            }

        }
        RadComboBoxItem lst = new RadComboBoxItem();
        lst.Selected = true;
        lst.Text = "Select";
        lst.Value = "0";
        ddlService.Items.Insert(0, lst);
        //}
    }

    void BindMessage()
    {
        BaseC.Patient objbc = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        ds = objbc.GetAlertBlockMessage(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            common.myInt(ViewState["NewRegID"]));

        if (ds.Tables[0].Rows.Count > 0)
        {
            //hdnNoteType.Value = ds.Tables[0].Rows[0]["NoteType"].ToString().Trim();

            dlMissingDocument.DataSource = ds.Tables[0];
            dlMissingDocument.DataBind();
            dvMessage.Visible = true;
        }
        else
        {
            dvMessage.Visible = false;
        }
    }

    protected void btnOk_OnClick(object sender, EventArgs e)
    {
        dvMessage.Visible = false;
    }

    protected void lnkAppointmentlist_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnERegistrationId.Value) != 0)
        {
            RadWindowForNew.NavigateUrl = "/Appointment/AppointmentList.aspx?regForDetails=" + hdnERegistrationId.Value;

            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Please Select Patient! ";
        }
    }
    protected void btnPackageRendering_OnClick(object sender, EventArgs e)
    {
        divPackageDetails.Visible = false;
    }
    //protected void lnkOnlineAppointment_OnClick(object sender, EventArgs e)
    //{
    //    RadWindowForNew.NavigateUrl = "/Appointment/OnlineAppointmentRequest.aspx?doctorId="+ common.myStr(Request.QueryString["doctorId"]);
    //    RadWindowForNew.Top = 40;
    //    RadWindowForNew.Left = 100;
    //    RadWindowForNew.OnClientClose = "SearchPatientOnClientCloseOnlineRequest";// "OnClientClose";
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.InitialBehavior = WindowBehaviors.Default;
    //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //    RadWindowForNew.VisibleStatusbar = false;

    //}

    protected void lnkUHID_Click(object sender, EventArgs e)
    {
        try
        {

            if (common.myStr(txtAccountNo.Text) != string.Empty)
            {
                populateControlsForEditing(common.myInt(txtAccountNo.Text));
            }
            else
            {
                Alert.ShowAjaxMsg("Please! Enter UHID.", Page);
                return;

            }



        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private void populateControlsForEditing(int PID)
    {
        try
        {

            btnGetInfo_Click(null, null);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSearchMob_Click(object sender, EventArgs e)
    {
        txtAccountNo.Text = string.Empty;
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        if (txtSearchMobile.Text != "")
        {
            int mob = common.myInt(txtSearchMobile.Text);
            int hos = common.myInt(Session["HospitalLocationId"]);


            DataSet objDs = objPatient.getPatientDetailsByMobileNo(common.myStr(txtSearchMobile.Text), common.myInt(Session["HospitalLocationId"]));


            if (objDs.Tables[0].Rows.Count > 0)
            {
                if (objDs.Tables[0].Rows.Count >= 2)
                {
                    //  ClearFields();

                    //RadWindowForNew.NavigateUrl = "/Pharmacy//Saleissue/PatientDetails.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();
                 //   RadWindowForNew.NavigateUrl = "/EMR/PatientDetailsNew.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();
                    RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();

                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 970;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;



                    //////RadWindowForNew.NavigateUrl = "~/Pharmacy/Saleissue/PatientDetailsNewMob.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();
                    //////RadWindowForNew.Width = 1200;
                    //////RadWindowForNew.Height = 630;
                    //////RadWindowForNew.Top = 10;
                    //////RadWindowForNew.Left = 10;
                    //////RadWindowForNew.OnClientClose = "BindPatientOnClientClose";
                    //////RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    //////RadWindowForNew.Modal = true;
                    //////RadWindowForNew.VisibleStatusbar = false;
                    //////RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

                }
                else
                {
                    txtAccountNo.Text = common.myStr(objDs.Tables[0].Rows[0][1]);
                    populateControlsForEditing(Convert.ToInt32(objDs.Tables[0].Rows[0][1]));
                }
            }
        }
        else
        {

            Alert.ShowAjaxMsg("Please! Enter Mobile No..", Page);
            return;


        }
    }
 
}
