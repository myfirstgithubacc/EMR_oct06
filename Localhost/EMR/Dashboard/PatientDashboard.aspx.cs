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
using System.Web.Script.Serialization;
using System.Collections.Generic;
using BaseC;
using System.IO;
using System.Diagnostics;

public partial class EMR_Dashboard_Default : System.Web.UI.Page
{
    //WebPartManager _manager;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Session["FromPage"] = common.myStr(Request.QueryString["From"]);
            ComboPatientSearch.Enabled = false;
            btnprint.Visible = false;
        }
        if (!IsPostBack)
        {

            BindPatientHiddenDetails(common.myInt(Session["RegistrationNo"]));
            BaseC.Security AuditCA = new BaseC.Security(sConString);
            Session["SelectFindPatient"] = null;
            BindReports();
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, Session["CurrentNode"].ToString().Length - 1);
            }
            else
            {
                ViewState["PageId"] = "6";
            }


            AuditCA.AuditCommonAccess(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]),
                common.myInt(Session["RegistrationId"]), common.myInt(Session["encounterid"]),
                common.myInt(ViewState["PageId"]), 0, common.myInt(Session["UserID"]), 0,
                "ACCESSED", common.myStr(Session["IPAddress"]));

            if (Convert.ToString(Session["encounterid"]) == "")
            {
                Response.Redirect("/default.aspx?RegNo=0", false);
            }
            else
            {
                tdDateRange.Visible = false;
                tdfalse.Visible = false;

                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;

                BindControl();

                //SETVALUEView3();
                SETVALUEView2();
                //SETVALUEView1();

                if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
                {
                    lblmsg.Text = "";
                }
                else
                {
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                    lblmsg.Text = "No Active Encounter Exist for this Appointment";
                }
            }
        }
    }
    private void BindControl()
    {
        RadDockLayout1.Visible = false;
        RadDockLayout2.Visible = true;
        RadDockLayout3.Visible = false;

    }
    private void BindReports()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dsReport = new DataSet();
            Hashtable hshTable = new Hashtable();
            hshTable.Add("@inyHospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
            string strReport = "SELECT ReportID, HospitalLocationID, ReportName, ReportLocation, ReportType " +
             "FROM  Reports WHERE HospitalLocationID=@inyHospitalLocationID AND ReportType='Report'";
            dsReport = objDl.FillDataSet(CommandType.Text, strReport, hshTable);
            if (dsReport.Tables[0].Rows.Count > 0)
            {
                cboReport.DataSource = dsReport;
                cboReport.DataTextField = "ReportName";
                cboReport.DataValueField = "ReportID";
                cboReport.DataBind();
                cboReport.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
                cboReport.Items[0].Value = "0";
                //cboReport.Items.Insert(0, "Select");
                //cboReport.Items.Add(
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
        ComboPatientSearch.Text = "";

        if (ddlTime.SelectedValue == "4")
        {
            tdDateRange.Visible = true;
            tdfalse.Visible = true;
        }
        else
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
        }
        BindControl();
    }

    // Start of View 3 Value

    void setvaluesAppointment3()
    {
        TextBox hdD = new TextBox();
        hdD = Appointments.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Appointments.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Appointments.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Appointments.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
    }

    void setvaluesVitals3()
    {
        TextBox hdD = new TextBox();


        hdD = Vital.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Vital.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Vital.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Vital.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Vital.BindGDVitals();
    }

    void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
        if (RegistrationNo > 0)
        {
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;
            int EncounterId = 0;
            int EncodedBy = common.myInt(Session["UserId"]);
            DataSet ds = new DataSet();
            ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);
            if (ds.Tables.Count > 0)
            {
                DataView dvIP = new DataView(ds.Tables[0]);
                //  dvIP.RowFilter = "OPIP = 'O'";
                DataTable dt = new DataTable();
                dt = dvIP.ToTable();
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    lblPatientName1.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblMobile.Text = common.myStr(dr["MobileNo"]);
                }

            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblmsg.Text = "Patient not found !";
                return;
            }
        }
    }
    //void setvaluesTask3()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = Tasks.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = Tasks.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = Tasks.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = Tasks.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    //void setvaluesNotes3()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = Notes.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = Notes.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = Notes.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = Notes.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    void setvaluesProblems3()
    {
        TextBox hdD = new TextBox();
        hdD = Problems.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Problems.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Problems.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Problems.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Problems.BindProblems();
    }

    void setvaluesCurrentMedicine3()
    {
        TextBox hdD = new TextBox();
        hdD = CurrentMedications1.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = CurrentMedications1.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = CurrentMedications1.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = CurrentMedications1.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        CurrentMedications1.BindMedications();
    }

    void setvaluesDiagnosis3()
    {
        TextBox hdD = new TextBox();
        hdD = Diagnosis.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Diagnosis.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Diagnosis.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Diagnosis.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Diagnosis.BindGDDiagnosis();
    }

    void setvaluesOrders3()
    {
        TextBox hdD = new TextBox();
        hdD = Orders.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Orders.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Orders.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Orders.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Orders.BindGDOrders();
    }

    void setvaluesLabResult3()
    {
        TextBox hdD = new TextBox();
        hdD = LabResults3.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = LabResults3.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = LabResults3.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = LabResults3.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }

        LabResults3.bindTestData();
    }

    // End of View 3 Value

    // Start of view 2 Value

    void setvaluesAppointment2()
    {
        TextBox hdD = new TextBox();
        hdD = Appointments1.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Appointments1.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Appointments1.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Appointments1.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }

    }

    void setvaluesVitals2()
    {
        TextBox hdD = new TextBox();
        hdD = Vital1.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Vital1.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Vital1.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Vital1.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Vital2.BindGDVitals();
    }



    //void setvaluesTask2()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = Tasks1.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = Tasks1.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = Tasks1.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = Tasks1.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    //void setvaluesNotes2()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = Notes1.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = Notes1.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = Notes1.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = Notes1.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    void setvaluesProblems2()
    {
        TextBox hdD = new TextBox();
        hdD = Problems1.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Problems1.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Problems1.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Problems1.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Problems1.BindProblems();
    }

    void setvaluesCurrentMedicine2()
    {
        TextBox hdD = new TextBox();
        hdD = CurrentMedications2.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = CurrentMedications2.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = CurrentMedications2.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = CurrentMedications2.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        CurrentMedications2.BindMedications();
    }

    void setvaluesDiagnosis2()
    {
        TextBox hdD = new TextBox();
        hdD = Diagnosis.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Diagnosis.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Diagnosis.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Diagnosis.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Diagnosis.BindGDDiagnosis();
    }

    void setvaluesOrders2()
    {
        TextBox hdD = new TextBox();
        hdD = Orders1.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Orders1.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Orders1.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Orders1.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Orders1.BindGDOrders();
    }

    void setvaluesLabResult2()
    {
        TextBox hdD = new TextBox();
        hdD = LabResults2.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = LabResults2.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = LabResults2.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = LabResults2.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }

        LabResults2.bindTestData();
    }

    // End of View 2 Value


    // Start of view 1 Value

    void setvaluesAppointment1()
    {
        TextBox hdD = new TextBox();
        hdD = Appointments2.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Appointments2.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Appointments2.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Appointments2.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }

    }

    void setvaluesVitals1()
    {
        TextBox hdD = new TextBox();
        hdD = Vital2.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Vital2.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Vital2.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Vital2.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Vital2.BindGDVitals();
    }

    //void setvaluesVitalsHistory1()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = VitalHistory2.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = VitalHistory2.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = VitalHistory2.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = VitalHistory2.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    //void setvaluesChiefComplaintsHistory1()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = ChiefComplaintsHistory2.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = ChiefComplaintsHistory2.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = ChiefComplaintsHistory2.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = ChiefComplaintsHistory2.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    //void setvaluesTask1()
    //{
    //    TextBox hdD = new TextBox();
    //    hdD = Tasks2.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = Tasks2.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = Tasks2.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = Tasks2.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    //void setvaluesNotes1()
    //{

    //    TextBox hdD = new TextBox();
    //    hdD = Notes2.FindControl("hdnDateVale") as TextBox;
    //    hdD.Text = ddlTime.SelectedValue;
    //    if (ddlTime.SelectedValue == "4")
    //    {
    //        string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
    //        string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

    //        TextBox hdT = new TextBox();
    //        hdT = Notes2.FindControl("hdnToDate") as TextBox;
    //        hdT.Text = sToDate;

    //        TextBox hdF = new TextBox();
    //        hdF = Notes2.FindControl("hdnFromDate") as TextBox;
    //        hdF.Text = sFromDate;
    //    }

    //    TextBox hdA = new TextBox();
    //    hdA = Notes2.FindControl("hdnEncounterNumber") as TextBox;

    //    if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
    //    {
    //        lblEncouterDetails.Text = "";
    //        hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
    //    }
    //    else
    //    {
    //        string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[0];
    //        hdA.Text = strEncounterNo;
    //    }
    //}

    void setvaluesProblems1()
    {
        TextBox hdD = new TextBox();
        hdD = Problems2.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Problems2.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Problems2.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Problems2.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Problems2.BindProblems();
    }

    void setvaluesCurrentMedicine1()
    {
        TextBox hdD = new TextBox();
        hdD = CurrentMedications3.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = CurrentMedications3.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = CurrentMedications3.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = CurrentMedications3.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        CurrentMedications3.BindMedications();
    }

    void setvaluesDiagnosis1()
    {
        TextBox hdD = new TextBox();
        hdD = Diagnosis.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Diagnosis.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Diagnosis.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Diagnosis.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Diagnosis.BindGDDiagnosis();
    }

    void setvaluesOrders1()
    {
        TextBox hdD = new TextBox();
        hdD = Orders2.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = Orders2.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = Orders2.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = Orders2.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }
        Orders2.BindGDOrders();
    }

    void setvaluesLabResult1()
    {
        TextBox hdD = new TextBox();
        hdD = LabResults1.FindControl("hdnDateVale") as TextBox;
        hdD.Text = ddlTime.SelectedValue;
        if (ddlTime.SelectedValue == "4")
        {
            string sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
            string sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";

            TextBox hdT = new TextBox();
            hdT = LabResults1.FindControl("hdnToDate") as TextBox;
            hdT.Text = sToDate;

            TextBox hdF = new TextBox();
            hdF = LabResults1.FindControl("hdnFromDate") as TextBox;
            hdF.Text = sFromDate;
        }

        TextBox hdA = new TextBox();
        hdA = LabResults1.FindControl("hdnEncounterNumber") as TextBox;

        if (ViewState["SelectedEncounterId"] == null || ViewState["SelectedEncounterId"].ToString() == "")
        {
            lblEncouterDetails.Text = "";
            hdA.Text = common.myStr(common.myInt(Session["EncounterId"]));
        }
        else
        {
            string strEncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[0];
            hdA.Text = strEncounterNo;
        }

        LabResults1.bindTestData();
    }

    // End of View 1 Value

    private string[] getToFromDate()
    {
        string sFromDate = "", sToDate = "";
        string[] str = new string[2];

        if (ddlTime.SelectedValue == "7")
        {
            tdDateRange.Visible = true;
            tdfalse.Visible = true;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 00:00";
                sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy-MM-dd") + " 23:59";
            }
            else
            {
                sFromDate = "";
                sToDate = "";
            }
        }
        else if (ddlTime.SelectedValue == "1")
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
            sFromDate = DateTime.Today.ToString("yyyy-MM-dd") + " 00:00";
            sToDate = DateTime.Today.ToString("yyyy-MM-dd") + " 23:59";
        }
        else if (ddlTime.SelectedValue == "2")
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
        }
        else if (ddlTime.SelectedValue == "3")
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
            sFromDate = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00";
            sToDate = DateTime.Today.ToString("yyyy-MM-dd") + " 23:59";
        }
        else if (ddlTime.SelectedValue == "4")
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
            sFromDate = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00";
            sToDate = DateTime.Today.ToString("yyyy-MM-dd") + " 23:59";
        }
        else if (ddlTime.SelectedValue == "5")
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
            sFromDate = DateTime.Today.AddMonths(-6).ToString("yyyy-MM-dd") + " 00:00";
            sToDate = DateTime.Today.ToString("yyyy-MM-dd") + " 23:59";
        }
        else if (ddlTime.SelectedValue == "6")
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
            sFromDate = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd") + " 00:00";
            sToDate = DateTime.Today.ToString("yyyy-MM-dd") + " 23:59";
        }
        else
        {
            tdDateRange.Visible = false;
            tdfalse.Visible = false;
            sFromDate = "1900-01-01 00:00";
            sToDate = "2079-01-01 00:00";
        }
        str[0] = sFromDate;
        str[1] = sToDate;

        return str;
    }

    private void bindData()
    {
        try
        {
            if (Session["RegistrationId"] != null)
            {
                string[] str = getToFromDate();
                string sFromDate = str[0];
                string sToDate = str[1];

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hstInput = new Hashtable();

                hstInput.Add("inyHospitalLocationId", Session["HospitalLocationID"]);
                hstInput.Add("intRegistrationId", Session["RegistrationId"]);
                hstInput.Add("chvFromDate", sFromDate);
                hstInput.Add("chvToDate", sToDate);

                DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetEncounterDetail", hstInput);

                DataTable dataTable = objDs.Tables[0];

                this.ComboPatientSearch.Items.Clear();

                RadComboBoxItem item1 = new RadComboBoxItem();

                item1.Text = "All";
                item1.Value = "0";
                item1.Attributes.Add("EncounterDate", "");
                item1.Attributes.Add("DcotorName", "");
                item1.Attributes.Add("FacilityName", "");
                item1.Selected = true;

                this.ComboPatientSearch.Items.Add(item1);

                item1.DataBind();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = common.myStr(dataRow["EncounterDate"]).Trim();

                    item.Value = common.myInt(dataRow["EncounterId"]).ToString();

                    string EncounterNo = common.myStr(dataRow["EncounterNo"]).Trim();
                    string strDoctorName = common.myStr(dataRow["DoctorName"]).Trim();
                    string strFacilityName = common.myStr(dataRow["FacilityName"]).Trim();

                    item.Attributes.Add("EncounterNo", EncounterNo);
                    item.Attributes.Add("DcotorName", strDoctorName);
                    item.Attributes.Add("FacilityName", strFacilityName);

                    item.Value += ":" + EncounterNo + ":" + strDoctorName + ":" + strFacilityName;

                    this.ComboPatientSearch.Items.Add(item);
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

    protected void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        bindData();
    }

    protected void RadComboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SelectedEncounterId"] = ComboPatientSearch.SelectedValue;
        if (String.IsNullOrEmpty(ComboPatientSearch.SelectedValue))
        {
            lblEncouterDetails.Text = "";
        }
        else
        {
            string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[1];
            string strDoctorName = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[2];
            string strFacilityName = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[3];

            lblEncouterDetails.Text = "<b>Encounter No:</b> " + strEncounterNo + " <b>Provider Name:</b> " + strDoctorName + " <b>Facility:</b> " + strFacilityName;

            //SETVALUEView1();
            SETVALUEView2();
            //SETVALUEView3();
        }
    }

    void SETVALUEView1()
    {
        setvaluesAppointment1();
        setvaluesVitals1();
        // setvaluesTask1();
        //setvaluesNotes1();
        setvaluesProblems1();
        setvaluesCurrentMedicine1();
        setvaluesDiagnosis1();
        setvaluesOrders1();
        setvaluesLabResult1();

        //setvaluesVitalsHistory1();
        //setvaluesChiefComplaintsHistory1();
    }

    void SETVALUEView2()
    {
        setvaluesAppointment2();
        setvaluesVitals2();
        //setvaluesTask2();
        //setvaluesNotes2();
        setvaluesProblems2();
        setvaluesCurrentMedicine2();
        setvaluesDiagnosis2();
        setvaluesOrders2();
        setvaluesLabResult2();

        //setvaluesVitalsHistory2();
        //setvaluesChiefComplaintsHistory2();
    }

    void SETVALUEView3()
    {
        setvaluesAppointment3();
        setvaluesVitals3();
        //setvaluesTask3();
        //setvaluesNotes3();
        setvaluesProblems3();
        setvaluesCurrentMedicine3();
        setvaluesDiagnosis3();
        setvaluesOrders3();
        setvaluesLabResult3();

        //setvaluesVitalsHistory3();
        //setvaluesChiefComplaintsHistory3();
    }

    protected void btnSearchDateRange_Click(object sender, EventArgs e)
    {
        ViewState["SelectedEncounterId"] = ComboPatientSearch.SelectedValue;

        //string strEncounterNo = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[1];
        //string strDoctorName = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[2];
        //string strFacilityName = ComboPatientSearch.SelectedValue.Split(new char[] { ':' })[3];
        BindControl();

        if (ViewState["SelectedEncounterId"] != null && ViewState["SelectedEncounterId"].ToString() != "")
        {
            string[] str = getToFromDate();
            string sFromDate = str[0];
            string sToDate = str[1];
            Dashboard dsh = new Dashboard();
            string EncounterNo = ViewState["SelectedEncounterId"].ToString().Split(new char[] { ':' })[1];

            //SETVALUEView3();
            SETVALUEView2();
            //SETVALUEView1();


            //Server.Transfer("PatientDashboard.aspx");
            //Response.Redirect("PatientDashboard.aspx");

            //ComboPatientSearch.SelectedValue = ViewState["SelectedEncounterId"].ToString();
            if (ddlTime.SelectedValue == "4")
            {
                tdDateRange.Visible = true;
                tdfalse.Visible = true;
            }
            else
            {
                tdDateRange.Visible = false;
                tdfalse.Visible = false;
            }
        }
    }

    protected void btnSaveLayout_OnClick(object sender, EventArgs e)
    {
        //List<DockState> dockstates = RadDockLayout1.GetRegisteredDocksState();
        //Dictionary<string, string> myPositions = new Dictionary<string, string>();
        //Dictionary<string, int> myIndices = new Dictionary<string, int>(); 
        //Telerik.Web.UI.DockLayoutEventArgs myEvent = new DockLayoutEventArgs(myPositions, myIndices);

        //SaveDashboardLayout(sender, myEvent, dockstates);
        //Response.Redirect("Default.aspx?Mpg=P6");
    }

    protected void SaveDashboardLayout(object sender, Telerik.Web.UI.DockLayoutEventArgs e, List<DockState> dockStates)
    {

        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //string serializedPositions = serializer.Serialize(e.Positions);
        //string serializedIndices = serializer.Serialize(e.Indices);
        //string serializedDockStates = serializer.Serialize(dockStates);
        //HttpCookie positionsCookie = new HttpCookie("PatientDashboard_"+Session["UserId"], serializer.Serialize(new string[] { serializedPositions, serializedIndices, serializedDockStates }));

        ////Ensure that the cookie will not expire soon
        //positionsCookie.Expires = DateTime.Now.AddYears(1);
        //Response.Cookies.Add(positionsCookie);
        // //eve.
        // //RadDockLayout1_SaveDockLayout(this, Telerik.Web.UI.DockLayoutEventArgs);
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        dtpFromDate.SelectedDate = null;
        dtpToDate.SelectedDate = null;
        ddlTime.SelectedIndex = 0;
        ComboPatientSearch.Text = "";
    }

    protected void RadDockLayout1_SaveDockLayout(object sender, Telerik.Web.UI.DockLayoutEventArgs e)
    {
        // Store the positions in a cookie. Note, that if there are lots of dock objects on the page
        // the cookie length might become insufficient. In this case it would be better to use the
        // cookie to store a key from a database, where the positions will be actually stored.
        //
        // You can store the positions directly in a database and use the ID of the currently logged
        // user as a key to his personalized positions.
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //string serializedPositions = serializer.Serialize(e.Positions);
        //string serializedIndices = serializer.Serialize(e.Indices);

        //HttpCookie positionsCookie = new HttpCookie("PatientDashboard_" + Session["UserId"], serializer.Serialize(new string[] { serializedPositions, serializedIndices }));

        ////Ensure that the cookie will not expire soon
        //positionsCookie.Expires = DateTime.Now.AddYears(1);
        //Response.Cookies.Add(positionsCookie);
    }

    protected void RadDockLayout1_LoadDockLayout(object sender, Telerik.Web.UI.DockLayoutEventArgs e)
    {
        //HttpCookie positionsCookie = Request.Cookies["PatientDashboard_" + Session["UserId"]];
        //if (!Object.Equals(positionsCookie, null))
        //{
        //    string serializedPositionsAndIndices = positionsCookie.Value;
        //    if (!string.IsNullOrEmpty(serializedPositionsAndIndices))
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        string[] positionsAndIndices = serializer.Deserialize<string[]>(serializedPositionsAndIndices);

        //        e.Positions = serializer.Deserialize<Dictionary<string, string>>(positionsAndIndices[0]);
        //        e.Indices = serializer.Deserialize<Dictionary<string, int>>(positionsAndIndices[1]);

        //    }
        //}
    }

    private void GetHealthRecordsForCoreMeasure()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            Hashtable hstable = new Hashtable();
            hstable.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationHealthRecords", hstable);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
                {
                    #region Add a new screen that logs electronic health record requests and counter
                    Hashtable logHash = new Hashtable();
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    logHash.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                    logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                    logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
                    logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
                    logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogHealthRecordRequests", logHash);
                    #endregion
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



    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["RegistrationId"] != null)
            {
                if (cboReport.SelectedIndex != 0)
                {
                    GetHealthRecordsForCoreMeasure();
                    RadWindowForNew.NavigateUrl = "~/EMRReports/PatientSummary.aspx?RegistrationId=" + Session["RegistrationId"].ToString() + "&Summary=" + true + "";

                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 990;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    // RadWindowForNew.Title = "Time Slot";
                    //RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please select report type!", Page);
                    return;

                }
                if (Session["MUDMeasure"] != null && Convert.ToBoolean(Session["MUDMeasure"]) == true)
                {
                    #region Log is CPT entered is E&M code and clinical summary is printed or patient portal is active
                    Hashtable logHash = new Hashtable();
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    logHash.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                    logHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                    logHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
                    logHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
                    logHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                    logHash.Add("@intFacilityID", Convert.ToInt32(Session["FacilityID"]));

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogCPTInE&MAndClinicalSummary", logHash);
                #endregion

                #region Log for each encounter when a referring physician is selected in an order and a summary is printed or patient portal is active
                if (Session["SelRefPhy"] != null)
                {
                    Hashtable logOrderHash = new Hashtable();
                    logOrderHash.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                    logOrderHash.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                    logOrderHash.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
                    logOrderHash.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
                    logOrderHash.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                    //logOrderHash.Add("@intFacilityID", Convert.ToInt32(Session["FacilityID"]));
                    logOrderHash.Add("@bitIsSelectRefPhysicianInOrder", Convert.ToBoolean(1));
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogOrderRefPhyAndSummary", logOrderHash);
                }
                #endregion

                    #region Log encounters if patient has access to portal
                    Hashtable logHashTble = new Hashtable();

                    logHashTble.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
                    logHashTble.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                    logHashTble.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
                    logHashTble.Add("@intDoctorId", Convert.ToInt32(Session["DoctorID"]));
                    logHashTble.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogAccessPatientPortal", logHashTble);
                    #endregion
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please Select Patient !", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnAttachment_Click(object sender, EventArgs e)
    {
        if (Session["RegistrationId"] != null)
        {
            RadWindowForNew.NavigateUrl = "~/Editor/ImageEditor.aspx";

            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 990;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            // RadWindowForNew.Title = "Time Slot";
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
    }
  
}
