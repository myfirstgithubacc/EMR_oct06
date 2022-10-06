using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;

public partial class ICM_DischargeSummary : System.Web.UI.Page
{
    DataSet ds;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ICM ObjIcm;

    int it = 0;
    string sMutipleRecordsDate = string.Empty;
    StringBuilder sb = new StringBuilder();
    DL_Funs ff = new DL_Funs();
    private int iPrevId = 0;
    StringBuilder sbSign = new StringBuilder();
    bool bTRecords = false;
    bool bTMRecords = false;
    //DAL.DAL dl;
    string FlagIsRequiredDischargeSummaryDate = string.Empty;
    bool flagOTHeadingName = true;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["HospitalLocationID"] = common.myInt(Session["HospitalLocationID"]);
        Session["FacilityId"] = common.myInt(Session["FacilityId"]);
        Session["UserId"] = common.myInt(Session["UserId"]);

        if (!IsPostBack)
        {
            // Spell Check
            RTF1.Languages.Add(new SpellCheckerLanguage("en-US", "English"));

            Session["FacilityName"] = Session["FacilityName"];

            hdnFontName.Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "DischargeSummaryFont", sConString);
            if (common.myStr(hdnFontName.Value).Equals(string.Empty))
            {
                hdnFontName.Value = "Candara";
            }

