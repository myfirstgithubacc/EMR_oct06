using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Dashboard_PopUpPatientDashboardForDoctorNew : System.Web.UI.Page
{
    //private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //StringBuilder strXMLDrug;
    //StringBuilder strXMLOther;
    bool Confirm_value = false;
    //clsExceptionLog objException = new clsExceptionLog();
    private string DellBoomi = System.Configuration.ConfigurationManager.AppSettings["AFG_DellBoomi"].ToString();


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMasterWithTopDetails_1.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        Page.MaintainScrollPositionOnPostBack = true;
        lbladdfavorders.Text = string.Empty;
        if (!IsPostBack)
        {
            try
            {

                // setTabVisibility();
                //Session["UserId"] = null;
                Session["PreviousRowIndex"] = null;
                //if (common.myInt(Session["UserId"]).Equals(0) || common.myStr(Session["UserId"])=="")
                //{
                //    Response.Redirect("Login.aspx", false);
                //    return;
                //}

                if (common.myInt(Session["HospitalLocationId"]).Equals(0) || common.myInt(Session["FacilityId"]).Equals(0))
                {
                    Response.Redirect("/Default.aspx", false);
                    return;
                }
                ClearMessageControl();
                bindEncounters();
                SetPermission(common.myInt(Session["EncounterId"]));
                //if (common.myStr(Session["OPIP"]).Equals("O")
                //    && !common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                //{
                //    btnSave.Visible = true;
                //}
                //else
                //{
                //    btnSave.Visible = false;
                //    if (!common.myStr(Session["OPIP"]).Equals("O") && !common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                //    {
                //        btnSave.Visible = true;
                //    }
                //}
                //if (common.myBool(Session["isEMRSuperUser"]) && common.myStr(Session["OPIP"]).Equals("I") && !common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
                //{
                //    btnSave.Visible = true;
                //    btnSave.Text = "Save (Ctrl+F3)";
                //}
                if (common.myStr(Request.QueryString["CloseButtonShow"]) == "No")
                {
                    btnClose.Visible = false;
                }
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                clsExceptionLog objException = new clsExceptionLog();
                objException.HandleException(Ex);
                objException = null;
            }
        }

        else
        {
            if (Request.Form["confirm_value"] == "Yes")   //if user clicks "OK" to confirm 
            {
                Request.Form["confirm_value"].Replace("Yes", string.Empty);
            }
            else if (Request.Form["confirm_value"] == "No")   //if user clicks "OK" to confirm 
            {
                Request.Form["confirm_value"].Replace("No", string.Empty);
            }
        }
    }
    private void setTabVisibility()
    {
        //clsIVF objI = new clsIVF(sConString);
        clsIVF objI = new clsIVF(string.Empty);
        DataSet dsSpec = new DataSet();
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        try
        {
            trChiefComplaints.Visible = false;
            trHistory.Visible = false;
            trExamination.Visible = false;
            trPlanOfCare.Visible = false;
            trProvisionalDiagnosis.Visible = false;
            trOrdersAndProcedures.Visible = false;
            int SpecialisationId = 0;
            dsSpec = objI.getDoctorSpecialisation(common.myInt(Session["EmployeeId"]));
            if (dsSpec.Tables[0].Rows.Count > 0)
            {
                SpecialisationId = common.myInt(dsSpec.Tables[0].Rows[0]["SpecialisationId"]);
            }
            if (SpecialisationId > 0 && common.myInt(Session["FacilityId"]) > 0)
            {
                ds = objI.getSingleScreenUserTemplates(SpecialisationId, common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DV = ds.Tables[0].DefaultView;
                        DV.RowFilter = "TemplateCode='COM'";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            trChiefComplaints.Visible = true;
                        }
                        DV.RowFilter = string.Empty;

                        //History              [HIS]                        
                        DV.RowFilter = "TemplateCode='HIS'";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            trHistory.Visible = true;
                        }
                        DV.RowFilter = string.Empty;

                        //Examination           [EXM]                        
                        DV.RowFilter = "TemplateCode='EXM'";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            trExamination.Visible = true;
                        }
                        DV.RowFilter = string.Empty;

                        //Plan Of Care          [POC]                        
                        DV.RowFilter = "TemplateCode='POC'";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            trPlanOfCare.Visible = true;
                        }
                        DV.RowFilter = string.Empty;

                        //Provisional Diagnosis [PDG]                        
                        DV.RowFilter = "TemplateCode='PDG'";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            trProvisionalDiagnosis.Visible = true;
                        }
                        DV.RowFilter = string.Empty;

                        //Orders And Procedures [ORD]                        
                        DV.RowFilter = "TemplateCode='ORD'";
                        if (DV.ToTable().Rows.Count > 0)
                        {
                            trOrdersAndProcedures.Visible = true;
                        }
                        DV.RowFilter = string.Empty;
                    }
                    //else
                    //{
                    //    btnSave.Visible = false;
                    //}
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objI = null;
            dsSpec.Dispose();
            ds.Dispose();
            DV.Dispose();
        }
    }


    #region Common method

    private void ClearMessageControl()
    {
        lblPlanOfCareMessage.Text = string.Empty;
        lblExamMessage.Text = string.Empty;
        lblChiefMessage.Text = string.Empty;
    }

    protected void ViewHide_OnClick(object sender, EventArgs e)
    {
        ImageButton imgbtn = (ImageButton)sender;
    }

    protected void RetrievePatientDiagnosis()
    {
        DataSet ds = new DataSet();
        DataView dvStTemplate = new DataView();
        DataView dvDiagnosisDetail = new DataView();
        DataTable dtChronicDiagnosisDetail = new DataTable();
        DataTable dtDiagnosisDetail = new DataTable();
        try
        {
            //objDiag = new BaseC.DiagnosisDA(sConString);


            if (Session["encounterid"] != null)
            {
                //ds = objDiag.GetPatientDiagnosis(common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["RegistrationId"]),
                // common.myInt(Session["encounterid"]), 0, 0, 0, "", "", "", "%%", false, 0, "", false, 0);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientDiagnosis";
                APIRootClass.GetPatientDiagnosis objRoot = new global::APIRootClass.GetPatientDiagnosis();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacilityId = 0;
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["encounterid"]);
                objRoot.DoctorId = 0;
                objRoot.DiagnosisGroupId = 0;
                objRoot.DiagnosisSubGroupId = 0;
                objRoot.DateRange = "";
                objRoot.FromDate = "";
                objRoot.ToDate = "";
                objRoot.SearchKeyword = "%%";
                objRoot.IsDistinct = false;
                objRoot.StatusId = 0;
                objRoot.VisitType = "";
                objRoot.IsChronic = false;
                objRoot.DiagnosisId = 0;


                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (ds.Tables[0].Rows.Count > 0)
                {


                    if (Request.QueryString["POPUP"] != null && Request.QueryString["POPUP"] == "StaticTemplate")
                    {
                        dvStTemplate = new DataView(ds.Tables[0]);
                        dvStTemplate.RowFilter = "ISNULL(TemplateFieldId,0)<>0";
                    }
                    else
                    {
                        dvStTemplate = new DataView(ds.Tables[0]);
                    }

                    ViewState["Record"] = 1;
                    dvDiagnosisDetail = new DataView(dvStTemplate.ToTable());

                    //dvDiagnosisDetail.RowFilter = "ISNULL(IsChronic,0)=1 and EncounterID=" + common.myInt(Session["encounterid"]);
                    //dtChronicDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    //if (dtChronicDiagnosisDetail.Rows.Count > 0)
                    //{
                    //    //gvChronicDiagnosis.DataSource = dtChronicDiagnosisDetail;
                    //    //gvChronicDiagnosis.DataBind();
                    //}
                    //else
                    //{
                    //    //BindBlankChronicDiagnosisGrid();
                    //}

                    // dvDiagnosisDetail.RowFilter = "ISNULL(IsChronic,0) <> 1 and EncounterID=" + common.myInt(Session["encounterid"]);
                    dtDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    if (dtDiagnosisDetail.Rows.Count > 0)
                    {
                        //txtICDCode.Text = dtDiagnosisDetail.Rows[0]["ICDCode"].ToString();
                        gvDiagnosisDetails.DataSource = dtDiagnosisDetail;
                        gvDiagnosisDetails.DataBind();
                        // chkPullDiagnosis.Checked = Convert.ToString(dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"]) == "" ? false : (Boolean)dtDiagnosisDetail.Rows[0]["PullForwardDiagnosis"];
                        //Cache.Insert("DignosisDetails", dtDiagnosisDetail, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                        //gvDiagnosisDetails.Columns[4].Visible = true;
                        //divDiagnosis.Visible = true;
                        //GridViewDiagnosis.DataSource = dtDiagnosisDetail;
                        //GridViewDiagnosis.DataBind();
                    }
                    else
                    {
                        //BindBlankDiagnosisDetailGrid();
                        //chkPullDiagnosis.Checked = false;
                    }
                    // if (dtChronicDiagnosisDetail.Rows.Count > 0 || dtDiagnosisDetail.Rows.Count > 0)

                    //    chkPregnant.Checked = (Boolean)ds.Tables[0].Rows[0]["IsPregnant"];
                    //chkBreastFeeding.Checked = (Boolean)ds.Tables[0].Rows[0]["IsBreastFeeding"];

                    //if (!IsPostBack)
                    //    AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]), Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));

                }
                else
                {
                    ViewState["Record"] = 0;
                    //BindBlankChronicDiagnosisGrid();
                    //BindBlankDiagnosisDetailGrid();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            // objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            dvStTemplate.Dispose();
            dvDiagnosisDetail.Dispose();
            dtChronicDiagnosisDetail.Dispose();
            dtDiagnosisDetail.Dispose();
        }
    }
    private void BindCommonData(string sTemplateName, string sTemplateType, string sTemplateCode, int iTemplateId, int PageNo, int EncounterId)
    {
        DataSet ds = new DataSet();
        //BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataView dvFilterHistory = new DataView();
        DataView dvPreviousTreatment = new DataView();
        DataView dvCostAnalysis = new DataView();
        DataView dvFilterExamination = new DataView();
        DataView dvFilterNutritional = new DataView();
        DataView dvFilterPlanOfCare = new DataView();
        DataView dvFilterOtherNotes = new DataView();
        DataView dvFilterPreTreatment = new DataView();
        DataView dvFiltergvCostAnalysis = new DataView();

        int pageSize = 7;
        try
        {
            //ds = emr.GetEMRDataForSingleScreen(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["FacilityId"]), EncounterId, sTemplateType, iTemplateId, sTemplateName, null, null, pageSize, PageNo, true);

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRDataForSingleScreen";
            APIRootClass.GetEMRDataForSingleScreen objRoot = new global::APIRootClass.GetEMRDataForSingleScreen();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = EncounterId;
            objRoot.sTemplateType = sTemplateType;
            objRoot.iTemplateId = iTemplateId;
            objRoot.sTemplateName = sTemplateName;
            objRoot.EncounterDate = null;
            objRoot.ToDate = null;
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.pageSize = pageSize;
            objRoot.PageNo = PageNo;
            objRoot.IsCopyLastOPDSummary = true;

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            var sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals(string.Empty))
            {
                gvProblemDetails.EditIndex = 0;
                gvHistory.EditIndex = 0;
                gvExamination.EditIndex = 0;
                gvPlanOfCare.EditIndex = 0;
                gvData.EditIndex = 0;
                gvDiagnosisDetails.EditIndex = 0;

                //added by bhakti
                if (ds.Tables[11].Rows.Count > 0)
                {
                    gvDiagnosisDetails.DataSource = ds.Tables[11];
                    gvDiagnosisDetails.DataBind();
                    RetrievePatientDiagnosis();
                }
                else
                {
                    gvDiagnosisDetails.DataSource = ds.Tables[11];
                    gvDiagnosisDetails.DataBind();
                    divDiagnosisDetails.Visible = false;
                }

                //if (ds.Tables[10].Rows.Count.Equals(0))
                //{
                //    gvDiagnosisDetails.DataSource = ds.Tables[10];
                //    gvDiagnosisDetails.DataBind();

                //    //BindBlankDiagnosisDetailGrid();
                //    divDiagnosisDetails.Visible = false;
                //}
                //else
                //{
                //   // gvDiagnosisDetails.DataSource = ds.Tables[10];
                //   // gvDiagnosisDetails.DataBind();
                //    RetrievePatientDiagnosis();

                //}
                if (ds.Tables[0].Rows.Count.Equals(0))
                {
                    gvProblemDetails.DataSource = ds.Tables[0];
                    gvProblemDetails.DataBind();
                }
                else
                {
                    gvProblemDetails.DataSource = ds.Tables[0];
                    gvProblemDetails.DataBind();
                }

                if (ds.Tables[3].Rows.Count.Equals(0))
                {
                    BindProvisionalDiagnosisBlank();
                    trProvisionalDiagnosis.Visible = false;
                }
                else
                {
                    gvData.DataSource = ds.Tables[3];
                    gvData.DataBind();
                }

                if (ds.Tables[4].Rows.Count > 0)
                {
                    gvOrdersAndProcedures.DataSource = ds.Tables[4];
                    gvOrdersAndProcedures.DataBind();
                }
                else
                {
                    BindBlnkGrid();
                    trOrdersAndProcedures.Visible = false;
                }

                if (ds.Tables[6].Rows.Count > 0)
                {
                    ViewState["PrescriptionDetail"] = ds.Tables[6];//bhakti change from table[6] to table [5]
                    gvAddList.DataSource = ds.Tables[6];
                    gvAddList.DataBind();
                }
                else
                {
                    BindBlankItemGrid();
                    trPrescriptions.Visible = false;
                }

                if (ds.Tables[9].Rows.Count > 0)
                {
                    dvFilterHistory = new DataView(ds.Tables[9].Copy());
                    dvFilterHistory.RowFilter = "TemplateCode='PHIS' AND IsFreeTextTemplate=1";
                    if (dvFilterHistory.ToTable().Rows.Count > 0)
                    {
                        DataTable dtFilterHistory = new DataTable();
                        dtFilterHistory = dvFilterHistory.ToTable();
                        gvHistory.DataSource = dtFilterHistory;
                        gvHistory.DataBind();
                    }
                    else
                    {
                        gvHistory.DataSource = BindHistoryAndExaminationGrid();
                        gvHistory.DataBind();
                    }
                    dvFilterHistory.Dispose();


                    dvFilterExamination = new DataView(ds.Tables[9].Copy());
                    dvFilterExamination.RowFilter = "TemplateCode='EXM' AND IsFreeTextTemplate=1";
                    if (dvFilterExamination.ToTable().Rows.Count > 0)
                    {
                        DataTable dtFilterExamination = new DataTable();
                        dtFilterExamination = dvFilterExamination.ToTable();
                        gvExamination.DataSource = dtFilterExamination;
                        gvExamination.DataBind();
                    }
                    else
                    {
                        gvExamination.DataSource = BindHistoryAndExaminationGrid();
                        gvExamination.DataBind();
                        trExamination.Visible = false;
                    }
                    dvFilterExamination.Dispose();

                    dvFilterPlanOfCare = new DataView(ds.Tables[9].Copy());
                    dvFilterPlanOfCare.RowFilter = "TemplateCode='POC' AND IsFreeTextTemplate=1";
                    if (dvFilterPlanOfCare.ToTable().Rows.Count > 0)
                    {

                        DataTable dtPlanOfCare = new DataTable();
                        dtPlanOfCare = dvFilterPlanOfCare.ToTable();
                        gvPlanOfCare.DataSource = dtPlanOfCare;
                        gvPlanOfCare.DataBind();
                    }
                    else
                    {
                        gvPlanOfCare.DataSource = BindHistoryAndExaminationGrid();
                        gvPlanOfCare.DataBind();
                        trPlanOfCare.Visible = false;
                    }
                    dvFilterPlanOfCare.Dispose();
                    dvFilterOtherNotes.Dispose();
                }
                else
                {
                    trHistory.Visible = false;
                    trExamination.Visible = false;
                    trPlanOfCare.Visible = false;
                }


            }
            else
            {
                if ((sTemplateName.Equals("Chief Complaints") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[0].Rows.Count.Equals(0))
                    {
                        gvProblemDetails.DataSource = ds.Tables[0];
                        gvProblemDetails.DataBind();
                    }
                    else
                    {
                        gvProblemDetails.DataSource = ds.Tables[0];
                        gvProblemDetails.DataBind();
                    }
                }
                if ((sTemplateName.Equals("Provisional Diagnosis") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[3].Rows.Count.Equals(0))
                    {
                        BindProvisionalDiagnosisBlank();
                        trProvisionalDiagnosis.Visible = false;
                    }
                    else
                    {
                        gvData.DataSource = ds.Tables[3];
                        gvData.DataBind();
                    }
                }
                if ((sTemplateName.Equals("Orders And Procedures") && sTemplateType.Equals("S")))
                {
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        gvOrdersAndProcedures.DataSource = ds.Tables[4];
                        gvOrdersAndProcedures.DataBind();

                    }
                    else
                    {
                        BindBlnkGrid();
                        trOrdersAndProcedures.Visible = false;

                    }
                }
                if ((sTemplateName.Equals("Prescription") && sTemplateType.Equals("S")))
                {
                    BindBlankItemGrid();
                }
                else
                {
                    gvAddList.DataSource = ds.Tables[6];
                    gvAddList.DataBind();
                }



                //if (sTemplateName.Equals(string.Empty) && sTemplateType.Equals("D") && sTemplateCode.Equals("EXM"))
                //{
                //    dvFilterExamination = new DataView(ds.Tables[9].Copy());
                //    dvFilterExamination.RowFilter = "TemplateCode='" + sTemplateCode + "' AND IsFreeTextTemplate=1";
                //    if (dvFilterExamination.ToTable().Rows.Count > 0)
                //    {

                //        DataTable dtnew = new DataTable();
                //        dtnew = dvFilterExamination.ToTable();
                //        gvExamination.DataSource = dtnew;
                //        gvExamination.DataBind();
                //    }
                //    else
                //    {
                //        gvExamination.DataSource = BindHistoryAndExaminationGrid();
                //        gvExamination.DataBind();
                //    }
                //    dvFilterExamination.Dispose();
                //}
            }

            //lblLastEncounterDate.Text = string.Empty;
            //if (ds.Tables[11] != null && ds.Tables.Count > 10)
            //{
            //    if (ds.Tables[11].Rows.Count > 0)
            //    {
            //        lblLastEncounterDate.Text = common.myStr(ds.Tables[11].Rows[0]["LastEncounterDate"]);
            //    }
            //}
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            dvFilterHistory.Dispose();
            dvPreviousTreatment.Dispose();
            dvCostAnalysis.Dispose();
            dvFilterExamination.Dispose();
            dvFilterNutritional.Dispose();
            dvFilterPlanOfCare.Dispose();
            dvFilterOtherNotes.Dispose();
            dvFilterPreTreatment.Dispose();
            dvFiltergvCostAnalysis.Dispose();
            //emr = null;
        }
    }
    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ItemId", typeof(int));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("XMLData", typeof(string));
            dt.Columns.Add("Dose", typeof(string));
            dt.Columns.Add("Days", typeof(double));
            dt.Columns.Add("Frequency", typeof(double));
            dt.Columns.Add("FrequencyName", typeof(string));
            dt.Columns.Add("FrequencyId", typeof(int));
            dt.Columns.Add("Duration", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("DurationText", typeof(string));
            dt.Columns.Add("UnitId", typeof(int));
            dt.Columns.Add("UnitName", typeof(string));
            dt.Columns.Add("FoodRelationshipId", typeof(int));
            dt.Columns.Add("FoodRelationship", typeof(string));
            dt.Columns.Add("DoseTypeId", typeof(int));
            dt.Columns.Add("DoseTypeName", typeof(string));
            dt.Columns.Add("TimeUnit", typeof(string));
            dt.Columns.Add("Instructions", typeof(string));
            dt.Columns.Add("DetailsId", typeof(int));
            dt.Columns.Add("IndentId", typeof(int));
            dt.Columns.Add("MergedUniqueId", typeof(string));
            //added by bhakti
            dt.Columns.Add("GenericName", typeof(string));
            dt.Columns.Add("PrescriptionDetail", typeof(string));

            return dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
            return dt;
        }

    }
    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {
            dr["ItemId"] = 0;
            dr["ItemName"] = string.Empty;
            dr["ItemName"] = string.Empty;
            dr["XMLData"] = string.Empty;
            dr["Dose"] = 0;
            dr["Days"] = 0;
            dr["Frequency"] = 0;
            dr["FrequencyName"] = string.Empty;
            dr["FrequencyId"] = 0;
            dr["Duration"] = string.Empty;
            dr["Type"] = string.Empty;
            dr["DurationText"] = string.Empty;
            dr["UnitId"] = 0;
            dr["UnitName"] = string.Empty;
            dr["FoodRelationshipId"] = 0;
            dr["FoodRelationship"] = string.Empty;
            dr["DoseTypeId"] = 0;
            dr["DoseTypeName"] = string.Empty;
            dr["TimeUnit"] = string.Empty;
            dr["Instructions"] = string.Empty;
            //added by bhakti
            dr["GenericName"] = string.Empty;
            dr["PrescriptionDetail"] = string.Empty;

            dr["IndentId"] = 0;
            dr["DetailsId"] = 0;
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            gvAddList.DataSource = dt;
            gvAddList.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dt.Dispose();
        }

    }
    private DataTable BindHistoryAndExaminationGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TemplateName");
        dt.Columns.Add("RecordId");
        dt.Columns.Add("DocDate");
        dt.Columns.Add("EncodedBy");
        dt.Columns.Add("TemplateId");
        dt.Columns.Add("TemplateType");
        dt.Columns.Add("EncodedById", System.Type.GetType("System.Int32"));
        dt.Columns.Add("DetailId");
        DataRow dr = dt.NewRow();
        dr["TemplateName"] = string.Empty;
        dr["RecordId"] = string.Empty;
        dr["DocDate"] = string.Empty;
        dr["EncodedBy"] = string.Empty;
        dr["TemplateId"] = string.Empty;
        dr["TemplateType"] = string.Empty;
        dr["DetailId"] = string.Empty;
        dt.Rows.Add(dr);
        return dt;
    }
    #endregion

    #region Chief Complaints

    protected void gvProblemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit) || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.BackColor = System.Drawing.Color.White;
            e.Row.BorderColor = System.Drawing.Color.LightGray;

            HiddenField hdnProblemId = (HiddenField)e.Row.FindControl("hdnProblemId");
            TextBox txtTemplate = (TextBox)e.Row.FindControl("txtTemplate");

            if (common.myInt(hdnProblemId.Value).Equals(0))
            {
                CheckBox chkProblemSelectAll = (CheckBox)e.Row.FindControl("chkProblemSelectAll");

                chkProblemSelectAll.Checked = false;
                chkProblemSelectAll.Visible = false;
                txtTemplate.Enabled = false;
            }

            txtTemplate.Text = common.clearHTMLTags(txtTemplate.Text);
            txtTemplate.Text = txtTemplate.Text.Replace("&lt", "<");
            txtTemplate.Text = txtTemplate.Text.Replace("&gt", ">");
        }
    }
    #endregion

    #region History
    protected void gvHistory_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.BackColor = System.Drawing.Color.White;
            e.Row.BorderColor = System.Drawing.Color.LightGray;

            HiddenField hdnTemplateId = (HiddenField)e.Row.FindControl("hdnTemplateId");
            TextBox txtTemplate = (TextBox)e.Row.FindControl("txtTemplate");

            if (common.myInt(hdnTemplateId.Value).Equals(0))
            {
                CheckBox chkHistorySelectAll = (CheckBox)e.Row.FindControl("chkHistorySelectAll");

                chkHistorySelectAll.Checked = false;
                chkHistorySelectAll.Visible = false;
                txtTemplate.Enabled = false;
            }

            txtTemplate.Text = common.clearHTMLTags(txtTemplate.Text);
            txtTemplate.Text = txtTemplate.Text.Replace("&lt", "<");
            txtTemplate.Text = txtTemplate.Text.Replace("&gt", ">");
        }
    }
    #endregion

    #region Examination
    protected void gvExamination_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.BackColor = System.Drawing.Color.White;
            e.Row.BorderColor = System.Drawing.Color.LightGray;

            HiddenField hdnTemplateId = (HiddenField)e.Row.FindControl("hdnTemplateId");
            TextBox txtTemplate = (TextBox)e.Row.FindControl("txtTemplate");

            if (common.myInt(hdnTemplateId.Value).Equals(0))
            {
                CheckBox chkExamSelectAll = (CheckBox)e.Row.FindControl("chkExamSelectAll");

                chkExamSelectAll.Checked = false;
                chkExamSelectAll.Visible = false;
                txtTemplate.Enabled = false;
            }

            txtTemplate.Text = common.clearHTMLTags(txtTemplate.Text);
            txtTemplate.Text = txtTemplate.Text.Replace("&lt", "<");
            txtTemplate.Text = txtTemplate.Text.Replace("&gt", ">");
        }
    }
    #endregion

    #region Plan Of Care
    protected void gvPlanOfCare_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.BackColor = System.Drawing.Color.White;
            e.Row.BorderColor = System.Drawing.Color.LightGray;

            HiddenField hdnTemplateId = (HiddenField)e.Row.FindControl("hdnTemplateId");
            TextBox txtTemplate = (TextBox)e.Row.FindControl("txtTemplate");

            if (common.myInt(hdnTemplateId.Value).Equals(0))
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)e.Row.FindControl("chkPlanOfCareSelectAll");

                chkPlanOfCareSelectAll.Checked = false;
                chkPlanOfCareSelectAll.Visible = false;
                txtTemplate.Enabled = false;
            }

            txtTemplate.Text = common.clearHTMLTags(txtTemplate.Text);
            txtTemplate.Text = txtTemplate.Text.Replace("&lt", "<");
            txtTemplate.Text = txtTemplate.Text.Replace("&gt", ">");
        }
    }
    #endregion

    #region Provisional Diagnosis
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.BackColor = System.Drawing.Color.White;
                e.Row.BorderColor = System.Drawing.Color.LightGray;


                HiddenField hdnProvisionalDiagnosisID = (HiddenField)e.Row.FindControl("hdnProvisionalDiagnosisID");
                TextBox txtTemplate = (TextBox)e.Row.FindControl("txtTemplate");

                if (common.myInt(hdnProvisionalDiagnosisID.Value).Equals(0))
                {
                    CheckBox chkProvisionalSelectAll = (CheckBox)e.Row.FindControl("chkProvisionalSelectAll");

                    chkProvisionalSelectAll.Checked = false;
                    chkProvisionalSelectAll.Visible = false;
                    txtTemplate.Enabled = false;
                }

                txtTemplate.Text = common.clearHTMLTags(txtTemplate.Text);
                txtTemplate.Text = txtTemplate.Text.Replace("&lt", "<");
                txtTemplate.Text = txtTemplate.Text.Replace("&gt", ">");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    #endregion

    #region Order And Procedure
    protected void gvOrdersAndProcedures_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.BackColor = System.Drawing.Color.White;
                e.Row.BorderColor = System.Drawing.Color.LightGray;

                HiddenField hdnServiceId = (HiddenField)e.Row.FindControl("hdnServiceId");

                if (common.myInt(hdnServiceId.Value).Equals(0))
                {
                    CheckBox chkOrderSelectAll = (CheckBox)e.Row.FindControl("chkOrderSelectAll");

                    chkOrderSelectAll.Checked = false;
                    chkOrderSelectAll.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    #endregion

    #region Other Events
    protected void btnSaveDashboard_OnClick(object sender, EventArgs e)
    {
        try
        {
            SaveOrder();
            SaveDasboardData();
            hdnSaveToClose.Value = "1";
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void chkSelectDeselectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectDeselectAll.Checked)
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                chkProblemSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                chkHistorySelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                chkExamSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                chkPlanOfCareSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                chkProvisionalSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)item.FindControl("chkOrderSelectAll");
                chkOrderSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                chkDiagnosisDetails.Checked = true;
            }
            foreach (GridViewRow item in gvAddList.Rows)
            {
                CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                chkPrecriptionAll.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                chkProblemSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                chkHistorySelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                chkExamSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                chkPlanOfCareSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                chkProvisionalSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)item.FindControl("chkOrderSelectAll");
                chkOrderSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                chkDiagnosisDetails.Checked = false;
            }
            foreach (GridViewRow item in gvAddList.Rows)
            {
                CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                chkPrecriptionAll.Checked = false;
            }
        }
    }

    private void SaveDasboardData()
    {
        ArrayList col = new ArrayList();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbPrescription = new StringBuilder();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        //Hashtable HshOut = new Hashtable();
        string strData = string.Empty;
        DataSet dshOutput = new DataSet();
        BindDischargeSummary emr = new BindDischargeSummary(string.Empty);
        bool CheckDiagnosisPrimaryForPatient = false;
        try
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                HiddenField hdnProblemId = (HiddenField)item.FindControl("hdnProblemId");
                TextBox txtTemplate = (TextBox)item.FindControl("txtTemplate");

                strData = txtTemplate.Text;

                strData = strData.Replace("<", "&lt");
                strData = strData.Replace(">", "&gt");
                strData = strData.Replace("\n", "<br/>");
                strData = strData.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);

                if (chkProblemSelectAll.Checked && common.myLen(strData) > 0)
                {
                    col.Add("PRO");
                    col.Add(common.myInt(hdnProblemId.Value));
                    col.Add(strData);

                    sb.Append(common.setXmlTable(ref col));
                }
                strData = string.Empty;
            }

            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                HiddenField hdnTemplateId = (HiddenField)item.FindControl("hdnTemplateId");
                TextBox txtTemplate = (TextBox)item.FindControl("txtTemplate");

                strData = txtTemplate.Text;

                strData = strData.Replace("<", "&lt");
                strData = strData.Replace(">", "&gt");
                strData = strData.Replace("\n", "<br/>");
                strData = strData.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);

                if (chkHistorySelectAll.Checked && common.myLen(strData) > 0)
                {
                    col.Add("HIS");
                    col.Add(common.myInt(hdnTemplateId.Value));
                    col.Add(strData);

                    sb.Append(common.setXmlTable(ref col));
                }
            }

            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                HiddenField hdnTemplateId = (HiddenField)item.FindControl("hdnTemplateId");
                TextBox txtTemplate = (TextBox)item.FindControl("txtTemplate");

                strData = txtTemplate.Text;

                strData = strData.Replace("<", "&lt");
                strData = strData.Replace(">", "&gt");
                strData = strData.Replace("\n", "<br/>");
                strData = strData.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);

                if (chkExamSelectAll.Checked && common.myLen(strData) > 0)
                {
                    col.Add("EXM");
                    col.Add(common.myInt(hdnTemplateId.Value));
                    col.Add(strData);

                    sb.Append(common.setXmlTable(ref col));
                }

                strData = string.Empty;
            }

            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                HiddenField hdnTemplateId = (HiddenField)item.FindControl("hdnTemplateId");
                TextBox txtTemplate = (TextBox)item.FindControl("txtTemplate");

                strData = txtTemplate.Text;

                strData = strData.Replace("<", "&lt");
                strData = strData.Replace(">", "&gt");
                strData = strData.Replace("\n", "<br/>");
                strData = strData.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);

                if (chkPlanOfCareSelectAll.Checked && common.myLen(strData) > 0)
                {
                    col.Add("POC");
                    col.Add(common.myInt(hdnTemplateId.Value));
                    col.Add(strData);

                    sb.Append(common.setXmlTable(ref col));
                }
                strData = string.Empty;
            }

            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                HiddenField hdnProvisionalDiagnosisID = (HiddenField)item.FindControl("hdnProvisionalDiagnosisID");
                TextBox txtTemplate = (TextBox)item.FindControl("txtTemplate");

                strData = txtTemplate.Text;

                strData = strData.Replace("<", "&lt");
                strData = strData.Replace(">", "&gt");
                strData = strData.Replace("\n", "<br/>");
                strData = strData.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);

                if (chkProvisionalSelectAll.Checked && common.myLen(strData) > 0)
                {
                    col.Add("PRV");
                    col.Add(common.myInt(hdnProvisionalDiagnosisID.Value));
                    col.Add(strData);

                    sb.Append(common.setXmlTable(ref col));
                }
                strData = string.Empty;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

                if (chkDiagnosisDetails.Checked)
                {
                    col.Add("DGN");
                    col.Add(common.myInt(hdnId.Value));
                    col.Add(DBNull.Value);
                    sb.Append(common.setXmlTable(ref col));
                    CheckDiagnosisPrimaryForPatient = true;
                }
            }
            //BaseC.clsEMR emr = new clsEMR(sConString);
            if (ViewState["PrescriptionDetail"] != null)
            {
                if (!CheckDiagnosisPrimaryForPatient)
                {
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckPrimaryDiagnosisForEncounter";
                    APIRootClass.CheckPrimaryDiagnosisForEncounter objRoot = new global::APIRootClass.CheckPrimaryDiagnosisForEncounter();
                    objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    CheckDiagnosisPrimaryForPatient = JsonConvert.DeserializeObject<bool>(sValue);
                }

                ////if (phr.CheckDiagnosisPrimaryForPatient(common.myInt(Session["EncounterId"])) == false)
                //if (!CheckDiagnosisPrimaryForPatient)
                //{
                //    Alert.ShowAjaxMsg("Atleast One Primery Diagnosis Is Required. Please Enter Diagnosis then Continue to copy prescription..", this);
                //    //return;
                //}
                //else
                {
                    foreach (GridViewRow item in gvAddList.Rows)
                    {
                        CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                        HiddenField hdnIndentId = (HiddenField)item.FindControl("hdnIndentId");
                        HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
                        HiddenField hdnDetailsId = (HiddenField)item.FindControl("hdnDetailsId");
                        HiddenField hdnPrescriptionDetail = (HiddenField)item.FindControl("hdnPrescriptionDetail");
                        HiddenField hdnGenericName = (HiddenField)item.FindControl("hdnGenericName");
                        if (chkPrecriptionAll.Checked)
                        {
                            DataView dv = new DataView((DataTable)ViewState["PrescriptionDetail"]);
                            dv.RowFilter = "ItemId = " + common.myInt(hdnItemId.Value);
                            string PrescriptionDetail = emr.GetPrescriptionDetailStringNew(dv.ToTable());
                            col.Add("PRS");
                            col.Add(common.myInt(hdnIndentId.Value));
                            col.Add(common.myInt(hdnItemId.Value));
                            //col.Add(common.myStr(hdnPrescriptionDetail.Value));
                            //col.Add(common.myStr(hdnGenericName.Value));
                            col.Add(common.myStr(PrescriptionDetail));
                            col.Add(common.myDate(dv.ToTable().Rows[0]["StartDate"]).ToString("yyyy/MM/dd"));
                            col.Add(common.myDate(dv.ToTable().Rows[0]["EndDate"]).ToString("yyyy/MM/dd"));
                            col.Add(common.myInt(hdnDetailsId.Value));
                            sbPrescription.Append(common.setXmlTable(ref col));
                            dv.Dispose();
                        }
                    }
                }
            }
            //emr = null;
            if (sb.ToString() != "" || sbPrescription.ToString() != "")
            {
                //HshOut = objEMR.SavePatientLastEncounterData(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]),
                //                        common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["DoctorID"]),
                //                        common.myInt(Session["UserId"]), sb.ToString(), sbPrescription.ToString());

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientLastEncounterData";
                APIRootClass.SavePatientLastEncounterData objRoot = new global::APIRootClass.SavePatientLastEncounterData();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.TemplateIdDetails = sb.ToString();
                objRoot.PrescriptionDetails = sbPrescription.ToString();
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                var sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dshOutput = JsonConvert.DeserializeObject<DataSet>(sValue);
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = common.myStr(dshOutput.Tables[0].Rows[0]["chvErrorStatus"]);

                //#region ePrescription
                //string strMsg = lblMessage.Text;
                //if (common.myStr(Session["OPIP"]) == "O" || common.myStr(Session["OPIP"]) == "E")
                //{
                //    string OutRefno = "0";
                //    int indentid = common.myInt(common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"]).Split('$')[1]);
                //    if (indentid == 0)
                //        return;
                //    string active = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EpresActive", string.Empty);
                //    //string s = "Operation is successful";
                //    string s = "RECORD SAVED";
                //    //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //    //int isactive = common.myInt(dl.ExecuteScalar(CommandType.Text, "selecT convert(int,isnull(EprescriptionEnabled,0))  From doctorDetails with(nolock) where DoctorID=" + common.myInt(Session["EmployeeId"]).ToString()));

                //    client = new WebClient();
                //    client.Headers["Content-type"] = "application/json";
                //    client.Encoding = Encoding.UTF8;
                //    ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/IsDOctorEprescriptionEnabled";
                //    APIRootClass.IsDOctorEprescriptionEnabled objRoot1 = new global::APIRootClass.IsDOctorEprescriptionEnabled();
                //    objRoot1.DoctorId = common.myInt(Session["EmployeeId"]);

                //    inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                //    sValue = client.UploadString(ServiceURL, inputJson);
                //    int isactive = JsonConvert.DeserializeObject<int>(sValue);

                //    if (isactive == 1)
                //    {
                //        if (common.myInt(active) == 1)
                //        {

                //            if (isactive == 1)
                //            {
                //                try
                //                {
                //                    s = PostePrescription(common.myInt(common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"]).Split('$')[1]), "PRODUCTION", out OutRefno);
                //                    Alert.ShowAjaxMsg(s, this);
                //                }
                //                catch(Exception Ex)
                //                {
                //                    clsExceptionLog objException = new clsExceptionLog();
                //                    objException.HandleException(Ex);
                //                    objException = null;
                //                }
                //                //        dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                //            }
                //            else
                //            {
                //                //   DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //                //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());

                //                client = new WebClient();
                //                client.Headers["Content-type"] = "application/json";
                //                client.Encoding = Encoding.UTF8;
                //                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                //                APIRootClass.UpdateDHARefNo objRoot2 = new global::APIRootClass.UpdateDHARefNo();
                //                objRoot2.DHARefNo = OutRefno;
                //                objRoot2.IndentId = indentid;

                //                inputJson = (new JavaScriptSerializer()).Serialize(objRoot2);
                //                sValue = client.UploadString(ServiceURL, inputJson);
                //            }
                //        }
                //        else
                //        {
                //            //DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //            //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());

                //            client = new WebClient();
                //            client.Headers["Content-type"] = "application/json";
                //            client.Encoding = Encoding.UTF8;
                //            ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                //            APIRootClass.UpdateDHARefNo objRoot2 = new global::APIRootClass.UpdateDHARefNo();
                //            objRoot2.DHARefNo = OutRefno;
                //            objRoot2.IndentId = indentid;

                //            inputJson = (new JavaScriptSerializer()).Serialize(objRoot2);
                //            sValue = client.UploadString(ServiceURL, inputJson);
                //        }
                //        if (s.Contains("RECORD SAVED") || s.Contains("Operation is successful"))
                //        {
                //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //            strMsg = "Prescription No : " +
                //                (common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"])).Split('$')[0] +
                //                " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;
                //            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //            //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                //            client = new WebClient();
                //            client.Headers["Content-type"] = "application/json";
                //            client.Encoding = Encoding.UTF8;
                //            ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                //            APIRootClass.UpdateDHARefNo objRoot2 = new global::APIRootClass.UpdateDHARefNo();
                //            objRoot2.DHARefNo = OutRefno;
                //            objRoot2.IndentId = indentid;

                //            inputJson = (new JavaScriptSerializer()).Serialize(objRoot2);
                //            sValue = client.UploadString(ServiceURL, inputJson);
                //        }
                //        else
                //        {
                //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //            lblMessage.Text = s;
                //        }
                //    }
                //    else
                //    {
                //        //DAL.DAL dl1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //        //dl1.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                //        client = new WebClient();
                //        client.Headers["Content-type"] = "application/json";
                //        client.Encoding = Encoding.UTF8;
                //        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                //        APIRootClass.UpdateDHARefNo objRoot2 = new global::APIRootClass.UpdateDHARefNo();
                //        objRoot2.DHARefNo = OutRefno;
                //        objRoot2.IndentId = indentid;

                //        inputJson = (new JavaScriptSerializer()).Serialize(objRoot2);
                //        sValue = client.UploadString(ServiceURL, inputJson);
                //    }
                //    if (s.Contains("RECORD SAVED") || s.Contains("Operation is successful"))
                //    {
                //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //        strMsg = "Prescription No : " +
                //            (common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"])).Split('$')[0] +
                //            " " + strMsg + " DHA REFNo:" + OutRefno + " DHA Operation Message:" + s;
                //        //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //        //dl.ExecuteNonQuery(CommandType.Text, "Exec uspUpdateDHARefNo " + OutRefno + "," + indentid.ToString());
                //        client = new WebClient();
                //        client.Headers["Content-type"] = "application/json";
                //        client.Encoding = Encoding.UTF8;
                //        ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateDHARefNo";
                //        APIRootClass.UpdateDHARefNo objRoot2 = new global::APIRootClass.UpdateDHARefNo();
                //        objRoot2.DHARefNo = OutRefno;
                //        objRoot2.IndentId = indentid;

                //        inputJson = (new JavaScriptSerializer()).Serialize(objRoot2);
                //        sValue = client.UploadString(ServiceURL, inputJson);
                //    }
                //    else
                //    {
                //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //        lblMessage.Text = s;
                //    }
                //}
                //else
                //{
                //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //    lblMessage.Text = strMsg;
                //}

                //#endregion

                ////RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" +
                ////    common.myInt(Session["EncounterId"]) + "&PId=" + common.myInt(common.myStr(dshOutput.Tables[0].Rows[0]["chvPrescriptionNo"]).Split('$')[1]);
                ////RadWindow1.Height = 650;
                ////RadWindow1.Width = 900;
                ////RadWindow1.Top = 10;
                ////RadWindow1.Left = 10;
                ////RadWindow1.Modal = true;
                ////RadWindow1.VisibleOnPageLoad = true;
                ////RadWindow1.VisibleStatusbar = false;
                ////RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

                Session["SaveToClose"] = 1;

                if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            col = null;
            sb = null;
            sbPrescription = null;
            strData = string.Empty;
            dshOutput.Dispose();
            emr = null;
        }
    }

    private void SetPermission(int EncounterId)
    {
        if (!common.myInt(Session["ModuleId"]).Equals(3))
        {
            return;
        }
        //UserAuthorisations ua1 = new UserAuthorisations(sConString);
        UserAuthorisations ua1 = new UserAuthorisations(string.Empty);
        try
        {
            ViewState["SaveEnable"] = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, EncounterId);
            ViewState["EditEnable"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, EncounterId);
            ViewState["DeleteEnable"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, EncounterId);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ua1.Dispose();
        }
    }

    #endregion

    protected void gvAddList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit)
            || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.BackColor = System.Drawing.Color.White;
                e.Row.BorderColor = System.Drawing.Color.LightGray;

                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");

                if (common.myInt(hdnIndentId.Value).Equals(0))
                {
                    CheckBox chkPrecriptionAll = (CheckBox)e.Row.FindControl("chkPrecriptionAll");

                    chkPrecriptionAll.Checked = false;
                    chkPrecriptionAll.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }

    }

    private void SaveOrder()
    {

        ArrayList coll = new ArrayList();
        ArrayList coll1 = new ArrayList();
        DataSet ds = new DataSet();
        DataTable dtSave = new DataTable();
        try
        {
            //BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            string doctorid = "0";
            if (Request.QueryString["For"] != null && Request.QueryString["DoctorId"] != null)
            {
                doctorid = common.myStr(Request.QueryString["DoctorId"]);
            }
            else
            {
                doctorid = Session["EmployeeId"].ToString();
            }
            StringBuilder strXML = new StringBuilder();
            int DoctorId = 0;
            foreach (GridViewRow row in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)row.FindControl("chkOrderSelectAll");
                Label lblRemarks = (Label)row.FindControl("lblRemarks");
                HiddenField lblServiceId = (HiddenField)row.FindControl("hdnServiceId");
                HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                HiddenField hdnStat = (HiddenField)row.FindControl("hdnStat");
                if (chkOrderSelectAll.Checked)
                {
                    if (common.myInt(hdnDoctorId.Value) > 0)
                    {
                        DoctorId = common.myInt(Session["EmployeeId"]);
                    }
                    strXML.Append("<Table1><c1>");
                    strXML.Append(common.myInt(lblServiceId.Value));
                    strXML.Append("</c1><c2>");
                    strXML.Append("</c2><c3>");
                    strXML.Append(1);
                    strXML.Append("</c3><c4>");
                    strXML.Append(DoctorId);
                    strXML.Append("</c4><c5>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c5><c6>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c6><c7>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c7><c8>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c8><c9>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c9><c10>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c10><c11>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c11><c12>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c12><c13>");
                    strXML.Append(common.myInt(0));
                    strXML.Append("</c13><c14>");
                    strXML.Append("0");
                    strXML.Append("</c14><c15>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c15><c16>");

                    strXML.Append("</c16><c17>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c17><c18>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c18><c19>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c19><c20>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c20><c21>");
                    strXML.Append(common.myStr(lblRemarks.Text));

                    strXML.Append("</c21><c22>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c22><c23>");
                    strXML.Append(0);
                    strXML.Append("</c23><c24>");
                    strXML.Append(0);
                    strXML.Append("</c24><c25>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c25><c26>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c26><c27>");
                    strXML.Append(DBNull.Value);
                    strXML.Append("</c27><c28>");
                    strXML.Append(common.myInt(Session["FacilityId"]));
                    strXML.Append("</c28><c29>");

                    strXML.Append(common.myBool(hdnStat.Value));
                    strXML.Append("</c29><c30>");

                    strXML.Append(DBNull.Value);

                    strXML.Append("</c30><c31>");

                    strXML.Append(DBNull.Value);
                    strXML.Append("</c31></Table1>");
                }
            }
            //  ArrayList coll = new ArrayList();
            StringBuilder strXMLAleart = new StringBuilder();

            if (strXML.ToString() != "")
            {

                //ds = order.GetEncounterCompany(common.myInt(Session["EncounterId"]));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEncounterCompany";
                APIRootClass.GetEncounterCompany objRoot = new global::APIRootClass.GetEncounterCompany();
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                //string strtriageID = string.Empty;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                string sChargeCalculationRequired = "Y";
                string stype = "P" + ds.Tables[0].Rows[0]["opip"].ToString().Trim();
                string opip = ds.Tables[0].Rows[0]["opip"].ToString().Trim();

                int CompanyId = 0, InsuranceId = 0, CardId = 0;
                if (ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim() != "")
                {
                    CompanyId = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim() != "")
                {
                    InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["CardId"].ToString().Trim() != "")
                {
                    CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"].ToString().Trim());
                }
                int RequestId = common.myInt(Request.QueryString["RequestId"]);
                Hashtable hshOut = new Hashtable();
                if (opip == "E")
                {
                    opip = "O";

                }
                Session["DuplicateCheck"] = 1;
                int HospitalLocationID = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int RegistrationId = common.myInt(Session["RegistrationId"]);
                int EncounterId = common.myInt(Session["EncounterId"]);
                int UserID = common.myInt(Session["UserID"]);
                DateTime dt = common.myDate(DateTime.Now);
                int dId = common.myInt(doctorid);
                //hshOut = order.saveOrders(HospitalLocationID, FacilityId,
                //                    RegistrationId, EncounterId, strXML.ToString(),
                //                    strXMLAleart.ToString(), string.Empty, UserID,
                //                    dId, CompanyId, stype, "E", opip, InsuranceId,
                //                    CardId, dt, sChargeCalculationRequired, false, 1,
                //                    RequestId, string.Empty);



                if (ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim() != "")
                {
                    CompanyId = common.myInt(ds.Tables[0].Rows[0]["CompanyCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim() != "")
                {
                    InsuranceId = common.myInt(ds.Tables[0].Rows[0]["InsuranceCode"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["CardId"].ToString().Trim() != "")
                {
                    CardId = common.myInt(ds.Tables[0].Rows[0]["CardId"].ToString().Trim());
                }
                //Hashtable hshOut = new Hashtable();
                if (opip == "E")
                {
                    opip = "O";
                }
                int isERRequest = 1;
                //if (Session["DuplicateCheck"].Equals(0))
                //{
                //Session["DuplicateCheck"] = 1;

                string dtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveServiceOrderEMR";
                APIRootClass.SaveServiceOrderEMR objRoot1 = new global::APIRootClass.SaveServiceOrderEMR();
                objRoot1.HospitalLocationId = HospitalLocationID;
                objRoot1.FacilityId = FacilityId;
                objRoot1.RegistrationId = RegistrationId;
                objRoot1.EncounterId = EncounterId;
                objRoot1.xmlServiceList = strXML.ToString();
                objRoot1.XMLPatientAlert = strXMLAleart.ToString();
                objRoot1.Remark = "";
                objRoot1.UserId = UserID;
                objRoot1.DoctorId = common.myInt(doctorid);
                objRoot1.CompanyId = CompanyId;
                objRoot1.OrderType = stype;
                objRoot1.PayerType = "E";
                objRoot1.PatientOPIP = opip;
                objRoot1.InsuranceId = InsuranceId;
                objRoot1.CardId = CardId;
                objRoot1.OrderDate = dtime;
                objRoot1.ChargeCalculationRequired = sChargeCalculationRequired;
                objRoot1.IsAllergyReviewed = false;
                objRoot1.IsERorEMRServices = isERRequest;
                objRoot1.RequestId = RequestId;
                objRoot1.xmlTemplateDetails = string.Empty;
                //string strtriageID = string.Empty;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dtSave = JsonConvert.DeserializeObject<DataTable>(sValue);


                if (dtSave.Rows[0]["chvErrorStatus"].ToString().Length == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    ViewState["OrderId"] = dtSave.Rows[0]["intOrderId"];
                    lblMessage.Text = "Order No:" + dtSave.Rows[0]["intOrderNo"] + " Saved Successfully";
                    lblMessage.Font.Bold = true; ;

                    Session["TemplateDataDetails"] = null;
                    Session["SaveToClose"] = 1;

                    if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "There is some error while saving order." + dtSave.Rows[0]["chvErrorStatus"].ToString();
                }
            }
            else
            {
                //Alert.ShowAjaxMsg("No Service selected", Page);
                //return;
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            dtSave.Dispose();
        }
    }
    private void BindBlnkGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            DataRow dr;
            dr = dT.NewRow();
            dr[0] = DBNull.Value;
            dr[1] = DBNull.Value;
            dr[2] = DBNull.Value;
            dr[3] = DBNull.Value;
            dr[4] = DBNull.Value;
            dr[5] = DBNull.Value;
            dr[6] = DBNull.Value;
            dr[7] = DBNull.Value;
            dr[8] = DBNull.Value;
            dr[9] = DBNull.Value;

            dT.Rows.Add(dr);
            gvOrdersAndProcedures.DataSource = dT;
            gvOrdersAndProcedures.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    //added by bhakti
    private void BindBlnkDiagnosisGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            DataRow dr;
            dr = dT.NewRow();
            dr[0] = DBNull.Value;
            dr[1] = DBNull.Value;
            dr[2] = DBNull.Value;
            dr[3] = DBNull.Value;
            dr[4] = DBNull.Value;
            dr[5] = DBNull.Value;
            dr[6] = DBNull.Value;
            dr[7] = DBNull.Value;

            dT.Rows.Add(dr);
            gvDiagnosisDetails.DataSource = dT;
            gvDiagnosisDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    private void BindProvisionalDiagnosisBlank()
    {
        DataTable Dt = new DataTable();
        try
        {
            DataColumn dc = new DataColumn("ID", typeof(int));
            dc.DefaultValue = 0;
            Dt.Columns.Add(dc);
            Dt.Columns.Add("ProvisionalDiagnosis", typeof(string));
            DataRow dr;
            dr = Dt.NewRow();
            dr[0] = DBNull.Value;
            dr[1] = DBNull.Value;
            Dt.Rows.Add(dr);
            if (Dt.Rows.Count > 0)
            {
                gvData.DataSource = Dt;
                gvData.DataBind();
            }
            else
            {
                gvData.DataSource = Dt;
                gvData.DataBind();

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;

        }
        finally
        {
            Dt.Dispose();
        }
    }

    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        try
        {

            DataColumn dc = new DataColumn("ID", typeof(int));
            dc.DefaultValue = 0;
            Dt.Columns.Add(dc);
            Dt.Columns.Add("OrderDate", typeof(string));
            Dt.Columns.Add("ServiceName", typeof(string));
            Dt.Columns.Add("ServiceId", typeof(int));
            Dt.Columns.Add("Instruction", typeof(string));
            Dt.Columns.Add("orderid", typeof(int));
            Dt.Columns.Add("OrderDetailId", typeof(int));
            Dt.Columns.Add("MergeUniqueId", typeof(string));
            Dt.Columns.Add("DoctorId", typeof(int));
            Dt.Columns.Add("Stat", typeof(int));
            Dt.Columns.Add("Remarks", typeof(int));

            return Dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
            return Dt;
        }
        finally
        {
            Dt.Dispose();
        }
    }

    public void bindEncounters()
    {
        //BaseC.Dashboard objCls = new BaseC.Dashboard();
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        ViewState["FirstEncounterId"] = string.Empty;
        lblLastEncounterDate.Text = string.Empty;
        try
        {
            //ds = objCls.getPatientEncounterDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(Session["RegistrationId"]));

            string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetPatientEncounterDetails";
            APIRootClass.GetPatientEncounterDetails objRoot1 = new global::APIRootClass.GetPatientEncounterDetails();
            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = Convert.ToInt16(Session["FacilityId"]);
            objRoot1.RegistrationId = common.myInt(Session["RegistrationId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            dv = ds.Tables[0].DefaultView;

            if (Request.QueryString["EncounterType"] == "OP")
            {
                dv.RowFilter = "OPIP='O' AND ISNULL(IsOPCLBilled,0)=1 AND EncounterNo=" + Request.QueryString["EncounterNo"];
            }
            else
            {
                dv.RowFilter = "OPIP='O' AND ISNULL(IsOPCLBilled,0)=1 AND ID<" + common.myInt(Session["EncounterId"]);
            }

            // dv.RowFilter = "OPIP='O' AND ISNULL(IsOPCLBilled,0)=1 AND ID<" + common.myInt(Session["EncounterId"]);

            dt = dv.ToTable();

            if (dt.Rows.Count > 0)
            {
                ViewState["FirstEncounterId"] = common.myInt(dt.Rows[0]["Id"]).ToString();
                lblLastEncounterDate.Text = "Visit Date : " + common.myStr(dt.Rows[0]["EncounterDate"]);
            }
            else
            {
                DataRow DR = dt.NewRow();
                dt.Rows.Add(DR);
            }

            //gvEncounters.DataSource = dt;
            //gvEncounters.DataBind();

            hdnEncounterId.Value = dt.Rows[0]["ID"].ToString();
            hdnDoctorId.Value = dt.Rows[0]["DoctorId"].ToString();
            hdnOPIP.Value = dt.Rows[0]["OPIP"].ToString();
            hdnEncounterStatusName.Value = dt.Rows[0]["EncounterStatusName"].ToString();
            //lblEncounterDate.Text = dt.Rows[0]["EncounterDate"].ToString();
            lblDoctor.Text = dt.Rows[0]["Doctor"].ToString();


            BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1, common.myInt(ViewState["FirstEncounterId"]));
            ViewState["FirstEncounterId"] = string.Empty;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
            dt.Dispose();
            //objCls = null;
        }
    }

    protected void gvEncounters_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnEncounterId = (HiddenField)e.Row.FindControl("hdnEncounterId");

                if (common.myInt(hdnEncounterId.Value).Equals(common.myInt(ViewState["FirstEncounterId"]))
                    && common.myInt(ViewState["FirstEncounterId"]) > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightCoral;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    //protected void gvEncounters_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        lblMessage.Text = "&nbsp;";
    //        if (e.CommandName == "SelectEncounter")
    //        {
    //            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

    //            HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
    //            HiddenField hdnOPIP = (HiddenField)row.FindControl("hdnOPIP");
    //            HiddenField hdnEncounterStatusName = (HiddenField)row.FindControl("hdnEncounterStatusName");
    //            Label lblEncounterDate = (Label)row.FindControl("lblEncounterDate");

    //            foreach (GridViewRow item in gvEncounters.Rows)
    //            {
    //                item.BackColor = System.Drawing.Color.White;

    //                HiddenField hdnEncounterIdLoop = (HiddenField)item.FindControl("hdnEncounterId");
    //                if (common.myInt(hdnEncounterId.Value).Equals(common.myInt(hdnEncounterIdLoop.Value)))
    //                {
    //                    item.BackColor = System.Drawing.Color.LightCoral;
    //                }
    //            }

    //            //if (common.myStr(hdnOPIP.Value).Equals("O")
    //            //&& !common.myStr(hdnEncounterStatusName.Value).ToUpper().Contains("CLOSE"))
    //            //{
    //            //    btnSave.Visible = true;
    //            //}
    //            //else
    //            //{
    //            //    btnSave.Visible = false;
    //            //}

    //            SetPermission(common.myInt(hdnEncounterId.Value));

    //            BindCommonData(string.Empty, string.Empty, string.Empty, 0, 1, common.myInt(hdnEncounterId.Value));

    //            lblLastEncounterDate.Text = "Last Visit Date : " + common.myStr(lblEncounterDate.Text);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        clsExceptionLog objException = new clsExceptionLog();
    //        objException.HandleException(Ex);
    //        objException = null;
    //    }
    //}
    protected string PatientandMedicineValidateion(int indentid)
    {
        System.Text.StringBuilder strb = new StringBuilder();
        string ValidationFail = string.Empty;
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            //ds = dl.FillDataSet(CommandType.Text, "Exec UspCheckGenerateeOPRxXml " + common.myStr(Session["FacilityId"]) + "," + common.myStr(indentid));

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            APIRootClass.CheckGenerateeOPRxXml objRoot = new global::APIRootClass.CheckGenerateeOPRxXml();
            objRoot.IndentId = indentid;
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/CheckGenerateeOPRxXml";
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0)
            {
                if (common.myInt(ds.Tables[0].Rows[0][0]) == 0)
                {
                    ValidationFail = "Failed";
                }
                else
                {
                    ValidationFail = string.Empty;
                }

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            //dl = null;
        }

        return ValidationFail;
    }
    private string PostePrescription(int PrescriptionID, string XMLflag, out string refno)
    {
        var outerrormessage = "";
        refno = "0";
        try
        {
            //SqlConnection con = new SqlConnection(sConString);
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UspGenerateeOPRxXml";
            //cmd.CommandType = CommandType.StoredProcedure;

            //SqlParameter prm = new SqlParameter("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
            //cmd.Parameters.Add(prm);

            //SqlParameter prm1 = new SqlParameter("@iFacilityId", common.myInt(Session["FacilityId"]).ToString());
            //cmd.Parameters.Add(prm1);

            //SqlParameter prm2 = new SqlParameter("@PrescriptionID", PrescriptionID);
            //cmd.Parameters.Add(prm2);

            //SqlParameter prm3 = new SqlParameter("@DispositionFlag", XMLflag);
            //cmd.Parameters.Add(prm3);

            //SqlParameter prm4 = new SqlParameter("@returnXML", "");
            //prm4.Direction = ParameterDirection.Output;
            //prm4.DbType = DbType.Xml;

            //cmd.Parameters.Add(prm4);

            //cmd.Connection = con;

            //Hashtable hsin = new Hashtable();
            //Hashtable hsout = new Hashtable();
            //hsin.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
            //hsin.Add("@iFacilityId", common.myInt(Session["FacilityID"]).ToString());
            //hsin.Add("@PrescriptionID", common.myInt(PrescriptionID));
            //hsin.Add("@DispositionFlag", XMLflag);

            //hsout.Add("@returnXML", "");

            //SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet("ds");
            //con.Open();
            //SqlDataReader rdr = cmd.ExecuteReader();


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GenerateeOPRxXml";
            APIRootClass.GenerateeOPRxXml objRoot = new global::APIRootClass.GenerateeOPRxXml();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityID"]);
            objRoot.IndentId = common.myInt(PrescriptionID);
            objRoot.DispositionFlag = XMLflag;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string filename = PrescriptionID.ToString() + "_" + DateTime.Now.ToString("ddMMyyHHmm") + ".xml";
                string fileLoc = Server.MapPath("~/PatientDocuments/" + filename);
                // var fileData=File.reof
                //FileStream fs = null;
                if (File.Exists(fileLoc))
                {
                    File.Delete(fileLoc);
                }

                try
                {

                    string strxmltxt = common.myStr(ds.Tables[0].Rows[0]["returnXML"]);
                    //  Alert.ShowAjaxMsg(strxmltxt, this);
                    var xdoc = new XmlDocument();
                    xdoc.LoadXml(strxmltxt);
                    xdoc.Save(fileLoc);
                }
                catch (Exception ex)
                {
                    Alert.ShowAjaxMsg(ex.Message, this);
                }
                //Alert.ShowAjaxMsg(fileLoc, this);
                //con.Close();

                decimal outputrefno;
                string outputfile = "";

                byte[] outfile = null;



                string uname = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "eclaimWebServiceLoginID", string.Empty);
                string password = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), "eclaimWebServicePassword", string.Empty);
                //System.Net.ServicePointManager.CertificatePolicy = new ClsPolicy();
                //AFG_Request_ValidateTransaction.WebServiceProvider v = new AFG_Request_ValidateTransaction.WebServiceProvider();
                //AFG_Request_ValidateTransaction.UploadERxRequest Request = new AFG_Request_ValidateTransaction.UploadERxRequest();
                //AFG_Request_ValidateTransaction.UploadERxRequestResponse response = new AFG_Request_ValidateTransaction.UploadERxRequestResponse();
                //string[] DellBoomiuserpwd = DellBoomi.Split('!');

                //v.Credentials = new NetworkCredential(DellBoomiuserpwd[0], DellBoomiuserpwd[1]);

                //// NewErx.eRxValidateTransactionSoapClient vt = new NewErx.eRxValidateTransactionSoapClient();
                ////int postrncount = vt.UploadERxRequest(uname, password, "DHA-P-0242455", "DHA-P-0242455", common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);
                ////DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ////DataSet dsdoctor = dl.FillDataSet(CommandType.Text, "Exec uspGetClinicianLoginforErx " + common.myInt(Session["UserID"]).ToString());
                //client = new WebClient();
                //client.Headers["Content-type"] = "application/json";
                //client.Encoding = Encoding.UTF8;
                //ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetClinicianLoginforErx";
                //APIRootClass.GetClinicianLoginforErx objRoot2 = new global::APIRootClass.GetClinicianLoginforErx();
                //objRoot2.DoctorId = common.myInt(Session["UserID"]);

                //inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                //sValue = client.UploadString(ServiceURL, inputJson);
                //sValue = JsonConvert.DeserializeObject<string>(sValue);
                //DataSet dsdoctor = JsonConvert.DeserializeObject<DataSet>(sValue);
                //if (dsdoctor.Tables[0].Rows.Count > 0)
                //{

                //    string cluname = common.myStr(dsdoctor.Tables[0].Rows[0]["UserName"]);
                //    string clpwd = common.myStr(dsdoctor.Tables[0].Rows[0]["Password"]);
                //    v.UseDefaultCredentials = true;
                //    v.Credentials = new NetworkCredential(DellBoomiuserpwd[0], DellBoomiuserpwd[1]);


                //    Request.facilityLogin = uname;
                //    Request.facilityPwd = password;
                //    Request.clinicianLogin = cluname;// "eRxClinTest02";
                //    Request.clinicianPwd = clpwd;// "pwderxclintest02";
                //    Request.fileContent = Convert.ToBase64String(common.FileToByteArray(fileLoc), 0, common.FileToByteArray(fileLoc).Length);
                //    Request.fileName = filename;
                //    response = v.AFG_UploadERxRequest(Request);

                //    outerrormessage = response.errorMessage;
                //    outputrefno = response.eRxReferenceNo;
                //    outputfile = response.errorReport;

                //    //   int trncount = vt.UploadERxRequest("Aster002", "insurance002", cluname, clpwd, common.FileToByteArray(fileLoc), filename, out outputrefno, out outerrormessage, out outfile);
                //    refno = outputrefno.ToString();
                //    if (outputfile != "" && outputfile != "null" && outputfile != null)
                //    {

                //        outfile = Convert.FromBase64String(outputfile);
                //    }
                //}
                //else
                //{

                //    Alert.ShowAjaxMsg("No Doctor Login Found", this);
                //    refno = "0";
                //    return "No Doctor Login Found";
                //}


                //if (returnfilename == true)
                //{
                if (outfile != null)
                {
                    string erfilename = Server.MapPath("~/PatientDocuments/Error.Csv");
                    if (File.Exists(erfilename))
                    {
                        File.Delete(erfilename);
                    }

                    Response.BinaryWrite(outfile);


                    String err = System.Text.Encoding.UTF8.GetString(outfile);

                    err = err.Replace("'", string.Empty);

                    outerrormessage = outerrormessage.Replace("'", string.Empty);

                    //BaseC.clsErx objerx = new BaseC.clsErx(sConString);
                    //objerx.SaveDhaError(common.myInt(PrescriptionID), err, "PrescripeMedication", "PostPrescription", "UploadTransaction", common.myInt(Session["FacilityID"]),
                    //    common.myInt(Session["ModuleId"]), outerrormessage);

                    client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveDhaError";
                    APIRootClass.SaveDhaError objRoot1 = new global::APIRootClass.SaveDhaError();
                    objRoot1.IndentId = common.myInt(Session["UserID"]);
                    objRoot1.Error = err;
                    objRoot1.FormName = "PrescripeMedication";
                    objRoot1.MethodName = "PostPrescription";
                    objRoot1.DHAMethodName = "UploadTransaction";
                    objRoot1.Facilityid = common.myInt(Session["FacilityID"]);
                    objRoot1.ModuleId = common.myInt(Session["ModuleId"]);
                    objRoot1.ErrorMessage = outerrormessage;

                    inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);


                    //dl.ExecuteNonQuery(CommandType.Text, "Exec  UspSaveDhaError " + PrescriptionID.ToString() + ",'" + err + "','PrescripeMedication','PostPrescription','UploadERxRequest'," + common.myInt(Session["FacilityID"]).ToString() + ",'" + outerrormessage + "'");

                    common.ByteArrayToFile(erfilename, outfile, out erfilename);
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Can not generate rx", this);
                refno = "0";
                return "Can not generate rx";
            }
        }
        catch (Exception ex)
        {
            outerrormessage = ex.Message;
        }
        return outerrormessage;
        //}
    }

    protected void sellectAllOrder(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvOrdersAndProcedures.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvOrdersAndProcedures.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkOrderSelectAll");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }
    protected void sellectAllPrescription(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvAddList.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvAddList.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkPrecriptionAll");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

    protected void chkSelectDeselectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectDeselectAll.Checked)
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                chkProblemSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                chkHistorySelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                chkExamSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                chkPlanOfCareSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                chkProvisionalSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)item.FindControl("chkOrderSelectAll");
                chkOrderSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                chkDiagnosisDetails.Checked = true;
            }
            foreach (GridViewRow item in gvAddList.Rows)
            {
                CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                chkPrecriptionAll.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                chkProblemSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                chkHistorySelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                chkExamSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                chkPlanOfCareSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                chkProvisionalSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)item.FindControl("chkOrderSelectAll");
                chkOrderSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                chkDiagnosisDetails.Checked = false;
            }
            foreach (GridViewRow item in gvAddList.Rows)
            {
                CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                chkPrecriptionAll.Checked = false;
            }
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (chkSelectDeselectAll.Checked)
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                chkProblemSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                chkHistorySelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                chkExamSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                chkPlanOfCareSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                chkProvisionalSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)item.FindControl("chkOrderSelectAll");
                chkOrderSelectAll.Checked = true;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                chkDiagnosisDetails.Checked = true;
            }
            foreach (GridViewRow item in gvAddList.Rows)
            {
                CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                chkPrecriptionAll.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow item in gvProblemDetails.Rows)
            {
                CheckBox chkProblemSelectAll = (CheckBox)item.FindControl("chkProblemSelectAll");
                chkProblemSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvHistory.Rows)
            {
                CheckBox chkHistorySelectAll = (CheckBox)item.FindControl("chkHistorySelectAll");
                chkHistorySelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvExamination.Rows)
            {
                CheckBox chkExamSelectAll = (CheckBox)item.FindControl("chkExamSelectAll");
                chkExamSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvPlanOfCare.Rows)
            {
                CheckBox chkPlanOfCareSelectAll = (CheckBox)item.FindControl("chkPlanOfCareSelectAll");
                chkPlanOfCareSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvData.Rows)
            {
                CheckBox chkProvisionalSelectAll = (CheckBox)item.FindControl("chkProvisionalSelectAll");
                chkProvisionalSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvOrdersAndProcedures.Rows)
            {
                CheckBox chkOrderSelectAll = (CheckBox)item.FindControl("chkOrderSelectAll");
                chkOrderSelectAll.Checked = false;
            }
            foreach (GridViewRow item in gvDiagnosisDetails.Rows)
            {
                CheckBox chkDiagnosisDetails = (CheckBox)item.FindControl("chkDiagnosisDetails");
                chkDiagnosisDetails.Checked = false;
            }
            foreach (GridViewRow item in gvAddList.Rows)
            {
                CheckBox chkPrecriptionAll = (CheckBox)item.FindControl("chkPrecriptionAll");
                chkPrecriptionAll.Checked = false;
            }
        }
    }

    protected void btnorder_Click(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvOrdersAndProcedures.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvOrdersAndProcedures.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkOrderSelectAll");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

    protected void btnPrescription_Click(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvAddList.HeaderRow.FindControl("chkprescription");
        foreach (GridViewRow row in gvAddList.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkPrecriptionAll");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }
    //add by ss for psri
    protected void chkb1_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvOrdersAndProcedures.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvOrdersAndProcedures.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkOrderSelectAll");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }
}
