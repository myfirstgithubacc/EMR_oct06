using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;

public partial class MPages_Employee : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    BaseC.ParseData bc = new BaseC.ParseData();
    public static string sFileName = "";
    public static string sFileExt = "";
    private const string DefaultCountry = "DefaultCountry";
    private const string DefaultState = "DefaultState";
    private const string DefaultCity = "DefaultCity";
    private const string DefaultZip = "DefaultZip";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected UserControl ucl;

    protected void Page_Load(object sender, EventArgs e)
    {
        // UpdateEmployeeData();
        if (Session["UserID"] == null)
        {
            Response.Redirect("/login.aspx?Logout=1", false);
        }

        lblMessage.Text = "";
        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            string pid = Session["CurrentNode"].ToString();
            int len = pid.Length;
            ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
            Session["Pid"] = ViewState["PageId"].ToString();
        }
        else
        {
            ViewState["PageId"] = "0";
        }

        ViewState["pwd"] = txtPassword.Text.Trim();
        txtPassword.Attributes.Add("value", ViewState["pwd"].ToString());
        txtConfirmPassword.Attributes.Add("value", ViewState["pwd"].ToString());
        string IsRequiredUserDepartmentTagging = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), "IsRequiredUserWiseDepartmentTagging", sConString);
        if (common.myStr(IsRequiredUserDepartmentTagging) == "Y")
        {
            lnkUserDepartmentTagging.Visible = true;
        }
        if (!IsPostBack)
        {
            ViewState["Mode"] = "new";
            ViewState["DoctorId"] = null;
            txtExpiryPeriod.Text = "15";
            txtExpirynotification.Text = "3";
            //////Check Mode= Edit Or New/////
            string strEmpNoEnable = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsEmployeeNoMandatory", sConString);
            if (strEmpNoEnable == "Y")
            {
                lblEmployeeno.Visible = true;
            }

            if (common.myStr(Application["IsSAPOrInterfaceEnabled"]).Equals("Y"))
            {
                txtemployeeno.Enabled = false;
                txtfirstname.Enabled = false;
                txtmiddlename.Enabled = false;
                txtlastname.Enabled = false;
                // ddlemployeetype.Enabled = false;
                // ddlEmploymentstatus.Enabled = false;
                ddltitle.Enabled = false;
                txtMobile.Enabled = false;
                ddlstatus.Enabled = false;
            }

            if (Request.QueryString["mode"] == "edit")
            {
                btnfind.Visible = true;
            }
            //////End Check Edit Or Mode/////
            Hashtable htIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            StringBuilder sbSQL = new StringBuilder();
            htIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));

            sbSQL.Append(" SELECT TitleId, Name FROM TitleMaster");

            sbSQL.Append(" SELECT DEPARTMENTID, DEPARTMENTNAME FROM DEPARTMENTMAIN WHERE ACTIVE=1 and HOSPITALLOCATIONID = @inyHospitalLocationId Order By DEPARTMENTNAME");

            sbSQL.Append(" SELECT ID, Description, EmployeeType FROM EmployeeType ORDER BY Description");

            ds = dl.FillDataSet(CommandType.Text, sbSQL.ToString(), htIn);
            //populate Kin Relation drop down control
            //dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT titleid,name FROM titlemaster");
            ddltitle.DataSource = ds.Tables[0];
            ddltitle.DataTextField = "Name";
            ddltitle.DataValueField = "TitleId";
            ddltitle.DataBind();
            //dr.Close();
            ddltitle.Items.Insert(0, "Select");
            ddltitle.Items[0].Value = "0";
            ConsultationVisit();
            BindddlEmploymentstatus();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //populate Kin Relation drop down control
            //dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT DEPARTMENTID,DEPARTMENTNAME FROM DEPARTMENTMAIN WHERE HOSPITALLOCATIONID=" + Session["HospitalLocationID"] + "Order By DEPARTMENTNAME");
            ddldepartment.DataSource = ds.Tables[1];
            ddldepartment.DataTextField = "DEPARTMENTNAME";
            ddldepartment.DataValueField = "DEPARTMENTID";
            ddldepartment.DataBind();
            //dr.Close();

            chkIsNoAppointmentAllowBeyondTime.Visible = false;
            chkMultipleAppointment.Visible = false;
            chkSendOrders.Visible = false;
            chkPrescribeMedication.Visible = false;
            chkEncounterReOpen.Visible = false;
            chkDecisionSupport.Visible = false;
            chkDose.Visible = false;


            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["Description"];
                item.Value = dr["ID"].ToString();
                item.Attributes.Add("EmployeeType", common.myStr(dr["EmployeeType"]).Trim());
                ddlemployeetype.Items.Add(item);
                item.DataBind();
            }

            ViewState["ds"] = ds.Tables[2];
            BindCountry();

            bindstate();
            BindGroups();
            BindFacility();
            bindStation();
            BindEntrySite();
            BindDefaultURL();
            chkUpdateLogin.Visible = false;

            chkEmail.Visible = true;
            pnlUserCredentaila.Enabled = true;

            if (Session["HospitalLocationId"] != null)
            {
                ClinicDefaults cd = new ClinicDefaults(Page);


                ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(cd.GetHospitalDefaults(DefaultCountry, Session["HospitalLocationId"].ToString())));
                //DataSet dss = new DataSet();
                //DAL.DAL obs = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //if (txtemployeeno.Text != "")
                //{
                //    dss = obs.FillDataSet(CommandType.Text, "select * from Employee where EmployeeNo=" + txtemployeeno.Text + " and StateId is not null");
                //    if (dss.Tables[0].Rows.Count > 0)
                //    {
                ddlcountry_SelectedIndexChanged(sender, e);
                // }
                // }
                ddlstate.SelectedIndex = ddlstate.Items.IndexOf(ddlstate.Items.FindByValue(cd.GetHospitalDefaults(DefaultState, Session["HospitalLocationId"].ToString())));
                ddlstate_SelectedIndexChanged(sender, e);

                ddlcity.SelectedIndex = ddlcity.Items.IndexOf(ddlcity.Items.FindByValue(cd.GetHospitalDefaults(DefaultCity, Session["HospitalLocationId"].ToString())));
                LocalCity_OnSelectedIndexChanged(sender, e);

                ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(cd.GetHospitalDefaults(DefaultZip, Session["HospitalLocationId"].ToString())));

            }
            if (Request.QueryString["EmpNo"] != null)
            {
                txtemployeeno.Text = Request.QueryString["EmpNo"].Trim();
                findoption(txtemployeeno.Text, 0);
                btnSaveEmployee.Text = "Update";
                ViewState["emp"] = Request.QueryString["EmpNo"];

                chkUpdateLogin.Visible = true;
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                txtConfirmPassword.Enabled = false;
                ddlHintQuestions.Enabled = false;
                txtHintAnswer.Enabled = false;
                chkLocked.Enabled = false;
                ddldefaultpage.Enabled = false;
                ddldfurl.Enabled = false;
                btnAvailability.Enabled = false;
                txtOtherHintQuestions.Enabled = false;
                //btnpasshelp.Enabled = false;
                txtExpiryPeriod.Enabled = false;
                txtExpirynotification.Enabled = false;
            }
            rdpDateFrom.SelectedDate = common.myDate(DateTime.Now.ToString());
        }
    }

    void BindDefaultURL()
    {
        try
        {
            BaseC.Security objSec = new BaseC.Security(sConString);
            DataSet ds;
            ds = objSec.GetDefaultURL(common.myInt(Session["FacilityId"]));
            ddldfurl.Items.Clear();
            ddldfurl.DataSource = ds.Tables[0];
            ddldfurl.DataTextField = "URLName";
            ddldfurl.DataValueField = "URLID";
            ddldfurl.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    // To make controls readonly if they mention in OracleNonEditableColumn
    private void NonEditableColumns()
    {
        DataSet ds = common.NonEditableColumns(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "Employee", sConString);
        DataView dv = ds.Tables[0].DefaultView;
        dv.RowFilter = "FormName = 'Employee'";
        foreach (DataRow dr in ((DataTable)dv.ToTable()).Rows)
        {
            string strControl = dr["ControlName"].ToString();
            if (dr["ControlType"].ToString() == "TextBox")
            {
                TextBox txt;
                txt = (TextBox)this.Page.Master.FindControl("ContentPlaceHolder1").FindControl(strControl.Trim());
                txt.ReadOnly = true;
            }
            else if (dr["ControlType"].ToString() == "DropDownList")
            {
                DropDownList txt;
                txt = (DropDownList)this.Page.Master.FindControl("ContentPlaceHolder1").FindControl(strControl.Trim());
                txt.Enabled = false;
            }
            else if (dr["ControlType"].ToString() == "RadComboBox")
            {
                RadComboBox txt;
                txt = (RadComboBox)this.Page.Master.FindControl("ContentPlaceHolder1").FindControl(strControl.Trim());
                txt.Enabled = false;
            }
            else if (dr["ControlType"].ToString() == "RadioButton")
            {
                RadioButton txt;
                txt = (RadioButton)this.Page.Master.FindControl("ContentPlaceHolder1").FindControl(strControl.Trim());
                txt.Enabled = false;
            }
        }
    }

    protected void chkUpdateLogin_Click(object sender, EventArgs e)
    {
        try
        {
            // pnlUserCredentaila.Enabled = true;
            if (chkUpdateLogin.Checked)
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                txtConfirmPassword.Enabled = true;
                ddlHintQuestions.Enabled = true;
                ddldefaultpage.Enabled = true;
                ddldfurl.Enabled = true;
                txtHintAnswer.Enabled = true;
                chkLocked.Enabled = true;
                btnAvailability.Enabled = true;
                txtOtherHintQuestions.Enabled = true;
                //btnpasshelp.Enabled = true;
                txtExpiryPeriod.Enabled = true;
                txtExpirynotification.Enabled = true;
                txtPassword.Text = "";
                txtPassword.Text.Trim();
                txtConfirmPassword.Text = "";
                txtConfirmPassword.Text.Trim();
                txtPassword.Attributes.Add("value", "");
                txtConfirmPassword.Attributes.Add("value", "");
                hdnPassword.Value = "";
                chkDoNotExpire.Enabled = true;
            }
            else
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                txtConfirmPassword.Enabled = false;
                ddlHintQuestions.Enabled = false;
                ddldefaultpage.Enabled = false;
                txtHintAnswer.Enabled = false;
                chkLocked.Enabled = false;
                ddldefaultpage.Enabled = false;
                btnAvailability.Enabled = false;
                txtOtherHintQuestions.Enabled = false;
                //btnpasshelp.Enabled = false;
                txtUserName.Enabled = false;
                txtExpiryPeriod.Enabled = false;
                txtExpirynotification.Enabled = false;
                chkDoNotExpire.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void bindStation()
    {
        try
        {
            BaseC.clsLISSampleReceivingStation objval = new BaseC.clsLISSampleReceivingStation(sConString);
            DataSet ds = objval.getMainData(0, common.myInt(Session["HospitalLocationID"]), 0);
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active=1";
            ddlStation.DataSource = dv;
            ddlStation.DataValueField = "StationId";
            ddlStation.DataTextField = "StationName";
            ddlStation.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void ConsultationVisit()
    {
        BaseC.EMRMasters objEmr = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();
        ds = objEmr.GetConsultationVisit();
        ds.Tables[0].DefaultView.RowFilter = "Type='CL'";
        if (ds.Tables[0].DefaultView.Count > 0)
        {
            if (ds.Tables[0].DefaultView.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;

                dv.RowFilter = "Code = 'FIV'";
                dv.Sort = "ServiceName ASC";
                ddlFirstVisit.DataSource = (DataTable)dv.ToTable();
                ddlFirstVisit.DataTextField = "ServiceName";
                ddlFirstVisit.DataValueField = "ServiceId";
                dv.RowFilter = "";

                dv.RowFilter = "Code = 'FUV'";
                dv.Sort = "ServiceName ASC";
                ddlFollowupVisit.DataSource = (DataTable)dv.ToTable();
                ddlFollowupVisit.DataTextField = "ServiceName";
                ddlFollowupVisit.DataValueField = "ServiceId";
                dv.RowFilter = "";

                dv.RowFilter = "Code = 'RV'";
                dv.Sort = "ServiceName ASC";
                ddlFreeVisit.DataSource = (DataTable)dv.ToTable();
                ddlFreeVisit.DataTextField = "ServiceName";
                ddlFreeVisit.DataValueField = "ServiceId";
                dv.RowFilter = "";


                dv.RowFilter = "Code In ('OFV','FIV')";
                dv.Sort = "ServiceName ASC";
                ddlOtherFirstVisit.DataSource = (DataTable)dv.ToTable();
                ddlOtherFirstVisit.DataTextField = "ServiceName";
                ddlOtherFirstVisit.DataValueField = "ServiceId";
                dv.RowFilter = "";

                ddlFirstVisit.DataBind();
                ddlFollowupVisit.DataBind();
                ddlFreeVisit.DataBind();
                ddlOtherFirstVisit.DataBind();
                if (chkIsMultipleFirstVisit.Checked == false)
                {
                    ddlOtherFirstVisit.Enabled = false;
                    ddlOtherFirstVisit.SelectedIndex = ddlOtherFirstVisit.Items.IndexOf(ddlOtherFirstVisit.Items.FindByValue("0"));
                }
            }
        }
        ds.Tables[0].DefaultView.RowFilter = "";
        ds.Tables[0].DefaultView.RowFilter = "Type IN('VL','VF')";
        if (ds.Tables[0].DefaultView.Count > 0)
        {
            if (ds.Tables[0].DefaultView.Count > 0)
            {


                ddlIpFirstvisit.DataSource = ds.Tables[0].DefaultView;
                ddlIpFirstvisit.DataTextField = "ServiceName";
                ddlIpFirstvisit.DataValueField = "ServiceId";
                ddlIpFirstvisit.DataBind();

                ddlIPSecondVisit.DataSource = ds.Tables[0].DefaultView;
                ddlIPSecondVisit.DataTextField = "ServiceName";
                ddlIPSecondVisit.DataValueField = "ServiceId";
                ddlIPSecondVisit.DataBind();

                DropFEmergencyVisit.DataSource = ds.Tables[0].DefaultView;
                DropFEmergencyVisit.DataTextField = "ServiceName";
                DropFEmergencyVisit.DataValueField = "ServiceId";
                DropFEmergencyVisit.DataBind();

                DropSEmergencyVisit.DataSource = ds.Tables[0].DefaultView;
                DropSEmergencyVisit.DataTextField = "ServiceName";
                DropSEmergencyVisit.DataValueField = "ServiceId";
                DropSEmergencyVisit.DataBind();
            }
        }


    }
    void BindddlEmploymentstatus()
    {
        try
        {
            DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationId"]));

            ds = dL.FillDataSet(CommandType.Text, "SELECT StatusId, Status FROM dbo.GetStatus(@inyHospitalLocationId,'Employment') ", hsinput);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlEmploymentstatus.DataSource = ds;
                ddlEmploymentstatus.DataValueField = "StatusId";
                ddlEmploymentstatus.DataTextField = "Status";
                ddlEmploymentstatus.DataBind();

            }
            ddlEmploymentstatus.Items.Insert(0, "Select");
            ddlEmploymentstatus.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void BindCountry()
    {
        try
        {
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            if (Cache["CountryTable"] == null)
            {
                DataSet objDs = objBc.GetPatientCountry();
                Cache.Insert("CountryTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                ddlcountry.DataSource = Cache["CountryTable"];
                ddlcountry.DataTextField = "CountryName";
                ddlcountry.DataValueField = "CountryId";
                ddlcountry.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["CountryTable"];
                ddlcountry.DataSource = objDs;
                ddlcountry.DataTextField = "CountryName";
                ddlcountry.DataValueField = "CountryId";
                ddlcountry.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void BindFacility()
    {
        try
        {
            if (common.myInt(hdnemployeeno.Text) > 0 && common.myStr(Application["IsSAPOrInterfaceEnabled"]).Equals("Y"))
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                cblfacility.DataSource = objSec.GetFacilityName("D", common.myInt(hdnemployeeno.Text), common.myStr(Application["IsSAPOrInterfaceEnabled"]));
                cblfacility.DataTextField = "Name";
                cblfacility.DataValueField = "FacilityId";
                cblfacility.DataBind();
            }
            else if (Session["HospitalLocationID"] != null)
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                cblfacility.DataSource = objSec.GetFacilityName(Convert.ToInt16(Session["HospitalLocationID"]));
                cblfacility.DataTextField = "Name";
                cblfacility.DataValueField = "FacilityId";
                cblfacility.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnAvailability_Click(object sender, EventArgs e)
    {
        ViewState["pwd"] = txtPassword.Text.Trim();
        if (checkAvailability() != null)
        {
            lblAvailabilityMessage.Text = "User Name Not Available!";
            lblAvailabilityMessage.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            lblAvailabilityMessage.Text = "User Name Available!";
            lblAvailabilityMessage.ForeColor = System.Drawing.Color.Green;
        }
        txtPassword.Attributes.Add("value", ViewState["pwd"].ToString());
        txtConfirmPassword.Attributes.Add("value", ViewState["pwd"].ToString());
    }

    protected string checkAvailability()
    {
        BaseC.ParseData objParse = new BaseC.ParseData();
        BaseC.User objUser = new BaseC.User(sConString);
        string sUserName = objParse.ParseQ(txtUserName.Text);
        string i = objUser.GetUserID(sUserName);
        return i;
    }
    protected string checkUserAvailabilityOnEdit()
    {
        BaseC.ParseData objParse = new BaseC.ParseData();
        BaseC.User objUser = new BaseC.User(sConString);
        string sUserName = objParse.ParseQ(txtUserName.Text);
        string i = objUser.checkUserAvailabilityOnEdit(sUserName, common.myInt(ViewState["DoctorId"]));
        return i;
    }

    //Bind Entry Site
    private void BindEntrySite()
    {
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                cbEntrySite.DataSource = objSec.GetEntrySite(Convert.ToInt16(Session["HospitalLocationID"]));
                cbEntrySite.DataTextField = "ESName";
                cbEntrySite.DataValueField = "ESId";
                cbEntrySite.DataBind();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    private void BindGroups()
    {
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                cblUserGroup.DataSource = objSec.GetGroupName(Convert.ToInt16(Session["HospitalLocationID"]));
                cblUserGroup.DataTextField = "GroupName";
                cblUserGroup.DataValueField = "GroupID";
                cblUserGroup.DataBind();
                foreach (ListItem item in cblUserGroup.Items)
                {
                    if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
                    {
                        item.Attributes.Add("Style", "background-color:Pink;");
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
    }

    //////For Employee
    int checkemployee()
    {
        int i = 0;
        try
        {
            BaseC.Security s = new BaseC.Security(sConString);
            bool check = false;
            check = s.checkempID(txtemployeeno.Text, Convert.ToInt32(Session["HospitalLocationID"]));

            if (check == true)
            {
                Alert.ShowAjaxMsg("Employee Number already exist.", Page);
                //Alert.ShowAjaxMsg("Employee Number already exist.", Page);
                txtemployeeno.Text = "";
                txtemployeeno.Focus();
                i = 1;
            }
            else
            {
                txtfirstname.Focus();
                i = 0;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        return i;
    }

    protected void btnfind_Click(object sender, EventArgs e)
    {
        //  BaseC.Security AuditCA = new BaseC.Security(sConString);
        if (common.myStr(hdnemployeeno.Text).Equals("") && !common.myStr(txtemployeeno.Text).Equals(""))
            hdnemployeeno.Text = txtemployeeno.Text.Trim();
        txtemployeeno.Text = hdnemployeeno.Text.Trim();
        findoption(txtemployeeno.Text.Trim(), 0);
        // AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), 0, 0, Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(hdnemployeeno.Text.Trim()), "ACCESSED", Convert.ToString(Session["IPAddress"]));
    }

    void findoption(string EmployeeId, int option)
    {
        try
        {
            txtUserName.Text = "";
            chkUpdateLogin.Visible = true;
            txtUserName.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
            chkMultipleAppointment.Checked = false;
            ddlHintQuestions.Enabled = false;
            txtHintAnswer.Enabled = false;
            chkLocked.Enabled = true;
            ddldefaultpage.Enabled = false;
            btnAvailability.Enabled = false;
            txtOtherHintQuestions.Enabled = false;
            //btnpasshelp.Enabled = false;
            txtExpiryPeriod.Enabled = false;
            txtExpirynotification.Enabled = false;
            Chkopdprovider.Checked = false;
            Chkipdprovider.Checked = false;
            chkCanDownloadPatientDocument.Checked = false;

            BaseC.ParseData objParse = new BaseC.ParseData();
            lnkProviderDetails.Visible = true;
            if (objParse.ParseQ(txtemployeeno.Text.Trim()) != "")
            {
                if (CheckDoctor(common.myInt(txtemployeeno.Text)) == 0)
                {
                    lnkAppointmentTemplate.Visible = false;
                }
                else
                {
                    lnkAppointmentTemplate.Visible = true;
                }
            }
            else
            {
                lnkAppointmentTemplate.Visible = false;
            }


            BaseC.EncryptDecrypt eN = new BaseC.EncryptDecrypt();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshtablein = new Hashtable();
            DataSet dsEmp;

            hshtablein.Add("intEmployeeId", EmployeeId);
            dsEmp = dl.FillDataSet(CommandType.StoredProcedure, "UspShowEmployeeDetail", hshtablein);

            if (dsEmp.Tables[0].Rows.Count > 0)
            {
                ViewState["Mode"] = "edit";
                ddlStation.Enabled = false;
                BaseC.ParseData Parse = new BaseC.ParseData();
                ViewState["emp"] = Parse.ParseQ(txtemployeeno.Text.Trim());
                DataRow dr = dsEmp.Tables[0].Rows[0] as DataRow;
                // ViewState["DoctorId"] = Convert.ToString(dr["Id"]);
                ddlemployeetype.SelectedValue = Convert.ToString(dr["EmployeeType"]);
                txtemployeeno.Text = Convert.ToString(dr["EmployeeNo"]);
                DataTable ds = (DataTable)ViewState["ds"];
                DataRow[] dr1 = ds.Select("ID=" + ddlemployeetype.SelectedValue);
                if (dr1.Length > 0)
                {
                    string strType = common.myStr(dr1[0]["EmployeeType"]).Trim();
                    if ((strType == "LIC") || (strType == "LS") || (strType == "LT") || (strType == "LD") || (strType == "LDIR"))
                    {
                        ddlStation.Enabled = true;
                    }
                    else
                    {
                        ddlStation.Enabled = false;
                    }
                    if (strType == "D")
                    {
                        if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isAllowLabStationToDoctor", sConString).Equals("Y"))
                        {
                            ddlStation.Enabled = true;
                        }
                    }//
                }



                if (common.myStr(dr["ProvidingService"]) == "O")
                {
                    Chkopdprovider.Checked = true;
                }
                if (common.myStr(dr["ProvidingService"]) == "I")
                {
                    Chkipdprovider.Checked = true;
                }
                if (common.myStr(dr["ProvidingService"]) == "B")
                {
                    Chkopdprovider.Checked = true;
                    Chkipdprovider.Checked = true;
                }
                ddltitle.SelectedIndex = ddltitle.Items.IndexOf(ddltitle.Items.FindByValue(dr["TitleId"].ToString()));
                //ddltitle.SelectedValue = Convert.ToString(dr["TitleId"]).Trim();
                txtEmpCode.Text = common.myStr(dr["EmpCode"]);
                ViewState["EmpName"] = Convert.ToString(dr["FirstName"]) + Convert.ToString(dr["MiddleName"]) + Convert.ToString(dr["LastName"]);
                txtfirstname.Text = Convert.ToString(dr["FirstName"]);
                txtmiddlename.Text = Convert.ToString(dr["MiddleName"]);
                txtlastname.Text = Convert.ToString(dr["LastName"]);
                txtEducation.Text = Convert.ToString(dr["Education"]);
                txtLAddress.Text = Convert.ToString(dr["Address1"]);
                txtLAddress2.Text = Convert.ToString(dr["Address2"]);
                chkPrescribeMedication.Checked = Convert.ToBoolean(dr["PrescribeMedication"]);
                chkSendOrders.Checked = Convert.ToBoolean(dr["SendOrders"]);
                chkIsMedicalProvider.Checked = Convert.ToBoolean(dr["IsMedicalProvider"]);
                chkCanDownloadPatientDocument.Checked = common.myBool(dr["CanDownloadPatientDocument"]);
                chkEmail.Checked = Convert.ToBoolean(dr["IsUserPostEmail"]);

                rdpDateFrom.SelectedDate = null;
                rdpDateto.SelectedDate = null;
                rdpDateFrom.SelectedDate = common.myDate(dr["JoiningDate"]);
                if (common.myStr(dr["LeavingDate"]) != "")
                {
                    rdpDateto.SelectedDate = common.myDate(dr["LeavingDate"]);
                }
                chkAccessSpecialisationResource.Checked = Convert.ToBoolean(dr["IsAccessSpecialisationResource"]);
                chkIsAccessAllEncounter.Checked = Convert.ToBoolean(dr["IsAccessAllEncounter"]);
                chkResource.Checked = Convert.ToBoolean(dr["IsResource"]);


                if (common.myStr(dr["IsNoAppointmentAllowBeyondTime"]) == "False" || common.myStr(dr["IsNoAppointmentAllowBeyondTime"]) == "0")
                    chkIsNoAppointmentAllowBeyondTime.Checked = false;
                else
                    chkIsNoAppointmentAllowBeyondTime.Checked = true;

                if (common.myStr(dr["MultipleAppointmentsPerSlot"]) == "False" || common.myStr(dr["MultipleAppointmentsPerSlot"]) == "0")
                    chkMultipleAppointment.Checked = false;
                else
                    chkMultipleAppointment.Checked = true;
                //Added by Ujjwal 28April2015 to capture Copy Clinical casesheet permission start
                if (common.myStr(dr["IsCopyClinicalCasesheet"]) == "False" || common.myStr(dr["IsCopyClinicalCasesheet"]) == "0")
                    chkCopyClinicalCaseshet.Checked = false;
                else
                    chkCopyClinicalCaseshet.Checked = true;
                //Added by Ujjwal 28April2015 to capture Copy Clinical casesheet permission start
                //Added by Ujjwal 19May2015 to capture Copy Clinical casesheet permission start
                if (common.myStr(dr["isEMRSuperUser"]) == "False" || common.myStr(dr["isEMRSuperUser"]) == "0")
                    chkIsEMRSuperUser.Checked = false;
                else
                    chkIsEMRSuperUser.Checked = true;
                //Added by Ujjwal 19May2015 to capture Copy Clinical casesheet permission start
                #region --- || Added By Abhishek Goel 02/03/2016 || ---
                chkIsPrintOrignalReceipt.Checked = common.myBool(dr["IsPrintOrignalOPIPReceipt"]);
                #endregion
                chkPrintCaseSheet.Checked = common.myBool(dr["UnablePrintCaseSheet"]);// added by abhishek goel
                chkEncounterReOpen.Checked = Convert.ToBoolean(dr["EncounterReopen"]);
                txtDiscountPercentAuthorised.Text = common.myStr(dr["DiscountPrecentage"]);
                txtDiscountPercentAuthorised.Text = common.myDec(txtDiscountPercentAuthorised.Text).ToString("F2");
                txtDesignation.Text = common.myStr(dr["Designation"]);
                txtRoomNo.Text = common.myStr(dr["RoomNo"]);
                if (dr["DecisionSupport"].ToString().Trim() != null && dr["DecisionSupport"].ToString().Trim() != "")
                {
                    chkDecisionSupport.Checked = Convert.ToBoolean(dr["DecisionSupport"]);
                }
                else
                {
                    chkDecisionSupport.Checked = false;
                }
                if (dr["CareNotification"].ToString().Trim() != null && dr["CareNotification"].ToString().Trim() != "")
                {
                    chkCareNotification.Checked = Convert.ToBoolean(dr["CareNotification"]);
                }
                else
                {
                    chkCareNotification.Checked = false;
                }

                chkDose.Checked = common.myBool(dr["CheckDose"]);

                //for employee type -D - Doctor ,E - Employee ,N - Nurse :  Employee Classification will be visible
                if (common.myInt(dr["EmployeeType"]) == 1 || common.myInt(dr["EmployeeType"]) == 2 || common.myInt(dr["EmployeeType"]) == 7)
                {
                    lnkClassification.Visible = true;
                }



                if (Convert.ToString(dr["MedicationSeverityValue"]) != "" && Convert.ToString(dr["MedicationSeverityValue"]) != null)
                {
                    ddlMedicationSeverity.SelectedValue = Convert.ToString(dr["MedicationSeverityValue"]);
                }
                else
                {
                    ddlMedicationSeverity.SelectedIndex = 0;
                }
                if (Convert.ToString(dr["AllergyPlausibilityValue"]) != "" && Convert.ToString(dr["AllergyPlausibilityValue"]) != null)
                {
                    ddlAllergyPlausibility.SelectedValue = Convert.ToString(dr["AllergyPlausibilityValue"]);
                }
                else
                {
                    ddlAllergyPlausibility.SelectedIndex = 0;
                }

                if (dr["PasswordExpiryPeriod"].ToString().Trim() != "0")
                {
                    txtExpiryPeriod.Text = dr["PasswordExpiryPeriod"].ToString().Trim();
                }
                else
                {
                    txtExpiryPeriod.Text = "";
                }
                if (dr["PasswordExpiryNotification"].ToString().Trim() != "0")
                {
                    txtExpirynotification.Text = dr["PasswordExpiryNotification"].ToString().Trim();
                }
                else
                {
                    txtExpirynotification.Text = "";
                }
                if (Convert.ToString(dr["countryid"]).Trim() != "" || Convert.ToString(dr["countryid"]).Trim() != "0")
                {
                    ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(dr["countryid"].ToString()));
                    ddlcountry_SelectedIndexChanged(this, null);
                }

                if (Convert.ToString(dr["stateid"]).Trim() != "" || Convert.ToString(dr["stateid"]).Trim() != "0")
                {
                    ddlstate.SelectedIndex = ddlstate.Items.IndexOf(ddlstate.Items.FindByValue(dr["stateid"].ToString()));
                    ddlstate_SelectedIndexChanged(this, null);
                }
                if (Convert.ToString(dr["cityid"]).Trim() != "" || Convert.ToString(dr["cityid"]).Trim() != "0")
                {
                    ddlcity.SelectedIndex = ddlcity.Items.IndexOf(ddlcity.Items.FindByValue(dr["cityid"].ToString()));
                    //ddlcity.SelectedValue = Convert.ToString(dr["cityid"]);
                    LocalCity_OnSelectedIndexChanged(this, null);
                }

                if (dr["pincode"].ToString().Trim() != "" || dr["pincode"].ToString().Trim() != "0")
                {
                    txtPin.Text = dr["pincode"].ToString();
                    //ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(dr["pincode"].ToString()));
                    //ddlZip.SelectedValue = Convert.ToString(dr["pincode"]);
                }
                else
                {
                    txtPin.Text = "";
                }
                txtPhoneHome.Text = Convert.ToString(dr["phonehome"]);
                txtMobile.Text = Convert.ToString(dr["mobile"]);
                txtEmailID.Text = Convert.ToString(dr["email"].ToString().Trim());
                ddldepartment.SelectedIndex = ddldepartment.Items.IndexOf(ddldepartment.Items.FindByValue(dr["departmentId"].ToString()));
                ddldepartment.ToolTip = ddldepartment.SelectedItem.Text;

                //ddldepartment.SelectedValue = Convert.ToString(dr["departmentId"]);
                ddlEmploymentstatus.SelectedIndex = ddlEmploymentstatus.Items.IndexOf(ddlEmploymentstatus.Items.FindByValue(dr["EmploymentStatusId"].ToString()));
                ViewState["DoctorId"] = Convert.ToString(dr["Id"]);
                ddlstatus.SelectedValue = dr["active"].ToString();
                if (common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "D" ||
                    common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "TM" ||
                    common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "LD" ||
                common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "LDIR")
                {
                    ddlFirstVisit.SelectedIndex = ddlFirstVisit.Items.IndexOf(ddlFirstVisit.Items.FindByValue(common.myStr(dr["FirstConsultationServiceId"])));
                    ddlFirstVisit.ToolTip = ddlFirstVisit.SelectedItem.Text;

                    ddlFollowupVisit.SelectedIndex = ddlFollowupVisit.Items.IndexOf(ddlFollowupVisit.Items.FindByValue(common.myStr(dr["FollowUpConsultationServiceId"])));
                    ddlOtherFirstVisit.SelectedIndex = ddlOtherFirstVisit.Items.IndexOf(ddlOtherFirstVisit.Items.FindByValue(common.myStr(dr["OtherFirstConsultationServiceId"])));
                    ddlFollowupVisit.ToolTip = ddlFollowupVisit.SelectedItem.Text;
                    ddlOtherFirstVisit.ToolTip = ddlOtherFirstVisit.SelectedItem.Text;

                    ddlFreeVisit.SelectedIndex = ddlFreeVisit.Items.IndexOf(ddlFreeVisit.Items.FindByValue(common.myStr(dr["FreeFollowUpConsultationServiceId"])));
                    ddlFreeVisit.ToolTip = ddlFreeVisit.SelectedItem.Text;
                    // Done By Abhishek Goel Start                  
                    //ddlIpFirstvisit.SelectedValue = common.myStr(dr["ipFirstVisit"]);
                    //ddlIPSecondVisit.SelectedValue = common.myStr(dr["ipSecondVisit"]);
                    ddlIpFirstvisit.SelectedIndex = ddlIpFirstvisit.Items.IndexOf(ddlIpFirstvisit.Items.FindByValue(common.myStr(dr["ipFirstVisit"])));
                    ddlIPSecondVisit.SelectedIndex = ddlIPSecondVisit.Items.IndexOf(ddlIPSecondVisit.Items.FindByValue(common.myStr(dr["ipSecondVisit"])));
                    // Done By Abhishek Goel End
                    ddlIpFirstvisit.ToolTip = ddlIpFirstvisit.SelectedItem.Text;
                    ddlIPSecondVisit.ToolTip = ddlIPSecondVisit.SelectedItem.Text;

                    // Done By Abhishek Goel Start 
                    //DropFEmergencyVisit.SelectedValue = common.myStr(dr["EmergencyIPFirstVisit"]);
                    //DropSEmergencyVisit.SelectedValue = common.myStr(dr["EmergencyIPSecondVisit"]);
                    DropFEmergencyVisit.SelectedIndex = DropFEmergencyVisit.Items.IndexOf(DropFEmergencyVisit.Items.FindByValue(common.myStr(dr["EmergencyIPFirstVisit"])));
                    DropSEmergencyVisit.SelectedIndex = DropSEmergencyVisit.Items.IndexOf(DropSEmergencyVisit.Items.FindByValue(common.myStr(dr["EmergencyIPSecondVisit"])));
                    // Done By Abhishek Goel End
                    DropFEmergencyVisit.ToolTip = ddlIpFirstvisit.SelectedItem.Text;
                    DropSEmergencyVisit.ToolTip = ddlIPSecondVisit.SelectedItem.Text;

                    ddlFirstVisit.Enabled = true;
                    ddlFollowupVisit.Enabled = true;
                    ddlOtherFirstVisit.Enabled = true;
                    ddlFreeVisit.Enabled = true;
                    ddlIpFirstvisit.Enabled = true;
                    ddlIPSecondVisit.Enabled = true;
                    DropFEmergencyVisit.Enabled = true;
                    DropSEmergencyVisit.Enabled = true;
                }
                else
                {
                    ddlFirstVisit.SelectedIndex = -1;
                    ddlFollowupVisit.SelectedIndex = -1;
                    ddlOtherFirstVisit.SelectedIndex = -1;
                    ddlFreeVisit.SelectedIndex = -1;
                    ddlFirstVisit.Enabled = false;
                    ddlFollowupVisit.Enabled = false;
                    ddlOtherFirstVisit.Enabled = false;
                    ddlFreeVisit.Enabled = false;
                    ddlIpFirstvisit.Enabled = false;
                    ddlIPSecondVisit.Enabled = false;
                    DropFEmergencyVisit.Enabled = false;
                    DropSEmergencyVisit.Enabled = false;
                }
                ddlemployeetype_OnSelectedIndexChanged(null, null);
                chkIsMedicalProvider_OnCheckedChanged(null, null);

                chkAccessAllResource.Checked = Convert.ToBoolean(dr["IsAccessAllResource"]);
                chkIsMultipleFirstVisit.Checked = Convert.ToBoolean(dr["IsMultipleFirstVisit"]);
                if (chkIsMultipleFirstVisit.Checked == false)
                {
                    ddlOtherFirstVisit.Enabled = false;
                    ddlOtherFirstVisit.SelectedIndex = ddlOtherFirstVisit.Items.IndexOf(ddlOtherFirstVisit.Items.FindByValue("0"));
                }

                string Password = "";
                if (common.myStr(dr["Password"]) != "")
                {
                    Password = eN.Decrypt(dr["Password"].ToString(), eN.getKey(sConString), true);
                    txtPassword.Attributes.Add("value", eN.Decrypt(dr["Password"].ToString(), eN.getKey(sConString), true));
                    txtConfirmPassword.Attributes.Add("value", eN.Decrypt(dr["Password"].ToString(), eN.getKey(sConString), true));

                    txtUserName.Text = eN.Decrypt(dr["UserName"].ToString(), eN.getKey(sConString), true);
                }
                hshInput = new Hashtable();
                hshInput.Add("@UserName", eN.Encrypt(txtUserName.Text.Trim(), eN.getKey(sConString), true, ""));
                hdnPassword.Value = "";
                SqlDataReader objDr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u INNER JOIN UserPrivateKey up ON u.ID = up.UserId WHERE UserName = @UserName", hshInput);
                string sPrivateKey = "";

                if (objDr.Read())
                {
                    sPrivateKey = common.myStr(objDr["PrivateKey"]).Trim();
                    Password = Password.Replace(sPrivateKey, "");
                    hdnPassword.Value = Password;
                }

                //txtPassword.Text = eN.Decrypt(dr["Password"].ToString(), eN.getKey(sConString), true);
                //txtConfirmPassword.Text = eN.Decrypt(dr["Password"].ToString(), eN.getKey(sConString), true);

                if (common.myStr(dr["HintQuestion"]) != null && common.myStr(dr["HintQuestion"]) != "")
                {
                    if (ddlHintQuestions.Items.FindByText(eN.Decrypt(Convert.ToString(dr["HintQuestion"]), eN.getKey(sConString), true)) != null)
                    {
                        ddlHintQuestions.SelectedItem.Selected = false;
                        ddlHintQuestions.Items.FindByText(eN.Decrypt(Convert.ToString(dr["HintQuestion"]), eN.getKey(sConString), true)).Selected = true;
                    }
                    else
                        txtOtherHintQuestions.Text = eN.Decrypt(Convert.ToString(dr["HintQuestion"]), eN.getKey(sConString), true);
                }

                if (common.myStr(dr["HintAnswer"]) != null && common.myStr(dr["HintAnswer"]) != "")
                {
                    txtHintAnswer.Text = eN.Decrypt(dr["HintAnswer"].ToString(), eN.getKey(sConString), true);
                }
                if (dr["IsLocked"] != null && dr["IsLocked"].ToString() != "")
                {
                    chkLocked.Checked = Convert.ToBoolean(dr["IsLocked"]);
                }
                if (dr["defaultpageid"] != null && dr["defaultpageid"].ToString() != "")
                {
                    ddldefaultpage.SelectedValue = Convert.ToString(dr["defaultpageid"]);
                }
                if (dr["defaultURLid"] != null && dr["defaultURLid"].ToString() != "" && common.myInt(dr["defaultURLid"]) > 0)
                {
                    ddldfurl.SelectedValue = Convert.ToString(dr["defaultURLid"]);
                }
                NonEditableColumns();
                chkAdmissionAuthorised.Checked = Convert.ToBoolean(dr["AdmissionAuthorised"]);
                chkRefundAuthorizationAboveMaxLimit.Checked = common.myBool(dr["RefundAuthorizationAboveMaxLimit"]);
                chkIncludeForDischargeSummary.Checked = common.myBool(dr["IsIncludeForDischargeSummary"]);

                chkDoNotExpire.Checked = common.myBool(dr["NeverExpirePwd"]);


                chkAllowPanelExcludItems.Checked = Convert.ToBoolean(dr["AllowPanelExcludedItems"]);

                cblUserGroup.ClearSelection();
                for (int i = 0; i < dsEmp.Tables[1].Rows.Count; i++)
                {
                    ListItem lstUserGroup = (ListItem)cblUserGroup.Items.FindByValue(dsEmp.Tables[1].Rows[i]["GroupId"].ToString().Trim());
                    if (lstUserGroup != null)
                    {
                        lstUserGroup.Selected = true;
                    }
                }
                BindFacility();
                cblfacility.ClearSelection();
                for (int i = 0; i < dsEmp.Tables[2].Rows.Count; i++)
                {
                    //cblfacility.SelectedIndex = cblfacility.Items.IndexOf(cblfacility.Items.FindByValue(dsEmp.Tables[2].Rows[i]["FacilityId"].ToString().Trim()));
                    ListItem lstFacility = (ListItem)cblfacility.Items.FindByValue(dsEmp.Tables[2].Rows[i]["FacilityId"].ToString().Trim());
                    if (lstFacility != null)
                    {
                        lstFacility.Selected = true;
                    }
                }



                for (int i = 0; i < dsEmp.Tables[4].Rows.Count; i++)
                {
                    foreach (RadComboBoxItem item in ddlStation.Items)
                    {
                        if (item.Value == common.myStr(dsEmp.Tables[4].Rows[i]["StationId"]))
                        {
                            CheckBox chkStation = (CheckBox)item.FindControl("chkStationId");
                            chkStation.Checked = true;
                            break;
                        }
                    }
                }



                for (int i = 0; i < dsEmp.Tables[5].Rows.Count; i++)
                {
                    //cblfacility.SelectedIndex = cblfacility.Items.IndexOf(cblfacility.Items.FindByValue(dsEmp.Tables[2].Rows[i]["FacilityId"].ToString().Trim()));
                    ListItem lstEntrySite = (ListItem)cbEntrySite.Items.FindByValue(dsEmp.Tables[5].Rows[i]["ESId"].ToString().Trim());
                    if (lstEntrySite != null)
                    {
                        lstEntrySite.Selected = true;
                    }
                }


                btnSaveEmployee.Text = "Update";
                if (dsEmp.Tables[3].Rows.Count > 0)
                {
                    ShowImage();
                }
                else
                {
                    PatientImage.ImageUrl = "/Images/patientLeft.jpg";
                }
            }
            //else
            //{
            //    Alert.ShowAjaxMsg("Record Not Found.", Page);
            //}
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
        btnSaveEmployee.Text = "Save";
        Response.Redirect("employee.aspx", false);
    }

    protected void btnSaveEmployee_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;

            StringBuilder strUserGroups = new StringBuilder();
            StringBuilder strFacilities = new StringBuilder();
            StringBuilder strStation = new StringBuilder();
            StringBuilder strEntrySite = new StringBuilder();

            foreach (ListItem item in cblUserGroup.Items)
            {
                if (item.Selected == true)
                {
                    strUserGroups.Append("<Table1>");
                    strUserGroups.Append("<c1>");
                    strUserGroups.Append(item.Value);
                    strUserGroups.Append("</c1>");
                    strUserGroups.Append("</Table1>");
                }
            }

            foreach (ListItem item in cblfacility.Items)
            {
                if (item.Selected == true)
                {
                    strFacilities.Append("<Table1>");
                    strFacilities.Append("<c1>");
                    strFacilities.Append(item.Value);
                    strFacilities.Append("</c1>");
                    strFacilities.Append("</Table1>");
                }
            }

            foreach (ListItem item in cbEntrySite.Items)
            {
                if (item.Selected == true)
                {
                    strEntrySite.Append("<Table1>");
                    strEntrySite.Append("<c1>");
                    strEntrySite.Append(item.Value);
                    strEntrySite.Append("</c1>");
                    strEntrySite.Append("</Table1>");
                }
            }

            if (lblEmployeeno.Visible)
            {
                if (common.myStr(txtemployeeno.Text).Trim().Length == 0)
                {
                    Alert.ShowAjaxMsg("Please! enter employee no", Page);
                    return;
                }
            }

            if (ddlStation.Enabled == true && !common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]).Equals("N"))
            {
                foreach (RadComboBoxItem item in ddlStation.Items)
                {
                    CheckBox chkStation = (CheckBox)item.FindControl("chkStationId");
                    if (chkStation.Checked == true)
                    {
                        strStation.Append("<Table1>");
                        strStation.Append("<c1>");
                        strStation.Append(item.Value);
                        strStation.Append("</c1>");
                        strStation.Append("</Table1>");
                    }
                }
                if (strStation.Length == 0 && !common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]).Equals("D"))
                {
                    Alert.ShowAjaxMsg("Select Lab Station", Page);
                    return;
                }
            }
            if (strUserGroups.ToString() == "")
            {

                Alert.ShowAjaxMsg("Select Group(s)!", Page);
                return;
            }

            if (strFacilities.ToString() == "")
            {

                Alert.ShowAjaxMsg("Select Facility !", Page);
                return;
            }

            //if (strEntrySite.ToString() == "")
            //{

            //    Alert.ShowAjaxMsg("Select EntrySite !", Page);
            //    return;
            //}



            if (txtPassword.Text.ToString() == "")
            {

                Alert.ShowAjaxMsg("Enter Password !", Page);
                return;
            }



            if (txtConfirmPassword.Text.ToString() == "")
            {

                Alert.ShowAjaxMsg("Re-enter Password !", Page);
                return;
            }
            if (chkDoNotExpire.Checked == false)
            {
                if (txtExpiryPeriod.Text.ToString().Trim() == "")
                {
                    Alert.ShowAjaxMsg("Please Enter Expiry Period", Page);
                    return;
                }

                if (txtExpirynotification.Text.ToString().Trim() == "")
                {
                    Alert.ShowAjaxMsg("Please Enter Expiry Notification Priod", Page);
                    return;
                }
            }
            if (ViewState["Mode"].ToString().Trim() == "edit")
            {

                if (checkUserAvailabilityOnEdit() != null)
                {
                    //lblMessage.Text = "User Name Not Available!";
                    //lblAvailabilityMessage.ForeColor = System.Drawing.Color.Red;
                    Alert.ShowAjaxMsg("User Name Not Available!", Page);
                    return;
                }

                SaveRecord(Convert.ToInt32(ViewState["DoctorId"]), strUserGroups.ToString(), strFacilities.ToString(), common.myStr(strStation), common.myStr(strEntrySite));


                //Alert.ShowAjaxMsg("Employee Detail Updated.",Page);

                findoption(Convert.ToString(ViewState["DoctorId"]), 0);

                //if (lblMessage.Text == "Can Not In-Active When Only One Employee Exist!")
                //{
                //    lblMessage.Text = "Can Not In-Active When Only One Employee Exist!";
                //}
                //else
                //{
                //    lblMessage.Text = "Employee Detail Updated...";
                //}
            }
            else
            {
                if (txtemployeeno.Text.Trim().Length > 0)
                {
                    i = checkemployee();
                }

                if (checkAvailability() != null)
                {
                    //lblMessage.Text = "User Name Not Available!";
                    //lblAvailabilityMessage.ForeColor = System.Drawing.Color.Red;
                    Alert.ShowAjaxMsg("User Name Not Available!", Page);
                    return;
                }
                if (txtemployeeno.Text.Trim().Length > 0)
                {
                    i = checkemployee();
                }




                if (i == 0)
                {
                    BaseC.ParseData Parse = new BaseC.ParseData();
                    SaveRecord(0, common.myStr(strUserGroups), common.myStr(strFacilities), common.myStr(strStation), common.myStr(strEntrySite));

                    //Alert.ShowAjaxMsg("Employee Detail Saved.", Page);
                    lblMessage.Text = "Record(s) Has Been Saved.";
                    ViewState["emp"] = Parse.ParseQ(txtemployeeno.Text.Trim());
                    ViewState["Mode"] = "edit";
                }

            }
            //btnSaveEmployee.Text = "Save";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    //
    void SaveRecord(Int32 EmployeeId, String strUserGroups, String strUserFacilities, String LabStation, String strEntrySite)
    {
        try
        {
            if ((common.myInt(ddlIpFirstvisit.SelectedValue) > 0) || (common.myInt(ddlIPSecondVisit.SelectedValue) > 0))
            {
                if (common.myInt(ddlIpFirstvisit.SelectedValue) == common.myInt(ddlIPSecondVisit.SelectedValue))
                {
                    Alert.ShowAjaxMsg("IP first and second visit cannot be same", Page);
                    return;
                }
            }

            BaseC.EncryptDecrypt eN = new BaseC.EncryptDecrypt();
            BaseC.ParseData objParse = new BaseC.ParseData();
            DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            Hashtable hshtableout = new Hashtable();
            Hashtable hshtablein = new Hashtable();
            //if (chkUpdateLogin.Checked)
            hshInput.Add("UserName", eN.Encrypt(txtUserName.Text.Trim(), eN.getKey(sConString), true, ""));
            //else
            //    hshInput.Add("UserName", txtUserName.Text.Trim());
            SqlDataReader objDr = (SqlDataReader)dL.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u INNER JOIN UserPrivateKey up ON u.ID = up.UserId WHERE UserName = @UserName", hshInput);
            string sUserName = "", sPrivateKey = "";
            //byte[] bPrivateKey;
            if (objDr.Read())
            {
                sUserName = objDr["UserName"].ToString();
                sPrivateKey = objDr["PrivateKey"].ToString().Trim();

                //bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);
            }
            else
            {
                //RNGCryptoServiceProvider rng;
                //rng = new RNGCryptoServiceProvider();
                //bPrivateKey = new byte[8];

                //rng.GetBytes(bPrivateKey);
                //sPrivateKey = Convert.ToBase64String(bPrivateKey);

                sPrivateKey = System.Guid.NewGuid().ToString();
            }

            string provider = "";

            if (Chkopdprovider.Checked == true)
            {
                provider = "O";
            }
            if (Chkipdprovider.Checked == true)
            {
                provider = "I";
            }
            if (Chkipdprovider.Checked == true && Chkopdprovider.Checked == true)
            {
                provider = "B";
            }

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hshtablein.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
            hshtablein.Add("chvEmployeeNo", bc.ParseQ(txtemployeeno.Text.Trim()));
            hshtablein.Add("inyEmployeeType", ddlemployeetype.SelectedValue);
            hshtablein.Add("intEmploymentStatusId", ddlEmploymentstatus.SelectedValue);
            if (ddltitle.SelectedValue != "0")
                hshtablein.Add("inyTitleId", ddltitle.SelectedValue);

            hshtablein.Add("@chvempCode", common.myStr(txtEmpCode.Text));
            hshtablein.Add("chvFirstName", bc.ParseQ(txtfirstname.Text));
            hshtablein.Add("chvMiddleName", bc.ParseQ(txtmiddlename.Text));
            hshtablein.Add("chvLastName", bc.ParseQ(txtlastname.Text));
            hshtablein.Add("chvEducation", bc.ParseQ(txtEducation.Text.Trim()));
            hshtablein.Add("mnyDiscountPercentAuthorised", common.myDec(txtDiscountPercentAuthorised.Text));
            hshtablein.Add("chvDesignation", common.myStr(txtDesignation.Text));

            if (txtLAddress.Text.Length > 0)
            {
                hshtablein.Add("chvAddress", bc.ParseQ(txtLAddress.Text));
            }
            if (txtLAddress2.Text.Length > 0)
            {
                hshtablein.Add("chvAddress2", bc.ParseQ(txtLAddress2.Text));
            }

            if (ddlcity.SelectedValue != "0")
            {
                hshtablein.Add("insCityId", ddlcity.SelectedValue);
            }
            if (ddlstate.SelectedValue != "0")
            {
                hshtablein.Add("insStateId", ddlstate.SelectedValue);
            }
            if (ddlcountry.SelectedValue != "0")
            {
                hshtablein.Add("insCountryId", ddlcountry.SelectedValue);
            }
            if (txtPin.Text != "")
            {
                hshtablein.Add("chvPinCode", txtPin.Text);
            }
            if (txtPhoneHome.Text != "___-___-____")
                hshtablein.Add("chvPhoneHome", bc.ParseQ(txtPhoneHome.Text));
            else
                hshtablein.Add("chvPhoneHome", "");

            if (txtMobile.Text != "___-___-____")
                hshtablein.Add("chvMobile", bc.ParseQ(txtMobile.Text));
            else
                hshtablein.Add("chvMobile", "");


            hshtablein.Add("chvEmail", bc.ParseQ(txtEmailID.Text));
            if (ddldepartment.SelectedValue != "")
            {
                hshtablein.Add("intDepartmentId", ddldepartment.SelectedValue);
            }
            hshtablein.Add("intEncodedBy", Session["UserID"]);
            hshtablein.Add("intEmpId", EmployeeId);

            if (sUserName == "")
            {
                hshtablein.Add("UserName", eN.Encrypt(objParse.ParseQ(txtUserName.Text.Trim()), eN.getKey(sConString), true, ""));
            }
            else
                hshtablein.Add("UserName", sUserName);
            hshtablein.Add("@sActUserName", txtUserName.Text);
            hshtablein.Add("@sActPassword", hdnPassword.Value == "" ? txtPassword.Text : hdnPassword.Value);

            if (chkUpdateLogin.Checked)
                hshtablein.Add("Password", eN.Encrypt(objParse.ParseQ(txtPassword.Text.Trim()), eN.getKey(sConString), true, sPrivateKey));
            else
                if (chkUpdateLogin.Visible && chkUpdateLogin.Checked == false)
                hshtablein.Add("Password", eN.Encrypt(objParse.ParseQ(txtPassword.Text.Trim()), eN.getKey(sConString), true, ""));
            else
                hshtablein.Add("Password", eN.Encrypt(objParse.ParseQ(txtPassword.Text.Trim()), eN.getKey(sConString), true, sPrivateKey));


            hshtablein.Add("HintQuestion", eN.Encrypt(objParse.ParseQ(ddlHintQuestions.SelectedItem.Text), eN.getKey(sConString), true, ""));
            hshtablein.Add("HintAnswer", eN.Encrypt(objParse.ParseQ(txtHintAnswer.Text), eN.getKey(sConString), true, ""));
            hshtablein.Add("IsLocked", chkLocked.Checked);
            hshtablein.Add("chvRoomNo", txtRoomNo.Text);

            //txtRoomNo
            hshtablein.Add("@PrivateKey", sPrivateKey);

            hshtablein.Add("@xmlUserGroup", strUserGroups);
            hshtablein.Add("@xmlUserFacility", strUserFacilities);
            hshtablein.Add("@xmlEntrySite", strEntrySite);

            hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);
            hshtableout.Add("intDoctorId", SqlDbType.Int);

            hshtablein.Add("@intLoginFacilityId", Session["FacilityID"]);
            hshtablein.Add("@intPageId", ViewState["PageId"]);

            hshtablein.Add("@bitPrescribeMedication", chkPrescribeMedication.Checked);
            hshtablein.Add("@bitSendOrders", chkSendOrders.Checked);
            hshtablein.Add("@bitIsMedicalProvider", chkIsMedicalProvider.Checked);
            hshtablein.Add("@bitCanDownloadPatientDocument", chkCanDownloadPatientDocument.Checked);
            hshtablein.Add("@bitIsUserPostEmail", chkEmail.Checked);
            hshtablein.Add("@bitEncounterReOpen", chkEncounterReOpen.Checked);
            hshtablein.Add("@Active", ddlstatus.SelectedValue);
            hshtablein.Add("@intPwdExpirePeriod", objParse.ParseQ(txtExpiryPeriod.Text.Trim()));
            hshtablein.Add("@intPwdExpireNotification", objParse.ParseQ(txtExpirynotification.Text.Trim()));
            hshtablein.Add("@bitNeverExpirePwd", chkDoNotExpire.Checked);
            hshtablein.Add("@intSeverityValue", ddlMedicationSeverity.SelectedValue);
            hshtablein.Add("@intPlausibilityValue", ddlAllergyPlausibility.SelectedValue);
            hshtablein.Add("@intSeverityText", ddlMedicationSeverity.SelectedItem.Text);
            hshtablein.Add("@intPlausibilityText", ddlAllergyPlausibility.SelectedItem.Text);
            hshtablein.Add("@xmlLabStation", common.myStr(LabStation));

            if (common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "D"
                || common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "TM"
                || common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "LDIR"
                || common.myStr(ddlemployeetype.SelectedItem.Attributes["EmployeeType"]) == "LD")
            {
                hshtablein.Add("@intFirstConsultationServiceId", common.myStr(ddlFirstVisit.SelectedValue));
                hshtablein.Add("@intFollowUpConsultationServiceId", common.myStr(ddlFollowupVisit.SelectedValue));
                hshtablein.Add("@intFreeFollowUpConsultationServiceId", common.myStr(ddlFreeVisit.SelectedValue));
                hshtablein.Add("@intOtherFirstConsultationServiceId", common.myStr(ddlOtherFirstVisit.SelectedValue));
                //
                hshtablein.Add("@intEmergencyIPFirstVisitServiceId", common.myStr(DropFEmergencyVisit.SelectedValue));
                hshtablein.Add("@intEmergencyIPSecondVisitServiceId", common.myStr(DropSEmergencyVisit.SelectedValue));
            }
            else
            {
                hshtablein.Add("@intFirstConsultationServiceId", common.myStr("0"));
                hshtablein.Add("@intFollowUpConsultationServiceId", common.myStr("0"));
                hshtablein.Add("@intFreeFollowUpConsultationServiceId", common.myStr("0"));
                hshtablein.Add("@intOtherFirstConsultationServiceId", common.myStr("0"));
                //
                hshtablein.Add("@intEmergencyIPFirstVisitServiceId", common.myStr("0"));
                hshtablein.Add("@intEmergencyIPSecondVisitServiceId", common.myStr("0"));
            }


            if (chkDecisionSupport.Checked == false)
            {
                hshtablein.Add("@bitDecisionSupport", Convert.ToBoolean(0));
            }
            else
            {
                hshtablein.Add("@bitDecisionSupport", chkDecisionSupport.Checked);
            }
            if (chkCareNotification.Checked == false)
            {
                hshtablein.Add("@bitCareNotification", Convert.ToBoolean(0));
            }
            else
            {
                hshtablein.Add("@bitCareNotification", chkCareNotification.Checked);
            }

            hshtablein.Add("@bitCheckDose", chkDose.Checked);

            if (ddldefaultpage.SelectedValue != "0")
            {
                hshtablein.Add("@intDefaultPageId", ddldefaultpage.SelectedValue);
            }
            if (ddldfurl.SelectedValue != "0")
            {
                hshtablein.Add("@intDefaultURLId", ddldfurl.SelectedValue);
            }
            hshtablein.Add("@bitAdmissionAuthorised", chkAdmissionAuthorised.Checked);
            hshtablein.Add("@bitRefundAuthorizationAboveMaxLimit", chkRefundAuthorizationAboveMaxLimit.Checked);
            hshtablein.Add("@bitIsIncludeForDischargeSummary", chkIncludeForDischargeSummary.Checked);
            hshtablein.Add("@chvUserName", txtUserName.Text);
            hshtablein.Add("@chvPassword", txtPassword.Text);

            int chkAppointmentAllow = 0;
            int ichkMultipleApp = 0;
            if (chkIsNoAppointmentAllowBeyondTime.Checked)
                chkAppointmentAllow = 1;
            if (chkMultipleAppointment.Checked)
                ichkMultipleApp = 1;
            int CopyClinicalCaseshet = 0;
            if (chkCopyClinicalCaseshet.Checked)
                CopyClinicalCaseshet = 1;

            bool isPrintCaseSheet = Convert.ToBoolean(chkPrintCaseSheet.Checked); // Added by abhishek goel

            int IsEMRSuperUser = 0;
            if (chkIsEMRSuperUser.Checked == true)
                IsEMRSuperUser = 1;
            hshtablein.Add("@bitIsNoAppointmentAllowBeyondTime", common.myInt(chkAppointmentAllow));
            hshtablein.Add("@bitIsMultipleAppointmentsPerSlot", common.myInt(ichkMultipleApp));

            hshtablein.Add("@bitIsAccessAllResource", chkAccessAllResource.Checked);
            hshtablein.Add("@bitIsMultipleFirstVisit", chkIsMultipleFirstVisit.Checked);
            hshtablein.Add("@bitIsAccessSpecialisationResource", chkAccessSpecialisationResource.Checked);
            hshtablein.Add("@bitIsAccessAllEncounter", chkIsAccessAllEncounter.Checked);
            hshtablein.Add("@bitIsResource", chkResource.Checked);
            hshtablein.Add("@intIPFirstVisit", ddlIpFirstvisit.SelectedValue);
            hshtablein.Add("@intIPsecondVisit", ddlIPSecondVisit.SelectedValue);
            hshtablein.Add("@ProvidingService", provider);
            hshtablein.Add("@bitIsCopyClinicalCasesheet", common.myBool(CopyClinicalCaseshet));
            hshtablein.Add("@bitIsEMRSuperUser", common.myBool(IsEMRSuperUser));
            hshtablein.Add("@bitUnablePrintCaseSheet", isPrintCaseSheet);   // Added by abhishek goel
            hshtablein.Add("@bitIsPrintOrignalReceipt", chkIsPrintOrignalReceipt.Checked); // Added by Abhishek Goel 02/03/2016
            hshtablein.Add("@bitAllowPanelExludeItem", chkAllowPanelExcludItems.Checked);

            hshtablein.Add("@DateOfJoining", rdpDateFrom.SelectedDate);
            hshtablein.Add("@DateOfLeaving", rdpDateto.SelectedDate);

            hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEmployee", hshtablein, hshtableout);
            //hshtableout = dl.ExecuteNonQuery(CommandType .StoredProcedure ,"",hshtablein ,hshtableout );  

            ViewState["DoctorId"] = hshtableout["intDoctorId"];
            //DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/PatientImages"));
            //if (objDir.Exists == true)
            //{
            //    FileInfo[] fi_array = objDir.GetFiles();
            //    foreach (FileInfo files in fi_array)
            //    {
            //        if (files.Exists)
            //        {
            //            SaveImage(Convert.ToString( ViewState["DoctorId"]));
            //        }
            //    }
            //}

            if (lblMessage.Text == "Can Not In-Active When Only One Employee Exist!")
            {
                lblMessage.Text = "Can Not In-Active When Only One Employee Exist!";
            }
            else
            {
                lblMessage.Text = common.myStr(hshtableout["chvErrorStatus"]);
            }
            if (txtImageId.Text != "")
            {
                SaveImage(Convert.ToString(ViewState["DoctorId"]));
            }

            if (hshtableout["chvErrorStatus"].ToString() == "Can Not In-Active When Only One Employee Exist!")
            {
                lblMessage.Text = "Can Not In-Active When Only One Employee Exist!";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void LocalCity_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            string strCityID = "";
            ddlZip.Items.Clear();
            ddlZip.SelectedValue = null;
            if (ddlcity.SelectedValue == "0")
            {
                strCityID = "null";
            }
            else
            {
                strCityID = ddlcity.SelectedValue;
            }
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID='" + strCityID + "' ORDER BY ZipCode");
            if (dr.HasRows)
            {
                dr.Read();
                txtPin.Text = dr["ZipCode"].ToString();
                //ddlZip.DataSource = dr;

                //ddlZip.DataTextField = "ZipCode";
                //ddlZip.DataValueField = "ZIPID";
                //ddlZip.DataBind();
                dr.Close();
            }

            ddlZip.Items.Insert(0, "Select");
            ddlZip.Items[0].Value = "0";

            //ddlZip.Items.Insert(0, "Select");
            //ddlZip.Items[0].Value = "0";
            ddlZip.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlstate.SelectedValue = null;
            ddlstate.Items.Clear();
            //populate Local State drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlcountry.SelectedValue.ToString() + "' ORDER BY StateName");
            if (ddlcountry.SelectedValue.ToString() != "0")
            {

                ddlstate.DataSource = dr;
                ddlstate.DataTextField = "StateName";
                ddlstate.DataValueField = "StateID";
                ddlstate.DataBind();

            }
            dr.Close();
            ListItem L = new ListItem("Select", "0");
            ddlstate.Items.Insert(0, L);

            //ddlWorkState.Items.Insert(0, "Select");
            //ddlWorkState.Items[0].Value = "0";

            ddlstate_SelectedIndexChanged(sender, e);

            ddlstate.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlcity.SelectedValue = null;
            ddlcity.Items.Clear();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname from CityMaster where StateId='" + ddlstate.SelectedValue.ToString() + "' ORDER BY cityname");
            ddlcity.DataSource = dr;
            ddlcity.DataTextField = "CityName";
            ddlcity.DataValueField = "CityID";
            ddlcity.DataBind();
            dr.Close();
            ListItem L = new ListItem("Select", "0");
            ddlcity.Items.Insert(0, L);

            //ddlWorkCity.Items.Insert(0, "[Select]");
            //ddlWorkCity.Items[0].Value = "0";
            LocalCity_OnSelectedIndexChanged(sender, e);
            ddlcity.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void bindstate()
    {
        try
        {
            if (ddlcountry.SelectedValue.Trim() != "")
            {
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                SqlDataReader dr;
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@CountryId", Convert.ToInt32(ddlcountry.SelectedValue));
                dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT stateid,statename FROM statemaster where Active = 1 and CountryId = @CountryId order by statename", hshIn);
                if (dr.RecordsAffected != -1)
                {
                    ddlstate.DataSource = dr;
                    ddlstate.DataTextField = "statename";
                    ddlstate.DataValueField = "stateid";
                    ddlstate.DataBind();
                    dr.Close();
                    ////ddlstate.Items.Insert(0, "[Select]");
                    ////ddlstate.Items[0].Value = "0";
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

    protected void btnfindwithOption_Click(object sender, EventArgs e)
    {
        //if (ddlsearch.SelectedIndex == 0)
        //{
        //    findoption(txtfindwithoption.Text.Trim(), Convert.ToInt16(ddlsearch.SelectedValue));
        //}
        //else
        //{
        //    findoption(txtmobfind.Text.Trim(), Convert.ToInt16(ddlsearch.SelectedValue));
        //}
    }

    protected void ddlsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtmobfind.Text = "";
        //txtfindwithoption.Text = "";
        //if (ddlsearch.SelectedIndex == 1)
        //{
        //    txtmobfind.Visible = true;
        //    txtmobfind.Text = "";
        //    txtfindwithoption.Text = "";
        //    txtfindwithoption.Visible = false;
        //}
        //else
        //{
        //    txtmobfind.Visible = false;
        //    txtfindwithoption.Visible = true;
        //}
    }

    protected void lnkAppointmentTemplate_OnClick(object sender, EventArgs e)
    {
        if (ViewState["emp"] != null)
        {
            Response.Redirect("/mpages/providertimings.aspx?EmpNo=" + ViewState["emp"].ToString().Trim(), false);
        }
        else
        {
            Response.Redirect("/mpages/providertimings.aspx", false);
        }
    }

    protected void lnkProviderDetails_OnClick(object sender, EventArgs e)
    {
        if (ViewState["emp"] != null)
        {
            Response.Redirect("/MPages/ProviderDetails.aspx?EmpNo=" + ViewState["emp"].ToString().Trim(), false);
        }
        else
        {
            Response.Redirect("/MPages/ProviderDetails.aspx", false);
        }
    }

    protected void lnkClassification_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MPages/EmployeeClassification.aspx?EmpId=" + common.myInt(ViewState["emp"]), false);
    }

    protected void lnkProviderProfile_OnClick(object sender, EventArgs e)
    {
        if (ViewState["emp"] != null)
        {
            Response.Redirect("/MPages/DoctorProfileMaster.aspx?EmpNo=" + ViewState["emp"].ToString().Trim(), false);
        }
        else
        {
            Response.Redirect("/MPages/DoctorProfileMaster.aspx", false);
        }
    }

    protected void lnkEmployeeSequence_OnClick(object sender, EventArgs e)
    {

        Response.Redirect("~/mpages/EmployeeSequenceOrder.aspx", false);

    }
    protected void lnkEmployeeLookup_OnClick(object sebder, EventArgs e)
    {
        Response.Redirect("~/mpages/EmployeeLockUp.aspx", false);
    }

    protected Int32 CheckDoctor(Int32 EmpId)
    {
        if (EmpId > 0)
        {
            try
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshInput = new Hashtable();
                StringBuilder str = new StringBuilder();
                hshInput.Add("EmpId", EmpId);
                str.Append("select Ismedicalprovider from Employee where ID =@EmpId  ");
                DataSet ds = objDl.FillDataSet(CommandType.Text, str.ToString(), hshInput);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Ismedicalprovider"].ToString() == "True")
                    {
                        return 1;
                    }
                    else
                        return 0;
                }

            }
            catch (Exception)
            {
                return 0;
            }
        }
        return 0;
    }

    private Int32 CheckUserDoctor(Int32 iUserId)
    {
        try
        {
            if (iUserId > 0)
            {
                hshInput = new Hashtable();
                StringBuilder objStr = new StringBuilder();
                objStr.Append("SELECT e.Id FROM Employee e");
                objStr.Append(" INNER JOIN Users u on e.Id = u.EmpId");
                objStr.Append(" INNER JOIN EmployeeType et ON e.EmployeeType = et.Id");
                objStr.Append(" WHERE et.id in (1, 2) AND u.ID = @UserID");
                hshInput.Add("UserID", iUserId);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                int str = (int)objDl.ExecuteScalar(CommandType.Text, objStr.ToString(), hshInput);
                return str;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        return 0;
    }

    protected void Upload_OnClick(Object sender, EventArgs e)
    {
        try
        {
            string sFileName = "";
            string sFileExtension = "";
            string sSavePath = "/PatientDocuments/EmployeeImages/";
            StringBuilder objStr = new StringBuilder();
            if (FileUploader1.FileName != "" || FileUploader1.PostedFile != null)
            {
                DeleteFiles();
                HttpPostedFile myFile = FileUploader1.PostedFile;
                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                {
                    txtImageId.Text = "";
                    lblMessage.Text = "Error: The file size is zero.";
                    return;
                }
                // Read file into a data stream
                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                sFileExtension = System.IO.Path.GetExtension(myFile.FileName).ToLower();
                sFileName = txtfirstname.Text.ToString() + txtlastname.Text.ToString() + System.IO.Path.GetExtension(myFile.FileName).ToLower();
                System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFileName), System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();
                txtImageId.Text = sFileName;


                // FileUploader1.SaveAs(Server.MapPath("/PatientDocuments/EmployeeImages/") + sFileName.ToString());
                PatientImage.ImageUrl = "/PatientDocuments/EmployeeImages/" + sFileName.ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void RemoveImage_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if ((txtemployeeno.Text == "0") || (txtemployeeno.Text == ""))
            {
                DeleteFiles();
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
            }
            else
            {
                Hashtable hshTable = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshTable.Add("@EmployeeId", bc.ParseQ(txtemployeeno.Text.ToString().Trim()));
                String strSQL = "Update EmployeeImage set Active=0 where EmployeeId=@EmployeeId and Active=1";
                objDl.ExecuteNonQuery(CommandType.Text, strSQL, hshTable);
                //Alert.ShowAjaxMsg("Image Removed", Page);
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void DeleteFiles()
    {
        try
        {
            string strImageid = "";
            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/EmployeeImages"));
            if (objDir.Exists == true)
            {
                FileInfo[] fi_array = objDir.GetFiles();
                foreach (FileInfo files in fi_array)
                {
                    if (files.ToString() == sFileName.ToString())
                    {
                        if (files.Exists)
                        {
                            strImageid = Path.GetFileNameWithoutExtension(Server.MapPath("/PatientDocuments/PatientImages/") + files);
                            if ((Convert.ToString(txtemployeeno.Text) == strImageid) || (Convert.ToString(txtfirstname.Text + txtlastname.Text) == strImageid))
                            {
                                File.Delete(Server.MapPath("/PatientDocuments/EmployeeImages/") + files);
                            }
                        }
                    }
                }
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
            }
            else
            {
                objDir.Create();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void SaveImage(string strEmployeeId)
    {
        try
        {
            StringBuilder strSQL = new StringBuilder();
            byte[] byteImageData;
            //String FileName = "";
            //string sFileExtension = "";
            string sNewFileName = "";
            if (txtImageId.Text != "")
            {
                //DirectoryInfo dir = new DirectoryInfo(Server.MapPath("/PatientDocuments/EmployeeImages/"));
                //FileInfo[] fi_array = dir.GetFiles();
                //foreach (FileInfo file in fi_array)
                //{

                //   FileName = file.ToString();
                // sFileExtension = System.IO.Path.GetExtension(FileName).ToLower();
                //    if (FileName.ToString() == txtImageId.Text.ToString())
                //    {
                //        File.Delete(Server.MapPath("/PatientDocuments/EmployeeImages/") + strEmployeeId + sFileExtension.ToLower());
                //        File.Move(Server.MapPath("/PatientDocuments/EmployeeImages/" + txtImageId.Text), Server.MapPath("/PatientDocuments/EmployeeImages/" + strEmployeeId + sFileExtension.ToLower()));
                //sNewFileName = strEmployeeId + ".jpeg";
                sNewFileName = txtfirstname.Text.ToString() + txtlastname.Text.ToString() + ".jpg";
                //        break;
                //    }

                //}
                String FilePath = Server.MapPath("/PatientDocuments/EmployeeImages/") + sNewFileName.ToString();
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] image = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                byteImageData = image;
                SqlConnection con = new SqlConnection(sConString);
                SqlCommand cmdTemp;
                SqlParameter prm1, prm2, prm3, prm4, prm5;
                strSQL.Append("Exec UspSaveEmployeeImage @intEmployeeId, @Image, @chvImageType,@inyHospitalLocationId,@EncodedBy");
                cmdTemp = new SqlCommand(strSQL.ToString(), con);
                cmdTemp.CommandType = CommandType.Text;

                prm1 = new SqlParameter();
                prm1.ParameterName = "@intEmployeeId";
                prm1.Value = bc.ParseQ(strEmployeeId);
                prm1.SqlDbType = SqlDbType.Int;
                prm1.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm1);

                prm2 = new SqlParameter();
                prm2.ParameterName = "@Image";
                prm2.Value = byteImageData;
                prm2.SqlDbType = SqlDbType.Image;
                prm2.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm2);

                prm3 = new SqlParameter();
                prm3.ParameterName = "@chvImageType";
                prm3.Value = sNewFileName;
                prm3.SqlDbType = SqlDbType.VarChar;
                prm3.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm3);

                prm4 = new SqlParameter();
                prm4.ParameterName = "@inyHospitalLocationId";
                prm4.Value = common.myStr(Session["HospitalLocationId"]);
                prm4.SqlDbType = SqlDbType.Int;
                prm4.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm4);

                prm5 = new SqlParameter();
                prm5.ParameterName = "@EncodedBy";
                prm5.Value = common.myInt(Session["UserId"]);
                prm5.SqlDbType = SqlDbType.Int;
                prm5.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm5);

                con.Open();
                cmdTemp.ExecuteNonQuery();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void ShowImage()
    {
        try
        {
            StringBuilder strSQL = new StringBuilder();
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmdTemp;
            SqlParameter prm2;
            strSQL.Append("select EmployeeImage, ImageType from EmployeeImage where EmployeeId = @intEmployeeId and Active=1");
            cmdTemp = new SqlCommand(strSQL.ToString(), con);
            cmdTemp.CommandType = CommandType.Text;

            prm2 = new SqlParameter();
            prm2.ParameterName = "@intEmployeeId";
            prm2.Value = bc.ParseQ(common.myInt(txtemployeeno.Text).ToString());
            //prm2.Value = "100129";
            prm2.SqlDbType = SqlDbType.Int;
            prm2.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm2);


            con.Open();
            SqlDataReader dr = cmdTemp.ExecuteReader();
            if (dr.HasRows == true)
            {
                dr.Read();
                Stream strm;
                Object img = dr["EmployeeImage"];
                String FileName = dr["ImageType"].ToString();
                strm = new MemoryStream((byte[])img);
                byte[] buffer = new byte[strm.Length];
                int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/EmployeeImages/" + FileName), FileMode.Create, FileAccess.Write);
                fs.Write(buffer, 0, byteSeq);
                fs.Dispose();
                PatientImage.ImageUrl = "~/PatientDocuments/EmployeeImages/" + FileName;
            }
            else
            {
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void chkDoNotExpire_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkDoNotExpire.Checked == true)
        {
            txtExpiryPeriod.Text = "";
            txtExpirynotification.Text = "";
            txtExpiryPeriod.Enabled = false;
            txtExpirynotification.Enabled = false;
        }
        else
        {
            txtExpiryPeriod.Text = "15";
            txtExpirynotification.Text = "3";
            txtExpiryPeriod.Enabled = true;
            txtExpirynotification.Enabled = true;
        }
    }

    protected void btnZipSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@chvZipCode", txtPin.Text);
            DataSet ds = dal.FillDataSet(CommandType.StoredProcedure, "USPEMRGetZipCode", hshInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(Convert.ToString(ds.Tables[1].Rows[0]["CountryId"])));
                ddlcountry_SelectedIndexChanged(this, null);

                ddlstate.SelectedIndex = ddlstate.Items.IndexOf(ddlstate.Items.FindByValue(Convert.ToString(ds.Tables[1].Rows[0]["StateId"]).Trim()));
                ddlstate_SelectedIndexChanged(this, null);

                ddlcity.SelectedIndex = ddlcity.Items.IndexOf(ddlcity.Items.FindByValue(Convert.ToString(ds.Tables[1].Rows[0]["CityId"]).Trim()));
            }
            else
            {
                ddlcountry.SelectedIndex = -1;
                ddlstate.SelectedIndex = -1;
                ddlcity.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void chkIsMedicalProvider_OnCheckedChanged(object sender, EventArgs e)
    {
        chkIsNoAppointmentAllowBeyondTime.Visible = false;
        chkMultipleAppointment.Visible = false;
        if (chkIsMedicalProvider.Checked == true)
        {
            chkIsNoAppointmentAllowBeyondTime.Visible = true;
            chkMultipleAppointment.Visible = true;

        }
    }

    protected void ddlemployeetype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable ds = (DataTable)ViewState["ds"];
            DataRow[] dr = ds.Select("ID=" + ddlemployeetype.SelectedValue);
            if (dr.Length > 0)
            {
                string strType = common.myStr(dr[0]["EmployeeType"]).Trim();
                ddlStation.Enabled = false;
                chkEmail.Visible = true;
                chkSendOrders.Visible = false;
                chkPrescribeMedication.Visible = false;
                chkEncounterReOpen.Visible = false;
                chkDecisionSupport.Visible = false;
                chkDose.Visible = false;
                if ((strType == "LDIR") || (strType == "LIC") || (strType == "LS") || (strType == "LT") || (strType == "LD") || (strType == "N"))
                {
                    ddlStation.Enabled = true;
                    chkEmail.Visible = true;
                }
                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isAllowLabStationToDoctor", sConString).Equals("Y") && (strType == "D"))
                {
                    ddlStation.Enabled = true;
                }
                if ((strType == "D") || (strType == "N") || (strType == "LD") || (strType == "LDIR") || (strType == "DT") || (strType == "PT"))
                {
                    chkSendOrders.Visible = true;
                    chkPrescribeMedication.Visible = true;
                    chkEncounterReOpen.Visible = true;
                    chkDecisionSupport.Visible = true;
                    chkDose.Visible = true;

                }
                if (common.myStr(strType) == "D" || common.myStr(strType) == "TM" || common.myStr(strType) == "LD" || common.myStr(strType) == "LDIR")
                {
                    ddlFirstVisit.Enabled = true;
                    ddlFollowupVisit.Enabled = true;
                    ddlFreeVisit.Enabled = true;
                    ddlIpFirstvisit.Enabled = true;
                    ddlIPSecondVisit.Enabled = true;
                    DropFEmergencyVisit.Enabled = true;
                    DropSEmergencyVisit.Enabled = true;
                    if (chkIsMultipleFirstVisit.Checked)
                    {
                        ddlOtherFirstVisit.Enabled = true;
                    }
                    updVisit.Update();
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

    //public void UpdateEmployeeData() // Dont delete or uncomment this function 
    //{
    //    BaseC.EncryptDecrypt eN = new BaseC.EncryptDecrypt();
    //    BaseC.ParseData objParse = new BaseC.ParseData();
    //    DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    DataSet ds = new DataSet();
    //    string sUserID = "", sPrivateKey = "";

    //    Hashtable hsinput = new Hashtable();
    //    hsinput.Add("@inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationId"]));

    //    ds = dL.FillDataSet(CommandType.Text, "SELECT u.ID, '' PrivateKey, ud.EmployeeNo , ud.Password FROM Employee e inner JOIN  __UserDataforUpdate ud  ON ud.Employeeid = e.ID inner join Users u on ud.UserId = u.ID WHERE e.Active = 1 and ud.EmployeeNo is not null and ud.Password is not null and ud.UserId <> 1", hsinput);
    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    {
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            sPrivateKey = common.myStr(ds.Tables[0].Rows[i]["PrivateKey"]).Trim();
    //            sUserID = common.myStr(ds.Tables[0].Rows[i]["Id"]);
    //            if (sPrivateKey == "")
    //            {
    //                sPrivateKey = System.Guid.NewGuid().ToString();
    //            }
    //            string Password = eN.Encrypt(objParse.ParseQ(common.myStr(ds.Tables[0].Rows[i]["Password"])), eN.getKey(sConString), true, sPrivateKey);
    //            string UserName = eN.Encrypt(objParse.ParseQ(common.myStr(ds.Tables[0].Rows[i]["EmployeeNo"])), eN.getKey(sConString), true, "");

    //            hsinput = new Hashtable();
    //            hsinput.Add("@UserId", sUserID);
    //            hsinput.Add("@PrivateKey", sPrivateKey);

    //            string UserPrivateKey = "IF EXISTS(SELECT ID FROM UserPrivateKey WHERE UserId = @UserId) BEGIN UPDATE UserPrivateKey SET PrivateKey = @PrivateKey WHERE UserId = @UserId  END ELSE  BEGIN   INSERT INTO UserPrivateKey(UserId, PrivateKey) VALUES(@UserId, @PrivateKey) END  ";
    //            DataSet ds1 = dL.FillDataSet(CommandType.Text, UserPrivateKey, hsinput);

    //            hsinput = new Hashtable();
    //            hsinput.Add("@UserName", UserName);
    //            hsinput.Add("@Password", Password);
    //            hsinput.Add("@UserId", sUserID);
    //            hsinput.Add("@inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));

    //            string users = "UPDATE Users SET UserName = @UserName, Password = @Password,HintQuestion = NULL, HintAnswer = '', isLocked = 0,DefaultPageId = NULL, NextExpiryDate = '2016-11-14', PasswordExpiryPeriod = 0,PasswordExpiryNotification = 0,NeverExpirePwd=1 WHERE Id = @UserId And HospitalLocationId = @inyHospitalLocationId  ";
    //            DataSet ds2 = dL.FillDataSet(CommandType.Text, users, hsinput);
    //        }
    //    }
    //}

    protected void chkIsMultipleFirstVisit_OnCheckedChanged(object sender, EventArgs e)
    {
        ddlOtherFirstVisit.Enabled = false;
        if (chkIsMultipleFirstVisit.Checked == true)
        { ddlOtherFirstVisit.Enabled = true; }
        updVisit.Update();
    }

    protected void lnkUserDepartmentTagging_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["emp"]) != "")
        {
            RadWindow1.NavigateUrl = "/MPages/UserDepartmentTagging.aspx?EmpNo=" + ViewState["emp"].ToString().Trim() + "&EmpName=" + ViewState["EmpName"];
            RadWindow1.Height = 550;
            RadWindow1.Width = 950;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        else
        {
            RadWindow1.NavigateUrl = "/MPages/UserDepartmentTagging.aspx";
            RadWindow1.Height = 550;
            RadWindow1.Width = 950;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            //ClientScript.RegisterStartupScript(GetType(), "open", "javascript:openRadWindow('/MPages/UserDepartmentTagging.aspx?EmpNo = " + ViewState["emp"].ToString().Trim() +"EmpName="+ ViewState["EmpName"] + "',900,500)");
        }

    }
}