            Session["IsDefinalizedSuccess"] = "0";
            RadTimeFrom.TimeView.Interval = new TimeSpan(0, 15, 0);
            //RadTimeFrom.TimeView.StartTime = new TimeSpan(9, 0, 0);
            //RadTimeFrom.TimeView.EndTime = new TimeSpan(23, 0, 0);
            dtpdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            RadComboBox1.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                    RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                else
                    RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }

            if (!common.myStr(Request.QueryString["For"]).ToUpper().Equals("DTHSUM"))
            {
                string IsTimeEditInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsTimeEditInDischargeSummary", sConString);

                if (IsTimeEditInDischargeSummary.ToUpper().Equals("Y"))
                {
                    lblTime.Visible = true;
                    RadTimeFrom.Visible = true;
                    RadComboBox1.Visible = true;

                    try
                    {
                        RadTimeFrom.SelectedDate = common.myDate(DateTime.Now);
                        if (RadTimeFrom.SelectedDate != null)
                        {
                            string min = RadTimeFrom.SelectedDate.Value.Minute.ToString();
                            min = min.Length == 1 ? "0" + min : min;
                            RadComboBox1.SelectedIndex = RadComboBox1.Items.IndexOf(RadComboBox1.Items.FindItemByValue(min));
                        }
                    }
                    catch
                    {
                    }
                }
            }

            FillSessionAndQuesryStirnValue();
            if (common.myInt(Session["UserId"]) == 0)
            {
                Response.Redirect("~/Login.aspx", false);
            }
            RTF1.FontNames.Clear();
            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowAllFontNameForDischargeSummary", sConString).Equals("Y"))
            {
                clsIVF objIvf = new clsIVF(sConString);
                DataTable dtAllFontName = new DataTable();
                dtAllFontName = objIvf.getAllFontName();

                if (dtAllFontName.Rows.Count > 0)
                {
                    for (int iAllFontName = 0; iAllFontName < dtAllFontName.Rows.Count; iAllFontName++)
                    {
                        RTF1.FontNames.Add(common.myStr(dtAllFontName.Rows[iAllFontName]["fontname"]));
                    }
                }
                //RTF1.FontNames.Add("Arial");
                //RTF1.FontNames.Add("Courier New");
                //RTF1.FontNames.Add("Garamond");
                //RTF1.FontNames.Add("Georgia");
                //RTF1.FontNames.Add("MS Sans Serif");
                //RTF1.FontNames.Add("Segoe UI");
                //RTF1.FontNames.Add("Tahoma");
                //RTF1.FontNames.Add("Times New Roman");
                //RTF1.FontNames.Add("Verdana");
                //RTF1.FontNames.Add("Candara");
                //RTF1.FontNames.Add("Calibri");
                dtAllFontName.Dispose();
                objIvf = null;
            }
            else
            {
                RTF1.FontNames.Add(common.myStr(hdnFontName.Value));
            }

            RTF1.RealFontSizes.Clear();
            RTF1.RealFontSizes.Add("9pt");
            RTF1.RealFontSizes.Add("10pt");
            RTF1.RealFontSizes.Add("11pt");
            RTF1.RealFontSizes.Add("12pt");
            RTF1.RealFontSizes.Add("14pt");
            RTF1.RealFontSizes.Add("18pt");
            RTF1.RealFontSizes.Add("20pt");
            RTF1.RealFontSizes.Add("24pt");
            RTF1.RealFontSizes.Add("26pt");
            RTF1.RealFontSizes.Add("36pt");
            RTF1.StripFormattingOptions = Telerik.Web.UI.EditorStripFormattingOptions.MSWordRemoveAll | Telerik.Web.UI.EditorStripFormattingOptions.ConvertWordLists;
            dtpdate.SelectedDate = DateTime.Now;
            BindTemplateData();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            BindPatientHiddenDetails();
            BindSignDoctor();
            BindJuniorSignDoctor();
            BindOtherDoctor();
            ViewState["CheckPendingIndentForDischargeSummaryOnFinalized"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "CheckPendingIndentForDischargeSummaryOnFinalized", sConString);
            if (common.myInt(ViewState["EncounterId"]) > 0)
            {

                //yogesh 2/05/22
                ddlReportType_SelectedIndexChanged(this, null);
                //string type = common.myStr(ddlReportType.SelectedValue) == "CS" ? "CS" : "W";  // comment 19-05-2022 kuldeep
                //BindReportTemplateNames(common.myInt(ViewState["EncounterId"]), type);
            }
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
            {
                Page.Title = "Death Summary";
                lbltitle.Text = "Death Summary";
            }
            ViewState["EncounterDate"] = common.myStr(Request.QueryString["EncounterDate"]).Trim();
            if (common.myStr(ViewState["EncounterDate"]).Equals(string.Empty))
            {
                ViewState["EncounterDate"] = common.myStr(Session["EncounterDate"]).Trim();
            }
            if (common.myStr(Request.QueryString["callby"]).ToUpper().Equals("MRD"))
            {
                btnFinalize.Visible = false;
                btnSave.Visible = false;
                btnClose.Visible = false;
            }
            else
            {
                btnFinalize.Visible = true;
                btnSave.Visible = true;
                btnClose.Visible = true;
            }
            if (common.myStr(Request.QueryString["Master"]).ToUpper() != "NO")
            {
                btnClose.Visible = false;
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                btnFinalize.Visible = false;
                btnSave.Visible = false;
            }
            bindDepartmentAndHeader();
            BindPatientDischargeSummary();
            if (common.myStr(btnFinalize.Text).ToUpper().Contains("DEFINALIZED"))
            {
                chkAutoSave.Checked = false;
                Timer1.Enabled = false;
            }
            else
            {
                chkAutoSave.Checked = true;
                Timer1.Enabled = true;
            }

            int TimerInerval = common.myInt(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "AllowAutoSaveDischargeSummary", sConString));
            if (TimerInerval.Equals(0))
            {
                TimerInerval = 60000;
            }

            Timer1.Interval = TimerInerval;
            ViewState["TimerInerval"] = TimerInerval;

            CheckIsRequiredDischargeSummaryDate();
            chkAutoSave.Checked = false;

            string IsShowJuniorDoctorInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowJuniorDoctorInDischargeSummary", sConString);

            if (IsShowJuniorDoctorInDischargeSummary.Equals("Y"))
            {
                lblJuniorDoctor.Visible = true;
                lblJuniorDoctor.Attributes.Add("style", "display:''");

                ddlJuniorDoctorSign.Visible = true;
                ddlJuniorDoctorSign.Attributes.Add("style", "display:''");
            }
            else
            {
                lblJuniorDoctor.Visible = false;
                lblJuniorDoctor.Attributes.Add("style", "display:none");
                lblJuniorDoctor.Attributes.Add("style", "display:inline-block");

                ddlJuniorDoctorSign.Visible = false;
                ddlJuniorDoctorSign.Attributes.Add("style", "display:none");
                ddlJuniorDoctorSign.Attributes.Add("style", "display:inline-block");
                ddlJuniorDoctorSign.SelectedIndex = 0;
            }

            string IsShowOtherDoctorInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowOtherDoctorInDischargeSummary", sConString);

            if (IsShowOtherDoctorInDischargeSummary.Equals("Y"))
            {
                lblSignThirdDoctor.Visible = true;
                lblSignThirdDoctor.Attributes.Add("style", "display:''");

                ddlSignThirdDoctor.Visible = true;
                ddlSignThirdDoctor.Attributes.Add("style", "display:''");



                lblSignFourthDoctor.Visible = true;
                lblSignFourthDoctor.Attributes.Add("style", "display:''");

                ddlSignFourthDoctor.Visible = true;
                ddlSignFourthDoctor.Attributes.Add("style", "display:''");
            }
            else
            {
                lblSignThirdDoctor.Visible = false;
                lblSignThirdDoctor.Attributes.Add("style", "display:none");
                lblSignThirdDoctor.Attributes.Add("style", "display:inline-block");

                ddlSignThirdDoctor.Visible = false;
                ddlSignThirdDoctor.Attributes.Add("style", "display:none");
                ddlSignThirdDoctor.Attributes.Add("style", "display:inline-block");
                ddlSignThirdDoctor.SelectedIndex = 0;



                lblSignFourthDoctor.Visible = false;
                lblSignFourthDoctor.Attributes.Add("style", "display:none");
                lblSignFourthDoctor.Attributes.Add("style", "display:inline-block");

                ddlSignFourthDoctor.Visible = false;
                ddlSignFourthDoctor.Attributes.Add("style", "display:none");
                ddlSignFourthDoctor.Attributes.Add("style", "display:inline-block");
                ddlSignFourthDoctor.SelectedIndex = 0;
            }
            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("PUSHPAWATI"))
            {
                BtnGetRISReport.Visible = true;
            }
            ViewState["IsLockEmrDischargeSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsLockEmrDischargeSummary", sConString);
            if (common.myStr(ViewState["IsLockEmrDischargeSummary"]) == "Y")
                IsLockEmrDischargeSummary();
            else
                chkIslock.Visible = false;
        }
    }

    private void CheckIsRequiredDischargeSummaryDate()
    {
        if (!common.myStr(Request.QueryString["For"]).Equals("DthSum"))
        {
            FlagIsRequiredDischargeSummaryDate = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsRequiredDischargeSummaryDate", sConString);
            if (FlagIsRequiredDischargeSummaryDate.Equals("Y"))
            {
                ltrldatetime.Visible = true;
                dtpdate.Visible = true;
                spnStar.Visible = true;
            }
            else if (FlagIsRequiredDischargeSummaryDate.Equals("N"))
            {
                ltrldatetime.Visible = false;
                dtpdate.Visible = false;
                spnStar.Visible = false;
            }
        }
    }
    void FillSessionAndQuesryStirnValue()
    {
        if (common.myInt(Request.QueryString["RegNo"]) > 0)
            ViewState["RegistrationNo"] = common.myStr(Request.QueryString["RegNo"]);
        else
            ViewState["RegistrationNo"] = common.myStr(Session["RegistrationNo"]);

        if (common.myInt(Request.QueryString["RegId"]) > 0)
            ViewState["RegistrationId"] = common.myStr(Request.QueryString["RegId"]);
        else
            ViewState["RegistrationId"] = common.myStr(Session["RegistrationId"]);

        if (common.myInt(Request.QueryString["EncId"]) > 0)
            ViewState["EncounterId"] = common.myStr(Request.QueryString["EncId"]);
        else
            ViewState["EncounterId"] = common.myStr(Session["EncounterId"]);

        if (!common.myStr(Request.QueryString["EncNo"]).Equals(string.Empty))
            ViewState["EncounterNo"] = common.myStr(Request.QueryString["EncNo"]);
        else
            ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);

        BaseC.ATD Objstatus = new BaseC.ATD(sConString);
        ds = new DataSet();
        ds = Objstatus.GetRegistrationId(common.myLong(ViewState["RegistrationNo"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["OPIP"] = "I";
            //Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
            //Session["RegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["id"]);
            //DateTime adminsiondate = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
            Session["FollowUpDoctorId"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
            Session["FollowUpRegistrationId"] = ds.Tables[0].Rows[0]["id"].ToString().Trim();
            ViewState["AdmDId"] = common.myInt(ds.Tables[0].Rows[0]["DoctorId"]).ToString();
            ViewState["AdmissionDate"] = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
        }
        ds.Dispose();

    }

    private void BindPatientDischargeSummary()
    {
        try
        {
            ObjIcm = new BaseC.ICM(sConString);

            ds = new DataSet();
            string type = common.myStr(ddlReportType.SelectedValue) == "CS" ? "CS" : "W";
            string SummaryId;
            int ReportId = 0;
            if (ddlReportFormat.SelectedValue.ToString().Contains("#"))
            {
                SummaryId = ddlReportFormat.SelectedValue.ToString().Split('#')[0];
                ReportId = common.myInt(ddlReportFormat.SelectedValue.ToString().Split('#')[1]);
            }
            else
            {
                SummaryId = "0";
            }
            if (Request.QueryString["For"] != null)
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {

                    ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"])
                        , common.myStr(ViewState["EncounterId"]), ReportId, common.myInt(Session["FacilityId"]), "DE", type, SummaryId);
                }
                else
                {
                    ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"]),
                                        common.myStr(ViewState["EncounterId"]), ReportId, common.myInt(Session["FacilityId"]), "DI", type, SummaryId);
                }
            }
            else
            {
                ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"]),
                                    common.myStr(ViewState["EncounterId"]), ReportId, common.myInt(Session["FacilityId"]), "DI", type, SummaryId);
            }

            ViewState["ICMPatientSummaryDetailsRecordCount"] = ds.Tables[0].Rows.Count;


            if (ds.Tables[0].Rows.Count > 0)
            {
                dtpdate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DOD"]);
                if (!string.IsNullOrEmpty(common.myStr(common.myDate(ds.Tables[0].Rows[0]["DOD"]))))
                    RadTimeFrom.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DOD"]);
                if (RadTimeFrom.SelectedDate != null)
                {
                    string min = RadTimeFrom.SelectedDate.Value.Minute.ToString();
                    min = min.Length == 1 ? "0" + min : min;
                    RadComboBox1.SelectedIndex = RadComboBox1.Items.IndexOf(RadComboBox1.Items.FindItemByValue(min));
                }


                //yogesh 19/05/2022 
                //yogesh modified 21/05/2022

                if (RTF1.Content.Equals("") || !common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]).Equals(""))
                {
                    RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                    ////RTF1.Content = RTF1.Content.Replace("<br/>\r\n<br/>", "<br/>\r\n");
                    hdnTemplateData.Value = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                }

                hdnSummaryID.Value = common.myStr(ds.Tables[0].Rows[0]["SummaryID"]);

                hdnDoctorSignID.Value = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
                hdnSignJuniorDoctorID.Value = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]);

                hdnFinalize.Value = common.myStr(ds.Tables[0].Rows[0]["Finalize"]);
                hdnEncodedBy.Value = common.myStr(ds.Tables[0].Rows[0]["EncodedBy"]);
                lblPreparedBy.Text = common.myStr(ds.Tables[0].Rows[0]["PreparedByName"]);
                lblLastUpdatedBy.Text = common.myStr(ds.Tables[0].Rows[0]["LastUpdatedByName"]);
                lblLastUpdatedDate.Text = common.myStr(ds.Tables[0].Rows[0]["LastUpdatedDate"]);

                lblFinalizedBy.Text = string.Empty;
                if (common.myInt(ds.Tables[0].Rows[0]["FinalizeBy"]) > 0)
                {
                    if (common.myBool(ds.Tables[0].Rows[0]["Finalize"]))
                    {
                        lblFinalizedBy.Text = "<b> Finalize By: </b>" + common.myStr(ds.Tables[0].Rows[0]["FinalizeByName"]);
                    }
                    else
                    {
                        lblFinalizedBy.Text = "<b> Definalized By:  </b>" + common.myStr(ds.Tables[0].Rows[0]["FinalizeByName"]);
                    }
                }

                lblDischargeType.Text = common.myStr(ds.Tables[0].Rows[0]["DischargeStatus"]);



                txtsynopsis.Text = common.myStr(ds.Tables[0].Rows[0]["Synopsis"]);

                txtAddendum.Enabled = true;
                txtAddendum.Text = common.myStr(ds.Tables[0].Rows[0]["Addendum"]);

                chkIsMultiDepartmentCase.Checked = common.myBool(ds.Tables[0].Rows[0]["IsMultiDepartmentCase"]);
                chkIsMultiDepartmentCase_OnCheckedChanged(null, null);
                ddlDepartmentCase.SelectedIndex = ddlDepartmentCase.Items.IndexOf(ddlDepartmentCase.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["CaseId"])));

                if (ds.Tables[0].Rows[0]["FormatId"].ToString().Contains("#"))
                {
                    ViewState["SavedReportFormat"] = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).Split('#')[1];
                }
                else
                {
                    ViewState["SavedReportFormat"] = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).ToString();
                }
                ddlReportFormat.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).ToString();
                //ddlReportFormat.Enabled = false;
                //divFormat.Visible = false;
                btnRefresh.Visible = false;
                //ddlReportType.Enabled = false;

                //divFormatLabel.Visible = true;
                lblReportFormat.Text = string.Empty;
                lblReportFormat.Text = common.myStr(ds.Tables[0].Rows[0]["ReportName"]);

                if (common.myStr(ddlReportType.SelectedValue) == "CS")
                {
                    ddlReportFormat.Enabled = true;

                    btnRefresh.Visible = true;
                    //ddlReportType.Enabled = true;

                }
                else
                {
                    ddlReportFormat.Enabled = false;
                }


                if (common.myInt(ds.Tables[0].Rows[0]["SignDoctorID"]) > 0)
                    ddlDoctorSign.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
                if (common.myInt(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]) > 0)
                    ddlJuniorDoctorSign.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]);

                if (common.myInt(ds.Tables[0].Rows[0]["SignThirdDoctorId"]) > 0)
                    ddlSignThirdDoctor.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["SignThirdDoctorId"]);
                if (common.myInt(ds.Tables[0].Rows[0]["SignFourthDoctorId"]) > 0)
                    ddlSignFourthDoctor.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["SignFourthDoctorId"]);


                if (common.myBool(ds.Tables[0].Rows[0]["AllowEdit"]))
                {
                    ViewState["AllowEdit"] = true;
                    //RTF1.EditModes = Telerik.Web.UI.EditModes.Design;

                    RTF1.Enabled = true;
                    btnFinalize.Visible = true;
                    btnSave.Visible = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    ViewState["AllowEdit"] = false;
                    //  RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;

                    RTF1.Enabled = false;
                    btnSave.Enabled = false;
                }

                if (common.myBool(ViewState["AllowEdit"]))
                {
                    if (hdnFinalize.Value.Trim().Contains("1") && hdnDoctorSignID.Value != "0")
                    {
                        //  RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
                        string script = "function f(){changeMode('" + "4" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        //btnSave.Enabled = false;
                        dtpdate.Enabled = false;
                        ddlDoctorSign.Enabled = false;
                        ddlJuniorDoctorSign.Enabled = false;
                        ddlSignThirdDoctor.Enabled = false;
                        ddlSignFourthDoctor.Enabled = false;
                        if (common.myStr(ddlReportType.SelectedValue) != "CS")
                        {
                            ddlReportFormat.Enabled = false;
                        }
                        //ddlReportType.Enabled = false;
                        ddlTemplates.Enabled = false;
                        chkDateWise.Enabled = false;
                        spell1.Enabled = false;
                        btnFinalize.Text = "Definalized";
                        btnFinalize.ToolTip = "(ctrl+f2)";
                        hdnFinalizeDeFinalize.Value = "DF";
                    }
                    else
                    {
                        string script = "function f(){changeMode('" + "1" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        btnFinalize.Text = "Finalized ";
                        btnFinalize.ToolTip = "(ctrl+f2)";
                        hdnFinalizeDeFinalize.Value = "F";
                        dtpdate.Enabled = true;
                    }
                }
                else
                {
                    //string script = "function f(){changeMode('" + "4" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
            }
            else
            {
                txtAddendum.Enabled = false;
                //dtpdate.SelectedDate = DateTime.Now;
                lblPreparedBy.Text = string.Empty;
                lblLastUpdatedBy.Text = string.Empty;
                lblLastUpdatedDate.Text = string.Empty;
                hdnSummaryID.Value = "0";


                //yogesh
                //palendra change 17

                // if (common.myStr(ddlReportType.SelectedValue) != "CS")
                //{
                //  ddlReportFormat_SelectedIndexChanged(null, null);
                // }



                //if (!common.myStr(hdnDischargeSummaryBind.Value).Equals("1"))
                //{
                //    if (common.myStr(ddlReportType.SelectedValue) != "CS")
                //    {
                //        hdnDischargeSummaryBind.Value = "1";
                //        ddlReportFormat_SelectedIndexChanged(null, null);
                //    }

                //}


                ViewState["AllowEdit"] = true;
                //RTF1.EditModes = Telerik.Web.UI.EditModes.Design;
                string script = "function f(){changeMode('" + "1" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

                RTF1.Enabled = true;
                btnFinalize.Visible = true;
                btnFinalize.Text = "Finalized";
                btnSave.Visible = true;
                btnSave.Enabled = true;

                ddlReportFormat.Enabled = true;
                //ddlReportType.Enabled = true;
                //divFormat.Visible = true;
                btnRefresh.Visible = true;

                // divFormatLabel.Visible = false;
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    checkDepartments(ds.Tables[1]);
                }
            }

            ds.Dispose();
            if (common.myStr(Request.QueryString["For"]).Equals("DthSum"))  // Death summary date 
            {
                //dtpdate.Enabled = false;
                dtpdate.Enabled = true;

                lblTime.Visible = true;
                RadTimeFrom.Visible = true;
                RadComboBox1.Visible = true;
                ltDateTime.Visible = true;
                if (dtpdate.SelectedDate == null)
                {
                    dtpdate.SelectedDate = DateTime.Now;
                }
            }
            CheckDeFinalize();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;

        }
    }

    private void CheckDeFinalize()
    {
        if (common.myStr(btnFinalize.Text).ToUpper().Contains("DEFINALIZED"))
        {
            btnSave.Visible = false;
            btnCancelSummary.Visible = false;
        }
        else
        {
            btnSave.Visible = true;
            btnCancelSummary.Visible = true;
        }
    }

    private void BindSignDoctor()
    {
        try
        {
            ObjIcm = new BaseC.ICM(sConString);
            ds = new DataSet();
            ds = ObjIcm.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDoctorSign.DataSource = ds.Tables[0];
                ddlDoctorSign.DataTextField = "DoctorName";
                ddlDoctorSign.DataValueField = "ID";
                ddlDoctorSign.DataBind();

                ddlDoctorSign.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            }
            ds.Dispose();

            if (common.myInt(ViewState["AdmDId"]) > 0)
            {
                ddlDoctorSign.SelectedValue = common.myInt(ViewState["AdmDId"]).ToString();
            }
            else if (common.myInt(Session["UserID"]) > 0 && common.myStr(Session["OPIP"]).Equals("E"))
            {
                ddlDoctorSign.SelectedIndex = ddlDoctorSign.Items.IndexOf(ddlDoctorSign.Items.FindItemByValue(common.myInt(Session["UserID"]).ToString()));
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void BindJuniorSignDoctor()
    {
        try
        {
            ObjIcm = new BaseC.ICM(sConString);
            ds = new DataSet();
            ds = ObjIcm.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlJuniorDoctorSign.DataSource = ds.Tables[0];
                ddlJuniorDoctorSign.DataTextField = "DoctorName";
                ddlJuniorDoctorSign.DataValueField = "ID";
                ddlJuniorDoctorSign.DataBind();

                ddlJuniorDoctorSign.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            }
            ds.Dispose();

            if (common.myInt(ViewState["AdmDId"]) > 0)
            {
                //ddlJuniorDoctorSign.SelectedIndex = ddlJuniorDoctorSign.Items.IndexOf(ddlJuniorDoctorSign.Items.FindItemByValue(common.myInt(ViewState["AdmDId"]).ToString()));
            }
            else if (common.myInt(Session["UserID"]) > 0 && common.myStr(Session["OPIP"]).Equals("E"))
            {
                ddlJuniorDoctorSign.SelectedIndex = ddlJuniorDoctorSign.Items.IndexOf(ddlJuniorDoctorSign.Items.FindItemByValue(common.myInt(Session["UserID"]).ToString()));
            }
            else
            {
                ddlJuniorDoctorSign.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }


    private void BindOtherDoctor()
    {
        try
        {
            ObjIcm = new BaseC.ICM(sConString);
            ds = new DataSet();
            ds = ObjIcm.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSignThirdDoctor.DataSource = ds.Tables[0];
                ddlSignThirdDoctor.DataTextField = "DoctorName";
                ddlSignThirdDoctor.DataValueField = "ID";
                ddlSignThirdDoctor.DataBind();

                ddlSignThirdDoctor.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));

                ddlSignFourthDoctor.DataSource = ds.Tables[0];
                ddlSignFourthDoctor.DataTextField = "DoctorName";
                ddlSignFourthDoctor.DataValueField = "ID";
                ddlSignFourthDoctor.DataBind();

                ddlSignFourthDoctor.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            }
            ds.Dispose();

            ddlSignThirdDoctor.SelectedIndex = 0;
            ddlSignFourthDoctor.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void chkDateWise_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkDateWise.Checked == true)
        {
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
        }
        else
        {
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
        }
    }

    protected void ddlReportFormat_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            RTF1.Content = string.Empty;
            txtsynopsis.Text = string.Empty;
            txtAddendum.Text = string.Empty;
            txtCancelRemarks.Text = string.Empty;
            int sValue = common.myInt(ddlReportFormat.SelectedValue);
            // if (common.myStr(ddlReportType.SelectedValue) == "CS" || common.myInt(ViewState["ReportId"]) > 0)
            if (common.myInt(ViewState["ICMPatientSummaryDetailsRecordCount"]) > 0)
            {

                BindPatientDischargeSummary();
            }
            else
            {
                if (common.myInt(sValue) > 0)
                {
                    hdnTemplateData.Value = BindDischargeSummary();
                    RTF1.Content = hdnTemplateData.Value;
                }
                else
                {
                    return;
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
    private string BindDischargeSummary()
    {
        try
        {
            StringBuilder sbTemplateStyle = new StringBuilder();
            DataSet dsTemplate = new DataSet();
            DataTable dtTemplate = new DataTable();
            DataSet dsTemplateStyle = new DataSet();
            DataRow drTemplateStyle = null;
            DataTable dtDischargeType = new DataTable();

            string Templinespace = string.Empty;
            string fdate = string.Empty;
            string tdate = string.Empty;
            if (chkDateWise.Checked)
            {
                fdate = common.myDate(dtpFromDate.SelectedDate.Value).ToString();
                tdate = common.myDate(dtpToDate.SelectedDate.Value).ToString();
            }

            int RegId = common.myInt(ViewState["RegistrationId"]);
            int HospitalId = common.myInt(Session["HospitalLocationID"]);
            int EncounterId = common.myInt(ViewState["EncounterId"]);
            //int UserId = common.myInt(Session["UserID"]);
            Int16 UserId = Convert.ToInt16(common.myInt(Session["UserID"]));
            DL_Funs ff = new DL_Funs();
            BindSummary bnotes = new BindSummary(sConString);
            //fun = new BaseC.DiagnosisDA(sConString);
            string DoctorId = common.myStr(ViewState["DoctorId"]);//fun.GetDoctorId(HospitalId, UserId);
            string FormId = "0";
            if (Session["formId"] != null)
            {
                FormId = common.myStr(Session["formId"]);
            }
            string Saved_RTF_Content = string.Empty;
            clsIVF note = new clsIVF(sConString);
            // dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, "0");
            dsTemplate = note.getEMRTemplateReportSequence(common.myInt(ddlReportFormat.SelectedValue));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            dtDischargeType = note.getDischargeType(EncounterId);


            if (common.myInt(dtDischargeType.Rows.Count) > 0)
            {
                lblDischargeType.Text = common.myStr(dtDischargeType.Rows[0]["DischargeType"]);
            }


            dtTemplate = dsTemplate.Tables[0];


            string gBegin = string.Empty;
            string gEnd = string.Empty;
            string Fonts = string.Empty;

            // sb.Append("<span style='" + Fonts + "'>");
            //sb.Append("<span style='font-family: courier new;color: #000000;'>");
            sb.Append("<span>");

            if (dtTemplate.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTemplate.Rows)
                {
                    string strTemplateType = common.myStr(dr["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    //string TEXT = common.myStr(dr["HeadingName"]);

                    if (common.myStr(dr["PageName"]).Trim().Equals(string.Empty) && common.myStr(dr["SectionName"]).Trim().Equals(string.Empty)
                        && common.myStr(dr["HeadingName"]).Trim().ToUpper() != "MEDICATIONS")
                    {
                        sbTemplateStyle = new StringBuilder();
                        if (!common.myStr(dr["HeadingName"]).Trim().Equals(string.Empty))
                        {
                            if (common.myBool(dr["IsHeading"]))
                            {
                                sbTemplateStyle.Append("<table width='100%'><tr><td><hr width='100%' size='1' /></td></tr><tr><td><span style=' font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + "; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span></td></tr><tr><td><hr width='100%' size='1' /></td></tr></table>");//<br/>
                            }
                            else
                            {
                                if (!common.myStr(dr["DefaultText"]).Trim().Equals(string.Empty))
                                {
                                    sbTemplateStyle.Append("<span style=' font-weight: 700; font-family:" + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                                }
                                else
                                {
                                    sbTemplateStyle.Append("<span style=' font-weight: 700; font-family:" + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");//< br />
                                }
                            }
                            if (!common.myStr(dr["DefaultText"]).Trim().Equals(string.Empty))
                            {
                                //sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                                sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");//<br/>						
                            }
                        }

                        sb.Append(sbTemplateStyle);
                        drTemplateStyle = null;
                        Templinespace = string.Empty;
                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "VITALS")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (!common.myStr(dr["HeadingName"]).Equals(string.Empty))
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + " </span> : <br/>");
                        }
                        if (!common.myStr(dr["DefaultText"]).Trim().Equals(string.Empty))
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        }
                        StringBuilder sbTemp = new StringBuilder();

                        bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                                    Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                                    fdate, tdate, 0, "0");

                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        else
                        {
                            sb.Append("<br/>");
                        }
                        drTemplateStyle = null;
                        Templinespace = string.Empty;
                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "LAB HISTORY")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myLen(dr["HeadingName"]) > 0)
                        {
                            sbTemplateStyle.Append("<span style=' font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + "; font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myLen(dr["DefaultText"]) > 0)
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        //StringBuilder sbTemp = new StringBuilder();
                        //bnotes.BindLabTestResultReport(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page);

                        //if (sbTemp.ToString() != "")
                        //    sb.Append(sbTemp + "<br/><br/>");
                        //else
                        sb.Append(sbTemplateStyle + "<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;
                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "CHIEF COMPLAINTS" || common.myStr(dr["PageName"]).ToUpper().Trim() == "COMPLAINTS")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        }
                        if (common.myLen(dr["HeadingName"]) > 0)
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myLen(dr["DefaultText"]) > 0)
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                            Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                           fdate, tdate);
                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "DIAGNOSIS")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId), DoctorId,
                                              sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                              common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                              fdate, tdate, 0, "0");

                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        else
                        {
                            sb.Append(sbTemplateStyle + "<br/>");
                        }


                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    if (common.myStr(dr["PageName"]).ToUpper().Trim() == "PROVISIONAL DIAGNOSIS")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        BindNotes objB = new BindNotes(sConString);

                        objB.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId), DoctorId,
                                              sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                              common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                              fdate, tdate, 0, "0", string.Empty);

                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        else
                        {
                            sb.Append(sbTemplateStyle + "<br/>");
                        }


                        drTemplateStyle = null;
                        Templinespace = "";
                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "ALLERGIES")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        drTemplateStyle = null;// = dv[0].Row;
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
                                    common.myStr(Session["UserID"]), common.myStr(dr["PageID"]),
                                  fdate, tdate, 0);
                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//<br/>
                            }
                        }
                        else
                            sb.Append(sbTemplateStyle + "<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    // else if (common.myStr(dr["PageName"]).Trim().Contains("Prescription"))
                    else if (common.myStr(dr["HeadingName"]).ToUpper().Trim().ToUpper() == "MEDICATIONS")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        // DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        // dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        //if (dv.Count > 0)
                        //{
                        //    drTemplateStyle = dv[0].Row;
                        //}
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        sb.Append(sbTemplateStyle);
                        BindMedicationIP(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
                                        Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                       fdate, tdate, common.myInt(Session["FacilityId"]), "D");
                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "CURRENT MEDICATION")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700;font-family: " + common.myStr(hdnFontName.Value) + "; font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700;font-family: " + common.myStr(hdnFontName.Value) + "; font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                            Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                            fdate, tdate);

                        //bnotes.BindMedicationTabular(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                        //                   Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                        //                   fdate, tdate);
                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "ORDERS AND PROCEDURES" || common.myStr(dr["PageName"]).ToUpper().Trim() == "ORDER")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindOrders(common.myInt(ViewState["RegistrationId"]), HospitalId, EncounterId,
                                        Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                        common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                        fdate, tdate, "0");

                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//<br/>
                            }
                        }
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "IMMUNIZATION CHART" || common.myStr(dr["PageName"]).ToUpper().Trim() == "IMMUNIZATION")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindImmunization(HospitalId.ToString(), common.myInt(ViewState["RegistrationId"]),
                                    common.myInt(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                                    fdate, tdate);
                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//<br/>
                            }
                        }
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    else if ((common.myStr(dr["PageName"]).ToUpper().Trim() == "ROS"))
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            string sBegin = "", sEnd = "";
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        BindProblemsROS(HospitalId, EncounterId, sbTemp, common.myStr(dr["DisplayName"]).Trim(), common.myStr(dr["TemplateName"]).Trim(), common.myStr(dr["PageId"]));

                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//<br/>
                            }
                        }
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    else if (common.myStr(dr["SectionName"]).ToUpper().Trim() != "" && !common.myStr(dr["EmrTemplateTypesCode"]).ToUpper().Trim().Equals("OT")) //&& common.myStr(dr["TemplateCode"]).Trim() == "HIS")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : ");//<br/>
                        }
                        else if (common.myInt(dr["DisplaySectionName"]) > 0)
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["SectionName"]).Trim() + "</span> : ");//<br/>
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  ");//<br/>
                        }
                        sb.Append(sbTemplateStyle);
                        StringBuilder sbTemp = new StringBuilder();
                        bindDataDischargeSummary(FormId, common.myStr(dr["TemplateId"]), sbTemp, common.myStr(dr["SectionId"]), common.myStr(dr["FieldId"]), common.myStr(ddlReportFormat.SelectedValue), common.myInt(dr["DisplaySectionName"]), common.myInt(dr["DisplayFieldName"]), string.Empty);

                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp.ToString() + "<br/>");//<br/>
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";
                    }

                    /**** Start Pending Investigation Part *****/
                    #region Pending Investigation
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "PENDING INVESTIGATION")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        //bnotes.BindImmunization(HospitalId.ToString(), common.myInt(ViewState["RegistrationId"]),
                        //            common.myInt(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                        //            Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                        //            fdate, tdate);

                        bnotes.BindPendingInvestigation(common.myInt(HospitalId), common.myInt(Session["Facilityid"]), common.myInt(ViewState["EncounterId"])
                            , sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]));

                        //sbTemp.Append ("< span style = 'font-weight: 700; font-family: "+ common.myStr(hdnFontName.Value) +";font-weight:bold; ' > Pending Investigation </ span > : < br />< u >< b ></ b ></ u >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > The patient describes the chronic abortion complicated by renal failure symptoms for the last 4 week(s). </ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > The patient describes the chronic fever, associated with sorethroat, running nose, bodyaches and weakness symptoms for the last 4 week(s). </ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > The patient describes the chronic fever, associated with sorethroat, running nose, bodyaches and weakness symptoms for the last 2 week(s). </ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > The patient describes the chronic abrasion and / or friction burn of thigh with infection symptoms for the last 4 month(s). </ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > low grade fever, associated with bodyaches and weakness < br /></ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > hku < br /></ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > yhkyk < br /></ span >< span style = ' font-size:11pt; color: #000000; font-family: "+ common.myStr(hdnFontName.Value) +" ; ' > yhukjiyk < br /></ span >");


                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//< br />
                            }
                        }
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    #endregion
                    /**** End Pending Investigation Part *****/

                    /**** Start Medication Tabular Part *****/
                    #region Medication Tabular
                    else if (common.myStr(dr["PageName"]).ToUpper().Trim() == "MEDICATION TABULAR")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'> MEDICATIONS </span> : <br/><br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'> MEDICATIONS </span> : <br/><br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();

                        bnotes.BindMedicationTabular(common.myInt(HospitalId), common.myInt(Session["Facilityid"]), common.myInt(ViewState["EncounterId"])
                            , sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]));


                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//<br/>
                            }
                        }
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = "";

                    }
                    #endregion
                    /**** End Medication Tabular Part *****/

                    #region OT Notes
                    else if (common.myStr(dr["SectionName"]).ToUpper().Trim() != "" && common.myStr(dr["EmrTemplateTypesCode"]).ToUpper().Trim().Equals("OT")) //&& common.myStr(dr["TemplateCode"]).Trim() == "HIS")
                    {
                        int lck = 0;

                        sbTemplateStyle = new StringBuilder();
                        if (flagOTHeadingName)
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'> OT Notes</span> : <br/>");
                            flagOTHeadingName = false;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 700; font-family: " + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>" + common.myStr(dr["SectionName"]).Trim() + "</span> : <br/>");
                        }
                        if (common.myStr(dr["DefaultText"]).Trim() != "")
                        {
                            sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                        }
                        sb.Append(sbTemplateStyle);
                        StringBuilder sbTemp = new StringBuilder();
                        bindDataDischargeSummary(FormId, common.myStr(dr["TemplateId"]), sbTemp, common.myStr(dr["SectionId"]), common.myStr(dr["FieldId"]), common.myStr(ddlReportFormat.SelectedValue), common.myInt(dr["DisplaySectionName"]), common.myInt(dr["DisplayFieldName"]), "OT");

                        if (!sbTemp.ToString().Equals(string.Empty))
                        {
                            if (sbTemp.ToString().EndsWith("<br />") || sbTemp.ToString().EndsWith("<br/>"))
                            {
                                sb.Append(sbTemp + "<br/>");
                            }
                            else
                            {
                                sb.Append(sbTemp + "<br/>");//<br/>
                            }
                        }
                        else
                            sb.Append(sbTemp + "<br/>");


                        drTemplateStyle = null;
                        Templinespace = "";
                    }
                    #endregion


                }
                sb.Append("</span>");


                sbTemplateStyle = null;
                StringBuilder temp = new StringBuilder();
                bnotes.GetEncounterFollowUpAppointment(Session["HospitalLocationId"].ToString(), common.myInt(ViewState["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
                if (temp.Length > 20)
                {
                    //sb.Append(temp);
                    sb.Append("</span>");
                    Saved_RTF_Content += temp.ToString();
                }

                if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
                {
                    string sbReplacement = string.Empty;
                    sbReplacement = Regex.Replace(sb.ToString(), @"\t|\n|\r", "");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /><br /></span>", "<br/>");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span>  <br/><br/>", "<br/></span>");//<br/>
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /><br /><br /><br /></span>  <br/><br/>", "<br/><br/>");//< br />
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /></span>  <br/><br/>", "<br/><br/>");//< br />
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /><br /><br /><br/><br/>", "<br/><br/>");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br/><br/>", "<br/><br/>");//<br/>

                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><br/><br/><br/>", "<br/>");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span> <br/>", "<br/>");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span> <br/><br/>", "<br/>");

                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /><br /><span style=' font-size:11pt; color: #000033; font-family: Arial ; '><br/>", "<br/><br/>");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style=' font-size:11pt; color: #000033; font-family: Arial ; '><br/><span style=' font-size:9pt; color: #000033; font-family: Arial; font-weight: bold; '>", "<br/><br/>");
                    sbReplacement = Regex.Replace(sbReplacement.Trim(), "</span></span></p><br /><span style=' font-size:11pt; color: #000000; font-family: Arial ; '><br/><span style=' font-size:9pt; color: #000000; font-family: Arial; font-weight: bold; '>", "<br/><br/>");

                    return sbReplacement.ToString();
                    // return sb.ToString();
                }
                else
                {
                    if (sb.ToString() == "" || sb.ToString() == null)
                    {
                        string sbReplacement = string.Empty;
                        sbReplacement = Regex.Replace(Saved_RTF_Content.ToString(), @"\t|\n|\r", "");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /><br /></span>", "<br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span>  <br/><br/>", "<br/>");//<br/>
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /><br /><br /><br /></span>  <br/><br/>", "<br/><br/>");//<br/>
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /></span>  <br/><br/>", "<br/><br/>");//<br/>
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /><br /><br /><br/><br/>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br/><br/>", "<br/><br/>");//<br/>

                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><br/><br/><br/>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span> <br/>", "<br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span> <br/><br/>", "<br/>");

                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /><br /><span style=' font-size:11pt; color: #000033; font-family: Arial ; '><br/>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style=' font-size:11pt; color: #000033; font-family: Arial ; '><br/><span style=' font-size:9pt; color: #000033; font-family: Arial; font-weight: bold; '>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "</span></span></p><br /><span style=' font-size:11pt; color: #000000; font-family: Arial ; '><br/><span style=' font-size:9pt; color: #000000; font-family: Arial; font-weight: bold; '>", "<br/><br/>");

                        return sbReplacement.ToString();
                        // return Saved_RTF_Content;
                    }
                    else
                    {
                        string sbReplacement = string.Empty;
                        sbReplacement = Regex.Replace(sb.ToString() + Saved_RTF_Content, @"\t|\n|\r", "");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /><br /></span>", "<br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span>  <br/><br/>", "<br/>");//<br/>
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /><br /><br /><br /></span>  <br/><br/>", "<br/><br/>");//<br/>
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br /></span>  <br/><br/>", "<br/><br/>");//<br/>
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /><br /><br /><br/><br/>", "<br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style='font-weight: 100; font-family: Arial;'><br/><br/>", "<br/><br/>");//<br/>

                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><br/><br/><br/>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span> <br/>", "<br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /></span> <br/><br/>", "<br/>");

                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br /><br /><span style=' font-size:11pt; color: #000033; font-family: Arial ; '><br/>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "<br/><span style=' font-size:11pt; color: #000033; font-family: Arial ; '><br/><span style=' font-size:9pt; color: #000033; font-family: Arial; font-weight: bold; '>", "<br/><br/>");
                        sbReplacement = Regex.Replace(sbReplacement.Trim(), "</span></span></p><br /><span style=' font-size:11pt; color: #000000; font-family: Arial ; '><br/><span style=' font-size:9pt; color: #000000; font-family: Arial; font-weight: bold; '>", "<br/><br/>");

                        return sbReplacement.ToString();
                        // return sb.ToString() + Saved_RTF_Content;
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
        return "";
    }
    void BindTemplateData()
    {
        //try
        //{
        DataTable data = BindTemplateNames(common.myInt(ViewState["EncounterId"]));

        int itemOffset = 0;// e.NumberOfItems;
        if (itemOffset == 0)
        {
            this.ddlTemplates.Items.Clear();
        }
        int endOffset = Math.Min(itemOffset + data.Rows.Count, data.Rows.Count);
        // e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["PageName"];
            StringBuilder sTemplate = BindEditor(data.Rows[i]["PageName"].ToString(), data.Rows[i]["PageID"].ToString(), data.Rows[i]["TemplateIdentification"].ToString());
            item.Value = sTemplate.ToString();
            this.ddlTemplates.Items.Add(item);
            item.DataBind();
        }
        //  e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        //}
        //catch(Exception ex)
        //{
        //lblMessage.Text = ex.Message;
        //}

    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        BaseC.Security ObjSecurity = new BaseC.Security(sConString);
        try
        {
            Timer1.Enabled = false;
            chkAutoSave.Checked = false;
            if (common.myInt(hdnSummaryID.Value) == 0)
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {
                    Alert.ShowAjaxMsg("Please save death summary !!", Page);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please save discharge summary !!", Page);
                }
                lblMessage.Text = "";
                Timer1.Enabled = true;
                chkAutoSave.Checked = true;
                return;
            }
            string strDthSum = "DISSUM";
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
                strDthSum = common.myStr(Request.QueryString["For"]);


            string IsBlockDischargeSummaryPrintBeforeFinalize = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsBlockDischargeSummaryPrintBeforeFinalize", sConString);

            if (IsBlockDischargeSummaryPrintBeforeFinalize.Equals("Y"))
            {
                if (!hdnFinalize.Value.Trim().Contains("1") || hdnDoctorSignID.Value == "0")
                {
                    string flgAllowPrint = "IsAllowDischargeSummaryPrintBeforeFinalize";
                    bool IsAllowDischargeSummaryPrintBeforeFinalize = ObjSecurity.CheckUserRights(Convert.ToInt32(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), flgAllowPrint);

                    if (!IsAllowDischargeSummaryPrintBeforeFinalize)
                    {
                        if (common.myStr(Request.QueryString["For"]) == "DthSum")
                        {
                            Alert.ShowAjaxMsg("Can not Print Death Summary Before Finalize !!", Page);
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Can not Print Discharge Summary Before Finalize !!", Page);
                        }

                        lblMessage.Text = "";
                        return;
                    }
                }
            }


            if (chkExport.Checked)
            {
                Response.Redirect("/EMRReports/PrintPdf1.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked) + "&Type=" + common.myStr(ddlReportType.SelectedValue) + "&SummaryId=" + common.myStr(hdnSummaryID.Value), true);
            }
            else
            {
                RadWindow2.NavigateUrl = "/EMRReports/PrintPdf1.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked) + "&Type=" + common.myStr(ddlReportType.SelectedValue) + "&SummaryId=" + common.myStr(hdnSummaryID.Value);

                RadWindow2.Height = 598;
                RadWindow2.Width = 900;
                RadWindow2.Top = 10;
                RadWindow2.Left = 10;
                RadWindow2.Modal = true;
                RadWindow2.OnClientClose = "OnClientClose";
                RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

                RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindow2.VisibleStatusbar = false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ObjSecurity = null;
        }
    }
    protected void btnPrintPDFNew_Click(object sender, EventArgs e)
    {
        BaseC.Security ObjSecurity = new BaseC.Security(sConString);
        try
        {
            Timer1.Enabled = false;
            chkAutoSave.Checked = false;
            if (common.myInt(hdnSummaryID.Value) == 0)
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {
                    Alert.ShowAjaxMsg("Please save death summary !!", Page);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please save discharge summary !!", Page);
                }
                lblMessage.Text = "";
                Timer1.Enabled = true;
                chkAutoSave.Checked = true;
                return;
            }
            string strDthSum = "DISSUM";
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
                strDthSum = common.myStr(Request.QueryString["For"]);


            string IsBlockDischargeSummaryPrintBeforeFinalize = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsBlockDischargeSummaryPrintBeforeFinalize", sConString);

            if (IsBlockDischargeSummaryPrintBeforeFinalize.Equals("Y"))
            {
                if (!hdnFinalize.Value.Trim().Contains("1") || hdnDoctorSignID.Value == "0")
                {
                    string flgAllowPrint = "IsAllowDischargeSummaryPrintBeforeFinalize";
                    bool IsAllowDischargeSummaryPrintBeforeFinalize = ObjSecurity.CheckUserRights(Convert.ToInt32(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), flgAllowPrint);

                    if (!IsAllowDischargeSummaryPrintBeforeFinalize)
                    {
                        if (common.myStr(Request.QueryString["For"]) == "DthSum")
                        {
                            Alert.ShowAjaxMsg("Can not Print Death Summary Before Finalize !!", Page);
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Can not Print Discharge Summary Before Finalize !!", Page);
                        }

                        lblMessage.Text = "";
                        return;
                    }
                }
            }

            if (chkExport.Checked)
            {
                Response.Redirect("/EMRReports/PrintPdfNew.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked), true);
            }
            else
            {
                //RadWindow2.NavigateUrl = "/EMRReports/PrintPdfNew.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked);

                //RadWindow2.Height = 598;
                //RadWindow2.Width = 900;
                //RadWindow2.Top = 10;
                //RadWindow2.Left = 10;
                //RadWindow2.Modal = true;
                //RadWindow2.OnClientClose = "OnClientClose";
                //RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

                //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                //RadWindow2.VisibleStatusbar = false;

                // Response.Redirect("/ICM/PrintDischargeSummary.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked) + " ");

                var URLPath = "/ICM/PrintDischargeSummary.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked) + " ";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + URLPath.Replace("+", "%2B") + "','_blank')", true);

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ObjSecurity = null;
        }
    }
    private StringBuilder BindEditor(string sTemplateName, string sPageID, string sTemplateType)
    {
        ObjIcm = new BaseC.ICM(sConString);
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder sbTemp = new StringBuilder();
        ds = new DataSet();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;
        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        Int16 UserId = Convert.ToInt16(common.myInt(Session["UserID"]));
        int facilityId = common.myInt(Session["Facilityid"]);
        DL_Funs ff = new DL_Funs();
        BindDischargeSummary bnotes = new BindDischargeSummary(sConString);
        fun = new BaseC.DiagnosisDA(sConString);
        string DoctorId = fun.GetDoctorId(HospitalId, UserId);
        dsTemplateStyle = ObjIcm.GetICMTemplateStyle(common.myInt(Session["HospitalLocationID"]));
        string sFromDate = string.Empty, sToDate = string.Empty;
        if (chkDateWise.Checked)
        {
            sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
            sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
        }
        if (sTemplateType == "P")
        {
            if (sTemplateName == "Vitals" || sTemplateName == "Vital")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());
            }
            else if (sTemplateName == "Investigations")
            {
                bnotes.BindLabResult(EncounterId, HospitalId, facilityId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page);
            }
            else if (sTemplateName == "Chief Complaint" || sTemplateName == "Chief Complaints")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());
            }
            else if (sTemplateName == "Diagnosis")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindAssessments(RegId, HospitalId, EncounterId, sFromDate, sToDate, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());
            }
            else if (sTemplateName == "Allergies" || sTemplateName == "Allergy")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindAllergies(RegId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page, Session["HospitalLocationId"].ToString(), common.myInt(Session["UserID"]).ToString(), sPageID);
            }
            else if (sTemplateName == "Medication")
            {

                bnotes.BindMedication(EncounterId, HospitalId, facilityId, sFromDate, sToDate, sbTemp, sbTemplateStyle, "P", drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());
            }
            else if (sTemplateName == "Current Medication")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindMedication(EncounterId, HospitalId, RegId, sFromDate, sToDate, sbTemp, sbTemplateStyle, "C", drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());

            }
            else if (sTemplateName == "Orders And Procedures" || sTemplateName == "Orders" || sTemplateName == "Procedures")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindOrders(common.myInt(ViewState["RegistrationId"]), HospitalId, EncounterId, sFromDate, sToDate, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());
            }
            else if (sTemplateName == "Immunization Chart")
            {
                sbTemplateStyle = new StringBuilder();

                bnotes.BindImmunization(HospitalId.ToString(), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString());
            }
            if (sTemplateName == "ROS")
            {
                //sbTemplateStyle = new StringBuilder();

                //BindProblemsROS(HospitalId, EncounterId, sFromDate, sToDate, sbTemp, sTemplateName, sTemplateName, sPageID);
            }
            else if (sTemplateName == "OT")
            {
                //bTRecords = false;
                //bTMRecords = false;
                //it = 0;
                //sMutipleRecordsDate = "";
                //sbTemplateStyle = new StringBuilder();

                //bindData("0", sFromDate, sToDate, sPageID, sbTemp, sTemplateName);
            }
        }
        else
        {


        }
        return sbTemp;
    }
    protected string GetTemplateId(string TemplateName, int HospitalLocationId)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string sqlQ = "Select Id from EMRTemplate where HospitalLocationId=@HospitalLocationId and templateName like @TemplateName";
        Hashtable hs = new Hashtable();
        hs.Add("@TemplateName", TemplateName);
        hs.Add("@HospitalLocationId", HospitalLocationId);
        Object templateId = dl.ExecuteScalar(CommandType.Text, sqlQ, hs);
        return templateId.ToString();
    }
    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, string sFromDate, string sToDate, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds;
        DataSet dsGender;
        string sEncodedDate = "";
        int pi = 0, ni = 0;
        string strGender = "He";
        bool bPRRecords = false;
        bool bNRRecords = false;
        bool bDisplayDate = false;
        int iNagRowCount = 0;
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        StringBuilder objStrTmp = new StringBuilder();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", GetTemplateId(sTemplateName, Convert.ToInt16(Session["HospitalLocationID"])));
        if (Convert.ToInt16(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (Convert.ToInt16(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        hstInput.Add("@intFormId", common.myInt(Session["formId"]));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;
        hsGender.Add("@intRegistrationId", ViewState["RegistrationId"]);
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
                strGender = "He";
            else
                strGender = "She";
        }
        int pti = 0, nti = 0;
        hsProblems.Add("@intEncounterId", EncounterId);
        hsProblems.Add("@intTemplateId", GetTemplateId(sTemplateName, Convert.ToInt16(Session["HospitalLocationID"])));
        hsProblems.Add("@chvFromDate", sFromDate);
        hsProblems.Add("@chvToDate", sToDate);
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEncodedDate = ds.Tables[0].Rows[0]["EntryDate"].ToString();
            DataView dv1 = new DataView(ds.Tables[0]);
            dv1.RowFilter = "PositiveValue <> ''";
            dtPositiveRos = dv1.ToTable();

            DataView dv2 = new DataView(ds.Tables[0]);
            dv2.RowFilter = "NegativeValue <> ''";
            dtNegativeRos = dv2.ToTable();

        }
        string strSectionId = "", sDate = "", sPreviousValue = "";
        foreach (DataRow tdr in ds.Tables[0].Rows)
        {
            if (sEncodedDate == tdr["EntryDate"].ToString())
            {
                if (bPRRecords == false && bNRRecords == false)
                {
                    objStrTmp.Append("<b>" + tdr["EntryDate"].ToString() + "</b> : <br/>");
                    DataTable dt = new DataTable();
                    for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
                    {
                        DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                        if (sEncodedDate == dr["EntryDate"].ToString())
                        {
                            if (dr["SectionId"].ToString() != strSectionId)
                            {
                                string sBegin = "", sEnd = "";
                                if (drFont["SectionsBold"].ToString() != "" || drFont["SectionsItalic"].ToString() != "" || drFont["SectionsUnderline"].ToString() != "" || drFont["SectionsFontSize"].ToString() != "" || drFont["SectionsForecolor"].ToString() != "" || drFont["SectionsListStyle"].ToString() != "")
                                {
                                    MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                    if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (pti == 0)
                                        {
                                            objStrTmp.Append(sBegin + "Positive Symptoms:" + sEnd);
                                        }
                                        objStrTmp.Append("<br />" + sBegin + dr["SectionName"].ToString().Trim() + ": " + sEnd);
                                    }
                                }
                                else
                                {
                                    if (pti == 0)
                                    {
                                        objStrTmp.Append("<br />" + "Positive Symptoms:");
                                    }
                                    objStrTmp.Append("<br />" + dr["SectionName"].ToString().Trim() + ": ");
                                }
                                pti++;

                                if (dr["FieldsBold"].ToString() != "" || dr["FieldsItalic"].ToString() != "" || dr["FieldsUnderline"].ToString() != "" || dr["FieldsFontSize"].ToString() != "" || dr["FieldsForecolor"].ToString() != "" || dr["FieldsListStyle"].ToString() != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                    objStrTmp.Append(sBegin + strGender + " has ");
                                }
                                else
                                    objStrTmp.Append(strGender + " has ");

                                strSectionId = Convert.ToString(dr["SectionId"]);
                                DataView dv = new DataView(dtPositiveRos);
                                dv.RowFilter = "SectionId =" + dr["SectionId"].ToString();
                                dt = dv.ToTable();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {

                                    if (j == dt.Rows.Count - 1)
                                    {
                                        if (dt.Rows.Count == 1)
                                        {
                                            objStrTmp.Append("" + dt.Rows[j]["PositiveValue"].ToString().ToLower().Trim() + ". ");
                                        }
                                        else
                                        {
                                            objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                            objStrTmp.Append(" and " + dt.Rows[j]["PositiveValue"].ToString().ToLower().Trim() + ". ");
                                        }
                                    }
                                    else
                                        objStrTmp.Append(dt.Rows[j]["PositiveValue"].ToString().ToLower().Trim() + ", ");

                                    bPRRecords = true;
                                }
                                objStrTmp.Append(sEnd);
                                strSectionId = dr["SectionId"].ToString();
                                sDate = dr["EntryDate"].ToString();
                            }
                        }

                    }
                    //strSectionId = "";
                    for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
                    {
                        DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                        if (sEncodedDate == dr["EntryDate"].ToString())
                        {
                            if (dr["SectionId"].ToString() != strSectionId)
                            {
                                string sBegin = "", sEnd = "";
                                if (drFont["SectionsBold"].ToString() != "" || drFont["SectionsItalic"].ToString() != "" || drFont["SectionsUnderline"].ToString() != "" || drFont["SectionsFontSize"].ToString() != "" || drFont["SectionsForecolor"].ToString() != "" || drFont["SectionsListStyle"].ToString() != "")
                                {

                                    MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                    if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (nti == 0)
                                        {
                                            objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                                        }
                                        objStrTmp.Append("<br />" + sBegin + dr["SectionName"].ToString().Trim() + ": " + sEnd);
                                    }
                                }
                                else
                                {
                                    if (nti == 0)
                                    {
                                        objStrTmp.Append("<br />" + "Negative Symptoms:");
                                    }
                                    objStrTmp.Append("<br />" + dr["SectionName"].ToString().Trim() + ": ");
                                }
                                nti++;
                                if (dr["FieldsBold"].ToString() != "" || dr["FieldsItalic"].ToString() != "" || dr["FieldsUnderline"].ToString() != "" || dr["FieldsFontSize"].ToString() != "" || dr["FieldsForecolor"].ToString() != "" || dr["FieldsListStyle"].ToString() != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                    objStrTmp.Append(sBegin + strGender + " does not have ");
                                }
                                else
                                    objStrTmp.Append(strGender + " does not have ");

                                strSectionId = Convert.ToString(dr["SectionId"]);
                                DataView dv = new DataView(dtNegativeRos);
                                dv.RowFilter = "SectionId =" + dr["SectionId"].ToString();
                                dt = dv.ToTable();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {

                                    if (j == dt.Rows.Count - 1)
                                    {
                                        if (dt.Rows.Count == 1)
                                        {
                                            objStrTmp.Append("" + dt.Rows[j]["NegativeValue"].ToString().ToLower().Trim() + ". ");
                                        }
                                        else
                                        {
                                            objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                            objStrTmp.Append(" or " + dt.Rows[j]["NegativeValue"].ToString().ToLower().Trim() + ". ");
                                        }
                                    }
                                    else
                                        objStrTmp.Append(dt.Rows[j]["NegativeValue"].ToString().ToLower().Trim() + ", ");

                                    bNRRecords = true;
                                }
                                objStrTmp.Append(sEnd);
                                strSectionId = dr["SectionId"].ToString();
                                sDate = dr["EntryDate"].ToString();
                            }

                        }

                    }

                }
            }
            else
            {
                if (sDate != tdr["EntryDate"].ToString())
                {
                    objStrTmp.Append("<br/><br/>");
                    objStrTmp.Append("<b>" + tdr["EntryDate"].ToString() + "</b>  : <br/>");
                }
                DataTable dt = new DataTable();

                DataView dv = new DataView(dtPositiveRos);
                dv.RowFilter = "SectionID = " + tdr["SectionID"].ToString();
                dt = dv.ToTable();
                foreach (DataRow dr in dtPositiveRos.Rows)
                {
                    if (dr["EntryDate"].ToString() == tdr["EntryDate"].ToString())
                    {
                        //DataView dv = new DataView(dtPositiveRos);
                        //dv.RowFilter = "SectionID = " + tdr["SectionID"].ToString();
                        if (dr["SectionId"].ToString() != strSectionId && Convert.ToInt64(dr["SectionId"]) > Convert.ToInt64(strSectionId))
                        {
                            string sBegin = "", sEnd = "";
                            if (drFont["SectionsBold"].ToString() != "" || drFont["SectionsItalic"].ToString() != "" || drFont["SectionsUnderline"].ToString() != "" || drFont["SectionsFontSize"].ToString() != "" || drFont["SectionsForecolor"].ToString() != "" || drFont["SectionsListStyle"].ToString() != "")
                            {
                                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                {
                                    if (sDate != tdr["EntryDate"].ToString())
                                    {
                                        objStrTmp.Append("" + sBegin + "Positive Symptoms: " + sEnd);
                                    }
                                    objStrTmp.Append("<br />" + sBegin + dr["SectionName"].ToString().Trim() + ": " + sEnd);
                                }
                            }
                            else
                            {
                                if (sDate != tdr["EntryDate"].ToString())
                                {
                                    objStrTmp.Append("<br />" + "Positive Symptoms: ");
                                }
                                objStrTmp.Append("<br />" + dr["SectionName"].ToString().Trim() + ": ");
                            }

                            if (dr["FieldsBold"].ToString() != "" || dr["FieldsItalic"].ToString() != "" || dr["FieldsUnderline"].ToString() != "" || dr["FieldsFontSize"].ToString() != "" || dr["FieldsForecolor"].ToString() != "" || dr["FieldsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                objStrTmp.Append(sBegin + strGender + " has ");
                            }
                            else
                                objStrTmp.Append(strGender + " has ");

                            strSectionId = Convert.ToString(dr["SectionId"]);
                            DataView dvf = new DataView(dtPositiveRos);
                            dvf.RowFilter = "SectionId =" + dr["SectionId"].ToString();
                            dt = dvf.ToTable();
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {

                                if (j == dt.Rows.Count - 1)
                                {
                                    if (dt.Rows.Count == 1)
                                    {
                                        objStrTmp.Append("" + dt.Rows[j]["PositiveValue"].ToString().ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                        objStrTmp.Append(" and " + dt.Rows[j]["PositiveValue"].ToString().ToLower().Trim() + ". ");
                                    }
                                }
                                else
                                    objStrTmp.Append(dt.Rows[j]["PositiveValue"].ToString().ToLower().Trim() + ", ");

                            }
                            objStrTmp.Append(sEnd);
                            sDate = dr["EntryDate"].ToString();
                            //strSectionId = dr["SectionId"].ToString();
                        }
                    }
                    // sDate = tdr["EntryDate"].ToString();
                    pi++;
                    //strSectionId = dr["SectionId"].ToString();
                }
                //strSectionId = "";

                DataTable dtN = new DataTable();
                DataView dvN = new DataView(dtNegativeRos);
                dvN.RowFilter = "SectionID = " + tdr["SectionID"].ToString();
                DataTable dtNg = dvN.ToTable();
                foreach (DataRow dr in dtNg.Rows)
                {
                    if (strSectionId != tdr["SectionId"].ToString())
                    {
                        bNRRecords = false;
                    }
                    else
                    {
                        if (sPreviousValue != "")
                        {
                            bNRRecords = false;
                        }
                    }
                    if (dr["EntryDate"].ToString() == tdr["EntryDate"].ToString() && bNRRecords == false)
                    {
                        if (sPreviousValue != dr["NegativeValue"].ToString() && strSectionId == dr["SectionId"].ToString())
                        {
                            string sBegin = "", sEnd = "";
                            if (drFont["SectionsBold"].ToString() != "" || drFont["SectionsItalic"].ToString() != "" || drFont["SectionsUnderline"].ToString() != "" || drFont["SectionsFontSize"].ToString() != "" || drFont["SectionsForecolor"].ToString() != "" || drFont["SectionsListStyle"].ToString() != "")
                            {

                                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                {
                                    //if (strSectionId == dr["SectionId"].ToString())
                                    //{
                                    objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                                    // bDisplayDate = false;
                                    //}
                                    objStrTmp.Append("<br />" + sBegin + dr["SectionName"].ToString().Trim() + ": " + sEnd);
                                }
                            }
                            else
                            {
                                if (sDate != tdr["EntryDate"].ToString())
                                {
                                    objStrTmp.Append("<br /><br />" + "Negative Symptoms:");
                                }
                                objStrTmp.Append("<br />" + dr["SectionName"].ToString().Trim() + ": ");
                            }

                            if (dr["FieldsBold"].ToString() != "" || dr["FieldsItalic"].ToString() != "" || dr["FieldsUnderline"].ToString() != "" || dr["FieldsFontSize"].ToString() != "" || dr["FieldsForecolor"].ToString() != "" || dr["FieldsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                objStrTmp.Append(sBegin + strGender + " does not have ");
                            }
                            else
                                objStrTmp.Append(strGender + " does not have ");


                            strSectionId = Convert.ToString(dr["SectionId"]);
                            DataTable dtNv = new DataTable();
                            DataView dvf = new DataView(dtNegativeRos);
                            dvf.RowFilter = "SectionId =" + dr["SectionId"].ToString();

                            dtNv = dvf.ToTable();
                            for (int j = 0; j < dtNv.Rows.Count; j++)
                            {
                                if (j == dtNv.Rows.Count - 1)
                                {
                                    if (dtNv.Rows.Count == 1)
                                    {
                                        objStrTmp.Append("" + dtNv.Rows[j]["NegativeValue"].ToString().ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                        objStrTmp.Append(" or " + dtNv.Rows[j]["NegativeValue"].ToString().ToLower().Trim() + ". ");
                                    }
                                }
                                else
                                    objStrTmp.Append(dtNv.Rows[j]["NegativeValue"].ToString().ToLower().Trim() + ", ");

                                sPreviousValue = dtNv.Rows[0]["NegativeValue"].ToString();
                                //bNRRecords = false;
                            }
                            objStrTmp.Append(sEnd);
                            strSectionId = dr["SectionId"].ToString();
                            sDate = tdr["EntryDate"].ToString();
                            bNRRecords = true;
                            break;

                        }
                        iNagRowCount = dtNg.Rows.Count;
                    }
                    ni++;
                    // strSectionId = dr["SectionId"].ToString();
                    break;

                }
            }
        }
        sb.Append(objStrTmp);
        return sb;
    }
    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds;
        DataSet dsGender;
        string strGender = "He";
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        StringBuilder objStrTmp = new StringBuilder();

        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //rafat
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        //hstInput.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hstInput.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        hstInput.Add("@intFormId", common.myStr(Session["formId"]));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", ViewState["RegistrationId"]);
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
            {
                strGender = "He";
            }
            else
            {
                strGender = "She";
            }
        }
        hsProblems.Add("@intEncounterId", EncounterId);
        hsProblems.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv1 = new DataView(ds.Tables[0]);
            dv1.RowFilter = "PositiveValue <> ''";
            dtPositiveRos = dv1.ToTable();

            DataView dv2 = new DataView(ds.Tables[0]);
            dv2.RowFilter = "NegativeValue <> ''";
            dtNegativeRos = dv2.ToTable();
            //Make font start

            if (common.myStr(drFont["TemplateBold"]) != ""
                || common.myStr(drFont["TemplateItalic"]) != ""
                || common.myStr(drFont["TemplateUnderline"]) != ""
                || common.myStr(drFont["TemplateFontSize"]) != ""
                || common.myStr(drFont["TemplateForecolor"]) != ""
                || common.myStr(drFont["TemplateListStyle"]) != "")
            {
                string sBegin = "", sEnd = "";
                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                    objStrTmp.Append(sBegin + sDisplayName.ToString() + sEnd);
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
            }
            else
            {
                if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                    objStrTmp.Append(sDisplayName.ToString());//Default Setting
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
            }

            // Make Font End

            //sb.Append("<u><Strong>Review of systems</Strong></u>");

        }
        // For Positive Symptoms
        if (dtPositiveRos.Rows.Count > 0)
        {
            string strSectionId = ""; // dtPositiveRos.Rows[0]["SectionId"].ToString();
            DataTable dt = new DataTable();
            for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
            {

                DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != ""
                        || common.myStr(drFont["SectionsItalic"]) != ""
                        || common.myStr(drFont["SectionsUnderline"]) != ""
                        || common.myStr(drFont["SectionsFontSize"]) != ""
                        || common.myStr(drFont["SectionsForecolor"]) != ""
                        || common.myStr(drFont["SectionsListStyle"]) != "")
                    {
                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Positive Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }

                    if (common.myStr(dr["FieldsBold"]) != ""
                        || common.myStr(dr["FieldsItalic"]) != ""
                        || common.myStr(dr["FieldsUnderline"]) != ""
                        || common.myStr(dr["FieldsFontSize"]) != ""
                        || common.myStr(dr["FieldsForecolor"]) != ""
                        || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " has ");
                    }
                    else
                    {
                        objStrTmp.Append(strGender + " has ");
                    }

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " has ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (j == (dt.Rows.Count - 1))
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                        {
                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");
                        }
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }

        // For Negative Symptoms
        if (dtNegativeRos.Rows.Count > 0)
        {
            //if (drFont["TemplateBold"].ToString() != "" || drFont["TemplateItalic"].ToString() != "" || drFont["TemplateUnderline"].ToString() != "" || drFont["TemplateFontSize"].ToString() != "" || drFont["TemplateForecolor"].ToString() != "" || drFont["TemplateListStyle"].ToString() != "")
            //{
            //    string sBegin = "", sEnd = "";
            //    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
            //    //objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
            //    //objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}
            //else
            //{
            //    objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}          
            string strSectionId = ""; // 
            DataTable dt = new DataTable();
            for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
            {

                DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != ""
                        || common.myStr(drFont["SectionsItalic"]) != ""
                        || common.myStr(drFont["SectionsUnderline"]) != ""
                        || common.myStr(drFont["SectionsFontSize"]) != ""
                        || common.myStr(drFont["SectionsForecolor"]) != ""
                        || common.myStr(drFont["SectionsListStyle"]) != "")
                    {

                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Negative Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != ""
                        || common.myStr(dr["FieldsItalic"]) != ""
                        || common.myStr(dr["FieldsUnderline"]) != ""
                        || common.myStr(dr["FieldsFontSize"]) != ""
                        || common.myStr(dr["FieldsForecolor"]) != ""
                        || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " does not have ");
                    }
                    else
                    {
                        objStrTmp.Append(strGender + " does not have ");
                    }

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " does not have ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtNegativeRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" or " + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                        {
                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");
                        }
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }
        sb.Append(objStrTmp);
        //sb.Append("<br/>");
        return sb;
    }

    protected void bindData(string iFormId, string sFromDate, string sToDate, string TemplateId, StringBuilder sb, string sTemplateName)
    {
        string str = "";
        string sEncodedDate = "";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);
        if (Convert.ToInt16(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (Convert.ToInt16(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        hstInput.Add("@intFormId", iFormId);
        hstInput.Add("@bitDischargeSummary", Convert.ToBoolean(1));
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Cache.Insert("SectionFormat" + TemplateId + Convert.ToInt16(Session["HospitalLocationID"]), ds.Tables[0], null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);
        hstInput.Add("@intEncounterId", Convert.ToInt32(ViewState["EncounterId"]));
        if (Convert.ToInt16(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (Convert.ToInt16(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");


        hstInput.Add("@chrFromDate", sFromDate);
        hstInput.Add("@chrToDate", sToDate);
        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);
        if (dsAllSectionDetails.Tables[2].Rows.Count > 0)
        {
            sEncodedDate = dsAllSectionDetails.Tables[2].Rows[0]["EntryDate"].ToString();
        }
        string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            DataTable dtFieldValue = new DataTable();
            DataView dv1 = new DataView(dsAllSectionDetails.Tables[3]);
            dv1.RowFilter = "SectionId=" + Convert.ToString(item["SectionId"]);

            DataTable dtFieldName = dv1.ToTable();
            if (dsAllSectionDetails.Tables.Count > 2)
            {
                DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
                dtFieldValue = dv2.ToTable();
            }
            DataSet dsAllFieldsDetails = new DataSet();
            dsAllFieldsDetails.Tables.Add(dtFieldName);
            dsAllFieldsDetails.Tables.Add(dtFieldValue);
            if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
            {
                if (dsAllSectionDetails.Tables.Count > 2)
                {

                    if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                    {
                        string sBegin = "", sEnd = "";

                        DataRow dr3;
                        dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                        getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                        ViewState["iTemplateId"] = Convert.ToInt32(item["TemplateId"]);
                        str = CreateString1(dsAllFieldsDetails, Convert.ToInt32(item["TemplateId"]), sTemplateName, item["SectionID"].ToString(), item["SectionName"].ToString(), sEncodedDate, item["Tabular"].ToString());
                        str += " ";
                        objStrTmp.Append(str);
                    }

                }
            }
        }
        sb.Append(objStrTmp);
    }
    protected string CreateString1(DataSet objDs, int iRootId, string iRootName, string sSectionID, string sSectionName, string sEncodedDate, string TabularType)
    {
        StringBuilder objStr = new StringBuilder();
        string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "", sBegin = "", sEnd = "";
        DataView objDv = null;
        DataTable objDt = null;

        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        int t = 0, t2 = 0, t3 = 0;
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;

                    if (MaxLength != 0)
                    {
                        objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                        FieldsLength = objDs.Tables[0].Rows.Count;
                        for (int i = 0; i < FieldsLength; i++)   // it makes table
                        {
                            objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + objDs.Tables[0].Rows[i]["FieldName"].ToString() + "</th>");
                            dr = objDs.Tables[0].Rows[i];
                            dvValues = new DataView(objDs.Tables[1]);
                            dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                            dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));


                            if (dvValues.ToTable().Rows.Count > MaxLength)
                                MaxLength = dvValues.ToTable().Rows.Count;
                        }
                        objStr.Append("</tr>");
                        if (MaxLength == 0)
                        {
                            //objStr.Append("<tr>");
                            //for (int i = 0; i < FieldsLength; i++)
                            //{
                            //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                            //}
                            //objStr.Append("</tr></table>");
                        }


                        else
                        {
                            if (dsMain.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < MaxLength; i++)
                                {
                                    objStr.Append("<tr>");
                                    for (int j = 0; j < dsMain.Tables.Count; j++)
                                    {
                                        if (dsMain.Tables[j].Rows.Count > i
                                            && dsMain.Tables[j].Rows.Count > 0)
                                        {
                                            objStr.Append("<td style=' " + sFontSize + "' align='right'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                        }
                                        else
                                        {
                                            objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                        }
                                    }
                                    objStr.Append("</tr>");
                                }
                                objStr.Append("</table>");
                            }
                        }
                    }
                }
            }
            else // For Non Tabular Templates
            {
                string fBeginList = "", fEndList = "";
                string sfBegin = "", sfEnd = "";
                int tf = 0;

                string sPSectionName = "";
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    if (sEncodedDate == objDs.Tables[1].Rows[it]["EntryDate"].ToString())
                    {
                        if (bTRecords == false)
                        {
                            objStr.Append("<b>" + objDs.Tables[1].Rows[it]["EntryDate"].ToString() + "</b> : ");
                            bTRecords = true;
                        }

                        DataTable dsSection = (DataTable)Cache["SectionFormat" + iRootId + Convert.ToInt16(Session["HospitalLocationID"])];
                        DataView dv = new DataView(dsSection);
                        dv.RowFilter = "SectionID =" + sSectionID;
                        DataTable dt = dv.ToTable();

                        if (iPrevId == Convert.ToInt32(dt.Rows[0]["TemplateId"]))
                        {
                            if (t2 == 0)
                                if (t3 == 0)//Template
                                {
                                    t3 = 1;
                                    if (dt.Rows[0]["SectionsListStyle"].ToString() == "1")
                                    { BeginList3 = "<ul>"; EndList3 = "</ul>"; }
                                    else if (dt.Rows[0]["SectionsListStyle"].ToString() == "2")
                                    { BeginList3 = "<ol>"; EndList3 = "</ol>"; }
                                }
                            if (dt.Rows[0]["SectionsBold"].ToString() != "" || dt.Rows[0]["SectionsItalic"].ToString() != "" || dt.Rows[0]["SectionsUnderline"].ToString() != "" || dt.Rows[0]["SectionsFontSize"].ToString() != "" || dt.Rows[0]["SectionsForecolor"].ToString() != "" || dt.Rows[0]["SectionsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != dt.Rows[0]["SectionName"].ToString())   //19June2010
                                {
                                    objStr.Append(BeginList3 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                }
                                BeginList3 = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != dt.Rows[0]["SectionName"].ToString())    //19June
                                {
                                    objStr.Append(dt.Rows[0]["SectionName"].ToString()); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                }
                            }
                            if (dt.Rows[0]["SectionsListStyle"].ToString() == "3")
                                objStr.Append("<br />");
                        }
                        else
                        {
                            if (t2 == 0)
                            {
                                t2 = 1;
                                if (dt.Rows[0]["SectionsListStyle"].ToString() == "1")
                                { BeginList2 = "<ul>"; EndList3 = "</ul>"; }
                                else if (dt.Rows[0]["SectionsListStyle"].ToString() == "2")
                                { BeginList2 = "<ol>"; EndList3 = "</ol>"; }
                            }
                            if (dt.Rows[0]["SectionsBold"].ToString() != "" || dt.Rows[0]["SectionsItalic"].ToString() != "" || dt.Rows[0]["SectionsUnderline"].ToString() != "" || dt.Rows[0]["SectionsFontSize"].ToString() != "" || dt.Rows[0]["SectionsForecolor"].ToString() != "" || dt.Rows[0]["SectionsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                {
                                    objStr.Append(BeginList2 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                }
                                BeginList2 = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]))// Comment ON 19June2010
                                    objStr.Append(dt.Rows[0]["SectionName"].ToString()); //Comment On 19June2010
                            }
                            if (dt.Rows[0]["SectionsListStyle"].ToString() == "3" || dt.Rows[0]["SectionsListStyle"].ToString() == "0")
                                objStr.Append("<br />");

                            iPrevId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);

                        }
                        sPSectionName = dt.Rows[0]["SectionName"].ToString();
                    }
                    else
                    {
                        if (bTMRecords == false)
                        {
                            objStr.Append("<br/>");
                            objStr.Append("<b>" + objDs.Tables[1].Rows[it]["EntryDate"].ToString() + "</b> : <br/>");
                            bTMRecords = true;
                        }
                        else if (sMutipleRecordsDate != objDs.Tables[1].Rows[it]["EntryDate"].ToString())
                        {
                            objStr.Append("<br/>");
                            objStr.Append("<b>" + objDs.Tables[1].Rows[it]["EntryDate"].ToString() + "</b> : <br/>");
                        }
                        DataTable dsSection = (DataTable)Cache["SectionFormat" + iRootId + Convert.ToInt16(Session["HospitalLocationID"])];
                        DataView dv = new DataView(dsSection);
                        dv.RowFilter = "SectionID =" + sSectionID;
                        DataTable dt = dv.ToTable();

                        if (iPrevId == Convert.ToInt32(dt.Rows[0]["TemplateId"]))
                        {
                            if (t2 == 0)
                                if (t3 == 0)//Template
                                {
                                    t3 = 1;
                                    if (dt.Rows[0]["SectionsListStyle"].ToString() == "1")
                                    { BeginList3 = "<ul>"; EndList3 = "</ul>"; }
                                    else if (dt.Rows[0]["SectionsListStyle"].ToString() == "2")
                                    { BeginList3 = "<ol>"; EndList3 = "</ol>"; }
                                }
                            if (dt.Rows[0]["SectionsBold"].ToString() != "" || dt.Rows[0]["SectionsItalic"].ToString() != "" || dt.Rows[0]["SectionsUnderline"].ToString() != "" || dt.Rows[0]["SectionsFontSize"].ToString() != "" || dt.Rows[0]["SectionsForecolor"].ToString() != "" || dt.Rows[0]["SectionsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != dt.Rows[0]["SectionName"].ToString())   //19June2010
                                {
                                    objStr.Append(BeginList3 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                }
                                BeginList3 = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != dt.Rows[0]["SectionName"].ToString())    //19June
                                {
                                    objStr.Append(dt.Rows[0]["SectionName"].ToString()); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                }
                            }
                            if (dt.Rows[0]["SectionsListStyle"].ToString() == "3")
                                objStr.Append("<br />");
                        }
                        else
                        {
                            if (t2 == 0)
                            {
                                t2 = 1;
                                if (dt.Rows[0]["SectionsListStyle"].ToString() == "1")
                                { BeginList2 = "<ul>"; EndList3 = "</ul>"; }
                                else if (dt.Rows[0]["SectionsListStyle"].ToString() == "2")
                                { BeginList2 = "<ol>"; EndList3 = "</ol>"; }
                            }
                            if (dt.Rows[0]["SectionsBold"].ToString() != "" || dt.Rows[0]["SectionsItalic"].ToString() != "" || dt.Rows[0]["SectionsUnderline"].ToString() != "" || dt.Rows[0]["SectionsFontSize"].ToString() != "" || dt.Rows[0]["SectionsForecolor"].ToString() != "" || dt.Rows[0]["SectionsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                {
                                    objStr.Append(BeginList2 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                }
                                BeginList2 = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]))// Comment ON 19June2010
                                    objStr.Append(dt.Rows[0]["SectionName"].ToString()); //Comment On 19June2010
                            }
                            if (dt.Rows[0]["SectionsListStyle"].ToString() == "3" || dt.Rows[0]["SectionsListStyle"].ToString() == "0")
                                objStr.Append("<br />");

                            iPrevId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);

                        }
                        sPSectionName = dt.Rows[0]["SectionName"].ToString();
                        sMutipleRecordsDate = objDs.Tables[1].Rows[it]["EntryDate"].ToString();
                    }
                    it++;

                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + item["FieldId"].ToString() + "'";
                    objDt = objDv.ToTable();
                    if (tf == 0)
                    {
                        tf = 1;
                        if (item["FieldsListStyle"].ToString() == "1")
                        { fBeginList = "<ul>"; fEndList = "</ul>"; }
                        else if (item["FieldsListStyle"].ToString() == "2")
                        { fBeginList = "<ol>"; fEndList = "</ol>"; }
                    }
                    if (item["FieldsBold"].ToString() != "" || item["FieldsItalic"].ToString() != "" || item["FieldsUnderline"].ToString() != "" || item["FieldsFontSize"].ToString() != "" || item["FieldsForecolor"].ToString() != "" || item["FieldsListStyle"].ToString() != "")
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sfBegin, ref sfEnd, item);
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                            {
                                objStr.Append(BeginList + sfBegin + item["FieldName"].ToString());
                                if (objStr.ToString() != "")
                                    objStr.Append(sfEnd + "</li>");
                            }
                            fBeginList = "";
                            sfBegin = "";
                        }

                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                                objStr.Append(item["FieldName"].ToString());
                        }
                    }
                    if (objDs.Tables.Count > 1)
                    {
                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + item["FieldId"].ToString() + "'";
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + item["FieldId"].ToString() + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            if (sEncodedDate == objDs.Tables[1].Rows[i]["EntryDate"].ToString())
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    string FType = dtFieldType.Rows[0]["FieldType"].ToString();
                                    if (FType == "C")
                                        FType = "C";
                                    if (FType == "C" || FType == "D" || FType == "B")
                                    {
                                        if (FType == "B")
                                        {
                                            if (objDt.Rows[i]["FieldValue"].ToString() == "1" || objDt.Rows[i]["FieldValue"].ToString() == "0")
                                            {

                                                DataView dv1 = new DataView(objDs.Tables[1]);
                                                if (objDs.Tables[1].Rows[i]["FieldValue"].ToString() == "1")

                                                    dv1.RowFilter = "TextValue='Yes'";
                                                else
                                                    dv1.RowFilter = "TextValue='No'";
                                                DataTable dt1 = dv1.ToTable();
                                                if (dt1.Rows.Count > 0)
                                                {
                                                    if (dt1.Rows[i]["MainText"].ToString().Trim() != "")
                                                    {
                                                        if (i == 0)
                                                            objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
                                                        else
                                                            objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
                                                    }
                                                    else
                                                    {
                                                        if (i == 0)
                                                            objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                                        else
                                                            objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                        objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                                    else
                                                        objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                                }
                                            }
                                            else
                                            {
                                                BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);
                                        }
                                    }
                                    else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                    {
                                        if (ViewState["iTemplateId"].ToString() != "163")
                                        {
                                            if (i == 0)
                                                objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                            else
                                                objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                        }
                                        else
                                        {
                                            if (i == 0)
                                                objStr.Append(": " + objDt.Rows[i]["TextValue"].ToString());
                                            else
                                                objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                        }
                                    }
                                    if (item["FieldsListStyle"].ToString() == "")
                                        if (ViewState["iTemplateId"].ToString() != "163")
                                        {
                                            if (FType != "C")
                                                objStr.Append("<br />");
                                        }
                                        else
                                        {
                                            if (FType != "C" && FType != "T")
                                                objStr.Append("<br />");
                                        }
                                }
                            }
                            else
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    string FType = dtFieldType.Rows[0]["FieldType"].ToString();
                                    if (FType == "C")
                                        FType = "C";
                                    if (FType == "C" || FType == "D" || FType == "B")
                                    {
                                        if (FType == "B")
                                        {
                                            if (objDt.Rows[i]["FieldValue"].ToString() == "1" || objDt.Rows[i]["FieldValue"].ToString() == "0")
                                            {

                                                DataView dv1 = new DataView(objDs.Tables[1]);
                                                if (objDs.Tables[1].Rows[i]["FieldValue"].ToString() == "1")

                                                    dv1.RowFilter = "TextValue='Yes'";
                                                else
                                                    dv1.RowFilter = "TextValue='No'";
                                                DataTable dt1 = dv1.ToTable();
                                                if (dt1.Rows.Count > 0)
                                                {
                                                    if (dt1.Rows[i]["MainText"].ToString().Trim() != "")
                                                    {
                                                        if (i == 0)
                                                            objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
                                                        else
                                                            objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
                                                    }
                                                    else
                                                    {
                                                        if (i == 0)
                                                            objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                                        else
                                                            objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                        objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                                    else
                                                        objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                                }
                                            }
                                            else
                                            {
                                                BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);
                                        }
                                    }
                                    else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                    {
                                        if (ViewState["iTemplateId"].ToString() != "163")
                                        {
                                            if (i == 0)
                                                objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                            else
                                                objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                        }
                                        else
                                        {
                                            if (i == 0)
                                                objStr.Append(": " + objDt.Rows[i]["TextValue"].ToString());
                                            else
                                                objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                        }
                                    }
                                    if (item["FieldsListStyle"].ToString() == "")
                                    {
                                        if (ViewState["iTemplateId"].ToString() != "163")
                                        {
                                            if (FType != "C")
                                                objStr.Append("<br />");
                                        }
                                        else
                                        {
                                            if (FType != "C" && FType != "T")
                                                objStr.Append("<br />");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (objStr.ToString() != "")
                objStr.Append(EndList);
            if (t2 == 1 && t3 == 1)
                objStr.Append(EndList3);
        }
        return objStr.ToString();
    }
    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int FieldDisplay)
    {
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();

                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();
                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {


                            //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                            objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                            FieldsLength = objDs.Tables[0].Rows.Count;


                            if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
                                && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
                            {
                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");

                            }

                            for (int i = 0; i < FieldsLength; i++)   // it makes table
                            {
                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                dr = objDs.Tables[0].Rows[i];
                                dvValues = new DataView(objDs.Tables[1]);
                                dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                                dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                                if (dvValues.ToTable().Rows.Count > MaxLength)
                                {
                                    MaxLength = dvValues.ToTable().Rows.Count;
                                }
                            }

                            objStr.Append("</tr>");
                            if (MaxLength == 0)
                            {
                                //objStr.Append("<tr>");
                                //for (int i = 0; i < FieldsLength; i++)
                                //{
                                //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                                //}
                                //objStr.Append("</tr></table>");
                            }
                            else
                            {
                                if (dsMain.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < MaxLength; i++)
                                    {
                                        objStr.Append("<tr>");
                                        if (common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != ""
                                            && common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != "0")
                                        {
                                            objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) + "</td>");
                                        }
                                        //else
                                        //{
                                        //     objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                        //}

                                        for (int j = 0; j < dsMain.Tables.Count; j++)
                                        {
                                            if (dsMain.Tables[j].Rows.Count > i
                                                && dsMain.Tables[j].Rows.Count > 0)
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                            }
                                            else
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                            }
                                        }
                                        objStr.Append("</tr>");
                                    }
                                    objStr.Append("</table>");
                                }
                            }
                        }
                    }
                    else
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (common.myInt(Session["Gender"]) == 1)
                        {
                            hstInput.Add("chrGenderType", "F");
                        }
                        else if (common.myInt(Session["Gender"]) == 2)
                        {
                            hstInput.Add("chrGenderType", "M");
                        }
                        else
                        {
                            hstInput.Add("chrGenderType", "M");
                        }

                        hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
                        hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
                        DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                        DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                        dvRowCaption.RowFilter = "RowCaptionId>0";
                        if (dvRowCaption.ToTable().Rows.Count > 0)
                        {
                            StringBuilder sbCation = new StringBuilder();
                            dvRowCaption.RowFilter = "RowNum>0";
                            DataTable dt = dvRowCaption.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                int column = dt.Columns.Count;
                                int ColumnCount = 0;
                                int count = 1;
                                for (int k = 1; k < (column - 5); k++)
                                {
                                    if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                        && ColumnCount == 0)
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(" + ");
                                        sbCation.Append("</td>");
                                    }
                                    else
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                        sbCation.Append("</td>");
                                        count++;
                                    }
                                    ColumnCount++;
                                }
                                sbCation.Append("</tr>");

                                DataView dvRow = new DataView(dt);
                                dvRow.RowFilter = "RowCaptionId>0";
                                DataTable dtRow = dvRow.ToTable();
                                for (int l = 1; l <= dtRow.Rows.Count; l++)
                                {
                                    sbCation.Append("<tr>");
                                    for (int i = 0; i < ColumnCount; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                        }
                                        else
                                        {
                                            if (dt.Rows[1]["Col" + i].ToString() == "D")
                                            {
                                                DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                    if (dvDrop.ToTable().Rows.Count > 0)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                        }

                                    }
                                    sbCation.Append("</tr>");
                                }
                                sbCation.Append("</table>");
                            }
                            objStr.Append(sbCation);
                        }

                    }
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;
                string FieldId = "";
                string sStaticTemplate = "";
                string sEnterBy = "";
                string sVisitDate = "";
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (common.myStr(item["FieldsListStyle"]) == "1")
                        {
                            BeginList = "<ul>"; EndList = "</ul>";
                        }
                        else if (item["FieldsListStyle"].ToString() == "2")
                        {
                            BeginList = "<ol>"; EndList = "</ol>";
                        }
                    }
                    if (common.myStr(item["FieldsBold"]) != ""
                        || common.myStr(item["FieldsItalic"]) != ""
                        || common.myStr(item["FieldsUnderline"]) != ""
                        || common.myStr(item["FieldsFontSize"]) != ""
                        || common.myStr(item["FieldsForecolor"]) != ""
                        || common.myStr(item["FieldsListStyle"]) != "")
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (FieldDisplay == 1)
                            {
                                if (Convert.ToBoolean(item["DisplayTitle"]))
                                {
                                    if (EntryType != "M")
                                    {
                                        objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                    }
                                    if (objStr.ToString() != "")
                                    {
                                        objStr.Append(sEnd + "</li>");
                                    }
                                }
                            }
                            BeginList = "";
                            sBegin = "";
                        }

                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (sStaticTemplate != "<br/><br/>")
                            {
                                if (FieldDisplay == 1)
                                {
                                    objStr.Append(common.myStr(item["FieldName"]));
                                }
                            }
                        }
                    }
                    if (objDs.Tables.Count > 1)
                    {

                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            if (EntryType == "M")
                            {
                                if (FieldDisplay == 1)
                                {
                                    objStr.Append("<br/>" + BeginList + sBegin + common.myStr(item["FieldName"]));
                                }
                            }
                            if (objDt.Rows.Count > 0)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        if (EntryType == "M")
                                                        {
                                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        }
                                                        else
                                                            objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        if (EntryType == "M")
                                                        {
                                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        }
                                                        else
                                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    if (EntryType == "M")
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            if (EntryType == "M")
                                            {
                                                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else
                                            {
                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            if (EntryType == "M")
                                            {
                                                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else

                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                else if (FType == "L")
                                {
                                    objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (ViewState["iTemplateId"].ToString() != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                }
                            }
                            sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                            sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                            //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                            //{
                            //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                            //}
                        }
                    }

                }

                if (objStr.ToString() != "")
                {
                    objStr.Append(EndList);
                }
            }
        }
        return objStr.ToString();
    }
    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        if (i == 0)
            objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
        else
        {
            if (FType != "C")
                objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
            else
            {
                if (i == 0)
                    objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                    objStr.Append(" and " + objDt.Rows[i]["TextValue"].ToString() + ".");
                else
                    objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
            }
        }
    }
    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
            }
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }
    protected void MakeDefaultFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        ArrayList aEnd = new ArrayList();
        if (item[typ + "ListStyle"].ToString() == "1")
        {
            sBegin += "<li>";
        }
        else if (item[typ + "ListStyle"].ToString() == "2")
        {
            sBegin += "<li>";
        }
        else
            if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "")
        {
            sBegin += "<span style='";
            if (item[typ + "FontSize"].ToString() != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (item[typ + "Forecolor"].ToString() != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (item[typ + "FontStyle"].ToString() != "")
            { sBegin += GetFontFamily(typ, item); }
        }
        if (item[typ + "Bold"].ToString() == "True")
        { sBegin += " font-weight: bold;"; }
        if (item[typ + "Italic"].ToString() == "True")
        { sBegin += " font-style: italic;"; }
        if (item[typ + "Underline"].ToString() == "True")
        { sBegin += " text-decoration: underline;"; }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
            sBegin += " '>";
    }
    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", Session["HospitalLocationId"].ToString());
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
                sFontSize = " font-size: " + sFontSize + ";";
        }
        return sFontSize;
    }
    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", item[typ + "FontStyle"].ToString());
        if (FontName != "")
            sBegin += " font-family: " + FontName + ";";
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", Session["HospitalLocationId"].ToString());
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }
    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "")
        {
            sBegin += "<span style='";
            if (item[typ + "FontSize"].ToString() != "")
            { sBegin += " font-size:" + item[typ + "FontSize"].ToString() + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (item[typ + "Forecolor"].ToString() != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            sBegin += " font-family: " + common.myStr(hdnFontName.Value) + " ;";
        }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
            sBegin += " '>";
    }
    string sFontSize = "";
    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        sBegin += "<span style='";
        ArrayList aEnd = new ArrayList();
        if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "")
        {
            if (item[typ + "FontSize"].ToString() != "")
            { sBegin += " font-size:11pt;"; }
            else { sBegin += getDefaultFontSize(); }
            if (item[typ + "Forecolor"].ToString() != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }

            sBegin += " font-family: " + common.myStr(hdnFontName.Value) + " ;";

            if (item[typ + "Bold"].ToString() == "True")
            { sBegin += " font-weight: bold;"; }
            if (item[typ + "Italic"].ToString() == "True")
            { sBegin += " font-style: italic;"; }
            if (item[typ + "Underline"].ToString() == "True")
            { sBegin += " text-decoration: underline;"; }
            aEnd.Add("</span>");
        }
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        sBegin += " '>";


        return sBegin;
    }
    private DataTable BindTemplateNames(int iEncounterID)
    {
        try
        {
            ObjIcm = new BaseC.ICM(sConString);
            ds = new DataSet();
            ds = ObjIcm.GetICMTemplateName(iEncounterID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlTemplates.DataSource = ds.Tables[0];
                ddlTemplates.DataTextField = "PageName";
                ddlTemplates.DataValueField = "PageID";
                ddlTemplates.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        return ds.Tables[0];
    }
    private DataTable BindReportTemplateNames(int iEncounterID, string Type)
    {
        try
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();

            HshIn.Add("@intReportId", 0);
            HshIn.Add("@intTemplateId", 0);
            HshIn.Add("@intDoctorId", common.myInt(Session["EmployeeId"]));
            //HshIn.Add("@chvFlag", "W");
            HshIn.Add("@chvFlag", Type);
            HshIn.Add("@intEncounterId", iEncounterID);
            HshIn.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));


            HshIn.Add("@bitActive", 1);
            HshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            //if (common.myStr(Request.QueryString["For"]) == "DthSum")
            //    HshIn.Add("@chvReportType", "DE");// For Death summary
            //else
            //{
            //    HshIn.Add("@chvReportType", "DI");// For Discharge Summary
            //}

            HshIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateReportSetup", HshIn, HshOut);
            DataView dv = new DataView(ds.Tables[0]);
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
            {
                dv.RowFilter = "ReportType ='DE'";
            }
            else
            {
                if (Type == "CS")
                {
                    dv.RowFilter = "ReportType IN ('CS')";
                }
                else
                {
                    dv.RowFilter = "ReportType IN ('DI','SD')";
                }
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["ReportId"] = common.myStr(ds.Tables[1].Rows[0]["FormatId"]);
                }
            }
            ddlReportFormat.Items.Clear();
            ddlReportFormat.DataSource = null;
            ddlReportFormat.SelectedValue = string.Empty;


            if (dv.ToTable().Rows.Count > 0)
            {
                //for (int i = 0; i < dv.ToTable().Rows.Count; i++)
                //{
                //    RadComboBoxItem item = new RadComboBoxItem();
                //    item.Text = common.myStr(dv.ToTable().Rows[i]["ReportName"]);
                //    item.Value = common.myStr(dv.ToTable().Rows[i]["ReportId"]) + "##" + common.myStr(common.myStr(dv.ToTable().Rows[i]["SummaryId"]));
                //    ddlReportFormat.Items.Add(item);
                //    item.DataBind();
                //}
                ddlReportFormat.DataSource = dv.ToTable();
                ddlReportFormat.DataTextField = "ReportName";
                ddlReportFormat.DataValueField = "ReportId";
                ddlReportFormat.DataBind();
            }

            //foreach (DataRow dsTitle in dv.ToTable().Rows)
            //{
            //    RadComboBoxItem item = new RadComboBoxItem();
            //    item.Text = (string)dsTitle["PageName"];
            //    item.Value = dsTitle["ReportType"].ToString();
            //    item.Attributes.Add("PageId", common.myStr(dsTitle["PageId"]));

            //    ddlReportFormat.Items.Add(item);
            //    item.DataBind();
            //}

            //ddlReportFormat.DataSource = ds.Tables[0];
            //ddlReportFormat.DataTextField = "PageName";
            //ddlReportFormat.DataValueField = "PageId";
            //ddlReportFormat.DataBind();
            if (Type != "CS")
            {
                if (common.myStr(Session["OPIP"]) == "O" || common.myStr(Session["OPIP"]) == "E")
                {
                    if (common.myStr(Request.QueryString["OPIP"]) == "")
                    {
                        ddlReportFormat.SelectedValue = "20";
                    }
                    else if (common.myStr(Request.QueryString["OPIP"]) == "I")
                    {
                        if (common.myInt(ViewState["ReportId"]) > 0)
                        {
                            ddlReportFormat.SelectedValue = common.myStr(ViewState["ReportId"]);
                        }
                        else
                        {
                            //change Palendra
                            ddlReportFormat.SelectedValue = "6";
                        }
                    }
                }
                else
                {
                    if (common.myInt(ViewState["ReportId"]) > 0)
                    {
                        ddlReportFormat.SelectedValue = common.myStr(ViewState["ReportId"]);
                    }
                    else
                    {
                        //change Palendra
                        ddlReportFormat.SelectedValue = "6";
                    }
                }
            }
            //else
            //{
            //    ddlReportFormat.SelectedIndex = 0;
            //}
            //ddlReportFormat.Enabled = false;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        return ds.Tables[0];
    }
    protected void btnCancelSummary_Click(object sender, EventArgs e)
    {
        ObjIcm = new BaseC.ICM(sConString);
        ds = new DataSet();
        try
        {
            if (common.myInt(hdnSummaryID.Value) != 0)
            {
                txtCancelRemarks.Text = string.Empty;
                DivCancelRemarks.Visible = true;
                lblMessage.Text = "Please Enter Cancellation Remarks !";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                return;
            }
            else
            {
                lblMessage.Text = "Please save Summary";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ObjIcm = null;
            ds.Dispose();
        }
    }
    private void IsValidPassword()
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;

        lblMessage.Text = "";

        hdnIsValidPassword.Value = "0";
        // hdnFrom.Value = from;
        RadWindow2.NavigateUrl = "/Pharmacy/Components/PasswordChecker.aspx";

        RadWindow2.Height = 120;
        RadWindow2.Width = 340;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow2.VisibleOnPageLoad = true;
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }
    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            if (!common.myInt(hdnIsValidPassword.Value).Equals(0))
            {
                CancelEMRDischargeDeathSummary();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void CancelEMRDischargeDeathSummary()
    {
        ObjIcm = new BaseC.ICM(sConString);
        Hashtable hshOutput = new Hashtable();
        string CancelStatus = string.Empty;
        try
        {
            CancelStatus = ObjIcm.CancelEMRDischargeOrDeathSummary(common.myInt(Session["HospitalLocationID"]),
                       common.myInt(hdnSummaryID.Value), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myInt(Session["UserId"]), common.myStr(txtCancelRemarks.Text), common.myStr(ddlReportType.SelectedValue));


            btnRefresh_OnClick(this, null);
            //BindPatientDischargeSummary();
            if (CancelStatus.Contains("Cancelled"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ddlReportType_SelectedIndexChanged(this, null);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
            lblMessage.Text = CancelStatus;
            btnFinalize.Text = "Finalized";
            hdnFinalizeDeFinalize.Value = "F";
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            ObjIcm = null;
            hshOutput = null;
            CancelStatus = string.Empty;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // IsValidPassword("S");
        if (CheckSignDoctor())
        {
            //Change Palendra
            if (common.myStr(ddlReportFormat.SelectedValue).Equals(""))
            {
                Alert.ShowAjaxMsg("Please select Report Format", Page);
                lblMessage.Text = "";
                return;
            }
            //Change Palendra
            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsValidateDischargeSummaryDate", sConString).Equals("Y"))
            {
                divDisDateConfirm.Visible = true;
            }
            else
            {
                SaveData();
            }
        }
        else
        {
            //Alert.ShowAjaxMsg("Signature Doctor Should not be same", Page);
            if (common.myStr(ddlDoctorSign.SelectedValue).Equals(string.Empty) || common.myInt(ddlDoctorSign.SelectedValue).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select the Sign Doctor", Page);
                lblMessage.Text = "";
                return;
            }
            else
            {
                lblMessage.Text = "Signature Doctor Should not be same";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

    }
    private void SaveData()
    {
        try
        {


            if (!common.myBool(ViewState["AllowEdit"]))
            {
                lblMessage.Text = "Edit limit crossed, not allow to edit !";
                return;
            }

            ObjIcm = new BaseC.ICM(sConString);
            Hashtable hshOutput = new Hashtable();
            string sFormatID = "0", sPatientSumamry = "";
            int iSignDoctorID = 0;
            int iSignJuniorDoctorID = 0;


            if (common.myLen(RTF1.Content) == 0)
            {
                Alert.ShowAjaxMsg("Discharge Summary is blank. ", Page);
                lblMessage.Text = "";
                return;
            }

            if (common.myStr(RTF1.Content).Contains("<table"))
            {

                RTF1.Content = RTF1.Content.Replace("<table", "<table border='1' ");

                //"<td style =\"border: 1px solid black;\">&nbsp;</td>"

            }

            if (common.myStr(ddlDoctorSign.SelectedValue).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select the Sign Doctor", Page);
                lblMessage.Text = "";
                return;
            }
            if (common.myStr(dtpdate.SelectedDate) == "")
            {
                Alert.ShowAjaxMsg("Please select the Date", Page);
                lblMessage.Text = "";
                return;
            }

            string Hourtime = string.Empty;
            if (RadTimeFrom.SelectedDate != null)
                Hourtime = RadTimeFrom.SelectedDate.Value.TimeOfDay.ToString();

            string DischargeDate1 = common.myStr(dtpdate.SelectedDate);
            if ((!string.IsNullOrEmpty(DischargeDate1) && !string.IsNullOrEmpty(Hourtime)) && !Hourtime.Equals("00:00:00"))
            {
                DischargeDate1 = common.myStr(Convert.ToDateTime(common.myStr((DischargeDate1.Split(' '))[0]) + " " + Hourtime));
            }
            else if ((!string.IsNullOrEmpty(DischargeDate1) && !string.IsNullOrEmpty(Hourtime)) && Hourtime.Equals("00:00:00"))
            {
                Hourtime = DateTime.Now.TimeOfDay.ToString();
                DischargeDate1 = common.myStr(Convert.ToDateTime(common.myStr((DischargeDate1.Split(' '))[0]) + " " + Hourtime));
            }
            else if (!string.IsNullOrEmpty(DischargeDate1) && string.IsNullOrEmpty(Hourtime))
            {
                Hourtime = DateTime.Now.TimeOfDay.ToString();
                DischargeDate1 = common.myStr(Convert.ToDateTime(common.myStr((DischargeDate1.Split(' '))[0]) + " " + Hourtime));
            }

            string strEncDate = common.myDate(ViewState["EncounterDate"]).ToString("yyyy/MM/dd");
            string strDisDate = common.myDate(Convert.ToDateTime(DischargeDate1)).ToString("yyyy/MM/dd");

            if (common.myDate(strEncDate) > common.myDate(strDisDate))
            {
                Alert.ShowAjaxMsg("Date cannot be less than admission date.", Page);
                lblMessage.Text = "";
                dtpdate.Focus();
                return;
            }

            //return;
            //if (common.myStr(txtsynopsis.Text ).Trim() == "")
            //{
            //    Alert.ShowAjaxMsg("Synopsis is blank. ", Page);
            //    lblMessage.Text = "";
            //    return;
            //}
            if (ddlDoctorSign.SelectedValue != "")
            {
                iSignDoctorID = Convert.ToInt32(ddlDoctorSign.SelectedValue);
            }
            else if (hdnDoctorSignID.Value != "")
            {
                iSignDoctorID = Convert.ToInt32(hdnDoctorSignID.Value);
            }

            if (ddlJuniorDoctorSign.SelectedValue != "")
            {
                iSignJuniorDoctorID = Convert.ToInt32(ddlJuniorDoctorSign.SelectedValue);
            }
            else if (hdnSignJuniorDoctorID.Value != "")
            {
                iSignJuniorDoctorID = Convert.ToInt32(hdnSignJuniorDoctorID.Value);
            }



            if (common.myStr(ddlReportFormat.SelectedValue) != "0")
            {
                sFormatID = ddlReportFormat.SelectedValue;
            }
            if (hdnSummaryID.Value != "")
            {
                sPatientSumamry = common.myStr(RTF1.Content).Replace("..", "&#46;&#46;");
                sPatientSumamry = sPatientSumamry.Replace("<br />", "<br/>");
                //sPatientSumamry = sPatientSumamry.Replace("<br/><br/><br/>", "<br/>");
                //sPatientSumamry = sPatientSumamry.Replace(@"<br/>\t\n\r<br/>", "<br/>");
                //sPatientSumamry = sPatientSumamry.Replace("<br/></span><br/>", "<br/></span>");
                //sPatientSumamry = sPatientSumamry.Replace("<br/> <br/> <br/>", "<br/>");
                sPatientSumamry = sPatientSumamry.Replace("</table><br/>", "</table>");
            }
            else
            {
                sPatientSumamry = "<div>" + common.myStr(RTF1.Content).Replace("..", "&#46;&#46;") + "</div>";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(sPatientSumamry);

            string DeathDate = string.Empty;
            string DischargeDate = string.Empty;

            if (common.myStr(Request.QueryString["For"]) == "DthSum")  // Death summary date 
            {
                DeathDate = common.myStr(dtpdate.SelectedDate);
                if (!string.IsNullOrEmpty(DeathDate) && !string.IsNullOrEmpty(Hourtime))
                    DeathDate = common.myStr(Convert.ToDateTime(common.myStr((DeathDate.Split(' '))[0]) + " " + Hourtime));
            }
            else  //Discharge summary Date
            {
                DischargeDate = common.myStr(dtpdate.SelectedDate);
                if (!string.IsNullOrEmpty(DischargeDate) && !string.IsNullOrEmpty(Hourtime))
                    DischargeDate = common.myStr(Convert.ToDateTime(common.myStr((DischargeDate.Split(' '))[0]) + " " + Hourtime));
            }

            string[] strDepartmentIds = common.GetCheckedItems(ddlDepartment).Split(',');
            StringBuilder sbXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            foreach (string Id in strDepartmentIds)
            {
                if (common.myInt(Id) > 0)
                {
                    coll.Add(common.myInt(Id));
                    sbXML.Append(common.setXmlTable(ref coll));
                }
            }

            if (chkIsMultiDepartmentCase.Checked)
            {
                //if (sbXML.ToString().Equals(string.Empty))
                //{
                //    Alert.ShowAjaxMsg("Please select department!", Page);
                //    return;
                //}
                if (common.myInt(ddlDepartmentCase.SelectedValue).Equals(0))
                {
                    Alert.ShowAjaxMsg("Please select department case!", Page);
                    return;
                }
            }
            string type = common.myStr(ddlReportType.SelectedValue) == "CS" ? "CS" : "W";
            string FormatId = sFormatID;
            if (type == "CS")
            {
                //Change Palendra
                //hdnSummaryID.Value = "0";
                if (FormatId.Contains("#"))
                {
                    FormatId = FormatId.ToString().Split('#')[1];
                }

            }
            string message = ObjIcm.SaveUpdateICMPatientSummaryNew(common.myInt(hdnSummaryID.Value), common.myInt(Session["HospitalLocationID"]),
                                         common.myStr(ViewState["RegistrationId"]), common.myStr(ViewState["EncounterId"]),
                                         common.myInt(FormatId), sb.ToString(), iSignDoctorID, common.myInt(Session["UserID"]),
                                         common.myStr(Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMMdd")), DeathDate,
                                         DischargeDate, common.myInt(Session["FacilityId"]), common.myStr(txtsynopsis.Text).Trim(),
                                         common.myStr(txtAddendum.Text).Trim(), chkIsMultiDepartmentCase.Checked,
                                         common.myInt(ddlDepartmentCase.SelectedValue), sbXML.ToString(), iSignJuniorDoctorID, -1, string.Empty,
                                         common.myInt(ddlSignThirdDoctor.SelectedValue), common.myInt(ddlSignFourthDoctor.SelectedValue), type);

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = message;  // hshOutput["@chvErrorStatus"].ToString();
            string sMsg = message;   //hshOutput["@chvErrorStatus"].ToString();
            lblMessage.Text = sMsg;
            type = string.Empty;
            FormatId = string.Empty;
            //Alert.ShowAjaxMsg(sMsg, Page);
            if ((!common.myStr(lblMessage.Text).ToUpper().Contains("INVALID")) && (!common.myStr(lblMessage.Text).ToUpper().Contains("ALREADY")))
            {
                BindPatientDischargeSummary();
                //Change Palendra
                ddlReportType_SelectedIndexChanged(this, null);
                //hdnSummaryID.Value = "999999";
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private bool CheckSignDoctor()
    {
        int a, c;
        a = common.myInt(ddlJuniorDoctorSign.SelectedValue);
        c = common.myInt(ddlDoctorSign.SelectedValue);

        if (c.Equals(a))
        {
            return false;
        }
        return true;
    }
    protected void btnExporttoWord_Click(object sender, EventArgs e)
    {
        try
        {
            RTF1.ExportToRtf();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
    protected void dtpFromDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        string soDate = FindFutureDate(dtpToDate.SelectedDate.Value.ToString());
        string sFromDate = FindFutureDate(dtpFromDate.SelectedDate.Value.ToString());
        if (Convert.ToInt32(soDate) <= Convert.ToInt32(sFromDate))
        {
            Alert.ShowAjaxMsg("To Date should be before to From Date", Page);
            return;

        }
    }
    protected void dtpToDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        string soDate = FindFutureDate(dtpToDate.SelectedDate.Value.ToString());
        string sFromDate = FindFutureDate(dtpFromDate.SelectedDate.Value.ToString());
        if (Convert.ToInt32(soDate) <= Convert.ToInt32(sFromDate))
        {
            Alert.ShowAjaxMsg("To Date should be letter to From Date", Page);
            return;

        }
        //ddlTemplates.Text = "";
        //ddlTemplates.Items.Clear();
        //ddlTemplates.DataSource = null;
        //ddlTemplates.DataBind();
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
    void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                //lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Replace(string Ttype, ref string t, string strNew, string Lock)
    {
        if (t != null)
        {
            t = t.Replace('"', '$');
            string st = "<div id=$" + Ttype + "$>";
            int RosSt = t.IndexOf(st);
            if (RosSt > 0 || RosSt == 0)
            {
                int RosEnd = t.IndexOf("</div>", RosSt);
                string str = t.Substring(RosSt, (RosEnd) - RosSt);
                string ne = t.Replace(str, strNew);
                t = ne.Replace('$', '"');
            }
            else
            {
            }
        }
        t = t.Replace('$', '"');
    }
    protected string CreateReportString(DataSet objDs, int iRootId, string iRootName, string TabularType, int NoOfBlankRows)
    {
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (common.myBool(TabularType))
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    //changes start
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    DataRow dr2;
                    foreach (DataRow dr in objDs.Tables[0].Rows)
                    {
                        dvValues.RowFilter = "FieldId = " + common.myStr(dr["FieldId"]);

                        //MaxLength = dvValues.ToTable().Rows.Count;

                        MaxLength = common.myInt(dvValues.ToTable().Compute("MAX(RowNo)", string.Empty));

                        if (MaxLength > 0)
                        {
                            dr2 = dr;
                            break;
                        }
                    }

                    if (MaxLength != 0)
                    {
                        int tableBorder = 1;

                        int TRows = 0;
                        int SectionId = 0;
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            TRows = common.myInt(objDs.Tables[0].Rows[0]["TRows"]);
                            SectionId = common.myInt(objDs.Tables[0].Rows[0]["SectionId"]);
                        }

                        if (SectionId == 4608
                            || SectionId == 4610
                            || SectionId == 4611)
                        {
                            tableBorder = 0;
                        }

                        objStr.Append("<br /><table border='" + tableBorder + "' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' ><tr align='center'>");

                        FieldsLength = objDs.Tables[0].Rows.Count;

                        #region header row - tabular with rows defination

                        DataSet dsR = new DataSet();
                        if (TRows > 0)
                        {
                            //border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;
                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + "+" + "</th>");
                        }
                        #endregion

                        for (int i = 0; i < FieldsLength; i++)   // it makes table header
                        {
                            //border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;

                            string strHeader = common.myStr(objDs.Tables[0].Rows[i]["FieldName"]);

                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + strHeader + "</th>");
                            dr2 = objDs.Tables[0].Rows[i];

                            dvValues.RowFilter = "";
                            dvValues = new DataView(objDs.Tables[1]);
                            dvValues.RowFilter = "FieldId='" + common.myStr(dr2["FieldId"]) + "'";
                            dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                            if (dvValues.ToTable().Rows.Count > MaxLength)
                            {
                                MaxLength = dvValues.ToTable().Rows.Count;
                            }
                        }

                        objStr.Append("</tr>");
                        if (MaxLength == 0)
                        {
                        }
                        else
                        {
                            for (int i = 0; i < MaxLength; i++)
                            {
                                StringBuilder sbTR = new StringBuilder();
                                bool isDataFound = false;

                                for (int j = 0; j < dsMain.Tables.Count; j++)
                                {
                                    DataView dvM = dsMain.Tables[j].DefaultView;
                                    dvM.RowFilter = "RowNo=" + (i + 1);
                                    dvM.Sort = "RowNo ASC";

                                    DataTable tbl = dvM.ToTable();

                                    if (TRows > 0 && j == 0)
                                    {
                                        if (tbl.Rows.Count > 0)
                                        {
                                            if (common.myLen(tbl.Rows[0]["RowCaption"]) > 0)
                                            {
                                                sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tbl.Rows[0]["RowCaption"]) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (dsMain.Tables.Count > (j + 1))
                                            {
                                                DataView dvM2 = dsMain.Tables[j + 1].DefaultView;
                                                dvM2.RowFilter = "RowNo=" + (i + 1);
                                                dvM2.Sort = "RowNo ASC";

                                                DataTable tblH = dvM2.ToTable();
                                                if (tblH.Rows.Count > 0)
                                                {
                                                    if (common.myLen(tblH.Rows[0]["RowCaption"]) > 0)
                                                    {
                                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tblH.Rows[0]["RowCaption"]) + "</td>");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (tbl.Rows.Count > 0)
                                    {
                                        isDataFound = true;
                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tbl.Rows[0]["TextValue"]) + "</td>");
                                    }
                                    else
                                    {
                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>&nbsp;</td>");
                                    }
                                }

                                if (isDataFound)
                                {
                                    objStr.Append("<tr valign='top'>");
                                    objStr.Append(sbTR.ToString());
                                    objStr.Append("</tr>");
                                }
                            }

                            for (int rIdx = 0; rIdx < NoOfBlankRows; rIdx++)
                            {
                                objStr.Append("<tr valign='top'>");

                                for (int cIdx = 0; cIdx < dsMain.Tables.Count; cIdx++)
                                {
                                    objStr.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "' align='right'>&nbsp;</td>");
                                }
                                objStr.Append("</tr>");
                            }

                            objStr.Append("</table>");
                            //}
                        }
                    }

                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;

                objStr.Append("<br /><br /><table border='0' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' >");

                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objStr.Append("<tr valign='top'>");

                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (common.myStr(item["FieldsListStyle"]) == "1")
                        {
                            BeginList = "<ul>"; EndList = "</ul>";
                        }
                        else if (common.myStr(item["FieldsListStyle"]) == "2")
                        {
                            BeginList = "<ol>"; EndList = "</ol>";
                        }
                    }
                    if (common.myStr(item["FieldsBold"]) != ""
                        || common.myStr(item["FieldsItalic"]) != ""
                        || common.myStr(item["FieldsUnderline"]) != ""
                        || common.myStr(item["FieldsFontSize"]) != ""
                        || common.myStr(item["FieldsForecolor"]) != ""
                        || common.myStr(item["FieldsListStyle"]) != "")
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);

                            if (sBegin.StartsWith("<br/>"))
                            {
                                if (sBegin.Length > 5)
                                {
                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                }
                            }

                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");

                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    //changes
                                    //objStr.Append(sEnd + "</li>");
                                    objStr.Append(sEnd);
                                }
                                //}

                                objStr.Append("</td>");
                            }
                            BeginList = "";
                            sBegin = "";
                        }
                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");
                                objStr.Append(common.myStr(item["FieldName"]));
                                objStr.Append("</td>");
                            }
                        }
                    }

                    if (objDs.Tables.Count > 1)
                    {
                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        objDt = objDv.ToTable();

                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");

                        if (dtFieldType.Rows.Count > 0
                            && objDt.Rows.Count == 0)
                        {
                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                            if (FType == "O")
                            {
                                int DataObjectId = common.myInt(dtFieldType.Rows[0]["DataObjectId"]);

                                clsIVF objivf = new clsIVF(sConString);

                                string strOutput = objivf.getDataObjectValue(DataObjectId);

                                if (common.myLen(strOutput) > 0)
                                {
                                    objStr.Append("<td>" + common.myStr(dtFieldType.Rows[0]["FieldName"]) + "</td>");
                                    objStr.Append("<td>" + strOutput + "</td>");
                                }
                            }
                        }

                        if (objDt.Rows.Count > 0)
                        {
                            objStr.Append("<td>");

                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }

                                }
                            }


                            objStr.Append("</td>");
                        }
                    }

                    objStr.Append("</tr>");
                }

                if (objStr.ToString() != "")
                {
                    objStr.Append(EndList);
                }

                objStr.Append("</table>");
            }
        }

        return objStr.ToString();
    }
    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        RTF1.Content = string.Empty;
        txtsynopsis.Text = string.Empty;
        txtAddendum.Text = string.Empty;
        lblMessage.Text = string.Empty;
        txtsynopsis.Text = string.Empty;
        txtCancelRemarks.Text = string.Empty;
        // ddlReportFormat.SelectedIndex = 0;
        dtpdate.SelectedDate = DateTime.Now;
        RadTimeFrom.SelectedDate = null;
        RadComboBox1.SelectedIndex = 0;
        BindPatientDischargeSummary();

    }
    protected void bindDataDischargeSummary(string iFormId, string TemplateId, StringBuilder sb, string SectionId, string FieldId, string ReportId, int SectionDisplay, int FieldDisplay, string EmrTemplateTypesCode)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }

        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = common.myStr(ds.Tables[0].Rows[0]["EntryType"]);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);
        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        else
        {
            hstInput.Add("chrGenderType", "M");
        }
        if (Request.QueryString["ER"] != null && Request.QueryString["ER"].ToString() == "ER")
        {
            hstInput.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
            hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
        }
        else
        {
            if (sEntryType == "S")
            {
                hstInput.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
            }
            else
            {
                hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
            }
        }

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        hstInput.Add("@intEREncounterId", Request.QueryString["EREncounterId"] == null ? "0" : Request.QueryString["EREncounterId"].ToString());
        if (FieldId != "")
        {
            hstInput.Add("@intFieldId", common.myInt(FieldId));
        }
        if (SectionId != "")
        {
            hstInput.Add("@intSectionID", common.myInt(SectionId));
        }
        hstInput.Add("@EmrTemplateTypesCode", common.myStr(EmrTemplateTypesCode));

        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        DataView dv = new DataView(dsAllSectionDetails.Tables[2]);


        DataTable dtEntry = dv.ToTable(true, "RecordId");
        int iRecordId = 0;

        DataTable DsSec = new DataTable();
        DataView dvsec = new DataView(ds.Tables[0]);
        dvsec.RowFilter = "SectionId=" + SectionId;
        DsSec = dvsec.ToTable();
        foreach (DataRow item in DsSec.Rows)
        {

            DataTable dtFieldValue = new DataTable();
            DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
            dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
            DataTable dtFieldName = dv1.ToTable();

            if (dsAllSectionDetails.Tables.Count > 2)
            {
                DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
                dv2.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                dtFieldValue = dv2.ToTable();
            }
            DataSet dsAllFieldsDetails = new DataSet();
            dsAllFieldsDetails.Tables.Add(dtFieldName);
            dsAllFieldsDetails.Tables.Add(dtFieldValue);
            if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
            {
                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                    {
                        string sBegin = "", sEnd = "";

                        DataRow dr3;
                        dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr3);

                        ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                        str = sBegin + CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]),
                            item["SectionId"].ToString(), common.myStr(item["EntryType"]), FieldDisplay) + sEnd;
                        str += " ";
                        //}
                        if (iPrevId == common.myInt(item["TemplateId"]))
                        {

                            if (t2 == 0)
                            {
                                if (t3 == 0)//Template
                                {
                                    t3 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    {
                                        BeginList3 = "<ul>"; EndList3 = "</ul>";
                                    }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    {
                                        BeginList3 = "<ol>"; EndList3 = "</ol>";
                                    }
                                }
                            }

                            if (common.myStr(item["SectionsBold"]) != ""
                                || common.myStr(item["SectionsItalic"]) != ""
                                || common.myStr(item["SectionsUnderline"]) != ""
                                || common.myStr(item["SectionsFontSize"]) != ""
                                || common.myStr(item["SectionsForecolor"]) != ""
                                || common.myStr(item["SectionsListStyle"]) != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (SectionDisplay == 1)
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                        }
                                    }
                                }
                                BeginList3 = "";
                            }
                            else
                            {
                                if (SectionDisplay == 1)
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                        }
                                    }
                                }
                            }

                            if (common.myStr(item["SectionsListStyle"]) == "3"
                                || common.myStr(item["TemplateListStyle"]) == "0")
                            {
                                //objStrTmp.Append("<br />");
                            }
                            else
                            {
                                if (str.Trim() != "")
                                {
                                    objStrTmp.Append(str);
                                }
                            }
                        }
                        else
                        {

                            if (t == 0)
                            {
                                t = 1;
                                if (common.myStr(item["TemplateListStyle"]) == "1")
                                {
                                    BeginList = "<ul>"; EndList = "</ul>";
                                }
                                else if (common.myStr(item["TemplateListStyle"]) == "2")
                                {
                                    BeginList = "<ol>"; EndList = "</ol>";
                                }
                            }
                            if (common.myStr(item["TemplateBold"]) != ""
                                || common.myStr(item["TemplateItalic"]) != ""
                                || common.myStr(item["TemplateUnderline"]) != ""
                                || common.myStr(item["TemplateFontSize"]) != ""
                                || common.myStr(item["TemplateForecolor"]) != ""
                                || common.myStr(item["TemplateListStyle"]) != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Template", ref sBegin, ref sEnd, item);
                                if (SectionDisplay == 1)
                                {
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        if (sBegin.Contains("<br/>") == true)
                                        {
                                            sBegin = sBegin.Remove(0, 5);
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                        else
                                        {
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                    }
                                }
                                //if (sEntryType == "M")
                                //{
                                //    objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                //}
                                BeginList = "";
                            }
                            else
                            {
                                if (SectionDisplay == 1)
                                {
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                    }
                                }
                                //if (sEntryType == "M")
                                //{
                                //    objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                //}
                            }
                            if (common.myStr(item["TemplateListStyle"]) == "3"
                                || common.myStr(item["TemplateListStyle"]) == "0")
                            {
                                // objStrTmp.Append("<br />");
                            }
                            objStrTmp.Append(EndList);
                            if (t2 == 0)
                            {
                                t2 = 1;
                                if (common.myStr(item["SectionsListStyle"]) == "1")
                                {
                                    BeginList2 = "<ul>"; EndList3 = "</ul>";
                                }
                                else if (common.myStr(item["SectionsListStyle"]) == "2")
                                {
                                    BeginList2 = "<ol>"; EndList3 = "</ol>";
                                }
                            }
                            if (common.myStr(item["SectionsBold"]) != ""
                                || common.myStr(item["SectionsItalic"]) != ""
                                || common.myStr(item["SectionsUnderline"]) != ""
                                || common.myStr(item["SectionsFontSize"]) != ""
                                || common.myStr(item["SectionsForecolor"]) != ""
                                || common.myStr(item["SectionsListStyle"]) != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (SectionDisplay == 1)
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        }
                                    }
                                }
                                BeginList2 = "";
                            }
                            else
                            {
                                if (SectionDisplay == 1)
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    {
                                        objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                    }
                                }
                            }
                            if (common.myStr(item["SectionsListStyle"]) == "3"
                                || common.myStr(item["SectionsListStyle"]) == "0")
                            {
                                //objStrTmp.Append("<br />");
                            }

                            objStrTmp.Append(str);
                        }
                        //iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                        iPrevId = common.myInt(item["TemplateId"]);
                    }
                }
            }
        }
        //}

        if (t2 == 1 && t3 == 1)
        {
            objStrTmp.Append(EndList3);
        }
        else
        {
            objStrTmp.Append(EndList);
        }

        if (GetPageProperty(iFormId) != null)
        {
            objStrSettings.Append(objStrTmp.ToString());
            sb.Append(objStrSettings.ToString());
        }
        else
        {
            sb.Append(objStrTmp.ToString());
        }
    }
    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        string sEREncounterId = "0";
        if (Request.QueryString["EREncounterId"] != null)
        {
            sEREncounterId = Request.QueryString["EREncounterId"].ToString();
        }
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        int UserId = common.myInt(Session["UserID"]);

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();
        string Fonts = "";
        sb.Append("<span style='" + Fonts + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]),
                            common.myDate(dtpFromDate.SelectedDate.Value).ToString(),
                            common.myDate(dtpToDate.SelectedDate.Value).ToString(), TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                                    common.myDate(dtpFromDate.SelectedDate.Value).ToString(),
                                    common.myDate(dtpToDate.SelectedDate.Value).ToString(), TemplateFieldId, sEREncounterId, "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                            common.myDate(dtpFromDate.SelectedDate.Value).ToString(),
                            common.myDate(dtpToDate.SelectedDate.Value).ToString(), TemplateFieldId, sEREncounterId, "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }
    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null
            && iFormId != "")
        {
            if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                Hashtable hstInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", iFormId);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }
    protected void btnLabResult_OnClick(object sender, EventArgs e)
    {

        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
            {
                //DateTime adminsiondate = common.myDate(ViewState["AdmissionDate"]);
                Timer1.Enabled = false;
                chkAutoSave.Checked = false;
                RadWindowPopup.NavigateUrl = "/ICM/PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) +
                                            "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) +
                                            "&OP_IP=I&Master=Blank" +
                                            "&From=Ward&Discharge=Summary" +
                                            "&EncNo=" + common.myStr(ViewState["EncounterNo"]) +
                                            "&EncId=" + common.myInt(Request.QueryString["EncId"]) +
                                            "&Station=Lab";

                RadWindowPopup.Height = 500;
                RadWindowPopup.Width = 1000;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.OnClientClose = "wndAddService_OnClientClose";
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                //RadWindowPopup.InitialBehavior = WindowBehaviors.Resize;
                RadWindowPopup.VisibleStatusbar = false;
            }
        }
    }
    protected void lnkBtnMedication_OnClick(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;
        try
        {
            if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
            {
                if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
                {
                    BaseC.Patient patient = new BaseC.Patient(sConString);
                    BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                    DataSet ds = new DataSet();
                    DataSet dsStatus = new DataSet();

                    ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(ViewState["RegistrationNo"]), common.myStr(ViewState["EncounterNo"]),
                                           common.myInt(Session["UserId"]), 0);

                    dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);

                    RadWindowPopup.NavigateUrl = "PatientPrescriptionHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) +
                                                "&Encno=" + common.myStr(ViewState["EncounterNo"]) +
                                                "&RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]) +
                                                "&EncId=" + common.myStr(ds.Tables[0].Rows[0]["EncounterId"]) +
                                                "&DoctorId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);

                    RadWindowPopup.Height = 600;
                    RadWindowPopup.Width = 1300;
                    RadWindowPopup.Top = 10;
                    RadWindowPopup.Left = 10;
                    RadWindowPopup.OnClientClose = "wndAddMedication_OnClientClose";
                    RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowPopup.Modal = true;
                    //RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindowPopup.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkVital_OnClick(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;
        try
        {
            if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
            {
                if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
                {
                    BaseC.Patient patient = new BaseC.Patient(sConString);
                    BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
                    DataSet ds = new DataSet();
                    DataSet dsStatus = new DataSet();

                    ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                           common.myStr(ViewState["RegistrationNo"]), common.myStr(ViewState["EncounterNo"]),
                                           common.myInt(Session["UserId"]), 0);

                    dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);

                    RadWindowPopup.NavigateUrl = "PatientVitalHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) +
                                                "&Encno=" + common.myStr(ViewState["EncounterNo"]) +
                                                "&RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]) +
                                                "&EncId=" + common.myStr(ds.Tables[0].Rows[0]["EncounterId"]) +
                                                "&DoctorId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"]) +
                                                "&From=POPUP&FromEMR=1";

                    RadWindowPopup.Height = 600;
                    RadWindowPopup.Width = 1300;
                    RadWindowPopup.Top = 10;
                    RadWindowPopup.Left = 10;
                    RadWindowPopup.OnClientClose = "wndAddVital_OnClientClose";
                    RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowPopup.Modal = true;
                    //RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
                    RadWindowPopup.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnOther_OnClick(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
            {
                //RadWindowPopup.NavigateUrl = "PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) + "&OP_IP=I&Master=Blank&From=Ward&EncNo=" + common.myStr(ViewState["EncounterNo"]) + "&Station=Other";

                RadWindowPopup.NavigateUrl = "/ICM/PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) +
                                            "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) +
                                            "&OP_IP=I&Master=Blank" +
                                            "&From=Ward&Discharge=Summary" +
                                            "&EncNo=" + common.myStr(ViewState["EncounterNo"]) +
                                            "&EncId=" + common.myInt(Request.QueryString["EncId"]) +
                                            "&Station=Other";

                RadWindowPopup.Height = 500;
                RadWindowPopup.Width = 1000;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.OnClientClose = "wndAddService_OnClientClose";
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                //RadWindowPopup.InitialBehavior = WindowBehaviors.Resize;
                RadWindowPopup.VisibleStatusbar = false;
            }
        }
    }
    protected void btnFollowUpappointment_OnClick(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
            {
                // some session value fill from back page

                //BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
                // string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]),Convert.ToInt16(Session["UserId"]));
                //Session["FollowUpDoctorId"] = DoctorId;
                //Session["DoctorId"] = DoctorId;
                // DateTime adminsiondate = common.myDate(ViewState["AdmissionDate"]);
                //RadWindowPopup.NavigateUrl = "/Appointment/AppScheduler.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&Admisiondate=" + adminsiondate + "&Master=NO&EncNo=" + common.myStr( ViewState["EncounterNo"]) + "&EncId=" + common.myInt(Request.QueryString["EncId"]);
                RadWindowPopup.NavigateUrl = "/Appointment/AppScheduler.aspx?Master=NO";
                RadWindowPopup.Height = 500;
                RadWindowPopup.Width = 1000;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.OnClientClose = "wndAddService_OnClientClose";
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                RadWindowPopup.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowPopup.VisibleStatusbar = false;
            }
        }
    }
    protected void btnRadiology_OnClick(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO" || common.myStr(Session["OPIP"]) == "I" || common.myStr(Session["OPIP"]) == "E")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != "" && common.myStr(ViewState["EncounterNo"]).Trim() != "")
            {
                //DateTime adminsiondate = common.myDate(ViewState["AdmissionDate"]);
                //RadWindowPopup.NavigateUrl = "PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) + "&OP_IP=I&Master=Blank&From=Ward&EncNo=" + common.myStr(ViewState["EncounterNo"]) + "&Station=Radiology&Discharge=1";

                RadWindowPopup.NavigateUrl = "/ICM/PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) +
                                        "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) +
                                        "&OP_IP=I&Master=Blank" +
                                        "&From=Ward&Discharge=Summary" +
                                        "&EncNo=" + common.myStr(ViewState["EncounterNo"]) +
                                        "&EncId=" + common.myInt(Request.QueryString["EncId"]) +
                                        "&Station=Radiology";

                RadWindowPopup.Height = 500;
                RadWindowPopup.Width = 1000;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.OnClientClose = "wndAddService_OnClientClose";
                RadWindowPopup.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowPopup.Modal = true;
                //RadWindowPopup.InitialBehavior = WindowBehaviors.Resize;
                RadWindowPopup.VisibleStatusbar = false;
            }
        }
    }
    protected void btnBindLabService_OnClick(object sender, EventArgs e)
    {

        string str = common.myStr(Session["ReturnLabNo"]);
        string str1 = common.myStr(Session["ReturnServiceId"]);

        ObjIcm = new BaseC.ICM(sConString);
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder sbTemp = new StringBuilder();
        ds = new DataSet();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;
        int RegId = Convert.ToInt32(ViewState["RegistrationId"]);
        int HospitalId = Convert.ToInt32(Session["HospitalLocationID"]);
        int EncounterId = Convert.ToInt32(ViewState["EncounterId"]);
        Int16 UserId = Convert.ToInt16(Session["UserID"]);
        int facilityId = common.myInt(Session["Facilityid"]);
        DL_Funs ff = new DL_Funs();
        BindDischargeSummary bnotes = new BindDischargeSummary(sConString);
        fun = new BaseC.DiagnosisDA(sConString);
        string DoctorId = fun.GetDoctorId(HospitalId, UserId);
        dsTemplateStyle = ObjIcm.GetICMTemplateStyle(Convert.ToInt32(Session["HospitalLocationID"]));
        string sFromDate = "";
        string sToDate = "";
        if (chkDateWise.Checked)
        {
            sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
            sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
        }

        bnotes.BindLabResult(EncounterId, HospitalId, facilityId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page);

        hdnReturnLab.Value = sbTemp.ToString();
        // RTF1.Content =RTF1.Content + sbTemp.ToString();

    }
    public StringBuilder BindMedicationIP(int EncounterId, int HospitalId, int RegId, StringBuilder sb,
                StringBuilder sbTemplateStyle, string MedicationType, DataRow drTemplateListStyle,
                Page pg, string pageID, string userID, string fromDate, string toDate, int FacilityId, string IndentCode)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        Hashtable hsMed = new Hashtable();
        hsMed = new Hashtable();
        DataSet ds = new DataSet();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataTable dtPriscription = new DataTable();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sbPrescribedFinal = new StringBuilder();
        BaseC.clsEMR clsemrobj = new BaseC.clsEMR(sConString);

        hsMed.Add("@inyHospitalLocationId", HospitalId);
        hsMed.Add("@intFacilityId", FacilityId);
        hsMed.Add("@intEncounterId", EncounterId);
        hsMed.Add("@chvIndentCode", IndentCode);
        //  hsMed.Add("@chvPreviousMedication", "T");
        //if (fromDate != "" && toDate != "")
        //{
        //    hsMed.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
        //    hsMed.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        //}       
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", hsMed);
        dtPriscription = ds.Tables[1];
        if (drTemplateListStyle != null)
        {
            if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
            {
                BeginList = "<ul>"; EndList = "</ul>";
            }
            else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
            {
                BeginList = "<ol>"; EndList = "</ol>";
            }
        }
        sBegin = ""; sEnd = "";
        int rowcount = 1;
        if (dtPriscription.Rows.Count > 0)
        {
            //sbPrescribedFinal.Append(sBegin + "Medicine Name - ");           
            sbPrescribedFinal.Append(sBegin);
            foreach (DataRow dr in dtPriscription.Rows)
            {
                DataView dv = new DataView(ds.Tables[1]);
                dv.RowFilter = "IndentId=" + common.myStr(dr["IndentId"]) + " And ItemId=" + common.myStr(dr["ItemId"]);
                //sbPrescribed.Append(dr["StartDate"].ToString() + " :  </br> ");
                sbPrescribed.Append(" " + common.myStr(rowcount) + ". " + dr["ItemName"].ToString() + "  </br>");
                sbPrescribed.Append("&nbsp;&nbsp;&nbsp; " + clsemrobj.GetPrescriptionDetailString(dv.ToTable()) + "  </br> ");
                sbPrescribed.Append(sEnd);
                rowcount = rowcount + 1;
            }
            sbPrescribedFinal.Append(sbPrescribed);
        }
        sb.Append(sbPrescribedFinal);
        return sb;
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            if (RadTimeFrom.SelectedDate == null)
            {
                lblMessage.Text = "Please select time.";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
            sb.Append(RadTimeFrom.SelectedDate.Value.ToString());
            sb.Remove(RadTimeFrom.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(RadTimeFrom.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
            RadTimeFrom.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            sb = null;
        }
    }

    protected void RadTimeFrom_SelectedIndexChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (RadTimeFrom.SelectedDate != null)
        {
            string min = RadTimeFrom.SelectedDate.Value.Minute.ToString();
            min = min.Length == 1 ? "0" + min : min;
            RadComboBox1.SelectedIndex = RadComboBox1.Items.IndexOf(RadComboBox1.Items.FindItemByValue(min));
        }
    }

    //protected void dtpdate_SelectedDateChanged(object sender, EventArgs e)
    //{
    //    int time;
    //    string strbactime = "BackTimeAllow";
    //    string strfutureDate = "FutureDaysAkkow";
    //    baseHs = new BaseC.HospitalSetup(sConString);
    //    ds = new DataSet();
    //    if (common.myDate(dtptransferdate.SelectedDate) < common.myDate(DateTime.Now))
    //    {
    //        string str = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), strbactime, common.myInt(Session["FacilityId"]));
    //        time = Convert.ToInt32(str);
    //        dtptransferdate.MaxDate = DateTime.Now;
    //        DateTime ctime = DateTime.Now.AddMinutes(-time);
    //        DateTime fDays = DateTime.Now.AddDays(-time);
    //        if (dtptransferdate.SelectedDate < ctime)
    //        {
    //            //   lblmsg.Text = "Invalid admission date selected";
    //            AlertForPage("Invalid admission date selected !");
    //            //  dtptransferdate.SelectedDate = ctime;
    //            dtptransferdate.SelectedDate = dtptransferdate.MaxDate;
    //        }
    //    }
    //    else if (common.myDate(dtptransferdate.SelectedDate) > common.myDate(DateTime.Now))
    //    {
    //        string str = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), strfutureDate, common.myInt(Session["FacilityId"]));
    //        time = Convert.ToInt32(str);
    //        DateTime fDays = DateTime.Now.AddDays(time);
    //        if (dtptransferdate.SelectedDate > fDays)
    //        {
    //            AlertForPage("Invalid admission date selected !");
    //            //lblmsg.Text = "Invalid admission date selected";
    //            dtptransferdate.SelectedDate = dtptransferdate.MaxDate;
    //        }

    //    }
    //}

    //added by 24 june 2015 ujjwal to capture cancel remarks for discharge and death summary start
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCancelRemarks.Text))
        {
            IsValidPassword();
            DivCancelRemarks.Visible = false;
        }
    }
    protected void btnCloseDiv_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        txtCancelRemarks.Text = string.Empty;
        DivCancelRemarks.Visible = false;
    }
    //added by 24 june 2015 ujjwal to capture cancel remarks for discharge and death summary end

    private void bindDepartmentAndHeader()
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objICM.getDepartmentListOfDoctor(common.myInt(Session["HospitalLocationID"]));

            ddlDepartment.DataSource = ds;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();

            ds = objICM.getEMRDSMultiDepartmentCase();

            ddlDepartmentCase.DataSource = ds;
            ddlDepartmentCase.DataTextField = "CaseName";
            ddlDepartmentCase.DataValueField = "CaseId";
            ddlDepartmentCase.DataBind();
            ddlDepartmentCase.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlDepartmentCase.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void chkIsMultiDepartmentCase_OnCheckedChanged(object sender, EventArgs e)
    {
        ddlDepartment.Enabled = chkIsMultiDepartmentCase.Checked;
        ddlDepartmentCase.Enabled = chkIsMultiDepartmentCase.Checked;

        ddlDepartmentCase.SelectedIndex = 0;
    }

    protected void checkDepartments(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string strDepartmentId = common.myStr(dr["DepartmentId"]);
                foreach (RadComboBoxItem chk in ddlDepartment.Items)
                {
                    if (chk.Value == strDepartmentId)
                    {
                        chk.Checked = true;
                    }
                }
            }
        }
    }


    protected void btnDeFinalisationClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            if (common.myInt(Session["IsDefinalizedSuccess"]).Equals(1))
            {

                BindPatientDischargeSummary();
                if (common.myStr(hdnFinalize.Value).Contains("1"))
                {
                    // RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
                    string script = "function f(){changeMode('" + "4" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    btnSave.Enabled = false;
                    ddlDoctorSign.Enabled = false;
                    ddlJuniorDoctorSign.Enabled = false;
                    ddlSignThirdDoctor.Enabled = false;
                    ddlSignFourthDoctor.Enabled = false;

                    ddlTemplates.Enabled = false;
                    chkDateWise.Enabled = false;
                    spell1.Enabled = false;
                    //   ddlSummaryFormat.Enabled = false;
                    btnFinalize.Text = "Definalized";
                    lblMessage.Text = "Finalized success";
                    hdnFinalizeDeFinalize.Value = "DF";
                }
                else
                {
                    RTF1.EditModes = Telerik.Web.UI.EditModes.All;
                    btnSave.Enabled = true;
                    ddlDoctorSign.Enabled = true;
                    ddlJuniorDoctorSign.Enabled = true;
                    ddlSignThirdDoctor.Enabled = true;
                    ddlSignFourthDoctor.Enabled = true;

                    ddlTemplates.Enabled = true;
                    chkDateWise.Enabled = true;
                    // ddlSummaryFormat.Enabled = true;
                    spell1.Enabled = true;
                    btnFinalize.Text = "Finalized ";
                    lblMessage.Text = "Discharge summary Definalized";
                    hdnFinalizeDeFinalize.Value = "F";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
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

    protected void btnBindMedication_OnClick(object sender, EventArgs e)
    {

        //string str = common.myStr(Session["ReturnLabNo"]);
        //string str1 = common.myStr(Session["ReturnServiceId"]);

        //ObjIcm = new BaseC.ICM(sConString);
        //StringBuilder sbTemplateStyle = new StringBuilder();
        //StringBuilder sbTemp = new StringBuilder();
        //ds = new DataSet();
        //DataSet dsTemplate = new DataSet();
        //DataSet dsTemplateStyle = new DataSet();
        //DataRow drTemplateStyle = null;
        //Hashtable hst = new Hashtable();
        //string Templinespace = "";
        //BaseC.DiagnosisDA fun;
        //int RegId = Convert.ToInt32(ViewState["RegistrationId"]);
        //int HospitalId = Convert.ToInt32(Session["HospitalLocationID"]);
        //int EncounterId = Convert.ToInt32(ViewState["EncounterId"]);
        //Int16 UserId = Convert.ToInt16(Session["UserID"]);
        //int facilityId = common.myInt(Session["Facilityid"]);
        //DL_Funs ff = new DL_Funs();
        //BindDischargeSummary bnotes = new BindDischargeSummary(sConString);
        //fun = new BaseC.DiagnosisDA(sConString);
        //string DoctorId = fun.GetDoctorId(HospitalId, UserId);
        //dsTemplateStyle = ObjIcm.GetICMTemplateStyle(Convert.ToInt32(Session["HospitalLocationID"]));
        //string sFromDate = "";
        //string sToDate = "";
        //if (chkDateWise.Checked)
        //{
        //    sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
        //    sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
        //}

        //bnotes.BindLabResult(EncounterId, HospitalId, facilityId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page);

        //hdnReturnLab.Value = sbTemp.ToString();
        // RTF1.Content =RTF1.Content + sbTemp.ToString();

    }
    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        Finalize();
        CheckDeFinalize();
    }
    public void Finalize()
    {
        ObjIcm = new BaseC.ICM(sConString);
        BaseC.Security ObjSecurity = new BaseC.Security(sConString);
        clsIVF objivf = new clsIVF(sConString);
        bool bFinal = false;
        string flgDefinalize = "IsAllowDischargeSummaryDefinalize";
        string sDoctorSignID = "";
        string sSignJuniorDoctorID = "";
        string sSignCenterDoctorID = "";
        Hashtable hshOutput = new Hashtable();
        hshOutput = ObjIcm.FindICMAdminUser(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));

        string type = common.myStr(ddlReportType.SelectedValue) == "CS" ? "CS" : "W";
        ds = new DataSet();
        ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"]),
                        common.myStr(ViewState["EncounterId"]), common.myInt(ddlReportFormat.SelectedValue), common.myInt(Session["FacilityId"]), "", type);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hdnSummaryID.Value = ds.Tables[0].Rows[0]["SummaryID"].ToString();
        }
        if (hdnDoctorSignID.Value != "0")
        {
            sDoctorSignID = hdnDoctorSignID.Value;
        }
        else
        {
            sDoctorSignID = ddlDoctorSign.SelectedValue;
        }

        //if (hdnSignLeftDoctorID.Value != "0")
        //{
        //    sSignLeftDoctorID = hdnSignLeftDoctorID.Value;
        //}
        //else
        //{
        //    sSignLeftDoctorID = ddlLeftDoctorSign.SelectedValue;
        //}

        //if (hdnSignCenterDoctorID.Value != "0")
        //{
        //    sSignCenterDoctorID = hdnSignCenterDoctorID.Value;
        //}
        //else
        //{
        //    sSignCenterDoctorID = ddlCenterDoctorSign.SelectedValue;
        //}
        if (hdnSummaryID.Value != "0")
        {
            if (Session["EmployeeType"] != null
                && (common.myStr(Session["EmployeeType"]) == "E"
                    || common.myStr(Session["EmployeeType"]) == "N"
                    || common.myStr(Session["EmployeeType"]) == "TM"
                    || common.myStr(Session["EmployeeType"]) == "D"
                    || common.myStr(Session["EmployeeType"]) == "LD"
                    || common.myStr(Session["EmployeeType"]) == "LDIR")
                )
            {
                if (hdnDoctorSignID.Value != "0" || ddlDoctorSign.SelectedValue != "")
                {
                    //if (btnFinalize.Text.Trim().Contains("Finalized (Ctrl+F2)"))
                    if (!common.myStr(btnFinalize.Text).ToUpper().Contains("DEFINALIZED"))
                    {
                        bFinal = true;
                    }

                    if (common.myStr(btnFinalize.Text).ToUpper().Contains("DEFINALIZED"))
                    {
                        ObjIcm = new BaseC.ICM(sConString);
                        bool IsAllowDischargeSummaryDeFinalisation = ObjSecurity.CheckUserRights(Convert.ToInt32(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), flgDefinalize);

                        if (!IsAllowDischargeSummaryDeFinalisation)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "You are not authorized to definalize!";
                            return;
                        }

                        RadWindow2.NavigateUrl = "~/ICM/DischargeSummaryDeFinalisationAuthentication.aspx?SummaryId=" + common.myInt(hdnSummaryID.Value) +
                                                "&FinalizeBy=" + common.myInt(sDoctorSignID) + "&Type=" + type;
                        RadWindow2.Height = 450;
                        RadWindow2.Width = 600;
                        RadWindow2.Top = 10;
                        RadWindow2.Left = 10;
                        RadWindow2.VisibleOnPageLoad = true;
                        RadWindow2.OnClientClose = "OnClientDeFinalisationClose";
                        RadWindow2.Modal = true;
                        RadWindow2.VisibleStatusbar = false;

                        return;
                    }

                    if (!common.myStr(Request.QueryString["For"]).ToUpper().Equals("DTHSUM")
                        && common.myStr(ViewState["CheckPendingIndentForDischargeSummaryOnFinalized"]).Equals("Y"))
                    {
                        bool IsAllowFinalized = objivf.getIsChkPendingIndent(common.myInt(ViewState["EncounterId"]));
                        if (!IsAllowFinalized)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Not allow for finalized because indent are pending!";
                            return;
                        }
                    }

                    //ObjIcm.UpdateICMFinalize(Convert.ToInt32(Session["HospitalLocationID"]), Convert.ToInt32(hdnSummaryID.Value), bFinal, Convert.ToInt32(sDoctorSignID), common.myInt(Session["userId"]));
                    ObjIcm.UpdateICMFinalize(Convert.ToInt32(Session["HospitalLocationID"]), Convert.ToInt32(hdnSummaryID.Value), bFinal, Convert.ToInt32(ddlDoctorSign.SelectedValue),
                        common.myInt(Session["userId"]), type);
                    BindPatientDischargeSummary();
                    type = string.Empty;
                    if (hdnFinalize.Value.Trim().Contains("1"))
                    {
                        // RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
                        string script = "function f(){changeMode('" + "4" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        //btnSave.Enabled = false;
                        dtpdate.Enabled = false;
                        ddlDoctorSign.Enabled = false;
                        ddlJuniorDoctorSign.Enabled = false;
                        ddlSignThirdDoctor.Enabled = false;
                        ddlSignFourthDoctor.Enabled = false;
                        ddlTemplates.Enabled = false;
                        chkDateWise.Enabled = false;
                        spell1.Enabled = false;
                        btnFinalize.Text = "Definalized ";
                        lblMessage.Text = "Summary finalized";
                        hdnFinalizeDeFinalize.Value = "DF";
                    }
                    else
                    {
                        RTF1.EditModes = Telerik.Web.UI.EditModes.All;
                        btnSave.Enabled = true;
                        dtpdate.Enabled = true;
                        ddlDoctorSign.Enabled = true;
                        ddlJuniorDoctorSign.Enabled = true;
                        ddlSignThirdDoctor.Enabled = true;
                        ddlSignFourthDoctor.Enabled = true;
                        ddlTemplates.Enabled = true;
                        chkDateWise.Enabled = true;
                        spell1.Enabled = true;
                        btnFinalize.Text = "Finalized";
                        lblMessage.Text = "Summary definalized";
                        hdnFinalizeDeFinalize.Value = "F";
                    }

                    if (common.myStr(btnFinalize.Text).ToUpper().Contains("DEFINALIZED"))
                    {
                        chkAutoSave.Checked = false;
                        Timer1.Enabled = false;
                    }
                    else
                    {
                        chkAutoSave.Checked = true;
                        Timer1.Enabled = true;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Please select the Sign Doctor", Page);
                    lblMessage.Text = "";
                    return;
                }
            }
            else
            {
                if (common.myStr(btnFinalize.Text).ToUpper().Contains("DEFINALIZED"))
                {
                    Alert.ShowAjaxMsg("You Are Not Authorized To Unfinalized the Discharge Summary", Page);
                }
                else
                {
                    Alert.ShowAjaxMsg("You Are Not Authorized To finalized the Discharge Summary", Page);
                }

                lblMessage.Text = "";
                return;
            }
        }
        else
        {
            Alert.ShowAjaxMsg("Please save the discharge summary and try again", Page);
            lblMessage.Text = "";
            return;
        }
        type = string.Empty;
    }

    protected void AotoSave_OnClick(object Sender, EventArgs e)
    {

        int TimerInerval = common.myInt(ViewState["TimerInerval"]);

        if (TimerInerval > 0)
        {
            Timer1.Interval = common.myInt(TimerInerval);

            if (chkAutoSave.Checked)
            {
                SaveData();
                Session["hdnSummaryID"] = hdnSummaryID.Value;
            }
            Timer1.Interval = common.myInt(TimerInerval);
        }

        //Timer1.Interval = 60000;
        //double TimerInerval = common.myDbl(Session["DischargeSummaryAutoSaveInterval"]);
        //if (TimerInerval > 0)
        //{

        //    if (Timer1.Interval == TimerInerval)
        //    {
        //        if (chkAutoSave.Checked == true)
        //        {
        //            SaveData();
        //            Session["hdnSummaryID"] = hdnSummaryID.Value;
        //        }
        //    }
        //    Timer1.Interval = common.myInt(TimerInerval);
        //}
    }

    protected void btnEnableTimer_OnClick(object sender, EventArgs e)
    {
        Timer1.Enabled = true;
        chkAutoSave.Checked = true;
    }
    protected void btnRadWinClose_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }


    //public  bool AllSame(this IEnumerable<int> list)
    //  {
    //      bool first = true;
    //      int comparand = 0;
    //      foreach (int i in list)
    //      {
    //          if (first) comparand = i;
    //          else if (i != comparand) return false;
    //          first = false;
    //      }
    //      return true;
    //  }


    //private bool CheckForSameDoctorname()
    //{
    //    //if(common.myInt( ddlCenterDoctorSign.SelectedValue) != common.myInt(ddlCenterDoctorSign.SelectedValue) != common.myInt(ddlCenterDoctorSign.SelectedValue))
    //    //{
    //    //}
    //}

    protected void btnDisDateYes_Click(object sender, EventArgs e)
    {
        divDisDateConfirm.Visible = false;
    }

    protected void btnDisDateNo_Click(object sender, EventArgs e)
    {
        divDisDateConfirm.Visible = false;
        SaveData();
    }

    protected void BtnGetRISReport_Click(object sender, EventArgs e)
    {
        RadWindow2.NavigateUrl = "/ICM/RisTextReport.aspx?IPno=" + "IPD-" + common.myStr(common.myStr(ViewState["RegistrationNo"])) + "&Fromdate=" + common.myStr("5-jan-2018") + "&ToDate=" + common.myStr("5-jan-2018") + "&AdmissionDate=" + ViewState["AdmissionDate"] + "&OpenFrom=DS";
        RadWindow2.Height = 598;
        RadWindow2.Width = 600;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.Modal = true;
        RadWindow2.OnClientClose = "wndRISdata_OnClientClose";
        RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;
    }

    protected void btnDiagnosticHistory_OnClick(object sender, EventArgs e)
    {
        try
        {

            Timer1.Enabled = false;
            chkAutoSave.Checked = false;

            RadWindow2.NavigateUrl = "/EMR/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myInt(ViewState["EncounterId"]) + "&RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&PageSource=Ward&IsFromDischargeSummary=1";
            RadWindow2.Height = 598;
            RadWindow2.Width = 900;
            RadWindow2.Top = 10;
            RadWindow2.Left = 10;
            RadWindow2.Modal = true;
            // RadWindow2.OnClientClose = "OnClientClose";
            RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow2.VisibleStatusbar = false;
        }
        catch
        {
        }
    }
    protected void btnExportToPdf_Click(object sender, EventArgs e)
    {
        StringBuilder sbtopheader = new StringBuilder();
        StringBuilder sbNextHeader;
        DataTable dtHeaderMerge = new DataTable();
        BaseC.Hospital objHospital = new BaseC.Hospital(sConString);
        BindSummary bnotes = new BindSummary(sConString);
        DataSet dsHeader = new DataSet();
        DataSet ds = new DataSet();
        StringBuilder sbTitle = new StringBuilder();

        DataSet dsReport = new DataSet();


        clsIVF objivf = new clsIVF(sConString);
        StringBuilder sbPatientSummary = new StringBuilder();

        try
        {
            ViewState["SignaturePrintOnlyInFinalization"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                            common.myInt(Session["FacilityId"]), "SignaturePrintOnlyInFinalization", sConString);

            ds = bnotes.GetPrintDischargeSummary(common.myInt(ViewState["EncounterId"]), common.myInt(HttpContext.Current.Session["HospitalLocationId"]),
                                          common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["SavedReportFormat"]),
                                          common.myInt(HttpContext.Current.Session["FacilityId"]));

            ViewState["SignDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
            ViewState["SignJuniorDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]);
            ViewState["SignSecondaryDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorId"]);

            // ViewState["PreparedById"] = common.myStr(ds.Tables[0].Rows[0]["PreparedById"]);
            ViewState["PreparedByName"] = common.myStr(ds.Tables[0].Rows[0]["PreparedByName"]);

            //ViewState["FinalizeBy"] = common.myStr(ds.Tables[0].Rows[0]["FinalizeBy"]);
            //ViewState["FinalizeByName"] = common.myStr(ds.Tables[0].Rows[0]["FinalizeByName"]);


            ViewState["SignDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignDoctorName"]);
            ViewState["SignJuniorDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorName"]);
            ViewState["SignSecondaryDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorName"]);

            ViewState["SignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]);
            ViewState["SignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]);
            ViewState["SignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]);
            ViewState["SignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]);

            ViewState["JuniorSignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine1"]);
            ViewState["JuniorSignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine2"]);
            ViewState["JuniorSignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine3"]);
            ViewState["JuniorSignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine4"]);

            ViewState["SecondarySignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine1"]);
            ViewState["SecondarySignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine2"]);
            ViewState["SecondarySignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine3"]);
            ViewState["SecondarySignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine4"]);

            //ViewState["signdesg"] = common.myStr(ds.Tables[0].Rows[0]["signdesg"]);
            //ViewState["DepartmentName"] = common.myStr(ds.Tables[0].Rows[0]["DepartmentName"]);
            // ViewState["Education"] = common.myStr(ds.Tables[0].Rows[0]["Education"]);
            // ViewState["SpecializationName"] = common.myStr(ds.Tables[0].Rows[0]["SpecializationName"]);
            //ViewState["UPIN"] = common.myStr(ds.Tables[0].Rows[0]["UPIN"]).Trim();
            ViewState["Addendum"] = common.myStr(ds.Tables[0].Rows[0]["Addendum"]).Trim();

            //ViewState["ShowPrintByInPageFooter"] = common.myBool(ds.Tables[0].Rows[0]["ShowPrintByInPageFooter"]);
            //ViewState["ShowPrintDateInPageFooter"] = common.myBool(ds.Tables[0].Rows[0]["ShowPrintDateInPageFooter"]);


            string Addendum = string.Empty;

            if (common.myLen(ViewState["Addendum"]) > 0)
            {
                Addendum = "</br> <b>ADDENDUM </b>: " + common.myStr(ViewState["Addendum"]).Trim();
            }

            if (common.myStr(Request.QueryString["For"]) == "DthSum")
            {
                if (common.myInt(ViewState["SavedReportFormat"]) > 0)
                {
                    dsHeader = objHospital.GetReportHeader("DE", common.myInt(ViewState["SavedReportFormat"]));//Death summary header
                }
                else
                {
                    dsHeader = objHospital.GetReportHeader("DE");
                }
            }
            else
            {
                if (common.myInt(ViewState["SavedReportFormat"]) > 0)
                {
                    dsHeader = objHospital.GetReportHeader("DS", common.myInt(ViewState["SavedReportFormat"]));//Discharge summary header
                }
                else
                {
                    dsHeader = objHospital.GetReportHeader("DS");//Discharge summary header
                }
            }

            //dsHeader = objHospital.GetReportHeader("DS", common.myInt(ViewState["SavedReportFormat"]));//Discharge summary header

            dtHeaderMerge = dsHeader.Tables[0];

            #region Title header
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(hdnFinalizeDeFinalize.Value) != "")
                {
                    if (!common.myStr(hdnFinalizeDeFinalize.Value).Equals(string.Empty))
                    {
                        if (common.myStr(hdnFinalizeDeFinalize.Value).Equals("F"))
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%' ><tr ><td>&nbsp;</td><td>&nbsp;</td><td valign='top'  border='0' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + " ( DRAFT ) " + "</td><td>&nbsp;</td></tr></table>");
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td><td>&nbsp;</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td><td>&nbsp;</td></tr></table>");
                    }
                }
                else
                {
                    sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td><td>&nbsp;</td></tr></table>");
                }

            }
            else
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {
                    sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Death Summary</td></tr></table>");
                }
                else
                {
                    if (common.myStr(hdnFinalizeDeFinalize.Value) != "")
                    {
                        if (!common.myStr(hdnFinalizeDeFinalize.Value).Equals(string.Empty))
                        {
                            if (common.myStr(hdnFinalizeDeFinalize.Value).Equals("F"))
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary ( Draft )</td><td>&nbsp;</td></tr></table>");
                            }
                            else
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary</td><td>&nbsp;</td></tr></table>");
                            }
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary</td><td>&nbsp;</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'  width='100%'><tr><td>&nbsp;</td><td>&nbsp;</td><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary</td><td>&nbsp;</td></tr></table>");
                    }

                    //sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:20pt;'>Discharge Summary</td></tr></table>");
                }
            }

            #endregion

            #region Header Section of Report

            if (dsHeader.Tables.Count > 0)
            {
                int RowNo = 0;
                string colspan = "";

                if (dsHeader.Tables[0].Rows.Count > 0)
                {
                    //if (common.myStr(Request.QueryString["For"]) == "DthSum" || common.myStr(Request.QueryString["For"]) == "DISSUM")
                    //{

                    #region OtherClients
                    //else
                    {
                        sb.Append("<table border='0' cellpadding='5' cellspacing='5' width='100%'>");

                        int ColumnCount = common.myInt(dsHeader.Tables[0].Compute("MAX(ColNo)", string.Empty));

                        //sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                        //sb.Append("<tr><td colspan='" + (ColumnCount * 2) + "' >" + headerRow + "</td></tr>");
                        //sb.Append("<tr><td colspan='7'>" + headerRow + "</td></tr>");

                        for (int i = 0; i < dsHeader.Tables[0].Rows.Count; i++)
                        {
                            if (RowNo == 0)
                            {
                                RowNo = common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]);
                                sb.Append("<tr align='left'>");

                                if (ColumnCount == 1)
                                {
                                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'></td></tr>");
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }

                                if (ColumnCount == 2)
                                {
                                    //i = +1;
                                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'>dsadas</td>");
                                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'>Male</td></tr>");

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        i += 1;
                                        colspan = "";
                                    }
                                    else
                                    {
                                        colspan = " colspan='3'";
                                    }

                                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td colspan='2'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }

                                if (ColumnCount == 3)
                                {
                                    i += 2;
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                        {
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    //else
                                    //{
                                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                    //}
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }
                            }
                            else if (RowNo == common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]))
                            {
                                sb.Append("<tr align='left'>");
                                if (ColumnCount == 1)
                                {
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }
                                if (ColumnCount == 2)
                                {
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        colspan = "";
                                        dsHeader.Tables[0].DefaultView.RowFilter = "";

                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            i += 1;
                                        }
                                    }
                                    else
                                    {
                                        colspan = " colspan='3'";
                                    }

                                    bool IsAddressPrint = false;
                                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;

                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        if (common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]).Equals("PADD"))
                                        {
                                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                            {
                                                IsAddressPrint = true;

                                                sb.Append("<td  colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td colspan='3'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");


                                            }
                                            else
                                            {
                                                sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                                sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                            {
                                                sb.Append("<td  colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                                sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }

                                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        if (common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]).Equals("PADD"))
                                        {
                                            sb.Append("<td                  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td   valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td   align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        if (!IsAddressPrint)
                                        {
                                            sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                            sb.Append("<td  align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td></tr>");
                                        }
                                    }
                                }
                                if (ColumnCount == 3)
                                {
                                    i += 2;

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");

                                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                        {
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            // sb.Append("<td  colspan='3' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");

                                    }
                                    //else
                                    //{
                                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                    //}
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family:" + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }
                            }
                            else
                            {
                                sb.Append("<tr align='left'>");
                                if (ColumnCount == 1)
                                {
                                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'></td></tr>");
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }
                                if (ColumnCount == 2)
                                {
                                    //i = +1;
                                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'>dsadas</td>");
                                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'>Male</td></tr>");

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        i += 1;
                                        colspan = "";
                                    }
                                    else
                                    {
                                        colspan = " colspan='3'";
                                    }
                                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td                  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td " + colspan + "  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    //else
                                    //{
                                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'>&nbsp;</td>");
                                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'>&nbsp;</td>");
                                    //}
                                }
                                if (ColumnCount == 3)
                                {
                                    i += 2;
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                        {
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    //else
                                    //{
                                    //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                    //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9.5pt;'></td>");
                                    //}
                                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                    }
                                    else
                                    {
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9.5pt;'>&nbsp;</td>");
                                    }
                                }

                            }
                            //if (i + 1 != common.myInt(dsHeader.Tables[0].Rows.Count))
                            if ((i + 1) < common.myInt(dsHeader.Tables[0].Rows.Count))
                            {
                                RowNo = common.myInt(dsHeader.Tables[0].Rows[i + 1]["Rowno"]);

                                //sb.Append("<tr><td >" + headerRow + "</td></tr>");

                            }

                            //if ((RowNo - 1) < 5)
                            //{
                            if ((RowNo - 1) < 2)
                            {
                                sbNextHeader = new StringBuilder();
                                sbNextHeader.Append(sb.ToString());
                                //sbNextHeader.Append("<tr><td colspan='7'>" + headerRow + "</td></tr>");

                                sbNextHeader.Append("</table>");
                                // sbNextHeader.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
                            }
                        }
                        sb.Append("</table>");

                        // sb.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");

                    }
                    #endregion
                }
            }

            #endregion

            sbtopheader.Append(sbTitle.ToString());

            sbtopheader.Append(sb.ToString());



            dsReport = objivf.EditReportName(common.myInt(ViewState["SavedReportFormat"]));
            if (dsReport.Tables.Count > 0)
            {
                ViewState["IsPrintDoctorSignature"] = common.myBool(dsReport.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
            }



            Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider provider = new Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider();

            RadFlowDocument document = new RadFlowDocument();
            string strHTML = string.Empty;

            try
            {
                if (!common.myStr(Request.QueryString["For"]).ToUpper().Equals("DTHSUM"))
                {
                    if (common.myLen(ds.Tables[0].Rows[0]["ExternalReferralDoctor"]) > 0)
                    {
                        strHTML = "<span style=\"font-family:arial;\"><b>External Referral Doctor</b> : " + common.myStr(ds.Tables[0].Rows[0]["ExternalReferralDoctor"]) + "</span><br/>";
                    }
                }
            }
            catch
            {
            }

            strHTML += common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]) + Addendum;

            strHTML = strHTML.Replace("border='1'", "border='0.5'");
            strHTML = strHTML.Replace("border=\"1\"", "border=\"0.5\"");

            strHTML = strHTML.Replace("border: 1px", "border: 0.5px");
            strHTML = strHTML.Replace("border:1px", "border:0.5px");


            sbPatientSummary.Append(strHTML);

            sbPatientSummary = sbPatientSummary.Replace("line-height: 19.2pt; background: white;", "");
            sbPatientSummary = sbPatientSummary.Replace("<h5 style=", "<b><style=");
            sbPatientSummary = sbPatientSummary.Replace("<h1>", "<b>");
            sbPatientSummary = sbPatientSummary.Replace("<h2>", "<b>");
            sbPatientSummary = sbPatientSummary.Replace("<h3>", "<b>");
            sbPatientSummary = sbPatientSummary.Replace("<h4>", "<b>");
            sbPatientSummary = sbPatientSummary.Replace("<h5>", "<b>");

            sbPatientSummary = sbPatientSummary.Replace("</h1>", "</b>");
            sbPatientSummary = sbPatientSummary.Replace("</h2>", "</b>");
            sbPatientSummary = sbPatientSummary.Replace("</h3>", "</b>");
            sbPatientSummary = sbPatientSummary.Replace("</h4>", "</b>");
            sbPatientSummary = sbPatientSummary.Replace("</h5>", "</b>");

            if (common.myStr(ViewState["SignaturePrintOnlyInFinalization"]).Equals("Y"))
            {
                if (common.myStr(hdnFinalizeDeFinalize.Value).Equals("DF"))
                {
                    if (common.myBool(ViewState["IsPrintDoctorSignature"]))
                    {
                        document = provider.Import(sbtopheader + "<br/> " + sbPatientSummary + getReportsSignature(true));
                        // cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(true) + sbFooterLabel.ToString();
                    }
                    else
                    {
                        document = provider.Import(sbtopheader + "<br/> " + sbPatientSummary + getReportsSignature(false));
                        //cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(false) + sbFooterLabel.ToString();
                    }
                }
                else
                {
                    document = provider.Import(sbtopheader + "<br/> " + sbPatientSummary + getReportsSignature(false));
                    //cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(false) + sbFooterLabel.ToString();
                }
            }
            else
            {
                if (common.myBool(ViewState["IsPrintDoctorSignature"]))
                {
                    document = provider.Import(sbtopheader + "<br/> " + sbPatientSummary + getReportsSignature(true));
                    //cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(true) + sbFooterLabel.ToString();
                }
                else
                {
                    document = provider.Import(sbtopheader + "<br/> " + sbPatientSummary + getReportsSignature(false));
                    //cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(false) + sbFooterLabel.ToString();
                }
            }

            Section sectionBody = document.Sections.First();

            sectionBody.PageMargins = new Telerik.Windows.Documents.Primitives.Padding(37, 137, 37, 80);
            sectionBody.PageSize = new System.Windows.Size(816, 1200);
            sectionBody.PageNumberingSettings.StartingPageNumber = 1;

            RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);

            PdfFormatProvider providerpdf = new PdfFormatProvider();
            int numberOfPages = providerpdf.ExportToFixedDocument(document).Pages.Count;

            Paragraph para = document.Sections[0].Footers.Add().Blocks.AddParagraph();
            para.TextAlignment = Telerik.Windows.Documents.Flow.Model.Styles.Alignment.Right;

            editor.MoveToParagraphStart(para);

            //editor.MoveToParagraphStart(document.Sections[0].Footers.Add().Blocks.AddParagraph());
            editor.InsertText("Page ");
            editor.InsertField("PAGE", "Page number will be evaluated on each page footer.");
            editor.InsertText(" of ");
            editor.InsertField("NUMPAGES", numberOfPages.ToString());

            byte[] renderedBytes = null;

            IFormatProvider<RadFlowDocument> formatProvider = new PdfFormatProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                formatProvider.Export(document, ms);
                renderedBytes = ms.ToArray();
            }

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=ExportedFile" + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(renderedBytes);
            Response.Flush();
            Response.End();
            //}

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sbtopheader = null;
            sbNextHeader = null;
            dtHeaderMerge.Dispose();
            objHospital = null;
            bnotes = null;
            dsHeader.Dispose();
            ds.Dispose();
            sbTitle = null;
            dsReport.Dispose();
            objivf = null;
            sbPatientSummary = null;
        }
    }
    private StringBuilder getReportsSignature(bool IsPrintDoctorSignature)
    {
        string sImage = "";
        StringBuilder sbSign = new StringBuilder();
        try
        {

            //  DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            ds = new DataSet();
            // dt = objivf.getDoctorSignatureDetails(common.myInt(Session["DoctorId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];
            if (IsPrintDoctorSignature)

            {
                string IsShowJuniorDoctorInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowJuniorDoctorInDischargeSummary", sConString);
                string IsShowSecondaryDoctorInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowSecondaryDoctorInDischargeSummary", sConString);

                //DoctorSignImage();
                //sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                //sbSign.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                //sbSign.Append("<tr>");
                if (IsShowJuniorDoctorInDischargeSummary.Equals("Y"))
                {
                    #region SignJuniorDoctorID
                    if (common.myInt(ViewState["SignJuniorDoctorID"]) > 0)
                    {
                        // sbSign.Append("<td colspan='3' valign='top'>");
                        sbSign.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;text-align:right;'  >");
                        if (common.myLen(ViewState["SignJuniorDoctorName"]) > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + hdnDoctorImage.Value.ToString() + "</td>");
                            sbSign.Append("</tr>");

                            sbSign.Append("<tr>");
                            sbSign.Append("<td align='right'>" + common.myStr(ViewState["SignJuniorDoctorName"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        if (common.myStr(ViewState["JuniorSignatureLine1"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine1"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["JuniorSignatureLine2"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine2"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["JuniorSignatureLine3"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine3"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["JuniorSignatureLine4"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine4"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        sbSign.Append("</table>");
                        sImage = common.myStr(sbSign);
                        //sbSign.Append("</td>");
                    }
                    else
                    {
                        // sbSign.Append("<td colspan='3' valign='top'></td>");
                    }
                    #endregion
                }
                else
                {
                    // sbSign.Append("<td colspan='3' valign='top'></td>");
                }

                if (IsShowSecondaryDoctorInDischargeSummary.Equals("Y"))
                {
                    #region SignSecondaryDoctorID
                    if (common.myInt(ViewState["SignSecondaryDoctorID"]) > 0)
                    {
                        //sbSign.Append("<td colspan='3' valign='top'>");
                        sbSign.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;text-align:right;'  >");
                        if (common.myLen(ViewState["SignSecondaryDoctorName"]) > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + hdnDoctorImage.Value.ToString() + "</td>");
                            sbSign.Append("</tr>");

                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SignSecondaryDoctorName"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        if (common.myStr(ViewState["SecondarySignatureLine1"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine1"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["SecondarySignatureLine2"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine2"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["SecondarySignatureLine3"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine3"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["SecondarySignatureLine4"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine4"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        sbSign.Append("</table>");
                        sImage = common.myStr(sbSign);
                        // sbSign.Append("</td>");
                    }
                    else
                    {
                        //sbSign.Append("<td colspan='3' valign='top'></td>");
                    }
                    #endregion
                }
                else
                {
                    //sbSign.Append("<td colspan='3' valign='top'></td>");
                }

                #region SignDoctorID
                if (common.myInt(ViewState["SignDoctorID"]) > 0)
                {
                    // sbSign.Append("<td colspan='5' valign='top' align='right'>");
                    sbSign.Append("<table  cellspacing='0' cellpadding='0' width='100%'  style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;text-align:right;'  >");
                    if (common.myLen(ViewState["SignDoctorName"]) > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td >" + hdnDoctorImage.Value.ToString() + "</td>");
                        sbSign.Append("</tr>");

                        sbSign.Append("<tr>");
                        sbSign.Append("<td align='right'>" + common.myStr(ViewState["SignDoctorName"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }

                    if (common.myStr(ViewState["SignatureLine1"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td >" + common.myStr(ViewState["SignatureLine1"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine2"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + common.myStr(ViewState["SignatureLine2"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine3"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td >" + common.myStr(ViewState["SignatureLine3"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine4"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td '>" + common.myStr(ViewState["SignatureLine4"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }

                    sbSign.Append("</table>");
                    sImage = common.myStr(sbSign);
                    //sbSign.Append("</td>");
                }
                #endregion
                // sbSign.Append("</tr></table>");
            }
            //else
            //{
            //    sbSign.Append("<br />");
            //    sbSign.Append("<br />");
            //    sbSign.Append("<br />");
            //}
            return sbSign;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            sbSign = new StringBuilder();
            return sbSign;
        }
    }
    public string Addvalue(string Caption, DataSet ds)
    {
        string value = string.Empty;
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                switch (common.myStr(Caption).ToUpper().Trim())
                {
                    case "PN"://Patient Name
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
                        break;
                    case "PMN"://Patient Mobile No
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientMobileNo"]);
                        break;
                    case "RN"://Registration Number
                        value = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                        break;
                    case "SD"://Sample Collected Date
                        value = common.myStr(ds.Tables[0].Rows[0]["SampleCollectedDate"]);
                        break;
                    case "PGA"://Patient Age
                        value = common.myStr(ds.Tables[0].Rows[0]["Age"]);
                        break;
                    case "EN"://Encounter Number
                        value = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                        break;
                    case "RD"://Result Date
                        value = common.myStr(ds.Tables[0].Rows[0]["ResultDate"]);
                        break;
                    case "BNW"://Ward
                        value = common.myStr(ds.Tables[0].Rows[0]["Ward"]);
                        break;
                    case "LN"://Lab No
                        value = common.myStr(ds.Tables[0].Rows[0]["LabNo"]);
                        break;
                    case "RS"://Report Status
                        value = common.myStr(ds.Tables[0].Rows[0]["ReportStatus"]);
                        break;
                    case "RB"://Referred By
                        value = common.myStr(ds.Tables[0].Rows[0]["Referred By"]);
                        break;
                    case "PM"://Nationality Name
                        value = common.myStr(ds.Tables[0].Rows[0]["Nationality_Name"]);
                        break;
                    case "CN"://Company Name                    
                    case "CMN"://Company Name
                        value = common.myStr(ds.Tables[0].Rows[0]["CompanyName"]);
                        break;
                    case "DOA"://Date Of Admission
                        if (common.myStr(Request.QueryString["For"]) == "DISSUM")
                        {
                            if (common.myStr(ds.Tables[0].Rows[0]["DOA"]).Length > 10)
                            {
                                value = common.myDate(ds.Tables[0].Rows[0]["DOA"]).ToString("dd/MM/yyyy hh:mm tt");
                            }
                            else
                            {
                                value = common.myStr(ds.Tables[0].Rows[0]["DOA"]);
                            }
                        }
                        else
                        {
                            value = common.myStr(ds.Tables[0].Rows[0]["DOA"]);
                        }
                        break;
                    case "DOD"://Date Of Discharge
                        if (common.myStr(Request.QueryString["For"]) == "DISSUM")
                        {
                            value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
                        }
                        else
                        {
                            value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
                        }
                        break;
                    case "CDN"://Consulting Doctor Name                    
                    case "EDC":
                        value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctor"]);
                        break;
                    case "CDD"://Consulting Doctor Designation                  
                    case "PDE":
                        value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDesignation"]);
                        break;
                    case "CND"://Consulting Doctor Department              
                    case "PDN":
                        value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]);
                        break;
                    case "PAG"://Patient Of Gender
                        value = common.myStr(ds.Tables[0].Rows[0]["Age/Gender"]);
                        break;
                    case "BNO"://Bed Number
                        value = common.myStr(ds.Tables[0].Rows[0]["BedNo"]);
                        break;
                    case "BCM"://Bed Category
                        value = common.myStr(ds.Tables[0].Rows[0]["BedCategoryName"]);
                        break;
                    case "PADD"://Patient Address
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientAddress"]);
                        break;
                    case "PSP"://Provider Speciality
                        value = common.myStr(ds.Tables[0].Rows[0]["SpecializationName"]);
                        break;
                    case "MN"://Mother Name
                        value = common.myStr(ds.Tables[0].Rows[0]["MotherName"]);
                        break;
                    case "FN"://Father Name
                        value = common.myStr(ds.Tables[0].Rows[0]["FatherName"]);
                        break;
                    case "SDN"://Secondary Doctor Name
                        value = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorName"]);
                        break;
                    case "DST"://Discharge Status
                        value = common.myStr(ds.Tables[0].Rows[0]["DischargeStatus"]);
                        break;
                    case "DDT"://Delivery Date Time
                        value = common.myStr(ds.Tables[0].Rows[0]["DeliveryDateTime"]);
                        break;
                    case "GN"://Guardian Name
                        value = common.myStr(ds.Tables[0].Rows[0]["GuardianName"]);
                        break;
                    case "PVT": // PatientVisit
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientVisit"]);
                        break;
                    case "MLC": // PatientVisit
                        value = common.myStr(ds.Tables[0].Rows[0]["MLC"]);
                        break;
                    case "SN": // PatientVisit
                        value = common.myStr(ds.Tables[0].Rows[0]["SponserName"]);
                        break;
                    case "PC":
                        value = common.myStr(ds.Tables[0].Rows[0]["Patientcategory"]);
                        break;
                    case "REL":
                        value = common.myStr(ds.Tables[0].Rows[0]["ReligionName"]);
                        break;
                    case "OCC":
                        value = common.myStr(ds.Tables[0].Rows[0]["OccupationName"]);
                        break;
                    case "ROR":
                        value = common.myStr(ds.Tables[0].Rows[0]["RegistrationOtherRemarks"]);
                        break;
                    case "PG":
                        value = common.myStr(ds.Tables[0].Rows[0]["Gender"]);
                        break;
                    default:
                        value = string.Empty;
                        break;
                }
            }

        }
        catch
        {
        }
        return value;
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //yogesh 19/05/2022
        string type = string.Empty;
        if (!ddlReportType.SelectedValue.Equals(""))
        {
            RTF1.Content = "";
            type = common.myStr(ddlReportType.SelectedValue) == "CS" ? "CS" : "W";
            BindReportTemplateNames(common.myInt(ViewState["EncounterId"]), type);
            ddlReportFormat_SelectedIndexChanged(this, null);

            if (ddlReportType.SelectedValue.Equals("CS") && RTF1.Content.Equals(""))
                RTF1.Content = "No data Found.";
            else if (ddlReportType.SelectedValue.Equals("DI") && RTF1.Content.Equals(""))
                RTF1.Content = "No Discharge Summary Found.";
        }
        else
        {
            RTF1.Content = "Please Select Report Type";
        }
    }

    protected void btnICDCode_Click(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        chkAutoSave.Checked = false;

        RadWindow2.NavigateUrl = "/EMR/Assessment/Diagnosis.aspx?Diag=Tx&From=POPUP";
        RadWindow2.Height = 598;
        RadWindow2.Width = 900;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.Modal = true;
        // RadWindow2.OnClientClose = "OnClientClose";
        RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;
    }

    protected void chkIslock_CheckedChanged(object sender, EventArgs e)
    {
        BaseC.ICM obj = new BaseC.ICM(sConString);
        try
        {
            if (common.myInt(ViewState["DischargeSummaryLockByUserId"]) == common.myInt(Session["UserId"]))
            {
                int i = obj.LockEmrDischargeSummary(common.myInt(Session["EncounterId"]), common.myInt(Session["UserId"]), chkIslock.Checked);
            }
            else
            {
                chkIslock.Checked = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            obj = null;
        }

    }
    protected void IsLockEmrDischargeSummary()
    {
        BaseC.ICM obj = new BaseC.ICM(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = obj.IsLockEmrDischargeSummary(common.myInt(Session["EncounterId"]));
            ViewState["IsLockDischargeSummary"] = common.myBool(0);
            ViewState["DischargeSummaryLockByUserId"] = common.myInt(Session["UserId"]);
            if (dt.Rows.Count == 0)
                return;
            if (common.myInt(dt.Rows[0]["IsLock"]) == 1 || common.myBool(dt.Rows[0]["IsLock"]) == true && common.myInt(Session["UserId"]) != common.myInt(dt.Rows[0]["UserId"]))
            {
                btnSave.Enabled = false; chkIslock.Checked = common.myBool(ViewState["IsLockDischargeSummary"]);
                chkIslock.ToolTip = " Discharge Summary is Locked by: " + dt.Rows[0]["UserName"].ToString();
                ViewState["IsLockDischargeSummary"] = common.myBool(dt.Rows[0]["IsLock"]);
                ViewState["DischargeSummaryLockByUserId"] = common.myInt(dt.Rows[0]["UserId"]);
                RTF1.Content = "";
                RTF1.Content = "<span style='font - weight: bold; font - family: arial; color: #ff0000;'>Discharge Summary is Locked by : '" + dt.Rows[0]["UserName"].ToString() + "'</span>";
                RTF1.Enabled = false;
            }
            else
                btnSave.Enabled = true;
            chkIslock.Checked = common.myBool(ViewState["IsLockDischargeSummary"]);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            obj = null;
        }
    }
}
