using System;
using System.Configuration;
using System.Data;

public partial class Include_Master_EMRBlankMaster : System.Web.UI.MasterPage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            if (!Page.IsCallback)
            {
                if (Session["StrO"] == null)
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Session Expired", false);
                }
                else if (Session["StrO"] != null && common.myStr(Request.QueryString["irtrf"]) != "" && common.myStr(Session["StrO"]) != common.myStr(Request.QueryString["irtrf"]))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Invalid URL", false);
                }
                else if (Session["StrO"] != null)
                {
                    string output = "";
                    BaseC.User usr = new BaseC.User(sConString);
                    usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["StrO"]), out output);
                    if (output.Contains("Expired") || output.Contains("Invalid"))
                    {
                        Session["UserID"] = null;
                        Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                    }
                }
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                }
            }
            if (common.myStr(Request.QueryString["irtrf"]) != ""
                && Request.QueryString["OP"] != null && common.myStr(Request.QueryString["OP"]).Split('_').Length > 10)
            {
                Session["irtrf"] = null;
                Session["IsAdminGroup"] = null;
                Session["LoginIsAdminGroup"] = null;
                Session["HospitalLocationID"] = null;
                Session["FacilityID"] = null;
                Session["GroupID"] = null;
                Session["FinancialYearId"] = null;
                Session["EntrySite"] = null;
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["ModuleId"] = null;
                Session["URLPId"] = null;


                Session["irtrf"] = Request.QueryString["irtrf"];
                Session["IsAdminGroup"] = Request.QueryString["OP"].Split('_')[0];
                Session["LoginIsAdminGroup"] = Request.QueryString["OP"].Split('_')[1];
                Session["HospitalLocationID"] = Request.QueryString["OP"].Split('_')[2];
                Session["FacilityID"] = Request.QueryString["OP"].Split('_')[3];
                Session["GroupID"] = Request.QueryString["OP"].Split('_')[4];
                Session["FinancialYearId"] = Request.QueryString["OP"].Split('_')[5];
                Session["EntrySite"] = Request.QueryString["OP"].Split('_')[6];
                Session["UserID"] = Request.QueryString["OP"].Split('_')[7];
                Session["UserName"] = Request.QueryString["OP"].Split('_')[8].Replace("%", " ");
                Session["ModuleId"] = Request.QueryString["OP"].Split('_')[9];
                Session["URLPId"] = Request.QueryString["OP"].Split('_')[10];


                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 11)
                    Session["FacilityName"] = common.myStr(Request.QueryString["OP"]).Split('_')[11].Replace("%", " ");
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 12)
                    Session["CanDownloadPatientDocument"] = common.myStr(Request.QueryString["OP"]).Split('_')[12];
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 13)
                    Session["FacilityStateId"] = common.myStr(Request.QueryString["OP"]).Split('_')[13];
            }
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "PageInit");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbkBtnSpouse.Visible = common.myBool(Session["IsIVFSpecialisation"]);

            //BaseC.EMR objEmr = new BaseC.EMR(sConString);

            if (common.myInt(Session["EncounterId"]) > 0 && (common.myInt(Session["ModuleId"]) == 3 || common.myStr(Session["ModuleName"]) == "EMR"))
            {
                imgVisitHistory.Visible = true;
                imgVitalHistory.Visible = true;
                imgDiagnosticHistory.Visible = true;
                imgXray.Visible = true;
                imgImmunization.Visible = true;
                imgGrowthChart.Visible = true;
                imgAttachment.Visible = true;
                imgOTScheduler.Visible = true;
                imgFollowUpAppointment.Visible = true;
                imgCaseSheet.Visible = true;
                imgPastClinicalNote.Visible = true;
                imgUTD.Visible = true;
                txtUptodateSearch.Visible = true;
                imgReferal.Visible = true;
                imgRefrealHistory.Visible = true;
                imgImmunization.Visible = true;

                hdnNewUploadSite.Value = common.myStr(Session["EmployeeId"]);

                if (common.myBool(Session["AllergiesAlert"])
                    || common.myStr(Session["AllergiesAlert"]).Equals("YES"))
                {
                    imgAllergyAlert.Visible = true;
                }

                if (common.myBool(Session["MedicalAlert"])
                    || common.myStr(Session["MedicalAlert"]).Equals("YES"))
                {
                    imgMedicalAlert.Visible = true;
                }
            }
        }
    }

    protected void imgUTD_OnClick(object sender, EventArgs e)
    {
        if (txtUptodateSearch.Text.Trim().Length < 3)
        {
            //Alert.ShowAjaxMsg("Please Type in SearchBox then Continue..", this);
            txtUptodateSearch.Focus();
            return;
        }

        //RadWindowForNew.NavigateUrl = "http://www.uptodate.com/contents/search?sp=0&source=USER_PREF&search=" + txtUptodateSearch.Text + "&searchType=PLAIN_TEXT";
        RadWindowForNew.NavigateUrl = "http://www.uptodate.com/contents/search?srcsys=HMGR374606&id=" + common.myStr(Session["EmployeeId"]) + "&search=" + txtUptodateSearch.Text;
        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 600;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        // RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }

    protected void lnkbtnRadWindow_Click(object sender, EventArgs e)
    {

    }

    protected void lbkBtnSpouse_OnClick(object sender, EventArgs e)
    {
        getSpouseRegistrationId();
    }
    protected void imgPastClinicalNote_OnClick(object sender, EventArgs e)
    {
        Session["SelectedCaseSheet"] = "PN";
        callPopup("/WardManagement/VisitHistory.aspx?Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"])
            + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp");
    }

    protected void getSpouseRegistrationId()
    {
        try
        {
            if (common.myInt(Session["RegistrationId"]) == 0)
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }

            //clsIVF objivf = new clsIVF(sConString);
            clsIVF objivf = new clsIVF(String.Empty);

            DataSet ds = new DataSet();

            ds = objivf.getIVFRegistrationId(common.myInt(Session["IVFId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["RegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);
                Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                Session["IVFId"] = common.myInt(ds.Tables[0].Rows[0]["IVFId"]);
                Session["IVFNo"] = common.myInt(ds.Tables[0].Rows[0]["IVFNo"]);

                Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
            }
        }
        catch
        {
        }
    }

    protected void imgVisitHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/emr/Masters/PatientHistory.aspx?MP=NO");
    }

    protected void imgVitalHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/Vitals/PreviousVitals.aspx?POPUP=StaticTemplate");
    }

    protected void imgImmunization_OnClick(object sender, EventArgs e)
    {
        callPopup("~/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP");
    }
    protected void imgGrowthChart_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/Vitals/GrowthChart.aspx?MP=NO");
    }

    protected void imgDiagnosticHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) +
                                        "&Flag=LIS&Station=All");
    }

    protected void imgFollowUpAppointment_OnClick(object sender, EventArgs e)
    {
        if (!common.myStr(Session["EncounterStatus"]).Trim().ToUpper().Contains("CLOSE"))
        {
            callPopup("/Appointment/AppScheduler.aspx?MASTER=NO");
        }
        else
        {
            Alert.ShowAjaxMsg("Encounter is Close. Not allowed Follow-up Appointment.", Page);
            return;
        }
    }
    protected void imgRadiology_OnClick(object sender, EventArgs e)
    {
        callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]));
    }
    protected void imgAttachment_OnClick(object sender, EventArgs e)
    {
        if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "UseFTPAttachDocument", sConString).Equals("Y"))
            callPopup("/EMR/AttachDocumentFTP.aspx?MASTER=No");
        else
            callPopup("/EMR/AttachDocument.aspx?MASTER=No");
    }
    protected void imgOTScheduler_OnClick(object sender, EventArgs e)
    {
        //if (!common.myStr(Session["EncounterStatus"]).Trim().ToUpper().Contains("CLOSE"))
        //{
        callPopup("/OTScheduler/OTAppointment.aspx?From=POPUP&MASTER=No");
        //}
        //else
        //{
        //    Alert.ShowAjaxMsg("Encounter is Close. Not allowed OT Schedule.", Page);
        //    return;
        //}
    }
    protected void imgXray_OnClick(object sender, EventArgs e)
    {
        callPopup("/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["encounterid"]) +
                                        "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) +
                                        "&Flag=RIS&Station=All");
    }

    protected void imgReferal_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/ReferralSlip.aspx?OP_IP=B&MASTER=NO&EncId=" + common.myStr(Session["encounterid"]) +
                                       "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS");
    }

    protected void imgRefrealHistory_OnClick(object sender, EventArgs e)
    {
        callPopup("/EMR/ReferralSlipHistory.aspx?OP_IP=B&MASTER=NO&EncId=" + common.myStr(Session["encounterid"]) +
                                       "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&Source=" + common.myStr(Session["OPIP"]) + "&Flag=RIS");
    }

    protected void imgAllergyAlert_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=A&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";
        RadWindowForNew.Width = 800;
        RadWindowForNew.Height = 400;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }

    protected void imgMedicalAlert_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=M&CF=PTA&EId=" + common.myStr(Session["encounterid"]) + "&PId=" + common.myStr(Session["RegistrationID"]) + "&PN=" + common.myStr(Session["PatientName"]) + "&PNo=" + common.myStr(Session["RegistrationNo"]) + "&PAG=" + common.myStr(Session["AgeGender"]) + "";
        RadWindowForNew.Width = 800;
        RadWindowForNew.Height = 400;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        // RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }

    protected void imgCaseSheet_OnClick(object sender, EventArgs e)
    {
        callPopup("/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myInt(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"])
                                            + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
    }


    private void callPopup(string url)
    {
        try
        {
            if (common.myInt(Session["RegistrationID"]) == 0)
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }
            if (common.myInt(Session["encounterid"]) == 0)
            {
                Alert.ShowAjaxMsg("Patient not selected !", this.Page);
                return;
            }

            RadWindowForNew.NavigateUrl = url;

            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            //RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch
        {
        }
    }

}
