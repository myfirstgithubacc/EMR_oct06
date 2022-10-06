using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;
using System.Configuration;

public partial class ICM_HealthCheckUpMedicalSummary : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private int iPrevId = 0;
    string sFontSize = string.Empty;
    DataSet ds = new DataSet();
    BaseC.ICM ObjIcm;


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
        objException = new clsExceptionLog();

        if (!IsPostBack)
        {
            try
            {

                hdnFontName.Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "DischargeSummaryFont", sConString);
                if (common.myStr(hdnFontName.Value).Equals(string.Empty))
                {
                    hdnFontName.Value = "Candara";
                }

                string FlaglnkChangeDoctorName = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), "ChangeDoctorCodeInMHC", sConString);

                hndFlaglnkChangeDoctorName.Value = FlaglnkChangeDoctorName;


                string FlagMHCReportFormatId = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
              common.myInt(Session["FacilityId"]), "IsMHCFormatServiceWise", sConString);

                if (FlaglnkChangeDoctorName.Equals("Y"))
                {
                    lnkChangeDoctorName.Visible = true;

                }
                else if (FlaglnkChangeDoctorName.Equals("N"))
                {
                    lnkChangeDoctorName.Visible = false;

                }


                //else if (FlagMHCReportFormatId.Equals("N"))
                //{
                //    if (ddlReportFormat.Visible)
                //    {
                //        if (Session["MHCReportFormatId"] != null)
                //        {
                //            if(common.myStr(Session["MHCReportFormatId"])!=string.Empty )
                //            {
                //            ddlReportFormat.SelectedValue = common.myStr(Session["MHCReportFormatId"]);
                //            }
                //        }
                //    }

                //}




                FillSessionAndQuesryStirnValue();
                if (common.myInt(Session["UserId"]) == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                RTF1.FontNames.Clear();
                RTF1.FontNames.Add(common.myStr(hdnFontName.Value));
                
                RTF1.RealFontSizes.Clear();
                RTF1.RealFontSizes.Add("10pt");

                RTF1.StripFormattingOptions = Telerik.Web.UI.EditorStripFormattingOptions.MSWordRemoveAll | Telerik.Web.UI.EditorStripFormattingOptions.ConvertWordLists;

                dtpdate.SelectedDate = DateTime.Now;
                //BindTemplateData();
                //dtpFromDate.SelectedDate = DateTime.Now;
                //dtpToDate.SelectedDate = DateTime.Now;
                BindPatientHiddenDetails();
                BindSignDoctor();
                if (common.myStr(ViewState["EncounterId"]) != string.Empty)
                {
                    BindReportTemplateNames(common.myInt(ViewState["EncounterId"]));
                }
                BindPatientDischargeSummary();

                if (common.myStr(Request.QueryString["HC"]) == "HC")
                {
                    Page.Title = "Health Check Up Summary";
                    lbltitle.Text = "Health Check Up Summary";
                }
                if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO")
                {
                    btnFinalize.Visible = false;
                    btnSave.Visible = false;
                }
                else
                {
                    btnClose.Visible = false;
                }
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                {
                    btnFinalize.Visible = false;
                    btnSave.Visible = false;
                    btnCancelSummary.Visible = false;
                }
                hdnReportId.Value = common.myStr(ddlReportFormat.SelectedValue);

                if (FlagMHCReportFormatId.Equals("Y"))
                {
                    if (ddlReportFormat.Visible)
                    {
                        if (Session["MHCReportFormatId"] != null)
                        {
                            if (common.myStr(Session["MHCReportFormatId"]) != string.Empty)
                            {
                                ddlReportFormat.SelectedIndex = ddlReportFormat.Items.IndexOf(ddlReportFormat.Items.FindItemByValue(common.myStr(Session["MHCReportFormatId"])));
                                //ddlReportFormat.SelectedValue = common.myStr(Session["MHCReportFormatId"]);
                                ddlReportFormat_SelectedIndexChanged(null, null);
                                ddlReportFormat.Enabled = false;
                            }
                        }
                    }

                }
                CheckIsCheckListRequired();
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
            finally
            {
            }
        }

        if (common.myBool(rdoIsReviewRequired.SelectedValue))
        {
            lblReviewRemarks.Style["display"] = "";
            txtReviewRemarks.Style["display"] = "";
        }
        else
        {
            lblReviewRemarks.Style["display"] = "none";
            txtReviewRemarks.Style["display"] = "none";
        }
    }

    private void CheckIsCheckListRequired()
    {
        try
        {  
            if (common.myBool(ddlReportFormat.SelectedItem.Attributes["IsCheckListRequired"]))
            {
                lnkCheckLists.Visible = true;
            }
            else
            {
                lnkCheckLists.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }




    private void FillSessionAndQuesryStirnValue()
    {
        DataSet ds = new DataSet();
        BaseC.ATD Objstatus = new BaseC.ATD(sConString);

        try
        {
            if (common.myStr(Request.QueryString["RegNo"]) != string.Empty)
                ViewState["RegistrationNo"] = common.myStr(Request.QueryString["RegNo"]);
            else
                ViewState["RegistrationNo"] = common.myStr(Session["RegistrationNo"]);

            if (common.myStr(Request.QueryString["RegId"]) != string.Empty)
                ViewState["RegistrationId"] = common.myStr(Request.QueryString["RegId"]);
            else
                ViewState["RegistrationId"] = common.myStr(Session["RegistrationId"]);

            if (common.myStr(Request.QueryString["EncId"]) != string.Empty)
                ViewState["EncounterId"] = common.myStr(Request.QueryString["EncId"]);
            else
                ViewState["EncounterId"] = common.myStr(Session["EncounterId"]);


            if (common.myStr(Request.QueryString["EncNo"]) != string.Empty)
                ViewState["EncounterNo"] = common.myStr(Request.QueryString["EncNo"]);
            else
                ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);


            ds = Objstatus.GetRegistrationId(common.myLong(ViewState["RegistrationNo"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Session["OPIP"] = "I";
                //Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                //Session["RegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["id"]);
                //DateTime adminsiondate = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
                Session["FollowUpDoctorId"] = common.myInt(ds.Tables[0].Rows[0]["DoctorId"]);
                Session["FollowUpRegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["id"]);
                ViewState["AdmDId"] = common.myInt(ds.Tables[0].Rows[0]["DoctorId"]);
                ViewState["AdmissionDate"] = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
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
            Objstatus = null;
        }
    }

    private void BindPatientDischargeSummary()
    {
        DataSet ds = new DataSet();
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        try
        {

            //ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"])
            //  , common.myStr(ViewState["EncounterId"]), common.myInt(ddlReportFormat.SelectedValue), common.myInt(Session["FacilityId"]));

            Session["HealthCheckUpCheckListPrintAdviceRegistrationId"] = ViewState["RegistrationId"];
            Session["HealthCheckUpCheckListPrintAdviceEncounterId"] = ViewState["EncounterId"];

            if (Request.QueryString["HC"] != null)
            {
                if (common.myStr(Request.QueryString["HC"]) == "HC")
                {

                    ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"])
                        , common.myStr(ViewState["EncounterId"]), 0, common.myInt(Session["FacilityId"]), "HC");
                }
                else
                {
                    ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"])
                      , common.myStr(ViewState["EncounterId"]), common.myInt(ddlReportFormat.SelectedValue), common.myInt(Session["FacilityId"]));
                }
            }
            else
            {
                ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"])
                  , common.myStr(ViewState["EncounterId"]), common.myInt(ddlReportFormat.SelectedValue), common.myInt(Session["FacilityId"]));
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                dtpdate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DOD"]);
                RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                hdnTemplateData.Value = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                hdnSummaryID.Value = common.myInt(ds.Tables[0].Rows[0]["SummaryID"]).ToString();
                hdnDoctorSignID.Value = common.myInt(ds.Tables[0].Rows[0]["SignDoctorID"]).ToString();
                hdnFinalize.Value = common.myStr(ds.Tables[0].Rows[0]["Finalize"]);
                hdnEncodedBy.Value = common.myInt(ds.Tables[0].Rows[0]["EncodedBy"]).ToString();
                lblPreparedBy.Text = common.myStr(ds.Tables[0].Rows[0]["PreparedByName"]).Trim();


                ddlReportFormat.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).ToString();
                ddlReportFormat.Enabled = false;
                divFormat.Visible = false;
                btnRefresh.Visible = false;

                divFormatLabel.Visible = true;
                lblReportFormat.Text = string.Empty;
                lblReportFormat.Text = common.myStr(ds.Tables[0].Rows[0]["ReportName"]);


                if (common.myInt(ds.Tables[0].Rows[0]["SignDoctorID"]) > 0)
                {
                    ddlDoctorSign.SelectedIndex = ddlDoctorSign.Items.IndexOf(ddlDoctorSign.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["SignDoctorID"]).ToString()));
                }

                if (common.myBool(hdnFinalize.Value) && common.myInt(hdnDoctorSignID.Value) > 0)
                {
                    RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
                    btnSave.Enabled = false;
                    ddlDoctorSign.Enabled = false;
                    ddlReportFormat.Enabled = false;
                    //ddlTemplates.Enabled = false;
                    //chkDateWise.Enabled = false;
                    //spell1.Enabled = false;
                    btnFinalize.Text = "Definalized";
                }
            }
            else
            {
                dtpdate.SelectedDate = DateTime.Now;
                lblPreparedBy.Text = string.Empty;
                hdnSummaryID.Value = "0";
                ddlReportFormat_SelectedIndexChanged(null, null);

                ddlReportFormat.Enabled = true;
                divFormat.Visible = true;
                btnRefresh.Visible = true;

                divFormatLabel.Visible = false;

            }
            CheckDeFinalize();
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
            ObjIcm = null;
        }
    }

    private void BindSignDoctor()
    {
        DataSet ds = new DataSet();
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        try
        {
            ds = ObjIcm.GetICMSignDoctors(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDoctorSign.DataSource = ds.Tables[0];
                ddlDoctorSign.DataTextField = "DoctorName";
                ddlDoctorSign.DataValueField = "ID";
                ddlDoctorSign.DataBind();
            }

            if (common.myStr(ViewState["AdmDId"]) != string.Empty)
            {
                ddlDoctorSign.SelectedIndex = ddlDoctorSign.Items.IndexOf(ddlDoctorSign.Items.FindItemByValue(common.myInt(ViewState["AdmDId"]).ToString()));
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
            ObjIcm = null;
        }
    }

    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        FinalizeSummary();
        CheckDeFinalize();
    }

    public void FinalizeSummary()
    {
        DataSet ds = new DataSet();
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);

        bool bFinal = false;
        string sDoctorSignID = string.Empty;
        Hashtable hshOutput = new Hashtable();
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        bool IsValidate = false;

        try
        {
            if (CheckFinalisation())
            {

                hshOutput = ObjIcm.FindICMAdminUser(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));

                ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"]),
                    common.myStr(ViewState["EncounterId"]), common.myInt(ddlReportFormat.SelectedValue), common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnSummaryID.Value = common.myInt(ds.Tables[0].Rows[0]["SummaryID"]).ToString();
                }
                if (common.myInt(hdnDoctorSignID.Value) > 0)
                {
                    sDoctorSignID = hdnDoctorSignID.Value;
                }
                else
                {
                    sDoctorSignID = ddlDoctorSign.SelectedValue;
                }
                if (hdnSummaryID.Value != "0")
                {
                    if (common.myStr(btnFinalize.Text).Equals("Finalized"))
                    {
                        IsValidate = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizeToFinalizeMHCSummary");
                    }
                    else if (common.myStr(btnFinalize.Text).Equals("Definalized"))
                    {
                        IsValidate = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizeToDeFinalizeMHCSummary");
                    }
                    if (IsValidate)
                    {

                        if (btnFinalize.Text.Trim() == "Finalized")
                        {
                            ObjIcm.UpdateStatusReviewed(common.myInt(Session["EncounterId"]), "F");
                        }
                        else
                        {
                            ObjIcm.UpdateStatusReviewed(common.myInt(Session["EncounterId"]), "D");
                        }

                        if (Session["EmployeeType"] != null && (common.myStr(Session["EmployeeType"]) == "D"
                             || common.myStr(Session["EmployeeType"]) == "LD" || common.myStr(Session["EmployeeType"]) == "LDIR"))
                        {
                            if (hdnDoctorSignID.Value != "0" || ddlDoctorSign.SelectedValue != string.Empty)
                            {
                                if (btnFinalize.Text.Trim() == "Finalized")
                                {
                                    bFinal = true;
                                }

                                //ObjIcm.UpdateICMFinalize(Convert.ToInt32(Session["HospitalLocationID"]), Convert.ToInt32(hdnSummaryID.Value), bFinal, Convert.ToInt32(sDoctorSignID), common.myInt(Session["userId"]));
                                string ErrorStatus = ObjIcm.UpdateICMFinalize(common.myInt(Session["HospitalLocationID"]), common.myInt(hdnSummaryID.Value), bFinal, common.myInt(ddlDoctorSign.SelectedValue), common.myInt(Session["userId"]));
                                BindPatientDischargeSummary();
                                if (common.myBool(hdnFinalize.Value.Trim()))
                                {
                                    RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
                                    btnSave.Enabled = false;
                                    ddlDoctorSign.Enabled = false;
                                    ddlDoctorSign.Enabled = false;
                                    //ddlTemplates.Enabled = false;
                                    //chkDateWise.Enabled = false;
                                    //spell1.Enabled = false;
                                    //ddlSummaryFormat.Enabled = false;
                                    btnFinalize.Text = "Definalized";
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                                    lblMessage.Text = ErrorStatus;
                                }
                                else if (common.myStr(ErrorStatus).ToUpper().Contains("SUMMARY CANNOT BE FINALIZED"))
                                {
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                                    lblMessage.Text = ErrorStatus;
                                    return;
                                }
                                else if (common.myStr(ErrorStatus).ToUpper().Contains("INSUFFICIENT DETAILS"))
                                {
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                                    lblMessage.Text = ErrorStatus;
                                    return;
                                }
                                else
                                {
                                    RTF1.EditModes = Telerik.Web.UI.EditModes.All;
                                    btnSave.Enabled = true;
                                    ddlDoctorSign.Enabled = true;
                                    ddlDoctorSign.Enabled = true;
                                    //ddlTemplates.Enabled = true;
                                    //chkDateWise.Enabled = true;
                                    // ddlSummaryFormat.Enabled = true;
                                    //spell1.Enabled = true;
                                    btnFinalize.Text = "Finalized";
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                                    lblMessage.Text = "Definalized success";
                                }

                            }
                            else
                            {
                                Alert.ShowAjaxMsg("Please select the Sign Doctor", Page);
                                lblMessage.Text = string.Empty;
                                return;
                            }
                        }
                        else
                        {
                            //if (btnFinalize.Text.Trim() == "Finalized")
                            //    Alert.ShowAjaxMsg("You are not authorized to finalize the summary", Page);
                            //else
                            //    Alert.ShowAjaxMsg("You are not authorized to definalize the summary", Page);

                            //lblMessage.Text = string.Empty;
                            //return;
                            string flgDefinalize = "IsAllowHealthCheckSummaryDefinalize";
                            BaseC.Security ObjSecurity = new BaseC.Security(sConString);
                            ObjIcm = new BaseC.ICM(sConString);
                            bool IsAllowDischargeSummaryDeFinalisation = ObjSecurity.CheckUserRights(Convert.ToInt32(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), flgDefinalize);

                            if (!IsAllowDischargeSummaryDeFinalisation)
                            {
                                if (btnFinalize.Text.Trim() == "Finalized")
                                    Alert.ShowAjaxMsg("You are not authorized to finalize the summary", Page);
                                else
                                    Alert.ShowAjaxMsg("You are not authorized to definalize the summary", Page);

                                lblMessage.Text = string.Empty;
                                return;

                            }
                        }

                    }
                    /////
                    else
                    {
                        //Alert.ShowAjaxMsg("Not Authorized To "+ common.myStr(btnFinalize.Text)+" MHC!", Page);
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Not Authorized To " + common.myStr(btnFinalize.Text) + " " + Resources.PRegistration.MHC + "!";
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Please save the summary and try again", Page);
                    lblMessage.Text = string.Empty;
                    return;
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Summary cannot be finalize as few activities are pending.";
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
            ObjIcm = null;
            hshOutput = null;
        }
    }

    private void CheckDeFinalize()
    {
        if (btnFinalize.Text == "Definalized")
        {
            lnkChangeDoctorName.Visible = false;
            btnSave.Visible = false;
            btnCancelSummary.Visible = false;
        }
        else
        {
            if (common.myStr(hndFlaglnkChangeDoctorName.Value).Equals("Y"))
            {
                lnkChangeDoctorName.Visible = true;

            }
            else if (common.myStr(hndFlaglnkChangeDoctorName.Value).Equals("N"))
            {
                lnkChangeDoctorName.Visible = false;

            }
            btnSave.Visible = true;
            btnCancelSummary.Visible = true;
        }
    }

    protected bool CheckFinalisation()
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        ds = emr.getHealthCheckUpCheckLIsts(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), common.myInt(ddlReportFormat.SelectedValue), common.myInt(Session["DoctorId"]));


        if (btnFinalize.Text == "Definalized")
        {
            return true;
        }

        if (ds.Tables.Count > 0)
        {
            #region Finalize MHC
            //if (ds.Tables[4].Rows.Count > 0)
            //{
            //    if (common.myStr(ds.Tables[4].Rows[0][0]) != string.Empty)
            //    {
            //        //if (common.myStr(ds.Tables[4].Rows[0][0]).Equals("Not Authorized To Finalize MHC"))
            //        //{
            //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //        lblMessage.Text = "Not Authorized To Finalize MHC!";
            //        hdnMHCFinalize.Value  = "1";
            //        return false;
            //        //}
            //    }
            //}
            #endregion

            if (ds.Tables[3].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[3].Rows.Count - 1; i++)
                {
                    if (common.myInt(ds.Tables[3].Rows[i]["IsData"]) == 0 && lnkCheckLists.Visible)
                    {

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Summary cannot be finalize as few activities are pending.";
                        return false;
                    }

                }


            }
        }
        return true;

    }
    //protected bool IsAuthorizeToFinalizeMHCSummary(string Status)
    //{
    //    BaseC.Hospital objHospital = new BaseC.Hospital(sConString);
    //    DataSet ds = new DataSet();
    //    if (Status.Equals("Finalized"))
    //    {
    //        ds = objHospital.GetSpecialRightsEmployeeList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]), "IsAuthorizeToFinalizeMHCSummary");
    //        if (ds != null)
    //        {
    //            if (ds.Tables.Count > 0)
    //            {
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    return true;
    //                }
    //                else
    //                { return false; }
    //            }
    //        }
    //    }
    //    else if (Status.Equals("Definalized"))
    //    {
    //        ds = objHospital.GetSpecialRightsEmployeeList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]), "IsAuthorizeToDeFinalizeMHCSummary");
    //        if (ds != null)
    //        {
    //            if (ds.Tables.Count > 0)
    //            {
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    return true;
    //                }
    //                else
    //                { return false; }
    //            }
    //        }
    //    }
    //    return false;
    //}

    protected void chkDateWise_OnCheckedChanged(object sender, EventArgs e)
    {
        //if (chkDateWise.Checked)
        //{
        //    dtpFromDate.Enabled = true;
        //    dtpToDate.Enabled = true;
        //}
        //else
        //{
        //    dtpFromDate.Enabled = false;
        //    dtpToDate.Enabled = false;
        //}
    }

    protected void ddlReportFormat_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            CheckIsCheckListRequired();
            RTF1.Content = string.Empty;
            int sValue = common.myInt(ddlReportFormat.SelectedValue);
            hdnReportId.Value = common.myStr(sValue);
            if (common.myInt(sValue) > 0)
            {
                if (common.myBool(ddlReportFormat.SelectedItem.Attributes["IsShowFilledTemplates"]))
                {
                    hdnTemplateData.Value = PrintReport(false);
                }
                else
                {
                    hdnTemplateData.Value = BindDischargeSummary();
                }
                RTF1.Content = hdnTemplateData.Value;
            }
            else
            {
                return;
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
        }
    }

    private string BindDischargeSummary()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataTable dtTemplate = new DataTable();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        string Templinespace = string.Empty;
        string fdate = string.Empty;
        string tdate = string.Empty;
        BindSummary bnotes = new BindSummary(sConString);
        clsIVF note = new clsIVF(sConString);

        try
        {
            //if (chkDateWise.Checked)
            //{
            //    fdate = common.myDate(dtpFromDate.SelectedDate.Value).ToString();
            //    tdate = common.myDate(dtpToDate.SelectedDate.Value).ToString();
            //}

            int RegId = common.myInt(ViewState["RegistrationId"]);
            int HospitalId = common.myInt(Session["HospitalLocationID"]);
            int EncounterId = common.myInt(ViewState["EncounterId"]);
            //int UserId = common.myInt(Session["UserID"]);
            int UserId = common.myInt(Session["UserID"]);

            //fun = new BaseC.DiagnosisDA(sConString);
            string DoctorId = common.myStr(ViewState["DoctorId"]);//fun.GetDoctorId(HospitalId, UserId);
            string FormId = "0";
            if (Session["formId"] != null)
            {
                FormId = common.myStr(Session["formId"]);
            }

            string Saved_RTF_Content = string.Empty;

            // dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, "0");
            dsTemplate = note.getEMRTemplateReportSequence(common.myInt(ddlReportFormat.SelectedValue));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            dtTemplate = dsTemplate.Tables[0];

            string gBegin = string.Empty;
            string gEnd = string.Empty;
            string Fonts = string.Empty;

            //sb.Append("<span style='" + Fonts + "'>");
            //sb.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + ";color: #000000;'>");

            if (dtTemplate.Rows.Count > 0)
            {
                sb.Append("<span>");

                foreach (DataRow dr in dtTemplate.Rows)
                {
                    string strTemplateType = common.myStr(dr["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    string TEXT = common.myStr(dr["HeadingName"]);
                    if (common.myStr(dr["PageName"]).Trim() == string.Empty && common.myStr(dr["SectionName"]).Trim() == string.Empty && common.myStr(dr["HeadingName"]).Trim().ToUpper() != "MEDICATIONS")
                    {
                        sbTemplateStyle = new StringBuilder();
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/><br/>");
                        }

                        if (!common.myStr(dr["DefaultText"]).Trim().Equals(string.Empty))
                        {
                            //sbTemplateStyle.Append("<span style='font-weight: 100; font-family: " + common.myStr(hdnFontName.Value) + ";'>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/>");
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 100; '>" + common.myStr(dr["DefaultText"]).Trim() + "</span>  <br/><br/>");
                        }
                        sb.Append(sbTemplateStyle);
                        drTemplateStyle = null;
                        Templinespace = string.Empty;
                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Vitals")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (common.myStr(dr["HeadingName"]) != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + " </span> : <br/>");
                        }
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        }
                        StringBuilder sbTemp = new StringBuilder();

                        bnotes.BindVitals(common.myInt(HospitalId).ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                                    Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                                    fdate, tdate, 0, "0");

                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append("<br/>");

                        drTemplateStyle = null;
                        Templinespace = string.Empty;
                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "LAB History")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]) != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        //StringBuilder sbTemp = new StringBuilder();
                        //bnotes.BindLabTestResultReport(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page);

                        //if (sbTemp.ToString() != string.Empty)
                        //    sb.Append(sbTemp + "<br/><br/>");
                        //else
                        sb.Append(sbTemplateStyle + "<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Chief Complaints" || common.myStr(dr["PageName"]).Trim() == "Complaints")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                            Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                           fdate, tdate);
                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Diagnosis")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }

                        //StringBuilder sbTemp = new StringBuilder();
                        //bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId), DoctorId,
                        //                      sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                        //                      common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
                        //                      fdate, tdate, 0, "0");

                        //if (sbTemp.ToString() != string.Empty)
                        //    sb.Append(sbTemp + "<br/><br/>");
                        //else
                        sb.Append(sbTemplateStyle + "<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Allergies")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        drTemplateStyle = null;// = dv[0].Row;
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
                                    common.myInt(Session["UserID"]).ToString(), common.myStr(dr["PageID"]),
                                  fdate, tdate, 0);
                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append(sbTemplateStyle + "<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    // else if (common.myStr(dr["PageName"]).Trim().Contains("Prescription"))
                    else if (common.myStr(dr["HeadingName"]).Trim().ToUpper() == "MEDICATIONS")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        // DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        // dv.RowFilter = "PageId=" + common.myStr(dr["PageId"]);
                        //if (dv.Count > 0)
                        //{
                        //    drTemplateStyle = dv[0].Row;
                        //}
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        sb.Append(sbTemplateStyle);
                        BindMedicationIP(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
                                        Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                       fdate, tdate, common.myInt(Session["FacilityId"]), "D");
                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append("<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Current Medication")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }

                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                            Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                            fdate, tdate);
                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Orders And Procedures" || common.myStr(dr["PageName"]).Trim() == "Order")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindOrders(common.myInt(ViewState["RegistrationId"]), HospitalId, EncounterId,
                                        Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                        common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                        fdate, tdate, "0");

                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append("<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Immunization Chart" || common.myStr(dr["PageName"]).Trim() == "Immunization")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        bnotes.BindImmunization(common.myInt(HospitalId).ToString(), common.myInt(ViewState["RegistrationId"]),
                                    common.myInt(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                    fdate, tdate);
                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append("<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["PageName"]).Trim() == "Provisional Diagnosis")
                    {
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                        Request.QueryString["DoctorId"] != null ? common.myInt(Request.QueryString["DoctorId"]).ToString() : common.myInt(DoctorId).ToString(),
                                        sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                        fdate, tdate, 0, "0");

                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append("<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if ((common.myStr(dr["PageName"]).Trim() == "ROS"))
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dr["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        }
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["PageName"]).Trim() + "</span> : <br/>");
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        BindProblemsROS(HospitalId, EncounterId, sbTemp, common.myStr(dr["DisplayName"]).Trim(), common.myStr(dr["TemplateName"]).Trim(), common.myStr(dr["PageId"]));

                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append("<br/>");
                        drTemplateStyle = null;
                        Templinespace = string.Empty;

                    }
                    else if (common.myStr(dr["SectionName"]).Trim() != string.Empty)//&& common.myStr(dr["TemplateCode"]).Trim() == "HIS")
                    {
                        int lck = 0;
                        sbTemplateStyle = new StringBuilder();
                        if (common.myStr(dr["HeadingName"]).Trim() != string.Empty)
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["HeadingName"]).Trim() + "</span> : <br/>");
                        }
                        else
                        {
                            sbTemplateStyle.Append("<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-weight: 700; '>" + common.myStr(dr["SectionName"]).Trim() + "</span> : <br/>");
                        }
                        sb.Append(sbTemplateStyle);
                        StringBuilder sbTemp = new StringBuilder();
                        bindDataDischargeSummary(FormId, common.myStr(dr["TemplateId"]), sbTemp, common.myStr(dr["SectionId"]), common.myStr(dr["FieldId"]), ddlReportFormat.SelectedValue, common.myInt(dr["DisplaySectionName"]), common.myInt(dr["DisplayFieldName"]));
                        if (sbTemp.ToString() != string.Empty)
                            sb.Append(sbTemp + "<br/><br/>");
                        else
                            sb.Append(sbTemp + "<br/>");

                        drTemplateStyle = null;
                        Templinespace = string.Empty;
                    }
                }

                sb.Append("</span>");
                sbTemplateStyle = null;
                StringBuilder temp = new StringBuilder();
                bnotes.GetEncounterFollowUpAppointment(common.myInt(Session["HospitalLocationId"]).ToString(), common.myInt(ViewState["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
                if (temp.Length > 20)
                {
                    //sb.Append("</span>"); //mmb
                    Saved_RTF_Content += temp.ToString();
                }

                if (Saved_RTF_Content == string.Empty || Saved_RTF_Content == null)
                {
                    return sb.ToString();
                }
                else
                {
                    if (sb.ToString() == string.Empty || sb.ToString() == null)
                    {
                        return Saved_RTF_Content;
                    }
                    else
                    {
                        return sb.ToString() + Saved_RTF_Content;
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
        finally
        {
            dsTemplate.Dispose();
            dtTemplate.Dispose();
            dsTemplateStyle.Dispose();
            note = null;
        }

        return string.Empty;
    }

    private string PrintReport(bool sign)
    {
        string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
        Session["DisplayEnteredByInCaseSheet"] = string.Empty;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();

        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        clsIVF objIVF = new clsIVF(sConString);

        #region Declare DataSet
        DataSet dsTemplateData = new DataSet();
        #endregion
        #region Call Bind Case Sheet class
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
        #endregion
        try
        {
            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            #region Call Bind Case Sheet method to get data
            dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                                    common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"),
                                    common.myDate(DateTime.Now).ToString("yyyy/MM/dd"),
                                    string.Empty, 0, string.Empty, false, common.myInt(ddlReportFormat.SelectedValue));
            #endregion

            dvDataFilter = new DataView(dsTemplateData.Tables[21]);
            dtEncounter = dsTemplateData.Tables[22];
            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    #region Template Wise
                    {
                        dtTemplate = dvDataFilter.ToTable();
                        TemplateString = new StringBuilder();
                        for (int i = 0; i < dtTemplate.Rows.Count; i++)
                        {
                            #region Admission Request
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "AdmissionRequest"
                                    && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bPatientBookingDisplay == false)
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                #region Call Bind Patient Booking
                                sbTemp = new StringBuilder();
                                BindCaseSheet.BindPatientBooking(dsTemplateData.Tables[20], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                   Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "");
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                bPatientBookingDisplay = true;
                                #endregion
                            }
                            #endregion
                            #region Chief Complaints
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Chief Complaints"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";

                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);

                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 98))
                                //{
                                #region Call Bind Problem data
                                BindCaseSheet.BindProblemsHPI(dsTemplateData.Tables[0], common.myInt(ViewState["RegistrationId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    //////TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Template History Type
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                               && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                               && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call History Type Dynamic Template
                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                //if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //{
                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dtDyTempTable = dvDyTable4.ToTable();
                                dvDyTable4.Sort = "RecordId ASC";
                                //}
                                //else
                                //{
                                //    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dtDyTempTable = dvDyTable4.ToTable();
                                //    dvDyTable4.Sort = "RecordId ASC";
                                //}
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                        // TemplateString.Append(sbTemp);
                                        TemplateString.Append(sbTemp + "<br/>");
                                }

                                sbTemp = null;
                                dsDymanicTemplateData.Dispose();
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Allergy
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bAllergyDisplay == false)
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                drTemplateStyle = null;// = dv[0].Row;
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 8))
                                //{
                                #region Call Allergy template data
                                BindCaseSheet.BindAllergies(dsTemplateData.Tables[1], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageID"]), 0, "", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                {
                                    TemplateString.Append(sbTemp + "<br/>");
                                    bAllergyDisplay = true;
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Vital
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 14))
                                //{
                                #region Call Vital Template data
                                BindCaseSheet.BindVitals(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                    Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), true);

                                #endregion
                                if (sbTemp.ToString() != "")
                                    //////TemplateString.Append(sbTemp + "<br/>" + "<br/>");

                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";

                            }
                            #endregion
                            #region All the Templates except Hitory and Plan of case
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                //if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //{
                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dtDyTempTable = dvDyTable4.ToTable();
                                dvDyTable4.Sort = "RecordId ASC";
                                //}
                                //else
                                //{
                                //    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dtDyTempTable = dvDyTable4.ToTable();
                                //    dvDyTable4.Sort = "RecordId ASC";
                                //}
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Lab
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
                                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                            {
                                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                                strTemplateType = strTemplateType.Substring(0, 1);
                                //sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                                TemplateString.Append(sbTemp);
                                drTemplateStyle = null;

                                sbTemp = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Diagnosis
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
                               && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 133))
                                //{
                                #region Call Diagnosis Template Data
                                BindCaseSheet.BindAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                           0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", common.myStr(ViewState["IsShowDiagnosisGroupHeading"]), true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Provisional Diagnosis
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 1085))
                                //{
                                #region Call Provisional Diagnosis template data
                                BindCaseSheet.BindPatientProvisionalDiagnosis(dsTemplateData.Tables[2],
                                           Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Template Plan of Care
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call Dynamic Template Plan of Care
                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                //if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //{
                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                dtDyTempTable = dvDyTable4.ToTable();
                                dvDyTable4.Sort = "RecordId ASC";
                                //}
                                //else
                                //{
                                //    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                //    dtDyTempTable = dvDyTable4.ToTable();
                                //    dvDyTable4.Sort = "RecordId ASC";
                                //}

                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;

                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Orders And Procedures
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Orders And Procedures"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 17))
                                //{
                                #region Call Bind Order data
                                BindCaseSheet.BindOrders(dsTemplateData.Tables[8], DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", "", true, true);
                                #endregion


                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");

                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Prescription
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }

                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 153))
                                //{
                                DataSet dsMedication = new DataSet();
                                DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);

                                dvTable1.ToTable().TableName = "Item";

                                dvTable1.RowFilter = "EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);


                                dsMedication.Tables.Add(dvTable1.ToTable());


                                dvTable1.Dispose();

                                #region Call Medication Template data
                                BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
                                               common.myInt(Session["UserID"]).ToString(), string.Empty, 0, string.Empty, string.Empty, true);
                                #endregion

                                dsMedication.Dispose();
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Non Drug Order
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Non Drug Order"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 1166))
                                //{
                                #region Call Non Drug Order template data
                                BindCaseSheet.BindNonDrugOrder(dsTemplateData.Tables[7], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                                  common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Diet Order
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diet Order"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }

                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 1172))
                                //{
                                #region Call Diet Order data
                                BindCaseSheet.BindDietOrderInNote(dsTemplateData.Tables[9], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0", "",
                                    common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                #endregion

                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Doctor Progress Note
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 1013))
                                //{
                                #region Call Doctor Progress Note template data
                                BindCaseSheet.BindDoctorProgressNote(dsTemplateData.Tables[3], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]), "",
                                           common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                            #region Referal History
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Referral History"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 1081))
                                //{
                                StringBuilder temp1 = new StringBuilder();
                                #region Call Referral History Template Data
                                BindCaseSheet.BindReferalHistory(dsTemplateData.Tables[5], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                    common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                            #region Current Medication
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
                                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10005))
                                //{
                                bnotes.BindMedication(common.myInt(ViewState["EncounterId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["RegistrationId"]), sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                                common.myDate(DateTime.Now.AddMonths(-1)).ToString(),
                                                common.myDate(DateTime.Now).ToString(), common.myStr(ViewState["OPIP"]), "");
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Immunization
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
                              && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 113))
                                //{
                                BindCaseSheet.BindImmunization(dsTemplateData.Tables[13], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                            common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Daily Injection
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }

                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 805))
                                //{
                                BindCaseSheet.BindInjection(dsTemplateData.Tables[12], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                             common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));

                                TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                            #region Follow-up Appointment
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim().ToUpper().Equals("FOLLOW UP APPOINTMENT")
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 919))
                                //{
                                StringBuilder temp = new StringBuilder();
                                #region FollowUp Appointment
                                BindCaseSheet.GetEncounterFollowUpAppointment(dsTemplateData.Tables[6],
                                       temp, sbTemplateStyle, drTemplateStyle, Page, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                #endregion

                                TemplateString.Append(temp);
                                temp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                        }
                        if (TemplateString.Length > 30)
                        {
                            //if (iEn == 0)
                            //{
                            //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
                            //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
                            //sb.Append("</span>");
                            //}
                            sb.Append("<span style='" + String.Empty + "'>");
                            sb.Append(TemplateString);
                            sb.Append("</span><br/>");
                            TemplateString = null;
                        }
                    }
                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;

            sbTemplateStyle = null;
            TemplateString = null;
            ds.Dispose();
            dsTemplateStyle.Dispose();
            dvDataFilter.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            Templinespace = "";
            bnotes = null;
            fun = null;
            emr = null;
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
        }
        return sb.ToString();
    }

    protected void bindData(DataSet dsDynamicTemplateData, string TemplateId, StringBuilder sb, string GroupingDate)
    {
        DataSet ds = new DataSet();
        DataSet dsAllNonTabularSectionDetails = new DataSet();
        DataSet dsAllTabularSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();

        DataTable dtFieldValue = new DataTable();
        DataTable dtEntry = new DataTable();
        DataTable dtFieldName = new DataTable();

        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dv2 = new DataView();

        DataRow dr3;

        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        StringBuilder str = new StringBuilder();
        string sEntryType = "V";
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string BeginList2 = string.Empty;
        string BeginList3 = string.Empty;
        string EndList3 = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;

        int t = 0;
        int t2 = 0;
        int t3 = 0;
        int iRecordId = 0;
        DataView dvDyTable1 = new DataView();
        try
        {
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;

            t = 0;
            t2 = 0;
            t3 = 0;

            dvDyTable1 = new DataView(dsDynamicTemplateData.Tables[0]);
            DataView dvDyTable2 = new DataView(dsDynamicTemplateData.Tables[1]);
            DataView dvDyTable3 = new DataView(dsDynamicTemplateData.Tables[2]);

            dvDyTable1.ToTable().TableName = "TemplateSectionName";
            dvDyTable2.ToTable().TableName = "FieldName";
            dvDyTable3.ToTable().TableName = "PatientValue";
            dsAllNonTabularSectionDetails = new DataSet();
            if (dvDyTable3.ToTable().Rows.Count > 0)
            {
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable2.ToTable());
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable3.ToTable());
            }
            dvDyTable2.Dispose();
            dvDyTable3.Dispose();

            dsDynamicTemplateData.Dispose();

            #region Non Tabular
            if (dsAllNonTabularSectionDetails.Tables.Count > 0 && dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvNonTabular = new DataView(dvDyTable1.ToTable());
                dvNonTabular.RowFilter = "Tabular=0";
                if (dvNonTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvNonTabular.ToTable());//Section Name Table

                    dv = new DataView(dsAllNonTabularSectionDetails.Tables[1]);

                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvNonTabular.Dispose();

                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllNonTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllNonTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]) + " AND SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllNonTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dsAllNonTabularSectionDetails.Tables[0].Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();

                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                            //  str.Append("<br/> ");


                                            dr3 = null;
                                            dsAllNonTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();
                                            string sBreak = common.myBool(item["IsConfidential"]) == true ? "<br/>" : "";
                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        ////// objStrTmp.Append("<br />"); //code commented  for Examination (SectonName and fieldname getting extra space)
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (sBegin.Contains("<br/>"))
                                                        {
                                                            sBegin = sBegin.Remove(0, 5);
                                                            objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                        }
                                                        else
                                                        {
                                                            objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(sBreak + common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {

                                                            if (sBegin.StartsWith("<br/>"))
                                                            {
                                                                if (sBegin.Length > 5)
                                                                {

                                                                    //sBegin = sBegin.Remove(0, 5);
                                                                    //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                                                    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);

                                                            }

                                                            //if (sBegin.Contains("<br/>"))
                                                            //{
                                                            //    sBegin = sBegin.Remove(0, 5);
                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}
                                                            //else
                                                            //{

                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}

                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            //if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            //{
                                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                            ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            // }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Tabular
            DataView dvDyTable4 = new DataView(dsDynamicTemplateData.Tables[3]);
            DataView dvDyTable5 = new DataView(dsDynamicTemplateData.Tables[4]);
            DataView dvDyTable6 = new DataView(dsDynamicTemplateData.Tables[5]);

            dvDyTable4.ToTable().TableName = "TabularData";
            dvDyTable5.ToTable().TableName = "TabularColumnCount";
            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";

            dsAllTabularSectionDetails = new DataSet();
            if (dvDyTable4.ToTable().Rows.Count > 0)
            {
                dsAllTabularSectionDetails.Tables.Add(dvDyTable4.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable5.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable6.ToTable());
            }

            dvDyTable4.Dispose();
            dvDyTable5.Dispose();



            if (dsAllTabularSectionDetails.Tables.Count > 0 && dsAllTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvTabular = new DataView(dvDyTable1.ToTable());
                dvTabular.RowFilter = "Tabular=1";
                if (dvTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvTabular.ToTable());//Section Name Table
                    dv = new DataView(dsAllTabularSectionDetails.Tables[0]);
                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvTabular.Dispose();
                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                DataView dvFieldStyle = new DataView(dsAllTabularSectionDetails.Tables[2]);
                                dvFieldStyle.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = " SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dsAllFieldsDetails.Tables.Add(dvDyTable6.ToTable());
                                dvDyTable6.Dispose();
                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dvFieldStyle.ToTable().Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();
                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                            str.Append("<br/> ");

                                            dr3 = null;
                                            dsAllTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();

                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        objStrTmp.Append("<br />");
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            if (sBegin.Contains("<br/>"))
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
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            {
                                                iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                                ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            if (t2.Equals(1) && t3.Equals(1))
            {
                objStrTmp.Append(EndList3);
            }
            else
            {
                objStrTmp.Append(EndList);
            }
            if (GetPageProperty("1") != null)
            {
                objStrSettings.Append(objStrTmp.ToString());
                sb.Append(objStrSettings.ToString());
            }
            else
            {
                sb.Append(objStrTmp.ToString());
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dsAllNonTabularSectionDetails.Dispose();
            dsAllTabularSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();

            dtFieldValue.Dispose();
            dtEntry.Dispose();
            dtFieldName.Dispose();
            dvDyTable1.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dv2.Dispose();

            dr3 = null;

            objStrTmp = null;
            objStrSettings = null;

            sEntryType = string.Empty;
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;
            sBegin = string.Empty;
            sEnd = string.Empty;
        }
    }

    public StringBuilder BindPatientProvisionalDiagnosis(int RegId, int HospitalId, int EncounterId, Int16 UserId, string DocId,
                        StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID, string fromDate, string toDate, int TemplateFieldId, string sEREncounterId)
    {
        StringBuilder sbProvisional = new StringBuilder();
        DataSet ds = new DataSet();
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            string sBegin = string.Empty;
            string sEnd = string.Empty;
            string sBeginSection = string.Empty;
            string sEndSection = string.Empty;

            ds = objDiag.GetPatientProvisionalDiagnosis(HospitalId, RegId, EncounterId, UserId, fromDate, toDate, string.Empty);
            MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle);
            sBeginSection = sBegin;
            sEndSection = sEnd;
            if (ds.Tables[0].Rows.Count > 0)
            {
                sb.Append("<br />" + sBeginSection + sbTemplateStyle + sEndSection + "<br />");
                sbProvisional.Append(common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]));
                sb.Append(sbProvisional);
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
            objDiag = null;
        }

        return sbProvisional;
    }

    //private void BindTemplateData()
    //{
    //    DataTable data = new DataTable();
    //    try
    //    {
    //        data = BindTemplateNames(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));

    //        int itemOffset = 0;// e.NumberOfItems;
    //        if (itemOffset == 0)
    //        {
    //            //this.ddlTemplates.Items.Clear();
    //        }
    //        int endOffset = Math.Min(itemOffset + data.Rows.Count, data.Rows.Count);
    //        // e.EndOfItems = endOffset == data.Rows.Count;

    //        for (int i = itemOffset; i < endOffset; i++)
    //        {
    //            RadComboBoxItem item = new RadComboBoxItem();
    //            item.Text = (string)data.Rows[i]["DisplayTemplateName"];
    //            StringBuilder sTemplate = BindEditor(common.myStr(data.Rows[i]["DisplayTemplateName"]),
    //                                        common.myStr(data.Rows[i]["TemplateIdentification"]) == "P" ? common.myInt(data.Rows[i]["PageID"]).ToString() : common.myInt(data.Rows[i]["TemplateId"]).ToString(),
    //                                        common.myStr(data.Rows[i]["TemplateIdentification"]));

    //            item.Value = sTemplate.ToString();
    //            this.ddlTemplates.Items.Add(item);
    //            item.DataBind();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        data.Dispose();
    //    }
    //}

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(hdnSummaryID.Value) == 0)
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {
                    Alert.ShowAjaxMsg("Please save death summary !!", Page);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please save Health Check up summary !!", Page);
                }
                lblMessage.Text = string.Empty;
                return;
            }
            string strDthSum = string.Empty;
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
                strDthSum = common.myStr(Request.QueryString["For"]);

            if (common.myStr(Session["FacilityName"]).Trim().Contains("Arvind Medicare") || common.myStr(Session["FacilityName"]).Trim().Contains("Miracles Fertility") || common.myStr(Session["FacilityName"]).Trim().Contains("Arvind Medicare"))
            {
                RadWindow2.NavigateUrl = "/EMRReports/PrintHealthCheckUpArvind.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&DoctorId=" + common.myStr(ViewState["DoctorId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + ddlReportFormat.SelectedValue + "&HC=HC";
            }
            else
            {
                RadWindow2.NavigateUrl = "/EMRReports/PrintHealthCheckUp.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&DoctorId=" + common.myStr(ViewState["DoctorId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + ddlReportFormat.SelectedValue + "&HC=HC";
            }
            RadWindow2.Height = 550;
            RadWindow2.Width = 1100;
            RadWindow2.Top = 10;
            RadWindow2.Left = 10;
            RadWindow2.Modal = true;
            RadWindow2.OnClientClose = "OnClientClose";
            RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow2.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    private StringBuilder BindEditor(string sTemplateName, string sPageID, string sTemplateType)
    {
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder sbTemp = new StringBuilder();
        DataSet ds = new DataSet();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        Hashtable hst = new Hashtable();
        string Templinespace = string.Empty;
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString); ;
        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        int UserId = common.myInt(common.myInt(Session["UserID"]));
        int facilityId = common.myInt(Session["Facilityid"]);
        DL_Funs ff = new DL_Funs();
        BindNotes bnotes = new BindNotes(sConString);

        try
        {

            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
            dsTemplateStyle = ObjIcm.GetICMTemplateStyle(common.myInt(Session["HospitalLocationID"]));
            string sFromDate = string.Empty;
            string sToDate = string.Empty;
            //if (chkDateWise.Checked)
            //{
            //    sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
            //    sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
            //}
            if (sTemplateType == "P")
            {
                if (sTemplateName == "Vitals" || sTemplateName == "Vital")
                {
                    sbTemplateStyle = new StringBuilder();
                    bnotes.BindVitals(common.myInt(HospitalId).ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                              Page, sPageID, common.myInt(Session["UserID"]).ToString(),
                                              sFromDate, sToDate, 0, "0", string.Empty);
                }
                else if (sTemplateName == "Investigations")
                {
                    bool bShowAllParameters = common.myStr(Session["ModuleName"]) == "EMR" ? false : true;
                    bnotes.BindLabTestResult(RegId, HospitalId, EncounterId, Convert.ToInt16(common.myInt(Session["UserID"])), DoctorId, sbTemp, sbTemplateStyle,
                        drTemplateStyle, Page, common.myInt(Session["FacilityId"]), sPageID, common.myInt(Session["UserID"]).ToString(), bShowAllParameters);
                }
                else if (sTemplateName == "Chief Complaint" || sTemplateName == "Chief Complaints")
                {
                    sbTemplateStyle = new StringBuilder();
                    bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                          Page, sPageID, common.myInt(Session["UserID"]).ToString(),
                                          sFromDate, sToDate, string.Empty);

                }
                else if (sTemplateName == "Diagnosis")
                {
                    sbTemplateStyle = new StringBuilder();
                    bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                Request.QueryString["DoctorId"] != null ? common.myInt(Request.QueryString["DoctorId"]).ToString() : common.myInt(DoctorId).ToString(),
                                sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString(),
                                sFromDate, sToDate, 0, "0", string.Empty);

                }
                else if (sTemplateName == "Allergies" || sTemplateName == "Allergy")
                {
                    sbTemplateStyle = new StringBuilder();
                    bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                                      common.myInt(Session["UserID"]).ToString(), sPageID,
                                      sFromDate, sToDate, 0, string.Empty);
                }
                else if (sTemplateName == "Medication")
                {
                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
                                                           Page, sPageID, common.myInt(Session["UserID"]).ToString(),
                                                           sFromDate,
                                                           sToDate, Session["OPIP"] == null ? common.myStr(Request.QueryString["OPIP"]) : common.myStr(Session["OPIP"]), string.Empty);
                }
                else if (sTemplateName == "Orders And Procedures" || sTemplateName == "Orders" || sTemplateName == "Procedures")
                {
                    sbTemplateStyle = new StringBuilder();
                    bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
                                   Convert.ToInt16(UserId), Request.QueryString["DoctorId"] != null ? common.myInt(Request.QueryString["DoctorId"]).ToString() : common.myInt(DoctorId).ToString(),
                                   sbTemp, sbTemplateStyle, drTemplateStyle, Page, sPageID, common.myInt(Session["UserID"]).ToString(),
                                   sFromDate, sToDate, "0", string.Empty);
                }
                else if (sTemplateName == "Immunization Chart")
                {
                    sbTemplateStyle = new StringBuilder();
                    bnotes.BindImmunization(common.myInt(HospitalId).ToString(), common.myInt(Session["RegistrationId"]),
                                       common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                       sPageID, common.myInt(Session["UserID"]).ToString(), sFromDate, sToDate, string.Empty);
                }

            }
            else
            {
                bindData("0", sFromDate, sToDate, sPageID, sbTemp, sTemplateName);
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
            ObjIcm = null;
            ds.Dispose();
            dsTemplate.Dispose();
            dsTemplateStyle.Dispose();
            fun = null;
            ff = null;
            bnotes = null;
        }
        return sbTemp;
    }

    protected string GetTemplateId(string TemplateName, int HospitalLocationId)
    {
        Object templateId = new Object();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hs = new Hashtable();

        try
        {
            string sqlQ = "Select Id from EMRTemplate with (nolock) where HospitalLocationId=@HospitalLocationId and templateName like @TemplateName";

            hs.Add("@TemplateName", TemplateName);
            hs.Add("@HospitalLocationId", HospitalLocationId);

            templateId = dl.ExecuteScalar(CommandType.Text, sqlQ, hs);
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
        return templateId.ToString();
    }

    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, string sFromDate, string sToDate, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds = new DataSet();
        DataSet dsGender = new DataSet();
        DataSet dsFont = new DataSet();
        string sEncodedDate = string.Empty;
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
        DataTable dt = new DataTable();
        DataTable dtN = new DataTable();
        DataTable dtNg = new DataTable();
        DataTable dtNv = new DataTable();

        try
        {
            hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            hstInput.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
            if (common.myInt(Session["Gender"]) == 1)
                hstInput.Add("chrGenderType", "F");
            else if (common.myInt(Session["Gender"]) == 2)
                hstInput.Add("chrGenderType", "M");
            hstInput.Add("@intFormId", common.myInt(Session["formId"]));

            dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
            DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;
            hsGender.Add("@intRegistrationId", ViewState["RegistrationId"]);
            string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration with (nolock) where Id = @intRegistrationId";
            dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
            if (dsGender.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
                    strGender = "He";
                else
                    strGender = "She";
            }
            int pti = 0, nti = 0;
            hsProblems.Add("@intEncounterId", EncounterId);
            hsProblems.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
            hsProblems.Add("@chvFromDate", sFromDate);
            hsProblems.Add("@chvToDate", sToDate);
            ds = new DataSet();
            ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["EntryDate"]);
                DataView dv1 = new DataView(ds.Tables[0]);
                dv1.RowFilter = "PositiveValue<>''";
                dtPositiveRos = dv1.ToTable();

                DataView dv2 = new DataView(ds.Tables[0]);
                dv2.RowFilter = "NegativeValue<>''";
                dtNegativeRos = dv2.ToTable();

            }
            string strSectionId = string.Empty;
            string sDate = string.Empty;
            string sPreviousValue = string.Empty;

            foreach (DataRow tdr in ds.Tables[0].Rows)
            {
                if (sEncodedDate == common.myStr(tdr["EntryDate"]))
                {
                    if (!bPRRecords && !bNRRecords)
                    {
                        objStrTmp.Append("<b>" + common.myStr(tdr["EntryDate"]) + "</b> : <br/>");

                        dt = new DataTable();
                        for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
                        {
                            DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                            if (sEncodedDate == common.myStr(dr["EntryDate"]))
                            {
                                if (common.myStr(dr["SectionId"]) != strSectionId)
                                {
                                    string sBegin = string.Empty;
                                    string sEnd = string.Empty;

                                    if (common.myStr(drFont["SectionsBold"]) != string.Empty || common.myStr(drFont["SectionsItalic"]) != string.Empty || common.myStr(drFont["SectionsUnderline"]) != string.Empty || common.myStr(drFont["SectionsFontSize"]) != string.Empty || common.myStr(drFont["SectionsForecolor"]) != string.Empty || common.myStr(drFont["SectionsListStyle"]) != string.Empty)
                                    {
                                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                        {
                                            if (pti == 0)
                                            {
                                                objStrTmp.Append(sBegin + "Positive Symptoms:" + sEnd);
                                            }
                                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                                        }
                                    }
                                    else
                                    {
                                        if (pti == 0)
                                        {
                                            objStrTmp.Append("<br />" + "Positive Symptoms:");
                                        }
                                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                                    }
                                    pti++;

                                    if (common.myStr(dr["FieldsBold"]) != string.Empty || common.myStr(dr["FieldsItalic"]) != string.Empty || common.myStr(dr["FieldsUnderline"]) != string.Empty || common.myStr(dr["FieldsFontSize"]) != string.Empty || common.myStr(dr["FieldsForecolor"]) != string.Empty || common.myStr(dr["FieldsListStyle"]) != string.Empty)
                                    {
                                        sBegin = string.Empty; sEnd = string.Empty;
                                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                        objStrTmp.Append(sBegin + strGender + " has ");
                                    }
                                    else
                                        objStrTmp.Append(strGender + " has ");

                                    strSectionId = common.myStr(dr["SectionId"]);
                                    DataView dv = new DataView(dtPositiveRos);
                                    dv.RowFilter = "SectionId=" + common.myInt(dr["SectionId"]);
                                    dt = dv.ToTable();
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {

                                        if (j == dt.Rows.Count - 1)
                                        {
                                            if (dt.Rows.Count == 1)
                                            {
                                                objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                                            }
                                            else
                                            {
                                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                                objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                                            }
                                        }
                                        else
                                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");

                                        bPRRecords = true;
                                    }
                                    objStrTmp.Append(sEnd);
                                    strSectionId = common.myStr(dr["SectionId"]);
                                    sDate = common.myStr(dr["EntryDate"]);
                                }
                            }

                        }
                        //strSectionId = string.Empty;
                        for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
                        {
                            DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                            if (sEncodedDate == common.myStr(dr["EntryDate"]))
                            {
                                if (common.myStr(dr["SectionId"]) != strSectionId)
                                {
                                    string sBegin = string.Empty;
                                    string sEnd = string.Empty;
                                    if (common.myStr(drFont["SectionsBold"]) != string.Empty || common.myStr(drFont["SectionsItalic"]) != string.Empty || common.myStr(drFont["SectionsUnderline"]) != string.Empty || common.myStr(drFont["SectionsFontSize"]) != string.Empty || common.myStr(drFont["SectionsForecolor"]) != string.Empty || common.myStr(drFont["SectionsListStyle"]) != string.Empty)
                                    {

                                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                        {
                                            if (nti == 0)
                                            {
                                                objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                                            }
                                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                                        }
                                    }
                                    else
                                    {
                                        if (nti == 0)
                                        {
                                            objStrTmp.Append("<br />" + "Negative Symptoms:");
                                        }
                                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                                    }
                                    nti++;
                                    if (common.myStr(dr["FieldsBold"]) != string.Empty || common.myStr(dr["FieldsItalic"]) != string.Empty || common.myStr(dr["FieldsUnderline"]) != string.Empty || common.myStr(dr["FieldsFontSize"]) != string.Empty || common.myStr(dr["FieldsForecolor"]) != string.Empty || common.myStr(dr["FieldsListStyle"]) != string.Empty)
                                    {
                                        sBegin = string.Empty; sEnd = string.Empty;
                                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                        objStrTmp.Append(sBegin + strGender + " does not have ");
                                    }
                                    else
                                        objStrTmp.Append(strGender + " does not have ");

                                    strSectionId = common.myStr(dr["SectionId"]);
                                    DataView dv = new DataView(dtNegativeRos);
                                    dv.RowFilter = "SectionId=" + common.myInt(dr["SectionId"]);
                                    dt = dv.ToTable();
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {

                                        if (j == dt.Rows.Count - 1)
                                        {
                                            if (dt.Rows.Count == 1)
                                            {
                                                objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                                            }
                                            else
                                            {
                                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                                objStrTmp.Append(" or " + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                                            }
                                        }
                                        else
                                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");

                                        bNRRecords = true;
                                    }
                                    objStrTmp.Append(sEnd);
                                    strSectionId = common.myStr(dr["SectionId"]);
                                    sDate = common.myStr(dr["EntryDate"]);
                                }

                            }

                        }

                    }
                }
                else
                {
                    if (sDate != common.myStr(tdr["EntryDate"]))
                    {
                        objStrTmp.Append("<br/><br/>");
                        objStrTmp.Append("<b>" + common.myStr(tdr["EntryDate"]) + "</b>  : <br/>");
                    }

                    dt = new DataTable();

                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionID=" + common.myInt(tdr["SectionID"]);
                    dt = dv.ToTable();
                    foreach (DataRow dr in dtPositiveRos.Rows)
                    {
                        if (common.myStr(dr["EntryDate"]) == common.myStr(tdr["EntryDate"]))
                        {
                            //DataView dv = new DataView(dtPositiveRos);
                            //dv.RowFilter = "SectionID = " + tdr["SectionID"].ToString();
                            if (common.myStr(dr["SectionId"]) != strSectionId && common.myInt(dr["SectionId"]) > common.myInt(strSectionId))
                            {
                                string sBegin = string.Empty;
                                string sEnd = string.Empty;
                                if (common.myStr(drFont["SectionsBold"]) != string.Empty || common.myStr(drFont["SectionsItalic"]) != string.Empty || common.myStr(drFont["SectionsUnderline"]) != string.Empty || common.myStr(drFont["SectionsFontSize"]) != string.Empty || common.myStr(drFont["SectionsForecolor"]) != string.Empty || common.myStr(drFont["SectionsListStyle"]) != string.Empty)
                                {
                                    MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                    if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (sDate != common.myStr(tdr["EntryDate"]))
                                        {
                                            objStrTmp.Append(sBegin + "Positive Symptoms: " + sEnd);
                                        }
                                        objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                                    }
                                }
                                else
                                {
                                    if (sDate != common.myStr(tdr["EntryDate"]))
                                    {
                                        objStrTmp.Append("<br />" + "Positive Symptoms: ");
                                    }
                                    objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                                }

                                if (common.myStr(dr["FieldsBold"]) != string.Empty || common.myStr(dr["FieldsItalic"]) != string.Empty || common.myStr(dr["FieldsUnderline"]) != string.Empty || common.myStr(dr["FieldsFontSize"]) != string.Empty || common.myStr(dr["FieldsForecolor"]) != string.Empty || common.myStr(dr["FieldsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                    objStrTmp.Append(sBegin + strGender + " has ");
                                }
                                else
                                    objStrTmp.Append(strGender + " has ");

                                strSectionId = common.myStr(dr["SectionId"]);
                                DataView dvf = new DataView(dtPositiveRos);
                                dvf.RowFilter = "SectionId=" + common.myInt(dr["SectionId"]);
                                dt = dvf.ToTable();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {

                                    if (j == dt.Rows.Count - 1)
                                    {
                                        if (dt.Rows.Count == 1)
                                        {
                                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                                        }
                                        else
                                        {
                                            objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                            objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                                        }
                                    }
                                    else
                                        objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");

                                }
                                objStrTmp.Append(sEnd);
                                sDate = common.myStr(dr["EntryDate"]);
                                //strSectionId = dr["SectionId"].ToString();
                            }
                        }
                        // sDate = tdr["EntryDate"].ToString();
                        pi++;
                        //strSectionId = dr["SectionId"].ToString();
                    }
                    //strSectionId = string.Empty;

                    dtN = new DataTable();
                    DataView dvN = new DataView(dtNegativeRos);
                    dvN.RowFilter = "SectionID=" + common.myInt(tdr["SectionID"]);
                    dtNg = dvN.ToTable();
                    foreach (DataRow dr in dtNg.Rows)
                    {
                        if (strSectionId != common.myStr(tdr["SectionId"]))
                        {
                            bNRRecords = false;
                        }
                        else
                        {
                            if (sPreviousValue != string.Empty)
                            {
                                bNRRecords = false;
                            }
                        }
                        if (common.myStr(dr["EntryDate"]) == common.myStr(tdr["EntryDate"]) && !bNRRecords)
                        {
                            if (sPreviousValue != common.myStr(dr["NegativeValue"]) && strSectionId == common.myStr(dr["SectionId"]))
                            {
                                string sBegin = string.Empty;
                                string sEnd = string.Empty;
                                if (common.myStr(drFont["SectionsBold"]) != string.Empty || common.myStr(drFont["SectionsItalic"]) != string.Empty || common.myStr(drFont["SectionsUnderline"]) != string.Empty || common.myStr(drFont["SectionsFontSize"]) != string.Empty || common.myStr(drFont["SectionsForecolor"]) != string.Empty || common.myStr(drFont["SectionsListStyle"]) != string.Empty)
                                {

                                    MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                                    if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                                    {
                                        //if (strSectionId == dr["SectionId"].ToString())
                                        //{
                                        objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                                        // bDisplayDate = false;
                                        //}
                                        objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                                    }
                                }
                                else
                                {
                                    if (sDate != common.myStr(tdr["EntryDate"]))
                                    {
                                        objStrTmp.Append("<br /><br />" + "Negative Symptoms:");
                                    }
                                    objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                                }

                                if (common.myStr(dr["FieldsBold"]) != string.Empty || common.myStr(dr["FieldsItalic"]) != string.Empty || common.myStr(dr["FieldsUnderline"]) != string.Empty || common.myStr(dr["FieldsFontSize"]) != string.Empty || common.myStr(dr["FieldsForecolor"]) != string.Empty || common.myStr(dr["FieldsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                                    objStrTmp.Append(sBegin + strGender + " does not have ");
                                }
                                else
                                    objStrTmp.Append(strGender + " does not have ");


                                strSectionId = common.myStr(dr["SectionId"]);
                                dtNv = new DataTable();
                                DataView dvf = new DataView(dtNegativeRos);
                                dvf.RowFilter = "SectionId=" + common.myInt(dr["SectionId"]);

                                dtNv = dvf.ToTable();
                                for (int j = 0; j < dtNv.Rows.Count; j++)
                                {
                                    if (j == dtNv.Rows.Count - 1)
                                    {
                                        if (dtNv.Rows.Count == 1)
                                        {
                                            objStrTmp.Append(common.myStr(dtNv.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                                        }
                                        else
                                        {
                                            objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                            objStrTmp.Append(" or " + common.myStr(dtNv.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                                        }
                                    }
                                    else
                                        objStrTmp.Append(common.myStr(dtNv.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");

                                    sPreviousValue = common.myStr(dtNv.Rows[0]["NegativeValue"]);
                                    //bNRRecords = false;
                                }
                                objStrTmp.Append(sEnd);
                                strSectionId = common.myStr(dr["SectionId"]);
                                sDate = common.myStr(tdr["EntryDate"]);
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
            dsGender.Dispose();
            dsFont.Dispose();
            dtPositiveRos.Dispose();
            dtNegativeRos.Dispose();
            DlObj = null;
            dt.Dispose();
            dtN.Dispose();
            dtNg.Dispose();
            dtNv.Dispose();
        }
        return sb;
    }

    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds = new DataSet();
        DataSet dsGender = new DataSet();
        DataSet dsFont = new DataSet();
        string strGender = "He";
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        StringBuilder objStrTmp = new StringBuilder();
        DataTable dt = new DataTable();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
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
            dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
            DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


            hsGender.Add("@intRegistrationId", ViewState["RegistrationId"]);
            string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration with (nolock) where Id = @intRegistrationId";
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
                dv1.RowFilter = "PositiveValue<>''";
                dtPositiveRos = dv1.ToTable();

                DataView dv2 = new DataView(ds.Tables[0]);
                dv2.RowFilter = "NegativeValue<>''";
                dtNegativeRos = dv2.ToTable();
                //Make font start

                if (common.myStr(drFont["TemplateBold"]) != string.Empty
                    || common.myStr(drFont["TemplateItalic"]) != string.Empty
                    || common.myStr(drFont["TemplateUnderline"]) != string.Empty
                    || common.myStr(drFont["TemplateFontSize"]) != string.Empty
                    || common.myStr(drFont["TemplateForecolor"]) != string.Empty
                    || common.myStr(drFont["TemplateListStyle"]) != string.Empty)
                {
                    string sBegin = string.Empty;
                    string sEnd = string.Empty;
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                    if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                    {
                        //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                        objStrTmp.Append(sBegin + common.myStr(sDisplayName) + sEnd);
                    }
                    //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                    //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
                }
                else
                {
                    if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                    {
                        //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                        objStrTmp.Append(common.myStr(sDisplayName));//Default Setting
                    }
                    //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                }

                // Make Font End

                //sb.Append("<u><Strong>Review of systems</Strong></u>");

            }
            // For Positive Symptoms
            if (dtPositiveRos.Rows.Count > 0)
            {
                string strSectionId = string.Empty; // dtPositiveRos.Rows[0]["SectionId"].ToString();
                dt = new DataTable();
                for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
                {

                    DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                    if (common.myStr(dr["SectionId"]) != strSectionId)
                    {
                        string sBegin = string.Empty;
                        string sEnd = string.Empty;
                        if (common.myStr(drFont["SectionsBold"]) != string.Empty
                            || common.myStr(drFont["SectionsItalic"]) != string.Empty
                            || common.myStr(drFont["SectionsUnderline"]) != string.Empty
                            || common.myStr(drFont["SectionsFontSize"]) != string.Empty
                            || common.myStr(drFont["SectionsForecolor"]) != string.Empty
                            || common.myStr(drFont["SectionsListStyle"]) != string.Empty)
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

                        if (common.myStr(dr["FieldsBold"]) != string.Empty
                            || common.myStr(dr["FieldsItalic"]) != string.Empty
                            || common.myStr(dr["FieldsUnderline"]) != string.Empty
                            || common.myStr(dr["FieldsFontSize"]) != string.Empty
                            || common.myStr(dr["FieldsForecolor"]) != string.Empty
                            || common.myStr(dr["FieldsListStyle"]) != string.Empty)
                        {
                            sBegin = string.Empty; sEnd = string.Empty;
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
                        dv.RowFilter = "SectionId=" + common.myInt(dr["SectionId"]);
                        dt = dv.ToTable();

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (j == (dt.Rows.Count - 1))
                            {
                                if (dt.Rows.Count == 1)
                                {
                                    objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
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
                //if (drFont["TemplateBold"].ToString() != string.Empty || drFont["TemplateItalic"].ToString() != string.Empty || drFont["TemplateUnderline"].ToString() != string.Empty || drFont["TemplateFontSize"].ToString() != string.Empty || drFont["TemplateForecolor"].ToString() != string.Empty || drFont["TemplateListStyle"].ToString() != string.Empty)
                //{
                //    string sBegin = "", sEnd = string.Empty;
                //    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                //    //objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                //    //objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
                //}
                //else
                //{
                //    objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
                //}          
                string strSectionId = string.Empty; // 
                dt = new DataTable();
                for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
                {

                    DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                    if (common.myStr(dr["SectionId"]) != strSectionId)
                    {
                        string sBegin = string.Empty;
                        string sEnd = string.Empty;
                        if (common.myStr(drFont["SectionsBold"]) != string.Empty
                            || common.myStr(drFont["SectionsItalic"]) != string.Empty
                            || common.myStr(drFont["SectionsUnderline"]) != string.Empty
                            || common.myStr(drFont["SectionsFontSize"]) != string.Empty
                            || common.myStr(drFont["SectionsForecolor"]) != string.Empty
                            || common.myStr(drFont["SectionsListStyle"]) != string.Empty)
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


                        if (common.myStr(dr["FieldsBold"]) != string.Empty
                            || common.myStr(dr["FieldsItalic"]) != string.Empty
                            || common.myStr(dr["FieldsUnderline"]) != string.Empty
                            || common.myStr(dr["FieldsFontSize"]) != string.Empty
                            || common.myStr(dr["FieldsForecolor"]) != string.Empty
                            || common.myStr(dr["FieldsListStyle"]) != string.Empty)
                        {
                            sBegin = string.Empty; sEnd = string.Empty;
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
                        dv.RowFilter = "SectionId=" + common.myInt(dr["SectionId"]);
                        dt = dv.ToTable();
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {

                            if (j == dt.Rows.Count - 1)
                            {
                                if (dt.Rows.Count == 1)
                                {
                                    objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
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
            dsGender.Dispose();
            dsFont.Dispose();
            dtPositiveRos.Dispose();
            dtNegativeRos.Dispose();
            dt.Dispose();
            DlObj = null;

        }
        return sb;
    }

    protected void bindData(string iFormId, string sFromDate, string sToDate, string TemplateId, StringBuilder sb, string sTemplateName)
    {
        DataSet ds = new DataSet();
        DataSet dsAllSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();
        string str = string.Empty;
        string sEncodedDate = string.Empty;
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataTable dtFieldValue = new DataTable();
        DataTable dtFieldName = new DataTable();
        try
        {
            hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            hstInput.Add("@intTemplateId", TemplateId);

            if (common.myInt(Session["Gender"]) == 1)
                hstInput.Add("chrGenderType", "F");
            else if (common.myInt(Session["Gender"]) == 2)
                hstInput.Add("chrGenderType", "M");

            hstInput.Add("@intFormId", iFormId);
            hstInput.Add("@bitDischargeSummary", Convert.ToBoolean(1));

            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cache.Insert("SectionFormat" + TemplateId + common.myInt(Session["HospitalLocationID"]), ds.Tables[0], null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            hstInput = new Hashtable();
            hstInput.Add("@intTemplateId", TemplateId);
            hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));

            if (common.myInt(Session["Gender"]) == 1)
                hstInput.Add("chrGenderType", "F");
            else if (common.myInt(Session["Gender"]) == 2)
                hstInput.Add("chrGenderType", "M");

            hstInput.Add("@chrFromDate", sFromDate);
            hstInput.Add("@chrToDate", sToDate);

            dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

            if (dsAllSectionDetails.Tables[2].Rows.Count > 0)
            {
                sEncodedDate = common.myStr(dsAllSectionDetails.Tables[2].Rows[0]["EntryDate"]);
            }

            string BeginList = string.Empty;
            string EndList = string.Empty;
            string BeginList2 = string.Empty;
            string EndList2 = string.Empty;
            string BeginList3 = string.Empty;
            string EndList3 = string.Empty;
            int t = 0, t2 = 0, t3 = 0;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[3]);
                dv1.RowFilter = "SectionId=" + common.myInt(item["SectionId"]);

                dtFieldName = dv1.ToTable();
                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
                    dtFieldValue = dv2.ToTable();
                }
                dsAllFieldsDetails = new DataSet();
                dsAllFieldsDetails.Tables.Add(dtFieldName);
                dsAllFieldsDetails.Tables.Add(dtFieldValue);
                if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
                {
                    if (dsAllSectionDetails.Tables.Count > 2)
                    {

                        if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;

                            DataRow dr3;
                            dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                            str = CreateString1(dsAllFieldsDetails, common.myInt(item["TemplateId"]), sTemplateName, common.myInt(item["SectionID"]).ToString(),
                                                common.myStr(item["SectionName"]), sEncodedDate, common.myStr(item["Tabular"]));

                            str += " ";

                            if (common.myStr(item["SectionsBold"]) != string.Empty
                                       || common.myStr(item["SectionsItalic"]) != string.Empty
                                       || common.myStr(item["SectionsUnderline"]) != string.Empty
                                       || common.myStr(item["SectionsFontSize"]) != string.Empty
                                       || common.myStr(item["SectionsForecolor"]) != string.Empty
                                       || common.myStr(item["SectionsListStyle"]) != string.Empty)
                            {
                                sBegin = string.Empty; sEnd = string.Empty;
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                {
                                    if (str.Trim() != string.Empty)
                                    {
                                        objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd + "<br/>"); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                    }
                                }
                                BeginList3 = string.Empty;
                            }
                            else
                            {
                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                {
                                    if (str.Trim() != string.Empty)
                                    {
                                        objStrTmp.Append(common.myStr(item["SectionName"]) + "<br/>"); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                    }
                                }
                            }


                            objStrTmp.Append(str);
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
        finally
        {
            ds.Dispose();
            dsAllSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();
            dl = null;
            dtFieldValue.Dispose();
            dtFieldName.Dispose();
        }

        sb.Append(objStrTmp);
    }

    protected string CreateString1(DataSet objDs, int iRootId, string iRootName, string sSectionID, string sSectionName,
                            string sEncodedDate, string TabularType)
    {
        DataSet ds = new DataSet();
        DataSet dsTabulerTemplate = new DataSet();
        StringBuilder objStr = new StringBuilder();
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string BeginList2 = string.Empty;
        string EndList2 = string.Empty;
        string BeginList3 = string.Empty;
        string EndList3 = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        int t = 0, t2 = 0, t3 = 0;
        int it = 0;
        string sMutipleRecordsDate = string.Empty;
        Hashtable hstInput = new Hashtable();
        DataTable dtFilter = new DataTable();
        DataTable dtNewTable = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtRow = new DataTable();
        DataTable dsSection = new DataTable();
        DataTable dtFieldType = new DataTable();
        DataTable dt1 = new DataTable();

        try
        {
            if (objDs != null)
            {
                if (bool.Parse(TabularType))
                {
                    if (objDs.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = objDs.Tables[0].Rows[0];
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
                        MaxLength = dvValues.ToTable().Rows.Count;

                        DataView dvRowHeader = objDs.Tables[1].DefaultView;
                        dvRowHeader.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]) + " AND ISNULL(RowCaption,'')<>''";

                        bool isRowHeader = false;
                        if (dvRowHeader.ToTable().Rows.Count > 0)
                        {
                            isRowHeader = true;
                        }

                        dtFilter = dvValues.ToTable();

                        DataView dvFilter = new DataView(dtFilter);
                        dvFilter.RowFilter = "RowCaption='0'";
                        dtNewTable = dvFilter.ToTable();
                        if (dtNewTable.Rows.Count > 0)
                        {
                            if (MaxLength != 0)
                            {
                                //objStr.Append("<br /><span style=' " + sFontSize + "'>" + sSectionName + "</span>");

                                objStr.Append("<table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' >");
                                objStr.Append("<tr align='center'>");

                                FieldsLength = objDs.Tables[0].Rows.Count;

                                if (FieldsLength > 0)
                                {
                                    if (isRowHeader)
                                    {
                                        objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' > + </th>");
                                    }
                                }

                                for (int i = 0; i < FieldsLength; i++)   // it makes table
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");
                                    dr = objDs.Tables[0].Rows[i];
                                    dvValues = new DataView(objDs.Tables[1]);
                                    dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
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
                                                if (j == 0 && isRowHeader)
                                                {
                                                    if (ds.Tables.Count > 0)
                                                    {
                                                        objStr.Append("<td style=' " + sFontSize + "' align='right'>" + common.myStr(dsMain.Tables[j].Rows[i]["RowCaption"]) + "</td>");
                                                    }
                                                }

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
                        else
                        {
                            hstInput = new Hashtable();
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
                            hstInput.Add("@intSecId", common.myInt(sSectionID));

                            dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                            DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                            dvRowCaption.RowFilter = "RowCaptionId>0";
                            if (dvRowCaption.ToTable().Rows.Count > 0)
                            {
                                StringBuilder sbCation = new StringBuilder();
                                dvRowCaption.RowFilter = "RowNum>0";
                                dt = dvRowCaption.ToTable();
                                if (dt.Rows.Count > 0)
                                {
                                    sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                    int column = dt.Columns.Count;
                                    int ColumnCount = 0;
                                    int count = 1;
                                    for (int k = 1; k < (column - 5); k++)
                                    {
                                        //if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                        //    && ColumnCount == 0)
                                        //{
                                        //    sbCation.Append("<td>");
                                        //    sbCation.Append(" + ");
                                        //    sbCation.Append("</td>");
                                        //}
                                        //else
                                        //{
                                        sbCation.Append("<td>");
                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                        sbCation.Append("</td>");
                                        count++;
                                        //}
                                        ColumnCount++;
                                    }
                                    sbCation.Append("</tr>");

                                    DataView dvRow = new DataView(dt);
                                    dvRow.RowFilter = "RowCaptionId>0";
                                    dtRow = dvRow.ToTable();
                                    for (int l = 1; l <= dtRow.Rows.Count; l++)
                                    {
                                        sbCation.Append("<tr>");
                                        for (int i = 1; i < ColumnCount + 1; i++)
                                        {
                                            if (i == 1)
                                            {
                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                            }
                                            else
                                            {
                                                if (common.myStr(dt.Rows[1]["Col" + i]) == "D")
                                                {
                                                    DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                    if (common.myStr(dt.Rows[l + 1]["Col" + i]) != string.Empty)
                                                    {
                                                        dvDrop.RowFilter = "ValueId=" + common.myInt(dt.Rows[l + 1]["Col" + i]);
                                                        if (dvDrop.ToTable().Rows.Count > 0)
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvDrop.ToTable().Rows[0]["ValueName"]) + "</td>");
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
                                                    if (common.myStr(dt.Rows[l + 1]["Col" + i]) != string.Empty)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["Col" + i]) + "</td>");
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
                    string fBeginList = string.Empty;
                    string fEndList = string.Empty;
                    string sfBegin = string.Empty;
                    string sfEnd = string.Empty;
                    int tf = 0;
                    it = 0;
                    string sPSectionName = string.Empty;
                    foreach (DataRow item in objDs.Tables[0].Rows)
                    {
                        if (sEncodedDate == common.myStr(objDs.Tables[1].Rows[it]["EntryDate"]))
                        {
                            //if (bTRecords == false)
                            //{
                            //    objStr.Append("<b>" + objDs.Tables[1].Rows[it]["EntryDate"].ToString() + "</b> : ");
                            //    bTRecords = true;
                            //}

                            dsSection = (DataTable)Cache["SectionFormat" + iRootId + common.myInt(Session["HospitalLocationID"])];
                            DataView dv = new DataView(dsSection);
                            dv.RowFilter = "SectionID=" + common.myInt(sSectionID);
                            dt = dv.ToTable();

                            if (iPrevId == common.myInt(dt.Rows[0]["TemplateId"]))
                            {
                                if (t2 == 0)
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "1")
                                        { BeginList3 = "<ul>"; EndList3 = "</ul>"; }
                                        else if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "2")
                                        { BeginList3 = "<ol>"; EndList3 = "</ol>"; }
                                    }
                                if (common.myStr(dt.Rows[0]["SectionsBold"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsItalic"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsUnderline"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsFontSize"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsForecolor"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                    if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != common.myStr(dt.Rows[0]["SectionName"]))   //19June2010
                                    {
                                        // objStr.Append(BeginList3 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                    }
                                    BeginList3 = string.Empty;
                                }
                                else
                                {
                                    if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != common.myStr(dt.Rows[0]["SectionName"]))    //19June
                                    {
                                        // objStr.Append(dt.Rows[0]["SectionName"].ToString()); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                    }
                                }
                                if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "3")
                                    objStr.Append("<br />");
                            }
                            else
                            {
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "1")
                                    { BeginList2 = "<ul>"; EndList3 = "</ul>"; }
                                    else if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "2")
                                    { BeginList2 = "<ol>"; EndList3 = "</ol>"; }
                                }
                                if (common.myStr(dt.Rows[0]["SectionsBold"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsItalic"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsUnderline"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsFontSize"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsForecolor"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                    if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        //objStr.Append(BeginList2 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                    }
                                    BeginList2 = string.Empty;
                                }
                                else
                                {
                                    //if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]))// Comment ON 19June2010
                                    // objStr.Append(dt.Rows[0]["SectionName"].ToString()); //Comment On 19June2010
                                }
                                if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "3" || common.myStr(dt.Rows[0]["SectionsListStyle"]) == "0")
                                    objStr.Append("<br />");

                                iPrevId = common.myInt(dt.Rows[0]["TemplateId"]);

                            }
                            sPSectionName = common.myStr(dt.Rows[0]["SectionName"]);
                        }
                        else
                        {
                            //if (bTMRecords == false)
                            //{
                            //    objStr.Append("<br/>");
                            //    objStr.Append("<b>" + objDs.Tables[1].Rows[it]["EntryDate"].ToString() + "</b> : <br/>");
                            //    bTMRecords = true;
                            //}
                            //else if (sMutipleRecordsDate != objDs.Tables[1].Rows[it]["EntryDate"].ToString())
                            //{
                            //    objStr.Append("<br/>");
                            //    objStr.Append("<b>" + objDs.Tables[1].Rows[it]["EntryDate"].ToString() + "</b> : <br/>");
                            //}
                            dsSection = (DataTable)Cache["SectionFormat" + iRootId + common.myInt(Session["HospitalLocationID"])];
                            DataView dv = new DataView(dsSection);
                            dv.RowFilter = "SectionID=" + common.myInt(sSectionID);
                            dt = dv.ToTable();

                            if (iPrevId == common.myInt(dt.Rows[0]["TemplateId"]))
                            {
                                if (t2 == 0)
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "1")
                                        { BeginList3 = "<ul>"; EndList3 = "</ul>"; }
                                        else if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "2")
                                        { BeginList3 = "<ol>"; EndList3 = "</ol>"; }
                                    }
                                if (common.myStr(dt.Rows[0]["SectionsBold"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsItalic"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsUnderline"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsFontSize"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsForecolor"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                    if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != common.myStr(dt.Rows[0]["SectionName"]))   //19June2010
                                    {
                                        //objStr.Append(BeginList3 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                    }
                                    BeginList3 = string.Empty;
                                }
                                else
                                {
                                    if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]) && sPSectionName != common.myStr(dt.Rows[0]["SectionName"]))    //19June
                                    {
                                        // objStr.Append(dt.Rows[0]["SectionName"].ToString()); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                    }
                                }
                                if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "3")
                                    objStr.Append("<br />");
                            }
                            else
                            {
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "1")
                                    { BeginList2 = "<ul>"; EndList3 = "</ul>"; }
                                    else if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "2")
                                    { BeginList2 = "<ol>"; EndList3 = "</ol>"; }
                                }
                                if (common.myStr(dt.Rows[0]["SectionsBold"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsItalic"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsUnderline"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsFontSize"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsForecolor"]) != string.Empty || common.myStr(dt.Rows[0]["SectionsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Sections", ref sBegin, ref sEnd, dt.Rows[0]);
                                    if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        //objStr.Append(BeginList2 + sBegin + dt.Rows[0]["SectionName"].ToString() + sEnd);
                                    }
                                    BeginList2 = string.Empty;
                                }
                                else
                                {
                                    //if (Convert.ToBoolean(dt.Rows[0]["SectionDisplayTitle"]))// Comment ON 19June2010
                                    // objStr.Append(dt.Rows[0]["SectionName"].ToString()); //Comment On 19June2010
                                }
                                if (common.myStr(dt.Rows[0]["SectionsListStyle"]) == "3" || common.myStr(dt.Rows[0]["SectionsListStyle"]) == "0")
                                    objStr.Append("<br />");

                                iPrevId = common.myInt(dt.Rows[0]["TemplateId"]);

                            }
                            sPSectionName = common.myStr(dt.Rows[0]["SectionName"]);
                            sMutipleRecordsDate = common.myStr(objDs.Tables[1].Rows[it]["EntryDate"]);
                        }
                        it++;

                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                        objDt = objDv.ToTable();
                        if (tf == 0)
                        {
                            tf = 1;
                            if (common.myStr(item["FieldsListStyle"]) == "1")
                            { fBeginList = "<ul>"; fEndList = "</ul>"; }
                            else if (common.myStr(item["FieldsListStyle"]) == "2")
                            { fBeginList = "<ol>"; fEndList = "</ol>"; }
                        }
                        if (common.myStr(item["FieldsBold"]) != string.Empty || common.myStr(item["FieldsItalic"]) != string.Empty || common.myStr(item["FieldsUnderline"]) != string.Empty || common.myStr(item["FieldsFontSize"]) != string.Empty || common.myStr(item["FieldsForecolor"]) != string.Empty || common.myStr(item["FieldsListStyle"]) != string.Empty)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                sEnd = string.Empty;
                                MakeFont("Fields", ref sfBegin, ref sfEnd, item);
                                if (Convert.ToBoolean(item["DisplayTitle"]))
                                {
                                    objStr.Append(BeginList + sfBegin + common.myStr(item["FieldName"]));
                                    if (objStr.ToString() != string.Empty)
                                        objStr.Append(sfEnd + "</li>");
                                }
                                fBeginList = string.Empty;
                                sfBegin = string.Empty;
                            }

                        }
                        else
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                //if (Convert.ToBoolean(item["DisplayTitle"]))
                                //objStr.Append(item["FieldName"].ToString());
                            }
                        }
                        if (objDs.Tables.Count > 1)
                        {
                            objDv = new DataView(objDs.Tables[1]);
                            objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                            objDt = objDv.ToTable();
                            DataView dvFieldType = new DataView(objDs.Tables[0]);
                            dvFieldType.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                            dtFieldType = dvFieldType.ToTable("FieldType");
                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                            {
                                if (sEncodedDate == common.myStr(objDs.Tables[1].Rows[i]["EntryDate"]))
                                {
                                    if (objDt.Rows.Count > 0)
                                    {
                                        string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                        if (FType == "C")
                                            FType = "C";
                                        if (FType == "C" || FType == "D" || FType == "B")
                                        {
                                            if (FType == "B")
                                            {
                                                if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1" || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                                {

                                                    DataView dv1 = new DataView(objDs.Tables[1]);
                                                    if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")

                                                        dv1.RowFilter = "TextValue='Yes'";
                                                    else
                                                        dv1.RowFilter = "TextValue='No'";

                                                    dt1 = dv1.ToTable();

                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != string.Empty)
                                                        {
                                                            if (i == 0)
                                                                objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                            else
                                                                objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                            else
                                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (i == 0)
                                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        else
                                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
                                            if (common.myInt(ViewState["iTemplateId"]) != 163)
                                            {
                                                if (i == 0)
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                else
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                    objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                else
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                        }
                                        if (common.myStr(item["FieldsListStyle"]) == string.Empty)
                                            if (common.myInt(ViewState["iTemplateId"]) != 163)
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
                                        string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                        if (FType == "C")
                                            FType = "C";
                                        if (FType == "C" || FType == "D" || FType == "B")
                                        {
                                            if (FType == "B")
                                            {
                                                if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1" || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                                {

                                                    DataView dv1 = new DataView(objDs.Tables[1]);
                                                    if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                                        dv1.RowFilter = "TextValue='Yes'";
                                                    else
                                                        dv1.RowFilter = "TextValue='No'";

                                                    dt1 = dv1.ToTable();
                                                    if (dt1.Rows.Count > 0)
                                                    {
                                                        if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != string.Empty)
                                                        {
                                                            if (i == 0)
                                                                objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                            else
                                                                objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                            else
                                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (i == 0)
                                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        else
                                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
                                            if (common.myInt(ViewState["iTemplateId"]) != 163)
                                            {
                                                if (i == 0)
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                else
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                    objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                else
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                        }
                                        if (common.myStr(item["FieldsListStyle"]) == string.Empty)
                                        {
                                            if (common.myInt(ViewState["iTemplateId"]) != 163)
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
                if (objStr.ToString() != string.Empty)
                    objStr.Append(EndList);
                if (t2 == 1 && t3 == 1)
                    objStr.Append(EndList3);
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
            dsTabulerTemplate.Dispose();
            objDv.Dispose();
            objDt.Dispose();
            dl = null;
            dsMain.Dispose();
            dtFilter.Dispose();
            dtNewTable.Dispose();
            dt.Dispose();
            dtRow.Dispose();
            dsSection.Dispose();
            dtFieldType.Dispose();
            dt1.Dispose();
        }
        return objStr.ToString();
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
                                string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        DataSet dsTabulerTemplate = new DataSet();
        try
        {
            if (objDs != null)
            {
                if (IsConfidential == false)
                {
                    #region Tabular
                    if (bool.Parse(TabularType) == true)
                    {
                        DataView dvFilter = new DataView(objDs.Tables[0]);
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;
                            dvFilter.Sort = "RowNum ASC";
                            if (GroupingDate != "")
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
                            }
                            DataTable dtNewTable = dvFilter.ToTable();
                            if (dtNewTable.Rows.Count > 0)
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                StringBuilder sbCation = new StringBuilder();
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();
                                        for (int k = 0; k < column; k++)
                                        {
                                            sbCation.Append("<td>");
                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                            sbCation.Append("</td>");
                                            count++;
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId;
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 1; i < ColumnCount + 1; i++)
                                            {
                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        dt.Dispose();
                                        dvData.Dispose();
                                    }
                                    sbCation.Append("</table>");
                                }
                                objStr.Append(sbCation);
                                dsTabulerTemplate.Dispose();
                                sbCation = null;

                            }
                            else
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                if (GroupingDate != "")
                                {
                                    dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                                }
                                else
                                {
                                    dvRowCaption.RowFilter = "RecordId= " + RecordId;
                                }
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    StringBuilder sbCation = new StringBuilder();
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    // dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();

                                        for (int k = 0; k < column + 1; k++)
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

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 0; i < ColumnCount; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
                                                }
                                                else
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
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
                                        dvData.Dispose();
                                    }
                                    objStr.Append(sbCation);
                                    dt.Dispose();
                                    sbCation = null;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Non Tabular
                    else // For Non Tabular Templates
                    {
                        string BeginList = "", EndList = "";
                        string sBegin = "", sEnd = "";
                        int t = 0;
                        string FieldId = "";
                        string sStaticTemplate = "";
                        string sEnterBy = "";
                        string sVisitDate = "";
                        objStr.Append("<table border='0' style='border: 0px;'>"); //string tagTable = "<table border='0' style='border: 0px;'>";
                                                                                  // objStr.Append("<tr><td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> </tr>");
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
                                //rafat1
                                if (objDt.Rows.Count > 0)
                                {
                                    sBegin = "";
                                    sEnd = "";

                                    MakeFont("Fields", ref sBegin, ref sEnd, item);
                                    if (sBegin.Contains("<br/>"))
                                    {
                                        sBegin = sBegin.Remove(0, 5);
                                    }
                                    string sBeginFontWeightNormal = sBegin.Replace("bold", "normal");
                                    if (Convert.ToBoolean(item["DisplayTitle"]))
                                    {
                                        // if (EntryType != "M")
                                        // {
                                        ////if (sBegin.StartsWith("<br/>"))
                                        ////{
                                        ////    if (sBegin.Length > 5)
                                        ////    {
                                        ////        sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                        ////    }
                                        ////}
                                        //}
                                        //else
                                        //{
                                        //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        //}
                                        // 28/08/2011
                                        //if (objDt.Rows.Count > 0)
                                        //{

                                        if (common.myStr(item["FieldType"]) == "H")
                                        {
                                            objStr.Append("<tr><td colspan='9' style='border:0px;'>" + BeginList + sBegin + "<b>" + common.myStr(item["FieldName"]).Replace("<", "&lt;").Replace(">", "&gt;") + "</b>" + sEnd + EndList + "</td>");
                                        }
                                        else
                                        {
                                            objStr.Append("<tr><td colspan='2' style='border:0px;'>" + BeginList + sBegin + common.myStr(item["FieldName"]).Replace("<", "&lt;").Replace(">", "&gt;") + sEnd + EndList + "</td>");
                                        }

                                        // objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));

                                        if (objStr.ToString() != "")
                                        {
                                            //objStr.Append(sEnd + "</li>");
                                        }
                                        ViewState["sBegin"] = sBegin;
                                    }
                                    else
                                    {
                                        objStr.Append("<tr><td colspan='2' style='border:0px;'></td>");
                                    }

                                    BeginList = "";
                                    sBegin = "";
                                    sEnd = "";
                                }
                            }
                            else
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (sStaticTemplate != "<br/><br/>")
                                    {
                                        // objStr.Append(common.myStr(item["FieldName"]));
                                        objStr.Append("<tr><td colspan='2' style='border:0px;'>" + common.myStr(item["FieldName"]) + "</td>");
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
                                sBegin = "";
                                sEnd = "";

                                string sbeginTemp = string.Empty;
                                MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
                                string sBeginFontWeightNormalWithoutBR = sBegin.Replace("bold", "normal");
                                // MakeFont("Fields", ref sBegin, ref sEnd, item);
                                for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                                {
                                    if (objDt.Rows.Count > 0)
                                    {

                                        sbeginTemp = common.myStr(ViewState["sBegin"]);
                                        if (sbeginTemp.StartsWith("<br/>"))
                                        {
                                            if (sbeginTemp.Length > 5)
                                            {
                                                sbeginTemp = sbeginTemp.Substring(0, 5);

                                                //objStrTmp.Append(sBegin + common.myStr(item["SectionName"]) + sEnd);
                                            }
                                        }

                                        string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                        FieldId = common.myStr(dtFieldType.Rows[0]["FieldId"]);
                                        if (FType == "C")
                                        {
                                            FType = "C";
                                        }
                                        if (FType == "C" || FType == "D" || FType == "B" || FType == "R" || (FType == "T" && FieldId == "141708") || (FType == "T" && FieldId == "141709") || (FType == "T" && FieldId == "141698") || (FType == "T" && FieldId == "143774"))
                                        {
                                            if (FType == "B")
                                            {
                                                if (objStr.ToString().EndsWith("<tr>"))
                                                {
                                                    objStr.Append("<td colspan='3'></td>");
                                                }

                                                objStr.Append("<td colspan='7' style='border:0px;'>:" + sBeginFontWeightNormalWithoutBR + objDt.Rows[i]["TextValue"] + sEnd + "</td></tr>");

                                                //  objStr.Append("<td colspan='4'> </td>");
                                                //objStr.Append("  " + objDt.Rows[i]["TextValue"]);
                                            }
                                            else if (FType == "T")
                                            {
                                                objStr.Append("<td colspan='2' style='border:0px;'>" + sBeginFontWeightNormalWithoutBR + common.myStr(item["FieldName"]) + sEnd + "</td>");
                                                objStr.Append("<td colspan='7' style='border:0px;'>:" + sBeginFontWeightNormalWithoutBR + objDt.Rows[i]["TextValue"] + sEnd + "</td></tr>");
                                            }
                                            else
                                            {
                                                //////BindDataValue(objDs, objDt, objStr, i, FType) //comeented by niraj , create and added below overloading methd
                                                //   BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
                                                BindDataValueNew(objDs, objDt, objStr, i, FType, sBegin, sEnd);
                                            }
                                        }
                                        else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                        {
                                            if (common.myStr(ViewState["iTemplateId"]) != "163")
                                            {
                                                if (i == 0)
                                                {
                                                    if (FType == "W")
                                                    {
                                                        objStr.Append(sBeginFontWeightNormalWithoutBR + "<br/>" + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    }
                                                    else
                                                    {
                                                        //objStr.Append(sBegin + ":" + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                        //objStr.Append(sBeginFontWeightNormalWithoutBR + ":" + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                        //if (objStr.ToString().EndsWith("<tr>"))
                                                        //{
                                                        //    objStr.Append("<td colspan='3'></td>");
                                                        //}
                                                        //if(common.myStr(ViewState["iTemplateId"]).Equals("29084") && FType == "T")
                                                        //{
                                                        //    objStr.Append("<td colspan='2'></td>");
                                                        //}

                                                        if (objStr.ToString().Trim().EndsWith("</td></tr>"))
                                                        {
                                                            int objStrLen = objStr.ToString().Length - 10;
                                                            objStr = objStr.Remove(objStrLen, 10);
                                                            //  objStr.ToString().Trim().Replace("</td></tr>", "</td>");
                                                        }
                                                        objStr.Append("<td colspan='7' style='border:0px;'>" + sBeginFontWeightNormalWithoutBR + ":" + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n", "<br/>") + sEnd + "</td></tr>");
                                                        // objStr.Append(sBeginFontWeightNormalWithoutBR + ":" + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                        // objStr.Append("<td colspan='4'> </td>");

                                                    }

                                                }
                                                else
                                                {
                                                    objStr.Append(sBeginFontWeightNormalWithoutBR + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //if (FType == "M" || FType == "W")
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //}
                                                    //else
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                    //}

                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(":" + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else if (FType == "L")
                                        {
                                            objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                        }
                                        else if (FType == "IM")
                                        {
                                            objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
                                        }
                                        else if (FType == "H")
                                        {
                                            objStr.Append("</tr>");
                                        }
                                        if (common.myStr(item["FieldsListStyle"]) == "")
                                        {
                                            if (ViewState["iTemplateId"].ToString() != "163")
                                            {
                                                if (FType != "C")
                                                {

                                                    if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        objStr.Append("<br />");

                                                    }

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
                                    //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                                    //}
                                }
                                sBegin = "";
                                sEnd = "";
                                dvFieldType.Dispose();
                                dtFieldType.Dispose();

                                // Cmt 25/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                //    if (objStr.ToString() != "")
                                //        objStr.Append(sEnd + "</li>");
                                //}
                            }

                            //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                        }
                        objStr.Append("</table>");
                        if (objStr.ToString() != "")
                        {
                            objStr.Append(EndList);
                        }
                    }
                    #endregion
                }
                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
                {
                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                        }
                        dvValues.Dispose();
                    }
                    else
                    {
                        if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == true)
                        {
                            DataView dvValues = new DataView(objDs.Tables[0]);
                            dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
                            if (dvValues.ToTable().Rows.Count > 0)
                            {
                                objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
                            }
                            dvValues.Dispose();
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
        finally
        {
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            objDs.Dispose();
            dsTabulerTemplate.Dispose();
        }
        return objStr.ToString();
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int FieldDisplay)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        DataSet dsTabulerTemplate = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        Hashtable hstInput = new Hashtable();
        DataTable dtFilter = new DataTable();
        DataTable dtNewTable = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtRow = new DataTable();
        DataTable dtFieldType = new DataTable();
        DataTable dt1 = new DataTable();
        try
        {

            if (objDs != null)
            {
                if (bool.Parse(TabularType))
                {
                    if (objDs.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = objDs.Tables[0].Rows[0];
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
                        MaxLength = dvValues.ToTable().Rows.Count;
                        dtFilter = dvValues.ToTable();

                        DataView dvFilter = new DataView(dtFilter);
                        dvFilter.RowFilter = "RowCaption='0'";
                        dtNewTable = dvFilter.ToTable();
                        if (dtNewTable.Rows.Count > 0)
                        {
                            if (MaxLength != 0)
                            {


                                //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                                FieldsLength = objDs.Tables[0].Rows.Count;

                                if (common.myInt(objDs.Tables[0].Rows[0]["TRows"]) > 0)
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");
                                }

                                for (int i = 0; i < FieldsLength; i++)   // it makes table
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                    dr = objDs.Tables[0].Rows[i];
                                    dvValues = new DataView(objDs.Tables[1]);
                                    dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
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
                                            if (common.myInt(dsMain.Tables[0].Rows[i]["RowCaption"]) > 0)
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
                            hstInput = new Hashtable();
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
                            hstInput.Add("@intSecId", common.myInt(sectionId));

                            dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                            DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                            dvRowCaption.RowFilter = "RowCaptionId>0";
                            if (dvRowCaption.ToTable().Rows.Count > 0)
                            {
                                StringBuilder sbCation = new StringBuilder();
                                dvRowCaption.RowFilter = "RowNum>0";
                                dt = dvRowCaption.ToTable();
                                if (dt.Rows.Count > 0)
                                {
                                    sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                    int column = dt.Columns.Count;
                                    int ColumnCount = 0;
                                    int count = 1;
                                    for (int k = 1; k < (column - 5); k++)
                                    {
                                        if (common.myStr(dt.Rows[0]["RowCaptionName"]) == string.Empty && ColumnCount == 0)
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
                                    dtRow = dvRow.ToTable();
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
                                                if (common.myStr(dt.Rows[1]["Col" + i]) == "D")
                                                {
                                                    DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                    if (common.myStr(dt.Rows[l + 1]["Col" + i]) != string.Empty)
                                                    {
                                                        dvDrop.RowFilter = "ValueId=" + common.myInt(dt.Rows[l + 1]["Col" + i]);
                                                        if (dvDrop.ToTable().Rows.Count > 0)
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvDrop.ToTable().Rows[0]["ValueName"]) + "</td>");
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
                                                    if (common.myStr(dt.Rows[l + 1]["Col" + i]) != string.Empty)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["Col" + i]) + "</td>");
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
                    string BeginList = string.Empty;
                    string EndList = string.Empty;
                    string sBegin = string.Empty;
                    string sEnd = string.Empty;
                    int t = 0;
                    string FieldId = string.Empty;
                    string sStaticTemplate = string.Empty;
                    string sEnterBy = string.Empty;
                    string sVisitDate = string.Empty;
                    foreach (DataRow item in objDs.Tables[0].Rows)
                    {
                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
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
                        if (common.myStr(item["FieldsBold"]) != string.Empty
                            || common.myStr(item["FieldsItalic"]) != string.Empty
                            || common.myStr(item["FieldsUnderline"]) != string.Empty
                            || common.myStr(item["FieldsFontSize"]) != string.Empty
                            || common.myStr(item["FieldsForecolor"]) != string.Empty
                            || common.myStr(item["FieldsListStyle"]) != string.Empty)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                sEnd = string.Empty;
                                MakeFont("Fields", ref sBegin, ref sEnd, item);
                                if (FieldDisplay == 1)
                                {
                                    if (common.myBool(item["DisplayTitle"]))
                                    {
                                        if (EntryType != "M")
                                        {
                                            objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        }
                                        if (objStr.ToString() != string.Empty)
                                        {
                                            objStr.Append(sEnd + "</li>");
                                        }
                                    }
                                    else
                                    {
                                        //if (common.myInt(item["ParentId"]) > 0)
                                        //{
                                        //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                                        //}   
                                    }
                                }
                                BeginList = string.Empty;
                                sBegin = string.Empty;
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
                            objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                            objDt = objDv.ToTable();
                            DataView dvFieldType = new DataView(objDs.Tables[0]);
                            dvFieldType.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                            dtFieldType = dvFieldType.ToTable("FieldType");
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

                                                dt1 = dv1.ToTable();
                                                if (dt1.Rows.Count > 0)
                                                {
                                                    if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != string.Empty)
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
                                    if (common.myStr(item["FieldsListStyle"]) == string.Empty)
                                    {
                                        if (common.myInt(ViewState["iTemplateId"]) != 163)
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
                                sEnterBy = common.myStr(objDt.Rows[i]["EnterBy"]);
                                sVisitDate = common.myStr(objDt.Rows[i]["VisitDateTime"]);
                                //if (EntryType == "M" && sEnterBy != string.Empty && sVisitDate != string.Empty)
                                //{
                                //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                                //}
                            }
                        }

                    }

                    if (objStr.ToString() != string.Empty)
                    {
                        objStr.Append(EndList);
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
        finally
        {
            dl = null;
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            dsTabulerTemplate.Dispose();
            dtFilter.Dispose();
            dtNewTable.Dispose();
            dt.Dispose();
            dtRow.Dispose();
            dtFieldType.Dispose();
            dt1.Dispose();
        }
        return objStr.ToString();
    }

    private string BindNonTabularImageTypeFieldValueTemplates(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' runat='server'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["TextValue"]) + "</td></tr><tr align='left'><td align='center'><img src='" + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }

    protected void MakeFontWithoutBR(string typ, ref string sBegin, ref string sEnd, DataRow item)
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
            //if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            //{
            //    sBegin += "<br/>";
            //}
            //else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            //{
            //    sBegin += "; ";
            //}
            //else
            //{
            //    sBegin += "<br/>";
            //}
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


    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        try
        {
            if (i == 0)
                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
            else
            {
                if (FType != "C")
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                else
                {
                    if (i == 0)
                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                    else if (i + 1 == objDs.Tables[1].Rows.Count)
                        objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                    else
                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
        }
    }

    protected void BindDataValueNew(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        string sBeginFontWeightNormal = sBegin.Replace("bold", "normal");
        bool FlagTagCloseTable = true;

        FlagTagCloseTable = false;

        if (i == 0)
        {
            if (FlagTagCloseTable)
            {
                objStr.Append("<td style='border: 0px;'> : " + sBeginFontWeightNormal + common.myStr(objDt.Rows[i]["TextValue"]) + "</td></tr>" + sEnd);
            }
            else
            {
                objStr.Append("<td colspan='7' style='border: 0px;'> : " + sBeginFontWeightNormal + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            }
            if (i + 1 == objDt.Rows.Count)
            {
                objStr.Append("</td></tr>");
            }
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append("<td style='border: 0px;'>" + sBeginFontWeightNormal + ", " + sBeginFontWeightNormal + common.myStr(objDt.Rows[i]["TextValue"]) + "</td>" + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBeginFontWeightNormal + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBeginFontWeightNormal + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBeginFontWeightNormal + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                if (i + 1 == objDt.Rows.Count)
                {
                    objStr.Append("</td></tr>");
                }
            }
        }
    }


    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        string sBeginFontWeightNormal = sBegin.Replace("bold", "normal");
        if (i == 0)
        {
            objStr.Append(sBeginFontWeightNormal + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(sBeginFontWeightNormal + ", " + sBeginFontWeightNormal + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBeginFontWeightNormal + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBeginFontWeightNormal + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBeginFontWeightNormal + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
            }
        }
    }

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        try
        {
            //string sBegin = "", sEnd = string.Empty;
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

            if (common.myStr(item[typ + "Forecolor"]) != string.Empty
                || common.myStr(item[typ + "FontSize"]) != string.Empty
                || common.myStr(item[typ + "FontStyle"]) != string.Empty)
            {
                sBegin += "<span style='";
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                {
                    sBegin += " font-size:" + item[typ + "FontSize"] + ";";
                }
                else
                {
                    sBegin += getDefaultFontSize();
                }
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty)
                {
                    sBegin += " color: #" + item[typ + "Forecolor"] + ";";
                }
                if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
                {
                    sBegin += GetFontFamily(typ, item);
                }
            }
            if (common.myBool(item[typ + "Bold"]))
            {
                sBegin += " font-weight: bold;";
            }
            if (common.myBool(item[typ + "Italic"]))
            {
                sBegin += " font-style: italic;";
            }
            if (common.myBool(item[typ + "Underline"]))
            {
                sBegin += " text-decoration: underline;";
            }

            aEnd.Add("</span>");
            for (int i = aEnd.Count - 1; i >= 0; i--)
            {
                sEnd += aEnd[i];
            }
            //sEnd += "<br/>";
            if (sBegin != string.Empty)
            {
                sBegin += " '>";
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
        }
    }

    protected void MakeDefaultFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        try
        {
            ArrayList aEnd = new ArrayList();
            if (common.myStr(item[typ + "ListStyle"]) == "1")
            {
                sBegin += "<li>";
            }
            else if (common.myStr(item[typ + "ListStyle"]) == "2")
            {
                sBegin += "<li>";
            }
            else
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty || common.myStr(item[typ + "FontSize"]) != string.Empty || common.myStr(item[typ + "FontStyle"]) != string.Empty)
            {
                sBegin += "<span style='";
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += getDefaultFontSize(); }
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty)
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
                { sBegin += GetFontFamily(typ, item); }
            }
            if (common.myBool(item[typ + "Bold"]))
            { sBegin += " font-weight: bold;"; }
            if (common.myBool(item[typ + "Italic"]))
            { sBegin += " font-style: italic;"; }
            if (common.myBool(item[typ + "Underline"]))
            { sBegin += " text-decoration: underline;"; }

            aEnd.Add("</span>");
            for (int i = aEnd.Count - 1; i >= 0; i--)
            {
                sEnd += aEnd[i];
            }
            if (sBegin != string.Empty)
                sBegin += " '>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    public string getDefaultFontSize()
    {
        string sFontSize = string.Empty;
        string FieldValue = string.Empty;
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        try
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myInt(Session["HospitalLocationId"]).ToString());
            if (FieldValue != string.Empty)
            {
                sFontSize = fonts.GetFont("Size", FieldValue);
                if (sFontSize != string.Empty)
                    sFontSize = " font-size: " + sFontSize + ";";
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
            fonts = null;
        }
        return sFontSize;
    }

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = string.Empty;
        string FontName = string.Empty;
        string sBegin = string.Empty;
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        try
        {
            FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
            if (FontName != string.Empty)
                sBegin += " font-family: " + FontName + ";";
            else
            {
                FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myInt(Session["HospitalLocationId"]).ToString());
                if (FieldValue != string.Empty)
                {
                    FontName = fonts.GetFont("Name", FieldValue);
                    if (FontName != string.Empty)
                        sBegin += " font-family: " + FontName + ";";
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
            fonts = null;
        }
        return sBegin;
    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        try
        {
            //string sBegin = "", sEnd = string.Empty;
            ArrayList aEnd = new ArrayList();
            if (common.myStr(item[typ + "Forecolor"]) != string.Empty || common.myStr(item[typ + "FontSize"]) != string.Empty || common.myStr(item[typ + "FontStyle"]) != string.Empty)
            {
                sBegin += "<span style='";
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += getDefaultFontSize(); }
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty)
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
                { sBegin += GetFontFamily(typ, item); }
            }

            if (common.myBool(item[typ + "Bold"]))
            { sBegin += " font-weight: bold;"; }
            if (common.myBool(item[typ + "Italic"]))
            { sBegin += " font-style: italic;"; }
            if (common.myBool(item[typ + "Underline"]))
            { sBegin += " text-decoration: underline;"; }
            aEnd.Add("</span>");
            for (int i = aEnd.Count - 1; i >= 0; i--)
            {
                sEnd += aEnd[i];
            }
            if (sBegin != string.Empty)
                sBegin += " '>";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        sFontSize = string.Empty;

        ArrayList aEnd = new ArrayList();
        try
        {
            if (common.myStr(item[typ + "Forecolor"]) != string.Empty || common.myStr(item[typ + "FontSize"]) != string.Empty || common.myStr(item[typ + "FontStyle"]) != string.Empty)
            {
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                { sFontSize += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sFontSize += getDefaultFontSize(); }
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty)
                { sFontSize += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
                { sFontSize += GetFontFamily(typ, item); };

                if (common.myBool(item[typ + "Bold"]))
                { sFontSize += " font-weight: bold;"; }
                if (common.myBool(item[typ + "Italic"]))
                { sFontSize += " font-style: italic;"; }
                if (common.myBool(item[typ + "Underline"]))
                { sFontSize += " text-decoration: underline;"; }

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
        }
        return sFontSize;
    }

    private DataTable BindTemplateNames(int iEncounterID, int iRegistrationId)
    {
        BaseC.Appointment appointment = new BaseC.Appointment(sConString);
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            dt = appointment.GetEncounterStatusToDelete(iEncounterID, iRegistrationId);
            dv = new DataView(dt);
            dv.RowFilter = "TemplateCode='WN' OR TemplateIdentification='P'";
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            dt.Dispose();
            appointment = null;
        }
        return dv.ToTable();
    }

    private DataTable BindReportTemplateNames(int iEncounterID)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            RadComboBoxItem item = new RadComboBoxItem();

            HshIn.Add("@intReportId", 0);
            HshIn.Add("@intTemplateId", 0);
            HshIn.Add("@intDoctorId", common.myInt(Session["EmployeeId"]));
            HshIn.Add("@chvFlag", "W");
            HshIn.Add("@bitActive", 1);
            HshIn.Add("@inyHospitalLocationId", 1);
            //if (common.myStr(Request.QueryString["For"]) == "DthSum")
            //    HshIn.Add("@chvReportType", "DE");// For Death summary
            //else if (common.myStr(Request.QueryString["HC"]) == "HC")
            //    HshIn.Add("@chvReportType", "HC");// For Discharge Summary
            //else
            //    HshIn.Add("@chvReportType", "DI");// For Discharge Summary



            HshIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateReportSetup", HshIn, HshOut);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "ReportType='HC'";
                    Telerik.Web.UI.RadComboBoxItem rcbItem;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        
                        foreach (DataRow DR in dv.ToTable().Rows)
                        {
                            rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                            rcbItem.Text = (string)DR["ReportName"];
                            rcbItem.Value = common.myStr(DR["ReportId"]);
                            rcbItem.Attributes.Add("IsShowFilledTemplates", common.myStr(DR["IsShowFilledTemplates"]));
                            rcbItem.Attributes.Add("IsCheckListRequired", common.myStr(DR["IsCheckListRequired"]));

                            this.ddlReportFormat.Items.Add(rcbItem);
                            item.DataBind();

                        }
                        //ddlReportFormat.DataSource = dv.ToTable();
                        //ddlReportFormat.DataTextField = "PageName";
                        //ddlReportFormat.DataValueField = "PageId";
                        //ddlReportFormat.DataBind();
                    }
                    else
                    {
                        rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                        rcbItem.Attributes.Add("IsCheckListRequired","false");
                        this.ddlReportFormat.Items.Add(rcbItem);
                        item.DataBind();
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
        finally
        {
            objDl = null;
        }
        return ds.Tables[0];
    }



    private void SaveData()
    {
        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        Hashtable hshOutput = new Hashtable();

        try
        {
            string sFormatID = "0", sPatientSumamry = string.Empty;
            int iSignDoctorID = 0;
            if (common.myStr(RTF1.Content).Trim() == string.Empty)
            {
                Alert.ShowAjaxMsg("Discharge Summary is blank. ", Page);
                lblMessage.Text = string.Empty;
                return;
            }
            if (common.myStr(ddlDoctorSign.SelectedValue) == string.Empty)
            {
                Alert.ShowAjaxMsg("Please select the Sign Doctor", Page);
                lblMessage.Text = string.Empty;
                return;
            }
            if (common.myStr(dtpdate.SelectedDate) == string.Empty)
            {
                Alert.ShowAjaxMsg("Please select the Date & Time", Page);
                lblMessage.Text = string.Empty;
                return;
            }
            if (ddlDoctorSign.SelectedValue != string.Empty)
            {
                iSignDoctorID = common.myInt(ddlDoctorSign.SelectedValue);
            }
            else if (hdnDoctorSignID.Value != string.Empty)
            {
                iSignDoctorID = common.myInt(hdnDoctorSignID.Value);
            }

            if (ddlReportFormat.SelectedValue != "0")
            {
                sFormatID = ddlReportFormat.SelectedValue;
            }
            if (hdnSummaryID.Value != string.Empty)
            {
                sPatientSumamry = common.myStr(RTF1.Content).Replace("..", "&#46;&#46;");
            }
            else
            {
                sPatientSumamry = "<div>" + common.myStr(RTF1.Content).Replace("..", "&#46;&#46;") + "</div>";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(sPatientSumamry);

            sb = sb.Replace("<div>", "<div style=\"font-size: 10pt; font-family: arial; color: #000000;\">");
            sb = sb.Replace("<span style=\"font-size: 10pt;\">", "<span style=\"font-size: 10pt; font-family: arial; color: #000000;\">");
            sb = sb.Replace("<span>", "<span style=\"font-size: 10pt; font-family: arial; color: #000000;\">");

            //sb = sb.Replace("font-family: arial;", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: garamond;", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: georgia;", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: ms sans serif;", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: segoe ui;", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: " + common.myStr(hdnFontName.Value) + ";", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: times new roman;", "font-family: " + common.myStr(hdnFontName.Value) + ";");
            //sb = sb.Replace("font-family: verdana;", "font-family: " + common.myStr(hdnFontName.Value) + ";");

            //sb = sb.Replace("font-size: 8px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 9px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 10px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 11px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 12px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 13px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 14px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 16px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 18px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 20px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 22px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 24px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 26px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 28px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 32px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 36px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 48px;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 72px;", "font-weight: 700;");

            //sb = sb.Replace("font-size: 8pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 9pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 10pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 11pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 12pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 13pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 14pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 16pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 18pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 20pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 22pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 24pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 26pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 28pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 32pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 36pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 48pt;", "font-weight: 700;");
            //sb = sb.Replace("font-size: 72pt;", "font-weight: 700;");

            string DeathDate = string.Empty;
            string DischargeDate = string.Empty;
            if (common.myStr(Request.QueryString["For"]) == "DthSum")  // Death summary date 
            {
                DeathDate = common.myStr(dtpdate.SelectedDate);
            }
            else  //  Discharge summary Date
            {
                DischargeDate = common.myStr(dtpdate.SelectedDate);
            }

            int ReviewRequired = -1;
            string ReviewRemarks = string.Empty;
            if (rdoIsReviewRequired.SelectedIndex == 0 || rdoIsReviewRequired.SelectedIndex == 1)
            {
                ReviewRequired = common.myInt(rdoIsReviewRequired.SelectedValue);
            }

            if (common.myBool(rdoIsReviewRequired.SelectedValue))
            {
                ReviewRemarks = common.myStr(txtReviewRemarks.Text);
            }

            hshOutput = ObjIcm.SaveUpdateICMPatientSummary(common.myInt(hdnSummaryID.Value), common.myInt(Session["HospitalLocationID"]),
                                            common.myInt(ViewState["RegistrationId"]).ToString(), common.myInt(ViewState["EncounterId"]).ToString(),
                                            common.myInt(sFormatID), sb.ToString(), iSignDoctorID, common.myInt(Session["UserID"]),
                                            common.myStr(Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMMdd")), DeathDate,
                                            DischargeDate, common.myInt(Session["FacilityId"]), string.Empty, string.Empty, false, 0, string.Empty,
                                            0, ReviewRequired, ReviewRemarks, 0, 0);

            btnRefresh_OnClick(this, null);

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = hshOutput["@chvErrorStatus"].ToString();



            //BindPatientDischargeSummary();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ObjIcm = null;
        }
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

    //protected void dtpFromDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    //{
    //string soDate = FindFutureDate(common.myStr(dtpToDate.SelectedDate.Value));
    //string sFromDate = FindFutureDate(common.myStr(dtpFromDate.SelectedDate.Value));
    //if (common.myInt(soDate) <= common.myInt(sFromDate))
    //{
    //    Alert.ShowAjaxMsg("To Date should be before to From Date", Page);
    //    return;
    //}
    //}

    //protected void dtpToDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    //{
    //    string soDate = FindFutureDate(common.myStr(dtpToDate.SelectedDate.Value));
    //    string sFromDate = FindFutureDate(common.myStr(dtpFromDate.SelectedDate.Value));
    //    if (common.myInt(soDate) <= common.myInt(sFromDate))
    //    {
    //        Alert.ShowAjaxMsg("To Date should be letter to From Date", Page);
    //        return;

    //    }
    //    //ddlTemplates.Text = string.Empty;
    //    //ddlTemplates.Items.Clear();
    //    //ddlTemplates.DataSource = null;
    //    //ddlTemplates.DataBind();
    //}

    private string FindFutureDate(string outputFutureDate)
    {
        BaseC.Patient formatdate = new BaseC.Patient(sConString);
        string strDate = Convert.ToDateTime(outputFutureDate).Date.ToShortDateString();
        string firstApptDate = string.Empty;
        string NewApptDate = string.Empty;
        string strDateAppointment = formatdate.FormatDateDateMonthYear(strDate);
        string strformatApptDate = formatdate.FormatDate(strDateAppointment, "dd/MM/yyyy", "yyyy/MM/dd");
        firstApptDate = strformatApptDate.Remove(4, 1);
        NewApptDate = firstApptDate.Remove(6, 1);
        return NewApptDate;
    }

    private void BindPatientHiddenDetails()
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

    private void IsValidPassword(string from)
    {
        lblMessage.Text = string.Empty;

        hdnIsValidPassword.Value = "0";
        hdnFrom.Value = from;
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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                if (hdnFrom.Value == "S")
                {
                    SaveData();
                }
                else
                {
                    FinalizeSummary();
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
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        DataSet dsR = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        DataTable tbl = new DataTable();
        DataTable tblH = new DataTable();
        DataTable dtFieldType = new DataTable();
        DataTable dt1 = new DataTable();
        try
        {
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
                            dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);

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

                            dsR = new DataSet();
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

                                dvValues.RowFilter = string.Empty;
                                dvValues = new DataView(objDs.Tables[1]);
                                dvValues.RowFilter = "FieldId=" + common.myInt(dr2["FieldId"]);
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

                                        tbl = dvM.ToTable();

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

                                                    tblH = dvM2.ToTable();
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
                    string BeginList = string.Empty;
                    string EndList = string.Empty;
                    string sBegin = string.Empty;
                    string sEnd = string.Empty;
                    int t = 0;

                    objStr.Append("<br /><br /><table border='0' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' >");

                    foreach (DataRow item in objDs.Tables[0].Rows)
                    {
                        objStr.Append("<tr valign='top'>");

                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
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
                        if (common.myStr(item["FieldsBold"]) != string.Empty
                            || common.myStr(item["FieldsItalic"]) != string.Empty
                            || common.myStr(item["FieldsUnderline"]) != string.Empty
                            || common.myStr(item["FieldsFontSize"]) != string.Empty
                            || common.myStr(item["FieldsForecolor"]) != string.Empty
                            || common.myStr(item["FieldsListStyle"]) != string.Empty)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                sEnd = string.Empty;
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
                                    if (objStr.ToString() != string.Empty)
                                    {
                                        //changes
                                        //objStr.Append(sEnd + "</li>");
                                        objStr.Append(sEnd);
                                    }
                                    //}

                                    objStr.Append("</td>");
                                }
                                BeginList = string.Empty;
                                sBegin = string.Empty;
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
                            objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                            objDt = objDv.ToTable();

                            DataView dvFieldType = new DataView(objDs.Tables[0]);
                            dvFieldType.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                            dtFieldType = dvFieldType.ToTable("FieldType");

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

                                                dt1 = dv1.ToTable();
                                                if (dt1.Rows.Count > 0)
                                                {
                                                    if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != string.Empty)
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
                                    if (common.myStr(item["FieldsListStyle"]) == string.Empty)
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

                    if (objStr.ToString() != string.Empty)
                    {
                        objStr.Append(EndList);
                    }

                    objStr.Append("</table>");
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
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            dsR.Dispose();
            tbl.Dispose();
            tblH.Dispose();
            dtFieldType.Dispose();
            dt1.Dispose();
        }
        return objStr.ToString();
    }

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        RTF1.Content = string.Empty;
        lblMessage.Text = string.Empty;
        // ddlReportFormat.SelectedIndex = 0;
        BindPatientDischargeSummary();
    }

    protected void bindDataDischargeSummary(string iFormId, string TemplateId, StringBuilder sb, string SectionId, string FieldId, string ReportId, int SectionDisplay, int FieldDisplay)
    {
        string str = string.Empty;
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsAllSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();
        DataTable dtEntry = new DataTable();
        DataTable DsSec = new DataTable();
        DataTable dtFieldValue = new DataTable();
        DataTable dtFieldName = new DataTable();
        try
        {
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
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);

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
            if (Request.QueryString["ER"] != null && common.myStr(Request.QueryString["ER"]) == "ER")
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
            hstInput.Add("@intEREncounterId", common.myInt(Request.QueryString["EREncounterId"]).ToString());
            if (FieldId != string.Empty)
            {
                hstInput.Add("@intFieldId", common.myInt(FieldId));
            }
            if (SectionId != string.Empty)
            {
                hstInput.Add("@intSectionID", common.myInt(SectionId));
            }

            dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

            string BeginList = string.Empty;
            string EndList = string.Empty;
            string BeginList2 = string.Empty;
            string BeginList3 = string.Empty;
            string EndList3 = string.Empty;
            int t = 0, t2 = 0, t3 = 0;
            DataView dv = new DataView(dsAllSectionDetails.Tables[2]);

            dtEntry = dv.ToTable(true, "RecordId");
            int iRecordId = 0;

            DsSec = new DataTable();
            DataView dvsec = new DataView(ds.Tables[0]);
            dvsec.RowFilter = "SectionId=" + common.myInt(SectionId);
            DsSec = dvsec.ToTable();

            foreach (DataRow item in DsSec.Rows)
            {
                dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
                dv1.RowFilter = "SectionId=" + common.myInt(item["SectionId"]);
                dtFieldName = dv1.ToTable();

                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
                    dv2.RowFilter = "SectionId=" + common.myInt(item["SectionId"]);
                    dtFieldValue = dv2.ToTable();
                }

                dsAllFieldsDetails = new DataSet();
                dsAllFieldsDetails.Tables.Add(dtFieldName);
                dsAllFieldsDetails.Tables.Add(dtFieldValue);
                if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
                {
                    if (dsAllSectionDetails.Tables.Count > 2)
                    {
                        if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;

                            DataRow dr3;
                            dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);

                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);
                            str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]),
                                    common.myInt(item["SectionId"]).ToString(), common.myStr(item["EntryType"]), FieldDisplay);
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

                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (SectionDisplay == 1)
                                    {
                                        if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                        {
                                            if (str.Trim() != string.Empty)
                                            {
                                                objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                            }
                                        }
                                    }
                                    BeginList3 = string.Empty;
                                }
                                else
                                {
                                    if (SectionDisplay == 1)
                                    {
                                        if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                        {
                                            if (str.Trim() != string.Empty)
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
                                    if (str.Trim() != string.Empty)
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
                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                    if (SectionDisplay == 1)
                                    {
                                        if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                        {
                                            if (sBegin.Contains("<br/>"))
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
                                    BeginList = string.Empty;
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
                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                {
                                    sBegin = string.Empty; sEnd = string.Empty;
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (SectionDisplay == 1)
                                    {
                                        if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                        {
                                            if (str.Trim() != string.Empty) //add 19June2010
                                            {
                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                            }
                                        }
                                    }
                                    BeginList2 = string.Empty;
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            dl = null;
            dsAllSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();
            dtEntry.Dispose();
            DsSec.Dispose();
            dtFieldValue.Dispose();
            dtFieldName.Dispose();
        }
    }

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        string sEREncounterId = common.myInt(Request.QueryString["EREncounterId"]).ToString();

        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = string.Empty;
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);

        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        int UserId = common.myInt(Session["UserID"]);

        BindNotes bnotes = new BindNotes(sConString);
        try
        {
            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));

            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
            DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
            dvFilterStaticTemplate.RowFilter = "PageId=" + common.myInt(StaticTemplateId);
            dtTemplate = dvFilterStaticTemplate.ToTable();
            string Fonts = string.Empty;
            //sb.Append("<span style='" + Fonts + "'>");

            if (dtTemplate.Rows.Count > 0)
            {
                if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                    && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[0]["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = string.Empty;
                        string sEnd = string.Empty;
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();


                    bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                                common.myInt(Session["UserID"]).ToString(), common.myStr(dtTemplate.Rows[0]["PageID"]),
                                string.Empty,
                                string.Empty, TemplateFieldId, string.Empty);

                    // sb.Append(sbTemp + "<br/>");


                    drTemplateStyle = null;
                    Templinespace = string.Empty;
                }
                else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                    && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[0]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = string.Empty;
                        string sEnd = string.Empty;
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();


                    bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbStatic, sbTemplateStyle, drTemplateStyle,
                                        Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                        string.Empty,
                                        string.Empty, TemplateFieldId, sEREncounterId, string.Empty);

                    //sb.Append(sbTemp + "<br/>" + "<br/>");


                    drTemplateStyle = null;
                    Templinespace = string.Empty;

                }

                else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                    && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[0]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = string.Empty;
                        string sEnd = string.Empty;
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();


                    bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                                common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                string.Empty, string.Empty, TemplateFieldId, sEREncounterId, string.Empty);

                    //sb.Append(sbTemp + "<br/>");

                    drTemplateStyle = null;
                    Templinespace = string.Empty;
                }
                //sb.Append("</span>");
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
            dsTemplate.Dispose();
            dsTemplateStyle.Dispose();
            dtTemplate.Dispose();
            fun = null;
            bnotes = null;
        }
        return "<br/>" + sbStatic.ToString();
    }

    protected DataSet GetPageProperty(string iFormId)
    {
        DataSet ds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();

        try
        {
            if (Session["HospitalLocationID"] != null && iFormId != string.Empty)
            {
                if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
                {
                    hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                    hstInput.Add("@intFormId", iFormId);
                    ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                    //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                    return ds;
                }
                else
                {
                    ds = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                    return ds;
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

        return null;
    }

    protected void btnLabResult_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Session["OPIP"]) == "O")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != string.Empty && common.myStr(ViewState["EncounterNo"]).Trim() != string.Empty)
            {
                //DateTime adminsiondate = common.myDate(ViewState["AdmissionDate"]);
                RadWindowPopup.NavigateUrl = "PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) + "&OP_IP=O&Master=Blank&From=Ward&EncNo=" + common.myStr(ViewState["EncounterNo"]) + "&Station=Lab&HC=HC";
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

    protected void btnOther_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Session["OPIP"]) == "O")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != string.Empty && common.myStr(ViewState["EncounterNo"]).Trim() != string.Empty)
            {
                RadWindowPopup.NavigateUrl = "PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) + "&OP_IP=O&Master=Blank&From=Ward&EncNo=" + common.myStr(ViewState["EncounterNo"]) + "&Station=Other&HC=HC";
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
        if (common.myStr(Session["OPIP"]) == "O")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != string.Empty && common.myStr(ViewState["EncounterNo"]).Trim() != string.Empty)
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
        if (common.myStr(Session["OPIP"]) == "O")
        {
            if (common.myStr(ViewState["RegistrationNo"]).Trim() != string.Empty && common.myStr(ViewState["EncounterNo"]).Trim() != string.Empty)
            {
                //DateTime adminsiondate = common.myDate(ViewState["AdmissionDate"]);
                RadWindowPopup.NavigateUrl = "PatientLabHistory.aspx?RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&AdmissionDate=" + common.myStr(ViewState["AdmissionDate"]) + "&OP_IP=O&Master=Blank&From=Ward&EncNo=" + common.myStr(ViewState["EncounterNo"]) + "&Station=Radiology&HC=HC";
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

        BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder sbTemp = new StringBuilder();
        DataSet ds = new DataSet();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        Hashtable hst = new Hashtable();
        string Templinespace = string.Empty;
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        int UserId = common.myInt(Session["UserID"]);
        int facilityId = common.myInt(Session["Facilityid"]);
        DL_Funs ff = new DL_Funs();
        BindDischargeSummary bnotes = new BindDischargeSummary(sConString);
        try
        {
            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
            dsTemplateStyle = ObjIcm.GetICMTemplateStyle(common.myInt(Session["HospitalLocationID"]));
            string sFromDate = string.Empty;
            string sToDate = string.Empty;
            //if (chkDateWise.Checked)
            //{
            //    sFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
            //    sToDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).Date.ToString("yyyy/MM/dd");
            //}

            bnotes.BindLabResult(EncounterId, HospitalId, facilityId, sFromDate, sToDate, sbTemp, sbTemplateStyle, drTemplateStyle, Page);

            hdnReturnLab.Value = sbTemp.ToString();
            // RTF1.Content =RTF1.Content + sbTemp.ToString();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ObjIcm = null;
            ds.Dispose();
            dsTemplate.Dispose();
            dsTemplateStyle.Dispose();
            fun = null;
            ff = null;
            bnotes = null;
        }
    }

    public StringBuilder BindMedicationIP(int EncounterId, int HospitalId, int RegId, StringBuilder sb,
                StringBuilder sbTemplateStyle, string MedicationType, DataRow drTemplateListStyle,
                Page pg, string pageID, string userID, string fromDate, string toDate, int FacilityId, string IndentCode)
    {
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        string sBeginSection = string.Empty;
        string sEndSection = string.Empty;
        Hashtable hsMed = new Hashtable();
        DataSet ds = new DataSet();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataTable dtPriscription = new DataTable();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sbPrescribedFinal = new StringBuilder();
        BaseC.clsEMR clsemrobj = new BaseC.clsEMR(sConString);
        try
        {
            hsMed.Add("@inyHospitalLocationId", HospitalId);
            hsMed.Add("@intFacilityId", FacilityId);
            hsMed.Add("@intEncounterId", EncounterId);
            hsMed.Add("@chvIndentCode", IndentCode);
            //if (fromDate != string.Empty && toDate != string.Empty)
            //{
            //    hsMed.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            //    hsMed.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
            //}       
            ds = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", hsMed);
            dtPriscription = ds.Tables[1];
            if (drTemplateListStyle != null)
            {
                if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sBegin = string.Empty; sEnd = string.Empty;
            int rowcount = 1;
            if (dtPriscription.Rows.Count > 0)
            {
                //sbPrescribedFinal.Append(sBegin + "Medicine Name - ");           
                sbPrescribedFinal.Append(sBegin);
                foreach (DataRow dr in dtPriscription.Rows)
                {
                    DataView dv = new DataView(ds.Tables[1]);
                    dv.RowFilter = "IndentId=" + common.myInt(dr["IndentId"]) + " AND ItemId=" + common.myInt(dr["ItemId"]);
                    //sbPrescribed.Append(dr["StartDate"].ToString() + " :  </br> ");
                    sbPrescribed.Append(" " + common.myStr(rowcount) + ". " + common.myStr(dr["ItemName"]) + "  </br>");
                    sbPrescribed.Append("&nbsp;&nbsp;&nbsp; " + clsemrobj.GetPrescriptionDetailString(dv.ToTable()) + "  </br> ");
                    sbPrescribed.Append(sEnd);
                    rowcount = rowcount + 1;
                }
                sbPrescribedFinal.Append(sbPrescribed);
            }
            sb.Append(sbPrescribedFinal);
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
            DlObj = null;
            dtPriscription.Dispose();
            clsemrobj = null;
        }
        return sb;
    }

    protected void dtpdate_SelectedDateChanged(object sender, EventArgs e)
    {
        //int time;
        //string strbactime = "BackTimeAllow";
        //string strfutureDate = "FutureDaysAkkow";
        //baseHs = new BaseC.HospitalSetup(sConString);
        //ds = new DataSet();
        //if (common.myDate(dtptransferdate.SelectedDate) < common.myDate(DateTime.Now))
        //{
        //    string str = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), strbactime, common.myInt(Session["FacilityId"]));
        //    time = Convert.ToInt32(str);
        //    dtptransferdate.MaxDate = DateTime.Now;
        //    DateTime ctime = DateTime.Now.AddMinutes(-time);
        //    DateTime fDays = DateTime.Now.AddDays(-time);
        //    if (dtptransferdate.SelectedDate < ctime)
        //    {
        //        //   lblmsg.Text = "Invalid admission date selected";
        //        AlertForPage("Invalid admission date selected !");
        //        //  dtptransferdate.SelectedDate = ctime;
        //        dtptransferdate.SelectedDate = dtptransferdate.MaxDate;
        //    }
        //}
        //else if (common.myDate(dtptransferdate.SelectedDate) > common.myDate(DateTime.Now))
        //{
        //    string str = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), strfutureDate, common.myInt(Session["FacilityId"]));
        //    time = Convert.ToInt32(str);
        //    DateTime fDays = DateTime.Now.AddDays(time);
        //    if (dtptransferdate.SelectedDate > fDays)
        //    {
        //        AlertForPage("Invalid admission date selected !");
        //        //lblmsg.Text = "Invalid admission date selected";
        //        dtptransferdate.SelectedDate = dtptransferdate.MaxDate;
        //    }
        //}
    }
    protected void btnCancelSummary_Click(object sender, EventArgs e)
    {
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

        }
    }

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
    private void IsValidPassword()
    {
        lblMessage.Text = "";

        hdnIsValidPassword.Value = "0";
        // hdnFrom.Value = from;
        RadWindow2.NavigateUrl = "/Pharmacy/Components/PasswordChecker.aspx";

        RadWindow2.Height = 120;
        RadWindow2.Width = 340;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.OnClientClose = "OnClientIsValidPasswordCancelSummary";
        RadWindow2.VisibleOnPageLoad = true;
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }


    protected void btnIsValidPasswordCancelSummary_OnClick(object sender, EventArgs e)
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
                       common.myInt(hdnSummaryID.Value), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myInt(Session["UserId"]), common.myStr(txtCancelRemarks.Text));


            btnRefresh_OnClick(this, null);
            //BindPatientDischargeSummary();
            if (CancelStatus.Contains("Cancelled"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
            lblMessage.Text = CancelStatus;
            btnFinalize.Text = "Finalized (F2)";
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
        SaveData();
    }
}
